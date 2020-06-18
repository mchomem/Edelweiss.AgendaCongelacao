<%@ Page
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="Login.aspx.cs"
    Inherits="Edelweiss.AgendaCongelacao.Site.Admin.Login" %>

<%@ Register
    Src="~/Admin/UserControl/Message.ascx"
    TagName="Message"
    TagPrefix="ucEdelweissAdmin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <link rel="stylesheet" href="Content/css/w3.css" />
    <link rel="stylesheet" href="Content/css/Login.css" />
    <script src="Content/script/jquery-3.4.1.min.js"></script>
    <title>Login - Administração de Agendamento de congelação</title>
</head>
<body class="w3-container">
    <div class="w3-row">
        <div class="w3-col">
            <form id="form" runat="server" class="w3-card-4 w3-margin-top">

                <ucEdelweissAdmin:Message ID="msgDialog" runat="server" />

                <div class="w3-row w3-padding-24">
                    <div class="w3-col w3-center">
                        <img src="Content/images/Logo.png" alt="Edelweiss logo" />
                        <h1>Login</h1>
                        <h5>Administração - Agendamento de congelação</h5>
                    </div>
                </div>
                <div class="w3-row-padding w3-margin-bottom">
                    <div class="w3-col">
                        <asp:Label ID="lblUsuario" runat="server">Login</asp:Label>
                        <asp:TextBox
                            ID="txtLogin"
                            CssClass="w3-input w3-border"
                            runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="w3-row-padding w3-margin-bottom">
                    <div class="w3-col">
                        <asp:Label ID="lblSenha" runat="server">Senha</asp:Label>
                        <asp:TextBox
                            ID="txtSenha"
                            CssClass="w3-input w3-border"
                            runat="server"
                            TextMode="Password"></asp:TextBox>
                    </div>
                </div>
                <div class="w3-row-padding w3-margin-bottom">
                    <div class="w3-col s6 m6 l6">
                        <asp:Button
                            ID="btnVoltar"
                            CssClass="w3-btn w3-block w3-border w3-white w3-border-gray"
                            runat="server"
                            Text="Voltar"
                            OnClick="btnVoltar_Click"/>
                    </div>
                    <div class="w3-col s6 m6 l6">
                        <asp:Button
                            ID="btnLogin"
                            CssClass="w3-btn w3-block w3-green"
                            runat="server"
                            Text="Login"
                            OnClick="btnLogin_Click"/>
                    </div>
                </div>
                <div class="w3-padding"></div>
            </form>
        </div>
    </div>
</body>
</html>
