<%@ Page Title="Módulo de Produção" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="prod_menu.aspx.cs" Inherits="Relatorios.prod_menu" %>

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
                        <legend>Módulo de Produção</legend>
                        <asp:Panel ID="pnlProducao1" runat="server">
                            <table border="0" class="style1">
                                <tr>
                                    <td style="width: 480px;">
                                        <fieldset style="height: 200px;">
                                            <legend>Ordem de Corte</legend>
                                            <asp:Menu ID="menuOrdemCorte" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_cad_hb.aspx?t=1" Text="1. HB Novo"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_cad_hb_altera_filtro.aspx?d=1" Text="2. HB Alteração"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_cad_hb_altera_filtro.aspx?d=2" Text="3. HB Alteração Corte"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_cad_hb_altera_filtro.aspx?d=3" Text="4. HB Alteração Ampliação"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_rel_hb_impressao.aspx" Text="5. Impressão de Fichas"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_con_tecido_estoque.aspx?t=2" Text="6. Estoque (Cartelinha)"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_cad_modelagem_baixa.aspx?t=2" Text="7. Baixa de Modelagem" Enabled="true"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td style="width: 480px;">
                                        <fieldset style="height: 200px;">
                                            <legend>Gestão de Ordem de Corte</legend>
                                            <asp:Menu ID="menuGestao" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_fila_mostruario.aspx" Text="1. Mostruário"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_fila_risco.aspx" Text="2. Risco Pendente"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_fila_riscado_dia.aspx" Text="3. Risco Diário"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_fila_riscado.aspx" Text="4. Riscado"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_fila_corte.aspx" Text="5. Corte"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_fila_cortado.aspx" Text="6. Cortado"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_fila_riscocorte.aspx" Text="7. Controle Risco/Corte"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_cad_hb_altera_data.aspx" Text="8. Alterar Data Processo"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_cad_hb_libera_faccao.aspx" Text="9. Liberar Controle de Facção"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset style="height: 200px;">
                                            <legend>Relatórios Ordem de Corte</legend>
                                            <asp:Menu ID="menuRelatorio" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_con_hb_status.aspx?t=1" Text="1. HB Consulta Status"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_con_hb_filtro.aspx?d=1&t=2" Text="2. HB Consulta"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_con_hb_filtro.aspx?d=2&t=2" Text="3. HB Consulta Detalhe"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_con_hb_finalizado.aspx" Text="4. HB Consulta Finalizados"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_controle_producao_calen.aspx?t=2" Text="5. Calendário Controle de Produção" Enabled="true"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <fieldset style="height: 200px;">
                                            <legend>Custo/Preço de Produto 27+</legend>
                                            <asp:Menu ID="mnuCusto27" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_cad_produto_custo_simulacao_novo.aspx" Text="1. Simulação sem Corte"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_cad_produto_custo_novo.aspx?c=0&p=0&m=0" Text="1 Simulação de Custo/Preço"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_cad_produto_custo_lista.aspx?c=N" Text="2. Lista Simulação de Custo/Preço"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_cad_produto_custo_aprov_novo.aspx?c=0&p=0&m=0" Text="3. Aprovação de Preço"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_cad_produto_custo_aprov_lista.aspx?c=N" Text="4. Lista Aprovação de Preço"></asp:MenuItem>

                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset style="height: 200px;">
                                            <legend>Custo/Preço de Produto 26-</legend>
                                            <asp:Menu ID="menuCusto26" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_cad_produto_custo_simulacao.aspx" Text="1. Simulação sem Corte"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_cad_produto_custo.aspx?c=0&p=0&m=0" Text="2. Simulação de Custo/Preço"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_cad_produto_custo_lista.aspx?c=A" Text="3. Lista Simulação de Custo/Preço"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_cad_produto_custo_aprov.aspx?c=0&p=0&m=0" Text="4. Aprovação de Preço"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_cad_produto_custo_aprov_lista.aspx?c=A" Text="5. Lista Aprovação de Preço"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset style="height: 200px;">
                                            <legend>Custo/Preço de Produto Geral</legend>
                                            <asp:Menu ID="mnuCustogeral" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_cad_produto_custo_aprovterceiro.aspx?c=0&p=0&m=0" Text="1. Aprovação de Preço Terceiros"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_rel_produto_custo.aspx" Text="2. Produtos Sem Preço"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_rel_produto_preco_aprovado.aspx?t=1" Text="3. Preços Aprovados"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_controle_produto/cprod_produto_cadastro_resp.aspx?t=2" Text="4. Etiqueta de Composição"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <fieldset style="height: 240px;">
                                            <legend>Logística</legend>
                                            <asp:Menu ID="menuLogistica" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_def_logistica.aspx" Text="1. Definição Logística"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_def_logistica_imp.aspx" Text="2. Impressão Ficha Logística"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset style="height: 240px;">
                                            <legend>Módulo de Facção</legend>
                                            <asp:Menu ID="mnuFacao" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo.aspx?t=2" Text="1. Resumo Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_atacadovarejo.aspx?t=2" Text="2. Facção por Atacado/Varejo"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_grupo_faltante.aspx?t=2" Text="3. Facção por Grupo"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_periodo.aspx?t=2" Text="4. Facção por Período"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_time.aspx?t=2" Text="5. Time Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_historico.aspx?t=2" Text="6. Ficha Entrada"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_status.aspx?t=2" Text="7. Status Facção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_fila_faccao.aspx?t=2" Text="8. Facção Diário"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_fila_acabamento.aspx?t=2" Text="9. Acabamento Diário"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_entrada_produto.aspx?a=1&p=9" Text="10. Entrada de Produto Facção/Estamparia/Lavanderia"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_entrada_produto_interno.aspx?a=2&p=9" Text="11. Entrada Acabamento Interno"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_faccao/facc_entrada_produto_externo.aspx?a=2&p=9" Text="12. Entrada Acabamento Externo"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset style="height: 240px;">
                                            <legend>Módulo de Desenvolvimento</legend>
                                            <asp:Menu ID="mnuApoioDesenvolvimento" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_tripa_view.aspx?t=2" Text="1. Liberação de Produto (View)"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_material.aspx?t=2" Text="2. Cadastro de Materiais"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_tripa_manut.aspx?t=2" Text="3. Desenvolvimento de Coleção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_ficha_tecnica.aspx?t=2" Text="4. Ficha Técnica"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_material_comprav2.aspx?t=2" Text="5. Pedido de Tecido"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_material_compra.aspx?t=2" Text="5.1. Pedido de Tecido (Até Pedido 8000)"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_pocket_con.aspx?t=2" Text="6. Consultar Coleção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_controle_mostruario.aspx?t=2" Text="7. Controle de Mostruário" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_controle_producao.aspx?t=2" Text="8. Controle de Produção" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_pedido_aviamento_linx_con.aspx?t=2" Text="9. Pedidos Aviamentos Recebidos" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gestao/gest_venda_semana.aspx?t=3" Text="10. Venda de Produtos Semana" Enabled="true"></asp:MenuItem>
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
