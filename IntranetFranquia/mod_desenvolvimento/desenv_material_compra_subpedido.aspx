<%@ Page Title="Pedido" Language="C#" AutoEventWireup="true" CodeBehind="desenv_material_compra_subpedido.aspx.cs"
    Inherits="Relatorios.desenv_material_compra_subpedido" %>

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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Apoio&nbsp;&nbsp;>&nbsp;&nbsp;Compra de Materiais&nbsp;&nbsp;>&nbsp;&nbsp;Pedido</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset style="margin-top: 4px; padding-top: 0;">
                            <legend>Pedido</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="labNumeroPedido" runat="server" Text="Número Pedido"></asp:Label>
                                        <asp:HiddenField ID="hidCodigoPedido" runat="server" />
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
                                    <td colspan="5">
                                        <fieldset>
                                            <legend>Aviamento HB</legend>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td style="text-align: right;">
                                                        <asp:CheckBox ID="chkMarcarTodos" runat="server" Checked="false" OnCheckedChanged="chkMarcarTodos_CheckedChanged" AutoPostBack="true" />
                                                        <asp:Label ID="labMarcarTodos" runat="server" Text="Marcar Todos"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div class="rounded_corners">
                                                            <asp:GridView ID="gvAviamentoHB" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvAviamentoHB_RowDataBound"
                                                                OnDataBound="gvAviamentoHB_DataBound" ShowFooter="true"
                                                                DataKeyNames="CODIGO">
                                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                                <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Coleção" HeaderStyle-Width="220px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litColecao" runat="server" Text='<%# Bind("DESC_COLECAO") %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="HB" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litHB" runat="server" Text='<%# Bind("HB") %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Mostruário" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMostruario" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Quantidade" HeaderStyle-Width="180px" HeaderStyle-HorizontalAlign="Center"
                                                                        ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litQtde" runat="server" Text='<%# Bind("QTDE") %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Inserir no Pedido" HeaderStyle-Width="180px" HeaderStyle-HorizontalAlign="Center"
                                                                        ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="cbMarcar" runat="server" OnCheckedChanged="cbMarcar_CheckedChanged" AutoPostBack="true" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litEspaco" runat="server" Text=''></asp:Literal>
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
                                    <td colspan="5">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <fieldset>
                                            <legend>Sub Pedido</legend>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="labFornecedorPedido" runat="server" Text="Fornecedor"></asp:Label>
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
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 250px;">
                                                        <asp:DropDownList ID="ddlPedidoFornecedor" runat="server" Width="244px" Height="21px"
                                                            DataTextField="FORNECEDOR" DataValueField="FORNECEDOR" OnSelectedIndexChanged="ddlPedidoFornecedor_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 200px;">
                                                        <asp:TextBox ID="txtQtde" runat="server" Width="190px" MaxLength="10" Style="text-align: right;"
                                                            onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 200px;">
                                                        <asp:TextBox ID="txtPreco" runat="server" Width="190px" MaxLength="10" Style="text-align: right;"
                                                            onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 170px;">
                                                        <asp:TextBox ID="txtPedidoData" runat="server" Width="160px" MaxLength="10" Style="text-align: right;"
                                                            onkeypress="return fnValidarData(event);"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPedidoPrevEntrega" runat="server" Width="160px" MaxLength="10"
                                                            Style="text-align: right;" onkeypress="return fnValidarData(event);"></asp:TextBox>
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <asp:CheckBox ID="chkEmail" runat="server" Text="Enviar e-mail" Checked="true" OnCheckedChanged="chkEmail_CheckedChanged" AutoPostBack="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <asp:Label ID="labEmail" runat="server" Text="E-mail"></asp:Label>
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
                                        <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                                        <asp:Button ID="btSalvar" runat="server" Width="120px" Text="Salvar" ToolTip="Salvar" OnClick="btSalvar_Click" />
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
