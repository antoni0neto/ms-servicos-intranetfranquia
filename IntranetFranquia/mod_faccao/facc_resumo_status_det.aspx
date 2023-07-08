<%@ Page Title="Status Facção Detalhe" Language="C#" AutoEventWireup="true" CodeBehind="facc_resumo_status_det.aspx.cs"
    Inherits="Relatorios.facc_resumo_status_det" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Status Facção Detalhe</title>
    <style type="text/css">
        .fs {
            border: 1px solid #ccc;
            font-family: Calibri;
        }

        .alinharDireita {
            text-align: right;
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
            <ContentTemplate>
                <div style="margin-top: 10px; padding-left: 12px; padding-right: 12px; padding-bottom: -350px; background-color: White;">
                    <br />
                    <div>
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Facção&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Status Facção&nbsp;&nbsp;>&nbsp;&nbsp;Status Facção Detalhe</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Status Facção Detalhe</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>Fornecedor
                                    </td>
                                    <td>Total Qtde Faltante</td>
                                </tr>
                                <tr>
                                    <td style="width: 250px;">
                                        <asp:TextBox ID="txtFornecedor" runat="server" Width="240px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotal" runat="server" Width="150px" CssClass="alinharDireita" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="pnlCorteGrande" runat="server" Visible="false">
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <fieldset>
                                                <legend>Corte Grande</legend>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvCorteGrande" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white"
                                                        ShowFooter="true" OnRowDataBound="gvCorte_RowDataBound" OnDataBound="gvCorte_DataBound">
                                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true"></FooterStyle>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-VerticalAlign="Middle">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../Image/plus.png"
                                                                        Width="15px" runat="server" />
                                                                    <asp:Panel ID="pnlFoto" runat="server" Style="display: none" Width="100%">
                                                                        <asp:GridView ID="gvFoto" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvFoto_RowDataBound"
                                                                            Width="100%">
                                                                            <HeaderStyle BackColor="LightSteelBlue" HorizontalAlign="Center"></HeaderStyle>
                                                                            <RowStyle BorderColor="GradientActiveCaption" BorderWidth="1px" />
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Foto Peça" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px">
                                                                                    <ItemTemplate>
                                                                                        <asp:Image ID="imgFotoPeca" runat="server" ImageAlign="Middle" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Coleção" HeaderStyle-Width="180px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColecao" runat="server" Text='<%# Bind("DESC_COLECAO")%>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="HB" HeaderStyle-Width="100px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litHB" runat="server" Text='<%# Bind("HB") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Nome" SortExpression="NOME">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litNome" runat="server" Text='<%# Bind("NOME") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Qtde Faltante" HeaderStyle-Width="170px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litQtde" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlCortePequeno" runat="server" Visible="false">
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <fieldset>
                                                <legend>Corte Pequeno</legend>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvCortePequeno" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white"
                                                        ShowFooter="true" OnRowDataBound="gvCorte_RowDataBound" OnDataBound="gvCorte_DataBound">
                                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true"></FooterStyle>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-VerticalAlign="Middle">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../Image/plus.png"
                                                                        Width="15px" runat="server" />
                                                                    <asp:Panel ID="pnlFoto" runat="server" Style="display: none" Width="100%">
                                                                        <asp:GridView ID="gvFoto" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvFoto_RowDataBound"
                                                                            Width="100%">
                                                                            <HeaderStyle BackColor="LightSteelBlue" HorizontalAlign="Center"></HeaderStyle>
                                                                            <RowStyle BorderColor="GradientActiveCaption" BorderWidth="1px" />
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Foto Peça" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px">
                                                                                    <ItemTemplate>
                                                                                        <asp:Image ID="imgFotoPeca" runat="server" ImageAlign="Middle" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Coleção" HeaderStyle-Width="180px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColecao" runat="server" Text='<%# Bind("DESC_COLECAO")%>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="HB" HeaderStyle-Width="100px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litHB" runat="server" Text='<%# Bind("HB") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Nome" SortExpression="NOME">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litNome" runat="server" Text='<%# Bind("NOME") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Qtde Faltante" HeaderStyle-Width="170px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litQtde" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlCorteGrandeAtr" runat="server" Visible="false">
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <fieldset>
                                                <legend>Corte Grande Atrasado</legend>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvCorteGrandeAtrasado" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white"
                                                        ShowFooter="true" OnRowDataBound="gvCorte_RowDataBound" OnDataBound="gvCorte_DataBound">
                                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true"></FooterStyle>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-VerticalAlign="Middle">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../Image/plus.png"
                                                                        Width="15px" runat="server" />
                                                                    <asp:Panel ID="pnlFoto" runat="server" Style="display: none" Width="100%">
                                                                        <asp:GridView ID="gvFoto" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvFoto_RowDataBound"
                                                                            Width="100%">
                                                                            <HeaderStyle BackColor="LightSteelBlue" HorizontalAlign="Center"></HeaderStyle>
                                                                            <RowStyle BorderColor="GradientActiveCaption" BorderWidth="1px" />
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Foto Peça" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px">
                                                                                    <ItemTemplate>
                                                                                        <asp:Image ID="imgFotoPeca" runat="server" ImageAlign="Middle" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Coleção" HeaderStyle-Width="180px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColecao" runat="server" Text='<%# Bind("DESC_COLECAO")%>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="HB" HeaderStyle-Width="100px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litHB" runat="server" Text='<%# Bind("HB") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Nome" SortExpression="NOME">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litNome" runat="server" Text='<%# Bind("NOME") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Qtde Faltante" HeaderStyle-Width="170px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litQtde" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlCortePequenoAtr" runat="server" Visible="false">
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <fieldset>
                                                <legend>Corte Pequeno Atrasado</legend>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvCortePequenoAtrasado" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white"
                                                        ShowFooter="true" OnRowDataBound="gvCorte_RowDataBound" OnDataBound="gvCorte_DataBound">
                                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true"></FooterStyle>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-VerticalAlign="Middle">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../Image/plus.png"
                                                                        Width="15px" runat="server" />
                                                                    <asp:Panel ID="pnlFoto" runat="server" Style="display: none" Width="100%">
                                                                        <asp:GridView ID="gvFoto" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvFoto_RowDataBound"
                                                                            Width="100%">
                                                                            <HeaderStyle BackColor="LightSteelBlue" HorizontalAlign="Center"></HeaderStyle>
                                                                            <RowStyle BorderColor="GradientActiveCaption" BorderWidth="1px" />
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Foto Peça" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px">
                                                                                    <ItemTemplate>
                                                                                        <asp:Image ID="imgFotoPeca" runat="server" ImageAlign="Middle" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Coleção" HeaderStyle-Width="180px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColecao" runat="server" Text='<%# Bind("DESC_COLECAO")%>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="HB" HeaderStyle-Width="100px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litHB" runat="server" Text='<%# Bind("HB") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Nome" SortExpression="NOME">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litNome" runat="server" Text='<%# Bind("NOME") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Qtde Faltante" HeaderStyle-Width="170px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litQtde" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlMostruario" runat="server" Visible="false">
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <fieldset>
                                                <legend>Mostruário</legend>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvMostruario" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white"
                                                        ShowFooter="true" OnRowDataBound="gvCorte_RowDataBound" OnDataBound="gvCorte_DataBound">
                                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true"></FooterStyle>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-VerticalAlign="Middle">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../Image/plus.png"
                                                                        Width="15px" runat="server" />
                                                                    <asp:Panel ID="pnlFoto" runat="server" Style="display: none" Width="100%">
                                                                        <asp:GridView ID="gvFoto" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvFoto_RowDataBound"
                                                                            Width="100%">
                                                                            <HeaderStyle BackColor="LightSteelBlue" HorizontalAlign="Center"></HeaderStyle>
                                                                            <RowStyle BorderColor="GradientActiveCaption" BorderWidth="1px" />
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Foto Peça" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px">
                                                                                    <ItemTemplate>
                                                                                        <asp:Image ID="imgFotoPeca" runat="server" ImageAlign="Middle" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Coleção" HeaderStyle-Width="180px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColecao" runat="server" Text='<%# Bind("DESC_COLECAO")%>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="HB" HeaderStyle-Width="100px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litHB" runat="server" Text='<%# Bind("HB") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Nome" SortExpression="NOME">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litNome" runat="server" Text='<%# Bind("NOME") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Qtde Faltante" HeaderStyle-Width="170px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litQtde" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </fieldset>
                    </div>
                    <br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
