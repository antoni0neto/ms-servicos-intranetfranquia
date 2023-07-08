<%@ Page Title="Comparativo Vendas - Analytics X Magento" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="ecom_venda_compara_analytics_mag.aspx.cs" Inherits="Relatorios.ecom_venda_compara_analytics_mag"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>

    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:updatepanel id="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Comparativo Vendas - Analytics X Magento</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Comparativo Vendas - Analytics X Magento"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Data Inicial
                        </td>
                        <td>Data Final
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 180px;">
                            <asp:TextBox ID="txtDataIni" runat="server" autocomplete="off" Width="170px" onkeypress="return fnValidarData(event);"
                                MaxLength="10" Style="text-align: right;" />
                        </td>
                        <td style="width: 180px;">
                            <asp:TextBox ID="txtDataFim" runat="server" autocomplete="off" Width="170px" onkeypress="return fnValidarData(event);"
                                MaxLength="10" Style="text-align: right;" />
                        </td>
                        <td>&nbsp;
                        </td>
                        <td style="text-align: right;">
                            <asp:Button ID="btDia" runat="server" Text="Dia" Width="100px" OnClick="btDia_Click" />&nbsp;
                            <asp:Button ID="btSemana" runat="server" Text="Semana" Width="100px" OnClick="btSemana_Click" />&nbsp;
                            <asp:Button ID="btMes" runat="server" Text="Mês" Width="100px" OnClick="btMes_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr style="line-height: 20px;">
                        <td colspan="4">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvPedido" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvPedido_RowDataBound"
                                    OnDataBound="gvPedido_DataBound" ShowFooter="true">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Bold="true" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Data" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="" SortExpression="DATA"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="">
                                            <ItemTemplate>
                                                <asp:Literal ID="litData" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                      <asp:TemplateField HeaderText="Valor Ads" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="95px" SortExpression="VALOR_ADS"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="AntiqueWhite">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValAds" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Qtde Analytics" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="110px" SortExpression="QTDE_PED_ANALYTICS"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="Ivory" ItemStyle-ForeColor="">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeAnalytics" runat="server" Text='<%# Bind("QTDE_PED_ANALYTICS") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Valor Analytics" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="115px" SortExpression="VAL_ANALYTICS"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="Ivory">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValAnalytics" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qtde Pedido" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="90px" SortExpression="QTDE_PED_MAGENTO"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdePedido" runat="server" Text='<%# Bind("QTDE_PED_MAGENTO") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Valor Bruto" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="110px" SortExpression="VAL_BRUTO"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValBruto" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Val Bol Pendente" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" SortExpression="VAL_BOLETO_PEN"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValBoletoPen" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Val Cartão Pendente" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="140px" SortExpression="VAL_CARTAO_PEN"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValCartaPen" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Valor Cancelado" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" SortExpression="VAL_CANCELADO"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="LightSalmon">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValCancelado" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Valor Pago" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" SortExpression="VAL_PAGO"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="PaleGreen">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValPago" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Cota" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="95px" SortExpression="COTAS"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValCota" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="% Bruto" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="85px" SortExpression="ATINGIDO_BRUTO"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="">
                                            <ItemTemplate>
                                                <asp:Literal ID="litAtingidoBruto" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="% Pago" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="85px" SortExpression="ATINGIDO_PAGO"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="">
                                            <ItemTemplate>
                                                <asp:Literal ID="litAtingidoPago" runat="server"></asp:Literal>
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
    </asp:updatepanel>
</asp:Content>
