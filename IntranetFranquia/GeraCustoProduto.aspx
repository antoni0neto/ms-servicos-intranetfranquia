<%@ Page Title="Raio X de Produto" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="GeraCustoProduto.aspx.cs" Inherits="Relatorios.GeraCustoProduto" %>

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
        }
        </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="accountInfo">
        <fieldset class="login">
        </fieldset>
        <asp:Button runat="server" ID="btMovimento" Text="Gera Custo dos Produtos" OnClick="btMovimento_Click"/>
        <asp:Label runat="server" ID="lblMensagem" ForeColor="Red"></asp:Label>
    </div>
</asp:Content>
