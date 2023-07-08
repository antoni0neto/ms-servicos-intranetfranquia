<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultDesenvolvimento.aspx.cs" Inherits="Relatorios.DefaultDesenvolvimento" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .login
        {
            height: 280px;
            width: 280px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="login">
        <fieldset class="login">
            <legend>Desenvolvimento</legend>
            <asp:Menu ID="Menu10" runat="server" StaticDisplayLevels="3">
                <Items>
                    <asp:MenuItem NavigateUrl="~/CadastroChamadas.aspx" Text="1. Cadastro de Chamadas" Value="B_1"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/UploadImagem.aspx" Text="2. Disponibiliza Imagem p/ Lojas" Value="B_2"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/BranchReportCotas.aspx" Text="3. Relatório Vendas X Cotas" Value="B_3"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/ControleEstoqueLoja.aspx" Text="4. Controle Estoque Loja" Value="B_4"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/VendaEstoque.aspx" Text="5. Relatório Vendas x Estoque" Value="B_5"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/EntradaProduto.aspx" Text="6. Entrada Produtos em Loja" Value="B_6"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/VendasAtacado.aspx" Text="7. Vendas Atacado" Value="B_7"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/VendasAtacadoPorGrupo.aspx" Text="8. Vendas Atacado p/ Grupo Prod." Value="B_8"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/UploadImagemProduto.aspx" Text="9. Disponibiliza Imagem Produto" Value="B_9"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/MenuProduto.aspx" Text="10. Consulta Movimento Produto" Value="B_10"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/VendaProduto.aspx" Text="11. Saída de Produtos da Loja" Value="B_11"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/ConsultaValeMercadoria.aspx" Text="12. Consulta Preço Funcionário" Value="B_12"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/RaioXProduto.aspx" Text="13. RaioX por Produto" Value="B_13"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/RaioXGrupo.aspx" Text="14. RaioX por Grupo Produto" Value="B_14"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/RaioXGriffe.aspx" Text="15. RaioX por Griffe" Value="B_15"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/CotasPorLoja.aspx" Text="16. RaioX Rel. Cotas por Loja" Value="B_16"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/ListaMovimentoGrupo.aspx" Text="17. RaioX Anál.Grupo Desempenho" Value="B_17"></asp:MenuItem>
                </Items>
            </asp:Menu>
        </fieldset>
    </div>
</asp:Content>
