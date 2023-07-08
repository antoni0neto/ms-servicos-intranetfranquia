<%@ Page Title="Módulo de Seguros" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="seg_menu.aspx.cs" Inherits="Relatorios.seg_menu" %>

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
                        <legend>Módulo de Seguros</legend>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Cadastros</legend>
                                        <asp:Menu ID="menuApoio" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_seguro/seg_tipo_seguro.aspx" Text="1. Cadastro de Tipos de Seguros"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_seguro/seg_seguradora.aspx" Text="2. Cadastro de Seguradoras"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_seguro/seg_corretor.aspx" Text="3. Cadastro de Corretores"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_seguro/seg_tipo_cobertura.aspx" Text="4. Cadastro de Tipos de Coberturas"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Seguros</legend>
                                        <asp:Menu ID="menuSeguro" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_seguro/seg_seguro_cad.aspx?s=0" Text="1. Novo Seguro"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="height: 200px;">
                                        <legend>Relatórios</legend>
                                        <asp:Menu ID="menuRelatorio" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_seguro/seg_rel_seguro.aspx" Text="1. Seguros"></asp:MenuItem>
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
