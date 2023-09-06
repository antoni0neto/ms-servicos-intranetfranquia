<%@ Page Title="Definir Grade Logística" Language="C#" AutoEventWireup="true" CodeBehind="prod_def_logistica_edit.aspx.cs"
    Inherits="Relatorios.prod_def_logistica_edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Definir Grade Logística</title>
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Logística&nbsp;&nbsp;>&nbsp;&nbsp;Definição Grade Logística&nbsp;&nbsp;>&nbsp;&nbsp;Definir Grade Logística</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Definir Grade Logística</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 180px;">Coleção:
                                        <asp:HiddenField ID="hidCodigoHB" runat="server" />
                                        <asp:HiddenField ID="hidColecao" runat="server" />
                                        <asp:HiddenField ID="hidProduto" runat="server" />
                                        <asp:HiddenField ID="hidCor" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="labColecao" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>HB:
                                    </td>
                                    <td>
                                        <asp:Label ID="labHB" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Produto:
                                    </td>
                                    <td>
                                        <asp:Label ID="labProduto" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Nome:
                                    </td>
                                    <td>
                                        <asp:Label ID="labNome" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Cor:
                                    </td>
                                    <td>
                                        <asp:Label ID="labCor" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <fieldset>
                                            <legend>Grade Venda Atacado</legend>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvGradeVendaAtacado" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                    Style="background: white" OnRowDataBound="gvGradeVendaAtacado_RowDataBound">
                                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                                    <RowStyle HorizontalAlign="Left" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Grade" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litGrade" runat="server" Text="VENDA ATACADO"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="VE9" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="EXP" HeaderStyle-Width="105px" />
                                                        <asp:BoundField DataField="VE1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="XP" HeaderStyle-Width="105px" />
                                                        <asp:BoundField DataField="VE2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="PP" HeaderStyle-Width="105px" />
                                                        <asp:BoundField DataField="VE3" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="P" HeaderStyle-Width="105px" />
                                                        <asp:BoundField DataField="VE4" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="M" HeaderStyle-Width="105px" />
                                                        <asp:BoundField DataField="VE5" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="G" HeaderStyle-Width="105px" />
                                                        <asp:BoundField DataField="VE6" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="GG" HeaderStyle-Width="105px" />
                                                        <asp:TemplateField HeaderText="Total" HeaderStyle-Width="130px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litTotal" runat="server" Text='<%#Bind("QTDE_ATACADO_VE") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <fieldset style="margin-top: -5px;">
                                            <legend>Grade Produção Atacado/Varejo</legend>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvGradeProducao" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                    Style="background: white" OnRowDataBound="gvGradeProducao_RowDataBound">
                                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                                    <RowStyle HorizontalAlign="Left" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Grade" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litGrade" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="GRADE_EXP" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="EXP" HeaderStyle-Width="105px" />
                                                        <asp:BoundField DataField="GRADE_XP" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="XP" HeaderStyle-Width="105px" />
                                                        <asp:BoundField DataField="GRADE_PP" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="PP" HeaderStyle-Width="105px" />
                                                        <asp:BoundField DataField="GRADE_P" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="P" HeaderStyle-Width="105px" />
                                                        <asp:BoundField DataField="GRADE_M" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="M" HeaderStyle-Width="105px" />
                                                        <asp:BoundField DataField="GRADE_G" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="G" HeaderStyle-Width="105px" />
                                                        <asp:BoundField DataField="GRADE_GG" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="GG" HeaderStyle-Width="105px" />
                                                        <asp:TemplateField HeaderText="Total" HeaderStyle-Width="130px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <fieldset style="margin-top: -5px;">
                                            <legend>
                                                <asp:Label ID="labTituloGradeNova" runat="server" Text="Nova Grade Atacado"></asp:Label>
                                            </legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td colspan="10" style="text-align: right;">
                                                        <asp:Button ID="btCopiar" runat="server" Text="Copiar" Width="80px" OnClick="btCopiar_Click" />&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="10">&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left">&nbsp;
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                    <td style="text-align: center">EXP
                                                    </td>
                                                    <td style="text-align: center">XP
                                                    </td>
                                                    <td style="text-align: center">PP
                                                    </td>
                                                    <td style="text-align: center">P
                                                    </td>
                                                    <td style="text-align: center">M
                                                    </td>
                                                    <td style="text-align: center">G
                                                    </td>
                                                    <td style="text-align: center">GG
                                                    </td>
                                                    <td style="text-align: center">Total
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 80px;" valign="middle">Grade
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                    <td style="width: 116px;">
                                                        <asp:TextBox ID="txtGradeEXP_R" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="106px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 115px;">
                                                        <asp:TextBox ID="txtGradeXP_R" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="105px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 115px;">
                                                        <asp:TextBox ID="txtGradePP_R" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="105px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 116px;">
                                                        <asp:TextBox ID="txtGradeP_R" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="106px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 118px;">
                                                        <asp:TextBox ID="txtGradeM_R" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="108px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 117px;">
                                                        <asp:TextBox ID="txtGradeG_R" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="107px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 116px;">
                                                        <asp:TextBox ID="txtGradeGG_R" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="106px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 143px;">
                                                        <asp:TextBox ID="txtGradeTotal_R" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                            onkeypress="return fnValidarNumero(event);" Width="135px" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: right;">
                                        <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>&nbsp;&nbsp;<asp:Button ID="btSalvar" runat="server" Width="135px" Text="Salvar" OnClientClick="DesabilitarBotao(this);" OnClick="btSalvar_Click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
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

