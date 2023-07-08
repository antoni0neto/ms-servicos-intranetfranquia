<%@ Page Title="Criação de Emails MKT" Language="C#" AutoEventWireup="true" CodeBehind="ecom_email_criacao_conteudo.aspx.cs"
    Inherits="Relatorios.ecom_email_criacao_conteudo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Criação de Emails MKT</title>
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Emails&nbsp;&nbsp;>&nbsp;&nbsp;Criação de Emails MKT</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Criação de Emails MKT - Corpo do Bloco de Email</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="hidCodigoCorpo" runat="server" Value="" />
                                        <asp:HiddenField ID="hidCodigoBloco" runat="server" Value="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <fieldset id="fsBanner" runat="server" visible="false">
                                            <legend>Banner</legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>Link Banner</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtLinkBanner" runat="server" Width="100%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Link Imagem</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtLinkBannerImagem" runat="server" Width="100%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>

                                        <fieldset id="fsLink" runat="server" visible="false">
                                            <legend>Link Botão/Texto</legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>Texto</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtTextoBotaoFrase" runat="server" Width="100%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Link</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtLinkBotaoFrase" runat="server" Width="100%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>

                                        <fieldset id="fsFrase" runat="server" visible="false">
                                            <legend>Frase</legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>Frase</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtFrase" runat="server" Width="100%" TextMode="MultiLine" Height="50px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>

                                        <fieldset id="fsTitulo" runat="server" visible="false">
                                            <legend>Título</legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>Título</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtTitulo" runat="server" Width="100%" TextMode="MultiLine" Height="50px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>

                                        <fieldset id="fsCupom" runat="server" visible="false">
                                            <legend>Cupom</legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>Cupom</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtCupom" runat="server" Width="100%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>

                                        <fieldset id="fs3Produto" runat="server" visible="false">
                                            <legend>
                                                <asp:Label ID="lab3Produto" runat="server" Text=""></asp:Label>
                                            </legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="text-align: center">SKU 1</td>
                                                    <td style="text-align: center">SKU 2</td>
                                                    <td style="text-align: center">SKU 3</td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: center; width: 33.33%">
                                                        <asp:TextBox ID="txtSKU1" runat="server" Width="100px" CssClass="alinharCentro" OnTextChanged="txtSKU_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:DropDownList ID="ddlFoto1" runat="server" Width="130px" Height="21px" OnSelectedIndexChanged="ddlFoto_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                    </td>
                                                    <td style="text-align: center; width: 33.33%">
                                                        <asp:TextBox ID="txtSKU2" runat="server" Width="100px" CssClass="alinharCentro" OnTextChanged="txtSKU_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:DropDownList ID="ddlFoto2" runat="server" Width="130px" Height="21px" OnSelectedIndexChanged="ddlFoto_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                    </td>
                                                    <td style="text-align: center; width: 33.33%">
                                                        <asp:TextBox ID="txtSKU3" runat="server" Width="100px" CssClass="alinharCentro" OnTextChanged="txtSKU_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:DropDownList ID="ddlFoto3" runat="server" Width="130px" Height="21px" OnSelectedIndexChanged="ddlFoto_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: center">
                                                        <asp:Image ID="imgProduto1" runat="server" Width="240px" />
                                                    </td>
                                                    <td style="text-align: center">
                                                        <asp:Image ID="imgProduto2" runat="server" Width="240px" />
                                                    </td>
                                                    <td style="text-align: center">
                                                        <asp:Image ID="imgProduto3" runat="server" Width="240px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labPreco1" runat="server"></asp:Label><br />
                                                        <asp:Label ID="labPrecoPromo1" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labPreco2" runat="server"></asp:Label><br />
                                                        <asp:Label ID="labPrecoPromo2" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labPreco3" runat="server"></asp:Label><br />
                                                        <asp:Label ID="labPrecoPromo3" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: center">SKU 4</td>
                                                    <td style="text-align: center">SKU 5</td>
                                                    <td style="text-align: center">SKU 6</td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: center">
                                                        <asp:TextBox ID="txtSKU4" runat="server" Width="100px" CssClass="alinharCentro" OnTextChanged="txtSKU_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:DropDownList ID="ddlFoto4" runat="server" Width="130px" Height="21px" OnSelectedIndexChanged="ddlFoto_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <asp:TextBox ID="txtSKU5" runat="server" Width="100px" CssClass="alinharCentro" OnTextChanged="txtSKU_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:DropDownList ID="ddlFoto5" runat="server" Width="130px" Height="21px" OnSelectedIndexChanged="ddlFoto_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <asp:TextBox ID="txtSKU6" runat="server" Width="100px" CssClass="alinharCentro" OnTextChanged="txtSKU_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:DropDownList ID="ddlFoto6" runat="server" Width="130px" Height="21px" OnSelectedIndexChanged="ddlFoto_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: center">
                                                        <asp:Image ID="imgProduto4" runat="server" Width="240px" />
                                                    </td>
                                                    <td style="text-align: center">
                                                        <asp:Image ID="imgProduto5" runat="server" Width="240px" />
                                                    </td>
                                                    <td style="text-align: center">
                                                        <asp:Image ID="imgProduto6" runat="server" Width="240px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labPreco4" runat="server"></asp:Label><br />
                                                        <asp:Label ID="labPrecoPromo4" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labPreco5" runat="server"></asp:Label><br />
                                                        <asp:Label ID="labPrecoPromo5" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labPreco6" runat="server"></asp:Label><br />
                                                        <asp:Label ID="labPrecoPromo6" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="labErro" runat="server" Font-Bold="true" ForeColor="Red" Text=""></asp:Label>
                                        &nbsp;
                                        <asp:Button ID="btSalvar" runat="server" Text="Salvar" Width="100px" OnClick="btSalvar_Click" />
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
