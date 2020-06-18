using Edelweiss.AgendaCongelacao.Model;
using Edelweiss.AgendaCongelacao.Model.Entities;
using Edelweiss.AgendaCongelacao.Model.Repositories;
using Edelweiss.Utils;
using System;

namespace Edelweiss.AgendaCongelacao.Site.Admin
{
    public partial class LogSmsAgenda_Detalhe : EdelweissAdminPage
    {
        #region Properties

        public Int32? LogSmsAgendaID
        {
            get
            {
                if (Request.QueryString["LogSmsAgendaID"] != null)
                    return Convert.ToInt32(Request.QueryString["LogSmsAgendaID"]);
                return null;
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

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            IrParaPagina("LogSmsAgenda-Listagem.aspx");
        }

        #endregion

        #region Methods

        public void CarregarLogSmsAgenda()
        {
            try
            {
                LogSmsAgenda logSmsAgenda = new LogSmsAgenda();
                logSmsAgenda = new LogSmsAgendaRepository().Details(new LogSmsAgenda() { LogSmsAgendaID = LogSmsAgendaID });

                this.txtLogSmsAgendaID.Text = logSmsAgenda.LogSmsAgendaID.Value.ToString();

                Boolean smsEnviado = logSmsAgenda.SMSEnviado.Value;
                this.txtSMSEnviado.Text = smsEnviado ? "Sim" : "Não";
                this.txtSMSEnviado.Attributes["style"] = String.Format("color:{0} !important;", smsEnviado ? "green" : "red");
                this.txtSMSEnviado.CssClass = String.Format("{0} {1}", this.txtSMSEnviado.CssClass, (smsEnviado ? "w3-pale-green" : "w3-pale-red"));
                this.txtSMSEnviado.Font.Bold = true;

                this.txtSMSDataProcessamento.Text = logSmsAgenda.SMSDataProcessamento.Value.ToString("yyyy-MM-dd");
                this.txtSMSHoraProcessamento.Text = logSmsAgenda.SMSDataProcessamento.Value.ToString("HH:mm:ss");
                this.txtSMSMessageID.Text = (logSmsAgenda.SMSMessageID != "NULL") ? logSmsAgenda.SMSMessageID : String.Empty;
                this.txtObservacao.Text = logSmsAgenda.Observacao;

                this.CarregarAgenda(logSmsAgenda.Agenda);
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                this.msgDialog.Show("Erro", "Ocorreu uma falha ao carregar o log de SMS de agenda.", UserControl.Message.Type.Error);
            }
        }

        public void CarregarAgenda(Agenda agenda)
        {
            try
            {
                agenda = new AgendaRepository().Details(agenda);

                this.txtDataEvento.Text = agenda.DataHoraEvento.Value.ToString("yyyy-MM-dd");
                this.txtHoraEvento.Text = agenda.DataHoraEvento.Value.ToString("HH:mm:ss");
                this.txtLocal.Text = agenda.Local;
                this.txtNomeMedico.Text = agenda.NomeMedico;
                this.txtNomePaciente.Text = agenda.NomePaciente;
                this.txtConvenio.Text = agenda.Convenio;
                this.txtProcedimento.Text = agenda.Procedimento;

                String telefoneMascara =
                    String.Format(
                                    "({0}) {1}"
                                    , agenda.TelefoneContato.Substring(0, 2)
                                    , agenda.TelefoneContato.Remove(0, 2)
                                );
                this.txtTelefone.Text = telefoneMascara;
                this.txtEstadoAgenda.Text = agenda.EstadoAgenda.Estado;

                this.CarreagarMedicoExecucaoAgenda(agenda.MedicoExecucaoAgenda);
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                this.msgDialog.Show("Erro", "Ocorreu uma falha ao carregar o registro da agenda.", UserControl.Message.Type.Error);
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