<%@ Page
    Language="C#"
    MasterPageFile="~/Admin/Site.Master"
    AutoEventWireup="true"
    CodeBehind="ConfiguracaoNotificacaoAgenda-Listagem.aspx.cs"
    Inherits="Edelweiss.AgendaCongelacao.Site.Admin.ConfiguracaoNotificacaoAgenda_Listagem" %>

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
                    <i class="fas fa-cogs fa-lg w3-margin-right"></i>Configurações das notificações
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
                        <div class="w3-col s12 m3 l1">
                            <asp:Label ID="lblTempo" runat="server">Tempo</asp:Label>
                            <asp:TextBox
                                ID="txtTempo"
                                CssClass="w3-input w3-border w3-small"
                                placeholder="000"
                                MaxLength="3"
                                runat="server" />
                        </div>
                        <div class="w3-col s12 m3 l2">
                            <asp:Label
                                ID="lblUnidadeTempo"
                                runat="server">Unidade de tempo</asp:Label>
                            <asp:DropDownList
                                ID="ddlUnidadeTempo"
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
                            ID="gvConfiguracaoNotificacao"
                            runat="server"
                            AllowSorting="true"
                            AllowPaging="true"
                            AutoGenerateColumns="false"
                            CssClass="w3-table-all w3-small"
                            GridLines="None"
                            OnPageIndexChanging="gvConfiguracaoNotificacao_PageIndexChanging"
                            OnRowCommand="gvConfiguracaoNotificacao_RowCommand"
                            OnRowDataBound="gvConfiguracaoNotificacao_RowDataBound"
                            OnSorting="gvConfiguracaoNotificacao_Sorting"
                            PageSize="10">

                            <HeaderStyle CssClass="w3-dark-gray" />
                            <PagerSettings PageButtonCount="10" />

                            <EmptyDataTemplate>
                                <div class="w3-panel w3-khaki w3-center w3-padding-24" style="font: bold;">
                                    Nenhum registro encontrado.
                                </div>
                            </EmptyDataTemplate>

                            <Columns>
                                <%--Tempo--%>
                                <asp:TemplateField
                                    HeaderText="Tempo"
                                    ItemStyle-Width="20px"
                                    SortExpression="Tempo">
                                    <ItemTemplate>
                                        <asp:Literal ID="litTempo" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--Unidade Tempo Agenda--%>
                                <asp:TemplateField
                                    HeaderText="Unidade de tempo p/ agenda"
                                    ItemStyle-Width="20px"
                                    SortExpression="UnidadeTempoAgenda">
                                    <ItemTemplate>
                                        <asp:Literal ID="litUnidadeTempo" runat="server"></asp:Literal>
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
                                            CommandArgument="<%# ((Edelweiss.AgendaCongelacao.Model.Entities.ConfiguracaoNotificacaoAgenda)Container.DataItem).ConfiguracaoNotificacaoAgendaID %>">
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
                                            CommandArgument="<%# ((Edelweiss.AgendaCongelacao.Model.Entities.ConfiguracaoNotificacaoAgenda)Container.DataItem).ConfiguracaoNotificacaoAgendaID %>"
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

            ConfiguracaoNotificacaoAgendaListagem = {

                selt: this

                , init: function () {
                    this.addControl();
                }

                , addControl: function () {
                    self.$txtTempo = $('input[id*=txtTempo]');
                    self.$txtTempo.mask('000');
                }

            }

            ConfiguracaoNotificacaoAgendaListagem.init();

        });
    </script>

</asp:Content>
