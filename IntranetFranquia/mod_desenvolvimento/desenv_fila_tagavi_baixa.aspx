<%@ Page Title="Baixar Etiqueta" Language="C#" AutoEventWireup="true" CodeBehind="desenv_fila_tagavi_baixa.aspx.cs"
    Inherits="Relatorios.desenv_fila_tagavi_baixa" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Baixar Etiqueta</title>
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Liberação Produto Acabado&nbsp;&nbsp;>&nbsp;&nbsp;Baixar TAG/Aviamento</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>
                                <asp:Label ID="labTitulo" runat="server" Text=""></asp:Label>
                            </legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 150px;">Produto:
                                        <asp:HiddenField ID="hidPedido" runat="server" />
                                        <asp:HiddenField ID="hidProduto" runat="server" />
                                        <asp:HiddenField ID="hidCor" runat="server" />
                                        <asp:HiddenField ID="hidEntrega" runat="server" />
                                        <asp:HiddenField ID="hidEti" runat="server" />
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
                                    <td>Tipo:
                                    </td>
                                    <td>
                                        <asp:Label ID="labTipo" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <fieldset>
                                            <legend>Grade</legend>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvGrade" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                    Style="background: white" OnRowDataBound="gvGrade_RowDataBound">
                                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                                    <RowStyle HorizontalAlign="Left" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="-" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litEspaco" runat="server"></asp:Literal>
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
                                    <td colspan="2">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: right;">
                                        <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>&nbsp;&nbsp;<asp:Button ID="btSalvar" runat="server" Width="122px" Text="Baixar" OnClientClick="DesabilitarBotao(this);" OnClick="btSalvar_Click" />&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">&nbsp;                                        
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

