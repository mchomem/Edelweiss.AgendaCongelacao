using System;

namespace Edelweiss.AgendaCongelacao.Model.Entities
{
    [Serializable()]
    public class MedicoExecucaoAgenda
    {
        #region Properties

        public Int32? MedicoExecucaoAgendaID { get; set; }

        public String Nome { get; set; }

        public String Email { get; set; }

        public String Celular { get; set; }

        #endregion
    }
}
