<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultMenuImportacao.aspx.cs" Inherits="Relatorios.DefaultMenuImportacao" %>

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
            <legend>Menu de Importação</legend>
            <asp:Menu ID="Menu4" runat="server" StaticDisplayLevels="3">
                <Items>
                    <asp:MenuItem NavigateUrl="~/ManualUsuarioImportacao.aspx" Text="1. Manual do Módulo de Importação" Value="H_1"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/DefaultImportacao.aspx" Text="2. Módulo de Importação" Value="H_2"></asp:MenuItem>
                </Items>
            </asp:Menu>
        </fieldset>
    </div>
</asp:Content>
