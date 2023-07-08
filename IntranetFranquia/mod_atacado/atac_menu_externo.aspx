<%@ Page Title="Módulo do Representante" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="atac_menu_externo.aspx.cs" Inherits="Relatorios.atac_menu_externo" %>

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
                        <legend>Módulo do Representante</legend>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Clientes</legend>
                                        <asp:Menu ID="mnuRel" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_atacado/atac_venda.aspx?t=2" Text="1. Vendas" Enabled="true"></asp:MenuItem>
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
