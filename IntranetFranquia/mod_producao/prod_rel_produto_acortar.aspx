<%@ Page Title="Produtos Liberados" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="prod_rel_produto_acortar.aspx.cs" Inherits="Relatorios.prod_rel_produto_acortar"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Ordem de Corte&nbsp;&nbsp;>&nbsp;&nbsp;Produtos Liberados</span>
                <div style="float: right; padding: 0;">
                    <a href="prod_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Produtos Liberados"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Coleção
                        </td>
                        <td>Origem
                        </td>
                        <td>Grupo
                        </td>
                        <td>Modelo
                        </td>
                        <td>Nome
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 160px;">
                            <asp:DropDownList ID="ddlColecoes" runat="server" Width="154px" Height="21px"
                                DataTextField="DESC_COLECAO" DataValueField="COLECAO" OnSelectedIndexChanged="ddlColecoes_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 180px;" valign="top">
                            <asp:DropDownList ID="ddlOrigem" runat="server" Width="174px" Height="21px" DataTextField="DESCRICAO"
                                DataValueField="CODIGO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 170px;" valign="top">
                            <asp:DropDownList ID="ddlGrupo" runat="server" Width="164px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                DataValueField="GRUPO_PRODUTO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 130px;" valign="top">
                            <asp:TextBox ID="txtModelo" runat="server" Width="120px" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width: 170px;" valign="top">
                            <asp:TextBox ID="txtNome" runat="server" Width="160px" MaxLength="20"></asp:TextBox>
                        </td>
                        <td>&nbsp;
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                    Style="background: white" OnRowDataBound="gvProduto_RowDataBound" OnDataBound="gvProduto_DataBound" ShowFooter="true" AllowSorting="true" OnSorting="gvProduto_Sorting">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                    <FooterStyle BackColor="Gainsboro" Font-Bold="true" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../Image/plus.png"
                                                    Width="15px" runat="server" />
                                                <asp:Panel ID="pnlFoto" runat="server" Style="display: none" Width="100%">
                                                    <asp:GridView ID="gvFoto" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvFoto_RowDataBound"
                                                        Width="100%" DataKeyNames="CODIGO">
                                                        <HeaderStyle BackColor="LightSteelBlue" HorizontalAlign="Center"></HeaderStyle>
                                                        <RowStyle BorderColor="Gainsboro" BorderWidth="1px" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Foto Peça" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-BorderColor="Gainsboro" ItemStyle-BorderWidth="1px">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgFotoPeca" runat="server" ImageAlign="Middle" />
                                                                    <asp:Image ID="imgFotoPeca2" runat="server" ImageAlign="Middle" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Coleção" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" SortExpression="DESC_COLECAO">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColecao" runat="server" Text='<%# Eval("DESC_COLECAO")%>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Origem" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" SortExpression="ORIGEM">
                                            <ItemTemplate>
                                                <asp:Literal ID="litOrigem" runat="server" Text='<%# Eval("ORIGEM")%>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Modelo" HeaderStyle-Width="130px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" SortExpression="MODELO">
                                            <ItemTemplate>
                                                <asp:Literal ID="litModelo" runat="server" Text='<%# Eval("MODELO")%>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Grupo" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Left" SortExpression="GRUPO">
                                            <ItemTemplate>
                                                <asp:Literal ID="litGrupo" runat="server" Text='<%# Eval("GRUPO")%>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nome" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Left" SortExpression="NOME">
                                            <ItemTemplate>
                                                <asp:Literal ID="litNome" runat="server" Text='<%# Eval("NOME")%>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cor" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Left" SortExpression="DESC_COR">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCor" runat="server" Text='<%# Eval("DESC_COR")%>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qtde Mostruário" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeMostruario" runat="server" Text='<%# Eval("QTDE_MOSTRUARIO")%>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qtde Atacado" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeAtacado" runat="server" Text='<%# Eval("QTDE_ATACADO")%>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qtde Varejo" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeVarejo" runat="server" Text='<%# Eval("QTDE_VAREJO")%>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" style="text-align: right;">
                            <asp:Button ID="btImprimir" runat="server" Width="100px" Text="Imprimir" OnClick="btImprimir_Click" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
