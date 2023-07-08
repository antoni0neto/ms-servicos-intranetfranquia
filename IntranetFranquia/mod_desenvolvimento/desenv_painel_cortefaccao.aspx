<%@ Page Title="Painel de Entrega Corte/Facção/Loja" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="desenv_painel_cortefaccao.aspx.cs" Inherits="Relatorios.desenv_painel_cortefaccao"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Painel de Entrega Corte/Facção/Loja</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset style="background-color: white;">
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Painel de Entrega Corte/Facção/Loja"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Coleção Original</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 180px;">
                            <asp:ListBox ID="lstColecao" runat="server" DataValueField="COLECAO" DataTextField="DESC_COLECAO" SelectionMode="Multiple" Width="174px" Height="140px"></asp:ListBox>
                        </td>
                        <td valign="top">
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="120px" OnClick="btBuscar_Click" />&nbsp;
                            <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="3">&nbsp;</td>
                    </tr>
                </table>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div class="rounded_corners">
                                <asp:GridView ID="gvCorteFaccao" runat="server" AutoGenerateColumns="False" Style="background-color: white;"
                                    ForeColor="#333333" OnRowDataBound="gvCorteFaccao_RowDataBound" OnDataBound="gvCorteFaccao_DataBound" ShowFooter="true">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" Font-Bold="true" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="rounded_corners">
                                <asp:GridView ID="gvDiasUteis" runat="server" AutoGenerateColumns="False" Style="background-color: white;"
                                    ForeColor="#333333" OnRowDataBound="gvDiasUteis_RowDataBound" OnDataBound="gvDiasUteis_DataBound" ShowFooter="true">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" Font-Bold="true" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="Ano" HeaderStyle-Width="200px"
                                            SortExpression="ANO">
                                            <ItemTemplate>
                                                <asp:Literal ID="litAno" runat="server" Text='<%# Bind("ANO") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Mês" HeaderStyle-Width="200px"
                                            SortExpression="MES">
                                            <ItemTemplate>
                                                <asp:Literal ID="litMes" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Dias Úteis" HeaderStyle-Width="200px"
                                            SortExpression="UTIL">
                                            <ItemTemplate>
                                                <asp:Literal ID="litDiasUteis" runat="server" Text='<%# Bind("UTIL") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
