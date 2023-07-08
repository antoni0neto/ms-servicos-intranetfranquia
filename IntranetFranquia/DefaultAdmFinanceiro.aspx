<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultAdmFinanceiro.aspx.cs" Inherits="Relatorios.DefaultAdmFinanceiro" %>

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
                        <legend>Financeiro</legend>
                        <asp:Menu ID="mnuFinanceiro" runat="server">
                            <Items>
                                <asp:MenuItem NavigateUrl="~/DefaultManutencaoMensal.aspx" Text="1. Módulo Manutenção Mensal"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/mod_acomp_mensal/acomp_menu.aspx" Text="2. Módulo Acompanhamento Mensal"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_menu.aspx" Text="3. Módulo de Contabilidade"></asp:MenuItem>
                            </Items>
                        </asp:Menu>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
