<%@ Page Title="BranchReportCotas" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="BranchReportCotas.aspx.cs" Inherits="Relatorios.BranchReportCotas" %>

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
        <fieldset class="login">
            <div style="width: 400px;" class="alinhamento">
                <div style="width: 400px;"  class="alinhamento">
                    <label>Ano/Semana 454:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlSemana454" DataValueField="ANO_SEMANA_454" DataTextField="TEXTO" Height="22px" 
                        Width="396px" ondatabound="ddlSemana454_DataBound"></asp:DropDownList>
                </div>    
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="ButtonPesquisarMovimento" Text="Buscar Vendas Loja" OnClick="ButtonPesquisarMovimento_Click" ValidationGroup="entrada"/>
            <asp:ValidationSummary ID="ValidationSummaryEntrada" runat="server" ValidationGroup="entrada" ShowMessageBox="true" ShowSummary="false" />
            <asp:Label runat="server" ID="lblMensagemEntrada" ForeColor="Red"></asp:Label>
        </div>
        <fieldset class="login">
        <table border="1" class="style1">
            <tr>
                <td>
                    <asp:GridView id="GridViewVendaLoja" runat="server" Width="100%" CssClass="DataGrid_Padrao" PageSize="1000" AllowPaging="True" AutoGenerateColumns="False" ShowFooter="true" 
                        onrowdatabound="GridViewVendaLoja_RowDataBound"> 
	                    <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
	                    <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
	                    <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
	                    <RowStyle HorizontalAlign="Center"></RowStyle>
	                    <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                        <Columns>
                            <asp:BoundField DataField="descricao" HeaderText="Loja" />
                            <asp:BoundField DataField="vl_venda_semana" HeaderText="Vl Vd Semana" />
                            <asp:BoundField DataField="qt_pecas_semana" HeaderText="Qt Pçs Semana" />
                            <asp:BoundField DataField="vl_medio_semana" HeaderText="Vl Médio Semana" />
                            <asp:BoundField DataField="perc_venda_semana" HeaderText="% Vd Semana" />
                            <asp:BoundField DataField="qt_pecas_estoque_semana" HeaderText="Estoque Atual" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="ButtonPequisar" Text="Vêr" OnClick="ButtonPesquisar_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="perc_estoque_atuaL" HeaderText="% Estoque Atual" />
                            <asp:BoundField DataField="vl_venda_mes" HeaderText="Vl Vd Mês" />
                            <asp:BoundField DataField="qt_pecas_mes" HeaderText="Qt Pçs Mês" />
                            <asp:BoundField DataField="vl_medio_mes" HeaderText="Vl Médio Mês" />
                            <asp:BoundField DataField="vl_cota" HeaderText="Valor Cota" />
                            <asp:BoundField DataField="perc_cota" HeaderText="% Cota" />
                        </Columns>
	                    <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <table border="1" class="style1">
            <tr>
                <td>
                    <asp:GridView id="GridViewEstoque" runat="server" Width="100%" 
                        CssClass="DataGrid_Padrao" PageSize="1000" AllowPaging="True" 
                        AutoGenerateColumns="False" ShowFooter="true"> 
	                    <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
	                    <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
	                    <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
	                    <RowStyle HorizontalAlign="Center"></RowStyle>
	                    <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                        <Columns>
                            <asp:BoundField DataField="descricao" HeaderText="Grupo de Produto" />
                            <asp:BoundField DataField="vl_venda_semana" HeaderText="Vl Vd Semana" />
                            <asp:BoundField DataField="qt_pecas_semana" HeaderText="Qt Pçs Semana" />
                            <asp:BoundField DataField="vl_medio_semana" HeaderText="Vl Médio Semana" />
                            <asp:BoundField DataField="perc_venda_semana" HeaderText="% Vd Semana" />
                            <asp:BoundField DataField="qt_pecas_estoque_semana" HeaderText="Estoque Atual" />
                            <asp:BoundField DataField="perc_estoque_atuaL" HeaderText="% Estoque Atual" />
                            <asp:BoundField DataField="vl_venda_mes" HeaderText="Vl Vd Mês" />
                            <asp:BoundField DataField="qt_pecas_mes" HeaderText="Qt Pçs Mês" />
                            <asp:BoundField DataField="vl_medio_mes" HeaderText="Vl Médio Mês" />
                            <asp:BoundField DataField="vl_cota" HeaderText="Valor Cota" />
                            <asp:BoundField DataField="perc_cota" HeaderText="% Cota" />
                        </Columns>
	                    <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        </fieldset>
    </div>
</asp:Content>
