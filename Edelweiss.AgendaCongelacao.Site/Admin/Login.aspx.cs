using Edelweiss.AgendaCongelacao.Model;
using Edelweiss.AgendaCongelacao.Model.Entities;
using Edelweiss.AgendaCongelacao.Model.Repositories;
using Edelweiss.Utils;
using System;

namespace Edelweiss.AgendaCongelacao.Site.Admin
{
    public partial class Login : EdelweissAdminPage
    {
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            this.txtLogin.Focus();
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            this.Page.Response.Redirect("../CalendarioAgenda.aspx", false);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            this.FazerLogin();
        }

        #endregion

        #region Methods

        private void FazerLogin()
        {
            try
            {
                if (String.IsNullOrEmpty(this.txtLogin.Text) || String.IsNullOrEmpty(this.txtSenha.Text))
                {
                    this.msgDialog.Show("Atenção", "Informe o login e senha.", UserControl.Message.Type.Warning);
                    return;
                }

                UsuarioAdministracaoAgenda usuario = new UsuarioAdministracaoAgenda();
                usuario.Login = this.txtLogin.Text;
                usuario.Senha = Cypher.Encrypt(this.txtSenha.Text);
                usuario = new UsuarioAdministracaoAgendaRepository().Authenticate(usuario);

                if (usuario != null && usuario.Ativo.Value)
                {
                    UsuarioLogado = usuario;
                    this.Page.Response.Redirect("Home.aspx", false);
                }
                else
                {
                    this.msgDialog.Show("Atenção", "Login inválido.", UserControl.Message.Type.Warning);
                }
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                this.msgDialog.Show("Erro", "Falha ao tentar fazer o login.", UserControl.Message.Type.Error);
            }
        }

        #endregion
    }
}