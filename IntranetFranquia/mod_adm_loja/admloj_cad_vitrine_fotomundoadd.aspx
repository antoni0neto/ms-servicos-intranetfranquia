<%@ Page Title="Cadastro de Fotos Vitrine - Inclusão" Language="C#" AutoEventWireup="true" CodeBehind="admloj_cad_vitrine_fotomundoadd.aspx.cs"
    Inherits="Relatorios.admloj_cad_vitrine_fotomundoadd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cadastro de Fotos Vitrine - Inclusão</title>
    <style type="text/css">
        .fs {
            border: 1px solid #ccc;
            font-family: Calibri;
        }

        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
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
            <Triggers>
                <asp:PostBackTrigger ControlID="btIncluirFotoVitrine" />
            </Triggers>
            <ContentTemplate>
                <div style="margin-top: 10px; padding-left: 12px; padding-right: 12px; padding-bottom: -350px; background-color: White;">
                    <br />
                    <div>
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Administração de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Vitrine&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Fotos Vitrine - Inclusão</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Cadastro de Fotos Vitrine - Inclusão</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="hidCodigoVitrineMundo" runat="server" Value="" />
                                        <asp:HiddenField ID="hidOrdem" runat="server" Value="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <fieldset style="margin-top: 4px; padding-top: 0;">
                                            <legend>
                                                <asp:Label ID="labFotoVitrineTitulo" runat="server" Text="Foto"></asp:Label></legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 150px; vertical-align: top;">
                                                        <asp:FileUpload ID="uploadFotoVitrine" runat="server" Width="300px" /><br />
                                                        <br />
                                                        <br />
                                                        <br />
                                                        <br />
                                                        <asp:Button ID="btIncluirFotoVitrine" runat="server" OnClick="btIncluirFotoVitrine_Click"
                                                            Text=">>" Width="32px" />
                                                        &nbsp;
                                                        <asp:Label ID="labErro" runat="server" ForeColor="Red" Font-Bold="true" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="text-align: center;">
                                                        <asp:Image ID="imgFotoVitrine" runat="server" Width="80px" Height="100px" AlternateText="Nenhum Arquivo..." BorderWidth="1" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
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
