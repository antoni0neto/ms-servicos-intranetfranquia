<%@ Page Title="Documentos" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="faccadm_rel_documento.aspx.cs" Inherits="Relatorios.faccadm_rel_documento"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .jGrowl .manilla {
            color: black;
        }

        .jGrowl .redError {
            color: red;
        }

        .alinharDireita {
            text-align: right;
        }
    </style>
    <script src="http://code.jquery.com/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Facção Administrativo&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Documentos</span>
                <div style="float: right; padding: 0;">
                    <a href="faccadm_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Documentos"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Facção
                        </td>
                        <td>Tipo de Documento
                        </td>
                        <td>Vigência Inicial
                        </td>
                        <td>Vigência Final
                        </td>
                        <td>Status
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 290px;">
                            <asp:DropDownList runat="server" ID="ddlFaccao" DataValueField="NOME_CLIFOR" DataTextField="NOME_CLIFOR"
                                Height="21px" Width="284px">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 400px;">
                            <asp:DropDownList runat="server" ID="ddlTipoDocumento" DataValueField="CODIGO" DataTextField="DOCUMENTO"
                                Height="21px" Width="394px">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 200px;">
                            <asp:TextBox ID="txtDataVigenciaIni" runat="server" MaxLength="10" Width="190px"
                                onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                        </td>
                        <td style="width: 200px;">
                            <asp:TextBox ID="txtDataVigenciaFim" runat="server" MaxLength="10" Width="190px"
                                onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                        </td>
                        <td style="width: 160px;">
                            <asp:DropDownList ID="ddlStatusDocumento" runat="server" Width="154px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="E" Text="ENTREGUE"></asp:ListItem>
                                <asp:ListItem Value="F" Text="FALTANTE"></asp:ListItem>
                                <asp:ListItem Value="V" Text="VENCIDO"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
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
                                <asp:GridView ID="gvDocumento" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                    Style="background: white" OnRowDataBound="gvDocumento_RowDataBound" OnSorting="gvDocumento_Sorting" AllowSorting="true">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                    <FooterStyle BackColor="Gainsboro" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="NOME_CLIFOR" HeaderText="Facção"  ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left" SortExpression="NOME_CLIFOR" />
                                        <asp:BoundField DataField="TIPO_DOCUMENTO_DESC" HeaderText="Tipo de Documento" HeaderStyle-Width="440px" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left" SortExpression="TIPO_DOCUMENTO_DESC" />
                                        <asp:TemplateField HeaderText="Vigência Inicial" HeaderStyle-Width="160px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="DATA_VIGENCIA_INI">
                                            <ItemTemplate>
                                                <asp:Literal ID="litVigenciaIni" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vigência Final" HeaderStyle-Width="160px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="DATA_VIGENCIA_FIM">
                                            <ItemTemplate>
                                                <asp:Literal ID="litVigenciaFim" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="DOC_FALTANTE">
                                            <ItemTemplate>
                                                <asp:Literal ID="litStatusDocumento" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtImprimir" runat="server" Width="15px" ImageUrl="~/Image/print.png"
                                                    ToolTip="Imprimir" OnClick="ibtImprimir_Click" />
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
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
