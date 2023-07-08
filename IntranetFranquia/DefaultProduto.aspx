<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultProduto.aspx.cs" Inherits="Relatorios.DefaultProduto" %>

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
            <legend>Produto</legend>
            <asp:Menu ID="Menu9" runat="server" StaticDisplayLevels="3">
                <Items>
                    <asp:MenuItem NavigateUrl="~/UploadImagemProduto.aspx" Text="1. Disponibiliza Imagem Produto" Value="E_1"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/MenuProduto.aspx" Text="2. Gestão de Produtos em Loja" Value="E_2"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/MaisVendidos.aspx" Text="3. Os mais vendidos" Value="E_3"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/MenuProduto.aspx" Text="4. Consulta Movimento Produto" Value="E_4"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/VendaProduto.aspx" Text="5. Saída de Produtos da Loja" Value="E_5"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/MenuProduto.aspx" Text="6. Gestão de Produtos em Loja" Value="E_6"></asp:MenuItem>
                </Items>
            </asp:Menu>
        </fieldset>
    </div>
</asp:Content>
