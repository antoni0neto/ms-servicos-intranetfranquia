<%@ Page Title="Módulo do CAD" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="prod_menu_cad.aspx.cs" Inherits="Relatorios.prod_menu_cad" %>

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
                        <legend>Módulo de CAD</legend>
                        <asp:Panel ID="pnlCAD" runat="server">
                            <table border="0" class="style1">
                                <tr>
                                    <td style="width: 480px;">
                                        <fieldset style="height: 200px;">
                                            <legend>CAD</legend>
                                            <asp:Menu ID="menuCAD" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_con_hb_filtro.aspx?d=1&t=4" Text="1. HB Consulta"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_pocket_con.aspx?t=4" Text="2. Consultar Coleção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_cad_molde.aspx?t=4" Text="3. Molde"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td style="width: 480px;">&nbsp;
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
