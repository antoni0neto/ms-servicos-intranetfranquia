<%@ Page Title="Comparativo Vendas - Colecao X Outlet" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="ecom_venda_compara_col_outlet.aspx.cs" Inherits="Relatorios.ecom_venda_compara_col_outlet"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>

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


    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Comparativo Vendas - Colecao X Outlet</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Comparativo Vendas - Colecao X Outlet"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Data Pedido Inicial
                        </td>
                        <td>Data Pedido Final
                        </td>
                        <td>Griffe
                        </td>
                        <td>Grupo Produto
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 180px;">
                            <asp:TextBox ID="txtDataIni" runat="server" autocomplete="off" Width="170px" onkeypress="return fnValidarData(event);"
                                MaxLength="10" Style="text-align: right;" />
                        </td>
                        <td style="width: 180px;">
                            <asp:TextBox ID="txtDataFim" runat="server" autocomplete="off" Width="170px" onkeypress="return fnValidarData(event);"
                                MaxLength="10" Style="text-align: right;" />
                        </td>
                        <td style="width: 200px;">
                            <asp:DropDownList ID="ddlGriffe" runat="server" Width="194px" Height="21px" DataTextField="GRIFFE"
                                DataValueField="GRIFFE">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlGrupo" runat="server" Width="250px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                DataValueField="GRUPO_PRODUTO">
                            </asp:DropDownList>
                        </td>
                        <td>&nbsp;
                        </td>
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
                    <tr>
                        <td colspan="5">
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr style="line-height: 20px;">
                        <td colspan="5">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound"
                                    OnDataBound="gvProduto_DataBound" ShowFooter="true">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Bold="true" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../Image/plus.png"
                                                    Width="15px" runat="server" />
                                                <asp:Panel ID="pnlVendas" runat="server" Style="display: none" Width="100%">
                                                    <asp:GridView ID="gvProdutoGrupo" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvProdutoGrupo_RowDataBound" OnDataBound="gvProdutoGrupo_DataBound" ShowFooter="true"
                                                        Width="100%">
                                                        <HeaderStyle BackColor="LightSteelBlue" HorizontalAlign="Left"></HeaderStyle>
                                                        <RowStyle BorderColor="Gainsboro" BorderWidth="1px" />
                                                        <Columns>

                                                            <asp:BoundField DataField="GRUPO_PRODUTO" HeaderText="Grupo Produto" SortExpression="GRUPO_PRODUTO" />

                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="135px" SortExpression="QTDE_OUTLET"
                                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="Beige">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litQtdeOutlet" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="135px" SortExpression="VALOR_OUTLET"
                                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="Beige">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litValorOutlet" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="135px" SortExpression="QTDE_COLECAO"
                                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="LightCyan">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litQtdeColecao" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="135px" SortExpression="VALOR_COLECAO"
                                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="LightCyan">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litValorColecao" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="135px" SortExpression="QTDE_TOTAL"
                                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="LavenderBlush">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litQtdeTotal" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="135px" SortExpression="VALOR_TOTAL"
                                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="LavenderBlush">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litValorTotal" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="135px" SortExpression="PORC_OUTLET" HeaderStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litPorcOutlet" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="135px" SortExpression="PORC_COLECAO" HeaderStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litPorcColecao" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="GRIFFE" HeaderText="Griffe" SortExpression="GRIFFE" />

                                        <asp:TemplateField HeaderText="Qtde Outlet" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="135px" SortExpression="QTDE_OUTLET"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="Beige">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeOutlet" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Valor Outlet" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="135px" SortExpression="VALOR_OUTLET"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="Beige">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValorOutlet" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Qtde Coleção" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="135px" SortExpression="QTDE_COLECAO"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="LightCyan">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeColecao" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Valor Coleção" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="135px" SortExpression="VALOR_COLECAO"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="LightCyan">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValorColecao" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Qtde Total" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="135px" SortExpression="QTDE_TOTAL"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="LavenderBlush">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeTotal" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Valor Total" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="135px" SortExpression="VALOR_TOTAL"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="LavenderBlush">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValorTotal" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Outlet (%)" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="135px" SortExpression="PORC_OUTLET" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litPorcOutlet" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Coleção (%)" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="135px" SortExpression="PORC_COLECAO" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litPorcColecao" runat="server"></asp:Literal>
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
