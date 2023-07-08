<%@ Page Title="Módulo de Desenvolvimento/Produto/Produção" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="cprod_producao_menu.aspx.cs" Inherits="Relatorios.cprod_producao_menu" %>

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
                        <legend>Intranet</legend>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Módulo de Desenvolvimento/Produto/Produção</legend>
                                        <asp:menu id="mnuDesenvProdProducao" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_menu.aspx" Text="1. Módulo de Desenvolvimento"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_controle_produto/cprod_menu.aspx" Text="2. Módulo de Controle de Produto"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_producao/prod_menu.aspx" Text="3. Módulo de Produção"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_prodacab/pacab_menu.aspx" Text="4. Módulo de Produto Acabado"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_faccao/facc_menu.aspx" Text="5. Módulo de Facção"></asp:MenuItem>
                                            </Items>
                                        </asp:menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">&nbsp;
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
