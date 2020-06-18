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
    public partial class LogSmsAgenda_Listagem : EdelweissAdminPage
    {
        #region Properties

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
                this.CarregarLogSmsAgenda();
            }
        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            this.LimparFiltros();
            this.CarregarLogSmsAgenda();
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            this.CarregarLogSmsAgenda();
        }

        protected void gvLogSmsAgenda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvLogSmsAgenda.PageIndex = e.NewPageIndex;
            this.CarregarLogSmsAgenda();
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

        private void CarregarLogSmsAgenda()
        {
            try
            {
                LogSmsAgenda logSmsAgenda = new LogSmsAgenda();
                logSmsAgenda.SMSMessageID = this.txtSMSMessageID.Text.Length > 0 ? this.txtSMSMessageID.Text : null;

                if (this.rbtTodos.Checked)
                {
                    logSmsAgenda.SMSEnviado = null;
                }
                else if (this.rbtEnviado.Checked)
                {
                    logSmsAgenda.SMSEnviado = true;
                }
                else
                {
                    logSmsAgenda.SMSEnviado = false;
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

                List<LogSmsAgenda> logs = new LogSmsAgendaRepository().Retreave(logSmsAgenda, dataInicial, dataFinal);

                VsLogSmsAgendas = logs;
                this.gvLogSmsAgenda.DataSource = logs;
                this.gvLogSmsAgenda.DataBind();
                this.lblTotalRegistros.Text = logs.Count.ToString();
                this.infoTotalRegistros.Visible = true;
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                this.msgDialog.Show("Erro", "Ocorreu uma falha ao carregar os registros de log's de SMS das agendas.", UserControl.Message.Type.Error);
            }
        }

        private void LimparFiltros()
        {
            this.txtDataInicial.Text = String.Empty;
            this.txtDataFinal.Text = String.Empty;
            this.txtSMSMessageID.Text = String.Empty;
            this.rbtEnviado.Checked = false;
            this.rbtNaoEnviado.Checked = false;
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

        #endregion
    }
}
