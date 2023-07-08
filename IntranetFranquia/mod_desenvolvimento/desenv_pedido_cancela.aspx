<%@ Page Title="Cancelar Pedido" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="desenv_pedido_cancela.aspx.cs" Inherits="Relatorios.desenv_pedido_cancela" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinharDireita
        {
            text-align: right;
        }
    </style>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Controle
                    de Produto&nbsp;&nbsp;>&nbsp;&nbsp;Cancelar Pedido</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <table border="0" class="table" width="100%">
                    <tr>
                        <td>
                            <fieldset>
                                <legend>Cancelar Pedido</legend>
                                <div>
                                    <label>
                                        Número do Pedido</label><br />
                                    <asp:TextBox ID="txtPedidoNumero" runat="server" Width="150px" onkeypress="return fnValidarNumero(event);"
                                        MaxLength="10" Style="text-align: right;" />&nbsp;&nbsp;
                                    <asp:Button ID="btBuscar" runat="server" Text="Buscar Pedido" OnClick="btBuscar_Click"
                                        Width="100px" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="labMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </div>
                                <br />
                                <table border="0" cellpadding="0" class="tb" width="100%">
                                    <tr>
                                        <td style="width: 100%;">
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvPedido" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvPedido_RowDataBound"
                                                    OnDataBound="gvPedido_DataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="NUMERO_PEDIDO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="Número Pedido" HeaderStyle-Width="150px" />
                                                        <asp:BoundField DataField="FORNECEDOR" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderText="Fornecedor" HeaderStyle-Width="" />
                                                        <asp:BoundField DataField="DATA_PEDIDO" DataFormatString="{0:d}" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" HeaderText="Data do Pedido" HeaderStyle-Width="200px" />
                                                        <asp:BoundField DataField="DATA_ENTREGA_PREV" DataFormatString="{0:d}" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" HeaderText="Previsão de Entrega" HeaderStyle-Width="200px" />
                                                        <asp:TemplateField HeaderText="Qtde de Produto" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litQtdeProduto" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="QTDE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="Qtde Inicial" HeaderStyle-Width="150px" />
                                                        <asp:TemplateField HeaderText="Qtde em Casa" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litQtdeEmCasa" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="" HeaderText="" HeaderStyle-Width="" />
                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btCancelar" runat="server" Height="20px" Text="Cancelar" Width="100px"
                                                                    OnClick="btCancelar_Click" OnClientClick="return ConfirmarCancelamento();" />
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
