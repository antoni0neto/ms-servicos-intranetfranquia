<%@ Page Title="Desempenho Vendedor Cliente" Language="C#" AutoEventWireup="true" CodeBehind="gerloja_desempenho_vendedor_nvtkt.aspx.cs"
    Inherits="Relatorios.gerloja_desempenho_vendedor_nvtkt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Desempenho Vendedor Cliente</title>
    <style type="text/css">
        .bodyMaterial {
            font-family: "Helvetica Neue", "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif !important;
            margin: 0px;
            padding: 0px;
            color: #696969;
        }

        .jGrowl .manilla {
            background-color: #000;
            color: red;
            font-weight: bold;
        }
    </style>
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
    </script>

</head>
<body>
    <div class="main">
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="up1" runat="server">
                <ContentTemplate>
                    <div style="margin-top: 10px; padding-left: 12px; padding-right: 12px; padding-bottom: -350px; background-color: White;">
                        <br />
                        <div>
                            <span style="font-family: Calibri; font-size: 14px;">Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Desempenho&nbsp;&nbsp;>&nbsp;&nbsp;Desempenho Vendedor Cliente&nbsp;&nbsp;</span>
                            <div style="float: right; padding: 0;">
                                <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                            </div>
                        </div>
                        <hr />
                        <div>
                            <fieldset>
                                <legend>Desempenho Vendedor Cliente</legend>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td>Data Início
                                            <asp:HiddenField ID="hidVendedor" runat="server" Value="" />
                                        </td>
                                        <td>Data Fim
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px;">
                                            <asp:TextBox ID="txtDataInicial" runat="server" Width="190px" MaxLength="10" Style="text-align: right;" onkeypress="return fnValidarData(event);"></asp:TextBox>
                                        </td>
                                        <td style="width: 200px;">
                                            <asp:TextBox ID="txtDataFinal" runat="server" Width="190px" MaxLength="10" Style="text-align: right;" onkeypress="return fnValidarData(event);"></asp:TextBox>
                                        </td>
                                        <td style="width: 310px;">
                                            <asp:Button runat="server" ID="btBuscar" Text="Buscar Vendas" Width="150px" OnClick="btBuscar_Click" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvTicket" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                    OnRowDataBound="gvTicket_RowDataBound" OnDataBound="gvTicket_DataBound" ShowFooter="true"
                                                    OnSorting="gvTicket_Sorting" AllowSorting="true">
                                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left"></FooterStyle>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="VENDEDOR" SortExpression="VENDEDOR" HeaderText="Vendedor" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="NOME_VENDEDOR" SortExpression="NOME_VENDEDOR" HeaderText="Nome Vendedor" ItemStyle-Font-Size="Smaller" />
                                                        <asp:BoundField DataField="TICKET" SortExpression="TICKET" HeaderText="Ticket" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="CPF" SortExpression="CPF" HeaderText="CPF" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:TemplateField HeaderText="Data Venda" SortExpression="DATA_VENDA" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litDataVenda" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Valor Pago" SortExpression="VALOR_PAGO" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litValorPago" runat="server"></asp:Literal>
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
    </div>
</body>
</html>
