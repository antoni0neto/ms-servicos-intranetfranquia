<%@ Page Title="Estoque de Aviamentos" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="desenv_pedido_aviamento_estoque.aspx.cs" Inherits="Relatorios.desenv_pedido_aviamento_estoque"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Aviamentos&nbsp;&nbsp;>&nbsp;&nbsp;Estoque de Aviamentos</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1200px; margin-left: 8%;">
                <fieldset>
                    <legend>Estoque de Aviamentos</legend>
                    <table border="0" cellpadding="0" cellspacing="0" class="tb" width="100%">
                        <tr>
                            <td>Material</td>
                            <td>Grupo Material</td>
                            <td>SubGrupo Material</td>
                            <td>Cor</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtMaterial" runat="server" Width="150px"></asp:TextBox>
                            </td>
                            <td style="width: 230px;">
                                <asp:DropDownList ID="ddlMaterialGrupo" runat="server" Width="224px" Height="21px"
                                    DataTextField="GRUPO" DataValueField="CODIGO_GRUPO" OnSelectedIndexChanged="ddlMaterialGrupo_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 230px;">
                                <asp:DropDownList ID="ddlMaterialSubGrupo" runat="server" Width="224px" Height="21px"
                                    DataTextField="SUBGRUPO" DataValueField="CODIGO_SUBGRUPO" OnSelectedIndexChanged="ddlMaterialSubGrupo_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 230px;">
                                <asp:DropDownList ID="ddlCor" runat="server" Width="224px" Height="21px" DataTextField="DESC_COR"
                                    DataValueField="COR">
                                </asp:DropDownList>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Button ID="btBuscar" runat="server" Width="100px" Text="Buscar" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />
                                &nbsp;
                                <asp:Label ID="labErro" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table border="0" cellpadding="0" cellspacing="0" class="tb" width="100%">
                        <tr>
                            <td style="width: 100%;">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvEstoqueAviamento" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvEstoqueAviamento_RowDataBound" OnSorting="gvEstoqueAviamento_Sorting" AllowSorting="true">
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Material" HeaderStyle-Width="150px" SortExpression="MATERIAL">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litMaterial" runat="server" Text='<%# Bind("MATERIAL") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Grupo Material" HeaderStyle-Width="" SortExpression="GRUPO_MATERIAL">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litGrupo" runat="server" Text='<%# Bind("GRUPO_MATERIAL") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="SubGrupo Material" HeaderStyle-Width="" SortExpression="SUBGRUPO_MATERIAL">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litSubGrupo" runat="server" Text='<%# Bind("SUBGRUPO_MATERIAL") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Cor Material" HeaderStyle-Width="" SortExpression="DESC_COR_MATERIAL">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litCor" runat="server" Text='<%# Bind("DESC_COR_MATERIAL") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Cor Fornecedor" HeaderStyle-Width="" SortExpression="COR_FORNECEDOR">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litCorFornecedor" runat="server" Text='<%# Bind("COR_FORNECEDOR") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Qtde" HeaderStyle-Width="120px" SortExpression="QTDE_ESTOQUE">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtde" runat="server" Text='<%# Bind("QTDE_ESTOQUE") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Unidade" HeaderStyle-Width="120px" SortExpression="UNID_ESTOQUE">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litUnidade" runat="server" Text='<%# Bind("UNID_ESTOQUE") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
