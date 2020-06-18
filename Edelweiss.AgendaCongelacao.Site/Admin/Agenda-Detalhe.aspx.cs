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
    public partial class Agenda_Detalhe : EdelweissAdminPage
    {
        #region Properties

        public Int32? AgendaID
        {
            get
            {
                if (Request.QueryString["AgendaID"] != null)
                    return Convert.ToInt32(Request.QueryString["AgendaID"]);
                return null;
            }
        }

        public List<LogSmsAgenda> VsLogSmsAgendas
        {
            get
            {
                if (ViewState["LogSmsAgendas"] != null)
                    return (List<LogSmsAgenda>)ViewState["LogSmsAgendas"];
                return null;
            }
            set
            {
                ViewState["LogSmsAgendas"] = value;
            }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (AgendaID != null)
                {
                    Int32 id = AgendaID.Value;
                    this.CarregarAgenda(id);
                }
            }
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            this.IrParaPagina("Agenda-Listagem.aspx");
        }

        protected void gvNotificacaoAgenda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvNotificacaoAgenda.PageIndex = e.NewPageIndex;
            this.CarregarNotificacoesAvenda(new Agenda() { AgendaID = AgendaID });
        }

        protected void gvNotificacaoAgenda_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ;
        }

        protected void gvNotificacaoAgenda_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                NotificacaoAgenda notificacao = (NotificacaoAgenda)e.Row.DataItem;

                Literal litConfiguracaoTempo = (Literal)e.Row.FindControl("litConfiguracaoTempo");
                Literal litConfiguracaoUnidadeTempo = (Literal)e.Row.FindControl("litConfiguracaoUnidadeTempo");
                Literal litUtilizado = (Literal)e.Row.FindControl("litUtilizado");

                litConfiguracaoTempo.Text = notificacao.ConfiguracaoNotificacaoAgenda.Tempo.ToString();
                litConfiguracaoUnidadeTempo.Text = notificacao.ConfiguracaoNotificacaoAgenda.UnidadeTempoAgenda.Unidade;
                Boolean utilizao = notificacao.Utilizado.Value;
                litUtilizado.Text = "<span style='font-weight:bold;color:" + (utilizao ? "green" : "red") + ";'>" + (utilizao ? "Sim" : "Não") + "</span>";
            }
        }

        protected void gvLogSmsAgenda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvLogSmsAgenda.PageIndex = e.NewPageIndex;
            this.CarregarLogsSms(new Agenda() { AgendaID = AgendaID });
        }

        protected void gvLogSmsAgenda_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Detalhe":
                    this.IrParaPagina
                        (
                            String.Format("LogSmsAgenda-Detalhe.aspx?LogSmsAgendaID={0}", e.CommandArgument)
                        );
                    break;

                default:
                    break;
            }
        }

        protected void gvLogSmsAgenda_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LogSmsAgenda logSmsAgenda = (LogSmsAgenda)e.Row.DataItem;

                Literal litSMSEnviado = (Literal)e.Row.FindControl("litSMSEnviado");
                Literal litDataProcessamento = (Literal)e.Row.FindControl("litDataProcessamento");
                Literal litSMSMessageID = (Literal)e.Row.FindControl("litSMSMessageID");

                Boolean smsEnviado = logSmsAgenda.SMSEnviado.Value;
                litSMSEnviado.Text = "<span style='font-weight:bold;color:" + (smsEnviado ? "green" : "red") + ";'>" + (smsEnviado ? "Sim" : "Não") + "</span>";
                litDataProcessamento.Text = logSmsAgenda.SMSDataProcessamento.Value.ToString("dd/MM/yyyy HH:mm:ss");
                litSMSMessageID.Text =
                    logSmsAgenda.SMSMessageID.Length > 0
                    ?
                    String.Format("<a class='w3-text-blue' href='https://www.twilio.com/console/sms/logs/{0}' target='_blank'>{0}</a>", logSmsAgenda.SMSMessageID)
                    :
                    String.Empty;
            }
        }

        protected void gvLogSmsAgenda_Sorting(object sender, GridViewSortEventArgs e)
        {
            List<LogSmsAgenda> logSmsAgendas = VsLogSmsAgendas;
            String sortExpression = e.SortExpression;
            String sortDirection = GridViewColumnSort.ReturnSortDirection(sortExpression);

            if (sortDirection.Equals("ASC"))
            {
                switch (sortExpression)
                {
                    case "SMSEnviado":
                        logSmsAgendas = logSmsAgendas.OrderBy(l => l.SMSEnviado).ToList();
                        break;

                    case "SMSDataProcessamento":
                        logSmsAgendas = logSmsAgendas.OrderBy(l => l.SMSDataProcessamento).ToList();
                        break;

                    case "SMSMessageID":
                        logSmsAgendas = logSmsAgendas.OrderBy(l => l.SMSMessageID).ToList();
                        break;
                }
            }
            else
            {
                switch (sortExpression)
                {
                    case "SMSEnviado":
                        logSmsAgendas = logSmsAgendas.OrderByDescending(l => l.SMSEnviado).ToList();
                        break;

                    case "SMSDataProcessamento":
                        logSmsAgendas = logSmsAgendas.OrderByDescending(l => l.SMSDataProcessamento).ToList();
                        break;

                    case "SMSMessageID":
                        logSmsAgendas = logSmsAgendas.OrderByDescending(l => l.SMSMessageID).ToList();
                        break;
                }
            }

            VsLogSmsAgendas = logSmsAgendas;
            this.gvLogSmsAgenda.PageIndex = 0;
            this.gvLogSmsAgenda.DataSource = logSmsAgendas;
            this.gvLogSmsAgenda.DataBind();
        }

        #endregion

        #region Methods

        private void CarregarAgenda(Int32 id)
        {
            try
            {
                Agenda agenda = new Agenda();
                agenda = new AgendaRepository().Details(new Agenda() { AgendaID = id });

                this.txtDataEvento.Text = agenda.DataHoraEvento.Value.ToString("yyyy-MM-dd");
                this.txtHoraEvento.Text = agenda.DataHoraEvento.Value.ToString("HH:mm");
                this.txtLocal.Text = agenda.Local;
                this.txtNomePaciente.Text = agenda.NomePaciente;
                this.txtConvenio.Text = agenda.Convenio;
                this.txtProcedimento.Text = agenda.Procedimento;
                this.txtNomeMedico.Text = agenda.NomeMedico;
                this.txtEstadoAgenda.Text = agenda.EstadoAgenda.Estado;

                String telefoneMascara =
                    String.Format
                    (
                        "({0}) {1}"
                        , agenda.TelefoneContato.Substring(0, 2)
                        , agenda.TelefoneContato.Remove(0, 2)
                    );
                this.txtTelefoneContato.Text = telefoneMascara;

                this.CarreagarMedicoExecucaoAgenda(agenda.MedicoExecucaoAgenda);
                this.CarregarNotificacoesAvenda(agenda);
                this.CarregarLogsSms(agenda);
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                this.msgDialog.Show
                    (
                        "Erro"
                        , "Não foi possível carregar os registros para edição."
                        , UserControl.Message.Type.Error
                    );
            }
        }

        public void CarreagarMedicoExecucaoAgenda(MedicoExecucaoAgenda medicoExecucaoAgenda)
        {
            try
            {
                medicoExecucaoAgenda = new MedicoExecucaoAgendaRepository().Details(medicoExecucaoAgenda);
                this.txtNomeMedicoExecutor.Text = medicoExecucaoAgenda.Nome;

                String celularMascara =
                    String.Format
                    (
                        "({0}) {1}"
                        , medicoExecucaoAgenda.Celular.Substring(0, 2)
                        , medicoExecucaoAgenda.Celular.Remove(0, 2)
                    );
                this.txtCelular.Text = celularMascara;
                this.txtEmail.Text = medicoExecucaoAgenda.Email;
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                this.msgDialog.Show("Erro", "Ocorreu uma falha ao carregar o registro do médico executor da agenda.", UserControl.Message.Type.Error);
            }
        }

        private void CarregarNotificacoesAvenda(Agenda agenda)
        {
            try
            {
                List<NotificacaoAgenda> notificacoes
                    = new NotificacaoAgendaRepository()
                        .Retreave(new NotificacaoAgenda() { Agenda = agenda });

                this.gvNotificacaoAgenda.DataSource = notificacoes;
                this.gvNotificacaoAgenda.DataBind();

                this.lblTotalRegistrosNotificacoesAgenda.Text = notificacoes.Count.ToString();
                this.infoTotalRegistrosNotificacaoesAgenda.Visible = true;
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                this.msgDialog.Show("Erro", "Ocorreu uma falha ao carregar os registros de notificações (lembretes) SMS da agenda.", UserControl.Message.Type.Error);
            }
        }

        private void CarregarLogsSms(Agenda agenda)
        {
            try
            {
                List<LogSmsAgenda> logs =
                    new LogSmsAgendaRepository()
                    .Retreave(new LogSmsAgenda() { Agenda = agenda }, null, null);

                VsLogSmsAgendas = logs;
                this.gvLogSmsAgenda.DataSource = logs;
                this.gvLogSmsAgenda.DataBind();

                this.lblTotalRegistrosLogSms.Text = logs.Count.ToString();
                this.infoTotalRegistrosLogSms.Visible = true;
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                this.msgDialog.Show("Erro", "Ocorreu uma falha ao carregar os registros de log SMS da agenda.", UserControl.Message.Type.Error);
            }
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