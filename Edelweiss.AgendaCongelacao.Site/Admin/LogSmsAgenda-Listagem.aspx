<%@ Page
    Language="C#"
    MasterPageFile="~/Admin/Site.Master"
    AutoEventWireup="true"
    CodeBehind="LogSmsAgenda-Listagem.aspx.cs"
    Inherits="Edelweiss.AgendaCongelacao.Site.Admin.LogSmsAgenda_Listagem" %>

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

            <%--Filtros--%>
            <div class="w3-panel">
                <div class="w3-card">
                    <div class="w3-gray">
                        <h5 class="w3-margin-left w3-text-white">Filtros</h5>
                    </div>

                    <div class="w3-row-padding w3-margin-top">
                        <div class="w3-col s12 m3 l2">
                            <asp:Label ID="lblDataInicial" runat="server">Data inicial</asp:Label>
                            <asp:TextBox
                                ID="txtDataInicial"
                                CssClass="w3-input w3-border w3-small"
                                runat="server"
                                TextMode="Date"
                                Style="height: 36px;" />
                        </div>
                        <div class="w3-col s12 m3 l2">
                            <asp:Label ID="lblDataFinal" runat="server">Data final</asp:Label>
                            <asp:TextBox
                                ID="txtDataFinal"
                                CssClass="w3-input w3-border w3-small"
                                runat="server"
                                TextMode="Date"
                                Style="height: 36px;" />
                        </div>
                        <div class="w3-col s12 m3 l4">
                            <asp:Label ID="lblSMSMessageID" runat="server">SMSMessageID (SID)</asp:Label>
                            <asp:TextBox
                                ID="txtSMSMessageID"
                                CssClass="w3-input w3-border w3-small"
                                runat="server" />
                        </div>
                        <div class="w3-col s12 m3 l4">
                            <label>Situação de envio do SMS</label>
                            <div>
                                <input id="rbtEnviado" class="w3-radio" type="radio" runat="server" />
                                <label>Enviado</label>
                                <input id="rbtNaoEnviado" class="w3-radio" type="radio" runat="server" />
                                <label>Não enviado</label>
                                <input id="rbtTodos" class="w3-radio" type="radio" checked runat="server" />
                                <label>Todos</label>
                            </div>
                        </div>
                    </div>

                    <div class="w3-row-padding w3-margin-top">
                        <div class="w3-col s6 m3 l2">
                            <asp:Button
                                ID="btnLimpar"
                                CssClass="w3-button w3-small w3-border w3-border-blue-gray w3-block"
                                runat="server"
                                Text="Limpar"
                                OnClick="btnLimpar_Click" />
                        </div>
                        <div class="w3-col s6 m3 l2">
                            <asp:Button
                                ID="btnPesquisar"
                                CssClass="w3-button w3-small w3-border w3-blue-gray w3-border-blue-gray w3-block"
                                runat="server"
                                Text="Pesquisar"
                                OnClick="btnPesquisar_Click" />
                        </div>
                    </div>

                    <div class="w3-padding"></div>
                </div>
            </div>

            <%--Grid view--%>
            <div class="w3-row-padding w3-margin-top">
                <div class="w3-col s12 m12 l12">
                    <%--Contador de registros--%>
                    <div id="infoTotalRegistros" class="w3-margin-top w3-left" runat="server" visible="false">
                        Total de registros
                        <span class="w3-badge w3-dark-gray" style="font-weight: bold;">
                            <asp:Label ID="lblTotalRegistros" runat="server"></asp:Label>
                        </span>
                    </div>
                    <%--Implementação futura de exportadores de registros--%>
                    <%--<div class="w3-right">
                        <button class="w3-button w3-green w3-border w3-border-green">
                            <i class="fas fa-file-excel"></i>
                        </button>
                        <button class="w3-button w3-light-blue w3-border w3-border-light-blue">
                            <i class="fas fa-file-csv"></i>
                        </button>
                        <button class="w3-button w3-light-gray w3-border w3-border-blue-gray">
                            <i class="far fa-file-alt"></i>
                        </button>
                    </div>--%>
                </div>
            </div>

            <div class="w3-row-padding w3-margin-top">
                <div class="w3-col s12 m12 l12">
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
                                            CommandArgument="<%# ((Edelweiss.AgendaCongelacao.Model.Entities.LogSmsAgenda)Container.DataItem).LogSmsAgendaID %>"
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

</asp:Content>
