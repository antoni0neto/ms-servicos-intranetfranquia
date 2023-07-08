<%@ Page Title="Módulo de Frete" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="fre_menu.aspx.cs" Inherits="Relatorios.mod_frete.fre_menu" %>

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
                        <legend>Módulo de Frete</legend>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Cadastros</legend>
                                        <asp:Menu ID="menuApoio" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_frete/fre_cad_grupo_precomin.aspx" Text="1. Grupos e Preço Mínimo"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_frete/fre_cad_tipo_pagamento.aspx" Text="2. Tipos de Pagamento"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_frete/fre_cad_tipo_amostra.aspx" Text="3. Tipos de Amostra"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_frete/fre_cad_tipo_envio.aspx" Text="4. Tipos de Envio"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_frete/fre_cad_valor_peso.aspx" Text="5. Custo por Peso"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_frete/fre_cad_taxa_combustivel.aspx" Text="6. Taxas de Combustível"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Controle AWB</legend>
                                        <asp:Menu ID="menuProcesso" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_frete/fre_cad_recebimento_nota.aspx" Text="1. Recebimento de Nota"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_frete/fre_cad_imposto.aspx" Text="2. Impostos"></asp:MenuItem>
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

