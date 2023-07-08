<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultAdmNF.aspx.cs" Inherits="Relatorios.DefaultAdmNF" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .login
        {
            height: 280px;
            width: 280px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="login">
        <fieldset class="login">
            <legend>Administrativo-Nota Fiscal</legend>
            <asp:Menu ID="Menu14" runat="server" StaticDisplayLevels="3">
                <Items>
                    <asp:MenuItem NavigateUrl="~/GerarNotasDefeito.aspx" Text="1. Gerar Arq.Texto Produtos c/ Defeito" Value="G_1"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/VerificarNotaDefeito.aspx" Text="2. Gestão Baixas Produtos c/ Defeito" Value="G_2"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/AtualizaRetorno.aspx" Text="3. Baixa Retorno Produtos c/ Defeito" Value="G_3"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/PendenciasNotaDefeito.aspx" Text="4. Total Produtos Defeito/Loja" Value="G_4"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/CadastroNotaDefeito.aspx" Text="5. Cadastro Produtos c/ Defeito Matriz" Value="G_5"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/AtualizaDefeitoAtacado.aspx" Text="6. Baixa Destino Prod.c/ Defeito Atacado" Value="G_6"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/BuscaProdutoDefeito.aspx" Text="7. Busca Produto Defeito 2a Qualidade" Value="G_7"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/BaixaDefeitoRetorno.aspx" Text="8. Baixa Defeito Retorno" Value="G_8"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/mod_adm_fiscal/NFeCompararNotas.aspx" Text="9. NFe - Comparar Notas Retaguarda X Portal" Value="G_9"></asp:MenuItem>
                </Items>
            </asp:Menu>
        </fieldset>
    </div>
</asp:Content>
