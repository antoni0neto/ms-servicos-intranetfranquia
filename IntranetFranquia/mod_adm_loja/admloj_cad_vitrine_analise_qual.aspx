<%@ Page Title="Foto - Vitrine da Semana" Language="C#" AutoEventWireup="true" CodeBehind="admloj_cad_vitrine_analise_qual.aspx.cs"
    Inherits="Relatorios.admloj_cad_vitrine_analise_qual" %>

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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Administração de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Vitrine&nbsp;&nbsp;>&nbsp;&nbsp;<asp:Label ID="labTituloMenu" runat="server" Text="Vitrine da Semana - Análise"></asp:Label></span>
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
                                        <td style="text-align: center;">
                                            <asp:Image ID="imgFoto" runat="server" Width="" Height="" AlternateText="Nenhuma Foto" BorderWidth="0" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <fieldset>
                                <legend>Qualificação da Foto da Vitrine</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td colspan="5">
                                            <asp:CheckBox ID="chkMarcarTodos" runat="server" Checked="false" OnCheckedChanged="chkMarcarTodos_CheckedChanged" AutoPostBack="true" />Marcar Todos
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HiddenField ID="hidCodigoVitrineMundoFoto" runat="server" Value="" />
                                            <asp:CheckBox ID="chkHarmonia" runat="server" Checked="false" />Harmonia
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkPosicao" runat="server" Checked="false" />Posição
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkMontagem" runat="server" Checked="false" />Montagem
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkCubos" runat="server" Checked="false" />Cubos
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkAcessorios" runat="server" Checked="false" />Acessórios
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtObservacao" runat="server" Width="100%" TextMode="MultiLine" Height="100px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" style="text-align: right;">
                                            <asp:Button ID="btSalvar" runat="server" Width="100px" Text="Salvar" OnClick="btSalvar_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" style="text-align: right;">
                                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Font-Bold="true" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </fieldset>
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
