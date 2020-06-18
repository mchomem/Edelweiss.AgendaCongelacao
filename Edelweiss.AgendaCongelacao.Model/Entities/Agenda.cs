using System;

namespace Edelweiss.AgendaCongelacao.Model.Entities
{
    [Serializable()]
    public class Agenda
    {
        #region Properties

        public Int32? AgendaID { get; set; }
        public DateTime? DataHoraEvento { get; set; }
        public String Local { get; set; }
        public String NomeMedico { get; set; }
        public MedicoExecucaoAgenda MedicoExecucaoAgenda { get; set; }
        public String NomePaciente { get; set; }
        public String Convenio { get; set; }
        public String Procedimento { get; set; }
        public String TelefoneContato { get; set; }
        public EstadoAgenda EstadoAgenda { get; set; }
        public Boolean? Ativo { get; set; }

        #endregion

        #region Constructors

        public Agenda()
        {
            this.MedicoExecucaoAgenda = new MedicoExecucaoAgenda();
            this.EstadoAgenda = new EstadoAgenda();
        }

        #endregion

        #region Methods (roles)

        public Boolean DataHoraEhNoiteOuFimSemana(DateTime dataHoraEvento)
        {
            if ((dataHoraEvento.DayOfWeek == DayOfWeek.Saturday)
                || (dataHoraEvento.DayOfWeek == DayOfWeek.Sunday)
                || dataHoraEvento.Hour >= 19)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Boolean DataHoraEhRetroativa(DateTime dataHoraEvento)
        {
            return dataHoraEvento < DateTime.Now;
        }

        #endregion
    }
}
