<%@ Page
    Language="C#"
    MasterPageFile="~/Admin/Site.Master"
    AutoEventWireup="true"
    CodeBehind="MedicoExecucaoAgenda-Listagem.aspx.cs"
    Inherits="Edelweiss.AgendaCongelacao.Site.Admin.MedicoExecucaoAgenda_Listagem" %>

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
                    <i class="fas fa-user-md fa-lg w3-margin-right"></i>Médico de execução da agenda
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
                        <div class="w3-col s12 m3 l3">
                            <asp:Label
                                ID="lblEmail"
                                runat="server">E-mail</asp:Label>
                            <asp:TextBox
                                ID="txtEmail"
                                CssClass="w3-input w3-border w3-small"
                                runat="server" />
                        </div>
                        <div class="w3-col s12 m3 l2">
                            <asp:Label
                                ID="lblCelular"
                                runat="server">Celular</asp:Label>
                            <asp:TextBox
                                ID="txtCelular"
                                CssClass="w3-input w3-border w3-small"
                                placeholder="(00) 000000000"
                                runat="server" />
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
                            ID="gvMedicoExecucaoAgenda"
                            runat="server"
                            AllowSorting="true"
                            AllowPaging="true"
                            AutoGenerateColumns="false"
                            CssClass="w3-table-all w3-small"
                            GridLines="None"
                            OnPageIndexChanging="gvMedicoExecucaoAgenda_PageIndexChanging"
                            OnRowCommand="gvMedicoExecucaoAgenda_RowCommand"
                            OnRowDataBound="gvMedicoExecucaoAgenda_RowDataBound"
                            OnSorting="gvMedicoExecucaoAgenda_Sorting"
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

                                <%--E-mail--%>
                                <asp:TemplateField
                                    HeaderText="E-mail"
                                    ItemStyle-Width="20px"
                                    SortExpression="Email">
                                    <ItemTemplate>
                                        <asp:Literal ID="litEmail" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--Celular--%>
                                <asp:TemplateField
                                    HeaderText="Celular"
                                    ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <asp:Literal ID="litCelular" runat="server"></asp:Literal>
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
                                            CommandArgument="<%# ((Edelweiss.AgendaCongelacao.Model.Entities.MedicoExecucaoAgenda)Container.DataItem).MedicoExecucaoAgendaID %>">
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
                                            CommandArgument="<%# ((Edelweiss.AgendaCongelacao.Model.Entities.MedicoExecucaoAgenda)Container.DataItem).MedicoExecucaoAgendaID %>"
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

    <script>

        $(document).ready(function () {

            MedicoExecucaoAgendaListagem = {

                self: this

                , init: function () {
                    this.addControl();
                }

                , addControl: function () {
                    self.$txtCelular = $('input[id*=txtCelular]');
                    self.$txtCelular.mask('(00) 000000000');
                }

            }

            MedicoExecucaoAgendaListagem.init();

        });

    </script>

</asp:Content>
