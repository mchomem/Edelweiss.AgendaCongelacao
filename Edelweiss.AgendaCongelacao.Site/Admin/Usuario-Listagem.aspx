<%@ Page
    Language="C#"
    MasterPageFile="~/Admin/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Usuario-Listagem.aspx.cs"
    Inherits="Edelweiss.AgendaCongelacao.Site.Admin.Usuario_Listagem" %>

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
                    <i class="fas fa-user fa-lg w3-margin-right"></i>Usuário
                </h5>
            </div>

            <div class="w3-row-padding w3-margin-top">
                <div class="w3-col s12 m3 l2">
                    <asp:Button
                        ID="btnNovo"
                        CssClass="w3-button w3-border w3-border-green w3-green w3-small"
                        runat="server"
                        Text="Novo"
                        OnClick="btnNovo_Click" />
                </div>
            </div>

            <%--Filtros--%>
            <div class="w3-panel">
                <div class="w3-card">
                    <div class="w3-gray">
                        <h5 class="w3-margin-left w3-text-white">Filtros</h5>
                    </div>

                    <div class="w3-row-padding w3-margin-top">
                        <div class="w3-col s12 m3 l3">
                            <asp:Label
                                ID="lblNome"
                                runat="server">Nome</asp:Label>
                            <asp:TextBox
                                ID="txtNome"
                                CssClass="w3-input w3-border w3-small"
                                runat="server" />
                        </div>
                        <div class="w3-col s12 m3 l4">
                            <label>Situação do usuário</label>
                            <div>
                                <input id="rbtAtivo" class="w3-radio" type="radio" runat="server" />
                                <label>Ativo</label>
                                <input id="rbtDesativado" class="w3-radio" type="radio" runat="server" />
                                <label>Desativado</label>
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
                    <div id="infoTotalRegistros" class="w3-margin-top w3-margin-bottom" runat="server" visible="false">
                        Total de registros
                        <span class="w3-badge w3-dark-gray" style="font-weight: bold;">
                            <asp:Label ID="lblTotalRegistros" runat="server"></asp:Label>
                        </span>
                    </div>

                    <%--Grid--%>
                    <div class="w3-responsive">
                        <asp:GridView
                            ID="gvUsuario"
                            runat="server"
                            AllowSorting="true"
                            AllowPaging="true"
                            AutoGenerateColumns="false"
                            CssClass="w3-table-all w3-small"
                            GridLines="None"
                            OnPageIndexChanging="gvUsuario_PageIndexChanging"
                            OnRowCommand="gvUsuario_RowCommand"
                            OnRowDataBound="gvUsuario_RowDataBound"
                            OnSorting="gvUsuario_Sorting"
                            PageSize="10">

                            <HeaderStyle CssClass="w3-dark-gray" />
                            <PagerSettings PageButtonCount="10" />

                            <EmptyDataTemplate>
                                <div class="w3-panel w3-khaki w3-center w3-padding-24" style="font: bold;">
                                    Nenhum registro encontrado.
                                </div>
                            </EmptyDataTemplate>

                            <Columns>
                                <%--Nome--%>
                                <asp:TemplateField
                                    HeaderText="Nome"
                                    ItemStyle-Width="20px"
                                    SortExpression="Nome">
                                    <ItemTemplate>
                                        <asp:Literal ID="litNome" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--Ativo--%>
                                <asp:TemplateField
                                    HeaderText="Ativo"
                                    ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <asp:Literal ID="litAtivo" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--Edição--%>
                                <asp:TemplateField ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <center>
                                        <asp:LinkButton
                                            ID="lkbEditar"
                                            runat="server"
                                            ToolTip="Editar"
                                            CommandName="Editar"
                                            CommandArgument="<%# ((Edelweiss.AgendaCongelacao.Model.Entities.UsuarioAdministracaoAgenda)Container.DataItem).UsuarioAdministracaoAgendaID %>">
                                            <i class="fas fa-pen"></i>
                                        </asp:LinkButton>
                                    </center>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--Exclusão--%>
                                <asp:TemplateField ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <center>
                                        <asp:LinkButton
                                            ID="lkbExcluir"
                                            runat="server"
                                            ToolTip="Excluir"
                                            CommandName="Excluir"
                                            CommandArgument="<%# ((Edelweiss.AgendaCongelacao.Model.Entities.UsuarioAdministracaoAgenda)Container.DataItem).UsuarioAdministracaoAgendaID %>"
                                            OnClientClick="return confirm('Deseja excluir este registro?');">
                                            <i class="fas fa-trash-alt" style="color:red;"></i>
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
