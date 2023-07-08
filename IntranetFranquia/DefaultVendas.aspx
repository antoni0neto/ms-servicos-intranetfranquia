<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultVendas.aspx.cs" Inherits="Relatorios.DefaultVendas" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .login
        {
            height: 180px;
            width: 280px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="login">
        <fieldset class="login">
            <legend>Vendas</legend>
            <asp:Menu ID="Menu7" runat="server" StaticDisplayLevels="3">
                <Items>
                    <asp:MenuItem NavigateUrl="~/VendasLoja.aspx" Text="1. Vendas por Loja" Value="F_1"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/AnaliseVenda.aspx" Text="2. Análise Vendas" Value="F_2"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/Farol.aspx" Text="3. Farol" Value="F_3"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/DesempenhoVendedor.aspx" Text="4. Desempenho Vendedor" Value="F_4"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/CriticaLoja.aspx" Text="5. Vendas c/ Divergência Cliente" Value="F_5"></asp:MenuItem>
                </Items>
            </asp:Menu>
        </fieldset>
    </div>
</asp:Content>
