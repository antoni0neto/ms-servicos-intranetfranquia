<%@ Page Title="Cadastro de Descrição Produtos E-Commerce" Language="C#" AutoEventWireup="true" CodeBehind="ecom_cad_produto_descricao_edit.aspx.cs"
    Inherits="Relatorios.ecom_cad_produto_descricao_edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bloqueio de Clientes</title>
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Descrição Produtos E-Commerce</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Cadastro de Descrição Produtos E-Commerce</legend>
                            <fieldset style="margin-top: -10px; padding-top: -50px;">
                                <legend>Fotos</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="text-align: center;">
                                            <asp:Image ID="imgFoto" runat="server" Width="156px" Height="228px" AlternateText="Nenhuma Foto" BorderWidth="0" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <fieldset>
                                <legend>Produto</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>Produto
                                        </td>
                                        <td>Grupo Produto
                                        </td>
                                        <td>Nome
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 185px;">
                                            <asp:TextBox ID="txtProduto" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                                Width="175px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 185px;">
                                            <asp:TextBox ID="txtGrupoProduto" runat="server" Width="175px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNome" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="labDescTecido" runat="server" Text="Descrição do Tecido"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtDescTecido" runat="server" Width="100%" TextMode="MultiLine" Height="70px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="labDescPeca" runat="server" Text="Descrição da Peça"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtDescPeca" runat="server" Width="100%" TextMode="MultiLine" Height="70px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="labOcasiaoUso" runat="server" Text="Ocasião de Uso"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtOcasiaoUso" runat="server" Width="100%" TextMode="MultiLine" Height="70px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Button ID="btSalvar" runat="server" Width="200px" Text="Salvar" OnClientClick="DesabilitarBotao(this);" OnClick="btSalvar_Click" />&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
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
</body>
</html>
