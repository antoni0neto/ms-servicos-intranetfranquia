<%@ Page Title="Foto - Vitrine da Semana" Language="C#" AutoEventWireup="true" CodeBehind="zold_mkt_con_vitrine_semanal_foto_obs.aspx.cs"
    Inherits="Relatorios.zold_mkt_con_vitrine_semanal_foto_obs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foto - Vitrine da Semana</title>
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }
    </style>
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../js/jquery.elevateZoom-3.0.8.min.js" type="text/javascript"></script>
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Marketing&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Vitrine da Semana</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Foto - Vitrine da Semana</legend>
                            <fieldset style="margin-top: -10px; padding-top: -50px;">
                                <legend>Foto</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="text-align: center; width: 50%">
                                            <asp:Image ID="imgFoto" runat="server" Width="" Height="" AlternateText="Nenhuma Foto" BorderWidth="0" />
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr><td colspan="2">&nbsp;</td></tr>
                                    <tr>
                                        <td style="text-align: center; width: 50%">
                                            <asp:Button ID="btPrev" runat="server" Text="<<" Width="50px" OnClick="btPrev_Click" />
                                            <asp:Button ID="btNext" runat="server" Text=">>" Width="50px" OnClick="btNext_Click" />
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>
                                </table>
                            </fieldset>
                            <fieldset>
                                <legend>E-Mail</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>Semana
                                        </td>
                                        <td>Filial
                                        </td>
                                        <td>Setor
                                        </td>
                                        <td>Tipo
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 250px;">
                                            <asp:TextBox ID="txtSemana" runat="server" Width="240px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 290px;">
                                            <asp:TextBox ID="txtFilial" runat="server" Width="280px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 200px;">
                                            <asp:TextBox ID="txtSetor" runat="server" Width="190px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTipo" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="labEmail" runat="server" Text="Observação"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:TextBox ID="txtEmail" runat="server" Width="100%" TextMode="MultiLine" Height="70px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Button ID="btSalvar" runat="server" Width="200px" Text="Enviar E-Mail" OnClientClick="DesabilitarBotao(this);" OnClick="btSalvar_Click" />&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="hidCodigoFilial" runat="server" Value="" />
                                        <asp:HiddenField ID="hidCodigoVitrine" runat="server" Value="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <br />
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
                                    <strong>Aviso</strong>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; color: red;">ENVIADO COM SUCESSO
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script type="text/javascript">
        $('img').elevateZoom({
            zoomWindowFadeIn: 500,
            zoomWindowFadeOut: 500,
            zoomWindowWidth: 450,
            zoomWindowHeight: 450,
            scrollZoom: true
        });
    </script>
</body>

</html>
