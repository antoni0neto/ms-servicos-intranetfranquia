<%@ Page Title="Liberar Corte de Produto" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master"
    CodeBehind="desenv_produto_liberacao.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="Relatorios.mod_desenvolvimento.desenv_produto_liberacao" %>

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
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro
                    de Apoio&nbsp;&nbsp;>&nbsp;&nbsp;Liberar Corte de Produto</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <br />
            <div style="width: 100%;">
                <fieldset>
                    <legend>Liberar Corte de Produto</legend>
                    <div style="padding-left: 0px;">
                        <asp:ImageButton ID="ibtPesquisar" runat="server" Width="19px" ImageUrl="~/Image/search.png"
                            ToolTip="Pesquisar Produtos" OnClick="ibtPesquisar_Click" />
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
                            <td>
                                <asp:Label ID="labTipo" runat="server" Text="Tipo"></asp:Label>
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
                                    DataTextField="COR_FORNECEDOR" DataValueField="COR_FORNECEDOR">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTipo" runat="server" Width="154px" Height="21px">
                                    <asp:ListItem Value="" Text="" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="M" Text="MOSTRUÁRIO"></asp:ListItem>
                                    <asp:ListItem Value="V" Text="VAREJO"></asp:ListItem>
                                    <asp:ListItem Value="A" Text="ATACADO"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div id="tabs">
                        <ul>
                            <li><a href="#tabs-produtos" id="tabProdutos" runat="server" onclick="MarcarAba(0);">Produtos</a></li>
                        </ul>
                        <div id="tabs-produtos">
                            <fieldset>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                    <tr>
                                        <td style="width: 230px;">Quantidade em Estoque:
                                        </td>
                                        <td>
                                            <asp:Label ID="labQtdeEstoque" runat="server" Font-Bold="true" ForeColor="Green"
                                                Text="0,000"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Quantidade Liberada:
                                        </td>
                                        <td>
                                            <asp:Label ID="labQtdeLiberado" runat="server" Font-Bold="true" ForeColor="Green"
                                                Text="0,000"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Quantidade Reservada:
                                        </td>
                                        <td>
                                            <asp:Label ID="labQtdeReservada" runat="server" Font-Bold="true" ForeColor="Green"
                                                Text="0,000"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Saldo:
                                        </td>
                                        <td>
                                            <asp:Label ID="labQtdeSaldo" runat="server" Font-Bold="true" ForeColor="Green" Text="0,000"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                    <tr>
                                        <td style="width: 28px;">&nbsp;</td>
                                        <td style="width: 28px;">&nbsp;</td>
                                        <td style="width: 106px;">
                                            <asp:DropDownList ID="ddlModeloFiltro" runat="server" Height="20px" Width="106px" DataValueField="MODELO" DataTextField="MODELO" OnSelectedIndexChanged="ddlFiltro_OnSelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </td>
                                        <td style="width: 158px;">
                                            <asp:DropDownList ID="ddlNomeFiltro" runat="server" Height="20px" Width="158px" DataValueField="DESC_PRODUTO" DataTextField="DESC_PRODUTO" OnSelectedIndexChanged="ddlFiltro_OnSelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </td>
                                        <td colspan="8" style="width: 750px;">&nbsp;</td>
                                        <td>
                                            <asp:DropDownList ID="ddlStatusFiltro" runat="server" Height="20px" Width="125px"
                                                OnSelectedIndexChanged="ddlFiltro_OnSelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                                <asp:ListItem Value="V" Text="VAZIO"></asp:ListItem>
                                                <asp:ListItem Value="L" Text="LIBERADO"></asp:ListItem>
                                                <asp:ListItem Value="P" Text="LIB. PARCIAL"></asp:ListItem>
                                                <asp:ListItem Value="R" Text="RESERVADO"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="14">
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvProdutos" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvProdutos_RowDataBound" OnSorting="gvProdutos_Sorting"
                                                    AllowSorting="true"
                                                    OnDataBound="gvProdutos_DataBound" ShowFooter="true" DataKeyNames="DESENV_PRODUTO">
                                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-VerticalAlign="Middle">
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../Image/plus.png"
                                                                    Width="15px" runat="server" />
                                                                <asp:Panel ID="pnlFoto" runat="server" Style="display: none" Width="100%">
                                                                    <asp:GridView ID="gvFoto" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvFoto_RowDataBound"
                                                                        Width="100%" DataKeyNames="CODIGO">
                                                                        <HeaderStyle BackColor="LightSteelBlue" HorizontalAlign="Center"></HeaderStyle>
                                                                        <RowStyle BorderColor="Gainsboro" BorderWidth="1px" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Foto Peça" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                                ItemStyle-BorderColor="Gainsboro" ItemStyle-BorderWidth="1px">
                                                                                <ItemTemplate>
                                                                                    <asp:Image ID="imgFotoPeca" runat="server" ImageAlign="Middle" />
                                                                                    <asp:Image ID="imgFotoPeca2" runat="server" ImageAlign="Middle" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Modelo" ItemStyle-Width="110px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center" SortExpression="MODELO">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labModelo" runat="server" Text='<%# Eval("MODELO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Nome" ItemStyle-Width="170px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" SortExpression="DESC_PRODUTO">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labNome" runat="server" Text='<%# Eval("DESC_PRODUTO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cor" ItemStyle-Width="180px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" SortExpression="DESC_COR">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labCor" runat="server" Text='<%# Eval("DESC_COR") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Tipo" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left" SortExpression="TIPO">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labTipo" runat="server" Text='<%# Eval("TIPO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qtde Venda Atacado" ItemStyle-Width="160px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labQtdeVendaAtacado" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Consumo" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labConsumo" runat="server" Text='<%# Eval("CONSUMO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" ItemStyle-Width="1px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hidQtdeAtual" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Quantidade Planejada" ItemStyle-Width="160px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labQtde" runat="server" Text='<%# Eval("QTDE_LIBERADA") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Quantidade" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtQtde" runat="server" Width="120px" Style="text-align: center;" onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" ItemStyle-Width="1px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hidStatusAtual" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="125px" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlLiberacao" runat="server" Height="20px" Width="125px">
                                                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                                                    <asp:ListItem Value="L" Text="LIBERADO"></asp:ListItem>
                                                                    <asp:ListItem Value="P" Text="LIB. PARCIAL"></asp:ListItem>
                                                                    <asp:ListItem Value="R" Text="RESERVADO"></asp:ListItem>
                                                                    <asp:ListItem Value="S" Text="SEM AÇÃO"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="23px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btLiberarProduto" runat="server" Height="15px" Width="15px"
                                                                    ImageUrl="~/Image/approve.png" OnClick="btLiberarProduto_Click" OnClientClick="return Confirmar('Deseja realmente Alterar este Produto?');"
                                                                    ToolTip="Alterar Produto" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="14">&nbsp;
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
