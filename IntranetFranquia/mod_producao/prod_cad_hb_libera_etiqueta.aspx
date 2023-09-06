<%@ Page Title="Baixar Etiqueta" Language="C#" AutoEventWireup="true" CodeBehind="prod_cad_hb_libera_etiqueta.aspx.cs"
    Inherits="Relatorios.prod_cad_hb_libera_etiqueta" %>

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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Custo/Preço de Produto&nbsp;&nbsp;>&nbsp;&nbsp;Liberação de Etiqueta&nbsp;&nbsp;>&nbsp;&nbsp;Baixar Etiqueta</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Baixar Etiqueta</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 150px;">Produto:
                                        <asp:HiddenField ID="hidCodigoHB" runat="server" />
                                        <asp:HiddenField ID="hidPedido" runat="server" />
                                        <asp:HiddenField ID="hidProduto" runat="server" />
                                        <asp:HiddenField ID="hidCor" runat="server" />
                                        <asp:HiddenField ID="hidEntrega" runat="server" />
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
                                    <td>Custo - TC:
                                    </td>
                                    <td>
                                        <asp:Label ID="labCustoTC" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Preço - SC:
                                    </td>
                                    <td>
                                        <asp:Label ID="labPrecoSC" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Preço - TO:
                                    </td>
                                    <td>
                                        <asp:Label ID="labPrecoTO" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Preço - TL:
                                    </td>
                                    <td>
                                        <asp:Label ID="labPrecoTL" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Preço - TD:
                                    </td>
                                    <td>
                                        <asp:Label ID="labPrecoTD" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Preço - TN:
                                    </td>
                                    <td>
                                        <asp:Label ID="labPrecoTN" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Preço - TA:
                                    </td>
                                    <td>
                                        <asp:Label ID="labPrecoTA" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Preço - TT:
                                    </td>
                                    <td>
                                        <asp:Label ID="labPrecoTT" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Preço - TS:
                                    </td>
                                    <td>
                                        <asp:Label ID="labPrecoTS" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Preço - TX:
                                    </td>
                                    <td>
                                        <asp:Label ID="labPrecoTX" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <fieldset>
                                            <legend>Composição</legend>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvComposicao" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                    Style="background: white">
                                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                                    <RowStyle HorizontalAlign="Left" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="QTDE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="Quantidade" HeaderStyle-Width="190px" />
                                                        <asp:BoundField DataField="DESCRICAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderText="Descrição" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </fieldset>
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

