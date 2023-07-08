<%@ Page Title="Gerar Pedido" Language="C#" AutoEventWireup="true" CodeBehind="pacab_pedido_edit.aspx.cs"
    Inherits="Relatorios.pacab_pedido_edit" %>

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

            if (document.getElementById("txtGradeEXP_E").value != "")
                total = total + parseInt(document.getElementById("txtGradeEXP_E").value);

            if (document.getElementById("txtGradeXP_E").value != "")
                total = total + parseInt(document.getElementById("txtGradeXP_E").value);

            if (document.getElementById("txtGradePP_E").value != "")
                total = total + parseInt(document.getElementById("txtGradePP_E").value);

            if (document.getElementById("txtGradeP_E").value != "")
                total = total + parseInt(document.getElementById("txtGradeP_E").value);

            if (document.getElementById("txtGradeM_E").value != "")
                total = total + parseInt(document.getElementById("txtGradeM_E").value);

            if (document.getElementById("txtGradeG_E").value != "")
                total = total + parseInt(document.getElementById("txtGradeG_E").value);

            if (document.getElementById("txtGradeGG_E").value != "")
                total = total + parseInt(document.getElementById("txtGradeGG_E").value);

            document.getElementById("txtGradeTotal_E").value = total;
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Produto Acabado&nbsp;&nbsp;>&nbsp;&nbsp;Peças&nbsp;&nbsp;>&nbsp;&nbsp;Pedido&nbsp;&nbsp;>&nbsp;&nbsp;Gerar Pedido</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Gerar Pedido</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>Coleção
                                        <asp:HiddenField ID="hidCodigoProduto" runat="server" />
                                        <asp:HiddenField ID="hidCor" runat="server" />
                                        <asp:HiddenField ID="hidMostruario" runat="server" />
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
                                    <td>Qtde Varejo
                                    </td>
                                    <td>Qtde Atacado
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
                                    <td style="width: 130px;">
                                        <asp:TextBox ID="txtGriffe" runat="server" Width="120px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 113px;">
                                        <asp:TextBox ID="txtQtdeVarejo" runat="server" Width="103px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtQtdeAtacado" runat="server" Width="100px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">Mostruário
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <asp:TextBox ID="txtMostruario" runat="server" Width="145px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="labFornecedor" runat="server" Text="Fornecedor"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labDataEntrega" runat="server" Text="Limite Entrega"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="labFilialRateio" runat="server" Text="Filial Rateio"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="labCusto" runat="server" Text="Custo Produto"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlFornecedor" runat="server" Width="280px" Height="21px" DataTextField="FORNECEDOR"
                                            DataValueField="FORNECEDOR">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDataEntrega" runat="server" MaxLength="10" Width="110px"
                                            onkeypress="return fnValidarData(ev ent);" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlFilial" runat="server" Width="214px" Height="21px" DataValueField="value" DataTextField="text" OnSelectedIndexChanged="ddlFilial_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlFilialRateio" runat="server" Width="299px" Height="21px" DataValueField="value" DataTextField="text">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtCustoProduto" runat="server" MaxLength="6" CssClass="alinharDireita"
                                            onkeypress="return fnValidarNumeroDecimal(event);" Width="214px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <asp:Label ID="labStatus" runat="server" Text="Status"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="280px" Height="21px">
                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="A" Text="APROVADO" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="E" Text="ESTUDO"></asp:ListItem>
                                            <asp:ListItem Value="R" Text="REPROVADO"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <fieldset>
                                            <legend>
                                                <asp:Label ID="labGradeTitulo" runat="server" Text="Grade"></asp:Label></legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="text-align: left">&nbsp;
                                                    </td>
                                                    <td style="text-align: left">EXP/32
                                                    </td>
                                                    <td style="text-align: left">XP/34
                                                    </td>
                                                    <td style="text-align: left">PP/36
                                                    </td>
                                                    <td style="text-align: left">P/38
                                                    </td>
                                                    <td style="text-align: left">M/40
                                                    </td>
                                                    <td style="text-align: left">G/42
                                                    </td>
                                                    <td style="text-align: left">GG/44
                                                    </td>
                                                    <td style="text-align: left">Total
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 80px;" valign="middle">Compra
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeEXP_E" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeXP_E" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradePP_E" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeP_E" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeM_E" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeG_E" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeGG_E" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 117px;">
                                                        <asp:TextBox ID="txtGradeTotal_E" runat="server" MaxLength="6" CssClass="alinharCentro"
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
                                    <td colspan="8">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="text-align: left;">
                                        <asp:Button ID="btNovo" runat="server" Width="122px" Text="Novo" OnClick="btNovo_Click" />
                                    <td colspan="5" style="text-align: right;">
                                        <asp:Button ID="btSalvar" runat="server" Width="122px" Text="Gerar Pedido" OnClientClick="DesabilitarBotao(this);" OnClick="btSalvar_Click" />&nbsp;
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
