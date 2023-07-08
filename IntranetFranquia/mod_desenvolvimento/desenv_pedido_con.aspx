<%@ Page Title="Consultar Pedido" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="desenv_pedido_con.aspx.cs" Inherits="Relatorios.desenv_pedido_con"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita
        {
            text-align: right;
        }
        .alinhamento
        {
            position: relative;
            float: left;
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
                    de Produto&nbsp;&nbsp;>&nbsp;&nbsp;Consultar Pedido</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">
                        Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Pedido Consulta"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div style="width: 600px;" class="alinhamento">
                                <div style="width: 200px;" class="alinhamento">
                                    <label>
                                        <span style="color: Red;"></span>Data Início Entrega:&nbsp;
                                    </label>
                                    <asp:TextBox ID="txtDataInicio" runat="server" onkeypress="return fnValidarData(event);"
                                        Height="22px" MaxLength="10" Width="196px"></asp:TextBox>
                                    <asp:Calendar ID="CalendarDataInicio" runat="server" OnSelectionChanged="CalendarDataInicio_SelectionChanged"
                                        CaptionAlign="Bottom"></asp:Calendar>
                                </div>
                                <div style="width: 200px;" class="alinhamento">
                                    <label>
                                        <span style="color: Red;"></span>Data Fim Entrega:&nbsp;
                                    </label>
                                    <asp:TextBox ID="txtDataFim" runat="server" onkeypress="return fnValidarData(event);"
                                        Height="22px" MaxLength="10" Width="196px"></asp:TextBox>
                                    <asp:Calendar ID="CalendarDataFim" runat="server" OnSelectionChanged="CalendarDataFim_SelectionChanged"
                                        CaptionAlign="Bottom"></asp:Calendar>
                                </div>
                                <div style="width: 200px;" class="alinhamento">
                                    <label>
                                        <span style="color: Red;"></span>Número Pedido:&nbsp;
                                    </label>
                                    <asp:TextBox ID="txtPedidoNumero" runat="server" Width="120px" MaxLength="8" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                </div>
                                <p style="height: 13px">
                                    &nbsp;</p>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btBuscarPedido" runat="server" Text="Buscar Pedido" Width="100px"
                                OnClick="btBuscarPedido_Click" />&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="rounded_corners">
                                <asp:GridView ID="gvPedido" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvPedido_RowDataBound"
                                    ShowFooter="true" DataKeyNames="CODIGO">
                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="35px" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <img alt="" style="cursor: pointer" src="../Image/plus.png" width="18px" />
                                                <asp:Panel ID="pnlTecido" runat="server" Style="display: none" Width="100%">
                                                    <asp:GridView ID="gvProduto" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvProduto_RowDataBound"
                                                        Width="100%">
                                                        <HeaderStyle BackColor="LightSteelBlue" HorizontalAlign="Center"></HeaderStyle>
                                                        <RowStyle BorderColor="GradientActiveCaption" BorderWidth="1px" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="33px" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Coleção" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderStyle-Width="160px" ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColecao" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="GRUPO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px" HeaderText="Grupo"
                                                                HeaderStyle-Width="160px" />
                                                            <asp:BoundField DataField="MODELO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px" HeaderText="Modelo"
                                                                HeaderStyle-Width="170px" />
                                                            <asp:TemplateField HeaderText="Nome" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litNome" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="COR" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px" HeaderText="Cor"
                                                                HeaderStyle-Width="155px" />
                                                            <asp:BoundField DataField="QTDE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px" HeaderText="Qtde Varejo"
                                                                HeaderStyle-Width="115px" />
                                                            <asp:BoundField DataField="PRECO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px" HeaderText="Preço Varejo"
                                                                HeaderStyle-Width="115px" />
                                                            <asp:BoundField DataField="QTDE_ATACADO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px" HeaderText="Qtde Atacado"
                                                                HeaderStyle-Width="115px" />
                                                            <asp:BoundField DataField="PRECO_ATACADO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px" HeaderText="Preço Atacado"
                                                                HeaderStyle-Width="115px" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="NUMERO_PEDIDO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Número Pedido" HeaderStyle-Width="120px" />
                                        <asp:BoundField DataField="FORNECEDOR" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Fornecedor" HeaderStyle-Width="200px" />
                                        <asp:BoundField DataField="COR" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="170px" HeaderText="Cor" />
                                        <asp:BoundField DataField="DATA_PEDIDO" DataFormatString="{0:d}" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" HeaderText="Data do Pedido" HeaderStyle-Width="150px" />
                                        <asp:BoundField DataField="DATA_ENTREGA_PREV" DataFormatString="{0:d}" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" HeaderText="Previsão de Entrega" HeaderStyle-Width="150px" />
                                        <asp:TemplateField HeaderText="Qtde de Produto" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeProduto" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DATA_ENTREGA_REAL" DataFormatString="{0:d}" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" HeaderText="Data Entrega" HeaderStyle-Width="150px" />
                                        <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litStatus" runat="server"></asp:Literal>
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
