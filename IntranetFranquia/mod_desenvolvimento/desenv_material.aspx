<%@ Page Title="Cadastro de Materiais" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master"
    CodeBehind="desenv_material.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="Relatorios.mod_desenvolvimento.desenv_material" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
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
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
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
        <Triggers>
            <asp:PostBackTrigger ControlID="btFotoTecidoIncluir" />
        </Triggers>
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro
                    de Apoio&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Materiais</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <br />
            <div style="float: left; width: 1000px; margin-left: 15%;">
                <fieldset>
                    <legend>Cadastro de Materiais</legend>
                    <div style="padding-left: 0px;">
                        <asp:ImageButton ID="ibtPesquisar" runat="server" Width="19px" ImageUrl="~/Image/search.png"
                            ToolTip="Pesquisar" OnClick="ibtPesquisar_Click" />
                        <asp:ImageButton ID="ibtSalvar" runat="server" Width="19px" ImageUrl="~/Image/approve.png"
                            ToolTip="Salvar" OnClick="ibtSalvar_Click" />
                        <asp:ImageButton ID="ibtCancelar" runat="server" Width="18px" ImageUrl="~/Image/cancel.png"
                            ToolTip="Cancelar" OnClick="ibtCancelar_Click" />
                        <asp:ImageButton ID="ibtIncluir" runat="server" Width="20px" ImageUrl="~/Image/add.png"
                            ToolTip="Incluir" OnClick="ibtIncluir_Click" />
                        <asp:ImageButton ID="ibtEditar" runat="server" Width="20px" ImageUrl="~/Image/edit.jpg"
                            ToolTip="Editar" OnClick="ibtEditar_Click" />
                        <asp:ImageButton ID="ibtExcluir" runat="server" Width="18px" ImageUrl="~/Image/delete.png"
                            ToolTip="Excluir" OnClick="ibtExcluir_Click" OnClientClick="return ConfirmarExclusao();" />
                        <asp:ImageButton ID="ibtLimpar" runat="server" Width="18px" ImageUrl="~/Image/clean.png"
                            ToolTip="Limpar" OnClick="ibtLimpar_Click" />
                        <asp:HiddenField ID="hidTabSelected" runat="server" Value="" />
                        <asp:HiddenField ID="hidAcao" runat="server" Value="" />
                        <div style="text-align: right; float: right;">
                            <asp:Label ID="labErro" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                    <hr />
                    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                        <tr>
                            <td>Material
                            </td>
                            <td>
                                <asp:Label ID="labMaterialDescricao" runat="server" Text="Descrição"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 210px;">
                                <asp:TextBox ID="txtMaterial" runat="server" Width="200px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMaterialDescricao" runat="server" Width="300px" MaxLength="40"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div id="tabs">
                        <ul>
                            <li><a href="#tabs-material" id="tabMateriaPrima" runat="server" onclick="MarcarAba(0);">Matéria-Prima</a></li>
                            <li><a href="#tabs-composicao" id="tabComposicao" runat="server" onclick="MarcarAba(1);">Composição</a></li>
                            <li><a href="#tabs-lavagem" id="tabLavagem" runat="server" onclick="MarcarAba(2);">Lavagem</a></li>
                            <li><a href="#tabs-foto" id="tabFoto" runat="server" onclick="MarcarAba(3);">Foto</a></li>
                            <li><a href="#tabs-tributacao" id="tabTributacao" runat="server" onclick="MarcarAba(4);">Tributações</a></li>
                            <li>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </li>
                            <li><a href="#tabs-notificacao" id="tabNotificacao" runat="server" onclick="MarcarAba(5);">
                                <asp:Label ID="labNotificacao" runat="server" Text="Notificações"></asp:Label></a></li>
                            <li><a href="#tabs-todos" id="tabTodos" runat="server" onclick="MarcarAba(6);">Todos</a></li>
                        </ul>
                        <div id="tabs-material">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                <tr>
                                    <td>
                                        <asp:Label ID="labColecao" runat="server" Text="Coleção"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labGrupo" runat="server" Text="Grupo"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labSubGrupo" runat="server" Text="SubGrupo"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labTipoMaterial" runat="server" Text="Tipo do Material"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labUnidadeMedida" runat="server" Text="Unidade de Medida"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 180px;">
                                        <asp:DropDownList ID="ddlColecoes" runat="server" Width="174px" Height="21px" DataTextField="DESC_COLECAO"
                                            DataValueField="COLECAO">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 190px;">
                                        <asp:DropDownList ID="ddlMaterialGrupo" runat="server" Width="184px" Height="21px"
                                            DataTextField="GRUPO" DataValueField="CODIGO_GRUPO" OnSelectedIndexChanged="ddlMaterialGrupo_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 210px;">
                                        <asp:DropDownList ID="ddlMaterialSubGrupo" runat="server" Width="204px" Height="21px"
                                            DataTextField="SUBGRUPO" DataValueField="CODIGO_SUBGRUPO" OnSelectedIndexChanged="ddlMaterialSubGrupo_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 170px;">
                                        <asp:DropDownList ID="ddlTipoMaterial" runat="server" Width="164px" Height="21px"
                                            DataTextField="TIPO" DataValueField="TIPO">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlUnidadeMedida" runat="server" Width="175px" Height="21px"
                                            DataTextField="DESC_UNIDADE" DataValueField="UNIDADE1">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:Label ID="labFornecedor" runat="server" Text="Fornecedor"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlFornecedor" runat="server" Width="364px" Height="21px" DataTextField="FORNECEDOR"
                                            DataValueField="FORNECEDOR">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="3">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:HiddenField ID="hidDescricaoCor" runat="server" />
                                        <asp:HiddenField ID="hidReferFab" runat="server" />
                                        <fieldset>
                                            <legend>
                                                <asp:Label ID="labCores" runat="server" Text="Cores"></asp:Label></legend>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvCores" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvCores_RowDataBound"
                                                    OnDataBound="gvCores_DataBound" DataKeyNames="COR_MATERIAL" ShowFooter="true">
                                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="60px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btEditarCor" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/edit.jpg"
                                                                    OnClick="btEditarCor_Click" ToolTip="Editar" />
                                                                <asp:ImageButton ID="btSalvarCor" runat="server" Width="19px" ImageUrl="~/Image/approve.png"
                                                                    ToolTip="Salvar" OnClick="btSalvarCor_Click" />
                                                                <asp:ImageButton ID="btSairCor" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/cancel.png"
                                                                    OnClick="btSairCor_Click" ToolTip="Sair" />
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:ImageButton ID="btIncluirCor" runat="server" Height="18px" Width="18px" ImageUrl="~/Image/add.png"
                                                                    OnClick="btIncluirCor_Click" ToolTip="Incluir" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cor" FooterStyle-Width="200px" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labCor" runat="server" Text='<%# Bind("DESC_COR_MATERIAL") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:DropDownList ID="ddlCor" runat="server" Width="250px" Height="21px" DataTextField="DESC_COR"
                                                                    DataValueField="COR">
                                                                </asp:DropDownList>
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:DropDownList ID="ddlCorFooter" runat="server" Width="250px" Height="21px" DataTextField="DESC_COR"
                                                                    DataValueField="COR">
                                                                </asp:DropDownList>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cor Fornecedor" FooterStyle-Width="300px" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labRefCor" runat="server" Text='<%# Bind("REFER_FABRICANTE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtRefCor" runat="server" Width="296px" MaxLength="25"></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtRefCorFooter" runat="server" Width="296px" MaxLength="25"></asp:TextBox>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Quantidade Estoque" Visible="false" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="labQtdeEstoque" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="25px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btExcluirCor" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/delete.png"
                                                                    OnClick="btExcluirCor_Click" OnClientClick="return ConfirmarExclusao();" ToolTip="Excluir" />
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
                        <div id="tabs-composicao">
                            <div class="bodyMaterial">
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 146px;">Quantidade
                                                        <asp:HiddenField ID="hidTotalComp" runat="server" />
                                                    </td>
                                                    <td style="width: 260px;">Descrição
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 146px;">
                                                        <asp:TextBox ID="txtComposicaoQtde" runat="server" CssClass="alinharDireita" MaxLength="6"
                                                            onkeypress="return fnValidarNumeroDecimal(event);" Width="136px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 260px;">
                                                        <asp:TextBox ID="txtComposicaoDescricao" runat="server" Width="251px" MaxLength="70"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="btComposicaoIncluir" runat="server" Width="19px" ImageUrl="~/Image/approve.png"
                                                            ToolTip="Incluir" OnClick="btComposicaoIncluir_Click" Visible="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="labErroComposicao" runat="server" Visible="true" ForeColor="Red" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvComposicao" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvComposicao_RowDataBound"
                                                    OnDataBound="gvComposicao_DataBound">
                                                    <HeaderStyle BackColor="GradientActiveCaption" />
                                                    <FooterStyle BackColor="GradientActiveCaption" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litComposicaoColuna" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="QTDE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="Quantidade" HeaderStyle-Width="140px" />
                                                        <asp:BoundField DataField="DESCRICAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderText="Descrição" HeaderStyle-Width="715px" />
                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btComposicaoExcluir" runat="server" Height="15px" ImageUrl="~/Image/delete.png"
                                                                    OnClientClick="return ConfirmarExclusao();" OnClick="btComposicaoExcluir_Click"
                                                                    ToolTip="Excluir" />
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
                        <div id="tabs-lavagem">
                            <div class="bodyMaterial">
                                <fieldset>
                                    <legend>Lavagem</legend>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlLavagem" runat="server" BackColor="White" BorderWidth="1" BorderColor="White"
                                                    Height="100%">
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvLavagem" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvLavagem_RowDataBound" OnDataBound="gvLavagem_DataBound">
                                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgColuna1" runat="server" OnClick="imgColuna_Click" Width="35px" Height="35px"></asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgColuna2" runat="server" OnClick="imgColuna_Click" Width="35px" Height="35px"></asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgColuna3" runat="server" OnClick="imgColuna_Click" Width="35px" Height="35px"></asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgColuna4" runat="server" OnClick="imgColuna_Click" Width="35px" Height="35px"></asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgColuna5" runat="server" OnClick="imgColuna_Click" Width="35px" Height="35px"></asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgColuna6" runat="server" OnClick="imgColuna_Click" Width="35px" Height="35px"></asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgColuna7" runat="server" OnClick="imgColuna_Click" Width="35px" Height="35px"></asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgColuna8" runat="server" OnClick="imgColuna_Click" Width="35px" Height="35px"></asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgColuna9" runat="server" OnClick="imgColuna_Click" Width="35px" Height="35px"></asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgColuna10" runat="server" OnClick="imgColuna_Click" Width="35px" Height="35px"></asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgColuna11" runat="server" OnClick="imgColuna_Click" Width="35px" Height="35px"></asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgColuna12" runat="server" OnClick="imgColuna_Click" Width="35px" Height="35px"></asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgColuna13" runat="server" OnClick="imgColuna_Click" Width="35px" Height="35px"></asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgColuna14" runat="server" OnClick="imgColuna_Click" Width="35px" Height="35px"></asp:ImageButton>
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
                        <div id="tabs-foto">
                            <div class="bodyMaterial">
                                <fieldset style="margin-top: 4px; padding-top: 0;">
                                    <legend>
                                        <asp:Label ID="labFotoTecido" runat="server" Text="Foto Material"></asp:Label></legend>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 270px;">
                                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="width: 150px; vertical-align: top;">
                                                <asp:FileUpload ID="uploadFotoTecido" runat="server" Width="300px" /><br />
                                                <br />
                                                <br />
                                                <br />
                                                <br />
                                                <asp:Button ID="btFotoTecidoIncluir" runat="server" OnClick="btFotoTecidoIncluir_Click"
                                                    Text=">>" Width="32px" />
                                            </td>
                                            <td>
                                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="float: right;">
                                                            <asp:Image ID="imgFotoTecido" runat="server" BorderColor="Black" BorderWidth="0" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <div id="divErroFotoTecido" runat="server" style="text-align: center;">
                                                    <asp:Label ID="labErroFotoTecido" runat="server" Visible="true" ForeColor="Red" Text=""></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                        </div>
                        <div id="tabs-tributacao">
                            <div class="bodyMaterial">
                                <fieldset>
                                    <legend>Tributação</legend>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                        <tr>
                                            <td>
                                                <asp:Label ID="labClassifFiscal" runat="server" Text="Classificação Fiscal"></asp:Label>
                                            </td>
                                            <td>Origem
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 460px;">
                                                <asp:DropDownList ID="ddlClassifFiscal" runat="server" Width="454px" Height="21px"
                                                    DataTextField="DESC_CLASSIFICACAO" DataValueField="CLASSIF_FISCAL1">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlOrigem" runat="server" Width="435px" Height="21px" DataTextField="DESCRICAO"
                                                    DataValueField="TRIBUT_ORIGEM1">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <fieldset>
                                    <legend>Contas Contábeis</legend>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                        <tr>
                                            <td>Estoque
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 250px;">
                                                <asp:DropDownList ID="ddlContaContabilEstoque1" runat="server" Width="244px" Height="21px"
                                                    DataTextField="CONTA_CONTABIL" DataValueField="CONTA_CONTABIL" OnSelectedIndexChanged="ddlContaContabil"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtContaContabilEstoque1" runat="server" Width="640px" ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Compra
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlContaContabilCompra1" runat="server" Width="244px" Height="21px"
                                                    DataTextField="CONTA_CONTABIL" DataValueField="CONTA_CONTABIL" OnSelectedIndexChanged="ddlContaContabil"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtContaContabilCompra1" runat="server" Width="640px" ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Venda
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlContaContabilVenda1" runat="server" Width="244px" Height="21px"
                                                    DataTextField="CONTA_CONTABIL" DataValueField="CONTA_CONTABIL" OnSelectedIndexChanged="ddlContaContabil"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtContaContabilVenda1" runat="server" Width="640px" ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <fieldset>
                                    <legend>Contas Contábeis Devolução</legend>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                        <tr>
                                            <td>Compra
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 250px;">
                                                <asp:DropDownList ID="ddlContaContabilCompra2" runat="server" Width="244px" Height="21px"
                                                    DataTextField="CONTA_CONTABIL" DataValueField="CONTA_CONTABIL" OnSelectedIndexChanged="ddlContaContabil"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtContaContabilCompra2" runat="server" Width="640px" ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Venda
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlContaContabilVenda2" runat="server" Width="244px" Height="21px"
                                                    DataTextField="CONTA_CONTABIL" DataValueField="CONTA_CONTABIL" OnSelectedIndexChanged="ddlContaContabil"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtContaContabilVenda2" runat="server" Width="640px" ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <fieldset>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                        <tr>
                                            <td>Característica Contábil
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="width: 250px;">
                                                <asp:DropDownList ID="ddlCaractContabil" runat="server" Width="895px" Height="21px"
                                                    DataTextField="DESCRICAO_INDICADOR_CFOP" DataValueField="INDICADOR_CFOP">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Tipo Sped
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlTipoSped" runat="server" Width="244px" Height="21px" DataTextField="DESC_TIPO_ITEM"
                                                    DataValueField="COD_TIPO_SPED">
                                                </asp:DropDownList>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                        </div>
                        <div id="tabs-notificacao">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                <tr>
                                    <td>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvNotificacao" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvNotificacao_RowDataBound"
                                                DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Grupo" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="130px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labGrupo" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SubGrupo" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="180px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labSubGrupo" runat="server"></asp:Label>
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
                                                    <asp:TemplateField HeaderText="Fornecedor" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labFornecedor" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                        ItemStyle-Width="25px">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btMarcarLido" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/approve.png"
                                                                OnClick="btMarcarLido_Click" ToolTip="Marcar como Lido" />
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
                                                DataKeyNames="MATERIAL">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Material" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labMaterial" runat="server" Text='<%# Bind("MATERIAL") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Grupo" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="130px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labGrupo" runat="server" Text='<%# Bind("GRUPO") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SubGrupo" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="155px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labSubGrupo" runat="server" Text='<%# Bind("SUBGRUPO") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Fornecedor" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labFabricante" runat="server" Text='<%# Bind("FABRICANTE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Descrição" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labDescricaoMaterial" runat="server" Text='<%# Bind("DESC_MATERIAL") %>'></asp:Label>
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
