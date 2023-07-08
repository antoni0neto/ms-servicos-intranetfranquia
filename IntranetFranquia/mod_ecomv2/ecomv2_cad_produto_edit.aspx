<%@ Page Title="Cadastro de Produtos E-Commerce" Language="C#" AutoEventWireup="true" CodeBehind="ecomv2_cad_produto_edit.aspx.cs"
    Inherits="Relatorios.ecomv2_cad_produto_edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cadastro de Produtos E-Commerce</title>
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }
    </style>

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../js/js.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
                <div style="margin-top: 10px; padding-left: 12px; padding-right: 12px; padding-bottom: -350px; background-color: White;">
                    <br />
                    <div>
                        <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Produtos E-Commerce</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Cadastro de Produtos E-Commerce</legend>
                            <fieldset style="margin-top: -10px; padding-top: -50px;">
                                <legend>Fotos</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td colspan="6" style="text-align: center;">
                                            <asp:Label ID="labFotoProduto" runat="server" Text="Foto Original" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" style="text-align: center;">
                                            <asp:Image ID="imgProduto" runat="server" AlternateText="Nenhuma Foto" BorderWidth="0" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center;">
                                            <asp:Label ID="labFotoLook" runat="server" Text="Look" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:Label ID="labFotoFrenteCabeca" runat="server" Text="Frente Cabeça" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:Label ID="labFotoFrenteSemCabeca" runat="server" Text="Frente Sem Cabeça" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:Label ID="labFotoCostas" runat="server" Text="Costas" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:Label ID="labFotoDetalhe" runat="server" Text="Detalhe" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:Label ID="labFotoLado" runat="server" Text="Lado" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center;">
                                            <asp:Image ID="imgFotoLook" runat="server" Width="156px" Height="228px" AlternateText="Nenhuma Foto" BorderWidth="0" />
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:Image ID="imgFotoFrenteCabeca" runat="server" Width="156px" Height="228px" AlternateText="Nenhuma Foto" BorderWidth="0" />
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:Image ID="imgFotoFrenteSemCabeca" runat="server" Width="156px" Height="228px" AlternateText="Nenhuma Foto" BorderWidth="0" />
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:Image ID="imgFotoCostas" runat="server" Width="156px" Height="228px" AlternateText="Nenhuma Foto" BorderWidth="0" />
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:Image ID="imgFotoDetalhe" runat="server" Width="156px" Height="228px" AlternateText="Nenhuma Foto" BorderWidth="0" />
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:Image ID="imgFotoLado" runat="server" Width="156px" Height="228px" AlternateText="Nenhuma Foto" BorderWidth="0" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <fieldset>
                                                <legend>Foto da Vitrine</legend>
                                                <table border="0" width="100%" cellpadding="0" cellspacing="0" style="margin-top: -12px;">
                                                    <tr>
                                                        <td style="text-align: center;">
                                                            <asp:RadioButton ID="rbVitrineLook" runat="server" Checked="false" GroupName="vitrineGroup" />
                                                        </td>
                                                        <td style="text-align: center;">
                                                            <asp:RadioButton ID="rbVitrineFrenteCabeca" runat="server" Checked="false" GroupName="vitrineGroup" />
                                                        </td>
                                                        <td style="text-align: center;">
                                                            <asp:RadioButton ID="rbVitrineFrenteSemCabeca" runat="server" Checked="false" GroupName="vitrineGroup" />
                                                        </td>
                                                        <td style="text-align: center;">
                                                            <asp:RadioButton ID="rbVitrineCostas" runat="server" Checked="false" GroupName="vitrineGroup" />
                                                        </td>
                                                        <td style="text-align: center;">
                                                            <asp:RadioButton ID="rbVitrineDetalhe" runat="server" Checked="false" GroupName="vitrineGroup" />
                                                        </td>
                                                        <td style="text-align: center;">
                                                            <asp:RadioButton ID="rbVitrineLado" runat="server" Checked="false" GroupName="vitrineGroup" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <fieldset style="margin-top: -12px;">
                                                <legend>Hover da Vitrine</legend>
                                                <table border="0" width="100%" cellpadding="0" cellspacing="0" style="margin-top: -10px;">
                                                    <tr>
                                                        <td style="text-align: center;">
                                                            <asp:RadioButton ID="rbHoverLook" runat="server" Checked="false" GroupName="hoverGroup" />
                                                        </td>
                                                        <td style="text-align: center;">
                                                            <asp:RadioButton ID="rbHoverFrenteCabeca" runat="server" Checked="false" GroupName="hoverGroup" />
                                                        </td>
                                                        <td style="text-align: center;">
                                                            <asp:RadioButton ID="rbHoverFrenteSemCabeca" runat="server" Checked="false" GroupName="hoverGroup" />
                                                        </td>
                                                        <td style="text-align: center;">
                                                            <asp:RadioButton ID="rbHoverCostas" runat="server" Checked="false" GroupName="hoverGroup" />
                                                        </td>
                                                        <td style="text-align: center;">
                                                            <asp:RadioButton ID="rbHoverDetalhe" runat="server" Checked="false" GroupName="hoverGroup" />
                                                        </td>
                                                        <td style="text-align: center;">
                                                            <asp:RadioButton ID="rbHoverLado" runat="server" Checked="false" GroupName="hoverGroup" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <fieldset style="margin-top: -12px;">
                                                <legend>
                                                    <asp:Label ID="labOrdemFoto" runat="server" Text="Ordem das Fotos"></asp:Label></legend>
                                                <table border="0" width="100%" cellpadding="0" cellspacing="0" style="margin-top: -10px;">
                                                    <tr>
                                                        <td style="text-align: center;">
                                                            <asp:DropDownList ID="ddlOrdemLook" runat="server" Width="80px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="text-align: center;">
                                                            <asp:DropDownList ID="ddlOrdemFrenteCabeca" runat="server" Width="80px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="text-align: center;">
                                                            <asp:DropDownList ID="ddlOrdemFrenteSemCabeca" runat="server" Width="80px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="text-align: center;">
                                                            <asp:DropDownList ID="ddlOrdemCostas" runat="server" Width="80px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="text-align: center;">
                                                            <asp:DropDownList ID="ddlOrdemDetalhe" runat="server" Width="80px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="text-align: center;">
                                                            <asp:DropDownList ID="ddlOrdemLado" runat="server" Width="80px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <fieldset>
                                <legend>Produto</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td colspan="5">&nbsp;</td>
                                        <td style="text-align: right;">Cadastrar Produto no Marketplace?</td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">&nbsp;</td>
                                        <td style="text-align: right;">
                                            <asp:CheckBox ID="cbCadastrarMKTPlace" runat="server" Checked="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Produto
                                        </td>
                                        <td>SKU
                                        </td>
                                        <td>Cor Linx
                                        </td>
                                        <td>
                                            <asp:Label ID="labCorMagento" runat="server" Text="Cor Magento"></asp:Label>
                                        </td>
                                        <td>Grupo Produto
                                        </td>
                                        <td>
                                            <asp:Label ID="labGrupoMagento" runat="server" Text="Grupo Magento"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 185px;">
                                            <asp:TextBox ID="txtProduto" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                                Width="171px"></asp:TextBox>
                                        </td>
                                        <td style="width: 185px;">
                                            <asp:TextBox ID="txtSKU" runat="server" Width="171px"></asp:TextBox>
                                        </td>
                                        <td style="width: 185px;">
                                            <asp:TextBox ID="txtCorLinx" runat="server" Width="171px"></asp:TextBox>
                                        </td>
                                        <td style="width: 185px;">
                                            <asp:DropDownList ID="ddlCorMagento" runat="server" Width="179px" Height="21px" DataValueField="CODIGO" DataTextField="COR" OnSelectedIndexChanged="ddlCorMagento_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </td>
                                        <td style="width: 185px;">
                                            <asp:TextBox ID="txtGrupoProduto" runat="server" Width="171px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlGrupoMagento" runat="server" Width="179px" Height="21px" DataValueField="CODIGO" DataTextField="GRUPO"
                                                OnSelectedIndexChanged="ddlGrupoMagento_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="labNome" runat="server" Text="Nome Produto Linx"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="labNomeProdutoMagento" runat="server" Text="Nome Produto Magento"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labTipoRelacionado" runat="server" Text="Tipo Relacionado"></asp:Label>
                                        </td>
                                        <td>Bloco
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtNomeProduto" runat="server" Width="356px"></asp:TextBox>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtNomeProdutoMagento" runat="server" Width="356px" MaxLength="35" OnTextChanged="txtNomeProdutoMagento_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoRelacionado" runat="server" Width="179px" Height="21px" DataTextField="DESCRICAO"
                                                DataValueField="CODIGO">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlBloco" runat="server" Width="179px" Height="21px" DataTextField="BLOCO"
                                                DataValueField="CODIGO">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <asp:Label ID="labDescProduto" runat="server" Text="Descrição Produto"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <asp:TextBox ID="txtDescProduto" runat="server" Width="1100px" TextMode="MultiLine" Height="75px" OnTextChanged="txtDescProduto_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <asp:Label ID="labDescProdutoCurta" runat="server" Text="Descrição Curta Produto"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <asp:TextBox ID="txtDescProdutoCurta" runat="server" Width="1100px" TextMode="MultiLine" Enabled="false" Height="50px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="labPeso" runat="server" Text="Peso KG"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labGrupoMacro" runat="server" Text="Grupo Macro"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="labPreco" runat="server" Text="Preço"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:Label ID="labPrecoPromocional" runat="server" Text="Preço Promocional"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtPeso" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                                Width="356px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlGrupoMacro" runat="server" Width="179px" Height="21px" DataValueField="CODIGO" DataTextField="GRUPO_MACRO"></asp:DropDownList>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtPreco" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                                Width="356px"></asp:TextBox>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtPrecoPromocional" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                                Width="171px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="labURLProduto" runat="server" Text="URL Produto"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labCaixa" runat="server" Text="Caixa"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="labVisibilidade" runat="server" Text="Visibilidade"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labStatusCadastro" runat="server" Text="Status Cadastro"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtURLProduto" runat="server" Width="356px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCaixa" runat="server" Width="179px" DataValueField="CODIGO" DataTextField="DESCRICAO"></asp:DropDownList>
                                        </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="ddlVisibilidade" runat="server" Width="364px" Height="21px">
                                                <asp:ListItem Value="0" Text="Selecione"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Não Exibir Individualmente"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Catálogo"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Buscar"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="Catálogo, Busca" Selected="True"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStatusCadastro" runat="server" Width="171px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">Observação
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <asp:TextBox ID="txtObservacao" runat="server" Width="1096px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <fieldset>
                                <legend>Características</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="labTipoModelagem" runat="server" Text="Tipo Modelagem"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labTipoTecido" runat="server" Text="Tipo Tecido"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labTipoManga" runat="server" Text="Tipo Manga"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labTipoGola" runat="server" Text="Tipo Gola"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labTipoComprimento" runat="server" Text="Tipo Comprimento"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labTipoEstilo" runat="server" Text="Tipo Estilo"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoModelagem" runat="server" Width="179px" Height="21px" DataValueField="CODIGO" DataTextField="TIPO_MODELAGEM"></asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoTecido" runat="server" Width="179px" Height="21px" DataValueField="CODIGO" DataTextField="TIPO_TECIDO"></asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoManga" runat="server" Width="179px" Height="21px" DataValueField="CODIGO" DataTextField="TIPO_MANGA"></asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoGola" runat="server" Width="179px" Height="21px" DataValueField="CODIGO" DataTextField="TIPO_GOLA"></asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoComprimento" runat="server" Width="179px" Height="21px" DataValueField="CODIGO" DataTextField="TIPO_COMPRIMENTO"></asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoEstilo" runat="server" Width="179px" Height="21px" DataValueField="CODIGO" DataTextField="TIPO_ESTILO"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="labTipoLinha" runat="server" Text="Tipo Linha"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labSigned" runat="server" Text="Signed"></asp:Label>
                                        </td>
                                        <td>&nbsp;
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
                                            <asp:DropDownList ID="ddlTipoLinha" runat="server" Width="179px" Height="21px" DataValueField="CODIGO" DataTextField="TIPO_LINHA"></asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSigned" runat="server" Width="179px" Height="21px" DataValueField="CODIGO" DataTextField="SIGNED"></asp:DropDownList>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <fieldset>
                                <legend>Meta SEO</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">

                                    <tr>
                                        <td colspan="6">
                                            <asp:Label ID="labMetaTitulo" runat="server" Text="Meta Título"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <asp:TextBox ID="txtMetaTitulo" runat="server" Width="1096px" MaxLength="63"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <asp:Label ID="labMetaKeyword" runat="server" Text="Meta Palavras-Chaves"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <asp:TextBox ID="txtMetaKeyword" runat="server" Width="1096px" MaxLength="300"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <asp:Label ID="labMetaDescricao" runat="server" Text="Meta Descrição"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <asp:TextBox ID="txtMetaDescricao" runat="server" TextMode="MultiLine" Height="75px" Width="1100px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">&nbsp;</td>
                                    </tr>
                                </table>
                            </fieldset>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="6">
                                        <fieldset>
                                            <legend>Categorias/Momentos</legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>Categoria/Momento
                                                    </td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 210px;">
                                                        <asp:DropDownList ID="ddlCategoriaMag" runat="server" Width="204px" Height="21px" DataTextField="GRUPO"
                                                            DataValueField="CODIGO">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btAdicionarCategoria" runat="server" Width="100px" Text="Incluir" OnClick="btAdicionarCategoria_Click" />&nbsp;
                                                        <asp:Label ID="labErroCategoria" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvCategoria" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                    Style="background: white" OnRowDataBound="gvCategoria_RowDataBound" OnDataBound="gvCategoria_DataBound"
                                                    ShowFooter="true">
                                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" Font-Bold="true" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Categoria" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litCategoria" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btExcluirCategoriaProd" runat="server" Width="15px" ImageUrl="~/Image/delete.png" AlternateText=" " OnClick="btExcluirCategoriaProd_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <fieldset>
                                            <legend>Blocos</legend>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvBlocos" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                    Style="background: white" OnRowDataBound="gvBlocos_RowDataBound" OnDataBound="gvBlocos_DataBound"
                                                    ShowFooter="true">
                                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" Font-Bold="true" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Categoria" HeaderStyle-Width="300px" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litCategoria" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Bloco" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litBloco" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </fieldset>
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="6">
                                        <fieldset>
                                            <legend>Lojas</legend>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvLojas" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                    Style="background: white" OnRowDataBound="gvLojas_RowDataBound" ShowFooter="true">
                                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" Font-Bold="true" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Id" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litId" runat="server" Text='<%# Bind("ECOM_WEBSITE1.ID") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Código" HeaderStyle-Width="220px" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litCode" runat="server" Text='<%# Bind("ECOM_WEBSITE1.CODE") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Loja" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litNome" runat="server" Text='<%# Bind("ECOM_WEBSITE1.NAME") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </fieldset>
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="3">
                                        <asp:CheckBox ID="cbAtualizarImagem" runat="server" Text="Atualizar Imagens?" Checked="false" />
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Button ID="btAtualizarMagento" runat="server" Width="200px" Text="Atualizar Magento" OnClientClick="DesabilitarBotao(this);" OnClick="btAtualizarMagento_Click" />
                                    </td>
                                    <td style="width: 210px; text-align: right;">
                                        <asp:Button ID="btSalvar" runat="server" Width="200px" Text="Salvar" OnClientClick="DesabilitarBotao(this);" OnClick="btSalvar_Click" />&nbsp;
                                    </td>
                                    <td style="width: 210px; text-align: right;">
                                        <asp:Button ID="btSalvarContinuar" runat="server" Width="200px" Text="Salvar e Continuar" OnClientClick="DesabilitarBotao(this);" OnClick="btSalvarContinuar_Click" />&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:HiddenField ID="hidCodigo" runat="server" Value="" />
                                        <asp:HiddenField ID="hidIdMagento" runat="server" Value="" />
                                        <asp:HiddenField ID="hidIdMagentoConfig" runat="server" Value="" />
                                        <asp:HiddenField ID="hidColecao" runat="server" Value="" />
                                        <asp:HiddenField ID="hidProduto" runat="server" Value="" />
                                        <asp:HiddenField ID="hidCor" runat="server" Value="" />
                                        <asp:HiddenField ID="hidGriffe" runat="server" Value="" />
                                        <asp:HiddenField ID="hidGrupoProduto" runat="server" Value="" />
                                        <asp:HiddenField ID="hidCodCategoria" runat="server" Value="" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <br />
                </div>
                <div id="dialogPai" runat="server">
                    <div id="dialog" title="Mensagem" class="divPop">
                        <table border="0" width="100%">
                            <tr>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; color: red;">
                                    <strong>Aviso</strong>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; color: red;">ENVIADO COM SUCESSO
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
