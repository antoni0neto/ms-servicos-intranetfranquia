<%@ Page Title="Manutenção de Produto" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="desenv_produto_manut.aspx.cs" Inherits="Relatorios.desenv_produto_manut"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .jGrowl .manilla
        {
            background-color: #000;
            color: white;
        }
        .alinharDireita
        {
            text-align: right;
        }
    </style>
    <script type="text/javascript" src="http://code.jquery.com/jquery.js"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript">
        /*(function ($) {
        $(function () {
        $.jGrowl.defaults.pool = 1;
        $.jGrowl("Produto alterado com sucesso.", {
        header: 'Produto',
        life: 900,
        theme: 'manilla',
        speed: 'slow',
        position: 'top-right',
        animateOpen: {
        height: "hide",
        width: "show"
        },
        animateClose: {
        height: "hide",
        width: "show"
        }
        });
        });
        })(jQuery);*/
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Planejamento
                    de Coleção Nacional&nbsp;&nbsp;>&nbsp;&nbsp;Manutenção de Produto</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Manutenção de Produto"></asp:Label></legend>
                <fieldset style="margin-top: 0px;">
                    <legend>Novo Produto</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0" style="margin-top: -10px;">
                        <tr>
                            <td>
                                <asp:Label ID="labColecao" runat="server" Text="Coleção"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labOrigem" runat="server" Text="Origem"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labCodigo" runat="server" Text="Código"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labGrupo" runat="server" Text="Grupo"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labModelo" runat="server" Text="Modelo"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labCor" runat="server" Text="Cor"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 200px;">
                                <asp:DropDownList ID="ddlColecoes" runat="server" Width="194px" Height="21px" DataTextField="DESC_COLECAO"
                                    DataValueField="COLECAO" AutoPostBack="true" OnSelectedIndexChanged="ddlColecoes_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 180px;">
                                <asp:DropDownList ID="ddlOrigem" runat="server" Width="174px" Height="21px" DataTextField="DESCRICAO"
                                    DataValueField="CODIGO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtCodigoRef" runat="server" Width="150px" MaxLength="8" CssClass="alinharDireita"
                                    onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                            </td>
                            <td style="width: 180px;">
                                <asp:DropDownList ID="ddlGrupo" runat="server" Width="174px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                    DataValueField="GRUPO_PRODUTO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 210px;">
                                <asp:TextBox ID="txtModelo" runat="server" Width="200px" MaxLength="20"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCor" runat="server" Width="200px" MaxLength="20"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="labQtde" runat="server" Text="Qtde Varejo"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labPreco" runat="server" Text="Preço Varejo"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labQtdeAtacado" runat="server" Text="Qtde Atacado"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labPrecoAtacado" runat="server" Text="Preço Atacado"></asp:Label>
                            </td>
                            <td style="text-align: center;">
                                Produto Acabado
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtQtdeVarejo" runat="server" Width="190px" MaxLength="10" CssClass="alinharDireita"
                                    onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPreco" runat="server" Width="170px" MaxLength="8" CssClass="alinharDireita"
                                    onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtQtdeAtacado" runat="server" Width="150px" MaxLength="10" CssClass="alinharDireita"
                                    onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPrecoAtacado" runat="server" Width="170px" MaxLength="8" CssClass="alinharDireita"
                                    onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                            </td>
                            <td style="width: 100px; text-align: center;">
                                <asp:CheckBox ID="chkProdutoAcabado" runat="server" Checked="false" Text="" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td style="text-align: right;">
                                <asp:Button ID="btSalvar" runat="server" Text="Salvar" Width="100px" OnClick="btSalvar_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7" style="text-align: right;">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="labSalvar" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset style="margin-top: 0px;">
                    <legend>Alterar Produto</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0" style="margin-top: -7px;">
                        <tr>
                            <td style="width: 190px;">
                                Coleção
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlColecoesBuscar" runat="server" Width="174px" Height="21px"
                                    DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:CheckBox ID="cbModeloFiltro" runat="server" AutoPostBack="false" Checked="false"
                                    TextAlign="Right" Text="Modelo Vazio" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Grupo
                            </td>
                            <td>
                                Modelo
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 180px;">
                                <asp:DropDownList ID="ddlGrupoBuscar" runat="server" Width="174px" Height="21px"
                                    DataTextField="GRUPO_PRODUTO" DataValueField="GRUPO_PRODUTO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 210px;">
                                <asp:TextBox ID="txtModeloBuscar" runat="server" Width="200px" MaxLength="10"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btBuscarProduto" runat="server" Text="Buscar Produto" Width="120px"
                                    OnClick="btBuscarProduto_Click" />&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="labProduto" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound"
                                        DataKeyNames="CODIGO" OnLoad="gvProduto_Load">
                                        <HeaderStyle BackColor="Gainsboro" />
                                        <FooterStyle BackColor="Gainsboro" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CODIGO_REF" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Código" HeaderStyle-Width="60px" />
                                            <asp:TemplateField HeaderText="Origem" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="140px">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlOrigem" runat="server" Width="155px" Height="21px" DataTextField="DESCRICAO"
                                                        DataValueField="CODIGO">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Grupo" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="160px">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlGrupo" runat="server" Width="165px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                                        DataValueField="GRUPO_PRODUTO">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Modelo" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="130px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtModelo" runat="server" Width="130px" MaxLength="25"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cor" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="165px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCor" runat="server" Width="165px" MaxLength="20"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="QtdeVarejo" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtQtdeVarejo" runat="server" Width="100px" MaxLength="6" CssClass="alinharDireita"
                                                        onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Preço Varejo" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="125px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPreco" runat="server" Width="125px" MaxLength="8" CssClass="alinharDireita"
                                                        onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qtde Atacado" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtQtdeAtacado" runat="server" Width="100px" MaxLength="6" CssClass="alinharDireita"
                                                        onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Preço Atacado" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="125px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPrecoAtacado" runat="server" Width="125px" MaxLength="8" CssClass="alinharDireita"
                                                        onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Button ID="btProdutoExcluir" runat="server" Height="19px" Width="90px" Text="Excluir"
                                                        OnClick="btProdutoExcluir_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="90px">
                                                <ItemTemplate>
                                                    <asp:Button ID="btProdutoSalvar" runat="server" Height="19px" Width="90px" Text="Salvar"
                                                        OnClick="btProdutoSalvar_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
