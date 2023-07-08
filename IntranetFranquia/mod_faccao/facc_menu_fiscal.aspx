<%@ Page Title="Módulo de Facção/Fiscal" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="facc_menu_fiscal.aspx.cs" Inherits="Relatorios.facc_menu_fiscal" %>

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
                                        <legend>Módulo de Facção/Fiscal</legend>
                                        <asp:Menu ID="mnuFiscFacc" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_fiscal/fisc_menu.aspx" Text="1. Módulo do Fiscal"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_faccao/facc_menu.aspx" Text="2. Módulo de Facção"></asp:MenuItem>
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
