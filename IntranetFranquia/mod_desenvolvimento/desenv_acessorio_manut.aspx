<%@ Page Title="Desenvolvimento de Coleção de Acessórios" Language="C#" MasterPageFile="~/Site.Master"
    EnableEventValidation="false" AutoEventWireup="true" CodeBehind="desenv_acessorio_manut.aspx.cs" Inherits="Relatorios.desenv_acessorio_manut" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .jGrowl .manilla {
            background-color: #000;
            color: white;
        }

        .fs {
            border: 1px solid #ccc;
            font-family: Calibri;
        }

        .alinharDireita {
            text-align: right;
        }

        #drop_zone {
            margin: 10px 0;
            width: 324px;
            min-height: 100px;
            text-align: center;
            text-transform: uppercase;
            font-weight: normal;
            border: 5px dashed #CCC;
            height: 100px;
        }
    </style>
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Planejamento de Coleção de Acessórios&nbsp;&nbsp;>&nbsp;&nbsp;Desenvolvimento de Coleção de Acessórios</span>
        <div style="float: right; padding: 0;">
            <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <fieldset style="background-color: White;">
        <legend>
            <asp:Label ID="labTitulo" runat="server" Text="Desenvolvimento de Coleção de Acessórios"></asp:Label></legend>
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:Label ID="labColecao" runat="server" Text="Coleção"></asp:Label>
                </td>
                <td>&nbsp;
                </td>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 180px;">
                    <asp:DropDownList ID="ddlColecoes" runat="server" Width="174px" Height="21px" DataTextField="DESC_COLECAO"
                        DataValueField="COLECAO" AutoPostBack="true" OnSelectedIndexChanged="ddlColecoes_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style="width: 180px;">
                    <asp:Button ID="btAtualizar" runat="server" Text="Atualizar" Width="100px" OnClick="btAtualizar_Click" OnClientClick="DesabilitarBotao(this);" />
                </td>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlTripa" runat="server" Visible="false">
            <asp:UpdatePanel ID="up1" runat="server">
                <ContentTemplate>
                    <fieldset style="margin-top: 0px; background-color: White;">
                        <legend>
                            <asp:Label ID="labAcao" runat="server" Text="Novo Acessório"></asp:Label>
                        </legend>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0" style="margin-top: 0px;">
                            <tr>
                                <td>
                                    <asp:Label ID="labOrigem" runat="server" Text="Origem"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labGrupo" runat="server" Text="Grupo"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labProduto" runat="server" Text="Produto"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labGriffe" runat="server" Text="Griffe"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="labFornecedor" runat="server" Text="Fornecedor"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="labObservacao" runat="server" Text="Observação"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 180px;">
                                    <asp:DropDownList ID="ddlOrigem" runat="server" Width="174px" Height="21px" DataTextField="DESCRICAO"
                                        DataValueField="CODIGO">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 180px;">
                                    <asp:DropDownList ID="ddlGrupo" runat="server" Width="174px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                        DataValueField="GRUPO_PRODUTO">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 180px;">
                                    <asp:TextBox ID="txtProduto" runat="server" Width="149px" MaxLength="20"></asp:TextBox>
                                    <asp:ImageButton ID="btObterSeqProduto" runat="server" AlternateText=" " Width="17px" Height="17px" ImageAlign="AbsMiddle" ToolTip="Sequência de Produto" ImageUrl="~/Image/update.png" OnClick="btObterSeqProduto_Click"  />
                                </td>
                                <td style="width: 150px;">
                                    <asp:DropDownList ID="ddlGriffe" runat="server" Width="144px" Height="21px" DataTextField="GRIFFE"
                                        DataValueField="GRIFFE">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="3" style="width: 360px;">
                                    <asp:DropDownList ID="ddlFornecedor" runat="server" Width="354px" Height="21px" DataTextField="FORNECEDOR"
                                        DataValueField="FORNECEDOR">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2" style="width: 350px;">
                                    <asp:TextBox ID="txtObservacao" runat="server" Width="340px" MaxLength="40"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="9">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="labCor" runat="server" Text="Cor"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labCorFornecedor" runat="server" Text="Cor Fornecedor"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labDescricaoSugerida" runat="server" Text="Descrição Sugerida"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labReferFabricante" runat="server" Text="Referência Fabricante"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labPreco" runat="server" Text="Preço"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labCusto" runat="server" Text="Custo"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labQuantidade" runat="server" Text="Quantidade"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labPrevEntrega" runat="server" Text="Previsão Entrega"></asp:Label>
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlCor" runat="server" Width="174px" Height="21px" DataValueField="COR" DataTextField="DESC_COR">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCorFornecedor" runat="server" Width="170px" MaxLength="40"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDescricaoSugerida" runat="server" Width="170px" MaxLength="150"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReferFabricante" runat="server" Width="140px" MaxLength="150"></asp:TextBox>
                                </td>
                                <td style="width: 120px;">
                                    <asp:TextBox ID="txtPreco" runat="server" Width="110px" MaxLength="10"></asp:TextBox>
                                </td>
                                <td style="width: 120px;">
                                    <asp:TextBox ID="txtCusto" runat="server" Width="110px" MaxLength="10"></asp:TextBox>
                                </td>
                                <td style="width: 120px;">
                                    <asp:TextBox ID="txtQuantidade" runat="server" Width="110px" MaxLength="10"></asp:TextBox>
                                </td>
                                <td style="width: 120px;">
                                    <asp:TextBox ID="txtDataPrevEntrega" runat="server" Width="110px" MaxLength="10" onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="btSalvar" runat="server" Text="Incluir" Width="108px" OnClick="btSalvar_Click" />&nbsp;
                                    <asp:Button ID="btLimpar" runat="server" Text="Limpar Campos" Width="108px" OnClick="btLimpar_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="9">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="9">
                                    <asp:Label ID="labSalvar" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="9">
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="9">Total de Modelos:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="labModeloFiltroValor"
                                    ForeColor="Green" Font-Bold="true" runat="server" Text="150"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="9">Total de SKUs:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                                    ID="labEstiloFiltroValor" runat="server" ForeColor="Green" Font-Bold="true" Text="250"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table border="0" cellpadding="0" cellspacing="0" width="3500px" style="margin-top: 0px;">
                            <tr>
                                <td style="width: 30px;">&nbsp;
                                </td>
                                <td style="width: 35px;">&nbsp;
                                </td>
                                <td style="width: 35px;">&nbsp;
                                </td>
                                <td style="width: 40px;">&nbsp;
                                </td>
                                <td style="width: 27px;">
                                    <asp:CheckBox ID="chkFoto" runat="server" Checked="false" AutoPostBack="true" ToolTip="Filtrar Acessório sem Foto"
                                        OnCheckedChanged="chkFoto_CheckedChanged" />
                                </td>
                                <td style="width: 104px;">
                                    <asp:DropDownList ID="ddlProdutoFiltro" runat="server" Width="104px" DataTextField="PRODUTO"
                                        DataValueField="PRODUTO" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 163px;">
                                    <asp:DropDownList ID="ddlGrupoFiltro" runat="server" Width="163px" DataTextField="GRUPO_PRODUTO"
                                        DataValueField="GRUPO_PRODUTO" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 177px;">
                                    <asp:DropDownList ID="ddlNomeFiltro" runat="server" Width="177px" DataTextField="NOME"
                                        DataValueField="NOME" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 148px;">
                                    <asp:DropDownList ID="ddlOrigemFiltro" runat="server" Width="148px" DataTextField="DESCRICAO"
                                        DataValueField="CODIGO" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 152px;">
                                    <asp:DropDownList ID="ddlGriffeFiltro" runat="server" Width="152px" DataTextField="GRIFFE"
                                        DataValueField="GRIFFE" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 199px;">
                                    <asp:DropDownList ID="ddlCorFiltro" runat="server" Width="199px" DataTextField="DESC_COR"
                                        DataValueField="COR" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 294px;">
                                    <asp:DropDownList ID="ddlCorFornecedorFiltro" runat="server" Width="294px" DataTextField="COR_FORNECEDOR"
                                        DataValueField="COR_FORNECEDOR" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 252px;">
                                    <asp:DropDownList ID="ddlFornecedorFiltro" runat="server" Width="252px" DataTextField="FORNECEDOR"
                                        DataValueField="FORNECEDOR" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 133px;">
                                    <asp:DropDownList ID="ddlPrecoFiltro" runat="server" Width="133px" DataTextField="PRECO"
                                        DataValueField="PRECO" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 133px;">
                                    <asp:DropDownList ID="ddlCustoFiltro" runat="server" Width="133px" DataTextField="CUSTO"
                                        DataValueField="CUSTO" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 133px;">
                                    <asp:DropDownList ID="ddlQuantidadeFiltro" runat="server" Width="133px" DataTextField="QTDE"
                                        DataValueField="QTDE" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 153px;">
                                    <asp:DropDownList ID="ddlDataPrevisaoEntregaFiltro" runat="server" Width="153px" DataTextField="DATA_PREVISAO_ENTREGA"
                                        DataValueField="DATA_PREVISAO_ENTREGA" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 203px;">
                                    <asp:DropDownList ID="ddlDescricaoSugeridaFiltro" runat="server" Width="203px" DataTextField="DESCRICAO_SUGERIDA"
                                        DataValueField="DESCRICAO_SUGERIDA" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 203px;">
                                    <asp:DropDownList ID="ddlReferenciaFabricanteFiltro" runat="server" Width="203px" DataTextField="REFER_FABRICANTE"
                                        DataValueField="REFER_FABRICANTE" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 0px;">
                                    <asp:DropDownList ID="ddlPedidoFiltro" runat="server" Width="150px" DataTextField="PEDIDO" Visible="false"
                                        DataValueField="PEDIDO" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 500px;">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="21">
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvAcessorio" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ForeColor="#333333" Style="background: white" ShowFooter="true" OnRowDataBound="gvAcessorio_RowDataBound"
                                            DataKeyNames="CODIGO" AllowSorting="true" OnSorting="gvAcessorio_Sorting">
                                            <HeaderStyle BackColor="Gainsboro" />
                                            <FooterStyle BackColor="Gainsboro" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-Font-Bold="true"
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>' CssClass="alink" OnClick="lnkColuna_Click"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btAcessorioEditar" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/edit.jpg"
                                                            OnClick="btAcessorioEditar_Click" ToolTip="Editar" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btAcessorioCopiar" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/copy.png"
                                                            OnClick="btAcessorioCopiar_Click" ToolTip="Copiar" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btAcessorioExcluir" runat="server" Width="14px" CommandName="EX"
                                                            ImageUrl="~/Image/delete.png" OnClick="btAcessorioExcluir_Click" ToolTip="Excluir" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btAcessorioVisualizar" runat="server" Height="17px" Width="17px"
                                                            ImageUrl="~/Image/eye.png" OnClick="btAcessorioVisualizar_Click" ToolTip="Visualizar Foto" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Produto" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px"
                                                    SortExpression="ddlProdutoFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labProduto" runat="server" Text='<%# Bind("PRODUTO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Grupo" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="160px"
                                                    SortExpression="ddlGrupoFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labGrupo" runat="server" Text='<%# Bind("GRUPO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nome" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="175px"
                                                    SortExpression="ddlNomeFiltro">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNome" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Origem" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="146px"
                                                    SortExpression="ddlOrigemFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labOrigem" runat="server" Text='<%# Bind("DESCRICAO_ORIGEM") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Griffe" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="148px"
                                                    SortExpression="ddlGriffeFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labGriffe" runat="server" Text='<%# Bind("GRIFFE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cor" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="196px"
                                                    SortExpression="ddlCorFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labCor" runat="server" Text='<%# Bind("DESC_COR") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cor Fornecedor" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="290px" SortExpression="ddlCorFornecedorFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labCorFornecedor" runat="server" Text='<%# Bind("COR_FORNECEDOR") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Fornecedor" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="250px"
                                                    SortExpression="ddlFornecedorFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labFornecedor" runat="server" Text='<%# Bind("FORNECEDOR") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Preço" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="130px" SortExpression="ddlPrecoFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labPreco" Text='<%# Bind("PRECO") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Custo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="130px" SortExpression="ddlCustoFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labCusto" Text='<%# Bind("CUSTO") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Quantidade" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="130px" SortExpression="ddlQuantidadeFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labQuantidade" Text='<%# Bind("QTDE") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Previsão Entrega" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="150px" SortExpression="ddlDataPrevisaoEntregaFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labDataPrevisaoEntrega" runat="server" Text='<%# Bind("DATA_PREVISAO_ENTREGA") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Descrição Sugerida" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="200px" SortExpression="ddlDescricaoSugeridaFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labDescricaoSugerida" runat="server" Text='<%# Bind("DESCRICAO_SUGERIDA") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Referência Fabricante" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="200px" SortExpression="ddlReferenciaFabricanteFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labReferenciaFabricante" runat="server" Text='<%# Bind("REFER_FABRICANTE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false" HeaderText="Pedido" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="146px"
                                                    SortExpression="ddlPedidoFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labPedido" runat="server" Text='<%# Bind("PEDIDO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false" HeaderText="Quantidade" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="130px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labQtde" runat="server" Text='<%# Bind("QTDE_ORIGINAL") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false" HeaderText="Data do Pedido" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="130px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labDataPedido" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Observação" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labObservacao" runat="server" Text='<%# Bind("OBS") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="20">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="20">&nbsp;
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </fieldset>
</asp:Content>
