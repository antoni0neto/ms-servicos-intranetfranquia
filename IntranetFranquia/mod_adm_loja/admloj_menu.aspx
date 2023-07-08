<%@ Page Title="Módulo de Administração de Loja" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="admloj_menu.aspx.cs" Inherits="Relatorios.admloj_menu" %>

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
                        <legend>Módulo de Administração de Loja</legend>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Fechamento</legend>
                                        <asp:Menu ID="menuFechamento" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/VerificarFechamentoCaixa.aspx" Text="1. Verificar Fechamento Caixa"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/VerificarDespesas.aspx" Text="2. Verificar Despesas Caixa"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/VerificarReceitas.aspx" Text="3. Verificar Receitas Caixa"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/FechamentoCaixaNovo.aspx" Text="4. Fechamento de Caixa"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/FechamentoFundoFixo.aspx" Text="5. Fechamento Fundo Fixo"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_analise_ffc.aspx" Text="6. Análise de Fundo Fixo"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_financeiro/fin_deposito_banco.aspx" Text="7. Depósito em Banco"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Logística</legend>
                                        <asp:Menu ID="menuLogistica" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_adm_loja/admloj_cad_nota_defeito.aspx" Text="1. Cadastro NF Defeito"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_adm_loja/admloj_retirada_mercadoria.aspx" Text="2. Transferência/Devolução de Produtos"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_adm_loja/admloj_retirada_mercadoria_carr_retirada_pedidos.aspx" Text="3. Pedido de Transferência/Devolução de Produtos" Enabled="true"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="height: 200px;">
                                        <legend>Relatórios</legend>
                                        <asp:Menu ID="menuApoio" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_permuta.aspx?t=4" Text="1. Transferência/Devolução de Lojas" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_adm_loja/admloj_con_cupomfiscal.aspx" Text="2. Consultar Tickets"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Vitrine</legend>
                                        <asp:Menu ID="mnuVitrine" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_adm_loja/admloj_cad_vitrine.aspx?t=1" Text="1. Cadastro de Fotos Vitrine"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_adm_loja/admloj_cad_vitrine_analise.aspx?t=1" Text="2. Análise de Fotos Vitrine"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">&nbsp;
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
