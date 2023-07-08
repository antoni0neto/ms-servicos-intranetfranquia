<%@ Page Title="Manutenção de Documentos" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="faccadm_manutencao.aspx.cs" Inherits="Relatorios.faccadm_manutencao"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .jGrowl .manilla {
            color: black;
        }

        .jGrowl .redError {
            color: red;
        }

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
    <asp:UpdatePanel ID="up1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btDocumentoIncluir" />
        </Triggers>
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Facção Administrativo&nbsp;&nbsp;>&nbsp;&nbsp;Documentos&nbsp;&nbsp;>&nbsp;&nbsp;<asp:Label
                    ID="labTituloMenu" runat="server" Text="Manutenção de Documentos"></asp:Label></span>
                <div style="float: right; padding: 0;">
                    <a href="faccadm_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 962px; margin-left: 16%;">
                <fieldset>
                    <legend>
                        <asp:Label ID="labTitulo" runat="server" Text="Manutenção de Documentos"></asp:Label></legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="2">&nbsp;
                            </td>
                            <td style="text-align: right;">&nbsp;
                                    <asp:ImageButton ID="btImprimir" runat="server" Height="20px" Width="20px" ImageUrl="~/Image/print.png"
                                        OnClick="btImprimir_Click" ToolTip="Imprimir Carta de Pedido de Demissão" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="labFaccao" runat="server" Text="Facção"></asp:Label>
                                <asp:HiddenField ID="hidCodigoFaccaoDocumento" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="labTipoDocumento" runat="server" Text="Tipo de Documento"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:DropDownList runat="server" ID="ddlFaccao" DataValueField="NOME_CLIFOR" DataTextField="NOME_CLIFOR"
                                    Height="22px" Width="414px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlTipoDocumento" DataValueField="CODIGO" DataTextField="DOCUMENTO"
                                    Height="22px" Width="515px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="labDataVigenciaIni" runat="server" Text="Vigência Inicial"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labDataVigenciaFim" runat="server" Text="Vigência Final"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 210px;">
                                <asp:TextBox ID="txtDataVigenciaIni" runat="server" MaxLength="10" Width="200px"
                                    onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                            </td>
                            <td style="width: 210px;">
                                <asp:TextBox ID="txtDataVigenciaFim" runat="server" MaxLength="10" Width="200px"
                                    onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <fieldset style="margin-top: 4px; padding-top: 0;">
                                    <legend>
                                        <asp:Label ID="labDocumento" runat="server" Text="Documento"></asp:Label></legend>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 150px; vertical-align: top;">
                                                <asp:FileUpload ID="uploadDocumento" runat="server" Width="300px" /><br />
                                                <span style="font-size: smaller; color: red;">Tamanho Máximo: 3MB / Extensão: .PDF</span>
                                                <br />
                                                <br />
                                                <asp:Button ID="btDocumentoIncluir" runat="server" OnClick="btDocumentoIncluir_Click"
                                                    Text=">>" Width="32px" />
                                            </td>
                                            <td style="text-align: right;" valign="top">
                                                <asp:TextBox ID="txtDocumentoCaminho" runat="server" Enabled="false" Width="400px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="labObs" runat="server" Text="Observação"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:TextBox ID="txtObs" runat="server" TextMode="MultiLine" Height="40px" Width="922px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btExcluir" runat="server" Text="Excluir" Width="110px" OnClick="btExcluir_Click"
                                    OnClientClick="return ConfirmarExclusao();" Visible="false" />

                            </td>
                            <td colspan="2" style="text-align: right; line-height: 40px;">
                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                &nbsp;
                                <asp:Button ID="btEnviar" runat="server" Text="Enviar" Width="110px" OnClick="btEnviar_Click"
                                    OnClientClick="DesabilitarBotao(this);" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div id="dialogPai" runat="server">
                <div id="dialog" title="Mensagem" class="divPop">
                    <table border="0" width="100%">
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; color: red;">
                                <strong>Documento:&nbsp;&nbsp;<asp:Label ID="labPopUp" runat="server" Text=""></asp:Label></strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; color: red;">
                                <asp:Label ID="labMensagem" runat="server" Text="CADASTRADO COM SUCESSO"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>Por favor, selecione a tela desejada nos botões abaixo.
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
