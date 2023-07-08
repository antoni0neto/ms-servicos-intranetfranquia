<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultGestor.aspx.cs" Inherits="Relatorios.DefaultGestor" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .login
        {
            height: 280px;
            width: 280px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="login">
        <fieldset class="login">
            <legend>Gestor</legend>
            <asp:Menu ID="Menu10" runat="server" StaticDisplayLevels="3">
                <Items>
                    <asp:MenuItem NavigateUrl="~/LineReportFranquiaColectionLiq.aspx" Text="1. LineReport Venda Líquida" Value="B_1"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/AtualizaEstoque.aspx" Text="2. Atualiza Movimento Estoque" Value="B_2"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/AtualizaEstoqueLoja.aspx" Text="3. Atualiza Movimento Estoque Lj" Value="B_3"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/Farol.aspx" Text="4. Farol" Value="B_4"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/AlteraQtdeSobra.aspx" Text="5. Altera Qtde Sobra" Value="B_5"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/ComparaVendas.aspx" Text="6. Comparáveis (Lojas)" Value="B_6"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/ComparaVendasSuper.aspx" Text="7. Totais Completo Super(Lojas)" Value="B_7"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/ComparaVendasCotas.aspx" Text="8. Farol Vendas x Lojas" Value="B_8"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/ComparaVendasCotasAno.aspx" Text="9. Compara Vendas x Cotas" Value="B_9"></asp:MenuItem>
                </Items>
            </asp:Menu>
        </fieldset>
    </div>
</asp:Content>
