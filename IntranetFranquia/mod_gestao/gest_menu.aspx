<%@ Page Title="Módulo de Gestão" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="gest_menu.aspx.cs" Inherits="Relatorios.gest_menu" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1 {
            width: 100%;
            font-family: Calibri;
            font-size: 14px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="MainContent">
    <div class="login">
        <table border="0" class="style1">
            <tr>
                <td>
                    <fieldset class="login">
                        <legend>Módulo de Gestão</legend>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 295px;">
                                        <legend>Relatórios de Vendas</legend>
                                        <asp:Menu ID="menuRelatorioGestao" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_gestao/gest_farol.aspx" Text="1. Farol"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_gestao/gest_loja_compara_venda.aspx" Text="2. Comparáveis (Lojas)"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_gestao/gest_loja_compara_venda_super.aspx" Text="3. Totais Completo Super (Lojas)"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_gestao/gest_loja_compara_cota.aspx?t=1" Text="4. Farol (Vendas x Lojas)"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_gestao/gest_loja_compara_cota_ano.aspx" Text="5. Comparar Vendas x Cotas"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_gestao/gest_loja_desconto.aspx?ref=1" Text="6. Descontos de Venda"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_gestao/gest_venda_semana.aspx?t=1" Text="7. Venda de Produtos Semana" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_gestao/gest_venda_semana_online.aspx?t=1" Text="8. Venda de Produtos Semana Online" Enabled="true"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">
                                    <fieldset style="height: 295px;">
                                        <legend>Relatórios de Conferência</legend>
                                        <asp:Menu ID="mnuValidacao" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_gestao/gest_confere_hand_cliente.aspx?t=1" Text="1. Cliente Handclub"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_gestao/gest_historico_hand_cliente.aspx?t=2" Text="2. Histórico Cliente Handclub" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_financeiro/fin_confere_venda_diario.aspx?t=2" Text="3. Total Vendas Dia por Cliente"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_financeiro/fin_confere_venda_mensal.aspx?t=2" Text="4. Total Vendas Mês por Cliente"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_financeiro/fin_rel_pontos_hc.aspx?t=2" Text="5. Saldo Handclub - WhatsApp"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_gestao/gest_compara_intra_linx.aspx" Text="6. Movimento Intranet X Linx"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="height: 295px;">
                                        <legend>Outros</legend>
                                        <asp:Menu ID="mnuOutros" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_gestao/gest_linereport.aspx" Text="1. Line Report Venda Líquida"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_gestao/gest_estoque_atualiza.aspx" Text="2. Atualizar Movimento Estoque"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_gestao/gest_estoqueloja_atualiza.aspx" Text="3. Atualizar Movimento Estoque Loja"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_gestao/gest_alt_qtde_sobra.aspx" Text="4. Alterar Quantidade de Sobra"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>SMS</legend>
                                        <asp:Menu ID="mnuSMS" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_gestao/gest_sms.aspx?t=1" Text="1. SMS"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">&nbsp;
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
