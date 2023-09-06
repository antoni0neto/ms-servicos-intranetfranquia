<%@ Page Title="Painel de Entrega de Produto" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="desenv_painel_entrega_loja.aspx.cs" Inherits="Relatorios.desenv_painel_entrega_loja"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Painel de Entrega de Produto</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Painel de Entrega de Produto"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Semana</td>
                        <td>Data Inicial</td>
                        <td>Data Final</td>
                        <td>Tipo Produto</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 180px;">
                            <asp:DropDownList runat="server" ID="ddlSemana" DataValueField="CODIGO" DataTextField="SEMANA" Width="174px" Height="21px" OnSelectedIndexChanged="ddlSemana_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPeriodoInicial" runat="server" autocomplete="off" Width="170px" MaxLength="10" Style="text-align: right;"
                                onkeypress="return fnValidarData(event);"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPeriodoFinal" runat="server" autocomplete="off" Width="170px" MaxLength="10" Style="text-align: right;"
                                onkeypress="return fnValidarData(event);"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTipo" runat="server" Width="194px" Height="21px" OnSelectedIndexChanged="ddlTipo_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="01" Text="PEÇAS"></asp:ListItem>
                                <asp:ListItem Value="02" Text="ACESSÓRIOS"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>&nbsp;</td>
                    </tr>

                    <tr>
                        <td>Coleção Linx
                        </td>
                        <td>Griffe
                        </td>
                        <td>Grupo Produto
                        </td>
                        <td>Fabricação
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 180px;" valign="top">
                            <asp:ListBox ID="lstColecao" runat="server" DataValueField="COLECAO" DataTextField="DESC_COLECAO" SelectionMode="Multiple" Width="174px" Height="140px"></asp:ListBox>
                        </td>
                        <td style="width: 180px;" valign="top">
                            <asp:ListBox ID="lstGriffe" runat="server" DataValueField="GRIFFE" Width="174px" DataTextField="GRIFFE" SelectionMode="Multiple" Height="140px"></asp:ListBox>
                        </td>
                        <td style="width: 180px;" valign="top">
                            <asp:ListBox ID="lstGrupoProduto" runat="server" DataValueField="GRUPO_PRODUTO" DataTextField="GRUPO_PRODUTO" SelectionMode="Multiple" Width="174px" Height="140px"></asp:ListBox>
                        </td>
                        <td style="width: 200px;" valign="top">
                            <asp:DropDownList ID="ddlFabricacao" runat="server" Width="194px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="P" Text="PRÓPRIA"></asp:ListItem>
                                <asp:ListItem Value="T" Text="TERCEIROS"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td valign="top">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvEntregaLoja" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                    Style="background: white" OnRowDataBound="gvEntregaLoja_RowDataBound" OnDataBound="gvEntregaLoja_DataBound"
                                    ShowFooter="true">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Qtde SKU Novo" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="LightCyan">
                                            <ItemTemplate>
                                                <asp:Label ID="labQtdeSkuNovo" runat="server" Text='<%# Bind("QTDE_SKU_NOVO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qtde Novo" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="LightCyan">
                                            <ItemTemplate>
                                                <asp:Label ID="labQtdeNovo" runat="server" Text=''></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Valor Novo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-BackColor="LightCyan">
                                            <ItemTemplate>
                                                <asp:Label ID="labValorNovo" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qtde SKU Reposição" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="LightYellow">
                                            <ItemTemplate>
                                                <asp:Label ID="labQtdeSkuRepo" runat="server" Text='<%# Bind("QTDE_SKU_REPO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qtde Reposição" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="LightYellow">
                                            <ItemTemplate>
                                                <asp:Label ID="labQtdeRepo" runat="server" Text=''></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Valor Reposição" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-BackColor="LightYellow">
                                            <ItemTemplate>
                                                <asp:Label ID="labValorRepo" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qtde Total" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="labQtdeTotal" runat="server" Text=''></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Valor Total" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="labValorTotal" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">&nbsp;
                        </td>
                    </tr>
                </table>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 1069px;">&nbsp;</td>
                        <td>
                            <asp:DropDownList ID="ddlFiltroStatus" runat="server" Width="362px" OnSelectedIndexChanged="ddlFiltroStatus_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="N" Text="NOVO"></asp:ListItem>
                                <asp:ListItem Value="R" Text="REPOSIÇÃO"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvEntregaLojaProduto" runat="server" Style="background: white; width: 100%; border-collapse: collapse;" AutoGenerateColumns="False" ForeColor="#333333"
                                    OnRowDataBound="gvEntregaLojaProduto_RowDataBound" OnDataBound="gvEntregaLojaProduto_DataBound"
                                    OnSorting="gvEntregaLojaProduto_Sorting"
                                    ShowFooter="true" AllowSorting="true">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Center" Font-Size="Smaller" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgProduto" runat="server" Width="90%" ImageAlign="AbsMiddle" ToolTip="" AlternateText="" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Coleção" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="DESC_COLECAO" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="">
                                            <ItemTemplate>
                                                <asp:Label ID="labDescColecao" runat="server" Text='<%# Bind("DESC_COLECAO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Produto" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="PRODUTO" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labProduto" runat="server" Text='<%# Bind("PRODUTO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nome" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="160px"
                                            SortExpression="DESC_PRODUTO" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labNome" runat="server" Text='<%# Bind("DESC_PRODUTO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cor" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="DESC_COR" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labCor" runat="server" Text='<%# Bind("DESC_COR_PRODUTO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Griffe" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="140px"
                                            SortExpression="GRIFFE" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labGriffe" runat="server" Text='<%# Bind("GRIFFE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Qtde" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                                            SortExpression="QTDE_TOTAL" ItemStyle-Font-Size="">
                                            <ItemTemplate>
                                                <asp:Label ID="labQtdeTotal" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Valor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="130px"
                                            SortExpression="VALOR_TOTAL" ItemStyle-Font-Size="">
                                            <ItemTemplate>
                                                <asp:Label ID="labValorTotal" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width=""
                                            SortExpression="STATUS" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labStatus" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
