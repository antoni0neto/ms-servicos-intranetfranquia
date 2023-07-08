<%@ Page Title="Liberação Estoque" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ecomv2_cad_produto_magento.aspx.cs" Inherits="Relatorios.ecomv2_cad_produto_magento" %>

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
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Estoque&nbsp;&nbsp;>&nbsp;&nbsp;Liberação Estoque</span>
        <div style="float: right; padding: 0;">
            <a href="~/mod_ecom/ecom_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="login">
        <fieldset style="padding-top: 0;">
            <legend>Liberação Estoque</legend>
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
                    <td>Tem Nota Faturada?
                    </td>
                    <td>Tem Foto Cadastrada?
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px">
                        <asp:DropDownList ID="ddlColecao" runat="server" Width="194px" Height="21px"
                            DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 200px">
                        <asp:DropDownList ID="ddlGrupo" runat="server" Width="194px" Height="21px" DataTextField="GRUPO_PRODUTO"
                            DataValueField="GRUPO_PRODUTO">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 164px">
                        <asp:TextBox ID="txtProduto" runat="server" onkeypress="return fnValidarNumero(event);"
                            Width="150px"></asp:TextBox>
                    </td>
                    <td style="width: 200px">
                        <asp:DropDownList ID="ddlGriffe" runat="server" Width="194px" Height="21px" DataTextField="GRIFFE"
                            DataValueField="GRIFFE">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 190px">
                        <asp:DropDownList ID="ddlEstoque" runat="server" Width="184px" Height="21px" Enabled="false">
                            <asp:ListItem Value="" Text=""></asp:ListItem>
                            <asp:ListItem Value="S" Text="Sim" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 190px">
                        <asp:DropDownList ID="ddlFotoCadastrada" runat="server" Width="184px" Height="21px" Enabled="false">
                            <asp:ListItem Value="" Text=""></asp:ListItem>
                            <asp:ListItem Value="S" Text="Sim" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="7">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="7">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
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
                                                    DataKeyNames="CODIGO, PRODUTO">
                                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbEnviarProduto" runat="server" Checked="false" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="45px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgProduto" runat="server" Width="122px" Height="183px" ImageAlign="AbsMiddle" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="PRODUTO" HeaderText="Produto" HeaderStyle-Width="100px" SortExpression="PRODUTO" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="DESC_PRODUTO" HeaderText="Nome" SortExpression="DESC_PRODUTO" />
                                                        <asp:BoundField DataField="GRIFFE" HeaderText="Griffe" HeaderStyle-Width="130px" SortExpression="GRIFFE" />
                                                        <asp:BoundField DataField="DESC_COR_PRODUTO" HeaderText="Cor" SortExpression="DESC_COR_PRODUTO" />

                                                        <asp:BoundField DataField="QTDE_1" HeaderText="-" SortExpression="QTDE_1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                                                        <asp:BoundField DataField="QTDE_2" HeaderText="-" SortExpression="QTDE_2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                                                        <asp:BoundField DataField="QTDE_3" HeaderText="-" SortExpression="QTDE_3" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                                                        <asp:BoundField DataField="QTDE_4" HeaderText="-" SortExpression="QTDE_4" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                                                        <asp:BoundField DataField="QTDE_5" HeaderText="-" SortExpression="QTDE_5" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                                                        <asp:BoundField DataField="QTDE_6" HeaderText="-" SortExpression="QTDE_6" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                                                        <asp:BoundField DataField="QTDE_7" HeaderText="-" SortExpression="QTDE_7" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                                                        <asp:BoundField DataField="QTDE_8" HeaderText="-" SortExpression="QTDE_8" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                                                        <asp:BoundField DataField="QTDE_9" HeaderText="-" SortExpression="QTDE_9" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />

                                                        <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" SortExpression="DESC_STATUS_CADASTRO" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litStatusEnvio" runat="server" Text='<%# Bind("DESC_ESTOQUE") %>'></asp:Literal>
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
                <tr>
                    <td colspan="7" style="text-align: right;">
                        <asp:Label ID="labErroMagento" runat="server" Text="" ForeColor="Red"></asp:Label>
                        <asp:Button ID="btEnviarMagento" runat="server" Text="Enviar Magento" Width="130px" OnClick="btEnviarMagento_Click" />
                    </td>
                </tr>
            </table>
        </fieldset>
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
</asp:Content>
