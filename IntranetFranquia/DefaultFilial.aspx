<%@ Page Title="Menu Loja" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="DefaultFilial.aspx.cs" Inherits="Relatorios.DefaultFilial" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1 {
            width: 100%;
            font-family: Calibri;
            font-size: 14px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MainContent">
    <div class="login">
        <table border="0" class="style1">
            <tr>
                <td>
                    <fieldset class="login">
                        <legend>Menu Loja</legend>
                        <asp:Panel ID="pnlLoja" runat="server">
                            <table border="0" class="style1">
                                <tr>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 350px;">
                                            <legend>Loja</legend>
                                            <asp:Menu ID="mnuLoja" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_adm_loja/admloj_menu.aspx" Text="1. Administração de Loja"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_menu.aspx" Text="2. Gerenciamento de Loja"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_estoque/estoque_menu.aspx" Text="3. Controle de Estoque"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_rh/rh_menu.aspx" Text="4. RH"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_contagem/cont_menu.aspx" Text="5. Contagem" Enabled="true"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td style="width: 465px;">&nbsp;
                                       &nbsp;
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
