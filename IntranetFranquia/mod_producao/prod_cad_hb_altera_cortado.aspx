<%@ Page Title="HB Alteração Corte" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="prod_cad_hb_altera_cortado.aspx.cs" Inherits="Relatorios.prod_cad_hb_altera_cortado"
    EnableEventValidation="false" MaintainScrollPositionOnPostback="true" %>

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

        function Calcular(control) {

            var total = 0;
            var _gastoPorCorte = 0;
            var _gastoPorPeca = 0;

            var totalGalao = 0;
            var _gastoPorCorteGalao = 0;
            var _gastoPorPecaGalao = 0;

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

                document.getElementById("MainContent_txtGastoPorCorte").value = _gastoPorCorte;
                document.getElementById("MainContent_txtGastoPorCorte").value = document.getElementById("MainContent_txtGastoPorCorte").value.replace('.', ',');

                if (document.getElementById("MainContent_txtGastoPorCorte").value != "" && document.getElementById("MainContent_txtGastoRetalhos").value != "") {
                    var totalPorPeca = parseFloat(document.getElementById("MainContent_txtGastoPorCorte").value.replace(',', '.')) + parseFloat(document.getElementById("MainContent_txtGastoRetalhos").value.replace(',', '.'));
                    _gastoPorPeca = totalPorPeca / parseFloat(document.getElementById("MainContent_txtGradeTotal_R").value.replace(',', '.'));
                }

                if (document.getElementById("MainContent_ddlUnidade").value == "1") {
                    document.getElementById("MainContent_txtGastoPorPeca").value = (_gastoPorPeca).toFixed(3);
                } else {
                    document.getElementById("MainContent_txtGastoPorPeca").value = _gastoPorPeca.toFixed(3);
                }

                document.getElementById("MainContent_txtGastoPorPeca").value = document.getElementById("MainContent_txtGastoPorPeca").value.replace('.', ',');
            }

            //Calcular Corte Detalhe
            if (document.getElementById("MainContent_txtGradeTotal_R").value != "") {
                if (document.getElementById("MainContent_txtGastoPorFolhaGalao").value != "")
                    _gastoPorCorteGalao = parseFloat(document.getElementById("MainContent_txtGastoPorFolhaGalao").value.replace(',', '.')) * parseFloat(document.getElementById("MainContent_txtGradeTotal_R").value.replace(',', '.'));

                document.getElementById("MainContent_txtGastoPorCorteGalao").value = _gastoPorCorteGalao;
                document.getElementById("MainContent_txtGastoPorCorteGalao").value = document.getElementById("MainContent_txtGastoPorCorteGalao").value.replace('.', ',');

                if (document.getElementById("MainContent_txtGastoPorCorteGalao").value != "" && document.getElementById("MainContent_txtGastoRetalhosGalao").value != "") {
                    var totalPorPecaGalao = parseFloat(document.getElementById("MainContent_txtGastoPorCorteGalao").value.replace(',', '.')) + parseFloat(document.getElementById("MainContent_txtGastoRetalhosGalao").value.replace(',', '.'));
                    _gastoPorPecaGalao = totalPorPecaGalao / parseFloat(document.getElementById("MainContent_txtGradeTotal_R").value.replace(',', '.'));
                }

                /*Quando é metro tem que somar 4% no valor do consumo*/
                if (document.getElementById("MainContent_ddlUnidadeGalao").value == "1") {
                    document.getElementById("MainContent_txtGastoPorPecaGalao").value = (_gastoPorPecaGalao).toFixed(3);
                } else {
                    document.getElementById("MainContent_txtGastoPorPecaGalao").value = _gastoPorPecaGalao.toFixed(3);
                }
                document.getElementById("MainContent_txtGastoPorPecaGalao").value = document.getElementById("MainContent_txtGastoPorPecaGalao").value.replace('.', ',');
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Ordem
                    de Corte&nbsp;&nbsp;>&nbsp;&nbsp;HB Alteração Corte</span>
                <div style="float: right; padding: 0;">
                    <a href="" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login" style="float: left; width: 1000px; margin-left: 16%;">
                <fieldset>
                    <legend>HB Alteração Corte</legend>
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
                                                Width="66px"></asp:TextBox>
                                        </td>
                                        <td style="width: 160px;">
                                            <asp:TextBox ID="txtCodigoLinx" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                                Width="146px"></asp:TextBox>
                                        </td>
                                        <td style="width: 180px;">
                                            <asp:DropDownList ID="ddlGrupo" runat="server" Width="174px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                                DataValueField="GRUPO_PRODUTO">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 260px;">
                                            <asp:TextBox ID="txtNome" runat="server" Width="246px"></asp:TextBox>
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
                                        <td>
                                            <asp:Label ID="labGrupoTecido" runat="server" Text="Tecido Grupo"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="labSubGrupoTecido" runat="server" Text="Tecido SubGrupo"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labCor" runat="server" Text="Cor"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="labDescFornecedor" runat="server" Text="Cor Fornecedor"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="labFornecedor" runat="server" Text="Fornecedor"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 160px;">
                                            <asp:TextBox ID="txtGrupoTecido" runat="server" Width="146px"></asp:TextBox>
                                        </td>
                                        <td style="width: 240px;" colspan="2">
                                            <asp:TextBox ID="txtSubGrupoTecido" runat="server" Width="226px"></asp:TextBox>
                                        </td>
                                        <td style="width: 180px;">
                                            <asp:DropDownList ID="ddlCor" runat="server" Width="174px" Height="21px" DataTextField="DESC_COR"
                                                DataValueField="COR">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 260px;" colspan="2">
                                            <asp:TextBox ID="txtCorFornecedor" runat="server" Width="246px"></asp:TextBox>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtFornecedor" runat="server" Width="127px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="labPedidoNumero" runat="server" Text="No. Pedido"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labModelagem" runat="server" Text="Modelagem"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labLargura" runat="server" Text="Largura"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labCustoTecido" runat="server" Text="Custo Tecido"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labPCVarejo" runat="server" Text="PC Varejo"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labPCAtacado" runat="server" Text="PC Atacado"></asp:Label>
                                        </td>
                                        <td style="text-align: right;">Liquidar
                                        </td>
                                        <td style="text-align: right;">Atacado
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtPedidoNumero" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                                Width="146px"></asp:TextBox>
                                        </td>
                                        <td style="width: 120px;">
                                            <asp:TextBox ID="txtModelagem" runat="server" Width="106px"></asp:TextBox>
                                        </td>
                                        <td style="width: 120px;">
                                            <asp:TextBox ID="txtLargura" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                                Width="106px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCustoTecido" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                                Width="166px"></asp:TextBox>
                                        </td>
                                        <td style="width: 130px;">
                                            <asp:TextBox ID="txtPCVarejo" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                                Width="116px"></asp:TextBox>
                                        </td>
                                        <td style="width: 130px;">
                                            <asp:TextBox ID="txtPCAtacado" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                                Width="116px"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:CheckBox ID="cbLiquidar" runat="server" />
                                        </td>
                                        <td style="text-align: right;">
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
                                <td>&nbsp;<asp:HiddenField ID="hidGradeTotal" runat="server" />
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
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="labGradeEXP_A" runat="server" Text="EXP"></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="labGradeXP_A" runat="server" Text="XP"></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="labGradePP_A" runat="server" Text="PP"></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="labGradeP_A" runat="server" Text="P"></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="labGradeM_A" runat="server" Text="M"></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="labGradeG_A" runat="server" Text="G"></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="labGradeGG_A" runat="server" Text="GG"></asp:Label>
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
                            <asp:Label ID="labGradeCorte" runat="server" Text="Grade - Corte"></asp:Label></legend>
                        <table border="0" width="100%">
                            <tr>
                                <td valign="top" style="width: 300px;">
                                    <table border="0" width="265px" cellspacing="0" cellpadding="0">
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
                                                <table border="0" width="250px">
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
                                        <tr style="line-height: 3px;">
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="labRiscoNome" runat="server" Text="Nome"></asp:Label>
                                            </td>
                                            <td style="padding-left: 3px;">
                                                <asp:TextBox ID="txtRiscoNome" runat="server" MaxLength="20"
                                                    Width="238px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top" style="width: 200px;">
                                    <table border="0" width="200px" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td style="width: 50px;">
                                                <asp:Label ID="labCorteEnfesto" runat="server" Text="Enfesto"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlEnfesto1" runat="server" Width="120px" Height="22px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="line-height: 3px;">
                                            <td colspan="2">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlEnfesto2" runat="server" Width="120px" Height="22px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="line-height: 3px;">
                                            <td colspan="2">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlEnfesto3" runat="server" Width="120px" Height="22px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top" style="width: 200px;">
                                    <table border="0" width="200px" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="labCorteCorte" runat="server" Text="Corte"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCorte1" runat="server" Width="120px" Height="22px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="line-height: 3px;">
                                            <td colspan="2">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCorte2" runat="server" Width="120px" Height="22px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top" style="width: 200px;">
                                    <table border="0" width="200px" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="labCorteAmarrador" runat="server" Text="Amarrador"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlAmarrador1" runat="server" Width="120px" Height="22px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="line-height: 3px;">
                                            <td colspan="2">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlAmarrador2" runat="server" Width="120px" Height="22px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;" colspan="3">
                                    <asp:Label ID="labErroGradeSub" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                                <td style="text-align: right;">
                                    <asp:Button ID="btIncluirCorte" runat="server" Text="Incluir Corte" Width="100px"
                                        OnClick="btIncluirCorte_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
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
                                                            <asp:BoundField DataField="RISCO_NOME" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left" HeaderText="Nome" />
                                                            <asp:BoundField DataField="GRADE" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left" HeaderText="Grade" />
                                                            <asp:BoundField DataField="GASTO" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center" HeaderText="Gasto" />
                                                            <asp:BoundField DataField="FOLHA" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center" HeaderText="Folhas" />
                                                            <asp:BoundField DataField="ENFESTO" HeaderStyle-Width="" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left" HeaderText="Enfesto" />
                                                            <asp:BoundField DataField="CORTE" HeaderStyle-Width="" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left" HeaderText="Corte" />
                                                            <asp:BoundField DataField="AMARRADOR" HeaderStyle-Width="" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left" HeaderText="Amarrador" />
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="55px">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btGradeCorteHBExcluir" runat="server" Height="19px" Text="Excluir"
                                                                        OnClientClick="return ConfirmarExclusao();" OnClick="btGradeCorteHBExcluir_Click" />
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
                                            MaxLength="9" Width="96px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGastoRetalhos" runat="server" CssClass="alinharDireita" onblur="Calcular(this);"
                                            MaxLength="9" Width="96px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGastoPorCorte" runat="server" CssClass="alinharDireita" MaxLength="9"
                                            Width="96px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGastoPorPeca" runat="server" CssClass="alinharDireita" MaxLength="9"
                                            Width="96px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGastoPorPecaCusto" runat="server" CssClass="alinharDireita" MaxLength="9"
                                            Width="96px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
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
                        <fieldset style="margin-top: -10px; padding-top: 0px;">
                            <legend>
                                <asp:Label ID="labAviamentos" runat="server" Text="Aviamentos"></asp:Label></legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>Aviamento Grupo
                                                </td>
                                                <td>Aviamento SubGrupo
                                                </td>
                                                <td>Cor
                                                </td>
                                                <td colspan="2">Cor Fornecedor
                                                </td>
                                            </tr>
                                            <tr style="height: 25px;">
                                                <td style="width: 210px;">
                                                    <asp:DropDownList ID="ddlGrupoAviamento" runat="server" Width="204px" Height="21px"
                                                        DataTextField="GRUPO" DataValueField="CODIGO_GRUPO" OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 250px;">
                                                    <asp:DropDownList ID="ddlSubGrupoAviamento" runat="server" Width="244px" Height="21px"
                                                        DataTextField="SUBGRUPO" DataValueField="CODIGO_SUBGRUPO" OnSelectedIndexChanged="ddlSubGrupo_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 270px;">
                                                    <asp:DropDownList ID="ddlCorAviamento" runat="server" Width="264px" Height="21px" DataTextField="DESC_COR_PRODUTO"
                                                        DataValueField="COR_PRODUTO">
                                                    </asp:DropDownList>
                                                </td>
                                                <td colspan="2">
                                                    <asp:DropDownList ID="ddlCorFornecedorAviamento" runat="server" Width="215px" Height="21px"
                                                        DataTextField="REFER_FABRICANTE" DataValueField="REFER_FABRICANTE">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Quantidade Total
                                                </td>
                                                <td>Quantidade Por Peça
                                                </td>
                                                <td>Descrição
                                                </td>
                                                <td>Etiqueta Composição
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtAviamentoQtde" MaxLength="7" CssClass="alinharDireita" runat="server"
                                                        onkeypress="return fnValidarNumeroDecimal(event);" Width="196px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAviamentoQtdePorPeca" MaxLength="7" CssClass="alinharDireita" runat="server"
                                                        onkeypress="return fnValidarNumeroDecimal(event);" Width="236px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAviamentoDescricao" runat="server" Width="256px" Text="POR PEÇA" MaxLength="50"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlEtiquetaComposicaoAviamento" runat="server" Width="139px" Height="21px">
                                                        <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                                        <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                                        <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="text-align: right;">
                                                    <asp:Button ID="btAviamentoIncluir" runat="server" OnClick="btAviamentoIncluir_Click"
                                                        Text="Incluir" Width="70px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Label ID="labErroAviamento" runat="server" Visible="true" ForeColor="Red" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvAviamento" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvAviamento_RowDataBound" DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litAviamentoColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qtde Total" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labQtde" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Grupo" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left"
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
                                                    <asp:BoundField DataField="DESCRICAO" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left" HeaderText="Descrição" />
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btAviamentoExcluir" runat="server" Height="19px" Text="Excluir" OnClientClick="return ConfirmarExclusao();"
                                                                OnClick="btAviamentoExcluir_Click" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>

                        <%--Alteracao de Rota--%>
                        <fieldset id="fsFaccaoSemRota" runat="server" visible="true" style="margin-top: -1px; padding-top: 3px;">
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
                        <table border="0" width="100%">
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
                        </table>
                        <label>
                            Observação</label>
                        <asp:TextBox ID="txtObservacao" TextMode="MultiLine" ReadOnly="true" runat="server"
                            Width="967px" Height="50px"></asp:TextBox>
                        <hr />
                    </div>
                    <div style="float: right;">
                        <asp:Label ID="labErroEnvio" runat="server" ForeColor="red" Text=""></asp:Label>
                        &nbsp;&nbsp;&nbsp;<asp:Button ID="btSalvar" runat="server" Text="Salvar" OnClick="btSalvar_Click"
                            Width="100px" />
                    </div>
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
                                <asp:Label ID="labConteudoDiv" runat="server" Text="ALTERADO COM SUCESSO"></asp:Label>
                                <br />
                                <br />
                                <asp:Label ID="labAviamento" runat="server" ForeColor="Green" Text="HB COM GRADE ALTERADA. VERIQUE A NECESSIDADE DE COMPRAR AVIAMENTOS EXTRAS"></asp:Label>
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
