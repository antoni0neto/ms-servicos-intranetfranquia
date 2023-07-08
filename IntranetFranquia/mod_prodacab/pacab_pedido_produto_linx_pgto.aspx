<%@ Page Title="Pagamento - Pedido Linx" Language="C#" AutoEventWireup="true" CodeBehind="pacab_pedido_produto_linx_pgto.aspx.cs"
    Inherits="Relatorios.pacab_pedido_produto_linx_pgto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Gerar Pedido</title>
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Produto Acabado&nbsp;&nbsp;>&nbsp;&nbsp;Produto&nbsp;&nbsp;>&nbsp;&nbsp;Pedido&nbsp;&nbsp;>&nbsp;&nbsp;Pagamento - Pedido Linx</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Pagamento - Pedido Produto</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>Total de Parcelas
                                        <asp:HiddenField ID="hidPedidoIntra" runat="server" />
                                        <asp:HiddenField ID="hidProduto" runat="server" />
                                        <asp:HiddenField ID="hidCor" runat="server" />
                                        <asp:HiddenField ID="hidEntrega" runat="server" />
                                    </td>
                                    <td>Entrega Inicial
                                    </td>
                                    <td>Previsão Final
                                    </td>
                                    <td>Valor Pedido
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width: 170px">
                                        <asp:DropDownList ID="ddlParcela" runat="server" Width="164px" Height="21px" OnSelectedIndexChanged="ddlParcela_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="0" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="1x"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="2x"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="3x"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="4x"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="5x"></asp:ListItem>
                                            <asp:ListItem Value="6" Text="6x"></asp:ListItem>
                                            <asp:ListItem Value="7" Text="7x"></asp:ListItem>
                                            <asp:ListItem Value="8" Text="8x"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 180px">
                                        <asp:TextBox ID="txtEntregaIni" runat="server" MaxLength="10" Width="170px" onkeypress="return fnValidarData(event);" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 150px">
                                        <asp:TextBox ID="txtPrevFinal" runat="server" MaxLength="10" Width="140px" onkeypress="return fnValidarData(event);" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 150px">
                                        <asp:TextBox ID="txtValor" runat="server" MaxLength="10" Width="140px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>Número da Parcela</td>
                                                <td>Data de Pagamento</td>
                                                <td>Porcentagem (%)</td>
                                                <td>Valor (R$)</td>
                                                <td style="text-align: center;">Pago</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="width: 170px;">
                                                    <asp:TextBox ID="txtParc1" runat="server" Width="160px" Enabled="false" Text="" CssClass="alinharCentro"></asp:TextBox>
                                                </td>
                                                <td style="width: 180px;">
                                                    <asp:TextBox ID="txtPgto1" runat="server" MaxLength="10" Width="170px" onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                                </td>
                                                <td style="width: 150px;">
                                                    <asp:TextBox ID="txtPorc1" runat="server" Width="140px" MaxLength="6" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" OnTextChanged="txtPorc_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </td>
                                                <td style="width: 150px;">
                                                    <asp:TextBox ID="txtVal1" runat="server" Width="140px" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" OnTextChanged="txtVal_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </td>
                                                <td style="width: 100px; text-align: center">
                                                    <asp:CheckBox ID="cbPago1" runat="server" Checked="false" Width="50px" OnCheckedChanged="cbPago_CheckedChanged" AutoPostBack="true" />
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="line-height: 6px;" colspan="6">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtParc2" runat="server" Width="160px" Enabled="false" Text="" CssClass="alinharCentro"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPgto2" runat="server" MaxLength="10" Width="170px" onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPorc2" runat="server" Width="140px" MaxLength="6" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" OnTextChanged="txtPorc_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtVal2" runat="server" Width="140px" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" OnTextChanged="txtVal_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:CheckBox ID="cbPago2" runat="server" Checked="false" OnCheckedChanged="cbPago_CheckedChanged" AutoPostBack="true" />
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="line-height: 6px;" colspan="6">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtParc3" runat="server" Width="160px" Enabled="false" Text="" CssClass="alinharCentro"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPgto3" runat="server" MaxLength="10" Width="170px" onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPorc3" runat="server" Width="140px" MaxLength="6" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" OnTextChanged="txtPorc_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtVal3" runat="server" Width="140px" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" OnTextChanged="txtVal_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:CheckBox ID="cbPago3" runat="server" Checked="false" OnCheckedChanged="cbPago_CheckedChanged" AutoPostBack="true" />
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="line-height: 6px;" colspan="6">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtParc4" runat="server" Width="160px" Enabled="false" Text="" CssClass="alinharCentro"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPgto4" runat="server" MaxLength="10" Width="170px" onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPorc4" runat="server" Width="140px" MaxLength="6" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" OnTextChanged="txtPorc_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtVal4" runat="server" Width="140px" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" OnTextChanged="txtVal_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:CheckBox ID="cbPago4" runat="server" Checked="false" OnCheckedChanged="cbPago_CheckedChanged" AutoPostBack="true" />
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="line-height: 6px;" colspan="6">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtParc5" runat="server" Width="160px" Enabled="false" Text="" CssClass="alinharCentro"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPgto5" runat="server" MaxLength="10" Width="170px" onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPorc5" runat="server" Width="140px" MaxLength="6" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" OnTextChanged="txtPorc_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtVal5" runat="server" Width="140px" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" OnTextChanged="txtVal_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:CheckBox ID="cbPago5" runat="server" Checked="false" OnCheckedChanged="cbPago_CheckedChanged" AutoPostBack="true" />
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="line-height: 6px;" colspan="6">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtParc6" runat="server" Width="160px" Enabled="false" Text="" CssClass="alinharCentro"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPgto6" runat="server" MaxLength="10" Width="170px" onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPorc6" runat="server" Width="140px" MaxLength="6" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" OnTextChanged="txtPorc_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtVal6" runat="server" Width="140px" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" OnTextChanged="txtVal_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:CheckBox ID="cbPago6" runat="server" Checked="false" OnCheckedChanged="cbPago_CheckedChanged" AutoPostBack="true" />
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="line-height: 6px;" colspan="6">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtParc7" runat="server" Width="160px" Enabled="false" Text="" CssClass="alinharCentro"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPgto7" runat="server" MaxLength="10" Width="170px" onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPorc7" runat="server" Width="140px" MaxLength="6" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" OnTextChanged="txtPorc_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtVal7" runat="server" Width="140px" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" OnTextChanged="txtVal_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:CheckBox ID="cbPago7" runat="server" Checked="false" OnCheckedChanged="cbPago_CheckedChanged" AutoPostBack="true" />
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="line-height: 6px;" colspan="6">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtParc8" runat="server" Width="160px" Enabled="false" Text="" CssClass="alinharCentro"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPgto8" runat="server" MaxLength="10" Width="170px" onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPorc8" runat="server" Width="140px" MaxLength="6" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" OnTextChanged="txtPorc_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtVal8" runat="server" Width="140px" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" OnTextChanged="txtVal_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:CheckBox ID="cbPago8" runat="server" Checked="false" OnCheckedChanged="cbPago_CheckedChanged" AutoPostBack="true" />
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="line-height: 6px;" colspan="6">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>Falta Pagar
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValorTotalFal" runat="server" Width="140px" BackColor="LightYellow" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="line-height: 6px;" colspan="6">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>Total Pago
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValorTotalPago" runat="server" Width="140px" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="5" style="text-align: right;">
                                        <asp:Button ID="btSalvar" runat="server" Width="150px" Text="Salvar Pagamento" Enabled="true" OnClientClick="DesabilitarBotao(this);" OnClick="btSalvar_Click" />&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;
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
