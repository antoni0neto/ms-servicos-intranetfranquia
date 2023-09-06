<%@ Page Title="Baixar Encaixe" Language="C#" AutoEventWireup="true" CodeBehind="facc_encaixe_baixar.aspx.cs"
    Inherits="Relatorios.facc_encaixe_baixar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Baixar Encaixe</title>
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
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Facção&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Facção&nbsp;&nbsp;>&nbsp;&nbsp;Encaixe&nbsp;&nbsp;>&nbsp;&nbsp;Baixar Encaixe</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Baixar Encaixe</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>Coleção
                                        <asp:HiddenField ID="hidCodigoSaida" runat="server" />
                                        <asp:HiddenField ID="hidTela" runat="server" />
                                        <asp:HiddenField ID="hidProdHB" runat="server" />
                                        <asp:HiddenField ID="hidGradeTotal" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidColecao" runat="server" Value="0" />
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
                                    <td style="width: 120px;">
                                        <asp:TextBox ID="txtHB" runat="server" Width="110px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
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
                                        <asp:TextBox ID="txtMostruario" runat="server" Width="115px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="labFornecedor" runat="server" Text="Fornecedor"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labTipo" runat="server" Text="Setor"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labServico" runat="server" Text="Fase"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labPrecoCusto" runat="server" Text="Preço Custo"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labPrecoProducao" runat="server" Text="Preço Produção"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labVolume" runat="server" Text="Volume"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlFornecedor" runat="server" Width="300px" Height="21px" DataTextField="FORNECEDOR"
                                            DataValueField="FORNECEDOR" OnSelectedIndexChanged="ddlFornecedor_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTipo" runat="server" Width="114px" Height="21px">
                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="I" Text="INTERNO"></asp:ListItem>
                                            <asp:ListItem Value="E" Text="EXTERNO"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlServico" runat="server" Width="261px" Height="21px" DataTextField="DESCRICAO"
                                            DataValueField="CODIGO" OnSelectedIndexChanged="ddlServico_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrecoCusto" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                            Width="190px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrecoProducao" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                            Width="120px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVolume" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                            Width="115px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Label ID="labFornecedorSub" runat="server" Text="Sub Fornecedor"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labPrevEntrega" runat="server" Text="Previsão de Entrega"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:DropDownList ID="ddlFornecedorSub" runat="server" Width="300px" Height="21px" DataTextField="FORNECEDOR_SUB"
                                            DataValueField="FORNECEDOR_SUB" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPrevEntrega" runat="server" MaxLength="10" Width="115px"
                                            onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <fieldset>
                                            <legend>
                                                <asp:Label ID="labGradeTitulo" runat="server" Text="Grade"></asp:Label></legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="text-align: left">&nbsp;
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labGradeEXP" runat="server" Text="EXP"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labGradeXP" runat="server" Text="XP"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labGradePP" runat="server" Text="PP"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labGradeP" runat="server" Text="P"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labGradeM" runat="server" Text="M"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labGradeG" runat="server" Text="G"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labGradeGG" runat="server" Text="GG"></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:Label ID="labGradeTotal" runat="server" Text="Total"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 80px;" valign="middle">Original
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeEXP_O" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeXP_O" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradePP_O" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeP_O" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeM_O" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeG_O" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 80px;">
                                                        <asp:TextBox ID="txtGradeGG_O" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 117px;">
                                                        <asp:TextBox ID="txtGradeTotal_O" runat="server" MaxLength="6" CssClass="alinharCentro"
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
                                                    <td style="width: 80px;" valign="middle">Enviada
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
                                    <td colspan="7">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="7" style="text-align: right;">
                                        <asp:Button ID="btSalvar" runat="server" Width="122px" Text="Baixar" OnClientClick="DesabilitarBotao(this);" OnClick="btSalvar_Click" />&nbsp;
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
                                <tr>
                                    <td colspan="7">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvEncaixeHistorico" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                Style="background: white" OnRowDataBound="gvEncaixeHistorico_RowDataBound">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                                <RowStyle HorizontalAlign="Left" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Fornecedor">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litFornecedor" runat="server" Text='<%# Bind("FORNECEDOR") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Nota Fiscal" HeaderStyle-Width="110px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litNF" runat="server" Text='<%# Bind("NF_SAIDA") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Série" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litSerie" runat="server" Text='<%# Bind("SERIE_NF") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Emissão" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litEmissao" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Fase" HeaderStyle-Width="135px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litServico" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Setor" HeaderStyle-Width="120px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litRecurso" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Custo" HeaderStyle-Width="100px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litCusto" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Preço" HeaderStyle-Width="100px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litPreco" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Grade" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litGrade" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
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
