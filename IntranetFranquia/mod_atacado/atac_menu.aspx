<%@ Page Title="Módulo do Atacado" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="atac_menu.aspx.cs" Inherits="Relatorios.atac_menu" %>

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
                        <legend>Módulo do Atacado</legend>
                        <asp:Panel ID="pnlAtacado" runat="server">
                            <table border="0" class="style1">
                                <tr>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 200px;">
                                            <legend>Relatórios</legend>
                                            <asp:Menu ID="mnuRelatorio" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_atacado/atac_rel_faturamento.aspx" Text="1. Faturamento Atacado"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_atacado/atac_rel_faturamento_comissao.aspx" Text="2. Comissão Representante"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_atacado/atac_rel_hist_representante.aspx" Text="3. Histórico Representante"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_atacado/atac_rel_representante_cota.aspx" Text="4. Histórico de Clientes/Cota"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_controle_mostruario.aspx?t=3" Text="5. Controle de Mostruário" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_atacado/atac_rel_cliente_inadimplente.aspx" Text="6. Clientes Inadimplentes"></asp:MenuItem>

                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 200px;">
                                            <legend>Controle</legend>
                                            <asp:Menu ID="mnuControle" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_atacado/atac_cad_cliente_bloqueio.aspx" Text="1. Liberação Financeiro" Enabled="true"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 200px;">
                                            <legend>Etiqueta</legend>
                                            <asp:Menu ID="menuEti" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_atacado/atac_rel_eti_netshoes.aspx" Text="1. Etiqueta Netshoes" Enabled="true"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 150px;">
                                            <legend>Módulo Financeiro</legend>
                                            <asp:Menu ID="mnuModFinanceiro" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/DefaultFinanceiro.aspx" Text="1. Módulo Financeiro"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>







