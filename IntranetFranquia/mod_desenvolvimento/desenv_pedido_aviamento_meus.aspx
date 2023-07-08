<%@ Page Title="Meus Pré-pedidos de Aviamentos" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="desenv_pedido_aviamento_meus.aspx.cs" Inherits="Relatorios.desenv_pedido_aviamento_meus"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <script src="http://code.jquery.com/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Aviamentos&nbsp;&nbsp;>&nbsp;&nbsp;Meus Pré-pedidos de Aviamentos</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1200px; margin-left: 8%;">
                <fieldset>
                    <legend>Meus Pré-pedidos de Aviamentos</legend>
                    <fieldset>
                        <legend>Novo Pré-Pedido</legend>

                        <table border="0" cellpadding="0" cellspacing="0" class="tb" width="100%">
                            <tr>
                                <td>Coleção</td>
                                <td>Pedido</td>
                                <td>Tipo de Compra</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 160px;">
                                    <asp:DropDownList ID="ddlColecao" runat="server" Width="154px" Height="21px" DataTextField="DESC_COLECAO"
                                        DataValueField="COLECAO">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 210px;">
                                    <asp:TextBox ID="txtPedido" runat="server" Text="" Width="200px" MaxLength="16"></asp:TextBox>
                                </td>
                                <td style="width: 210px;">
                                    <asp:DropDownList ID="ddlTipoCompra" runat="server" Width="204px" DataValueField="CODIGO" DataTextField="PERFIL">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btnCriarPedido" runat="server" Width="100px" Text="Criar Pedido" OnClick="btnCriarPedido_Click" />
                                    &nbsp;
                                <asp:Label ID="labErro" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset>
                        <legend>Pré-Pedidos</legend>
                        <table border="0" cellpadding="0" cellspacing="0" class="tb" width="100%">
                            <tr>
                                <td>Pedido</td>
                                <td>Meus?</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 210px;">
                                    <asp:TextBox ID="txtPedidoFiltro" runat="server" Text="" Width="200px" MaxLength="16"></asp:TextBox>
                                </td>
                                <td style="width: 160px;">
                                    <asp:DropDownList ID="ddlMeusFiltro" runat="server" Width="154px" Height="21px">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="S" Text="Sim" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvMeusPedidos" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvMeusPedidos_RowDataBound">
                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Pedido" HeaderStyle-Width="250px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litPedido" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderText="Tipo de Compra" HeaderStyle-Width="250px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTipoCompra" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Data de Abertura" HeaderStyle-Width="250px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDataAbertura" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Data do Pedido" HeaderStyle-Width="250px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDataPedido" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Data Fechamento" HeaderStyle-Width="250px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDataFechamento" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="" HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnExcluir" runat="server" Text="Excluir" Width="100px" OnClick="btnExcluir_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="" HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnAbrirPedido" runat="server" Text="Abrir" Width="100px" OnClick="btnAbrirPedido_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </fieldset>
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
