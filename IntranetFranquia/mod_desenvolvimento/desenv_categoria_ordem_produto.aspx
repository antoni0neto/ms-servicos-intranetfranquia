<%@ Page Title="Adicionar Produtos na Categoria" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="desenv_categoria_ordem_produto.aspx.cs" Inherits="Relatorios.desenv_categoria_ordem_produto" %>

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Análise de Coleção&nbsp;&nbsp;>&nbsp;&nbsp;Adicionar Produtos na Categoria</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Adicionar Produtos na Categoria</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>Coleção</td>
                            <td>Griffe</td>
                            <td>Categoria</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 220px;">
                                <asp:DropDownList ID="ddlColecao" runat="server" Width="214px" Height="21px" DataTextField="DESC_COLECAO"
                                    DataValueField="COLECAO" OnSelectedIndexChanged="ddlColecao_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 200px;">
                                <asp:DropDownList ID="ddlGriffe" runat="server" Width="194px" Height="21px" DataTextField="GRIFFE"
                                    DataValueField="GRIFFE" OnSelectedIndexChanged="ddlGriffe_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 180px;">
                                <asp:DropDownList ID="ddlCategoria" runat="server" Width="174px" Height="21px" DataTextField="NOME"
                                    DataValueField="CODIGO">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" OnClientClick="DesabilitarBotao(this);" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4"><asp:Label ID="labMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;
                            </td>
                        </tr>
                    </table>
                    <fieldset>
                        <legend>Filtro de Produtos da Categoria</legend>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td>Grupo Produto</td>
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
                                <td style="width: 180px;">
                                    <asp:DropDownList ID="ddlGrupoProduto" runat="server" Width="174px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                        DataValueField="GRUPO_PRODUTO">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 150px">
                                    <asp:TextBox ID="txtProduto" runat="server" onkeypress="return fnValidarNumero(event);" Width="140px"></asp:TextBox>
                                </td>
                                <td style="width: 290px">
                                    <asp:DropDownList ID="ddlTecido" runat="server" Width="284px" Height="21px" DataTextField="TECIDO_POCKET"
                                        DataValueField="TECIDO_POCKET">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 190px">
                                    <asp:DropDownList ID="ddlCorLinx" runat="server" Width="184px" Height="21px" DataTextField="DESC_COR"
                                        DataValueField="COR">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCorFornecedor" runat="server" Width="210px" Height="21px"
                                        DataTextField="FORNECEDOR_COR" DataValueField="FORNECEDOR_COR">
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="5">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Button ID="btBuscarFiltro" runat="server" Text="Filtrar" Width="120px" OnClick="btBuscarFiltro_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound" ShowFooter="true"
                                            OnSorting="gvProduto_Sorting" AllowSorting="true"
                                            DataKeyNames="CODIGO">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                            <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btAdicionarCarrinho" runat="server" Text="Adicionar" Width="80px" Height="21px" OnClick="btAdicionarCarrinho_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgProduto" runat="server" Width="70%" ImageAlign="AbsMiddle" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Produto" HeaderStyle-Width="120px" SortExpression="PRODUTO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litProduto" runat="server" Text='<%# Bind("PRODUTO") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderText="Nome" HeaderStyle-Width="220px" SortExpression="DESC_PRODUTO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNome" runat="server" Text='<%# Bind("DESC_PRODUTO") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px"
                                                    HeaderText="Cor" SortExpression="DESC_COR">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litCor" runat="server" Text='<%# Bind("DESC_COR") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px"
                                                    HeaderText="Cor Fornecedor" SortExpression="COR_FORNECEDOR">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litCorFornecedor" runat="server" Text='<%# Bind("COR_FORNECEDOR") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px"
                                                    HeaderText="Tecido" SortExpression="TECIDO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTecido" runat="server" Text='<%# Bind("TECIDO") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="125px"
                                                    HeaderText="Qtde Varejo" SortExpression="QTDE_VAREJO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litQtdeVarejo" runat="server" Text='<%# Bind("QTDE_VAREJO") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="125px"
                                                    HeaderText="Preço" SortExpression="PRECO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litPreco" runat="server" Text=''></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle"
                                                    HeaderText="Categorias" SortExpression="CATEGORIAS">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litCategorias" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td colspan="3">
                                <fieldset>
                                    <legend>Produtos na Categoria</legend>
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvCarrinho" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvCarrinho_RowDataBound" ShowFooter="true"
                                            DataKeyNames="CODIGO">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                            <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgProduto" runat="server" Width="90%" ImageAlign="AbsMiddle" />
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
                                                    HeaderText="Cor Fornecedor" HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litCorFornecedor" runat="server" Text='<%# Bind("COR_FORNECEDOR") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderText="Tecido" HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTecido" runat="server" Text='<%# Bind("TECIDO") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Qtde Varejo" HeaderStyle-Width="170px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litQtdeVarejo" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="170px"
                                                    HeaderText="Preço" SortExpression="">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litPreco" runat="server" Text=''></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btExcluirItemCarrinho" runat="server" Text="Excluir" Width="100px" Height="21px" OnClick="btExcluirItemCarrinho_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div>
                                        <div style="float: left;">
                                            <br />
                                            <asp:Button ID="btExcluirCarrinho" runat="server" Text="Excluir Categoria" Width="150px" OnClick="btExcluirCarrinho_Click" />
                                        </div>
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
