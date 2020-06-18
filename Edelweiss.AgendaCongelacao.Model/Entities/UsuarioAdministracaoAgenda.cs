using System;

namespace Edelweiss.AgendaCongelacao.Model.Entities
{
    [Serializable()]
    public class UsuarioAdministracaoAgenda
    {
        #region Properties

        public Int32? UsuarioAdministracaoAgendaID { get; set; }
        public String Login { get; set; }
        public String Senha { get; set; }
        public String Nome { get; set; }
        public Boolean? Ativo { get; set; }

        #endregion
    }
}
