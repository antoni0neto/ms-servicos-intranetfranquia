<%@ Page Title="Compra de Materiais" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master"
    CodeBehind="desenv_material_compra.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="Relatorios.mod_desenvolvimento.desenv_material_compra" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro
                    de Apoio&nbsp;&nbsp;>&nbsp;&nbsp;Compra de Materiais</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <br />
            <div style="width: 100%;">
                <fieldset>
                    <legend>Compra de Materiais</legend>
                    <div style="padding-left: 0px;">
                        &nbsp;
                        <asp:HiddenField ID="hidTabSelected" runat="server" Value="" />
                        <div style="text-align: right; float: right;">
                            <asp:Label ID="labErro" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                    <hr />
                    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                        <tr>
                            <td>&nbsp;
                            </td>
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

                        </tr>
                        <tr>
                            <td style="width: 30px; text-align: left;">
                                <asp:RadioButton ID="rdbFiltroDDL" runat="server" GroupName="gnFiltro" Checked="true" OnCheckedChanged="rdbFiltroTexto_CheckedChanged" AutoPostBack="true" />
                            </td>
                            <td style="width: 160px;">
                                <asp:DropDownList ID="ddlMaterialGrupo" runat="server" Width="154px" Height="21px"
                                    DataTextField="GRUPO" DataValueField="CODIGO_GRUPO" OnSelectedIndexChanged="ddlMaterialGrupo_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 190px;">
                                <asp:DropDownList ID="ddlMaterialSubGrupo" runat="server" Width="184px" Height="21px"
                                    DataTextField="SUBGRUPO" DataValueField="CODIGO_SUBGRUPO" OnSelectedIndexChanged="ddlMaterialSubGrupo_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 220px;">
                                <asp:DropDownList ID="ddlCor" runat="server" Width="214px" Height="21px" DataTextField="DESC_COR"
                                    DataValueField="COR">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 350px;">
                                <asp:DropDownList ID="ddlCorFornecedor" runat="server" Width="344px" Height="21px"
                                    DataTextField="COR_FORNECEDOR" DataValueField="COR_FORNECEDOR">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFornecedor" runat="server" Width="254px" Height="21px" DataTextField="FORNECEDOR"
                                    DataValueField="FORNECEDOR">
                                </asp:DropDownList>
                            </td>

                        </tr>
                        <tr>
                            <td colspan="6">&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:RadioButton ID="rdbFiltroTexto" runat="server" GroupName="gnFiltro" OnCheckedChanged="rdbFiltroTexto_CheckedChanged" AutoPostBack="true" />
                            </td>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtGrupoFiltro" runat="server" Width="150px"></asp:TextBox>
                            </td>
                            <td style="width: 190px;">
                                <asp:TextBox ID="txtSubGrupoFiltro" runat="server" Width="180px"></asp:TextBox>
                            </td>
                            <td style="width: 220px;">&nbsp;
                            </td>
                            <td style="width: 350px;">&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>

                        </tr>
                    </table>
                    <br />
                    <div id="tabs">
                        <ul>
                            <li><a href="#tabs-produtos" id="tabProdutos" runat="server" onclick="MarcarAba(0);">Produtos</a></li>
                            <li><a href="#tabs-alterar-pedido" id="tabAlterarPedido" runat="server" onclick="MarcarAba(1);">Inclusão / Alteração de Pedidos</a></li>
                            <li><a href="#tabs-entradas" id="tabEntradas" runat="server" onclick="MarcarAba(2);">Entradas</a></li>
                            <li><a href="#tabs-pedido" id="tabPedido" runat="server" onclick="MarcarAba(3);">Pedidos</a></li>
                            <li><a href="#tabs-estoque" id="tabEstoque" runat="server" onclick="MarcarAba(4);">Estoque</a></li>
                            <li><a href="#tabs-notas" id="tabRelNotas" runat="server" onclick="MarcarAba(5);">Relatório de Notas</a></li>
                            <li>&nbsp;</li>
                        </ul>
                        <div id="tabs-produtos">
                            <fieldset>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                    <tr>
                                        <td>
                                            <asp:Label ID="labModelo" runat="server" Text="Produto"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labNome" runat="server" Text="Nome"></asp:Label>
                                        </td>
                                        <td style="text-align: center;">&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 140px;">
                                            <asp:TextBox ID="txtModelo" runat="server" Width="130px" Height="15px"></asp:TextBox>
                                        </td>
                                        <td style="width: 200px;">
                                            <asp:TextBox ID="txtNome" runat="server" Width="190px" Height="15px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtPesquisar" runat="server" Width="19px" ImageUrl="~/Image/search.png"
                                                ToolTip="Pesquisar" OnClick="ibtPesquisar_Click" />
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td style="text-align: right;">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">&nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                    <tr>
                                        <td colspan="15">
                                            <fieldset>
                                                <legend>Pedidos</legend>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvPedidoProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvPedidoProduto_RowDataBound"
                                                        OnDataBound="gvPedidoProduto_DataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="No. Pedido" ItemStyle-Width="85px" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labNumeroPedido" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Grupo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labGrupo" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="SubGrupo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labSubGrupo" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Fornecedor" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labFornecedor" runat="server"></asp:Label>
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
                                                                    <asp:Label ID="labCorFornecedor" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Qtde" ItemStyle-Width="95px" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labQtde" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Qtde Entregue" ItemStyle-Width="110px" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labQtdeEntregue" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="15">
                                            <fieldset>
                                                <legend>Produtos</legend>
                                                <div style="text-align: left; float: left;">
                                                    Quantidade Selecionada:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                                                        ID="labQtdeSelecionada" runat="server" ForeColor="Green" Text="0" Font-Bold="true"></asp:Label>&nbsp;&nbsp;
                                                </div>
                                                <div style="text-align: right;">
                                                    <asp:Label ID="labMarcarTodos" runat="server" Text="Marcar Todos"></asp:Label>
                                                    <asp:CheckBox ID="chkProdutoTodos" runat="server" Checked="false" OnCheckedChanged="chkProdutoTodos_CheckedChanged"
                                                        AutoPostBack="true" />
                                                    &nbsp;
                                                </div>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvProdutos" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProdutos_RowDataBound"
                                                        OnDataBound="gvProdutos_DataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Modelo" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labModelo" runat="server" Text='<%# Eval("MODELO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Nome" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labNome" runat="server" Text='<%# Eval("NOME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Detalhe" ItemStyle-Width="95px" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labDetalhe" runat="server" Text='<%# Eval("DETALHE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Grupo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labGrupo" runat="server" Text='<%# Eval("GRUPO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="SubGrupo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labSubGrupo" runat="server" Text='<%# Eval("SUBGRUPO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Cor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labCor" runat="server" Text='<%# Eval("DESC_COR_MATERIAL") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Cor Fornecedor" ItemStyle-Width="210px" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labCorFornecedor" runat="server" Text='<%# Eval("COR_FORNECEDOR") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Fornecedor" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labFornecedor" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Tipo" ItemStyle-Width="125px" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labTipo" runat="server" Text='<%# Eval("TIPO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Qtde" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labQtde" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="10px">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkMarcar" runat="server" Checked="false" OnCheckedChanged="chkMarcar_CheckedChanged"
                                                                        AutoPostBack="true" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="15">&nbsp;
                                            <asp:HiddenField ID="hidMaterialGrupoPedido" runat="server" />
                                            <asp:HiddenField ID="hidMaterialSubGrupoPedido" runat="server" />
                                            <asp:HiddenField ID="hidCorPedido" runat="server" />
                                            <asp:HiddenField ID="hidCorFornecedorPedido" runat="server" />
                                            <asp:HiddenField ID="hidFornecedorPedido" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px;">Qtde Total de Pedido:
                                        </td>
                                        <td colspan="14">
                                            <asp:Label ID="labQtdeTotalPedido" runat="server" ForeColor="Green" Text="0" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px;">Qtde Total de Produto:
                                        </td>
                                        <td colspan="14">
                                            <asp:Label ID="labQtdeTotalProduto" runat="server" ForeColor="Green" Text="0" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px;">Necessidade de Compra:
                                        </td>
                                        <td colspan="14">
                                            <asp:Label ID="labQtdeNecessidadeCompra" runat="server" ForeColor="Red" Text="0"
                                                Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px;">Qtde Total de Sobra:
                                        </td>
                                        <td colspan="14">
                                            <asp:Label ID="labQtdeSobra" runat="server" ForeColor="Green" Text="0" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;" colspan="15">
                                            <asp:Label ID="labErroGerarPedido" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>&nbsp;&nbsp;
                                            <asp:Button ID="btGerarPedido" runat="server" Text="Gerar Pedido" Width="120px" Visible="false"
                                                OnClick="btGerarPedido_Click" />
                                            <asp:HiddenField ID="hidLimpaFiltro" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                        <div id="tabs-pedido">
                            <fieldset>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                    <tr>
                                        <td>Número do Pedido
                                        </td>
                                        <td colspan="4">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 180px;">
                                            <asp:TextBox ID="txtPedidoNumeroTodos" runat="server" Width="170px" Height="15px"
                                                CssClass="alinharDireita"></asp:TextBox>
                                        </td>
                                        <td colspan="4">
                                            <asp:ImageButton ID="ibtPesquisarTodos" runat="server" Width="19px" ImageUrl="~/Image/search.png"
                                                ToolTip="Pesquisar Pedidos" OnClick="ibtPesquisarTodos_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvPedidos" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvPedidos_RowDataBound"
                                                    OnDataBound="gvPedidos_DataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btEditarPedido" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/edit.jpg"
                                                                    OnClick="btEditarPedido_Click" ToolTip="Editar" />
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:ImageButton ID="btIncluirPedido" runat="server" Height="18px" Width="18px" ImageUrl="~/Image/add.png"
                                                                    OnClick="btIncluirPedido_Click" ToolTip="Incluir" Visible="false" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Pedido" ItemStyle-Width="85px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labNumeroPedido" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Data Pedido" ItemStyle-Width="85px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labDataPedido" runat="server" Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Prev. Entrega" ItemStyle-Width="95px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labPrevisaoEntrega" runat="server" Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Fornecedor" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labFornecedor" runat="server" Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SubGrupo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labSubGrupo" runat="server" Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labCor" runat="server" Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cor Fornecedor" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labCorFornecedor" runat="server" Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qtde Pedido" ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labQtde" runat="server" Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qtde Entregue" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labQtdeEntregue" runat="server" Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qtde Falt" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labQtdeFalt" runat="server" Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="95px" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labStatus" runat="server" Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="25px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btExcluirPedido" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/delete.png"
                                                                    OnClientClick="return ConfirmarExclusao();" OnClick="btExcluirPedido_Click" ToolTip="Excluir" />
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
                        <div id="tabs-alterar-pedido">
                            <div class="bodyMaterial">
                                <asp:ImageButton ID="ibtIncluir" runat="server" Width="20px" ImageUrl="~/Image/add.png" Visible="false"
                                    ToolTip="Novo Pedido" OnClick="ibtPedidoNovo_Click" />
                                <fieldset style="margin-top: 4px; padding-top: 0;">
                                    <legend>Pedido</legend>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="labNumeroPedido" runat="server" Text="Número Pedido"></asp:Label>
                                                <asp:HiddenField ID="hidCodigoPedido" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="labColecao" runat="server" Text="Coleção"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labFornecedorPedido" runat="server" Text="Fornecedor"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labMaterialGrupoPedido" runat="server" Text="Grupo"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labMaterialSubGrupoPedido" runat="server" Text="SubGrupo"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labStatus" runat="server" Text="Status"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 180px;">
                                                <asp:TextBox ID="txtPedidoNumero" runat="server" Width="170px" Height="15px" Enabled="false"
                                                    CssClass="alinharDireita" OnTextChanged="txtPedidoNumero_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            </td>
                                            <td style="width: 210px;">
                                                <asp:DropDownList ID="ddlColecoes" runat="server" Width="204px" Height="22px" DataTextField="DESC_COLECAO"
                                                    DataValueField="COLECAO">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 240px;">
                                                <asp:DropDownList ID="ddlPedidoFornecedor" runat="server" Width="234px" Height="21px"
                                                    DataTextField="FORNECEDOR" DataValueField="FORNECEDOR" OnSelectedIndexChanged="ddlPedidoFornecedor_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 210px;">
                                                <asp:DropDownList ID="ddlMaterialGrupoPedido" runat="server" Width="204px" Height="21px"
                                                    DataTextField="GRUPO" DataValueField="CODIGO_GRUPO" OnSelectedIndexChanged="ddlMaterialGrupo_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 220px;">
                                                <asp:DropDownList ID="ddlMaterialSubGrupoPedido" runat="server" Width="214px" Height="21px"
                                                    DataTextField="SUBGRUPO" DataValueField="CODIGO_SUBGRUPO" OnSelectedIndexChanged="ddlMaterialSubGrupo_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlStatus" runat="server" Width="295px" Height="22px">
                                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                                    <asp:ListItem Value="A" Text="AGUARDANDO" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Value="B" Text="BAIXADO"></asp:ListItem>
                                                    <asp:ListItem Value="R" Text="RESERVADO"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="labCorPedido" runat="server" Text="Cor"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labCorFornecedorPedido" runat="server" Text="Cor Fornecedor"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labQuantidade" runat="server" Text="Quantidade"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labPreco" runat="server" Text="Preço"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labCondicaoPgto" runat="server" Text="Condição de Pagamento"></asp:Label>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlCorPedido" runat="server" Width="174px" Height="21px" DataTextField="DESC_COR"
                                                    DataValueField="COR">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCorFornecedorPedido" runat="server" Width="204px" Height="21px"
                                                    DataTextField="REFER_FABRICANTE" DataValueField="REFER_FABRICANTE">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtQtde" runat="server" Width="230px" MaxLength="10" Style="text-align: right;"
                                                    onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPreco" runat="server" Width="200px" MaxLength="10" Style="text-align: right;"
                                                    onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCondicaoPgto" runat="server" Width="214px" Height="22px"
                                                    DataTextField="DESC_COND_PGTO" DataValueField="CONDICAO_PGTO" OnSelectedIndexChanged="ddlCondicaoPgto_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCondicaoPgtoOutro" runat="server" Width="291px" Height="16px"
                                                    MaxLength="20"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="labDataPedido" runat="server" Text="Data do Pedido"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labDataPrevisaoEntrega" runat="server" Text="Previsão de Entrega"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labDataReserva" runat="server" Text="Data da Reserva"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labUnidadeMedida" runat="server" Text="Unidade de Medida"></asp:Label>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtPedidoData" runat="server" Width="170px" MaxLength="10" Style="text-align: right;"
                                                    onkeypress="return fnValidarData(event);"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPedidoPrevEntrega" runat="server" Width="200px" MaxLength="10"
                                                    Style="text-align: right;" onkeypress="return fnValidarData(event);"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPedidoDataReserva" runat="server" Width="230px" MaxLength="10"
                                                    Style="text-align: right;" onkeypress="return fnValidarData(event);"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlUnidade" Height="21px" runat="server" DataValueField="CODIGO"
                                                    DataTextField="DESCRICAO" Width="204px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkEmail" runat="server" Text="Enviar e-mail" Checked="false" OnCheckedChanged="chkEmail_CheckedChanged" AutoPostBack="true" />
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="6">
                                                <asp:Label ID="labEmail" runat="server" Text="E-mail"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <asp:TextBox ID="txtEmail" runat="server" Width="830px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <asp:Label ID="labCorpoEmail" runat="server" Text="Corpo do E-mail"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <asp:TextBox ID="txtCorpoEmail" runat="server" Width="1350px" Height="60px" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Button ID="btAbrirSubPedido" runat="server" Width="150px" Text="Abrir Sub Pedidos" ToolTip="Abrir Sub Pedidos" OnClick="btAbrirSubPedido_Click" />
                                                <asp:Button ID="btAtualizarSubPedido" runat="server" Width="180px" Text="Atualizar Sub Pedidos" ToolTip="Atualizar Sub Pedidos" OnClick="btAtualizarSubPedido_Click" />
                                            </td>
                                            <td colspan="3" style="text-align: right;">
                                                <asp:Button ID="btSalvarPedido" runat="server" Width="120px" Text="Salvar Pedido" ToolTip="Salvar" OnClick="btSalvarPedido_Click" />
                                                <asp:ImageButton ID="btCancelarPedido" runat="server" Width="18px" ImageUrl="~/Image/cancel.png"
                                                    ToolTip="Cancelar" Visible="false" OnClick="btCancelarPedido_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvSubPedido" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvSubPedido_RowDataBound"
                                                        OnDataBound="gvSubPedido_DataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="SubPedido" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labNumeroSubPedido" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Fornecedor" ItemStyle-Width="155px" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labFornecedor" runat="server" Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Qtde" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labQtde" runat="server" Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Custo" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labCusto" runat="server" Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Data Pedido" ItemStyle-Width="90px" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labDataPedido" runat="server" Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Previsão Entrega" ItemStyle-Width="90px" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labDataPedidoPrevisao" runat="server" Font-Size="Smaller"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="20px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btExcluirSubPedido" runat="server" Height="15px" Width="15px"
                                                                        ImageUrl="~/Image/delete.png" OnClick="btExcluirSubPedido_Click" OnClientClick="return Confirmar('Deseja realmente Excluir este SubPedido?');"
                                                                        ToolTip="Baixar Pedido" />
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
                        <div id="tabs-entradas">
                            <fieldset>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                    <tr>
                                        <td>Número do Pedido
                                        </td>
                                        <td colspan="4">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 180px;">
                                            <asp:TextBox ID="txtPedidoNumeroEntrada" runat="server" Width="170px" Height="15px"
                                                CssClass="alinharDireita"></asp:TextBox>
                                        </td>
                                        <td colspan="4">
                                            <asp:ImageButton ID="ibtPesquisarEntrada" runat="server" Width="19px" ImageUrl="~/Image/search.png"
                                                ToolTip="Pesquisar Entradas" OnClick="ibtPesquisarEntrada_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvPedidosEntrada" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvPedidosEntrada_RowDataBound"
                                                    OnDataBound="gvPedidosEntrada_DataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btEditarPedidoEntrada" runat="server" Height="15px" Width="15px"
                                                                    ImageUrl="~/Image/edit.jpg" OnClick="btEditarPedido_Click" ToolTip="Editar" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Pedido" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labNumeroPedido" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Data Pedido" ItemStyle-Width="90px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labDataPedido" runat="server" Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Fornecedor" ItemStyle-Width="155px" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labFornecedor" runat="server" Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SubGrupo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labSubGrupo" runat="server" Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labCor" runat="server" Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cor Fornecedor" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labCorFornecedor" runat="server" Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qtde Pedido" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labQtde" runat="server" Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qtde Entregue" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labQtdeEntregue" runat="server" Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Qtde Falt" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labQtdeFalt" runat="server" Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="20px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btEntrarQtde" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/update.png"
                                                                    OnClick="btEntrarQtde_Click" ToolTip="Entrar Quantidade Entregue" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="20px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btBaixarPedidoEntrada" runat="server" Height="15px" Width="15px"
                                                                    ImageUrl="~/Image/approve.png" OnClick="btBaixarPedido_Click" OnClientClick="return Confirmar('Deseja realmente Baixar este Pedido?');"
                                                                    ToolTip="Baixar Pedido" />
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
                        <div id="tabs-estoque">
                            <fieldset>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="ibtPesquisarEstoque" runat="server" Width="19px" ImageUrl="~/Image/search.png"
                                                ToolTip="Pesquisar Estoque" OnClick="ibtPesquisarEstoque_Click" />
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:Label ID="labErroEstoque" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvEstoque" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvEstoque_RowDataBound"
                                                    OnDataBound="gvEstoque_DataBound" ShowFooter="true" DataKeyNames="COR">
                                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Grupo" ItemStyle-Width="130px" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labGrupo" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SubGrupo" ItemStyle-Width="180px" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labSubGrupo" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cor" ItemStyle-Width="180px" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labCor" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cor Fornecedor" ItemStyle-Width="190px" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labCorFornecedor" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Fornecedor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labFornecedor" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qtde Entregue" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labQtdeEntregue" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qtde Consumida" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labQtdeConsumida" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qtde Estoque" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labQtde" runat="server"></asp:Label>
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
                        <div id="tabs-notas">
                            <fieldset>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                    <tr>
                                        <td>Número do Pedido
                                        </td>
                                        <td>Número Nota Fiscal
                                        </td>
                                        <td colspan="3" style="text-align: right;">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 180px;">
                                            <asp:TextBox ID="txtPedidoNumeroNota" runat="server" Width="170px" Height="15px"
                                                CssClass="alinharDireita"></asp:TextBox>
                                        </td>
                                        <td style="width: 180px;">
                                            <asp:TextBox ID="txtNotaFiscal" runat="server" Width="170px" Height="15px"
                                                CssClass="alinharDireita"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtPesquisarNotas" runat="server" Width="19px" ImageUrl="~/Image/search.png"
                                                ToolTip="Pesquisar Notas" OnClick="ibtPesquisarNotas_Click" />
                                        </td>
                                        <td colspan="2" style="text-align: right;">
                                            <asp:Button ID="btImprimir" runat="server" Text="Imprimir" Width="100px" OnClick="btImprimir_Click" Visible="false" />

                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvPedidosNota" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvPedidosNota_RowDataBound"
                                                    OnDataBound="gvPedidosNota_DataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Nota Fiscal" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labNotaFiscal" runat="server" Text='<%# Eval("NOTA_FISCAL") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Fornecedor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labFornecedor" runat="server" Text='<%# Eval("FORNECEDOR") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Material" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labMaterial" runat="server" Text='<%# Eval("MATERIAL") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labCor" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Quantidade" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labQtde" runat="server" Text='<%# Eval("QTDE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Data Recebimento" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labDataRecebimento" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="No. Pedido" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labNumeroPedido" runat="server" Text='<%# Eval("NUMERO_PEDIDO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="15px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btExcluir" runat="server" Width="14px"
                                                                    ImageUrl="~/Image/delete.png" OnClick="btExcluir_Click" ToolTip="Excluir" />
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
