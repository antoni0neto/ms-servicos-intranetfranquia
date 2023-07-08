<%@ Page Title="Conta Itens de Produto" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ContaItensProduto.aspx.cs" Inherits="Relatorios.ContaItensProduto" %>

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
            <legend>Filtro de Pesquisa</legend>
            <div style="width: 600px;" class="alinhamento">
                <div style="width: 200px;" class="alinhamento">
                    <label>Data Início:&nbsp; </label>
                    <asp:TextBox ID="TextBoxDataInicio" runat="server" CssClass="textEntry" Height="22px" Width="196px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDataInicio" runat="server" ondayrender="CalendarDataInicio_DayRender" OnSelectionChanged="CalendarDataInicio_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                </div>
                <div style="width: 200px;" class="alinhamento">
                    <label>Data Fim:&nbsp; </label>
                    <asp:TextBox ID="TextBoxDataFim" runat="server" CssClass="textEntry" Height="22px" Width="196px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDataFim" runat="server" ondayrender="CalendarDataInicio_DayRender" OnSelectionChanged="CalendarDataFim_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                </div>
                <div style="width: 200px;"  class="alinhamento">
                    <label>Filial:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="22px" Width="220px" ondatabound="ddlFilial_DataBound"></asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btBuscar" Text="Buscar Vendas" OnClick="btBuscar_Click"/>
            <asp:ValidationSummary ID="ValidationSummaryVendas" runat="server" ShowMessageBox="true" ShowSummary="false" />
        </div>
        <table border="1" class="style1">
            <tr>
                <td>
                    <asp:GridView id="GridViewVendas" runat="server" Width="100%" CssClass="DataGrid_Padrao" PageSize="10" AllowPaging="True" AutoGenerateColumns="False" showfooter="true"
                                  ondatabound="GridViewVendas_DataBound" onrowdatabound="GridViewVendas_RowDataBound" onpageindexchanging="GridViewVendas_PageIndexChanging">
    	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                    <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
	                    <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
	                    <RowStyle HorizontalAlign="Center"></RowStyle>
	                    <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                        <Columns>
                            <asp:BoundField DataField="QtdeItensProduto" HeaderText="Qtde Itens" />
                            <asp:BoundField DataField="QtdeTicket" HeaderText="Qtde Ticket" />
                            <asp:BoundField DataField="ValorTicket" HeaderText="Valor Ticket" />
                            <asp:BoundField DataField="DescontoTicket" HeaderText="Desconto Ticket" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="ButtonPequisar" Text="Vêr" OnClick="ButtonPesquisar_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
	                    <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView id="GridViewVendedor" runat="server" Width="100%" CssClass="DataGrid_Padrao" AutoGenerateColumns="False" showfooter="true" Visible="false"
                                  onrowdatabound="GridViewVendedor_RowDataBound">
    	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                    <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
	                    <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
	                    <RowStyle HorizontalAlign="Center"></RowStyle>
	                    <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                        <Columns>
                            <asp:BoundField DataField="Ticket" HeaderText="Ticket" />
                            <asp:TemplateField HeaderText="Vendedor">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="LiteralVendedor"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Data_Venda" HeaderText="Data da Venda" />
                            <asp:BoundField DataField="Valor_Pago" HeaderText="Valor Pago" />
                            <asp:BoundField DataField="Desconto" HeaderText="Desconto" />
                            <asp:BoundField DataField="Valor_Troca" HeaderText="Valor da Troca" />
                        </Columns>
	                    <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
