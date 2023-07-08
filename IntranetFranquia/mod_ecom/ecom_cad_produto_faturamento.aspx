<%@ Page Title="Faturamento" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ecom_cad_produto_faturamento.aspx.cs" Inherits="Relatorios.ecom_cad_produto_faturamento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>

    <script type="text/javascript">
        function openwindow(l) {
            window.open(l, "PEDIDOMAG_FATURAM", "menubar=1,resizable=0,width=1000,height=500");
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Pedido&nbsp;&nbsp;>&nbsp;&nbsp;Faturamento</span>
        <div style="float: right; padding: 0;">
            <a href="ecom_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div>
        <fieldset class="login">
            <legend>Faturamento</legend>
            <div class="login">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:Button ID="btAtualizar" runat="server" Text="Atualizar" Width="150px" OnClick="btAtualizar_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btImprimir" runat="server" Text="Imprimir" Width="150px" OnClick="btImprimir_Click" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <fieldset>
                                <legend>Para Envio</legend>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvFaturamento" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvFaturamento_RowDataBound"
                                        ShowFooter="true" DataKeyNames="PEDIDO">
                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litAbrirPedido" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PEDIDO_ORIGEM" HeaderText="Origem" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="PEDIDO" HeaderText="Pedido Linx" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="Data Pedido" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDataPedido" runat="server" Text=""></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CLIFOR" HeaderText="CLIFOR" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="CLIENTE_ATACADO" HeaderText="Cliente" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderText="Quantidade de Produto" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtdeProduto" runat="server" Text=""></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ERRO" HeaderText="Erro" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="" HeaderText="" />
                                            <asp:TemplateField HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btBaixar" Text="Baixar" Width="80px" OnClick="btBaixar_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <fieldset>
                                <legend>Retirada em Loja</legend>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvFaturamentoLoja" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvFaturamento_RowDataBound"
                                        ShowFooter="true" DataKeyNames="PEDIDO">
                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litAbrirPedido" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PEDIDO_ORIGEM" HeaderText="Origem" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="PEDIDO" HeaderText="Pedido Linx" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="Data Pedido" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDataPedido" runat="server" Text=""></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CLIFOR" HeaderText="CLIFOR" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="CLIENTE_ATACADO" HeaderText="Cliente" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderText="Quantidade de Produto" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtdeProduto" runat="server" Text=""></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ERRO" HeaderText="Erro" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="" HeaderText="" />
                                            <asp:TemplateField HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btBaixar" Text="Baixar" Width="80px" OnClick="btBaixar_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <fieldset>
                                <legend>Marketplace</legend>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvFaturamentoMP" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvFaturamento_RowDataBound"
                                        ShowFooter="true" DataKeyNames="PEDIDO">
                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litAbrirPedido" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PEDIDO_ORIGEM" HeaderText="Origem" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="PEDIDO" HeaderText="Pedido Linx" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="Data Pedido" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDataPedido" runat="server" Text=""></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CLIFOR" HeaderText="CLIFOR" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="CLIENTE_ATACADO" HeaderText="Cliente" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderText="Quantidade de Produto" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtdeProduto" runat="server" Text=""></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ERRO" HeaderText="Erro" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="" HeaderText="" />
                                            <asp:TemplateField HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btBaixar" Text="Baixar" Width="80px" OnClick="btBaixar_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                </table>
            </div>
        </fieldset>
    </div>
</asp:Content>
