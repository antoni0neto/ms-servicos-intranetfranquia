<%@ Page Title="Apuração PIS e COFINS - Resumo" Language="C#" AutoEventWireup="true" CodeBehind="fisc_apuracao_imposto_det.aspx.cs"
    Inherits="Relatorios.fisc_apuracao_imposto_det" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Apuração PIS e COFINS - Resumo</title>
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo do Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Apuração PIS e COFINS&nbsp;&nbsp;>&nbsp;&nbsp;Resumo</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Apuração PIS e COFINS - Resumo</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>Empresa
                                        <asp:HiddenField ID="hidCTBApuracaoEmpresa" runat="server" />
                                        <asp:HiddenField ID="hidTipoImposto" runat="server" />
                                    </td>
                                    <td>Competência
                                    </td>
                                    <td>Imposto</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width: 250px;">
                                        <asp:TextBox ID="txtEmpresa" runat="server" Width="240px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td style="width: 200px;">
                                        <asp:TextBox ID="txtCompetencia" runat="server" Width="190px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtImposto" runat="server" Width="150px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Button ID="btImprimir" runat="server" Width="130px" Text="Imprimir Cálculo" OnClick="btImprimir_Click" />
                                        &nbsp;
                                        <asp:Button ID="btDARF" runat="server" Width="130px" Text="DARF" OnClick="btDARF_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="border: 1px solid #ccc;">
                                        <br />
                                        <br />
                                        <asp:Panel ID="pnlApuracaoImp" runat="server" BackColor="White" BorderWidth="1" BorderColor="White"
                                            Height="100%">
                                        </asp:Panel>
                                        <br />
                                        <br />
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
