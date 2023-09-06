<%@ Page Title="Relatório Contagem Física" Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="cont_estoque_loja_rel_detalhes.aspx.cs"
    Inherits="Relatorios.cont_estoque_loja_rel_detalhes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Relatório Contagem Física</title>
    <style type="text/css">
    </style>
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../js/js.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="up1" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btExcel" />
            </Triggers>
            <ContentTemplate>
                <div style="margin-top: 10px; padding-left: 12px; padding-right: 12px; padding-bottom: -350px; background-color: White;">
                    <br />
                    <div>
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Contagem&nbsp;&nbsp;>&nbsp;&nbsp;Estoque Loja&nbsp;&nbsp;>&nbsp;&nbsp;Contagem Estoque Loja&nbsp;&nbsp;>&nbsp;&nbsp;Relatório Contagem Física</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Relatório Contagem Física</legend>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>Filial
                                    </td>
                                    <td>Descrição
                                    </td>
                                    <td>Data Contagem
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width: 210px;">
                                        <asp:TextBox ID="txtFilial" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td style="width: 210px;">
                                        <asp:TextBox ID="txtDescricao" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td style="width: 210px;">
                                        <asp:TextBox ID="txtDataContagem" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Button ID="btExcel" runat="server" Width="100px" Text="Excel" OnClick="btExcel_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvEstoqueLojaCont" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvEstoqueLojaCont_RowDataBound"
                                                OnSorting="gvEstoqueLojaCont_Sorting" AllowSorting="true"
                                                OnDataBound="gvEstoqueLojaCont_DataBound" ShowFooter="true">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Produto" HeaderStyle-Width="83px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" SortExpression="PRODUTO">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litProduto" runat="server" Text='<%# Bind("PRODUTO") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Nome" ItemStyle-HorizontalAlign="Left"
                                                        SortExpression="DESC_PRODUTO">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litNome" runat="server" Text='<%# Bind("DESC_PRODUTO") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cor" ItemStyle-HorizontalAlign="Left"
                                                        SortExpression="COR">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litCor" runat="server" Text='<%# Bind("DESC_COR") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qtde Cont" HeaderStyle-Width="97px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" SortExpression="TOTAL_CONTAGEM" ItemStyle-ForeColor="Red" ItemStyle-Font-Bold="true">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litQtdeContagem" runat="server" Text='<%# Bind("TOTAL_CONTAGEM") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Saldo" HeaderStyle-Width="93px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" SortExpression="TOTAL_ESTOQUE" ItemStyle-ForeColor="YellowGreen" ItemStyle-Font-Bold="true">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litSaldo" runat="server" Text='<%# Bind("TOTAL_ESTOQUE") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Diferença" HeaderStyle-Width="93px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" SortExpression="DIFERENCA" ItemStyle-ForeColor="Blue" ItemStyle-Font-Bold="true">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litDiff" runat="server" Text='<%# Bind("DIFERENCA") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="XP" HeaderStyle-Width="33px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-BackColor="WhiteSmoke">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litC1" runat="server" Text='<%# Bind("C1") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PP" HeaderStyle-Width="33px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-BackColor="WhiteSmoke">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litC2" runat="server" Text='<%# Bind("C2") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="P" HeaderStyle-Width="33px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-BackColor="WhiteSmoke">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litC3" runat="server" Text='<%# Bind("C3") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="M" HeaderStyle-Width="33px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-BackColor="WhiteSmoke">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litC4" runat="server" Text='<%# Bind("C4") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="G" HeaderStyle-Width="33px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-BackColor="WhiteSmoke">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litC5" runat="server" Text='<%# Bind("C5") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="GG" HeaderStyle-Width="33px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-BackColor="WhiteSmoke">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litC6" runat="server" Text='<%# Bind("C6") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Total" HeaderStyle-Width="42px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-BackColor="Gainsboro" ItemStyle-Font-Bold="true">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litQtdeContagemTotal" runat="server" Text='<%# Bind("TOTAL_CONTAGEM") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="XP" HeaderStyle-Width="33px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-BackColor="WhiteSmoke">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litS1" runat="server" Text='<%# Bind("S1") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PP" HeaderStyle-Width="33px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-BackColor="WhiteSmoke">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litS2" runat="server" Text='<%# Bind("S2") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="P" HeaderStyle-Width="33px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-BackColor="WhiteSmoke">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litS3" runat="server" Text='<%# Bind("S3") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="M" HeaderStyle-Width="33px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-BackColor="WhiteSmoke">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litS4" runat="server" Text='<%# Bind("S4") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="G" HeaderStyle-Width="33px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-BackColor="WhiteSmoke">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litS5" runat="server" Text='<%# Bind("S5") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="GG" HeaderStyle-Width="33px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-BackColor="WhiteSmoke">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litS6" runat="server" Text='<%# Bind("S6") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Total" HeaderStyle-Width="42px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-BackColor="Gainsboro" ItemStyle-Font-Bold="true">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litSaldoTotal" runat="server" Text='<%# Bind("TOTAL_ESTOQUE") %>'></asp:Literal>
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
