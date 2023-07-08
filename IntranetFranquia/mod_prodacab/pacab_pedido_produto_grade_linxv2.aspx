<%@ Page Title="Produto - Grade" Language="C#" AutoEventWireup="true" CodeBehind="pacab_pedido_produto_grade_linxv2.aspx.cs"
    Inherits="Relatorios.pacab_pedido_produto_grade_linxv2" %>

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

            document.getElementById("txtGradeTotal").value = total;


            var gradeOriginal = 0;
            var gradeReal = 0;
            var resultado = 0;

            var nomeCampo = "";
            nomeCampo = control.id;

            if (document.getElementById(nomeCampo).value != "" && !document.getElementById(nomeCampo.replace("txtGrade", "txtGradeNova")).disabled) {
                gradeOriginal = parseInt(document.getElementById(nomeCampo.replace("txtGrade", "txtGradeO")).value);
                gradeReal = parseInt(document.getElementById(nomeCampo).value);
                resultado = (gradeOriginal - gradeReal);

                if (resultado < 0)
                    resultado = 0;

                document.getElementById(nomeCampo.replace("txtGrade", "txtGradeNova")).value = resultado;
            } else {
                document.getElementById(nomeCampo.replace("txtGrade", "txtGradeNova")).value = "0";
            }

            SomarGradeNova(null);


        }

        function SomarGradeNova(control) {
            var total = 0;

            if (document.getElementById("txtGradeNova1").value != "")
                total = total + parseInt(document.getElementById("txtGradeNova1").value);

            if (document.getElementById("txtGradeNova2").value != "")
                total = total + parseInt(document.getElementById("txtGradeNova2").value);

            if (document.getElementById("txtGradeNova3").value != "")
                total = total + parseInt(document.getElementById("txtGradeNova3").value);

            if (document.getElementById("txtGradeNova4").value != "")
                total = total + parseInt(document.getElementById("txtGradeNova4").value);

            if (document.getElementById("txtGradeNova5").value != "")
                total = total + parseInt(document.getElementById("txtGradeNova5").value);

            if (document.getElementById("txtGradeNova6").value != "")
                total = total + parseInt(document.getElementById("txtGradeNova6").value);

            if (document.getElementById("txtGradeNova7").value != "")
                total = total + parseInt(document.getElementById("txtGradeNova7").value);

            if (document.getElementById("txtGradeNova8").value != "")
                total = total + parseInt(document.getElementById("txtGradeNova8").value);

            if (document.getElementById("txtGradeNova9").value != "")
                total = total + parseInt(document.getElementById("txtGradeNova9").value);

            if (document.getElementById("txtGradeNova10").value != "")
                total = total + parseInt(document.getElementById("txtGradeNova10").value);

            document.getElementById("txtGradeNovaTot").value = total;
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Produto Acabado&nbsp;&nbsp;>&nbsp;&nbsp;Produto&nbsp;&nbsp;>&nbsp;&nbsp;Pedido&nbsp;&nbsp;>&nbsp;&nbsp;Produto - Grade Linx</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Produto - Grade Linx</legend>
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
                                    <td>Qtde Atacado
                                    </td>
                                    <td>Qtde Varejo
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
                                    <td style="width: 200px;">
                                        <asp:TextBox ID="txtNome" runat="server" Width="190px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 150px;">
                                        <asp:TextBox ID="txtCor" runat="server" Width="140px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 150px;">
                                        <asp:TextBox ID="txtGriffe" runat="server" Width="140px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 100px;">
                                        <asp:TextBox ID="txtQtdeAtacado" runat="server" Width="90px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtQtdeVarejo" runat="server" Width="103px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="labCusto" runat="server" Text="Custo"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labCustoNota" runat="server" Text="Custo Nota"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labEntrega" runat="server" Text="Entrega"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:Label ID="labNotaFiscal" runat="server" Text="Nota Fiscal"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtCusto" runat="server" Width="145px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCustoNota" runat="server" Width="120px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEntrega" runat="server" Width="110px"></asp:TextBox>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtNotaFiscal" runat="server" Width="190px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <fieldset>
                                            <legend>
                                                <asp:Label ID="labGradeTitulo" runat="server" Text="Grade"></asp:Label></legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="text-align: center">&nbsp;
                                                    </td>
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
                                                    <td style="text-align: center">Total
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="text-align: left">
                                                        <asp:Label ID="labOriginal" runat="server" Text="Original"></asp:Label>
                                                    </td>
                                                    <td style="width: 85px;">
                                                        <asp:TextBox ID="txtGradeO1" runat="server" MaxLength="6" CssClass="alinharCentro" Enabled="false" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 85px;">
                                                        <asp:TextBox ID="txtGradeO2" runat="server" MaxLength="6" CssClass="alinharCentro" Enabled="false" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 85px;">
                                                        <asp:TextBox ID="txtGradeO3" runat="server" MaxLength="6" CssClass="alinharCentro" Enabled="false" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 85px;">
                                                        <asp:TextBox ID="txtGradeO4" runat="server" MaxLength="6" CssClass="alinharCentro" Enabled="false" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 85px;">
                                                        <asp:TextBox ID="txtGradeO5" runat="server" MaxLength="6" CssClass="alinharCentro" Enabled="false" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 85px;">
                                                        <asp:TextBox ID="txtGradeO6" runat="server" MaxLength="6" CssClass="alinharCentro" Enabled="false" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 85px;">
                                                        <asp:TextBox ID="txtGradeO7" runat="server" MaxLength="6" CssClass="alinharCentro" Enabled="false" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 85px;">
                                                        <asp:TextBox ID="txtGradeO8" runat="server" MaxLength="6" CssClass="alinharCentro" Enabled="false" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 85px;">
                                                        <asp:TextBox ID="txtGradeO9" runat="server" MaxLength="6" CssClass="alinharCentro" Enabled="false" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 85px;">
                                                        <asp:TextBox ID="txtGradeO10" runat="server" MaxLength="6" CssClass="alinharCentro" Enabled="false" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 128px;">
                                                        <asp:TextBox ID="txtGradeOTot" runat="server" MaxLength="6" CssClass="alinharCentro" Enabled="false" Width="118px"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td colspan="12" style="line-height: 15px;">&nbsp;</td>
                                                </tr>

                                                <tr>
                                                    <td style="text-align: left">
                                                        <asp:Label ID="labGradePedido" runat="server" Text="Real"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtGrade1" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtGrade2" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtGrade3" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtGrade4" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtGrade5" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtGrade6" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtGrade7" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtGrade8" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtGrade9" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtGrade10" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtGradeTotal" runat="server" MaxLength="6" CssClass="alinharCentro" Enabled="false" Width="118px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>

                                        </fieldset>
                                        <fieldset>
                                            <legend>Diferença de Grade</legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td colspan="12">Nova Entrega
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="12">
                                                        <asp:TextBox ID="txtEntregaNova" runat="server" Width="110px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="cbGerarOutroPedido" runat="server" Text="Outro Pedido" Checked="false" Enabled="true" />
                                                    </td>
                                                    <td style="width: 85px;">
                                                        <asp:TextBox ID="txtGradeNova1" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeNova(this);" onkeyup="SomarGradeNova(this);" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 85px;">
                                                        <asp:TextBox ID="txtGradeNova2" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeNova(this);" onkeyup="SomarGradeNova(this);" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 85px;">
                                                        <asp:TextBox ID="txtGradeNova3" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeNova(this);" onkeyup="SomarGradeNova(this);" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 85px;">
                                                        <asp:TextBox ID="txtGradeNova4" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeNova(this);" onkeyup="SomarGradeNova(this);" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 85px;">
                                                        <asp:TextBox ID="txtGradeNova5" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeNova(this);" onkeyup="SomarGradeNova(this);" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 85px;">
                                                        <asp:TextBox ID="txtGradeNova6" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeNova(this);" onkeyup="SomarGradeNova(this);" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 85px;">
                                                        <asp:TextBox ID="txtGradeNova7" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeNova(this);" onkeyup="SomarGradeNova(this);" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 85px;">
                                                        <asp:TextBox ID="txtGradeNova8" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeNova(this);" onkeyup="SomarGradeNova(this);" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 85px;">
                                                        <asp:TextBox ID="txtGradeNova9" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeNova(this);" onkeyup="SomarGradeNova(this);" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 85px;">
                                                        <asp:TextBox ID="txtGradeNova10" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeNova(this);" onkeyup="SomarGradeNova(this);" Width="75px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 128px;">
                                                        <asp:TextBox ID="txtGradeNovaTot" runat="server" MaxLength="6" CssClass="alinharCentro" Enabled="false" Width="118px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="8" style="text-align: right;">
                                        <asp:Button ID="btSalvar" runat="server" Width="122px" Text="Salvar" OnClientClick="DesabilitarBotao(this);" OnClick="btSalvar_Click" />&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">&nbsp;
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
