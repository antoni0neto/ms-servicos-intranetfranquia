<%@ Page Title="Cadastro de Grades" Language="C#" AutoEventWireup="true" CodeBehind="pacab_pedido_produto_grade_cadastrov2.aspx.cs"
    Inherits="Relatorios.pacab_pedido_produto_grade_cadastrov2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cadastro de Grades</title>
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Produto Acabado&nbsp;&nbsp;>&nbsp;&nbsp;Produto&nbsp;&nbsp;>&nbsp;&nbsp;Pedido&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Grades</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Cadastro de Grades</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="labGrade" runat="server" Text="Grade"></asp:Label>
                                        <asp:HiddenField ID="hidCodigo" runat="server" Value="0" />
                                    </td>
                                    <td>
                                        <asp:Label ID="labNome" runat="server" Text="Nome"></asp:Label>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px;">
                                        <asp:DropDownList ID="ddlGrade" runat="server" Width="194px" DataValueField="GRADE" DataTextField="GRADE" Height="22px" OnSelectedIndexChanged="ddlGrade_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </td>
                                    <td style="width: 150px;">
                                        <asp:TextBox ID="txtNome" runat="server" Width="120px" MaxLength="10"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="3">
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
                                                        <asp:TextBox ID="txtGradeTotal" runat="server" MaxLength="6" CssClass="alinharCentro" Enabled="false"
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
                                    <td colspan="3">
                                        <asp:Button ID="btSalvar" runat="server" Width="122px" Text="Salvar" OnClientClick="DesabilitarBotao(this);" OnClick="btSalvar_Click" />&nbsp;
                                        <asp:Button ID="btCancelar" runat="server" Width="122px" Text="Cancelar" OnClick="btCancelar_Click" Visible="false" />&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvGrade" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" ShowFooter="true"
                                                OnRowDataBound="gvGrade_RowDataBound"
                                                DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Size="Smaller"></HeaderStyle>
                                                <RowStyle Font-Size="Smaller" HorizontalAlign="Center" />
                                                <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Grade" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litGrade" runat="server" Text='<%# Bind("GRADE") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Nome" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litNome" runat="server" Text='<%# Bind("NOME") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="GRADE1" HeaderText="Grade 1" FooterStyle-Width="55px" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller" />
                                                    <asp:BoundField DataField="GRADE2" HeaderText="Grade 2" FooterStyle-Width="55px" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller" />
                                                    <asp:BoundField DataField="GRADE3" HeaderText="Grade 3" FooterStyle-Width="55px" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller" />
                                                    <asp:BoundField DataField="GRADE4" HeaderText="Grade 4" FooterStyle-Width="55px" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller" />
                                                    <asp:BoundField DataField="GRADE5" HeaderText="Grade 5" FooterStyle-Width="55px" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller" />
                                                    <asp:BoundField DataField="GRADE6" HeaderText="Grade 6" FooterStyle-Width="55px" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller" />
                                                    <asp:BoundField DataField="GRADE7" HeaderText="Grade 7" FooterStyle-Width="55px" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller" />
                                                    <asp:BoundField DataField="GRADE8" HeaderText="Grade 8" FooterStyle-Width="55px" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller" />
                                                    <asp:BoundField DataField="GRADE9" HeaderText="Grade 9" FooterStyle-Width="55px" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller" />
                                                    <asp:BoundField DataField="GRADE10" HeaderText="Grade 10" FooterStyle-Width="55px" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller" />
                                                    <asp:BoundField DataField="GRADE11" HeaderText="Grade 11" FooterStyle-Width="55px" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller" />
                                                    <asp:BoundField DataField="GRADE12" HeaderText="Grade 12" FooterStyle-Width="55px" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller" />
                                                    <asp:BoundField DataField="GRADE13" HeaderText="Grade 13" FooterStyle-Width="55px" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller" />
                                                    <asp:BoundField DataField="GRADE14" HeaderText="Grade 14" FooterStyle-Width="55px" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller" />

                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="65px">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btAlterarGrade" runat="server" Text="Alterar" Width="65px" Height="21px" OnClick="btAlterarGrade_Click" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="65px">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btExcluirGrade" runat="server" Text="Excluir" Width="65px" Height="21px" OnClick="btExcluirGrade_Click" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">&nbsp;</td>
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
