<%@ Page Title="Módulo de Representantes" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" 
    CodeBehind="representante_menu.aspx.cs" Inherits="Relatorios.representante_menu" %>

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
                        <legend>Módulo de Representantes</legend>
                        <asp:Panel ID="pnlVendas" runat="server" Visible="false">
                            <table border="0" class="style1">
                                <tr>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 200px;">
                                            <legend>Pedidos</legend>
                                            <asp:Menu ID="mnuPedidos" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_representante/representante_dig_pedidos.aspx?s=0" Text="1. Cadastro de Pedidos"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
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