using Edelweiss.AgendaCongelacao.Model;
using Edelweiss.AgendaCongelacao.Model.Entities;
using Edelweiss.AgendaCongelacao.Model.Repositories;
using Edelweiss.Utils;
using Edelweiss.Utils.Web.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Edelweiss.AgendaCongelacao.Site.Admin
{
    public partial class Usuario_Listagem : EdelweissAdminPage
    {
        #region Properties

        public List<UsuarioAdministracaoAgenda> VsUsuarioAdministracaoAgenda
        {
            get
            {
                if (ViewState["UsuarioAdministracaoAgendas"] != null)
                    return (List<UsuarioAdministracaoAgenda>)ViewState["UsuarioAdministracaoAgendas"];
                return null;
            }
            set
            {
                ViewState["UsuarioAdministracaoAgendas"] = value;
            }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.CarregarUsuarios();
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            this.IrParaPagina("Usuario-Manutencao.aspx");
        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            this.LimparFiltros();
            this.CarregarUsuarios();
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            this.CarregarUsuarios();
        }

        protected void gvUsuario_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvUsuario.PageIndex = e.NewPageIndex;
            this.CarregarUsuarios();
        }

        protected void gvUsuario_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Editar":
                    this.IrParaPagina
                        (
                            String.Format
                            (
                                "Usuario-Manutencao.aspx?UsuarioAdministracaoAgendaID={0}"
                                , e.CommandArgument
                            )
                        );
                    break;

                case "Excluir":
                    Int32 id = Convert.ToInt32(e.CommandArgument);
                    this.Excluir(id);
                    this.CarregarUsuarios();
                    break;

                default:
                    break;
            }
        }

        protected void gvUsuario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                UsuarioAdministracaoAgenda usuario = (UsuarioAdministracaoAgenda)e.Row.DataItem;

                Literal litNome = (Literal)e.Row.FindControl("litNome");
                Literal litAtivo = (Literal)e.Row.FindControl("litAtivo");

                litNome.Text = usuario.Nome;
                Boolean ativo = usuario.Ativo.Value;
                litAtivo.Text = "<span style='font-weight:bold;color:" + (ativo ? "green" : "red") + ";'>" + (ativo ? "Sim" : "Não") + "</span>";
            }
        }

        protected void gvUsuario_Sorting(object sender, GridViewSortEventArgs e)
        {
            List<UsuarioAdministracaoAgenda> usuarios = VsUsuarioAdministracaoAgenda;
            String sortExpression = e.SortExpression;
            String sortDirection = GridViewColumnSort.ReturnSortDirection(sortExpression);

            if (sortDirection.Equals("ASC"))
            {
                switch (sortExpression)
                {
                    case "Nome":
                        usuarios = usuarios.OrderBy(u => u.Nome).ToList();
                        break;
                }
            }
            else
            {
                switch (sortExpression)
                {
                    case "Nome":
                        usuarios = usuarios.OrderByDescending(u => u.Nome).ToList();
                        break;
                }
            }

            VsUsuarioAdministracaoAgenda = usuarios;
            this.gvUsuario.PageIndex = 0;
            this.gvUsuario.DataSource = usuarios;
            this.gvUsuario.DataBind();
        }

        #endregion

        #region Methods

        private void CarregarUsuarios()
        {
            try
            {
                UsuarioAdministracaoAgenda usuario = new UsuarioAdministracaoAgenda();

                if (this.txtNome.Text.Length > 0)
                    usuario.Nome = this.txtNome.Text;

                if (this.rbtTodos.Checked)
                {
                    usuario.Ativo = null;
                }
                else if (this.rbtAtivo.Checked)
                {
                    usuario.Ativo = true;
                }
                else
                {
                    usuario.Ativo = false;
                }

                List<UsuarioAdministracaoAgenda> usuarios
                    = new UsuarioAdministracaoAgendaRepository().Retreave(usuario);

                VsUsuarioAdministracaoAgenda = usuarios;
                this.gvUsuario.DataSource = usuarios;
                this.gvUsuario.DataBind();
                this.lblTotalRegistros.Text = usuarios.Count.ToString();
                this.infoTotalRegistros.Visible = true;
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                this.msgDialog.Show("Erro", "Ocorreu uma falha ao carregar os registros de usuários.", UserControl.Message.Type.Error);
            }
        }

        private void LimparFiltros()
        {
            this.txtNome.Text = String.Empty;
            this.rbtAtivo.Checked = false;
            this.rbtDesativado.Checked = false;
            this.rbtTodos.Checked = true;
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

        private void Excluir(Int32 id)
        {
            try
            {
                UsuarioAdministracaoAgenda usuario = new UsuarioAdministracaoAgenda();
                usuario.UsuarioAdministracaoAgendaID = id;
                new UsuarioAdministracaoAgendaRepository().Delete(usuario);
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                this.msgDialog.Show("Erro", "Ocorreu uma falha ao excluir o registro.", UserControl.Message.Type.Error);
            }
        }

        #endregion
    }
}