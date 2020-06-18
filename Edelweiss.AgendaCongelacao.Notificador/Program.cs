using Edelweiss.AgendaCongelacao.Model.Entities;
using Edelweiss.AgendaCongelacao.Model.Repositories;
using Edelweiss.AgendaCongelacao.SMS.Services;
using Edelweiss.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;

namespace Edelweiss.AgendaCongelacao.Notificador
{
    class Program
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

        #region Methods

        static void Main(string[] args)
        {
            String titulo = "Edelweiss - Agendamento congelação";
            String subtitulo = "Serviço de notificação por SMS";
            Console.Title = String.Format("{0} : {1}", titulo, subtitulo);
            Console.WriteLine("".PadRight(Console.WindowWidth, '='));
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("".PadRight((Console.WindowWidth / 2) - (titulo.Length / 2), ' ') + titulo.ToUpper());
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("");
            Console.WriteLine("".PadRight((Console.WindowWidth / 2) - (subtitulo.Length / 2), ' ') + subtitulo.ToUpper());
            Console.WriteLine("".PadRight(Console.WindowWidth, '='));
            Console.WindowWidth = 122;

            try
            {
                Console.WriteLine("\r\nLog:\r\n");

                Program p = new Program();
                ConsoleLogMessage("Processo iniciado.");
                p.IniciarProcesso();
                ConsoleLogMessage("Processo concluído.");

                Console.WriteLine("\r\nOBS: Este console será encerrado em 1 minuto.");
                Thread.Sleep(Convert.ToInt32(new TimeSpan(0, 1, 0).TotalMilliseconds));
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                ConsoleLogMessage(String.Format("Ocorreu um erro. Detalhes: {0}", e.Message), ConsoleMessageType.ERROR);
            }
        }

        private void IniciarProcesso()
        {
            // Consultar as agendas disponíveis (agendas ativas com data de evento futura e com estado = "Confirmado").
            List<Agenda> agendas = new AgendaRepository()
                .Retreave(new Agenda(), null, null)
                    .Where(a => a.DataHoraEvento.Value > DateTime.Now
                            && a.Ativo.Value == true
                            && a.EstadoAgenda.Estado.Equals("Confirmado"))
                        .ToList();

            foreach (Agenda agenda in agendas)
            {
                // Verifica se existem notificações ativas e não utilizadas para a agenda.
                List<NotificacaoAgenda> notificacoes = new NotificacaoAgendaRepository()
                    .Retreave
                    (
                        new NotificacaoAgenda()
                        {
                            Agenda = agenda
                            ,
                            Utilizado = false
                            ,
                            Ativo = true
                        }
                    );

                // Existem notificações para a agenda?
                if (notificacoes.Count > 0)
                {
                    /*
                     * Verificar para cada notificação se chegou o momento de
                     * enviar(18, 4 ou 2 horas antes da data/hora evento da agenda).
                     */
                    foreach (NotificacaoAgenda notificacao in notificacoes)
                    {
                        // Consulta a configuração
                        ConfiguracaoNotificacaoAgenda configuracao = new ConfiguracaoNotificacaoAgenda();
                        configuracao = new ConfiguracaoNotificacaoAgendaRepository()
                            .Details
                            (
                                new ConfiguracaoNotificacaoAgenda()
                                {
                                    ConfiguracaoNotificacaoAgendaID =
                                    notificacao.ConfiguracaoNotificacaoAgenda.ConfiguracaoNotificacaoAgendaID
                                }
                            );

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
                         */
                        if (DateTime.Now >= inicioNotificacao && DateTime.Now <= agenda.DataHoraEvento.Value)
                        {
                            String mensagemSMS =
                                Model.SMS.FormataMensagemSMS
                                (
                                    agenda
                                    , String.Format
                                    (
                                        "Lembrete ({0} {1} antes) para o agendamento de congelação."
                                        , configuracao.Tempo
                                        , configuracao.UnidadeTempoAgenda.Unidade
                                    )
                                );

                            this.EnviarSMS(agenda, configuracao, mensagemSMS);

                            notificacao.Utilizado = true;
                            notificacao.ConfiguracaoNotificacaoAgenda = configuracao;
                            new NotificacaoAgendaRepository().UtilizarNotificacao(notificacao);
                        }
                    }
                }
            }
        }

