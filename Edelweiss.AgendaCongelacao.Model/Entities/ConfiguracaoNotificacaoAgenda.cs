using System;

namespace Edelweiss.AgendaCongelacao.Model.Entities
{
    [Serializable()]
    public class ConfiguracaoNotificacaoAgenda
    {
        #region Properties

        public Int32? ConfiguracaoNotificacaoAgendaID { get; set; }

        public Int32? Tempo { get; set; }

        public UnidadeTempoAgenda UnidadeTempoAgenda { get; set; }

        #endregion

        #region Constructors

        public ConfiguracaoNotificacaoAgenda()
        {
            this.UnidadeTempoAgenda = new UnidadeTempoAgenda();
        }

        #endregion
    }
}
