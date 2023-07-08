<%@ Page Title="Módulo de Gerenciamento de Loja" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="gerloja_menu.aspx.cs" Inherits="Relatorios.gerloja_menu" %>

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
                        <legend>Módulo de Gerenciamento de Loja</legend>
                        <asp:Panel ID="pnlLoja" runat="server" Visible="false">
                            <table border="0" class="style1">
                                <tr>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 200px;">
                                            <legend>Relatórios</legend>
                                            <asp:Menu ID="mnuLojaDesempenho" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_mais_vendidos.aspx?t=1" Text="1. 60+ Vendidos"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_venda_cota_loja.aspx" Text="2. Venda/Cotas por Loja (Com Vale Mercadoria)"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_venda_cota_loja_semvale.aspx" Text="3. Venda/Cotas por Loja (Sem Vale Mercadoria)"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_venda_ecom.aspx?t=1" Text="4. Vendas Online"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_flash_varejo.aspx" Text="5. Flash Varejo" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_handclub.aspx" Text="6. Vendas Handclub" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_estoque_foto.aspx?t=1" Text="7. Estoque Lojas" Enabled="true"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 200px;">
                                            <legend>Vendedores</legend>
                                            <asp:Menu ID="mnuLojaVendedores" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_desempenho_vendedorv2.aspx" Text="1. Desempenho de Vendas"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_venda_vendedor.aspx" Text="2. Vendas por Vendedor"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_analise_atendimento.aspx" Text="3. Análise Hora de Atendimento"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_analise_atendimento_dia.aspx" Text="4. Análise Hora de Atendimento Dia"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_vale_mercadoria_fun.aspx" Text="5. Vale Mercadoria Funcionário"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_vendedorranknv.aspx" Text="6. Ranking de Vendedores 2020" Enabled="true"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset style="height: 200px;">
                                            <legend>Apoio</legend>
                                            <asp:Menu ID="mnuLojaApoio" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/ManualFundoFixo.aspx" Text="1. Fundo Fixo"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/ManualDepositoBanco.aspx" Text="2. Registro Depósito Banco"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/ManualViagens.aspx" Text="3. Viagens"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/PDF/MANUAL_VALE_MERCADORIA_FUNCIONARIO.pdf" Target="_blank" Text="4. Manual de Venda de Vale Mercadoria Funcionário" Enabled="false"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 465px;">
                                        <fieldset id="fsSupervisorLoja" runat="server" style="height: 200px;">
                                            <legend>Painel do Supervisor</legend>
                                            <asp:Menu ID="mnuSupervisorPaine" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_visita_supervisor.aspx?t=1" Text="1. Agenda de Visitas"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_visita_supervisor_cotas.aspx?t=1" Text="2. Análise de Cotas"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_griffe_porc.aspx?t=1" Text="3. Griffe Porcentagem"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gestao/gest_loja_compara_cota.aspx?t=2" Text="4. Farol (Vendas x Lojas)"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_trab_domingofer.aspx?t=1" Text="5. Domingos e Feriados Trabalhados" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_consulta_entrega_loja.aspx?t=1" Text="6. Consultar Produto Recebido" Enabled="true"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                        <fieldset id="fsGerenteLoja" runat="server" style="height: 200px;">
                                            <legend>Painel do Gerente</legend>
                                            <asp:Menu ID="mnuGerentePaine" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_visita_supervisor_cotas.aspx?t=1" Text="1. Análise de Cotas"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_griffe_porc.aspx?t=1" Text="2. Griffe Porcentagem"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gestao/gest_loja_compara_cota.aspx?t=2" Text="3. Farol (Vendas x Lojas)"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td style="width: 465px;">
                                        <fieldset id="fsHandclub" runat="server" style="height: 200px;">
                                            <legend>Handclub</legend>
                                            <asp:Menu ID="mnuHandclub" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_gestao/gest_historico_hand_cliente.aspx?t=3" Text="1. Histórico Cliente Handclub" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_financeiro/fin_confere_venda_diario.aspx?t=3" Text="2. Total Vendas Dia por Cliente"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_financeiro/fin_confere_venda_mensal.aspx?t=3" Text="3. Total Vendas Mês por Cliente"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlEscritorio" runat="server" Visible="false">
                            <table border="0" class="style1">
                                <tr>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 200px;">
                                            <legend>Relatórios</legend>
                                            <asp:Menu ID="mnuEscritorioDesempenho" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_mais_vendidos.aspx?t=1" Text="1. 60+ Vendidos"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_venda_cota_loja.aspx" Text="2. Venda/Cotas por Loja (COM Vale Mercadoria)"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_venda_cota_loja_semvale.aspx" Text="3. Venda/Cotas por Loja (SEM Vale Mercadoria)"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_venda_ecom.aspx?t=1" Text="4. Vendas Online"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_flash_varejo.aspx" Text="5. Flash Varejo" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_handclub.aspx" Text="6. Vendas Handclub" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_estoque_foto.aspx?t=1" Text="7. Estoque Lojas" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_permuta.aspx?t=1" Text="8. Transferência/Devolução de Lojas" Enabled="true"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 200px;">
                                            <legend>Vendedores</legend>
                                            <asp:Menu ID="mnuEscritorioVendedores" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_desempenho_vendedorv2.aspx" Text="1. Desempenho de Vendas"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_venda_vendedor.aspx" Text="2. Vendas por Vendedor"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_analise_atendimento.aspx" Text="3. Análise Hora de Atendimento"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_analise_atendimento_dia.aspx" Text="4. Análise Hora de Atendimento Dia"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_vale_mercadoria_fun.aspx" Text="5. Vale Mercadoria Funcionário"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_vendedorrank.aspx" Text="6. Ranking de Vendedores 2019"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_vendedorranknv.aspx" Text="7. Ranking de Vendedores 2020" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_desempenho_vendedor_nv.aspx" Text="8. Desempenho Vendedor Cliente"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset style="height: 200px;">
                                            <legend>Apoio</legend>
                                            <asp:Menu ID="mnuEscritorioApoio" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/ManualFundoFixo.aspx" Text="1. Fundo Fixo"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/ManualDepositoBanco.aspx" Text="2. Registro Depósito Banco"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/ManualViagens.aspx" Text="3. Viagens"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/PDF/MANUAL_VALE_MERCADORIA_FUNCIONARIO.pdf" Target="_blank" Text="4. Manual de Venda de Vale Mercadoria Funcionário" Enabled="false"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 200px;">
                                            <legend>Vendas</legend>
                                            <asp:Menu ID="mnuEscritorioVendas" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_acessorio_venda.aspx?t=1" Text="1. Vendas de Produtos Diário"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 200px;">
                                            <legend>Painel do Supervisor</legend>
                                            <asp:Menu ID="mnuSuperEsc" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_visita_supervisor.aspx?t=1" Text="1. Agenda de Visitas"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_visita_supervisor_cotas.aspx?t=1" Text="2. Análise de Cotas"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_griffe_porc.aspx?t=1" Text="3. Griffe Porcentagem"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_trab_domingofer.aspx?t=1" Text="4. Domingos e Feriados Trabalhados" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_consulta_entrega_loja.aspx?t=1" Text="5. Consultar Produto Recebido" Enabled="true"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset style="height: 200px;">
                                            <legend>Lojinha</legend>
                                            <asp:Menu ID="mnuLojinha" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_lojinha.aspx" Text="1. Lojinha" Enabled="true"></asp:MenuItem>
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
