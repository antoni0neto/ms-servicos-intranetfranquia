<%@ Page Title="Ficha Técnica" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master"
    CodeBehind="desenv_ficha_tecnica.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="Relatorios.mod_desenvolvimento.desenv_ficha_tecnica" %>

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

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);

        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.disabled = true;
        }

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Apoio&nbsp;&nbsp;>&nbsp;&nbsp;Ficha Técnica</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <br />
            <div style="float: left; width: 1000px; margin-left: 15%;">
                <fieldset>
                    <legend>Ficha Técnica</legend>
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
                            <td>Produto
                                <asp:HiddenField ID="hidCodigoProduto" runat="server" />
                            </td>
                            <td>Descrição
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 210px;">
                                <asp:TextBox ID="txtModelo" runat="server" Width="200px" Text=""></asp:TextBox>
                            </td>
                            <td style="width: 260px;">
                                <asp:TextBox ID="txtModeloDescricao" runat="server" Width="250px"></asp:TextBox>
                            </td>
                            <td style="width: 190px;">&nbsp;
                                <asp:ImageButton ID="ibtCopiarHB" runat="server" Width="17px" ImageUrl="~/Image/copy.png"
                                    Visible="false" ToolTip="Copiar Ficha do HB" OnClick="ibtCopiarHB_Click" />
                            </td>
                            <td>&nbsp;
                            </td>
                            <td style="text-align: right;">
                                <asp:ImageButton ID="ibtAprovarFichaTecnica" runat="server" Width="17px" ImageUrl="~/Image/disapprove.png"
                                    Visible="false" ToolTip="Aprovar Ficha Técnica" OnClick="ibtAprovarFichaTecnica_Click"
                                    OnClientClick="return Confirmar('Deseja realmente Aprovar esta Ficha Técnica?');" />
                                <asp:ImageButton ID="ibtAbrirFichaTecnica" runat="server" Width="15px" ImageUrl="~/Image/approve.png"
                                    Visible="false" ToolTip="Abrir Ficha Técnica" OnClick="ibtAprovarFichaTecnica_Click" OnClientClick="return Confirmar('Deseja realmente Abrir esta Ficha Técnica?');" />
                            </td>
                        </tr>
                        <tr>
                            <td>Cor
                            </td>
                            <td>Cor Fornecedor
                            </td>
                            <td>Fornecedor
                            </td>
                            <td>Tecido
                            </td>
                            <td style="text-align: right;">Gradeado
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtCor" runat="server" Width="200px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCorFornecedor" runat="server" Width="250px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFornecedor" runat="server" Width="180px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTecido" runat="server" Width="237px"></asp:TextBox>
                            </td>
                            <td style="text-align: right;">
                                <asp:CheckBox ID="chkGradeado" runat="server" Checked="false" OnCheckedChanged="chkGradeado_CheckedChanged" AutoPostBack="true" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;" colspan="5">
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
                    </table>
                    <br />
                    <div id="tabs">
                        <ul>
                            <li><a href="#tabs-material" id="tabMaterial" runat="server" onclick="MarcarAba(0);">Materiais</a></li>
                            <li><a href="#tabs-alterar-material" id="tabAlterarMaterial" runat="server" onclick="MarcarAba(1);">Inclusão / Alteração de Materiais</a></li>
                            <li><a href="#tabs-copia" id="tabCopia" runat="server" onclick="MarcarAba(2);">Cópia</a></li>
                            <li><a href="#tabs-filtro" id="tabFiltro" runat="server" onclick="MarcarAba(3);">Filtros</a></li>
                            <li>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </li>
                            <li><a href="#tabs-notificacao" id="tabNotificacao" runat="server" onclick="MarcarAba(4);">
                                <asp:Label ID="labNotificacao" runat="server" Text="Notificações"></asp:Label></a></li>
                            <li><a href="#tabs-todos" id="tabTodos" runat="server" onclick="MarcarAba(5);">Todos</a></li>
                        </ul>
                        <div id="tabs-material">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                <tr>
                                    <td colspan="5">
                                        <fieldset>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvMateriais" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvMateriais_RowDataBound"
                                                    ShowFooter="true" DataKeyNames="CODIGO">
                                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="25px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btEditarMaterial" runat="server" Height="15px" Width="15px"
                                                                    ImageUrl="~/Image/edit.jpg" OnClick="btEditarMaterial_Click" ToolTip="Editar" />
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:ImageButton ID="btIncluirMaterial" runat="server" Height="18px" Width="18px"
                                                                    ImageUrl="~/Image/add.png" OnClick="btIncluirMaterial_Click" ToolTip="Incluir" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Detalhe" ItemStyle-Width="105px" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labDetalhe" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Grupo" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labGrupo" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SubGrupo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labSubGrupo" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cor" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labCor" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cor Fornecedor" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labRefCor" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Consumo" ItemStyle-Width="90px" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labConsumo" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
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
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="tabs-alterar-material">
                            <div class="bodyMaterial">
                                <fieldset style="margin-top: 4px; padding-top: 0;">
                                    <legend>
                                        <asp:Label ID="labMaterial" runat="server" Text="Material"></asp:Label></legend>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td colspan="5">
                                                <asp:Label ID="labMaterialFiltro" runat="server" Text="Material"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">
                                                <asp:TextBox ID="txtMaterialFiltro" runat="server" MaxLength="10" Width="180px" Height="16px"
                                                    OnTextChanged="txtMaterialFiltro_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="labGrupo" runat="server" Text="Grupo"></asp:Label>
                                                <asp:HiddenField ID="hidCodigoFichaTecnica" runat="server" />
                                                <asp:HiddenField ID="hidAcaoMaterial" runat="server" />
                                                <asp:HiddenField ID="hidDetalhe" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="labSubGrupo" runat="server" Text="SubGrupo"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="labCor" runat="server" Text="Cor"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labFornecedor" runat="server" Text="Fornecedor"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 190px;">
                                                <asp:DropDownList ID="ddlMaterialGrupo" runat="server" Width="184px" Height="21px"
                                                    DataTextField="GRUPO" DataValueField="CODIGO_GRUPO" OnSelectedIndexChanged="ddlMaterialGrupo_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 260px;">
                                                <asp:DropDownList ID="ddlMaterialSubGrupo" runat="server" Width="254px" Height="21px"
                                                    DataTextField="SUBGRUPO" DataValueField="CODIGO_SUBGRUPO" OnSelectedIndexChanged="ddlMaterialSubGrupo_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 260px;" colspan="2">
                                                <asp:DropDownList ID="ddlCor" runat="server" Width="254px" Height="21px" DataTextField="DESC_COR"
                                                    DataValueField="COR" OnSelectedIndexChanged="ddlCor_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtRefCor" runat="server" Width="120px" MaxLength="20" Visible="false"
                                                    Height="15px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlFornecedor" runat="server" Width="185px" Height="21px" DataTextField="FORNECEDOR"
                                                    DataValueField="FORNECEDOR">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="labDetalhe" runat="server" Text="Detalhe"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labConsumo" runat="server" Text="Consumo"></asp:Label>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlDetalhe" runat="server" Width="184px" DataTextField="DESCRICAO"
                                                    DataValueField="CODIGO">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtConsumo" runat="server" Width="250px" MaxLength="6" Height="15px"
                                                    onkeypress="return fnValidarNumeroDecimal(event);" CssClass="alinharDireita"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="btSalvarMaterial" runat="server" Width="19px" ImageUrl="~/Image/approve.png"
                                                    ToolTip="Salvar" OnClick="btSalvarMaterial_Click" />
                                                <asp:ImageButton ID="btCancelarMaterial" runat="server" Width="18px" ImageUrl="~/Image/cancel.png"
                                                    ToolTip="Cancelar" OnClick="btCancelarMaterial_Click" />
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                        </div>
                        <div id="tabs-copia">
                            <div class="bodyMaterial">
                                <fieldset style="margin-top: 4px; padding-top: 0;">
                                    <legend>
                                        <asp:Label ID="labCopiaTitulo" runat="server" Text="Modelo"></asp:Label></legend>
                                    <asp:Panel ID="pnlCopiaModelo" runat="server">
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>Modelo
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 210px;">
                                                    <asp:TextBox ID="txtModeloCopia" runat="server" Width="200px" Text=""></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="ibtPesquisarModelo" runat="server" Width="19px" ImageUrl="~/Image/search.png"
                                                        ToolTip="Pesquisar" OnClick="ibtPesquisarModelo_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvModeloCopia" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvModeloCopia_RowDataBound"
                                                            DataKeyNames="CODIGO">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
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
                                                                            <asp:GridView ID="gvFotoCopia" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvFotoCopia_RowDataBound"
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
                                                                <asp:TemplateField HeaderText="Modelo" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="labModelo" runat="server" Text='<%# Bind("MODELO") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Nome" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="labNome" runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Cor" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="labCor" runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Cor Fornecedor" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="labCorFornecedor" runat="server" Text='<%# Bind("FORNECEDOR_COR") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    ItemStyle-Width="25px">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btCopiar" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/copy.png"
                                                                            OnClick="btCopiar_Click" ToolTip="Copiar" OnClientClick="return Confirmar('Deseja realmente copiar esta Ficha Técnica?');" />
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
                            </div>
                        </div>
                        <div id="tabs-filtro">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                <tr>
                                    <td>Coleção
                                    </td>
                                    <td>Origem
                                    </td>
                                    <td>Grupo Produto
                                    </td>
                                    <td>Tecido
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 210px;">
                                        <asp:DropDownList ID="ddlColecaoFiltro" runat="server" Width="204px" Height="21px"
                                            DataTextField="DESC_COLECAO" DataValueField="COLECAO" OnSelectedIndexChanged="ddlColecaoFiltro_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 210px;">
                                        <asp:DropDownList ID="ddlOrigemFiltro" runat="server" Width="204px" Height="21px"
                                            DataTextField="DESCRICAO" DataValueField="CODIGO">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 210px;">
                                        <asp:DropDownList ID="ddlGrupoProdutoFiltro" runat="server" Width="204px" Height="21px"
                                            DataTextField="GRUPO_PRODUTO" DataValueField="GRUPO_PRODUTO">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTecidoFiltro" runat="server" Width="295px" Height="21px"
                                            DataTextField="TECIDO_POCKET" DataValueField="TECIDO_POCKET">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Fornecedor
                                    </td>
                                    <td>Griffe
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td style="text-align: right;">Ficha Técnica Não Aprovada
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlFornecedorFiltro" runat="server" Width="204px" Height="21px"
                                            DataTextField="FORNECEDOR" DataValueField="FORNECEDOR">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlGriffeFiltro" runat="server" Width="204px" Height="21px"
                                            DataTextField="GRIFFE" DataValueField="GRIFFE">
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:CheckBox ID="chkSemFicha" runat="server" Checked="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>Grupo
                                    </td>
                                    <td>SubGrupo
                                    </td>
                                    <td colspan="2">Cor
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlMaterialGrupoFiltro" runat="server" Width="204px" Height="21px"
                                            DataTextField="GRUPO" DataValueField="CODIGO_GRUPO" OnSelectedIndexChanged="ddlMaterialGrupo_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlMaterialSubGrupoFiltro" runat="server" Width="204px" Height="21px"
                                            DataTextField="SUBGRUPO" DataValueField="CODIGO_SUBGRUPO" OnSelectedIndexChanged="ddlMaterialSubGrupo_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlMaterialCorFiltro" runat="server" Width="505px" Height="21px" DataTextField="DESC_COR"
                                            DataValueField="COR">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="tabs-notificacao">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                <tr>
                                    <td>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvNotificacao" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvNotificacao_RowDataBound"
                                                DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
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
                                                                <asp:GridView ID="gvNotificacaoFoto" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvNotificacaoFoto_RowDataBound"
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
                                                    <asp:TemplateField HeaderText="Modelo" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="130px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labModelo" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Nome" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="180px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labNome" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cor" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="150px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labCor" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cor Fornecedor" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labCorFornecedor" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Mensagem" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labMsg" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                        ItemStyle-Width="25px">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btMarcarLido" runat="server" Height="14px" Width="14px" ImageUrl="~/Image/delete.png"
                                                                OnClick="btMarcarLido_Click" ToolTip="Marcar como Lido" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                        ItemStyle-Width="25px">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btPesquisar" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/search.png"
                                                                OnClick="btPesquisarTodos_Click" ToolTip="Pesquisar" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="tabs-todos">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                <tr>
                                    <td>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvTodos" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvTodos_RowDataBound"
                                                DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
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
                                                    <asp:TemplateField HeaderText="Modelo" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labModelo" runat="server" Text='<%# Bind("MODELO") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Nome" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labNome" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cor" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labCor" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cor Fornecedor" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labCorFornecedor" runat="server" Text='<%# Bind("FORNECEDOR_COR") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                        ItemStyle-Width="25px">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btPesquisar" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/search.png"
                                                                OnClick="btPesquisarTodos_Click" ToolTip="Pesquisar" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </fieldset>
                <br />
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
