<%@ Page Title="Módulo de Controle de Estoque" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="estoque_menu.aspx.cs" Inherits="Relatorios.estoque_menu" %>

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
                        <legend>Módulo de Controle de Estoque</legend>
                        <asp:Panel ID="pnlRetaguarda" runat="server" Visible="false">
                            <table border="0" class="style1">
                                <tr>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 200px;">
                                            <legend>Estoque Loja</legend>
                                            <asp:Menu ID="menuEstRet" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_estoque/estoque_mercadoria_recebimento.aspx" Text="1. Recebimento de Mercadoria"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_estoque/estoque_mercadoria_conferencia.aspx" Text="2. Conferência de Mercadoria"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_estoque/estoque_mercadoria_rel.aspx" Text="3. Relatório de Divergência"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_estoque.aspx?t=2" Text="4. Estoque Lojas" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_estoque/estoque_entrada_merc_externo.aspx?t=2" Text="5. Entrada de Mercadoria - Externo" Enabled="true"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 200px;">
                                            <legend>Estoque Fábrica</legend>
                                            <asp:Menu ID="mnuEstFabrica" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_estoque/estoque_mercadoria_recebimento_fabrica.aspx" Text="1. Recebimento de Mercadoria Fábrica" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_estoque/estoque_mercadoria_rel_fabrica.aspx" Text="2. Relatório de Mercadoria Fábrica"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset style="height: 200px;">
                                            <legend>Notas Fiscais</legend>
                                            <asp:Menu ID="mnuBaixaEntrada" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_estoque/estoque_baixa_nf_entrada_prod.aspx" Text="1. Baixa NF Entrada Produto Acabado"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlLoja" runat="server" Visible="false">
                            <table border="0" class="style1">
                                <tr>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 200px;">
                                            <legend>Estoque Loja</legend>
                                            <asp:Menu ID="menuEstRetLoja" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_estoque/estoque_mercadoria_recebimento.aspx" Text="1. Recebimento de Mercadoria"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_estoque/estoque_mercadoria_rel.aspx" Text="2. Relatório de Mercadoria"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_estoque/estoque_entrada_merc_externo.aspx?t=2" Text="3. Entrada de Mercadoria - Externo" Enabled="true"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>

                                        </fieldset>
                                    </td>
                                    <td style="width: 465px;">&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
