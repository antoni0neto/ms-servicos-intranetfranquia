<%@ Page Title="Aviamento - Pedidos Recebidos" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="desenv_pedido_aviamento_linx_con.aspx.cs" Inherits="Relatorios.desenv_pedido_aviamento_linx_con" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../Image/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "../Image/plus.png");
            $(this).closest("tr").next().remove();
        });
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Aviamentos&nbsp;&nbsp;>&nbsp;&nbsp;Pedidos Recebidos</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Pedidos Recebidos</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">

                        <tr>
                            <td>Coleção</td>
                            <td>Origem</td>
                            <td>Grupo</td>
                            <td>Produto</td>
                            <td>Filial</td>
                            <td>Fornecedor</td>
                            <td>Pedido Intranet</td>
                            <td>Pedido Linx</td>
                        </tr>
                        <tr>
                            <td style="width: 230px;" valign="top">
                                <asp:DropDownList ID="ddlColecoes" runat="server" Width="224px" Height="21px" DataTextField="DESC_COLECAO"
                                    DataValueField="COLECAO" OnSelectedIndexChanged="ddlColecoes_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 190px;" valign="top">
                                <asp:DropDownList ID="ddlOrigem" runat="server" Width="184px" Height="21px" DataTextField="DESCRICAO"
                                    DataValueField="CODIGO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 180px;" valign="top">
                                <asp:DropDownList ID="ddlGrupo" runat="server" Width="174px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                    DataValueField="GRUPO_PRODUTO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 150px;" valign="top">
                                <asp:TextBox ID="txtProduto" runat="server" Width="140px" MaxLength="10"></asp:TextBox>
                            </td>
                            <td style="width: 230px">
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="21px" Width="224px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 220px">
                                <asp:DropDownList ID="ddlFabricante" runat="server" Width="214px" Height="21px" DataTextField="FORNECEDOR"
                                    DataValueField="FORNECEDOR">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 130px">
                                <asp:TextBox ID="txtPedidoIntranet" runat="server" Width="120px" MaxLength="15"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPedidoLinx" runat="server" Width="90px" MaxLength="15"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Grupo Material</td>
                            <td>Subgrupo Material</td>
                            <td colspan="6">Pedido Recebido</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlMaterialGrupo" runat="server" Width="224px" Height="21px"
                                    DataTextField="GRUPO" DataValueField="CODIGO_GRUPO" OnSelectedIndexChanged="ddlMaterialGrupo_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMaterialSubGrupo" runat="server" Width="184px" Height="21px"
                                    DataTextField="SUBGRUPO" DataValueField="CODIGO_SUBGRUPO">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPedidoRecebido" runat="server" Width="174px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td colspan="5">
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" OnClientClick="DesabilitarBotao(this);" />
                                &nbsp;&nbsp;<asp:Label ID="labMsg" runat="server" ForeColor="Red" Text=""></asp:Label>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvMaterial" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvMaterial_RowDataBound"
                                        OnDataBound="gvMaterial_DataBound" ShowFooter="true"
                                        OnSorting="gvMaterial_Sorting" AllowSorting="true">
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Bold="true" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Pedido Intranet" HeaderStyle-Width="120px" SortExpression="PEDIDO_INTRANTE">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPedidoIntranet" runat="server" Text='<%# Bind("PEDIDO_INTRANTE") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Pedido Linx" HeaderStyle-Width="90px" SortExpression="PEDIDO_LINX">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPedidoLinx" runat="server" Text='<%# Bind("PEDIDO_LINX") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Fornecedor" HeaderStyle-Width="" SortExpression="FORNECEDOR_MATERIAL">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litFornecedor" runat="server" Text='<%# Bind("FORNECEDOR_MATERIAL") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Produto" HeaderStyle-Width="85px" SortExpression="PRODUTO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litModelo" runat="server" Text='<%# Bind("PRODUTO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Nome" HeaderStyle-Width="" SortExpression="DESC_MODELO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNome" runat="server" Text='<%# Bind("DESC_MODELO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Cor Produto" HeaderStyle-Width="" SortExpression="DESC_COR_PRODUTO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litCor" runat="server" Text='<%# Bind("DESC_COR_PRODUTO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Griffe" HeaderStyle-Width="120px" SortExpression="GRIFFE">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litGriffe" runat="server" Text='<%# Bind("GRIFFE") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="SubGrupo" HeaderStyle-Width="" SortExpression="SUBGRUPO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litSubGrupo" runat="server" Text='<%# Bind("SUBGRUPO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Cor Material" HeaderStyle-Width="" SortExpression="COR_FORNECEDOR_MATERIAL">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litCorMaterial" runat="server" Text='<%# Bind("COR_FORNECEDOR_MATERIAL") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Qtde" HeaderStyle-Width="80px" SortExpression="QTDE">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtde" runat="server" Text='<%# Bind("QTDE") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
