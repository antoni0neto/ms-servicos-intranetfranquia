<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ErroEsperaBanco.aspx.cs" Inherits="Relatorios.ErroEsperaBanco" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Label ID="Label1" runat="server" 
        Text="Acesso a Banco Sobrecarregado ! Tente mais Tarde !"></asp:Label>
    <br />
    <br />
    <asp:Button ID="btOk" runat="server" onclick="btOk_Click" 
        Text="Ok" Width="63px" />

        <asp:Literal runat="server" ID="LiteralErro"></asp:Literal>
</asp:Content>

