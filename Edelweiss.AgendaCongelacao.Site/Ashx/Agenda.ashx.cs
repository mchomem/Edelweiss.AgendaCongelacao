using Edelweiss.AgendaCongelacao.Model.Entities;
using Edelweiss.AgendaCongelacao.Model.Json;
using Edelweiss.AgendaCongelacao.Model.Repositories;
using Edelweiss.AgendaCongelacao.SMS.Services;
using Edelweiss.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Edelweiss.AgendaCongelacao.Site.Ashx
{
    public class Agenda : IHttpHandler
    {
        #region Properties

        private String CodigoE164
        {
            get
            {
                return "+55"; // Código no formato E 164 do Brasil.
            }
        }

        #endregion

        #region Special Methods

        public void ProcessRequest(HttpContext context)
        {
            String op = context.Request["op"];

            switch (op)
            {
                case "salvar":
                    this.Salvar(context);
                    break;
                // More options of CRUD inside "case" clausules!

                case "consultar":
                    this.Consultar(context);
                    break;

                default:
                    break;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Methods

        private void Salvar(HttpContext context)
        {
            Model.Entities.Agenda agenda = new Model.Entities.Agenda();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ReturnJSON<Model.Entities.Agenda> returnJSON = new ReturnJSON<Model.Entities.Agenda>();
            String retorno = null;

            try
            {
                String json = new StreamReader(context.Request.InputStream).ReadToEnd();
                agenda = (Model.Entities.Agenda)serializer.Deserialize<Model.Entities.Agenda>(json);

                // Recupera novamente o EstadoAgenda, pois no client/browser só foi possível enviar o seu ID.
                agenda.EstadoAgenda =
                    new EstadoAgendaRepository()
                        .Details(new EstadoAgenda() { EstadoAgendaID = agenda.EstadoAgenda.EstadoAgendaID });

                // Recupera novamente o MedicoExecucaoAgenda, pois no client/browser só foi possível enviar o seu ID.
                agenda.MedicoExecucaoAgenda =
                    new MedicoExecucaoAgendaRepository()
                        .Details(new MedicoExecucaoAgenda()
                        {
                            MedicoExecucaoAgendaID = agenda.MedicoExecucaoAgenda.MedicoExecucaoAgendaID
                        });

                if (!agenda.DataHoraEhNoiteOuFimSemana(agenda.DataHoraEvento.Value))
                {
                    returnJSON.Message = "O agendamento da congelação <b>não</b> pode ser realizado após as 19:00 e aos finais de semana.";
                    returnJSON.ReturnCode = Enum.GetName(typeof(ReturnType), ReturnType.WARNING);
                    retorno = serializer.Serialize(returnJSON);
                    context.Response.ContentType = "text/json";
                    context.Response.ContentEncoding = Encoding.UTF8;
                    context.Response.Write(retorno);
                    return;
                }

                // É novo registro? Verifica se não está sendo gravado uma nova agenda com data retroativa.
                if (agenda.AgendaID.HasValue && agenda.AgendaID.Value == 0)
                {
                    if (agenda.DataHoraEhRetroativa(agenda.DataHoraEvento.Value))
                    {
                        returnJSON.Message = "Não é permitido agendar uma congelação com data/hora retroativa.";
                        returnJSON.ReturnCode = Enum.GetName(typeof(ReturnType), ReturnType.WARNING);
                        retorno = serializer.Serialize(returnJSON);
                        context.Response.ContentType = "text/json";
                        context.Response.ContentEncoding = Encoding.UTF8;
                        context.Response.Write(retorno);
                        return;
                    }
                }

                String assuntoSMS = String.Empty;

                // Registro em edição?
                if (agenda.AgendaID.HasValue && agenda.AgendaID.Value > 0)
                {
                    assuntoSMS = "Atualização de agendamento de congelação";

                    if (agenda.EstadoAgenda.Estado.Equals("Confirmado"))
                    {
                        // Consulta a existência de lembrestes configurados para a agenda.
                        List<NotificacaoAgenda> notificacoesExistentes = new NotificacaoAgendaRepository()
                            .Retreave(new NotificacaoAgenda() { Agenda = agenda });

                        // Se não houver nenhum lembrete, cria com base nas configurações existentes (recorrência de notificação).
                        if (notificacoesExistentes.Count == 0)
                        {
                            List<ConfiguracaoNotificacaoAgenda> configuracaoNotificacaoAgendas
                                = new ConfiguracaoNotificacaoAgendaRepository()
                                    .Retreave(new ConfiguracaoNotificacaoAgenda());

                            foreach (ConfiguracaoNotificacaoAgenda configuracao in configuracaoNotificacaoAgendas)
                            {
                                DateTime inicioNotificacao = agenda.DataHoraEvento.Value;

                                // Verifica a unidade de tempo da configuração para determinar a data/hora inicial da notificação.
                                switch (configuracao.UnidadeTempoAgenda.Unidade)
                                {
                                    case "Minutos":
                                        inicioNotificacao = inicioNotificacao.AddMinutes(-Convert.ToDouble(configuracao.Tempo));
                                        break;

                                    case "Horas":
                                        inicioNotificacao = inicioNotificacao.AddHours(-Convert.ToDouble(configuracao.Tempo));
                                        break;

                                    case "Dias":
                                        inicioNotificacao = inicioNotificacao.AddDays(-Convert.ToDouble(configuracao.Tempo));
                                        break;

                                    case "Semanas":
                                        inicioNotificacao = inicioNotificacao.AddDays(-Convert.ToDouble(configuracao.Tempo) * 7);
                                        break;

                                    default:
                                        break;
                                }

                                /* 
                                 * Verifica se a data/hora do momento está dentro do intervalo:
                                 * data/hora de início da notificação e a data/hora evento (fim) da agenda.
                                 * Como é uma nova agenda ou alteração de agenda, se a data/hora evento estiver
                                 * dentro do tempo de intervalo de notificação, a notificãção não será criada.
                                 */
                                if (DateTime.Now >= inicioNotificacao && DateTime.Now <= agenda.DataHoraEvento.Value)
                                {
                                    // Evita criar o registro no banco.
                                    continue;
                                }

                                NotificacaoAgenda notificacaoAgenda = new NotificacaoAgenda();
                                notificacaoAgenda.Agenda = agenda;
                                notificacaoAgenda.ConfiguracaoNotificacaoAgenda = configuracao;
                                new NotificacaoAgendaRepository().Create(notificacaoAgenda);
                            }
                        }
                        // Se não, se já existir o lembrete (por que a mesma agenda foi cancelada e confirmada posteriormente)
                        //, mantém ativado.
                        else
                        {
                            NotificacaoAgenda notificacaoAgenda = new NotificacaoAgenda();
                            notificacaoAgenda.Agenda = agenda;
                            notificacaoAgenda.Utilizado = false;
                            notificacaoAgenda.Ativo = true;
                            new NotificacaoAgendaRepository().ReativarNotificacao(notificacaoAgenda);
                        }
                    }
                    else if (agenda.EstadoAgenda.Estado.Equals("Cancelado")
                            || agenda.EstadoAgenda.Estado.Equals("Agendado")
                            || agenda.EstadoAgenda.Estado.Equals("Finalizado")
                        )
                    {
                        NotificacaoAgenda notificacaoAgenda = new NotificacaoAgenda();
                        notificacaoAgenda.Agenda = agenda;
                        new NotificacaoAgendaRepository().Delete(notificacaoAgenda);
                    }

                    new AgendaRepository().Update(agenda);
                }
                // Ou é novo registro?
                else
                {
                    agenda.AgendaID = new AgendaRepository().CreateWithReturnID(agenda);
                    assuntoSMS = "Novo agendamento de congelação";

                    // Se a nova agenda é para o mesmo dia, evidencia no assunto do SMS.
                    if (agenda.DataHoraEvento.Value.Day == DateTime.Now.Day
                        && agenda.DataHoraEvento.Value.Month == DateTime.Now.Month
                        && agenda.DataHoraEvento.Value.Year == DateTime.Now.Year)
                    {
                        assuntoSMS = "*** Atenção *** Uma nova congelação foi marcada hoje.";
                    }
                }

                String mensagemSMS = Model.SMS.FormataMensagemSMS(agenda, assuntoSMS);

                if (ConfigurationManager.AppSettings["ENVIAR_SMS"] != null)
                {
                    String numeroTelefone = String.Format("{0}{1}", CodigoE164, agenda.MedicoExecucaoAgenda.Celular);

                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["ENVIAR_SMS"]))
                    {
                        String MessageId = TwilioServices.SendSMS(numeroTelefone, mensagemSMS);

                        LogSmsAgenda logSmsAgenda = new LogSmsAgenda();
                        logSmsAgenda.Agenda = agenda;
                        logSmsAgenda.SMSEnviado = true;
                        logSmsAgenda.SMSDataProcessamento = DateTime.Now;
                        logSmsAgenda.SMSMessageID = MessageId;
                        logSmsAgenda.Observacao =
                            String.Format
                            (
                                "Origem SITE: mensagem destinado ao nº {0} enviada com sucesso ao servidor de serviço SMS."
                                , numeroTelefone
                            );
                        new LogSmsAgendaRepository().Create(logSmsAgenda);
                    }
                    else
                    {
                        LogSmsAgenda logSmsAgenda = new LogSmsAgenda();
                        logSmsAgenda.Agenda = agenda;
                        logSmsAgenda.SMSEnviado = false;
                        logSmsAgenda.SMSDataProcessamento = DateTime.Now;
                        logSmsAgenda.SMSMessageID = null;
                        logSmsAgenda.Observacao =
                            String.Format
                            (
                                "Origem SITE: mensagem destinado ao nº {0} não foi enviada, o serviço de envio de SMS está desabilitado. Utilize a chave ENVIAR_SMS do web.config da aplicação."
                                , numeroTelefone
                            );
                        new LogSmsAgendaRepository().Create(logSmsAgenda);
                    }
                }

                returnJSON.Message = "Registro salvo com sucesso.";
                returnJSON.ReturnCode = Enum.GetName(typeof(ReturnType), ReturnType.SUCCESS);
                context.Response.StatusCode = 201;
            }
            catch (SqlException e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                returnJSON.Message = "Não foi possível salvar o registro.";
                returnJSON.ReturnCode = Enum.GetName(typeof(ReturnType), ReturnType.ERROR);
                context.Response.StatusCode = 500;
            }
            catch (Exception e)
            {
                LogSmsAgenda logSmsAgenda = new LogSmsAgenda();
                logSmsAgenda.Agenda = agenda;
                logSmsAgenda.SMSEnviado = false;
                logSmsAgenda.SMSDataProcessamento = DateTime.Now;
                logSmsAgenda.Observacao =
                    String.Format
                    (
                        "Ocorreu uma falha ao enviar o SMS para o nº {0}. Detalhes: {1}"
                        , String.Format("{0}{1}", CodigoE164, agenda.MedicoExecucaoAgenda.Celular)
                        , e.Message
                    );
                new LogSmsAgendaRepository().Create(logSmsAgenda);

                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);

                returnJSON.Message = "Não foi possível enviar o SMS.";
                returnJSON.ReturnCode = Enum.GetName(typeof(ReturnType), ReturnType.ERROR);
                context.Response.StatusCode = 500;
            }

            retorno = serializer.Serialize(returnJSON);
            context.Response.ContentType = "text/json";
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.Write(retorno);
        }

        private void Consultar(HttpContext context)
        {
            Model.Entities.Agenda agenda = new Model.Entities.Agenda();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ReturnJSON<Model.Entities.Agenda> returnJSON = new ReturnJSON<Model.Entities.Agenda>();
            String retorno = null;

            try
            {
                String json = new StreamReader(context.Request.InputStream).ReadToEnd();
                agenda = (Model.Entities.Agenda)serializer.Deserialize<Model.Entities.Agenda>(json);
                agenda = new AgendaRepository().Details(new Model.Entities.Agenda() { AgendaID = agenda.AgendaID });

                returnJSON.Entity = agenda;
                returnJSON.ReturnCode = Enum.GetName(typeof(ReturnType), ReturnType.SUCCESS);
                context.Response.StatusCode = 200;
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                returnJSON.Message = "Não foi possível recuperar os registros.";
                returnJSON.ReturnCode = Enum.GetName(typeof(ReturnType), ReturnType.ERROR);
                context.Response.StatusCode = 500;
            }

            retorno = serializer.Serialize(returnJSON);
            context.Response.ContentType = "text/json";
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.Write(retorno);
        }

        #endregion
    }
}
