<%@ Page Title="Venda de Calçados" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="desenv_acessorio_venda_calc.aspx.cs" Inherits="Relatorios.desenv_acessorio_venda_calc"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Planejamento de Coleção de Acessórios&nbsp;&nbsp;>&nbsp;&nbsp;Venda de Calçados</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Venda de Calçados"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Coleção</td>
                        <td>Data Inicial</td>
                        <td>Data Final</td>
                        <td>Grupo</td>
                        <td>Produto</td>
                        <td>Fornecedor</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 200px;">
                            <asp:DropDownList ID="ddlColecoes" runat="server" Width="194px" Height="21px" DataTextField="DESC_COLECAO"
                                DataValueField="COLECAO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 130px;">
                            <asp:TextBox ID="txtDataInicial" runat="server" Width="120px" MaxLength="10" Style="text-align: right;"
                                onkeypress="return fnValidarData(event);"></asp:TextBox>
                        </td>
                        <td style="width: 130px;">
                            <asp:TextBox ID="txtDataFinal" runat="server" Width="120px" MaxLength="10" Style="text-align: right;"
                                onkeypress="return fnValidarData(event);"></asp:TextBox>
                        </td>
                        <td style="width: 180px;">
                            <asp:DropDownList ID="ddlGrupo" runat="server" Width="174px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                DataValueField="GRUPO_PRODUTO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 140px;">
                            <asp:TextBox ID="txtProduto" runat="server" Width="130px" MaxLength="5" CssClass="alinharDireita"
                                onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                        </td>
                        <td style="width: 360px;">
                            <asp:DropDownList ID="ddlFornecedor" runat="server" Width="354px" Height="21px" DataTextField="FORNECEDOR"
                                DataValueField="FORNECEDOR">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" OnClientClick="DesabilitarBotao(this);" />
                            &nbsp;&nbsp;<asp:Label ID="labMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" style="line-height: 8px;">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvVendaCalcado" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                    Style="background: white" OnRowDataBound="gvVendaCalcado_RowDataBound" OnDataBound="gvVendaCalcado_DataBound"
                                    OnSorting="gvVendaCalcado_Sorting"
                                    ShowFooter="true" AllowSorting="true">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Left" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../Image/plus.png"
                                                    Width="15px" runat="server" />
                                                <asp:Panel ID="pnlFoto" runat="server" Style="display: none" Width="100%">
                                                    <asp:GridView ID="gvHistMensal" runat="server" Width="100%" AutoGenerateColumns="False" HeaderStyle-CssClass="GVFixedHeader"
                                                        ForeColor="#333333" OnRowDataBound="gvHistMensal_RowDataBound" ShowFooter="true">
                                                        <HeaderStyle BackColor="LightSteelBlue" />
                                                        <FooterStyle BackColor="Gainsboro" Font-Bold="true" />
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Foto" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-BorderColor="Gainsboro" ItemStyle-BorderWidth="1px">
                                            <ItemTemplate>
                                                <asp:Image ID="imgFotoPeca" runat="server" ImageAlign="Middle" Width="100px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Produto" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="PRODUTO">
                                            <ItemTemplate>
                                                <asp:Label ID="labProduto" runat="server" Text='<%# Bind("PRODUTO") %>' Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nome" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="DESC_PRODUTO">
                                            <ItemTemplate>
                                                <asp:Label ID="labNome" runat="server" Text='<%# Bind("DESC_PRODUTO") %>' Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="DESC_COR">
                                            <ItemTemplate>
                                                <asp:Label ID="labCor" runat="server" Text='<%# Bind("DESC_COR") %>' Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fornecedor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="FABRICANTE">
                                            <ItemTemplate>
                                                <asp:Label ID="labFabricante" runat="server" Text='<%# Bind("FABRICANTE") %>' Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Estoque" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="ESTOQUE">
                                            <ItemTemplate>
                                                <asp:Label ID="labEstoque" runat="server" Text='<%# Bind("ESTOQUE") %>' Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qtde Venda" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="QTDE_VENDIDA_PERIODO">
                                            <ItemTemplate>
                                                <asp:Label ID="labQtdeVenda" runat="server" Text='<%# Bind("QTDE_VENDIDA_PERIODO") %>' Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Valor Venda" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="VALOR_VENDA_PERIODO">
                                            <ItemTemplate>
                                                <asp:Label ID="labValorVenda" runat="server" Text='' Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Custo Venda" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="CUSTO_VENDA">
                                            <ItemTemplate>
                                                <asp:Label ID="labCustoVenda" runat="server" Text='' Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Lucro Venda" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="LUCRO_VENDA">
                                            <ItemTemplate>
                                                <asp:Label ID="labLucroVenda" runat="server" Text='' Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Valorização" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="VALORIZACAO">
                                            <ItemTemplate>
                                                <asp:Label ID="labValorizacao" runat="server" Text='' Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Custo Estoque" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="CUSTO_ESTOQUE">
                                            <ItemTemplate>
                                                <asp:Label ID="labCustoEstoque" runat="server" Text='' Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Lucro Estoque" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="LUCRO_ESTOQUE">
                                            <ItemTemplate>
                                                <asp:Label ID="labLucroEstoque" runat="server" Text='' Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Lucro Total" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="LUCRO_TOTAL">
                                            <ItemTemplate>
                                                <asp:Label ID="labLucroTotal" runat="server" Text='' Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">&nbsp;
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
