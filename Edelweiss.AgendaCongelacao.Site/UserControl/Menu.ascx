<%@ Control
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="Menu.ascx.cs"
    Inherits="Edelweiss.AgendaCongelacao.Site.UserControl.Menu" %>

<aside runat="server">
    <asp:LinkButton
        ID="lkbMenuItemCalendario"
        runat="server"
        CssClass="btn btn-outline-primary btn-block"
        OnClick="lkbMenuItemCalendario_Click">
        <div class="row">
			<div class="col-2"><i class="fas fa-calendar-alt"></i></div>
			<div class="col-10 text-left">Calendário</div>
		</div>
    </asp:LinkButton>
    <asp:LinkButton
        ID="lkbMenuItemAgendamento"
        runat="server"
        CssClass="btn btn-outline-primary btn-block"
        OnClick="lkbMenuItemAgendamento_Click">
        <div class="row">
			<div class="col-2"><i class="fas fa-edit"></i></div>
			<div class="col-10 text-left">Agendamento</div>
		</div>
    </asp:LinkButton>
</aside>
