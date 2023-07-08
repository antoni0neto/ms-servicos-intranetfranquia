<%@ Page Title="CRM - Cliente Detalhe" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="ecom_pedido_cliente_crm_det.aspx.cs" Inherits="Relatorios.ecom_pedido_cliente_crm_det"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }
    </style>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Clientes&nbsp;&nbsp;>&nbsp;&nbsp;CRM - Cliente Detalhe</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title=""></a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="CRM - Cliente Detalhe"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="2">
                            <fieldset>
                                <legend>Cliente</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>Cliente Atacado
                                            <asp:HiddenField ID="hidClienteAtacado" runat="server" Value="" />
                                        </td>
                                        <td>CPF
                                        </td>
                                        <td>Cliente
                                        </td>
                                        <td>E-Mail
                                        </td>
                                        <td>Qtde Pedido</td>
                                        <td>Valor Total</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:TextBox ID="txtClienteAtacado" runat="server" Width="140px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 150px;">
                                            <asp:TextBox ID="txtCPF" runat="server" Width="140px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 350px;">
                                            <asp:TextBox ID="txtCliente" runat="server" Width="340px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 280px;">
                                            <asp:TextBox ID="txtEmail" runat="server" Width="270px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 210px;">
                                            <asp:TextBox ID="txtQtdePedido" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtValorTotal" runat="server" Width="250px" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <fieldset>
                                                <legend>WhatsApp</legend>
                                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>Telefone 1</td>
                                                        <td>Telefone 2</td>
                                                        <td>Telefone 3</td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 270px;">
                                                            <asp:TextBox ID="txtTelefone1WA" runat="server" Width="210px" Enabled="false"></asp:TextBox>&nbsp;
                                                        <asp:ImageButton ID="btTelefone1WA" runat="server" Width="15px" ImageUrl="~/Image/whatsapp.png" ToolTip="Número 1" OnClick="btTelefoneWA_Click" />&nbsp;
                                                        <asp:ImageButton ID="btTelefone1WABloq" runat="server" Width="15px" ImageUrl="~/Image/delete.png" ToolTip="Bloquear Número" OnClick="btTelefoneWABloq_Click" OnClientClick="javascript: return confirm('Tem certeza que deseja bloquear este número?');" />
                                                        </td>
                                                        <td style="width: 270px;">
                                                            <asp:TextBox ID="txtTelefone2WA" runat="server" Width="210px" Enabled="false"></asp:TextBox>&nbsp;
                                                        <asp:ImageButton ID="btTelefone2WA" runat="server" Width="15px" ImageUrl="~/Image/whatsapp.png" ToolTip="Número 2" OnClick="btTelefoneWA_Click" />&nbsp;
                                                        <asp:ImageButton ID="btTelefone2WABloq" runat="server" Width="15px" ImageUrl="~/Image/delete.png" ToolTip="Bloquear Número" OnClick="btTelefoneWABloq_Click" OnClientClick="javascript: return confirm('Tem certeza que deseja bloquear este número?');" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtTelefone3WA" runat="server" Width="210px" Enabled="false"></asp:TextBox>&nbsp;
                                                        <asp:ImageButton ID="btTelefone3WA" runat="server" Width="15px" ImageUrl="~/Image/whatsapp.png" ToolTip="Número 3" OnClick="btTelefoneWA_Click" />&nbsp;
                                                        <asp:ImageButton ID="btTelefone3WABloq" runat="server" Width="15px" ImageUrl="~/Image/delete.png" ToolTip="Bloquear Número" OnClick="btTelefoneWABloq_Click" OnClientClick="javascript: return confirm('Tem certeza que deseja bloquear este número?');" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <fieldset>
                                                                <legend>SMS</legend>
                                                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td colspan="3">Texto
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3">
                                                                            <asp:TextBox ID="txtSMS" runat="server" Width="100%" MaxLength="150"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3">&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3">
                                                                            <asp:Button ID="btEnviarSMS" runat="server" Text="Enviar SMS" Width="120px" OnClick="btEnviarSMS_Click" Enabled="true" />
                                                                            &nbsp;<asp:Label ID="labSMS" runat="server" ForeColor="Red" Text=""></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
