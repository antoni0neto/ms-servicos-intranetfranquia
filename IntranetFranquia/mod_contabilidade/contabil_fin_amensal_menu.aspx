<%@ Page Title="Intranet" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="contabil_fin_amensal_menu.aspx.cs" Inherits="Relatorios.contabil_fin_amensal_menu" %>

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
                                        <legend>Acompanhamento Mensal/Contabilidade/Financeiro</legend>
                                        <asp:Menu ID="mnuFinanceiro" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_manut_mensal/manutm_menu.aspx" Text="1. Módulo Manutenção Mensal"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_acomp_mensal/acomp_menu.aspx" Text="2. Módulo Acompanhamento Mensal"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_menu.aspx" Text="3. Módulo de Contabilidade"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_financeiro/fin_menu.aspx" Text="4. Módulo Financeiro"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_fiscal/fisc_menu.aspx" Text="5. Módulo Fiscal"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_faccao/facc_menu.aspx" Text="6. Módulo Facção"></asp:MenuItem>
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
