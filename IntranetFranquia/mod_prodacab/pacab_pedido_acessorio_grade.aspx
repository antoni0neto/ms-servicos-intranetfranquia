<%@ Page Title="Produto - Grade" Language="C#" AutoEventWireup="true" CodeBehind="pacab_pedido_acessorio_grade.aspx.cs"
    Inherits="Relatorios.pacab_pedido_acessorio_grade" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Gerar Pedido</title>
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

            if (document.getElementById("txtGrade1").value != "")
                total = total + parseInt(document.getElementById("txtGrade1").value);

            if (document.getElementById("txtGrade2").value != "")
                total = total + parseInt(document.getElementById("txtGrade2").value);

            if (document.getElementById("txtGrade3").value != "")
                total = total + parseInt(document.getElementById("txtGrade3").value);

            if (document.getElementById("txtGrade4").value != "")
                total = total + parseInt(document.getElementById("txtGrade4").value);

            if (document.getElementById("txtGrade5").value != "")
                total = total + parseInt(document.getElementById("txtGrade5").value);

            if (document.getElementById("txtGrade6").value != "")
                total = total + parseInt(document.getElementById("txtGrade6").value);

            if (document.getElementById("txtGrade7").value != "")
                total = total + parseInt(document.getElementById("txtGrade7").value);

            if (document.getElementById("txtGrade8").value != "")
                total = total + parseInt(document.getElementById("txtGrade8").value);

            if (document.getElementById("txtGrade9").value != "")
                total = total + parseInt(document.getElementById("txtGrade9").value);

            if (document.getElementById("txtGrade10").value != "")
                total = total + parseInt(document.getElementById("txtGrade10").value);

            if (document.getElementById("txtGrade11").value != "")
                total = total + parseInt(document.getElementById("txtGrade11").value);

            if (document.getElementById("txtGrade12").value != "")
                total = total + parseInt(document.getElementById("txtGrade12").value);

            if (document.getElementById("txtGrade13").value != "")
                total = total + parseInt(document.getElementById("txtGrade13").value);

            if (document.getElementById("txtGrade14").value != "")
                total = total + parseInt(document.getElementById("txtGrade14").value);

            document.getElementById("txtGradeTotal").value = total;
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Produto Acabado&nbsp;&nbsp;>&nbsp;&nbsp;Acessórios&nbsp;&nbsp;>&nbsp;&nbsp;Pedido&nbsp;&nbsp;>&nbsp;&nbsp;Produto - Grade</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Produto - Grade</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>Coleção
                                        <asp:HiddenField ID="hidCodigoCarrinho" runat="server" />
                                    </td>
                                    <td>Grupo Produto
                                    </td>
                                    <td>Produto
                                    </td>
                                    <td>Nome
                                    </td>
                                    <td>Cor
                                    </td>
                                    <td>Griffe
                                    </td>
                                    <td>Qtde
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 155px;">
                                        <asp:TextBox ID="txtColecao" runat="server" Width="145px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 130px;">
                                        <asp:TextBox ID="txtGrupoProduto" runat="server" Width="120px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 120px;">
                                        <asp:TextBox ID="txtProduto" runat="server" Width="110px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 220px;">
                                        <asp:TextBox ID="txtNome" runat="server" Width="210px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 175px;">
                                        <asp:TextBox ID="txtCor" runat="server" Width="165px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 240px;">
                                        <asp:TextBox ID="txtGriffe" runat="server" Width="230px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtQtde" runat="server" Width="103px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <asp:Label ID="labCusto" runat="server" Text="Custo"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <asp:TextBox ID="txtCusto" runat="server" Width="145px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <fieldset>
                                            <legend>
                                                <asp:Label ID="labGradeTitulo" runat="server" Text="Grade"></asp:Label></legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="text-align: center">
                                                        <asp:Label ID="labGrade1" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <asp:Label ID="labGrade2" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <asp:Label ID="labGrade3" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <asp:Label ID="labGrade4" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <asp:Label ID="labGrade5" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <asp:Label ID="labGrade6" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <asp:Label ID="labGrade7" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <asp:Label ID="labGrade8" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <asp:Label ID="labGrade9" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <asp:Label ID="labGrade10" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <asp:Label ID="labGrade11" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <asp:Label ID="labGrade12" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <asp:Label ID="labGrade13" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">
                                                        <asp:Label ID="labGrade14" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center">Total
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGrade1" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="60px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGrade2" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="60px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGrade3" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="60px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGrade4" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="60px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGrade5" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="60px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGrade6" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="60px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGrade7" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="60px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGrade8" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="60px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGrade9" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="60px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGrade10" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="60px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGrade11" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="60px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGrade12" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="60px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGrade13" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="60px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGrade14" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="60px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 117px;">
                                                        <asp:TextBox ID="txtGradeTotal" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="100px"></asp:TextBox>
                                                    </td>
                                                    <td style="text-align: right;">&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="7" style="text-align: right;">
                                        <asp:Button ID="btSalvar" runat="server" Width="122px" Text="Salvar Grade" OnClientClick="DesabilitarBotao(this);" OnClick="btSalvar_Click" />&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">&nbsp;
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
