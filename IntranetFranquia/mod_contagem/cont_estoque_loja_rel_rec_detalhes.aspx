<%@ Page Title="Relatório Recontagem Física" Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="cont_estoque_loja_rel_rec_detalhes.aspx.cs"
    Inherits="Relatorios.cont_estoque_loja_rel_rec_detalhes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Relatório Recontagem Física</title>
    <style type="text/css">
        .alinharCentro {
            text-align: center;
        }
    </style>
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
            <Triggers>
                <asp:PostBackTrigger ControlID="btExcel" />
            </Triggers>
            <ContentTemplate>
                <div style="margin-top: 10px; padding-left: 12px; padding-right: 12px; padding-bottom: -350px; background-color: White;">
                    <br />
                    <div>
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Contagem&nbsp;&nbsp;>&nbsp;&nbsp;Estoque Loja&nbsp;&nbsp;>&nbsp;&nbsp;Contagem Estoque Loja&nbsp;&nbsp;>&nbsp;&nbsp;Relatório Recontagem Física</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Relatório Recontagem Física</legend>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>Filial
                                        <asp:HiddenField ID="hidEditMode" runat="server" Value="" />
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
                                            <asp:GridView ID="gvEstoqueLojaContRec" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvEstoqueLojaContRec_RowDataBound"
                                                OnDataBound="gvEstoqueLojaContRec_DataBound" ShowFooter="true"
                                                OnSorting="gvEstoqueLojaContRec_Sorting" AllowSorting="true" DataKeyNames="ESTOQUE_LOJA_CONT_RET">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center" 
                                                        ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../Image/plus.png" Visible='<%# IsInEditMode %>'
                                                                Width="15px" runat="server" />
                                                            <asp:Panel ID="pnlFoto" runat="server" Style="display: none" Width="100%"  Visible='<%# IsInEditMode %>'>
                                                                <asp:GridView ID="gvFoto" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvFoto_RowDataBound"
                                                                    Width="100%" DataKeyNames="CODIGO">
                                                                    <HeaderStyle BackColor="LightSteelBlue" HorizontalAlign="Center"></HeaderStyle>
                                                                    <RowStyle BorderColor="Gainsboro" BorderWidth="1px" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Foto Peça" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                            ItemStyle-BorderColor="Gainsboro" ItemStyle-BorderWidth="1px">
                                                                            <ItemTemplate>
                                                                                <asp:Image ID="imgFotoPeca" runat="server" ImageAlign="Middle" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Produto" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" SortExpression="PRODUTO">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litProduto" runat="server" Text='<%# Bind("PRODUTO") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Nome" ItemStyle-HorizontalAlign="Left" SortExpression="DESC_PRODUTO">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litNome" runat="server" Text='<%# Bind("DESC_PRODUTO") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cor" ItemStyle-HorizontalAlign="Left" SortExpression="DESC_COR">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litCor" runat="server" Text='<%# Bind("DESC_COR") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Saldo" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-ForeColor="YellowGreen" ItemStyle-Font-Bold="true"
                                                        SortExpression="TOTAL_ESTOQUE">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litSaldo" runat="server" Text='<%# Bind("TOTAL_ESTOQUE") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Recontagem" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Blue" ItemStyle-Font-Bold="true">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litContagem" runat="server" Visible='<%# !(bool) IsInEditMode %>' Text='<%# Bind("ESTOQUE_REC") %>'></asp:Literal>
                                                            <asp:TextBox ID="txtContagem" runat="server" OnTextChanged="txtContagem_TextChanged" AutoPostBack="true" Visible='<%# IsInEditMode %>' CssClass="alinharCentro" Width="160px"
                                                                onkeypress="return fnValidarNumero(event);" Text='<%# Bind("ESTOQUE_REC") %>'>
                                                            </asp:TextBox>
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
