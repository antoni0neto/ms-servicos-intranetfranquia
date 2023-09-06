<%@ Page Title="Produção Brasil Atacado" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="desenv_rel_geral_atacado.aspx.cs" Inherits="Relatorios.desenv_rel_geral_atacado"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Produção
                    Brasil Atacado</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Produção Brasil Atacado"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 160px;" valign="top" colspan="14">Coleção
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 210px;" valign="top" colspan="7">
                            <asp:DropDownList ID="ddlColecoesBuscar" runat="server" Width="200px" Height="21px"
                                DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                            </asp:DropDownList>
                            &nbsp;
                            <asp:Button ID="btFiltroStatus" runat="server" Text="Buscar" Width="100px" OnClick="btFiltroStatus_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="labStatus" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                        <td colspan="7">
                            &nbsp;
                            &nbsp;
                            <asp:DropDownList ID="ddlStatusResumo" runat="server" Width="254px" Height="21px"
                                OnSelectedIndexChanged="ddlStatusResumo_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="14">&nbsp;
                        </td>
                    </tr>
                    <asp:Panel ID="pnlRelatorio" runat="server" Visible="false">
                        <tr>
                            <td colspan="14" valign="top">
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 650px;" valign="top">
                                            <table border="0" width="100%" style="height: 100%;">
                                                <tr>
                                                    <td style="width: 130px;">
                                                        <asp:Label ID="labBudTotal" runat="server" ToolTip="Total do Budget de toda a coleção."
                                                            Text="Budget Total:" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="labTotalBudget" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="labBudImportacao" runat="server" ToolTip="Total do Budget de Produtos Importados."
                                                            Text="Importação:" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="labTotalImportacao" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="labBudNacional" runat="server" ToolTip="Total do Budget de Produtos Nacionais."
                                                            Text="Nacional:" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="labTotalNacional" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="labBudFinal" runat="server" ToolTip="Sobra do Budget Nacional." Text="Final:"
                                                            Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="labValorFinal" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="line-height: 30px;">&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="labBudFiltroTotal" runat="server" ToolTip="Valor total do filtro selecionado."
                                                            Text="Valor Total Filtro:" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="labValorTotalFiltro" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="labBudFiltroqtde" runat="server" ToolTip="Quantidade total do filtro selecionado."
                                                            Text="Qtde Total Filtro:" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="labQtdeTotalFiltro" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td valign="top">
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td valign="top">
                                                        <div class="rounded_corners">
                                                            <asp:GridView ID="gvResumo" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvResumo_RowDataBound"
                                                                OnDataBound="gvResumo_DataBound" ShowFooter="true">
                                                                <HeaderStyle BackColor="Gainsboro" />
                                                                <FooterStyle BackColor="Gainsboro" Font-Bold="true" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="GRUPO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Grupo" HeaderStyle-Width="160px" />
                                                                    <asp:BoundField DataField="SKU" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                                        HeaderText="SKU" HeaderStyle-Width="130px" />
                                                                    <asp:BoundField DataField="QTDE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                                        HeaderText="Quantidade" HeaderStyle-Width="140px" />
                                                                    <asp:TemplateField HeaderText="Valor Total" ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litValorTotal" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="14">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="14">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 28px;">&nbsp;
                            </td>
                            <td style="width: 30px;">&nbsp;
                            </td>
                            <td style="width: 119px;">
                                <asp:DropDownList ID="ddlOrigem" runat="server" Width="119px" Height="21px" DataTextField="DESCRICAO"
                                    DataValueField="CODIGO" OnSelectedIndexChanged="ddlOrigem_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 126px;">
                                <asp:DropDownList ID="ddlGrupo" runat="server" Width="126px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                    DataValueField="GRUPO_PRODUTO" OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 85px;">&nbsp;
                            </td>
                            <td style="width: 136px;">&nbsp;
                            </td>
                            <td style="width: 131px;">&nbsp;
                            </td>
                            <td style="width: 135px;">&nbsp;
                            </td>
                            <td style="width: 128px;">
                                <asp:DropDownList ID="ddlNumeroPedido" runat="server" Width="128px" Height="21px"
                                    DataTextField="NUMERO_PEDIDO" DataValueField="NUMERO_PEDIDO" OnSelectedIndexChanged="ddlNumeroPedido_SelectedIndexChanged"
                                    AutoPostBack="true" Visible="false">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 91px;">&nbsp;
                            </td>
                            <td style="width: 101px;">&nbsp;
                            </td>
                            <td style="width: 90px;">&nbsp;
                            </td>
                            <td style="width: 95px;">&nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" Width="180px" Height="21px" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="14">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvGeral" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ShowFooter="true" ForeColor="#333333" Style="background: white" OnDataBound="gvGeral_DataBound"
                                        OnRowDataBound="gvGeral_RowDataBound" AllowSorting="true" OnSorting="gvGeral_Sorting">
                                        <HeaderStyle BackColor="Gainsboro" />
                                        <FooterStyle BackColor="Gainsboro" Font-Bold="true" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>
                                                    <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../Image/plus.png"
                                                        Width="15px" runat="server" />
                                                    <asp:Panel ID="pnlFoto" runat="server" Style="display: none" Width="100%">
                                                        <asp:GridView ID="gvFoto" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvFoto_RowDataBound"
                                                            Width="100%" DataKeyNames="CODIGO">
                                                            <HeaderStyle BackColor="LightSteelBlue" HorizontalAlign="Center"></HeaderStyle>
                                                            <RowStyle BorderColor="Gainsboro" BorderWidth="1px" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Foto Peça" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    ItemStyle-BorderColor="Gainsboro" ItemStyle-BorderWidth="1px">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="imgFotoPeca" runat="server" ImageAlign="Middle" />
                                                                        <asp:Image ID="imgFotoPeca2" runat="server" ImageAlign="Middle" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ORIGEM" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Origem" HeaderStyle-Width="115px" SortExpression="ORIGEM" />
                                            <asp:BoundField DataField="GRUPO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Grupo" HeaderStyle-Width="123px" SortExpression="GRUPO" />
                                            <asp:BoundField DataField="MODELO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Modelo" HeaderStyle-Width="115px" SortExpression="MODELO" />
                                            <asp:BoundField DataField="NOME" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Nome" HeaderStyle-Width="165px" SortExpression="NOME" />
                                            <asp:BoundField DataField="COR" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Cor" HeaderStyle-Width="130px" SortExpression="COR" />
                                            <asp:BoundField DataField="QTDE_ATACADO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Qtde Planejada" HeaderStyle-Width="130px" SortExpression="QTDE_ATACADO" />

                                            <asp:BoundField DataField="QTDE_ATACADO_LIB" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Qtde Liberada" HeaderStyle-Width="130px" Visible="false" />

                                            <%--                           <asp:BoundField DataField="NUMERO_PEDIDO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="No. do Pedido" HeaderStyle-Width="125px" SortExpression="NUMEROPEDIDO" />
                                            <asp:BoundField DataField="HB" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="HB" HeaderStyle-Width="75px" SortExpression="HB" />--%>
                                            <asp:TemplateField HeaderText="Grade Real" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="130px" SortExpression="GRADE_REAL">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litGradeReal" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Preço" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="130px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPreco" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valor Total" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="130px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValorTotal" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                SortExpression="STATUS">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </asp:Panel>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
