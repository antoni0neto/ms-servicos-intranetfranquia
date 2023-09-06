<%@ Page Title="Cadastro de Produto" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="cprod_produto_cadastro.aspx.cs" Inherits="Relatorios.cprod_produto_cadastro" %>

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
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Controle de Produto&nbsp;&nbsp;>&nbsp;&nbsp;Cadastros&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Produto</span>
                <div style="float: right; padding: 0;">
                    <a href="cprod_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Cadastro de Produto</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">

                        <tr>
                            <td>Coleção</td>
                            <td>Griffe</td>
                            <td>Grupo Produto</td>
                            <td>Produto</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 250px;">
                                <asp:DropDownList ID="ddlColecoes" runat="server" Width="244px" Height="21px" DataTextField="DESC_COLECAO"
                                    DataValueField="COLECAO">
                                </asp:DropDownList>
                            </td>

                            <td style="width: 170px">
                                <asp:DropDownList ID="ddlGriffe" runat="server" Width="164px" Height="21px" DataTextField="GRIFFE"
                                    DataValueField="GRIFFE">
                                </asp:DropDownList>
                            </td>

                            <td style="width: 200px">
                                <asp:DropDownList ID="ddlGrupo" runat="server" Width="194px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                    DataValueField="GRUPO_PRODUTO">
                                </asp:DropDownList>
                            </td>


                            <td style="width: 140px;">
                                <asp:TextBox ID="txtProduto" runat="server" Width="130px" MaxLength="5" CssClass="alinharDireita"
                                    onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" OnClientClick="DesabilitarBotao(this);" />
                                &nbsp;&nbsp;<asp:Label ID="labMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <div id="accordionP">
                                    <h3>Produtos</h3>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound"
                                                            ShowFooter="true" DataKeyNames="COLECAO">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Coleção" HeaderStyle-Width="120px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColecao" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Produto" HeaderStyle-Width="160px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litModelo" runat="server" Text='<%# Bind("MODELO") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Cor">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litCor" runat="server" Text='<%# Bind("DESC_COR") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Grupo Produto" HeaderStyle-Width="200px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litGrupo" runat="server" Text='<%# Bind("GRUPO") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Griffe" HeaderStyle-Width="200px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litGriffe" runat="server" Text='<%# Bind("GRIFFE") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Produto Acabado">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litProdutoAcabado" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Enviado">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litEnviado" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                                    <ItemTemplate>
                                                                        <asp:Button ID="btBaixar" runat="server" Text="Enviar" Height="21px" OnClick="btBaixar_Click" />
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
                                <div style="float: right;">
                                    <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
