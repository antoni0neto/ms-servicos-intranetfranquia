<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="FinalizadoComSucesso.aspx.cs" Inherits="Relatorios.FinalizadoComSucesso" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Label ID="Label1" runat="server" Text="GRAVADO COM SUCESSO !!!" ForeColor="Red"></asp:Label>
    <asp:Button ID="btOk" runat="server" onclick="btOk_Click" Text="Ok" Width="63px" />
    <asp:Literal runat="server" ID="LiteralErro"></asp:Literal>
</asp:Content>

