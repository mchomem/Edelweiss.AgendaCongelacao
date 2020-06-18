using System;

namespace Edelweiss.AgendaCongelacao.Model.Entities
{
    [Serializable()]
    public class UnidadeTempoAgenda
    {
        #region Properties

        public Int32? UnidadeTempoAgendaID { get; set; }

        public String Unidade { get; set; }

        #endregion
    }
}