        private void EnviarSMS(Agenda agenda, ConfiguracaoNotificacaoAgenda configuracao, String mensagemSMS)
        {
            if (ConfigurationManager.AppSettings["ENVIAR_SMS"] != null)
            {
                String numeroTelefone = String.Format("{0}{1}", CodigoE164, agenda.MedicoExecucaoAgenda.Celular);

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["ENVIAR_SMS"]))
                {
                    String MessageId = TwilioServices.SendSMS(numeroTelefone, mensagemSMS);
                    ConsoleLogMessage("SMS enviado", ConsoleMessageType.SUCCESS);

                    LogSmsAgenda logSmsAgenda = new LogSmsAgenda();
                    logSmsAgenda.Agenda = agenda;
                    logSmsAgenda.SMSEnviado = true;
                    logSmsAgenda.SMSDataProcessamento = DateTime.Now;
                    logSmsAgenda.SMSMessageID = MessageId;
                    logSmsAgenda.Observacao =
                        String.Format
                        (
                            "Origem SERVIÇO: mensagem destinado ao nº {0} como lembrete ({1} {2} antes), enviada com sucesso ao servidor de serviço SMS."
                            , numeroTelefone
                            , configuracao.Tempo
                            , configuracao.UnidadeTempoAgenda.Unidade
                        );
                    new LogSmsAgendaRepository().Create(logSmsAgenda);
                    ConsoleLogMessage("Log gravado", ConsoleMessageType.SUCCESS);
                }
                else
                {
                    ConsoleLogMessage("SMS não foi enviado.", ConsoleMessageType.WARNING);
                    LogSmsAgenda logSmsAgenda = new LogSmsAgenda();
                    logSmsAgenda.Agenda = agenda;
                    logSmsAgenda.SMSEnviado = false;
                    logSmsAgenda.SMSDataProcessamento = DateTime.Now;
                    logSmsAgenda.SMSMessageID = null;
                    logSmsAgenda.Observacao =
                        String.Format
                            (
                                "Origem SERVIÇO: mensagem destinado ao nº {0} como lembrete ({1} {2} antes) não foi enviada, o serviço de envio de SMS está desabilitado. Utilize a chave ENVIAR_SMS do app.config da aplicação."
                                , numeroTelefone
                                , configuracao.Tempo
                                , configuracao.UnidadeTempoAgenda.Unidade
                            );
                    new LogSmsAgendaRepository().Create(logSmsAgenda);
                    ConsoleLogMessage("Log gravado", ConsoleMessageType.SUCCESS);
                }
            }
        }

        private static void ConsoleLogMessage(String message, ConsoleMessageType consoleMessageType = ConsoleMessageType.INFORMATION)
        {
            ConsoleColor consoleColor = ConsoleColor.Gray;

            switch (consoleMessageType)
            {
                case ConsoleMessageType.ERROR:
                    consoleColor = ConsoleColor.Red;
                    break;

                case ConsoleMessageType.INFORMATION:
                    consoleColor = ConsoleColor.Gray;

                    break;
                case ConsoleMessageType.SUCCESS:
                    consoleColor = ConsoleColor.Green;
                    break;

                case ConsoleMessageType.WARNING:
                    consoleColor = ConsoleColor.Yellow;
                    break;

                default:
                    consoleColor = ConsoleColor.Gray;
                    break;
            }

            Console.Write(String.Format("[{0}]: ", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(String.Format("{0}", message));
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        #endregion
    }

    enum ConsoleMessageType
    {
        ERROR
        , INFORMATION
        , SUCCESS
        , WARNING
    }
}
