<%@ Page Title="Recontagem Física" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="cont_estoque_loja_rec.aspx.cs" Inherits="Relatorios.cont_estoque_loja_rec"
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
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Contagem&nbsp;&nbsp;>&nbsp;&nbsp;Estoque Loja&nbsp;&nbsp;>&nbsp;&nbsp;Recontagem Física</span>
                <div style="float: right; padding: 0;">
                    <a href="cont_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1200px; margin-left: 8%;">
                <fieldset>
                    <legend>Recontagem Física</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>Filial
                            </td>
                            <td>Contagem</td>
                            <td>Descrição
                            </td>
                            <td>Data Recontagem
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 256px;">
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="22px" Width="250px" OnSelectedIndexChanged="ddlFilial_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 256px;">
                                <asp:DropDownList runat="server" ID="ddlContagem" DataValueField="CODIGO" DataTextField="DESCRICAO"
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
                            <td style="text-align: right;">
                                <asp:Button ID="btGerar" runat="server" Text="Gerar Recontagem" Width="130px" OnClick="btGerar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;&nbsp;
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
                                <asp:Button ID="btAbrirRecontagem" runat="server" Text="Abrir Recontagem" Width="130px" Visible="false" OnClick="btAbrirRecontagem_Click" />
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
