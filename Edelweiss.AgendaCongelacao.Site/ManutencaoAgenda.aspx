<%@ Page
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="ManutencaoAgenda.aspx.cs"
    Inherits="Edelweiss.AgendaCongelacao.Site.ManutencaoAgenda" %>

<%@ Register
    Src="~/UserControl/Message.ascx"
    TagName="Message"
    TagPrefix="ucEdelweiss" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <ucEdelweiss:Message ID="msgDialog" runat="server" />

    <div class="card">
        <div class="card-header bg-primary text-white">
            <h5 class="mb-0"><i class="far fa-edit fa-lg mr-2"></i>Agendamento</h5>
            <asp:HiddenField ID="hifAgendaID" runat="server" Value="0" />
        </div>
        <div id="divImpressao" class="card-body">
            <div class="row">
                <div class="col-6 col-sm-3 col-md-3">
                    <div class="form-group">
                        <asp:Label
                            ID="lblDataAgenda"
                            runat="server">
                            Data<span class="ml-2 text-danger">*</span>
                        </asp:Label>
                        <asp:TextBox
                            ID="txtDataAgenda"
                            CssClass="form-control form-control-sm"
                            runat="server"
                            TextMode="Date">
                        </asp:TextBox>
                    </div>
                </div>
                <div class="col-5 col-sm-3 col-md-2">
                    <div class="form-group">
                        <asp:Label
                            ID="lblHoraAgenda"
                            runat="server">
                            Hora<span class="ml-2 text-danger">*</span>
                        </asp:Label>
                        <asp:TextBox
                            ID="txtHoraAgenda"
                            CssClass="form-control form-control-sm"
                            runat="server"
                            TextMode="Time">
                        </asp:TextBox>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-4">
                    <div class="form-group">
                        <asp:Label
                            ID="lblLocal"
                            runat="server">
                            Local<span class="ml-2 text-danger">*</span>
                        </asp:Label>
                        <asp:TextBox
                            ID="txtLocal"
                            CssClass="form-control form-control-sm"
                            MaxLength="100"
                            runat="server">
                        </asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-6 col-md-5">
                    <div class="form-group">
                        <asp:Label
                            ID="lblNomeMedico"
                            runat="server">
                            Nome médico<span class="ml-2 text-danger">*</span>
                        </asp:Label>
                        <asp:TextBox
                            ID="txtNomeMedico"
                            CssClass="form-control form-control-sm"
                            MaxLength="100"
                            runat="server">
                        </asp:TextBox>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-4">
                    <div class="form-group">
                        <asp:Label
                            ID="lblMedicoExecucaoAgenda"
                            runat="server">
                            Médico (executa agenda)<span class="ml-2 text-danger">*</span>
                        </asp:Label>
                        <asp:DropDownList
                            ID="ddlMedicoExecucaoAgenda"
                            runat="server"
                            CssClass="form-control form-control-sm">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-6 col-md-5">
                    <div class="form-group">
                        <asp:Label
                            ID="lblNomePaciente"
                            runat="server">
                            Nome paciente<span class="ml-2 text-danger">*</span>
                        </asp:Label>
                        <asp:TextBox
                            ID="txtNomePaciente"
                            CssClass="form-control form-control-sm"
                            MaxLength="50"
                            runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="col-12 col-sm-6 col-md-4">
                    <div class="form-group">
                        <asp:Label
                            ID="lblConvenio"
                            runat="server">
                            Convênio<span class="ml-2 text-danger">*</span>
                        </asp:Label>
                        <asp:TextBox
                            ID="txtConvenio"
                            CssClass="form-control form-control-sm"
                            MaxLength="50"
                            runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-12 col-md-5">
                    <div class="form-group">
                        <asp:Label
                            ID="lblProcedimento"
                            runat="server">
                            Procedimento<span class="ml-2 text-danger">*</span>
                        </asp:Label>
                        <asp:TextBox
                            ID="txtProcedimento"
                            CssClass="form-control form-control-sm"
                            MaxLength="100"
                            runat="server">
                        </asp:TextBox>
                    </div>
                </div>
                <div class="col-12 col-sm-12 col-md-2">
                    <div class="form-group">
                        <asp:Label
                            ID="lblTelefoneContato"
                            runat="server">
                            Telefone contato<span class="ml-2 text-danger">*</span>
                        </asp:Label>
                        <asp:TextBox
                            ID="txtTelefoneContato"
                            CssClass="form-control form-control-sm"
                            MaxLength="11"
                            placeholder="(00) 000000000"
                            runat="server">
                        </asp:TextBox>
                    </div>
                </div>
                <div class="col-12 col-sm-12 col-md-2">
                    <div class="form-group">
                        <asp:Label
                            ID="lblEstadoAgenda"
                            runat="server">
                            Estado da agenda<span class="ml-2 text-danger">*</span>
                        </asp:Label>
                        <asp:DropDownList
                            ID="ddlEstadoAgenda"
                            runat="server"
                            CssClass="form-control form-control-sm">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-sm-12 col-md-9 col-lg-9">
                    <div class="alert alert-warning mb-0 pt-1">
                        <b>Observações:</b><br>
                        <div class="mt-2">
                            Qualquer alteração na agenda fará com que o serviço SMS
                            reenvie a mensagem para o médico novamente.
                            <b>Não</b> é permitido marcar agendamentos após as 
                            <b>19:00</b> e também aos <b>finais de semana</b>.
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="card-footer">
            <div class="row">
                <div class="col">
                    <asp:Button
                        ID="btnCancelar"
                        CssClass="btn btn-outline-primary btn-sm"
                        Text="Cancelar"
                        runat="server"
                        OnClick="btnCancelar_Click" />
                    <button id="btnSalvar"
                        class="btn btn-success btn-sm"
                        type="button">
                        Salvar
                    </button>
                    <button
                        id="btnImprimirAgenda"
                        class="btn btn-primary btn-sm float-right"
                        type="button"
                        style="display: none;">
                        Imprimir
                    </button>
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {

            ManutencaoAgenda = {

                self: this

                , init: function () {
                    self.$agenda = null;

                    this.addControls();
                    this.attachEvent();
                }

                , addControls: function () {
                    self.$hifAgendaID = $('input[id*=hifAgendaID]');
                    self.$divImpressao = $('#divImpressao');
                    self.$txtDataAgenda = $('input[id*=txtDataAgenda]');
                    self.$txtHoraAgenda = $('input[id*=txtHoraAgenda]');
                    self.$txtLocal = $('input[id*=txtLocal]');
                    self.$txtNomeMedico = $('input[id*=txtNomeMedico]');
                    self.$ddlMedicoExecucaoAgenda = $('select[id*=ddlMedicoExecucaoAgenda]');
                    self.$txtNomePaciente = $('input[id*=txtNomePaciente]');
                    self.$txtConvenio = $('input[id*=txtConvenio]');
                    self.$txtProcedimento = $('input[id*=txtProcedimento]');
                    self.$txtTelefoneContato = $('input[id*=txtTelefoneContato]');
                    self.$txtTelefoneContato.mask('(00) 000000000');
                    self.$ddlEstadoAgenda = $('select[id*=ddlEstadoAgenda]');
                    self.$btnSalvar = $('#btnSalvar');
                    self.$btnImprimirAgenda = $('#btnImprimirAgenda');

                    // Somente exibe o botão de impressão quando em edição.
                    if (Number(self.$hifAgendaID.val()) > 0) {
                        self.$btnImprimirAgenda.css('display', 'block');
                    }
                }

                , attachEvent: function () {
                    self.$btnSalvar.on('click', function () {
                        ManutencaoAgenda.save();
                    });

                    self.$btnImprimirAgenda.on('click', function () {

                        if (Number(self.$hifAgendaID.val()) > 0) {
                            // Consultar a agenda(em edição) da base de dados e comparar os dados da
                            // agenda com os dados na tela. Se for igual permite imprimir, se não barra e emite uma mensagem.
                            ManutencaoAgenda.getAgenda();

                        } else {
                            ManutencaoAgenda.printForm(self.$divImpressao);
                        }

                    });
                }

                , check: function () {
                    if (
                        self.$txtDataAgenda.val().length == 0
                        || self.$txtHoraAgenda.val().length == 0
                        || self.$txtLocal.val().length == 0
                        || self.$txtNomeMedico.val().length == 0
                        || self.$ddlMedicoExecucaoAgenda.prop('selectedIndex') == 0
                        || self.$txtNomePaciente.val().length == 0
                        || self.$txtConvenio.val().length == 0
                        || self.$txtProcedimento.val().length == 0
                        || self.$txtTelefoneContato.val().length == 0
                        || self.$ddlEstadoAgenda.prop('selectedIndex') == 0
                    ) {
                        return true;
                    }

                    return false;
                }

                , save: function () {
                    if (ManutencaoAgenda.check()) {
                        Dialog.show
                            (
                                'Atenção!'
                                , 'Informe os campos obrigatórios.'
                                , Dialog.MessageType.WARNING
                            );
                        return;
                    }

                    var MedicoExecucaoAgenda = {
                        MedicoExecucaoAgendaID: self.$ddlMedicoExecucaoAgenda.val()
                    }

                    var EstadoAgenda = {
                        EstadoAgendaID: self.$ddlEstadoAgenda.val()
                    }

                    var Agenda = {
                        AgendaID: Number(self.$hifAgendaID.val())
                        , DataHoraEvento: ManutencaoAgenda.joinDateTime()
                        , Local: self.$txtLocal.val()
                        , NomeMedico: self.$txtNomeMedico.val()
                        , MedicoExecucaoAgenda: MedicoExecucaoAgenda
                        , NomePaciente: self.$txtNomePaciente.val()
                        , Convenio: self.$txtConvenio.val()
                        , Procedimento: self.$txtProcedimento.val()
                        , TelefoneContato: Utils.onlyNumbers(self.$txtTelefoneContato.val())
                        , EstadoAgenda: EstadoAgenda
                        , Ativo: true
                    }

                    var agendaJson = JSON.stringify(Agenda);

                    $.ajax({
                        type: 'post'
                        , url: 'Ashx/agenda.ashx?op=salvar'
                        , data: agendaJson
                        , contentType: 'application/json; charset=utf-8'
                        , dataType: "json"
                    })
                    .done(function (data, textStatus, jqXHR) {
                        switch (data.ReturnCode) {
                            case 'SUCCESS':
                                ManutencaoAgenda.printForm(self.$divImpressao);
                                window.location.href = 'ListagemAgenda.aspx';
                                break;

                            case 'WARNING':
                                self.$btnImprimirAgenda.css('display', 'none');
                                Dialog.show('Aviso!', data.Message, Dialog.MessageType.WARNING);
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

                , joinDateTime: function () {
                    var date = self.$txtDataAgenda.val()
                    var time = self.$txtHoraAgenda.val();
                    var dateTime = moment(date + ' ' + time).format();
                    return dateTime;
                }

                , printForm: function (el) {
                    if (ManutencaoAgenda.check()) {
                        Dialog.show
                            (
                                'Aviso!'
                                , 'Para imprimir a agenda, os campos obrigatórios devem estar preenchidos!'
                                , Dialog.MessageType.WARNING
                            );
                        return;
                    }

                    //var janelaImpressao = window.open('', '_blank', '', 'print');
                    var janelaImpressao = window.open('', 'print');

                    var html = $('<html></html>');
                    var body = $('<body></body>');
                    var table = $('<table></table>');

                    table.css({
                        'border-color': '#000000'
                        , 'border-width': '1px'
                        , 'border-style': 'solid'
                        , 'font-family': 'Verdana'
                        , 'width': '100%'
                    });

                    var colHeader = $('<th colspan=2>' + document.title + '</th>');
                    colHeader.css({
                        'border-bottom-color': 'black'
                        , 'border-bottom-style': 'solid'
                        , 'border-bottom-width': '1px'
                        , 'padding-bottom': '10px'
                        , 'padding-top': '5px'
                    });

                    var rowHeader = $('<tr></tr>');
                    rowHeader.append(colHeader);

                    var dataHora = moment(self.$txtDataAgenda.val()).format('DD[/]MM[/]YYYY') + ' às ' + self.$txtHoraAgenda.val();
                    var row1 = $('<tr><td style=\'width:150px;\'>Data/hora:</td><td><b>' + dataHora + '</b></td></tr>');
                    var row2 = $('<tr><td>Local:</td><td>' + self.$txtLocal.val() + '</td></tr>');
                    var row3 = $('<tr><td>Médico:</td><td>' + self.$txtNomeMedico.val() + '</td></tr>');
                    var row4 = $('<tr><td>Médico execução:</td><td>' + self.$ddlMedicoExecucaoAgenda.find(':selected').text() + '</td></tr>');
                    var row5 = $('<tr><td>Paciente:</td><td>' + self.$txtNomePaciente.val() + '</td></tr>');
                    var row6 = $('<tr><td>Convênio:</td><td>' + self.$txtConvenio.val() + '</td></tr>');
                    var row7 = $('<tr><td>Procedimento:</td><td>' + self.$txtProcedimento.val() + '</td></tr>');
                    var row8 = $('<tr><td>Telefone contato:</td><td>' + self.$txtTelefoneContato.val() + '</td></tr>');
                    var row9 = $('<tr><td>Estado agenda:</td><td>' + self.$ddlEstadoAgenda.find(':selected').text() + '</td></tr>');

                    table.append(rowHeader);
                    table.append(row1);
                    table.append(row2);
                    table.append(row3);
                    table.append(row4);
                    table.append(row5);
                    table.append(row6);
                    table.append(row7);
                    table.append(row8);
                    table.append(row9);

                    body.append(table);
                    html.append(body);
                    janelaImpressao.document.write(html.html());

                    janelaImpressao.document.close(); // Necessary for IE >= 10
                    janelaImpressao.focus(); // Necessary for IE >= 10*/

                    janelaImpressao.print();
                    /* Ajuste para atrasar o comando de encerramento da janela de impressão */
                    setTimeout(function () { janelaImpressao.close(); }, 500);

                    return true;
                }

                , getAgenda: function () {

                    var Agenda = {
                        AgendaID: Number(self.$hifAgendaID.val())
                    }

                    var agendaJson = JSON.stringify(Agenda);

                    $.ajax({
                        type: 'post'
                        , url: 'Ashx/Agenda.ashx?op=consultar'
                        , data: agendaJson
                        , contentType: 'application/json; charset=utf-8'
                        , dataType: "json"
                    })
                    .done(function (data, textStatus, jqXHR) {
                        if (data.ReturnCode == 'SUCCESS') {
                            if (data.Entity != null) {
                                self.$agenda = data.Entity;

                                var dataBanco = moment(self.$agenda.DataHoraEvento).format('DD[/]MM[/]YYYY[ ]HH[:]mm[:]ss');
                                var dataCampo = moment(ManutencaoAgenda.joinDateTime()).format('DD[/]MM[/]YYYY[ ]HH[:]mm[:]ss');

                                if (
                                    dataBanco != dataCampo
                                    || self.$agenda.Local != self.$txtLocal.val()
                                    || self.$agenda.NomeMedico != self.$txtNomeMedico.val()
                                    || self.$agenda.MedicoExecucaoAgenda.Nome != self.$ddlMedicoExecucaoAgenda.find('option:selected').text()
                                    || self.$agenda.NomePaciente != self.$txtNomePaciente.val()
                                    || self.$agenda.Convenio != self.$txtConvenio.val()
                                    || self.$agenda.Procedimento != self.$txtProcedimento.val()
                                    || self.$agenda.TelefoneContato != Utils.onlyNumbers(self.$txtTelefoneContato.val())
                                    || self.$agenda.EstadoAgenda.Estado != self.$ddlEstadoAgenda.find('option:selected').text()
                                ) {
                                    Dialog.show
                                        (
                                            'Aviso!'
                                            , 'Foi detectado uma alteração nos campos do formulário, para imprimir esta agenda você precisa salvar o registro primeiro.'
                                            , Dialog.MessageType.WARNING
                                        );
                                    return;
                                } else {
                                    ManutencaoAgenda.printForm(self.$divImpressao);
                                }
                            }
                        }
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        var detalhe = '';

                        if (jqXHR.responseJSON != null || jqXHR.responseJSON != undefined) {
                            detalhe = jqXHR.responseJSON.Message;
                        }

                        Dialog.show('Erro!', 'Ocorreu um erro: ' + detalhe, Dialog.MessageType.ERROR);
                    })
                    .always();

                }
            }

            ManutencaoAgenda.init();

        });
    </script>
</asp:Content>
