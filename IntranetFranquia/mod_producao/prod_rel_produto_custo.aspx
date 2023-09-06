<%@ Page Title="Produtos Sem Preço" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="prod_rel_produto_custo.aspx.cs" Inherits="Relatorios.prod_rel_produto_custo"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Custo/Preço de Produto&nbsp;&nbsp;>&nbsp;&nbsp;Produtos Sem Preço</span>
                <div style="float: right; padding: 0;">
                    <a href="prod_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Produtos Sem Preço"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Coleção
                        </td>
                        <td>Produto
                        </td>
                        <td>Nome
                        </td>
                        <td>Mostruário
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 160px;">
                            <asp:DropDownList ID="ddlColecoesBuscar" runat="server" Width="154px" Height="21px"
                                DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 110px;">
                            <asp:TextBox ID="txtProdutoBuscar" runat="server" Width="100px" MaxLength="5" CssClass="alinharDireita"
                                onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                        </td>
                        <td style="width: 150px;">
                            <asp:TextBox ID="txtNomeBuscar" runat="server" Width="140px"></asp:TextBox>
                        </td>
                        <td style="width: 130px;">
                            <asp:DropDownList ID="ddlMostruario" runat="server" Width="124px" Height="21px">
                                <asp:ListItem Value="" Text="" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                            <asp:HiddenField ID="hidTela" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                    Style="background: white" OnRowDataBound="gvProduto_RowDataBound">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Coleção" HeaderStyle-Width="200px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColecao" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                        <asp:BoundField DataField="PRODUTO" HeaderText="Produto" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="GRUPO" HeaderText="Grupo" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="NOME" HeaderText="Nome" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField HeaderText="Mostruário" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litMostruario" runat="server"></asp:Literal>
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
