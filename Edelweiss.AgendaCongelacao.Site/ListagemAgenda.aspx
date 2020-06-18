<%@ Page
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="ListagemAgenda.aspx.cs"
    Inherits="Edelweiss.AgendaCongelacao.Site.ListagemAgenda" %>

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
            <h5 class="mb-0"><i class="far fa-edit fa-lg mr-2"></i>Agendamento</h5>
        </div>
        <div class="card-body">

            <%--Navegação para novo registro--%>
            <div class="row">
                <div class="col-3 col-sm-2 col-md-1">
                    <div class="form-group">
                        <asp:Button
                            ID="btnNovo"
                            CssClass="btn btn-sm btn-success"
                            runat="server"
                            Text="Novo"
                            OnClick="btnNovo_Click" />
                    </div>
                </div>
            </div>

            <%--Filtros--%>
            <div class="row">
                <div class="col">
                    <div class="card">
                        <div class="card-header bg-secondary py-0 px-2 text-white font-weight-bold">
                            Filtros
                        </div>
                        <div class="card-body p-2">
                            <div class="row">
                                <div class="col-3 col-sm-2 col-md-2">
                                    <div class="form-group">
                                        <asp:Label
                                            ID="lblDataInicial"
                                            runat="server"
                                            Text="Data inicial"></asp:Label>
                                        <asp:TextBox
                                            ID="txtDataInicial"
                                            CssClass="form-control p-1 form-control-sm"
                                            runat="server"
                                            TextMode="Date"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-3 col-sm-2 col-md-2">
                                    <div class="form-group">
                                        <asp:Label
                                            ID="lblDataFinal"
                                            runat="server"
                                            Text="Data final"></asp:Label>
                                        <asp:TextBox
                                            ID="txtDataFinal"
                                            CssClass="form-control p-1 form-control-sm"
                                            runat="server"
                                            TextMode="Date"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-3 col-sm-3 col-md-3">
                                    <div class="form-group">
                                        <asp:Label
                                            ID="lblLocal"
                                            runat="server"
                                            Text="Local"></asp:Label>
                                        <asp:TextBox
                                            ID="txtLocal"
                                            CssClass="form-control form-control-sm py-0"
                                            runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-3 col-sm-3 col-md-3">
                                    <div class="form-group">
                                        <asp:Label
                                            ID="lblMedico"
                                            runat="server"
                                            Text="Médico"></asp:Label>
                                        <asp:TextBox
                                            ID="txtMedico"
                                            CssClass="form-control form-control-sm py-0"
                                            runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-3 col-sm-3 col-md-3">
                                    <div class="form-group">
                                        <asp:Label
                                            ID="lblConvenio"
                                            runat="server"
                                            Text="Convênio"></asp:Label>
                                        <asp:TextBox
                                            ID="txtConvenio"
                                            CssClass="form-control form-control-sm py-0"
                                            runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-3 col-sm-2 col-md-2">
                                    <div class="form-group">
                                        <asp:Label
                                            ID="lblEstadoAgenda"
                                            runat="server"
                                            Text="Estado agenda"></asp:Label>
                                        <asp:DropDownList
                                            ID="ddlEstadoAgenda"
                                            CssClass="form-control form-control-sm py-0"
                                            runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-3 col-sm-2 col-md-2">
                                    <asp:Button
                                        ID="btnLimpar"
                                        CssClass="btn btn-outline-primary btn-sm"
                                        runat="server"
                                        Text="Limpar"
                                        OnClick="btnLimpar_Click" />
                                    <asp:Button
                                        ID="btnPesquisar"
                                        CssClass="btn btn-primary btn-sm"
                                        runat="server"
                                        Text="Pesquisar"
                                        OnClick="btnPesquisar_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <%--Grid view--%>
            <div class="row">
                <div class="col">

                    <%--Contador de registros--%>
                    <div id="infoTotalRegistros" class="mt-4 mb-2" runat="server" visible="false">
                        Total de registros<span class="ml-2 badge badge-dark">
                            <asp:Label ID="lblTotalRegistros" runat="server"></asp:Label>
                        </span>
                    </div>

                    <%--Grid--%>
                    <div class="table-responsive">
                        <asp:GridView
                            ID="gvAgendamentos"
                            runat="server"
                            AllowSorting="true"
                            AllowPaging="true"
                            AutoGenerateColumns="false"
                            CssClass="dataGrid m-0"
                            GridLines="None"
                            OnPageIndexChanging="gvAgendamentos_PageIndexChanging"
                            OnRowCommand="gvAgendamentos_RowCommand"
                            OnRowDataBound="gvAgendamentos_RowDataBound"
                            OnSorting="gvAgendamentos_Sorting"
                            PageSize="10"
                            Style="width: 100%;">

                            <RowStyle BackColor="#E0E0E0" />
                            <AlternatingRowStyle BackColor="#C0C0C0" />
                            <PagerSettings PageButtonCount="10" />
                            <PagerStyle CssClass="pgr" />
                            <EmptyDataTemplate>
                                <div class="alert alert-warning text-center m-0">
                                    Não foram localizados agendamentos
                                </div>
                            </EmptyDataTemplate>

                            <Columns>
                                <%--Icon--%>
                                <asp:TemplateField
                                    ItemStyle-Width="25px"
                                    HeaderText="">
                                    <ItemTemplate>
                                        <div class="pl-2">
                                            <asp:Literal ID="litIcon" runat="server"></asp:Literal>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--Data/hora--%>
                                <asp:TemplateField
                                    HeaderText="Data/hora"
                                    ItemStyle-Width="130px"
                                    SortExpression="datahora">
                                    <ItemTemplate>
                                        <div class="pl-2">
                                            <asp:Literal ID="litDataHora" runat="server"></asp:Literal>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--Local--%>
                                <asp:TemplateField
                                    HeaderText="Local"
                                    ItemStyle-Width="250px"
                                    SortExpression="local">
                                    <ItemTemplate>
                                        <div class="pl-2">
                                            <asp:Literal ID="litLocal" runat="server"></asp:Literal>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--NomeMedico--%>
                                <asp:TemplateField
                                    HeaderText="Médico"
                                    ItemStyle-Width="250px"
                                    SortExpression="medico">
                                    <ItemTemplate>
                                        <div class="pl-2">
                                            <asp:Literal ID="litNomeMedico" runat="server"></asp:Literal>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--Convênio--%>
                                <asp:TemplateField
                                    HeaderText="Convênio"
                                    ItemStyle-Width="150px"
                                    SortExpression="convenio">
                                    <ItemTemplate>
                                        <div class="pl-2">
                                            <asp:Literal ID="litConvenio" runat="server"></asp:Literal>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--StatusAgenda--%>
                                <asp:TemplateField
                                    HeaderText="Estado agenda"
                                    ItemStyle-Width="100px"
                                    SortExpression="estadoagenda">
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
</asp:Content>
