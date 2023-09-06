<%@ Page Title="Vendas de Produtos Diário" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="gerloja_acessorio_venda.aspx.cs" Inherits="Relatorios.gerloja_acessorio_venda"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Vendas&nbsp;&nbsp;>&nbsp;&nbsp;Vendas de Produtos Diário</span>
                <div style="float: right; padding: 0;">
                    <a href="gerloja_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Vendas de Produtos Diário"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Período Inicial</td>
                        <td>Período Final</td>
                        <td>Tipo Produto</td>
                        <td colspan="4">&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtPeriodoInicial" runat="server" autocomplete="off" Width="170px" MaxLength="10" Style="text-align: right;"
                                onkeypress="return fnValidarData(event);"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPeriodoFinal" runat="server" autocomplete="off" Width="170px" MaxLength="10" Style="text-align: right;"
                                onkeypress="return fnValidarData(event);"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTipo" runat="server" Width="174px" OnSelectedIndexChanged="ddlTipo_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="01" Text="PEÇAS"></asp:ListItem>
                                <asp:ListItem Value="02" Text="ACESSÓRIOS"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td colspan="4">&nbsp;</td>
                    </tr>

                    <tr>
                        <td style="width: 30px;">Coleção Linx
                        </td>
                        <td>Griffe
                        </td>
                        <td>Grupo Produto
                        </td>
                        <td>Produto
                        </td>
                        <td>Nome
                        </td>
                        <td>Tecido
                        </td>
                        <td>Cor Fornecedor
                        </td>
                        <td>Produto Acabado
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 180px;" valign="top">
                            <asp:ListBox ID="lstColecao" runat="server" DataValueField="COLECAO" DataTextField="DESC_COLECAO" SelectionMode="Multiple" Width="174px" Height="140px"></asp:ListBox>
                        </td>
                        <td style="width: 180px;" valign="top">
                            <asp:ListBox ID="lstGriffe" runat="server" DataValueField="GRIFFE" Width="174px" DataTextField="GRIFFE" SelectionMode="Multiple" Height="140px"></asp:ListBox>
                        </td>
                        <td style="width: 180px;" valign="top">
                            <asp:ListBox ID="lstGrupoProduto" runat="server" DataValueField="GRUPO_PRODUTO" DataTextField="GRUPO_PRODUTO" SelectionMode="Multiple" Width="174px" Height="140px"></asp:ListBox>
                        </td>
                        <td style="width: 150px;" valign="top">
                            <asp:TextBox ID="txtProduto" runat="server" Width="140px" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width: 160px;" valign="top">
                            <asp:TextBox ID="txtNome" runat="server" Width="150px" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width: 210px;" valign="top">
                            <asp:DropDownList ID="ddlTecido" runat="server" Width="204px" Height="21px" DataTextField="TECIDO_POCKET"
                                DataValueField="TECIDO_POCKET" OnSelectedIndexChanged="ddlTecido_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 210px;" valign="top">
                            <asp:DropDownList ID="ddlCorFornecedor" runat="server" Width="204px" Height="21px"
                                DataTextField="FORNECEDOR_COR" DataValueField="FORNECEDOR_COR">
                            </asp:DropDownList>
                        </td>
                        <td valign="top">
                            <asp:DropDownList ID="ddlProdutoAcabado" runat="server" Width="155px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Coleção Intranet
                        </td>
                        <td>Origem
                        </td>
                        <td colspan="3">Signed Nome
                        </td>
                        <td>Fornecedor
                        </td>
                        <td>SubGrupo Produto
                        </td>
                        <td>SubCategoria Acessório</td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:DropDownList ID="ddlColecoes" runat="server" Width="174px" Height="21px" DataTextField="DESC_COLECAO"
                                DataValueField="COLECAO" OnSelectedIndexChanged="ddlColecoes_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td valign="top">
                            <asp:DropDownList ID="ddlOrigem" runat="server" Width="174px" Height="21px" DataTextField="DESCRICAO"
                                DataValueField="CODIGO" OnSelectedIndexChanged="ddlOrigem_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td colspan="3" valign="top">
                            <asp:DropDownList ID="ddlSignedNome" runat="server" Width="484px" Height="21px" DataTextField="SIGNED_NOME"
                                DataValueField="SIGNED_NOME">
                            </asp:DropDownList>
                        </td>
                        <td valign="top">
                            <asp:DropDownList ID="ddlFornecedor" runat="server" Width="204px" Height="21px" DataTextField="FORNECEDOR"
                                DataValueField="FORNECEDOR">
                            </asp:DropDownList>
                        </td>
                        <td valign="top">
                            <asp:DropDownList ID="ddlSubGrupoProduto" runat="server" Width="204px" Height="21px" DataTextField="SUBGRUPO_PRODUTO"
                                DataValueField="SUBGRUPO_PRODUTO">
                            </asp:DropDownList>
                        </td>
                        <td valign="top">
                            <asp:DropDownList ID="ddlSubCategoria" runat="server" Width="155px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="03" Text="Normal"></asp:ListItem>
                                <asp:ListItem Value="04" Text="Promoção"></asp:ListItem>
                                <asp:ListItem Value="06" Text="Saldo"></asp:ListItem>
                                <asp:ListItem Value="10" Text="Virado"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvVendaAcessorio" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                    Style="background: white" OnRowDataBound="gvVendaAcessorio_RowDataBound" OnDataBound="gvVendaAcessorio_DataBound"
                                    OnSorting="gvVendaAcessorio_Sorting"
                                    ShowFooter="true" AllowSorting="true">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Foto" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-Font-Size="XX-Small">
                                            <ItemTemplate>
                                                <asp:Image ID="imgProduto" runat="server" Width="100px" ImageAlign="AbsMiddle" AlternateText='<%# String.Concat(Eval("PRODUTO"), Eval("COR_PRODUTO") + ".jpg")  %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Coleção" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="DESC_COLECAO" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labColecao" runat="server" Text='<%# Bind("DESC_COLECAO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Produto" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="PRODUTO">
                                            <ItemTemplate>
                                                <asp:Label ID="labProduto" runat="server" Text='<%# Bind("PRODUTO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cor" HeaderStyle-Width="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="DESC_COR_PRODUTO" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labCor" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nome" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="DESC_PRODUTO" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labNome" runat="server" Text='<%# Bind("DESC_PRODUTO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fabricante" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="FABRICANTE" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labFabricante" runat="server" Text='<%# Bind("FABRICANTE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Venda Período" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="QTDE_LIQ_PERIODO">
                                            <ItemTemplate>
                                                <asp:Label ID="labQtdeLiqPeriodo" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Venda Total" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="QTDE_LIQ_TOTAL">
                                            <ItemTemplate>
                                                <asp:Label ID="labQtdeLiqTotal" runat="server" Text='<%# Bind("QTDE_LIQ_TOTAL") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Estoque" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="ESTOQUE">
                                            <ItemTemplate>
                                                <asp:Label ID="labEstoque" runat="server" Text='<%# Bind("ESTOQUE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Custo" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="CUSTO">
                                            <ItemTemplate>
                                                <asp:Label ID="labCusto" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Preço Original" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="PRECO_ORIGINAL">
                                            <ItemTemplate>
                                                <asp:Label ID="labPrecoOriginal" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Preço Loja" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="PRECO_LOJA">
                                            <ItemTemplate>
                                                <asp:Label ID="labPrecoLoja" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">&nbsp;
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
