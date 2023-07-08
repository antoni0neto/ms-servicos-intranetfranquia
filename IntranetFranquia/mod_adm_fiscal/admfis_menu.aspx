<%@ Page Title="Módulo ADM Nota Fiscal" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="admfis_menu.aspx.cs" Inherits="Relatorios.admfis_menu" %>

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
                        <legend>Módulo ADM Nota Fiscal</legend>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Nota Fiscal Transferência/Devolução</legend>
                                        <asp:Menu ID="mnuAdmNF" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_adm_loja/admloj_retirada_mercadoria.aspx" Text="1. Transferência/Devolução de Produtos"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_adm_loja/admloj_retirada_mercadoria_porarquivo.aspx" Text="2. Devolução Interna de Produto (Arquivo)"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_adm_loja/admloj_retirada_mercadoria_carr_retirada.aspx" Text="3. Solicitar Transferência/Devolução" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_adm_loja/admloj_retirada_mercadoria_carr_retirada_pedidos.aspx" Text="4. Pedido de Transferência/Devolução" Enabled="true"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Nota Fiscal Defeito</legend>
                                        <asp:Menu ID="mnuAdmNF2" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_adm_fiscal/admfis_gerar_nota_defeito.aspx" Text="1. Gerar Arquivo de Produtos c/ Defeito"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_adm_fiscal/admfis_verificar_nota_defeito.aspx" Text="2. Gestão Baixas Produtos c/ Defeito"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_adm_fiscal/admfis_total_itens_defeito.aspx" Text="3. Total Produtos Defeito/Loja"></asp:MenuItem>
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
