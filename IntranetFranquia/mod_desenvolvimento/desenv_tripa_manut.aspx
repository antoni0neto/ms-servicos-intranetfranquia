<%@ Page Title="Desenvolvimento de Coleção" Language="C#" MasterPageFile="~/Site.Master"
    MaintainScrollPositionOnPostback="true" EnableEventValidation="false" AutoEventWireup="true"
    CodeBehind="desenv_tripa_manut.aspx.cs" Inherits="Relatorios.desenv_tripa_manut" %>

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
            de Coleção Nacional&nbsp;&nbsp;>&nbsp;&nbsp;Desenvolvimento de Coleção</span>
        <div style="float: right; padding: 0;">
            <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <fieldset style="background-color: White;">
        <legend>
            <asp:Label ID="labTitulo" runat="server" Text="Desenvolvimento de Coleção"></asp:Label></legend>
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:Label ID="labMarca" runat="server" Text="Marca"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="labColecao" runat="server" Text="Coleção"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="labOrigemFiltroIni" runat="server" Text="Origem"></asp:Label>
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
                    <asp:DropDownList ID="ddlColecoes" runat="server" Width="174px" Height="21px" DataTextField="DESC_COLECAO"
                        DataValueField="COLECAO" AutoPostBack="true" OnSelectedIndexChanged="ddlColecoes_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style="width: 260px;">
                    <asp:DropDownList ID="ddlOrigemFiltroIni" runat="server" Width="254px" Height="21px" DataTextField="DESCRICAO"
                        DataValueField="CODIGO">
                    </asp:DropDownList>
                </td>
                <td style="width: 180px;">
                    <asp:Button ID="btAtualizar" runat="server" Text="Atualizar" Width="100px" OnClick="btAtualizar_Click" OnClientClick="DesabilitarBotao(this);" />
                </td>
                <td>
                    <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="5">&nbsp;
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlTripa" runat="server" Visible="false">
            <asp:UpdatePanel ID="up1" runat="server">
                <ContentTemplate>
                    <fieldset style="margin-top: 0px; background-color: White;">
                        <%--                        <legend>
                            <asp:Label ID="labAcao" runat="server" Text="Novo Produto"></asp:Label>
                        </legend>--%>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0" style="margin-top: 0px; visibility: collapse;">
                            <tr>
                                <td colspan="2">
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
                                <td>
                                    <asp:Label ID="labSegmento" runat="server" Text="Segmento"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labTecido" runat="server" Text="Tecido"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="labFornecedor" runat="server" Text="Fornecedor"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlOrigem" runat="server" Width="354px" Height="21px" DataTextField="DESCRICAO"
                                        DataValueField="CODIGO">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtCodigoRef" runat="server" Width="170px" MaxLength="8" CssClass="alinharDireita" Enabled="false"
                                        onkeypress="return fnValidarNumero(event);" Visible="false"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlGrupo" runat="server" Width="154px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                        DataValueField="GRUPO_PRODUTO">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtProduto" runat="server" Width="150px" MaxLength="20"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlGriffe" runat="server" Width="154px" Height="21px" DataTextField="GRIFFE"
                                        DataValueField="GRIFFE">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 170px;">
                                    <asp:TextBox ID="txtSegmento" runat="server" Width="160px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td style="width: 170px;">
                                    <asp:TextBox ID="txtTecido" runat="server" Width="160px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlFornecedor" runat="server" Width="245px" Height="21px" DataTextField="FORNECEDOR"
                                        DataValueField="FORNECEDOR">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="labRefModelagem" runat="server" Text="REF de Modelagem"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labPrecoVendaVarejo" runat="server" Text="Preço Venda Varejo"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labPrecoVendaAtacado" runat="server" Text="Preço Venda Atacado"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labBrinde" runat="server" Text="Brinde"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labProdutoAcabado" runat="server" Text="Produto Acabado"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labLinha" runat="server" Text="Linha"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labObservacao" runat="server" Text="Observação"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labSigned" runat="server" Text="Signed"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labSignedNome" runat="server" Text="Signed Nome"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 180px;">
                                    <asp:TextBox ID="txtREFModelagem" runat="server" Width="170px" MaxLength="26"></asp:TextBox>
                                </td>
                                <td style="width: 180px;">
                                    <asp:TextBox ID="txtPrecoVendaVarejo" runat="server" Width="170px" MaxLength="10"
                                        CssClass="alinharDireita" OnTextChanged="txtPreco_TextChanged" AutoPostBack="true" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                </td>
                                <td style="width: 160px;">
                                    <asp:TextBox ID="txtPrecoVendaAtacado" runat="server" Width="150px" MaxLength="10"
                                        CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                </td>
                                <td style="width: 160px;">
                                    <asp:DropDownList ID="ddlBrinde" runat="server" Width="154px" Height="21px">
                                        <asp:ListItem Value="N" Text="Não" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 160px;">
                                    <asp:DropDownList ID="ddlProdutoAcabado" runat="server" Width="154px" Height="21px">
                                        <asp:ListItem Value="N" Text="Não" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlLinha" runat="server" Width="164px" DataValueField="LINHA" DataTextField="LINHA" Height="22px"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtObservacao" runat="server" Width="160px" MaxLength="30"></asp:TextBox>
                                </td>
                                <td style="width: 100px;">
                                    <asp:DropDownList ID="ddlSigned" runat="server" Width="94px" Height="21px">
                                        <asp:ListItem Value="N" Text="Não" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSignedNome" runat="server" Width="141px" MaxLength="30"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="9">
                                    <asp:Label ID="labObsImpressao" runat="server" Text="Observação Impressão"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="9">
                                    <asp:TextBox ID="txtObsImpressao" runat="server" Width="1420px" Height="50px" TextMode="MultiLine"></asp:TextBox>
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
                                    <asp:Label ID="labQtdeMostruario" runat="server" Text="Qtde Mostruário"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labQtdeVarejo" runat="server" Text="Qtde Varejo"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labQtdeAtacado" runat="server" Text="Qtde Atacado"></asp:Label>
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                                <td colspan="2">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlCor" runat="server" Width="174px" Height="21px" DataValueField="COR" DataTextField="DESC_COR">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCorFornecedor" runat="server" Width="170px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtQtdeMostruario" runat="server" Width="150px" MaxLength="10" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtQtdeVarejo" runat="server" Width="150px" MaxLength="10" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtQtdeAtacado" runat="server" Width="150px" MaxLength="10" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="btSalvar" runat="server" Text="Incluir" Width="100px" OnClick="btSalvar_Click" />
                                </td>
                                <td>&nbsp;
                                </td>
                                <td colspan="2">
                                    <asp:Button ID="btLimpar" runat="server" Text="Limpar Campos" Width="130px" OnClick="btLimpar_Click" />&nbsp;&nbsp;
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
                        </table>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0" style="margin-top: 0px;">
                            <tr>
                                <td>Total de Modelos:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="labModeloFiltroValor"
                                    ForeColor="Green" Font-Bold="true" runat="server" Text="150"></asp:Label>
                                </td>
                                <td>&nbsp;
                                </td>
                                <td colspan="7">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>Total de SKUs:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                                    ID="labEstiloFiltroValor" runat="server" ForeColor="Green" Font-Bold="true" Text="250"></asp:Label>
                                </td>
                                <td>&nbsp;
                                </td>
                                <td colspan="7">&nbsp;
                                </td>
                            </tr>
                        </table>
                        <table border="0" cellpadding="0" cellspacing="0" width="4970px" style="margin-top: 0px;">
                            <tr>
                                <td style="width: 41px;">&nbsp;
                                </td>
                                <td style="width: 40px;">&nbsp;
                                </td>
                                <td style="width: 40px;">&nbsp;
                                </td>
                                <td style="width: 40px;">&nbsp;
                                </td>
                                <td style="width: 45px;">&nbsp;
                                </td>
                                <td style="width: 39px;">
                                    <asp:CheckBox ID="chkFoto" runat="server" Checked="false" AutoPostBack="true" ToolTip="Filtrar Produto sem Foto"
                                        OnCheckedChanged="chkFoto_CheckedChanged" />
                                </td>
                                <td style="width: 31px;">
                                    <asp:CheckBox ID="chkAprovar" runat="server" Checked="false" AutoPostBack="true"
                                        ToolTip="Filtrar Produto sem Aprovação" OnCheckedChanged="chkAprovar_CheckedChanged" />
                                </td>
                                <td style="width: 106px;">
                                    <asp:DropDownList ID="ddlProdutoFiltro" runat="server" Width="106px" DataTextField="MODELO"
                                        DataValueField="MODELO" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 199px;">
                                    <asp:DropDownList ID="ddlGrupoFiltro" runat="server" Width="199px" DataTextField="GRUPO_PRODUTO"
                                        DataValueField="GRUPO_PRODUTO" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 184px;">
                                    <asp:DropDownList ID="ddlNomeFiltro" runat="server" Width="184px" DataTextField="NOME"
                                        DataValueField="NOME" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 150px;">
                                    <asp:DropDownList ID="ddlOrigemFiltro" runat="server" Width="150px" DataTextField="DESCRICAO"
                                        DataValueField="CODIGO" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 156px;">
                                    <asp:DropDownList ID="ddlGriffeFiltro" runat="server" Width="156px" DataTextField="GRIFFE"
                                        DataValueField="GRIFFE" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 226px;">
                                    <asp:DropDownList ID="ddlSegmentoFiltro" runat="server" Width="226px" DataTextField="SEGMENTO"
                                        DataValueField="SEGMENTO" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 247px;">
                                    <asp:DropDownList ID="ddlTecidoFiltro" runat="server" Width="247px" DataTextField="TECIDO_POCKET"
                                        DataValueField="TECIDO_POCKET" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 202px;">
                                    <asp:DropDownList ID="ddlCorFiltro" runat="server" Width="202px" DataTextField="DESC_COR"
                                        DataValueField="COR" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 321px;">
                                    <asp:DropDownList ID="ddlCorFornecedorFiltro" runat="server" Width="321px" DataTextField="FORNECEDOR_COR"
                                        DataValueField="FORNECEDOR_COR" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 183px;">&nbsp;
                                </td>
                                <td style="width: 255px;">&nbsp;
                                </td>
                                <td style="width: 179px;">&nbsp;
                                </td>
                                <td style="width: 180px;">&nbsp;
                                </td>
                                <td style="width: 178px;">&nbsp;
                                </td>
                                <td style="width: 200px;">&nbsp;
                                </td>
                                <td style="width: 205px;">
                                    <asp:DropDownList ID="ddlFornecedorFiltro" runat="server" Width="205px" DataTextField="FORNECEDOR"
                                        DataValueField="FORNECEDOR" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 141px;">
                                    <asp:DropDownList ID="ddlMostruarioCorteFiltro" runat="server" Width="141px" DataTextField="MOSTRUARIOCORTE"
                                        DataValueField="MOSTRUARIOCORTE" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 321px;">
                                    <asp:DropDownList ID="ddlRefModelagemFiltro" runat="server" Width="321px" DataTextField="REF_MODELAGEM"
                                        DataValueField="REF_MODELAGEM" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 236px;">&nbsp;
                                </td>
                                <td style="width: 108px;">
                                    <asp:DropDownList ID="ddlVarejoFiltro" runat="server" Width="108px" DataTextField="CORTE_VAREJO"
                                        DataValueField="CORTE_VAREJO" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 109px;">
                                    <asp:DropDownList ID="ddlAtacadoFiltro" runat="server" Width="109px" DataTextField="CORTE_ATACADO"
                                        DataValueField="CORTE_ATACADO" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 89px;">
                                    <asp:DropDownList ID="ddlBrindeFiltro" runat="server" Width="89px" DataTextField="BRINDE"
                                        DataValueField="BRINDE" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 130px;">
                                    <asp:DropDownList ID="ddlProdutoAcabadoFiltro" runat="server" Width="130px" DataTextField="PRODUTO_ACABADO"
                                        DataValueField="PRODUTO_ACABADO" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 78px;">
                                    <asp:DropDownList ID="ddlSignedFiltro" runat="server" Width="78px" DataTextField="SIGNED"
                                        DataValueField="SIGNED" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSignedNomeFiltro" runat="server" Width="165px" DataTextField="SIGNED_NOME"
                                        DataValueField="SIGNED_NOME" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="32">
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ForeColor="#333333" Style="background: white" ShowFooter="true" OnRowDataBound="gvProduto_RowDataBound"
                                            DataKeyNames="CODIGO" AllowSorting="true" OnSorting="gvProduto_Sorting">
                                            <HeaderStyle BackColor="Gainsboro" />
                                            <FooterStyle BackColor="Gainsboro" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-Font-Bold="true"
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="30px" />
                                                    <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btProdutoImprimir" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/print.png"
                                                            OnClick="btProdutoImprimir_Click" ToolTip="Imprimir" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="35px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btProdutoEditar" runat="server" Height="15px" Width="15px" Enabled="false" ImageUrl="~/Image/edit.jpg"
                                                            OnClick="btProdutoEditar_Click" ToolTip="Editar" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="35px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btProdutoCopiar" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/copy.png"
                                                            OnClick="btProdutoCopiar_Click" ToolTip="Copiar" Enabled="false" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="35px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btProdutoExcluir" runat="server" Width="14px" CommandName="EX"
                                                            ImageUrl="~/Image/delete.png" OnClick="btProdutoExcluir_Click" ToolTip="Excluir" Enabled="false" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="35px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btProdutoVisualizar" runat="server" Height="17px" Width="17px"
                                                            ImageUrl="~/Image/eye.png" OnClick="btProdutoVisualizar_Click" ToolTip="Visualizar Foto" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="35px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btProdutoAprovar" runat="server" Height="17px" Width="17px"
                                                            ImageUrl="~/Image/approve.png" CommandName="AP" OnClick="btProdutoAprovar_Click"
                                                            Visible="false" ToolTip="Aprovar Produto" OnClientClick="return Confirmar('Deseja realmente Aprovar este Produto?');" />
                                                        <asp:ImageButton ID="btProdutoDesaprovar" runat="server" Height="17px" Width="17px"
                                                            ImageUrl="~/Image/disapprove.png" CommandName="DE" OnClick="btProdutoAprovar_Click"
                                                            Visible="false" ToolTip="Desaprovar Produto" OnClientClick="return Confirmar('Deseja realmente Desaprovar este Produto?');" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Produto" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px"
                                                    SortExpression="ddlProdutoFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labProduto" runat="server" Text='<%# Bind("MODELO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Grupo" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="185px"
                                                    SortExpression="ddlGrupoFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labGrupo" runat="server" Text='<%# Bind("GRUPO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nome" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="170px"
                                                    SortExpression="ddlNomeFiltro">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNome" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Origem" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="140px"
                                                    SortExpression="ddlOrigemFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labOrigem" runat="server" Text='<%# Bind("DESCRICAO_ORIGEM") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="140px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Griffe" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="145px"
                                                    SortExpression="ddlGriffeFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labGriffe" runat="server" Text='<%# Bind("GRIFFE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Segmento" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="210px"
                                                    SortExpression="ddlSegmentoFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labSegmento" runat="server" Text='<%# Bind("SEGMENTO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tecido" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="230px"
                                                    SortExpression="ddlTecidoFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labTecido" runat="server" Text='<%# Bind("TECIDO_POCKET") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cor" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="190px"
                                                    SortExpression="ddlCorFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labCor" runat="server" Text='<%# Bind("DESC_COR") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cor Fornecedor" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="300px" SortExpression="ddlCorFornecedorFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labCorFornecedor" runat="server" Text='<%# Bind("FORNECEDOR_COR") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Linha" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="250px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labLinha" runat="server" Text='<%# Bind("LINHA") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Quantidade Mostruário" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="170px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labQtdeMostruario" runat="server" Text='<%# Bind("QTDE_MOSTRUARIO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Quantidade Varejo" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="170px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labQtdeVarejo" runat="server" Text='<%# Bind("QTDE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Preço Varejo" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="170px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labPrecoVarejo" runat="server" Text='<%# Bind("PRECO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Quantidade Atacado" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="170px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labQtdeAtacado" runat="server" Text='<%# Bind("QTDE_ATACADO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Preço Atacado" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="170px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labPrecoAtacado" runat="server" Text='<%# Bind("PRECO_ATACADO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Fornecedor" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="190px"
                                                    SortExpression="ddlFornecedorFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labFornecedor" runat="server" Text='<%# Bind("FORNECEDOR") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mostruário Corte" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="130px" SortExpression="ddlMostruarioCorteFiltro">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMostruarioCorte" runat="server" Text='<%# Bind("MOSTRUARIOCORTE") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="REF de Modelagem" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="300px" SortExpression="ddlRefModelagemFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labRefModelagem" runat="server" Text='<%# Bind("REF_MODELAGEM") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Observação" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="220px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labObservacao" runat="server" Text='<%# Bind("OBSERVACAO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Varejo" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px"
                                                    SortExpression="ddlVarejoFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labVarejo" runat="server" Text='<%# Bind("CORTE_VAREJO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Atacado" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px"
                                                    SortExpression="ddlAtacadoFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labAtacado" runat="server" Text='<%# Bind("CORTE_ATACADO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Brinde" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labBrinde" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Produto Acabado" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="120px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labProdutoAcabado" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Signed" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labSigned" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Signed Nome" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left"
                                                    SortExpression="ddlSignedNomeFiltro">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labSignedNome" Text='<%# Bind("SIGNED_NOME") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Data de Aprovação" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="labAprovacao" runat="server" Text='<%# Bind("DATA_APROVACAO", "{0:dd/MM/yyyy}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="32">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="32">&nbsp;
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </fieldset>
</asp:Content>
