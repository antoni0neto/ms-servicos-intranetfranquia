<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultMenuAdmLoja.aspx.cs" Inherits="Relatorios.DefaultMenuAdmLoja" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .login
        {
            height: 320px;
            width: 280px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="login">
        <table border="1" class="style1">
            <tr>
                <td>
                    <fieldset class="login">
                        <legend>Administrador de Loja</legend>
                        <asp:Menu ID="Menu1" runat="server" StaticDisplayLevels="3">
                            <Items>
                                <asp:MenuItem NavigateUrl="~/VerificarFechamentoCaixa.aspx" Text="4. Verificar Fechamento Caixa" Value="I_4"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/VerificarDespesas.aspx" Text="5. Verificar Despesas Caixa" Value="I_5"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/VerificarReceitas.aspx" Text="6. Verificar Receitas Caixa" Value="I_6"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/FechamentoCaixaNovo.aspx" Text="1. Fechamento de Caixa" Value="M_1"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/FechamentoFundoFixo.aspx" Text="2. Fechamento Fundo Fixo" Value="M_2"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/DepositoBanco.aspx" Text="3. Depósito em Banco" Value="M_3"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/CadastroNotaDefeito.aspx" Text="1. Cadastro NF Defeito" Value="L_1"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/GerirNotaTransferencia.aspx" Text="3. Gestão NF Transf." Value="L_3"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/VendaPorVendedor.aspx" Text="3. Venda por Vendedor" Value="L_3"></asp:MenuItem>
                            </Items>
                        </asp:Menu>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
