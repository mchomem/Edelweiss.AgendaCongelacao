using System;

namespace Edelweiss.AgendaCongelacao.Site
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Response.Redirect("CalendarioAgenda.aspx", false);
        }
    }
}