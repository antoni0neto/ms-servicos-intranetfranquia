<%@ Page Title="Pré-pedido de Aviamento" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="desenv_pedido_aviamento.aspx.cs" Inherits="Relatorios.desenv_pedido_aviamento" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
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
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Aviamentos&nbsp;&nbsp;>&nbsp;&nbsp;Pré-pedido de Aviamento</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Pré-pedido de Aviamento</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>Coleção</td>
                            <td>Origem</td>
                            <td>Grupo Produto</td>
                            <td>Griffe</td>
                            <td>Fornecedor Material</td>
                            <td>Produto</td>
                            <td>
                                <asp:HiddenField ID="hidCodigoCarrinhoCab" runat="server" Value="" />
                                <asp:HiddenField ID="hidBloqueio" runat="server" Value="N" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 210px;" valign="top">
                                <asp:DropDownList ID="ddlColecoes" runat="server" Width="204px" Height="21px" DataTextField="DESC_COLECAO"
                                    DataValueField="COLECAO" OnSelectedIndexChanged="ddlColecoes_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 200px;" valign="top">
                                <asp:DropDownList ID="ddlOrigem" runat="server" Width="194px" Height="21px" DataTextField="DESCRICAO"
                                    DataValueField="CODIGO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 190px;" valign="top">
                                <asp:DropDownList ID="ddlGrupo" runat="server" Width="184px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                    DataValueField="GRUPO_PRODUTO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 200px">
                                <asp:DropDownList ID="ddlGriffe" runat="server" Width="194px" Height="21px" DataTextField="GRIFFE"
                                    DataValueField="GRIFFE">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 250px;" valign="top">
                                <asp:DropDownList ID="ddlFabricante" runat="server" Width="244px" Height="21px" DataTextField="FORNECEDOR"
                                    DataValueField="FORNECEDOR">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 150px;" valign="top">
                                <asp:TextBox ID="txtProduto" runat="server" Width="140px" MaxLength="10"></asp:TextBox>
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>Grupo Material</td>
                            <td>Subgrupo Material</td>
                            <td colspan="5">Cor Material</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlMaterialGrupo" runat="server" Width="204px" Height="21px"
                                    DataTextField="GRUPO" DataValueField="CODIGO_GRUPO" OnSelectedIndexChanged="ddlMaterialGrupo_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMaterialSubGrupo" runat="server" Width="194px" Height="21px"
                                    DataTextField="SUBGRUPO" DataValueField="CODIGO_SUBGRUPO" OnSelectedIndexChanged="ddlMaterialSubGrupo_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCor" runat="server" Width="184px" Height="21px" DataTextField="DESC_COR"
                                    DataValueField="COR">
                                </asp:DropDownList>
                            </td>
                            <td colspan="4">
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" OnClientClick="DesabilitarBotao(this);" />
                                &nbsp;&nbsp;<asp:Label ID="labMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <fieldset>
                                    <legend>Aviamentos do Pedido</legend>
                                    <div style="text-align: right;">
                                        <asp:Label ID="labPedidoTipo" runat="server" ForeColor="Red" Font-Size="Small"></asp:Label>&nbsp;
                                    </div>
                                    <br />
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvCarrinho" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvCarrinho_RowDataBound" OnDataBound="gvCarrinho_DataBound" ShowFooter="true"
                                            OnSorting="gvCarrinho_Sorting" AllowSorting="true"
                                            DataKeyNames="CODIGO_CARRINHO, TIPO">
                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left"
                                                    HeaderText="Origem" HeaderStyle-Width="" SortExpression="DESC_PEDIDO_ORIGEM">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litOrigem" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Produto" HeaderStyle-Width="" SortExpression="PRODUTO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litProduto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="HB" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" SortExpression="HB">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtHB" runat="server" Width="50px" CssClass="alinharCentro" OnTextChanged="txtHB_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderText="Nome" SortExpression="DESC_PRODUTO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNome" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderText="Cor Produto" HeaderStyle-Width="" SortExpression="COR_FORNECEDOR_PRODUTO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litCor" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderText="Detalhe" HeaderStyle-Width="" SortExpression="DETALHE">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDetalhe" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderText="Aviamento" HeaderStyle-Width="" SortExpression="SUBGRUPO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litSubGrupo" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderText="Cor Material" HeaderStyle-Width="" SortExpression="COR_FORNECEDOR_MATERIAL">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litCorMaterial" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Medida" HeaderStyle-Width="70px" SortExpression="">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litUnidadeMedida" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Quantidade" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" SortExpression="QTDE">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtQtdeMaterial" runat="server" Width="80px" CssClass="alinharCentro" OnTextChanged="txtQtdeMaterial_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Custo Unit" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" SortExpression="CUSTO">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtCusto" runat="server" Width="80px" CssClass="alinharCentro" OnTextChanged="txtCusto_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Desconto Unit" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" SortExpression="DESCONTO_ITEM">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDesconto" runat="server" Width="100px" CssClass="alinharCentro" OnTextChanged="txtDesconto_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Total" HeaderStyle-Width="70px" SortExpression="">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litValTotal" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="65px">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btExcluirItemCarrinho" runat="server" Text="Excluir" Width="65px" Height="21px" OnClick="btExcluirItemCarrinho_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div>
                                        <div style="float: left;">
                                            <br />
                                            <asp:Button ID="btExcluirCarrinho" runat="server" Text="Excluir Carrinho" Width="150px" OnClick="btExcluirCarrinho_Click" />
                                        </div>
                                        <div style="text-align: right;">
                                            <br />
                                            <asp:Label ID="labPedido" runat="server" ForeColor="Red" Text=""></asp:Label>
                                            &nbsp;&nbsp;
                                        <asp:Button ID="btGerarPedido" runat="server" OnClick="btGerarPedido_Click" Width="150px" Text="Gerar Pedido" />&nbsp;
                                        </div>
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <fieldset>
                                    <legend>Adicionar Compra Sem Produto</legend>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td>Grupo
                                            </td>
                                            <td>Subgrupo
                                            </td>
                                            <td>Cor
                                            </td>
                                            <td>Cor Fornecedor
                                            </td>
                                            <td>Quantidade
                                            </td>
                                            <td>Custo
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style="width: 210px;">
                                                <asp:DropDownList ID="ddlMaterialGrupoEstoque" runat="server" Width="204px" Height="21px"
                                                    DataTextField="GRUPO" DataValueField="CODIGO_GRUPO" OnSelectedIndexChanged="ddlMaterialGrupoEstoque_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 200px;">
                                                <asp:DropDownList ID="ddlMaterialSubGrupoEstoque" runat="server" Width="194px" Height="21px"
                                                    DataTextField="SUBGRUPO" DataValueField="CODIGO_SUBGRUPO" OnSelectedIndexChanged="ddlMaterialSubGrupoEstoque_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 190px;">
                                                <asp:DropDownList ID="ddlMaterialCorEstoque" runat="server" Width="184px" Height="21px" DataTextField="DESC_COR"
                                                    DataValueField="COR" OnSelectedIndexChanged="ddlMaterialCorEstoque_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 350px;">
                                                <asp:DropDownList ID="ddlMaterialCorFornecedorEstoque" runat="server" Width="344px" Height="21px"
                                                    DataTextField="REFER_FABRICANTE" DataValueField="REFER_FABRICANTE">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 130px;">
                                                <asp:TextBox ID="txtMaterialQtdeEstoque" runat="server" Width="120px" CssClass="alinharCentro"></asp:TextBox>
                                            </td>
                                            <td style="width: 130px;">
                                                <asp:TextBox ID="txtMaterialCustoEstoque" runat="server" Width="120px" CssClass="alinharCentro"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="btAdicionarQtdeEstoque" runat="server" Text="Adicionar no Carrinho" Width="160px" OnClick="btAdicionarQtdeEstoque_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <fieldset>
                                    <legend>Materiais</legend>

                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td style="width: 200px;">
                                                <asp:Label ID="labMsgEstoque" runat="server" ForeColor="Red" Text=""></asp:Label>
                                            </td>
                                            <td style="text-align: right;">
                                                <asp:HiddenField ID="hidSaldoEstoque" runat="server" Value="" />
                                                <asp:Button ID="btAdicionarDiff" runat="server" Width="140px" Text="Adicionar Saldo" OnClick="btAdicionarDiff_Click" Enabled="false" />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvProdutoFicTec" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvProdutoFicTec_RowDataBound" ShowFooter="true"
                                            OnSorting="gvProdutoFicTec_Sorting" AllowSorting="true">
                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                                    HeaderText="Grupo Produto" HeaderStyle-Width="" SortExpression="GRUPO_PRODUTO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litGrupoProduto" runat="server" Text='<%# Bind("GRUPO_PRODUTO") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                                    HeaderText="Produto" HeaderStyle-Width="" SortExpression="PRODUTO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litModelo" runat="server" Text='<%# Bind("PRODUTO") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                                    HeaderText="Nome" SortExpression="NOME">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNome" runat="server" Text='<%# Bind("NOME") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                                    HeaderText="Cor" HeaderStyle-Width="" SortExpression="COR_FORNECEDOR">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litCor" runat="server" Text='<%# Bind("COR_FORNECEDOR") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                                    HeaderText="Tecido" HeaderStyle-Width="" SortExpression="TECIDO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTecido" runat="server" Text='<%# Bind("TECIDO") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                                    HeaderText="Detalhe" HeaderStyle-Width="" SortExpression="AVIAMENTO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDetalhe" runat="server" Text='<%# Bind("AVIAMENTO") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                                    HeaderText="Aviamento" HeaderStyle-Width="" SortExpression="SUBGRUPO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litSubGrupo" runat="server" Text='<%# Bind("SUBGRUPO") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                                    HeaderText="Cor Material" HeaderStyle-Width="" SortExpression="DESC_COR_MATERIAL">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litCorMaterial" runat="server" Text='<%# Bind("DESC_COR_MATERIAL") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                                    HeaderText="Qtde" HeaderStyle-Width="" SortExpression="CONSUMO_TOTAL">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litQtde" runat="server" Text='<%# Bind("CONSUMO_TOTAL") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                                    HeaderText="Medida" HeaderStyle-Width="" SortExpression="UNIDADE_MEDIDA">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litUnidadeMedida" runat="server" Text='<%# Bind("UNIDADE_MEDIDA") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btAdicionarDOEstoque" runat="server" Text="Estoque" Width="80px" Height="21px" OnClick="btAdicionarDOEstoque_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btAdicionarCarrinho" runat="server" Text="Adicionar" Width="80px" Height="21px" OnClick="btAdicionarCarrinho_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
