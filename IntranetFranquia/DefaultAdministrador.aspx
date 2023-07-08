<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultAdministrador.aspx.cs" Inherits="Relatorios.DefaultAdministrador" %>

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
    <div>
        <fieldset class="login">
            <legend>Administrador</legend>
            <asp:Menu ID="Menu1" runat="server" StaticDisplayLevels="3">
                <Items>
                    <asp:MenuItem NavigateUrl="~/Usuario.aspx" Text="1. Cadastramento de Usuários" Value="A_1"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/LigaUsuarioLoja.aspx" Text="2. Relacionamento Loja x Super." Value="A_2"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/UploadImagem.aspx" Text="3. Disponibiliza Imagem p/ Lojas" Value="A_3"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/BranchReportCotas.aspx" Text="4. Relatório Vendas X Cotas" Value="A_4"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/LineReportFranquiaColection.aspx" Text="5. LineReport Coleção Produtos" Value="A_5"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/LineReportFranquiaColectionLiq.aspx" Text="6. LineReport Venda Líquida" Value="A_6"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/ListaClienteFidelidade.aspx" Text="7. Fidelidade Cliente" Value="A_7"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/ExibeImagem.aspx" Text="8. Visualizar Imagens" Value="A_8"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/CadastroChamadas.aspx" Text="9. Cadastro de Chamadas" Value="A_9"></asp:MenuItem>
                </Items>
            </asp:Menu>
        </fieldset>
    </div>
</asp:Content>
