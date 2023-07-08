<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultManutencaoMensal.aspx.cs" Inherits="Relatorios.DefaultManutencaoMensal" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .login {
            height: 180px;
            width: 280px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="login">
        <fieldset class="login">
            <legend>Aluguéis</legend>
            <asp:Menu ID="mnuAluguel" runat="server" Visible="false">
                <Items>
                    <asp:MenuItem NavigateUrl="~/CadastroAluguel.aspx" Text="1. Cadastro de Aluguel"></asp:MenuItem>
                </Items>
            </asp:Menu>
        </fieldset>
    </div>
</asp:Content>
