<%@ Page Title="Módulo de Contabilidade" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="contabil_menu.aspx.cs" Inherits="Relatorios.contabil_menu" %>

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
                        <legend>Módulo de Contabilidade</legend>
                        <asp:Panel ID="pnlContabilidade" runat="server">
                            <table border="0" class="style1">
                                <tr>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 250px;">
                                            <legend>Conciliação</legend>
                                            <asp:Menu ID="mnuConciliacao" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_cad_param_folha.aspx" Text="1. Cadastro de Parâmetros de Conta"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_cad_param_linx.aspx" Text="2. Cadastro de Período de Fechamento"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_analise_ffc.aspx" Text="3. Análise de Fundo Fixo"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_saldo_ffc.aspx" Text="4. Saldo de Fundo Fixo"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_det_despesa_ffc.aspx" Text="5. Histórico de Despesas de Fundo Fixo"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_extrato_caixa.aspx" Text="6. Extrato de Caixa"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_receita_x_despesa.aspx" Text="7. Receitas X Despesas"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 250px;">
                                            <legend>Apoio</legend>
                                            <asp:Menu ID="menuApoio" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_cad_fornecedor_conta.aspx" Text="1. Cadastro de Contas por Fornecedores"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_producao/prod_cad_fornecedor.aspx?t=4" Text="2. Cadastro de Fornecedores"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_con_lancamento.aspx?t=1" Text="3. Consultar Lançamento"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_cad_ncm.aspx?t=1" Text="4. Manutenção NCM"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_con_venda_consig.aspx?t=1" Text="5. Controle de Consignado"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_con_mostruario.aspx?t=1" Text="6. Controle de Mostruário"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_con_extrato_sacola.aspx?t=1" Text="7. Extrato Estoque de Sacolas"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset style="height: 250px;">
                                            <legend>Relatórios Fiscal</legend>
                                            <asp:Menu ID="mnuRelFiscal" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_fiscal/fisc_demonstrativo_inss.aspx?t=2" Text="1. Demonstrativo INSS"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_fiscal/fisc_apuracao_imposto.aspx?t=2" Text="2. Apuração PIS e COFINS"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_fiscal/fisc_declaracao_faturamento.aspx?t=2" Text="3. Declaração de Faturamento"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_fiscal/fisc_resumo_imposto.aspx?t=2" Text="4. Resumo dos Impostos"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                </tr>

                                <tr>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 250px;">
                                            <legend>Documentos</legend>
                                            <asp:Menu ID="mnuDocs" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/ManualFundoFixo.aspx" Text="1. Fundo Fixo"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/ManualViagens.aspx" Text="2. Viagens"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 250px;">
                                            <legend>Seguros</legend>
                                            <asp:Menu ID="mnuSeguro" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_seguro/seg_menu.aspx" Text="1. Módulo de Seguros"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td>&nbsp;
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
