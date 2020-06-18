<%@ Page
    Language="C#"
    MasterPageFile="~/Admin/Site.Master"
    AutoEventWireup="true"
    CodeBehind="ConfiguracaoNotificacaoAgenda-Manutencao.aspx.cs"
    Inherits="Edelweiss.AgendaCongelacao.Site.Admin.ConfiguracaoNotificacaoAgenda_Manutencao" %>

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
                <h5><i class="fas fa-cogs fa-lg w3-margin-right"></i>Configurações das notificações</h5>
                <asp:HiddenField ID="hifConfiguracaoNotificacaoAgendaID" runat="server" />
            </div>

            <div class="w3-row-padding w3-margin-top">
                <div class="w3-col s12 m3 l2">
                    <asp:Label
                        ID="lblTempo"
                        runat="server">Tempo<span class="w3-text-red" style="margin-left:4px;">*</span>
                    </asp:Label>
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
                        runat="server">Unidade de tempo<span class="w3-text-red" style="margin-left:4px;">*</span></asp:Label>
                    <asp:DropDownList
                        ID="ddlUnidadeTempo"
                        CssClass="w3-input w3-border w3-small"
                        runat="server">
                    </asp:DropDownList>
                </div>
            </div>

            <div class="w3-row-padding">
                <div class="w3-col s12 m12 l12">
                    <div class="w3-panel w3-pale-blue w3-border w3-border-blue" style="color: darkblue !important;">
                        <h3><i class="fas fa-info-circle fa-1x w3-margin-right"></i>Informação!</h3>
                        <p class="w3-small">
                            As configurações para notificação de agenda funcionam como um lembrete e <b>são aplicadas de forma global</b> para todas as agendas existentes.
                            Quando uma agenda tem seu estado alterado para <b>Confirmado</b>, automaticamente o sistema cria a exata quantidade de notificações
                            existente nas configurações para essa agenda. <br/>
                            Posteriormente, um serviço (um console executado por uma tarefa agendada no Windows) utiliza cada uma dessas configurações (conforme o seu tempo de antecipação) para enviar o SMS para o celular de destino.
                        </p>
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

            ConfiguracaoNotificacaoAgendaManutencao = {

                self: this

                , init: function () {
                    this.addControls();
                    this.attachEvent();
                }

                , addControls: function () {
                    self.$hifConfiguracaoNotificacaoAgendaID = $('input[id*=hifConfiguracaoNotificacaoAgendaID]');
                    self.$txtTempo = $('input[id*=txtTempo]');
                    self.$txtTempo.mask('000');
                    self.$ddlUnidadeTempo = $('select[id*=ddlUnidadeTempo]');
                    self.$btnSalvar = $('#btnSalvar');
                }

                , attachEvent: function () {
                    self.$btnSalvar.on('click', function () {
                        ConfiguracaoNotificacaoAgendaManutencao.save();
                    });
                }

                , check: function () {
                    if (
                        self.$txtTempo.val().length == 0
                        || self.$ddlUnidadeTempo.prop('selectedIndex') == 0
                    ) {
                        return true;
                    }
                    return false;
                }

                , save: function () {
                    if (ConfiguracaoNotificacaoAgendaManutencao.check()) {
                        Dialog.show
                            (
                                'Atenção!'
                                , 'Informe os campos obrigatórios.'
                                , Dialog.MessageType.WARNING
                            );
                        return;
                    }

                    var UnidadeTempoAgenda = {
                        UnidadeTempoAgendaID: self.$ddlUnidadeTempo.val()
                    }

                    var ConfiguracaoNotificacaoAgenda = {
                        ConfiguracaoNotificacaoAgendaID: Number(self.$hifConfiguracaoNotificacaoAgendaID.val())
                        , Tempo: self.$txtTempo.val()
                        , UnidadeTempoAgenda: UnidadeTempoAgenda
                    }

                    var configuracaoJson = JSON.stringify(ConfiguracaoNotificacaoAgenda);

                    $.ajax({
                        type: 'post'
                        , url: 'Ashx/ConfiguracaoNotificacaoAgenda.ashx?op=salvar'
                        , data: configuracaoJson
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

            ConfiguracaoNotificacaoAgendaManutencao.init();

        });
    </script>
</asp:Content>
