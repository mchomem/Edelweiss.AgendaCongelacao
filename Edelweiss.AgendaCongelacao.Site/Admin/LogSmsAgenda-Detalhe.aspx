<%@ Page
    Language="C#"
    MasterPageFile="~/Admin/Site.Master"
    AutoEventWireup="true"
    CodeBehind="LogSmsAgenda-Detalhe.aspx.cs"
    Inherits="Edelweiss.AgendaCongelacao.Site.Admin.LogSmsAgenda_Detalhe" %>

<%@ Register
    Src="~/Admin/UserControl/Message.ascx"
    TagName="Message"
    TagPrefix="ucEdelweissAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ucEdelweissAdmin:Message ID="msgDialog" runat="server" />

    <div class="w3-panel">
        <div class="w3-card-4">
            <div class="w3-container w3-blue-gray">
                <h5><i class="fas fa-sms fa-lg w3-margin-right"></i>Log SMS</h5>
            </div>

            <%--Log Sms Agenda--%>
            <fieldset class="w3-margin-top w3-margin-left w3-margin-right">
                <legend>Log de envio de SMS da Agenda</legend>
                <div class="w3-row-padding">
                    <div class="w3-col s12 m3 l2">
                        <asp:Label
                            ID="lblLogSmsAgendaID"
                            runat="server"
                            Text="LogSmsAgendaID" />
                        <asp:TextBox
                            ID="txtLogSmsAgendaID"
                            CssClass="w3-input w3-border w3-small"
                            Enabled="false"
                            runat="server"></asp:TextBox>
                    </div>
                    <div class="w3-col s12 m3 l2">
                        <asp:Label
                            ID="lblSMSEnviado"
                            runat="server"
                            Text="SMS enviado" />
                        <asp:TextBox
                            ID="txtSMSEnviado"
                            CssClass="w3-input w3-border w3-small"
                            Enabled="false"
                            runat="server"></asp:TextBox>
                    </div>
                    <div class="w3-col s12 m3 l3">
                        <asp:Label
                            ID="lblSMSDataProcessamento"
                            runat="server"
                            Text="Data processamento" />
                        <asp:TextBox
                            ID="txtSMSDataProcessamento"
                            CssClass="w3-input w3-border w3-small"
                            Enabled="false"
                            runat="server"
                            TextMode="Date"
                            Style="height: 36px;"></asp:TextBox>
                    </div>
                    <div class="w3-col s12 m3 l3">
                        <asp:Label
                            ID="lblSMSHoraProcessamento"
                            runat="server"
                            Text="Hora processamento" />
                        <asp:TextBox
                            ID="txtSMSHoraProcessamento"
                            CssClass="w3-input w3-border w3-small"
                            Enabled="false"
                            runat="server"
                            TextMode="Time"
                            Style="height: 36px;"></asp:TextBox>
                    </div>
                </div>
                <div class="w3-row-padding w3-margin-top">
                    <div class="w3-col s12 m3 l4">
                        <asp:Label
                            ID="lblSMSMessageID"
                            runat="server"
                            Text="SMS Message ID" />
                        <asp:TextBox
                            ID="txtSMSMessageID"
                            CssClass="w3-input w3-border w3-small"
                            Enabled="false"
                            runat="server"></asp:TextBox>
                    </div>
                    <div class="w3-col s12 m3 l2">
                        <label>&nbsp;</label>
                        <a id="aConsultarMessageIDProvedor"
                            class="w3-button w3-blue w3-border w3-border-blue w3-small"
                            href="#"
                            style="display:none;">Consultar MessageID</a>
                    </div>
                </div>
                <div class="w3-row-padding w3-margin-top">
                    <div class="w3-col s12 m12 l12">
                        <asp:Label
                            ID="lblObservacao"
                            runat="server"
                            Text="Observação" />
                        <asp:TextBox
                            ID="txtObservacao"
                            CssClass="w3-input w3-border w3-small"
                            Enabled="false"
                            runat="server"
                            TextMode="MultiLine"
                            Rows="2"
                            Style="resize: none;"></asp:TextBox>
                    </div>
                </div>

                <div class="w3-row-padding">
                    <div class="w3-col s12 m12 l12">
                        <div class="w3-panel w3-pale-yellow w3-border w3-border-brown" style="color: saddlebrown !important;">
                            <h3><i class="fas fa-exclamation-triangle fa-1x w3-margin-right"></i>Atenção!</h3>
                            <p class="w3-small">
                                O provedor de serviço na nuvem utilizado para envio de SMS é o Twilio.
                                Para um melhor detalhamento dos log's das mensagens o console do provedor pode ser
                                acessado pelo link <b><a href="https://www.twilio.com/console" target="_blank">aqui</a></b>.<br />
                                Quando houver uma falha de envio de SMS o campo <b>SMS MessageID</b> não terá valor.
                                Quando o campo <b>SMS MessageID</b> tiver o seu valor hash, significa que a mensagem foi enviada da aplicação para o serviço do provedor.
                                Se mesmo assim a mensagem não for entregue ao celular de destino, deve ser verificado o seu estado nos log's do console do provedor.
                            </p>
                        </div>
                    </div>
                </div>
            </fieldset>

            <%--Agenda--%>
            <fieldset class="w3-margin-top w3-margin-left w3-margin-right">
                <legend>Agenda atual</legend>
                <div class="w3-row-padding">
                    <div class="w3-col s12 m3 l2">
                        <asp:Label
                            ID="lblDataEvento"
                            runat="server"
                            Text="Data evento" />
                        <asp:TextBox
                            ID="txtDataEvento"
                            CssClass="w3-input w3-border w3-small"
                            Enabled="false"
                            runat="server"
                            TextMode="Date"
                            Style="height: 36px;" />
                    </div>
                    <div class="w3-col s12 m3 l2">
                        <asp:Label
                            ID="lblHoraEvento"
                            runat="server"
                            Text="Hora evento" />
                        <asp:TextBox
                            ID="txtHoraEvento"
                            CssClass="w3-input w3-border w3-small"
                            Enabled="false"
                            runat="server"
                            TextMode="Time"
                            Style="height: 36px;" />
                    </div>
                    <div class="w3-col s12 m3 l3">
                        <asp:Label
                            ID="lblLocal"
                            runat="server"
                            Text="Local" />
                        <asp:TextBox
                            ID="txtLocal"
                            CssClass="w3-input w3-border w3-small"
                            Enabled="false"
                            runat="server" />
                    </div>
                    <div class="w3-col s12 m3 l3">
                        <asp:Label
                            ID="lblNomeMedico"
                            runat="server"
                            Text="Médico" />
                        <asp:TextBox
                            ID="txtNomeMedico"
                            CssClass="w3-input w3-border w3-small"
                            Enabled="false"
                            runat="server" />
                    </div>
                </div>
                <div class="w3-row-padding w3-margin-top">
                    <div class="w3-col s12 m3 l3">
                        <asp:Label
                            ID="lblNomePaciente"
                            runat="server"
                            Text="Paciente" />
                        <asp:TextBox
                            ID="txtNomePaciente"
                            CssClass="w3-input w3-border w3-small"
                            Enabled="false"
                            runat="server" />
                    </div>
                    <div class="w3-col s12 m3 l2">
                        <asp:Label
                            ID="lblConvenio"
                            runat="server"
                            Text="Convênio" />
                        <asp:TextBox
                            ID="txtConvenio"
                            CssClass="w3-input w3-border w3-small"
                            Enabled="false"
                            runat="server" />
                    </div>
                    <div class="w3-col s12 m3 l2">
                        <asp:Label
                            ID="lblProcedimento"
                            runat="server"
                            Text="Procedimento" />
                        <asp:TextBox
                            ID="txtProcedimento"
                            CssClass="w3-input w3-border w3-small"
                            Enabled="false"
                            runat="server" />
                    </div>
                    <div class="w3-col s12 m3 l2">
                        <asp:Label
                            ID="lblTelefone"
                            runat="server"
                            Text="Telefone" />
                        <asp:TextBox
                            ID="txtTelefone"
                            CssClass="w3-input w3-border w3-small"
                            placeholder="(00) 000000000"
                            Enabled="false"
                            runat="server" />
                    </div>
                    <div class="w3-col s12 m3 l2">
                        <asp:Label
                            ID="lblEstadoAgenda"
                            runat="server"
                            Text="Estado agenda" />
                        <asp:TextBox
                            ID="txtEstadoAgenda"
                            CssClass="w3-input w3-border w3-small"
                            Enabled="false"
                            runat="server" />
                    </div>
                </div>
            </fieldset>

            <%--Médico executor da agenda--%>
            <fieldset class="w3-margin-top w3-margin-left w3-margin-right">
                <legend>Médico de execução da agenda</legend>
                <div class="w3-row-padding">
                    <div class="w3-col s12 m3 l4">
                        <asp:Label
                            ID="lblNomeMedicoExecutor"
                            runat="server"
                            Text="Nome" />
                        <asp:TextBox
                            ID="txtNomeMedicoExecutor"
                            CssClass="w3-input w3-border w3-small"
                            Enabled="false"
                            runat="server" />
                    </div>
                    <div class="w3-col s12 m3 l2">
                        <asp:Label
                            ID="lblCelular"
                            runat="server"
                            Text="Celular" />
                        <asp:TextBox
                            ID="txtCelular"
                            CssClass="w3-input w3-border w3-small"
                            Enabled="false"
                            runat="server" />
                    </div>
                    <div class="w3-col s12 m3 l4">
                        <asp:Label
                            ID="lblEmail"
                            runat="server"
                            Text="E-mail" />
                        <asp:TextBox
                            ID="txtEmail"
                            CssClass="w3-input w3-border w3-small"
                            Enabled="false"
                            runat="server" />
                    </div>
                </div>
            </fieldset>

            <div class="w3-padding"></div>

            <div class="w3-container w3-light-grey w3-border-top w3-padding">
                <div class="w3-right">
                    <asp:Button
                        ID="btnVoltar"
                        CssClass="w3-button w3-white w3-border w3-border-gray"
                        runat="server"
                        Text="Voltar"
                        OnClick="btnVoltar_Click" />
                </div>
            </div>

        </div>
    </div>

    <script>
        $(document).ready(function () {

            LogSmsAgendaDetalhe = {

                self: this

                , init: function () {
                    this.addControls();
                    this.attachEvent();
                }

                , addControls: function () {
                    self.$txtSMSMessageID = $('input[id*=txtSMSMessageID]');
                    self.$aConsultarMessageIDProvedor = $('#aConsultarMessageIDProvedor');

                    if (self.$txtSMSMessageID.val().length != 0) {
                        self.$aConsultarMessageIDProvedor.attr('style', 'display:block;');
                    }
                }

                , attachEvent: function () {
                    self.$aConsultarMessageIDProvedor.on('click', function () {
                        LogSmsAgendaDetalhe.consultarMessageID();
                    });
                }

                , consultarMessageID: function () {
                    self.$aConsultarMessageIDProvedor.attr({
                        'href': 'https://www.twilio.com/console/sms/logs/' + self.$txtSMSMessageID.val()
                        , 'target': '_blank'
                    });
                }
            }

            LogSmsAgendaDetalhe.init();

        });
    </script>

</asp:Content>
