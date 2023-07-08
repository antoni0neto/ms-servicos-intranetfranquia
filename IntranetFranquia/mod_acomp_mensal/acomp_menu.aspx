<%@ Page Title="Módulo Acompanhamento Mensal" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="acomp_menu.aspx.cs" Inherits="Relatorios.acomp_menu" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .login {
            height: 180px;
            width: 280px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <table border="0" class="style1" width="100%">
        <tr>
            <td>
                <fieldset>
                    <legend>Módulo Acompanhamento Mensal</legend>
                    <table border="0" width="100%">
                        <tr>
                            <td style="width: 465px;">
                                <fieldset style="height: 150px;">
                                    <legend>Despesas Lojas</legend>
                                    <asp:Menu ID="mDespesaLoja" runat="server">
                                        <Items>
                                            <asp:MenuItem NavigateUrl="~/mod_acomp_mensal/desp_loja/desp_loja_grafico.aspx" Text="1. Aluguel"></asp:MenuItem>
                                            <asp:MenuItem NavigateUrl="~/mod_acomp_mensal/desp_loja/desp_loja_performance.aspx"
                                                Text="2. Performance de Loja"></asp:MenuItem>
                                            <asp:MenuItem NavigateUrl="~/mod_acomp_mensal/desp_loja/desp_loja_performance_rede.aspx"
                                                Text="3. Performance de Rede"></asp:MenuItem>
                                        </Items>
                                    </asp:Menu>
                                </fieldset>
                            </td>
                            <td style="width: 465px;">
                                <fieldset style="height: 150px;">
                                    <legend>DRE</legend>
                                    <asp:Menu ID="mDRE" runat="server" Visible="false">
                                        <Items>
                                            <asp:MenuItem NavigateUrl="~/mod_acomp_mensal/dre/dre_dre_v10_fabrica.aspx" Text="1. DRE Fábrica" Enabled="true"></asp:MenuItem>
                                            <asp:MenuItem NavigateUrl="~/mod_acomp_mensal/dre/dre_dre_v10_loja.aspx" Text="2. DRE Lojas" Enabled="true"></asp:MenuItem>

                                        </Items>
                                    </asp:Menu>
                                </fieldset>
                            </td>
                            <td>
                                <fieldset style="height: 150px;">
                                    <legend>Relatórios</legend>
                                    <asp:Menu ID="Menu1" runat="server" Visible="true">
                                        <Items>
                                            <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_painel_resultado_produto.aspx?t=2" Text="1. Painel Resultado de Produto" Enabled="true"></asp:MenuItem>
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
</asp:Content>