<%--                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <fieldset>
                                <legend>Histórico de Ações</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>--%>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <fieldset>
                                <legend>Últimas Peças que
                                    <asp:Label ID="labNomeClienteTit2" runat="server" Font-Bold="true"></asp:Label>
                                    comprou</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="text-align: center;">
                                            <asp:ImageButton ID="imgProduto1" runat="server" Width="140px" ImageAlign="AbsMiddle" ToolTip=" " AlternateText=" " Visible="false" /><br />
                                            <asp:Label ID="labProduto1" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:ImageButton ID="imgProduto2" runat="server" Width="140px" ImageAlign="AbsMiddle" ToolTip=" " AlternateText=" " Visible="false" /><br />
                                            <asp:Label ID="labProduto2" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:ImageButton ID="imgProduto3" runat="server" Width="140px" ImageAlign="AbsMiddle" ToolTip=" " AlternateText=" " Visible="false" /><br />
                                            <asp:Label ID="labProduto3" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:ImageButton ID="imgProduto4" runat="server" Width="140px" ImageAlign="AbsMiddle" ToolTip=" " AlternateText=" " Visible="false" /><br />
                                            <asp:Label ID="labProduto4" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:ImageButton ID="imgProduto5" runat="server" Width="140px" ImageAlign="AbsMiddle" ToolTip=" " AlternateText=" " Visible="false" /><br />
                                            <asp:Label ID="labProduto5" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:ImageButton ID="imgProduto6" runat="server" Width="140px" ImageAlign="AbsMiddle" ToolTip=" " AlternateText=" " Visible="false" /><br />
                                            <asp:Label ID="labProduto6" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>

                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <fieldset>
                                <legend>O que
                                    <asp:Label ID="labNomeClienteTit1" runat="server" Font-Bold="true"></asp:Label>
                                    mais compra</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvMaisCompra" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                    Style="background: white" OnRowDataBound="gvMaisCompra_RowDataBound"
                                                    ShowFooter="true">
                                                    <HeaderStyle BackColor="GradientActiveCaption" />
                                                    <FooterStyle BackColor="GradientActiveCaption" Font-Bold="true" HorizontalAlign="Left" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Griffe" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litGriffe" runat="server" Text='<%# Bind("GRIFFE") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Grupo Produto" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litGrupoProduto" runat="server" Text='<%# Bind("GRUPO_PRODUTO") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qtde de Produto" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litQtdeProduto" runat="server" Text='<%# Bind("QTDE_GRUPO_PRODUTO") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="" HeaderText="" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <fieldset>
                                <legend>Histórico Mensal</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvHistCliente" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                    Style="background: white" OnRowDataBound="gvHistCliente_RowDataBound" OnDataBound="gvHistCliente_DataBound"
                                                    ShowFooter="true">
                                                    <HeaderStyle BackColor="GradientActiveCaption" />
                                                    <FooterStyle BackColor="GradientActiveCaption" Font-Bold="true" HorizontalAlign="Left" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-VerticalAlign="Middle">
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../Image/plus.png"
                                                                    Width="15px" runat="server" />
                                                                <asp:Panel ID="pnlProduto" runat="server" Style="display: none" Width="100%">
                                                                    <asp:GridView ID="gvHistClienteProduto" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvHistClienteProduto_RowDataBound" OnDataBound="gvHistClienteProduto_DataBound" ShowFooter="true"
                                                                        Width="100%">
                                                                        <HeaderStyle BackColor="LightSteelBlue" HorizontalAlign="Center"></HeaderStyle>
                                                                        <RowStyle BorderColor="GradientActiveCaption" BorderWidth="1px" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="imgProduto" runat="server" Width="90%" ImageAlign="AbsMiddle" ToolTip=" " AlternateText=" " />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:BoundField DataField="SKU" HeaderText="SKUSemana" HeaderStyle-Width="200px" SortExpression="SKU" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                                            <asp:BoundField DataField="QTDE" HeaderText="Qtde" HeaderStyle-Width="160px" SortExpression="QTDE" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                                            <asp:TemplateField HeaderText="Preço" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="130px" SortExpression="PRECO" HeaderStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:Literal ID="litPreco" runat="server"></asp:Literal>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Desconto" ItemStyle-HorizontalAlign="Left" SortExpression="DESCONTO" HeaderStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:Literal ID="litDesconto" runat="server" Text=''></asp:Literal>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Valor Total" ItemStyle-HorizontalAlign="Left" SortExpression="VALOR_TOTAL" HeaderStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:Literal ID="litValTotal" runat="server" Text=''></asp:Literal>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Tipo" ItemStyle-HorizontalAlign="Left" SortExpression="TIPO_VENDA" HeaderStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:Literal ID="litTipo" runat="server" Text='<%# Bind("TIPO_VENDA") %>'></asp:Literal>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Ano" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litAno" runat="server" Text='<%# Bind("ANO") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mês" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litMes" runat="server" Text='<%# Bind("MES_EXT") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qtde de Pedido" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litQtdePedido" runat="server" Text='<%# Bind("TOT_PEDIDO") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Valor Subtotal" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litValorSubTotal" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Valor Frete" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litValorFrete" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Desconto" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litDesconto" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Valor Pago" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
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
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
