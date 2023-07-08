<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultSuperLoja.aspx.cs" Inherits="Relatorios.DefaultSuperLoja" %>

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
            <legend>Supervisor de Loja</legend>
            <asp:Menu ID="Menu3" runat="server" StaticDisplayLevels="3">
                <Items>
                    <asp:MenuItem NavigateUrl="~/Desempenho.aspx" Text="1. Desempenho de Vendas" Value="I_1"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/ContaItensProduto.aspx" Text="2. Resumo Venda - Qtde Itens" Value="I_2"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/CotasPorLoja.aspx" Text="3. Rel. Cota Mensal por Loja" Value="I_3"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/VerificarFechamentoCaixa.aspx" Text="4. Verificar Fechamento Caixa" Value="I_4"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/VerificarDespesas.aspx" Text="5. Verificar Despesas Caixa" Value="I_5"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/VerificarReceitas.aspx" Text="6. Verificar Receitas Caixa" Value="I_6"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/DesempenhoVendedor.aspx" Text="7. Desempenho por Vendedor" Value="I_7"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/MaisVendidos.aspx" Text="8. Os mais vendidos" Value="I_8"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/FarolSupervisor.aspx" Text="9. Farol Supervisor" Value="I_9"></asp:MenuItem>
                </Items>
            </asp:Menu>
        </fieldset>
    </div>
</asp:Content>
