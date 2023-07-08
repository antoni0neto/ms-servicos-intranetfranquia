<%@ Page Title="Módulo de Contagem" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="cont_menu.aspx.cs" Inherits="Relatorios.cont_menu" %>

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
                        <legend>Módulo de Contagem</legend>
                        <asp:Panel ID="pnlLoja" runat="server" Visible="true">
                            <table border="0" class="style1">
                                <tr>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 200px;">
                                            <legend>Contagem</legend>
                                            <asp:Menu ID="mnuContagemLoja" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_contagem/cont_entrada60mais.aspx" Text="1. Contagem Semanal"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_contagem/cont_estoque_loja.aspx" Text="2. Contagem Física"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_contagem/cont_estoque_loja_rec.aspx" Text="3. Recontagem Física" Enabled="true"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 200px;">
                                            <legend>Relatório</legend>
                                            <asp:Menu ID="mnuContagemFisica" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_contagem/cont_estoque_loja_rel.aspx" Text="1. Relatório Contagem Física" Enabled="true"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_contagem/cont_estoque_loja_rel_rec.aspx" Text="2. Relatório Recontagem Física" Enabled="true"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlRetaguarda" runat="server" Visible="true">
                            <table border="0" class="style1">
                                <tr>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 200px;">
                                            <legend>Cadastro</legend>
                                            <asp:Menu ID="menuCadastro" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_contagem/cont_programacao.aspx" Text="1. Programação da Contagem" Enabled="true"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td style="width: 465px;">
                                        <fieldset style="height: 200px;">
                                            <legend>Contagem</legend>
                                            <asp:Menu ID="menuContagemRet" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_contagem/cont_resultado.aspx" Text="1. Resultado de Peças"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_contagem/cont_entrada60mais_resultado.aspx" Text="2. Conferência de Contagem 60+" Enabled="true"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <fieldset style="height: 200px;">
                                            <legend>Relatórios</legend>
                                            <asp:Menu ID="menuRel" runat="server">
                                                <Items>
                                                    <asp:MenuItem NavigateUrl="~/mod_contagem/cont_resultado_grafico.aspx" Text="1. Gráfico de Resultado"></asp:MenuItem>
                                                    <asp:MenuItem NavigateUrl="~/mod_contagem/cont_resultado_media.aspx" Text="2. Resultado de Rede" Enabled="true"></asp:MenuItem>
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
