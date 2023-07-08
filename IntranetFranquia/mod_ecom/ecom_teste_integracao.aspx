<%@ Page Title="Cadastro de Produtos E-Commerce" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ecom_teste_integracao.aspx.cs" Inherits="Relatorios.ecom_teste_integracao" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btImgProduct" />
        </Triggers>
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Produtos E-Commerce</span>
                <div style="float: right; padding: 0;">
                    <a href="ecom_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Cadastro de Produtos E-Commerce</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <fieldset style="margin-top: 4px; padding-top: 0;">
                                    <legend>
                                        <asp:Label ID="labUploadImage" runat="server" Text="Imagem do Produto"></asp:Label></legend>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 150px; vertical-align: top;">
                                                <asp:FileUpload ID="uploadImgProduct" runat="server" Width="300px" /><br />
                                                <br />
                                                <br />
                                                <asp:Button ID="btImgProduct" runat="server" OnClick="btImgProduct_Click"
                                                    Text=">>" Width="32px" />
                                                <asp:TextBox ID="txtPathImage" runat="server" Text="" Width="300px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="text-align: right;">
                                                            <asp:Image ID="imgProduto" runat="server" Width="50px" Height="50px" AlternateText="Nenhum Arquivo..." BorderWidth="0" /><br />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtSKU" runat="server" Text="44" Width="100px"></asp:TextBox>
                                <asp:TextBox ID="txtCodigoCategoria" runat="server" Text="44" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btEnviarProduto" runat="server" Text="Enviar Produto" OnClick="btEnviarProduto_Click" />
                                <asp:Button ID="btCategoria" runat="server" Text="Associar Categoria" OnClick="btCategoria_Click" />
                                <asp:Button ID="btTeste" runat="server" Text="Fotos APP" OnClick="btTeste_Click" />
                                <asp:Button ID="btCriarFoto" runat="server" Text="Criar Foto" OnClick="btCriarFoto_Click" />
                                <asp:Button ID="btObterProdutoCategoria" runat="server" Text="Obter Produto Categoria" OnClick="btObterProdutoCategoria_Click" />

                                <asp:Button ID="btDiminuirFoto" runat="server" Text="Diminuir Fotos" OnClick="btDiminuirFoto_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btRelacionar" runat="server" Text="Relacionamento" OnClick="btRelacionar_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btNatal" runat="server" Text="Fotos Natal" OnClick="btNatal_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="">
                                <div id="accordionP">
                                    <h3>Produtos</h3>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound"
                                                            OnDataBound="gvProduto_DataBound"
                                                            ShowFooter="true">
                                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                            <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
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
                                <br />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
