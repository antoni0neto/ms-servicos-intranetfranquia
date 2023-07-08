<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultMenuGerenteLoja.aspx.cs" Inherits="Relatorios.DefaultMenuGerenteLoja" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .login
        {
            height: 320px;
            width: 280px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="login">
        <table border="1" class="style1">
            <tr>
                <td>
                    <fieldset class="login">
                        <legend>Gerente de Loja</legend>
                        <asp:Menu ID="Menu1" runat="server" StaticDisplayLevels="3">
                            <Items>
                                <asp:MenuItem NavigateUrl="~/ListaClienteFidelidade.aspx" Text="10. Fidelidade Cliente" Value="B_10"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/MaisVendidos.aspx" Text="8. Os mais vendidos" Value="I_8"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/Farol.aspx" Text="11. Farol" Value="B_11"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/VendasPorCotaLoja.aspx" Text="5. Venda/Cotas por Loja" Value="J_5"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/CotasPorLoja.aspx" Text="3. Rel. Cota Mensal por Loja" Value="I_3"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/Desempenho.aspx" Text="1. Desempenho de Vendas" Value="J_1"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/VendaPorVendedor.aspx" Text="1. Vendas por Vendedor" Value="J_1"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/AnaliseAtendimento.aspx" Text="2. Análise Hora de Atend." Value="J_2"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/AnaliseAtendimentoDia.aspx" Text="3. Análise Hr Atend. Dia" Value="J_3"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/ValeMercadoriaFuncionario.aspx" Text="1. Vale Mercadoria Funcionário" Value="J_1"></asp:MenuItem>
                            </Items>
                        </asp:Menu>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
