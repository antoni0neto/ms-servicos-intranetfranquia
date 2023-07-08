<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="FundoFixoInicializado.aspx.cs" Inherits="Relatorios.FundoFixoInicializado" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Label ID="txtMensagem" runat="server" ForeColor="Red" Font-Size="XX-Large"></asp:Label>
    <asp:Button ID="btOk" runat="server" onclick="btOk_Click" Text="Ok" Width="63px" />
    <asp:Literal runat="server" ID="LiteralErro"></asp:Literal>
</asp:Content>

