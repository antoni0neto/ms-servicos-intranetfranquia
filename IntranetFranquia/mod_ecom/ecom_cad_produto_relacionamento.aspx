<%@ Page Title="Relacionados - Criar Produto Relacionado" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ecom_cad_produto_relacionamento.aspx.cs" Inherits="Relatorios.ecom_cad_produto_relacionamento" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
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
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>Coleção
                            </td>
                            <td>Grupo Produto
                            </td>
                            <td>Produto
                            </td>
                            <td>Griffe
                            </td>
                            <td>Cor
                            </td>
                            <td>Cor Fornecedor
                            </td>
                            <td>Grupo Macro
                            </td>
                            <td>Relacionado</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 200px">
                                <asp:DropDownList ID="ddlColecao" runat="server" Width="194px" Height="21px"
                                    DataTextField="DESC_COLECAO" DataValueField="COLECAO" OnSelectedIndexChanged="ddlColecao_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 180px">
                                <asp:DropDownList ID="ddlGrupo" runat="server" Width="174px" Height="21px" DataValueField="GRUPO_PRODUTO" DataTextField="GRUPO_PRODUTO"></asp:DropDownList>
                            </td>
                            <td style="width: 160px">
                                <asp:TextBox ID="txtProduto" runat="server" onkeypress="return fnValidarNumero(event);"
                                    Width="150px"></asp:TextBox>
                            </td>
                            <td style="width: 200px">
                                <asp:DropDownList ID="ddlGriffe" runat="server" Width="194px" Height="21px" DataTextField="GRIFFE"
                                    DataValueField="GRIFFE">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 180px">
                                <asp:DropDownList ID="ddlCorMagento" runat="server" Width="174px" Height="21px" DataValueField="CODIGO" DataTextField="COR"></asp:DropDownList>
                            </td>
                            <td style="width: 210px">
                                <asp:DropDownList ID="ddlCorFornecedor" runat="server" Width="204px" Height="21px"
                                    DataTextField="FORNECEDOR_COR" DataValueField="FORNECEDOR_COR">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 150px">
                                <asp:DropDownList ID="ddlGrupoMacro" runat="server" Width="144px" Height="21px" DataValueField="CODIGO" DataTextField="GRUPO_MACRO"></asp:DropDownList>
                            </td>
                            <td style="width: 150px">
                                <asp:DropDownList ID="ddlRelacionado" runat="server" Width="144px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>Tecido
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlTecido" runat="server" Width="194px" Height="21px" DataTextField="TECIDO_POCKET"
                                    DataValueField="TECIDO_POCKET">
                                </asp:DropDownList>
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="9">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="9">
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="9">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="9">
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
                                                            OnSorting="gvProduto_Sorting" AllowSorting="true" ShowFooter="true"
                                                            DataKeyNames="">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="45px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="imgProduto" runat="server" Width="122px" Height="183px" ImageAlign="AbsMiddle" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="PRODUTO" HeaderText="Produto" HeaderStyle-Width="100px" SortExpression="PRODUTO" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                                <asp:BoundField DataField="NOME" HeaderText="Nome" SortExpression="NOME" />
                                                                <asp:BoundField DataField="GRIFFE" HeaderText="Griffe" HeaderStyle-Width="130px" SortExpression="GRIFFE" />
                                                                <asp:BoundField DataField="COR" HeaderText="Cor" SortExpression="COR" />
                                                                <asp:BoundField DataField="COR_FORNCEDOR" HeaderText="Cor Fornecedor" SortExpression="COR_FORNCEDOR" />
                                                                <asp:BoundField DataField="TECIDO" HeaderText="Tecido" SortExpression="TECIDO" />
                                                                <asp:BoundField DataField="GRUPO_MACRO" HeaderText="Grupo Macro" SortExpression="GRUPO_MACRO" />

                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="45px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:Button ID="btAbrir" runat="server" Text=">>" Width="40px" OnClick="btAbrir_Click" />
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
