<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultMenuAdmNF.aspx.cs" Inherits="Relatorios.DefaultMenuAdmNF" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .login
        {
            height: 320px;
            width: 280px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="login">
        <table border="1" class="style1">
            <tr>
                <td>
                    <fieldset class="login">
                        <legend>Administrador</legend>
                        <asp:Menu ID="Menu1" runat="server" StaticDisplayLevels="3">
                            <Items>
                                <asp:MenuItem NavigateUrl="~/DefaultAdmNF.aspx" Text="1. Módulo Adm de Nota Fiscal" Value="M_1"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/DefaultNormas.aspx" Text="2. Módulo de Normas e Procedimentos" Value="M_2"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/DefaultLojaNF.aspx" Text="3. Módulo de Nota Fiscal de Loja" Value="M_3"></asp:MenuItem>
                            </Items>
                        </asp:Menu>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
