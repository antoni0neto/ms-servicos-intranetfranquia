<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultVendasLoja.aspx.cs" Inherits="Relatorios.DefaultVendasLoja" %>

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
            <legend>Análise de Venda</legend>
            <asp:Menu ID="Menu11" runat="server" StaticDisplayLevels="3">
                <Items>
                    <asp:MenuItem NavigateUrl="~/Desempenho.aspx" Text="1. Desempenho de Vendas" Value="K_1"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/DesempenhoVendedor.aspx" Text="2. Desempenho p/ Vendedor" Value="K_2"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/FarolFilial.aspx" Text="3. Farol Loja" Value="K_3"></asp:MenuItem>
                </Items>
            </asp:Menu>
        </fieldset>
    </div>
</asp:Content>
