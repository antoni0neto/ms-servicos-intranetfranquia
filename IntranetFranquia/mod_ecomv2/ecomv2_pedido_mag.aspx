<%@ Page Title="Vendas - Pedido" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="ecomv2_pedido_mag.aspx.cs" Inherits="Relatorios.ecomv2_pedido_mag"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }

        .corTD {
            background-color: #EFEFEF;
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
    <link href="../Styles/gridviewScroll.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/1.9.0/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.9.1/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../js/js.js"></script>
    <script type="text/javascript" src="../js/gridviewScroll.min.js"></script>

    <script type="text/javascript">

        function gridviewScroll() {

            gvAtacado = $('#MainContent_gvPedido').gridviewScroll({
                width: 1430,
                height: 450,
                railcolor: "#F0F0F0",
                barcolor: "#CDCDCD",
                barhovercolor: "#606060",
                bgcolor: "#F0F0F0",
                freezesize: 5,
                arrowsize: 30,
                varrowtopimg: "../Image/arrowvt.png",
                varrowbottomimg: "../Image/arrowvb.png",
                harrowleftimg: "../Image/arrowhl.png",
                harrowrightimg: "../Image/arrowhr.png",
                headerrowcount: 1,
                railsize: 16,
                barsize: 12
            });
        }

        function openwindow(l) {
            window.open(l, "PEDIDOMAG", "menubar=1,resizable=0,width=1000,height=700");
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Clientes&nbsp;&nbsp;>&nbsp;&nbsp;Vendas - Pedido</span>
                <div style="float: right; padding: 0;">
                    <a href="~/mod_ecom/ecom_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
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
                        <td>Método
                        </td>
                        <td>Val. Pago Inicial
                        </td>
                        <td>Val. Pago Fim
                        </td>
                        <td>Status Magento
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 120px;">
                            <asp:TextBox ID="txtPedido" runat="server" Width="106px" onkeypress="return fnValidarNumero(event);"
                                MaxLength="10" Style="text-align: right;" />
                        </td>
                        <td style="width: 120px;">
                            <asp:TextBox ID="txtDataIni" runat="server" autocomplete="off" Width="106px" onkeypress="return fnValidarData(event);"
                                MaxLength="10" Style="text-align: right;" />
                        </td>
                        <td style="width: 120px;">
                            <asp:TextBox ID="txtDataFim" runat="server" autocomplete="off" Width="106px" onkeypress="return fnValidarData(event);"
                                MaxLength="10" Style="text-align: right;" />
                        </td>
                        <td style="width: 170px;">
                            <asp:TextBox ID="txtNome" runat="server" Width="156px" MaxLength="30" />
                        </td>
                        <td style="width: 110px;">
                            <asp:TextBox ID="txtCPF" runat="server" Width="96px" MaxLength="11" />
                        </td>
                        <td style="width: 150px;">
                            <asp:TextBox ID="txtEmail" runat="server" Width="136px" MaxLength="50" />
                        </td>
                        <td style="width: 180px;">
                            <asp:DropDownList ID="ddlMetodo" runat="server" Width="174px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="B" Text="BOLETO"></asp:ListItem>
                                <asp:ListItem Value="C" Text="CARTÃO DE CRÉDITO"></asp:ListItem>
                                <asp:ListItem Value="W" Text="B2W"></asp:ListItem>
                                <asp:ListItem Value="F" Text="SEM COBRANÇA"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 130px;">
                            <asp:TextBox ID="txtValIni" runat="server" Width="116px" onkeypress="return fnValidarNumeroDecimal(event);"
                                MaxLength="10" Style="text-align: right;" />
                        </td>
                        <td style="width: 130px;">
                            <asp:TextBox ID="txtValFim" runat="server" Width="116px" onkeypress="return fnValidarNumeroDecimal(event);"
                                MaxLength="10" Style="text-align: right;" />
                        </td>
                        <td style="width: 190px;">
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="184px" Height="21px" DataValueField="CODIGO" DataTextField="DESCRICAO"></asp:DropDownList>
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>Fraude</td>
                        <td>CEP</td>
                        <td colspan="9">Cupom</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlFraude" runat="server" Width="114px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="S" Text="SIM"></asp:ListItem>
                                <asp:ListItem Value="N" Text="NÃO"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCEP" runat="server" Width="106px" onkeypress="return fnValidarNumero(event);"
                                MaxLength="10" Style="text-align: right;" />
                        </td>
                        <td colspan="9">
                            <asp:TextBox ID="txtCupom" runat="server" Width="106px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="11">
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="11">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="114px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />
                        </td>
                        <td colspan="10">
                            <asp:Button ID="btLimpar" runat="server" Text="Limpar" Width="114px" OnClick="btLimpar_Click" OnClientClick="DesabilitarBotao(this);" />
                        </td>
                    </tr>
                    <tr style="line-height: 20px;">
                        <td colspan="11">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="11">
                            <div class="rounded_corners">
                                <div style="text-align: left; color: red; float: left;">
                                    &nbsp;&nbsp;&nbsp;<i><%=gvPedido.Rows.Count%> Pedido(s)</i>
                                </div>
                                <div style="text-align: right; color: red; float: right;">
                                    Pedidos são atualizados a cada 5 minutos&nbsp;&nbsp;&nbsp;
                                </div>
                                <asp:GridView ID="gvPedido" runat="server" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white; width: 2800px; border-collapse: collapse;" OnRowDataBound="gvPedido_RowDataBound"
                                    OnDataBound="gvPedido_DataBound" ShowFooter="true"
                                    OnSorting="gvPedido_Sorting" AllowSorting="true">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Bold="true" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-Width="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="corTD">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="corTD">
                                            <ItemTemplate>
                                                <asp:Literal ID="litAbrirPedido" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="corTD">
                                            <ItemTemplate>
                                                <asp:Literal ID="litAbrirHistorico" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PEDIDO_EXTERNO" HeaderText="Pedido" SortExpression="PEDIDO_EXTERNO" ItemStyle-CssClass="corTD" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="CLIENTE" HeaderText="Cliente" SortExpression="CLIENTE" HeaderStyle-Width="210px" ItemStyle-Width="210px" ItemStyle-CssClass="corTD" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField DataField="TID" HeaderText="TID" SortExpression="TID" HeaderStyle-Width="90px" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Data" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="140px" ItemStyle-Width="140px" SortExpression="DATA_CRIACAO"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litData" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="METODO" HeaderText="Método" SortExpression="METODO" HeaderStyle-Width="160px" ItemStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="Val Subtotal" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" SortExpression="VALOR_SUBTOTAL"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValSubTotal" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Val Frete" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" SortExpression="VALOR_FRETE"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValFrete" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Val Pago" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" SortExpression="VALOR_PAGO"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValPago" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DESC_STATUS_MAGENTO" HeaderText="Status" SortExpression="DESC_STATUS_MAGENTO" HeaderStyle-Width="170px" />
                                        <asp:TemplateField HeaderText="CEP" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" SortExpression="CEP_ENTREGA"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCEP" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Endereço" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="800px" SortExpression="RUA_ENTREGA"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litEndereco" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cidade/Estado" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="" SortExpression="CIDADE_ENTREGA"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCidadeEstado" runat="server"></asp:Literal>
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
