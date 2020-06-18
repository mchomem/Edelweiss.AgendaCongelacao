<%@ Control
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="Message.ascx.cs"
    Inherits="Edelweiss.AgendaCongelacao.Site.UserControl.Message" %>

<asp:Panel
    ID="pnlMessage"
    CssClass="modal fade"
    runat ="server"
    tabindex="-1"
    role="dialog"
    aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div id="modalHeader" class="modal-header text-white" runat="server">
                <h5 class="modal-title">
                    <asp:Literal
                        ID="litTitle"
                        runat="server"></asp:Literal>
                </h5>
                <asp:LinkButton
                    ID="lkbClose"
                    runat="server"
                    OnClick="btnClose_Click">
                    <span aria-hidden="true">&times;</span>
                </asp:LinkButton>
            </div>
            <div class="modal-body">
                <p>
                    <asp:Literal
                        ID="litMenssage"
                        runat="server"></asp:Literal>
                </p>
            </div>
            <div class="modal-footer py-2">
                <asp:Button
                    ID="btnClose"
                    CssClass="btn btn-secondary"
                    runat ="server"
                    Text="OK"
                    OnClick="btnClose_Click"
                    Style="width:75px"/>
            </div>
        </div>
    </div>
</asp:Panel>
