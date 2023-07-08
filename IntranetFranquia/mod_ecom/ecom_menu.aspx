<%@ Page Title="Módulo do E-Commerce" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="ecom_menu.aspx.cs" Inherits="Relatorios.ecom_menu" %>

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
                        <legend>Módulo do E-Commerce</legend>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Cadastros</legend>
                                        <asp:Menu ID="mnuCadastro" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_ecomv2/ecomv2_cad_produto.aspx" Text="1. Produto" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_cad_produto_descricao.aspx?t=1" Text="2. Descrição de Produto"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_cad_foto_maquina.aspx" Text="3. Cadastro de Fotos Máquina"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_cad_foto_maquina_transfer.aspx" Text="4. Transferência de Fotos para Seleção"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_cad_foto.aspx" Text="5. Seleção de Fotos Ecommerce"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_cad_foto_app_e_lb.aspx" Text="6. Seleção de Fotos App/Lookbook"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_cad_produto_tratamento.aspx" Text="7. Seleção de Fotos Tratamento"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">
                                    <fieldset style="height: 88px;">
                                        <legend>Controle de Clientes</legend>
                                        <asp:Menu ID="mnuCRM" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_ecomv2/ecomv2_pedido_mag.aspx?t=1" Text="1. Pedidos" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_pedido_mag_boleto.aspx?t=1" Text="2. Controle de Boletos" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_crm/crm_cliente.aspx?t=2" Text="3. Clientes" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_crm/crm_cliente_acao.aspx?t=2" Text="4. Clientes - Ação" Enabled="true"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                    <fieldset style="height: 68px;">
                                        <legend>Controle de Emails</legend>
                                        <asp:Menu ID="mnuEmailMkt" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_email_criacao.aspx?email=0" Text="1. Criar Email" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_email_consulta.aspx" Text="2. Consultar Emails" Enabled="true"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="height: 200px;">
                                        <legend>Controle de Pedido</legend>
                                        <asp:Menu ID="mnuControle" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_cad_produto_lib_aprovacao.aspx" Text="1. Aprovação de Pedido"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_cad_produto_destino.aspx" Text="2. Destino do Pedido"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_cad_produto_expedicao.aspx" Text="3. Expedição"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecomv2/ecomv2_cad_produto_faturamento.aspx" Text="4. Faturamento" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_cad_produto_coleta.aspx" Text="5. Lista Pedido Coleta"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecomv2/ecomv2_rastreio_entrega.aspx" Text="6. Rastreio de Pedido" Enabled="true"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Ordenação de Produtos</legend>
                                        <asp:Menu ID="mnuOrdem" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_ordem_cad_bloco.aspx?t=1" Text="1. Gerenciamento de Bloco" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_ordem_cad_blocoproduto.aspx" Text="2. Adicionar Produtos no Bloco" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_ordem_cad_blocoproduto_sembloco.aspx" Text="3. Produtos e Blocos" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_ordem_produtoblocov2.aspx" Text="4. Ordenação de Produtos no Bloco" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecomv2/ecomv2_ordem_produtobloco_catfinal.aspx" Text="5. Atualizar Ordenação Magento" Enabled="true"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Controle de Produtos Relacionados</legend>
                                        <asp:Menu ID="mnuControleProdutoRel" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_rel_config_cad.aspx?gg=&gp=" Text="1. Cadastro de Configurações" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_rel_config_con.aspx" Text="2. Consulta de Configurações" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_rel_config_entrada_saida.aspx" Text="3. Configurações Entrada e Saída" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_rel_config_rel_lista.aspx" Text="4. Atualizar Produto Relacionado" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_rel_config_aux.aspx" Text="5. Relatório de Produtos (Estoque)" Enabled="true"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="height: 200px;">
                                        <legend>Relatórios</legend>
                                        <asp:Menu ID="mnuRelatorio" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_mais_vendidos.aspx?t=1" Text="1. 60+ Vendidos Online" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_mais_vendidos.aspx?t=3" Text="2. 60+ Vendidos Lojas"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_venda_compara_col_outlet.aspx?t=1" Text="3. Comparativo Vendas - Colecao X Outlet" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_venda_compara_analytics_mag.aspx?t=1" Text="4. Comparativo Vendas - Analytics X Magento" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_gestao/gest_historico_hand_cliente.aspx?t=4" Text="5. Histórico Cliente Handclub" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_financeiro/fin_rel_pontos_hc.aspx?t=4" Text="6. Saldo Handclub"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_venda_ecom.aspx?t=2" Text="7. Vendas de Loja Física"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_gestao/gest_venda_semana_online.aspx?t=3" Text="8. Venda de Produtos Semana Online" Enabled="true"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 80px;">
                                        <legend>Ajuda</legend>
                                        <asp:Menu ID="mnuAjuda" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_ajuda_video.aspx" Text="1. Vídeos de Ajuda"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                    <fieldset style="height: 176px;">
                                        <legend>Parametrização</legend>
                                        <asp:Menu ID="mnuIntegracao" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_parametros_api.aspx" Text="1. API Pedido"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">
                                    <fieldset style="height: 300px;">
                                        <legend>Estoque</legend>
                                        <asp:Menu ID="mnuEstoque" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_ecomv2/ecomv2_cad_produto_magento.aspx" Text="1. Liberação Estoque" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecomv2/ecomv2_dev_estoque.aspx" Text="2. Devolução Estoque" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_rel_produto_reservado.aspx" Text="3. Produtos Reservados"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecomv2/ecomv2_produto_magento_estoque_tamanho.aspx" Text="4. Estoque Magento Linx" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecomv2/ecomv2_produto_magento_estoque_salable.aspx" Text="5. Estoque Magento Salable" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_estoque.aspx?t=4" Text="6. Estoque Lojas" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_adm_loja/admloj_retirada_mercadoria_porarquivo.aspx" Text="7. Devolução Interna de Produto (Arquivo)"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_adm_loja/admloj_retirada_mercadoria.aspx" Text="8. Transferência/Devolução de Produtos"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="height: 300px;">
                                        <legend>Controle de Preços</legend>
                                        <asp:Menu ID="mnuPreco" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_ecomv2/ecomv2_produto_preco.aspx" Text="1. Remarcação de Preços" Enabled="true"></asp:MenuItem>
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
