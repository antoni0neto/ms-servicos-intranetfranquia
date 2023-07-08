<%@ Page Title="Intranet" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="fin_prod_fin_menu.aspx.cs" Inherits="Relatorios.fin_prod_fin_menu" %>

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
                                        <legend>Módulo Geral/Produção</legend>
                                        <asp:Menu ID="mnuCadastro" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_financeiro/fin_prod_menu.aspx" Text="1. Módulo Geral"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_producao/prod_menu.aspx" Text="2. Módulo de Produção"></asp:MenuItem>
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
