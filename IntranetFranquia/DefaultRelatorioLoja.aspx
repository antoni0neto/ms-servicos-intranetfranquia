<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultRelatorioLoja.aspx.cs" Inherits="Relatorios.DefaultRelatorioLoja" %>

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
        <fieldset class="Financeiro">
            <legend>Relatórios de Loja</legend>
            <asp:Menu ID="Menu2" runat="server" StaticDisplayLevels="3">
                <Items>
                    <asp:MenuItem NavigateUrl="~/VendaPorVendedor.aspx" Text="1. Vendas por Vendedor" Value="J_1"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/AnaliseAtendimento.aspx" Text="2. Análise Hora de Atend." Value="J_2"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/AnaliseAtendimentoDia.aspx" Text="3. Análise Hr Atend. Dia" Value="J_3"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/CotasPorLoja.aspx" Text="4. Cotas por Loja" Value="J_4"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/VendasPorCotaLoja.aspx" Text="5. Venda/Cotas por Loja" Value="J_5"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/ConsultaValeMercadoria.aspx" Text="6. Consulta Vale Mercadoria" Value="J_6"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/RelatorioAniversario.aspx" Text="7. Relatório de Aniversáriantes" Value="J_7"></asp:MenuItem>
                </Items>
            </asp:Menu>
        </fieldset>
    </div>
</asp:Content>
