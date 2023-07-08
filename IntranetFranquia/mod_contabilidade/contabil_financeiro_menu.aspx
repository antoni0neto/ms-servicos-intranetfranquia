<%@ Page Title="Intranet" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="contabil_financeiro_menu.aspx.cs" Inherits="Relatorios.contabil_financeiro_menu" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1 {
            width: 100%;
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
                                    <fieldset>
                                        <legend>Financeiro/Contabilidade</legend>
                                        <asp:Menu ID="mnuFinanceiroContabil" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/DefaultFinanceiro.aspx" Text="1. Módulo do Financeiro"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_menu.aspx" Text="2. Módulo de Contabilidade"></asp:MenuItem>

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
