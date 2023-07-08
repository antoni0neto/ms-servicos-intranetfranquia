<%@ Page Title="Nota - Avaliação de Produção" Language="C#" AutoEventWireup="true" CodeBehind="facc_avaliacao_prod_baixa.aspx.cs"
    Inherits="Relatorios.facc_avaliacao_prod_baixa" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Nota - Avaliação de Produção</title>
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
            <ContentTemplate>
                <div style="margin-top: 10px; padding-left: 12px; padding-right: 12px; padding-bottom: -350px; background-color: White;">
                    <br />
                    <div>
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Facção&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Facção&nbsp;&nbsp;>&nbsp;&nbsp;Avaliação de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Nota - Avaliação de Produção</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Nota - Avaliação de Produção</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>Coleção
                                        <asp:HiddenField ID="hidCodigoAvaliacao" runat="server" />
                                    </td>
                                    <td>HB
                                    </td>
                                    <td>Produto
                                    </td>
                                    <td>Nome
                                    </td>
                                    <td>Cor
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 310px;">
                                        <asp:TextBox ID="txtColecao" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 170px;">
                                        <asp:TextBox ID="txtHB" runat="server" Width="160px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 120px;">
                                        <asp:TextBox ID="txtProduto" runat="server" Width="110px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 267px;">
                                        <asp:TextBox ID="txtNome" runat="server" Width="257px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCor" runat="server" Width="276px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="labFornecedor" runat="server" Text="Fornecedor"></asp:Label>
                                    </td>
                                    <td colspan="4">
                                        <asp:Label ID="labEnvio" runat="server" Text="Data de Envio"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlFornecedor" runat="server" Width="304px" Height="21px" DataTextField="FORNECEDOR"
                                            DataValueField="FORNECEDOR" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="4">
                                        <asp:TextBox ID="txtDataEnvio" runat="server" Width="160px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <fieldset>
                                            <legend>
                                                <asp:Label ID="labNota" runat="server" Text="Nota"></asp:Label></legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="labNota1" runat="server" Text="Nota 1"></asp:Label>
                                                    </td>
                                                    <td>Observação 1
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 170px;">
                                                        <asp:TextBox ID="txtNota1" runat="server" Width="160px" MaxLength="5" CssClass="alinharDireita"
                                                            onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtObs1" runat="server" Width="286px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Nota 2
                                                    </td>
                                                    <td>Observação 2
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtNota2" runat="server" Width="160px" MaxLength="5" CssClass="alinharDireita"
                                                            onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtObs2" runat="server" Width="286px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="5" style="text-align: right;">
                                        <asp:Button ID="btSalvar" runat="server" Width="122px" Text="Baixar" OnClientClick="DesabilitarBotao(this);" OnClick="btSalvar_Click" />&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;
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
