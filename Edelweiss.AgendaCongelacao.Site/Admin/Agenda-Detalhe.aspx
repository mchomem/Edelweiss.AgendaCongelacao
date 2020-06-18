<%@ Page
    Language="C#"
    MasterPageFile="~/Admin/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Agenda-Detalhe.aspx.cs"
    Inherits="Edelweiss.AgendaCongelacao.Site.Admin.Agenda_Detalhe" %>

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
                <h5><i class="fas fa-edit fa-lg w3-margin-right"></i>Agenda</h5>
            </div>

            <%--Abas--%>
            <div class="w3-row-padding w3-margin-top">
                <div class="w3-border">
                    <div class="w3-bar w3-light-grey">
                        <div class="w3-margin-top w3-margin-left">
                            <%--Botões das abas--%>
                            <a id="aAgenda" name="aTabButtom" class="w3-bar-item w3-button w3-white w3-border-top w3-border-left w3-border-right">Agenda</a>
                            <a id="aMedicoExecucaoAgenda" name="aTabButtom" class="w3-bar-item w3-button w3-light-grey">Médico da agenda</a>
                            <a id="aNotificacaoAgenda" name="aTabButtom" class="w3-bar-item w3-button w3-light-grey">Notificações (lembretes) da agenda</a>
                            <a id="aLogSmsAgenda" name="aTabButtom" class="w3-bar-item w3-button w3-light-grey">Log Sms da agenda</a>
                        </div>
                    </div>

                    <%-- Aba: Agenda--%>
                    <div id="divAgenda" class="tabAgendaDetalhe">
                        <div class="w3-row-padding w3-margin-top">
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
                                    ID="lblTelefoneContato"
                                    runat="server"
                                    Text="Telefone" />
                                <asp:TextBox
                                    ID="txtTelefoneContato"
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
                        <div class="w3-padding"></div>
                    </div>

                    <%--Aba: Médico executor da agenda--%>
                    <div id="divMedicoExecucaoAgenda" class="tabAgendaDetalhe" style="display: none;">
                        <div class="w3-row-padding w3-margin-top">
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
                        <div class="w3-padding"></div>
                    </div>

                    <%--Aba: Notificações (lembretes) da agenda--%>
                    <div id="divNotificacaoAgenda" class="tabAgendaDetalhe" style="display: none;">
                        <div class="w3-row-padding w3-margin-top">
                            <div class="w3-col s12 m12 l12">
                                <%--Contador de registros--%>
                                <div id="infoTotalRegistrosNotificacaoesAgenda" class="w3-margin-top w3-margin-bottom" runat="server" visible="false">
                                    Total de registros
                                <span class="w3-badge w3-dark-gray" style="font-weight: bold;">
                                    <asp:Label ID="lblTotalRegistrosNotificacoesAgenda" runat="server"></asp:Label>
                                </span>
                                </div>

                                <%--Grid--%>
                                <div class="w3-responsive">
                                    <asp:GridView
                                        ID="gvNotificacaoAgenda"
                                        runat="server"
                                        AllowSorting="true"
                                        AllowPaging="true"
                                        AutoGenerateColumns="false"
                                        CssClass="w3-table-all w3-small"
                                        GridLines="None"
                                        OnPageIndexChanging="gvNotificacaoAgenda_PageIndexChanging"
                                        OnRowCommand="gvNotificacaoAgenda_RowCommand"
                                        OnRowDataBound="gvNotificacaoAgenda_RowDataBound"
                                        PageSize="5">

                                        <HeaderStyle CssClass="w3-dark-gray" />
                                        <PagerSettings PageButtonCount="5" />

                                        <EmptyDataTemplate>
                                            <div class="w3-panel w3-khaki w3-center w3-padding-24" style="font: bold;">
                                                Nenhum registro encontrado.
                                            </div>
                                        </EmptyDataTemplate>

                                        <Columns>
                                            <%--Configuração de notificação: Tempo--%>
                                            <asp:TemplateField
                                                HeaderText="Tempo"
                                                ItemStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litConfiguracaoTempo" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%--Configuração de notificação: Unidade de tempo--%>
                                            <asp:TemplateField
                                                HeaderText="Unidade de tempo"
                                                ItemStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litConfiguracaoUnidadeTempo" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%--Utilizado--%>
                                            <asp:TemplateField
                                                HeaderText="Utilizado?"
                                                ItemStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litUtilizado" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>

                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="w3-row-padding">
                            <div class="w3-col s12 m12 l12">
                                <div class="w3-panel w3-pale-yellow w3-border w3-border-brown" style="color: saddlebrown !important;">
                                    <h3><i class="fas fa-exclamation-triangle fa-1x w3-margin-right"></i>Atenção!</h3>
                                    <p class="w3-small">
                                        A quantidade de notificações por agenda no padrão são sempre a mesma quantidade de
                                    <a href="ConfiguracaoNotificacaoAgenda-Listagem.aspx"><b>configurações das notificações de agenda</b></a>.
                                    <br />
                                        Para que uma notificação seja criada (além do estado da agenda estar <b>Confirmada</b>) a data evento da
                                    agenda não deve estar dentro do intervalo entre a data/hora do tempo
                                    de antecipação e a data/hora do evento da agenda, caso esteja, a notificação não será criada.<br />
                                        Caso uma notificação não seja <b>utilizada</b>, pode significar que o serviço de envio pode ter falhado ou não ter sido executado.
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>

                    <%--Aba: Log Sms Agenda--%>
                    <div id="divLogSmsAgenda" class="tabAgendaDetalhe" style="display: none;">
                        <div class="w3-row-padding w3-margin-top">
                            <div class="w3-col s12 m12 l12">
                                <%--Contador de registros--%>
                                <div id="infoTotalRegistrosLogSms" class="w3-margin-top w3-margin-bottom" runat="server" visible="false">
                                    Total de registros
                                <span class="w3-badge w3-dark-gray" style="font-weight: bold;">
                                    <asp:Label ID="lblTotalRegistrosLogSms" runat="server"></asp:Label>
                                </span>
                                </div>

                                <%--Grid--%>
                                <div class="w3-responsive">
                                    <asp:GridView
                                        ID="gvLogSmsAgenda"
                                        runat="server"
                                        AllowSorting="true"
                                        AllowPaging="true"
                                        AutoGenerateColumns="false"
                                        CssClass="w3-table-all w3-small"
                                        GridLines="None"
                                        OnPageIndexChanging="gvLogSmsAgenda_PageIndexChanging"
                                        OnRowCommand="gvLogSmsAgenda_RowCommand"
                                        OnRowDataBound="gvLogSmsAgenda_RowDataBound"
                                        OnSorting="gvLogSmsAgenda_Sorting"
                                        PageSize="10">

                                        <HeaderStyle CssClass="w3-dark-gray" />
                                        <PagerSettings PageButtonCount="10" />

                                        <EmptyDataTemplate>
                                            <div class="w3-panel w3-khaki w3-center w3-padding-24" style="font: bold;">
                                                Nenhum registro encontrado.
                                            </div>
                                        </EmptyDataTemplate>

                                        <Columns>
                                            <%--SmsEnviado--%>
                                            <asp:TemplateField
                                                HeaderText="SMS Enviado"
                                                ItemStyle-Width="100px"
                                                SortExpression="SMSEnviado">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litSMSEnviado" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%--SMSDataProcessamento--%>
                                            <asp:TemplateField
                                                HeaderText="Data processamento"
                                                ItemStyle-Width="100px"
                                                SortExpression="SMSDataProcessamento">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDataProcessamento" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%--SMSMessageID--%>
                                            <asp:TemplateField
                                                HeaderText="SMSMessageID (SID)"
                                                ItemStyle-Width="300px"
                                                SortExpression="SMSMessageID">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litSMSMessageID" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%--Visualizar detalhe--%>
                                            <asp:TemplateField ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <center>
                                                    <asp:LinkButton
                                                        ID="lkbDetalhe"
                                                        runat="server"
                                                        CommandName="Detalhe"
                                                        CommandArgument="<%# ((Edelweiss.AgendaCongelacao.Model.LogSmsAgenda)Container.DataItem).LogSmsAgendaID %>"
                                                        ToolTip="Detalhe">
                                                        <i class="fas fa-eye fa-lg" style="color:darkslategray;"></i>
                                                    </asp:LinkButton>
                                                </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>

                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="w3-padding"></div>
                    </div>
                </div>
            </div>

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

            AgendaDetalhe = {

                self: this

                , init: function () {
                    this.addControls();
                    this.attachEvent();
                }

                , addControls: function () {
                    self.$buttonTabGroup = $('a[name=aTabButtom]');
                    self.$aAgenda = $('#aAgenda');
                    self.$aMedicoExecucaoAgenda = $('#aMedicoExecucaoAgenda');
                    self.$aNotificacaoAgenda = $('#aNotificacaoAgenda');
                    self.$aLogSmsAgenda = $('#aLogSmsAgenda');

                    self.$tabsGroup = $('.tabAgendaDetalhe');
                    self.$divAgenda = $('#divAgenda');
                    self.$divMedicoExecucaoAgenda = $('#divMedicoExecucaoAgenda');
                    self.$divNotificacaoAgenda = $('#divNotificacaoAgenda');
                    self.$divLogSmsAgenda = $('#divLogSmsAgenda');
                }

                , attachEvent: function () {
                    self.$aAgenda.on('click', function () {
                        AgendaDetalhe.manageTabs(self.$aAgenda, self.$divAgenda);
                    });

                    self.$aMedicoExecucaoAgenda.on('click', function () {
                        AgendaDetalhe.manageTabs(self.$aMedicoExecucaoAgenda, self.$divMedicoExecucaoAgenda);
                    });

                    self.$aNotificacaoAgenda.on('click', function () {
                        AgendaDetalhe.manageTabs(self.$aNotificacaoAgenda, self.$divNotificacaoAgenda);
                    });

                    self.$aLogSmsAgenda.on('click', function () {
                        AgendaDetalhe.manageTabs(self.$aLogSmsAgenda, self.$divLogSmsAgenda);
                    });
                }

                , manageTabs: function (button, tab) {

                    var buttons = self.$buttonTabGroup
                    var tabs = self.$tabsGroup;

                    for (var x = 0; x < buttons.length; x++) {
                        $(buttons[x]).removeClass('w3-white w3-border-top w3-border-left w3-border-right').addClass('w3-light-grey');
                    }

                    for (var i = 0; i < tabs.length; i++) {
                        $(tabs[i]).css('display', 'none');
                    }

                    button.removeClass('w3-light-grey').addClass('w3-white w3-border-top w3-border-left w3-border-right');
                    tab.css('display', 'block');
                }
            }

            AgendaDetalhe.init();

        });

    </script>

</asp:Content>
