<%@ Page
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="CalendarioAgenda.aspx.cs"
    Inherits="Edelweiss.AgendaCongelacao.Site.CalendarioAgenda" %>

<%@ Register
    Src="~/UserControl/Message.ascx"
    TagName="Message"
    TagPrefix="ucEdelweiss" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <ucEdelweiss:Message ID="msgDialog" runat="server" />

    <div class="card">
        <div class="card-header bg-primary text-white">
            <h5 class="mb-0"><i class="far fa-calendar-alt fa-lg mr-2"></i>Calendário</h5>
        </div>
        <div class="card-body">
            <div class="row">

                <%--Calendário--%>
                <div class="col-4">
                    <asp:Calendar
                        ID="calAgenda"
                        runat="server"
                        ShowGridLines="true"
                        OnSelectionChanged="calAgenda_SelectionChanged"
                        OnDayRender="calAgenda_DayRender"
                        OnVisibleMonthChanged="calAgenda_VisibleMonthChanged"></asp:Calendar>
                </div>

                <%--Agendas do dia selecionado--%>
                <div class="col-8">
                    <div class="card">
                        <div class="card-header py-1">
                            Agendas do dia
                        </div>
                        <div class="card-body p-2">

                            <div class="row">
                                <div class="col">
                                    <div class="form-group">
                                        <asp:Button
                                            ID="btnNovo"
                                            CssClass="btn btn-sm btn-success"
                                            OnClick="btnNovo_Click"
                                            runat="server"
                                            Text="Novo" />
                                    </div>
                                </div>
                            </div>

                            <asp:GridView
                                ID="gvAgendaDoDia"
                                runat="server"
                                AllowPaging="true"
                                AllowSorting="true"
                                AutoGenerateColumns="false"
                                CssClass="dataGrid m-0"
                                OnPageIndexChanging="gvAgendaDoDia_PageIndexChanging"
                                OnRowCommand="gvAgendaDoDia_RowCommand"
                                OnRowDataBound="gvAgendaDoDia_RowDataBound"
                                PageSize="5"
                                Style="width: 100%;">

                                <RowStyle BackColor="#E0E0E0" />
                                <AlternatingRowStyle BackColor="#C0C0C0" />
                                <PagerSettings PageButtonCount="10" />
                                <PagerStyle CssClass="pgr" />
                                <EmptyDataTemplate>
                                    <div class="alert alert-warning text-center m-0">
                                        Não foram localizados agendamentos para este dia.<br />
                                        <asp:Button
                                            ID="btnAgendar"
                                            CssClass="btn btn-sm btn-success mt-2"
                                            runat="server"
                                            OnClick="btnAgendar_Click"
                                            Text="Agendar" />
                                    </div>
                                </EmptyDataTemplate>

                                <Columns>
                                    <%--Data/hora--%>
                                    <asp:TemplateField ItemStyle-Width="130px" HeaderText="Data/hora">
                                        <ItemTemplate>
                                            <div class="pl-2">
                                                <asp:Literal ID="litDataHora" runat="server"></asp:Literal>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--Local--%>
                                    <asp:TemplateField ItemStyle-Width="250px" HeaderText="Local">
                                        <ItemTemplate>
                                            <div class="pl-2">
                                                <asp:Literal ID="litLocal" runat="server"></asp:Literal>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--NomeMedico--%>
                                    <asp:TemplateField ItemStyle-Width="250px" HeaderText="Médico">
                                        <ItemTemplate>
                                            <div class="pl-2">
                                                <asp:Literal ID="litNomeMedico" runat="server"></asp:Literal>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--StatusAgenda--%>
                                    <asp:TemplateField ItemStyle-Width="100px" HeaderText="Estado agenda">
                                        <ItemTemplate>
                                            <div class="pl-2">
                                                <asp:Literal ID="litEstadoAgenda" runat="server"></asp:Literal>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--Edição--%>
                                <asp:TemplateField ItemStyle-Width="25px">
                                    <ItemTemplate>
                                        <div class="pl-2">
                                            <asp:LinkButton
                                                ID="lkbEditar"
                                                runat="server"
                                                ToolTip="Editar"
                                                CommandName="Editar"
                                                CommandArgument="<%# ((Edelweiss.AgendaCongelacao.Model.Entities.Agenda)Container.DataItem).AgendaID %>">
                                                <i class="fas fa-pen"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--Exclusão--%>
                                <asp:TemplateField ItemStyle-Width="25px">
                                    <ItemTemplate>
                                        <div class="pl-2">
                                            <asp:LinkButton
                                                ID="lkbExcluir"
                                                runat="server"
                                                ToolTip="Excluir"
                                                CommandName="Excluir"
                                                CommandArgument="<%# ((Edelweiss.AgendaCongelacao.Model.Entities.Agenda)Container.DataItem).AgendaID %>"
                                                OnClientClick="return confirm('Deseja excluir este registro de agenda?');">
                                                <i class="fas fa-trash-alt" style="color:red;"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                </Columns>

                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
