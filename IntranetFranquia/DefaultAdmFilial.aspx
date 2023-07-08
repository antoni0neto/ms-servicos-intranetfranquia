<%@ Page Title="Módulo de Administração de Loja" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="DefaultAdmFilial.aspx.cs" Inherits="Relatorios.DefaultAdmFilial" %>

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
                                                <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_analise_ffc.aspx" Text="6. Analise de Fundo Fixo"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/DepositoBanco.aspx" Text="7. Depósito em Banco"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Fiscal</legend>
                                        <asp:Menu ID="menuFiscal" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/CadastroNotaDefeito.aspx" Text="1. Cadastro NF Defeito"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_adm_loja/admloj_con_cupomfiscal.aspx" Text="2. Consultar Cupom Fiscal"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_adm_loja/admloj_retirada_mercadoria.aspx" Text="3. Lê Cód. Barra Produto"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="height: 200px;">
                                        <legend>Apoio</legend>
                                        <asp:Menu ID="menuApoio" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/ManualFundoFixo.aspx" Text="1. Fundo Fixo"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/ManualDepositoBanco.aspx" Text="2. Registro Depósito Banco"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/ManualViagens.aspx" Text="3. Viagens"></asp:MenuItem>
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
