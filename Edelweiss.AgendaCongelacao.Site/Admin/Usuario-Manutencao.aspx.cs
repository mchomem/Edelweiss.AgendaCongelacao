using Edelweiss.AgendaCongelacao.Model;
using Edelweiss.AgendaCongelacao.Model.Entities;
using Edelweiss.AgendaCongelacao.Model.Repositories;
using Edelweiss.Utils;
using System;

namespace Edelweiss.AgendaCongelacao.Site.Admin
{
    public partial class Usuario_Manutencao : EdelweissAdminPage
    {
        #region Properties

        public Int32? UsuarioAdministracaoAgendaID
        {
            get
            {
                if (Request.QueryString["UsuarioAdministracaoAgendaID"] != null)
                    return Convert.ToInt32(Request.QueryString["UsuarioAdministracaoAgendaID"]);
                return null;
            }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (UsuarioAdministracaoAgendaID != null)
                {
                    Int32 id = UsuarioAdministracaoAgendaID.Value;
                    this.hifUsuarioAdministracaoAgendaID.Value = id.ToString();
                    this.CarregarParaEdicao(id);
                }
            }

            this.txtSenha.Attributes["type"] = "password";
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.IrParaPagina("Usuario-Listagem.aspx");
        }

        #endregion

        private void CarregarParaEdicao(Int32 id)
        {
            try
            {
                UsuarioAdministracaoAgenda usuario = new UsuarioAdministracaoAgenda();
                usuario =
                    new UsuarioAdministracaoAgendaRepository()
                        .Details(new UsuarioAdministracaoAgenda() { UsuarioAdministracaoAgendaID = id });

                this.txtNome.Text = usuario.Nome;
                this.txtLogin.Text = usuario.Login;
                this.txtSenha.Text = Cypher.Decrypt(usuario.Senha);
                this.txtSenha.Attributes["type"] = "password";
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                this.msgDialog.Show("Erro", "Ocorreu uma falha ao carregar o registro para edição.", UserControl.Message.Type.Error);
            }
        }

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