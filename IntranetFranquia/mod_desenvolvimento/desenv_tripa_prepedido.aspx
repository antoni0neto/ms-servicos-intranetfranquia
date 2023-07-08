<%@ Page Title="Pré-Pedido de Tecido" Language="C#" MasterPageFile="~/Site.Master"
    CodeBehind="desenv_tripa_prepedido.aspx.cs" Inherits="Relatorios.desenv_tripa_prepedido" %>

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

        function calcularPrePedido(control) {

            var consumo = parseFloat(0);
            var precoTecido = parseFloat(0);
            var qtdeAtacado = parseFloat(0);
            var qtdeVarejo = parseFloat(0);
            var precoVenda = parseFloat(0);
            var precoTecido = parseFloat(0);
            var valorTecido = parseFloat(0);

            if (document.getElementById("MainContent_txtConsumo").value != "")
                consumo = parseFloat(document.getElementById("MainContent_txtConsumo").value.replace(',', '.'));

            if (document.getElementById("MainContent_txtPrecoTecido").value != "")
                precoTecido = parseFloat(document.getElementById("MainContent_txtPrecoTecido").value.replace(',', '.'));

            if (document.getElementById("MainContent_txtQtdeAtacado").value != "")
                qtdeAtacado = parseFloat(document.getElementById("MainContent_txtQtdeAtacado").value);

            if (document.getElementById("MainContent_txtQtdeVarejo").value != "")
                qtdeVarejo = parseFloat(document.getElementById("MainContent_txtQtdeVarejo").value);

            if (document.getElementById("MainContent_txtPrecoVenda").value != "")
                precoVenda = parseFloat(document.getElementById("MainContent_txtPrecoVenda").value.replace(',', '.'));

            if (document.getElementById("MainContent_txtPrecoVenda").value != "")
                precoVenda = parseFloat(document.getElementById("MainContent_txtPrecoVenda").value.replace(',', '.'));

            if (document.getElementById("MainContent_txtPrecoTecido").value != "")
                precoTecido = parseFloat(document.getElementById("MainContent_txtPrecoTecido").value.replace(',', '.'));

            document.getElementById("MainContent_txtConsumoTotal").value = ((qtdeAtacado + qtdeVarejo) * consumo).toFixed(0);
            document.getElementById("MainContent_txtTotalAtacado").value = "R$ " + ((qtdeAtacado) * (precoVenda / 2)).toFixed(2).replace('.', ',');
            document.getElementById("MainContent_txtTotalVarejo").value = "R$ " + ((qtdeVarejo) * precoVenda).toFixed(2).replace('.', ',');

            document.getElementById("MainContent_txtValorTecido").value = "R$ " + (precoTecido * ((qtdeAtacado + qtdeVarejo) * consumo)).toFixed(2).replace('.', ',');
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Planejamento
            de Coleção Nacional&nbsp;&nbsp;>&nbsp;&nbsp;Pré-Pedido de Tecido</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset style="background-color: White;">
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Pré-Pedido de Tecido"></asp:Label>
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
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Atualizar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />
                            &nbsp;
                    <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </td>
                        <td style="width: 480px; text-align: right;">
                            <asp:Panel ID="pnlResumo" runat="server" Visible="false">
                                <asp:Button ID="btTulp" runat="server" CommandArgument="TULP" OnClick="btResumo_Click" Width="100px" Text="TULP" />
                                &nbsp;
                        <asp:Button ID="btFeminino" runat="server" CommandArgument="FEMININO" OnClick="btResumo_Click" Width="100px" Text="Feminino" />
                                &nbsp;
                        <asp:Button ID="btMasculino" runat="server" CommandArgument="MASCULINO" OnClick="btResumo_Click" Width="100px" Text="Masculino" />
                                &nbsp;
                        <asp:Button ID="btPetit" runat="server" CommandArgument="PETIT" OnClick="btResumo_Click" Width="100px" Text="Petit" />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlPrePedido" runat="server" Visible="false">
                    <fieldset id="fsIncs" runat="server" visible="true">
                        <legend>Inclusão</legend>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="labOrigem" runat="server" Text="Origem"></asp:Label>
                                </td>
                                <td colspan="7">
                                    <asp:Label ID="labProdParcial" runat="server" Text="Produção Parcial"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlOrigem" runat="server" Width="344px" Height="21px" DataTextField="DESCRICAO"
                                        DataValueField="CODIGO">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="7">
                                    <asp:DropDownList ID="ddlProdParcial" runat="server" Width="166px" Height="21px">
                                        <asp:ListItem Value="N" Text="Não" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="labTecidoFiltro" runat="server" Text="Procurar Tecido..."></asp:Label></td>
                                <td colspan="4">
                                    <asp:Label ID="labTecido" runat="server" Text="Tecido"></asp:Label></td>
                                <td>
                                    <asp:Label ID="labPrecoTecido" runat="server" Text="Preço Tecido"></asp:Label>
                                </td>
                                <td rowspan="4" valign="middle" style="text-align: right;">
                                    <asp:Image ID="imgTecido" runat="server" ImageAlign="AbsMiddle" AlternateText="Sem Imagem" Width="150px" Height="60px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:TextBox ID="txtTecidoFiltro" runat="server" Width="336px" OnTextChanged="txtTecidoFiltro_TextChanged" AutoPostBack="true"></asp:TextBox>
                                </td>
                                <td colspan="4">
                                    <asp:DropDownList ID="ddlTecido" runat="server" Width="695px" Height="21px" DataTextField="DESCRICAO_MATERIAL"
                                        DataValueField="MATERIAL" OnTextChanged="ddlTecido_TextChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPrecoTecido" runat="server" Width="140px" MaxLength="10" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumeroDecimal(event);" onblur="calcularPrePedido(this);"></asp:TextBox>
                                    <asp:ImageButton ID="imgBtnAbrirTecido" runat="server" ImageUrl="~/Image/search.png" Width="15px" OnClick="imgBtnAbrirTecido_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="labFornecedor" runat="server" Text="Fornecedor"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labCorFornecedor" runat="server" Text="Cor Fornecedor"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="labCor" runat="server" Text="Cor"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="labGriffe" runat="server" Text="Griffe"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labUnidadeMedida" runat="server" Text="Unidade Medida"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlFornecedor" runat="server" Width="169px" Height="21px" DataTextField="FORNECEDOR"
                                        DataValueField="FORNECEDOR">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCorFornecedor" runat="server" Width="161px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlCor" runat="server" Width="345px" Height="21px" DataValueField="COR" DataTextField="DESC_COR">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlGriffe" runat="server" Width="345px" Height="21px" DataTextField="GRIFFE"
                                        DataValueField="GRIFFE">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlUnidadeMedida" runat="server" Width="169px" Height="21px"
                                        DataTextField="DESC_UNIDADE" DataValueField="UNIDADE1">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="labGrupoPedido" runat="server" Text="Grupo Produto"></asp:Label>
                                </td>
                                <td colspan="6">
                                    <asp:Label ID="labSignedNome" runat="server" Text="Signed"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlGrupoPedido" runat="server" Width="344px" Height="21px" DataTextField="GRUPO"
                                        DataValueField="GRUPO">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="6">
                                    <asp:TextBox ID="txtSignedNome" runat="server" Width="161px" MaxLength="30"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="labConsumo" runat="server" Text="Consumo"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labQtdeAtacado" runat="server" Text="Qtde Atacado"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labQtdeVarejo" runat="server" Text="Qtde Varejo"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labPrecoVenda" runat="server" Text="Preço Venda"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labConsumoTotal" runat="server" Text="Consumo Total"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labTotalTecido" runat="server" Text="Valor Tecido"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labTotalAtacado" runat="server" Text="Preço Atacado Total"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labTotalVarejo" runat="server" Text="Preço Varejo Total"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 175px;">
                                    <asp:TextBox ID="txtConsumo" runat="server" Width="160px" MaxLength="10" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumeroDecimal(event);" onblur="calcularPrePedido(this);"></asp:TextBox>
                                </td>
                                <td style="width: 175px;">
                                    <asp:TextBox ID="txtQtdeAtacado" runat="server" Width="161px" MaxLength="10" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="calcularPrePedido(this);"></asp:TextBox>
                                </td>
                                <td style="width: 175px;">
                                    <asp:TextBox ID="txtQtdeVarejo" runat="server" Width="161px" MaxLength="10" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);" onblur="calcularPrePedido(this);"></asp:TextBox>
                                </td>
                                <td style="width: 175px;">
                                    <asp:TextBox ID="txtPrecoVenda" runat="server" Width="161px" MaxLength="10" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumeroDecimal(event);" onblur="calcularPrePedido(this);"></asp:TextBox>
                                </td>
                                <td style="width: 175px;">
                                    <asp:TextBox ID="txtConsumoTotal" runat="server" Width="161px" MaxLength="10" CssClass="alinharDireita" Enabled="false"
                                        onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                </td>
                                <td style="width: 175px;">
                                    <asp:TextBox ID="txtValorTecido" runat="server" Width="161px" MaxLength="10" CssClass="alinharDireita" Enabled="false"
                                        onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                </td>
                                <td style="width: 175px;">
                                    <asp:TextBox ID="txtTotalAtacado" runat="server" Width="161px" MaxLength="10" CssClass="alinharDireita" Enabled="false"
                                        onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTotalVarejo" runat="server" Width="161px" MaxLength="10" CssClass="alinharDireita" Enabled="false"
                                        onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btIncluirPrePedido" runat="server" Width="167px" Text="Incluir" OnClick="btIncluirPrePedido_Click" OnClientClick="DesabilitarBotao(this);" />
                                </td>
                                <td colspan="8">
                                    <asp:Button ID="btLimparTodos" runat="server" Width="166px" Text="Limpar Campos" OnClick="btLimparTodos_Click" />
                                    &nbsp;
                                    <asp:Label ID="labErroInclusao" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                                    &nbsp;
                                    <asp:HiddenField ID="hidCodigo" runat="server" Value="" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <br />
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="width: 2100px;">
                        <tr>
                            <td style="width: 98px;">
                                <asp:DropDownList ID="ddlPaginaNumero" runat="server" Width="98px" OnSelectedIndexChanged="ddlPaginaNumero_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="10" Text="10"></asp:ListItem>
                                    <asp:ListItem Value="50" Text="50" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="100" Text="100"></asp:ListItem>
                                    <asp:ListItem Value="200" Text="200"></asp:ListItem>
                                    <asp:ListItem Value="400" Text="400"></asp:ListItem>
                                    <asp:ListItem Value="600" Text="600"></asp:ListItem>
                                    <asp:ListItem Value="800" Text="800"></asp:ListItem>
                                    <asp:ListItem Value="1000" Text="1000"></asp:ListItem>
                                    <asp:ListItem Value="2000" Text="2000"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 83px;">
                                <asp:DropDownList ID="ddlFiltroOrigem" runat="server" Width="83px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td style="width: 63px;">
                                <asp:DropDownList ID="ddlFiltroProduto" runat="server" Width="63px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td style="width: 173px;">
                                <asp:DropDownList ID="ddlFiltroTecido" runat="server" Width="173px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td style="width: 123px;">
                                <asp:DropDownList ID="ddlFiltroFornecedor" runat="server" Width="123px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td style="width: 133px;">
                                <asp:DropDownList ID="ddlFiltroCorFornecedor" runat="server" Width="133px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td style="width: 83px;">
                                <asp:DropDownList ID="ddlFiltroCorLinx" runat="server" Width="83px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td style="width: 103px;">
                                <asp:DropDownList ID="ddlFiltroGrupoProduto" runat="server" Width="103px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td style="width: 83px;">
                                <asp:DropDownList ID="ddlFiltroGriffe" runat="server" Width="83px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td style="width: 85px;">
                                <asp:DropDownList ID="ddlFiltroSigned" runat="server" Width="85px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td style="width: 63px;">
                                <asp:DropDownList ID="ddlFiltroMedida" runat="server" Width="63px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td style="width: 78px;">
                                <asp:DropDownList ID="ddlFiltroConsumo" runat="server" Width="78px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td style="width: 73px;">
                                <asp:DropDownList ID="ddlFiltroPrecoTecido" runat="server" Width="73px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td style="width: 63px;">
                                <asp:DropDownList ID="ddlFiltroQtdeAtacado" runat="server" Width="63px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td style="width: 63px;">
                                <asp:DropDownList ID="ddlFiltroQtdeVarejo" runat="server" Width="63px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td style="width: 63px;">
                                <asp:DropDownList ID="ddlFiltroProdParcial" runat="server" Width="63px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td style="width: 93px;">
                                <asp:DropDownList ID="ddlFiltroPrecoVenda" runat="server" Width="93px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td style="width: 98px;">
                                <asp:DropDownList ID="ddlFiltroTotalConsumo" runat="server" Width="98px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td style="">&nbsp;
                            </td>
                            <td style="width: 135px;">
                                <asp:DropDownList ID="ddlFiltroPedido" runat="server" Width="135px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td style="">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="20">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvPrePedido" runat="server" Width="2100px" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvPrePedido_RowDataBound"
                                        OnDataBound="gvPrePedido_DataBound" ShowFooter="true"
                                        OnSorting="gvPrePedido_Sorting" AllowSorting="true"
                                        OnPageIndexChanging="gvPrePedido_PageIndexChanging" AllowPaging="true" PageSize="50">
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                        <FooterStyle BackColor="Gainsboro" Font-Bold="true" Font-Size="Small" />
                                        <PagerStyle HorizontalAlign="Center" CssClass="pageStyl" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBtnAlterar" runat="server" Width="13px" ImageUrl="~/Image/edit.jpg" OnClick="imgBtnAlterar_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBtnCopiar" runat="server" Height="13px" Width="13px" ImageUrl="~/Image/copy.png"
                                                        OnClick="imgBtnCopiar_Click" ToolTip="Copiar" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBtnExcluir" runat="server" Height="10px" Width="10px" ImageUrl="~/Image/delete.png"
                                                        OnClick="imgBtnExcluir_Click" ToolTip="Excluir" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Origem" DataField="ORIGEM" HeaderStyle-Width="80px" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" SortExpression="ORIGEM" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                            <asp:BoundField HeaderText="Produto" DataField="PRODUTO" HeaderStyle-Width="60px" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" SortExpression="PRODUTO" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                            <asp:BoundField HeaderText="Tecido" DataField="TECIDO" HeaderStyle-Width="170px" SortExpression="TECIDO" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                            <asp:BoundField HeaderText="Fornecedor" DataField="FORNECEDOR" HeaderStyle-Width="120px" SortExpression="FORNECEDOR" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                            <asp:BoundField HeaderText="Cor Fornecedor" DataField="COR_FORNECEDOR" HeaderStyle-Width="130px" SortExpression="COR_FORNECEDOR" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                            <asp:BoundField HeaderText="Cor Linx" DataField="DESC_COR" SortExpression="DESC_COR" HeaderStyle-Width="80px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                            <asp:BoundField HeaderText="Grupo Produto" DataField="GRUPO_PRODUTO" SortExpression="GRUPO_PRODUTO" HeaderStyle-Width="100px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                            <asp:BoundField HeaderText="Griffe" DataField="GRIFFE" SortExpression="GRIFFE" HeaderStyle-Width="80px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                            <asp:BoundField HeaderText="Signed" DataField="SIGNED_NOME" SortExpression="SIGNED_NOME" HeaderStyle-Width="80px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                            <asp:BoundField HeaderText="Med" DataField="UNIDADE_MEDIDA" SortExpression="UNIDADE_MEDIDA" HeaderStyle-Width="60px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FooterStyle-Font-Size="Smaller" />
                                            <asp:TemplateField HeaderText="Consumo" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                                SortExpression="CONSUMO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litConsumo" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P. Tecido" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                                SortExpression="PRECO_TECIDO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPrecoTecido" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Atacado" DataField="QTDE_ATACADO" SortExpression="QTDE_ATACADO" HeaderStyle-Width="60px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                            <asp:BoundField HeaderText="Varejo" DataField="QTDE_VAREJO" SortExpression="QTDE_VAREJO" HeaderStyle-Width="60px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                            <asp:BoundField HeaderText="P. Parcial" DataField="PROD_PARCIAL" SortExpression="PROD_PARCIAL" HeaderStyle-Width="60px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                            <asp:TemplateField HeaderText="Preço Venda" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                                SortExpression="PRECO_VENDA">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPrecoVenda" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tot Consumo" HeaderStyle-Width="95px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                                SortExpression="CONSUMO_TOTAL">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litConsumoTotal" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Val Tecido" HeaderStyle-Width="95px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                                SortExpression="VALOR_TECIDO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValorTecido" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tot Atacado" HeaderStyle-Width="105px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                                SortExpression="VALOR_TOTAL_ATACADO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValorTotalAtacado" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tot Varejo" HeaderStyle-Width="105px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                                SortExpression="VALOR_TOTAL_VAREJO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValorTotalVarejo" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Status" DataField="STATUS_DESC" SortExpression="STATUS_DESC" HeaderStyle-Width="85px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                            <asp:TemplateField HeaderText="Pedido" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                                SortExpression="NUMERO_PEDIDO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPedido" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
