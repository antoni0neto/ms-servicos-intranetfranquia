<%@ Page Title="DRE - Conta Contábil" Language="C#" AutoEventWireup="true" CodeBehind="dre_dre_v10_conta_contabil.aspx.cs"
    Inherits="Relatorios.dre_dre_v10_conta_contabil" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>DRE - Conta Contábil</title>
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo Acompanhamento Mensal&nbsp;&nbsp;>&nbsp;&nbsp;DRE&nbsp;&nbsp;>&nbsp;&nbsp;Conta Contábil&nbsp;&nbsp;</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>DRE - Conta Contábil</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>Competência
                                    </td>
                                    <td>Centro de Custo
                                    </td>
                                    <td>Conta Contábil
                                    </td>
                                    <td>Valor
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width: 150px;">
                                        <asp:TextBox ID="txtCompetencia" runat="server" Width="140px" ReadOnly="true" Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 450px;">
                                        <asp:TextBox ID="txtCentroCusto" runat="server" Width="440px" ReadOnly="true" Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 450px;">
                                        <asp:TextBox ID="txtContaContabil" runat="server" Width="440px" ReadOnly="true" Text=""></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValor" runat="server" Width="150px" CssClass="alinharDireita" ReadOnly="true" Text=""></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:HiddenField ID="hidCodCentroCusto" runat="server" />
                                        <asp:HiddenField ID="hidContaContabil" runat="server" />
                                        <asp:HiddenField ID="hidUnidadeNegocio" runat="server" />
                                        <asp:HiddenField ID="hidCodigoFilial" runat="server" />
                                        <asp:HiddenField ID="hidMes" runat="server" />
                                        <asp:HiddenField ID="hidAno" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 0;" colspan="5">
                                        <fieldset>
                                            <legend>Lançamentos</legend>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvLancamento" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvLancamento_RowDataBound"
                                                    OnDataBound="gvLancamento_DataBound" OnSorting="gvLancamento_Sorting" AllowSorting="true">
                                                    <HeaderStyle BackColor="GradientActiveCaption" Font-Size="Small" HorizontalAlign="Center"></HeaderStyle>
                                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FILIAL" HeaderText="Filial" HeaderStyle-HorizontalAlign="Left" SortExpression="FILIAL" />
                                                        <asp:BoundField DataField="LANCAMENTO" HeaderText="Lançamento" ItemStyle-HorizontalAlign="Center" SortExpression="LANCAMENTO" />
                                                        <asp:BoundField DataField="DATA" HeaderText="Data" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:d}" SortExpression="DATA" />
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
