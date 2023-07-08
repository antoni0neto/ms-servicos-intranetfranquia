<%@ Page Title="Baixar Pedido" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="desenv_pedido_baixa.aspx.cs" Inherits="Relatorios.desenv_pedido_baixa" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .jGrowl .manilla {
            color: black;
        }

        .jGrowl .redError {
            color: red;
        }

        .alinharDireita {
            text-align: right;
        }
    </style>
    <script src="http://code.jquery.com/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Controle
                    de Produto&nbsp;&nbsp;>&nbsp;&nbsp;Baixar Pedido</span>
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
                                <legend>Baixar Pedido</legend>
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
                                        <td colspan="2">
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvPedido" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvPedido_RowDataBound"
                                                    OnDataBound="gvPedido_DataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="NUMERO_PEDIDO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="Número Pedido" HeaderStyle-Width="105px" />
                                                        <asp:BoundField DataField="FORNECEDOR" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderText="Fornecedor" HeaderStyle-Width="" />
                                                        <asp:BoundField DataField="COR" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderText="Cor" HeaderStyle-Width="" />
                                                        <asp:BoundField DataField="DATA_PEDIDO" DataFormatString="{0:d}" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" HeaderText="Data do Pedido" HeaderStyle-Width="115px" />
                                                        <asp:BoundField DataField="DATA_ENTREGA_PREV" DataFormatString="{0:d}" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" HeaderText="Previsão de Entrega" HeaderStyle-Width="130px" />
                                                        <asp:TemplateField HeaderText="Qtde de Produto" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litQtdeProduto" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="QTDE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="Qtde Inicial" HeaderStyle-Width="110px" />
                                                        <asp:TemplateField HeaderText="Qtde em Casa" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litQtdeEmCasa" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Data Entrega" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDataBaixa" runat="server" Height="12px" onkeypress="return fnReadOnly(event);"
                                                                    MaxLength="10" Width="120px" Style="text-align: center;" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qtde Entregue" ItemStyle-HorizontalAlign="Center"
                                                            HeaderStyle-Width="120px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtQtdeEntregue" runat="server" Height="12px" onkeypress="return fnValidarNumeroDecimalNegativo(event);"
                                                                    MaxLength="10" Width="120px" Style="text-align: right;" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btAtualizarQtde" runat="server" Height="20px" Text="Atualizar" Width="80px"
                                                                    OnClick="btAtualizarQtde_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbBaixar" runat="server" AutoPostBack="false" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <label>
                                                    Número do Pedido</label><br />
                                                <asp:TextBox ID="txtPedidoNumeroVoltar" runat="server" Width="148px" onkeypress="return fnValidarNumero(event);"
                                                    MaxLength="10" Style="text-align: right;" />&nbsp;&nbsp;
                                                <asp:Button ID="btVoltarPedido" runat="server" Text="Voltar Pedido" OnClick="btVoltarPedido_Click"
                                                    Width="100px" />&nbsp;&nbsp;
                                                <asp:Label ID="labErroVoltar" runat="server" ForeColor="Red" Text=""></asp:Label>
                                            </div>
                                        </td>
                                        <td>
                                            <br />
                                            <div style="float: right;">
                                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btBaixar" runat="server" Text="Baixar" OnClick="btBaixar_Click" Width="100px"
                                                    Visible="false" />
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
