<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultMenuSuperLoja.aspx.cs" Inherits="Relatorios.DefaultMenuSuperLoja" %>

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
                                <asp:MenuItem NavigateUrl="~/DefaultNormas.aspx" Text="1. Módulo de Normas e Procedimentos" Value="M_1"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/DefaultSuperLoja.aspx" Text="2. Módulo do Supervisor de Loja" Value="M_2"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/DefaultRelatorioLoja.aspx" Text="3. Módulo de Relatórios de Loja" Value="M_3"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/DefaultVendasLoja.aspx" Text="4. Módulo de Vendas de Loja" Value="M_4"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/DefaultLojaNF.aspx" Text="5. Módulo de Nota Fiscal de Loja" Value="M_5"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/DefaultLojaFaturamento.aspx" Text="6. Módulo de Faturamento de Loja" Value="M_6"></asp:MenuItem>
                            </Items>
                        </asp:Menu>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
