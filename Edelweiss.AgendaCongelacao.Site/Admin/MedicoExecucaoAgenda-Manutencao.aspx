<%@ Page
    Language="C#"
    MasterPageFile="~/Admin/Site.Master"
    AutoEventWireup="true"
    CodeBehind="MedicoExecucaoAgenda-Manutencao.aspx.cs"
    Inherits="Edelweiss.AgendaCongelacao.Site.Admin.MedicoExecucaoAgenda_Manutencao" %>

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
                <h5><i class="fas fa-user-md fa-lg w3-margin-right"></i>Médico de execução da agenda</h5>
                <asp:HiddenField ID="hifMedicoExecucaoAgendaID" runat="server" />
            </div>

            <div class="w3-row-padding w3-margin-top">
                <div class="w3-col s12 m3 l3">
                    <asp:Label
                        ID="lblNome"
                        runat="server">Nome<span class="w3-text-red" style="margin-left:4px;">*</span></asp:Label>
                    <asp:TextBox
                        ID="txtNome"
                        CssClass="w3-input w3-border w3-small"
                        MaxLength="100"
                        runat="server" />
                </div>
                <div class="w3-col s12 m3 l3">
                    <asp:Label
                        ID="lblEmail"
                        runat="server">E-mail<span class="w3-text-red" style="margin-left:4px;">*</span></asp:Label>
                    <asp:TextBox
                        ID="txtEmail"
                        CssClass="w3-input w3-border w3-small"
                        MaxLength="100"
                        runat="server" />
                </div>
                <div class="w3-col s12 m3 l2">
                    <asp:Label
                        ID="lblCelular"
                        runat="server">Celular<span class="w3-text-red" style="margin-left:4px;">*</span></asp:Label>
                    <asp:TextBox
                        ID="txtCelular"
                        CssClass="w3-input w3-border w3-small"
                        MaxLength="11"
                        placeholder="(00) 000000000"
                        runat="server" />
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

            MedicoExecucaoAgendaManutencao = {

                self: this

                , init: function () {
                    this.addControls();
                    this.attachEvent();
                }

                , addControls: function () {
                    self.$hifMedicoExecucaoAgendaID = $('input[id*=hifMedicoExecucaoAgendaID]');
                    self.$txtNome = $('input[id*=txtNome]');
                    self.$txtEmail = $('input[id*=txtEmail]');
                    self.$txtCelular = $('input[id*=txtCelular]');
                    self.$txtCelular.mask('(00) 000000000');
                    self.$btnSalvar = $('#btnSalvar');
                }

                , attachEvent: function () {
                    self.$btnSalvar.on('click', function () {
                        MedicoExecucaoAgendaManutencao.save();
                    });
                }

                , check: function () {
                    if (
                        self.$txtNome.val().length == 0
                        || self.$txtEmail.val().length == 0
                        || self.$txtCelular.val().length == 0) {
                        return true;
                    }
                    return false;
                }

                , save: function () {
                    if (MedicoExecucaoAgendaManutencao.check()) {
                        Dialog.show
                            (
                                'Atenção!'
                                , 'Informe os campos obrigatórios.'
                                , Dialog.MessageType.WARNING
                            );
                        return;
                    }

                    if ( Utils.onlyNumbers(self.$txtCelular.val()).length < 11 ) {
                        Dialog.show
                            (
                                'Atenção!'
                                , 'O nº do celular deve conter o DDD mais os 9 dígitos.'
                                , Dialog.MessageType.WARNING
                            );
                        return;
                    }

                    var MedicoExecucaoAgenda = {
                        MedicoExecucaoAgendaID: Number(self.$hifMedicoExecucaoAgendaID.val())
                        , Nome: self.$txtNome.val()
                        , Email: self.$txtEmail.val()
                        , Celular: Utils.onlyNumbers(self.$txtCelular.val())
                    }

                    var medicoExecucaoAgendaJson = JSON.stringify(MedicoExecucaoAgenda);

                    $.ajax({
                        type: 'post'
                        , url: 'Ashx/MedicoExecucaoAgenda.ashx?op=salvar'
                        , data: medicoExecucaoAgendaJson
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

            MedicoExecucaoAgendaManutencao.init();

        });
    </script>

</asp:Content>
