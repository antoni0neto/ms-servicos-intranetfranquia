<%@ Page Title="Relacionados - Criar Produto Relacionado" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ecom_rel_config_rel_criar_random.aspx.cs" Inherits="Relatorios.ecom_rel_config_rel_criar_random" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
        }

        .aligncenter {
            text-align: center;
        }
    </style>
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://code.jquery.com/jquery.js"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>

    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Produtos Relacionados&nbsp;&nbsp;>&nbsp;&nbsp;Criar Produto Relacionado</span>
                <div style="float: right; padding: 0;">
                    <a href="ecom_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Criar Produto Relacionado</legend>
                    <fieldset style="margin-top: 0px; padding-top: -50px;">
                        <legend>Produto</legend>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>Produto
                                    <asp:HiddenField ID="hidProduto" runat="server" Value="" />
                                    <asp:HiddenField ID="hidCor" runat="server" Value="" />
                                    <asp:HiddenField ID="hidIdMagento" runat="server" Value="" />
                                    <asp:HiddenField ID="hidCodigoEcom" runat="server" Value="" />
                                </td>
                                <td>Nome
                                </td>
                                <td>SKU
                                </td>
                                <td>Cor
                                </td>
                                <td>Grupo Produto
                                </td>
                                <td>Griffe
                                </td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 170px;">
                                    <asp:TextBox ID="txtProduto" runat="server" Width="160px" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width: 210px;">
                                    <asp:TextBox ID="txtNome" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width: 180px;">
                                    <asp:TextBox ID="txtSKU" runat="server" Width="170px" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width: 200px;">
                                    <asp:TextBox ID="txtCor" runat="server" Width="190px" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width: 190px;">
                                    <asp:TextBox ID="txtGrupoProduto" runat="server" Width="180px" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width: 190px;">
                                    <asp:TextBox ID="txtGriffe" runat="server" Width="180px" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width: 126px;">
                                    <asp:Button ID="btCriarRegra" runat="server" Width="120px" Text="Criar Regra" OnClick="btCriarRegra_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btRelacionarRegra" runat="server" Width="126px" Text="Relacionar Regra" OnClick="btRelacionarRegra_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <fieldset>
                                        <legend>Fotos</legend>

                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
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
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Button ID="btRandom" runat="server" Width="150px" Text="Trocar Produtos" OnClick="btRandom_Click" />
                                    &nbsp;
                                    <asp:Button ID="btApagarHistExc" runat="server" Width="150px" Text="Apagar Histórico" OnClick="btApagarHistExc_Click" />
                                    &nbsp;
                                    <asp:Label ID="labErro" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                                <td colspan="4" style="text-align: right;">
                                    <asp:Label ID="labErroMagento" runat="server" ForeColor="Red"></asp:Label>
                                    &nbsp;
                                    <asp:Button ID="btAbrirProdutosComprados" runat="server" Width="150px" Text="Compraram Também" OnClick="btAbrirProdutosComprados_Click" />
                                    &nbsp;
                                    <asp:Button ID="btAtualizarMagento" runat="server" Width="150px" Text="Atualizar Magento" OnClick="btAtualizarMagento_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <fieldset>
                                        <legend>Fotos Lado</legend>

                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="text-align: center;">
                                                    <asp:HiddenField ID="hidUpSell1" runat="server" Value="" />
                                                    SKU
                                                    <br />
                                                    <asp:TextBox ID="txtUpsell1" runat="server" Text="" Width="150px" MaxLength="12" CssClass="aligncenter" OnTextChanged="txtUpsell1_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                    <br />
                                                    <asp:CheckBox ID="cbUpSell1" runat="server" Checked="false" OnCheckedChanged="cbUpSell1_CheckedChanged" AutoPostBack="true" />
                                                    <br />
                                                    <asp:Image ID="imgUpSell1" runat="server" Width="156px" Height="228px" AlternateText="Nenhuma Foto" BorderWidth="0" />
                                                    <br />
                                                    Já foi relacionado
                                                    <asp:Label ID="labTotRelUp1" runat="server" Font-Bold="true"></asp:Label>
                                                    vez(es)
                                                </td>
                                                <td style="text-align: center;">
                                                    <asp:HiddenField ID="hidUpSell2" runat="server" Value="" />
                                                    SKU
                                                    <br />
                                                    <asp:TextBox ID="txtUpsell2" runat="server" Text="" Width="150px" MaxLength="12" CssClass="aligncenter" OnTextChanged="txtUpsell2_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                    <br />
                                                    <asp:CheckBox ID="cbUpSell2" runat="server" Checked="false" OnCheckedChanged="cbUpSell2_CheckedChanged" AutoPostBack="true" />
                                                    <br />
                                                    <asp:Image ID="imgUpSell2" runat="server" Width="156px" Height="228px" AlternateText="Nenhuma Foto" BorderWidth="0" />
                                                    <br />
                                                    Já foi relacionado
                                                    <asp:Label ID="labTotRelUp2" runat="server" Font-Bold="true"></asp:Label>
                                                    vez(es)
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <fieldset>
                                        <legend>Fotos Relacionados</legend>

                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="text-align: center;">
                                                    <asp:HiddenField ID="hidRelated1" runat="server" Value="" />
                                                    SKU
                                                    <br />
                                                    <asp:TextBox ID="txtRelated1" runat="server" Text="" Width="150px" MaxLength="12" CssClass="aligncenter" OnTextChanged="txtRelated1_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                    <br />
                                                    <asp:CheckBox ID="cbRelated1" runat="server" Checked="false" OnCheckedChanged="cbRelated1_CheckedChanged" AutoPostBack="true" />
                                                    <br />
                                                    <asp:Image ID="imgRelated1" runat="server" Width="156px" Height="228px" AlternateText="Nenhuma Foto" BorderWidth="0" />
                                                    <br />
                                                    Já foi relacionado
                                                    <asp:Label ID="labTotRel1" runat="server" Font-Bold="true"></asp:Label>
                                                    vez(es)
                                                </td>
                                                <td style="text-align: center;">
                                                    <asp:HiddenField ID="hidRelated2" runat="server" Value="" />
                                                    SKU
                                                    <br />
                                                    <asp:TextBox ID="txtRelated2" runat="server" Text="" Width="150px" MaxLength="12" CssClass="aligncenter" OnTextChanged="txtRelated2_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                    <br />
                                                    <asp:CheckBox ID="cbRelated2" runat="server" Checked="false" OnCheckedChanged="cbRelated2_CheckedChanged" AutoPostBack="true" />
                                                    <br />
                                                    <asp:Image ID="imgRelated2" runat="server" Width="156px" Height="228px" AlternateText="Nenhuma Foto" BorderWidth="0" />
                                                    <br />
                                                    Já foi relacionado
                                                    <asp:Label ID="labTotRel2" runat="server" Font-Bold="true"></asp:Label>
                                                    vez(es)
                                                </td>
                                                <td style="text-align: center;">
                                                    <asp:HiddenField ID="hidRelated3" runat="server" Value="" />
                                                    SKU
                                                    <br />
                                                    <asp:TextBox ID="txtRelated3" runat="server" Text="" Width="150px" MaxLength="12" CssClass="aligncenter" OnTextChanged="txtRelated3_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                    <br />
                                                    <asp:CheckBox ID="cbRelated3" runat="server" Checked="false" OnCheckedChanged="cbRelated3_CheckedChanged" AutoPostBack="true" />
                                                    <br />
                                                    <asp:Image ID="imgRelated3" runat="server" Width="156px" Height="228px" AlternateText="Nenhuma Foto" BorderWidth="0" />
                                                    <br />
                                                    Já foi relacionado
                                                    <asp:Label ID="labTotRel3" runat="server" Font-Bold="true"></asp:Label>
                                                    vez(es)
                                                </td>
                                                <td style="text-align: center;">
                                                    <asp:HiddenField ID="hidRelated4" runat="server" Value="" />
                                                    SKU
                                                    <br />
                                                    <asp:TextBox ID="txtRelated4" runat="server" Text="" Width="150px" MaxLength="12" CssClass="aligncenter" OnTextChanged="txtRelated4_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                    <br />
                                                    <asp:CheckBox ID="cbRelated4" runat="server" Checked="false" OnCheckedChanged="cbRelated4_CheckedChanged" AutoPostBack="true" />
                                                    <br />
                                                    <asp:Image ID="imgRelated4" runat="server" Width="156px" Height="228px" AlternateText="Nenhuma Foto" BorderWidth="0" />
                                                    <br />
                                                    Já foi relacionado
                                                    <asp:Label ID="labTotRel4" runat="server"></asp:Label>
                                                    vez(es)
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
