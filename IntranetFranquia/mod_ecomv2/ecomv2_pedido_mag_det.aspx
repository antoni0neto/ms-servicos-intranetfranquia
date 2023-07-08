<%@ Page Title="Vendas - Pedido" Language="C#" AutoEventWireup="true" CodeBehind="ecomv2_pedido_mag_det.aspx.cs"
    Inherits="Relatorios.ecomv2_pedido_mag_det" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Venda de Produtos Semana</title>
    <style type="text/css">
        .bodyMaterial {
            font-family: "Helvetica Neue", "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif !important;
            margin: 0px;
            padding: 0px;
            color: #696969;
        }

        .jGrowl .manilla {
            background-color: #000;
            color: red;
            font-weight: bold;
        }
    </style>
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            $("#tabs").tabs();

        });

        function MarcarAba(vAba) {
            document.getElementById("MainContent_hidTabSelected").value = vAba;
        }

    </script>

</head>
<body>
    <div class="main">
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="up1" runat="server">
                <ContentTemplate>
                    <div style="margin-top: 10px; padding-left: 12px; padding-right: 12px; padding-bottom: -350px; background-color: White;">
                        <br />
                        <div>
                            <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Clientes&nbsp;&nbsp;>&nbsp;&nbsp;Vendas - Pedido</span>
                            <div style="float: right; padding: 0;">
                                <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                            </div>
                        </div>
                        <hr />
                        <div>
                            <fieldset>
                                <legend>Vendas - Pedido</legend>
                                <asp:Panel ID="pnlFraude" runat="server" Visible="false">
                                    <asp:Label ID="litFraude" runat="server" Text="Este pedido é uma fraude!" ForeColor="Red" Font-Bold="true"></asp:Label>
                                    <hr />
                                </asp:Panel>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                    <tr>
                                        <td>Telefone 1</td>
                                        <td>Telefone 2</td>
                                        <td>Telefone 3</td>
                                        <td style="text-align: right;">Fraude</td>
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
                                        <td style="width: 270px;">
                                            <asp:TextBox ID="txtTelefone3WA" runat="server" Width="210px" Enabled="false"></asp:TextBox>&nbsp;
                                            <asp:ImageButton ID="btTelefone3WA" runat="server" Width="15px" ImageUrl="~/Image/whatsapp.png" ToolTip="Número 3" OnClick="btTelefoneWA_Click" />&nbsp;
                                            <asp:ImageButton ID="btTelefone3WABloq" runat="server" Width="15px" ImageUrl="~/Image/delete.png" ToolTip="Bloquear Número" OnClick="btTelefoneWABloq_Click" OnClientClick="javascript: return confirm('Tem certeza que deseja bloquear este número?');" />
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:ImageButton ID="btFraude" runat="server" Width="15px" ImageUrl="~/Image/clean.png" ToolTip="Marcar pedido como Fraude" OnClick="btFraude_Click" OnClientClick="javascript: return confirm('Tem certeza que deseja marcar este pedido como fraude?');" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <div id="tabs">
                                    <ul>
                                        <li><a href="#tabs-pedido" id="tabPedido" runat="server" onclick="MarcarAba(0);">Pedido</a></li>
                                        <li><a href="#tabs-endeti" id="tabEndEtiqueta" runat="server" onclick="MarcarAba(1);">Endereço Etiqueta</a></li>
                                        <li><a href="#tabs-pagarme" id="tabPagarme" runat="server" onclick="MarcarAba(2);">Pagamento</a></li>
                                        <li><a href="#tabs-estorno" id="tabEstorno" runat="server" onclick="MarcarAba(3);">Estorno</a></li>
                                        <li><a href="#tabs-cupom" id="tabCupom" runat="server" onclick="MarcarAba(4);">Cupom</a></li>
                                        <li><a href="#tabs-clipedidos" id="tabCliPedido" runat="server" onclick="MarcarAba(5);">Pedidos do Cliente</a></li>
                                    </ul>
                                    <div id="tabs-pedido">
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                            <tr>
                                                <td>Pedido Externo</td>
                                                <td>Pedido Linx</td>
                                                <td>Nome do Cliente</td>
                                                <td>Data</td>
                                            </tr>
                                            <tr>
                                                <td style="width: 160px;">
                                                    <asp:TextBox ID="txtPedidoExterno" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                                                </td>
                                                <td style="width: 160px;">
                                                    <asp:TextBox ID="txtPedido" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                                                </td>
                                                <td style="width: 310px;">
                                                    <asp:TextBox ID="txtNomeCliente" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtData" runat="server" Width="210px" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>CPF</td>
                                                <td>Sexo</td>
                                                <td>Email</td>
                                                <td>Data de Nascimento</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtCPF" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSexo" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEmail" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDataNascimento" runat="server" Width="210px" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">Status do Pedido</td>
                                                <td>Valor de Handclub</td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtStatusPedido" runat="server" Width="620px" Enabled="false"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtZerarPonto" runat="server" Width="185px" onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>&nbsp;
                                                    <asp:ImageButton ID="btZerarHC" runat="server" Width="15px" ImageUrl="~/Image/cancel.png" ToolTip="Apagar pontos Handclub" OnClick="btZerarHC_Click" OnClientClick="javascript: return confirm('Tem certeza que deseja zerar os pontos deste pedido?');" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Subtotal</td>
                                                <td>Frete</td>
                                                <td>Desconto</td>
                                                <td>Total</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtValorSubTotal" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValorFrete" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValorDesconto" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValorTotal" runat="server" Width="210px" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <fieldset>
                                                        <legend>Cobrança</legend>
                                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                                            <tr>
                                                                <td colspan="3">Nome</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:TextBox ID="txtNomeCob" runat="server" Width="810px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">Endereço</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" valign="middle">
                                                                    <asp:TextBox ID="txtEnderecoCob" runat="server" Width="780px" Enabled="false"></asp:TextBox>
                                                                    <asp:ImageButton ID="btAbrirGoogleMaps1" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Image/googlemaps.png" Width="25px" OnClick="btAbrirGoogleMaps1_Click" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Cidade</td>
                                                                <td>Estado</td>
                                                                <td>CEP</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 270px">
                                                                    <asp:TextBox ID="txtCidadeCob" runat="server" Width="260px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td style="width: 270px">
                                                                    <asp:TextBox ID="txtEstadoCob" runat="server" Width="260px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCEPCob" runat="server" Width="269px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Telefone 1</td>
                                                                <td colspan="2">Telefone 2</td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtTel1Cob" runat="server" Width="260px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:TextBox ID="txtTel2Cob" runat="server" Width="260px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <fieldset>
                                                        <legend>Entrega</legend>
                                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                                            <tr>
                                                                <td colspan="3">Nome</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:TextBox ID="txtNomeEnt" runat="server" Width="810px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">Endereço</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:TextBox ID="txtEnderecoEnt" runat="server" Width="780px" Enabled="false"></asp:TextBox>
                                                                    <asp:ImageButton ID="btAbrirGoogleMaps2" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Image/googlemaps.png" Width="25px" OnClick="btAbrirGoogleMaps2_Click" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Cidade</td>
                                                                <td>Estado</td>
                                                                <td>CEP</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 270px">
                                                                    <asp:TextBox ID="txtCidadeEnt" runat="server" Width="260px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td style="width: 270px">
                                                                    <asp:TextBox ID="txtEstadoEnt" runat="server" Width="260px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCEPEnt" runat="server" Width="269px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Telefone 1</td>
                                                                <td colspan="2">Telefone 2</td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtTel1Ent" runat="server" Width="260px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:TextBox ID="txtTel2Ent" runat="server" Width="260px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <fieldset>
                                                        <legend>Pagamento</legend>
                                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                                            <tr>
                                                                <td colspan="2">Método</td>
                                                                <td>Valor de Pagamento</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:TextBox ID="txtMetodo" runat="server" Width="590px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtValorPago" runat="server" Width="210px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Nome Cartão</td>
                                                                <td>Bandeira</td>
                                                                <td>Últimos Digitos</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 390px;">
                                                                    <asp:TextBox ID="txtNomeCartao" runat="server" Width="380px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td style="width: 210px;">
                                                                    <asp:TextBox ID="txtBandeira" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtUltDigito" runat="server" Width="210px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <fieldset>
                                                        <legend>Produtos</legend>
                                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                                            <tr>
                                                                <td>
                                                                    <div class="rounded_corners">
                                                                        <asp:GridView ID="gvProdutos" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvProdutos_RowDataBound" OnDataBound="gvProdutos_DataBound" ShowFooter="true"
                                                                            DataKeyNames="PEDIDO_EXTERNO">
                                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true"></FooterStyle>
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="45px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                                    <ItemTemplate>
                                                                                        <asp:Image ID="imgProduto" runat="server" Width="25px" Height="35px" ImageAlign="AbsMiddle" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:BoundField DataField="SKU" HeaderText="SKU" SortExpression="SKU" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                                                <asp:BoundField DataField="NOME" HeaderText="Nome" SortExpression="NOME" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                                                <asp:BoundField DataField="QTDE" HeaderText="Qtde" SortExpression="QTDE" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />

                                                                                <asp:TemplateField HeaderText="Preço" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px" SortExpression="PRECO"
                                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <asp:Literal ID="litPreco" runat="server"></asp:Literal>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Desconto" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px" SortExpression="PRECO"
                                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <asp:Literal ID="litDesconto" runat="server"></asp:Literal>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px" SortExpression="PRECO"
                                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal>
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
                                            <tr>
                                                <td colspan="4">
                                                    <fieldset>
                                                        <legend>Histórico</legend>
                                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                                            <tr>
                                                                <td>
                                                                    <div class="rounded_corners">
                                                                        <asp:GridView ID="gvHistorico" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvHistorico_RowDataBound"
                                                                            DataKeyNames="PEDIDO_EXTERNO">
                                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Data" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" SortExpression="DATA_CRIACAO"
                                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="170px"
                                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                                    <ItemTemplate>
                                                                                        <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Comentário" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width=""
                                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                                    <ItemTemplate>
                                                                                        <asp:Literal ID="litComentario" runat="server"></asp:Literal>
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
                                    <div id="tabs-endeti">
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                            <tr>
                                                <td colspan="3">CEP</td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtEtiCEP" runat="server" Enabled="true" MaxLength="9" Width="250px" onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">Endereco</td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtEtiEndereco" runat="server" MaxLength="90" Enabled="true" Width="100%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Número</td>
                                                <td colspan="2">Complemento</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtEtiNumero" runat="server" Enabled="true" MaxLength="10" Width="250px"></asp:TextBox>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtEtiComplemento" runat="server" Enabled="true" MaxLength="60" Width="100%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Bairro</td>
                                                <td>Cidade</td>
                                                <td>UF</td>
                                            </tr>
                                            <tr>
                                                <td style="width: 260px;">
                                                    <asp:TextBox ID="txtEtiBairro" runat="server" Enabled="true" MaxLength="25" Width="250px"></asp:TextBox>
                                                </td>
                                                <td style="width: 410px;">
                                                    <asp:TextBox ID="txtEtiCidade" runat="server" Enabled="true" MaxLength="35" Width="400px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEtiUF" runat="server" Enabled="true" MaxLength="2" Width="100%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Button ID="btSalvarEndEti" runat="server" Text="Salvar" Width="120px" OnClick="btSalvarEndEti_Click" />&nbsp;
                                                    <asp:Label ID="labErroEndEti" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="tabs-pagarme">
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                            <tr>
                                                <td>ID da Transação</td>
                                                <td>TID</td>
                                                <td>Data</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtPMIdTransacao" runat="server" Enabled="false" Width="270px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPMTID" runat="server" Enabled="false" Width="270px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPMData" runat="server" Enabled="false" Width="270px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Valor A Pagar</td>
                                                <td>Valor Pago</td>
                                                <td>Valor Estorno</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtPMValorPagar" runat="server" Enabled="false" Width="270px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPMValorPago" runat="server" Enabled="false" Width="270px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPMValorEstorno" runat="server" Enabled="false" Width="270px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Status Razão</td>
                                                <td>Motivo Recusa</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtPMStatusRazao" runat="server" Enabled="false" Width="270px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPMRecusa" runat="server" Enabled="false" Width="270px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">Status</td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtPMStatus" runat="server" Enabled="false" Width="838px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <fieldset>
                                                        <legend>Boleto</legend>
                                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                                            <tr>
                                                                <td colspan="2">URL Boleto</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:TextBox ID="txtPMBoletoURL" runat="server" Width="810px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Código de Barras do Boleto</td>
                                                                <td>Data de Vencimento</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 610px;">
                                                                    <asp:TextBox ID="txtPMBoletoCODBarra" runat="server" Width="600px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPMBoletoVencimento" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td colspan="3">
                                                    <fieldset>
                                                        <legend>Cartão de Crédito</legend>
                                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                                            <tr>
                                                                <td>Nome Cartão</td>
                                                                <td>Bandeira</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 410px;">
                                                                    <asp:TextBox ID="txtPMCCNome" runat="server" Width="400px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPMCCBandeira" runat="server" Width="400px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Número Cartão</td>
                                                                <td>Parcelas</td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtPMCCNumero" runat="server" Width="400px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPMCCParcela" runat="server" Width="400px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="tabs-estorno">
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                            <tr>
                                                <td>Subtotal</td>
                                                <td>Frete</td>
                                                <td>Desconto</td>
                                                <td>Total</td>
                                            </tr>
                                            <tr>
                                                <td style="width: 160px;">
                                                    <asp:TextBox ID="txtEstornoSubTotal" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                                                </td>
                                                <td style="width: 160px;">
                                                    <asp:TextBox ID="txtEstornoFrete" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                                                </td>
                                                <td style="width: 300px;">
                                                    <asp:TextBox ID="txtEstornoDesconto" runat="server" Width="290px" Enabled="false"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEstornoTotal" runat="server" Width="238px" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">Valor do Estorno</td>
                                                <td>Valor Ajuste Manutenção</td>
                                                <td>Valor Reembolsado</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtEstornoValor" runat="server" Enabled="true" Width="309px" onkeypress="return fnValidarNumeroDecimal(event);" OnTextChanged="txtEstornoValor_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEstornoValorAjuste" runat="server" Enabled="false" Width="290px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEstornoValReembolsado" runat="server" Enabled="false" Width="238px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">Nome</td>
                                                <td>CPF</td>
                                                <td>Tipo Conta</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtEstornoNome" runat="server" Enabled="true" MaxLength="200" Width="309px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEstornoCPF" runat="server" Enabled="true" MaxLength="14" Width="290px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEstornoTipoConta" runat="server" Enabled="true" Width="238px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">Banco</td>
                                                <td>Agência</td>
                                                <td>Número Conta</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtEstornoBanco" runat="server" Enabled="true" MaxLength="60" Width="309px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEstornoAgencia" runat="server" Enabled="true" MaxLength="50" Width="290px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEstornoNumeroConta" runat="server" Enabled="true" MaxLength="60" Width="238px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">Motivo</td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:TextBox ID="txtEstornoMotivo" runat="server" Enabled="true" Width="858px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">&nbsp;</td>
                                            </tr>
                                            <tr>

                                                <td colspan="3">
                                                    <asp:Button ID="btEstorno" runat="server" Width="200px" Text="Incluir Estorno" OnClick="btEstorno_Click" />&nbsp;&nbsp;
                                                    <asp:Label ID="labErroEstorno" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                                                </td>
                                                <td style="text-align: right;">
                                                    <asp:Button ID="btImprimir" runat="server" Width="111px" Text="Imprimir" OnClick="btImprimir_Click" />
                                                    &nbsp;
                                                    <asp:Button ID="btEnviarEmail" runat="server" Width="111px" Text="Email" OnClick="btEnviarEmail_Click" />
                                                </td>
                                            </tr>

                                        </table>
                                    </div>
                                    <div id="tabs-cupom">
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                            <tr>
                                                <td>Tipo de Cupom</td>
                                                <td>Frete Grátis</td>
                                                <td>Valor</td>
                                                <td>NF Entrada</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="width: 180px;">
                                                    <asp:DropDownList ID="ddlCupomTipo" runat="server" Width="174px" Height="22px">
                                                        <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                                        <asp:ListItem Value="T" Text="Troca"></asp:ListItem>
                                                        <asp:ListItem Value="J" Text="Unificar Pedido"></asp:ListItem>
                                                        <asp:ListItem Value="H" Text="Handclub"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 180px;">
                                                    <asp:DropDownList ID="ddlFreteGratis" runat="server" Width="174px" Height="22px">
                                                        <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="Sim"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="Não"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 200px;">
                                                    <asp:TextBox ID="txtCupomVal" runat="server" Enabled="true" Width="186px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                                </td>
                                                <td style="width: 180px;">
                                                    <asp:TextBox ID="txtNFEntrada" runat="server" Enabled="true" Width="166px" onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btGerarCupom" runat="server" Text="Gerar Cupom" Width="120px" OnClick="btGerarCupom_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <asp:Label ID="labCupomErro" runat="server" ForeColor="Red" Text="&nbsp;" Font-Bold="true"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvCupom" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvCupom_RowDataBound" OnDataBound="gvCupom_DataBound" ShowFooter="true"
                                                            DataKeyNames="PEDIDO_EXTERNO">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Bold="true"></FooterStyle>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Data Criação" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="165px" SortExpression="DATA_CRIACAO"
                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Cupom" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width=""
                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litCupom" runat="server" Text='<%# Bind("CUPOM") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Frete Grátis" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" SortExpression="FRETE_GRATIS"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litFreteGratis" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Valor Cupom" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" SortExpression="VALOR"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litValCupom" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="NF Entrada" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="" SortExpression="NF_ENTRADA"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litNFEntrada" runat="server" Text='<%# Bind("NF_ENTRADA") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Exclusão" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="165px"
                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litDataExc" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    ItemStyle-Width="25px">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btExcluirCupom" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/delete.png"
                                                                            OnClick="btExcluirCupom_Click" ToolTip="Excluir Cupom" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="tabs-clipedidos">
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                            <tr>
                                                <td>
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvPedidoCliente" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvPedidoCliente_RowDataBound" OnDataBound="gvPedidoCliente_DataBound" ShowFooter="true"
                                                            DataKeyNames="PEDIDO_EXTERNO">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Bold="true"></FooterStyle>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="PEDIDO_EXTERNO" HeaderText="Pedido" SortExpression="PEDIDO_EXTERNO" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                                <asp:TemplateField HeaderText="Data" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" SortExpression="DATA_CRIACAO"
                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Método de Pgto" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litMetodoPgto" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Valor Pago" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="135px" SortExpression="VALOR_PAGO"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litValPago" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width=""
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    ItemStyle-Width="25px">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btPesquisar" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/search.png"
                                                                            OnClick="btPesquisar_Click" ToolTip="Pesquisar" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </fieldset>
                            <br />
                            <br />
                        </div>
                        <asp:HiddenField ID="hidPedidoExterno" runat="server" Value="" />
                        <asp:HiddenField ID="hidTabSelected" runat="server" Value="" />
                        <br />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </form>
    </div>
</body>
</html>
