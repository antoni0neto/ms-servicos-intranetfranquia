<%@ Page Title="Venda de Produtos Semana Online" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="gest_venda_semana_online.aspx.cs" Inherits="Relatorios.gest_venda_semana_online"
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

        function openwindow(l) {
            window.open(l, "VENDASEMANA", "menubar=1,resizable=0,width=1000,height=500");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo de Ecommerce&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Venda de Produtos Semana Online</span>
        <div style="float: right; padding: 0;">
            <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <fieldset>
        <legend>
            <asp:Label ID="labTitulo" runat="server" Text="Venda de Produtos Semana Online"></asp:Label></legend>
        <table border="0" width="100%" cellpadding="0" cellspacing="0">
            <tr>
                <td>Semana</td>
                <td colspan="7">Tipo
                </td>
            </tr>
            <tr>
                <td style="width: 180px;">
                    <asp:DropDownList runat="server" ID="ddlSemana" DataValueField="CODIGO" DataTextField="SEMANA" Width="174px">
                    </asp:DropDownList>
                </td>
                <td colspan="7">
                    <asp:DropDownList ID="ddlTipo" runat="server" Width="174px" Enabled="false">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="01" Text="PEÇA" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="02" Text="ACESSÓRIO"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 30px;">Coleção Linx
                </td>
                <td>Griffe
                </td>
                <td>Grupo Produto
                </td>
                <td>Produto
                </td>
                <td>Nome
                </td>
                <td>Tecido
                </td>
                <td>Cor Fornecedor
                </td>
                <td>Produto Acabado
                </td>
            </tr>
            <tr>
                <td style="width: 180px;" valign="top">
                    <asp:ListBox ID="lstColecao" runat="server" DataValueField="COLECAO" DataTextField="DESC_COLECAO" SelectionMode="Multiple" Width="174px" Height="140px"></asp:ListBox>
                </td>
                <td style="width: 180px;" valign="top">
                    <asp:ListBox ID="lstGriffe" runat="server" DataValueField="GRIFFE" Width="174px" DataTextField="GRIFFE" SelectionMode="Multiple" Height="140px"></asp:ListBox>
                </td>
                <td style="width: 180px;" valign="top">
                    <asp:ListBox ID="lstGrupoProduto" runat="server" DataValueField="GRUPO_PRODUTO" DataTextField="GRUPO_PRODUTO" SelectionMode="Multiple" Width="174px" Height="140px"></asp:ListBox>
                </td>
                <td style="width: 150px;" valign="top">
                    <asp:TextBox ID="txtModelo" runat="server" Width="136px" MaxLength="20"></asp:TextBox>
                </td>
                <td style="width: 160px;" valign="top">
                    <asp:TextBox ID="txtNome" runat="server" Width="146px" MaxLength="20"></asp:TextBox>
                </td>
                <td style="width: 210px;" valign="top">
                    <asp:DropDownList ID="ddlTecido" runat="server" Width="204px" Height="21px" DataTextField="TECIDO_POCKET"
                        DataValueField="TECIDO_POCKET" OnSelectedIndexChanged="ddlTecido_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td style="width: 210px;" valign="top">
                    <asp:DropDownList ID="ddlCorFornecedor" runat="server" Width="204px" Height="21px"
                        DataTextField="FORNECEDOR_COR" DataValueField="FORNECEDOR_COR">
                    </asp:DropDownList>
                </td>
                <td valign="top">
                    <asp:DropDownList ID="ddlProdutoAcabado" runat="server" Width="155px" Height="21px">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                        <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Coleção Intranet
                </td>
                <td>Origem
                </td>
                <td>Signed
                </td>
                <td colspan="2">Signed Nome
                </td>
                <td>Fornecedor
                </td>
                <td>SubGrupo Produto
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:DropDownList ID="ddlColecoes" runat="server" Width="174px" Height="21px" DataTextField="DESC_COLECAO"
                        DataValueField="COLECAO" OnSelectedIndexChanged="ddlColecoes_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td valign="top">
                    <asp:DropDownList ID="ddlOrigem" runat="server" Width="174px" Height="21px" DataTextField="DESCRICAO"
                        DataValueField="CODIGO" OnSelectedIndexChanged="ddlOrigem_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td valign="top">
                    <asp:DropDownList ID="ddlSigned" runat="server" Width="174px" Height="21px">
                        <asp:ListItem Value="" Text=""></asp:ListItem>
                        <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                        <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td colspan="2" valign="top">
                    <asp:DropDownList ID="ddlSignedNome" runat="server" Width="304px" Height="21px" DataTextField="SIGNED_NOME"
                        DataValueField="SIGNED_NOME">
                    </asp:DropDownList>
                </td>
                <td valign="top">
                    <asp:DropDownList ID="ddlFornecedor" runat="server" Width="204px" Height="21px" DataTextField="FORNECEDOR"
                        DataValueField="FORNECEDOR">
                    </asp:DropDownList>
                </td>
                <td valign="top">
                    <asp:DropDownList ID="ddlSubGrupoProduto" runat="server" Width="204px" Height="21px" DataTextField="SUBGRUPO_PRODUTO"
                        DataValueField="SUBGRUPO_PRODUTO">
                    </asp:DropDownList>
                </td>
                <td valign="top">&nbsp;
                </td>
            </tr>
            <tr>
                <td>Preço Acima De</td>
                <td>Preço Abaixo De</td>
                <td>Qtd Venda Acima De</td>
                <td>Qtd Venda Abaixo De</td>
                <td>Estoq Loja Acima De</td>
                <td>Estoq Loja Abaixo De</td>
                <td colspan="2">&nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtPrecoIni" runat="server" CssClass="alinharDireira" Text="" Width="166px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtPrecoFim" runat="server" CssClass="alinharDireira" Text="" Width="166px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtQtdeVendidaIni" runat="server" CssClass="alinharDireira" Text="" Width="166px" onkeypress="return fnValidarNumeroNegativo(event);"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtQtdeVendidaFim" runat="server" CssClass="alinharDireira" Text="" Width="136px" onkeypress="return fnValidarNumeroNegativo(event);"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtEstoqueIni" runat="server" CssClass="alinharDireira" Text="" Width="146px" onkeypress="return fnValidarNumeroNegativo(event);"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtEstoqueFim" runat="server" CssClass="alinharDireira" Text="" Width="196px" onkeypress="return fnValidarNumeroNegativo(event);"></asp:TextBox>
                </td>
                <td colspan="2">&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="8">&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;
                            <asp:Button ID="btExcel" runat="server" Text="Excel" Width="100px" OnClick="btExcel_Click" OnClientClick="DesabilitarBotao(this);" />
                </td>
                <td colspan="4" style="text-align: right;">
                    <asp:Label ID="labErroRem" runat="server" ForeColor="Red" Text=""></asp:Label>&nbsp;
                    <asp:Button ID="btGerarTXT" runat="server" Enabled="true" Text="Gerar TXT" Width="150px" OnClick="btGerarTXT_Click" Visible="false" />&nbsp;
                    <asp:Button ID="btLimparRemarcacao" runat="server" Enabled="true" Text="Limpar Remarcação" Width="150px" OnClick="btLimparRemarcacao_Click" OnClientClick="javascript: return confirm('Todos os preços serão apagados. Confirma esta operação?');" Visible="false" />
                </td>
            </tr>
            <tr style="line-height: 20px;">
                <td colspan="8">&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <div class="rounded_corners">
                        <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound"
                            OnDataBound="gvProduto_DataBound" DataKeyNames="PRODUTO"
                            OnSorting="gvProduto_Sorting" AllowSorting="true" ShowFooter="true">
                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                            <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                            <Columns>
                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtImprimir" runat="server" Width="15px" ImageUrl="~/Image/print.png"
                                            ToolTip="Imprimir" OnClick="ibtImprimir_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-VerticalAlign="Middle">
                                    <ItemTemplate>
                                        <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../Image/plus.png"
                                            Width="15px" runat="server" />
                                        <asp:Panel ID="pnlVendas" runat="server" Style="display: none" Width="100%">
                                            <asp:GridView ID="gvProdutoHistorico" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvProdutoHistorico_RowDataBound" OnDataBound="gvProdutoHistorico_DataBound" ShowFooter="true"
                                                Width="100%">
                                                <HeaderStyle BackColor="LightSteelBlue" HorizontalAlign="Center"></HeaderStyle>
                                                <RowStyle BorderColor="GradientActiveCaption" BorderWidth="1px" />
                                                <FooterStyle BackColor="LightSteelBlue" HorizontalAlign="Center" Font-Bold="true"></FooterStyle>
                                                <Columns>
                                                    <asp:BoundField DataField="" HeaderText="" HeaderStyle-Width="210px" SortExpression="" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="QTDE_SEMANA" HeaderText="Qtde Semana" HeaderStyle-Width="100px" SortExpression="QTDE_SEMANA" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="SEMANA" HeaderText="Semana" HeaderStyle-Width="160px" SortExpression="SEMANA" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:TemplateField HeaderText="Venda" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="130px" SortExpression="QTDE_VENDIDA" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litQtdeVendida" runat="server" Text='<%# Bind("QTDE_VENDIDA") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Preço" ItemStyle-HorizontalAlign="Left" SortExpression="PRECO" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litPreco" runat="server" Text=''></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Filial" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litAbrirProduto" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgProduto" runat="server" Width="90%" ImageAlign="AbsMiddle" OnClick="imgProduto_Click" ToolTip="" AlternateText="" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Data Inicial" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Literal ID="litDataInicial" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="QTDE_SEMANA" HeaderText="Qtde Semana" HeaderStyle-Width="100px" SortExpression="QTDE_SEMANA" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="SEMANA" HeaderText="Semana" HeaderStyle-Width="160px" SortExpression="SEMANA" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="PRODUTO" HeaderText="Produto" HeaderStyle-Width="90px" SortExpression="PRODUTO" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="DESC_PRODUTO" HeaderText="Nome" SortExpression="DESC_PRODUTO" />
                                <asp:BoundField DataField="GRIFFE" HeaderText="Griffe" HeaderStyle-Width="" SortExpression="GRIFFE" />
                                <asp:BoundField DataField="DESC_COR" HeaderText="Cor" SortExpression="DESC_COR" />

                                <asp:TemplateField HeaderText="Venda" ItemStyle-HorizontalAlign="Center" SortExpression="QTDE_VENDIDA" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Literal ID="litQtdeVendida" runat="server" Text='<%# Bind("QTDE_VENDIDA") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Est. Online" ItemStyle-HorizontalAlign="Center" SortExpression="ESTOQUE_LOJA" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Literal ID="litEstoqueLoja" runat="server" Text='<%# Bind("ESTOQUE_LOJA") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Estoque Fábrica" ItemStyle-HorizontalAlign="Center" SortExpression="ESTOQUE_FAB" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Literal ID="litEstoqueFabrica" runat="server" Text='<%# Bind("ESTOQUE_FAB") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Custo TS" ItemStyle-HorizontalAlign="Left" SortExpression="CUSTO" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Literal ID="litCusto" runat="server" Text=''></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Preço" ItemStyle-HorizontalAlign="Left" SortExpression="PRECO" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Literal ID="litPreco" runat="server" Text=''></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Remarcação" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px" SortExpression="PRECO" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txPrecoRemarc" runat="server" Width="80px" OnTextChanged="txPrecoRemarc_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Filial" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Literal ID="litAbrirProduto" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
        <div>
            <asp:Panel ID="pnlExcel" runat="server">
                <asp:GridView ID="gvExcel" runat="server" Width="100%" AutoGenerateColumns="False"
                    ForeColor="#333333" Style="background: white" ShowFooter="true">
                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                    <Columns>
                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DATA_INICIAL" HeaderText="Data Inicial" />
                        <asp:BoundField DataField="QTDE_SEMANA" HeaderText="Qtde Semana" HeaderStyle-Width="100px" SortExpression="QTDE_SEMANA" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="SEMANA" HeaderText="Semana" HeaderStyle-Width="160px" SortExpression="SEMANA" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="PRODUTO" HeaderText="Produto" HeaderStyle-Width="90px" SortExpression="PRODUTO" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="DESC_PRODUTO" HeaderText="Nome" SortExpression="DESC_PRODUTO" />
                        <asp:BoundField DataField="GRIFFE" HeaderText="Griffe" HeaderStyle-Width="180px" SortExpression="GRIFFE" />
                        <asp:BoundField DataField="DESC_COR" HeaderText="Cor" SortExpression="DESC_COR" />
                        <asp:BoundField DataField="QTDE_VENDIDA" HeaderText="Qtde Venda" />
                        <asp:BoundField DataField="ESTOQUE_LOJA" HeaderText="Estoque Loja" />
                        <asp:BoundField DataField="ESTOQUE_FAB" HeaderText="Estoque Fábrica" />
                        <asp:BoundField DataField="CUSTO" HeaderText="Custo" />
                        <asp:BoundField DataField="PRECO" HeaderText="Preço" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </div>
    </fieldset>
</asp:Content>
