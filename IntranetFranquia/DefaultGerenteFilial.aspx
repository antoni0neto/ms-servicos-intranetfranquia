<%@ Page Title="Módulo de Gerenciamento de Loja" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="DefaultGerenteFilial.aspx.cs" Inherits="Relatorios.DefaultGerenteFilial" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1
        {
            width: 100%;
            font-family: Calibri;
            font-size: 14px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="login">
        <table border="0" class="style1">
            <tr>
                <td>
                    <fieldset class="login">
                        <legend>Módulo de Gerenciamento de Loja</legend>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Desempenho</legend>
                                        <asp:Menu ID="menuDesempenho" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/ListaClienteFidelidade.aspx" Text="1. Fidelidade Cliente">
                                                </asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/MaisVendidos.aspx" Text="2. Os mais vendidos"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/Farol.aspx" Text="3. Farol"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/VendasPorCotaLoja.aspx" Text="4. Venda/Cotas por Loja">
                                                </asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/CotasPorLoja.aspx" Text="5. Rel. Cota Mensal por Loja">
                                                </asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Vendedores</legend>
                                        <asp:Menu ID="menuVendedores" runat="server" StaticDisplayLevels="3">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/Desempenho.aspx" Text="1. Desempenho de Vendas"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/VendaPorVendedor.aspx" Text="2. Vendas por Vendedor">
                                                </asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/AnaliseAtendimento.aspx" Text="3. Análise Hora de Atend.">
                                                </asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/AnaliseAtendimentoDia.aspx" Text="4. Análise Hr Atend. Dia">
                                                </asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/ValeMercadoriaFuncionario.aspx" Text="5. Vale Mercadoria Funcionário">
                                                </asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/ConsultaValeMercadoria.aspx" Text="6. Consulta Preço Funcionário">
                                                </asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="height: 200px;">
                                        <legend>Apoio</legend>
                                        <asp:Menu ID="menuApoio" runat="server" StaticDisplayLevels="3">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/ManualFundoFixo.aspx" Text="1. Fundo Fixo"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/ManualDepositoBanco.aspx" Text="2. Registro Depósito Banco">
                                                </asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/ManualViagens.aspx" Text="3. Viagens"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
