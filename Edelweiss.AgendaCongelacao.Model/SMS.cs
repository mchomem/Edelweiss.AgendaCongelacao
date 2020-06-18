using System;

namespace Edelweiss.AgendaCongelacao.Model
{
    public static class SMS
    {
        public static String FormataMensagemSMS(Model.Entities.Agenda agenda, String assuntoSMS)
        {
            String telefoneComMascara =
                   String.Format
                       (
                           "({0}) {1}"
                           , agenda.TelefoneContato.Substring(0, 2)
                           , agenda.TelefoneContato.Remove(0, 2)
                       );

            return String.Format
                    (
                        "{0}\r\nData: {1}\r\nLocal: {2}\r\nMédico: {3} \r\nTelefone: {4}\r\nEstado agenda: *** {5} ***"
                        , assuntoSMS
                        , agenda.DataHoraEvento.Value
                        , agenda.Local
                        , agenda.NomeMedico
                        , telefoneComMascara
                        , agenda.EstadoAgenda.Estado.ToUpper()
                    );
        }
    }
}
