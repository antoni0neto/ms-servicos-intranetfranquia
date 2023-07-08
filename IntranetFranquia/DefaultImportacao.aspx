<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultImportacao.aspx.cs" Inherits="Relatorios.DefaultImportacao" %>

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
                        <legend>Importação de Produtos</legend>
                        <table border="1" class="style1">
                            <tr>
                                <td>
                                    <fieldset class="login">
                                        <legend>Gestão</legend>
                                        <asp:Menu ID="Menu6" runat="server" StaticDisplayLevels="3">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/ImportaProdutoProforma.aspx" Text="1. Importação Produtos (LINX)" Value="I_G1"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/AlteraQtdeProforma.aspx" Text="2. Alteração Qtdes Proforma" Value="I_G2"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/ImportaProdutoNel.aspx" Text="3. Importação Produtos NeL" Value="I_G3"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/AlteraQtdeNel.aspx" Text="4. Alteração Qtdes NeL Final" Value="I_G4"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/AlteraQtdeNelOriginal.aspx" Text="5. Alteração Qtdes NeL Original" Value="I_G5"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/DefineNelOriginal.aspx" Text="6. Define NeL Original" Value="I_G6"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset class="login">
                                        <legend>Cadastro</legend>
                                        <asp:Menu ID="Menu9" runat="server" StaticDisplayLevels="3">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/CadastroColecao.aspx" Text="1. Cadastro de Coleção" Value="I_C1"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/CadastroProforma.aspx" Text="2. Cadastro de Proforma" Value="I_C2"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/DefiniProforma.aspx" Text="3. Definição de Proforma" Value="I_C3"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/AlteraNel.aspx" Text="4. Administração de NeL" Value="I_C4"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/CadastroContainer.aspx" Text="5. Cadastro de Container" Value="I_C5"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/CadastroPackGrade.aspx" Text="6. Cadastro de Pack Grade" Value="I_C6"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/CadastroPackGroup.aspx" Text="7. Cadastro de Pack Group" Value="I_C7"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset class="login">
                                        <legend>Relatórios</legend>
                                        <asp:Menu ID="Menu10" runat="server" StaticDisplayLevels="3">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/BuscaGeral.aspx" Text="1. Busca Geral" Value="I_R1"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/ComparaLinxIntranet.aspx" Text="2. Divergências Linx/Intranet" Value="I_R2"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/BuscaPorFornecedor.aspx" Text="3. Busca P/ Cód Fornecedor" Value="I_R3"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/BuscaPorDescricaoProduto.aspx" Text="4. Busca Prod. p/ Descrição" Value="I_R4"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/BuscaProdutoProforma.aspx" Text="5. Busca Prod. Proforma" Value="I_R5"></asp:MenuItem>
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
