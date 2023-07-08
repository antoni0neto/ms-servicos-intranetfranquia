<%@ Page Title="Intranet" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="contabil_seguro_dre_menu.aspx.cs" Inherits="Relatorios.contabil_seguro_dre_menu" %>

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
                                    <fieldset style="height: 200px;">
                                        <legend>Contabilidade/Fiscal/Seguro</legend>
                                        <asp:Menu ID="mnuFinanceiro" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_menu.aspx" Text="1. Módulo de Contabilidade"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_fiscal/fisc_menu.aspx" Text="2. Módulo Fiscal"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_seguro/seg_menu.aspx" Text="3. Módulo de Seguros" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_acomp_mensal/acomp_menu.aspx" Text="4. Módulo Acompanhamento Mensal"></asp:MenuItem>
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
