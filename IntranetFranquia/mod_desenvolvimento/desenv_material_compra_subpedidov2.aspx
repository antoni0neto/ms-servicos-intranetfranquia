<%@ Page Title="Pedido" Language="C#" AutoEventWireup="true" CodeBehind="desenv_material_compra_subpedidov2.aspx.cs"
    Inherits="Relatorios.desenv_material_compra_subpedidov2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Pedidos</title>
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Tecidos&nbsp;&nbsp;>&nbsp;&nbsp;Pedido de Tecido&nbsp;&nbsp;>&nbsp;&nbsp;Compra</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset style="margin-top: 4px; padding-top: 0;">
                            <legend>Pedido de Tecido</legend>
                            <br />
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="labNumeroPedido" runat="server" Text="Número Pedido"></asp:Label>
                                        <asp:HiddenField ID="hidCodigoPedido" runat="server" />
                                        <asp:HiddenField ID="hidCodigoSubPedido" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="labMaterialGrupoPedido" runat="server" Text="Grupo"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labMaterialSubGrupoPedido" runat="server" Text="SubGrupo"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labCorPedido" runat="server" Text="Cor"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labCorFornecedorPedido" runat="server" Text="Cor Fornecedor"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 180px;">
                                        <asp:TextBox ID="txtPedidoNumero" runat="server" Width="170px" Height="15px" Enabled="false"
                                            CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                    <td style="width: 210px;">
                                        <asp:DropDownList ID="ddlMaterialGrupoPedido" runat="server" Width="204px" Height="21px" Enabled="false"
                                            DataTextField="GRUPO" DataValueField="CODIGO_GRUPO">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 250px;">
                                        <asp:DropDownList ID="ddlMaterialSubGrupoPedido" runat="server" Width="244px" Height="21px" Enabled="false"
                                            DataTextField="SUBGRUPO" DataValueField="CODIGO_SUBGRUPO">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 230px;">
                                        <asp:DropDownList ID="ddlCorPedido" runat="server" Width="224px" Height="21px" DataTextField="DESC_COR" Enabled="false"
                                            DataValueField="COR">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCorFornecedorPedido" runat="server" Width="277px" Height="21px" Enabled="false"
                                            DataTextField="REFER_FABRICANTE" DataValueField="REFER_FABRICANTE">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <fieldset>
                                            <legend>Sub Pedido</legend>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="labColecao" runat="server" Text="Colecão"></asp:Label>
                                                    </td>
                                                    <td colspan="5">
                                                        <asp:Label ID="labPagamento" runat="server" Text="Pagamento"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="ddlColecao" runat="server" Width="244px" Height="21px"
                                                            DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td colspan="5">
                                                        <asp:DropDownList ID="ddlPagamento" runat="server" Width="194px" Height="21px">
                                                            <asp:ListItem Value="0" Text="Selecione"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="1x" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="2x"></asp:ListItem>
                                                            <asp:ListItem Value="3" Text="3x"></asp:ListItem>
                                                            <asp:ListItem Value="4" Text="4x"></asp:ListItem>
                                                            <asp:ListItem Value="5" Text="5x"></asp:ListItem>
                                                            <asp:ListItem Value="6" Text="6x"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="labFornecedor" runat="server" Text="Fornecedor"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="labQuantidade" runat="server" Text="Quantidade"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="labPreco" runat="server" Text="Preço"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="labDataPedido" runat="server" Text="Data do Pedido"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="labDataPrevisaoEntrega" runat="server" Text="Previsão de Entrega"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="labStatus" runat="server" Text="Status"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 250px;">
                                                        <asp:DropDownList ID="ddlFornecedor" runat="server" Width="244px" Height="21px"
                                                            DataTextField="FORNECEDOR" DataValueField="FORNECEDOR" OnSelectedIndexChanged="ddlPedidoFornecedor_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 200px;">
                                                        <asp:TextBox ID="txtQtde" runat="server" Width="190px" MaxLength="10" Style="text-align: right;"
                                                            onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>

                                                        <asp:HiddenField ID="hidQtdeAnt" runat="server" Value="0" />
                                                    </td>
                                                    <td style="width: 200px;">
                                                        <asp:TextBox ID="txtPreco" runat="server" Width="190px" MaxLength="10" Style="text-align: right;"
                                                            onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 170px;">
                                                        <asp:TextBox ID="txtPedidoData" runat="server" Width="160px" MaxLength="10" Style="text-align: right;"
                                                            onkeypress="return fnValidarData(event);"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 170px;">
                                                        <asp:TextBox ID="txtPedidoPrevEntrega" runat="server" Width="160px" MaxLength="10"
                                                            Style="text-align: right;" onkeypress="return fnValidarData(event);"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="130px" Height="21px">
                                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                                            <asp:ListItem Value="R" Text="Reserva"></asp:ListItem>
                                                            <asp:ListItem Value="P" Text="Pedido"></asp:ListItem>
                                                            <asp:ListItem Value="B" Text="Baixado"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <asp:CheckBox ID="chkEmail" runat="server" Text="Enviar e-mail" Checked="true" OnCheckedChanged="chkEmail_CheckedChanged" AutoPostBack="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <asp:TextBox ID="txtEmail" runat="server" Width="1116px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <asp:Label ID="labCorpoEmail" runat="server" Text="Corpo do E-mail"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <asp:TextBox ID="txtCorpoEmail" runat="server" Width="1114px" Height="90px" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5" style="text-align: right;">
                                        <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                                        <asp:Button ID="btSalvar" runat="server" Width="120px" Text="Salvar" ToolTip="Salvar" OnClick="btSalvar_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvPrePedido" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvPrePedido_RowDataBound"
                                                            OnDataBound="gvPrePedido_DataBound" ShowFooter="true" DataKeyNames="CODIGO, CODIGO_PREPEDIDOSUB">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                                            <FooterStyle BackColor="Gainsboro" Font-Bold="true" Font-Size="Small" />
                                                            <PagerStyle HorizontalAlign="Center" CssClass="pageStyl" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField HeaderText="Produto" DataField="PRODUTO" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" SortExpression="PRODUTO" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" FooterStyle-HorizontalAlign="Center" />
                                                                <asp:BoundField HeaderText="Tecido" DataField="TECIDO" HeaderStyle-Width="150px" SortExpression="TECIDO" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                                                <asp:BoundField HeaderText="Fornecedor" DataField="FORNECEDOR" HeaderStyle-Width="140px" SortExpression="FORNECEDOR" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                                                <asp:BoundField HeaderText="Cor Fornecedor" DataField="COR_FORNECEDOR" HeaderStyle-Width="120px" SortExpression="COR_FORNECEDOR" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                                                <asp:BoundField HeaderText="Cor Linx" DataField="DESC_COR" SortExpression="DESC_COR" HeaderStyle-Width="110px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                                                <asp:BoundField HeaderText="Grupo Produto" DataField="GRUPO_PRODUTO" SortExpression="GRUPO_PRODUTO" HeaderStyle-Width="100px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                                                <asp:BoundField HeaderText="Griffe" DataField="GRIFFE" SortExpression="GRIFFE" HeaderStyle-Width="80px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                                                <asp:TemplateField HeaderText="Consumo" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                                                    SortExpression="CONSUMO">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litConsumo" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Preço Tecido" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                                                    SortExpression="PRECO_TECIDO">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litPrecoTecido" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tot Consumo" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                                                    SortExpression="CONSUMO_TOTAL">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litConsumoTotal" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Val Tecido" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                                                    SortExpression="VALOR_TECIDO">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litValorTecido" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Pedido" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                                                    SortExpression="NUMERO_PEDIDO">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litPedido" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField HeaderText="Status" DataField="STATUS_DESC" SortExpression="STATUS_DESC" HeaderStyle-Width="" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />

                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="cbAssociar" runat="server" Checked="false" OnCheckedChanged="cbAssociar_CheckedChanged" AutoPostBack="true" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btExcluir" runat="server" ImageUrl="~/Image/delete.png" Width="15px" OnClick="btExcluir_Click" />
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
                    </div>
                    <br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
