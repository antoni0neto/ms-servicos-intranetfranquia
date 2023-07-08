<%@ Page Title="Módulo de Desenvolvimento" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="desenv_menu.aspx.cs" Inherits="Relatorios.desenv_menu" %>

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
                        <legend>Módulo de Desenvolvimento</legend>
                        <asp:Panel ID="pnlDesenvolvimentoColecao" runat="server">
                            <table border="0" class="style1">
                                <tr>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 360px;">
                                            <legend>Cadastro</legend>
                                            <asp:Menu ID="mnuApoioPocket" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_material_gruposubgrupo.aspx" Text="1. Cadastro de Grupo e SubGrupo"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_material.aspx?t=1" Text="2. Cadastro de Materiais"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_ficha_tecnica.aspx?t=1" Text="3. Ficha Técnica"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_produto_origem.aspx" Text="4. Cadastro de Origem"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_pocket_cor.aspx" Text="5. Cadastro de Cores"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_cad_fornecedor.aspx?t=1" Text="6. Cadastro de Fornecedores"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 150px;">
                                            <legend>Planejamento de Coleção</legend>
                                            <asp:Menu ID="mnuPlanejamento" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_tripa_prepedido.aspx?t=1" Text="1. Pré-Pedido de Tecido"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_tripa_view.aspx?t=1" Text="2. Liberação de Produto (View)"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_tripa_manut.aspx?t=1" Text="3. Desenvolvimento de Coleção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_pocket_con.aspx?t=1" Text="4. Consultar Coleção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_pocket_con_print.aspx?t=1" Text="5. Consultar Coleção Print"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_pocket_con_gemeav2.aspx?t=1" Text="6. Liberar 1ª Peça/Produto" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_pocket_con_modelagem.aspx?t=1" Text="7. Liberar Modelagem" Enabled="true"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                        <fieldset style="height: 166px;">
                                            <legend>Análise de Coleção</legend>
                                            <asp:Menu ID="mnuAnalise" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_categoria_ordem.aspx?t=1" Text="1. Gerenciamento de Categorias" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_categoria_ordem_produto.aspx?t=1" Text="2. Adicionar Produtos na Categoria" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_categoria_ordem_bloco.aspx?t=1" Text="3. Ordenação Produtos na Categoria" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_categoria_ordem_bloco_analise.aspx?t=1" Text="4. Análise de Produtos na Categoria" Enabled="true"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset style="height: 360px;">
                                            <legend>Relatórios</legend>
                                            <asp:Menu ID="mnuRel2" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_rel_geral.aspx?t=V" Text="1. Produção Brasil Varejo"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_rel_geral.aspx?t=A" Text="2. Produção Brasil Atacado"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_rel_geral_acessorio.aspx" Text="3. Acessórios Varejo" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_con_tecido_estoque.aspx?t=1" Text="4. Estoque (Cartelinha)"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_controle_mostruario.aspx?t=1" Text="5. Controle de Mostruário" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_controle_producao.aspx?t=1" Text="6. Controle de Produção" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_rel_produto_preco_aprovado.aspx?t=2" Text="7. Preços Aprovados"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_con_hb_filtro.aspx?d=1&t=1" Text="8. HB Consulta"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gestao/gest_venda_semana.aspx?t=2" Text="9. Vendas de Produtos Semana" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gestao/gest_venda_semana_online.aspx?t=2" Text="10. Venda de Produtos Semana Online" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_acessorio_venda.aspx?t=2" Text="11. Vendas de Produtos Diário"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_mais_vendidos.aspx?t=2" Text="12. 60+ Vendidos"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_estoque.aspx?t=3" Text="13. Estoque Lojas" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_painel_resultado_produto.aspx?t=1" Text="14. Painel Resultado de Produto" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_painel_resultado_produtoconsol.aspx?t=1" Text="15. Painel Resultado de Produto Consolidado" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_painel_resultado_produto_online.aspx?t=1" Text="16. Painel Resultado de Produto Online" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_controle_producao_calen.aspx?t=1" Text="17. Calendário Controle de Produção" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_painel_entrega_loja.aspx?t=2" Text="18. Painel de Entrega de Produto" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_painel_cortefaccao.aspx?t=2" Text="19. Painel de Entrega Corte/Facção/Loja" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_painel_agenda.aspx" Text="20. Agenda de Marketing" Enabled="true"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 230px;">
                                            <legend>Tecidos</legend>
                                            <asp:Menu ID="mnuPedido" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_material_comprav2.aspx?t=1" Text="1. Pedido de Tecido"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_tripa_prepedido_sub.aspx?t=1" Text="2. Pré-pedido de Tecido (Pedidos)"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_material_controlpedido.aspx?t=1" Text="3. Controle de Pedido"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_compra_tecidofluxo.aspx?t=1" Text="4. Fluxo de Pagamento"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_material_devolcontrol.aspx?t=1" Text="5. Controle de Devolução"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_con_tecido_estoque.aspx?t=1" Text="6. Estoque (Cartelinha)"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_material_compra.aspx?t=1" Text="7. Pedido de Tecido (Até Pedido 8000)"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 230px;">
                                            <legend>Aviamentos</legend>
                                            <asp:Menu ID="mnuPedidoAviamento" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_pedido_aviamento_meus.aspx?t=1" Text="1. Pré-pedido de Aviamento" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_pedido_aviamento_linx_meus.aspx?t=1" Text="2. Recebimento de Pedido" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_pedido_aviamento_linx_con.aspx?t=1" Text="3. Pedidos Recebidos" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_pedido_aviamento_hb_saq.aspx?t=1" Text="4. Separar Aviamento" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_pedido_aviamento_hb_saq_liberado.aspx?t=1" Text="5. Liberar Aviamento" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_pedido_aviamento_faltv3.aspx?t=1" Text="6. Aviamento Faltante" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_pedido_aviamento_estoque_manut.aspx?t=1" Text="7. Ajuste de Estoque" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_pedido_aviamento_estoque.aspx?t=1" Text="8. Estoque de Aviamento" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_pedido_aviamento_tingirmat.aspx?t=1" Text="9. Solicitar Tingimento" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_pedido_aviamento_tingirmat_receb.aspx?t=1" Text="10. Receber Material Tingido" Enabled="true"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset style="height: 230px;">
                                            <legend>Liberação Produto Acabado</legend>
                                            <asp:Menu ID="Menu1" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_fila_tag.aspx" Text="1. Liberação de TAG" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_fila_aviamento.aspx" Text="2. Liberação de Aviamento" Enabled="true"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlDesenvAcessorio" runat="server">
                            <table border="0" class="style1">
                                <tr>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 230px;">
                                            <legend>Planejamento de Coleção de Acessórios</legend>
                                            <asp:Menu ID="mnuAcessorios" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_acessorio_manut.aspx?t=1" Text="1. Desenvolvimento de Coleção de Acessórios"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_acessorio_con.aspx?t=1" Text="2. Consultar Coleção"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_produto_origem.aspx" Text="3. Cadastro de Origem"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_acessorio_venda_calc.aspx" Text="4. Venda de Calçados"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 230px;">
                                            <legend>E-Commerce</legend>
                                            <asp:Menu ID="mnuECommerce" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_cad_produto_descricao.aspx?t=2" Text="1. Descrição de Produto E-Commerce"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <br />
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td>&nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
