<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultRaioX.aspx.cs" Inherits="Relatorios.DefaultRaioX" %>

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
            <legend>Raio X</legend>
            <asp:Menu ID="Menu6" runat="server" StaticDisplayLevels="3">
                <Items>
                    <asp:MenuItem NavigateUrl="~/RaioXProduto.aspx" Text="1. Por Produto" Value="R_1"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/RaioXGrupo.aspx" Text="2. Por Grupo de Produto" Value="R_2"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/RaioXGriffe.aspx" Text="3. Por Griffe" Value="R_3"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/CotasPorLoja.aspx" Text="4. Relatório de Cotas por Loja" Value="R_4"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/ListaMovimentoGrupo.aspx" Text="5. Análise Grupo Desempenho" Value="R_5"></asp:MenuItem>
                </Items>
            </asp:Menu>
        </fieldset>
    </div>
</asp:Content>
