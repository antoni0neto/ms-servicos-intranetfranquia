<%@ Page Title="Estoque Ecommerce - Divergente" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" EnableEventValidation="false" MaintainScrollPositionOnPostback="true"
    CodeBehind="ecomv2_produto_magento_estoque_tamanho.aspx.cs" Inherits="Relatorios.ecomv2_produto_magento_estoque_tamanho" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
        }
    </style>
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://code.jquery.com/jquery.js"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>

    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Estoque&nbsp;&nbsp;>&nbsp;&nbsp;Estoque Ecommerce Divergente</span>
        <div style="float: right; padding: 0;">
            <a href="~/mod_ecom/ecom_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="login">
        <fieldset style="padding-top: 0;">
            <legend>Estoque Ecommerce Divergente</legend>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>Griffe</td>
                    <td>Grupo Produto</td>
                    <td>Produto</td>
                    <td>Magento Maior Linx</td>
                </tr>
                <tr>
                    <td style="width: 220px">
                        <asp:DropDownList ID="ddlGriffe" runat="server" Width="214px" Height="21px" DataTextField="GRIFFE"
                            DataValueField="GRIFFE">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 200px">
                        <asp:DropDownList ID="ddlGrupo" runat="server" Width="194px" Height="21px" DataTextField="GRUPO_PRODUTO"
                            DataValueField="GRUPO_PRODUTO">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 160px">
                        <asp:TextBox ID="txtProduto" runat="server" onkeypress="return fnValidarNumero(event);" Width="146px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMagentoMaiorLinx" runat="server" Width="200px" Height="21px">
                            <asp:ListItem Value="" Text=""></asp:ListItem>
                            <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Button ID="btBuscar" runat="server" Text="Buscar Estoque Divergente" Width="200px" OnClick="btBuscar_Click" />
                        &nbsp;
                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div id="accordionP">
                            <h3>Produtos</h3>
                            <div>
                                <table border="0" cellpadding="0" class="tb" width="100%">
                                    <tr>
                                        <td style="width: 100%;">
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound"
                                                    OnDataBound="gvProduto_DataBound" ShowFooter="true"
                                                    DataKeyNames="">
                                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Size="Smaller"></HeaderStyle>
                                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Size="Smaller" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="SKU" HeaderText="Sku" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="SKU" ItemStyle-Font-Size="Smaller" />
                                                        <asp:BoundField DataField="PRODUTO" HeaderText="Produto" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="PRODUTO" ItemStyle-Font-Size="Smaller" />
                                                        <asp:BoundField DataField="NOME" HeaderText="Nome" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="NOME" ItemStyle-Font-Size="Smaller" />
                                                        <asp:BoundField DataField="DESC_COR" HeaderText="Cor" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="DESC_COR" ItemStyle-Font-Size="Smaller" />
                                                        <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="TAMANHO" ItemStyle-Font-Size="Smaller" />
                                                        <asp:BoundField DataField="ESTOQUE_ONLINE" HeaderText="Estoque Mag" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="ESTOQUE_ONLINE" ItemStyle-Font-Size="Smaller" />
                                                        <asp:BoundField DataField="ESTOQUE_LINX" HeaderText="Estoque Linx" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="ESTOQUE_LINX" ItemStyle-Font-Size="Smaller" />
                                                        <asp:BoundField DataField="OBSERVACAO" HeaderText="Observação" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="OBSERVACAO" ItemStyle-Font-Size="Smaller" />
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgAtualizarEst" runat="server" ImageUrl="~/Image/update.png" Width="15px" OnClick="imgAtualizarEst_Click" />
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
                        <br />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
