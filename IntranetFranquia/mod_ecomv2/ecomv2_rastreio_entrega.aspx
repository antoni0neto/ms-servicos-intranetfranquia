<%@ Page Title="Rastreio de Pedido" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ecomv2_rastreio_entrega.aspx.cs" Inherits="Relatorios.ecomv2_rastreio_entrega" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../js/js.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Pedido&nbsp;&nbsp;>&nbsp;&nbsp;Rastreio de Pedido</span>
        <div style="float: right; padding: 0;">
            <a href="~/mod_ecom/ecom_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div>
        <fieldset class="login">
            <legend>Rastreio de Pedido</legend>
            <div class="login">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="labTrackNumber" runat="server" Text="Código de Rastreio"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="labPedidoLinx" runat="server" Text="Pedido Linx"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="labPedidoExterno" runat="server" Text="Pedido Externo"></asp:Label>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 240px;">
                            <asp:TextBox ID="txtTrackNumberFiltro" runat="server" Width="226px"></asp:TextBox>
                        </td>
                        <td style="width: 210px;">
                            <asp:TextBox ID="txtPedidoLinxFiltro" runat="server" Width="196px"></asp:TextBox>
                        </td>
                        <td style="width: 210px;">
                            <asp:TextBox ID="txtPedidoExternoFiltro" runat="server" Width="196px"></asp:TextBox>
                        </td>
                        <td style="width: 210px;">
                            <asp:Button runat="server" ID="btBuscar" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />&nbsp&nbsp
                        </td>
                        <td style="text-align: right;">
                            <asp:Button ID="btAtualizarMagento" runat="server" Text="Atualizar Todos Status" Width="200px" OnClick="btAtualizarMagento_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Panel ID="pnlMandae" runat="server">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td colspan="7">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Código de Rastreio
                                        </td>
                                        <td>Pedido Linx
                                        </td>
                                        <td>Pedido Externo
                                        </td>
                                        <td>Cliente
                                        </td>
                                        <td>Serviço
                                        </td>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 240px;">
                                            <asp:TextBox ID="txtTrackNumber" runat="server" Width="176px" Enabled="false"></asp:TextBox>&nbsp;
                                            <asp:Button ID="btRastreio" runat="server" Text=">>" OnClick="btRastreio_Click" Width="42px" />
                                        </td>
                                        <td style="width: 210px;">
                                            <asp:TextBox ID="txtPedidoLinx" runat="server" Width="196px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 210px;">
                                            <asp:TextBox ID="txtPedidoExterno" runat="server" Width="196px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 210px;">
                                            <asp:TextBox ID="txtCliente" runat="server" Width="196px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 180px;">
                                            <asp:TextBox ID="txtServico" runat="server" Width="166px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 100px;">
                                            <asp:HiddenField ID="hidEtiquetaMandae" runat="server" Value="" />
                                            <asp:Button ID="btImprimir" runat="server" Text="Imprimir" Width="100px" OnClick="btImprimir_Click" />
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:Button ID="btAtualizarPedido" runat="server" Text="Atualizar Status Pedido" Width="200px" OnClick="btAtualizarPedido_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="7">
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvRastreioPedido" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ForeColor="#333333" Style="background: white;" OnRowDataBound="gvRastreioPedido_RowDataBound"
                                                    ShowFooter="true" DataKeyNames="">
                                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Data" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litData" runat="server" Text=""></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="NOME" HeaderText="Evento" HeaderStyle-Width="260px" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="DESCRICAO" HeaderText="Descrição" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" />

                                                        <asp:BoundField DataField="" HeaderText="" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">&nbsp;
                        </td>
                    </tr>
                </table>
            </div>
        </fieldset>
    </div>
</asp:Content>
