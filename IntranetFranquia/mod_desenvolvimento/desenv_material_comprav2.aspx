<%@ Page Title="Pedido de Tecido" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master"
    CodeBehind="desenv_material_comprav2.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="Relatorios.mod_desenvolvimento.desenv_material_comprav2" %>

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Tecidos&nbsp;&nbsp;>&nbsp;&nbsp;Pedido de Tecido</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <br />
            <div style="width: 100%;">
                <fieldset>
                    <legend>Pedido de Tecido</legend>
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
                    </table>
                    <br />
                    <div id="tabs">
                        <ul>
                            <li><a href="#tabs-pedido" id="tabPedido" runat="server" onclick="MarcarAba(0);">Pedidos</a></li>
                            <li><a href="#tabs-alterar-pedido" id="tabAlterarPedido" runat="server" onclick="MarcarAba(1);">Manutenção de Pedidos</a></li>
                            <li><a href="#tabs-lote-pedido" id="A1" runat="server" onclick="MarcarAba(2);">Pedidos em Lote</a></li>
                            <li><a href="#tabs-lote-pedido-res" id="A2" runat="server" onclick="MarcarAba(3);">Pedidos em Lote (Reservados)</a></li>
                            <li>&nbsp;</li>
                        </ul>
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
                                            <asp:TextBox ID="txtPedidoFiltro" runat="server" Width="170px" Height="15px" onkeypress="return fnValidarNumero(event);"
                                                CssClass="alinharDireita"></asp:TextBox>
                                        </td>
                                        <td colspan="4">
                                            <asp:ImageButton ID="btPesquisar" runat="server" Width="19px" ImageUrl="~/Image/search.png"
                                                ToolTip="Pesquisar Pedidos" OnClick="btPesquisarPedido_Click" />
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
                                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btEditarPedido" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/edit.jpg"
                                                                    OnClick="btEditarPedido_Click" ToolTip="Editar" />
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:ImageButton ID="btIncluirPedido" runat="server" Height="18px" Width="18px" ImageUrl="~/Image/add.png"
                                                                    OnClick="btIncluirPedido_Click" ToolTip="Incluir" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Pedido" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labNumeroPedido" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Fornecedor" ItemStyle-Width="" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labFornecedor" runat="server" Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Material" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labMaterial" runat="server" Font-Size="Smaller" Text="-"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Grupo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labGrupoMaterial" runat="server" Font-Size="Smaller"></asp:Label>
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
                                                        <asp:TemplateField HeaderText="Qtde Compra" ItemStyle-Width="110px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labQtde" runat="server" Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qtde Recebida" ItemStyle-Width="110px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labQtdeEntregue" runat="server" Font-Size="Smaller"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="A Receber" ItemStyle-Width="110px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labQtdeFalt" runat="server" Font-Size="Smaller"></asp:Label>
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
                                <fieldset>
                                    <asp:ImageButton ID="ibtIncluir" runat="server" Width="20px" ImageUrl="~/Image/add.png"
                                        ToolTip="Novo Pedido" OnClick="ibtPedidoNovo_Click" />
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                        <tr>
                                            <td>
                                                <asp:Label ID="labNumeroPedido" runat="server" Text="Número Pedido" Enabled="false"></asp:Label>
                                                <asp:HiddenField ID="hidCodigoPedido" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="labMaterialGrupoPedido" runat="server" Text="Grupo"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labMaterialSubGrupoPedido" runat="server" Text="SubGrupo"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labCorPedido" runat="server" Text="Cor"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labCorFornecedorPedido" runat="server" Text="Cor Fornecedor"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labFornecedorPedido" runat="server" Text="Fornecedor"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 180px;">
                                                <asp:TextBox ID="txtPedidoNumero" runat="server" Width="170px" Enabled="false"
                                                    CssClass="alinharDireita" OnTextChanged="txtPedidoNumero_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            </td>
                                            <td style="width: 210px;">
                                                <asp:DropDownList ID="ddlMaterialGrupoPedido" runat="server" Width="204px" Height="22px"
                                                    DataTextField="GRUPO" DataValueField="CODIGO_GRUPO" OnSelectedIndexChanged="ddlMaterialGrupo_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 240px;">
                                                <asp:DropDownList ID="ddlMaterialSubGrupoPedido" runat="server" Width="234px" Height="22px"
                                                    DataTextField="SUBGRUPO" DataValueField="CODIGO_SUBGRUPO" OnSelectedIndexChanged="ddlMaterialSubGrupo_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 210px;">
                                                <asp:DropDownList ID="ddlCorPedido" runat="server" Width="204px" Height="22px" DataTextField="DESC_COR"
                                                    DataValueField="COR" OnSelectedIndexChanged="ddlCor_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 270px;">
                                                <asp:DropDownList ID="ddlCorFornecedorPedido" runat="server" Width="264px" Height="22px"
                                                    DataTextField="REFER_FABRICANTE" DataValueField="REFER_FABRICANTE">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlPedidoFornecedor" runat="server" Width="245px" Height="22px"
                                                    DataTextField="FORNECEDOR" DataValueField="FORNECEDOR" OnSelectedIndexChanged="ddlPedidoFornecedor_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="labQtdeTotal" runat="server" Text="Quantidade Total"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labQtdeEntregue" runat="server" Text="Quantidade Recebida"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labUnidadeMedida" runat="server" Text="Unidade de Medida"></asp:Label>
                                            </td>
                                            <td colspan="2">&nbsp;
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtQtdeTotal" runat="server" Width="170px" MaxLength="10" Text="0" Style="text-align: right;" Enabled="false"
                                                    onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtQtdeEntregue" runat="server" Width="200px" MaxLength="10" Text="0" Style="text-align: right;" Enabled="false"
                                                    onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlUnidade" Height="22px" runat="server" DataValueField="CODIGO"
                                                    DataTextField="DESCRICAO" Width="234px">
                                                </asp:DropDownList>
                                            </td>
                                            <td colspan="2">&nbsp;
                                            </td>
                                            <td style="text-align: right;">
                                                <asp:Button ID="btSalvarPedido" runat="server" Width="120px" Text="Salvar Pedido" ToolTip="Salvar" OnClick="btSalvarPedido_Click" />
                                            </td>

                                        </tr>

                                        <tr>
                                            <td colspan="6">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">
                                                <asp:Button ID="btAbrirSubPedido" runat="server" Width="173px" Text="Abrir Compra" ToolTip="Abrir Sub Pedidos" OnClick="btAbrirSubPedido_Click" />
                                            </td>
                                            <td style="text-align: right;">
                                                <asp:Button ID="btAtualizarSubPedido" runat="server" Width="170px" Text="Atualizar" ToolTip="Atualizar Sub Pedidos" OnClick="btAtualizarSubPedido_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <fieldset>
                                                    <legend>Pedidos</legend>
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
                                                                <asp:TemplateField HeaderText="Pedido" ItemStyle-Width="85px" HeaderStyle-HorizontalAlign="Center"
                                                                    ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="labNumeroSubPedido" runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Data Pedido" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"
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
                                                                <asp:TemplateField HeaderText="Qtde" ItemStyle-Width="95px" HeaderStyle-HorizontalAlign="Center"
                                                                    ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="labQtde" runat="server" Font-Size="Smaller"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Custo" ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="Center"
                                                                    ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="labCusto" runat="server" Font-Size="Smaller"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Val. Pedido" ItemStyle-Width="110px" HeaderStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left"
                                                                    ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="labValorTotal" runat="server" Font-Size="Smaller"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Ent Inicial" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"
                                                                    ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="labDataPedidoPrevisao" runat="server" Font-Size="Smaller"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Prev Final" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"
                                                                    ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="labDataPrevFinal" runat="server" Font-Size="Smaller"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Qtde Recebida" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
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
                                                                        <asp:ImageButton ID="btEditarPedidoSub" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/edit.jpg"
                                                                            OnClick="btEditarPedidoSub_Click" ToolTip="Editar Pedido" />
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
                                                                        <asp:ImageButton ID="btExcluirSubPedido" runat="server" Height="15px" Width="15px"
                                                                            ImageUrl="~/Image/delete.png" OnClick="btExcluirSubPedido_Click" OnClientClick="return Confirmar('Deseja realmente Excluir este SubPedido?');"
                                                                            ToolTip="Excluir Pedido" />
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
                                                </fieldset>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                        </div>
                        <div id="tabs-lote-pedido">
                            <fieldset>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                    <tr>
                                        <td>Número do Pedido
                                        </td>
                                        <td colspan="2">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 180px;">
                                            <asp:TextBox ID="txtPedidoLote" runat="server" Width="170px" Height="15px" onkeypress="return fnValidarNumero(event);"
                                                CssClass="alinharDireita"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="btPesquisarLote" runat="server" Width="19px" ImageUrl="~/Image/search.png"
                                                ToolTip="Pesquisar Pedidos" OnClick="btPesquisarLote_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <fieldset>
                                                <legend>Pedidos do Lote</legend>
                                                <div style="text-align: right;">
                                                    <asp:Button ID="btAtualizarCarrinho" runat="server" Text="Atualizar" Width="150px" OnClick="btAtualizarCarrinho_Click" />
                                                    <br />
                                                    <br />
                                                </div>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvCarrinho" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvCarrinho_RowDataBound" ShowFooter="true" OnDataBound="gvCarrinho_DataBound"
                                                        DataKeyNames="CODIGO">
                                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Pedido" HeaderStyle-Width="120px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litPedido" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Material" HeaderStyle-Width="150px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litMaterial" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Tecido">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litTecido" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Cor Fornecedor" HeaderStyle-Width="">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litCorFornecedor" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Qtde" HeaderStyle-Width="120px">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtQtde" runat="server" Width="120px" CssClass="alinharCentro" OnTextChanged="txtQtde_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Medida" HeaderStyle-Width="90px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litUnidadeMedida" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Preço" HeaderStyle-Width="120px">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtPreco" runat="server" Width="120px" OnTextChanged="txtPreco_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Total" HeaderStyle-Width="150px" FooterStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litValTotal" runat="server"></asp:Literal>
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
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvPedidoLote" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvPedidoLote_RowDataBound"
                                                    OnDataBound="gvPedidoLote_DataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Pedido" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labNumeroPedido" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Material" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labMaterial" runat="server" Font-Size="Smaller" Text="-"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Grupo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labGrupoMaterial" runat="server" Font-Size="Smaller"></asp:Label>
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
                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btAdicionarCarrinho" runat="server" Text="Adicionar" Width="100px" Height="21px" OnClick="btAdicionarCarrinho_Click" />
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
                        <div id="tabs-lote-pedido-res">
                            <fieldset>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                    <tr>
                                        <td>Número do Pedido
                                        </td>
                                        <td colspan="2">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 180px;">
                                            <asp:TextBox ID="txtPedidoLoteReserva" runat="server" Width="170px" Height="15px" onkeypress="return fnValidarNumero(event);"
                                                CssClass="alinharDireita"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="btPesquisarLoteReserva" runat="server" Width="19px" ImageUrl="~/Image/search.png"
                                                ToolTip="Pesquisar Pedidos" OnClick="btPesquisarLoteReserva_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <fieldset>
                                                <legend>Pedidos Reservados Do Lote</legend>
                                                <div style="text-align: right;">
                                                    <asp:Button ID="btAtualizarCarrinhoReserva" runat="server" Text="Atualizar" Width="150px" OnClick="btAtualizarCarrinhoReserva_Click" />
                                                    <br />
                                                    <br />
                                                </div>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvCarrinhoReserva" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvCarrinhoReserva_RowDataBound" ShowFooter="true" OnDataBound="gvCarrinhoReserva_DataBound"
                                                        DataKeyNames="CODIGO">
                                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Pedido" HeaderStyle-Width="120px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litPedido" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Material" HeaderStyle-Width="150px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litMaterial" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Tecido">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litTecido" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Cor Fornecedor" HeaderStyle-Width="">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litCorFornecedor" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Qtde" HeaderStyle-Width="120px">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtQtdeRes" runat="server" Width="120px" CssClass="alinharCentro" OnTextChanged="txtQtdeRes_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Medida" HeaderStyle-Width="90px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litUnidadeMedida" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Preço" HeaderStyle-Width="120px">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtPrecoRes" runat="server" Width="120px" OnTextChanged="txtPrecoRes_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Total" HeaderStyle-Width="150px" FooterStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litValTotal" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="25px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btExcluirItemCarrinhoReserva" runat="server" ImageUrl="~/Image/delete.png" Width="15px" Height="15px" OnClick="btExcluirItemCarrinhoReserva_Click" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                                <div>
                                                    <div style="float: left;">
                                                        <br />
                                                        <asp:Button ID="btExcluirCarrinhoReserva" runat="server" Text="Excluir Carrinho" Width="150px" OnClick="btExcluirCarrinhoReserva_Click" />
                                                    </div>
                                                    <div style="text-align: right;">
                                                        <br />
                                                        <asp:Label ID="labErroReserva" runat="server" ForeColor="Red" Text=""></asp:Label>
                                                        &nbsp;&nbsp;
                                                        <asp:Button ID="btGerarPedidoReserva" runat="server" OnClick="btGerarPedidoReserva_Click" Width="150px" Text="Gerar Pedido" />&nbsp;
                                                    </div>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvPedidoLoteRes" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvPedidoLoteRes_RowDataBound"
                                                    OnDataBound="gvPedidoLoteRes_DataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Pedido" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labNumeroPedido" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Material" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labMaterial" runat="server" Font-Size="Smaller" Text="-"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Grupo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labGrupoMaterial" runat="server" Font-Size="Smaller"></asp:Label>
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
                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btAdicionarCarrinhoReserva" runat="server" Text="Adicionar" Width="100px" Height="21px" OnClick="btAdicionarCarrinhoReserva_Click" />
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
