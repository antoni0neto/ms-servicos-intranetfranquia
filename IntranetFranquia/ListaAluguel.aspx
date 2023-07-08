<%@ Page Title="Alteração de Aluguel" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ListaAluguel.aspx.cs" Inherits="Relatorios.ListaAluguel" %>

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
                    <div style="width: 400px;" class="alinhamento">
                        <div style="width: 200px;"  class="alinhamento">
                            <label>Filial:&nbsp; </label>
                            <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="26px" Width="200px" ondatabound="ddlFilial_DataBound"></asp:DropDownList>
                        </div>
                        <div style="width: 200px;" class="alinhamento">
                            <label>Data:&nbsp; </label>
                            <asp:TextBox ID="txtData" runat="server" CssClass="textEntry" Height="22px" Width="198px"></asp:TextBox>
                            <asp:Calendar ID="CalendarData" runat="server" OnSelectionChanged="CalendarData_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <asp:Button runat="server" ID="btBuscaAlugueis" Text="Buscar Alugueis" OnClick="btBuscaAlugueis_Click" ValidationGroup="datas" CausesValidation="true"/>
                        <asp:ValidationSummary ID="ValidationSummaryAlugueis" runat="server" ValidationGroup="datas" ShowMessageBox="true" ShowSummary="false"/>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView id="GridViewAlugueis" runat="server" Width="100%" CssClass="DataGrid_Padrao" PageSize="1000" AllowPaging="True" 
                        AutoGenerateColumns="False" ShowFooter="true" OnRowDataBound="GridViewAlugueis_RowDataBound" ondatabound="GridViewAlugueis_DataBound"> 
    	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                    <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
	                    <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
	                    <RowStyle HorizontalAlign="Center"></RowStyle>
	                    <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="Filial">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="LiteralFilial"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Referência">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="LiteralReferencia"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="data_vencimento" HeaderText="Data Vencimento" />
                            <asp:TemplateField HeaderText="Valor">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="LiteralValor"/>
                                </ItemTemplate>
                            </asp:TemplateField>
<%--                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="btAlterar" Text="Alterar" OnClick="btAlterar_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
--%>                        </Columns>
	                    <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
