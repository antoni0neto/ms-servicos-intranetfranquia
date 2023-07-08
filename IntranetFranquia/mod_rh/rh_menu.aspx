<%@ Page Title="Módulo do RH" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="rh_menu.aspx.cs" Inherits="Relatorios.rh_menu" %>

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
                        <legend>Módulo do RH</legend>
                        <asp:Panel ID="pnlLoja" runat="server" Visible="false">
                            <table border="0" class="style1">
                                <tr>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 200px;">
                                            <legend>Solicitações</legend>
                                            <asp:Menu ID="mnuSolicitacao" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_rh/rh_sol_transf_temp_vendedor.aspx?s=0" Text="1. Transferência Temporária de Funcionário"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_rh/rh_sol_transferencia_vendedor.aspx?s=0" Text="2. Transferência de Funcionário"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_rh/rh_sol_alteracao_cargo_sal_periodo.aspx?s=0" Text="3. Alteração de Cargo/Salário/Período de Trabalho"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_rh/rh_sol_alteracao_venda.aspx?s=0" Text="4. Alteração de Venda"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_rh/rh_sol_admissao.aspx?s=0" Text="5. Admissão"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_rh/rh_sol_demissao.aspx?s=0" Text="6. Demissão"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_rh/rh_sol_ferias.aspx?s=0" Text="7. Férias"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_rh/rh_sol_painel_pendente.aspx" Text="8. Solicitações Pendentes"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 200px;">
                                            <legend>Controle de Ponto</legend>
                                            <asp:Menu ID="mnuControlePonto" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="http://datasynchbf.ddns.net:3351/grhnet/" Text="1. Controle de Ponto Datamace" Target="_blank"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset style="height: 200px;">
                                            <legend>Relatórios</legend>
                                            <asp:Menu ID="mnuRelatorios" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_rh/rh_sol_painel.aspx" Text="1. Solicitações Finalizadas"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlEscritorio" runat="server" Visible="false">
                            <table border="0" class="style1">
                                <tr>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 200px;">
                                            <legend>Cadastro de Apoio</legend>
                                            <asp:Menu ID="mnuCadastro" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_rh/rh_cad_feriado.aspx" Text="1. Cadastro de Feriado"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_rh/rh_cad_periodo_trab.aspx" Text="2. Cadastro de Período de Trabalho"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_rh/rh_cad_funcionario.aspx" Text="3. Cadastro de Funcionário"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 200px;">
                                            <legend>-</legend>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset style="height: 200px;">
                                            <legend>Folha</legend>
                                            <asp:Menu ID="mnuFolha" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_rh/rh_folha_loja_calculo.aspx" Text="1. Calcular Comissão Loja"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_rh/rh_rel_folha.aspx?t=1" Text="2. Relatório Comissão"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_rh/rh_rel_valor_venda.aspx?t=1" Text="3. Calcular Venda Vendedor"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_rh/rh_vale_mercadoria_mensal.aspx?t=1" Text="4. Vale Mercadoria"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_trab_domingofer.aspx?t=2" Text="5. Domingos e Feriados Trabalhados" Enabled="true"></asp:MenuItem>
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







