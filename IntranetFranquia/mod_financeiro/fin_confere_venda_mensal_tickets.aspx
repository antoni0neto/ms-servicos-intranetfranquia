<%@ Page Title="Total Venda Mês por Cliente - Tickets" Language="C#" AutoEventWireup="true" CodeBehind="fin_confere_venda_mensal_tickets.aspx.cs"
    Inherits="Relatorios.fin_confere_venda_mensal_tickets" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Total Venda Mês por Cliente - Tickets</title>
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Clientes&nbsp;&nbsp;>&nbsp;&nbsp;Total Venda Mês por Cliente&nbsp;&nbsp;>&nbsp;&nbsp;Tickets</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Tickets</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="hidAno" runat="server" Value="" />
                                        <asp:HiddenField ID="hidMes" runat="server" Value="" />
                                        <asp:HiddenField ID="hidCPF" runat="server" Value="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>CPF
                                    </td>
                                    <td colspan="2">Cliente
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 210px;">
                                        <asp:TextBox ID="txtCPF" runat="server" Width="200px"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtCliente" runat="server" Width="500px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvTickets" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                Style="background: white" OnRowDataBound="gvTickets_RowDataBound" OnDataBound="gvTickets_DataBound" ShowFooter="true"
                                                OnSorting="gvTickets_Sorting" AllowSorting="true">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                                <FooterStyle BackColor="Gainsboro" Font-Bold="true" />
                                                <RowStyle HorizontalAlign="Left" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center"
                                                        ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../Image/plus.png"
                                                                Width="15px" runat="server" />
                                                            <asp:Panel ID="pnlVendas" runat="server" Style="display: none" BorderWidth="2" Width="100%">

                                                                <asp:GridView ID="gvHistCancelamento" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                                    Style="background: white" OnRowDataBound="gvHistCancelamento_RowDataBound" OnDataBound="gvHistCancelamento_DataBound"
                                                                    ShowFooter="true">
                                                                    <HeaderStyle BackColor="Gainsboro" />
                                                                    <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Left" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Data Evento" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="litDataEvento" runat="server"></asp:Literal>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Evento" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="litEvento" runat="server" Text='<%# Bind("DESCRICAO") %>'></asp:Literal>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Histórico" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="litHistorico" runat="server" Text='<%# Bind("HISTORICO") %>'></asp:Literal>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Quem Cancelou" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="litQuemCancelou" runat="server" Text='<%# Bind("QUEM_CANCELOU") %>'></asp:Literal>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Cargo" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="litCargo" runat="server" Text='<%# Bind("DESC_CARGO") %>'></asp:Literal>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Filial" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width=""
                                                        SortExpression="FILIAL">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litFilial" runat="server" Text='<%# Bind("FILIAL") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Vendedor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width=""
                                                        SortExpression="NOME_VENDEDOR">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litNomeVendedor" runat="server" Text='<%# Bind("NOME_VENDEDOR") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Data" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width=""
                                                        SortExpression="DATA_DIGITACAO">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litDataVenda" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Ticket" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width=""
                                                        SortExpression="TICKET">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTicket" runat="server" Text='<%# Bind("TICKET") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qtde" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width=""
                                                        SortExpression="QTDE">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litQtde" runat="server" Text='<%# Bind("QTDE") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Venda Bruta" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width=""
                                                        SortExpression="VALOR_VENDA_BRUTA">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litValVendaBruta" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Valor Troca" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width=""
                                                        SortExpression="VALOR_TROCA">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litValTroca" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Valor Pago" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width=""
                                                        SortExpression="VALOR_PAGO">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litValorPago" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Desconto" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width=""
                                                        SortExpression="DESCONTO">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litDesconto" runat="server"></asp:Literal>
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
