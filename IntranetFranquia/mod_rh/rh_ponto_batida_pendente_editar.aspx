<%@ Page Title="Editar Batida Pendente" Language="C#" AutoEventWireup="true" CodeBehind="rh_ponto_batida_pendente_editar.aspx.cs"
    Inherits="Relatorios.rh_ponto_batida_pendente_editar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Foto Produto</title>
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo do RH&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Ponto RH&nbsp;&nbsp;>&nbsp;&nbsp;Batidas Pendentes&nbsp;&nbsp;>&nbsp;&nbsp;Editar Batida Pendente</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Editar Batida Pendente</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>Filial
                                        <asp:HiddenField ID="hidCodigoBatida" runat="server" />
                                    </td>
                                    <td>Data Referência
                                    </td>
                                    <td>Funcionário
                                    </td>
                                    <td>Tipo Batida
                                    </td>
                                    <td>Entrada 1
                                    </td>
                                    <td>Saída 1
                                    </td>
                                    <td>Entrada 2
                                    </td>
                                    <td>Saída 2
                                    </td>
                                    <td>Total
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 185px;">
                                        <asp:TextBox ID="txtFilial" runat="server" Width="175px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 120px;">
                                        <asp:TextBox ID="txtDataRef" runat="server" Width="110px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 290px;">
                                        <asp:TextBox ID="txtFuncionario" runat="server" Width="280px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 110px;">
                                        <asp:TextBox ID="txtTipoBatida" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 90px;">
                                        <asp:TextBox ID="txtEntrada1" runat="server" Width="80px" CssClass="alinharCentro" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 90px;">
                                        <asp:TextBox ID="txtSaida1" runat="server" Width="80px" CssClass="alinharCentro" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 90px;">
                                        <asp:TextBox ID="txtEntrada2" runat="server" Width="80px" CssClass="alinharCentro" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 90px;">
                                        <asp:TextBox ID="txtSaida2" runat="server" Width="80px" CssClass="alinharCentro" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotal" runat="server" Width="80px" CssClass="alinharCentro" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="9">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 185px;">
                                        <asp:DropDownList runat="server" ID="ddlFilialN" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                            Height="22px" Width="179px" OnSelectedIndexChanged="ddlFilialN_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 120px;">
                                        <asp:TextBox ID="txtDataReferenciaNova" runat="server" Width="110px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td style="width: 290px;">
                                        <asp:DropDownList runat="server" ID="ddlFuncionarioN" DataValueField="CODIGO" DataTextField="NOME"
                                            Height="22px" Width="284px">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 110px;">
                                        <asp:DropDownList runat="server" ID="ddlTipoBatidaN" DataValueField="CODIGO" DataTextField="DESCRICAO"
                                            Height="22px" Width="104px" OnSelectedIndexChanged="ddlTipoBatidaN_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 90px;">
                                        <asp:TextBox ID="txtEntrada1N" runat="server" Width="80px" MaxLength="5" OnTextChanged="txtHora_TextChanged" AutoPostBack="true"
                                            onkeypress="return fnValidarHora(event);" CssClass="alinharCentro"></asp:TextBox>
                                    </td>
                                    <td style="width: 90px;">
                                        <asp:TextBox ID="txtSaida1N" runat="server" Width="80px" MaxLength="5" OnTextChanged="txtHora_TextChanged" AutoPostBack="true"
                                            onkeypress="return fnValidarHora(event);" CssClass="alinharCentro"></asp:TextBox>
                                    </td>
                                    <td style="width: 90px;">
                                        <asp:TextBox ID="txtEntrada2N" runat="server" Width="80px" MaxLength="5" OnTextChanged="txtHora_TextChanged" AutoPostBack="true"
                                            onkeypress="return fnValidarHora(event);" CssClass="alinharCentro"></asp:TextBox>
                                    </td>
                                    <td style="width: 90px;">
                                        <asp:TextBox ID="txtSaida2N" runat="server" Width="80px" MaxLength="5" OnTextChanged="txtHora_TextChanged" AutoPostBack="true"
                                            onkeypress="return fnValidarHora(event);" CssClass="alinharCentro"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotalN" runat="server" Width="80px" CssClass="alinharCentro" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="9">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    </td>
                                    <td colspan="2" style="text-align: right;">

                                        <asp:Button ID="btSalvar" runat="server" Width="84px" Text="Salvar" OnClick="btSalvar_Click" />
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
