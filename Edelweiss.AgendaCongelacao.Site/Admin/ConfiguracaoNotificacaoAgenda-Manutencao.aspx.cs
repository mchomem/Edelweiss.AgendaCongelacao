using Edelweiss.AgendaCongelacao.Model.Entities;
using Edelweiss.AgendaCongelacao.Model.Repositories;
using Edelweiss.Utils;
using System;
using System.Web.UI.WebControls;

namespace Edelweiss.AgendaCongelacao.Site.Admin
{
    public partial class ConfiguracaoNotificacaoAgenda_Manutencao : EdelweissAdminPage
    {
        #region Properties

        public Int32? ConfiguracaoNotificacaoAgendaID
        {
            get
            {
                if (Request.QueryString["ConfiguracaoNotificacaoAgendaID"] != null)
                    return Convert.ToInt32(Request.QueryString["ConfiguracaoNotificacaoAgendaID"]);
                return null;
            }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.CarregarUnidadeTempo();

                if (ConfiguracaoNotificacaoAgendaID != null)
                {
                    Int32 id = ConfiguracaoNotificacaoAgendaID.Value;
                    this.hifConfiguracaoNotificacaoAgendaID.Value = id.ToString();
                    this.CarregarParaEdicao(id);
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.IrParaPagina("ConfiguracaoNotificacaoAgenda-Listagem.aspx");
        }

        #endregion

        #region Methods

        private void CarregarParaEdicao(Int32 id)
        {
            try
            {
                ConfiguracaoNotificacaoAgenda configuracao = new ConfiguracaoNotificacaoAgenda();
                configuracao =
                    new ConfiguracaoNotificacaoAgendaRepository()
                        .Details(new ConfiguracaoNotificacaoAgenda() { ConfiguracaoNotificacaoAgendaID = id });

                this.txtTempo.Text = configuracao.Tempo.ToString();
                this.ddlUnidadeTempo.SelectedValue = configuracao.UnidadeTempoAgenda.UnidadeTempoAgendaID.Value.ToString();
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                this.msgDialog.Show("Erro", "Ocorreu uma falha ao carregar o registro para edição.", UserControl.Message.Type.Error);
            }
        }

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