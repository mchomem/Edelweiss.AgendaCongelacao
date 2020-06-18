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
    public partial class ConfiguracaoNotificacaoAgenda_Listagem : EdelweissAdminPage
    {
        #region Properties

        public List<ConfiguracaoNotificacaoAgenda> VsConfiguracaoNotificacaoAgenda
        {
            get
            {
                if (ViewState["ConfiguracaoNotificacaoAgendas"] != null)
                    return (List<ConfiguracaoNotificacaoAgenda>)ViewState["ConfiguracaoNotificacaoAgendas"];
                return null;
            }
            set
            {
                ViewState["ConfiguracaoNotificacaoAgendas"] = value;
            }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.CarregarUnidadeTempo();
                this.CarregarConfiguracaoNotificacao();
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            this.IrParaPagina("ConfiguracaoNotificacaoAgenda-Manutencao.aspx");
        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            this.LimparFiltros();
            this.CarregarConfiguracaoNotificacao();
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            this.CarregarConfiguracaoNotificacao();
        }

        protected void gvConfiguracaoNotificacao_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvConfiguracaoNotificacao.PageIndex = e.NewPageIndex;
            this.CarregarConfiguracaoNotificacao();
        }

        protected void gvConfiguracaoNotificacao_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Editar":
                    this.IrParaPagina
                        (
                            String.Format
                            (
                                "ConfiguracaoNotificacaoAgenda-Manutencao.aspx?ConfiguracaoNotificacaoAgendaID={0}"
                                , e.CommandArgument
                            )
                        );
                    break;

                case "Excluir":
                    Int32 id = Convert.ToInt32(e.CommandArgument);
                    this.Excluir(id);
                    this.CarregarConfiguracaoNotificacao();
                    break;

                default:
                    break;
            }
        }

        protected void gvConfiguracaoNotificacao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ConfiguracaoNotificacaoAgenda configuracaoNotificacao = (ConfiguracaoNotificacaoAgenda)e.Row.DataItem;

                Literal litTempo = (Literal)e.Row.FindControl("litTempo");
                Literal litUnidadeTempo = (Literal)e.Row.FindControl("litUnidadeTempo");

                litTempo.Text = configuracaoNotificacao.Tempo.ToString();
                litUnidadeTempo.Text = configuracaoNotificacao.UnidadeTempoAgenda.Unidade;
            }
        }

        protected void gvConfiguracaoNotificacao_Sorting(object sender, GridViewSortEventArgs e)
        {
            List<ConfiguracaoNotificacaoAgenda> configuracaoNotificacaoAgendas = VsConfiguracaoNotificacaoAgenda;
            String sortExpression = e.SortExpression;
            String sortDirection = GridViewColumnSort.ReturnSortDirection(sortExpression);

            if (sortDirection.Equals("ASC"))
            {
                switch (sortExpression)
                {
                    case "Tempo":
                        configuracaoNotificacaoAgendas = configuracaoNotificacaoAgendas.OrderBy(c => c.Tempo).ToList();
                        break;

                    case "UnidadeTempoAgenda":
                        configuracaoNotificacaoAgendas = configuracaoNotificacaoAgendas.OrderBy(c => c.UnidadeTempoAgenda.Unidade).ToList();
                        break;
                }
            }
            else
            {
                switch (sortExpression)
                {
                    case "Tempo":
                        configuracaoNotificacaoAgendas = configuracaoNotificacaoAgendas.OrderByDescending(c => c.Tempo).ToList();
                        break;

                    case "UnidadeTempoAgenda":
                        configuracaoNotificacaoAgendas = configuracaoNotificacaoAgendas.OrderByDescending(c => c.UnidadeTempoAgenda.Unidade).ToList();
                        break;
                }
            }

            VsConfiguracaoNotificacaoAgenda = configuracaoNotificacaoAgendas;
            this.gvConfiguracaoNotificacao.PageIndex = 0;
            this.gvConfiguracaoNotificacao.DataSource = configuracaoNotificacaoAgendas;
            this.gvConfiguracaoNotificacao.DataBind();
        }

        #endregion

        #region Methods

        private void CarregarUnidadeTempo()
        {
            try
            {
                UnidadeTempoAgenda unidadeTempoAgenda = new UnidadeTempoAgenda();
                this.ddlUnidadeTempo.DataSource = new UnidadeTempoAgendaRepository().Retreave(new UnidadeTempoAgenda());
                this.ddlUnidadeTempo.DataValueField = "unidadeTempoAgendaID";
                this.ddlUnidadeTempo.DataTextField = "Unidade";
                this.ddlUnidadeTempo.DataBind();
                this.ddlUnidadeTempo.Items.Insert(0, new ListItem("Selecione", "0"));
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                this.msgDialog.Show("Erro", "Ocorreu uma falha ao carregar as unidades de tempo.", UserControl.Message.Type.Error);
            }
        }

        private void CarregarConfiguracaoNotificacao()
        {
            try
            {
                ConfiguracaoNotificacaoAgenda configuracaoNotificacaoAgenda = new ConfiguracaoNotificacaoAgenda();

                if (this.txtTempo.Text.Length > 0)
                    configuracaoNotificacaoAgenda.Tempo = Convert.ToInt32(this.txtTempo.Text);

                if (this.ddlUnidadeTempo.SelectedIndex > 0)
                    configuracaoNotificacaoAgenda.UnidadeTempoAgenda.UnidadeTempoAgendaID = Convert.ToInt32(this.ddlUnidadeTempo.SelectedValue);

                List<ConfiguracaoNotificacaoAgenda> configuracaoNotificacaoAgendas
                    = new ConfiguracaoNotificacaoAgendaRepository()
                        .Retreave(configuracaoNotificacaoAgenda);

                VsConfiguracaoNotificacaoAgenda = configuracaoNotificacaoAgendas;
                this.gvConfiguracaoNotificacao.DataSource = configuracaoNotificacaoAgendas;
                this.gvConfiguracaoNotificacao.DataBind();
                this.lblTotalRegistros.Text = configuracaoNotificacaoAgendas.Count.ToString();
                this.infoTotalRegistros.Visible = true;
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                this.msgDialog.Show("Erro", "Ocorreu uma falha ao carregar os registros de configurações de notificações.", UserControl.Message.Type.Error);
            }
        }

        private void LimparFiltros()
        {
            this.txtTempo.Text = String.Empty;
            this.ddlUnidadeTempo.SelectedIndex = 0;
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
                ConfiguracaoNotificacaoAgenda configuracaoNotificacaoAgenda = new ConfiguracaoNotificacaoAgenda();
                configuracaoNotificacaoAgenda.ConfiguracaoNotificacaoAgendaID = id;
                new ConfiguracaoNotificacaoAgendaRepository()
                    .Delete(configuracaoNotificacaoAgenda);
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