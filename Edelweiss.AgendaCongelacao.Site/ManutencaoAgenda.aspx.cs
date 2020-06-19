using Edelweiss.AgendaCongelacao.Model;
using Edelweiss.AgendaCongelacao.Model.Entities;
using Edelweiss.AgendaCongelacao.Model.Repositories;
using Edelweiss.Utils;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace Edelweiss.AgendaCongelacao.Site
{
    public partial class ManutencaoAgenda : System.Web.UI.Page
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

        public DateTime? DiaSelecionadoCalendario
        {
            get
            {
                if (Session["DiaSelecionadoCalendario"] != null)
                    return Convert.ToDateTime(Session["DiaSelecionadoCalendario"]);
                return null;
            }
            set
            {
                Session["DiaSelecionadoCalendario"] = value;
            }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (DiaSelecionadoCalendario != null)
                {
                    this.txtDataAgenda.Text = DiaSelecionadoCalendario.Value.ToString("yyyy-MM-dd");
                }

                this.CarregarMedicoExecucaoAgenda();
                this.CarregarStatusAgenda();

                if (AgendaID != null)
                {
                    Int32 id = AgendaID.Value;
                    this.hifAgendaID.Value = id.ToString();
                    this.CarregarParaEdicao(id);
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.LimparVariaveisSessao();
            this.IrParaPagina("ListagemAgenda.aspx");
        }

        #endregion

        #region Methods

        private void CarregarMedicoExecucaoAgenda()
        {
            try
            {
                MedicoExecucaoAgenda medicoExecucaoAgenda = new MedicoExecucaoAgenda();
                this.ddlMedicoExecucaoAgenda.DataSource =
                    new MedicoExecucaoAgendaRepository().Retreave(new MedicoExecucaoAgenda());

                this.ddlMedicoExecucaoAgenda.DataValueField = "MedicoExecucaoAgendaID";
                this.ddlMedicoExecucaoAgenda.DataTextField = "Nome";
                this.ddlMedicoExecucaoAgenda.DataBind();
                this.ddlMedicoExecucaoAgenda.Items.Insert(0, new ListItem("Selecione", "0"));
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                this.msgDialog.Show
                    (
                        "Erro"
                        , "Ocorreu uma falha ao carregar os registros de médicos de execução de agenda."
                        , UserControl.Message.Type.Error
                    );
            }
        }

        private void CarregarStatusAgenda()
        {
            try
            {
                EstadoAgenda statusAgenda = new EstadoAgenda();
                this.ddlEstadoAgenda.DataSource =
                    new EstadoAgendaRepository().Retreave(new EstadoAgenda())
                        .OrderBy(ea => ea.Ordem);

                this.ddlEstadoAgenda.DataValueField = "EstadoAgendaID";
                this.ddlEstadoAgenda.DataTextField = "Estado";
                this.ddlEstadoAgenda.DataBind();
                this.ddlEstadoAgenda.Items.Insert(0, new ListItem("Selecione", "0"));

                // Se for uma NOVA agenda de congelação, mantém o estado "Agendado".
                if (AgendaID == null)
                {
                    this.ddlEstadoAgenda.Enabled = false;
                    this.ddlEstadoAgenda.SelectedIndex = 1;
                }
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                this.msgDialog.Show
                    (
                        "Erro"
                        , "Ocorreu uma falha ao carregar os Estados de agenda."
                        , UserControl.Message.Type.Error
                    );
            }
        }

        private void CarregarParaEdicao(Int32 id)
        {
            try
            {
                Agenda agenda = new Agenda();
                agenda = new AgendaRepository().Details(new Agenda() { AgendaID = id });

                this.txtDataAgenda.Text = agenda.DataHoraEvento.Value.ToString("yyyy-MM-dd");
                this.txtHoraAgenda.Text = agenda.DataHoraEvento.Value.ToString("HH:mm");
                this.txtLocal.Text = agenda.Local;
                this.txtNomePaciente.Text = agenda.NomePaciente;
                this.txtConvenio.Text = agenda.Convenio;
                this.txtProcedimento.Text = agenda.Procedimento;
                this.txtNomeMedico.Text = agenda.NomeMedico;
                this.ddlMedicoExecucaoAgenda.SelectedValue = agenda.MedicoExecucaoAgenda.MedicoExecucaoAgendaID.Value.ToString();
                this.ddlEstadoAgenda.SelectedValue = agenda.EstadoAgenda.EstadoAgendaID.Value.ToString();
                this.txtTelefoneContato.Text = agenda.TelefoneContato;
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
                this.msgDialog.Show
                    (
                        "Erro"
                        , "Não foi possível navegar para a página solicitada."
                        , UserControl.Message.Type.Error
                    );
            }
        }

        private void LimparVariaveisSessao()
        {
            DiaSelecionadoCalendario = null;
        }

        #endregion
    }
}
