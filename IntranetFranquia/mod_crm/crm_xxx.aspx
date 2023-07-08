<%@ Page Title="Resumo do Cliente" Language="C#" AutoEventWireup="true" CodeBehind="crm_xxx.aspx.cs"
    Inherits="Relatorios.crm_xxx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Clientes - Detalhe</title>
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }
    </style>

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../Image/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "../Image/plus.png");
            $(this).closest("tr").next().remove();
        });

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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de CRM&nbsp;&nbsp;>&nbsp;&nbsp;Clientes&nbsp;&nbsp;>&nbsp;&nbsp;Resumo do Cliente</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Resumo do Cliente</legend>
                            <fieldset>
                                <legend>Dados</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>Código Cliente
                                            <asp:HiddenField ID="hidCPF" runat="server" Value="" />
                                        </td>
                                        <td>CPF</td>
                                        <td>Cliente</td>
                                        <td>E-Mail</td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 170px;">
                                            <asp:TextBox ID="txtCodigoCliente" runat="server" Width="160px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 150px;">
                                            <asp:TextBox ID="txtCPF" runat="server" Width="140px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 350px;">
                                            <asp:TextBox ID="txtCliente" runat="server" Width="340px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 320px;">
                                            <asp:TextBox ID="txtEmail" runat="server" Width="310px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td colspan="2">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">&nbsp;</td>
                                    </tr>
                                </table>
                            </fieldset>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <fieldset>
                                            <legend>Últimas Peças que
                                    <asp:Label ID="labNomeClienteTit2" runat="server" Font-Bold="true"></asp:Label>
                                                comprou</legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="text-align: center;">
                                                        <asp:ImageButton ID="imgProduto1" runat="server" Width="140px" ImageAlign="AbsMiddle" ToolTip=" " AlternateText=" " Visible="false" /><br />
                                                        <asp:Label ID="labProduto1" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:ImageButton ID="imgProduto2" runat="server" Width="140px" ImageAlign="AbsMiddle" ToolTip=" " AlternateText=" " Visible="false" /><br />
                                                        <asp:Label ID="labProduto2" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:ImageButton ID="imgProduto3" runat="server" Width="140px" ImageAlign="AbsMiddle" ToolTip=" " AlternateText=" " Visible="false" /><br />
                                                        <asp:Label ID="labProduto3" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:ImageButton ID="imgProduto4" runat="server" Width="140px" ImageAlign="AbsMiddle" ToolTip=" " AlternateText=" " Visible="false" /><br />
                                                        <asp:Label ID="labProduto4" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:ImageButton ID="imgProduto5" runat="server" Width="140px" ImageAlign="AbsMiddle" ToolTip=" " AlternateText=" " Visible="false" /><br />
                                                        <asp:Label ID="labProduto5" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:ImageButton ID="imgProduto6" runat="server" Width="140px" ImageAlign="AbsMiddle" ToolTip=" " AlternateText=" " Visible="false" /><br />
                                                        <asp:Label ID="labProduto6" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <fieldset>
                                            <legend>O que
                                    <asp:Label ID="labNomeClienteTit1" runat="server" Font-Bold="true"></asp:Label>
                                                mais compra</legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <div class="rounded_corners">
                                                            <asp:GridView ID="gvMaisCompra" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                                Style="background: white" OnRowDataBound="gvMaisCompra_RowDataBound"
                                                                ShowFooter="true">
                                                                <HeaderStyle BackColor="GradientActiveCaption" />
                                                                <FooterStyle BackColor="GradientActiveCaption" Font-Bold="true" HorizontalAlign="Left" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Griffe" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litGriffe" runat="server" Text='<%# Bind("GRIFFE") %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Grupo Produto" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litGrupoProduto" runat="server" Text='<%# Bind("GRUPO_PRODUTO") %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Qtde de Produto" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litQtdeProduto" runat="server" Text='<%# Bind("QTDE_GRUPO_PRODUTO") %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="" HeaderText="" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
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
