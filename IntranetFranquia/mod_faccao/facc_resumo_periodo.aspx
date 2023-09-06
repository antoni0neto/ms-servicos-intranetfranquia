<%@ Page Title="Facção por Período" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="facc_resumo_periodo.aspx.cs" Inherits="Relatorios.facc_resumo_periodo"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Facção&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Facção por Período</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Facção por Período"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Data Inicial</td>
                        <td>Data Final</td>
                        <td colspan="6">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 130px;">
                            <asp:TextBox ID="txtPeriodoInicial" runat="server" autocomplete="off" Width="170px" MaxLength="10" Style="text-align: right;"
                                onkeypress="return fnValidarData(event);"></asp:TextBox>
                        </td>
                        <td style="width: 130px;">
                            <asp:TextBox ID="txtPeriodoFinal" runat="server" autocomplete="off" Width="170px" MaxLength="10" Style="text-align: right;"
                                onkeypress="return fnValidarData(event);"></asp:TextBox>
                        </td>
                        <td colspan="6">&nbsp;</td>
                    </tr>
                    <tr>
                        <td>Coleção Intra
                        </td>
                        <td>Griffe
                        </td>
                        <td>Grupo
                        </td>
                        <td>Serviço
                        </td>
                        <td>Fornecedor
                        </td>
                        <td>Mostruário
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;</td>
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
                            <asp:DropDownList ID="ddlServico" runat="server" Width="194px" Height="21px" DataTextField="DESCRICAO"
                                DataValueField="CODIGO">
                            </asp:DropDownList>
                        </td>
                        <td valign="top" style="width: 266px;">
                            <asp:DropDownList ID="ddlFornecedor" runat="server" Width="260px" Height="21px" DataTextField="FORNECEDOR"
                                DataValueField="FORNECEDOR" OnSelectedIndexChanged="ddlFornecedor_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td valign="top" style="width: 120px;">
                            <asp:DropDownList ID="ddlMostruario" runat="server" Width="114px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                <asp:ListItem Value="N" Text="Não" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td valign="top">
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />
                        </td>
                        <td valign="top" style="text-align: right;">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="labFornecedorSub" runat="server" Text="Sub Fornecedor" Visible="false"></asp:Label>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlFornecedorSub" runat="server" Width="260px" Height="21px" DataTextField="FORNECEDOR_SUB"
                                DataValueField="FORNECEDOR_SUB" Visible="false">
                            </asp:DropDownList>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top" style="width: 49%;">&nbsp;<span style="font-weight: bold; color:red;">ENVIADO</span>
                            <div class="rounded_corners">
                                <asp:GridView ID="gvFaccaoEnviadoPeriodo" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                    Style="background: white" OnRowDataBound="gvFaccaoPeriodo_RowDataBound" OnDataBound="gvFaccaoPeriodo_DataBound"
                                    ShowFooter="true">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Griffe" HeaderStyle-Width="180px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left"
                                            SortExpression="GRIFFE">
                                            <ItemTemplate>
                                                <asp:Label ID="labGriffe" runat="server" Text='<%# Bind("GRIFFE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Grupo Produto" HeaderStyle-Width="180px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="GRUPO_PRODUTO">
                                            <ItemTemplate>
                                                <asp:Label ID="labGrupoProduto" runat="server" Text='<%# Bind("GRUPO_PRODUTO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantidade" HeaderStyle-Width="" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"
                                            SortExpression="QTDE">
                                            <ItemTemplate>
                                                <asp:Label ID="labFaltante" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                        <td valign="top" style="width: 2%;">&nbsp;
                        </td>
                        <td valign="top" style="width: 49%;">&nbsp;<span style="font-weight: bold; color:red; ">RECEBIDO</span>
                            <div class="rounded_corners">
                                <asp:GridView ID="gvFaccaoRecebPeriodo" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                    Style="background: white" OnRowDataBound="gvFaccaoPeriodo_RowDataBound" OnDataBound="gvFaccaoPeriodo_DataBound"
                                    ShowFooter="true">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Griffe" HeaderStyle-Width="180px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left"
                                            SortExpression="GRIFFE">
                                            <ItemTemplate>
                                                <asp:Label ID="labGriffe" runat="server" Text='<%# Bind("GRIFFE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Grupo Produto" HeaderStyle-Width="180px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="GRUPO_PRODUTO">
                                            <ItemTemplate>
                                                <asp:Label ID="labGrupoProduto" runat="server" Text='<%# Bind("GRUPO_PRODUTO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantidade" HeaderStyle-Width="" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"
                                            SortExpression="QTDE">
                                            <ItemTemplate>
                                                <asp:Label ID="labFaltante" runat="server"></asp:Label>
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
