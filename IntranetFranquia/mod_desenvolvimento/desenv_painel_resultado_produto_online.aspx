<%@ Page Title="Painel Resultado de Produto Online" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="desenv_painel_resultado_produto_online.aspx.cs" Inherits="Relatorios.desenv_painel_resultado_produto_online"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Painel Resultado de Produto Online</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Painel Resultado de Produto Online"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Data Inicial</td>
                        <td>Data Final</td>
                        <td colspan="6">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 260px;">
                            <asp:TextBox ID="txtDataInicial" runat="server" autocomplete="off" Width="246px" MaxLength="10" Style="text-align: right;" onkeypress="return fnValidarData(event);"></asp:TextBox>
                        </td>
                        <td style="width: 260px;">
                            <asp:TextBox ID="txtDataFinal" runat="server" autocomplete="off" Width="250px" MaxLength="10" Style="text-align: right;" onkeypress="return fnValidarData(event);"></asp:TextBox>
                        </td>
                        <td colspan="6">&nbsp;</td>
                    </tr>
                    <tr>
                        <td>Coleção</td>
                        <td>Origem</td>
                        <td>Grupo Produto</td>
                        <td>Griffe</td>
                        <td>Produto</td>
                        <td>Com Venda?</td>
                        <td>Com Estoque?</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 260px;">
                            <asp:ListBox ID="lstColecao" runat="server" DataValueField="COLECAO" DataTextField="DESC_COLECAO" SelectionMode="Multiple" Width="254px" Height="110px"></asp:ListBox>
                        </td>
                        <td valign="top" style="width: 260px;">
                            <asp:DropDownList ID="ddlColecaoOrigem" runat="server" Width="254px" Height="21px" DataTextField="DESC_COLECAO"
                                DataValueField="COLECAO">
                            </asp:DropDownList>
                        </td>
                        <td valign="top" style="width: 170px;">
                            <asp:DropDownList ID="ddlGrupo" runat="server" Width="164px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                DataValueField="GRUPO_PRODUTO">
                            </asp:DropDownList>
                        </td>
                        <td valign="top" style="width: 250px;">
                            <asp:ListBox ID="lstGriffe" runat="server" DataValueField="GRIFFE" Width="244px" DataTextField="GRIFFE" SelectionMode="Multiple" Height="110px"></asp:ListBox>
                        </td>
                        <td valign="top" style="width: 150px;">
                            <asp:TextBox ID="txtProduto" runat="server" Width="140px" MaxLength="10" OnTextChanged="txtProduto_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </td>
                        <td valign="top" style="width: 160px;">
                            <asp:DropDownList ID="ddlComVenda" runat="server" Width="154px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="S" Text="Sim" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td valign="top" style="width: 160px;">
                            <asp:DropDownList ID="ddlComEstoque" runat="server" Width="154px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>Fabricação
                        </td>
                        <td>Tipo Produto
                        </td>
                        <td>Tecido
                        </td>
                        <td>Cor Fornecedor
                        </td>
                        <td>Cor Linx
                        </td>
                        <td>Signed
                        </td>
                        <td colspan="2">Virado
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlFabricacao" runat="server" Width="254px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="P" Text="PRÓPRIA"></asp:ListItem>
                                <asp:ListItem Value="T" Text="TERCEIROS"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTipo" runat="server" Width="254px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="01" Text="PEÇAS"></asp:ListItem>
                                <asp:ListItem Value="02" Text="ACESSÓRIOS"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTecido" runat="server" Width="164px" Height="21px" DataTextField="TECIDO_POCKET"
                                DataValueField="TECIDO_POCKET">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCorFornecedor" runat="server" Width="244px" Height="21px"
                                DataTextField="FORNECEDOR_COR" DataValueField="FORNECEDOR_COR">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCor" runat="server" Width="144px" Height="21px" DataTextField="DESC_COR"
                                DataValueField="COR">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSigned" runat="server" Width="154px" DataValueField="SIGNED_NOME" DataTextField="SIGNED_NOME" Height="21px"></asp:DropDownList>
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="ddlVirado" runat="server" Width="154px" Height="21px" ToolTip="Virado = Coleção <> Origem">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                            </asp:DropDownList>
                        </td>

                    </tr>
                    <tr>
                        <td colspan="8">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />
                            &nbsp;&nbsp;<asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">

                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="6">Ordenação
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100px;">
                                        <asp:DropDownList ID="ddlOrdem1" runat="server" Width="150px">
                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                            <asp:ListItem Value="QTDE_SEMANA ASC" Text="SEMANA ASC"></asp:ListItem>
                                            <asp:ListItem Value="QTDE_SEMANA DESC" Text="SEMANA DESC"></asp:ListItem>
                                            <asp:ListItem Value="PORC_VENDA_TO ASC" Text="% TO ASC"></asp:ListItem>
                                            <asp:ListItem Value="PORC_VENDA_TO DESC" Text="% TO DESC"></asp:ListItem>
                                            <asp:ListItem Value="PORC_VENDA_MARKDOWN ASC" Text="% Mdo ASC"></asp:ListItem>
                                            <asp:ListItem Value="PORC_VENDA_MARKDOWN DESC" Text="% Mdo DESC"></asp:ListItem>
                                            <asp:ListItem Value="QTDE_ESTOQUE ASC" Text="Q. Est ASC"></asp:ListItem>
                                            <asp:ListItem Value="QTDE_ESTOQUE DESC" Text="Q. Est DESC"></asp:ListItem>
                                            <asp:ListItem Value="LUCRO_FINAL ASC" Text="Luc Final ASC"></asp:ListItem>
                                            <asp:ListItem Value="LUCRO_FINAL DESC" Text="Luc Final DESC"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 100px;">
                                        <asp:DropDownList ID="ddlOrdem2" runat="server" Width="150px">
                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                            <asp:ListItem Value="QTDE_SEMANA ASC" Text="SEMANA ASC"></asp:ListItem>
                                            <asp:ListItem Value="QTDE_SEMANA DESC" Text="SEMANA DESC"></asp:ListItem>
                                            <asp:ListItem Value="PORC_VENDA_TO ASC" Text="% TO ASC"></asp:ListItem>
                                            <asp:ListItem Value="PORC_VENDA_TO DESC" Text="% TO DESC"></asp:ListItem>
                                            <asp:ListItem Value="PORC_VENDA_MARKDOWN ASC" Text="% Mdo ASC"></asp:ListItem>
                                            <asp:ListItem Value="PORC_VENDA_MARKDOWN DESC" Text="% Mdo DESC"></asp:ListItem>
                                            <asp:ListItem Value="QTDE_ESTOQUE ASC" Text="Q. Est ASC"></asp:ListItem>
                                            <asp:ListItem Value="QTDE_ESTOQUE DESC" Text="Q. Est DESC"></asp:ListItem>
                                            <asp:ListItem Value="LUCRO_FINAL ASC" Text="Luc Final ASC"></asp:ListItem>
                                            <asp:ListItem Value="LUCRO_FINAL DESC" Text="Luc Final DESC"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 100px;">
                                        <asp:DropDownList ID="ddlOrdem3" runat="server" Width="150px">
                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                            <asp:ListItem Value="QTDE_SEMANA ASC" Text="SEMANA ASC"></asp:ListItem>
                                            <asp:ListItem Value="QTDE_SEMANA DESC" Text="SEMANA DESC"></asp:ListItem>
                                            <asp:ListItem Value="PORC_VENDA_TO ASC" Text="% TO ASC"></asp:ListItem>
                                            <asp:ListItem Value="PORC_VENDA_TO DESC" Text="% TO DESC"></asp:ListItem>
                                            <asp:ListItem Value="PORC_VENDA_MARKDOWN ASC" Text="% Mdo ASC"></asp:ListItem>
                                            <asp:ListItem Value="PORC_VENDA_MARKDOWN DESC" Text="% Mdo DESC"></asp:ListItem>
                                            <asp:ListItem Value="QTDE_ESTOQUE ASC" Text="Q. Est ASC"></asp:ListItem>
                                            <asp:ListItem Value="QTDE_ESTOQUE DESC" Text="Q. Est DESC"></asp:ListItem>
                                            <asp:ListItem Value="LUCRO_FINAL ASC" Text="Luc Final ASC"></asp:ListItem>
                                            <asp:ListItem Value="LUCRO_FINAL DESC" Text="Luc Final DESC"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 100px;">
                                        <asp:DropDownList ID="ddlOrdem4" runat="server" Width="150px">
                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                            <asp:ListItem Value="QTDE_SEMANA ASC" Text="SEMANA ASC"></asp:ListItem>
                                            <asp:ListItem Value="QTDE_SEMANA DESC" Text="SEMANA DESC"></asp:ListItem>
                                            <asp:ListItem Value="PORC_VENDA_TO ASC" Text="% TO ASC"></asp:ListItem>
                                            <asp:ListItem Value="PORC_VENDA_TO DESC" Text="% TO DESC"></asp:ListItem>
                                            <asp:ListItem Value="PORC_VENDA_MARKDOWN ASC" Text="% Mdo ASC"></asp:ListItem>
                                            <asp:ListItem Value="PORC_VENDA_MARKDOWN DESC" Text="% Mdo DESC"></asp:ListItem>
                                            <asp:ListItem Value="QTDE_ESTOQUE ASC" Text="Q. Est ASC"></asp:ListItem>
                                            <asp:ListItem Value="QTDE_ESTOQUE DESC" Text="Q. Est DESC"></asp:ListItem>
                                            <asp:ListItem Value="LUCRO_FINAL ASC" Text="Luc Final ASC"></asp:ListItem>
                                            <asp:ListItem Value="LUCRO_FINAL DESC" Text="Luc Final DESC"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 100px;">&nbsp;
                                    </td>
                                    <td style="text-align: right;">
                                        <div style="text-align: right; color: Red;">&nbsp;</div>
                                    </td>
                                </tr>
                            </table>
                            <div class="rounded_corners">
                                <asp:GridView ID="gvResultadoProduto" runat="server" Style="background: white; width: 3500px; border-collapse: collapse;" AutoGenerateColumns="False" ForeColor="#333333"
                                    OnRowDataBound="gvResultadoProduto_RowDataBound" OnDataBound="gvResultadoProduto_DataBound"
                                    OnSorting="gvResultadoProduto_Sorting"
                                    ShowFooter="true" AllowSorting="true">
                                    <HeaderStyle BackColor="GradientActiveCaption" />
                                    <FooterStyle BackColor="GradientActiveCaption" Font-Bold="true" HorizontalAlign="Center" Font-Size="Smaller" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgProduto1" runat="server" Width="90%" ImageAlign="AbsMiddle" OnClick="imgProduto_Click" ToolTip="" AlternateText="" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Coleção" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="DESC_COLECAO" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="">
                                            <ItemTemplate>
                                                <asp:Label ID="labDescColecao" runat="server" Text='<%# Bind("DESC_COLECAO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Semana" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="QTDE_SEMANA" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labQtdeSemana" runat="server" Text='<%# Bind("QTDE_SEMANA") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Produto" HeaderStyle-Width="85px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="PRODUTO" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labProduto" runat="server" Text='<%# Bind("PRODUTO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nome" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="130px"
                                            SortExpression="DESC_PRODUTO" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labNome" runat="server" Text='<%# Bind("DESC_PRODUTO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Cor" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="DESC_COR" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labCor" runat="server" Text='<%# Bind("DESC_COR") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Custo TS" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px"
                                            SortExpression="CUSTO_TS" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labCustoTS" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Preço TO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px"
                                            SortExpression="PRECO_TO" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labPrecoTO" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MKP Inicial" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" HeaderStyle-Width="100px"
                                            SortExpression="MKP_INICIAL" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labMKPIni" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Griffe" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px"
                                            SortExpression="GRIFFE" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labGriffe" runat="server" Text='<%# Bind("GRIFFE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Fabricante" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px"
                                            SortExpression="FABRICANTE" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labFabricante" runat="server" Text='<%# Bind("FABRICANTE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Qtde Total" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px"
                                            SortExpression="QTDE_TOTAL" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labQtdeTotal" runat="server" Text='<%# Bind("QTDE_TOTAL") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Qtde Venda TO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px"
                                            SortExpression="QTDE_VENDA_TO" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labQtdeTO" runat="server" Text='<%# Bind("QTDE_VENDA_TO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="% Venda TO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px"
                                            SortExpression="PORC_VENDA_TO" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labPorcTO" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Qtde Venda Mdo" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px"
                                            SortExpression="QTDE_VENDA_MARKDOWN" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labQtdeMarkdown" runat="server" Text='<%# Bind("QTDE_VENDA_MARKDOWN") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="% Venda Mdo" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px"
                                            SortExpression="PORC_VENDA_MARKDOWN" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labPorcMarkdown" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Q. Venda VM" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px"
                                            SortExpression="QTDE_VALEMERCADORIA" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labQtdeVM" runat="server" Text='<%# Bind("QTDE_VALEMERCADORIA") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="% Venda VM" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px"
                                            SortExpression="PORC_VALEMERCADORIA" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labPorcVM" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Lucro (Venda-Custo)" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" HeaderStyle-Width="150px"
                                            SortExpression="LUCRO" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labLucro" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="MKP Parcial" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" HeaderStyle-Width="90px"
                                            SortExpression="MKP_PARCIAL" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labMKPParc" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Preço TL" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px"
                                            SortExpression="PRECO_TL" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labPrecoTL" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Qtde Estoque" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px"
                                            SortExpression="QTDE_ESTOQUE" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labQtdeEstoque" runat="server" Text='<%# Bind("QTDE_ESTOQUE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="% Estoque" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px"
                                            SortExpression="PORC_ESTOQUE" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labPorcEstoque" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Valor Estoque" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" HeaderStyle-Width="150px"
                                            SortExpression="VALOR_ESTOQUE" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labValEstoque" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Custo Total" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" HeaderStyle-Width="150px"
                                            SortExpression="CUSTO_TOTAL" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labCustoTotal" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgProduto2" runat="server" Width="90%" ImageAlign="AbsMiddle" OnClick="imgProduto_Click" ToolTip="" AlternateText="" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Qtde Produto Vendido" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" HeaderStyle-Width="150px"
                                            SortExpression="QTDE_PRODUTO_VENDIDO" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labQtdeProdutoVendido" runat="server" Text='<%# Bind("QTDE_PRODUTO_VENDIDO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Valor Venda" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" HeaderStyle-Width="150px"
                                            SortExpression="VALOR_VENDA" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labVendaAtual" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="MKP Final" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" HeaderStyle-Width="80px"
                                            SortExpression="MKP_FINAL" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labMKPFinal" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Lucro Final" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" HeaderStyle-Width=""
                                            SortExpression="LUCRO_FINAL" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labLucroFinal" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
