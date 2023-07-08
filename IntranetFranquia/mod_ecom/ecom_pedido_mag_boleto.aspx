<%@ Page Title="Vendas - Pedido" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="ecom_pedido_mag_boleto.aspx.cs" Inherits="Relatorios.ecom_pedido_mag_boleto"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .pageStyl a, .pageStyl span {
            display: block;
            height: 15px;
            width: 15px;
            font-weight: bold;
            text-align: center;
            text-decoration: none;
        }

        .pageStyl a {
            background-color: #f5f5f5;
            color: #969696;
            border: 1px solid #969696;
        }

        .pageStyl span {
            background-color: #A1DCF2;
            color: #000;
            border: 1px solid #3AC0F2;
        }
    </style>

    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        function openwindow(l) {
            window.open(l, "PEDIDOMAG_BOLETO", "menubar=1,resizable=0,width=1000,height=700");
        }
        function openwindowhist(l) {
            window.open(l, "PEDIDOMAG_BOLETO_HIST", "menubar=1,resizable=0,width=1000,height=700");
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Clientes&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Boletos</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Vendas - Pedido"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Pedido Externo</td>
                        <td>Data Inicial
                        </td>
                        <td>Data Final
                        </td>
                        <td>Nome
                        </td>
                        <td>CPF
                        </td>
                        <td>Email
                        </td>
                        <td>Status Magento
                        </td>
                        <td>Status Cliente
                        </td>
                        <td>Total de Contato
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 120px;">
                            <asp:TextBox ID="txtPedido" runat="server" Width="110px" onkeypress="return fnValidarNumero(event);"
                                MaxLength="10" Style="text-align: right;" />
                        </td>
                        <td style="width: 120px;">
                            <asp:TextBox ID="txtDataIni" runat="server" autocomplete="off" Width="110px" onkeypress="return fnValidarData(event);"
                                MaxLength="10" Style="text-align: right;" />
                        </td>
                        <td style="width: 120px;">
                            <asp:TextBox ID="txtDataFim" runat="server" autocomplete="off" Width="110px" onkeypress="return fnValidarData(event);"
                                MaxLength="10" Style="text-align: right;" />
                        </td>
                        <td style="width: 170px;">
                            <asp:TextBox ID="txtNome" runat="server" Width="160px" MaxLength="30" />
                        </td>
                        <td style="width: 110px;">
                            <asp:TextBox ID="txtCPF" runat="server" Width="100px" MaxLength="11" />
                        </td>
                        <td style="width: 150px;">
                            <asp:TextBox ID="txtEmail" runat="server" Width="140px" MaxLength="50" />
                        </td>
                        <td style="width: 190px;">
                            <asp:DropDownList ID="ddlStatusMag" runat="server" Width="184px" Height="21px" DataValueField="CODIGO" DataTextField="DESCRICAO"></asp:DropDownList>
                        </td>
                        <td style="width: 190px;">
                            <asp:DropDownList ID="ddlStatusCliente" runat="server" Width="184px" Height="21px" DataValueField="CODIGO" DataTextField="MOTIVO"></asp:DropDownList>
                        </td>
                        <td style="width: 190px;">
                            <asp:DropDownList ID="ddlTotalContato" runat="server" Width="184px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="0" Text="0"></asp:ListItem>
                                <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                <asp:ListItem Value="5" Text="5+"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10">
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="114px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />
                        </td>
                        <td colspan="9">
                            <asp:Button ID="btLimpar" runat="server" Text="Limpar" Width="114px" OnClick="btLimpar_Click" OnClientClick="DesabilitarBotao(this);" />
                        </td>
                    </tr>
                    <tr style="line-height: 20px;">
                        <td colspan="10">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10">
                            <div class="rounded_corners">
                                <div style="text-align: left; color: red; float: left;">
                                    &nbsp;&nbsp;&nbsp;<i><%=gvPedido.Rows.Count%> Pedido(s)</i>
                                </div>
                                <div style="text-align: right; color: red; float: right;">
                                    Pedidos são atualizados a cada 5 minutos&nbsp;&nbsp;&nbsp;
                                </div>
                                <asp:GridView ID="gvPedido" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvPedido_RowDataBound"
                                    OnDataBound="gvPedido_DataBound" ShowFooter="true"
                                    OnSorting="gvPedido_Sorting" AllowSorting="true">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Bold="true" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litAbrirPedido" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litAbrirHistorico" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PEDIDO_EXTERNO" HeaderText="Pedido" SortExpression="PEDIDO_EXTERNO" HeaderStyle-Width="95px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Data" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="130px" SortExpression="DATA_CRIACAO"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litData" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CLIENTE" HeaderText="Cliente" SortExpression="CLIENTE" />
                                        <asp:TemplateField HeaderText="Valor Boleto" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="90px" SortExpression="VALOR_BOLETO"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValBoleto" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="DIAS_EXPIRAR" HeaderText="Dias Exp" SortExpression="DIAS_EXPIRAR" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Vencimento" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" SortExpression="BOLETO_EXPIRACAO"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litDataBolExp" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="QTDE_PRODUTO" HeaderText="Qtde Produto" SortExpression="QTDE_PRODUTO" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="DESC_STATUS_MAGENTO" HeaderText="Status" SortExpression="DESC_STATUS_MAGENTO" />

                                        <asp:BoundField DataField="TOTAL_CONTATO" HeaderText="Total Contato" SortExpression="TOTAL_CONTATO" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="STATUS_CLIENTE" HeaderText="Status Cliente" SortExpression="STATUS_CLIENTE" />

                                        <asp:TemplateField HeaderText="Último Contato" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" SortExpression="ULTIMO_CONTATO"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litUltimoContato" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
