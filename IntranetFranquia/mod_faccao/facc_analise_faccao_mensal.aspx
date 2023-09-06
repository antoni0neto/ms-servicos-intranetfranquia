<%@ Page Title="Desempenho de Facção" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="facc_analise_faccao_mensal.aspx.cs" Inherits="Relatorios.facc_analise_faccao_mensal"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }

        .jGrowl .manilla {
            background-color: #000;
            color: white;
        }
    </style>
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://code.jquery.com/jquery.js"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Facção&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Desempenho de Facção</span>
                <div style="float: right; padding: 0;">
                    <a href="facc_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Desempenho de Facção"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Fornecedor</td>
                        <td>Coleção</td>
                        <td>Mostruário</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 344px;">
                            <asp:DropDownList ID="ddlFornecedor" runat="server" Width="338px" Height="21px" DataTextField="FORNECEDOR"
                                DataValueField="FORNECEDOR">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 180px;">
                            <asp:DropDownList ID="ddlColecao" runat="server" Width="174px" Height="21px"
                                DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 120px;">
                            <asp:DropDownList ID="ddlMostruario" runat="server" Width="114px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <fieldset style="margin-top: -3px;">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvFaccaoAnaliseDesempenho" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                        Style="background: white" OnRowDataBound="gvFaccaoAnaliseDesempenho_RowDataBound" 
                                        OnSorting="gvFaccaoAnaliseDesempenho_Sorting" AllowSorting="true"
                                         OnDataBound="gvFaccaoAnaliseDesempenho_DataBound" ShowFooter="true">
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                        <RowStyle HorizontalAlign="Center" />
                                        <FooterStyle HorizontalAlign="Center" BackColor="Gainsboro" Font-Bold="true" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fornecedor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                SortExpression="FORNECEDOR">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litFornecedor" runat="server" Text='<%# Bind("FORNECEDOR") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qtde de Funcionário" HeaderStyle-Width="160px"
                                                SortExpression="QTDE_FUN">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtdeFun" runat="server" Text='<%# Bind("QTDE_FUN") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Peças por Funcionário" HeaderStyle-Width="180px"
                                                SortExpression="PECAS_POR_FUN">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPecasFun" runat="server" Text='<%# Bind("PECAS_POR_FUN") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Peças por Dia" HeaderStyle-Width="155px"
                                                SortExpression="PECAS_POR_DIA">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPecasDia" runat="server" Text='<%# Bind("PECAS_POR_DIA") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Média Enviado" HeaderStyle-Width="155px"
                                                SortExpression="MEDIA_ENVIO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litMediaEnvio" runat="server" Text='<%# Bind("MEDIA_ENVIO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Média Produtividade" HeaderStyle-Width="160px"
                                                SortExpression="MEDIA_PRODUTIVIDADE">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litMediaProdutividade" runat="server" Text='<%# Bind("MEDIA_PRODUTIVIDADE") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Saldo" HeaderStyle-Width="140px"
                                                SortExpression="SALDO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litSaldo" runat="server" Text='<%# Bind("SALDO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Nota" HeaderStyle-Width="140px"
                                                SortExpression="NOTA">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNota" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtPesquisar" runat="server" Width="15px" ImageUrl="~/Image/search.png"
                                                    ToolTip="Pesquisar" OnClick="ibtPesquisar_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
