<%@ Page Title="Módulo de Produto Acabado" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="pacab_menu.aspx.cs" Inherits="Relatorios.pacab_menu" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1 {
            width: 100%;
            font-family: Calibri;
            font-size: 14px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="login">
        <table border="0" class="style1">
            <tr>
                <td>
                    <fieldset class="login">
                        <legend>Módulo de Produto Acabado</legend>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Pedidos</legend>
                                        <asp:Menu ID="mnuPedido" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_prodacab/pacab_pedido_produtov2.aspx" Text="1. Cadastro Pré-Pedido de Produto"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_prodacab/pacab_pedido_acessorio.aspx" Text="2. Cadastro Pré-Pedido de Acessório"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_prodacab/pacab_pedido_produto_control.aspx" Text="3. Controle de Pedidos"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>



                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Relatórios</legend>
                                        <asp:Menu ID="mnuRel" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_prodacab/pacab_pedido_produto_control_consol.aspx" Text="1. Pedidos de Produto Acabado" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_atacadovarejo_fal.aspx?t=4&s=1&f=HESSO LAVANDERIA" Text="2. Facção por Atacado/Varejo Faltante"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
