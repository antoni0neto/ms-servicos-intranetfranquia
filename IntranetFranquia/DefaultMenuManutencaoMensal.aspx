<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultMenuManutencaoMensal.aspx.cs" Inherits="Relatorios.DefaultMenuManutencaoMensal" %>

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
                        <legend>Manutenção Mensal</legend>
                        <asp:Menu ID="Menu1" runat="server" StaticDisplayLevels="3">
                            <Items>
                                <asp:MenuItem NavigateUrl="~/DefaultManutencaoMensal.aspx" Text="13. Módulo Manutenção Mensal" Value="M_13"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/DefaultAcompanhamentoMensal.aspx" Text="14. Módulo Acompanhamento Mensal" Value="M_14"></asp:MenuItem>
                            </Items>
                        </asp:Menu>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
