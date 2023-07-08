<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultLojaNF.aspx.cs" Inherits="Relatorios.DefaultLojaNF" %>

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
            <legend>Loja - Nota Fiscal</legend>
            <asp:Menu ID="Menu12" runat="server" StaticDisplayLevels="3">
                <Items>
                    <asp:MenuItem NavigateUrl="~/CadastroNotaDefeito.aspx" Text="1. Cadastro NF Defeito" Value="L_1"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/GerirNotaTransferencia.aspx" Text="3. Gestão NF Transf." Value="L_3"></asp:MenuItem>
                </Items>
            </asp:Menu>
        </fieldset>
    </div>
</asp:Content>
