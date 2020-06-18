using Edelweiss.AgendaCongelacao.Model;
using Edelweiss.AgendaCongelacao.Model.Entities;
using Edelweiss.AgendaCongelacao.Model.Repositories;
using Edelweiss.Utils;
using System;

namespace Edelweiss.AgendaCongelacao.Site.Admin
{
    public partial class MedicoExecucaoAgenda_Manutencao : EdelweissAdminPage
    {
        #region Properties

        public Int32? MedicoExecucaoAgendaID
        {
            get
            {
                if (Request.QueryString["MedicoExecucaoAgendaID"] != null)
                    return Convert.ToInt32(Request.QueryString["MedicoExecucaoAgendaID"]);
                return null;
            }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (MedicoExecucaoAgendaID != null)
                {
                    Int32 id = MedicoExecucaoAgendaID.Value;
                    this.hifMedicoExecucaoAgendaID.Value = id.ToString();
                    this.CarregarParaEdicao(id);
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.IrParaPagina("MedicoExecucaoAgenda-Listagem.aspx");
        }

        #endregion

        #region Methods

        private void CarregarParaEdicao(Int32 id)
        {
            try
            {
                MedicoExecucaoAgenda medicoExecucaoAgenda = new MedicoExecucaoAgenda();
                medicoExecucaoAgenda =
                    new MedicoExecucaoAgendaRepository().Details(new MedicoExecucaoAgenda() { MedicoExecucaoAgendaID = id });

                this.txtNome.Text = medicoExecucaoAgenda.Nome;
                this.txtEmail.Text = medicoExecucaoAgenda.Email;
                this.txtCelular.Text = medicoExecucaoAgenda.Celular;
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                this.msgDialog.Show("Erro", "Ocorreu uma falha ao carregar o registro para edição.", UserControl.Message.Type.Error);
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