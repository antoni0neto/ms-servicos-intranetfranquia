<%@ Page Title="Pré-pedido de Tecido (Pedidos)" Language="C#" MasterPageFile="~/Site.Master"
    CodeBehind="desenv_tripa_prepedido_sub.aspx.cs" Inherits="Relatorios.desenv_tripa_prepedido_sub" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .jGrowl .manilla {
            background-color: #000;
            color: white;
        }

        .alinharDireita {
            text-align: right;
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
    <script type="text/javascript" src="http://code.jquery.com/jquery.js"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Planejamento
            de Coleção Nacional&nbsp;&nbsp;>&nbsp;&nbsp;Pré-pedido de Tecido (Pedidos)</span>
        <div style="float: right; padding: 0;">
            <a href="desenv_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <fieldset style="background-color: White;">
        <legend>
            <asp:Label ID="labTitulo" runat="server" Text="Pré-pedido de Tecido (Pedidos)"></asp:Label>
        </legend>
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>
                    <asp:Label ID="labMarca" runat="server" Text="Marca"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="labColecao" runat="server" Text="Coleção"></asp:Label>
                </td>
                <td>Origem
                </td>
                <td>Griffe
                </td>
                <td>Tem Pedido?
                </td>
                <td>&nbsp;
                </td>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 150px;">
                    <asp:DropDownList ID="ddlMarca" runat="server" Width="144px" Height="21px" Enabled="false">
                        <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                        <asp:ListItem Value="HANDBOOK" Text="HANDBOOK" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="TULP" Text="TULP"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="width: 180px;">
                    <asp:DropDownList ID="ddlColecao" runat="server" Width="174px" Height="21px" DataTextField="DESC_COLECAO"
                        DataValueField="COLECAO" OnSelectedIndexChanged="ddlColecao_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td style="width: 197px;">
                    <asp:DropDownList ID="ddlOrigemFiltro" runat="server" Width="191px" Height="21px" DataTextField="DESCRICAO"
                        DataValueField="CODIGO">
                    </asp:DropDownList>
                </td>
                <td style="width: 160px;">
                    <asp:DropDownList ID="ddlGriffeFiltro" runat="server" Width="154px" Height="21px" DataTextField="GRIFFE"
                        DataValueField="GRIFFE">
                    </asp:DropDownList>
                </td>
                <td style="width: 130px;">
                    <asp:DropDownList ID="ddlTemPedido" runat="server" Width="124px" Height="21px">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                        <asp:ListItem Value="N" Text="Não" Selected="True"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />
                    &nbsp;
                    <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                </td>
                <td>&nbsp;
                </td>
            </tr>
        </table>
        <br />
        <br />
        <asp:Panel ID="pnlPrePedido" runat="server" Visible="false">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 29px;">&nbsp;
                    </td>
                    <td style="width: 79px;">
                        <asp:DropDownList ID="ddlFiltroProduto" runat="server" Width="79px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                    <td style="width: 153px;">
                        <asp:DropDownList ID="ddlFiltroTecido" runat="server" Width="153px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                    <td style="width: 143px;">
                        <asp:DropDownList ID="ddlFiltroFornecedor" runat="server" Width="143px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                    <td style="width: 122px;">
                        <asp:DropDownList ID="ddlFiltroCorFornecedor" runat="server" Width="122px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                    <td style="width: 113px;">
                        <asp:DropDownList ID="ddlFiltroCorLinx" runat="server" Width="113px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                    <td style="width: 104px;">
                        <asp:DropDownList ID="ddlFiltroGrupoProduto" runat="server" Width="104px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                    <td style="width: 80px;">&nbsp;
                    </td>
                    <td style="width: 60px;">&nbsp;
                    </td>
                    <td style="width: 65px;">&nbsp;
                    </td>
                    <td style="width: 64px;">&nbsp;
                    </td>
                    <td style="">&nbsp;
                    </td>
                    <td style="">&nbsp;
                    </td>
                    <td style="">&nbsp;
                    </td>
                    <td style="">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="15">
                        <div class="rounded_corners">
                            <asp:GridView ID="gvPrePedido" runat="server" Width="100%" AutoGenerateColumns="False"
                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvPrePedido_RowDataBound"
                                OnDataBound="gvPrePedido_DataBound" ShowFooter="true"
                                OnSorting="gvPrePedido_Sorting" AllowSorting="true" DataKeyNames="CODIGO, CODIGO_PREPEDIDOSUB">
                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                <FooterStyle BackColor="Gainsboro" Font-Bold="true" Font-Size="Small" />
                                <PagerStyle HorizontalAlign="Center" CssClass="pageStyl" />
                                <Columns>
                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                        <ItemTemplate>
                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Produto" DataField="PRODUTO" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" SortExpression="PRODUTO" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" FooterStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="Tecido" DataField="TECIDO" HeaderStyle-Width="150px" SortExpression="TECIDO" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                    <asp:BoundField HeaderText="Fornecedor" DataField="FORNECEDOR" HeaderStyle-Width="140px" SortExpression="FORNECEDOR" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                    <asp:BoundField HeaderText="Cor Fornecedor" DataField="COR_FORNECEDOR" HeaderStyle-Width="120px" SortExpression="COR_FORNECEDOR" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                    <asp:BoundField HeaderText="Cor Linx" DataField="DESC_COR" SortExpression="DESC_COR" HeaderStyle-Width="110px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                    <asp:BoundField HeaderText="Grupo Produto" DataField="GRUPO_PRODUTO" SortExpression="GRUPO_PRODUTO" HeaderStyle-Width="100px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                    <asp:BoundField HeaderText="Griffe" DataField="GRIFFE" SortExpression="GRIFFE" HeaderStyle-Width="80px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                    <asp:TemplateField HeaderText="Consumo" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                        SortExpression="CONSUMO">
                                        <ItemTemplate>
                                            <asp:Literal ID="litConsumo" runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Preço Tecido" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                        SortExpression="PRECO_TECIDO">
                                        <ItemTemplate>
                                            <asp:Literal ID="litPrecoTecido" runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tot Consumo" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                        SortExpression="CONSUMO_TOTAL">
                                        <ItemTemplate>
                                            <asp:Literal ID="litConsumoTotal" runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Val Tecido" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                        SortExpression="VALOR_TECIDO">
                                        <ItemTemplate>
                                            <asp:Literal ID="litValorTecido" runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Pedido" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                        SortExpression="NUMERO_PEDIDO">
                                        <ItemTemplate>
                                            <asp:Literal ID="litPedido" runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Status" DataField="STATUS_DESC" SortExpression="STATUS_DESC" HeaderStyle-Width="" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />

                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbAssociar" runat="server" Checked="false" OnCheckedChanged="cbAssociar_CheckedChanged" AutoPostBack="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btExcluir" runat="server" ImageUrl="~/Image/delete.png" Width="15px" OnClick="btExcluir_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="15">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="15">
                        <fieldset>
                            <legend>Pedidos</legend>

                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="labGrupo" runat="server" Text="Grupo"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labSubGrupo" runat="server" Text="SubGrupo"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labCor" runat="server" Text="Cor"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labCorFornecedor" runat="server" Text="Cor Fornecedor"></asp:Label>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 160px;">
                                        <asp:DropDownList ID="ddlMaterialGrupo" runat="server" Width="154px" Height="21px"
                                            DataTextField="GRUPO" DataValueField="GRUPO" OnSelectedIndexChanged="ddlMaterialGrupo_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 190px;">
                                        <asp:DropDownList ID="ddlMaterialSubGrupo" runat="server" Width="184px" Height="21px"
                                            DataTextField="SUBGRUPO" DataValueField="SUBGRUPO" OnSelectedIndexChanged="ddlMaterialSubGrupo_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 220px;">
                                        <asp:DropDownList ID="ddlCor" runat="server" Width="214px" Height="21px" DataTextField="DESC_COR"
                                            DataValueField="COR" OnSelectedIndexChanged="ddlCor_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 350px;">
                                        <asp:DropDownList ID="ddlCorFornecedor" runat="server" Width="344px" Height="21px"
                                            DataTextField="REFER_FABRICANTE" DataValueField="REFER_FABRICANTE">
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">Pedido</td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:DropDownList ID="ddlPedidoItem" runat="server" DataValueField="CODIGO" DataTextField="PEDIDO_ITEM_DESC" Width="564px"></asp:DropDownList>
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Label ID="labErroAss" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                                        <asp:Button ID="btAssociar" runat="server" Text="Salvar" Width="130px" OnClick="btAssociar_Click" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </fieldset>
</asp:Content>
