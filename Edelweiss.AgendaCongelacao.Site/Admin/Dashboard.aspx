<%@ Page
    Language="C#"
    MasterPageFile="~/Admin/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Dashboard.aspx.cs"
    Inherits="Edelweiss.AgendaCongelacao.Site.Admin.Dashboard" %>

<%@ Register
    Src="~/Admin/UserControl/Message.ascx"
    TagName="Message"
    TagPrefix="ucEdelweissAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ucedelweissadmin:message id="msgDialog" runat="server" />

    <div class="w3-panel">
        <div id="divDashboard" class="w3-card-4 w3-white">
            <div class="w3-container w3-blue-gray">
                <h5>
                    <i class="fas fa-chart-pie fa-lg w3-margin-right"></i>Dashboard
                    <button
                        id="btnExpandirRecolher"
                        class="w3-right w3-button"
                        title="Tela cheia"
                        type="button"
                        style="padding: 0px;">
                        <i class="fas fa-expand fa-lg"></i>
                    </button>
                </h5>
            </div>

            <div id="divLoader" class="w3-center" style="height: 400px; margin-top: 200px;">
                <i class="fa fa-spinner fa-spin fa-4x w3-text-blue-gray"></i>
            </div>

            <div id="divDataForm" class="w3-hide">
                <div class="w3-row-padding w3-margin-top">
                    <div class="w3-col s6 m3 l2">
                        <asp:Label ID="lblCores" runat="server">Paleta de cores</asp:Label>
                        <select id="selCores" class="w3-input w3-border w3-small">
                            <option value="1">Céu azul</option>
                            <option value="2">Crepúsculo</option>
                            <option value="3">Ultra violeta</option>
                        </select>
                    </div>
                    <div class="w3-col s6 m3 l2">
                        <asp:Label ID="lblAno" runat="server">Ano</asp:Label>
                        <select id="selAno" class="w3-input w3-border w3-small">
                            <option>Selecione o ano</option>
                        </select>
                    </div>
                    <div class="w3-col s12 m3 l1">
                        <label>&nbsp;</label>
                        <button
                            id="btnGerar"
                            class="w3-button w3-small w3-blue w3-border w3-border-blue w3-block"
                            type="button">
                            Gerar</button>
                    </div>
                    <div class="w3-col s12 m3 l5">
                        <br />
                        <input id="chkAtualizar" class="w3-check" type="checkbox" />
                        <label>Atualizar automaticamente</label>
                    </div>
                </div>

                <div class="w3-row-padding w3-margin-top">
                    <div class="w3-half w3-border">
                        <canvas id="barChartAgendasAno"></canvas>
                    </div>
                    <div class="w3-half w3-border">
                        <canvas id="pieChartAgendasAno"></canvas>
                    </div>
                </div>

                <div class="w3-row-padding w3-margin-top">
                    <div class="w3-quarter">
                        <div class="w3-container w3-teal w3-padding-16">
                            <div class="w3-left"><i class="fa fa-edit w3-xxxlarge"></i></div>
                            <div class="w3-right">
                                <h3 id="cardTotalAgendas">0</h3>
                            </div>
                            <div class="w3-clear"></div>
                            <h4 class="w3-center">Agendamentos no ano</h4>
                        </div>
                    </div>
                </div>
            </div>

            <div class="w3-padding"></div>
        </div>
    </div>

    <script>
        $(document).ready(function () {

            Dashboard = {

                self: this

                , init: function () {
                    self.$isFullScreen = false;
                    self.$refreshDashboard;
                    self.$barChart;
                    self.$pieChart;
                    self.$ListColors = Dashboard.setGradientColorForMonthsType1();

                    this.addContros();
                    this.attachEvent();
                    this.getAnosDisponiveis();
                    this.refreshDashboard();
                }

                , addContros: function () {
                    self.$divDashboard = ('#divDashboard');
                    self.$btnExpandirRecolher = $('#btnExpandirRecolher');
                    self.$divLoader = $('#divLoader');

                    self.$divDataForm = $('#divDataForm');
                    self.$selCores = $('#selCores');
                    self.$selAno = $('#selAno');
                    self.$btnGerar = $('#btnGerar');
                    self.$chkAtualizar = $('#chkAtualizar');
                    self.$chkAtualizar.prop('checked', 'checked');

                    self.$barChartAgendasAno = $('#barChartAgendasAno');
                    self.$pieChartAgendasAno = $('#pieChartAgendasAno');

                    self.$cardTotalAgendas = $('#cardTotalAgendas');
                }

                , attachEvent: function () {

                    self.$btnExpandirRecolher.on('click', function () {
                        self.$isFullScreen = (self.$isFullScreen ? false : true);
                        Dashboard.setFullScreen(self.$isFullScreen);
                    });

                    self.$selCores.on('change', function () {
                        switch (self.$selCores.val()) {
                            case '1':
                                self.$ListColors = Dashboard.setGradientColorForMonthsType1();
                                break;

                            case '2':
                                self.$ListColors = Dashboard.setGradientColorForMonthsType2();
                                break;

                            case '3':
                                self.$ListColors = Dashboard.setGradientColorForMonthsType3();
                                break;

                            default:
                                self.$ListColors = Dashboard.setGradientColorForMonthsType1();
                                break;
                        }
                    });

                    self.$btnGerar.on('click', function () {
                        Dashboard.getAgendasAno();
                    });

                    self.$chkAtualizar.on('click', function () {
                        if (self.$chkAtualizar.prop('checked')) {
                            Dashboard.refreshDashboard();
                        } else {
                            clearInterval(self.$refreshDashboard);
                        }
                    });
                }

                , setFullScreen(full) {
                    var icon = self.$btnExpandirRecolher.find('i')[0];
                    $(icon).toggleClass('fa-expand fa-compress');

                    if (full) {
                        $(self.$divDashboard).fullscreen();
                        self.$btnExpandirRecolher.prop('title', 'Sair da tela cheia');
                    } else {
                        $.fullscreen.exit();
                        self.$btnExpandirRecolher.prop('title', 'Tela cheia');
                    }
                }

                , check: function () {
                    if (self.$selAno.prop('selectedIndex') == 0) {
                        return true;
                    }
                    return false;
                }

                , getAnosDisponiveis: function () {
                    $.get({
                        url: 'Ashx/Dashboard.ashx?op=consultarAnosDisponiveis'
                    })
                        .done(function (data, textStatus, jqXHR) {
                            if (data.ReturnCode == 'SUCCESS') {
                                if (data.Entity != null) {

                                    // Alimenta os options do select com os anos disponíveis.
                                    for (var i = 0; i < data.Entity.length; i++) {
                                        self.$selAno.append('<option>' + data.Entity[i] + '</option>');
                                        // Seleciona o valor do ano, se um dos valores for o ano corrente.
                                        if (new Date().getFullYear() === data.Entity[i]) {
                                            self.$selAno.find('option').prop('selected', true);
                                        }
                                    }

                                    //  Se a combo foi carregada com o valor do ano corrente, carrega os gráficos.
                                    if (self.$selAno.prop('selectedIndex') != 0) {
                                        Dashboard.getAgendasAno();
                                    } else {
                                        // Varrer as options do select em busca de um ano dispónível.
                                        self.$selAno.find('option').each(function (index, element) {
                                            // Encontra o primeiro valor na option e marca como selecionado.
                                            if (!isNaN($(element).val())) {
                                                $(element).prop('selected', true);
                                                Dashboard.getAgendasAno();
                                            }
                                        });
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
                        .always(function (jqXHR, textStatus, errorThrown) {
                            ;
                        });
                }

                , getAgendasAno: function () {

                    if (Dashboard.check()) {
                        Dialog.show
                            (
                                'Atenção!'
                                , 'Selecione um ano para gerar os gráficos.'
                                , Dialog.MessageType.WARNING
                            );
                        return;
                    }

                    var AgendasAno = {
                        Ano: self.$selAno.val()
                    }

                    var agendasAnoJson = JSON.stringify(AgendasAno);

                    $.ajax({
                        type: 'post'
                        , url: 'Ashx/Dashboard.ashx?op=consultarAgendasAno'
                        , data: agendasAnoJson
                        , contentType: 'application/json; charset=utf-8'
                        , dataType: "json"
                    })
                        .done(function (data, textStatus, jqXHR) {
                            self.$divDataForm.removeClass('w3-hide');

                            if (data.ReturnCode == 'SUCCESS') {
                                if (data.Entity != null) {
                                    Dashboard.barChartAgendasAno(data.Entity);
                                    Dashboard.pieChartAgendasAno(data.Entity);
                                    Dashboard.getCardTotalAno(data.Entity);
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
                        .always(function (jqXHR, textStatus, errorThrown) {
                            self.$divLoader.addClass('w3-hide');
                        });
                }

                // Atualiza o dashboard a cada 30 segundos.
                , refreshDashboard: function () {
                    self.$refreshDashboard = setInterval(Dashboard.getAgendasAno, 30000);
                }

                , barChartAgendasAno: function (agendasAno) {

                    // Remove o gráfico anterior no canvas.
                    if (self.$barChart != undefined) {
                        self.$barChart.destroy();
                    }

                    var ctx = self.$barChartAgendasAno;

                    config = {
                        type: 'bar',
                        data: {
                            labels: agendasAno.Meses
                            , datasets: [{
                                label: 'Agendamentos de congelação'
                                , data: agendasAno.Quantidades
                                , backgroundColor: self.$ListColors
                                , borderColor: self.$ListColors
                                , borderWidth: 1
                            }]
                        },
                        options: {
                            legend: {
                                display: false
                            }
                            , title: {
                                display: true
                                , text: 'Agendamentos em ' + self.$selAno.find('option:selected').text()
                            }
                            , scales: {
                                yAxes: [{
                                    ticks: {
                                        beginAtZero: true
                                        , max: 20
                                    }
                                }]
                            }
                        }
                    }

                    self.$barChart = new Chart(ctx, config);
                }

                , pieChartAgendasAno: function (agendasAno) {

                    // Remove o gráfico anterior no canvas.
                    if (self.$pieChart != undefined) {
                        self.$pieChart.destroy();
                    }

                    var ctx = self.$pieChartAgendasAno

                    var config = {
                        type: 'pie'
                        , data: {
                            datasets: [{
                                data: agendasAno.Quantidades
                                , backgroundColor: self.$ListColors
                                , label: 'Dataset 1'
                            }]
                            , labels: agendasAno.Meses
                        }
                        , options: {
                            responsive: true
                            , title: {
                                display: true
                                , text: 'Agendamentos em ' + self.$selAno.find('option:selected').text()
                            }
                        }
                    };

                    self.$pieChart = new Chart(ctx, config);
                }

                , setGradientColorForMonthsType1: function () {
                    var listColors = [];

                    listColors[0] = 'rgba(170, 204, 255, 1)';
                    listColors[1] = 'rgba(154, 191, 247, 1)';
                    listColors[2] = 'rgba(139, 179, 239, 1)';
                    listColors[3] = 'rgba(123, 166, 231, 1)';
                    listColors[4] = 'rgba(108, 154, 224, 1)';
                    listColors[5] = 'rgba(92, 142, 216, 1)';
                    listColors[6] = 'rgba(77, 129, 208, 1)';
                    listColors[7] = 'rgba(61, 117, 200, 1)';
                    listColors[8] = 'rgba(46, 105, 193, 1)';
                    listColors[9] = 'rgba(30, 92, 185, 1)';
                    listColors[10] = 'rgba(15, 80, 177, 1)';
                    listColors[11] = 'rgba(0, 68, 170, 1)';

                    return listColors;
                }

                , setGradientColorForMonthsType2: function () {
                    var listColors = [];

                    listColors[0] = 'rgba(255, 204, 0, 1)';
                    listColors[1] = 'rgba(255, 163, 0, 1)';
                    listColors[2] = 'rgba(255, 122, 0, 1)';
                    listColors[3] = 'rgba(255, 81, 0, 1)';
                    listColors[4] = 'rgba(255, 40, 0, 1)';
                    listColors[5] = 'rgba(255, 0, 0, 1)';
                    listColors[6] = 'rgba(212, 8, 0, 1)';
                    listColors[7] = 'rgba(170, 17, 42, 1)';
                    listColors[8] = 'rgba(127, 25, 64, 1)';
                    listColors[9] = 'rgba(85, 34, 85, 1)';
                    listColors[10] = 'rgba(42, 42, 106, 1)';
                    listColors[11] = 'rgba(0, 51, 128, 1)';

                    return listColors;
                }

                , setGradientColorForMonthsType3: function () {
                    var listColors = [];

                    listColors[0] = 'rgba(221, 85, 255, 1)';
                    listColors[1] = 'rgba(207, 77, 247, 1)';
                    listColors[2] = 'rgba(193, 69, 239, 1)';
                    listColors[3] = 'rgba(179, 61, 231, 1)';
                    listColors[4] = 'rgba(165, 54, 224, 1)';
                    listColors[5] = 'rgba(151, 46, 216, 1)';
                    listColors[6] = 'rgba(137, 38, 208, 1)';
                    listColors[7] = 'rgba(123, 30, 200, 1)';
                    listColors[8] = 'rgba(109, 23, 193, 1)';
                    listColors[9] = 'rgba(95, 15, 185, 1)';
                    listColors[10] = 'rgba(81, 7, 177, 1)';
                    listColors[11] = 'rgba(68, 0, 170, 1)';

                    return listColors;
                }

                , getCardTotalAno: function (data) {
                    self.$cardTotalAgendas.html(data.Total);
                }
            }

            Dashboard.init();

        });
    </script>

</asp:Content>
