<%@ Page Title="Estoque Magento" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ecom_cad_produto_estoque.aspx.cs" Inherits="Relatorios.ecom_cad_produto_estoque" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
        }
    </style>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../Image/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "../Image/plus.png");
            $(this).closest("tr").next().remove();
        });
    </script>

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Estoque&nbsp;&nbsp;>&nbsp;&nbsp;Estoque Magento</span>
                <div style="float: right; padding: 0;">
                    <a href="ecom_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Estoque Magento</legend>
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
                            <td>Tipo
                            </td>
                            <td>Tem Estoque
                            </td>
                            <td>Status
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
                            <td style="width: 160px">
                                <asp:TextBox ID="txtProduto" runat="server" onkeypress="return fnValidarNumero(event);"
                                    Width="150px"></asp:TextBox>
                            </td>
                            <td style="width: 200px">
                                <asp:DropDownList ID="ddlGriffe" runat="server" Width="194px" Height="21px" DataTextField="GRIFFE"
                                    DataValueField="GRIFFE">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 200px">
                                <asp:DropDownList ID="ddlTipo" runat="server" Width="194px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="01" Text="PEÇAS"></asp:ListItem>
                                    <asp:ListItem Value="02" Text="ACESSÓRIOS"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 200px">
                                <asp:DropDownList ID="ddlTemEstoque" runat="server" Width="194px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 206px">
                                <asp:DropDownList ID="ddlStatusProduto" runat="server" Width="200px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="Cadastrado no Magento" Text="Cadastrado no Magento"></asp:ListItem>
                                    <asp:ListItem Value="Enviar Produto ao Magento" Text="Enviar Produto ao Magento"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>&nbsp;
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
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound"
                                                            OnDataBound="gvProduto_DataBound"
                                                            OnSorting="gvProduto_Sorting" AllowSorting="true" ShowFooter="true"
                                                            DataKeyNames="PRODUTO, COR">
                                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                                                            <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center"
                                                                    ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../Image/plus.png"
                                                                            Width="15px" runat="server" />
                                                                        <asp:Panel ID="pnlEStoque" runat="server" Style="display: none" Width="100%">
                                                                            <asp:GridView ID="gvEstoque" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvEstoque_RowDataBound"
                                                                                Width="100%">
                                                                                <HeaderStyle BackColor="LightSteelBlue" HorizontalAlign="Center"></HeaderStyle>
                                                                                <RowStyle BorderColor="GradientActiveCaption" BorderWidth="1px" />
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="TAM1" HeaderText="" ItemStyle-HorizontalAlign="Center" />
                                                                                    <asp:BoundField DataField="TAM2" HeaderText="" ItemStyle-HorizontalAlign="Center" />
                                                                                    <asp:BoundField DataField="TAM3" HeaderText="" ItemStyle-HorizontalAlign="Center" />
                                                                                    <asp:BoundField DataField="TAM4" HeaderText="" ItemStyle-HorizontalAlign="Center" />
                                                                                    <asp:BoundField DataField="TAM5" HeaderText="" ItemStyle-HorizontalAlign="Center" />
                                                                                    <asp:BoundField DataField="TAM6" HeaderText="" ItemStyle-HorizontalAlign="Center" />
                                                                                    <asp:BoundField DataField="TAM7" HeaderText="" ItemStyle-HorizontalAlign="Center" />
                                                                                    <asp:BoundField DataField="TAM8" HeaderText="" ItemStyle-HorizontalAlign="Center" />
                                                                                    <asp:BoundField DataField="TAM9" HeaderText="" ItemStyle-HorizontalAlign="Center" />
                                                                                    <asp:BoundField DataField="TAM10" HeaderText="" ItemStyle-HorizontalAlign="Center" />
                                                                                    <asp:BoundField DataField="TAM11" HeaderText="" ItemStyle-HorizontalAlign="Center" />
                                                                                    <asp:BoundField DataField="TAM12" HeaderText="" ItemStyle-HorizontalAlign="Center" />
                                                                                    <asp:BoundField DataField="TAM13" HeaderText="" ItemStyle-HorizontalAlign="Center" />
                                                                                    <asp:BoundField DataField="TAM14" HeaderText="" ItemStyle-HorizontalAlign="Center" />
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </asp:Panel>
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
                                                                <asp:BoundField DataField="DESC_COR" HeaderText="Cor" SortExpression="DESC_COR" />
                                                                <asp:BoundField DataField="ULTIMO_ENVIO" HeaderText="Enviado" SortExpression="ULTIMO_ENVIO" />
                                                                <asp:BoundField DataField="QTDE_ESTOQUE" HeaderText="Estoque" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="QTDE_ESTOQUE" />
                                                                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="180px" SortExpression="DESC_STATUS" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litStatusEnvio" runat="server" Text='<%# Bind("DESC_STATUS") %>'></asp:Literal>
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
