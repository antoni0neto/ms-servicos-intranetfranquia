<%@ Page Title="Produtos e Blocos" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ecom_ordem_cad_blocoproduto_sembloco.aspx.cs" Inherits="Relatorios.ecom_ordem_cad_blocoproduto_sembloco" %>

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


</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Ordenação de Produtos&nbsp;&nbsp;>&nbsp;&nbsp;Produtos e Blocos</span>
                <div style="float: right; padding: 0;">
                    <a href="ecom_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Produtos e Blocos</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>Categoria Magento
                            </td>
                            <td>Coleção
                            </td>
                            <td>Produto
                            </td>
                            <td>Tecido
                            </td>
                            <td>Cor
                            </td>
                            <td>Cor Fornecedor
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 200px;">
                                <asp:DropDownList ID="ddlCategoriaMag" runat="server" Width="194px" Height="21px" DataTextField="GRUPO"
                                    DataValueField="CODIGO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 200px">
                                <asp:DropDownList ID="ddlColecao" runat="server" Width="194px" Height="21px"
                                    DataTextField="DESC_COLECAO" DataValueField="COLECAO" OnSelectedIndexChanged="ddlColecao_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 200px">
                                <asp:TextBox ID="txtProduto" runat="server" onkeypress="return fnValidarNumero(event);"
                                    Width="190px"></asp:TextBox>
                            </td>

                            <td style="width: 200px">
                                <asp:DropDownList ID="ddlTecido" runat="server" Width="194px" Height="21px" DataTextField="TECIDO_POCKET"
                                    DataValueField="TECIDO_POCKET">
                                </asp:DropDownList>
                            </td>

                            <td style="width: 290px">
                                <asp:DropDownList ID="ddlCorLinx" runat="server" Width="284px" Height="21px" DataTextField="DESC_COR"
                                    DataValueField="COR">
                                </asp:DropDownList>
                            </td>

                            <td>
                                <asp:DropDownList ID="ddlCorFornecedor" runat="server" Width="224px" Height="21px"
                                    DataTextField="FORNECEDOR_COR" DataValueField="FORNECEDOR_COR">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Griffe
                            </td>
                            <td>Sem bloco
                            </td>
                            <td colspan="4">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlGriffe" runat="server" Width="194px" Height="21px" DataTextField="GRIFFE"
                                    DataValueField="GRIFFE">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSemBloco" runat="server" Width="194px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Sem nenhum bloco"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Sem bloco da categoria"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Com bloco da categoria"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td colspan="4">
                                <asp:Button ID="btBuscarFiltro" runat="server" Text="Buscar" Width="100px" OnClick="btBuscarFiltro_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Label ID="labErro" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound" ShowFooter="true"
                                        OnSorting="gvProduto_Sorting" AllowSorting="true"
                                        DataKeyNames="CODIGO, ID_PRODUTO_MAG">
                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="65px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>
                                                    <asp:Image ID="imgProduto" runat="server" Width="65px" ImageAlign="AbsMiddle" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Produto" HeaderStyle-Width="130px" SortExpression="PRODUTO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litProduto" runat="server" Text='<%# Bind("PRODUTO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Nome" HeaderStyle-Width="200px" SortExpression="NOME">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNome" runat="server" Text='<%# Bind("NOME") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Cor" HeaderStyle-Width="150px" SortExpression="DESC_COR">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litCor" runat="server" Text='<%# Bind("DESC_COR") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Griffe" HeaderStyle-Width="150px" SortExpression="GRIFFE">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litGriffe" runat="server" Text='<%# Bind("GRIFFE") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Estoque" HeaderStyle-Width="120px" SortExpression="ESTOQUE">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litEstoque" runat="server" Text='<%# Bind("ESTOQUE") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle"
                                                HeaderText="Blocos" SortExpression="BLOCOS">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litBlocos" runat="server"></asp:Literal>
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
