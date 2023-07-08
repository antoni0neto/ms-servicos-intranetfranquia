<%@ Page Title="Gerar Pedido Aviamento" Language="C#" AutoEventWireup="true" CodeBehind="desenv_pedido_aviamento_gerarprepedido.aspx.cs"
    Inherits="Relatorios.desenv_pedido_aviamento_gerarprepedido" %>

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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Aviamentos&nbsp;&nbsp;>&nbsp;&nbsp;Pré-pedido de Aviamento&nbsp;&nbsp;>&nbsp;&nbsp;Gerar Pedido</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Pedido Aviamento</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="4">
                                        <asp:HiddenField ID="hidCodigoCarrinhoCab" runat="server" Value="" />
                                    </td>
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
                                        <asp:DropDownList ID="ddlFabricante" runat="server" Width="294px" Height="21px" DataTextField="FORNECEDOR" DataValueField="FORNECEDOR"
                                            OnSelectedIndexChanged="ddlFabricante_SelectedIndexChanged" AutoPostBack="true">
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
                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="250px" Height="21px">
                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="A" Text="APROVADO" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="E" Text="ESTUDO"></asp:ListItem>
                                            <asp:ListItem Value="R" Text="REPROVADO"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="labEmail" runat="server" Text="Email"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" Width="290px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">Mensagem
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:TextBox ID="txtMensagem" runat="server" Text="" TextMode="MultiLine" Height="40px" Width="100%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <fieldset>
                                            <legend>Aviamentos do Pedido</legend>
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
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                                            HeaderText="Origem" HeaderStyle-Width="" SortExpression="DESC_PEDIDO_ORIGEM">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litOrigem" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                                            HeaderText="Produto" HeaderStyle-Width="" SortExpression="PRODUTO">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litProduto" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                                            HeaderText="HB" HeaderStyle-Width="" SortExpression="HB">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litHB" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                                            HeaderText="Nome" SortExpression="DESC_PRODUTO">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litNome" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                                            HeaderText="Cor Produto" HeaderStyle-Width="" SortExpression="COR_FORNECEDOR_PRODUTO">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litCor" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                                            HeaderText="Detalhe" HeaderStyle-Width="" SortExpression="DETALHE">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litDetalhe" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                                            HeaderText="Aviamento" HeaderStyle-Width="" SortExpression="SUBGRUPO">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litSubGrupo" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                                            HeaderText="Cor Material" HeaderStyle-Width="" SortExpression="COR_FORNECEDOR_MATERIAL">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litCorMaterial" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                                            HeaderText="Medida" HeaderStyle-Width="" SortExpression="">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litUnidadeMedida" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Quantidade" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litQtde" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Custo Unit" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litCusto" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Desconto Unit" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litDesconto" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                                            HeaderText="Total" HeaderStyle-Width="">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">&nbsp;
                                    </td>
                                    <td colspan="2" style="text-align: right;">Enviar Email&nbsp;<asp:CheckBox ID="cbEnviarEmail" runat="server" Checked="false" />&nbsp;
                                        <asp:Button ID="btSalvar" runat="server" Width="122px" Text="Enviar Pedido" OnClientClick="DesabilitarBotao(this);" OnClick="btSalvar_Click" />&nbsp;
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
