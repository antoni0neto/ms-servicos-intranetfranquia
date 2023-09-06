<%@ Page Title="Resumo do Pedido" Language="C#" AutoEventWireup="true" CodeBehind="desenv_pedido_cad_resumo.aspx.cs"
    Inherits="Relatorios.desenv_pedido_cad_resumo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cadastro de Aviamento</title>
    <style type="text/css">
        .fs
        {
            border: 1px solid #ccc;
            font-family: Calibri;
        }
        .tb
        {
            padding: 0;
            font-family: Calibri;
            font-size: 14px;
        }
    </style>
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="thisForm" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div style="margin-top: 10px; padding-left: 12px; padding-right: 12px; padding-bottom: -350px;
                background-color: White;">
                <br />
                <div>
                    <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Controle
                        de Produto&nbsp;&nbsp;>&nbsp;&nbsp;Resumo do Pedido</span>
                    <div style="float: right; padding: 0;">
                        <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                    </div>
                </div>
                <hr />
                <div>
                    <fieldset class="fs">
                        <legend>Resumo do Pedido</legend>
                        <table border="0" cellspacing="0" cellpadding="0" width="100%" class="tb">
                            <tr style="height: 20px; vertical-align: bottom;">
                                <td style="width: 250px;">
                                    Coleção:
                                    <asp:Label ID="labColecao" runat="server" Font-Bold="true" Text=""></asp:Label>
                                </td>
                                <td style="width: 220px;">
                                    Número do Pedido:
                                    <asp:Label ID="labPedidoNumero" runat="server" Font-Bold="true" Text=""></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr style="height: 20px; vertical-align: bottom;">
                                <td>
                                    Fornecedor:
                                    <asp:Label ID="labFornecedor" runat="server" Font-Bold="true" Text=""></asp:Label>
                                </td>
                                <td>
                                    Data do Pedido:
                                    <asp:Label ID="labDataPedido" runat="server" Font-Bold="true" Text=""></asp:Label>
                                </td>
                                <td>
                                    Previsão de Entrega:
                                    <asp:Label ID="labPrevisaoEntrega" runat="server" Font-Bold="true" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 20px; vertical-align: bottom;">
                                <td>
                                    Quantidade:
                                    <asp:Label ID="labQtde" runat="server" Font-Bold="true" Text=""></asp:Label>&nbsp;
                                    <asp:Label ID="labUnidadeMedida" runat="server" Font-Bold="true" Text=""></asp:Label>s
                                </td>
                                <td>
                                    Preço:
                                    <asp:Label ID="labPreco" runat="server" Font-Bold="true" Text=""></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr style="height: 20px; vertical-align: bottom;">
                                <td colspan="2">
                                    Forma de Pagamento:
                                    <asp:Label ID="labFormaPgto" runat="server" Font-Bold="true" Text=""></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr style="height: 20px;">
                                <td colspan="3">
                                </td>
                            </tr>
                        </table>
                        <div style="text-align: right; width: 100%; color:Red;">
                            <asp:Label ID="labAtencao" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
                        </div>
                        <div style="width: 100%; overflow: auto; height: 195px;">
                            <div>
                                <table border="0" cellpadding="0" class="tb" width="100%">
                                    <tr>
                                        <td style="width: 100%;">
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ShowFooter="true" ForeColor="#333333" Style="background: white" OnDataBound="gvProduto_DataBound"
                                                    OnRowDataBound="gvProduto_RowDataBound">
                                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="GRUPO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderText="Grupo" HeaderStyle-Width="100px" />
                                                        <asp:BoundField DataField="MODELO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderText="Modelo" HeaderStyle-Width="100px" />
                                                        <asp:BoundField DataField="COR" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderText="Cor" HeaderStyle-Width="100px" />
                                                        <asp:BoundField DataField="PRECO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                            HeaderText="Preço" HeaderStyle-Width="100px" />
                                                        <asp:BoundField DataField="QTDE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                            HeaderText="Qtde Varejo" HeaderStyle-Width="100px" />
                                                        <asp:BoundField DataField="CONSUMO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                            HeaderText="Consumo" HeaderStyle-Width="100px" />
                                                        <asp:TemplateField HeaderText="Total" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litQtdeConsumo" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </fieldset>
                </div>
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
