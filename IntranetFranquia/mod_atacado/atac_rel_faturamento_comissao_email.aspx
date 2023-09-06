<%@ Page Title="Comissão Representante - E-Mail" Language="C#" AutoEventWireup="true" CodeBehind="atac_rel_faturamento_comissao_email.aspx.cs"
    Inherits="Relatorios.atac_rel_faturamento_comissao_email" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>E-Mail - Comissão Representante</title>
    <style type="text/css">
        .fs {
            border: 1px solid #ccc;
            font-family: Calibri;
        }

        .alinharDireita {
            text-align: right;
        }
    </style>
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
                <div style="margin-top: 10px; padding-left: 12px; padding-right: 12px; padding-bottom: -350px; background-color: White;">
                    <br />
                    <div>
                        <span style="font-family: Calibri; font-size: 14px;">Módulo do Atacado&nbsp;&nbsp;>&nbsp;&nbsp;
                    Atacado&nbsp;&nbsp;>&nbsp;&nbsp;Comissão de Representante&nbsp;&nbsp;>&nbsp;&nbsp;E-Mail</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Comissão Representante - E-Mail</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 200px;">E-Mail&nbsp;
                                    </td>
                                    <td style="width: 310px; text-align: right;">
                                        <font face="Calibri" size="2" color="gray">(Separe os e-mails com virgula.)</font>&nbsp;&nbsp;&nbsp;

                                    </td>
                                    <td>
                                        <asp:HiddenField ID="hidRepresentante" runat="server" />
                                        <asp:HiddenField ID="hidFilial" runat="server" />
                                        <asp:HiddenField ID="hidAno" runat="server" />
                                        <asp:HiddenField ID="hidMes" runat="server" />
                                        <asp:HiddenField ID="hidEmail" runat="server" />
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtEmail" runat="server" Width="500px" Text=""></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btEnviarEmail" runat="server" Text="Enviar E-Mail"
                                            Width="120px" OnClick="btEnviarEmail_Click" OnClientClick="DesabilitarBotao(this);" />
                                    </td>
                                    <td>&nbsp;</td>
                                    <td style="text-align: right;">
                                        <asp:Button ID="btImprimir" runat="server" Text="Imprimir"
                                            Width="120px" OnClick="btImprimir_Click" OnClientClick="DesabilitarBotao(this);" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <div style="overflow: auto;">
                                            <asp:Panel ID="pnlEmail" runat="server" BackColor="White" BorderWidth="0" BorderColor="Gainsboro"
                                                Height="100%">
                                            </asp:Panel>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
