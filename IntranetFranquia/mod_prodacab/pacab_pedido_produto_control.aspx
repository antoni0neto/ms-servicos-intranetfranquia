<%@ Page Title="Controle de Pedidos" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="pacab_pedido_produto_control.aspx.cs" Inherits="Relatorios.pacab_pedido_produto_control" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
        }

        .alicentro {
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produto Acabado&nbsp;&nbsp;>&nbsp;&nbsp;Produto&nbsp;&nbsp;>&nbsp;&nbsp;Controle - Liberação Pedido Produto Acabado</span>
                <div style="float: right; padding: 0;">
                    <a href="pacab_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Pedido LINX</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>Filial</td>
                            <td>Coleção</td>
                            <td>Origem</td>
                            <td>Grupo</td>
                            <td>Produto</td>
                            <td>Cor</td>
                            <td colspan="2">Fornecedor</td>
                        </tr>
                        <tr>
                            <td style="width: 230px">
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="21px" Width="224px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 190px;" valign="top">
                                <asp:DropDownList ID="ddlColecoes" runat="server" Width="184px" Height="21px" DataTextField="DESC_COLECAO"
                                    DataValueField="COLECAO" OnSelectedIndexChanged="ddlColecoes_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 180px;" valign="top">
                                <asp:DropDownList ID="ddlOrigem" runat="server" Width="174px" Height="21px" DataTextField="DESCRICAO"
                                    DataValueField="CODIGO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 180px;" valign="top">
                                <asp:DropDownList ID="ddlGrupo" runat="server" Width="174px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                    DataValueField="GRUPO_PRODUTO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 150px;" valign="top">
                                <asp:TextBox ID="txtProduto" runat="server" Width="140px" MaxLength="10" OnTextChanged="txtProduto_TextChanged" AutoPostBack="true"></asp:TextBox>
                            </td>
                            <td style="width: 210px;" valign="top">
                                <asp:DropDownList ID="ddlCor" runat="server" Width="204px" Height="21px" DataTextField="DESC_COR_PRODUTO"
                                    DataValueField="COR_PRODUTO">
                                </asp:DropDownList>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlFabricante" runat="server" Width="294px" Height="21px" DataTextField="FORNECEDOR"
                                    DataValueField="FORNECEDOR">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Griffe</td>
                            <td>Pedido Intra</td>
                            <td>Tipo</td>
                            <td>Pedido Aberto?</td>
                            <td>Atrasado?</td>
                            <td>Itens Liberados?</td>
                            <td>Pagamento</td>
                            <td>Excluído?</td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:ListBox ID="lstGriffe" runat="server" DataValueField="GRIFFE" Width="224px" DataTextField="GRIFFE" SelectionMode="Multiple" Height="110px"></asp:ListBox>
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="txtPedidoIntra" runat="server" Width="180px" MaxLength="15"></asp:TextBox>
                            </td>
                            <td valign="top">
                                <asp:DropDownList ID="ddlTipo" runat="server" Width="174px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="01" Text="PEÇAS"></asp:ListItem>
                                    <asp:ListItem Value="02" Text="ACESSÓRIOS"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td valign="top">
                                <asp:DropDownList ID="ddlPedidoAberto" runat="server" Width="174px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td valign="top">
                                <asp:DropDownList ID="ddlAtrasado" runat="server" Width="144px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td valign="top">
                                <asp:DropDownList ID="ddlItensOK" runat="server" Width="204px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td valign="top" style="width: 160px;">
                                <asp:DropDownList ID="ddlPagamento" runat="server" Width="154px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Em Aberto"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Pago"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Sem Cadastro"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td valign="top">
                                <asp:DropDownList ID="ddlExcluido" runat="server" Width="134px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" OnClientClick="DesabilitarBotao(this);" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">&nbsp;&nbsp;<asp:Label ID="labMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <fieldset>
                                    <legend>Produtos do Pedido</legend>
                                    <div style="text-align: right;">
                                        <asp:Button ID="btAtualizarCarrinho" runat="server" Text="Atualizar" Width="150px" OnClick="btAtualizarCarrinho_Click" />
                                        <br />
                                        <br />
                                    </div>
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvCarrinho" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvCarrinho_RowDataBound" ShowFooter="true" OnDataBound="gvCarrinho_DataBound"
                                            DataKeyNames="PEDIDO, PRODUTO, COR_PRODUTO, TIPO">
                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Produto" HeaderStyle-Width="130px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litProduto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderText="Nome">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNome" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderText="Cor" HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litCor" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderText="Fornecedor" HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFornecedor" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderText="Custo" HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litCusto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderText="Custo Nota" HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtCustoNota" runat="server" Width="80px" CssClass="alicentro" OnTextChanged="txtCustoNota_TextChanged" onkeypress="return fnValidarNumeroDecimalNegativo(event);"
                                                            AutoPostBack="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Qtde" HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litGradeTotal" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderText="Total" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litValTotal" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="25px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btAdicionarGrade" runat="server" ImageUrl="~/Image/update.png" Width="15px" Height="15px" OnClick="btAdicionarGrade_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="25px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btExcluirItemCarrinho" runat="server" ImageUrl="~/Image/delete.png" Width="15px" Height="15px" OnClick="btExcluirItemCarrinho_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div>
                                        <div style="float: left;">
                                            <br />
                                            <asp:Button ID="btExcluirCarrinho" runat="server" Text="Excluir Carrinho" Width="150px" OnClick="btExcluirCarrinho_Click" />
                                        </div>
                                        <div style="text-align: right;">
                                            <br />
                                            <asp:Label ID="labPedido" runat="server" ForeColor="Red" Text=""></asp:Label>
                                            &nbsp;&nbsp;
                                        <asp:Button ID="btGerarPedido" runat="server" OnClick="btGerarPedido_Click" Width="150px" Text="Gerar Pedido" />&nbsp;
                                        </div>
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <hr />
                                <div style="text-align: right;">
                                    <asp:Button ID="btAdicionarTodos" runat="server" Text="Adicionar Todos" Width="200px" OnClick="btAdicionarTodos_Click" />
                                    <br />
                                    <br />
                                </div>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvProdutoAcabado" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProdutoAcabado_RowDataBound" ShowFooter="true"
                                        OnSorting="gvProdutoAcabado_Sorting" AllowSorting="true" OnDataBound="gvProdutoAcabado_DataBound"
                                        DataKeyNames="PEDIDO_INTRA, PRODUTO, COR, ENTREGA, TIPO">
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="25px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btImprimir" runat="server" Width="18px" Height="18px" ImageUrl="~/Image/print.png" OnClick="btImprimir_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgProduto" runat="server" Width="90%" ImageAlign="AbsMiddle" OnClick="imgProduto_Click" ToolTip="" AlternateText="" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Filial" HeaderStyle-Width="" SortExpression="FILIAL" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litFilial" runat="server" Text='<%# Bind("FILIAL") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Produto" HeaderStyle-Width="" SortExpression="PRODUTO" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litProduto" runat="server" Text='<%# Bind("PRODUTO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Nome" HeaderStyle-Width="" SortExpression="DESC_PRODUTO" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNome" runat="server" Text='<%# Bind("DESC_PRODUTO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Cor" HeaderStyle-Width="" SortExpression="DESC_COR_PRODUTO" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litCor" runat="server" Text='<%# Bind("DESC_COR_PRODUTO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Griffe" HeaderStyle-Width="" SortExpression="GRIFFE" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litGriffe" runat="server" Text='<%# Bind("GRIFFE") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Fornecedor" HeaderStyle-Width="" SortExpression="FORNECEDOR" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litFornecedor" runat="server" Text='<%# Bind("FORNECEDOR") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Custo" HeaderStyle-Width="" SortExpression="CUSTO" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litCusto" runat="server" Text='<%# Bind("CUSTO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Preço" HeaderStyle-Width="" SortExpression="PRECO" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPreco" runat="server" Text='<%# Bind("PRECO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Pedido Intra" HeaderStyle-Width="80px" SortExpression="PEDIDO_INTRA" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPedidoIntra" runat="server" Text='<%# Bind("PEDIDO_INTRA") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Qtde Intra" HeaderStyle-Width="80px" SortExpression="QTDE_INTRA" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="LightCyan">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtdeIntra" runat="server" Text='<%# Bind("QTDE_INTRA") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Pedido Linx" HeaderStyle-Width="80px" SortExpression="PEDIDO_LINX" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPedidoLinx" runat="server" Text='<%# Bind("PEDIDO_LINX") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Qtde Linx" HeaderStyle-Width="80px" SortExpression="QTDE_LINX" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="LightCyan">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtdeLinx" runat="server" Text='<%# Bind("QTDE_LINX") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Ent. Inicial" HeaderStyle-Width="95px" SortExpression="ENTREGA" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDataEntrega" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Prev Final" HeaderStyle-Width="85px" SortExpression="LIMITE_ENTREGA" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDataPrevEntrega" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Itens" HeaderStyle-Width="110px" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbComposicao" runat="server" Checked="false" Text="Eti Composição" OnCheckedChanged="cbItens_CheckedChanged" AutoPostBack="true" /><asp:Literal ID="lit1" runat="server" Text="<br />"></asp:Literal>
                                                    <asp:CheckBox ID="cbPreco" runat="server" Checked="false" Text="Eti Preço" OnCheckedChanged="cbItens_CheckedChanged" AutoPostBack="true" /><asp:Literal ID="lit2" runat="server" Text="<br />"></asp:Literal>
                                                    <asp:CheckBox ID="cbTAG" runat="server" Checked="false" Height="0px" Text="TAG" OnCheckedChanged="cbItens_CheckedChanged" AutoPostBack="true" /><asp:Literal ID="lit3" runat="server" Text="<br />"></asp:Literal>
                                                    <asp:CheckBox ID="cbAviamento" runat="server" Checked="false" Text="Aviamentos" OnCheckedChanged="cbItens_CheckedChanged" AutoPostBack="true" /><asp:Literal ID="lit4" runat="server" Text="<br />"></asp:Literal>
                                                    <asp:TextBox ID="txtObs" runat="server" TextMode="MultiLine" Width="105px" Height="45px" OnTextChanged="txtObs_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Pagamento" HeaderStyle-Width="140px" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>

                                                    <asp:Literal ID="litEsp0" runat="server" Text="<br />" />
                                                    <asp:Literal ID="litValorTotal" runat="server" Text="" />
                                                    <asp:Literal ID="litEsp2" runat="server" Text="<br /><br />" />
                                                    <asp:Literal ID="litParc1" runat="server" Text="" />
                                                    <asp:Literal ID="litParc2" runat="server" Text="" />
                                                    <asp:Literal ID="litParc3" runat="server" Text="" />
                                                    <asp:Literal ID="litParc4" runat="server" Text="" />
                                                    <asp:Literal ID="litParc5" runat="server" Text="" />
                                                    <asp:Literal ID="litParc6" runat="server" Text="" />
                                                    <asp:Literal ID="litParc7" runat="server" Text="" />
                                                    <asp:Literal ID="litParc8" runat="server" Text="" />
                                                    <asp:Literal ID="litEsp99" runat="server" Text="<br />" />
                                                    <asp:ImageButton ID="imgPagto" runat="server" ImageUrl="~/Image/cash.png" Width="15px" OnClick="imgPagto_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgExc" runat="server" ImageUrl="~/Image/delete.png" Width="15px" OnClick="imgExc_Click" OnClientClick="javascript: return confirm('Tem certeza que deseja excluir este Produto?');" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Button ID="btAdicionarCarrinho" runat="server" Text=">>" Width="35px" Height="21px" OnClick="btAdicionarCarrinho_Click" />
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
