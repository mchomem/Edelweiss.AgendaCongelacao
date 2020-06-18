using Edelweiss.AgendaCongelacao.Model;
using Edelweiss.AgendaCongelacao.Model.Entities;

namespace Edelweiss.AgendaCongelacao.Site.Admin
{
    public class EdelweissAdminPage : System.Web.UI.Page
    {
        #region Properties

        public UsuarioAdministracaoAgenda UsuarioLogado
        {
            get
            {
                if (Session["UsuarioLogado"] != null)
                    return (UsuarioAdministracaoAgenda)Session["UsuarioLogado"];
                else
                    return null;
            }
            set
            {
                Session["UsuarioLogado"] = value;
            }
        }

        #endregion
    }
}