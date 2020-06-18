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
    public partial class Agenda_Listagem : EdelweissAdminPage
    {
        #region Properteies

        public List<Agenda> VsAgendas
        {
            get
            {
                if (ViewState["Agendas"] != null)
                    return (List<Agenda>)ViewState["Agendas"];
                return null;
            }
            set
            {
                ViewState["Agendas"] = value;
            }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.CarregarEstadoAgenda();
                this.CarregarAgendamentos();
            }
        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            this.LimparFiltros();
            this.CarregarAgendamentos();
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            this.CarregarAgendamentos();
        }

        protected void gvAgendas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvAgendas.PageIndex = e.NewPageIndex;
            this.CarregarAgendamentos();
        }

        protected void gvAgendas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Detalhe":
                    this.IrParaPagina
                        (
                            String.Format("Agenda-Detalhe.aspx?AgendaID={0}", e.CommandArgument)
                        );
                    break;

                default:
                    break;
            }
        }

        protected void gvAgendas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Agenda agenda = (Agenda)e.Row.DataItem;

                Literal litIcon = (Literal)e.Row.FindControl("litIcon");
                Literal litDataHora = (Literal)e.Row.FindControl("litDataHora");
                Literal litLocal = (Literal)e.Row.FindControl("litLocal");
                Literal litNomeMedico = (Literal)e.Row.FindControl("litNomeMedico");
                Literal litConvenio = (Literal)e.Row.FindControl("litConvenio");
                Literal litEstadoAgenda = (Literal)e.Row.FindControl("litEstadoAgenda");

                litIcon.Text =
                    String.Format
                        (
                            "<i class='fas fa-circle fa-xs' style='color: {0}'></i>"
                            , agenda.EstadoAgenda.CorHexaCss
                        );

                litDataHora.Text = agenda.DataHoraEvento.Value.ToString("dd/MM/yyyy HH:mm");
                litLocal.Text = agenda.Local;
                litNomeMedico.Text = agenda.NomeMedico;
                litConvenio.Text = agenda.Convenio;
                litEstadoAgenda.Text = agenda.EstadoAgenda.Estado;
            }
        }

        protected void gvAgendas_Sorting(object sender, GridViewSortEventArgs e)
        {
            List<Agenda> agendas = VsAgendas;
            String sortExpression = e.SortExpression;
            String sortDirection = GridViewColumnSort.ReturnSortDirection(sortExpression);

            if (sortDirection.Equals("ASC"))
            {
                switch (sortExpression)
                {
                    case "datahora":
                        agendas = agendas.OrderBy(a => a.DataHoraEvento).ToList();
                        break;

                    case "local":
                        agendas = agendas.OrderBy(a => a.Local).ToList();
                        break;

                    case "medico":
                        agendas = agendas.OrderBy(a => a.NomeMedico).ToList();
                        break;

                    case "convenio":
                        agendas = agendas.OrderBy(a => a.Convenio).ToList();
                        break;

                    case "estadoagenda":
                        agendas = agendas.OrderBy(a => a.EstadoAgenda.Estado).ToList();
                        break;
                }
            }
            else
            {
                switch (sortExpression)
                {
                    case "datahora":
                        agendas = agendas.OrderByDescending(a => a.DataHoraEvento).ToList();
                        break;

                    case "local":
                        agendas = agendas.OrderByDescending(a => a.Local).ToList();
                        break;

                    case "medico":
                        agendas = agendas.OrderByDescending(a => a.NomeMedico).ToList();
                        break;

                    case "convenio":
                        agendas = agendas.OrderByDescending(a => a.Convenio).ToList();
                        break;

                    case "estadoagenda":
                        agendas = agendas.OrderByDescending(a => a.EstadoAgenda.Estado).ToList();
                        break;
                }
            }

            VsAgendas = agendas;
            this.gvAgendas.PageIndex = 0;
            this.gvAgendas.DataSource = agendas;
            this.gvAgendas.DataBind();
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
                this.msgDialog
                    .Show
                        (
                            "Erro"
                            , "Não foi possível navegar para a página solicitada."
                            , UserControl.Message.Type.Error
                        );
            }
        }

        private void CarregarEstadoAgenda()
        {
            try
            {
                EstadoAgenda statusAgenda = new EstadoAgenda();
                this.ddlEstadoAgenda.DataSource = new EstadoAgendaRepository().Retreave(new EstadoAgenda());
                this.ddlEstadoAgenda.DataValueField = "EstadoAgendaID";
                this.ddlEstadoAgenda.DataTextField = "Estado";
                this.ddlEstadoAgenda.DataBind();
                this.ddlEstadoAgenda.Items.Insert(0, new ListItem("Selecione", "0"));
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                this.msgDialog
                    .Show
                        (
                            "Erro"
                            , "Não foi possível carregar os Estados de agenda."
                            , UserControl.Message.Type.Error
                        );
            }
        }

        private void CarregarAgendamentos()
        {
            try
            {
                Agenda agenda = new Agenda();
                agenda.Local = this.txtLocal.Text.Length > 0 ? this.txtLocal.Text : null;
                agenda.NomeMedico = this.txtMedico.Text.Length > 0 ? this.txtMedico.Text : null;
                agenda.Convenio = this.txtConvenio.Text.Length > 0 ? this.txtConvenio.Text : null;

                if (this.ddlEstadoAgenda.SelectedIndex != 0)
                {
                    EstadoAgenda estadoAgenda =
                        new EstadoAgendaRepository()
                            .Details(new EstadoAgenda()
                            {
                                EstadoAgendaID = Convert.ToInt32(this.ddlEstadoAgenda.SelectedValue)
                            });

                    agenda.EstadoAgenda = estadoAgenda;
                }

                DateTime? dataInicial = null;
                DateTime? dataFinal = null;

                if (!String.IsNullOrEmpty(this.txtDataInicial.Text))
                    dataInicial = Convert.ToDateTime(this.txtDataInicial.Text);

                if (!String.IsNullOrEmpty(this.txtDataFinal.Text))
                {
                    dataFinal = Convert.ToDateTime(this.txtDataFinal.Text);
                    TimeSpan hora = new TimeSpan(23, 59, 59);
                    dataFinal = dataFinal.Value.Add(hora);
                }

                List<Agenda> agendas = new AgendaRepository().Retreave(agenda, dataInicial, dataFinal);
                agendas = agendas
                    .Where(a => a.Ativo.Value)
                        .OrderByDescending(a => a.AgendaID)
                            .ToList();

                VsAgendas = agendas;
                this.gvAgendas.DataSource = agendas;
                this.gvAgendas.DataBind();

                this.lblTotalRegistros.Text = agendas.Count.ToString();
                this.infoTotalRegistros.Visible = true;

            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                this.msgDialog
                    .Show
                        (
                            "Erro"
                            , "Não foi possível carregar registros de agenda."
                            , UserControl.Message.Type.Error
                        );
            }
        }

        private void LimparFiltros()
        {
            this.txtDataInicial.Text = String.Empty;
            this.txtDataFinal.Text = String.Empty;
            this.txtLocal.Text = String.Empty;
            this.txtMedico.Text = String.Empty;
            this.txtConvenio.Text = String.Empty;
            this.ddlEstadoAgenda.SelectedIndex = 0;
        }

        private void Excluir(Int32 id)
        {
            try
            {
                Agenda agenda = new Agenda();
                agenda.AgendaID = id;
                new AgendaRepository().Delete(agenda);

                List<NotificacaoAgenda> notificacoesExistentes = new NotificacaoAgendaRepository()
                            .Retreave(new NotificacaoAgenda() { Agenda = agenda });

                // Se houver lembrete da agenda, deve ser excluído.
                if (notificacoesExistentes.Count > 0)
                {
                    NotificacaoAgenda notificacaoAgenda = new NotificacaoAgenda();
                    notificacaoAgenda.Agenda = agenda;
                    new NotificacaoAgendaRepository().Delete(notificacaoAgenda);
                }
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                this.msgDialog
                    .Show
                        (
                            "Erro"
                            , "Ocorreu um problema ao excluir o registro."
                            , UserControl.Message.Type.Error
                        );
            }
        }

        #endregion
    }
}
