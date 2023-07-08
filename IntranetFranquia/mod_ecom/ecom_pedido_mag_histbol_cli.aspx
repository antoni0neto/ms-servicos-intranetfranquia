<%@ Page Title="Vendas - Pedido Histórico" Language="C#" AutoEventWireup="true" CodeBehind="ecom_pedido_mag_histbol_cli.aspx.cs"
    Inherits="Relatorios.ecom_pedido_mag_histbol_cli" %>

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
                            <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Clientes&nbsp;&nbsp;>&nbsp;&nbsp;Vendas - Pedido Histórico</span>
                            <div style="float: right; padding: 0;">
                                <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                            </div>
                        </div>
                        <hr />
                        <div>
                            <fieldset>
                                <legend>Vendas - Pedido Histórico</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                    <tr>
                                        <td>Pedido Externo</td>
                                        <td>Cliente</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 160px;">
                                            <asp:TextBox ID="txtPedidoExterno" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNomeCliente" runat="server" Width="740px" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <div id="tabs">
                                    <ul>
                                        <li><a href="#tabs-historico" id="tabHistPedido" runat="server" onclick="MarcarAba(0);">Histórico</a></li>
                                        <li><a href="#tabs-addcoment" id="tabComment" runat="server" onclick="MarcarAba(1);">Incluir Comentário</a></li>
                                    </ul>
                                    <div id="tabs-historico">
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                            <tr>
                                                <td>
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvPedidoHist" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvPedidoHist_RowDataBound" OnDataBound="gvPedidoHist_DataBound"
                                                            DataKeyNames="PEDIDO_EXTERNO">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Data" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Usuário" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="155px"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litUsuario" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="170px"
                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Comentário" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litComentario" runat="server" Text='<%# Bind("COMENTARIO") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>

                                    </div>
                                    <div id="tabs-addcoment">
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                            <tr>
                                                <td>Motivo
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="ddlMotivo" runat="server" Width="300px" DataValueField="CODIGO" DataTextField="MOTIVO"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Comentário
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtComentario" runat="server" TextMode="MultiLine" Height="200px" Width="856px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Button ID="btAdicionar" runat="server" Width="100px" Text="Salvar" OnClick="btAdicionar_Click" />&nbsp;
                                                    <asp:Label ID="labErro" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
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
