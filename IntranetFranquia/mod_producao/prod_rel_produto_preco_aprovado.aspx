<%@ Page Title="Preços Aprovados" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="prod_rel_produto_preco_aprovado.aspx.cs" Inherits="Relatorios.prod_rel_produto_preco_aprovado"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Custo/Preço de Produto&nbsp;&nbsp;>&nbsp;&nbsp;Preços Aprovados</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Preços Aprovados"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Coleção
                        </td>
                        <td>Grupo
                        </td>
                        <td>Produto
                        </td>
                        <td>Griffe
                        </td>
                        <td>Grupo Tecido
                        </td>
                        <td>SubGrupo Tecido
                        </td>
                        <td>Tipo
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 180px;">
                            <asp:DropDownList ID="ddlColecoesBuscar" runat="server" Width="174px" Height="21px"
                                DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 170px;">
                            <asp:DropDownList ID="ddlGrupo" runat="server" Width="164px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                DataValueField="GRUPO_PRODUTO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 110px;">
                            <asp:TextBox ID="txtProduto" runat="server" Width="100px" MaxLength="5" CssClass="alinharDireita"
                                onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                        </td>
                        <td style="width: 150px;">
                            <asp:DropDownList ID="ddlGriffe" runat="server" Width="144px" Height="21px" DataTextField="GRIFFE"
                                DataValueField="GRIFFE">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 160px;">
                            <asp:DropDownList ID="ddlMaterialGrupo" runat="server" Width="154px" Height="21px"
                                DataTextField="GRUPO" DataValueField="GRUPO" OnSelectedIndexChanged="ddlMaterialGrupo_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 260px;">
                            <asp:DropDownList ID="ddlMaterialSubGrupo" runat="server" Width="254px" Height="21px"
                                DataTextField="SUBGRUPO" DataValueField="SUBGRUPO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 160px;">
                            <asp:DropDownList ID="ddlTipo" runat="server" Width="154px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="N" Text="NACIONAL"></asp:ListItem>
                                <asp:ListItem Value="I" Text="IMPORTADO"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;&nbsp;
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
                                <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                    Style="background: white" OnRowDataBound="gvProduto_RowDataBound" OnSorting="gvProduto_Sorting" AllowSorting="true">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PRODUTO" HeaderText="Produto" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center" SortExpression="PRODUTO" />
                                        <asp:BoundField DataField="NOME" HeaderText="Nome" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left" SortExpression="NOME" />
                                        <asp:BoundField DataField="GRIFFE" HeaderText="Griffe" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left" SortExpression="GRIFFE" />
                                        <asp:BoundField DataField="GRUPO_TECIDO" HeaderText="Grupo Tecido" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left" SortExpression="GRUPO_TECIDO" />
                                        <asp:BoundField DataField="TECIDO" HeaderText="Tecido" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left" SortExpression="TECIDO" />
                                        <asp:TemplateField HeaderText="Preço TO/TL" HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="PRECO">
                                            <ItemTemplate>
                                                <asp:Literal ID="litTL" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Foto Produto" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Image ID="imgFoto" runat="server" BorderWidth="0px" Width="90px" Height="125px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtImprimir" runat="server" Width="15px" ImageUrl="~/Image/print.png"
                                                    ToolTip="Imprimir" OnClick="ibtImprimir_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtPesquisar" runat="server" Width="15px" ImageUrl="~/Image/search.png"
                                                    ToolTip="Pesquisar" OnClick="ibtPesquisar_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
