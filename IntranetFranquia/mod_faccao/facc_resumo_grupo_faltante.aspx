<%@ Page Title="Facção por Grupo" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="facc_resumo_grupo_faltante.aspx.cs" Inherits="Relatorios.facc_resumo_grupo_faltante"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Facção&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Facção por Grupo</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Facção por Grupo"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Grupo
                        </td>
                        <td>Coleção
                        </td>
                        <td>Empresa
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
                        <td style="width: 180px;">
                            <asp:DropDownList ID="ddlGrupo" runat="server" Width="174px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                DataValueField="GRUPO_PRODUTO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 180px;">
                            <asp:DropDownList ID="ddlColecao" runat="server" Width="174px" Height="21px"
                                DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 120px;">
                            <asp:DropDownList ID="ddlEmpresa" runat="server" Width="114px" Height="21px" Enabled="false">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="HANDBOOK" Text="HANDBOOK" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 230px;">
                            <asp:DropDownList ID="ddlServico" runat="server" Width="224px" Height="21px" DataTextField="DESCRICAO"
                                DataValueField="CODIGO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 266px;">
                            <asp:DropDownList ID="ddlFornecedor" runat="server" Width="260px" Height="21px" DataTextField="FORNECEDOR"
                                DataValueField="FORNECEDOR" OnSelectedIndexChanged="ddlFornecedor_SelectedIndexChanged" AutoPostBack="true">
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
                        <td style="text-align: right;">
                            <asp:Button ID="btImprimir" runat="server" Text="Imprimir" Width="100px" OnClick="btImprimir_Click" OnClientClick="DesabilitarBotao(this);" />
                        </td>
                    </tr>

                    <tr>
                        <td>&nbsp;</td>
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
                    <tr>
                        <td colspan="8">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvFaccaoGrupo" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                    Style="background: white" OnRowDataBound="gvFaccaoGrupo_RowDataBound" OnDataBound="gvFaccaoGrupo_DataBound"
                                    OnSorting="gvFaccaoGrupo_Sorting"
                                    ShowFooter="true" AllowSorting="true">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Grupo" HeaderStyle-Width="400px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="GRUPO">
                                            <ItemTemplate>
                                                <asp:Label ID="labGrupo" runat="server" Text='<%# Bind("GRUPO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Empresa" HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="EMPRESA">
                                            <ItemTemplate>
                                                <asp:Label ID="labEmpresa" runat="server" Text='<%# Bind("EMPRESA") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantidade Faltante" HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="QTDE_FALTANTE">
                                            <ItemTemplate>
                                                <asp:Label ID="labFaltante" runat="server" Text='<%# Bind("QTDE_FALTANTE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="" HeaderText="" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">&nbsp;
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
