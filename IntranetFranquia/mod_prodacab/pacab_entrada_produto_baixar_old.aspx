<%@ Page Title="Definição Logística - Grade" Language="C#" AutoEventWireup="true" CodeBehind="pacab_entrada_produto_baixar_old.aspx.cs"
    Inherits="Relatorios.pacab_entrada_produto_baixar_old" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Entrada de Produto Acabado</title>
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />
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
    <script src="http://code.jquery.com/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript">
        function SomarGrade(control) {
            var total = 0;

            if (document.getElementById("txtGradeEXP_R").value != "")
                total = total + parseInt(document.getElementById("txtGradeEXP_R").value);

            if (document.getElementById("txtGradeXP_R").value != "")
                total = total + parseInt(document.getElementById("txtGradeXP_R").value);

            if (document.getElementById("txtGradePP_R").value != "")
                total = total + parseInt(document.getElementById("txtGradePP_R").value);

            if (document.getElementById("txtGradeP_R").value != "")
                total = total + parseInt(document.getElementById("txtGradeP_R").value);

            if (document.getElementById("txtGradeM_R").value != "")
                total = total + parseInt(document.getElementById("txtGradeM_R").value);

            if (document.getElementById("txtGradeG_R").value != "")
                total = total + parseInt(document.getElementById("txtGradeG_R").value);

            if (document.getElementById("txtGradeGG_R").value != "")
                total = total + parseInt(document.getElementById("txtGradeGG_R").value);

            document.getElementById("txtGradeTotal_R").value = total;
        }
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Produto Acabado&nbsp;&nbsp;>&nbsp;&nbsp;Peças&nbsp;&nbsp;>&nbsp;&nbsp;Definição Logística&nbsp;&nbsp;>&nbsp;&nbsp;Grade</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Grade</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>Coleção
                                        <asp:HiddenField ID="hidCodigoEntrada" runat="server" />
                                        <asp:HiddenField ID="hidControleInsercao" runat="server" Value="" />
                                    </td>
                                    <td>Produto
                                    </td>
                                    <td>Nome
                                    </td>
                                    <td>Cor
                                    </td>
                                    <td>Quantidade
                                    </td>
                                    <td>Griffe
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 307px;">
                                        <asp:TextBox ID="txtColecao" runat="server" Width="297px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 120px;">
                                        <asp:TextBox ID="txtProduto" runat="server" Width="110px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 267px;">
                                        <asp:TextBox ID="txtNome" runat="server" Width="257px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 200px;">
                                        <asp:TextBox ID="txtCor" runat="server" Width="190px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 130px;">
                                        <asp:TextBox ID="txtQtde" runat="server" Width="120px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGriffe" runat="server" Width="118px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">Fornecedor</td>
                                    <td >Filial</td>
                                    <td colspan="3">Pedido</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtFornecedor" runat="server" Width="417px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFilial" runat="server" Width="257px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtPedido" runat="server" Width="190px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">&nbsp;</td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labQtdeNota" runat="server" Text="Qtde Nota"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labNF" runat="server" Text="NF Entrada"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labSerie" runat="server" Text="Série"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labEmissao" runat="server" Text="Emissão"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labRecebimento" runat="server" Text="Recebimento"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlFilial" Height="22px" Width="301px">
                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="000029" Text="CD - LUGZY               " Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtQtdeNota" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                            Width="110px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNF" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                            Width="257px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSerie" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                            Width="191px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmissao" runat="server" MaxLength="10" Width="120px"
                                            onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRecebimento" runat="server" MaxLength="10" Width="120px"
                                            onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <fieldset>
                                            <legend>Grade</legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="text-align: left">&nbsp;
                                                    </td>
                                                    <td style="text-align: left">EXP
                                                    </td>
                                                    <td style="text-align: left">XP
                                                    </td>
                                                    <td style="text-align: left">PP
                                                    </td>
                                                    <td style="text-align: left">P
                                                    </td>
                                                    <td style="text-align: left">M
                                                    </td>
                                                    <td style="text-align: left">G
                                                    </td>
                                                    <td style="text-align: left">GG
                                                    </td>
                                                    <td style="text-align: left">Total
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 80px;" valign="middle">Enviada
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeEXP_E" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeXP_E" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradePP_E" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeP_E" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeM_E" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeG_E" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeGG_E" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 117px;">
                                                        <asp:TextBox ID="txtGradeTotal_E" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="107px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="text-align: right;">&nbsp;
                                                    </td>
                                                </tr>
                                                <tr style="line-height: 9px;">
                                                    <td colspan="10">&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 80px;" valign="middle">Recebida
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeEXP_R" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeXP_R" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradePP_R" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeP_R" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeM_R" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeG_R" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeGG_R" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 117px;">
                                                        <asp:TextBox ID="txtGradeTotal_R" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="107px"></asp:TextBox>
                                                    </td>
                                                    <td style="text-align: right;">&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="1">
                                        <asp:Label ID="labStatus" runat="server" Text="Status"></asp:Label>
                                    </td>
                                    <td colspan="5">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="1">
                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="301px" Height="22px" >
                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="B" Text="FINALIZADO" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="5" style="text-align: right;">
                                        <asp:Button ID="btSalvar" runat="server" Width="122px" Text="Baixar" OnClientClick="DesabilitarBotao(this);" OnClick="btSalvar_Click" />&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
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
