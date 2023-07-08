<%@ Page Title="Intranet" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="fin_prod_menu.aspx.cs" Inherits="Relatorios.fin_prod_menu" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1 {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="login">
        <table border="0" class="style1">
            <tr>
                <td>
                    <fieldset>
                        <legend>Intranet</legend>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Financeiro</legend>
                                        <asp:Menu ID="mnuFinanceiro" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_financeiro/fin_fechamento_caixa_conferencia.aspx?t=2" Text="1. Consultar Fechamento de Caixa"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/VerificarPendenciasFechamentoCaixa.aspx?t=1" Text="2. Verificar Pendências de Caixa"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_financeiro/fin_altera_forma_pgto_funcionario.aspx" Text="3. Alterar Forma de Pagamento Vale Funcionário"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Etiquetas</legend>
                                        <asp:Menu ID="mnuProducao" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_producao/prod_fila_etiqueta.aspx?t=1" Text="1. Liberação Etiqueta de Preço"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_controle_produto/cprod_fila_etiqueta_comp.aspx" Text="2. Liberação Etiqueta de Composição"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="height: 200px;">
                                        <legend>Produtos</legend>
                                        <asp:Menu ID="Menu1" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_controle_produto/cprod_produto_cadastro.aspx?t=2" Text="1. Cadastro de Produtos"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Relatórios</legend>
                                        <asp:Menu ID="mnuRel" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_financeiro/fin_confere_venda_diario.aspx?t=1" Text="1. Total Vendas Dia por Cliente"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_financeiro/fin_confere_venda_mensal.aspx?t=1" Text="2. Total Vendas Mês por Cliente"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_rh/rh_vale_mercadoria_mensal.aspx?t=2" Text="3. Vale Mercadoria"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_financeiro/fin_rel_pontos_hc.aspx?t=1" Text="4. Saldo Handclub - WhatsApp"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_gestao/gest_historico_hand_cliente.aspx?t=5" Text="5. Histórico Cliente Handclub" Enabled="true"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Campanhas</legend>
                                        <asp:Menu ID="mnuCampanha" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_gestao/gest_confere_hand_cliente.aspx?t=2" Text="1. Cliente Handclub Mês Atual"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
