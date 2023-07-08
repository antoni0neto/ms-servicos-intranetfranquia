<%@ Page Title="Receitas X Despesas - Conta Contábil" Language="C#" AutoEventWireup="true" CodeBehind="contabil_receita_x_despesa_det.aspx.cs"
    Inherits="Relatorios.contabil_receita_x_despesa_det" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Receitas X Despesas - Conta Contábil</title>
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <link href="../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../../js/js.js" type="text/javascript"></script>
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Contabilidade&nbsp;&nbsp;>&nbsp;&nbsp;Conciliação&nbsp;&nbsp;>&nbsp;&nbsp;Receitas X Despesas&nbsp;&nbsp;</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Receitas X Despesas - Conta Contábil</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>Competência
                                    </td>
                                    <td>Conta Contábil
                                    </td>
                                    <td>Valor
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width: 150px;">
                                        <asp:TextBox ID="txtCompetencia" runat="server" Width="136px" ReadOnly="true" Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 450px;">
                                        <asp:TextBox ID="txtContaContabil" runat="server" Width="436px" ReadOnly="true" Text=""></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValor" runat="server" Width="150px" CssClass="alinharDireita" ReadOnly="true" Text=""></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:HiddenField ID="hidContaContabil" runat="server" />
                                        <asp:HiddenField ID="hidMatrizContabil" runat="server" />
                                        <asp:HiddenField ID="hidMes" runat="server" />
                                        <asp:HiddenField ID="hidAno" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 0;" colspan="4">
                                        <fieldset>
                                            <legend>Lançamentos</legend>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvLancamento" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvLancamento_RowDataBound"
                                                    OnDataBound="gvLancamento_DataBound" OnSorting="gvLancamento_Sorting" AllowSorting="true">
                                                    <HeaderStyle BackColor="GradientActiveCaption" Font-Size="Small" HorizontalAlign="Center"></HeaderStyle>
                                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FILIAL" HeaderText="Filial" HeaderStyle-HorizontalAlign="Left" SortExpression="FILIAL" />
                                                        <asp:BoundField DataField="DESC_CENTRO_CUSTO" HeaderText="Centro Custo" HeaderStyle-HorizontalAlign="Left" SortExpression="DESC_CENTRO_CUSTO" />
                                                        <asp:BoundField DataField="LANCAMENTO" HeaderText="Lançamento" ItemStyle-HorizontalAlign="Center" SortExpression="LANCAMENTO" />
                                                        <asp:BoundField DataField="DATA_LANCAMENTO" HeaderText="Data" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:d}" SortExpression="DATA" />
                                                        <asp:BoundField DataField="CREDITO" HeaderText="Crédito" ItemStyle-HorizontalAlign="Center" SortExpression="CREDITO" />
                                                        <asp:BoundField DataField="DEBITO" HeaderText="Débito" ItemStyle-HorizontalAlign="Center" SortExpression="DEBITO" />
                                                        <asp:BoundField DataField="HISTORICO" HeaderText="Histórico" HeaderStyle-HorizontalAlign="Left" SortExpression="HISTORICO" />
                                                        <asp:BoundField DataField="USUARIO_MOV" HeaderText="Usuário" HeaderStyle-HorizontalAlign="Left" SortExpression="USUARIO_MOV" />

                                                    </Columns>
                                                </asp:GridView>
                                            </div>
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
