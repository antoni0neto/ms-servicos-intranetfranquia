<%@ Page Title="Performance de Loja" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="desp_loja_performance.aspx.cs" Inherits="Relatorios.desp_loja_performance" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento
        {
            float: left;
            position: static;
        }
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo Acompanhamento Mensal&nbsp;&nbsp;>&nbsp;&nbsp;Despesas
            Lojas&nbsp;&nbsp;>&nbsp;&nbsp;Performance de Loja&nbsp;&nbsp;</span>
        <div style="float: right; padding: 0; position: ;">
            <a href="../acomp_menu.aspx" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset>
            <legend>Performance de Loja</legend>
            <table border="0" class="style1">
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <label>
                                        Vencimento&nbsp;
                                    </label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlVencimento" DataValueField="CODIGO_ACOMPANHAMENTO_MESANO"
                                        DataTextField="DESCRICAO" Height="26px" Width="200px" OnDataBound="ddlVencimento_DataBound">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <asp:Button runat="server" ID="btPerformance" Text="Buscar Performance" OnClick="btPerformance_Click" /><asp:Label
                                ID="labErro" runat="server" ForeColor="Red"></asp:Label>
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
                            <asp:GridView runat="server" ID="gvPerformance" AutoGenerateColumns="false" Width="100%"
                                OnRowDataBound="gvPerformance_RowDataBound" OnDataBound="gvPerformance_DataBound"
                                ShowFooter="True" AllowSorting="true" OnSorting="gvPerformance_Sorting">
                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="litColuna" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FILIAL" HeaderText="Filial" HeaderStyle-Width="300px"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="FATURAMENTO" HeaderText="Faturamento" DataFormatString="{0:c}"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BOLETO" HeaderText="Boleto" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="PERF" HeaderText="Perfomance" SortExpression="P" ItemStyle-HorizontalAlign="Center" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
