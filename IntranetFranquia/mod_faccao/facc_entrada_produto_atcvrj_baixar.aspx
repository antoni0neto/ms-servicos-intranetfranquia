<%@ Page Title="Baixar Entrada de Produto Atacado/Varejo" Language="C#" AutoEventWireup="true" CodeBehind="facc_entrada_produto_atcvrj_baixar.aspx.cs"
    Inherits="Relatorios.facc_entrada_produto_atcvrj_baixar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Entrada de Produto</title>
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
        function SomarGradeAtacadoFaltante(control) {
            var total = 0;

            if (document.getElementById("txtGradeEXP_AF").value != "")
                total = total + parseInt(document.getElementById("txtGradeEXP_AF").value);

            if (document.getElementById("txtGradeXP_AF").value != "")
                total = total + parseInt(document.getElementById("txtGradeXP_AF").value);

            if (document.getElementById("txtGradePP_AF").value != "")
                total = total + parseInt(document.getElementById("txtGradePP_AF").value);

            if (document.getElementById("txtGradeP_AF").value != "")
                total = total + parseInt(document.getElementById("txtGradeP_AF").value);

            if (document.getElementById("txtGradeM_AF").value != "")
                total = total + parseInt(document.getElementById("txtGradeM_AF").value);

            if (document.getElementById("txtGradeG_AF").value != "")
                total = total + parseInt(document.getElementById("txtGradeG_AF").value);

            if (document.getElementById("txtGradeGG_AF").value != "")
                total = total + parseInt(document.getElementById("txtGradeGG_AF").value);

            document.getElementById("txtGradeTotal_AF").value = total;
        }
        function SomarGradeAtacado(control) {
            var total = 0;

            if (document.getElementById("txtGradeEXP_A").value != "")
                total = total + parseInt(document.getElementById("txtGradeEXP_A").value);

            if (document.getElementById("txtGradeXP_A").value != "")
                total = total + parseInt(document.getElementById("txtGradeXP_A").value);

            if (document.getElementById("txtGradePP_A").value != "")
                total = total + parseInt(document.getElementById("txtGradePP_A").value);

            if (document.getElementById("txtGradeP_A").value != "")
                total = total + parseInt(document.getElementById("txtGradeP_A").value);

            if (document.getElementById("txtGradeM_A").value != "")
                total = total + parseInt(document.getElementById("txtGradeM_A").value);

            if (document.getElementById("txtGradeG_A").value != "")
                total = total + parseInt(document.getElementById("txtGradeG_A").value);

            if (document.getElementById("txtGradeGG_A").value != "")
                total = total + parseInt(document.getElementById("txtGradeGG_A").value);

            document.getElementById("txtGradeTotal_A").value = total;

            var atacadoA = 0;
            var gradeA = 0;
            var resultado = 0;

            var nomeCampo = "";
            nomeCampo = control.id;

            if (document.getElementById(nomeCampo).value != "" && !document.getElementById(nomeCampo.replace("_A", "_V")).disabled) {
                atacadoA = parseInt(document.getElementById(nomeCampo).value);
                gradeA = parseInt(document.getElementById(nomeCampo.replace("_A", "_E")).value);
                resultado = (gradeA - atacadoA);
                document.getElementById(nomeCampo.replace("_A", "_V")).value = resultado;
            } else {
                document.getElementById(nomeCampo.replace("_A", "_V")).value = "0";
            }

            SomarGradeVarejo(null);

        }
        function SomarGradeVarejoFaltante(control) {
            var total = 0;

            if (document.getElementById("txtGradeEXP_VF").value != "")
                total = total + parseInt(document.getElementById("txtGradeEXP_VF").value);

            if (document.getElementById("txtGradeXP_VF").value != "")
                total = total + parseInt(document.getElementById("txtGradeXP_VF").value);

            if (document.getElementById("txtGradePP_VF").value != "")
                total = total + parseInt(document.getElementById("txtGradePP_VF").value);

            if (document.getElementById("txtGradeP_VF").value != "")
                total = total + parseInt(document.getElementById("txtGradeP_VF").value);

            if (document.getElementById("txtGradeM_VF").value != "")
                total = total + parseInt(document.getElementById("txtGradeM_VF").value);

            if (document.getElementById("txtGradeG_VF").value != "")
                total = total + parseInt(document.getElementById("txtGradeG_VF").value);

            if (document.getElementById("txtGradeGG_VF").value != "")
                total = total + parseInt(document.getElementById("txtGradeGG_VF").value);

            document.getElementById("txtGradeTotal_VF").value = total;
        }
        function SomarGradeVarejo(control) {
            var total = 0;

            if (document.getElementById("txtGradeEXP_V").value != "")
                total = total + parseInt(document.getElementById("txtGradeEXP_V").value);

            if (document.getElementById("txtGradeXP_V").value != "")
                total = total + parseInt(document.getElementById("txtGradeXP_V").value);

            if (document.getElementById("txtGradePP_V").value != "")
                total = total + parseInt(document.getElementById("txtGradePP_V").value);

            if (document.getElementById("txtGradeP_V").value != "")
                total = total + parseInt(document.getElementById("txtGradeP_V").value);

            if (document.getElementById("txtGradeM_V").value != "")
                total = total + parseInt(document.getElementById("txtGradeM_V").value);

            if (document.getElementById("txtGradeG_V").value != "")
                total = total + parseInt(document.getElementById("txtGradeG_V").value);

            if (document.getElementById("txtGradeGG_V").value != "")
                total = total + parseInt(document.getElementById("txtGradeGG_V").value);

            document.getElementById("txtGradeTotal_V").value = total;
        }

        function SomarGradeSegQualidade(control) {
            var total = 0;

            if (document.getElementById("txtGradeEXPSeg").value != "")
                total = total + parseInt(document.getElementById("txtGradeEXPSeg").value);

            if (document.getElementById("txtGradeXPSeg").value != "")
                total = total + parseInt(document.getElementById("txtGradeXPSeg").value);

            if (document.getElementById("txtGradePPSeg").value != "")
                total = total + parseInt(document.getElementById("txtGradePPSeg").value);

            if (document.getElementById("txtGradePSeg").value != "")
                total = total + parseInt(document.getElementById("txtGradePSeg").value);

            if (document.getElementById("txtGradeMSeg").value != "")
                total = total + parseInt(document.getElementById("txtGradeMSeg").value);

            if (document.getElementById("txtGradeGSeg").value != "")
                total = total + parseInt(document.getElementById("txtGradeGSeg").value);

            if (document.getElementById("txtGradeGGSeg").value != "")
                total = total + parseInt(document.getElementById("txtGradeGGSeg").value);

            document.getElementById("txtGradeTotalSeg").value = total;
        }

        function SomarGradeLavanderia(control) {
            var total = 0;

            if (document.getElementById("txtGradeEXPLav").value != "")
                total = total + parseInt(document.getElementById("txtGradeEXPLav").value);

            if (document.getElementById("txtGradeXPLav").value != "")
                total = total + parseInt(document.getElementById("txtGradeXPLav").value);

            if (document.getElementById("txtGradePPLav").value != "")
                total = total + parseInt(document.getElementById("txtGradePPLav").value);

            if (document.getElementById("txtGradePLav").value != "")
                total = total + parseInt(document.getElementById("txtGradePLav").value);

            if (document.getElementById("txtGradeMLav").value != "")
                total = total + parseInt(document.getElementById("txtGradeMLav").value);

            if (document.getElementById("txtGradeGLav").value != "")
                total = total + parseInt(document.getElementById("txtGradeGLav").value);

            if (document.getElementById("txtGradeGGLav").value != "")
                total = total + parseInt(document.getElementById("txtGradeGGLav").value);

            document.getElementById("txtGradeTotalLav").value = total;
        }

        function SomarGradeConserto(control) {
            var total = 0;

            if (document.getElementById("txtGradeEXPCons").value != "")
                total = total + parseInt(document.getElementById("txtGradeEXPCons").value);

            if (document.getElementById("txtGradeXPCons").value != "")
                total = total + parseInt(document.getElementById("txtGradeXPCons").value);

            if (document.getElementById("txtGradePPCons").value != "")
                total = total + parseInt(document.getElementById("txtGradePPCons").value);

            if (document.getElementById("txtGradePCons").value != "")
                total = total + parseInt(document.getElementById("txtGradePCons").value);

            if (document.getElementById("txtGradeMCons").value != "")
                total = total + parseInt(document.getElementById("txtGradeMCons").value);

            if (document.getElementById("txtGradeGCons").value != "")
                total = total + parseInt(document.getElementById("txtGradeGCons").value);

            if (document.getElementById("txtGradeGGCons").value != "")
                total = total + parseInt(document.getElementById("txtGradeGGCons").value);

            document.getElementById("txtGradeTotalCons").value = total;
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Facção&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Facção&nbsp;&nbsp;>&nbsp;&nbsp;Entrada Produto Acabado&nbsp;&nbsp;>&nbsp;&nbsp;Baixar Entrada de Produto Atacado/Varejo</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Baixar Entrada de Produto Atacado/Varejo</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>Coleção
                                        <asp:HiddenField ID="hidCodigoEntrada" runat="server" />
                                        <asp:HiddenField ID="hidTela" runat="server" />
                                        <asp:HiddenField ID="hidControleInsercao" runat="server" Value="" />
                                    </td>
                                    <td>HB
                                    </td>
                                    <td>Produto
                                    </td>
                                    <td>Nome
                                    </td>
                                    <td>Cor
                                    </td>
                                    <td>Quantidade
                                    </td>
                                    <td>Mostruário
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 185px;">
                                        <asp:TextBox ID="txtColecao" runat="server" Width="175px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 110px;">
                                        <asp:TextBox ID="txtHB" runat="server" Width="100px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 120px;">
                                        <asp:TextBox ID="txtProduto" runat="server" Width="110px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 260px;">
                                        <asp:TextBox ID="txtNome" runat="server" Width="250px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 200px;">
                                        <asp:TextBox ID="txtCor" runat="server" Width="190px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 130px;">
                                        <asp:TextBox ID="txtQtde" runat="server" Width="119px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMostruario" runat="server" Width="119px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">Fornecedor</td>
                                    <td>Setor</td>
                                    <td>Fase</td>
                                    <td>Preço Custo</td>
                                    <td>Preço Produção</td>
                                    <td>Volume</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlFornecedor" runat="server" Width="289px" Height="21px" DataTextField="FORNECEDOR"
                                            DataValueField="FORNECEDOR" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTipo" runat="server" Width="114px" Height="21px" Enabled="false">
                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="I" Text="INTERNO"></asp:ListItem>
                                            <asp:ListItem Value="E" Text="EXTERNO"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlServico" runat="server" Width="253px" Height="21px" DataTextField="DESCRICAO"
                                            DataValueField="CODIGO" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrecoCusto" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                            Width="189px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrecoProducao" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                            Width="119px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVolume" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                            Width="118px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">Sub Fornecedor</td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <asp:TextBox ID="txtFornecedorSub" runat="server" Width="284px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">&nbsp;</td>
                                </tr>

                                <tr>
                                    <td colspan="2">
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
                                    <td colspan="2">
                                        <asp:DropDownList runat="server" ID="ddlFilial" Height="22px" Width="289px" Enabled="false">
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
                                            Width="249px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSerie" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                            Width="189px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmissao" runat="server" MaxLength="10" Width="119px"
                                            onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRecebimento" runat="server" MaxLength="10" Width="118px"
                                            onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <asp:Label ID="labProdutoAcabado" runat="server" Text="Produto Acabado"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <asp:DropDownList ID="ddlProdutoAcabado" runat="server" Width="289px" Height="22px">
                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                            <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                            <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="7">
                                        <fieldset>
                                            <legend>Grade</legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="text-align: left">&nbsp;
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labGradeEXP" runat="server" Text="-"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labGradeXP" runat="server" Text="-"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labGradePP" runat="server" Text="-"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labGradeP" runat="server" Text="-"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labGradeM" runat="server" Text="-"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labGradeG" runat="server" Text="-"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labGradeGG" runat="server" Text="-"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labGradeTotal" runat="server" Text="Total"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 150px;" valign="middle">Grade a Receber
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
                                                    <td colspan="9">&nbsp;<hr />
                                                    </td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 150px;" valign="middle">Atacado Faltante
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeEXP_AF" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeAtacadoFaltante(this);" onkeyup="SomarGradeAtacadoFaltante(this);" Width="70px" Enabled="false" Text="0"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeXP_AF" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeAtacadoFaltante(this);" onkeyup="SomarGradeAtacadoFaltante(this);" Width="70px" Enabled="false" Text="0"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradePP_AF" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeAtacadoFaltante(this);" onkeyup="SomarGradeAtacadoFaltante(this);" Width="70px" Enabled="false" Text="0"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeP_AF" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeAtacadoFaltante(this);" onkeyup="SomarGradeAtacadoFaltante(this);" Width="70px" Enabled="false" Text="0"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeM_AF" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeAtacadoFaltante(this);" onkeyup="SomarGradeAtacadoFaltante(this);" Width="70px" Enabled="false" Text="0"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeG_AF" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeAtacadoFaltante(this);" onkeyup="SomarGradeAtacadoFaltante(this);" Width="70px" Enabled="false" Text="0"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeGG_AF" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeAtacadoFaltante(this);" onkeyup="SomarGradeAtacadoFaltante(this);" Width="70px" Enabled="false" Text="0"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 117px;">
                                                        <asp:TextBox ID="txtGradeTotal_AF" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="107px" Enabled="false" Text="0"></asp:TextBox>
                                                    </td>
                                                    <td style="text-align: right;">&nbsp;
                                                    </td>
                                                </tr>
                                                <tr style="line-height: 9px;">
                                                    <td colspan="10">&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 150px;" valign="middle">Atacado
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeEXP_A" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeAtacado(this);" onkeyup="SomarGradeAtacado(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeXP_A" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeAtacado(this);" onkeyup="SomarGradeAtacado(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradePP_A" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeAtacado(this);" onkeyup="SomarGradeAtacado(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeP_A" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeAtacado(this);" onkeyup="SomarGradeAtacado(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeM_A" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeAtacado(this);" onkeyup="SomarGradeAtacado(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeG_A" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeAtacado(this);" onkeyup="SomarGradeAtacado(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeGG_A" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeAtacado(this);" onkeyup="SomarGradeAtacado(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 117px;">
                                                        <asp:TextBox ID="txtGradeTotal_A" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="107px" Text="0"></asp:TextBox>
                                                    </td>
                                                    <td style="text-align: right;">&nbsp;
                                                    </td>
                                                </tr>
                                                <tr style="line-height: 9px;">
                                                    <td colspan="9">&nbsp;<hr />
                                                    </td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 150px;" valign="middle">Varejo Faltante
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeEXP_VF" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeVarejoFaltante(this);" onkeyup="SomarGradeVarejoFaltante(this);" Width="70px" Enabled="false" Text="0"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeXP_VF" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeVarejoFaltante(this);" onkeyup="SomarGradeVarejoFaltante(this);" Width="70px" Enabled="false" Text="0"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradePP_VF" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeVarejoFaltante(this);" onkeyup="SomarGradeVarejoFaltante(this);" Width="70px" Enabled="false" Text="0"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeP_VF" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeVarejoFaltante(this);" onkeyup="SomarGradeVarejoFaltante(this);" Width="70px" Enabled="false" Text="0"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeM_VF" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeVarejoFaltante(this);" onkeyup="SomarGradeVarejoFaltante(this);" Width="70px" Enabled="false" Text="0"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeG_VF" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeVarejoFaltante(this);" onkeyup="SomarGradeVarejoFaltante(this);" Width="70px" Enabled="false" Text="0"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeGG_VF" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeVarejoFaltante(this);" onkeyup="SomarGradeVarejoFaltante(this);" Width="70px" Enabled="false" Text="0"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 117px;">
                                                        <asp:TextBox ID="txtGradeTotal_VF" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="107px" Enabled="false" Text="0"></asp:TextBox>
                                                    </td>
                                                    <td style="text-align: right;">&nbsp;
                                                    </td>
                                                </tr>
                                                <tr style="line-height: 9px;">
                                                    <td colspan="10">&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 150px;" valign="middle">Varejo
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeEXP_V" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeVarejo(this);" onkeyup="SomarGradeVarejo(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeXP_V" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeVarejo(this);" onkeyup="SomarGradeVarejo(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradePP_V" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeVarejo(this);" onkeyup="SomarGradeVarejo(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeP_V" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeVarejo(this);" onkeyup="SomarGradeVarejo(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeM_V" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeVarejo(this);" onkeyup="SomarGradeVarejo(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeG_V" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeVarejo(this);" onkeyup="SomarGradeVarejo(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeGG_V" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGradeVarejo(this);" onkeyup="SomarGradeVarejo(this);" Width="70px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 117px;">
                                                        <asp:TextBox ID="txtGradeTotal_V" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="107px" Text="0"></asp:TextBox>
                                                    </td>
                                                    <td style="text-align: right;">&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:Panel ID="pnlOutro" runat="server">
                                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                    <tr style="line-height: 9px;">
                                                        <td colspan="9">&nbsp;<hr />
                                                        </td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 150px;" valign="middle">Rev. Seg. Qualidade
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:TextBox ID="txtGradeEXPSeg" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGradeSegQualidade(this);" onkeyup="SomarGradeSegQualidade(this);" Width="70px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:TextBox ID="txtGradeXPSeg" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGradeSegQualidade(this);" onkeyup="SomarGradeSegQualidade(this);" Width="70px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:TextBox ID="txtGradePPSeg" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGradeSegQualidade(this);" onkeyup="SomarGradeSegQualidade(this);" Width="70px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:TextBox ID="txtGradePSeg" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGradeSegQualidade(this);" onkeyup="SomarGradeSegQualidade(this);" Width="70px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:TextBox ID="txtGradeMSeg" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGradeSegQualidade(this);" onkeyup="SomarGradeSegQualidade(this);" Width="70px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:TextBox ID="txtGradeGSeg" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGradeSegQualidade(this);" onkeyup="SomarGradeSegQualidade(this);" Width="70px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:TextBox ID="txtGradeGGSeg" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGradeSegQualidade(this);" onkeyup="SomarGradeSegQualidade(this);" Width="70px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 117px;">
                                                            <asp:TextBox ID="txtGradeTotalSeg" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                                onkeypress="return fnValidarNumero(event);" Width="107px" Enabled="false" Text="0"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: right;">&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr style="line-height: 9px;">
                                                        <td colspan="10">&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 150px;" valign="middle">Lavanderia
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:TextBox ID="txtGradeEXPLav" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGradeLavanderia(this);" onkeyup="SomarGradeLavanderia(this);" Width="70px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:TextBox ID="txtGradeXPLav" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGradeLavanderia(this);" onkeyup="SomarGradeLavanderia(this);" Width="70px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:TextBox ID="txtGradePPLav" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGradeLavanderia(this);" onkeyup="SomarGradeLavanderia(this);" Width="70px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:TextBox ID="txtGradePLav" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGradeLavanderia(this);" onkeyup="SomarGradeLavanderia(this);" Width="70px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:TextBox ID="txtGradeMLav" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGradeLavanderia(this);" onkeyup="SomarGradeLavanderia(this);" Width="70px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:TextBox ID="txtGradeGLav" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGradeLavanderia(this);" onkeyup="SomarGradeLavanderia(this);" Width="70px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:TextBox ID="txtGradeGGLav" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGradeLavanderia(this);" onkeyup="SomarGradeLavanderia(this);" Width="70px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 117px;">
                                                            <asp:TextBox ID="txtGradeTotalLav" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                                onkeypress="return fnValidarNumero(event);" Width="107px" Enabled="false" Text="0"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: right;">&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr style="line-height: 9px;">
                                                        <td colspan="10">&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 150px;" valign="middle">Conserto
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:TextBox ID="txtGradeEXPCons" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGradeConserto(this);" onkeyup="SomarGradeConserto(this);" Width="70px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:TextBox ID="txtGradeXPCons" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGradeConserto(this);" onkeyup="SomarGradeConserto(this);" Width="70px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:TextBox ID="txtGradePPCons" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGradeConserto(this);" onkeyup="SomarGradeConserto(this);" Width="70px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:TextBox ID="txtGradePCons" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGradeConserto(this);" onkeyup="SomarGradeConserto(this);" Width="70px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:TextBox ID="txtGradeMCons" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGradeConserto(this);" onkeyup="SomarGradeConserto(this);" Width="70px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:TextBox ID="txtGradeGCons" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGradeConserto(this);" onkeyup="SomarGradeConserto(this);" Width="70px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:TextBox ID="txtGradeGGCons" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGradeConserto(this);" onkeyup="SomarGradeConserto(this);" Width="70px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 117px;">
                                                            <asp:TextBox ID="txtGradeTotalCons" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                                onkeypress="return fnValidarNumero(event);" Width="107px" Enabled="false" Text="0"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: right;">&nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="labStatusAnterior" runat="server" Text="Status Anterior"></asp:Label>
                                        <asp:HiddenField ID="hidStatusAnterior" runat="server" Value="" />
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="labStatus" runat="server" Text="Status"></asp:Label>
                                    </td>
                                    <td colspan="3">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtStatusAnterior" runat="server" Width="284px" Text="" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="300px" Height="22px" DataTextField="Text" DataValueField="Value" Enabled="false">
                                            <asp:ListItem Value="B" Text="FINALIZADO"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="3" style="text-align: right;">
                                        <asp:Button ID="btSalvar" runat="server" Width="122px" Text="Baixar" OnClientClick="DesabilitarBotao(this);" OnClick="btSalvar_Click" />&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
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
