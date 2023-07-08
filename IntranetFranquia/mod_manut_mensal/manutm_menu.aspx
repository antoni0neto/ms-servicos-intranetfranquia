<%@ Page Title="Módulo de Manutenção de Aluguéis" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="manutm_menu.aspx.cs"
    Inherits="Relatorios.manutm_menu" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1 {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="login">
        <table border="0" class="style1">
            <tr>
                <td>
                    <fieldset class="login">
                        <legend>Módulo Manutenção Mensal</legend>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Cadastros</legend>
                                        <asp:Menu ID="mnuCadastro" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_manut_mensal/manutm_cad_aluguel_linx_intra.aspx" Text="1. Cadastro de Aluguéis pelo LINX"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_manut_mensal/manutm_cad_aluguel.aspx" Text="2. DRE - Aluguéis"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">&nbsp;
                                </td>
                                <td style="width: 465px;">&nbsp;
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
