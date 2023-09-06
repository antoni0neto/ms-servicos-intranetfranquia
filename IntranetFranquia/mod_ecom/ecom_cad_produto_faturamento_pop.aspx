<%@ Page Title="Faturamento" Language="C#" AutoEventWireup="true" CodeBehind="ecom_cad_produto_faturamento_pop.aspx.cs"
    Inherits="Relatorios.ecom_cad_produto_faturamento_pop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Faturamento</title>
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }
    </style>

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../js/js.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
                <div style="margin-top: 10px; padding-left: 12px; padding-right: 12px; padding-bottom: -350px; background-color: White;">
                    <br />
                    <div>
                        <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Pedido&nbsp;&nbsp;>&nbsp;&nbsp;Faturamento</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Faturamento</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>Pedido</td>
                                    <td>CLIFOR</td>
                                    <td colspan="2">Cliente</td>
                                    <td>Data Pedido</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtPedido" runat="server" Width="250px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCLIFOR" runat="server" Width="190px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td colspan="2" style="width: 400px;">
                                        <asp:TextBox ID="txtCliente" runat="server" Width="390px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDataPedido" runat="server" Width="280px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Pedido Externo</td>
                                    <td colspan="3">Transportadora</td>
                                    <td>Quantidade</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtPedidoExterno" runat="server" Width="250px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtTransportadora" runat="server" Width="590px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtQtde" runat="server" Width="280px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr>
                                    <td>Volume</td>
                                    <td>Peso KGs</td>
                                    <td>Tipo Frete</td>
                                    <td>Forma de Entrega</td>
                                    <td>Valor Frete</td>
                                </tr>
                                <tr>
                                    <td style="width: 260px;">
                                        <asp:TextBox ID="txtVolume" runat="server" Width="250px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 200px;">
                                        <asp:TextBox ID="txtPeso" runat="server" Width="190px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 160px;">
                                        <asp:TextBox ID="txtTipoFrete" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 240px;">
                                        <asp:DropDownList ID="ddlFormaEntrega" runat="server" Width="234px" Height="21px" Enabled="false">
                                            <asp:ListItem Value="PAC" Text="PAC"></asp:ListItem>
                                            <asp:ListItem Value="SEDEX" Text="SEDEX"></asp:ListItem>
                                            <asp:ListItem Value="Economico" Text="Econômico"></asp:ListItem>
                                            <asp:ListItem Value="Rapido" Text="Rápido"></asp:ListItem>
                                            <asp:ListItem Value="mandae_frete-mandae" Text="Frete Mandae"></asp:ListItem>
                                            <asp:ListItem Value="B2W" Text="B2W"></asp:ListItem>
                                            <asp:ListItem Value="L" Text="RET. LOJA"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValorFrete" runat="server" Width="280px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Rastreio B2W
                                    </td>
                                    <td colspan="4">Pedido no MP
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtRastreioMP" runat="server" Width="250px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td colspan="4">
                                        <asp:TextBox ID="txtPedidoMP" runat="server" Width="190px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">Obs Retirada Loja</td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtObsRetLoja" runat="server" TextMode="MultiLine" Height="50px" Width="100%" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvVolume" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                Style="background: white" OnRowDataBound="gvVolume_RowDataBound" OnDataBound="gvVolume_DataBound"
                                                ShowFooter="true">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Bold="true" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Caixa" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litCaixa" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Peso KGs" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litPesoKG" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Comprimento (CM)" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litComprimentoCM" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Largura (CM)" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litLarguraCM" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Altura (CM)" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litAlturaCM" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labNF" runat="server" Text="Nota Fiscal"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="labSerie" runat="server" Text="Série"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labEmissao" runat="server" Text="Emissão"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlFilial" Height="21px" Width="254px">
                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="1054" Text="HANDBOOK ONLINE          " Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="000029" Text="CD - LUGZY               "></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNF" runat="server" CssClass="alinharDireita" MaxLength="9" onkeypress="return fnValidarNumero(event);"
                                            Width="190px"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtSerie" runat="server" CssClass="alinharDireita" MaxLength="2" Text="20" onkeypress="return fnValidarNumero(event);"
                                            Width="390px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmissao" runat="server" MaxLength="10" Width="280px"
                                            onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">Mensagem para o Cliente</td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtMsgCliente" runat="server" TextMode="MultiLine" Height="50px" Width="100%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5" valign="middle" style="text-align: right;">
                                        <asp:CheckBox ID="cbGerarEtiqueta" runat="server" Checked="true" Text="Gerar Etiqueta" TextAlign="Right" />&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btSalvar" runat="server" Width="200px" Text="Salvar" OnClientClick="DesabilitarBotao(this);" OnClick="btSalvar_Click" />&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <br />
                </div>
                <div id="dialogPai" runat="server">
                    <div id="dialog" title="Mensagem" class="divPop">
                        <table border="0" width="100%">
                            <tr>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; color: red;">
                                    <strong>Aviso</strong>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; color: red;">BAIXADO COM SUCESSO
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
