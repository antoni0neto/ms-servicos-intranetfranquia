<%@ Page Title="Expedição" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ecom_cad_produto_expedicao.aspx.cs" Inherits="Relatorios.ecom_cad_produto_expedicao" %>

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
        <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Pedido&nbsp;&nbsp;>&nbsp;&nbsp;Expedição</span>
        <div style="float: right; padding: 0;">
            <a href="ecom_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div>
        <fieldset class="login">
            <legend>Expedição</legend>
            <div class="login">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>Frete Pago</td>
                        <td>Operadora</td>
                        <td>Data Aprov Ini</td>
                        <td>Data Aprov Final</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 150px;">
                            <asp:DropDownList ID="ddlFretePago" runat="server" Width="144px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="F" Text="Sim"></asp:ListItem>
                                <asp:ListItem Value="C" Text="Não"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 150px;">
                            <asp:DropDownList ID="ddlCarrier" runat="server" Width="144px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="MANDAE" Text="Mandae"></asp:ListItem>
                                <asp:ListItem Value="CORREIO" Text="Correios"></asp:ListItem>
                                <asp:ListItem Value="LOGGI" Text="Loggi"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 184px;">
                            <asp:TextBox ID="txtDataIni" runat="server" autocomplete="off" Width="170px" onkeypress="return fnValidarData(event);"
                                MaxLength="10" Style="text-align: right;" />
                        </td>
                        <td style="width: 184px;">
                            <asp:TextBox ID="txtDataFim" runat="server" autocomplete="off" Width="170px" onkeypress="return fnValidarData(event);"
                                MaxLength="10" Style="text-align: right;" />
                        </td>
                        <td style="width: 160px;">
                            <asp:Button ID="btAtualizar" runat="server" Text="Atualizar" Width="150px" OnClick="btAtualizar_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btImprimir" runat="server" Text="Imprimir" Width="150px" OnClick="btImprimir_Click" />
                        </td>
                        <td style="text-align: right;">
                            <asp:Button ID="btImprimirPorProduto" runat="server" Text="Imprimir Produtos" Width="150px" OnClick="btImprimirPorProduto_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvExpedicao" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvExpedicao_RowDataBound"
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

                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btVoltar" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/voltar.png"
                                                    OnClick="btVoltar_Click" OnClientClick="return Confirmar('Deseja voltar este Pedido para a Aprovação?');" ToolTip="Voltar Pedido para a Aprovação" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PEDIDO_ORIGEM" HeaderText="Origem" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="PEDIDO" HeaderText="Pedido Linx" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Data Pedido" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center">
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
                                        <asp:TemplateField HeaderText="Aprovação" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litDataAprovacao" runat="server" Text=""></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:Button runat="server" ID="btProblema" Text="Problema" Width="80px" OnClick="btProblema_Click" />
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
                    <tr>
                        <td colspan="7">
                            <fieldset>
                                <legend>Problemas</legend>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvExpedicaoProblema" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvExpedicao_RowDataBound"
                                        ShowFooter="true" DataKeyNames="PEDIDO_EXTERNO">
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

                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btVoltar" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/voltar.png"
                                                        OnClick="btVoltar_Click" OnClientClick="return Confirmar('Deseja voltar este Pedido para a Aprovação?');" ToolTip="Voltar Pedido para a Aprovação" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PEDIDO_ORIGEM" HeaderText="Origem" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="PEDIDO" HeaderText="Pedido Linx" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="Data Pedido" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDataPedido" runat="server" Text=""></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CLIENTE_ATACADO" HeaderText="Cliente" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderText="Aprovação" HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDataAprovacao" runat="server" Text=""></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Problema" HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtProblema" runat="server" TextMode="MultiLine" Width="250px" Height="50px" OnTextChanged="txtProblema_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btOK" Text="Pedido OK" Width="80px" OnClick="btOK_Click" />
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
