<%@ Page Title="Módulo de Facção" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="facc_menu.aspx.cs" Inherits="Relatorios.facc_menu" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1 {
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
                        <legend>Módulo de Facção</legend>
                        <asp:Panel ID="pnlMenuGeral" runat="server" Visible="false">
                            <table border="0" width="100%">
                                <tr>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 340px;">
                                            <legend>Controle de Facção</legend>
                                            <asp:Menu ID="mnuFaccao" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_encaixe.aspx?a=1&p=1" Text="1. Encaixe"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_emissao_nf.aspx?a=1" Text="2. Emissão Nota Fiscal"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_entrada_produto.aspx?a=1&p=1" Text="3. Entrada de Produto"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_produto_pendente.aspx?a=1" Text="4. Pendentes"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_avaliacao_prod.aspx" Text="5. Avaliação de Produção" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_retrabalho.aspx?a=1" Text="6. Retrabalho de Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_fase_alterar.aspx?t=1" Text="7. Alterar Fase de Encaixe"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 340px;">
                                            <legend>Controle de Facção Acabamento</legend>
                                            <asp:Menu ID="mnuFaccaoAcabamento" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_encaixe.aspx?a=2&p=1" Text="1. Encaixe Acabamento"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_emissao_nf.aspx?a=2" Text="2. Emissão Nota Fiscal Acabamento"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_entrada_produto_interno.aspx?a=2&p=1" Text="3. Entrada Produto Acabado Interno"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_entrada_produto_externo.aspx?a=2&p=1" Text="4. Entrada Produto Acabado Externo"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_entrada_produto_outros.aspx?a=2&p=1" Text="5. Entrada Produto Acabado Outros"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_entrada_produto_logistica.aspx?a=2&p=1" Text="6. Entrada Produto Logística" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_produto_pendente.aspx?a=2" Text="7. Perda"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset style="height: 340px;">
                                            <legend>Relatórios</legend>
                                            <asp:Menu ID="mnuRelatorio1" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo.aspx?t=1" Text="1. Resumo Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_atacadovarejo.aspx?t=1" Text="2. Facção por Atacado/Varejo"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_atacadovarejo_fal.aspx?t=1" Text="3. Facção por Atacado/Varejo Faltante"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_periodo.aspx?t=1" Text="5. Facção por Período"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_timev2.aspx?t=1" Text="6. Time Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_historico.aspx?t=1" Text="7. Ficha Entrada"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_status.aspx?t=1" Text="8. Status Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_custo.aspx?t=1" Text="9. Custo Produto Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_ficha_impressao.aspx?t=1" Text="10. Impressão Ficha Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_analise_faccao_mensal.aspx?t=1" Text="11. Desempenho de Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_fila_faccao.aspx?t=1" Text="12. Facção Diário"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_fila_acabamento.aspx?t=1" Text="13. Acabamento Diário"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_fila_acabamento_datareceb.aspx?t=1" Text="14. Acabamento Diário Recebimento"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_verificar_hb_liberado.aspx?t=1" Text="15. Status de Detalhes"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_con_hb_status.aspx?t=2" Text="16. HB Consulta Status"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_controle_producao_calen.aspx?t=3" Text="17. Calendário Controle de Produção" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_fila_producao_diario.aspx?t=1" Text="18. Produção do Dia"></asp:MenuItem>

                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlAcabamentoInterno" runat="server" Visible="false">
                            <table border="0" width="100%">
                                <tr>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 320px;">
                                            <legend>Controle de Acabamento Interno</legend>
                                            <asp:Menu ID="mnuAcabamentoInterno" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_encaixe.aspx?a=2&p=1" Text="1. Encaixe Acabamento"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_entrada_produto_interno.aspx?a=2&p=1" Text="2. Entrada Produto Acabado Interno"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_entrada_produto_outros.aspx?a=2&p=1" Text="3. Entrada Produto Acabado Outros"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_produto_pendente.aspx?a=2" Text="4. Perda"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 320px;">
                                            <legend>Transferência</legend>
                                            <asp:Menu ID="mnu2" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_adm_loja/admloj_retirada_mercadoria.aspx" Text="1. Transferência de Produto"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset style="height: 320px;">
                                            <legend>Relatórios</legend>
                                            <asp:Menu ID="mnuRelatorio2" runat="server" Enabled="true">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo.aspx?t=1" Text="1. Resumo Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_atacadovarejo.aspx?t=1" Text="2. Facção por Atacado/Varejo"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_atacadovarejo_fal.aspx?t=1" Text="3. Facção por Atacado/Varejo Faltante"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_periodo.aspx?t=1" Text="5. Facção por Período"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_timev2.aspx?t=1" Text="6. Time Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_historico.aspx?t=1" Text="7. Ficha Entrada"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_status.aspx?t=1" Text="8. Status Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_custo.aspx?t=1" Text="9. Custo Produto Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_ficha_impressao.aspx?t=1" Text="10. Impressão Ficha Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_analise_faccao_mensal.aspx?t=1" Text="11. Desempenho de Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_fila_faccao.aspx?t=1" Text="12. Facção Diário"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_fila_acabamento.aspx?t=1" Text="13. Acabamento Diário"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_fila_acabamento_datareceb.aspx?t=1" Text="14. Acabamento Diário Recebimento"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_verificar_hb_liberado.aspx?t=1" Text="15. Status de Detalhes"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_con_hb_status.aspx?t=2" Text="16. HB Consulta Status"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_con_hb_filtro.aspx?d=1&t=3" Text="17. HB Consulta"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_fila_producao_diario.aspx?t=1" Text="18. Produção do Dia"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlAcabamentoExterno" runat="server" Visible="false">
                            <table border="0" width="100%">
                                <tr>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 320px;">
                                            <legend>Controle de Facção</legend>
                                            <asp:Menu ID="mnuFaccaoExterno" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_entrada_produto.aspx?a=1&p=1" Text="1. Entrada de Produto Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_produto_pendente.aspx?a=1" Text="2. Pendentes"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 320px;">
                                            <legend>Controle de Acabamento</legend>
                                            <asp:Menu ID="mnuAcabamentoExterno" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_entrada_produto_externo.aspx?a=2&p=1" Text="1. Entrada Produto Acabado Externo"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_produto_pendente.aspx?a=2" Text="2. Perda"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset style="height: 320px;">
                                            <legend>Relatórios</legend>
                                            <asp:Menu ID="mnuRelatorio4" runat="server" Enabled="true">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo.aspx?t=1" Text="1. Resumo Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_atacadovarejo.aspx?t=1" Text="2. Facção por Atacado/Varejo"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_atacadovarejo_fal.aspx?t=1" Text="3. Facção por Atacado/Varejo Faltante"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_periodo.aspx?t=1" Text="5. Facção por Período"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_timev2.aspx?t=1" Text="6. Time Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_historico.aspx?t=1" Text="7. Ficha Entrada"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_status.aspx?t=1" Text="8. Status Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_custo.aspx?t=1" Text="9. Custo Produto Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_ficha_impressao.aspx?t=1" Text="10. Impressão Ficha Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_analise_faccao_mensal.aspx?t=1" Text="11. Desempenho de Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_fila_faccao.aspx?t=1" Text="12. Facção Diário"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_fila_acabamento.aspx?t=1" Text="13. Acabamento Diário"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_fila_acabamento_datareceb.aspx?t=1" Text="14. Acabamento Diário Recebimento"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_verificar_hb_liberado.aspx?t=1" Text="15. Status de Detalhes"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_con_hb_status.aspx?t=2" Text="16. HB Consulta Status"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlLogistica" runat="server" Visible="false">
                            <table border="0" width="100%">
                                <tr>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 320px;">
                                            <legend>Controle de Facção Logística</legend>
                                            <asp:Menu ID="mnuLogistica" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_entrada_produto_logistica.aspx?a=2&p=1" Text="1. Entrada Produto Logística" Enabled="true"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 320px;"><legend>-</legend>&nbsp;</fieldset>
                                    </td>
                                    <td>
                                        <fieldset style="height: 320px;">
                                            <legend>Relatórios</legend>
                                            <asp:Menu ID="mnuRelatorio3" runat="server" Enabled="true">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo.aspx?t=1" Text="1. Resumo Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_atacadovarejo.aspx?t=1" Text="2. Facção por Atacado/Varejo"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_atacadovarejo_fal.aspx?t=1" Text="3. Facção por Atacado/Varejo Faltante"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_periodo.aspx?t=1" Text="5. Facção por Período"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_timev2.aspx?t=1" Text="6. Time Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_historico.aspx?t=1" Text="7. Ficha Entrada"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_status.aspx?t=1" Text="8. Status Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_custo.aspx?t=1" Text="9. Custo Produto Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_ficha_impressao.aspx?t=1" Text="10. Impressão Ficha Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_analise_faccao_mensal.aspx?t=1" Text="11. Desempenho de Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_fila_faccao.aspx?t=1" Text="12. Facção Diário"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_fila_acabamento.aspx?t=1" Text="13. Acabamento Diário"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_fila_acabamento_datareceb.aspx?t=1" Text="14. Acabamento Diário Recebimento"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_verificar_hb_liberado.aspx?t=1" Text="15. Status de Detalhes"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_con_hb_status.aspx?t=2" Text="16. HB Consulta Status"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>







