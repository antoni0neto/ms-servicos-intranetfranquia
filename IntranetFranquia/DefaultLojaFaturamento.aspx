<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultLojaFaturamento.aspx.cs" Inherits="Relatorios.DefaultLojaFaturamento" %>

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
            <legend>Loja - Faturamento</legend>
            <asp:Menu ID="Menu13" runat="server" StaticDisplayLevels="3">
                <Items>
                    <asp:MenuItem NavigateUrl="~/FechamentoCaixaNovo.aspx" Text="1. Fechamento de Caixa" Value="M_1"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/FechamentoFundoFixo.aspx" Text="2. Fechamento Fundo Fixo" Value="M_2"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/DepositoBanco.aspx" Text="3. Depósito em Banco" Value="M_3"></asp:MenuItem>
                </Items>
            </asp:Menu>
        </fieldset>
    </div>
</asp:Content>
