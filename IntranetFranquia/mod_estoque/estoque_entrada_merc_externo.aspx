<%@ Page Title="Entrada de Mercadoria - Externo" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="estoque_entrada_merc_externo.aspx.cs" Inherits="Relatorios.estoque_entrada_merc_externo" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
        }

        .alicentro {
            text-align: center;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Controle de Estoque&nbsp;&nbsp;>&nbsp;&nbsp;Estoque Loja&nbsp;&nbsp;>&nbsp;&nbsp;Entrada de Mercadoria - Externo</span>
                <div style="float: right; padding: 0;">
                    <a href="estoque_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Entrada de Mercadoria - Externo</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>Filial</td>
                            <td>Coleção</td>
                            <td>Produto</td>
                            <td>Fornecedor</td>
                            <td>Produto Recebido?</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 230px">
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="21px" Width="224px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 190px;" valign="top">
                                <asp:DropDownList ID="ddlColecoes" runat="server" Width="184px" Height="21px" DataTextField="DESC_COLECAO"
                                    DataValueField="COLECAO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 150px;" valign="top">
                                <asp:TextBox ID="txtProduto" runat="server" Width="140px" MaxLength="10"></asp:TextBox>
                            </td>
                            <td style="width: 300px;" valign="top">
                                <asp:DropDownList ID="ddlFabricante" runat="server" Width="294px" Height="21px" DataTextField="FORNECEDOR"
                                    DataValueField="FORNECEDOR">
                                </asp:DropDownList>
                            </td>
                            <td valign="top">
                                <asp:DropDownList ID="ddlProdutoRecebido" runat="server" Width="174px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="6">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" OnClientClick="DesabilitarBotao(this);" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">&nbsp;&nbsp;<asp:Label ID="labMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">&nbsp;
                            </td>
                        </tr>

                        <tr>
                            <td colspan="8">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvProdutoAcabado" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProdutoAcabado_RowDataBound" ShowFooter="true"
                                        OnSorting="gvProdutoAcabado_Sorting" AllowSorting="true" OnDataBound="gvProdutoAcabado_DataBound"
                                        DataKeyNames="PEDIDO_INTRA, PRODUTO, COR, ENTREGA, TIPO">
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgProduto" runat="server" Width="100%" ImageAlign="AbsMiddle" ToolTip=" " AlternateText=" " />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Filial" HeaderStyle-Width="" SortExpression="FILIAL" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litFilial" runat="server" Text='<%# Bind("FILIAL") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Produto" HeaderStyle-Width="" SortExpression="PRODUTO" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litProduto" runat="server" Text='<%# Bind("PRODUTO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Nome" HeaderStyle-Width="" SortExpression="DESC_PRODUTO" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNome" runat="server" Text='<%# Bind("DESC_PRODUTO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Cor" HeaderStyle-Width="" SortExpression="DESC_COR_PRODUTO" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litCor" runat="server" Text='<%# Bind("DESC_COR_PRODUTO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Griffe" HeaderStyle-Width="" SortExpression="GRIFFE" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litGriffe" runat="server" Text='<%# Bind("GRIFFE") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Fornecedor" HeaderStyle-Width="" SortExpression="FORNECEDOR" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litFornecedor" runat="server" Text='<%# Bind("FORNECEDOR") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Pedido Intra" HeaderStyle-Width="110px" SortExpression="PEDIDO_INTRA" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPedidoIntra" runat="server" Text='<%# Bind("PEDIDO_INTRA") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Qtde Intra" HeaderStyle-Width="95px" SortExpression="QTDE_INTRA" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtdeIntra" runat="server" Text='<%# Bind("QTDE_INTRA") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Prev Entrega" HeaderStyle-Width="110px" SortExpression="LIMITE_ENTREGA" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDataPrevEntrega" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="NF Entrada" HeaderStyle-Width="120px" ItemStyle-Font-Size="Smaller" SortExpression="NF_ENTRADA">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtNFEntrada" Style="text-align: center;" runat="server" Width="120px"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Recebido Por" HeaderStyle-Width="120px" ItemStyle-Font-Size="Smaller" SortExpression="RECEBIDO_POR">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRecebidoPor" runat="server" Text='<%# Bind("RECEBIDO_POR") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Recebimento" HeaderStyle-Width="120px" ItemStyle-Font-Size="Smaller" SortExpression="RECEBIMENTO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litRecebimento" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>Recebido Por</td>
                            <td colspan="5"></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtRecebidoPor" runat="server" Width="220px"></asp:TextBox>
                            </td>
                            <td colspan="5">
                                <asp:Button ID="btBaixar" runat="server" Text="Baixar Entrada" OnClick="btBaixar_Click" Width="125px" OnClientClick="DesabilitarBotao(this);" />&nbsp;<asp:Label ID="labErroBaixa" runat="server" ForeColor="Red" Text="" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
