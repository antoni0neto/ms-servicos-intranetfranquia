<%@ Page Title="Digitação de Pedidos" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" 
    AutoEventWireup="true" CodeBehind="representante_dig_pedidos.aspx.cs" Inherits="Relatorios.representante_dig_pedidos" 
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
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

        .hiddencol
        {
            display: none;
        }
    </style>
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(IniEvents);

        function IniEvents() {
            $(function () {
                $("#tabs").tabs({
                    activate: function () {
                        var selectedTab = $('#tabs').tabs('option', 'active');
                        $("#<%= hidTabSelected.ClientID %>").val(selectedTab);
                    },
                    active: document.getElementById('<%= hidTabSelected.ClientID %>').value
                });
            });

            $(".txtselect").each(function (i, obj) {
                $(this).focusin(function () {
                    $(this).select();
                });
            });
        }
    </script>
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Representantes&nbsp;&nbsp;>&nbsp;&nbsp;Pedidos&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Pedidos</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <br />
            <div style="float: left; width: 1000px; margin-left: 15%;">
                <fieldset>
                    <legend>Cadastro de Pedidos</legend>
                    <div style="padding-left: 0px;">
                        <asp:ImageButton ID="ibtSalvar" runat="server" Width="19px" ImageUrl="~/Image/approve.png"
                            ToolTip="Salvar" OnClick="ibtSalvar_Click" />
                        <asp:ImageButton ID="ibtCancelar" runat="server" Width="18px" ImageUrl="~/Image/cancel.png"
                            ToolTip="Cancelar" OnClick="ibtCancelar_Click" />
                        <asp:ImageButton ID="ibtEditar" runat="server" Width="20px" ImageUrl="~/Image/edit.jpg"
                            ToolTip="Editar" OnClick="ibtEditar_Click" />
                        <asp:ImageButton ID="ibtExcluir" runat="server" Width="18px" ImageUrl="~/Image/delete.png"
                            ToolTip="Excluir" OnClick="ibtExcluir_Click" OnClientClick="return ConfirmarExclusao();" />
                        <asp:ImageButton ID="ibtLimpar" runat="server" Width="18px" ImageUrl="~/Image/clean.png"
                            ToolTip="Limpar" OnClick="ibtLimpar_Click" />
                        <asp:ImageButton ID="ibtPesquisar" runat="server" Width="19px" ImageUrl="~/Image/search.png"
                            ToolTip="Pesquisar" OnClick="ibtPesquisar_Click" />
                        <asp:HiddenField ID="hidTabSelected" runat="server" Value="" />
                        <asp:HiddenField ID="hidAcao" runat="server" Value="" />
                        <div style="text-align: right; float: right;">
                            <asp:Label ID="labErro" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                    <hr />
                    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                        <tr>
                            <td>
                                Cliente
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 210px;">
                                <asp:TextBox ID="txtNomeCliente" runat="server" Width="200px" Text="" ToolTip="Nome" placeholder="Nome"></asp:TextBox>
                            </td>
                            <td style="width: 210px;">
                                <asp:TextBox ID="txtCodigoCliente" runat="server" Width="200px" Text="" ToolTip="Código" placeholder="Código"></asp:TextBox>
                            </td>
                            <td style="width: 210px;">
                                <asp:TextBox ID="txtCnpjCliente" runat="server" Width="200px" Text="" ToolTip="CNPJ" placeholder="CNPJ"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server" id="trClientes" visible="false">
                            <td>
                                Cliente para Pedido
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbClientes" runat="server" Width="184px" Height="21px"
                                                    DataTextField="NOME_CLIFOR" DataValueField="CLIFOR" OnSelectedIndexChanged="cmbClientes_SelectedIndexChanged"
                                                    OnDataBinding="cmbClientes_DataBinding" AutoPostBack="true" AppendDataBoundItems="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Insc. Estadual
                            </td>
                            <td>
                                Endereço
                            </td>
                            <td>
                                Cidade
                            </td>
                            <td>
                                Estado
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtInscricaoEstadual" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEndereco" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCidade" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEstado" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Telefone 1
                            </td>
                            <td>
                                Telefone 2
                            </td>
                            <td>
                                Representante
                            </td>
                            <td>
                                Emissão
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtTelefone1" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTelefone2" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRepresentante" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmissao" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Coleção
                            </td>
                            <td>
                                Tab. de Preço
                            </td>
                            <td>
                                Cond. Pgto.
                            </td>
                            <td>
                                Transportadora
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtColecao" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTabPreco" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCondPgto" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                                <asp:HiddenField ID="hidCondPgto" runat="server" Value="04" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtTransportadora" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Email
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <div id="tabs">
                        <ul>
                            <li><a href="#tabs-produto" id="tabProduto" runat="server">Produto</a></li>
                            <li><a href="#tabs-lista-itens" id="tabListaItens" runat="server">Lista Itens</a></li>
                        </ul>
                        <div id="tabs-produto">
                            <fieldset style="margin-top: 4px; padding-top: 0;">
                                <legend>
                                        <asp:Label ID="lblProduto" runat="server" Text="Produto"></asp:Label>
                                </legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblProdutoFiltro" runat="server" Text="Produto"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtProdutoFiltro" runat="server" MaxLength="12" Width="180px" Height="16px"
                                                    OnTextChanged="txtProdutoFiltro_TextChanged" AutoPostBack="true" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblNomeProduto" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="text-align: center;">
                                                        <asp:Image ID="imgFotoPeca" runat="server" BorderColor="Black" BorderWidth="0" />
                                                        <asp:Image ID="imgFotoPeca2" runat="server" BorderColor="Black" BorderWidth="0" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblLabelGrupo" runat="server" Text="Grupo:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblGrupo" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblLabelSubgrupo" runat="server" Text="Subgrupo:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblSubgrupo" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblLabelTipo" runat="server" Text="Tipo:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblTipo" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblLabelGriffe" runat="server" Text="Griffe:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblGriffe" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblLabelLinha" runat="server" Text="Linha:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblLinha" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblLabelColecao" runat="server" Text="Coleção:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblColecao" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblLabelModelagem" runat="server" Text="Modelagem:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblModelagem" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblLabelCartela" runat="server" Text="Cartela:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblCartela" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblLabelPrecoVenda" runat="server" Text="Preço de Venda:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblPrecoVenda" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblLabelGrade" runat="server" Text="Grade:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblGrade" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <div id="divTableAdd" runat="server" visible="false">
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr align="center" style="background-color: ActiveCaption;">
                                                <th scope="col" style="display: none;">&nbsp;</th>
                                                <th scope="col" align="left">Cor/Grade</th>
                                                <th scope="col" align="center">Preço</th>
                                                <th scope="col" align="center">XP</th>
                                                <th scope="col" align="center">PP</th>
                                                <th scope="col" align="center">PQ</th>
                                                <th scope="col" align="center">MD</th>
                                                <th scope="col" align="center">GD</th>
                                                <th scope="col" align="center">GG</th>
                                            </tr>
                                            <asp:Repeater ID="rptProdutos" runat="server" OnItemDataBound="rptProdutos_ItemDataBound">
                                                <ItemTemplate>
                                        
                                                            <tr align="center" style="background-color: lightgray;">
                                                                <td style="display: none;">
                                                                    &nbsp;
                                                                </t>
                                                                <td style="width: 200px;" align="left">
                                                                    Estoque Pronta Entrega
                                                                </td>
                                                                <td>&nbsp;</td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblQuantEstoqueXP" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CO1").ToString().Trim() %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblQuantEstoquePP" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CO2").ToString().Trim() %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblQuantEstoquePQ" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CO3").ToString().Trim() %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblQuantEstoqueMD" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CO4").ToString().Trim() %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblQuantEstoqueGD" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CO5").ToString().Trim() %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblQuantEstoqueGG" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CO6").ToString().Trim() %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="display: none;">
                                                                    <asp:Label ID="lblIdentificador" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PRODUTO") + "|" + DataBinder.Eval(Container.DataItem, "COR") %>'></asp:Label>
                                                                    <asp:Label ID="lblEntrega" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ENTREGA") %>'></asp:Label>
                                                                    <asp:Label ID="lblLimiteEntrega" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LIMITE_ENTREGA") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblAddCor" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "COR").ToString().Trim() %>'></asp:Label>
                                                                    &nbsp;-&nbsp;
                                                                    <asp:Label ID="lblAddCorDesc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DESC_COR").ToString().Trim() %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblAddPreco" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PRECO1").ToString().Trim() %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtAddXP" runat="server" MaxLength="4" Text="0" style="width: 40px; text-align: right;" class="btnselect"></asp:TextBox>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtAddPP" runat="server" MaxLength="4" Text="0" style="width: 40px; text-align: right;" class="txtselect"></asp:TextBox>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtAddPQ" runat="server" MaxLength="4" Text="0" style="width: 40px; text-align: right;" class="txtselect"></asp:TextBox>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtAddMD" runat="server" MaxLength="4" Text="0" style="width: 40px; text-align: right;" class="txtselect"></asp:TextBox>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtAddGD" runat="server" MaxLength="4" Text="0" style="width: 40px; text-align: right;" class="txtselect"></asp:TextBox>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:TextBox ID="txtAddGG" runat="server" MaxLength="4" Text="0" style="width: 40px; text-align: right;" class="txtselect"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <tr>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="right" colspan="9">
                                                    <asp:Button ID="btnAdicionarProduto" runat="server" Text="Adicionar" OnClick="btnAdicionarProduto_Click" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </fieldset>
                        </div>
                        <div id="tabs-lista-itens">
                            <div class="rounded_corners">
                                <asp:GridView ID="grdItens" runat="server" Width="100%"
                                                    ForeColor="#333333" Style="background: white" OnRowDataBound="grdItens_RowDataBound"
                                                    ShowFooter="true" DataKeyNames="PRODUTO, COR_PRODUTO">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemIndex" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="25px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btEditarItem" runat="server" Height="15px" Width="15px"
                                                    ImageUrl="~/Image/edit.jpg" OnClick="btEditarItem_Click" ToolTip="Editar" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PRODUTO" HeaderText="" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                        <asp:BoundField DataField="PRODUTO_DESC" HeaderText="Produto" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="COR_PRODUTO" HeaderText="" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                        <asp:BoundField DataField="COR_PRODUTO_DESC" HeaderText="Cor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ENTREGA" HeaderText="" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                        <asp:BoundField DataField="LIMITE_ENTREGA" HeaderText="" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                        <asp:BoundField DataField="QTDE_ORIGINAL" HeaderText="" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                        <asp:BoundField DataField="VALOR_ORIGINAL" HeaderText="" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                        <asp:BoundField DataField="PRECO1" HeaderText="" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                        <asp:BoundField DataField="VO1" HeaderText="XP" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="VO2" HeaderText="PP" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="VO3" HeaderText="PQ" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="VO4" HeaderText="MD" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="VO5" HeaderText="GD" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="VO6" HeaderText="GG" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="COMISSAO_ITEM" HeaderText="" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                        <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btExcluirMaterial" runat="server" Height="15px" Width="15px"
                                                    ImageUrl="~/Image/delete.png" OnClick="btExcluirMaterial_Click" OnClientClick="return ConfirmarExclusao();"
                                                    ToolTip="Excluir" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </fieldset>
            </div>
            <asp:HiddenField ID="hdfCondPgto" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(IniEvents);
    </script>
</asp:Content>