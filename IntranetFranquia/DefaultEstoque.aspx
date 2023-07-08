<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultEstoque.aspx.cs" Inherits="Relatorios.DefaultEstoque" %>

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
            <legend>Estoque</legend>
            <asp:Menu ID="Menu6" runat="server" StaticDisplayLevels="3">
                <Items>
                    <asp:MenuItem NavigateUrl="~/AtualizaEstoque.aspx" Text="1. Atualiza Movimento Estoque" Value="D_1"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/AtualizaEstoqueLoja.aspx" Text="2. Atualiza Movimento Estoque Lj" Value="D_2"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/ControleEstoqueLoja.aspx" Text="3. Controle Estoque Loja" Value="D_3"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/VendaEstoque.aspx" Text="4. Relatório Vendas x Estoque" Value="D_4"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/EntradaProduto.aspx" Text="5. Entrada Produtos em Loja" Value="D_5"></asp:MenuItem>
                </Items>
            </asp:Menu>
        </fieldset>
    </div>
</asp:Content>
