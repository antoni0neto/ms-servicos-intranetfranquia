<%@ Page Title="Intranet" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="estoque_adm_facc_menu.aspx.cs" Inherits="Relatorios.estoque_adm_facc_menu" %>

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
                                        <legend>Fiscal/Facção/Estoque</legend>
                                        <asp:Menu ID="mnuFinanceiro" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/DefaultAdmNF.aspx" Text="1. Módulo Adm Nota Fiscal"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_faccao/facc_menu.aspx" Text="2. Módulo de Facção"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_estoque/estoque_menu.aspx" Text="3. Módulo de Controle de Estoque"></asp:MenuItem>
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
