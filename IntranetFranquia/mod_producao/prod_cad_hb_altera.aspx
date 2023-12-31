﻿<%@ Page Title="HB Alteração" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="prod_cad_hb_altera.aspx.cs" Inherits="Relatorios.prod_cad_hb_altera"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .divPop {
            font-family: "Trebuchet MS", "Helvetica", "Arial", "Verdana", "sans-serif";
            font-size: 100 %;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        function SomarGrade(control) {
            var total = 0;

            if (document.getElementById("MainContent_txtGradeEXP").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeEXP").value);

            if (document.getElementById("MainContent_txtGradeXP").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeXP").value);

            if (document.getElementById("MainContent_txtGradePP").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradePP").value);

            if (document.getElementById("MainContent_txtGradeP").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeP").value);

            if (document.getElementById("MainContent_txtGradeM").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeM").value);

            if (document.getElementById("MainContent_txtGradeG").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeG").value);

            if (document.getElementById("MainContent_txtGradeGG").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeGG").value);

            document.getElementById("MainContent_txtGradeTotal").value = total;
        }

        function PreencherGalaoProprio() {
            document.getElementById("MainContent_txtDetalheTecidoGalaoProprio").value = document.getElementById("MainContent_txtTecido").value;
            document.getElementById("MainContent_txtDetalheFornecedorGalaoProprio").value = document.getElementById("MainContent_txtFornecedor").value;
            document.getElementById("MainContent_txtDetalheLarguraGalaoProprio").value = document.getElementById("MainContent_txtLargura").value;
            document.getElementById("MainContent_txtDetalheCustoTecidoGalaoProprio").value = document.getElementById("MainContent_txtCustoTecido").value;
            document.getElementById("MainContent_imgFotoTecidoDetalheGalaoProprio").setAttribute("src", document.getElementById("MainContent_imgFotoTecido").src)
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btFotoTecidoIncluir" />
            <asp:PostBackTrigger ControlID="btFotoPecaIncluir" />
            <asp:PostBackTrigger ControlID="btFotoTecidoDetalheIncluirCor2" />
            <asp:PostBackTrigger ControlID="btFotoTecidoDetalheIncluirCor3" />
            <asp:PostBackTrigger ControlID="btFotoTecidoDetalheIncluirColante" />
            <asp:PostBackTrigger ControlID="btFotoTecidoDetalheIncluirForro" />
            <asp:PostBackTrigger ControlID="btFotoTecidoDetalheIncluirGalao" />
            <asp:PostBackTrigger ControlID="btFotoTecidoDetalheIncluirGalaoProprio" />
        </Triggers>
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Ordem
                    de Corte&nbsp;&nbsp;>&nbsp;&nbsp;HB Alteração</span>
                <div style="float: right; padding: 0; position: ;">
                    <a href="prod_cad_hb_altera_filtro.aspx?d=1" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1000px; margin-left: 15%;">
                <fieldset>
                    <legend>HB Alteração</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 160px;">
                                            <asp:Label ID="labColecao" runat="server" Text="Coleção"></asp:Label>
                                            <asp:HiddenField ID="hidHB" runat="server" />
                                            <asp:HiddenField ID="hidStatus" runat="server" />
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
                                                DataValueField="COLECAO" Enabled="false">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 80px;">
                                            <asp:TextBox ID="txtHB" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                                Width="70px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 160px;">
                                            <asp:TextBox ID="txtCodigoLinx" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                                Width="150px" OnTextChanged="txtCodigoLinx_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td style="width: 180px;">
                                            <asp:DropDownList ID="ddlGrupo" runat="server" Width="174px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                                DataValueField="GRUPO_PRODUTO">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 260px;">
                                            <asp:TextBox ID="txtNome" runat="server" Width="250px" MaxLength="50"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtData" runat="server" CssClass="alinharDireita" ReadOnly="true"
                                                Width="129px"></asp:TextBox>
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
                                        <td>
                                            <asp:Label ID="labCorFornecedor" runat="server" Text="Cor Fornecedor"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="labFornecedor" runat="server" Text="Fornecedor"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 160px;">
                                            <asp:DropDownList ID="ddlGrupoTecido" runat="server" Width="154px" Height="21px" DataTextField="GRUPO"
                                                DataValueField="GRUPO" OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged"
                                                AutoPostBack="true" Enabled="false">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 229px;" colspan="2">
                                            <asp:DropDownList ID="ddlSubGrupoTecido" runat="server" Width="233px" Height="21px" DataTextField="SUBGRUPO"
                                                DataValueField="SUBGRUPO" OnSelectedIndexChanged="ddlSubGrupo_SelectedIndexChanged"
                                                AutoPostBack="true" Enabled="false">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCor" runat="server" Width="173px" Height="21px" DataTextField="DESC_COR_PRODUTO"
                                                DataValueField="COR_PRODUTO" Enabled="false">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 180px;">
                                            <asp:DropDownList ID="ddlCorFornecedor" runat="server" Width="174px" Height="21px"
                                                DataTextField="REFER_FABRICANTE" DataValueField="REFER_FABRICANTE" Enabled="false">
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="ddlFornecedor" runat="server" Width="213px" Height="21px" DataTextField="FORNECEDOR"
                                                DataValueField="FORNECEDOR" Enabled="false">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="labMolde" runat="server" Text="Molde"></asp:Label>
                                        </td>
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
                                        <td style="text-align: right;">Liquidar
                                        </td>
                                        <td style="text-align: right;">Atacado
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtMolde" runat="server" CssClass="alinharDireita"
                                                Width="150px"></asp:TextBox>
                                        </td>
                                        <td style="width: 80px;">
                                            <asp:TextBox ID="txtPedidoNumero" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                                Width="70px" OnTextChanged="txtNumeroPedido_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td style="width: 160px;">
                                            <asp:TextBox ID="txtModelagem" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
                                        </td>
                                        <td style="width: 180px;">
                                            <asp:TextBox ID="txtLargura" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                                Width="169px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCustoTecido" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                                Width="170px"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:CheckBox ID="cbLiquidar" runat="server" Enabled="false" />
                                        </td>
                                        <td style="width: 70px; text-align: right;">
                                            <asp:CheckBox ID="cbAtacado" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="labGastoFolha" runat="server" Text="Gasto por Folha"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="labNomeGrade" runat="server" Text="Grade"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labGabarito" runat="server" Text="Gabarito"></asp:Label>
                                        </td>
                                        <td colspan="">
                                            <asp:Label ID="labEstamparia" runat="server" Text="Estamparia"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:Label ID="labPrecoFaccMostruario" runat="server" Text="Preço Facção Mostruário"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtGastoPorFolha" runat="server" CssClass="alinharDireita"
                                                MaxLength="9" ToolTip="Simulação" Width="150px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                        </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="ddlGrade" runat="server" Width="234px" Height="21px" DataTextField="GRADE"
                                                DataValueField="CODIGO" OnSelectedIndexChanged="ddlGrade_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlGabarito" runat="server" Width="174px" Height="21px">
                                                <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                                <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="">
                                            <asp:DropDownList ID="ddlEstamparia" runat="server" Width="174px" Height="21px">
                                                <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                                <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtPrecoFaccMostruario" runat="server" CssClass="alinharDireita" Text=""
                                                MaxLength="9" ToolTip="Preço de Facção para Mostruário" Width="169px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <fieldset style="margin-top: -1px; padding-top: 0;">
                        <legend>
                            <asp:Label ID="labFotoTecido" runat="server" Text="Foto Tecido"></asp:Label></legend>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="width: 270px; vertical-align: top;">
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="float: left;">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 150px; vertical-align: top;">
                                    <asp:FileUpload ID="uploadFotoTecido" runat="server" Width="300px" /><br />
                                    <br />
                                    <br />
                                    <br />
                                    <br />
                                    <asp:Button ID="btFotoTecidoIncluir" runat="server" OnClick="btFotoTecidoIncluir_Click"
                                        Text=">>" Width="32px" />
                                </td>
                                <td>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="float: right;">
                                                <asp:Image ID="imgFotoTecido" runat="server" Width='150px' Height='200px' BorderColor="Black"
                                                    BorderWidth="0" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <div id="divErroFotoTecido" runat="server" style="text-align: center;">
                                        <asp:Label ID="labErroFotoTecido" runat="server" Visible="true" ForeColor="Red" Text=""></asp:Label>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset style="margin-top: -10px; padding-top: 0px;">
                        <legend>
                            <asp:Label ID="labComposicaoTecido" runat="server" Text="Composição Tecido"></asp:Label></legend>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 146px;">Quantidade
                                                <asp:HiddenField ID="hidTotalComp" runat="server" />
                                            </td>
                                            <td style="width: 260px;">Descrição
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 146px;">
                                                <asp:TextBox ID="txtComposicaoQtde" runat="server" CssClass="alinharDireita" MaxLength="6"
                                                    onkeypress="return fnValidarNumeroDecimal(event);" Width="136px"></asp:TextBox>
                                            </td>
                                            <td style="width: 260px;">
                                                <asp:TextBox ID="txtComposicaoDescricao" runat="server" Width="251px" MaxLength="70"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="btComposicaoIncluir" runat="server" OnClick="btComposicaoIncluir_Click"
                                                    Text="Incluir" Width="116px" />&nbsp;&nbsp;&nbsp;
                                                <asp:Label ID="labErroComposicao" runat="server" Visible="true" ForeColor="Red" Text=""></asp:Label>
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
                                        <asp:GridView ID="gvComposicao" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvComposicao_RowDataBound"
                                            OnDataBound="gvComposicao_DataBound">
                                            <HeaderStyle BackColor="Gainsboro" />
                                            <FooterStyle BackColor="Gainsboro" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litComposicaoColuna" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="QTDE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Quantidade" HeaderStyle-Width="140px" />
                                                <asp:BoundField DataField="DESCRICAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderText="Descrição" HeaderStyle-Width="715px" />
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btComposicaoExcluir" runat="server" Height="19px" Text="Excluir"
                                                            OnClientClick="return ConfirmarExclusao();" OnClick="btComposicaoExcluir_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
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
                                        onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeXP" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradePP" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeP" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeM" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeG" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeGG" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" Width="70px"></asp:TextBox>
                                </td>
                                <td style="text-align: center;">
                                    <asp:TextBox ID="txtGradeTotal" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" Width="70px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <asp:Panel ID="pnlTipo" runat="server" Visible="false">
                        <fieldset style="margin-top: -10px; padding-top: 0px;">
                            <legend>Tipo</legend>
                            <table border="0" width="100%" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cbMostruario" runat="server" Text="Mostruário" />
                                        <asp:RadioButton ID="rdbAmpliacao" runat="server" TextAlign="Right" Text="Ampliação"
                                            GroupName="Tipo" />
                                        <asp:RadioButton ID="rdbRisco" runat="server" TextAlign="Right" Text="Risco" GroupName="Tipo" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </asp:Panel>
                    <label>
                        Observação</label>
                    <asp:TextBox ID="txtObservacao" TextMode="MultiLine" runat="server" Width="967px"
                        Height="50px"></asp:TextBox>
                    <fieldset style="margin-top: 0px; padding-top: 0px;" id="fsDetalhes" runat="server">
                        <legend>Detalhes</legend>
                        <fieldset id="fsCor2" style="margin-top: -0px; padding-top: 0px;" runat="server">
                            <legend><strong>Cor 2</strong></legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="middle" style="float: right;">
                                        <asp:HiddenField ID="hidCor2" runat="server" />
                                        <asp:CheckBox ID="cbCor2Inc" Text="Incluir" TextAlign="Left" runat="server" OnCheckedChanged="cbCor2_CheckedChanged"
                                            AutoPostBack="true" />
                                        <asp:CheckBox ID="cbCor2Alt" Text="Alterar" TextAlign="Left" runat="server" OnCheckedChanged="cbCor2_CheckedChanged"
                                            AutoPostBack="true" />
                                        <asp:CheckBox ID="cbCor2Exc" Text="Excluir" TextAlign="Left" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="labDetGrupoTecidoCor2" runat="server" Text="Tecido Grupo"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetSubGrupoTecidoCor2" runat="server" Text="Tecido SubGrupo"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetCorCor2" runat="server" Text="Cor"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetCorFornecedorCor2" runat="server" Text="Cor Fornecedor"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetFornecedorCor2" runat="server" Text="Fornecedor"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 160px;">
                                                    <asp:DropDownList ID="ddlDetalheGrupoTecidoCor2" runat="server" Width="154px" Height="21px"
                                                        DataTextField="GRUPO" DataValueField="GRUPO" OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 180px;">
                                                    <asp:DropDownList ID="ddlDetalheSubGrupoTecidoCor2" runat="server" Width="174px" Height="21px"
                                                        DataTextField="SUBGRUPO" DataValueField="SUBGRUPO" OnSelectedIndexChanged="ddlSubGrupo_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 160px;">
                                                    <asp:DropDownList ID="ddlDetalheCorCor2" runat="server" Width="154px" Height="21px"
                                                        DataTextField="DESC_COR_PRODUTO" DataValueField="COR_PRODUTO">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 200px;">
                                                    <asp:DropDownList ID="ddlDetalheCorFornecedorCor2" runat="server" Width="194px" Height="21px"
                                                        DataTextField="REFER_FABRICANTE" DataValueField="REFER_FABRICANTE">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlDetalheFornecedorCor2" runat="server" Width="217px" Height="21px"
                                                        DataTextField="FORNECEDOR" DataValueField="FORNECEDOR">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="labDetNumeroPedidoCor2" runat="server" Text="No. Pedido"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetMoldeCor2" runat="server" Text="Molde"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetLarguraCor2" runat="server" Text="Largura"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetCustoTecidoCor2" runat="server" Text="Custo Tecido"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetGastoFolhaCor2" runat="server" Text="Gasto por Folha"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheNumeroPedidoCor2" runat="server" CssClass="alinharDireita"
                                                        MaxLength="8" onkeypress="return fnValidarNumero(event);" Width="150px" OnTextChanged="txtNumeroPedido_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheMoldeCor2" runat="server" Enabled="false" CssClass="alinharDireita"
                                                        MaxLength="8" Width="170px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheLarguraCor2" runat="server" CssClass="alinharDireita"
                                                        MaxLength="8" onkeypress="return fnValidarNumeroDecimal(event);" Width="150px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheCustoTecidoCor2" runat="server" CssClass="alinharDireita"
                                                        MaxLength="8" onkeypress="return fnValidarNumeroDecimal(event);" Width="190px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheGastoPorFolhaCor2" runat="server" CssClass="alinharDireita"
                                                        MaxLength="8" onkeypress="return fnValidarNumeroDecimal(event);" Width="213px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <asp:Label ID="labEtiquetaComposicaoCor2" runat="server" Text="Etiqueta de Composicao"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <asp:DropDownList ID="ddlEtiquetaComposicaoCor2" runat="server" Width="217px" Height="21px">
                                                        <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                                        <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                                        <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <fieldset style="margin-top: 0; padding-top: 0px;">
                                            <legend>
                                                <asp:Label ID="labComposicaoDetalheCor2" runat="server" Text="Composição Tecido Cor 2"></asp:Label></legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="width: 146px;">Quantidade
                                                                    <asp:HiddenField ID="hidDetalheCompTotalCor2" runat="server" />
                                                                </td>
                                                                <td style="width: 260px;">Descrição
                                                                </td>
                                                                <td>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 146px;">
                                                                    <asp:TextBox ID="txtDetalheQtdeComposicaoCor2" runat="server" CssClass="alinharDireita"
                                                                        MaxLength="6" onkeypress="return fnValidarNumeroDecimal(event);" Width="136px"></asp:TextBox>
                                                                </td>
                                                                <td style="width: 260px;">
                                                                    <asp:TextBox ID="txtDetalheDescricaoComposicaoCor2" runat="server" Width="251px"
                                                                        MaxLength="70"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btDetalheComposicaoIncluirCor2" runat="server" OnClick="btDetalheComposicaoIncluirCor2_Click"
                                                                        Text="Incluir" Width="116px" />&nbsp;&nbsp;&nbsp;
                                                                    <asp:Label ID="labErroDetalheComposicaoCor2" runat="server" Visible="true" ForeColor="Red"
                                                                        Text=""></asp:Label>
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
                                                            <asp:GridView ID="gvComposicaoDetalheCor2" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvComposicaoDetalheCor2_RowDataBound"
                                                                OnDataBound="gvComposicaoDetalheCor2_DataBound">
                                                                <HeaderStyle BackColor="Gainsboro" />
                                                                <FooterStyle BackColor="Gainsboro" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litComposicaoColuna" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="QTDE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                        HeaderText="Quantidade" HeaderStyle-Width="140px" />
                                                                    <asp:BoundField DataField="DESCRICAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Descrição" HeaderStyle-Width="715px" />
                                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Button ID="btComposicaoDetalheExcluir" CommandName="COR2" runat="server" Height="19px"
                                                                                Text="Excluir" OnClientClick="return ConfirmarExclusao();" OnClick="btComposicaoDetalheExcluir_Click" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <fieldset style="margin-top: -1px; padding-top: 0;">
                                            <legend>Foto Tecido Cor 2</legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 150px; vertical-align: top;">
                                                        <asp:FileUpload ID="uploadFotoTecidoDetalheCor2" runat="server" Width="300px" /><br />
                                                        <br />
                                                        <br />
                                                        <br />
                                                        <br />
                                                        <asp:Button ID="btFotoTecidoDetalheIncluirCor2" runat="server" OnClick="btFotoTecidoDetalheIncluirCor2_Click"
                                                            Text=">>" Width="32px" />
                                                    </td>
                                                    <td>
                                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="float: right;">
                                                                    <asp:Image ID="imgFotoTecidoDetalheCor2" runat="server" Width='130px' Height='180px'
                                                                        BorderColor="Black" BorderWidth="0" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <div id="divErroFotoTecidoDetalheCor2" runat="server" style="text-align: center;">
                                                            <asp:Label ID="labErroFotoTecidoDetalheCor2" runat="server" Visible="true" ForeColor="Red"
                                                                Text=""></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Observação</label><br />
                                        <asp:TextBox ID="txtDetalheObservacaoCor2" TextMode="MultiLine" runat="server" Width="911px"
                                            Height="50px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <% // COR 3 %>
                        <fieldset id="fsCor3" style="margin-top: -0px; padding-top: 0px;" runat="server">
                            <legend><strong>Cor 3</strong></legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="middle" style="float: right;">
                                        <asp:HiddenField ID="hidCor3" runat="server" />
                                        <asp:CheckBox ID="cbCor3Inc" Text="Incluir" TextAlign="Left" runat="server" OnCheckedChanged="cbCor3_CheckedChanged"
                                            AutoPostBack="true" />
                                        <asp:CheckBox ID="cbCor3Alt" Text="Alterar" TextAlign="Left" runat="server" OnCheckedChanged="cbCor3_CheckedChanged"
                                            AutoPostBack="true" />
                                        <asp:CheckBox ID="cbCor3Exc" Text="Excluir" TextAlign="Left" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="labDetGrupoTecidoCor3" runat="server" Text="Tecido Grupo"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetSubGrupoTecidoCor3" runat="server" Text="Tecido SubGrupo"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetCorCor3" runat="server" Text="Cor"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetCorFornecedorCor3" runat="server" Text="Cor Fornecedor"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetFornecedorCor3" runat="server" Text="Fornecedor"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 160px;">
                                                    <asp:DropDownList ID="ddlDetalheGrupoTecidoCor3" runat="server" Width="154px" Height="21px"
                                                        DataTextField="GRUPO" DataValueField="GRUPO" OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 180px;">
                                                    <asp:DropDownList ID="ddlDetalheSubGrupoTecidoCor3" runat="server" Width="174px" Height="21px"
                                                        DataTextField="SUBGRUPO" DataValueField="SUBGRUPO" OnSelectedIndexChanged="ddlSubGrupo_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 160px;">
                                                    <asp:DropDownList ID="ddlDetalheCorCor3" runat="server" Width="154px" Height="21px"
                                                        DataTextField="DESC_COR_PRODUTO" DataValueField="COR_PRODUTO">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 200px;">
                                                    <asp:DropDownList ID="ddlDetalheCorFornecedorCor3" runat="server" Width="194px" Height="21px"
                                                        DataTextField="REFER_FABRICANTE" DataValueField="REFER_FABRICANTE">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlDetalheFornecedorCor3" runat="server" Width="217px" Height="21px"
                                                        DataTextField="FORNECEDOR" DataValueField="FORNECEDOR">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="labDetNumeroPedidoCor3" runat="server" Text="No. Pedido"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetMoldeCor3" runat="server" Text="Molde"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetLarguraCor3" runat="server" Text="Largura"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetCustoTecidoCor3" runat="server" Text="Custo Tecido"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetGastoFolhaCor3" runat="server" Text="Gasto por Folha"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheNumeroPedidoCor3" runat="server" CssClass="alinharDireita"
                                                        MaxLength="8" onkeypress="return fnValidarNumero(event);" Width="150px" OnTextChanged="txtNumeroPedido_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheMoldeCor3" runat="server" Enabled="false" CssClass="alinharDireita"
                                                        MaxLength="8" Width="170px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheLarguraCor3" runat="server" CssClass="alinharDireita"
                                                        MaxLength="8" onkeypress="return fnValidarNumeroDecimal(event);" Width="150px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheCustoTecidoCor3" runat="server" CssClass="alinharDireita"
                                                        MaxLength="8" onkeypress="return fnValidarNumeroDecimal(event);" Width="190px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheGastoPorFolhaCor3" runat="server" CssClass="alinharDireita"
                                                        MaxLength="8" onkeypress="return fnValidarNumeroDecimal(event);" Width="213px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <asp:Label ID="labEtiquetaComposicaoCor3" runat="server" Text="Etiqueta de Composicao"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <asp:DropDownList ID="ddlEtiquetaComposicaoCor3" runat="server" Width="217px" Height="21px">
                                                        <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                                        <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                                        <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <fieldset style="margin-top: 0; padding-top: 0px;">
                                            <legend>
                                                <asp:Label ID="labComposicaoDetalheCor3" runat="server" Text="Composição Tecido Cor 3"></asp:Label></legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="width: 146px;">Quantidade
                                                                    <asp:HiddenField ID="hidDetalheCompTotalCor3" runat="server" />
                                                                </td>
                                                                <td style="width: 260px;">Descrição
                                                                </td>
                                                                <td>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 146px;">
                                                                    <asp:TextBox ID="txtDetalheQtdeComposicaoCor3" runat="server" CssClass="alinharDireita"
                                                                        MaxLength="6" onkeypress="return fnValidarNumeroDecimal(event);" Width="136px"></asp:TextBox>
                                                                </td>
                                                                <td style="width: 260px;">
                                                                    <asp:TextBox ID="txtDetalheDescricaoComposicaoCor3" runat="server" Width="251px"
                                                                        MaxLength="70"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btDetalheComposicaoIncluirCor3" runat="server" OnClick="btDetalheComposicaoIncluirCor3_Click"
                                                                        Text="Incluir" Width="116px" />&nbsp;&nbsp;&nbsp;
                                                                    <asp:Label ID="labErroDetalheComposicaoCor3" runat="server" Visible="true" ForeColor="Red"
                                                                        Text=""></asp:Label>
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
                                                            <asp:GridView ID="gvComposicaoDetalheCor3" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvComposicaoDetalheCor3_RowDataBound"
                                                                OnDataBound="gvComposicaoDetalheCor3_DataBound">
                                                                <HeaderStyle BackColor="Gainsboro" />
                                                                <FooterStyle BackColor="Gainsboro" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litComposicaoColuna" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="QTDE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                        HeaderText="Quantidade" HeaderStyle-Width="140px" />
                                                                    <asp:BoundField DataField="DESCRICAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Descrição" HeaderStyle-Width="715px" />
                                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Button ID="btComposicaoDetalheExcluir" CommandName="COR3" runat="server" Height="19px"
                                                                                Text="Excluir" OnClientClick="return ConfirmarExclusao();" OnClick="btComposicaoDetalheExcluir_Click" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <fieldset style="margin-top: -1px; padding-top: 0;">
                                            <legend>Foto Tecido Cor 3</legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 150px; vertical-align: top;">
                                                        <asp:FileUpload ID="uploadFotoTecidoDetalheCor3" runat="server" Width="300px" /><br />
                                                        <br />
                                                        <br />
                                                        <br />
                                                        <br />
                                                        <asp:Button ID="btFotoTecidoDetalheIncluirCor3" runat="server" OnClick="btFotoTecidoDetalheIncluirCor3_Click"
                                                            Text=">>" Width="32px" />
                                                    </td>
                                                    <td>
                                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="float: right;">
                                                                    <asp:Image ID="imgFotoTecidoDetalheCor3" runat="server" Width='130px' Height='180px'
                                                                        BorderColor="Black" BorderWidth="0" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <div id="divErroFotoTecidoDetalheCor3" runat="server" style="text-align: center;">
                                                            <asp:Label ID="labErroFotoTecidoDetalheCor3" runat="server" Visible="true" ForeColor="Red"
                                                                Text=""></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Observação</label><br />
                                        <asp:TextBox ID="txtDetalheObservacaoCor3" TextMode="MultiLine" runat="server" Width="911px"
                                            Height="50px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <%//FORRO %>
                        <fieldset id="fsForro" style="margin-top: -0px; padding-top: 0px;" runat="server">
                            <legend><strong>Forro</strong></legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="middle" style="float: right;">
                                        <asp:HiddenField ID="hidForro" runat="server" />
                                        <asp:CheckBox ID="cbForroInc" Text="Incluir" TextAlign="Left" runat="server" OnCheckedChanged="cbForro_CheckedChanged"
                                            AutoPostBack="true" />
                                        <asp:CheckBox ID="cbForroAlt" Text="Alterar" TextAlign="Left" runat="server" OnCheckedChanged="cbForro_CheckedChanged"
                                            AutoPostBack="true" />
                                        <asp:CheckBox ID="cbForroExc" Text="Excluir" TextAlign="Left" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="labDetGrupoTecidoForro" runat="server" Text="Tecido Grupo"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetSubGrupoTecidoForro" runat="server" Text="Tecido SubGrupo"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetCorForro" runat="server" Text="Cor"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetCorFornecedorForro" runat="server" Text="Cor Fornecedor"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetFornecedorForro" runat="server" Text="Fornecedor"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 160px;">
                                                    <asp:DropDownList ID="ddlDetalheGrupoTecidoForro" runat="server" Width="154px" Height="21px"
                                                        DataTextField="GRUPO" DataValueField="GRUPO" OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 180px;">
                                                    <asp:DropDownList ID="ddlDetalheSubGrupoTecidoForro" runat="server" Width="174px" Height="21px"
                                                        DataTextField="SUBGRUPO" DataValueField="SUBGRUPO" OnSelectedIndexChanged="ddlSubGrupo_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 160px;">
                                                    <asp:DropDownList ID="ddlDetalheCorForro" runat="server" Width="154px" Height="21px"
                                                        DataTextField="DESC_COR_PRODUTO" DataValueField="COR_PRODUTO">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 200px;">
                                                    <asp:DropDownList ID="ddlDetalheCorFornecedorForro" runat="server" Width="194px"
                                                        Height="21px" DataTextField="REFER_FABRICANTE" DataValueField="REFER_FABRICANTE">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlDetalheFornecedorForro" runat="server" Width="217px" Height="21px"
                                                        DataTextField="FORNECEDOR" DataValueField="FORNECEDOR">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="labDetNumeroPedidoForro" runat="server" Text="No. Pedido"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetMoldeForro" runat="server" Text="Molde"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetLarguraForro" runat="server" Text="Largura"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetCustoTecidoForro" runat="server" Text="Custo Tecido"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetGastoFolhaForro" runat="server" Text="Gasto por Folha"></asp:Label>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheNumeroPedidoForro" runat="server" CssClass="alinharDireita"
                                                        MaxLength="8" onkeypress="return fnValidarNumero(event);" Width="150px" OnTextChanged="txtNumeroPedido_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheMoldeForro" runat="server" Enabled="false" CssClass="alinharDireita"
                                                        MaxLength="8" Width="170px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheLarguraForro" runat="server" CssClass="alinharDireita"
                                                        MaxLength="8" onkeypress="return fnValidarNumeroDecimal(event);" Width="150px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheCustoTecidoForro" runat="server" CssClass="alinharDireita"
                                                        MaxLength="8" onkeypress="return fnValidarNumeroDecimal(event);" Width="190px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheGastoPorFolhaForro" runat="server" CssClass="alinharDireita"
                                                        MaxLength="8" onkeypress="return fnValidarNumeroDecimal(event);" Width="213px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <asp:Label ID="labEtiquetaComposicaoForro" runat="server" Text="Etiqueta de Composicao"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <asp:DropDownList ID="ddlEtiquetaComposicaoForro" runat="server" Width="217px" Height="21px">
                                                        <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <fieldset style="margin-top: 0; padding-top: 0px;">
                                            <legend>
                                                <asp:Label ID="labComposicaoDetalheForro" runat="server" Text="Composição Tecido Forro"></asp:Label></legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="width: 146px;">Quantidade
                                                                    <asp:HiddenField ID="hidDetalheCompTotalForro" runat="server" />
                                                                </td>
                                                                <td style="width: 260px;">Descrição
                                                                </td>
                                                                <td>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 146px;">
                                                                    <asp:TextBox ID="txtDetalheQtdeComposicaoForro" runat="server" CssClass="alinharDireita"
                                                                        MaxLength="6" onkeypress="return fnValidarNumeroDecimal(event);" Width="136px"></asp:TextBox>
                                                                </td>
                                                                <td style="width: 260px;">
                                                                    <asp:TextBox ID="txtDetalheDescricaoComposicaoForro" runat="server" Width="251px"
                                                                        MaxLength="70"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btDetalheComposicaoIncluirForro" runat="server" OnClick="btDetalheComposicaoIncluirForro_Click"
                                                                        Text="Incluir" Width="116px" />&nbsp;&nbsp;&nbsp;
                                                                    <asp:Label ID="labErroDetalheComposicaoForro" runat="server" Visible="true" ForeColor="Red"
                                                                        Text=""></asp:Label>
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
                                                            <asp:GridView ID="gvComposicaoDetalheForro" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvComposicaoDetalheForro_RowDataBound"
                                                                OnDataBound="gvComposicaoDetalheForro_DataBound">
                                                                <HeaderStyle BackColor="Gainsboro" />
                                                                <FooterStyle BackColor="Gainsboro" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litComposicaoColuna" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="QTDE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                        HeaderText="Quantidade" HeaderStyle-Width="140px" />
                                                                    <asp:BoundField DataField="DESCRICAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Descrição" HeaderStyle-Width="715px" />
                                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Button ID="btComposicaoDetalheExcluir" runat="server" CommandName="FORRO" Height="19px"
                                                                                Text="Excluir" OnClientClick="return ConfirmarExclusao();" OnClick="btComposicaoDetalheExcluir_Click" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <fieldset style="margin-top: -1px; padding-top: 0;">
                                            <legend>Foto Tecido Forro</legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 150px; vertical-align: top;">
                                                        <asp:FileUpload ID="uploadFotoTecidoDetalheForro" runat="server" Width="300px" /><br />
                                                        <br />
                                                        <br />
                                                        <br />
                                                        <br />
                                                        <asp:Button ID="btFotoTecidoDetalheIncluirForro" runat="server" OnClick="btFotoTecidoDetalheIncluirForro_Click"
                                                            Text=">>" Width="32px" />
                                                    </td>
                                                    <td>
                                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="float: right;">
                                                                    <asp:Image ID="imgFotoTecidoDetalheForro" runat="server" Width='130px' Height='180px'
                                                                        BorderColor="Black" BorderWidth="0" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <div id="divErroFotoTecidoDetalheForro" runat="server" style="text-align: center;">
                                                            <asp:Label ID="labErroFotoTecidoDetalheForro" runat="server" Visible="true" ForeColor="Red"
                                                                Text=""></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Observação</label><br />
                                        <asp:TextBox ID="txtDetalheObservacaoForro" TextMode="MultiLine" runat="server" Width="911px"
                                            Height="50px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <%//COLANTE %>
                        <fieldset id="fsColante" style="margin-top: -0px; padding-top: 0px;" runat="server">
                            <legend><strong>Colante</strong></legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="middle" style="float: right;">
                                        <asp:HiddenField ID="hidColante" runat="server" />
                                        <asp:CheckBox ID="cbColanteInc" Text="Incluir" TextAlign="Left" runat="server" OnCheckedChanged="cbColante_CheckedChanged"
                                            AutoPostBack="true" />
                                        <asp:CheckBox ID="cbColanteAlt" Text="Alterar" TextAlign="Left" runat="server" OnCheckedChanged="cbColante_CheckedChanged"
                                            AutoPostBack="true" />
                                        <asp:CheckBox ID="cbColanteExc" Text="Excluir" TextAlign="Left" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="labDetGrupoTecidoColante" runat="server" Text="Tecido Grupo"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetSubGrupoTecidoColante" runat="server" Text="Tecido SubGrupo"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetCorColante" runat="server" Text="Cor"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetCorFornecedorColante" runat="server" Text="Cor Fornecedor"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetFornecedorColante" runat="server" Text="Fornecedor"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 160px;">
                                                    <asp:DropDownList ID="ddlDetalheGrupoTecidoColante" runat="server" Width="154px" Height="21px"
                                                        DataTextField="GRUPO" DataValueField="GRUPO" OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 180px;">
                                                    <asp:DropDownList ID="ddlDetalheSubGrupoTecidoColante" runat="server" Width="174px" Height="21px"
                                                        DataTextField="SUBGRUPO" DataValueField="SUBGRUPO" OnSelectedIndexChanged="ddlSubGrupo_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 160px;">
                                                    <asp:DropDownList ID="ddlDetalheCorColante" runat="server" Width="154px" Height="21px"
                                                        DataTextField="DESC_COR_PRODUTO" DataValueField="COR_PRODUTO">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 200px;">
                                                    <asp:DropDownList ID="ddlDetalheCorFornecedorColante" runat="server" Width="194px"
                                                        Height="21px" DataTextField="REFER_FABRICANTE" DataValueField="REFER_FABRICANTE">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlDetalheFornecedorColante" runat="server" Width="217px" Height="21px"
                                                        DataTextField="FORNECEDOR" DataValueField="FORNECEDOR">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="labDetNumeroPedidoColante" runat="server" Text="No. Pedido"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetMoldeColante" runat="server" Text="Molde"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetLarguraColante" runat="server" Text="Largura"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetCustoTecidoColante" runat="server" Text="Custo Tecido"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetGastoFolhaColante" runat="server" Text="Gasto por Folha"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheNumeroPedidoColante" runat="server" CssClass="alinharDireita"
                                                        MaxLength="8" onkeypress="return fnValidarNumero(event);" Width="150px" OnTextChanged="txtNumeroPedido_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheMoldeColante" runat="server" Enabled="false" CssClass="alinharDireita"
                                                        MaxLength="8" Width="170px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheLarguraColante" runat="server" CssClass="alinharDireita"
                                                        MaxLength="8" onkeypress="return fnValidarNumeroDecimal(event);" Width="150px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheCustoTecidoColante" runat="server" CssClass="alinharDireita"
                                                        MaxLength="8" onkeypress="return fnValidarNumeroDecimal(event);" Width="190px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheGastoPorFolhaColante" runat="server" CssClass="alinharDireita"
                                                        MaxLength="8" onkeypress="return fnValidarNumeroDecimal(event);" Width="213px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <asp:Label ID="labEtiquetaComposicaoColante" runat="server" Text="Etiqueta de Composicao"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <asp:DropDownList ID="ddlEtiquetaComposicaoColante" runat="server" Width="217px" Height="21px">
                                                        <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <fieldset style="margin-top: 0; padding-top: 0px;">
                                            <legend>
                                                <asp:Label ID="labComposicaoDetalheColante" runat="server" Text="Composição Tecido Colante"></asp:Label></legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="width: 146px;">Quantidade
                                                                    <asp:HiddenField ID="hidDetalheCompTotalColante" runat="server" />
                                                                </td>
                                                                <td style="width: 260px;">Descrição
                                                                </td>
                                                                <td>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 146px;">
                                                                    <asp:TextBox ID="txtDetalheQtdeComposicaoColante" runat="server" CssClass="alinharDireita"
                                                                        MaxLength="6" onkeypress="return fnValidarNumeroDecimal(event);" Width="136px"></asp:TextBox>
                                                                </td>
                                                                <td style="width: 260px;">
                                                                    <asp:TextBox ID="txtDetalheDescricaoComposicaoColante" runat="server" Width="251px"
                                                                        MaxLength="70"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btDetalheComposicaoIncluirColante" runat="server" OnClick="btDetalheComposicaoIncluirColante_Click"
                                                                        Text="Incluir" Width="116px" />&nbsp;&nbsp;&nbsp;
                                                                    <asp:Label ID="labErroDetalheComposicaoColante" runat="server" Visible="true" ForeColor="Red"
                                                                        Text=""></asp:Label>
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
                                                            <asp:GridView ID="gvComposicaoDetalheColante" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvComposicaoDetalheColante_RowDataBound"
                                                                OnDataBound="gvComposicaoDetalheColante_DataBound">
                                                                <HeaderStyle BackColor="Gainsboro" />
                                                                <FooterStyle BackColor="Gainsboro" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litComposicaoColuna" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="QTDE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                        HeaderText="Quantidade" HeaderStyle-Width="140px" />
                                                                    <asp:BoundField DataField="DESCRICAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Descrição" HeaderStyle-Width="715px" />
                                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Button ID="btComposicaoDetalheExcluir" runat="server" CommandName="COLANTE"
                                                                                Height="19px" Text="Excluir" OnClientClick="return ConfirmarExclusao();" OnClick="btComposicaoDetalheExcluir_Click" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <fieldset style="margin-top: -1px; padding-top: 0;">
                                            <legend>Foto Tecido Colante</legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 150px; vertical-align: top;">
                                                        <asp:FileUpload ID="uploadFotoTecidoDetalheColante" runat="server" Width="300px" /><br />
                                                        <br />
                                                        <br />
                                                        <br />
                                                        <br />
                                                        <asp:Button ID="btFotoTecidoDetalheIncluirColante" runat="server" OnClick="btFotoTecidoDetalheIncluirColante_Click"
                                                            Text=">>" Width="32px" />
                                                    </td>
                                                    <td>
                                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="float: right;">
                                                                    <asp:Image ID="imgFotoTecidoDetalheColante" runat="server" Width='130px' Height='180px'
                                                                        BorderColor="Black" BorderWidth="0" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <div id="divErroFotoTecidoDetalheColante" runat="server" style="text-align: center;">
                                                            <asp:Label ID="labErroFotoTecidoDetalheColante" runat="server" Visible="true" ForeColor="Red"
                                                                Text=""></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Observação</label><br />
                                        <asp:TextBox ID="txtDetalheObservacaoColante" TextMode="MultiLine" runat="server"
                                            Width="911px" Height="50px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <%//GALÃO %>
                        <fieldset id="fsGalao" style="margin-top: -0px; padding-top: 0px;" runat="server">
                            <legend><strong>Galão</strong></legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="middle" style="float: right;">
                                        <asp:HiddenField ID="hidGalao" runat="server" />
                                        <asp:CheckBox ID="cbGalaoInc" Text="Incluir" TextAlign="Left" runat="server" OnCheckedChanged="cbGalao_CheckedChanged"
                                            AutoPostBack="true" />
                                        <asp:CheckBox ID="cbGalaoAlt" Text="Alterar" TextAlign="Left" runat="server" OnCheckedChanged="cbGalao_CheckedChanged"
                                            AutoPostBack="true" />
                                        <asp:CheckBox ID="cbGalaoExc" Text="Excluir" TextAlign="Left" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="labDetGrupoTecidoGalao" runat="server" Text="Tecido Grupo"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetSubGrupoTecidoGalao" runat="server" Text="Tecido SubGrupo"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetCorGalao" runat="server" Text="Cor"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetCorFornecedorGalao" runat="server" Text="Cor Fornecedor"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetFornecedorGalao" runat="server" Text="Fornecedor"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 160px;">
                                                    <asp:DropDownList ID="ddlDetalheGrupoTecidoGalao" runat="server" Width="154px" Height="21px"
                                                        DataTextField="GRUPO" DataValueField="GRUPO" OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 180px;">
                                                    <asp:DropDownList ID="ddlDetalheSubGrupoTecidoGalao" runat="server" Width="174px" Height="21px"
                                                        DataTextField="SUBGRUPO" DataValueField="SUBGRUPO" OnSelectedIndexChanged="ddlSubGrupo_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 160px;">
                                                    <asp:DropDownList ID="ddlDetalheCorGalao" runat="server" Width="154px" Height="21px"
                                                        DataTextField="DESC_COR_PRODUTO" DataValueField="COR_PRODUTO">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 200px;">
                                                    <asp:DropDownList ID="ddlDetalheCorFornecedorGalao" runat="server" Width="194px"
                                                        Height="21px" DataTextField="REFER_FABRICANTE" DataValueField="REFER_FABRICANTE">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlDetalheFornecedorGalao" runat="server" Width="217px" Height="21px"
                                                        DataTextField="FORNECEDOR" DataValueField="FORNECEDOR">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="labDetNumeroPedidoGalao" runat="server" Text="No. Pedido"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetMoldeGalao" runat="server" Text="Molde"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetLarguraGalao" runat="server" Text="Largura"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetCustoTecidoGalao" runat="server" Text="Custo Tecido"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labDetGastoFolhaGalao" runat="server" Text="Gasto por Folha"></asp:Label>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheNumeroPedidoGalao" runat="server" CssClass="alinharDireita"
                                                        MaxLength="8" onkeypress="return fnValidarNumero(event);" Width="150px" OnTextChanged="txtNumeroPedido_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheMoldeGalao" runat="server" Enabled="false" CssClass="alinharDireita"
                                                        MaxLength="8" Width="170px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheLarguraGalao" runat="server" CssClass="alinharDireita"
                                                        MaxLength="8" onkeypress="return fnValidarNumeroDecimal(event);" Width="150px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheCustoTecidoGalao" runat="server" CssClass="alinharDireita"
                                                        MaxLength="8" onkeypress="return fnValidarNumeroDecimal(event);" Width="190px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetalheGastoPorFolhaGalao" runat="server" CssClass="alinharDireita"
                                                        MaxLength="8" onkeypress="return fnValidarNumeroDecimal(event);" Width="213px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <asp:Label ID="labEtiquetaComposicaoGalao" runat="server" Text="Etiqueta de Composicao"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <asp:DropDownList ID="ddlEtiquetaComposicaoGalao" runat="server" Width="217px" Height="21px">
                                                        <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <fieldset style="margin-top: 0; padding-top: 0px;">
                                            <legend>
                                                <asp:Label ID="labComposicaoDetalheGalao" runat="server" Text="Composição Tecido Galão"></asp:Label></legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="width: 146px;">Quantidade
                                                                    <asp:HiddenField ID="hidDetalheCompTotalGalao" runat="server" />
                                                                </td>
                                                                <td style="width: 260px;">Descrição
                                                                </td>
                                                                <td>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 146px;">
                                                                    <asp:TextBox ID="txtDetalheQtdeComposicaoGalao" runat="server" CssClass="alinharDireita"
                                                                        MaxLength="6" onkeypress="return fnValidarNumeroDecimal(event);" Width="136px"></asp:TextBox>
                                                                </td>
                                                                <td style="width: 260px;">
                                                                    <asp:TextBox ID="txtDetalheDescricaoComposicaoGalao" runat="server" Width="251px"
                                                                        MaxLength="70"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btDetalheComposicaoIncluirGalao" runat="server" OnClick="btDetalheComposicaoIncluirGalao_Click"
                                                                        Text="Incluir" Width="116px" />&nbsp;&nbsp;&nbsp;
                                                                    <asp:Label ID="labErroDetalheComposicaoGalao" runat="server" Visible="true" ForeColor="Red"
                                                                        Text=""></asp:Label>
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
                                                            <asp:GridView ID="gvComposicaoDetalheGalao" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvComposicaoDetalheGalao_RowDataBound"
                                                                OnDataBound="gvComposicaoDetalheGalao_DataBound">
                                                                <HeaderStyle BackColor="Gainsboro" />
                                                                <FooterStyle BackColor="Gainsboro" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litComposicaoColuna" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="QTDE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                        HeaderText="Quantidade" HeaderStyle-Width="140px" />
                                                                    <asp:BoundField DataField="DESCRICAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Descrição" HeaderStyle-Width="715px" />
                                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Button ID="btComposicaoDetalheExcluir" runat="server" CommandName="GALAO" Height="19px"
                                                                                Text="Excluir" OnClientClick="return ConfirmarExclusao();" OnClick="btComposicaoDetalheExcluir_Click" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <fieldset style="margin-top: -1px; padding-top: 0;">
                                            <legend>Foto Tecido Galão</legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 150px; vertical-align: top;">
                                                        <asp:FileUpload ID="uploadFotoTecidoDetalheGalao" runat="server" Width="300px" /><br />
                                                        <br />
                                                        <br />
                                                        <br />
                                                        <br />
                                                        <asp:Button ID="btFotoTecidoDetalheIncluirGalao" runat="server" OnClick="btFotoTecidoDetalheIncluirGalao_Click"
                                                            Text=">>" Width="32px" />
                                                    </td>
                                                    <td>
                                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="float: right;">
                                                                    <asp:Image ID="imgFotoTecidoDetalheGalao" runat="server" Width='130px' Height='180px'
                                                                        BorderColor="Black" BorderWidth="0" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <div id="divErroFotoTecidoDetalheGalao" runat="server" style="text-align: center;">
                                                            <asp:Label ID="labErroFotoTecidoDetalheGalao" runat="server" Visible="true" ForeColor="Red"
                                                                Text=""></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Observação</label><br />
                                        <asp:TextBox ID="txtDetalheObservacaoGalao" TextMode="MultiLine" runat="server" Width="911px"
                                            Height="50px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <%//GALÃO PROPRIO %>
                        <fieldset id="fsGalaoProprio" style="margin-top: -0px; padding-top: 0px;" runat="server">
                            <legend><strong>Galão Próprio</strong></legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="middle" style="float: right;">
                                        <asp:HiddenField ID="hidGalaoProprio" runat="server" />
                                        <asp:CheckBox ID="cbGalaoProprioInc" Text="Incluir" TextAlign="Left" runat="server"
                                            OnCheckedChanged="cbGalaoProprio_CheckedChanged" AutoPostBack="true" />
                                        <asp:CheckBox ID="cbGalaoProprioAlt" Text="Alterar" TextAlign="Left" runat="server"
                                            OnCheckedChanged="cbGalaoProprio_CheckedChanged" AutoPostBack="true" />
                                        <asp:CheckBox ID="cbGalaoProprioExc" Text="Excluir" TextAlign="Left" runat="server" />
                                    </td>
                                </tr>
                                <div id="divEscondeGalaoProprio" runat="server">
                                    <tr>
                                        <td>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="labDetGrupoTecidoGalaoProprio" runat="server" Text="Tecido Grupo"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="labDetSubGrupoTecidoGalaoProprio" runat="server" Text="Tecido SubGrupo"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="labDetCorGalaoProprio" runat="server" Text="Cor"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="labDetCorFornecedorGalaoProprio" runat="server" Text="Cor Fornecedor"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="labDetFornecedorGalaoProprio" runat="server" Text="Fornecedor"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 160px;">
                                                        <asp:DropDownList ID="ddlDetalheGrupoTecidoGalaoProprio" runat="server" Width="154px" Height="21px"
                                                            DataTextField="GRUPO" DataValueField="GRUPO">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 180px;">
                                                        <asp:DropDownList ID="ddlDetalheSubGrupoTecidoGalaoProprio" runat="server" Width="174px"
                                                            Height="21px" DataTextField="SUBGRUPO" DataValueField="SUBGRUPO">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 160px;">
                                                        <asp:DropDownList ID="ddlDetalheCorGalaoProprio" runat="server" Width="154px" Height="21px"
                                                            DataTextField="DESC_COR_PRODUTO" DataValueField="COR_PRODUTO">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 200px;">
                                                        <asp:DropDownList ID="ddlDetalheCorFornecedorGalaoProprio" runat="server" Width="194px"
                                                            Height="21px" DataTextField="REFER_FABRICANTE" DataValueField="REFER_FABRICANTE">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlDetalheFornecedorGalaoProprio" runat="server" Width="217px"
                                                            Height="21px" DataTextField="FORNECEDOR" DataValueField="FORNECEDOR">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="labDetNumeroPedidoGalaoProprio" runat="server" Text="No. Pedido"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="labDetMoldeGalaoProprio" runat="server" Text="Molde"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="labDetLarguraGalaoProprio" runat="server" Text="Largura"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Label ID="labDetCustoTecidoGalaoProprio" runat="server" Text="Custo Tecido"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtDetalheNumeroPedidoGalaoProprio" runat="server" CssClass="alinharDireita"
                                                            MaxLength="8" onkeypress="return fnValidarNumero(event);" Width="150px" OnTextChanged="txtNumeroPedido_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDetalheMoldeGalaoProprio" runat="server" Enabled="false" CssClass="alinharDireita"
                                                            MaxLength="8" Width="170px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDetalheLarguraGalaoProprio" runat="server" CssClass="alinharDireita"
                                                            MaxLength="8" onkeypress="return fnValidarNumeroDecimal(event);" Width="150px"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtDetalheCustoTecidoGalaoProprio" runat="server" CssClass="alinharDireita"
                                                            MaxLength="8" onkeypress="return fnValidarNumeroDecimal(event);" Width="190px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5">
                                                        <asp:Label ID="labEtiquetaComposicaoGalaoProprio" runat="server" Text="Etiqueta de Composicao"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5">
                                                        <asp:DropDownList ID="ddlEtiquetaComposicaoGalaoProprio" runat="server" Width="217px" Height="21px">
                                                            <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5">&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <fieldset style="margin-top: 0; padding-top: 0px;">
                                                <legend>
                                                    <asp:Label ID="labComposicaoDetalheGalaoProprio" runat="server" Text="Composição Tecido Galão Próprio"></asp:Label></legend>
                                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td style="width: 146px;">Quantidade
                                                                        <asp:HiddenField ID="hidDetalheCompTotalGalaoProprio" runat="server" />
                                                                    </td>
                                                                    <td style="width: 260px;">Descrição
                                                                    </td>
                                                                    <td>&nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 146px;">
                                                                        <asp:TextBox ID="txtDetalheQtdeComposicaoGalaoProprio" runat="server" Enabled="false"
                                                                            CssClass="alinharDireita" MaxLength="6" onkeypress="return fnValidarNumeroDecimal(event);"
                                                                            Width="136px"></asp:TextBox>
                                                                    </td>
                                                                    <td style="width: 260px;">
                                                                        <asp:TextBox ID="txtDetalheDescricaoComposicaoGalaoProprio" runat="server" Enabled="false"
                                                                            Width="251px" MaxLength="70"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btDetalheComposicaoIncluirGalaoProprio" runat="server" Enabled="false"
                                                                            OnClick="btDetalheComposicaoIncluirGalaoProprio_Click" Text="Incluir" Width="116px" />&nbsp;&nbsp;&nbsp;
                                                                        <asp:Label ID="labErroDetalheComposicaoGalaoProprio" runat="server" Visible="true"
                                                                            ForeColor="Red" Text=""></asp:Label>
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
                                                                <asp:GridView ID="gvComposicaoDetalheGalaoProprio" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                    ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvComposicaoDetalheGalaoProprio_RowDataBound"
                                                                    OnDataBound="gvComposicaoDetalheGalaoProprio_DataBound">
                                                                    <HeaderStyle BackColor="Gainsboro" />
                                                                    <FooterStyle BackColor="Gainsboro" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="litComposicaoColuna" runat="server"></asp:Literal>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="QTDE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                            HeaderText="Quantidade" HeaderStyle-Width="140px" />
                                                                        <asp:BoundField DataField="DESCRICAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                            HeaderText="Descrição" HeaderStyle-Width="715px" />
                                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Button ID="btComposicaoDetalheExcluir" runat="server" Enabled="false" CommandName="GALAOPROPRIO"
                                                                                    Height="19px" Text="Excluir" OnClientClick="return ConfirmarExclusao();" OnClick="btComposicaoDetalheExcluir_Click" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <fieldset style="margin-top: -1px; padding-top: 0;">
                                                <legend>Foto Tecido Galão Próprio</legend>
                                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 150px; vertical-align: top;">
                                                            <asp:FileUpload ID="uploadFotoTecidoDetalheGalaoProprio" runat="server" Enabled="false"
                                                                Width="300px" /><br />
                                                            <br />
                                                            <br />
                                                            <br />
                                                            <br />
                                                            <asp:Button ID="btFotoTecidoDetalheIncluirGalaoProprio" runat="server" Enabled="false"
                                                                OnClick="btFotoTecidoDetalheIncluirGalaoProprio_Click" Text=">>" Width="32px" />
                                                        </td>
                                                        <td>
                                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td style="float: right;">
                                                                        <asp:Image ID="imgFotoTecidoDetalheGalaoProprio" Width='130px' Height='180px' runat="server"
                                                                            BorderColor="Black" BorderWidth="0" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <div id="divErroFotoTecidoDetalheGalaoProprio" runat="server" style="text-align: center;">
                                                                <asp:Label ID="labErroFotoTecidoDetalheGalaoProprio" runat="server" Visible="true"
                                                                    ForeColor="Red" Text=""></asp:Label>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </div>
                                <tr>
                                    <td>
                                        <asp:Label ID="labDetGastoFolhaGalaoProprio" runat="server" Text="Gasto por Folha"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtDetalheGastoPorFolhaGalaoProprio" runat="server" CssClass="alinharDireita"
                                            MaxLength="8" onkeypress="return fnValidarNumeroDecimal(event);" Width="213px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Observação</label><br />
                                        <asp:TextBox ID="txtDetalheObservacaoGalaoProprio" TextMode="MultiLine" runat="server"
                                            Width="911px" Height="50px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </fieldset>
                    <fieldset style="margin-top: 0px; padding-top: 0px;">
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
                                            <td>Etiqueta de Composição
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtAviamentoQtde" MaxLength="7" CssClass="alinharDireita" runat="server"
                                                    onkeypress="return fnValidarNumeroDecimal(event);" Width="200px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAviamentoQtdePorPeca" MaxLength="7" CssClass="alinharDireita" runat="server"
                                                    onkeypress="return fnValidarNumeroDecimal(event);" Width="240px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAviamentoDescricao" runat="server" Width="260px" Text="POR PEÇA" MaxLength="50"></asp:TextBox>
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
                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvAviamento_RowDataBound">
                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
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
                    <fieldset style="margin-top: -1px; padding-top: 0;">
                        <legend>
                            <asp:Label ID="labFotoPeca" runat="server" Text="Foto Peça"></asp:Label></legend>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="width: 270px; vertical-align: top;">
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="float: left;">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 300px; vertical-align: top;">
                                    <asp:FileUpload ID="uploadFotoPeca" runat="server" Width="300px" /><br />
                                    <br />
                                    <br />
                                    <br />
                                    <br />
                                    <asp:Button ID="btFotoPecaIncluir" runat="server" OnClick="btFotoPecaIncluir_Click"
                                        Text=">>" Width="32px" />
                                </td>
                                <td>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="float: right;">
                                                <asp:Image ID="imgFotoPeca" runat="server" Width='150px' Height='200px' BorderColor="Black"
                                                    BorderWidth="0" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <div id="divErroFotoPeca" runat="server" style="text-align: center;">
                                        <asp:Label ID="labErroFotoPeca" runat="server" Visible="true" ForeColor="Red" Text=""></asp:Label>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <hr />
                    <div>
                        <div style="float: left;">
                            &nbsp;&nbsp;&nbsp;<asp:Button ID="btExcluir" runat="server" Text="Excluir HB" OnClientClick="return ConfirmarExclusao();"
                                OnClick="btExcluir_Click" Width="100px" />
                        </div>
                        <div style="float: right;">
                            <asp:Label ID="labErroEnvio" runat="server" ForeColor="red" Text=""></asp:Label>
                            &nbsp;&nbsp;&nbsp;<asp:Button ID="btEnviar" runat="server" Text="Salvar HB" OnClick="btEnviar_Click"
                                Width="100px" OnClientClick="DesabilitarBotao(this);" />
                        </div>
                    </div>
                </fieldset>
            </div>
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
                                <asp:Label ID="labMensagem" runat="server" Text="ENVIADO COM SUCESSO"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>Por favor, selecione a tela desejada nos botões abaixo.
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
