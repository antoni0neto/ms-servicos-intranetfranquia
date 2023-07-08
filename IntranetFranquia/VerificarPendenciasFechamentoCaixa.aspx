<%@ Page Title="Verificar Pendencias no Fechamento de Caixa" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="VerificarPendenciasFechamentoCaixa.aspx.cs"
    Inherits="Relatorios.VerificarPendenciasFechamentoCaixa" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
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
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo do Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Ver
            Pendências Fechamento Caixa</span>
        <div style="float: right; padding: 0;">
            <a href="DefaultAdmFilial.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset>
            <legend>Ver Pendências Fechamento Caixa</legend>
            <table border="0" class="style1">
                <tr>
                    <td>
                        <div style="width: 200px;" class="alinhamento">
                            <label>
                                Data Inicio:&nbsp;
                            </label>
                            <asp:TextBox ID="txtDataInicio" runat="server" CssClass="textEntry" Height="22px"
                                Width="196px" Enabled="False"></asp:TextBox>
                            <asp:Calendar ID="CalendarDataInicio" runat="server" OnSelectionChanged="CalendarDataInicio_SelectionChanged"
                                CaptionAlign="Bottom"></asp:Calendar>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorDataInicio" runat="server"
                                SetFocusOnError="true" Display="None" ErrorMessage="Escolha Data Inicio" ControlToValidate="txtDataInicio"
                                ValidationGroup="datas"></asp:RequiredFieldValidator>
                        </div>
                        <div style="width: 200px;" class="alinhamento">
                            <label>
                                Data Fim:&nbsp;
                            </label>
                            <asp:TextBox ID="txtDataFim" runat="server" CssClass="textEntry" Height="22px" Width="196px"
                                Enabled="False"></asp:TextBox>
                            <asp:Calendar ID="CalendarDataFim" runat="server" OnSelectionChanged="CalendarDataFim_SelectionChanged"
                                CaptionAlign="Bottom"></asp:Calendar>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorDataFim" runat="server" SetFocusOnError="true"
                                Display="None" ErrorMessage="Escolha Data Fim" ControlToValidate="txtDataFim"
                                ValidationGroup="datas"></asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <asp:Button runat="server" ID="btBuscaDivergencias" Text="Buscar Divergências" OnClick="btBuscaDivergencias_Click"
                                ValidationGroup="datas" CausesValidation="true" />
                            <asp:ValidationSummary ID="ValidationSummaryDivergencias" runat="server" ValidationGroup="datas"
                                ShowMessageBox="true" ShowSummary="false" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="GridViewFechamento" runat="server" Width="100%" CssClass="DataGrid_Padrao"
                            PageSize="1000" AllowPaging="True" AutoGenerateColumns="False" ShowFooter="true"
                            OnRowDataBound="GridViewFechamento_RowDataBound" OnDataBound="GridViewFechamento_DataBound">
                            <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
                            <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
                            <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
                            <RowStyle HorizontalAlign="Center"></RowStyle>
                            <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                            <Columns>
                                <asp:BoundField DataField="filial" HeaderText="Filial" />
                                <asp:BoundField DataField="data" HeaderText="Data" />
                                <asp:BoundField DataField="valorDinheiroLinx" HeaderText="Valor Dinheiro Linx" />
                                <asp:BoundField DataField="valorDinheiroIntranet" HeaderText="Valor Dinheiro Intranet" />
                                <asp:BoundField DataField="valorCartaoLinx" HeaderText="Valor Cartão Linx" />
                                <asp:BoundField DataField="valorCartaoIntranet" HeaderText="Valor Cartão Intranet" />
                                <asp:BoundField DataField="valorOutrosLinx" HeaderText="Valor Outros Linx" />
                                <asp:BoundField DataField="valorOutrosIntranet" HeaderText="Valor Outros Intranet" />
                                <asp:TemplateField HeaderText="Total Linx">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralTotalLinx" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Intranet">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralTotalIntranet" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="valorComandaIntranet" HeaderText="Valor Comanda Intranet" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button runat="server" ID="btPequisar" Text="Alterar" OnClick="btPesquisar_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
