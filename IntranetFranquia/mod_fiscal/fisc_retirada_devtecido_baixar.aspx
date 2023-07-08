<%@ Page Title="Tecidos Devolvidos" Language="C#" AutoEventWireup="true" CodeBehind="fisc_retirada_devtecido_baixar.aspx.cs"
    Inherits="Relatorios.fisc_retirada_devtecido_baixar" %>

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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Controle Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Baixa&nbsp;&nbsp;>&nbsp;&nbsp;Tecidos Devolvidos</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Tecidos Devolvidos</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>Fornecedor
                                        <asp:HiddenField ID="hidCodigoPedidoQtde" runat="server" />
                                    </td>
                                    <td>CNPJ
                                    </td>
                                    <td>NF Origem
                                    </td>
                                    <td>Transportadora
                                    </td>
                                    <td>Frete
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 250px;">
                                        <asp:TextBox ID="txtFornecedor" runat="server" Width="240px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 200px;">
                                        <asp:TextBox ID="txtCNPJ" runat="server" Width="190px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 200px;">
                                        <asp:TextBox ID="txtNFOrigem" runat="server" Width="190px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 300px;">
                                        <asp:TextBox ID="txtTransportadora" runat="server" Width="290px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFrete" runat="server" Width="194px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Tecido</td>
                                    <td>Cor</td>
                                    <td>Cor Fornecedor</td>
                                    <td>Produto Fornecedor</td>
                                    <td>Natureza</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtTecido" runat="server" Width="240px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCor" runat="server" Width="190px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCorFornecedor" runat="server" Width="190px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtProdutoFornecedor" runat="server" Width="290px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNatureza" runat="server" Width="194px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Quantidade</td>
                                    <td>Preço do Produto</td>
                                    <td>Volume</td>
                                    <td>Peso Líquido</td>
                                    <td>Peso Bruto</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtQtde" runat="server" Width="240px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPreco" runat="server" Width="190px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVolume" runat="server" Width="190px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPesoLiq" runat="server" Width="290px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPesoBruto" runat="server" Width="194px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labNF" runat="server" Text="Nota Fiscal"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labSerie" runat="server" Text="Série"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labEmissao" runat="server" Text="Emissão"></asp:Label>
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="2">
                                        <asp:DropDownList runat="server" ID="ddlFilial" Height="22px" Width="444px" Enabled="false">
                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="000029" Text="CD - LUGZY               "></asp:ListItem>
                                            <asp:ListItem Value="1055  " Text="C-MAX (NOVA)"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNF" runat="server" CssClass="alinharDireita" MaxLength="9" onkeypress="return fnValidarNumero(event);" Enabled="false"
                                            Width="190px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSerie" runat="server" CssClass="alinharDireita" MaxLength="2" onkeypress="return fnValidarNumero(event);" Enabled="false"
                                            Width="290px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmissao" runat="server" MaxLength="10" Width="194px" Enabled="false"
                                            onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">&nbsp;</td>
                                    <td>Data de Devolução</td>
                                </tr>
                                <tr>
                                    <td colspan="4">&nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="txtDataRetirada" runat="server" MaxLength="10" Width="194px" Enabled="false"
                                            onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="5" style="text-align: right;">
                                        <asp:Button ID="btSalvar" runat="server" Width="122px" Text="Baixar" OnClientClick="DesabilitarBotao(this);" OnClick="btSalvar_Click" />&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
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
