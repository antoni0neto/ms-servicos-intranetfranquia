<%@ Page Title="Módulo do Fiscal" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="fisc_menu.aspx.cs"
    Inherits="Relatorios.fisc_menu" %>

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
                    <fieldset class="login">
                        <legend>Módulo Fiscal</legend>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Cadastros</legend>
                                        <asp:Menu ID="mnuCadastro" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_producao/prod_cad_fornecedor.aspx?t=3" Text="1. Cadastro de Fornecedores"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_cad_ncm.aspx?t=2" Text="2. Manutenção NCM"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_fiscal/fisc_cad_cert_digital.aspx" Text="3. Certificado Digital"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Controle Fiscal</legend>
                                        <asp:Menu ID="mnuFiscal" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_fiscal/fisc_lista_postef.aspx" Text="1. Vendas POS/TEF"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_fiscal/fisc_critica_loja.aspx" Text="2. Vendas Divergência Cliente"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_fiscal/fisc_baixa_nf_aliq_icms.aspx" Text="3. Diferencial Alíquota ICMS"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_con_venda_consig.aspx?t=2" Text="4. Controle de Consignado"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_con_mostruario.aspx?t=2" Text="5. Controle de Mostruário"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_con_extrato_sacola.aspx?t=2" Text="6. Extrato Estoque de Sacolas"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_historico.aspx?t=4" Text="7. Ficha Entrada"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_con_lancamento.aspx?t=2" Text="8. Consultar Lançamento"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_fiscal/fisc_emissao_nf_devtecido.aspx" Text="9. Emissão NF Devolução de Tecido"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_fiscal/fisc_retirada_devtecido.aspx" Text="10. Baixa de Tecido Devolvido"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Relatórios</legend>
                                        <asp:Menu ID="mnuRelatorios" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_fiscal/fisc_demonstrativo_inss.aspx?t=1" Text="1. Demonstrativo INSS"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_fiscal/fisc_apuracao_imposto.aspx?t=1" Text="2. Apuração PIS e COFINS"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_fiscal/fisc_declaracao_faturamento.aspx?t=1" Text="3. Declaração de Faturamento"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_fiscal/fisc_resumo_imposto.aspx?t=1" Text="4. Resumo dos Impostos"></asp:MenuItem>
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
