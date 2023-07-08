<%@ Page Title="Manutenção de Pedido de Tecido" Language="C#" AutoEventWireup="true" CodeBehind="desenv_material_compra_entradav2.aspx.cs"
    Inherits="Relatorios.desenv_material_compra_entradav2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Manutenção de Pedido de Tecido</title>
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
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Tecidos&nbsp;&nbsp;>&nbsp;&nbsp;Pedido de Tecido&nbsp;&nbsp;>&nbsp;&nbsp;Manutenção de Pedido</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Manutenção de Pedido</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>Pedido<asp:HiddenField ID="hidCodigoSubPedido" runat="server" />
                                    </td>
                                    <td>Grupo
                                    </td>
                                    <td>SubGrupo
                                    </td>
                                    <td>Cor
                                    </td>
                                    <td>Cor Fornecedor
                                    </td>
                                    <td>Fornecedor
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 140px;">
                                        <asp:TextBox ID="txtNumeroPedido" runat="server" Enabled="false" Width="130px" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                    <td style="width: 200px;">
                                        <asp:TextBox ID="txtGrupoTecido" runat="server" Enabled="false" Width="190px"></asp:TextBox>
                                    </td>
                                    <td style="width: 210px;">
                                        <asp:TextBox ID="txtSubGrupoTecido" runat="server" Enabled="false" Width="200px"></asp:TextBox>
                                    </td>
                                    <td style="width: 180px;">
                                        <asp:TextBox ID="txtCor" runat="server" Enabled="false" Width="170px"></asp:TextBox>
                                    </td>
                                    <td style="width: 200px;">
                                        <asp:TextBox ID="txtCorFornecedor" runat="server" Enabled="false" Width="190px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFornecedor" runat="server" Enabled="false" Width="195px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <fieldset>
                                <legend>Pagamento</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="labParcela" runat="server" Text="Total de Parcelas"></asp:Label>
                                        </td>
                                        <td>Entrega Inicial
                                        </td>
                                        <td>Previsão Final
                                        </td>
                                        <td>Valor Inicial do Pedido
                                        </td>
                                        <td>Emissão</td>
                                        <td>Nota Fiscal</td>
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
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 180px">
                                            <asp:TextBox ID="txtEntregaIni" runat="server" MaxLength="10" Width="170px" onkeypress="return fnValidarData(event);" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtPrevFinal" runat="server" MaxLength="10" Width="140px" onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                        </td>
                                        <td style="width: 150px">
                                            <asp:TextBox ID="txtValor" runat="server" MaxLength="10" Width="140px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 140px">
                                            <asp:TextBox ID="txtEmissao" runat="server" Width="130px" MaxLength="10" onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                        </td>
                                        <td style="width: 140px">
                                            <asp:TextBox ID="txtNFPgto" runat="server" Width="130px" MaxLength="15" OnTextChanged="txtNFPgto_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:Button ID="btSalvar" runat="server" Text="Salvar Pagamento" Width="130px" OnClick="btSalvar_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" style="text-align: right;">
                                            <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
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
                                </table>
                            </fieldset>
                            <fieldset>
                                <legend>Entrada de Material</legend>

                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>Qtde do Pedido
                                        </td>
                                        <td colspan="7">Qtde Recebida
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtQtdePedido" runat="server" Enabled="false" Width="130px" MaxLength="10" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                        </td>
                                        <td colspan="7">
                                            <asp:TextBox ID="txtQtdeRecebimento" runat="server" Enabled="false" Width="130px" MaxLength="10" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="labDataEntrada" runat="server" Text="Data"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labQtdeEntrada" runat="server" Text="Quantidade"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labNFEntrada" runat="server" Text="Nota Fiscal"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labValorNota" runat="server" Text="Valor Total Nota"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labProdutoFornecedor" runat="server" Text="Produto Fornecedor"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labLargura" runat="server" Text="Largura"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labVolume" runat="server" Text="Volume"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labRendimentoMT" runat="server" Text="Rendimento (Metros)"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 140px;">
                                            <asp:TextBox ID="txtDataEntrada" runat="server" Width="130px" onkeypress="return fnReadOnly(event);" CssClass="alinharDireita" MaxLength="10"></asp:TextBox>
                                        </td>
                                        <td style="width: 140px;">
                                            <asp:TextBox ID="txtQuantidade" runat="server" Width="130px" MaxLength="10" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimalNegativo(event);"
                                                OnTextChanged="txtQuantidade_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td style="width: 140px;">
                                            <asp:TextBox ID="txtNF" runat="server" Width="130px" MaxLength="15"></asp:TextBox>
                                        </td>
                                        <td style="width: 150px;">
                                            <asp:TextBox ID="txtValorNF" runat="server" Width="140px" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" MaxLength="12" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 180px;">
                                            <asp:TextBox ID="txtProdutoFornecedor" runat="server" Width="170px" MaxLength="20"></asp:TextBox>
                                        </td>
                                        <td style="width: 115px;">
                                            <asp:TextBox ID="txtLargura" runat="server" Width="105px" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" MaxLength="12" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 95px;">
                                            <asp:TextBox ID="txtVolume" runat="server" Width="85px" onkeypress="return fnValidarNumero(event);" CssClass="alinharDireita" MaxLength="12"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRendimento" runat="server" Width="140px" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" MaxLength="12" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="labDataNota" runat="server" Text="Data Sol. Nota"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labTransportadora" runat="server" Text="Transportadora"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labFrete" runat="server" Text="Frete"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labCNPJ" runat="server" Text="CNPJ"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labNatOpe" runat="server" Text="Natureza de Operação"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labPrecoProduto" runat="server" Text="Preço do Produto"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="labDataRetirada" runat="server" Text="Data Retirada"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtDataNota" runat="server" Width="130px" onkeypress="return fnReadOnly(event);" CssClass="alinharDireita" MaxLength="10" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTransportadora" runat="server" Width="130px" MaxLength="100" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlFrete" runat="server" Width="134px" Enabled="false" Height="22px">
                                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                                <asp:ListItem Value="C" Text="Handbook"></asp:ListItem>
                                                <asp:ListItem Value="F" Text="Fornecedor"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCNPJ" runat="server" Width="140px" MaxLength="20" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNatOpe" runat="server" Width="170px" MaxLength="50" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtValProduto" runat="server" Width="105px" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" MaxLength="16" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtDataRetirada" runat="server" Width="234px" onkeypress="return fnReadOnly(event);" CssClass="alinharDireita" MaxLength="10" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="labPesoBrutoKG" runat="server" Text="Peso Bruto (KGs)"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labPesoLiqKG" runat="server" Text="Peso Líquido (KGs)"></asp:Label>
                                        </td>
                                        <td colspan="6">Observação</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtPesoBrutoKG" runat="server" Width="130px" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" MaxLength="16" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPesoLiqKG" runat="server" Width="130px" onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita" MaxLength="16" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td colspan="6">
                                            <asp:TextBox ID="txtObs" runat="server" Width="100%"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="8">&nbsp;
                                            <asp:HiddenField ID="hidCodigoPedidoQtde" runat="server" Value="" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <asp:Label ID="labEntrada" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td colspan="3" style="text-align: right;">
                                            <asp:Button ID="btCancelarEntrada" runat="server" Text="Cancelar" Width="130px" OnClick="btCancelarEntrada_Click" Visible="false" />&nbsp;&nbsp;
                                            <asp:Button ID="btIncluirEntrada" runat="server" Text="Incluir Entrada" Width="130px" OnClick="btIncluirEntrada_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <fieldset>
                                <legend>Entradas</legend>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvEntrada" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvEntrada_RowDataBound"
                                        OnDataBound="gvEntrada_DataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data Entrada" ItemStyle-Width="110px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labDataEntrada" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qtde Recebida" ItemStyle-Width="110px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labQtdeEntregue" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NF Entrada" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labNF" runat="server" Font-Size="Smaller" Text='<% #Bind("NOTA_FISCAL")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Produto Fornecedor" ItemStyle-Width="213px" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labProdutoForn" runat="server" Font-Size="Smaller" Text='<% #Bind("PRODUTO_FORNECEDOR")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valor da Nota" ItemStyle-Width="135px" HeaderStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labValorNota" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rendimento (MT)" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labRendimentoMT" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Obs" ItemStyle-Width="" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labObs" runat="server" Font-Size="Smaller" Text='<% #Bind("OBS")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btImprimir" runat="server" Width="15px" ImageUrl="~/Image/print.png"
                                                        ToolTip="Imprimir" OnClick="btImprimir_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btEditarEntrada" runat="server" Height="15px" Width="15px"
                                                        ImageUrl="~/Image/edit.jpg" OnClick="btEditarEntrada_Click" ToolTip="Editar Entrada" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btExcluirEntrada" runat="server" Height="15px" Width="15px"
                                                        ImageUrl="~/Image/delete.png" OnClick="btExcluirEntrada_Click" OnClientClick="return Confirmar('Deseja realmente Excluir esta Entrada?');"
                                                        ToolTip="Excluir Entrada" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                            <fieldset>
                                <legend>Devoluções</legend>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvDevolucao" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvDevolucao_RowDataBound"
                                        OnDataBound="gvDevolucao_DataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data Autorização" ItemStyle-Width="110px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labDataEntrada" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qtde Devolvida" ItemStyle-Width="110px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labQtdeDev" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data Sol. Nota" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labDataNota" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NF Origem" ItemStyle-Width="90px" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labNF" runat="server" Font-Size="Smaller" Text='<% #Bind("NOTA_FISCAL")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NF Devolução" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labNFDev" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Emissão" ItemStyle-Width="135px" HeaderStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labEmissao" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data Retirada" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labDataRetirada" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Obs" ItemStyle-Width="" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labObs" runat="server" Font-Size="Smaller" Text='<% #Bind("OBS")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btEditarDevol" runat="server" Height="15px" Width="15px"
                                                        ImageUrl="~/Image/edit.jpg" OnClick="btEditarEntrada_Click" ToolTip="Editar Devolução" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btExcluirEntrada" runat="server" Height="15px" Width="15px"
                                                        ImageUrl="~/Image/delete.png" OnClick="btExcluirEntrada_Click" OnClientClick="return Confirmar('Deseja realmente Excluir esta Devolução?');"
                                                        ToolTip="Excluir Entrada" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </fieldset>
                    </div>
                    <br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
