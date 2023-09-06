<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="VendasPorCotaLoja.aspx.cs" Inherits="Relatorios.VendasPorCotaLoja" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento {
            position: relative;
            float: left;
        }

        .style1 {
            width: 100%;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">
            <asp:Label ID="labTitulo" runat="server" Text="Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Desempenho&nbsp;&nbsp;>&nbsp;&nbsp;Venda/Cotas por Loja"></asp:Label></span>
        <div style="float: right; padding: 0;">
            <a href="DefaultGerenteFilial.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div>
        <fieldset>
            <legend>Venda/Cotas por Loja</legend>
            <table>
                <tr>
                    <td>
                        <div style="width: 600px;" class="alinhamento">
                            <div style="width: 200px;" class="alinhamento">
                                <label>Data Início:&nbsp; </label>
                                <asp:TextBox ID="txtDataInicio" runat="server" CssClass="textEntry" Height="22px" Width="198px"></asp:TextBox>
                                <asp:Calendar ID="CalendarDataInicio" runat="server" OnSelectionChanged="CalendarDataInicio_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                            </div>
                            <div style="width: 200px;" class="alinhamento">
                                <label>Data Fim:&nbsp; </label>
                                <asp:TextBox ID="txtDataFim" runat="server" CssClass="textEntry" Height="22px" Width="198px"></asp:TextBox>
                                <asp:Calendar ID="CalendarDataFim" runat="server" OnSelectionChanged="CalendarDataFim_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                            </div>
                            <div style="width: 200px;" class="alinhamento">
                                <label>Filial:&nbsp; </label>
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="26px"
                                    Width="200px" OnDataBound="ddlFilial_DataBound">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>

                        <div style="float: left;">
                            <asp:Button runat="server" ID="btBuscarVendas" Text="Buscar Vendas" OnClick="btBuscarVendas_Click" />
                            &nbsp;&nbsp;<asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </div>
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
