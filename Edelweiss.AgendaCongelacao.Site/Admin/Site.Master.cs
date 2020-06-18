using Edelweiss.Utils;
using System;

namespace Edelweiss.AgendaCongelacao.Site.Admin
{
    public partial class Site : EdelweissAdminMasterPage
    {
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (UsuarioLogado != null)
            {
                this.ExibirUsuarioLogado();
            }
            else
            {
                this.IrParaPagina("Login.aspx");
            }
        }

        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            this.Page.Response.Redirect("Login.aspx", false);
        }

        #endregion

        #region Methods

        private void ExibirUsuarioLogado()
        {
            Int32 maxChar = 20;

            String usuario =
                (UsuarioLogado.Nome.Length > maxChar)
                ?
                UsuarioLogado.Nome.Substring(0, maxChar)
                :
                UsuarioLogado.Nome;

            this.lblUsuarioLogado.Text = usuario;
        }

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