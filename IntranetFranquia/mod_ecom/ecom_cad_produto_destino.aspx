<%@ Page Title="Destino do Pedido" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ecom_cad_produto_destino.aspx.cs" Inherits="Relatorios.ecom_cad_produto_destino" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">


    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>

    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>



    <script type="text/javascript">
        function openwindow(l) {
            window.open(l, "PEDIDOMAG_EXP", "menubar=1,resizable=0,width=1000,height=500");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Pedido&nbsp;&nbsp;>&nbsp;&nbsp;Destino do Pedido</span>
        <div style="float: right; padding: 0;">
            <a href="ecom_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div>
        <fieldset class="login">
            <legend>Destino do Pedido</legend>
            <div class="login">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td colspan="5">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvDestino" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvDestino_RowDataBound"
                                    ShowFooter="true" DataKeyNames="PEDIDO_EXTERNO">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="labColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litAbrirPedido" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btVoltar" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/voltar.png"
                                                    OnClick="btVoltar_Click" OnClientClick="return Confirmar('Deseja voltar este Pedido para a Aprovação?');" ToolTip="Voltar Pedido para a Aprovação" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PEDIDO_ORIGEM" HeaderText="Origem" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="PEDIDO" HeaderText="Pedido Linx" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Data Pedido" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litDataPedido" runat="server" Text=""></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CLIENTE_ATACADO" HeaderText="Cliente" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="FORMA_ENTREGA" HeaderText="Forma Entrega" HeaderStyle-Width="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField HeaderText="Qtde de Produto" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeProduto" runat="server" Text=""></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Aprovação" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litDataAprovacao" runat="server" Text=""></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Destino">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlDestino" runat="server" Width="100px">
                                                    <asp:ListItem Value="E" Text="Envio" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Value="F" Text="Ret. Loja"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Obs Retirada Loja" HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtObs" runat="server" TextMode="MultiLine" Width="250px" Height="50px" OnTextChanged="txtObs_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:Button runat="server" ID="btBaixar" Text="Baixar" Width="80px" OnClick="btBaixar_Click" />
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
