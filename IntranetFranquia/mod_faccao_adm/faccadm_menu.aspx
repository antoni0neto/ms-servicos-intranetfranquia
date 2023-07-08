<%@ Page Title="Módulo de Facção Administrativo" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="faccadm_menu.aspx.cs" Inherits="Relatorios.faccadm_menu" %>

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
                        <legend>Módulo de Facção Administrativo</legend>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Cadastros</legend>
                                        <asp:Menu ID="menuApoio" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_faccao_adm/faccadm_tipo_doc_cad.aspx" Text="1. Cadastro de Tipos de Documento"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_faccao_adm/faccadm_faccao_cad.aspx" Text="2. Cadastro de Facção"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Documentos</legend>
                                        <asp:Menu ID="menuSeguro" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_faccao_adm/faccadm_manutencao.aspx?d=0" Text="1. Manutenção de Documentos"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="height: 200px;">
                                        <legend>Relatórios</legend>
                                        <asp:Menu ID="menuRelatorio" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_faccao_adm/faccadm_rel_documento.aspx" Text="1. Documentos"></asp:MenuItem>
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
