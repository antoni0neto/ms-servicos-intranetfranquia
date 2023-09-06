<%@ Page Title="Controle de Pedido" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="desenv_material_controlpedido.aspx.cs" Inherits="Relatorios.desenv_material_controlpedido" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
        }

        .alinharcentro {
            text-align: center;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../Image/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "../Image/plus.png");
            $(this).closest("tr").next().remove();
        });
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Tecidos&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Pedido</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Controle de Pedido</legend>
                    <br />
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">

                        <tr>
                            <td>Fornecedor</td>
                            <td>Coleção</td>
                            <td>Pedido</td>
                            <td>Grupo</td>
                            <td>SubGrupo</td>
                            <td>Status</td>
                            <td>Pagamento</td>
                        </tr>
                        <tr>
                            <td style="width: 280px;">
                                <asp:DropDownList ID="ddlFornecedor" runat="server" Width="274px" Height="21px"
                                    DataTextField="FORNECEDOR" DataValueField="FORNECEDOR">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 220px;">
                                <asp:DropDownList ID="ddlColecao" runat="server" Width="214px" Height="21px"
                                    DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 140px;">
                                <asp:TextBox ID="txtNumeroPedido" runat="server" Width="130px" CssClass="alinharDireita"></asp:TextBox>
                            </td>
                            <td style="width: 200px;">
                                <asp:DropDownList ID="ddlMaterialGrupo" runat="server" Width="194px" Height="21px"
                                    DataTextField="GRUPO" DataValueField="CODIGO_GRUPO" OnSelectedIndexChanged="ddlMaterialGrupo_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 230px;">
                                <asp:DropDownList ID="ddlMaterialSubGrupo" runat="server" Width="224px" Height="21px"
                                    DataTextField="SUBGRUPO" DataValueField="CODIGO_SUBGRUPO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 190px;">
                                <asp:DropDownList ID="ddlStatus" runat="server" Width="184px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="R" Text="Reserva"></asp:ListItem>
                                    <asp:ListItem Value="P" Text="Pedido"></asp:ListItem>
                                    <asp:ListItem Value="B" Text="Baixado"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPagamento" runat="server" Width="154px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Em Aberto"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Pago"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Sem Cadastro"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" OnClientClick="DesabilitarBotao(this);" />
                                &nbsp;&nbsp;<asp:Label ID="labMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvSubPedido" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvSubPedido_RowDataBound"
                                        OnDataBound="gvSubPedido_DataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pedido" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labNumeroSubPedido" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data Pedido" ItemStyle-Width="90px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labDataPedido" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fornecedor" ItemStyle-Width="" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labFornecedor" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Coleção" ItemStyle-Width="" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labColecao" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Tecido" ItemStyle-Width="" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labTecido" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Cor Fornecedor" ItemStyle-Width="" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labCorFornecedor" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Qtde" ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labQtde" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Custo" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labCusto" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Val. Pedido" ItemStyle-Width="105px" HeaderStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labValorTotal" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ent Inicial" ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labDataPedidoPrevisao" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Prev Final" ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labDataPrevFinal" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qtde Recebida" ItemStyle-Width="110px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labQtdeEntregue" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Pagamento" HeaderStyle-Width="140px" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litE1" runat="server" Text="<br />" />
                                                    <asp:Literal ID="litParc1" runat="server" Text="" />
                                                    <asp:Literal ID="litParc2" runat="server" Text="" />
                                                    <asp:Literal ID="litParc3" runat="server" Text="" />
                                                    <asp:Literal ID="litParc4" runat="server" Text="" />
                                                    <asp:Literal ID="litParc5" runat="server" Text="" />
                                                    <asp:Literal ID="litParc6" runat="server" Text="" />
                                                    <asp:Literal ID="litE2" runat="server" Text="<br />" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labStatus" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btEntrarQtde" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/update.png"
                                                        OnClick="btEntrarQtde_Click" ToolTip="Receber Pedido" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btBaixarSubPedido" runat="server" Height="15px" Width="15px"
                                                        ImageUrl="~/Image/approve.png" OnClick="btBaixarSubPedido_Click" OnClientClick="return Confirmar('Deseja realmente Fechar este Pedido?');"
                                                        ToolTip="Fechar Pedido" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
