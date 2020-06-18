<%@ Page
    Language="C#"
    MasterPageFile="~/Admin/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Usuario-Manutencao.aspx.cs"
    Inherits="Edelweiss.AgendaCongelacao.Site.Admin.Usuario_Manutencao" %>

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
                <h5><i class="fas fa-user fa-lg w3-margin-right"></i>Usuário</h5>
                <asp:HiddenField ID="hifUsuarioAdministracaoAgendaID" runat="server" />
            </div>

            <div class="w3-row-padding w3-margin-top">
                <div class="w3-col s12 m3 l4">
                    <asp:Label
                        ID="lblNome"
                        runat="server">Nome<span class="w3-text-red" style="margin-left:4px;">*</span></asp:Label>
                    <asp:TextBox
                        ID="txtNome"
                        CssClass="w3-input w3-border w3-small"
                        MaxLength="100"
                        runat="server" />
                </div>
            </div>

            <div class="w3-row-padding w3-margin-top">
                <div class="w3-col s12 m3 l3">
                    <asp:Label
                        ID="lblLogin"
                        runat="server">Login<span class="w3-text-red" style="margin-left:4px;">*</span></asp:Label>
                    <asp:TextBox
                        ID="txtLogin"
                        CssClass="w3-input w3-border w3-small"
                        MaxLength="20"
                        runat="server" />
                </div>
                <div class="w3-col s12 m3 l3">
                    <asp:Label
                        ID="lblSenha"
                        runat="server">Senha<span class="w3-text-red" style="margin-left:4px;">*</span></asp:Label>
                    <div class="w3-bar">
                        <asp:TextBox
                            ID="txtSenha"
                            CssClass="w3-input w3-border w3-small w3-bar-item"
                            MaxLength="30"
                            runat="server" />
                        <button id="showPassword"
                            class="w3-button w3-border w3-border-blue-gray w3-blue-gray w3-bar-item"
                            type="button"
                            style="height:36px;min-width:53px;">
                            <i class="fas fa-eye"></i>
                        </button>
                    </div>
                </div>
            </div>

            <div class="w3-padding"></div>

            <div class="w3-container w3-light-grey w3-border-top w3-padding">
                <div class="w3-right">
                    <asp:Button
                        ID="btnCancelar"
                        CssClass="w3-button w3-white w3-border w3-border-blue-gray"
                        runat="server"
                        Text="Cancelar"
                        OnClick="btnCancelar_Click" />
                    <button
                        id="btnSalvar"
                        class="w3-button w3-green w3-border w3-border-green"
                        type="button">
                        Salvar
                    </button>
                </div>
            </div>
        </div>
    </div>

    <script>
        $(document).ready(function () {

            UsuarioManutencao = {

                self: this

                , init: function () {
                    this.addControls();
                    this.attachEvent();
                }

                , addControls: function () {
                    self.$hifMedicoExecucaoAgendaID = $('input[id*=hifUsuarioAdministracaoAgendaID]');
                    self.$txtNome = $('input[id*=txtNome]');
                    self.$txtLogin = $('input[id*=txtLogin]');
                    self.$txtSenha = $('input[id*=txtSenha]');
                    self.$showPassword = $('#showPassword');
                    self.$btnSalvar = $('#btnSalvar');
                }

                , attachEvent: function () {

                    self.$showPassword.on('click', function () {
                        if (self.$txtSenha.attr('type') == 'password') {
                            self.$showPassword.find('i').attr('class', 'fas fa-eye-slash');                            
                            self.$txtSenha.attr('type', 'text');
                        } else {
                            self.$showPassword.find('i').attr('class', 'fas fa-eye');
                            self.$txtSenha.attr('type', 'password');
                        }
                    });

                    self.$btnSalvar.on('click', function () {
                        UsuarioManutencao.save();
                    });
                }

                , check: function () {
                    if (
                        self.$txtNome.val().length == 0
                        || self.$txtLogin.val().length == 0
                        || self.$txtSenha.val().length == 0) {
                        return true;
                    }
                    return false;
                }

                , save: function () {
                    if (UsuarioManutencao.check()) {
                        Dialog.show
                            (
                                'Atenção!'
                                , 'Informe os campos obrigatórios.'
                                , Dialog.MessageType.WARNING
                            );
                        return;
                    }

                    var UsuarioAdministracaoAgenda = {
                        UsuarioAdministracaoAgendaID: Number(self.$hifMedicoExecucaoAgendaID.val())
                        , Nome: self.$txtNome.val()
                        , Login: self.$txtLogin.val()
                        , Senha: self.$txtSenha.val()
                    }

                    var usuarioAdministracaoAgenda = JSON.stringify(UsuarioAdministracaoAgenda);

                    $.ajax({
                        type: 'post'
                        , url: 'Ashx/UsuarioAdministracaoAgenda.ashx?op=salvar'
                        , data: usuarioAdministracaoAgenda
                        , contentType: 'application/json; charset=utf-8'
                        , dataType: "json"
                    })
                        .done(function (data, textStatus, jqXHR) {
                            switch (data.ReturnCode) {
                                case 'SUCCESS':
                                    Dialog.show
                                        (
                                            'Sucesso!'
                                            , 'Registro salvo com sucesso.'
                                            , Dialog.MessageType.SUCCESS
                                        );
                                    break;

                                case 'WARNING':
                                    alert('Aviso! ' + data.Message);
                                    break;

                                default:
                                    break;
                            }
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            var detalhe = '';

                            if (jqXHR.responseJSON != null || jqXHR.responseJSON != undefined) {
                                detalhe = jqXHR.responseJSON.Message;
                            }

                            Dialog.show('Erro!', 'Ocorreu um erro: ' + detalhe, Dialog.MessageType.ERROR);
                        })
                        .always(function (jqXHR, textStatus, errorThrown) {
                            ;
                        });
                }

            }

            UsuarioManutencao.init();

        });
    </script>

</asp:Content>
