<%@ Page Title="DRE - Importação Folha" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="rh_folha_import.aspx.cs" Inherits="Relatorios.rh_folha_import"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <script src="http://code.jquery.com/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="updatePanel" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btImportar" />
        </Triggers>
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do RH&nbsp;&nbsp;>&nbsp;&nbsp;Folha&nbsp;&nbsp;>&nbsp;&nbsp;DRE - Importação Folha</span>
                <div style="float: right; padding: 0;">
                    <a href="rh_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1200px; margin-left: 8%;">
                <fieldset>
                    <legend>DRE - Importação Folha</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>Competência
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 256px;">
                                <asp:DropDownList runat="server" ID="ddlCompetencia" DataValueField="CODIGO_ACOMPANHAMENTO_MESANO" DataTextField="DESCRICAO"
                                    Height="22px" Width="250px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 200px;">
                                <asp:FileUpload ID="uploadArquivoFolha" runat="server" />
                            </td>
                            <td style="width: 110px;">&nbsp;
                            </td>
                            <td style="text-align: right;">
                                <asp:Button ID="btImportar" runat="server" Text="Importar" Width="100px" OnClick="btImportar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
