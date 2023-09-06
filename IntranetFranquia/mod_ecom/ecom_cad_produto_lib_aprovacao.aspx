<%@ Page Title="Aprovação de Pedido" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ecom_cad_produto_lib_aprovacao.aspx.cs" Inherits="Relatorios.ecom_cad_produto_lib_aprovacao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>

    <script type="text/javascript">
        function openwindow(l) {
            window.open(l, "PEDIDOMAG_APROV", "menubar=1,resizable=0,width=1000,height=500");
        }

        function openwindowhist(l) {
            window.open(l, "PEDIDOMAG_BOLETO_HIST", "menubar=1,resizable=0,width=1000,height=700");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Pedido&nbsp;&nbsp;>&nbsp;&nbsp;Aprovação de Pedido</span>
        <div style="float: right; padding: 0;">
            <a href="ecom_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div>
        <fieldset class="login">
            <legend>Aprovação de Pedido</legend>
            <div class="login">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td colspan="2">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvAprovacao" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvAprovacao_RowDataBound"
                                    ShowFooter="true" DataKeyNames="PEDIDO">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="labColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litAbrirHistorico" runat="server" />
                                                <asp:Literal ID="litQtdeTentativa" runat="server" Text="0" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="PEDIDO" HeaderText="Pedido Linx" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Data Pedido" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litDataPedido" runat="server" Text=""></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CLIENTE_ATACADO" HeaderText="Cliente" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField HeaderText="Qtde" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeProduto" runat="server" Text=""></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Valor Pedido" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValorPedido" runat="server" Text=""></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="MOTIVO" HeaderText="Antifraude" HeaderStyle-HorizontalAlign="Left" />

                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litAbrirPedido" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="btExcluir" Text="Excluir" Width="15px" ImageUrl="~/Image/delete.png" OnClick="btExcluir_Click" OnClientClick="javascript: return confirm('Tem certeza que deseja excluir este pedido?');" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="btBaixar" Text="Aprovar" ImageUrl="~/Image/approve.png" Width="15px" OnClick="btBaixar_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </fieldset>
    </div>
</asp:Content>
