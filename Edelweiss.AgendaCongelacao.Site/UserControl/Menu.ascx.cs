using Edelweiss.Utils;
using System;

namespace Edelweiss.AgendaCongelacao.Site.UserControl
{
    public partial class Menu : System.Web.UI.UserControl
    {
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            ;
        }

        protected void lkbMenuItemAgendamento_Click(object sender, EventArgs e)
        {
            this.IrParaPagina("ListagemAgenda.aspx");
        }

        protected void lkbMenuItemCalendario_Click(object sender, EventArgs e)
        {
            this.IrParaPagina("CalendarioAgenda.aspx");
        }

        #endregion

        #region Methods

        private void IrParaPagina(String pagina)
        {
            try
            {
                this.Response.Redirect(pagina, false);
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
            }
        }

        #endregion
    }
}