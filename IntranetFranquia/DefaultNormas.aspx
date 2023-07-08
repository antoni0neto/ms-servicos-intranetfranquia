<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultNormas.aspx.cs" Inherits="Relatorios.DefaultNormas" %>

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
            <legend>Normas e Procedimentos</legend>
            <asp:Menu ID="Menu4" runat="server" StaticDisplayLevels="3">
                <Items>
                    <asp:MenuItem NavigateUrl="~/ManualUsuarioImportacao.aspx" Text="1. Imporação de Produtos" Value="H_1"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/ManualFundoFixo.aspx" Text="2. Fundo Fixo" Value="H_2"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/ManualDepositoBanco.aspx" Text="3. Registro Depósito Banco" Value="H_3"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/ManualViagens.aspx" Text="4. Viagens" Value="H_4"></asp:MenuItem>
                </Items>
            </asp:Menu>
        </fieldset>
    </div>
</asp:Content>
