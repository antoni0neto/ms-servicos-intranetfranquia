<%@ Page Title="HB Baixar Corte" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="prod_cad_hb_baixa_corte.aspx.cs" Inherits="Relatorios.mod_producao.prod_cad_hb_baixa_corte" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .LinkButton {
            text-decoration: none;
        }

        .divPop {
            font-family: "Trebuchet MS", "Helvetica", "Arial", "Verdana", "sans-serif";
            font-size: 100%;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/js.js"></script>
    <script type="text/javascript">

        function CarregarFCorte() {

            var _linha, _enfesto1, _enfesto2, _enfesto3, _corte1, _corte2, _amarrador1, _amarrador2;

            //Sem tempo para arrumar melhor :)

            _linha = $("#<%=gvGradeCorteHBBaixa.ClientID %>").children("tbody").children("tr"); //BUSCAR LINHAS
            _enfesto1 = _linha.children("td:nth-child(5)");
            _enfesto2 = _linha.children("td:nth-child(6)");
            _enfesto3 = _linha.children("td:nth-child(7)");
            _corte1 = _linha.children("td:nth-child(8)");
            _corte2 = _linha.children("td:nth-child(9)");
            _amarrador1 = _linha.children("td:nth-child(10)");
            _amarrador2 = _linha.children("td:nth-child(11)");

            //LOOP
            var enfesto1;
            _enfesto1.each(function () {
                enfesto1 = $(this).children("select").attr("id");
                CarregarDDL(enfesto1);
            });
            var enfesto2;
            _enfesto2.each(function () {
                enfesto2 = $(this).children("select").attr("id");
                CarregarDDL(enfesto2);
            });
            var enfesto3;
            _enfesto3.each(function () {
                enfesto3 = $(this).children("select").attr("id");
                CarregarDDL(enfesto3);
            });

            var corte1;
            _corte1.each(function () {
                corte1 = $(this).children("select").attr("id");
                CarregarDDL(corte1);
            });
            var corte2;
            _corte2.each(function () {
                corte2 = $(this).children("select").attr("id");
                CarregarDDL(corte2);
            });

            var amarrador1;
            _amarrador1.each(function () {
                amarrador1 = $(this).children("select").attr("id");
                CarregarDDL(amarrador1);
            });
            var amarrador2;
            _amarrador2.each(function () {
                amarrador2 = $(this).children("select").attr("id");
                CarregarDDL(amarrador2);
            });

        }

        function CarregarDDL(objDDL) {
            var _id;
            _id = '#' + objDDL.toString();
            $(_id).empty();

            $.ajax({
                type: "POST",
                url: "prod_cad_hb_baixa_corte.aspx/CarregarCorteFuncionario",
                data: '',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    onPopulated(response, _id);
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }

        function onPopulated(response, _id) {
            PopulateControl(response, $(_id));
        }
        function PopulateControl(list, control) {
            $.each(list.d, function () {
                control.append($("<option></option>").val(this['Text']).html(this['Text']));
            });
        }

        function CalcularFaccao(control) {
            var total = 0;

            if (document.getElementById("MainContent_txtGradeEXP_F").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeEXP_F").value);

            if (document.getElementById("MainContent_txtGradeXP_F").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeXP_F").value);

            if (document.getElementById("MainContent_txtGradePP_F").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradePP_F").value);

            if (document.getElementById("MainContent_txtGradeP_F").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeP_F").value);

            if (document.getElementById("MainContent_txtGradeM_F").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeM_F").value);

            if (document.getElementById("MainContent_txtGradeG_F").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeG_F").value);

            if (document.getElementById("MainContent_txtGradeGG_F").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeGG_F").value);

            document.getElementById("MainContent_txtGradeTotal_F").value = total;
        }

        function Calcular(control) {

            var total = 0;
            var _gastoPorCorte = 0;
            var _gastoPorPeca = 0;

            var totalGalao = 0;
            var _gastoPorCorteGalao = 0;
            var _gastoPorPecaGalao = 0;

            var totalGalaoProprio = 0;
            var _gastoPorCorteGalaoProprio = 0;
            var _gastoPorPecaGalaoProprio = 0;

            if (document.getElementById("MainContent_txtGradeEXP_R").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeEXP_R").value);

            if (document.getElementById("MainContent_txtGradeXP_R").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeXP_R").value);

            if (document.getElementById("MainContent_txtGradePP_R").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradePP_R").value);

            if (document.getElementById("MainContent_txtGradeP_R").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeP_R").value);

            if (document.getElementById("MainContent_txtGradeM_R").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeM_R").value);

            if (document.getElementById("MainContent_txtGradeG_R").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeG_R").value);

            if (document.getElementById("MainContent_txtGradeGG_R").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeGG_R").value);

            document.getElementById("MainContent_txtGradeTotal_R").value = total;


            //Calcular Corte
            if (document.getElementById("MainContent_txtGradeTotal_R").value != "") {
                if (document.getElementById("MainContent_txtGastoPorFolha").value != "")
                    _gastoPorCorte = parseFloat(document.getElementById("MainContent_txtGastoPorFolha").value.replace(',', '.')) * parseFloat(document.getElementById("MainContent_txtGradeTotal_R").value.replace(',', '.'));

                document.getElementById("MainContent_txtGastoPorCorte").value = _gastoPorCorte.toFixed(3);
                document.getElementById("MainContent_txtGastoPorCorte").value = document.getElementById("MainContent_txtGastoPorCorte").value.replace('.', ',');

                if (document.getElementById("MainContent_txtGastoPorCorte").value != "" && document.getElementById("MainContent_txtGastoRetalhos").value != "") {
                    var totalPorPeca = parseFloat(document.getElementById("MainContent_txtGastoPorCorte").value.replace(',', '.')) + parseFloat(document.getElementById("MainContent_txtGastoRetalhos").value.replace(',', '.'));
                    _gastoPorPeca = totalPorPeca / parseFloat(document.getElementById("MainContent_txtGradeTotal_R").value.replace(',', '.'));
                }

                /*Quando é metro, tem que somar 4% do consumo para controle de estoque -- OLD*/
                if (document.getElementById("MainContent_ddlUnidade").value == "1") {
                    document.getElementById("MainContent_txtGastoPorPeca").value = (_gastoPorPeca).toFixed(3);
                } else {
                    document.getElementById("MainContent_txtGastoPorPeca").value = _gastoPorPeca.toFixed(3);
                }


                document.getElementById("MainContent_txtGastoPorPeca").value = document.getElementById("MainContent_txtGastoPorPeca").value.replace('.', ',');
            }

            //Calcular Corte Detalhe Galao
            if (document.getElementById("MainContent_txtGradeTotal_R").value != "" && (document.getElementById("MainContent_txtGastoPorFolhaGalao") != null && document.getElementById("MainContent_txtGastoPorFolhaGalao").value != "")) {

                _gastoPorCorteGalao = parseFloat(document.getElementById("MainContent_txtGastoPorFolhaGalao").value.replace(',', '.')) * parseFloat(document.getElementById("MainContent_txtGradeTotal_R").value.replace(',', '.'));

                document.getElementById("MainContent_txtGastoPorCorteGalao").value = _gastoPorCorteGalao.toFixed(3);
                document.getElementById("MainContent_txtGastoPorCorteGalao").value = document.getElementById("MainContent_txtGastoPorCorteGalao").value.replace('.', ',');

                if (document.getElementById("MainContent_txtGastoPorCorteGalao").value != "" && document.getElementById("MainContent_txtGastoRetalhosGalao").value != "") {
                    var totalPorPecaGalao = parseFloat(document.getElementById("MainContent_txtGastoPorCorteGalao").value.replace(',', '.')) + parseFloat(document.getElementById("MainContent_txtGastoRetalhosGalao").value.replace(',', '.'));
                    _gastoPorPecaGalao = totalPorPecaGalao / parseFloat(document.getElementById("MainContent_txtGradeTotal_R").value.replace(',', '.'));
                }

                /*Quando é metro, tem que somar 4% do consumo para controle de estoque*/
                if (document.getElementById("MainContent_ddlUnidadeGalao").value == "1") {
                    document.getElementById("MainContent_txtGastoPorPecaGalao").value = (_gastoPorPecaGalao).toFixed(3);
                } else {
                    document.getElementById("MainContent_txtGastoPorPecaGalao").value = _gastoPorPecaGalao.toFixed(3);
                }

                document.getElementById("MainContent_txtGastoPorPecaGalao").value = document.getElementById("MainContent_txtGastoPorPecaGalao").value.replace('.', ',');
            }

            //Calcular Corte Detalhe Galao Proprio
            if (document.getElementById("MainContent_txtGradeTotal_R").value != "" && (document.getElementById("MainContent_txtGastoPorFolhaGalaoProprio") != null && document.getElementById("MainContent_txtGastoPorFolhaGalaoProprio").value != "")) {

                _gastoPorCorteGalaoProprio = parseFloat(document.getElementById("MainContent_txtGastoPorFolhaGalaoProprio").value.replace(',', '.')) * parseFloat(document.getElementById("MainContent_txtGradeTotal_R").value.replace(',', '.'));

                document.getElementById("MainContent_txtGastoPorCorteGalaoProprio").value = _gastoPorCorteGalaoProprio.toFixed(3);
                document.getElementById("MainContent_txtGastoPorCorteGalaoProprio").value = document.getElementById("MainContent_txtGastoPorCorteGalaoProprio").value.replace('.', ',');

                if (document.getElementById("MainContent_txtGastoPorCorteGalaoProprio").value != "" && document.getElementById("MainContent_txtGastoRetalhosGalaoProprio").value != "") {
                    var totalPorPecaGalaoProprio = parseFloat(document.getElementById("MainContent_txtGastoPorCorteGalaoProprio").value.replace(',', '.')) + parseFloat(document.getElementById("MainContent_txtGastoRetalhosGalaoProprio").value.replace(',', '.'));
                    _gastoPorPecaGalaoProprio = totalPorPecaGalaoProprio / parseFloat(document.getElementById("MainContent_txtGradeTotal_R").value.replace(',', '.'));
                }

                /*Quando é metro, tem que somar 4% do consumo para controle de estoque -- OLD*/
                if (document.getElementById("MainContent_ddlUnidadeGalaoProprio").value == "1") {
                    document.getElementById("MainContent_txtGastoPorPecaGalaoProprio").value = (_gastoPorPecaGalaoProprio).toFixed(3);
                } else {
                    document.getElementById("MainContent_txtGastoPorPecaGalaoProprio").value = _gastoPorPecaGalaoProprio.toFixed(3);
                }

                document.getElementById("MainContent_txtGastoPorPecaGalaoProprio").value = document.getElementById("MainContent_txtGastoPorPecaGalaoProprio").value.replace('.', ',');
            }
        }

        function SomarGrade(control) {
            var total = 0;

            if (document.getElementById("MainContent_txtGradeEXP_R").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeEXP_R").value);

            if (document.getElementById("MainContent_txtGradeXP_R").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeXP_R").value);

            if (document.getElementById("MainContent_txtGradePP_R").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradePP_R").value);

            if (document.getElementById("MainContent_txtGradeP_R").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeP_R").value);

            if (document.getElementById("MainContent_txtGradeM_R").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeM_R").value);

            if (document.getElementById("MainContent_txtGradeG_R").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeG_R").value);

            if (document.getElementById("MainContent_txtGradeGG_R").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeGG_R").value);

            document.getElementById("MainContent_txtGradeTotal_R").value = total;
        }

        function SomarGradeAtacado(control) {
            var total = 0;

            if (document.getElementById("MainContent_txtGradeEXP_A").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeEXP_A").value);

            if (document.getElementById("MainContent_txtGradeXP_A").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeXP_A").value);

            if (document.getElementById("MainContent_txtGradePP_A").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradePP_A").value);

            if (document.getElementById("MainContent_txtGradeP_A").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeP_A").value);

            if (document.getElementById("MainContent_txtGradeM_A").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeM_A").value);

            if (document.getElementById("MainContent_txtGradeG_A").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeG_A").value);

            if (document.getElementById("MainContent_txtGradeGG_A").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeGG_A").value);

            document.getElementById("MainContent_txtGradeTotal_A").value = total;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Gestão
                    de Ordem de Corte&nbsp;&nbsp;>&nbsp;&nbsp;HB Baixar</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login" style="float: left; width: 1000px; margin-left: 16%;">
                <fieldset>
                    <legend>HB Baixar</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="labColecao" runat="server" Text="Coleção"></asp:Label>
                                            <asp:HiddenField ID="hidHB" runat="server" />
                                            <asp:HiddenField ID="hidData" runat="server" />
                                            <asp:HiddenField ID="hidTela" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="labHB" runat="server" Text="HB"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labCodigoLinx" runat="server" Text="Cód. Produto Linx"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labGrupo" runat="server" Text="Grupo"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labNome" runat="server" Text="Nome"></asp:Label>
                                        </td>
                                        <td>Data
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 160px;">
                                            <asp:DropDownList ID="ddlColecoes" runat="server" Width="154px" Height="21px" DataTextField="DESC_COLECAO"
                                                DataValueField="COLECAO">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 80px;">
                                            <asp:TextBox ID="txtHB" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                                Width="70px"></asp:TextBox>
                                        </td>
                                        <td style="width: 160px;">
                                            <asp:TextBox ID="txtCodigoLinx" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                                Width="150px"></asp:TextBox>
                                        </td>
                                        <td style="width: 180px;">
                                            <asp:DropDownList ID="ddlGrupo" runat="server" Width="174px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                                DataValueField="GRUPO_PRODUTO">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 260px;">
                                            <asp:TextBox ID="txtNome" runat="server" Width="250px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtData" runat="server" CssClass="alinharDireita" ReadOnly="true"
                                                Width="126px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 160px;">
                                            <asp:Label ID="labTecido" runat="server" Text="Tecido"></asp:Label>
                                        </td>
                                        <td style="width: 80px;">
                                            <asp:Label ID="labPedidoNumero" runat="server" Text="No. Pedido"></asp:Label>
                                        </td>
                                        <td style="width: 160px;">
                                            <asp:Label ID="labCor" runat="server" Text="Cor"></asp:Label>
                                        </td>
                                        <td style="width: 180px;">
                                            <asp:Label ID="labFornecedor" runat="server" Text="Fornecedor"></asp:Label>
                                        </td>
                                        <td style="">
                                            <asp:Label ID="labModelagem" runat="server" Text="Modelagem"></asp:Label>
                                        </td>
                                        <td style="">
                                            <asp:Label ID="labLargura" runat="server" Text="Largura"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labCustoTecido" runat="server" Text="Custo Tecido"></asp:Label>
                                        </td>
                                        <td style="text-align: right;">Liquidar
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 160px;">
                                            <asp:TextBox ID="txtTecido" runat="server" Width="150px"></asp:TextBox>
                                        </td>
                                        <td style="width: 80px;">
                                            <asp:TextBox ID="txtPedidoNumero" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                                Width="70px"></asp:TextBox>
                                        </td>
                                        <td style="width: 160px;">
                                            <asp:DropDownList ID="ddlCor" runat="server" Width="154px" Height="21px" DataTextField="DESC_COR"
                                                DataValueField="COR">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 180px;">
                                            <asp:TextBox ID="txtFornecedor" runat="server" Width="170px"></asp:TextBox>
                                        </td>
                                        <td style="width: 170px;">
                                            <asp:TextBox ID="txtModelagem" runat="server" Width="160px"></asp:TextBox>
                                        </td>
                                        <td style="width: 90px;">
                                            <asp:TextBox ID="txtLargura" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                                Width="80px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCustoTecido" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                                Width="70px"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:CheckBox ID="cbLiquidar" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan='2'>Cor Fornecedor</td>
                                        <td>
                                            <asp:Label ID="labVolume" runat="server" Text="Volume"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labGabarito" runat="server" Text="Gabarito"></asp:Label>
                                        </td>
                                        <td colspan='4' style="text-align: right;">Atacado
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan='2'>
                                            <asp:TextBox ID="txtCorFornecedor" runat="server" Width="230px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtVolume" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                                Width="150px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlGabarito" runat="server" Width="174px" Height="21px">
                                                <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                                <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan='4' style="text-align: right;">
                                            <asp:CheckBox ID="cbAtacado" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table border="0" width="100%">
                        <tr>
                            <td valign="top" style="width: 660px;">
                                <fieldset style="margin-top: 0px; padding-top: 0px; width: 650px">
                                    <legend>
                                        <asp:Label ID="labComposicaoTecido" runat="server" Text="Composição Tecido"></asp:Label></legend>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvComposicao" runat="server" Width="649px" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvComposicao_RowDataBound">
                                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litComposicaoColuna" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="QTDE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Quantidade" HeaderStyle-Width="140px" />
                                                            <asp:BoundField DataField="DESCRICAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Descrição" HeaderStyle-Width="250px" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td>&nbsp;
                            </td>
                            <td valign="top">
                                <fieldset style="margin-top: -1px; padding-top: 0;">
                                    <legend>
                                        <asp:Label ID="labFotoTecido" runat="server" Text="Foto Tecido"></asp:Label></legend>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="center" style="text-align: center;">
                                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="" align="center">
                                                            <asp:Image ID="imgFotoTecido" runat="server" Width='150px' Height='200px' BorderColor="Black"
                                                                BorderWidth="0" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                    <fieldset style="margin-top: -10px; padding-top: 0px;">
                        <legend>
                            <asp:Label ID="labGrade" runat="server" Text="Grade"></asp:Label></legend>
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width: 147px;">&nbsp;
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="labGradeEXP" runat="server" Text="EXP"></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="labGradeXP" runat="server" Text="XP"></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="labGradePP" runat="server" Text="PP"></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="labGradeP" runat="server" Text="P"></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="labGradeM" runat="server" Text="M"></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="labGradeG" runat="server" Text="G"></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="labGradeGG" runat="server" Text="GG"></asp:Label>
                                </td>
                                <td style="text-align: center;">Total
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 147px;">Prevista
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeEXP" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="Calcular(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeXP" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="Calcular(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradePP" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="Calcular(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeP" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="Calcular(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeM" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="Calcular(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeG" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="Calcular(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeGG" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="Calcular(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="text-align: center;">
                                    <asp:TextBox ID="txtGradeTotal" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" Width="70px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="line-height: 5px">
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr id="trGradeReal" runat="server">
                                <td style="width: 147px;">
                                    <asp:Label ID="labGradeReal" runat="server" Text="Real"></asp:Label>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeEXP_R" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="Calcular(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeXP_R" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="Calcular(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradePP_R" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="Calcular(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeP_R" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="Calcular(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeM_R" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="Calcular(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeG_R" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="Calcular(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeGG_R" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="Calcular(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="text-align: center;">
                                    <asp:TextBox ID="txtGradeTotal_R" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" Width="70px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="line-height: 5px">
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr id="trGradeFaccao" runat="server">
                                <td style="width: 147px;">
                                    <asp:Label ID="labGradeFaccao" runat="server" Text="Facção"></asp:Label>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeEXP_F" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="CalcularFaccao(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeXP_F" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="CalcularFaccao(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradePP_F" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="CalcularFaccao(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeP_F" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="CalcularFaccao(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeM_F" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="CalcularFaccao(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeG_F" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="CalcularFaccao(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeGG_F" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="CalcularFaccao(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="text-align: center;">
                                    <asp:TextBox ID="txtGradeTotal_F" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" Width="70px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset id="fsGradeAtacado" runat="server" style="margin-top: -10px; padding-top: 0px;"
                        visible="false">
                        <legend>
                            <asp:Label ID="labGradeAtacado" runat="server" Text="Grade Atacado"></asp:Label></legend>
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width: 147px;">&nbsp;
                                </td>
                                <td style="width: 80px; text-align: center;">EXP
                                </td>
                                <td style="width: 80px; text-align: center;">XP
                                </td>
                                <td style="width: 80px; text-align: center;">PP
                                </td>
                                <td style="width: 80px; text-align: center;">P
                                </td>
                                <td style="width: 80px; text-align: center;">M
                                </td>
                                <td style="width: 80px; text-align: center;">G
                                </td>
                                <td style="width: 80px; text-align: center;">GG
                                </td>
                                <td style="text-align: center;">Total
                                </td>
                            </tr>
                            <tr id="tr1" runat="server">
                                <td style="width: 147px;">
                                    <asp:Label ID="labGradeAtacadoReal" runat="server" Text="Real"></asp:Label>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeEXP_A" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="SomarGradeAtacado(this);"
                                        Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeXP_A" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="SomarGradeAtacado(this);"
                                        Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradePP_A" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="SomarGradeAtacado(this);"
                                        Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeP_A" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="SomarGradeAtacado(this);"
                                        Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeM_A" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="SomarGradeAtacado(this);"
                                        Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeG_A" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="SomarGradeAtacado(this);"
                                        Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeGG_A" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="SomarGradeAtacado(this);"
                                        Width="70px"></asp:TextBox>
                                </td>
                                <td style="text-align: center;">
                                    <asp:TextBox ID="txtGradeTotal_A" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" Width="70px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset id="fsRisco" runat="server">
                        <legend>
                            <asp:Label ID="labGradeCorte" runat="server" Text="Grade - Risco"></asp:Label></legend>
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width: 322px;">
                                    <table border="0" width="100%" cellspacing="0" cellpadding="0">
                                        <tr style="line-height: 29px;">
                                            <td>
                                                <asp:Label ID="labCorteRisco" runat="server" Text="Risco"></asp:Label>
                                                <asp:HiddenField ID="hidMostruario" runat="server" />
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <asp:DropDownList ID="ddlRisco" runat="server" Height="21px" Width="242px" DataTextField="Text"
                                                    DataValueField="Value" OnSelectedIndexChanged="ddlRisco_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 75px;">
                                                <asp:Label ID="labCorteGasto" runat="server" Text="Gasto"></asp:Label>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <asp:TextBox ID="txtCorteGasto" runat="server" MaxLength="6" CssClass="alinharDireita"
                                                    onkeypress="return fnValidarNumeroDecimal(event);" Width="238px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="labCorteGrade" runat="server" Text="Grade"></asp:Label>
                                            </td>
                                            <td>
                                                <table border="0" width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList ID="ddlGradeNumero" runat="server" Width="67px" Height="21px">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="ddlGradeLetra" runat="server" Width="66px" Height="21px">
                                                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                                                <asp:ListItem Value="A" Text="A"></asp:ListItem>
                                                                <asp:ListItem Value="B" Text="B"></asp:ListItem>
                                                                <asp:ListItem Value="C" Text="C"></asp:ListItem>
                                                                <asp:ListItem Value="D" Text="D"></asp:ListItem>
                                                                <asp:ListItem Value="E" Text="E"></asp:ListItem>
                                                                <asp:ListItem Value="F" Text="F"></asp:ListItem>
                                                                <asp:ListItem Value="G" Text="G"></asp:ListItem>
                                                                <asp:ListItem Value="H" Text="H"></asp:ListItem>
                                                                <asp:ListItem Value="I" Text="I"></asp:ListItem>
                                                                <asp:ListItem Value="J" Text="J"></asp:ListItem>
                                                                <asp:ListItem Value="K" Text="K"></asp:ListItem>
                                                                <asp:ListItem Value="L" Text="L"></asp:ListItem>
                                                                <asp:ListItem Value="M" Text="M"></asp:ListItem>
                                                                <asp:ListItem Value="N" Text="N"></asp:ListItem>
                                                                <asp:ListItem Value="O" Text="O"></asp:ListItem>
                                                                <asp:ListItem Value="P" Text="P"></asp:ListItem>
                                                                <asp:ListItem Value="Q" Text="Q"></asp:ListItem>
                                                                <asp:ListItem Value="R" Text="R"></asp:ListItem>
                                                                <asp:ListItem Value="S" Text="S"></asp:ListItem>
                                                                <asp:ListItem Value="T" Text="T"></asp:ListItem>
                                                                <asp:ListItem Value="U" Text="U"></asp:ListItem>
                                                                <asp:ListItem Value="V" Text="V"></asp:ListItem>
                                                                <asp:ListItem Value="X" Text="X"></asp:ListItem>
                                                                <asp:ListItem Value="Z" Text="Z"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txtGradeNumeroDetalhe" runat="server" Width="60px" MaxLength="3"
                                                                CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                                            <asp:Button ID="btGradeSubIncluir" runat="server" Text=">>" OnClick="btGradeSubIncluir_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div class="rounded_corners">
                                                                <asp:GridView ID="gvGradeSub" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvGradeSub_RowDataBound">
                                                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="GRADE" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center"
                                                                            ItemStyle-HorizontalAlign="Center" HeaderText="Grade" />
                                                                        <asp:BoundField DataField="LETRA" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center"
                                                                            ItemStyle-HorizontalAlign="Center" HeaderText="Letra" />
                                                                        <asp:BoundField DataField="NUMERO" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center"
                                                                            ItemStyle-HorizontalAlign="Center" HeaderText="Número" />
                                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right">
                                                                            <ItemTemplate>
                                                                                <asp:Button ID="btGradeSubExcluir" runat="server" Height="19px" Text="Excluir" OnClick="btGradeSubExcluir_Click" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="labCorteFolha" runat="server" Text="Folhas"></asp:Label>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <asp:TextBox ID="txtCorteFolha" runat="server" MaxLength="6" CssClass="alinharDireita"
                                                    onkeypress="return fnValidarNumero(event);" Width="238px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-left: 3px;">&nbsp;
                                            </td>
                                            <td style="text-align: right;">
                                                <asp:Button ID="btIncluirCorte" runat="server" Width="101px" Text="Incluir" OnClick="btIncluirCorte_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left;" colspan="2">
                                                <asp:Label ID="labErroGradeSub" runat="server" ForeColor="Red" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 15px;">&nbsp;
                                </td>
                                <td valign="top">
                                    <table border="0" width="100%" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvGradeCorteHB" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvGradeCorteHB_RowDataBound">
                                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Risco" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litDetalhe" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="GRADE" HeaderStyle-Width="180px" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left" HeaderText="Grade" />
                                                            <asp:BoundField DataField="GASTO" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center" HeaderText="Gasto" />
                                                            <asp:BoundField DataField="FOLHA" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center" HeaderText="Folhas" />
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="65px" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btGradeCorteHBExcluir" runat="server" Width="65px" Height="19px"
                                                                        Text="Excluir" OnClientClick="return ConfirmarExclusao();" OnClick="btGradeCorteHBExcluir_Click" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset id="fsCorteBaixa" runat="server">
                        <legend>
                            <asp:Label ID="labGradeCorteBaixa" runat="server" Text="Grade - Corte"></asp:Label></legend>
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td>
                                    <div style="float: right;">
                                        <asp:LinkButton ID="lknCorteCadastrar" Font-Size="Medium" runat="server" Text="Cadastro de Funcionários do Corte"
                                            CssClass="alink" Height="20px" ToolTip="Abrir tela para manutenção de funcionários do Corte"
                                            OnClientClick="fnAbrirTelaCadastro('prod_cad_func_corte.aspx');"></asp:LinkButton>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvGradeCorteHBBaixa" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvGradeCorteHBBaixa_RowDataBound"
                                            DataKeyNames="PROD_HB_CORTE">
                                            <HeaderStyle BackColor="GradientActiveCaption" />
                                            <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="15px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="15px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="rdbGradeCorte" runat="server" Enabled="false" GroupName="gradeCorteHB" Checked="false" OnCheckedChanged="chkGradeCorte_CheckedChanged" AutoPostBack="true" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="GRADE" HeaderStyle-Width="170px" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left" HeaderText="Grade" />
                                                <asp:BoundField DataField="GASTO" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" HeaderText="Gasto" />
                                                <asp:TemplateField HeaderText="Folhas" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="65px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtFolha" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                                            Height="12px" Width="65px" MaxLength="20" OnTextChanged="txtFolha_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Enfesto 1" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlEnfesto1" runat="server" DataTextField="NOME" DataValueField="CODIGO"
                                                            Width="90px">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Enfesto 2" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlEnfesto2" runat="server" DataTextField="NOME" DataValueField="CODIGO"
                                                            Width="90px">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Enfesto 3" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlEnfesto3" runat="server" DataTextField="NOME" DataValueField="CODIGO"
                                                            Width="90px">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Corte 1" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlCorte1" runat="server" DataTextField="NOME" DataValueField="CODIGO"
                                                            Width="90px">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Corte 2" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlCorte2" runat="server" DataTextField="NOME" DataValueField="CODIGO"
                                                            Width="90px">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amarrador 1" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlAmarrador1" runat="server" DataTextField="NOME" DataValueField="CODIGO"
                                                            Width="90px">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amarrador 2" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlAmarrador2" runat="server" DataTextField="NOME" DataValueField="CODIGO"
                                                            Width="90px">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <div id="divBaixaCorte" runat="server">
                        <fieldset style="margin-top: 0px; padding-top: 0px;">
                            <legend>
                                <asp:Label ID="labGasto" runat="server" Text="Gasto"></asp:Label></legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 206px;">
                                        <asp:Label ID="labGastoUnidade" runat="server" Text="Unidade de Medida"></asp:Label>
                                    </td>
                                    <td style="width: 110px;">
                                        <asp:Label ID="labGastoPorFolha" runat="server" Text="Por Folha"></asp:Label>
                                    </td>
                                    <td style="width: 110px;">
                                        <asp:Label ID="labGastoRetalhos" runat="server" Text="Retalhos"></asp:Label>
                                    </td>
                                    <td style="width: 110px;">
                                        <asp:Label ID="labGastoPorCorte" runat="server" Text="Por Corte"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labGastoPorPeca" runat="server" Text="Por Peça"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labGastoPorPecaCusto" runat="server" Text="Por Peça (Custo)"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlUnidade" runat="server" Height="21px" Width="200px" DataTextField="DESCRICAO"
                                            DataValueField="CODIGO" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGastoPorFolha" runat="server" CssClass="alinharDireita" onblur="Calcular(this);"
                                            MaxLength="9" Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGastoRetalhos" runat="server" CssClass="alinharDireita" onblur="Calcular(this);"
                                            MaxLength="9" Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGastoPorCorte" runat="server" CssClass="alinharDireita" MaxLength="9"
                                            Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGastoPorPeca" runat="server" CssClass="alinharDireita" MaxLength="9"
                                            Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGastoPorPecaCusto" runat="server" CssClass="alinharDireita" MaxLength="9"
                                            Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset style="margin-top: -10px; padding-top: 0px;">
                            <legend>Detalhes</legend>
                            <div class="rounded_corners">
                                <asp:GridView ID="gvDetalhe" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvDetalhe_RowDataBound"
                                    DataKeyNames="CODIGO">
                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litDetalheColuna" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="HB" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litHB" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Detalhe" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litDetalheDesc" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unidade de Medida" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlUnidadeMedida" runat="server" Height="21px" Width="150px"
                                                    DataTextField="DESCRICAO" DataValueField="CODIGO">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Por Folha" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGastoPorFolha" runat="server" CssClass="alinharDireita" MaxLength="9"
                                                    Width="100px" onkeypress="return fnValidarNumeroDecimal(event);" OnTextChanged="txtGasto_TextChanged"
                                                    AutoPostBack="true"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Retalhos" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGastoRetalhos" runat="server" CssClass="alinharDireita" MaxLength="9"
                                                    Width="100px" onkeypress="return fnValidarNumeroDecimal(event);" OnTextChanged="txtGasto_TextChanged"
                                                    AutoPostBack="true"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Por Corte" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGastoPorCorte" runat="server" CssClass="alinharDireita" MaxLength="9"
                                                    Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Por Peça" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGastoPorPeca" runat="server" CssClass="alinharDireita" MaxLength="9"
                                                    Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Por Peça (Custo)" HeaderStyle-Width="110px" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGastoPorPecaCusto" runat="server" CssClass="alinharDireita" MaxLength="9"
                                                    Width="110px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>

                        <asp:Panel ID="pnlGastoDetalhe" runat="server" Visible="false">
                            <fieldset style="margin-top: 0px; padding-top: 0px;">
                                <legend>
                                    <asp:Label ID="labGastoGalao" runat="server" Text=""></asp:Label></legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 206px;">
                                            <asp:Label ID="labGastoUnidadeGalao" runat="server" Text="Unidade de Medida"></asp:Label>
                                        </td>
                                        <td style="width: 110px;">
                                            <asp:Label ID="labGastoPorFolhaGalao" runat="server" Text="Por Folha"></asp:Label>
                                        </td>
                                        <td style="width: 110px;">
                                            <asp:Label ID="labGastoRetalhosGalao" runat="server" Text="Retalhos"></asp:Label>
                                        </td>
                                        <td style="width: 110px;">
                                            <asp:Label ID="labCorteGalao" runat="server" Text="Por Corte"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labPecaGalao" runat="server" Text="Por Peça"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labPecaGalaoCusto" runat="server" Text="Por Peça (Custo)"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlUnidadeGalao" runat="server" Height="21px" Width="200px"
                                                DataTextField="DESCRICAO" DataValueField="CODIGO" AutoPostBack="false">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGastoPorFolhaGalao" runat="server" CssClass="alinharDireita"
                                                onblur="Calcular(this);" MaxLength="9" Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGastoRetalhosGalao" runat="server" CssClass="alinharDireita"
                                                onblur="Calcular(this);" MaxLength="9" Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGastoPorCorteGalao" runat="server" CssClass="alinharDireita"
                                                MaxLength="9" Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGastoPorPecaGalao" runat="server" CssClass="alinharDireita" MaxLength="9"
                                                Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGastoPorPecaGalaoCusto" runat="server" CssClass="alinharDireita" MaxLength="9"
                                                Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </asp:Panel>

                        <%--<asp:CheckBox ID="chkBaixaGalaoProprio" runat="server" Checked="false" Text="Baixar também o Galão Próprio" Visible="false" AutoPostBack="true" OnCheckedChanged="chkBaixaGalaoProprio_CheckedChanged" /><br /><br />--%>
                        <asp:Panel ID="pnlGastoDetalheGalaoProprio" runat="server" Visible="false">
                            <fieldset style="margin-top: 0px; padding-top: 0px;">
                                <legend>
                                    <asp:Label ID="labGastoGalaoProprio" runat="server" Text=""></asp:Label></legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 206px;">
                                            <asp:Label ID="labGastoUnidadeGalaoProprio" runat="server" Text="Unidade de Medida"></asp:Label>
                                        </td>
                                        <td style="width: 110px;">
                                            <asp:Label ID="labGastoPorFolhaGalaoProprio" runat="server" Text="Por Folha"></asp:Label>
                                        </td>
                                        <td style="width: 110px;">
                                            <asp:Label ID="labGastoRetalhosGalaoProprio" runat="server" Text="Retalhos"></asp:Label>
                                        </td>
                                        <td style="width: 110px;">
                                            <asp:Label ID="labCorteGalaoProprio" runat="server" Text="Por Corte"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labPecaGalaoProprio" runat="server" Text="Por Peça"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labPecaGalaoCustoProprio" runat="server" Text="Por Peça (Custo)"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlUnidadeGalaoProprio" runat="server" Height="21px" Width="200px"
                                                DataTextField="DESCRICAO" DataValueField="CODIGO" AutoPostBack="false">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGastoPorFolhaGalaoProprio" runat="server" CssClass="alinharDireita"
                                                onblur="Calcular(this);" MaxLength="9" Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGastoRetalhosGalaoProprio" runat="server" CssClass="alinharDireita"
                                                onblur="Calcular(this);" MaxLength="9" Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGastoPorCorteGalaoProprio" runat="server" CssClass="alinharDireita"
                                                MaxLength="9" Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGastoPorPecaGalaoProprio" runat="server" CssClass="alinharDireita" MaxLength="9"
                                                Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGastoPorPecaGalaoCustoProprio" runat="server" CssClass="alinharDireita" MaxLength="9"
                                                Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </asp:Panel>
                        <fieldset style="margin-top: -10px; padding-top: 0px;">
                            <legend>
                                <asp:Label ID="labAviamentos" runat="server" Text="Aviamentos"></asp:Label></legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvAviamento" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvAviamento_RowDataBound"
                                                DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litAviamentoColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qtde" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labQtde" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qtde Extra" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="70px">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtQtdeExtra" runat="server" onkeypress="return fnValidarNumeroDecimalNegativo(event);"
                                                                Height="12px" CssClass="alinharDireita" Text="0" Width="70px" MaxLength="10" OnTextChanged="txtQtdeExtra_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qtde Total" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labQtdeTotal" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Grupo" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litGrupo" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SubGrupo" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litSubGrupo" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cor" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litCor" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Desc Fornecedor" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litCorFornecedor" runat="server" Text='<%# Eval("COR_FORNECEDOR") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qtde Por Peça" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labQtdePorPeca" runat="server" Width="90px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="DESCRICAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Descrição" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td valign="top">
                                    <fieldset style="margin-top: -1px; padding-top: 0;">
                                        <legend>
                                            <asp:Label ID="labFotoPeca" runat="server" Text="Foto Peça"></asp:Label></legend>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="center" style="text-align: center;">
                                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="" align="center">
                                                                <asp:Image ID="imgFotoPeca" runat="server" Width='150px' Height='200px' BorderColor="Black"
                                                                    BorderWidth="0" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <fieldset id="fsFaccaoSemRota" runat="server" visible="false" style="margin-top: -1px; padding-top: 3px;">
                                        <legend>Rotas para o Controle de Facção</legend>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="text-align: center;">Estamparia
                                                </td>
                                                <td style="text-align: center;">Lavanderia
                                                </td>
                                                <td style="text-align: center;">Facção
                                                </td>
                                                <td style="text-align: center;">Acabamento
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center;">
                                                    <asp:CheckBox ID="cbEstamparia" runat="server" Checked="false" />
                                                </td>
                                                <td style="text-align: center;">
                                                    <asp:CheckBox ID="cbLavanderia" runat="server" Checked="false" />
                                                </td>
                                                <td style="text-align: center;">
                                                    <asp:CheckBox ID="cbFaccao" runat="server" Checked="true" />
                                                </td>
                                                <td style="text-align: center;">
                                                    <asp:CheckBox ID="cbAcabamento" runat="server" Checked="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <fieldset id="fsFaccaoComRota" runat="server" visible="false" style="margin-top: -1px; padding-top: 3px;">
                                        <legend>Rotas para o Controle de Facção</legend>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="text-align: center;">Estamparia
                                                </td>
                                                <td style="text-align: center;">Lavanderia
                                                </td>
                                                <td style="text-align: center;">Facção
                                                </td>
                                                <td style="text-align: center;">Acabamento
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center;">
                                                    <asp:Label ID="lblEstamparia" runat="server" ForeColor="blue" Text="NÃO"></asp:Label>
                                                </td>
                                                <td style="text-align: center;">
                                                    <asp:Label ID="lblLavanderia" runat="server" ForeColor="blue" Text="NÃO"></asp:Label>
                                                </td>
                                                <td style="text-align: center;">
                                                    <asp:Label ID="lblFaccao" runat="server" ForeColor="blue" Text="NÃO"></asp:Label>
                                                </td>
                                                <td style="text-align: center;">
                                                    <asp:Label ID="lblAcabamento" runat="server" ForeColor="blue" Text="NÃO"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                        <label>
                            Observação</label>
                        <asp:TextBox ID="txtObservacao" TextMode="MultiLine" ReadOnly="true" runat="server"
                            Width="967px" Height="50px"></asp:TextBox>
                        <hr />
                    </div>
                    <div style="float: right;">
                        <asp:Label ID="labErroEnvio" runat="server" ForeColor="red" Text=""></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btSalvar" runat="server" Text="Salvar" OnClick="btSalvar_Click" Width="100px" OnClientClick="DesabilitarBotao(this);" />
                    </div>
                    <br />
                    <br />
                </fieldset>
            </div>
            <br />
            <br />
            <br />
            <div id="dialogPai" runat="server">
                <div id="dialog" title="Mensagem" class="divPop">
                    <table border="0" width="100%">
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; color: red;">
                                <strong>HB
                                    <asp:Label ID="labHBPopUp" runat="server" Text=""></asp:Label></strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; color: red;">
                                <asp:Literal ID="litConteudoDiv" runat="server" Text="BAIXADO COM SUCESSO"></asp:Literal><br />
                                <br />
                                <asp:Label ID="litAviamento" runat="server" ForeColor="Green" Visible="false" Text="ESTE HB POSSUI AVIAMENTO SEM ORÇAMENTO."></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
