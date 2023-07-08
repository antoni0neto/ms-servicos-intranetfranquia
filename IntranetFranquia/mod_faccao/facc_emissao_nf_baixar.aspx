<%@ Page Title="Emitir Nota" Language="C#" AutoEventWireup="true" CodeBehind="facc_emissao_nf_baixar.aspx.cs"
    Inherits="Relatorios.facc_emissao_nf_baixar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Emitir Nota</title>
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />
    <style type="text/css">
        .fs {
            border: 1px solid #ccc;
            font-family: Calibri;
        }

        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }
    </style>
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="http://code.jquery.com/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
                <div style="margin-top: 10px; padding-left: 12px; padding-right: 12px; padding-bottom: -350px; background-color: White;">
                    <br />
                    <div>
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Facção&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Facção&nbsp;&nbsp;>&nbsp;&nbsp;Emissão Nota Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Emitir Nota</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Emitir Nota</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>Coleção
                                        <asp:HiddenField ID="hidCodigoSaida" runat="server" />
                                        <asp:HiddenField ID="hidTela" runat="server" />
                                    </td>
                                    <td>HB
                                    </td>
                                    <td>Produto
                                    </td>
                                    <td>Nome
                                    </td>
                                    <td>Cor
                                    </td>
                                    <td>Quantidade
                                    </td>
                                    <td>Mostruário
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 185px;">
                                        <asp:TextBox ID="txtColecao" runat="server" Width="175px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 120px;">
                                        <asp:TextBox ID="txtHB" runat="server" Width="110px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 120px;">
                                        <asp:TextBox ID="txtProduto" runat="server" Width="110px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 267px;">
                                        <asp:TextBox ID="txtNome" runat="server" Width="257px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 200px;">
                                        <asp:TextBox ID="txtCor" runat="server" Width="190px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 130px;">
                                        <asp:TextBox ID="txtQtde" runat="server" Width="120px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMostruario" runat="server" Width="118px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">Fornecedor</td>
                                    <td>Setor</td>
                                    <td>Fase</td>
                                    <td>Preço Custo</td>
                                    <td>Preço Produção</td>
                                    <td>Volume</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlFornecedor" runat="server" Width="300px" Height="21px" DataTextField="FORNECEDOR"
                                            DataValueField="FORNECEDOR" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTipo" runat="server" Width="114px" Height="21px" Enabled="false">
                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="I" Text="INTERNO"></asp:ListItem>
                                            <asp:ListItem Value="E" Text="EXTERNO"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlServico" runat="server" Width="261px" Height="21px" DataTextField="DESCRICAO"
                                            DataValueField="CODIGO" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrecoCusto" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                            Width="190px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrecoProducao" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                            Width="120px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVolume" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                            Width="118px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">&nbsp;</td>
                                </tr>

                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="labNF" runat="server" Text="Nota Fiscal"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labSerie" runat="server" Text="Série"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labEmissao" runat="server" Text="Emissão"></asp:Label>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="2">
                                        <asp:DropDownList runat="server" ID="ddlFilial" Height="22px" Width="300px">
                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="000029" Text="CD - LUGZY               "></asp:ListItem>
                                            <asp:ListItem Value="1055  " Text="C-MAX (NOVA)"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtNF" runat="server" CssClass="alinharDireita" MaxLength="9" onkeypress="return fnValidarNumero(event);"
                                            Width="377px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSerie" runat="server" CssClass="alinharDireita" MaxLength="2" onkeypress="return fnValidarNumero(event);"
                                            Width="191px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmissao" runat="server" MaxLength="10" Width="120px"
                                            onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="7">
                                        <fieldset>
                                            <legend>
                                                <asp:Label ID="labGradeTitulo" runat="server" Text="Grade"></asp:Label></legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="text-align: left">&nbsp;
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labGradeEXP" runat="server" Text="EXP"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labGradeXP" runat="server" Text="XP"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labGradePP" runat="server" Text="PP"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labGradeP" runat="server" Text="P"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labGradeM" runat="server" Text="M"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labGradeG" runat="server" Text="G"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labGradeGG" runat="server" Text="GG"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labGradeTotal" runat="server" Text="Total"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 80px;" valign="middle">Emissão
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeEXP_O" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeXP_O" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradePP_O" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeP_O" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeM_O" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeG_O" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeGG_O" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 117px;">
                                                        <asp:TextBox ID="txtGradeTotal_O" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="107px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="text-align: right;">&nbsp;
                                                    </td>
                                                </tr>
                                                <tr style="line-height: 9px;">
                                                    <td colspan="10">&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>


                                <tr>
                                    <td colspan="7">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="7" style="text-align: right;">
                                        <asp:Button ID="btSalvar" runat="server" Width="122px" Text="Baixar" OnClientClick="DesabilitarBotao(this);" OnClick="btSalvar_Click" />&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
