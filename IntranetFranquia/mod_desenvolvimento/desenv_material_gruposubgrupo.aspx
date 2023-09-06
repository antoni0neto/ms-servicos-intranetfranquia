<%@ Page Title="Cadastro de Grupo e Sub Grupo" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="desenv_material_gruposubgrupo.aspx.cs" Inherits="Relatorios.desenv_material_gruposubgrupo"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .jGrowl .manilla {
            background-color: #000;
            color: white;
        }

        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }
    </style>
    <script type="text/javascript" src="http://code.jquery.com/jquery.js"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:updatepanel id="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro
                    de Apoio&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Grupo e SubGrupo</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1200px; margin-left: 8%;">
                <fieldset>
                    <legend>Cadastro de Grupo e SubGrupo</legend>
                    <fieldset>
                        <legend>Grupo</legend>
                        <table border="0" cellspacing="0" cellpadding="0" width="100%" class="tb">
                            <tr style="height: 10px; vertical-align: bottom;">
                                <td>
                                    <asp:Label ID="labCodigo" runat="server" Text="Código"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labGrupo" runat="server" Text="Grupo"></asp:Label>
                                    <asp:HiddenField ID="hidGrupo" runat="server" />
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 110px;">
                                    <asp:TextBox ID="txtCodigo" runat="server" MaxLength="2" CssClass="alinharDireita" Width="100px"></asp:TextBox>
                                </td>
                                <td style="width: 180px;">
                                    <asp:TextBox ID="txtGrupo" runat="server" Width="170px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="btIncluirGrupo" runat="server" Text="Incluir" Width="100px" OnClick="btIncluirGrupo_Click" />
                                    <asp:Label ID="labErro" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                                <td style="text-align: right; width: 110px;">
                                    <asp:Button ID="btCancelar" runat="server" Text="Cancelar" Width="100px" OnClick="btCancelar_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvMaterialGrupo" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvMaterialGrupo_RowDataBound"
                                            DataKeyNames="GRUPO" AllowSorting="true" OnSorting="gvMaterialGrupo_Sorting">
                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="CODIGO_GRUPO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Código" HeaderStyle-Width="150px" SortExpression="CodigoGrupo" />
                                                <asp:BoundField DataField="GRUPO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderText="Grupo" SortExpression="Grupo" />
                                                <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-Width="25px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btExcluir" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/delete.png"
                                                            OnClick="btExcluirGrupo_Click" OnClientClick="return ConfirmarExclusao();" ToolTip="Excluir" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btAbrirSubGrupo" runat="server" Height="19px" Width="40px" Text=">>"
                                                            OnClick="btAbrirSubGrupo_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <fieldset id="fsSubGrupo" runat="server" visible="false">
                                        <legend>SubGrupo</legend>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvMaterialSubGrupo" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvMaterialSubGrupo_RowDataBound"
                                                OnDataBound="gvMaterialSubGrupo_DataBound" DataKeyNames="SUBGRUPO" AllowSorting="true"
                                                OnSorting="gvMaterialSubGrupo_Sorting" ShowFooter="true">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                        ItemStyle-Width="25px">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btEditar" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/edit.jpg"
                                                                OnClick="btEditar_Click" ToolTip="Editar" />
                                                            <asp:ImageButton ID="btSair" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/cancel.png"
                                                                OnClick="btSair_Click" ToolTip="Sair" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:ImageButton ID="btIncluirSubGrupo" runat="server" Height="18px" Width="18px"
                                                                ImageUrl="~/Image/add.png" OnClick="btIncluirSubGrupo_Click" ToolTip="Incluir" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Código" FooterStyle-Width="100px" ItemStyle-HorizontalAlign="Center"
                                                        SortExpression="CodigoItem">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labCodigoItem" runat="server" Text='<%# Bind("CODIGO_SUBGRUPO") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtCodigoItem" runat="server" Width="96px" MaxLength="2" CssClass="alinharCentro" OnTextChanged="txtSubGrupo_TextChanged"
                                                                AutoPostBack="true"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtCodigoFooter" runat="server" Width="96px" MaxLength="2" CssClass="alinharCentro"></asp:TextBox>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SubGrupo" FooterStyle-Width="300px" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left" SortExpression="SubGrupoItem">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labSubGrupoItem" runat="server" Text='<%# Bind("SUBGRUPO") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtSubGrupoItem" runat="server" Width="296px" MaxLength="25" Enabled="false"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtSubGrupoFooter" runat="server" Width="296px" MaxLength="25"></asp:TextBox>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Unidade de Medida" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labUnidadeMedidaItem" runat="server" Text='<%# Bind("UNIDADE1.DESC_UNIDADE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="ddlUnidadeMedidaItem" runat="server" Width="296px" DataValueField="UNIDADE1"
                                                                DataTextField="DESC_UNIDADE" OnSelectedIndexChanged="ddlSubGrupo_SelectedIndexChanged"
                                                                AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlUnidadeMedidaFooter" runat="server" Width="296px" DataValueField="UNIDADE1"
                                                                DataTextField="DESC_UNIDADE">
                                                            </asp:DropDownList>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                        ItemStyle-Width="25px">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btExcluir" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/delete.png"
                                                                OnClick="btExcluir_Click" OnClientClick="return ConfirmarExclusao();" ToolTip="Excluir" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:HiddenField ID="hidSubGrupo" runat="server" />
                                    <asp:HiddenField ID="hidCodigoSubGrupo" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:updatepanel>
</asp:Content>
