<%@ Page Title="Módulo de Gerenciamento de Loja/Marketing" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="mkt_ger_loja_menu.aspx.cs" Inherits="Relatorios.mkt_ger_loja_menu" %>

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
                                        <legend>Módulo de Gerenciamento de Loja/Desenvolvimento/Produção</legend>
                                        <asp:Menu ID="mnuMarketing" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_menu.aspx" Text="1. Módulo Gerenciamento de Loja"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_menu.aspx" Text="2. Módulo de Desenvolvimento"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_producao/prod_menu.aspx" Text="3. Módulo de Produção"></asp:MenuItem>
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
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
