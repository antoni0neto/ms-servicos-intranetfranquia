<%@ Page Title="Estoque (Cartelinha)" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master"
    CodeBehind="prod_con_tecido_estoque_31122015.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="Relatorios.prod_con_tecido_estoque_31122015" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }

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
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
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
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Ordem
                    de Corte&nbsp;&nbsp;>&nbsp;&nbsp;Estoque (Cartelinha)</span>
                <div style="float: right; padding: 0;">
                    <a href="prod_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <br />
            <div style="width: 100%;">
                <fieldset>
                    <legend>Estoque (Cartelinha)</legend>
                    <div style="padding-left: 0px;">
                        <asp:ImageButton ID="ibtPesquisar" runat="server" Width="19px" ImageUrl="~/Image/search.png"
                            ToolTip="Pesquisar Materiais" OnClick="ibtPesquisar_Click" OnClientClick="DesabilitarBotao(this);" />
                        <asp:ImageButton ID="ibtLimpar" runat="server" Width="18px" ImageUrl="~/Image/clean.png"
                            ToolTip="Limpar" OnClick="ibtLimpar_Click" />
                        <asp:HiddenField ID="hidTabSelected" runat="server" Value="" />
                        <div style="text-align: right; float: right;">
                            <asp:Label ID="labErro" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                    <hr />
                    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                        <tr>
                            <td colspan="6">
                                <asp:Label ID="labNumeroPedido" runat="server" Text="Número do Pedido"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:TextBox ID="txtPedidoNumero" runat="server" Width="150px" Height="15px" onkeypress="return fnValidarNumero(event);"
                                    CssClass="alinharDireita"></asp:TextBox>
                            </td>
                            <td style="text-align: right;">
                                <asp:ImageButton ID="ibtImprimirNovo" runat="server" Width="18px" ImageUrl="~/Image/print.png"
                                    ToolTip="Imprimir Novo" OnClick="ibtImprimirNovo_Click" Visible="false" />
                            </td>
                        </tr>
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
                            <td>
                                <asp:Label ID="labFornecedor" runat="server" Text="Fornecedor"></asp:Label>
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
                            <td style="width: 220px;">
                                <asp:DropDownList ID="ddlMaterialSubGrupo" runat="server" Width="214px" Height="21px"
                                    DataTextField="SUBGRUPO" DataValueField="SUBGRUPO" OnSelectedIndexChanged="ddlMaterialSubGrupo_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 200px;">
                                <asp:DropDownList ID="ddlCor" runat="server" Width="194px" Height="21px" DataTextField="DESC_COR"
                                    DataValueField="COR" OnSelectedIndexChanged="ddlCor_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 350px;">
                                <asp:DropDownList ID="ddlCorFornecedor" runat="server" Width="344px" Height="21px"
                                    DataTextField="COR_FORNECEDOR" DataValueField="COR_FORNECEDOR" OnSelectedIndexChanged="ddlCorFornecedor_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 260px;">
                                <asp:DropDownList ID="ddlFornecedor" runat="server" Width="254px" Height="21px" DataTextField="FORNECEDOR"
                                    DataValueField="FORNECEDOR">
                                </asp:DropDownList>
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div id="tabs">
                        <ul>
                            <li><a href="#tabs-cartelinha" id="tabCartelinha" runat="server" onclick="MarcarAba(0);">Cartelinha</a></li>
                            <li><a href="#tabs-pedidos" id="tabPedidos" runat="server" onclick="MarcarAba(1);">Pedidos</a></li>
                        </ul>
                        <div id="tabs-cartelinha">
                            <div class="bodyMaterial">
                                <fieldset>
                                    <legend>Tecido</legend>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                        <tr>
                                            <td>Pedido
                                            </td>
                                            <td>Grupo
                                            </td>
                                            <td>SubGrupo
                                            </td>
                                            <td>Cor
                                            </td>
                                            <td>Cor Fornecedor
                                            </td>
                                            <td>Fornecedor
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 150px;">
                                                <asp:TextBox ID="txtPedidoLeitura" runat="server" CssClass="alinharDireita" Text="" Width="140px" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td style="width: 210px;">
                                                <asp:TextBox ID="txtGrupo" runat="server" Text="" Width="200px" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td style="width: 250px;">
                                                <asp:TextBox ID="txtSubGrupo" runat="server" Text="" Width="240px" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td style="width: 210px;">
                                                <asp:TextBox ID="txtCor" runat="server" Text="" Width="200px" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td style="width: 295px;">
                                                <asp:TextBox ID="txtCorFornecedor" runat="server" Text="" Width="285px" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFornecedor" runat="server" Text="" Width="240px" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Preço
                                            </td>
                                            <td colspan="5">Medida
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtPreco" runat="server" CssClass="alinharDireita" Text="" Width="140px" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td colspan="5">
                                                <asp:TextBox ID="txtMedida" runat="server" Text="" Width="200px" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="0" width="100%">
                                        <tr>
                                            <td valign="top" style="width: 660px;">
                                                <fieldset style="margin-top: 0px; padding-top: 0px; width: 650px">
                                                    <legend>
                                                        <asp:Label ID="labComposicao" runat="server" Text="Composição"></asp:Label></legend>
                                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <div class="rounded_corners">
                                                                    <asp:GridView ID="gvComposicao" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                        ForeColor="#333333" Style="background: white">
                                                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="QTDE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                                HeaderText="Quantidade" HeaderStyle-Width="220px" />
                                                                            <asp:BoundField DataField="DESCRICAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                                HeaderText="Descrição" />
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td valign="top">
                                                <fieldset style="margin-top: -1px; padding-top: 0;">
                                                    <legend>
                                                        <asp:Label ID="labFotoTecido" runat="server" Text="Foto"></asp:Label></legend>
                                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td align="center" style="text-align: center;">
                                                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td style="" align="center">
                                                                            <asp:Image ID="imgFotoTecido" runat="server" Width='150px' Height='200px' BorderColor="Black"
                                                                                BorderWidth="0" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <fieldset>
                                    <legend>Entradas/Saídas</legend>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvEntradas" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        OnRowDataBound="gvEntradas_RowDataBound" OnDataBound="gvEntradas_DataBound"
                                                        ForeColor="#333333" ShowFooter="true" Style="background: white">
                                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Nota Fiscal" HeaderStyle-Width="180px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litNF" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Data" HeaderStyle-Width="180px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litDataEntrada" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Quantidade" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litQtde" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Medida" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litMedida" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ibtImprimir" runat="server" Width="15px" ImageUrl="~/Image/print.png"
                                                                        ToolTip="Imprimir" OnClick="ibtImprimir_Click" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                                <asp:HiddenField ID="hidQtdeEntrada" runat="server" Value="" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <fieldset>
                                    <legend>HB</legend>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="margin-top: -12px;">
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td colspan="7">Quantidade Estoque
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="7">
                                                            <asp:TextBox ID="txtQtdeEstoque" runat="server" CssClass="alinharDireita" Text="" Width="140px" Enabled="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="labProduto" runat="server" Text="Produto"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="labProdutoNome" runat="server" Text="Nome"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="labPecas" runat="server" Text="Peças"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="labFolhas" runat="server" Text="G. por Folha"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="labGasto" runat="server" Text="GastoTotal"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="labEstoque" runat="server" Text="Sobra Estoque"></asp:Label>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 150px;">
                                                            <asp:TextBox ID="txtProduto" runat="server" CssClass="alinharDireita" Text="" Width="140px" onkeypress="return fnValidarNumero(event);"
                                                                OnTextChanged="txtProduto_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 190px;">
                                                            <asp:TextBox ID="txtProdutoNome" runat="server" Width="180px" Enabled="false"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 160px;">
                                                            <asp:TextBox ID="txtPecas" runat="server" CssClass="alinharDireita" Text="" Width="150px"
                                                                onkeypress="return fnValidarNumero(event);" OnTextChanged="CalcularSobraEstoque" AutoPostBack="true"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 160px;">
                                                            <asp:TextBox ID="txtFolhas" runat="server" CssClass="alinharDireita" Text="" Width="150px"
                                                                onkeypress="return fnValidarNumeroDecimal(event);" OnTextChanged="CalcularSobraEstoque" AutoPostBack="true"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 160px;">
                                                            <asp:TextBox ID="txtGasto" runat="server" CssClass="alinharDireita" Text="" Width="150px"
                                                                onkeypress="return fnValidarNumeroDecimal(event);" OnTextChanged="CalcularSobraEstoque" AutoPostBack="true"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 161px;">
                                                            <asp:TextBox ID="txtEstoque" runat="server" CssClass="alinharDireita" Text="" Width="150px"
                                                                onkeypress="return fnValidarNumeroDecimal(event);" Enabled="false"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btIncluirSimulacao" runat="server" Width="120px" Text="Incluir" OnClick="btIncluirSimulacao_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="labErroSimulacao" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvHB" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        OnRowDataBound="gvHB_RowDataBound" OnDataBound="gvHB_DataBound" OnSorting="gvHB_Sorting" AllowSorting="true"
                                                        ForeColor="#333333" ShowFooter="true" Style="background: white">
                                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Data" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                SortExpression="DATA">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Coleção" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                SortExpression="DESC_COLECAO">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColecao" runat="server" Text='<%# Bind("DESC_COLECAO") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="HB" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                SortExpression="HB">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litHB" runat="server" Text='<%# Bind("HB") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Detalhe" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                SortExpression="DETALHE">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litDetalhe" runat="server" Text='<%# Bind("DETALHE") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Mostruário" HeaderStyle-Width="105px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                SortExpression="MOSTRUARIO">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litMostruario" runat="server" Text='<%# Bind("MOSTRUARIO") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Produto" HeaderStyle-Width="105px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                SortExpression="PRODUTO">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litProduto" runat="server" Text='<%# Bind("PRODUTO") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtProdutoFooter" runat="server" Width="103px" CssClass="alinharCentro" OnTextChanged="txtProdutoFooter_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Nome" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                SortExpression="NOME">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litNome" runat="server" Text='<%# Bind("NOME") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Literal ID="litNomeFooter" runat="server"></asp:Literal>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Peças" HeaderStyle-Width="95px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                SortExpression="PECAS">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litPecas" runat="server" Text='<%# Bind("PECAS") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtPecasFooter" runat="server" Width="94px" CssClass="alinharCentro" OnTextChanged="CalcularSobraEstoqueRodape" AutoPostBack="true"
                                                                        onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="G. por Folha" HeaderStyle-Width="114px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                SortExpression="FOLHAS">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litFolhas" runat="server" Text='<%# Bind("FOLHAS") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtFolhasFooter" runat="server" Width="113px" CssClass="alinharCentro" OnTextChanged="CalcularSobraEstoqueRodape" AutoPostBack="true"
                                                                        onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Retalhos" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                SortExpression="RETALHOS">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litRetalhos" runat="server" Text='<%# Bind("RETALHOS") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Gasto Total" HeaderStyle-Width="110px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                SortExpression="GASTO">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litGasto" runat="server" Text='<%# Bind("GASTO") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtGastoFooter" runat="server" Width="109px" CssClass="alinharCentro" OnTextChanged="CalcularSobraEstoqueRodape" AutoPostBack="true"
                                                                        onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Estoque" HeaderStyle-Width="105px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                FooterStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litEstoque" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Literal ID="litEstoqueFooter" runat="server"></asp:Literal>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="25px"
                                                                FooterStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btExcluir" runat="server" Height="14px" Width="14px" ImageUrl="~/Image/delete.png"
                                                                        OnClientClick="return ConfirmarExclusao();" OnClick="btExcluir_Click" ToolTip="Excluir" />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:ImageButton ID="btIncluir" runat="server" Height="18px" Width="18px" ImageUrl="~/Image/add.png"
                                                                        OnClick="btIncluir_Click" ToolTip="Incluir" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:HiddenField ID="hidQtdeEstoque" runat="server" Value="" />
                                                <asp:Label ID="labErroHB" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                        </div>
                        <div id="tabs-pedidos">
                            <fieldset>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                    <tr>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvPedidos" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvPedidos_RowDataBound"
                                                    ShowFooter="true">
                                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="No. Pedido" ItemStyle-Width="85px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labNumeroPedido" runat="server" Text='<%# Bind("NUMERO_PEDIDO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Grupo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labGrupo" runat="server" Text='<%# Bind("GRUPO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SubGrupo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labSubGrupo" runat="server" Text='<%# Bind("SUBGRUPO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labCor" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cor Fornecedor" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labCorFornecedor" runat="server" Text='<%# Bind("COR_FORNECEDOR") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Fornecedor" ItemStyle-Width="250px" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labFornecedor" runat="server" Text='<%# Bind("FORNECEDOR") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibtPesquisar" runat="server" Width="15px" ImageUrl="~/Image/search.png"
                                                                    ToolTip="Pesquisar" OnClick="ibtPesquisarPedido_Click" />
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
                    </div>
                </fieldset>
                <br />
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
