using Edelweiss.AgendaCongelacao.Model;
using Edelweiss.AgendaCongelacao.Model.Entities;

namespace Edelweiss.AgendaCongelacao.Site.Admin
{
    public class EdelweissAdminMasterPage : System.Web.UI.MasterPage
    {
        public UsuarioAdministracaoAgenda UsuarioLogado
        {
            get
            {
                if (Page is EdelweissAdminPage)
                    return ((EdelweissAdminPage)Page).UsuarioLogado;
                else
                    return null;
            }
            set
            {
                if (Page is EdelweissAdminPage)
                    ((EdelweissAdminPage)Page).UsuarioLogado = value;
            }
        }
    }
}