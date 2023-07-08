﻿<%@ Page Title="Análise de Atendimento em Loja" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="AnaliseAtendimento.aspx.cs" Inherits="Relatorios.AnaliseAtendimento" %>

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
            <legend>Critérios de Vendas em Loja</legend>
            <div style="width: 600px;" class="alinhamento">
                <div style="width: 200px;" class="alinhamento">
                    <label>Data Início:&nbsp; </label>
                    <asp:TextBox ID="TextBoxDataInicio" runat="server" CssClass="textEntry" Height="22px" Width="198px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDataInicio" runat="server" OnSelectionChanged="CalendarDataInicio_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                </div>
                <div style="width: 200px;" class="alinhamento">
                    <label>Data Fim:&nbsp; </label>
                    <asp:TextBox ID="TextBoxDataFim" runat="server" CssClass="textEntry" Height="22px" Width="198px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDataFim" runat="server" OnSelectionChanged="CalendarDataFim_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                </div>
                <div style="width: 200px;"  class="alinhamento">
                    <label>Filial:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="26px" 
                        Width="200px" ondatabound="ddlFilial_DataBound"></asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="ButtonPesquisarVendas" Text="Buscar Vendas" OnClick="ButtonPesquisarVendas_Click"/>
            <asp:ValidationSummary ID="ValidationSummaryVendas" runat="server" ShowMessageBox="true" ShowSummary="false" />
        </div>
        <table border="1" class="style1">
            <tr>
                <td>
                    <asp:GridView id="GridViewVendas" runat="server" Width="100%" CssClass="DataGrid_Padrao" PageSize="15" AllowPaging="True" AutoGenerateColumns="False" ShowFooter="true"
                                  ondatabound="GridViewVendas_DataBound">
	                    <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                    <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
	                    <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
	                    <RowStyle HorizontalAlign="Center"></RowStyle>
	                    <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                        <Columns>
                            <asp:BoundField DataField="HORA" HeaderText="Hora" />
                            <asp:BoundField DataField="VALOR" HeaderText="Valor" />
                            <asp:BoundField DataField="N_VENDEDOR" HeaderText="Qt Vendedor" />
                            <asp:BoundField DataField="N_TICKET" HeaderText="Qt Ticket's" />
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
