<%@ Page Title="Estoque Lojas" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="gerloja_estoque.aspx.cs" Inherits="Relatorios.gerloja_estoque"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Estoque&nbsp;&nbsp;>&nbsp;&nbsp;Estoque Lojas</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Estoque Lojas"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="2">Filial</td>
                        <td colspan="2">Coleção</td>
                        <td>Grupo</td>
                        <td>Griffe</td>
                        <td>Produto</td>
                        <td>Cor</td>
                        <td>Tipo</td>
                    </tr>
                    <tr>
                        <td colspan="2" style="width: 260px;">
                            <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Width="254px">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2" style="width: 260px;">
                            <asp:DropDownList ID="ddlColecoes" runat="server" Width="254px" Height="21px" DataTextField="DESC_COLECAO"
                                DataValueField="COLECAO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 170px;">
                            <asp:DropDownList ID="ddlGrupo" runat="server" Width="164px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                DataValueField="GRUPO_PRODUTO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 230px;" valign="top">
                            <asp:DropDownList ID="ddlGriffe" runat="server" Width="224px" Height="21px" DataTextField="GRIFFE"
                                DataValueField="GRIFFE">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 150px;" valign="top">
                            <asp:TextBox ID="txtProduto" runat="server" Width="136px" MaxLength="10" OnTextChanged="txtProduto_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </td>
                        <td style="width: 220px;" valign="top">
                            <asp:DropDownList ID="ddlCor" runat="server" Width="214px" Height="21px" DataTextField="DESC_COR_PRODUTO"
                                DataValueField="COR_PRODUTO">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTipo" runat="server" Width="150px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="01" Text="PEÇAS"></asp:ListItem>
                                <asp:ListItem Value="02" Text="ACESSÓRIOS"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>De Preço</td>
                        <td>Até Preço</td>
                        <td>De Qtde Estoque</td>
                        <td>Até Qtde Estoque</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 130px;">
                            <asp:TextBox ID="txtPrecoDe" runat="server" Width="116px" MaxLength="10" Style="text-align: right;"
                                onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                        </td>
                        <td style="width: 130px;">
                            <asp:TextBox ID="txtPrecoAte" runat="server" Width="116px" MaxLength="10" Style="text-align: right;"
                                onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                        </td>

                        <td style="width: 130px;">
                            <asp:TextBox ID="txtQtdeDe" runat="server" Width="116px" MaxLength="10" Style="text-align: right;"
                                onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                        </td>
                        <td style="width: 130px;">
                            <asp:TextBox ID="txtQtdeAte" runat="server" Width="116px" MaxLength="10" Style="text-align: right;"
                                onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />
                        </td>
                        <td style="text-align: right;">&nbsp;
                        </td>
                        <td style="text-align: right;">&nbsp;
                        </td>
                        <td style="text-align: right;">&nbsp;
                        </td>
                        <td style="text-align: right;">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvEstoqueLoja" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                    Style="background: white" OnRowDataBound="gvEstoqueLoja_RowDataBound" OnDataBound="gvEstoqueLoja_DataBound"
                                    OnSorting="gvEstoqueLoja_Sorting"
                                    ShowFooter="true" AllowSorting="true">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Filial" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="FILIAL" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="">
                                            <ItemTemplate>
                                                <asp:Label ID="labFilial" runat="server" Text='<%# Bind("FILIAL") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Produto" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="PRODUTO">
                                            <ItemTemplate>
                                                <asp:Label ID="labProduto" runat="server" Text='<%# Bind("PRODUTO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nome" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="155px"
                                            SortExpression="DESC_PRODUTO" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labNome" runat="server" Text='<%# Bind("DESC_PRODUTO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cor" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="DESC_COR" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labCor" runat="server" Text='<%# Bind("DESC_COR") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Venda Total" HeaderStyle-Width="130px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="QTDE_VENDA">
                                            <ItemTemplate>
                                                <asp:Label ID="labQtdeVenda" runat="server" Text='<%# Bind("QTDE_VENDA") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Venda Últ. Semana" HeaderStyle-Width="140px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="QTDE_VENDA_SEMANA">
                                            <ItemTemplate>
                                                <asp:Label ID="labQtdeVendaSemana" runat="server" Text='<%# Bind("QTDE_VENDA_SEMANA") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Estoque" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="ESTOQUE">
                                            <ItemTemplate>
                                                <asp:Label ID="labQtdeEstoque" runat="server" Text='<%# Bind("ESTOQUE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField HeaderText="ES1" DataField="ES1" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="45px" />
                                        <asp:BoundField HeaderText="ES2" DataField="ES2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="45px" />
                                        <asp:BoundField HeaderText="ES3" DataField="ES3" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="45px" />
                                        <asp:BoundField HeaderText="ES4" DataField="ES4" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="45px" />
                                        <asp:BoundField HeaderText="ES5" DataField="ES5" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="45px" />
                                        <asp:BoundField HeaderText="ES6" DataField="ES6" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="45px" />
                                        <asp:BoundField HeaderText="ES7" DataField="ES7" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="45px" />

                                        <asp:TemplateField HeaderText="Preço Loja" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
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
                        <td colspan="9">&nbsp;
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
