using Edelweiss.AgendaCongelacao.Model;
using Edelweiss.AgendaCongelacao.Model.Entities;
using Edelweiss.AgendaCongelacao.Model.Repositories;
using Edelweiss.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;

namespace Edelweiss.AgendaCongelacao.NotificadorEmail
{
    class Program
    {
        #region Methods

        static void Main(string[] args)
        {
            String titulo = "Edelweiss - Agendamento congelação";
            String subtitulo = "Serviço de notificação por E-mail para agendas à confirmar.";
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
            List<Agenda> agendas = new List<Agenda>();
            Int32 notificacaoAntecipacaoHoras = Convert.ToInt32(ConfigurationManager.AppSettings["NOTIFICACAO_ANTECIPACAO_HORAS"]);
            agendas = new AgendaRepository().VerificarAgendados(notificacaoAntecipacaoHoras);

            if (agendas.Count != 0)
            {
                this.EnviarEmail(agendas);
            }
        }

        private void EnviarEmail(List<Agenda> agendas)
        {
            List<String> destinatario = new List<String>();
            destinatario = ConfigurationManager.AppSettings["EMAIL_CADASTRO_NOTIFICAO_AGENDAS"].Split(';').ToList();

            StringBuilder corpoHtml = new StringBuilder();
            corpoHtml.Append("<table style=\"border-collapse:collapse; border-width:1px; border-style: solid; border-color:#000000; margin-top:40px; width:100%;\">");
            corpoHtml.Append("<thead><tr><th colspan=\"8\" style=\"border-width:1px; border-bottom-style: solid;\">Detalhes</th></tr></thead>");
            corpoHtml.Append("<tbody style=\"font-size:8px;\">");

            corpoHtml.Append("<tr>");
            corpoHtml.Append("<th>Data/hora</th>");
            corpoHtml.Append("<th>Local</th>");
            corpoHtml.Append("<th>Paciente</th>");
            corpoHtml.Append("<th>Convênio</th>");
            corpoHtml.Append("<th>Médico</th>");
            corpoHtml.Append("<th>Procedimento</th>");
            corpoHtml.Append("<th>TelefoneContato</th>");
            corpoHtml.Append("<th>Estado agenda</th>");
            corpoHtml.Append("</tr>");

            foreach (Agenda agenda in agendas)
            {
                corpoHtml.Append("<tr>");

                corpoHtml.Append("<td><b>");
                corpoHtml.Append(agenda.DataHoraEvento.Value.ToString("dd/MM/yyyy HH:mm"));
                corpoHtml.Append("</b></td>");

                corpoHtml.Append("<td>");
                corpoHtml.Append(agenda.Local);
                corpoHtml.Append("</td>");

                corpoHtml.Append("<td>");
                corpoHtml.Append(agenda.NomePaciente);
                corpoHtml.Append("</td>");

                corpoHtml.Append("<td>");
                corpoHtml.Append(agenda.Convenio);
                corpoHtml.Append("</td>");

                corpoHtml.Append("<td>");
                corpoHtml.Append(agenda.NomeMedico);
                corpoHtml.Append("</td>");

                corpoHtml.Append("<td>");
                corpoHtml.Append(agenda.Procedimento);
                corpoHtml.Append("</td>");

                corpoHtml.Append("<td>");
                corpoHtml.Append(agenda.TelefoneContato);
                corpoHtml.Append("</td>");

                corpoHtml.Append("<td>");
                corpoHtml.Append(agenda.EstadoAgenda.Estado);
                corpoHtml.Append("</td>");

                corpoHtml.Append("</tr>");
            }

            corpoHtml.Append("</tbody>");
            corpoHtml.Append("<tfoot></tfoot>");
            corpoHtml.Append("</table>");

            Email.Send
                (
                    destinatario
                    , "Agendamento de Congelação - Agendas para confirmar"
                    , corpoHtml.ToString()
                    , EmailMessageType.INFORMATION
                );
            ConsoleLogMessage("E-mail enviado", ConsoleMessageType.SUCCESS);
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
