<%@ Page Title="Estoque Ecommerce" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" EnableEventValidation="false" MaintainScrollPositionOnPostback="true"
    CodeBehind="ecom_produto_magento_estoque.aspx.cs" Inherits="Relatorios.ecom_produto_magento_estoque" %>

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
        <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Estoque&nbsp;&nbsp;>&nbsp;&nbsp;Estoque Ecommerce</span>
        <div style="float: right; padding: 0;">
            <a href="ecom_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="login">
        <fieldset style="padding-top: 0;">
            <legend>Estoque Ecommerce</legend>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>Coleção
                    </td>
                    <td>Griffe
                    </td>
                    <td>Grupo Produto
                    </td>
                    <td>Categoria Magento
                    </td>
                    <td>Produto
                    </td>
                    <td>Estoque Linx
                    </td>
                    <td>Estoque Magento
                    </td>
                    <td>Habilitado
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px">
                        <asp:DropDownList ID="ddlColecao" runat="server" Width="194px" Height="21px"
                            DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 200px">
                        <asp:DropDownList ID="ddlGriffe" runat="server" Width="194px" Height="21px" DataTextField="GRIFFE"
                            DataValueField="GRIFFE">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 200px">
                        <asp:DropDownList ID="ddlGrupo" runat="server" Width="194px" Height="21px" DataTextField="GRUPO_PRODUTO"
                            DataValueField="GRUPO_PRODUTO">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 180px;">
                        <asp:DropDownList ID="ddlCategoriaMag" runat="server" Width="174px" Height="21px" DataTextField="GRUPO"
                            DataValueField="CODIGO">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 160px">
                        <asp:TextBox ID="txtProduto" runat="server" onkeypress="return fnValidarNumero(event);"
                            Width="150px"></asp:TextBox>
                    </td>
                    <td style="width: 160px">
                        <asp:DropDownList ID="ddlEstoqueLinx" runat="server" Width="154px">
                            <asp:ListItem Value="" Text=""></asp:ListItem>
                            <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                            <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 160px">
                        <asp:DropDownList ID="ddlEstoqueMag" runat="server" Width="154px">
                            <asp:ListItem Value="" Text=""></asp:ListItem>
                            <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                            <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlHabilitado" runat="server" Width="154px">
                            <asp:ListItem Value="" Text=""></asp:ListItem>
                            <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                            <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Estoque Diff</td>
                    <td colspan="7"></td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlEstoqueDiff" runat="server" Width="194px">
                            <asp:ListItem Value="" Text=""></asp:ListItem>
                            <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                            <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                        </asp:DropDownList></td>
                    <td colspan="7"></td>
                </tr>
                <tr>
                    <td colspan="8">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />&nbsp;
                                <asp:Button ID="btExcel" runat="server" Text="Excel" Width="100px" OnClick="btExcel_Click" Enabled="false" />&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="8">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <div id="accordionP">
                            <h3>Produtos</h3>
                            <div>
                                <table border="0" cellpadding="0" class="tb" width="100%">
                                    <tr>
                                        <td style="width: 100%;">
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound"
                                                    OnDataBound="gvProduto_DataBound"
                                                    OnSorting="gvProduto_Sorting" AllowSorting="true" ShowFooter="true"
                                                    DataKeyNames="">
                                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" Font-Size="Smaller"></HeaderStyle>
                                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Size="Smaller" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="DESC_COLECAO" HeaderText="Coleção" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="DESC_COLECAO" ItemStyle-Font-Size="Smaller" />
                                                        <asp:BoundField DataField="DESC_COLECAO_LINX" HeaderText="Coleção Linx" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="DESC_COLECAO_LINX" ItemStyle-Font-Size="Smaller" />
                                                        <asp:BoundField DataField="GRUPO_PRODUTO" HeaderText="Grupo Produto" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="GRUPO_PRODUTO" ItemStyle-Font-Size="Smaller" />
                                                        <asp:BoundField DataField="GRIFFE" HeaderText="Griffe" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="GRIFFE" ItemStyle-Font-Size="Smaller" />
                                                        <asp:BoundField DataField="CAT_MAGENTO" HeaderText="Categoria" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="CAT_MAGENTO" ItemStyle-Font-Size="Smaller" />
                                                        <asp:BoundField DataField="PRODUTO" HeaderText="Produto" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="PRODUTO" ItemStyle-Font-Size="Smaller" />
                                                        <asp:BoundField DataField="NOME" HeaderText="Nome" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="NOME" ItemStyle-Font-Size="Smaller" />
                                                        <asp:BoundField DataField="NOME_MAG" HeaderText="Nome Mag" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="NOME_MAG" ItemStyle-Font-Size="Smaller" />
                                                        <asp:BoundField DataField="DESC_COR" HeaderText="Cor" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="DESC_COR" ItemStyle-Font-Size="Smaller" />

                                                        <asp:BoundField DataField="QTDE_RESERVADA" HeaderText="Reserv" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="QTDE_RESERVADA" ItemStyle-Font-Size="Smaller" />

                                                        <asp:BoundField DataField="QTDE_MAG" HeaderText="Mag" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="QTDE_MAG" ItemStyle-Font-Size="Smaller" />
                                                        <asp:BoundField DataField="QTDE_LINX" HeaderText="Linx" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="QTDE_LINX" ItemStyle-Font-Size="Smaller" />

                                                        <asp:BoundField DataField="OBSERVACAO" HeaderText="Obs" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="OBSERVACAO" ItemStyle-Font-Size="Smaller" />

                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btDesabilitarProduto" runat="server" Text="Desabilitar Produto" Width="100px" OnClick="btDesabilitarProduto_Click" />
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
            <asp:Panel ID="pnlExcel" runat="server" Visible="false">
                <asp:GridView ID="gvExcel" runat="server" Width="100%" AutoGenerateColumns="False"
                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound"
                    OnDataBound="gvProduto_DataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DESC_COLECAO" HeaderText="Coleção" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="DESC_COLECAO" />
                        <asp:BoundField DataField="DESC_COLECAO_LINX" HeaderText="Coleção Linx" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="DESC_COLECAO_LINX" />
                        <asp:BoundField DataField="GRUPO_PRODUTO" HeaderText="Grupo Produto" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="GRUPO_PRODUTO" />
                        <asp:BoundField DataField="GRIFFE" HeaderText="Griffe" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="GRIFFE" />
                        <asp:BoundField DataField="PRODUTO" HeaderText="Produto" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="PRODUTO" />
                        <asp:BoundField DataField="NOME" HeaderText="Nome" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="NOME" />
                        <asp:BoundField DataField="NOME_MAG" HeaderText="Nome Mag" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="NOME_MAG" />
                        <asp:BoundField DataField="COR" HeaderText="Cor" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="COR" />
                        <asp:BoundField DataField="DESC_COR" HeaderText="Descrição Cor" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="DESC_COR" />
                        <asp:BoundField DataField="QTDE_MAG" HeaderText="Estoque Mag" HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="QTDE_MAG" />
                        <asp:BoundField DataField="QTDE_LINX" HeaderText="Estoque Linx" HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="QTDE_LINX" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </fieldset>
    </div>
</asp:Content>
