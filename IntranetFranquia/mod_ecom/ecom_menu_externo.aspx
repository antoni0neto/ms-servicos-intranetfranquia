<%@ Page Title="Módulo do E-Commerce" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="ecom_menu_externo.aspx.cs" Inherits="Relatorios.ecom_menu_externo" %>

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
                        <legend>Módulo do E-Commerce</legend>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Relatórios</legend>
                                        <asp:Menu ID="mnuRel" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_venda_compara_col_outlet.aspx?t=2" Text="1. Comparativo Vendas - Colecao X Outlet" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_venda_compara_analytics_mag.aspx?t=2" Text="2. Comparativo Vendas - Analytics X Magento" Enabled="true"></asp:MenuItem>
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
