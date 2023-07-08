<%@ Page Title="Módulo de Facção" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="facc_menu_relatorio.aspx.cs" Inherits="Relatorios.facc_menu_relatorio" %>

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
                        <legend>Módulo de Facção</legend>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Relatórios</legend>
                                        <asp:Menu ID="mnuRelatorio" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo.aspx?t=3" Text="1. Resumo Facção"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_atacadovarejo.aspx?t=3" Text="2. Facção por Atacado/Varejo"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_grupo_faltante.aspx?t=3" Text="3. Facção por Grupo"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_periodo.aspx?t=3" Text="4. Facção por Período"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_time.aspx?t=3" Text="5. Time Facção"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_historico.aspx?t=3" Text="6. Ficha Entrada"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_faccao/facc_resumo_status.aspx?t=3" Text="7. Status Facção"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>







