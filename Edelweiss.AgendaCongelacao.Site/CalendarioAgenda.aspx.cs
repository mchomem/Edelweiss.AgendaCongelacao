using Edelweiss.AgendaCongelacao.Model;
using Edelweiss.AgendaCongelacao.Model.Entities;
using Edelweiss.AgendaCongelacao.Model.Repositories;
using Edelweiss.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;

namespace Edelweiss.AgendaCongelacao.Site
{
    public partial class CalendarioAgenda : System.Web.UI.Page
    {
        #region Properties

        private List<Agenda> AgendasDoMes
        {
            get
            {
                if (ViewState["AgendasDoMes"] == null)
                    ViewState["AgendasDoMes"] = new List<Agenda>();
                return (List<Agenda>)ViewState["AgendasDoMes"];
            }
            set
            {
                ViewState["AgendasDoMes"] = value;
            }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.CarregarAgendamentosDoMes(DateTime.Now);
                this.CarregarAgendaDoDia(DateTime.Now);
            }
        }

        protected void calAgenda_DayRender(object sender, DayRenderEventArgs e)
        {
            // Exibe somente os dias do mês selecionado.
            if (e.Day.IsOtherMonth)
            {
                e.Cell.Text = "&nbsp;";
            }

            // Para cada agenda, busca o dia do mês correspondente no calendário.
            foreach (Agenda agenda in AgendasDoMes)
            {
                if (agenda.DataHoraEvento.Value.Day == e.Day.Date.Day
                    && agenda.DataHoraEvento.Value.Month == e.Day.Date.Month
                    && agenda.DataHoraEvento.Value.Year == e.Day.Date.Year)
                {
                    e.Cell.Attributes["style"] = "background-color: #99ccff; font-weight:bold;";
                }
            }

            // Destacar o dia atual no calendário.
            if (e.Day.IsToday)
            {
                e.Cell.BorderColor = Color.Red;
                e.Cell.BorderWidth = Unit.Pixel(2);
            }

            // Se a data é passada ou é final de semana, não permite selecionar.
            if (e.Day.Date < DateTime.Now.Date || e.Day.IsWeekend)
            {
                e.Day.IsSelectable = false;
                e.Cell.Attributes["style"] = "background-color: #f2f2f2;";
            }
        }

        protected void calAgenda_SelectionChanged(object sender, EventArgs e)
        {
            DateTime diaSelecionado = this.calAgenda.SelectedDate;
            this.CarregarAgendaDoDia(diaSelecionado);
            Session["DiaSelecionadoCalendario"] = diaSelecionado;

            this.calAgenda.SelectedDayStyle.BackColor = Color.FromArgb(21, 69, 114);
            this.calAgenda.SelectedDayStyle.ForeColor = Color.White;
            this.calAgenda.SelectedDayStyle.Font.Bold = true;
        }

        protected void calAgenda_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            DateTime dataMesSelecionado = new DateTime(e.NewDate.Year, e.NewDate.Month, e.NewDate.Day);
            this.CarregarAgendamentosDoMes(dataMesSelecionado);
        }

        protected void gvAgendaDoDia_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvAgendaDoDia.PageIndex = e.NewPageIndex;

            DateTime diaSelecionado = this.calAgenda.SelectedDate;
            this.CarregarAgendaDoDia(diaSelecionado);
        }

        protected void gvAgendaDoDia_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Editar":
                    this.IrParaPagina
                        (
                            String.Format("ManutencaoAgenda.aspx?AgendaID={0}", e.CommandArgument)
                        );
                    break;

                case "Excluir":
                    Int32 id = Convert.ToInt32(e.CommandArgument);
                    this.Excluir(id);
                    this.CarregarAgendamentosDoMes(DateTime.Now);
                    this.CarregarAgendaDoDia(DateTime.Now);
                    break;

                default:
                    break;
            }
        }

        protected void gvAgendaDoDia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Agenda agenda = (Agenda)e.Row.DataItem;

                Literal litDataHora = (Literal)e.Row.FindControl("litDataHora");
                Literal litLocal = (Literal)e.Row.FindControl("litLocal");
                Literal litNomeMedico = (Literal)e.Row.FindControl("litNomeMedico");
                Literal litEstadoAgenda = (Literal)e.Row.FindControl("litEstadoAgenda");

                litDataHora.Text = agenda.DataHoraEvento.Value.ToString("dd/MM/yyyy HH:mm");
                litLocal.Text = agenda.Local;
                litNomeMedico.Text = agenda.NomeMedico;
                litEstadoAgenda.Text = agenda.EstadoAgenda.Estado;
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            this.IrParaPagina("ManutencaoAgenda.aspx");
        }

        protected void btnAgendar_Click(object sender, EventArgs e)
        {
            this.IrParaPagina("ManutencaoAgenda.aspx");
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
                            , "Ocorreu uma falha ao navegar para a página solicitada."
                            , UserControl.Message.Type.Error
                        );
            }
        }

        private void CarregarAgendamentosDoMes(DateTime dataAtual)
        {
            try
            {
                Agenda agenda = new Agenda();
                DateTime primeiroDiaMes = new DateTime(dataAtual.Year, dataAtual.Month, 1);
                DateTime ultimoDiaMes = primeiroDiaMes.AddMonths(1).AddDays(-1);
                ultimoDiaMes = ultimoDiaMes.AddHours(23).AddMinutes(59).AddSeconds(59); // O último dia deve ser inteiro!
                AgendasDoMes = new AgendaRepository().Retreave(new Agenda(), primeiroDiaMes, ultimoDiaMes);
                AgendasDoMes = AgendasDoMes
                    .Where(a => a.Ativo.Value)
                        .ToList();
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                this.msgDialog
                    .Show
                        (
                            "Erro"
                            , "Ocorreu uma falha ao carregar as agendas do mês."
                            , UserControl.Message.Type.Error
                        );
            }
        }

        private void CarregarAgendaDoDia(DateTime agendaDoDia)
        {
            try
            {
                Agenda agenda = new Agenda();
                DateTime horaInicial = new DateTime(agendaDoDia.Year, agendaDoDia.Month, agendaDoDia.Day, 0, 0, 0);
                DateTime horaFinal = new DateTime(agendaDoDia.Year, agendaDoDia.Month, agendaDoDia.Day, 23, 59, 59);

                List<Agenda> agendas = new AgendaRepository().Retreave(new Agenda(), horaInicial, horaFinal);
                agendas = agendas
                    .Where(a => a.Ativo.Value)
                        .OrderBy(a => a.DataHoraEvento)
                            .ToList();

                this.btnNovo.Visible = (agendas.Count > 0 ? true : false);

                this.gvAgendaDoDia.DataSource = agendas;
                this.gvAgendaDoDia.DataBind();
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                this.msgDialog
                    .Show
                        (
                            "Erro"
                            , "Ocorreu uma falha ao carregar a agenda do dia."
                            , UserControl.Message.Type.Error
                        );
            }
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
                            , "Ocorreu uma falha ao excluir o registro"
                            , UserControl.Message.Type.Error
                        );
            }
        }

        #endregion
    }
}
