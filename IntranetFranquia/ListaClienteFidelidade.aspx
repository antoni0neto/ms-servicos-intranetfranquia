<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListaClienteFidelidade.aspx.cs" Inherits="Relatorios.ListaClienteFidelidade" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinhamento
        {
            position: relative;
            float: left;
        }
        .style1
        {
            width: 100%;
            background-color:White;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="accountInfo">
        <fieldset>
        </fieldset>
        <fieldset class="login">
            <div style="width: 400px;" class="alinhamento">
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
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="ButtonPesquisarVendas" Text="Buscar Vendas" OnClick="ButtonPesquisarVendas_Click"/>
            <asp:ValidationSummary ID="ValidationSummaryVendas" runat="server" ShowMessageBox="true" ShowSummary="false" />
        </div>
        <fieldset class="login">
        <table border="1" class="style1">
            <tr>
                <asp:GridView id="GridViewFidelidade" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333" 
                              ondatabound="GridViewFidelidade_DataBound" style="background:white">
	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="FILIAL" HeaderText="Filial" />
                        <asp:BoundField DataField="CLIENTE_FIEL" HeaderText="Linkado" />
                        <asp:TemplateField HeaderText="% Linkado">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralPercLinkado"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CLIENTE_NAO_FIEL" HeaderText="Não Linkado" />
                        <asp:TemplateField HeaderText="% Não Linkado">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralPercNaoLinkado"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </tr>
        </table>
        </fieldset>
    </div>
</asp:Content>
