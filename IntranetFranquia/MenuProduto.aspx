<%@ Page Title="Menu de Produto" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="MenuProduto.aspx.cs" Inherits="Relatorios.MenuProduto" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento
        {
            position: relative;
            float: left;
        }
        .style1
        {
            width: 100%;
            background-color:White;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="accountInfo">
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
            <asp:Button runat="server" ID="btGeraMovimentoProduto" 
                Text="Gera Movimento de Produto em Loja" 
                OnClick="btGeraMovimentoProduto_Click" style="height: 26px" Width="280px"/>
            <asp:Button runat="server" ID="btListaMovimentoProduto" 
                Text="Lista Movimento de Produtos em Loja" 
                OnClick="btListaMovimentoProduto_Click" style="height: 26px" Width="280px"/>
            <asp:Button runat="server" ID="btListaProduto" 
                Text="Lista Movimento de Produto em Loja" 
                OnClick="btListaProduto_Click" style="height: 26px" Width="280px"/>
            <asp:Button runat="server" ID="btImprimeMovimentoProduto" 
                Text="Imprime Movimento de Produtos em Loja" 
                OnClick="btImprimeMovimentoProduto_Click" style="height: 26px" Width="280px"/>
        </fieldset>
    </div>
</asp:Content>
