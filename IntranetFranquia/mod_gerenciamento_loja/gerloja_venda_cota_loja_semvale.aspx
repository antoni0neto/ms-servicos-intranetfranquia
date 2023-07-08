<%@ Page Title="Venda/Cotas por Loja" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="gerloja_venda_cota_loja_semvale.aspx.cs" Inherits="Relatorios.gerloja_venda_cota_loja_semvale" %>

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
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">
            <asp:Label ID="labTitulo" runat="server" Text="Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Desempenho&nbsp;&nbsp;>&nbsp;&nbsp;Venda/Cotas por Loja (SEM VALE MERCADORIA)"></asp:Label></span>
        <div style="float: right; padding: 0;">
            <a href="gerloja_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div>
        <fieldset>
            <legend>Venda/Cotas por Loja (SEM VALE MERCADORIA)</legend>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>Data Início
                    </td>
                    <td>Data Fim
                    </td>
                    <td>Filial
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;">
                        <asp:TextBox ID="txtPeriodoInicial" runat="server" autocomplete="off" Width="190px" MaxLength="10" Style="text-align: right;" onkeypress="return fnValidarData(event);"></asp:TextBox>
                    </td>
                    <td style="width: 200px;">
                        <asp:TextBox ID="txtPeriodoFinal" runat="server" autocomplete="off" Width="190px" MaxLength="10" Style="text-align: right;" onkeypress="return fnValidarData(event);"></asp:TextBox>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="21px" Width="300px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Button runat="server" ID="btBuscarVendas" Text="Buscar Vendas" OnClick="btBuscarVendas_Click" />
                        &nbsp;&nbsp;<asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <fieldset>
                <legend>Vendas por Cotas</legend>
                <div class="rounded_corners">
                    <asp:GridView ID="gvVendas" runat="server" Width="100%"
                        AutoGenerateColumns="False" ShowFooter="true" OnRowDataBound="gvVendas_RowDataBound">
                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                        <RowStyle HorizontalAlign="Center"></RowStyle>
                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                        <Columns>
                            <asp:BoundField DataField="DATA_VENDA" HeaderText="Data" />
                            <asp:TemplateField HeaderText="Vendas">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="LiteralValorVendas" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cotas">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="LiteralValorCotas" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="% Atingido">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="LiteralPercAtingido" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </div>
            </fieldset>
        </fieldset>
    </div>
</asp:Content>
