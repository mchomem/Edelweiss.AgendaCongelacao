using System;

namespace Edelweiss.AgendaCongelacao.Model.Entities
{
    [Serializable()]
    public class EstadoAgenda
    {
        #region Properties

        public Int32? EstadoAgendaID { get; set; }
        public String Estado { get; set; }
        public String CorHexaCss { get; set; }
        public Int32? Ordem { get; set; }

        #endregion
    }
}
