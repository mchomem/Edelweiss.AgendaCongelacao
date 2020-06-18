<%@ Page
    Language="C#"
    MasterPageFile="~/Admin/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Agenda-Listagem.aspx.cs"
    Inherits="Edelweiss.AgendaCongelacao.Site.Admin.Agenda_Listagem" %>

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
                <h5>
                    <i class="fas fa-edit fa-lg w3-margin-right"></i>Agenda
                </h5>
            </div>

            <%--Filtros--%>
            <div class="w3-panel">
                <div class="w3-card">
                    <div class="w3-gray">
                        <h5 class="w3-margin-left w3-text-white">Filtros</h5>
                    </div>

                    <div class="w3-row-padding w3-margin-top">
                        <div class="w3-col s12 m3 l2">
                            <asp:Label
                                ID="lblDataInicial"
                                runat="server"
                                Text="Data inicial"></asp:Label>
                            <asp:TextBox
                                ID="txtDataInicial"
                                CssClass="w3-input w3-border w3-small"
                                runat="server"
                                TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="w3-col s12 m3 l2">
                            <asp:Label
                                ID="lblDataFinal"
                                runat="server"
                                Text="Data final"></asp:Label>
                            <asp:TextBox
                                ID="txtDataFinal"
                                CssClass="w3-input w3-border w3-small"
                                runat="server"
                                TextMode="Date"></asp:TextBox>
                        </div>
                    </div>

                    <div class="w3-row-padding w3-margin-top">
                        <div class="w3-col s12 m3 l2">
                            <asp:Label
                                ID="lblLocal"
                                runat="server"
                                Text="Local"></asp:Label>
                            <asp:TextBox
                                ID="txtLocal"
                                CssClass="w3-input w3-border w3-small"
                                runat="server"></asp:TextBox>
                        </div>
                        <div class="w3-col s12 m3 l2">
                            <asp:Label
                                ID="lblMedico"
                                runat="server"
                                Text="Médico"></asp:Label>
                            <asp:TextBox
                                ID="txtMedico"
                                CssClass="w3-input w3-border w3-small"
                                runat="server"></asp:TextBox>
                        </div>
                        <div class="w3-col s12 m3 l3">
                            <asp:Label
                                ID="lblConvenio"
                                runat="server"
                                Text="Convênio"></asp:Label>
                            <asp:TextBox
                                ID="txtConvenio"
                                CssClass="w3-input w3-border w3-small"
                                runat="server"></asp:TextBox>
                        </div>
                        <div class="w3-col s12 m3 l3">
                            <asp:Label
                                ID="lblEstadoAgenda"
                                runat="server"
                                Text="Estado agenda"></asp:Label>
                            <asp:DropDownList
                                ID="ddlEstadoAgenda"
                                CssClass="w3-input w3-border w3-small"
                                runat="server">
                            </asp:DropDownList>
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
                    <div id="infoTotalRegistros" class="w3-margin-top w3-margin-bottom" runat="server" visible="false">
                        Total de registros
                        <span class="w3-badge w3-dark-gray" style="font-weight: bold;">
                            <asp:Label ID="lblTotalRegistros" runat="server"></asp:Label>
                        </span>
                    </div>

                    <%--Grid--%>
                    <div class="w3-responsive">
                        <asp:GridView
                            ID="gvAgendas"
                            runat="server"
                            AllowSorting="true"
                            AllowPaging="true"
                            AutoGenerateColumns="false"
                            CssClass="w3-table-all w3-small"
                            GridLines="None"
                            OnPageIndexChanging="gvAgendas_PageIndexChanging"
                            OnRowCommand="gvAgendas_RowCommand"
                            OnRowDataBound="gvAgendas_RowDataBound"
                            OnSorting="gvAgendas_Sorting"
                            PageSize="10">

                            <HeaderStyle CssClass="w3-dark-gray" />
                            <PagerSettings PageButtonCount="10" />

                            <EmptyDataTemplate>
                                <div class="w3-panel w3-khaki w3-center w3-padding-24" style="font: bold;">
                                    Nenhum registro encontrado.
                                </div>
                            </EmptyDataTemplate>

                            <Columns>
                              
                                <%--Icon--%>
                                <asp:TemplateField
                                    ItemStyle-Width="25px"
                                    HeaderText="">
                                    <ItemTemplate>
                                        <asp:Literal ID="litIcon" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--Data/hora--%>
                                <asp:TemplateField
                                    HeaderText="Data/hora"
                                    ItemStyle-Width="130px"
                                    SortExpression="datahora">
                                    <ItemTemplate>
                                        <asp:Literal ID="litDataHora" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--Local--%>
                                <asp:TemplateField
                                    HeaderText="Local"
                                    ItemStyle-Width="250px"
                                    SortExpression="local">
                                    <ItemTemplate>
                                        <asp:Literal ID="litLocal" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--NomeMedico--%>
                                <asp:TemplateField
                                    HeaderText="Médico"
                                    ItemStyle-Width="250px"
                                    SortExpression="medico">
                                    <ItemTemplate>
                                        <asp:Literal ID="litNomeMedico" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--Convênio--%>
                                <asp:TemplateField
                                    HeaderText="Convênio"
                                    ItemStyle-Width="150px"
                                    SortExpression="convenio">
                                    <ItemTemplate>
                                        <asp:Literal ID="litConvenio" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--StatusAgenda--%>
                                <asp:TemplateField
                                    HeaderText="Estado agenda"
                                    ItemStyle-Width="100px"
                                    SortExpression="estadoagenda">
                                    <ItemTemplate>
                                        <asp:Literal ID="litEstadoAgenda" runat="server"></asp:Literal>
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
                                            CommandArgument="<%# ((Edelweiss.AgendaCongelacao.Model.Entities.Agenda)Container.DataItem).AgendaID %>"
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
