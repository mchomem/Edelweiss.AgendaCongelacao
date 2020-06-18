using System;

namespace Edelweiss.AgendaCongelacao.Model.Entities
{
    [Serializable()]
    public class NotificacaoAgenda
    {
        #region Properties

        public Agenda Agenda { get; set; }

        public ConfiguracaoNotificacaoAgenda ConfiguracaoNotificacaoAgenda { get; set; }

        public Boolean? Utilizado { get; set; }

        public Boolean? Ativo { get; set; }

        #endregion

        #region Constructors

        public NotificacaoAgenda()
        {
            this.Agenda = new Agenda();
            this.ConfiguracaoNotificacaoAgenda = new ConfiguracaoNotificacaoAgenda();
        }

        #endregion
    }
}
