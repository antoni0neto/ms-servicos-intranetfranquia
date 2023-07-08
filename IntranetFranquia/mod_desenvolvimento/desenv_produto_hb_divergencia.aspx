<%@ Page Title="HB/Produto por Pedido" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="desenv_produto_hb_divergencia.aspx.cs" Inherits="Relatorios.desenv_produto_hb_divergencia"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita
        {
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;HB/Produto
                    por Pedido</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="HB/Produto por Pedido"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            Coleção
                        </td>
                        <td>
                            HB
                        </td>
                        <td>
                            Modelo
                        </td>
                        <td>
                            Nome
                        </td>
                        <td>
                            Número do Pedido
                        </td>
                        <td>
                            Mostruário
                        </td>
                        <td>
                            Status
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 150px;">
                            <asp:DropDownList ID="ddlColecoes" runat="server" Width="143px" Height="21px" DataTextField="DESC_COLECAO"
                                DataValueField="COLECAO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 160px;">
                            <asp:TextBox ID="txtHB" runat="server" Width="150px" MaxLength="8" CssClass="alinharDireita"
                                onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                        </td>
                        <td style="width: 160px;">
                            <asp:TextBox ID="txtModelo" runat="server" Width="150px"></asp:TextBox>
                        </td>
                        <td style="width: 160px;">
                            <asp:TextBox ID="txtNome" runat="server" Width="150px"></asp:TextBox>
                        </td>
                        <td style="width: 160px;">
                            <asp:TextBox ID="txtNumeroPedido" runat="server" Width="150px" MaxLength="8" CssClass="alinharDireita"
                                onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                        </td>
                        <td style="width: 150px;">
                            <asp:DropDownList ID="ddlMostruario" runat="server" Width="143px" Height="21px">
                                <asp:ListItem Value="" Text="" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="S" Text="SIM"></asp:ListItem>
                                <asp:ListItem Value="N" Text="NÃO"></asp:ListItem>
                                <asp:ListItem Value="V" Text="VAZIO"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 300px;">
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="293px" Height="21px">
                                <asp:ListItem Value="" Text="" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="PEDIDO COM HB E COM MODELO" Text="PEDIDO COM HB E COM MODELO"></asp:ListItem>
                                <asp:ListItem Value="PEDIDO COM HB E SEM MODELO" Text="PEDIDO COM HB E SEM MODELO"></asp:ListItem>
                                <asp:ListItem Value="PLANEJADO" Text="PLANEJADO"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvProdutoHB" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvProdutoHB_RowDataBound">
                                    <HeaderStyle BackColor="GradientActiveCaption" />
                                    <FooterStyle BackColor="GradientActiveCaption" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="NUMERO_PEDIDO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="No. Pedido" HeaderStyle-Width="90px" />
                                        <asp:BoundField DataField="FORNECEDOR_PEDIDO" HeaderText="Fornecedor" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="170px" />
                                        <asp:BoundField DataField="MODELO" HeaderText="Modelo" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" />
                                        <asp:BoundField DataField="COLECAO" HeaderText="Coleção" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="140px" />
                                        <asp:BoundField DataField="HB" HeaderText="HB" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="NOME" HeaderText="Nome" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="180px" />
                                        <asp:BoundField DataField="FORNECEDOR_HB" HeaderText="Fornecedor" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                                        <asp:BoundField DataField="TECIDO" HeaderText="Tecido" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="COR" HeaderText="Cor" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="170px" />
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
