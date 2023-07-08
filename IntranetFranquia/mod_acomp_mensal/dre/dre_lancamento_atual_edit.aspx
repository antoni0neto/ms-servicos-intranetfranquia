<%@ Page Title="DRE - Atualização Lançamento" Language="C#" AutoEventWireup="true" CodeBehind="dre_lancamento_atual_edit.aspx.cs"
    Inherits="Relatorios.dre_lancamento_atual_edit" %>

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
    <link href="../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../../js/js.js" type="text/javascript"></script>
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo Acompanhamento Mensal&nbsp;&nbsp;>&nbsp;&nbsp;DRE&nbsp;&nbsp;>&nbsp;&nbsp;DRE - Atualização Lançamento&nbsp;&nbsp;</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>DRE - Atualização Lançamento</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>Lançamento
                                        <asp:HiddenField ID="hidLancamento" runat="server" />
                                        <asp:HiddenField ID="hidItem" runat="server" />
                                    </td>
                                    <td>Item
                                    </td>
                                    <td>Filial
                                    </td>
                                    <td>Conta Contábil
                                    </td>
                                    <td>Centro de Custo
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 310px;">
                                        <asp:TextBox ID="txtLancamento" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 170px;">
                                        <asp:TextBox ID="txtItem" runat="server" Width="160px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 120px;">
                                        <asp:TextBox ID="txtFilial" runat="server" Width="110px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 267px;">
                                        <asp:TextBox ID="txtContaContabil" runat="server" Width="257px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCentroCusto" runat="server" Width="276px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Débito</td>
                                    <td colspan="4">Histórico</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtDebito" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td colspan="4">
                                        <asp:TextBox ID="txtHistorico" runat="server" Width="833px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;</td>
                                </tr>

                                <tr>
                                    <td colspan="5">
                                        <fieldset>
                                            <legend>Atualização</legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>Filial
                                                    </td>
                                                    <td>Conta Contábil
                                                    </td>
                                                    <td>Centro de Custo
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 300px;">
                                                        <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                                            Height="22px" Width="294px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 400px;">
                                                        <asp:DropDownList runat="server" ID="ddlContaContabil" DataValueField="CONTA_CONTABIL" DataTextField="DESC_CONTA"
                                                            Height="22px" Width="394px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlCCusto" DataValueField="CENTRO_CUSTO" DataTextField="DESC_CENTRO_CUSTO"
                                                            Height="22px" Width="244px">
                                                        </asp:DropDownList>
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
                                        <asp:Button ID="btSalvar" runat="server" Width="122px" Text="Atualizar" OnClientClick="DesabilitarBotao(this);" OnClick="btSalvar_Click" />&nbsp;
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
