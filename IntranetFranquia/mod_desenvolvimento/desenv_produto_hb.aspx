<%@ Page Title="Relacionar HB" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="desenv_produto_hb.aspx.cs" Inherits="Relatorios.desenv_produto_hb"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita
        {
            text-align: right;
        }
    </style>
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Controle
                    de Produto&nbsp;&nbsp;>&nbsp;&nbsp;Relacionar HB</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Relacionar HB"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 30px;">
                            Número do Pedido
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 160px;">
                            <asp:TextBox ID="txtPedidoNumero" runat="server" Width="150px" MaxLength="8" CssClass="alinharDireita"
                                onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btPedidoFiltro" runat="server" Text="Buscar Pedido" Width="100px"
                                OnClick="btPedidoFiltro_Click" />&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="labPedido" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound"
                                    DataKeyNames="CODIGO">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="35px" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hidCodigoProduto" runat="server" Value='<%# Eval("CODIGO") %>' />
                                                <img alt="" style="cursor: pointer" src="../Image/plus.png" width="18px" />
                                                <asp:Panel ID="pnlHB" runat="server" Style="display: none" Width="100%">
                                                    <asp:GridView ID="gvHB" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvHB_RowDataBound"
                                                        Width="100%" DataKeyNames="CODIGO">
                                                        <HeaderStyle BackColor="LightSteelBlue" HorizontalAlign="Center"></HeaderStyle>
                                                        <RowStyle BorderColor="Gainsboro" BorderWidth="1px" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="35px" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-BorderColor="Gainsboro" ItemStyle-BorderWidth="1px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="HB" ItemStyle-BorderColor="Gainsboro" ItemStyle-BorderWidth="1px"
                                                                HeaderStyle-Width="149px" HeaderText="HB" ItemStyle-HorizontalAlign="Center" />
                                                            <asp:TemplateField HeaderText="Coleção" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderStyle-Width="171px" ItemStyle-BorderColor="Gainsboro" ItemStyle-BorderWidth="1px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColecao" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="GRUPO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                ItemStyle-BorderColor="Gainsboro" ItemStyle-BorderWidth="1px" HeaderText="Grupo"
                                                                HeaderStyle-Width="192px" />
                                                            <asp:BoundField DataField="NOME" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                ItemStyle-BorderColor="Gainsboro" ItemStyle-BorderWidth="1px" HeaderText="Nome"
                                                                HeaderStyle-Width="150px" />
                                                            <asp:TemplateField HeaderText="Cor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderStyle-Width="150px" ItemStyle-BorderColor="Gainsboro" ItemStyle-BorderWidth="1px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litCor" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Foto Peça" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="260px" ItemStyle-BorderColor="Gainsboro" ItemStyle-BorderWidth="1px">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgFotoPeca" Width="80" Height="130" runat="server" ImageAlign="Middle" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btRelacionar" runat="server" Height="19px" OnClientClick="return ConfirmarTecidoHB();"
                                                                        Text="Incluir" OnClick="btRelacionar_Click" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Número do Pedido" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="140px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litNumeroPedido" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Coleção" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="160px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColecao" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="GRUPO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Grupo" HeaderStyle-Width="180px" />
                                        <asp:BoundField DataField="MODELO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Modelo" HeaderStyle-Width="140px" />
                                        <asp:BoundField DataField="COR" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Cor" HeaderStyle-Width="140px" />
                                        <asp:BoundField DataField="QTDE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                            HeaderText="Qtde Varejo" HeaderStyle-Width="120px" />
                                        <asp:BoundField DataField="PRECO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                            HeaderText="Preço Varejo" HeaderStyle-Width="120px" />
                                        <asp:BoundField DataField="QTDE_ATACADO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                            HeaderText="Qtde Atacado" HeaderStyle-Width="120px" />
                                        <asp:BoundField DataField="PRECO_ATACADO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                            HeaderText="Preço Atacado" HeaderStyle-Width="120px" />
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
