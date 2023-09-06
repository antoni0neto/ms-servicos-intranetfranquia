<%@ Page Title="Gerar Pedido Produto" Language="C#" AutoEventWireup="true" CodeBehind="pacab_pedido_produto_editv2.aspx.cs"
    Inherits="Relatorios.pacab_pedido_produto_editv2" %>

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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Produto Acabado&nbsp;&nbsp;>&nbsp;&nbsp;Produto&nbsp;&nbsp;>&nbsp;&nbsp;Pedido&nbsp;&nbsp;>&nbsp;&nbsp;Gerar Pedido</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Pedido Produto Acabado</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="4">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="labFornecedor" runat="server" Text="Fornecedor"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labDataEntrega" runat="server" Text="Limite Entrega"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labStatus" runat="server" Text="Status"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 300px">
                                        <asp:DropDownList ID="ddlFabricante" runat="server" Width="294px" Height="21px" DataTextField="FORNECEDOR"
                                            DataValueField="FORNECEDOR">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 170px">
                                        <asp:TextBox ID="txtDataEntrega" runat="server" MaxLength="10" Width="160px"
                                            onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                    <td style="width: 400px">
                                        <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                            Height="21px" Width="394px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="280px" Height="21px">
                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="A" Text="APROVADO" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="E" Text="ESTUDO"></asp:ListItem>
                                            <asp:ListItem Value="R" Text="REPROVADO"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <fieldset>
                                            <legend>Produtos do Pedido</legend>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvCarrinho" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvCarrinho_RowDataBound"
                                                    OnDataBound="gvCarrinho_DataBound" ShowFooter="true">
                                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="Produto" HeaderStyle-Width="130px">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litProduto" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderText="Nome">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litNome" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderText="Cor" HeaderStyle-Width="">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litCor" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderText="Fornecedor" HeaderStyle-Width="">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litRefFabricante" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderText="Custo" HeaderStyle-Width="">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litCusto" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="Grade Total" HeaderStyle-Width="">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litGradeTotal" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="btNovoPedido" runat="server" Width="122px" Text="Novo Pedido" OnClick="btNovoPedido_Click" />&nbsp;
                                    </td>
                                    <td colspan="2" style="text-align: right;">
                                        <asp:Button ID="btSalvar" runat="server" Width="122px" Text="Gerar Pedido" OnClientClick="DesabilitarBotao(this);" OnClick="btSalvar_Click" />&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">&nbsp;
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
