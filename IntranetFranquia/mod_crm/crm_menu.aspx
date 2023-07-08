<%@ Page Title="Módulo de CRM" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="crm_menu.aspx.cs" Inherits="Relatorios.crm_menu" %>

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
                        <legend>Módulo de CRM</legend>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Clientes</legend>
                                        <asp:Menu ID="mnuCliente" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_crm/crm_cliente.aspx?t=1" Text="1. Clientes" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_crm/crm_cliente_acao.aspx?t=1" Text="2. Clientes - Ação" Enabled="true"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Clientes</legend>
                                        <asp:Menu ID="mnuRel" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_crm/crm_ciclo_cliente.aspx?t=1" Text="1. Ciclo de Clientes"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_crm/crm_rfv.aspx?t=1" Text="2. RFV – Recência, Frequência e Valor"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_crm/crm_rfv_score.aspx?t=1&da=" Text="3. RFV – Score"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
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
