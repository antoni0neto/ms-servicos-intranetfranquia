<%@ Page Title="Módulo do E-Commerce" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="photo_menu.aspx.cs" Inherits="Relatorios.photo_menu" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="login">
        <table border="0" class="style1">
            <tr>
                <td>
                    <fieldset class="login">
                        <legend>Fotos</legend>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Cadastros</legend>
                                        <asp:Menu ID="mnuCadastro" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/photo_select/cad_phXHD123/photo_register.aspx" Text="1. Cadastro de Fotos"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/photo_select/cad_phXHD123/photo_register_transfer.aspx" Text="2. Transferência de Fotos para Fotos"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/photo_select/cad_phXHD123/photo_select_brand.aspx" Text="3. Seleção de Fotos"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
