<%@ Page Title="Venda de Produtos Semana" Language="C#" AutoEventWireup="true" CodeBehind="gest_venda_semana_filial.aspx.cs"
    Inherits="Relatorios.gest_venda_semana_filial" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Venda de Produtos Semana</title>
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Gestão&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Venda de Produtos Semana</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Venda de Produtos Semana - Filial</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="4">
                                        <asp:HiddenField ID="hidProduto" runat="server" Value="" />
                                        <asp:HiddenField ID="hidCor" runat="server" Value="" />
                                        <asp:HiddenField ID="hidDataIni" runat="server" Value="" />
                                        <asp:HiddenField ID="hidDataFim" runat="server" Value="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Produto</td>
                                    <td>Cor</td>
                                    <td>Início</td>
                                    <td>Fim</td>
                                </tr>
                                <tr>
                                    <td style="width: 210px;">
                                        <asp:TextBox ID="txtProduto" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 210px;">
                                        <asp:TextBox ID="txtCor" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 210px;">
                                        <asp:TextBox ID="txtDataIni" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDataFim" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvProdutos" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                Style="background: white" OnRowDataBound="gvProdutos_RowDataBound" OnDataBound="gvProdutos_DataBound" ShowFooter="true"
                                                OnSorting="gvProdutos_Sorting" AllowSorting="true">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                                <FooterStyle BackColor="GradientActiveCaption" Font-Bold="true" />
                                                <RowStyle HorizontalAlign="Left" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Filial" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width=""
                                                        SortExpression="FILIAL">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litFilial" runat="server" Text='<%# Bind("FILIAL") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qtde" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width=""
                                                        SortExpression="QTDE">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litQtde" runat="server" Text='<%# Bind("QTDE") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Valor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width=""
                                                        SortExpression="VALOR">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litValor" runat="server" Text=''></asp:Literal>
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
