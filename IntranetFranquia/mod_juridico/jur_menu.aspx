<%@ Page Title="Módulo Jurídico" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="jur_menu.aspx.cs" Inherits="Relatorios.jur_menu" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1
        {
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
                        <legend>Módulo Jurídico</legend>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Cadastros</legend>
                                        <asp:Menu ID="menuApoio" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_juridico/jur_tipo_processo_cad.aspx" Text="1. Cadastro Tipos de Processo">
                                                </asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_juridico/jur_instancia_cad.aspx" Text="2. Cadastro de Fase">
                                                </asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Processos</legend>
                                        <asp:Menu ID="menuProcesso" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_juridico/jur_processo_cad.aspx?p=0" Text="1. Novo Processo">
                                                </asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_juridico/jur_processo_altera.aspx" Text="2. Alterar Processo">
                                                </asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="height: 200px;">
                                        <legend>Relatórios</legend>
                                        <asp:Menu ID="menuRelatorio" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_juridico/jur_processo_rel.aspx" Text="1. Acompanhamento de Processos">
                                                </asp:MenuItem>
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
