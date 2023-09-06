<%@ Page Title="Cadastro de Produtos E-Commerce" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ecomv2_cad_produto.aspx.cs" Inherits="Relatorios.ecomv2_cad_produto" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
        }

        .btn-cursor {
            cursor: pointer;
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Produtos E-Commerce</span>
                <div style="float: right; padding: 0;">
                    <a href="~/mod_ecom/ecom_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Cadastro de Produtos E-Commerce</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>Coleção Linx
                            </td>
                            <td>Grupo Produto
                            </td>
                            <td>Produto
                            </td>
                            <td>Griffe
                            </td>
                            <td>Status Produto
                            </td>
                            <td>Tem Nota Faturada?</td>
                            <td>Diretório Foto</td>
                            <td>Foto Cadastrada?</td>
                        </tr>
                        <tr>
                            <td style="width: 180px">
                                <asp:DropDownList ID="ddlColecao" runat="server" Width="174px" Height="21px"
                                    DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 200px">
                                <asp:DropDownList ID="ddlGrupo" runat="server" Width="194px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                    DataValueField="GRUPO_PRODUTO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 140px">
                                <asp:TextBox ID="txtProduto" runat="server" onkeypress="return fnValidarNumero(event);" Width="126px"></asp:TextBox>
                            </td>
                            <td style="width: 170px">
                                <asp:DropDownList ID="ddlGriffe" runat="server" Width="164px" Height="21px" DataTextField="GRIFFE"
                                    DataValueField="GRIFFE">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 190px">
                                <asp:DropDownList ID="ddlStatusProduto" runat="server" Width="184px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="B" Text="Cadastrado no Magento"></asp:ListItem>
                                    <asp:ListItem Value="A" Text="Enviar Produto ao Magento"></asp:ListItem>
                                    <asp:ListItem Value="X" Text="Não Cadastrado"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 190px">
                                <asp:DropDownList ID="ddlNota" runat="server" Width="184px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 180px">
                                <asp:DropDownList ID="ddlFoto" runat="server" Width="174px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="0" Text="Sem Foto"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="FotosHandbookOnline"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="FotosHandbookTratamento"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="FotosHandbookOnlineGeral"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFotoCadastrada" runat="server" Width="164px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Coleção Ecom</td>
                            <td>Estoque</td>
                            <td>Preço</td>
                            <td>Signed</td>
                            <td>Por Qtde</td>
                            <td>MKTPlace</td>
                            <td colspan="2">Grupo Magento</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlColecaoEcom" runat="server" Width="174px" Height="21px"
                                    DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlEstoque" runat="server" Width="194px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPrecoDiff" runat="server" Width="134px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="D" Text="Preço Diferente TL"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSignedNome" runat="server" Width="164px" Height="21px" DataTextField="SIGNED_NOME"
                                    DataValueField="SIGNED_NOME">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFiltroQtde" runat="server" Width="184px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Somente Atacado"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Somente Varejo"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Tudo Atacado"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="Tudo Varejo"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="Atacado E Varejo"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td valign="top">
                                <asp:DropDownList ID="ddlB2W" runat="server" Width="184px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td colspan="2" valign="top">
                                <asp:DropDownList ID="ddlCategoriaMag" runat="server" Width="344px" Height="21px" DataTextField="GRUPO"
                                    DataValueField="CODIGO">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
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
                                                            DataKeyNames="COLECAO, PRODUTO, COR">
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
                                                                        <asp:Image ID="imgProduto" runat="server" Width="25px" Height="35px" ImageAlign="AbsMiddle" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="PRODUTO" HeaderText="Produto" HeaderStyle-Width="85px" SortExpression="PRODUTO" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                                <asp:BoundField DataField="DESC_PRODUTO" HeaderText="Nome" SortExpression="DESC_PRODUTO" />
                                                                <asp:BoundField DataField="GRUPO_PRODUTO" HeaderText="Grupo Produto" HeaderStyle-Width="140px" SortExpression="GRUPO_PRODUTO" />
                                                                <asp:BoundField DataField="GRIFFE" HeaderText="Griffe" HeaderStyle-Width="150px" SortExpression="GRIFFE" />

                                                                <asp:TemplateField HeaderText="Cor" ItemStyle-HorizontalAlign="Left" SortExpression="COR" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litCor" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Status Cadastro" ItemStyle-HorizontalAlign="Left" SortExpression="DESC_STATUS_CADASTRO" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litStatusEnvio" runat="server" Text='<%# Bind("DESC_STATUS_CADASTRO") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Diretório" ItemStyle-HorizontalAlign="Left" SortExpression="DIRETORIO" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litDirFoto" runat="server" Text='<%# Bind("DIRETORIO") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Data Alteração" ItemStyle-HorizontalAlign="Center" SortExpression="DATA_ALTERACAO" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litDataAlteracao" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Preço" HeaderStyle-Width="65px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btAtualizarPRECO" runat="server" ImageUrl="~/Image/upd_price.jpg" CssClass="btn-cursor" Width="15px" AlternateText="Preço" OnClick="btAtualizarPRECO_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Cat" HeaderStyle-Width="65px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btAtualizarCAT" runat="server" ImageUrl="~/Image/update.png" CssClass="btn-cursor" Width="15px" AlternateText="CAT" OnClick="btAtualizarCAT_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="65px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Button ID="btAbrir" runat="server" Text="Abrir" Width="65px" OnClick="btAbrir_Click" />
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
