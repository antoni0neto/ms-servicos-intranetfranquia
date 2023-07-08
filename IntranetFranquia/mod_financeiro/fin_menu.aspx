<%@ Page Title="Módulo Financeiro" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="fin_menu.aspx.cs" Inherits="Relatorios.fin_menu" %>

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
                        <legend>Módulo Financeiro</legend>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 350px;">
                                        <legend>Caixa</legend>
                                        <asp:Menu ID="menuCaixa" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_financeiro/fin_fechamento_caixa_conferencia.aspx?t=1" Text="1. Consultar Fechamento de Caixa"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/VerificarPendenciasFechamentoCaixa.aspx?t=2" Text="2. Verificar Pendências de Caixa"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">
                                    <fieldset style="height: 350px;">
                                        <legend>Depósitos</legend>
                                        <asp:Menu ID="mnuDeposito" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_financeiro/fin_deposito_banco.aspx" Text="1. Depósito em Banco"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_financeiro/fin_deposito_compimagem.aspx" Text="2. Comprovantes"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/GestaoDepositoBanco.aspx" Text="3. Gestão de Depósitos em Banco"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/ListaDepositoStatus.aspx" Text="4. Lista Depósitos Status"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/ResumoDinheiroLoja.aspx" Text="5. Movimento Dinheiro em Loja"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_financeiro/fin_deposito_banco_conferencia.aspx?" Text="6. Conferência de Depósitos em Banco"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">
                                    <fieldset style="height: 350px;">
                                        <legend>Apoio</legend>
                                        <asp:Menu ID="menuApoio" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/ManualDepositoBanco.aspx" Text="1. Registro Depósito Banco"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/ManualViagens.aspx" Text="2. Viagens"></asp:MenuItem>
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
