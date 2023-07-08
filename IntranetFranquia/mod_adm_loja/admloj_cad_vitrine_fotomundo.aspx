<%@ Page Title="Cadastro de Fotos Vitrine" Language="C#" AutoEventWireup="true" CodeBehind="admloj_cad_vitrine_fotomundo.aspx.cs"
    Inherits="Relatorios.admloj_cad_vitrine_fotomundo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cadastro de Fotos Vitrine</title>
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }
    </style>

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Administração de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Vitrine&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Fotos Vitrine</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Cadastro de Fotos Vitrine</legend>
                            <div>
                                <asp:HiddenField ID="hidCodigoVitrine" runat="server" Value="" />
                                <asp:HiddenField ID="hidCodigoFilial" runat="server" Value="" />
                                <asp:HiddenField ID="hidDataIni" runat="server" Value="" />
                                <asp:HiddenField ID="hidDataFim" runat="server" Value="" />
                            </div>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtFilial" runat="server" Text="" Width="300px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Button ID="btFechar" runat="server" Text="Fechar" Width="140px" OnClick="btFechar_Click" /><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btAtualizar1" runat="server" Text="Atualizar" Width="120px" OnClick="btAtualizar_Click" />
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Button ID="btMarcarSugestao" runat="server" Text="Marcar Sugestão" Width="140px" OnClick="btMarcarSugestao_Click" />
                                    </td>
                                </tr>
                            </table>
                            <fieldset>
                                <legend>Feminino</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0" style="text-align: center;">
                                    <tr>
                                        <td colspan="3">
                                            <asp:HiddenField ID="hidCodigoVitrineFeminino" runat="server" Value="" />
                                            Principal&nbsp;&nbsp;&nbsp;<font size="2" color="red" style="font-weight: bold;">(1900x850)</font>
                                            <asp:ImageButton ID="imgEFem0" runat="server" ImageUrl="~/Image/delete.png" Width="13px" OnClick="imgExcluir_Click" />&nbsp;
                                            <asp:ImageButton ID="imgEFem0SemFoto" runat="server" ImageUrl="~/Image/clean.png" Width="15px" OnClick="imgSemFoto_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:ImageButton ID="btFemininoPrincipal" runat="server" Height="300px" OnClick="btAdicionarFoto_Click" CommandArgument="0" AlternateText=" " ToolTip=" " />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 33%;">Mundo 1&nbsp;&nbsp;&nbsp;<font size="2" color="red" style="font-weight: bold;">(361x540)</font>
                                            <asp:ImageButton ID="imgEFem1" runat="server" ImageUrl="~/Image/delete.png" Width="13px" OnClick="imgExcluir_Click" />&nbsp;
                                            <asp:ImageButton ID="imgEFem1SemFoto" runat="server" ImageUrl="~/Image/clean.png" Width="15px" OnClick="imgSemFoto_Click" />
                                        </td>
                                        <td style="width: 33%;">Mundo 2&nbsp;&nbsp;&nbsp;<font size="2" color="red" style="font-weight: bold;">(361x540)</font>
                                            <asp:ImageButton ID="imgEFem2" runat="server" ImageUrl="~/Image/delete.png" Width="13px" OnClick="imgExcluir_Click" />&nbsp;
                                            <asp:ImageButton ID="imgEFem2SemFoto" runat="server" ImageUrl="~/Image/clean.png" Width="15px" OnClick="imgSemFoto_Click" />
                                        </td>
                                        <td style="width: 33%;">Mundo 3&nbsp;&nbsp;&nbsp;<font size="2" color="red" style="font-weight: bold;">(361x540)</font>
                                            <asp:ImageButton ID="imgEFem3" runat="server" ImageUrl="~/Image/delete.png" Width="13px" OnClick="imgExcluir_Click" />&nbsp;
                                            <asp:ImageButton ID="imgEFem3SemFoto" runat="server" ImageUrl="~/Image/clean.png" Width="15px" OnClick="imgSemFoto_Click" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="btFemininoMundo1" runat="server" Width="250px" OnClick="btAdicionarFoto_Click" ImageAlign="Middle" CommandArgument="1" AlternateText=" " ToolTip=" " />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="btFemininoMundo2" runat="server" Width="250px" OnClick="btAdicionarFoto_Click" ImageAlign="Middle" CommandArgument="2" AlternateText=" " ToolTip=" " />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="btFemininoMundo3" runat="server" Width="250px" OnClick="btAdicionarFoto_Click" ImageAlign="Middle" CommandArgument="3" AlternateText=" " ToolTip=" " />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <div>
                                <asp:Button ID="btAtualizar2" runat="server" Text="Atualizar" Width="120px" OnClick="btAtualizar_Click" />
                            </div>
                            <fieldset>
                                <legend>Masculino</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0" style="text-align: center;">
                                    <tr>
                                        <td colspan="2">
                                            <asp:HiddenField ID="hidCodigoVitrineMasculino" runat="server" Value="" />
                                            Principal&nbsp;&nbsp;&nbsp;<font size="2" color="red" style="font-weight: bold;">(1900x850)</font>
                                            <asp:ImageButton ID="imgEMasc0" runat="server" ImageUrl="~/Image/delete.png" Width="13px" OnClick="imgExcluir_Click" />&nbsp;
                                            <asp:ImageButton ID="imgEMasc0SemFoto" runat="server" ImageUrl="~/Image/clean.png" Width="15px" OnClick="imgSemFoto_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:ImageButton ID="btMasculinoPrincipal" runat="server" Height="300px" OnClick="btAdicionarFoto_Click" CommandArgument="0" AlternateText=" " ToolTip=" " />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">Mundo 1&nbsp;&nbsp;&nbsp;<font size="2" color="red" style="font-weight: bold;">(361x540)</font>
                                            <asp:ImageButton ID="imgEMasc1" runat="server" ImageUrl="~/Image/delete.png" Width="13px" OnClick="imgExcluir_Click" />&nbsp;
                                            <asp:ImageButton ID="imgEMasc1SemFoto" runat="server" ImageUrl="~/Image/clean.png" Width="15px" OnClick="imgSemFoto_Click" />
                                        </td>
                                        <td style="width: 50%;">Mundo 2&nbsp;&nbsp;&nbsp;<font size="2" color="red" style="font-weight: bold;">(361x540)</font>
                                            <asp:ImageButton ID="imgEMasc2" runat="server" ImageUrl="~/Image/delete.png" Width="13px" OnClick="imgExcluir_Click" />&nbsp;
                                            <asp:ImageButton ID="imgEMasc2SemFoto" runat="server" ImageUrl="~/Image/clean.png" Width="15px" OnClick="imgSemFoto_Click" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="btMasculinoMundo1" runat="server" Width="250px" OnClick="btAdicionarFoto_Click" ImageAlign="Middle" CommandArgument="1" AlternateText=" " ToolTip=" " />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="btMasculinoMundo2" runat="server" Width="250px" OnClick="btAdicionarFoto_Click" ImageAlign="Middle" CommandArgument="2" AlternateText=" " ToolTip=" " />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <div>
                                <asp:Button ID="btAtualizar3" runat="server" Text="Atualizar" Width="120px" OnClick="btAtualizar_Click" />
                            </div>
                            <fieldset>
                                <legend>Porta</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0" style="text-align: center;">
                                    <tr>
                                        <td colspan="2">
                                            <asp:HiddenField ID="hidCodigoVitrinePorta" runat="server" Value="" />
                                            Principal&nbsp;&nbsp;&nbsp;<font size="2" color="red" style="font-weight: bold;">(1900x850)</font>
                                            <asp:ImageButton ID="imgEPorta0" runat="server" ImageUrl="~/Image/delete.png" Width="13px" OnClick="imgExcluir_Click" />&nbsp;
                                            <asp:ImageButton ID="imgEPorta0SemFoto" runat="server" ImageUrl="~/Image/clean.png" Width="15px" OnClick="imgSemFoto_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:ImageButton ID="btPortaPrincipal" runat="server" Height="300px" OnClick="btAdicionarFoto_Click" CommandArgument="0" AlternateText=" " ToolTip=" " />
                                        </td>
                                    </tr>
                                </table>


                            </fieldset>
                            <div>
                                <asp:Button ID="btAtualizar4" runat="server" Text="Atualizar" Width="120px" OnClick="btAtualizar_Click" />
                            </div>
                            <fieldset>
                                <legend>Extra</legend>

                                <table border="0" width="100%" cellpadding="0" cellspacing="0" style="text-align: center;">
                                    <tr>
                                        <td colspan="2">
                                            <asp:HiddenField ID="hidCodigoVitrineExtra" runat="server" Value="" />
                                            Principal&nbsp;&nbsp;&nbsp;<font size="2" color="red" style="font-weight: bold;">(1900x850)</font>
                                            <asp:ImageButton ID="imgEExtra0" runat="server" ImageUrl="~/Image/delete.png" Width="13px" OnClick="imgExcluir_Click" />&nbsp;
                                            <asp:ImageButton ID="imgEExtra0SemFoto" runat="server" ImageUrl="~/Image/clean.png" Width="15px" OnClick="imgSemFoto_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:ImageButton ID="btExtraPrincipal" runat="server" Height="300px" OnClick="btAdicionarFoto_Click" CommandArgument="0" AlternateText=" " ToolTip=" " />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">Mundo 1&nbsp;&nbsp;&nbsp;<font size="2" color="red" style="font-weight: bold;">(361x540)</font>
                                            <asp:ImageButton ID="imgEExtra1" runat="server" ImageUrl="~/Image/delete.png" Width="13px" OnClick="imgExcluir_Click" />&nbsp;
                                            <asp:ImageButton ID="imgEExtra1SemFoto" runat="server" ImageUrl="~/Image/clean.png" Width="15px" OnClick="imgSemFoto_Click" />
                                        </td>
                                        <td style="width: 50%;">Mundo 2&nbsp;&nbsp;&nbsp;<font size="2" color="red" style="font-weight: bold;">(361x540)</font>
                                            <asp:ImageButton ID="imgEExtra2" runat="server" ImageUrl="~/Image/delete.png" Width="13px" OnClick="imgExcluir_Click" />&nbsp;
                                            <asp:ImageButton ID="imgEExtra2SemFoto" runat="server" ImageUrl="~/Image/clean.png" Width="15px" OnClick="imgSemFoto_Click" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="btExtraMundo1" runat="server" Width="250px" OnClick="btAdicionarFoto_Click" ImageAlign="Middle" CommandArgument="1" AlternateText=" " ToolTip=" " />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="btExtraMundo2" runat="server" Width="250px" OnClick="btAdicionarFoto_Click" ImageAlign="Middle" CommandArgument="2" AlternateText=" " ToolTip=" " />
                                        </td>
                                    </tr>
                                </table>

                            </fieldset>

                        </fieldset>
                    </div>
                    <br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
