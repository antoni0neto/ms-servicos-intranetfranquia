<%@ Page Title="Liberação de Produto (View)" Language="C#" MasterPageFile="~/Site.Master"
    CodeBehind="desenv_tripa_view.aspx.cs" Inherits="Relatorios.desenv_tripa_view" %>

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

        .alinharCentro {
            text-align: center;
        }

        .pageStyl a, .pageStyl span {
            display: block;
            height: 15px;
            width: 15px;
            font-weight: bold;
            text-align: left;
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

    <script type="text/javascript" src="../js/1.9.0/jquery.min.js"></script>
    <script type="text/javascript" src="../js/js.js"></script>
    <script type="text/javascript">
        function calcularPrePedido(control) {

            var consumo = parseFloat(0);
            var precoTecido = parseFloat(0);
            var qtdeAtacado = parseFloat(0);
            var qtdeVarejo = parseFloat(0);
            var precoVenda = parseFloat(0);

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

            document.getElementById("MainContent_txtConsumoTotal").value = ((qtdeAtacado + qtdeVarejo) * consumo).toFixed(0);
            document.getElementById("MainContent_txtTotalAtacado").value = "R$ " + ((qtdeAtacado) * (precoVenda / 2)).toFixed(2).replace('.', ',');
            document.getElementById("MainContent_txtTotalVarejo").value = "R$ " + ((qtdeVarejo) * precoVenda).toFixed(2).replace('.', ',');
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Planejamento
            de Coleção Nacional&nbsp;&nbsp;>&nbsp;&nbsp;Liberação de Produto (View)</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset style="background-color: White;">
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Liberação de Produto (View)"></asp:Label></legend>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="labMarca" runat="server" Text="Marca"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="labColecao" runat="server" Text="Coleção"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="labOrigem" runat="server" Text="Origem"></asp:Label>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 150px;">
                            <asp:DropDownList ID="ddlMarca" runat="server" Width="144px" Height="21px" Enabled="false">
                                <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                <asp:ListItem Value="HANDBOOK" Text="HANDBOOK" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="TULP" Text="TULP"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 156px;">
                            <asp:DropDownList ID="ddlColecao" runat="server" Width="150px" Height="21px" DataTextField="DESC_COLECAO"
                                DataValueField="COLECAO" OnSelectedIndexChanged="ddlColecao_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 197px;">
                            <asp:DropDownList ID="ddlOrigem" runat="server" Width="191px" Height="21px" DataTextField="DESCRICAO"
                                DataValueField="CODIGO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 400px;">
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;
                            <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </td>
                        <td style="width: 500px; text-align: right;">
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
                        <td>&nbsp;</td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="width: 550px;">&nbsp;</td>
                        <td colspan="2">
                            <asp:Panel ID="pnlTecido" runat="server" Width="850px" BorderWidth="0" Visible="false">
                                <br />
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvTecido" runat="server" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white; width: 100%; border-collapse: collapse;" OnRowDataBound="gvTecido_RowDataBound"
                                        OnDataBound="gvTecido_DataBound" ShowFooter="true"
                                        DataKeyNames="MATERIAL, TECIDO, COR, COR_FORNECEDOR">
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                        <FooterStyle BackColor="Gainsboro" Font-Bold="true" />
                                        <PagerStyle HorizontalAlign="Left" CssClass="pageStyl" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Tecido" DataField="TECIDO" HeaderStyle-Width="400px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                            <asp:BoundField HeaderText="Cor Linx" DataField="DESC_COR" HeaderStyle-Width="140px" HeaderStyle-Font-Size="Smaller" />
                                            <asp:BoundField HeaderText="Cor Fornecedor" DataField="COR_FORNECEDOR" HeaderStyle-Width="160px" HeaderStyle-Font-Size="Smaller" />
                                            <asp:TemplateField HeaderText="Qtde Comprada" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="Smaller"
                                                SortExpression="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtQtdeComprada" runat="server" Width="118px" onkeypress="return fnValidarNumeroDecimal(event);" OnTextChanged="txtQtdeComprada_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qtde Consumida" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="Smaller"
                                                SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtdeTecidoConsumida" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Estoque" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="Smaller"
                                                SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtdeTecidoEstoque" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <br />
                            </asp:Panel>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:ImageButton ID="btnImgMostrarInc" runat="server" Width="15px" ImageUrl="~/Image/add.png" OnClick="btnImgMostrarInclusao_Click" />
                            <asp:ImageButton ID="btnImgEsconderInc" runat="server" Width="15px" ImageUrl="~/Image/cancel.png" OnClick="btnImgEsconderInc_Click" Visible="false" />
                            <asp:Panel ID="pnlInc" runat="server" Visible="false">
                                <fieldset id="fsIncs" runat="server" visible="true">
                                    <legend>Inclusão</legend>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td colspan="9">
                                                <asp:Label ID="labOrigemInc" runat="server" Text="Origem"></asp:Label>
                                                <asp:HiddenField ID="hidCodigo" runat="server" Value="0" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9">
                                                <asp:DropDownList ID="ddlOrigemInc" runat="server" Width="344px" Height="21px" DataTextField="DESCRICAO"
                                                    DataValueField="CODIGO">
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
                                                <asp:TextBox ID="txtTecidoFiltro" runat="server" Width="340px" OnTextChanged="txtTecidoFiltro_TextChanged" AutoPostBack="true"></asp:TextBox>
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
                                                <asp:TextBox ID="txtCorFornecedor" runat="server" Width="165px" MaxLength="50"></asp:TextBox>
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
                                            <td>
                                                <asp:Label ID="labSignedNome" runat="server" Text="Signed"></asp:Label>
                                            </td>
                                            <td colspan="5">
                                                <asp:Label ID="labRefModelagem" runat="server" Text="REF Modelagem"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:DropDownList ID="ddlGrupoPedido" runat="server" Width="344px" Height="21px" DataTextField="GRUPO"
                                                    DataValueField="GRUPO">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSignedNome" runat="server" Width="165px" MaxLength="30"></asp:TextBox>
                                            </td>
                                            <td colspan="5">
                                                <asp:TextBox ID="txtRefModelagem" runat="server" Width="165px" MaxLength="30"></asp:TextBox>
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
                                                <asp:Label ID="labTotalAtacado" runat="server" Text="Preço Atacado Total"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="labTotalVarejo" runat="server" Text="Preço Varejo Total"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 175px;">
                                                <asp:TextBox ID="txtConsumo" runat="server" Width="165px" MaxLength="10" CssClass="alinharDireita"
                                                    onkeypress="return fnValidarNumeroDecimal(event);" onblur="calcularPrePedido(this);"></asp:TextBox>
                                            </td>
                                            <td style="width: 175px;">
                                                <asp:TextBox ID="txtQtdeAtacado" runat="server" Width="165px" MaxLength="10" CssClass="alinharDireita"
                                                    onkeypress="return fnValidarNumero(event);" onblur="calcularPrePedido(this);"></asp:TextBox>
                                            </td>
                                            <td style="width: 175px;">
                                                <asp:TextBox ID="txtQtdeVarejo" runat="server" Width="165px" MaxLength="10" CssClass="alinharDireita"
                                                    onkeypress="return fnValidarNumero(event);" onblur="calcularPrePedido(this);"></asp:TextBox>
                                            </td>
                                            <td style="width: 175px;">
                                                <asp:TextBox ID="txtPrecoVenda" runat="server" Width="165px" MaxLength="10" CssClass="alinharDireita"
                                                    onkeypress="return fnValidarNumeroDecimal(event);" onblur="calcularPrePedido(this);"></asp:TextBox>
                                            </td>
                                            <td style="width: 175px;">
                                                <asp:TextBox ID="txtConsumoTotal" runat="server" Width="165px" MaxLength="10" CssClass="alinharDireita" Enabled="false"
                                                    onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                            </td>
                                            <td style="width: 175px;">
                                                <asp:TextBox ID="txtTotalAtacado" runat="server" Width="165px" MaxLength="10" CssClass="alinharDireita" Enabled="false"
                                                    onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtTotalVarejo" runat="server" Width="165px" MaxLength="10" CssClass="alinharDireita" Enabled="false"
                                                    onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="8">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="8">
                                                <asp:Button ID="btIncluirPrePedido" runat="server" Width="167px" Text="Incluir" OnClick="btIncluirPrePedido_Click" OnClientClick="DesabilitarBotao(this);" />
                                                &nbsp;
                                                <asp:Button ID="btLimparTodos" runat="server" Width="166px" Text="Limpar Campos" OnClick="btLimparTodos_Click" />
                                                &nbsp;
                                                <asp:Label ID="labErroInclusao" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>

                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">&nbsp;
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="4">Total de Modelos:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="labModeloFiltroValor"
                            ForeColor="Green" Font-Bold="true" runat="server" Text="0"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">Total de SKUs:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                            ID="labEstiloFiltroValor" runat="server" ForeColor="Green" Font-Bold="true" Text="0"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">&nbsp;
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td colspan="2" style="width: 81px;">
                            <asp:DropDownList ID="ddlPaginaNumero" runat="server" Width="81px" OnSelectedIndexChanged="ddlPaginaNumero_SelectedIndexChanged" AutoPostBack="true">
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
                        <td style="width: 40px;">
                            <asp:DropDownList ID="ddlFiltroTecidoLib" runat="server" Width="40px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="S" Text="S"></asp:ListItem>
                                <asp:ListItem Value="N" Text="N"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 93px;">
                            <asp:DropDownList ID="ddlFiltroOrigem" runat="server" Width="93px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </td>
                        <td style="width: 113px;">
                            <asp:DropDownList ID="ddlFiltroFabricante" runat="server" Width="113px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </td>
                        <td style="width: 86px;">
                            <asp:DropDownList ID="ddlFiltroGriffe" runat="server" Width="86px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </td>
                        <td style="width: 85px;">
                            <asp:DropDownList ID="ddlFiltroProduto" runat="server" Width="85px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </td>
                        <td style="width: 152px;">&nbsp;
                        </td>
                        <td style="width: 314px;">
                            <asp:DropDownList ID="ddlFiltroTecido" runat="server" Width="314px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </td>
                        <td style="width: 105px;">
                            <asp:DropDownList ID="ddlFiltroCorLinx" runat="server" Width="105px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </td>
                        <td style="width: 128px;">
                            <asp:DropDownList ID="ddlFiltroCorFornecedor" runat="server" Width="128px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </td>
                        <td style="width: 93px;">
                            <asp:DropDownList ID="ddlFiltroGrupoProduto" runat="server" Width="93px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </td>
                        <td style="width: 102px;">&nbsp;</td>
                        <%--Atualizacao--%>
                        <td style="width: 88px;">
                            <asp:DropDownList ID="ddlFiltroLibVarejo" runat="server" Width="88px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </td>
                        <td style="width: 98px;">
                            <asp:DropDownList ID="ddlFiltroQtdeVarejo" runat="server" Width="98px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="" Text="" />
                                <asp:ListItem Value="S" Text="Sim" />
                                <asp:ListItem Value="N" Text="Nâo" />
                            </asp:DropDownList>
                        </td>
                        <td style="width: 98px;">
                            <asp:DropDownList ID="ddlFiltroQtdeVarejoRepique" runat="server" Width="98px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="" Text="" />
                                <asp:ListItem Value="S" Text="Sim" />
                                <asp:ListItem Value="N" Text="Nâo" />
                            </asp:DropDownList>
                        </td>
                        <td style="width: 80px;">&nbsp;</td>
                        <%--DEF Qtde Varejo--%>
                        <td style="width: 88px;">
                            <asp:DropDownList ID="ddlFiltroLibAtacado" runat="server" Width="88px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </td>
                        <td style="width: 98px;">
                            <asp:DropDownList ID="ddlFiltroQtdeAtacado" runat="server" Width="98px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </td>
                        <%--Qtde Atacado--%>
                        <td style="width: 106px;">&nbsp;</td>
                        <%--DEF Qtde Atacado--%>
                        <td style="width: 200px;">&nbsp;</td>
                        <%--Foto Aux--%>
                        <td style="width: 40px;">&nbsp;</td>
                        <%--Venda Atacado--%>
                        <td style="width: 100px;">&nbsp;</td>
                        <%--Valor Atacado--%>
                        <td style="width: 120px;">&nbsp;</td>
                        <%--Preço Venda--%>
                        <td style="width: 130px;">&nbsp;</td>
                        <%--Valor Total Atacado--%>
                        <td style="width: 144px;">&nbsp;</td>
                        <%--Consumo--%>
                        <td style="">&nbsp;</td>
                        <%--Total Consumo--%>

                        <td style="width: 88px;">
                            <asp:DropDownList ID="ddlFiltroSigned" runat="server" Width="88px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </td>
                        <td style="width: 10px;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="28">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvPrePedido" runat="server" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white; width: 2850px; border-collapse: collapse;" OnRowDataBound="gvPrePedido_RowDataBound"
                                    OnDataBound="gvPrePedido_DataBound" ShowFooter="true"
                                    OnSorting="gvPrePedido_Sorting" AllowSorting="true"
                                    OnPageIndexChanging="gvPrePedido_PageIndexChanging" AllowPaging="true" PageSize="50"
                                    DataKeyNames="CODIGO, DESENV_PRODUTO, PRODUTO">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                    <FooterStyle BackColor="Gainsboro" Font-Bold="true" />
                                    <PagerStyle HorizontalAlign="Left" CssClass="pageStyl" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="15px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtnLiberar" runat="server" Width="13px" ImageUrl="~/Image/approve.png" OnClick="imgBtnLiberar_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="17px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtnExcluir" runat="server" Width="10px" ImageUrl="~/Image/delete.png" OnClick="imgBtnExcluir_Click" ToolTip="Excluir" OnClientClick="javascript: return confirm('Tem certeza que deseja excluir?');" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="15px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtnCopiar" runat="server" Height="13px" Width="13px" ImageUrl="~/Image/copy.png"
                                                    OnClick="imgBtnCopiar_Click" ToolTip="Copiar" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="15px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtnAprovarTecido" runat="server" Height="13px" Width="13px" ImageUrl="~/Image/up_disabled.png"
                                                    OnClick="imgBtnAprovarTecido_Click" ToolTip="Aprovar Tecido" />
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="18px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtnFoto" runat="server" Width="15px" Height="25px" ImageAlign="AbsMiddle" OnClick="imgBtnFoto_Click" ToolTip=" " AlternateText=" " />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField HeaderText="Origem" DataField="ORIGEM" SortExpression="ORIGEM" HeaderStyle-Width="90px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                        <asp:BoundField HeaderText="Fornecedor" DataField="FORNECEDOR" SortExpression="FORNECEDOR" HeaderStyle-Width="112px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />

                                        <asp:BoundField HeaderText="Griffe" DataField="GRIFFE" SortExpression="GRIFFE" HeaderStyle-Width="83px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                        <asp:BoundField HeaderText="Produto" DataField="PRODUTO" SortExpression="PRODUTO" HeaderStyle-Width="80px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                        <asp:BoundField HeaderText="Nome" DataField="DESC_PRODUTO" SortExpression="DESC_PRODUTO" HeaderStyle-Width="150px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />

                                        <asp:BoundField HeaderText="Tecido" DataField="TECIDO" SortExpression="TECIDO" HeaderStyle-Width="310px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />

                                        <asp:BoundField HeaderText="Cor Linx" DataField="DESC_COR" SortExpression="DESC_COR" HeaderStyle-Width="102px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                        <asp:BoundField HeaderText="Cor Fornecedor" DataField="COR_FORNECEDOR" HeaderStyle-Width="125px" SortExpression="COR_FORNECEDOR" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />

                                        <asp:BoundField HeaderText="Grupo" DataField="GRUPO_PRODUTO" SortExpression="GRUPO_PRODUTO" HeaderStyle-Width="91px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                        <asp:BoundField HeaderText="Atualização" DataField="DATA_ATUALIZACAO" SortExpression="DATA_ATUALIZACAO" HeaderStyle-Width="95px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />


                                        <asp:BoundField HeaderText="Lib Varejo" DataField="LIBERACAO_VAREJO" SortExpression="LIBERACAO_VAREJO" HeaderStyle-Width="85px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Honeydew" />
                                        <asp:BoundField HeaderText="Qtde Varejo" DataField="QTDE_VAREJO" SortExpression="QTDE_VAREJO" HeaderStyle-Width="95px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Honeydew" />
                                        <asp:BoundField HeaderText="Repique" DataField="QTDE_VAREJO_REPIQUE" SortExpression="QTDE_VAREJO_REPIQUE" HeaderStyle-Width="95px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Honeydew" />
                                        <asp:TemplateField HeaderText="DEF Varejo" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="Honeydew">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDEFVarejo" runat="server" Width="71px" OnTextChanged="txtDEF_TextChanged" AutoPostBack="true" onkeypress="return fnValidarNumero(event);" CssClass="alinharCentro"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField HeaderText="Lib Atacado" DataField="LIBERACAO_ATACADO" SortExpression="LIBERACAO_ATACADO" HeaderStyle-Width="85px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="FloralWhite" />
                                        <asp:BoundField HeaderText="Qtde Atacado" DataField="QTDE_ATACADO" SortExpression="QTDE_ATACADO" HeaderStyle-Width="95px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="FloralWhite" />
                                        <asp:TemplateField HeaderText="DEF Atacado" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="FloralWhite">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDEFAtacado" runat="server" Width="71px" OnTextChanged="txtDEF_TextChanged" AutoPostBack="true" onkeypress="return fnValidarNumero(event);" CssClass="alinharCentro"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="18px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtnFotoAux" runat="server" Width="15px" Height="25px" ImageAlign="AbsMiddle" OnClick="imgBtnFoto_Click" ToolTip="" AlternateText="" />
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:BoundField HeaderText="Venda Atacado" DataField="QTDE_VENDA_ATACADO" SortExpression="QTDE_VENDA_ATACADO" HeaderStyle-Width="120px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="Valor Venda Atacado" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                            SortExpression="VALOR_VENDA_ATACADO">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValorAtacado" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Preço Venda" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                            SortExpression="PRECO_VENDA">
                                            <ItemTemplate>
                                                <asp:Literal ID="litPrecoVenda" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Total Varejo" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                            SortExpression="VALOR_VENDA_TOTAL_VAR">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValorTotVarejo" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Consumo" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                            SortExpression="CONSUMO">
                                            <ItemTemplate>
                                                <asp:Literal ID="litConsumo" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tot Consumo" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller"
                                            SortExpression="CONSUMO_TOTAL">
                                            <ItemTemplate>
                                                <asp:Literal ID="litConsumoTotal" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField HeaderText="Signed" DataField="SIGNED_NOME" SortExpression="SIGNED_NOME" HeaderStyle-Width="85px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" />

                                        <asp:BoundField HeaderText="" DataField="" SortExpression="" />
                                    </Columns>
                                </asp:GridView>
                                <asp:GridView ID="gvPrePedidoTotal" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvPrePedidoTotal_RowDataBound"
                                    ForeColor="#333333" Style="background: white; width: 2850px; border-collapse: collapse;"
                                    ShowFooter="false" ShowHeader="false">
                                    <RowStyle BackColor="GradientActiveCaption" Font-Bold="true" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" ItemStyle-Width="18px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text=''></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-Width="15px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna1" runat="server" Text=''></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-Width="17px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna2" runat="server" Text=''></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="" ItemStyle-Width="16px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna3" runat="server" Text=''></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="" ItemStyle-Width="19px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna4" runat="server" Text=''></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-Width="18px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna55" runat="server" Text=''></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField HeaderText="Origem" DataField="ORIGEM" ItemStyle-Width="80px" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField HeaderText="Fabricante" DataField="FORNECEDOR" ItemStyle-Width="112px" ItemStyle-Font-Size="Smaller" />

                                        <asp:BoundField HeaderText="Griffe" DataField="GRIFFE" ItemStyle-Width="82px" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField HeaderText="Produto" DataField="PRODUTO" ItemStyle-Width="80px" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField HeaderText="Nome" DataField="DESC_PRODUTO" ItemStyle-Width="150px" ItemStyle-Font-Size="Smaller" />

                                        <asp:BoundField HeaderText="Tecido" DataField="TECIDO" ItemStyle-Width="310px" ItemStyle-Font-Size="Smaller" />

                                        <asp:BoundField HeaderText="Cor Linx" DataField="DESC_COR" ItemStyle-Width="102px" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField HeaderText="Cor Fornecedor" DataField="COR_FORNECEDOR" ItemStyle-Width="125px" ItemStyle-Font-Size="Smaller" />

                                        <asp:BoundField HeaderText="Grupo" DataField="GRUPO_PRODUTO" ItemStyle-Width="106px" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField HeaderText="Atualização" DataField="DATA_ATUALIZACAO" ItemStyle-Width="95px" ItemStyle-Font-Size="Smaller" />


                                        <asp:BoundField HeaderText="Lib Varejo" DataField="LIBERACAO_VAREJO" ItemStyle-Width="78px" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField HeaderText="Qtde Varejo" DataField="QTDE_VAREJO" ItemStyle-Width="95px" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField HeaderText="Qtde Varejo Repique" DataField="QTDE_VAREJO_REPIQUE" ItemStyle-Width="95px" ItemStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="DEF Varejo" ItemStyle-Width="75px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna5" runat="server" Text='-'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField HeaderText="Lib Atacado" DataField="LIBERACAO_ATACADO" ItemStyle-Width="90px" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField HeaderText="Qtde Atacado" DataField="QTDE_ATACADO" ItemStyle-Width="95px" ItemStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="DEF Atacado" ItemStyle-Width="78px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna6" runat="server" Text='-'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="" ItemStyle-Width="18px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna7" runat="server" Text='-'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:BoundField HeaderText="Venda Atacado" DataField="QTDE_VENDA_ATACADO" ItemStyle-Width="120px" ItemStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="Valor Venda Atacado" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValorAtacado" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Preço Venda" ItemStyle-Width="110px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litPrecoVenda" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Total Varejo" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValorTotVarejo" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Consumo" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litConsumo" runat="server" Text="-"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tot Consumo" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litConsumoTotal" runat="server" Text="-"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField HeaderText="Signed" DataField="SIGNED_NOME" ItemStyle-Width="85px" ItemStyle-Font-Size="Smaller" />

                                        <asp:BoundField HeaderText="" DataField="" />
                                    </Columns>
                                </asp:GridView>
                            </div>

                        </td>
                    </tr>
                </table>
                <div>
                    <asp:HiddenField ID="hidOrdemCampo" runat="server" Value="" />
                    <asp:HiddenField ID="hidOrdem" runat="server" Value="ASC" />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
