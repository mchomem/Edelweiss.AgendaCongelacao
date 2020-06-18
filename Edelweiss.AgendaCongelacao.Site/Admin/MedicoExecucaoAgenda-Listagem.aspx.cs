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
    public partial class MedicoExecucaoAgenda_Listagem : EdelweissAdminPage
    {
        #region Properties

        public List<MedicoExecucaoAgenda> VsMedicoExecucaoAgenda
        {
            get
            {
                if (ViewState["MedicoExecucaoAgendas"] != null)
                    return (List<MedicoExecucaoAgenda>)ViewState["MedicoExecucaoAgendas"];
                return null;
            }
            set
            {
                ViewState["MedicoExecucaoAgendas"] = value;
            }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.CarregarMedicoExecucaoAgendas();
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            this.IrParaPagina("MedicoExecucaoAgenda-Manutencao.aspx");
        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            this.LimparFiltros();
            this.CarregarMedicoExecucaoAgendas();
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            this.CarregarMedicoExecucaoAgendas();
        }

        protected void gvMedicoExecucaoAgenda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvMedicoExecucaoAgenda.PageIndex = e.NewPageIndex;
            this.CarregarMedicoExecucaoAgendas();
        }

        protected void gvMedicoExecucaoAgenda_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Editar":
                    this.IrParaPagina
                        (
                            String.Format
                            (
                                "MedicoExecucaoAgenda-Manutencao.aspx?MedicoExecucaoAgendaID={0}"
                                , e.CommandArgument
                            )
                        );
                    break;

                case "Excluir":
                    Int32 id = Convert.ToInt32(e.CommandArgument);
                    this.Excluir(id);
                    this.CarregarMedicoExecucaoAgendas();
                    break;

                default:
                    break;
            }
        }

        protected void gvMedicoExecucaoAgenda_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                MedicoExecucaoAgenda medicoExecucaoAgenda = (MedicoExecucaoAgenda)e.Row.DataItem;

                Literal litNome = (Literal)e.Row.FindControl("litNome");
                Literal litEmail = (Literal)e.Row.FindControl("litEmail");
                Literal litCelular = (Literal)e.Row.FindControl("litCelular");

                litNome.Text = medicoExecucaoAgenda.Nome;
                litEmail.Text = medicoExecucaoAgenda.Email;
                String celularMascara =
                    String.Format
                    (
                        "({0}) {1}"
                        , medicoExecucaoAgenda.Celular.Substring(0, 2)
                        , medicoExecucaoAgenda.Celular.Remove(0, 2)
                    );
                litCelular.Text = celularMascara;
            }
        }

        protected void gvMedicoExecucaoAgenda_Sorting(object sender, GridViewSortEventArgs e)
        {
            List<MedicoExecucaoAgenda> medicoExecucaoAgenda = VsMedicoExecucaoAgenda;
            String sortExpression = e.SortExpression;
            String sortDirection = GridViewColumnSort.ReturnSortDirection(sortExpression);

            if (sortDirection.Equals("ASC"))
            {
                switch (sortExpression)
                {
                    case "Nome":
                        medicoExecucaoAgenda = medicoExecucaoAgenda.OrderBy(c => c.Nome).ToList();
                        break;

                    case "Email":
                        medicoExecucaoAgenda = medicoExecucaoAgenda.OrderBy(c => c.Email).ToList();
                        break;
                }
            }
            else
            {
                switch (sortExpression)
                {
                    case "Nome":
                        medicoExecucaoAgenda = medicoExecucaoAgenda.OrderByDescending(c => c.Nome).ToList();
                        break;

                    case "Email":
                        medicoExecucaoAgenda = medicoExecucaoAgenda.OrderByDescending(c => c.Email).ToList();
                        break;
                }
            }

            VsMedicoExecucaoAgenda = medicoExecucaoAgenda;
            this.gvMedicoExecucaoAgenda.PageIndex = 0;
            this.gvMedicoExecucaoAgenda.DataSource = medicoExecucaoAgenda;
            this.gvMedicoExecucaoAgenda.DataBind();
        }

        #endregion

        #region

        private void CarregarMedicoExecucaoAgendas()
        {
            try
            {
                MedicoExecucaoAgenda medicoExecucaoAgenda = new MedicoExecucaoAgenda();

                if (this.txtNome.Text.Length > 0)
                    medicoExecucaoAgenda.Nome = this.txtNome.Text;

                if (this.txtEmail.Text.Length > 0)
                    medicoExecucaoAgenda.Email = this.txtEmail.Text;

                if (this.txtCelular.Text.Length > 0)
                    medicoExecucaoAgenda.Celular = Text.OnlyNumbers(this.txtCelular.Text);

                List<MedicoExecucaoAgenda> medicoExecucaoAgendas
                    = new MedicoExecucaoAgendaRepository().Retreave(medicoExecucaoAgenda);

                VsMedicoExecucaoAgenda = medicoExecucaoAgendas;
                this.gvMedicoExecucaoAgenda.DataSource = medicoExecucaoAgendas;
                this.gvMedicoExecucaoAgenda.DataBind();
                this.lblTotalRegistros.Text = medicoExecucaoAgendas.Count.ToString();
                this.infoTotalRegistros.Visible = true;
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                this.msgDialog.Show("Erro", "Ocorreu uma falha ao carregar os registros de médicos de execução de agenda.", UserControl.Message.Type.Error);
            }
        }

        private void LimparFiltros()
        {
            this.txtNome.Text = String.Empty;
            this.txtEmail.Text = String.Empty;
            this.txtCelular.Text = String.Empty;
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
                MedicoExecucaoAgenda medicoExecucaoAgenda = new MedicoExecucaoAgenda();
                medicoExecucaoAgenda.MedicoExecucaoAgendaID = id;
                new MedicoExecucaoAgendaRepository().Delete(medicoExecucaoAgenda);
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