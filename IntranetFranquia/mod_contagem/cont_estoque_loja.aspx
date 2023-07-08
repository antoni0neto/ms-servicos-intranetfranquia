<%@ Page Title="Contagem Física" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="cont_estoque_loja.aspx.cs" Inherits="Relatorios.cont_estoque_loja"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Contagem&nbsp;&nbsp;>&nbsp;&nbsp;Estoque Loja&nbsp;&nbsp;>&nbsp;&nbsp;Contagem Física</span>
                <div style="float: right; padding: 0;">
                    <a href="cont_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1200px; margin-left: 8%;">
                <fieldset>
                    <legend>Contagem Física</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>Filial
                            </td>
                            <td>Descrição
                            </td>
                            <td>Data Contagem
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
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="22px" Width="250px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 200px;">
                                <asp:TextBox ID="txtDescricao" runat="server" Width="190px"></asp:TextBox>
                            </td>
                            <td style="width: 130px;">
                                <asp:TextBox ID="txtDataContagem" runat="server" Width="120px" MaxLength="10" Style="text-align: right;"
                                    onkeypress="return fnValidarData(event);"></asp:TextBox>
                            </td>
                            <td style="width: 200px;">
                                <asp:FileUpload ID="upEstoqueTxt" runat="server" />
                            </td>
                            <td style="width: 110px;">&nbsp;
                            </td>
                            <td style="text-align: right;">
                                <asp:Button ID="btImportar" runat="server" Text="Importar" Width="100px" OnClick="btImportar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Button ID="btAbrirRelatorio" runat="server" Text="Abrir Relatório" Width="100px" Visible="false" OnClick="btAbrirRelatorio_Click" />
                            </td>
                        </tr>
                    </table>
                    <div>
                        <asp:HiddenField ID="hidCodigoEstoqueLojaCont" runat="server" Value="" />
                    </div>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
