<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultMenuFinanceiro.aspx.cs" Inherits="Relatorios.DefaultMenuFinanceiro" %>

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
                        <legend>Financeiro</legend>
                        <asp:Menu ID="Menu1" runat="server" StaticDisplayLevels="3">
                            <Items>
                                <asp:MenuItem NavigateUrl="~/DefaultFinanceiro.aspx" Text="1. Módulo do Financeiro" Value="M_1"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/DefaultNormas.aspx" Text="2. Módulo de Normas e Procedimentos" Value="M_2"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/DefaultSuperLoja.aspx" Text="3. Módulo do Supervisor de Loja" Value="M_3"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/DepositoBanco.aspx" Text="4. Depósito em Banco" Value="M_4"></asp:MenuItem>
                            </Items>
                        </asp:Menu>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
