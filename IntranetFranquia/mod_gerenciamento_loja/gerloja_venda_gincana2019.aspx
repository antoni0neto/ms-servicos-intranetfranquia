<%@ Page Title="Gincana de Natal 2019" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="gerloja_venda_gincana2019.aspx.cs" Inherits="Relatorios.gerloja_venda_gincana2019" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
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
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="accountInfo">
        <br />
        <div>
            <span style="font-family: Calibri; font-size: 14px;">Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Vendedores&nbsp;&nbsp;>&nbsp;&nbsp;Gincana de Natal 2019</span>
            <div style="float: right; padding: 0;">
                <a href="gerloja_menu.aspx" class="alink" title="Voltar">Voltar</a>
            </div>
        </div>
        <hr />
        <fieldset>
            <legend>
                <asp:Label ID="labTitulo" runat="server" Text="Gincana de Natal 2019"></asp:Label></legend>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>Data Início
                    </td>
                    <td>Data Fim
                    </td>
                    <td>Filial
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 200px;">
                        <asp:TextBox ID="txtPeriodoInicial" runat="server" Width="190px" MaxLength="10" Style="text-align: right;" onkeypress="return fnValidarData(event);"></asp:TextBox>
                    </td>
                    <td style="width: 200px;">
                        <asp:TextBox ID="txtPeriodoFinal" runat="server" Width="190px" MaxLength="10" Style="text-align: right;" onkeypress="return fnValidarData(event);"></asp:TextBox>
                    </td>
                    <td style="width: 310px;">
                        <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="21px" Width="304px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btBuscar" Text="Buscar Vendas" OnClick="btBuscar_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <div class="rounded_corners">
                            <asp:GridView ID="gvVendas" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true"
                                OnRowDataBound="gvVendas_RowDataBound">
                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left"></FooterStyle>
                                <Columns>
                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FILIAL" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                        HeaderText="Filial" HeaderStyle-Width="" />
                                    <asp:BoundField DataField="COTA_PONTO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                        HeaderText="Cota" HeaderStyle-Width="250px" />
                                    <asp:BoundField DataField="PA_PONTO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                        HeaderText="PA" HeaderStyle-Width="250px" />
                                    <asp:BoundField DataField="MASC_PONTO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                        HeaderText="Masculino" HeaderStyle-Width="250px" />
                                    <asp:BoundField DataField="PONTOS" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderText="Pontos" HeaderStyle-Width="250px" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
