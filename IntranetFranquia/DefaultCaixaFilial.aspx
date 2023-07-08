<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultCaixaFilial.aspx.cs" Inherits="Relatorios.DefaultCaixaFilial" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="login">
        <table border="1" class="style1">
            <tr>
                <td>
                    <fieldset class="login">
                        <legend>Caixa de Loja</legend>
                        <table border="1" class="style1">
                            <tr>
                                <td>
                                    <fieldset class="login">
                                        <legend>Fechamento</legend>
                                        <asp:Menu ID="Menu6" runat="server" StaticDisplayLevels="3">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/VerificarFechamentoCaixa.aspx" Text="1. Verificar Fechamento Caixa" Value="I_1"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/VerificarReceitas.aspx" Text="2. Verificar Receitas Caixa" Value="I_2"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/FechamentoCaixaNovo.aspx" Text="3. Fechamento de Caixa" Value="I_3"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset class="login">
                                        <legend>Fiscal</legend>
                                        <asp:Menu ID="Menu9" runat="server" StaticDisplayLevels="3">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/CadastroNotaDefeito.aspx" Text="1. Cadastro NF Defeito" Value="L_1"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/GerirNotaTransferencia.aspx" Text="3. Gestão NF Transf." Value="L_3"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset class="login">
                                        <legend>Apoio</legend>
                                        <asp:Menu ID="Menu10" runat="server" StaticDisplayLevels="3">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/VendaPorVendedor.aspx" Text="1. Venda por Vendedor" Value="L_1"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/ManualFundoFixo.aspx" Text="2. Fundo Fixo" Value="L_2"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/ManualDepositoBanco.aspx" Text="3. Registro Depósito Banco" Value="L_3"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/ManualViagens.aspx" Text="4. Viagens" Value="L_4"></asp:MenuItem>
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
