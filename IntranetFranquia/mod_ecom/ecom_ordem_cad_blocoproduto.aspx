<%@ Page Title="Adicionar Produtos no Bloco" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ecom_ordem_cad_blocoproduto.aspx.cs" Inherits="Relatorios.ecom_ordem_cad_blocoproduto" %>

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Ordenação de Produtos&nbsp;&nbsp;>&nbsp;&nbsp;Adicionar Produtos no Bloco</span>
                <div style="float: right; padding: 0;">
                    <a href="ecom_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Adicionar Produtos no Bloco</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>Categoria Magento
                                <asp:HiddenField ID="hidTipoCategoria" runat="server" Value="" />
                            </td>
                            <td>Bloco</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 180px;">
                                <asp:DropDownList ID="ddlCategoriaMag" runat="server" Width="174px" Height="21px" DataTextField="GRUPO"
                                    DataValueField="CODIGO" OnSelectedIndexChanged="ddlCategoriaMag_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 180px;">
                                <asp:DropDownList ID="ddlBloco" runat="server" Width="174px" Height="21px" DataTextField="BLOCO" DataValueField="CODIGO">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" OnClientClick="DesabilitarBotao(this);" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;&nbsp;<asp:Label ID="labMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;
                            </td>
                        </tr>
                    </table>
                    <fieldset>
                        <legend>Filtro de Produtos da Categoria</legend>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td>Coleção
                                </td>
                                <td>Grupo Produto</td>
                                <td>Griffe</td>
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
                                <td style="width: 200px">
                                    <asp:DropDownList ID="ddlColecao" runat="server" Width="194px" Height="21px"
                                        DataTextField="DESC_COLECAO" DataValueField="COLECAO" OnSelectedIndexChanged="ddlColecao_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 180px;">
                                    <asp:DropDownList ID="ddlGrupoProduto" runat="server" Width="174px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                        DataValueField="GRUPO_PRODUTO" OnSelectedIndexChanged="ddlGrupoGriffe_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 180px;">
                                    <asp:DropDownList ID="ddlGriffe" runat="server" Width="174px" Height="21px" DataTextField="GRIFFE"
                                        DataValueField="GRIFFE" OnSelectedIndexChanged="ddlGrupoGriffe_SelectedIndexChanged" AutoPostBack="true">
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
                                <td>Tipo Modelagem
                                </td>
                                <td>Tipo Tecido
                                </td>
                                <td>Tipo Manga
                                </td>
                                <td>Tipo Gola
                                </td>
                                <td>Tipo Comprimento
                                </td>
                                <td>Tipo Estilo
                                </td>
                                <td>Na categoria selecionada?
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlTipoModelagem" runat="server" Width="194px" DataValueField="CODIGO" DataTextField="TIPO_MODELAGEM"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTipoTecido" runat="server" Width="174px" DataValueField="CODIGO" DataTextField="TIPO_TECIDO"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTipoManga" runat="server" Width="174px" DataValueField="CODIGO" DataTextField="TIPO_MANGA"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTipoGola" runat="server" Width="144px" DataValueField="CODIGO" DataTextField="TIPO_GOLA"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTipoComprimento" runat="server" Width="284px" DataValueField="CODIGO" DataTextField="TIPO_COMPRIMENTO"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTipoEstilo" runat="server" Width="184px" DataValueField="CODIGO" DataTextField="TIPO_ESTILO"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlBlocoSelec" runat="server" Width="210px">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                        <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                    <asp:Button ID="btBuscarFiltro" runat="server" Text="Filtrar" Width="120px" OnClick="btBuscarFiltro_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound" ShowFooter="true"
                                            OnSorting="gvProduto_Sorting" AllowSorting="true"
                                            DataKeyNames="CODIGO, ID_PRODUTO_MAG">
                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
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
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="65px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgProduto" runat="server" Width="65px" ImageAlign="AbsMiddle" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Produto" HeaderStyle-Width="150px" SortExpression="PRODUTO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litProduto" runat="server" Text='<%# Bind("PRODUTO") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderText="Nome" HeaderStyle-Width="250px" SortExpression="NOME">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNome" runat="server" Text='<%# Bind("NOME") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px"
                                                    HeaderText="Cor" SortExpression="DESC_COR">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litCor" runat="server" Text='<%# Bind("DESC_COR") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px"
                                                    HeaderText="Estoque" SortExpression="ESTOQUE">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litEstoque" runat="server" Text='<%# Bind("ESTOQUE") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px"
                                                    HeaderText="Preço" SortExpression="PRECO_PROMO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litPrecoTL" runat="server" Text=''></asp:Literal>
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
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td colspan="3">
                                <fieldset>
                                    <legend>Produtos no Bloco</legend>
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvCarrinho" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvCarrinho_RowDataBound" ShowFooter="true"
                                            DataKeyNames="CODIGO">
                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="45px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgProduto" runat="server" Width="25px" Height="35px" ImageAlign="AbsMiddle" />
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
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Estoque" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litEstoque" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px"
                                                    HeaderText="Preço" SortExpression="PRECO_PROMO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litPrecoTL" runat="server" Text=''></asp:Literal>
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
                                            <asp:Button ID="btExcluirCarrinho" runat="server" Text="Excluir Bloco" Width="150px" OnClick="btExcluirCarrinho_Click" />
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
