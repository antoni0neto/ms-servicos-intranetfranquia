<%@ Page Title="Expedição" Language="C#" AutoEventWireup="true" CodeBehind="ecom_cad_produto_expedicao_pop.aspx.cs"
    Inherits="Relatorios.ecom_cad_produto_expedicao_pop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Expedição</title>
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

    <script type="text/javascript">
        function doClick(buttonName, e) {
            //the purpose of this function is to allow the enter key to 
            //point to the correct button to click.
            var key;

            if (window.event)
                key = window.event.keyCode;     //IE
            else
                key = e.which;     //firefox

            if (key == 13) {
                //Get the button the user wants to have clicked
                var btn = document.getElementById(buttonName);
                if (btn != null) { //If we find the button click it
                    btn.click();
                    event.keyCode = 0
                }
            }
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('input').keypress(function (e) {
                var code = null;
                code = (e.keyCode ? e.keyCode : e.which);
                return (code == 13) ? false : true;
            });
        });
    </script>


</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="up1" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btGravar" />
            </Triggers>
            <ContentTemplate>
                <div style="margin-top: 10px; padding-left: 12px; padding-right: 12px; padding-bottom: -350px; background-color: White;">
                    <br />
                    <div>
                        <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Pedido&nbsp;&nbsp;>&nbsp;&nbsp;Expedição</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Expedição</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>Pedido
                                    </td>
                                    <td>Tipo</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width: 300px;">
                                        <asp:TextBox ID="txtPedido" runat="server" Width="290px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 300px;">
                                        <asp:TextBox ID="txtTipo" runat="server" Width="290px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <fieldset>
                                            <legend>Produtos do Pedido</legend>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvProdutoPedido" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                    Style="background: white" OnRowDataBound="gvProdutoPedido_RowDataBound" OnDataBound="gvProdutoPedido_DataBound"
                                                    ShowFooter="true">
                                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" Font-Bold="true" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="PRODUTO" HeaderText="Produto" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="DESC_PRODUTO" HeaderText="Nome" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="DESC_COR_PRODUTO" HeaderText="Cor" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="QTDE" HeaderText="Qtde" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">Código de Barras do Produto</td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtProdutoBarra" runat="server" Width="290px"></asp:TextBox>&nbsp&nbsp<asp:Button runat="server" ID="btGravar" Text=">>" OnClick="btGravar_Click" Width="40px" />&nbsp;&nbsp;
                                            <asp:Label ID="labErroBarra" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <fieldset>
                                            <legend>Produtos na Caixa</legend>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvProdutoCaixa" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                    Style="background: white" OnRowDataBound="gvProdutoCaixa_RowDataBound" OnDataBound="gvProdutoCaixa_DataBound"
                                                    ShowFooter="true">
                                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" Font-Bold="true" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="PRODUTO" HeaderText="Produto" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="DESC_PRODUTO" HeaderText="Nome" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="DESC_COR_PRODUTO" HeaderText="Cor" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="QTDE_CAIXA" HeaderText="Qtde" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TAMANHO_CAIXA" HeaderText="Tamanho" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <fieldset>
                                            <legend>Volumes</legend>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td>Caixa
                                                    </td>
                                                    <td>Peso KGs
                                                    </td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 300px;">
                                                        <asp:DropDownList ID="ddlCaixa" runat="server" Width="294px" DataValueField="CODIGO" DataTextField="DESCRICAO"></asp:DropDownList>
                                                    </td>
                                                    <td style="width: 300px;">
                                                        <asp:TextBox ID="txtPeso" runat="server" Width="290px" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btIncluirVolume" runat="server" Text="Incluir" OnClick="btIncluirVolume_Click" Width="100px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <div class="rounded_corners">
                                                            <asp:GridView ID="gvVolume" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                                Style="background: white" OnRowDataBound="gvVolume_RowDataBound" OnDataBound="gvVolume_DataBound"
                                                                ShowFooter="true">
                                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                                                <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" Font-Bold="true" />
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
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Button ID="btExcluir" runat="server" Text="Excluir" Width="80px" OnClick="btExcluir_Click" />
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
                                    <td colspan="3" style="text-align: right;">
                                        <asp:Button ID="btSalvar" runat="server" Width="200px" Text="Salvar" OnClientClick="DesabilitarBotao(this);" OnClick="btSalvar_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:HiddenField ID="hidPedido" runat="server" Value="" />
                                        <asp:HiddenField ID="hidQtdeProduto" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidQtdeProdutoCaixa" runat="server" Value="0" />
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
