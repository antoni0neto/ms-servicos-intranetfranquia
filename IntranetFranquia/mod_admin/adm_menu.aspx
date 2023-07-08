<%@ Page Title="Módulo de Administração do Site" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="adm_menu.aspx.cs" Inherits="Relatorios.adm_menu" %>

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
                        <legend>Módulo de Administração do Site</legend>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Manutenção de Usuários</legend>
                                        <asp:Menu ID="menuApoio" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_admin/adm_cad_usuario.aspx" Text="1. Cadastro de Usuários"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_admin/adm_relaciona_usuario_loja.aspx" Text="2. Relacionamento de Loja x Usuário"></asp:MenuItem>
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
