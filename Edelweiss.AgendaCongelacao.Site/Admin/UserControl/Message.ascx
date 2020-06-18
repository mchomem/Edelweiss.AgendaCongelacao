<%@ Control
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="Message.ascx.cs"
    Inherits="Edelweiss.AgendaCongelacao.Site.Admin.UserControl.Message" %>

<asp:Panel
    ID="pnlMessage"
    CssClass="w3-modal"
    runat="server"
    Style="display: none">
    <div class="w3-modal-content w3-animate-top w3-card-4">
        <header id="modalHeader" class="w3-container" runat="server">
            <asp:LinkButton
                ID="lkbClose"
                CssClass="w3-button w3-large w3-display-topright"
                runat="server"
                ToolTip="Pressione Esc para fechar."
                OnClick="btnClose_Click">x</asp:LinkButton>
            <h2>
                <asp:Literal ID="litTitle" runat="server"></asp:Literal>
            </h2>
        </header>
        <div class="w3-container w3-margin">
            <p>
                <asp:Literal ID="litMenssage" runat="server"></asp:Literal>
            </p>
        </div>
        <footer class="w3-container w3-border-top">
            <div class="w3-margin-top w3-margin-bottom w3-right">
                <asp:Button
                    ID="btnClose"
                    CssClass="w3-button"
                    runat="server"
                    Text="Ok"
                    ToolTip="Pressione Esc para fechar."
                    OnClick="btnClose_Click"
                    Style="width:75px;" />
            </div>
        </footer>
    </div>

</asp:Panel>

<script>
    $(document).ready(function () {

        Message = {

            self: this

            , init: function () {
                this.addControls();
                this.attachEvent();
            }

            , addControls: function () {
                self.$pnlMessage = $('div[id*=pnlMessage]');
            }

            , attachEvent: function () {
                $(document).keydown(function (e) {
                    if (e.keyCode == 27) {
                        self.$pnlMessage.attr('style', 'display:nome;');
                    }
                });
            }
        }

        Message.init();
    });
</script>
