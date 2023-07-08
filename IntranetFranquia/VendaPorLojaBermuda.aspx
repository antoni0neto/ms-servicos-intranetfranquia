<%@ Page Title="Venda por Loja" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="VendaPorLojaBermuda.aspx.cs" Inherits="Relatorios.VendaPorLojaBermuda" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento
        {
            position: relative;
            float: left;
        }
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="accountInfo">
        <fieldset class="login">
        </fieldset>
        <table border="1" class="style1">
            <tr>
                <td>
                    <asp:GridView id="GridViewVendas" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333" ondatabound="GridViewVendas_DataBound" style="background:white">
	                    <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                    <RowStyle HorizontalAlign="Center"></RowStyle>
	                    <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                        <Columns>
                            <asp:BoundField DataField="FILIAL" HeaderText="Filial" />
                            <asp:BoundField DataField="DATA_MINIMA" HeaderText="Data Primeira Venda" />
                            <asp:BoundField DataField="DATA_MAXIMA" HeaderText="Data da Última Venda" />
                            <asp:BoundField DataField="QTDE" HeaderText="Qtde" />
                            <asp:BoundField DataField="COTA" HeaderText="Cota" />
                            <asp:BoundField DataField="PERCATINGIDO" HeaderText="Atingido" />
                        </Columns>
	                    <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </td>
                <td>
                    <label style="font-family: Arial; color: #000000; text-decoration: underline"></label>
                    <asp:Chart ID="Chart1" runat="server">
                        <series>
                            <asp:Series Name="Series1">
                            </asp:Series>
                        </series>
                        <chartareas>
                            <asp:ChartArea Name="ChartArea1">
                            </asp:ChartArea>
                        </chartareas>
                    </asp:Chart>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
