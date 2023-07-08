<%@ Page Title="Cadastro de Origem" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="desenv_produto_origem.aspx.cs" Inherits="Relatorios.desenv_produto_origem"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <script src="http://code.jquery.com/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Planejamento
                    de Coleção Nacional&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Origem</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1200px; margin-left: 8%;">
                <fieldset>
                    <legend>Cadastro de Origem</legend>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="labColecao" runat="server" Text="Coleção"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labDescricao" runat="server" Text="Descrição"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labDataInicial" runat="server" Text="Data Inicial"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labDataFinal" runat="server" Text="Data Final"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labStatus" runat="server" Text="Status"></asp:Label>
                            </td>
                            <td>
                                <asp:HiddenField ID="hidCodigo" runat="server" />
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 220px;">
                                <asp:DropDownList ID="ddlColecoes" runat="server" Width="214px" Height="21px" DataTextField="DESC_COLECAO"
                                    DataValueField="COLECAO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 250px;">
                                <asp:TextBox ID="txtDescricao" runat="server" MaxLength="13"
                                    Width="240px"></asp:TextBox>
                            </td>
                            <td style="width: 130px;">
                                <asp:TextBox ID="txtDataInicial" runat="server" onkeypress="return fnReadOnly(event);"
                                    MaxLength="10" Width="120px" />
                            </td>
                            <td style="width: 130px;">
                                <asp:TextBox ID="txtDataFinal" runat="server" onkeypress="return fnReadOnly(event);"
                                    MaxLength="10" Width="120px" />
                            </td>
                            <td style="width: 140px;">
                                <asp:DropDownList ID="ddlStatus" Height="21px" runat="server" Width="134px">
                                    <asp:ListItem Text="Selecione" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Ativo" Selected="True" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="Inativo" Value="I"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btIncluir" Text="Incluir" OnClick="btIncluir_Click"
                                    Width="100px" Enabled="true" />
                            </td>
                            <td>
                                <div style="float: right; margin-right: 0px;">
                                    <asp:Button runat="server" ID="btCancelar" Text="Cancelar" OnClick="btCancelar_Click"
                                        Enabled="true" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="labErro" runat="server" ForeColor="red" Text="">
                                </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">&nbsp;
                            </td>
                        </tr>
                    </table>
                    <div>
                        <table border="0" cellpadding="0" cellspacing="0" class="tb" width="100%">
                            <tr>
                                <td style="width: 100%;">
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvProdutoOrigem" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvProdutoOrigem_RowDataBound">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                            <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderText="Coleção" HeaderStyle-Width="200px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColecao" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="DESCRICAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderText="Descrição" HeaderStyle-Width="230px" />

                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Data Inicial" HeaderStyle-Width="170px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDataInicial" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Data Final" HeaderStyle-Width="170px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDataFinal" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderText="Status">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="65px">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btAlterar" runat="server" Height="19px" Width="70px" Text="Alterar" OnClick="btAlterar_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="65px">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btExcluir" runat="server" Height="19px" Width="70px" Text="Excluir" OnClientClick="return ConfirmarExclusao();"
                                                            OnClick="btExcluir_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
