using System;

namespace Edelweiss.AgendaCongelacao.Model.Entities
{
    [Serializable()]
    public class LogSmsAgenda
    {
        #region Properties

        public Int32? LogSmsAgendaID { get; set; }
        public Agenda Agenda { get; set; }
        public Boolean? SMSEnviado { get; set; }
        public DateTime? SMSDataProcessamento { get; set; }
        public String SMSMessageID { get; set; }
        public String Observacao { get; set; }

        #endregion

        public LogSmsAgenda()
        {
            this.Agenda = new Agenda();
        }
    }
}
