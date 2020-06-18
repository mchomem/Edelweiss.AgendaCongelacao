<%@ Control
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="Menu.ascx.cs"
    Inherits="Edelweiss.AgendaCongelacao.Site.Admin.UserControl.Menu" %>

<aside runat="server">

    <div class="w3-card-4 w3-margin">
        <div class="w3-bar-block">
            <asp:LinkButton
                ID="lkbDashboard"
                runat="server"
                CssClass="w3-bar-item w3-button w3-blue-grey"
                OnClick="lkbDashboard_Click">
                <i class="fas fa-chart-pie w3-margin-right"></i>Dashboard
            </asp:LinkButton>
        </div>
    </div>

    <div class="w3-card-4 w3-margin">
        <div class="w3-bar-block">
            <asp:LinkButton
                ID="lbkMenuItemLogSmsAgenda"
                runat="server"
                CssClass="w3-bar-item w3-button w3-blue-grey"
                OnClick="lbkMenuItemLogSmsAgenda_Click">
                <i class="fas fa-sms w3-margin-right"></i>Log SMS
            </asp:LinkButton>
        </div>
    </div>

    <div class="w3-card-4 w3-margin">
        <div class="w3-bar-block">
            <asp:LinkButton
                ID="lkbMenuItemAgenda"
                runat="server"
                CssClass="w3-bar-item w3-button w3-blue-grey"
                OnClick="lkbMenuItemAgenda_Click">
                <i class="fas fa-edit w3-margin-right"></i>Agenda
            </asp:LinkButton>
        </div>
    </div>

    <div class="w3-card-4 w3-margin">
        <div class="w3-bar-block">
            <asp:LinkButton
                ID="lbkMenuItemConfiguracaoNotificacao"
                runat="server"
                CssClass="w3-bar-item w3-button w3-blue-grey"
                OnClick="lbkMenuItemConfiguracaoNotificacao_Click">
                <i class="fas fa-cogs w3-margin-right"></i>Configurações das Notificações
            </asp:LinkButton>
        </div>
    </div>

    <div class="w3-card-4 w3-margin">
        <div class="w3-bar-block">
            <asp:LinkButton
                ID="lbkMenuItemMedicoExecucaoAgenda"
                runat="server"
                CssClass="w3-bar-item w3-button w3-blue-grey"
                OnClick="lbkMenuItemMedicoExecucaoAgenda_Click">
                <i class="fas fa-user-md w3-margin-right"></i>Médico de execução da agenda
            </asp:LinkButton>
        </div>
    </div>

    <div class="w3-card-4 w3-margin">
        <div class="w3-bar-block">
            <asp:LinkButton
                ID="lbkMenuItemUsuario"
                runat="server"
                CssClass="w3-bar-item w3-button w3-blue-grey"
                OnClick="lbkMenuItemUsuario_Click">
                <i class="fas fa-user w3-margin-right"></i>Usuário
            </asp:LinkButton>
        </div>
    </div>
</aside>
