<%@ Page Title="Vendas Online" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="gerloja_venda_ecom.aspx.cs" Inherits="Relatorios.gerloja_venda_ecom" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">
            <asp:Label ID="labTitulo" runat="server" Text="Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Desempenho&nbsp;&nbsp;>&nbsp;&nbsp;Vendas Online"></asp:Label></span>
        <div style="float: right; padding: 0;">
            <a href="gerloja_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div>
        <fieldset>
            <legend>Vendas Online</legend>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>Data Início
                    </td>
                    <td>Data Fim
                    </td>
                    <td>Filial
                    </td>
                    <td>CÓD. Vendedor
                    </td>
                    <td>Nome
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;">
                        <asp:TextBox ID="txtPeriodoInicial" runat="server" Width="190px" MaxLength="10" Style="text-align: right;" onkeypress="return fnValidarData(event);"></asp:TextBox>
                    </td>
                    <td style="width: 200px;">
                        <asp:TextBox ID="txtPeriodoFinal" runat="server" Width="190px" MaxLength="10" Style="text-align: right;" onkeypress="return fnValidarData(event);"></asp:TextBox>
                    </td>
                    <td style="width: 300px;">
                        <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="21px" Width="294px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 170px;">
                        <asp:TextBox ID="txtCODVendedor" runat="server" Width="160px" MaxLength="4" Style="text-align: right;" onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNome" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:Button runat="server" ID="btBuscarVendas" Text="Buscar Vendas" Width="120px" OnClick="btBuscarVendas_Click" />
                        &nbsp;&nbsp;<asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <fieldset>
                <div class="rounded_corners">
                    <asp:GridView ID="gvVendas" runat="server" Width="100%" ForeColor="#333333" AutoGenerateColumns="False" OnRowDataBound="gvVendas_RowDataBound"
                        OnDataBound="gvVendas_DataBound" ShowFooter="true" OnSorting="gvVendas_Sorting" AllowSorting="true">
                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Bold="true" />
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Filial" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="200px"
                                SortExpression="FILIAL">
                                <ItemTemplate>
                                    <asp:Literal ID="litFilial" runat="server" Text='<%# Bind("FILIAL") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Vendedor" ItemStyle-Font-Size="Smaller"
                                SortExpression="NOME_VENDEDOR">
                                <ItemTemplate>
                                    <asp:Literal ID="litNome" runat="server" Text='<%# Bind("NOME_VENDEDOR") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="Pedido" HeaderStyle-Width="115px"
                                SortExpression="PEDIDO_EXTERNO">
                                <ItemTemplate>
                                    <asp:Literal ID="litPedidoExterno" runat="server" Text='<%# Bind("PEDIDO_EXTERNO") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="Linx" HeaderStyle-Width="115px"
                                SortExpression="PEDIDO">
                                <ItemTemplate>
                                    <asp:Literal ID="litPedido" runat="server" Text='<%# Bind("PEDIDO") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="Data Pagamento" HeaderStyle-Width="145px"
                                SortExpression="DATA_PAGAMENTO">
                                <ItemTemplate>
                                    <asp:Literal ID="litDataPagamento" runat="server"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Valor Pedido" HeaderStyle-Width="130px"
                                SortExpression="VALOR_PEDIDO">
                                <ItemTemplate>
                                    <asp:Literal ID="litValorPedido" runat="server"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Código de Rastreio" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="140px"
                                SortExpression="TRACK_NUMBER">
                                <ItemTemplate>
                                    <asp:Literal ID="litTrackNumber" runat="server" Text='<%# Bind("TRACK_NUMBER") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="" ItemStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:Button ID="btRastrearPedido" runat="server" Text=">>" Width="30px" OnClick="btRastrearPedido_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Status" ItemStyle-Font-Size="Smaller"
                                SortExpression="STATUS">
                                <ItemTemplate>
                                    <asp:Literal ID="litStatus" runat="server" Text='<%# Bind("STATUS") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Erro" ItemStyle-Font-Size="Smaller"
                                SortExpression="ERRO">
                                <ItemTemplate>
                                    <asp:Literal ID="litStatusErro" runat="server" Text='<%# Bind("ERRO") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset>
        </fieldset>
    </div>
</asp:Content>
