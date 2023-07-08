<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultMenuRh.aspx.cs" Inherits="Relatorios.DefaultMenuRh" %>

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
                        <legend>Recursos Humanos</legend>
                        <asp:Menu ID="Menu1" runat="server" StaticDisplayLevels="3">
                            <Items>
                                <asp:MenuItem NavigateUrl="~/CadastroApontamentos.aspx" Text="1. Apontamentos de Entradas e Saídas" Value="M_1"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/xxx.aspx" Text="2. Cadastro de Feriados" Value="M_2"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/xxx.aspx" Text="3. Cadastro de Regras de Premiação" Value="M_3"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/CadastroComissoes.aspx" Text="4. Cadastro de Comissões" Value="M_4"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/xxx.aspx" Text="5. Alterações no Contrato de Trabalho" Value="M_5"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/xxx.aspx" Text="6. Exportação de Dados JGM" Value="M_6"></asp:MenuItem>
                                <asp:MenuItem NavigateUrl="~/xxx.aspx" Text="7. Relatório Final JGM" Value="M_7"></asp:MenuItem>
                            </Items>
                        </asp:Menu>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
