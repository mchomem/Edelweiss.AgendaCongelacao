using Edelweiss.Utils;
using System;

namespace Edelweiss.AgendaCongelacao.Site.Admin.UserControl
{
    public partial class Menu : System.Web.UI.UserControl
    {
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            ;
        }

        protected void lkbDashboard_Click(object sender, EventArgs e)
        {
            this.IrParaPagina("Dashboard.aspx");
        }

        protected void lbkMenuItemLogSmsAgenda_Click(object sender, EventArgs e)
        {
            this.IrParaPagina("LogSmsAgenda-Listagem.aspx");
        }

        protected void lkbMenuItemAgenda_Click(object sender, EventArgs e)
        {
            this.IrParaPagina("Agenda-Listagem.aspx");
        }

        protected void lbkMenuItemConfiguracaoNotificacao_Click(object sender, EventArgs e)
        {
            this.IrParaPagina("ConfiguracaoNotificacaoAgenda-Listagem.aspx");
        }

        protected void lbkMenuItemMedicoExecucaoAgenda_Click(object sender, EventArgs e)
        {
            this.IrParaPagina("MedicoExecucaoAgenda-Listagem.aspx");
        }

        protected void lbkMenuItemUsuario_Click(object sender, EventArgs e)
        {
            this.IrParaPagina("Usuario-Listagem.aspx");
        }

        #endregion

        #region

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
