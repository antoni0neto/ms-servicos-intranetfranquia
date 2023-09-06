<%@ Page Title="Histórico Cliente Representante" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="atac_rel_hist_representante_cli.aspx.cs" Inherits="Relatorios.atac_rel_hist_representante_cli" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Atacado&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Histórico Representante&nbsp;&nbsp;>&nbsp;&nbsp;Histórico Cliente Representante</span>
                <div style="float: right; padding: 0;">
                    <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Histórico Cliente Representante</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">

                        <tr>
                            <td>Representante</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 250px;">
                                <asp:TextBox ID="txtRepresentante" runat="server" Width="240px" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td style="width: 200px;"><asp:HiddenField ID="hidColecaoDe" runat="server" />
                            </td>
                            <td style="width: 200px;"><asp:HiddenField ID="hidColecaoAte" runat="server" />
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvHistoricoCliente" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvHistoricoCliente_RowDataBound"
                                        OnDataBound="gvHistoricoCliente_DataBound" ShowFooter="true"
                                        OnSorting="gvHistoricoCliente_Sorting" AllowSorting="true">
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Cliente" SortExpression="ITEM">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litCliente" runat="server" Text='<%# Bind("ITEM") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Coleção" SortExpression="DESC_COLECAO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDescColecao" runat="server" Text='<%# Bind("DESC_COLECAO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Qtde Original" HeaderStyle-Width="150px" SortExpression="TOT_QTDE_ORIGINAL">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtdeOriginal" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Valor Original" HeaderStyle-Width="150px" SortExpression="TOT_VALOR_ORIGINAL">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValorOriginal" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Qtde Cancelada" HeaderStyle-Width="150px" SortExpression="TOT_QTDE_CANCELADA">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtdeCanc" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Valor Cancelado" HeaderStyle-Width="150px" SortExpression="TOT_VALOR_CANCELADO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValorCanc" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Qtde Entregue" HeaderStyle-Width="150px" SortExpression="TOT_QTDE_ENTREGUE">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtdeEntregue" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Valor Entregue" HeaderStyle-Width="150px" SortExpression="TOT_VALOR_ENTREGUE">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValorEntregue" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
