<%@ Page Title="Verificar Receitas" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="VerificarReceitas.aspx.cs" Inherits="Relatorios.VerificarReceitas" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
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
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">
            <asp:Label ID="labTitulo" runat="server" Text="Administração de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Fechamento&nbsp;&nbsp;>&nbsp;&nbsp;Verificar Receitas Caixa"></asp:Label></span>
        <div style="float: right; padding: 0;">
            <a href="~/mod_adm_loja/admloj_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">
                Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset class="login">
            <legend>Verificar Receitas Caixa</legend>
            <div style="width: 800px;" class="alinhamento">
                <div style="width: 200px;" class="alinhamento">
                    <label>
                        Data Início:&nbsp;
                    </label>
                    <asp:TextBox ID="TextBoxDataInicio" runat="server" CssClass="textEntry" Height="22px"
                        Width="198px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDataInicio" runat="server" OnSelectionChanged="CalendarDataInicio_SelectionChanged"
                        CaptionAlign="Bottom"></asp:Calendar>
                </div>
                <div style="width: 200px;" class="alinhamento">
                    <label>
                        Data Fim:&nbsp;
                    </label>
                    <asp:TextBox ID="TextBoxDataFim" runat="server" CssClass="textEntry" Height="22px"
                        Width="198px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDataFim" runat="server" OnSelectionChanged="CalendarDataFim_SelectionChanged"
                        CaptionAlign="Bottom"></asp:Calendar>
                </div>
                <div style="width: 200px;" class="alinhamento">
                    <label>
                        Filial:&nbsp;
                    </label>
                    <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                        Height="26px" Width="200px" OnDataBound="ddlFilial_DataBound">
                    </asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;<asp:Label ID="labErroFilial" runat="server" ForeColor="Red" Text=""></asp:Label>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btBuscarReceitas" Text="Buscar Receitas" OnClick="btBuscarReceitas_Click" />
        </div>
        <table border="1" class="style1">
            <tr>
                <td>
                    <asp:GridView ID="GridViewReceitas" runat="server" Width="100%" CssClass="DataGrid_Padrao"
                        PageSize="1000" AllowPaging="True" AutoGenerateColumns="False" ShowFooter="true"
                        OnRowDataBound="GridViewReceitas_RowDataBound" OnDataBound="GridViewReceitas_DataBound">
                        <FooterStyle HorizontalAlign="Center"></FooterStyle>
                        <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
                        <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
                        <RowStyle HorizontalAlign="Center"></RowStyle>
                        <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="Filial">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="LiteralFilial" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DATA_FECHAMENTO" HeaderText="Data do Fechamento" />
                            <asp:BoundField DataField="VALOR_DINHEIRO" HeaderText="Valor Dinheiro" />
                            <asp:BoundField DataField="VALOR_CHEQUE" HeaderText="Valor Cheque" />
                            <asp:BoundField DataField="VALOR_CHEQUE_PRE" HeaderText="Valor Cheque Pré" />
                            <asp:BoundField DataField="VALOR_CARTAO_CREDITO" HeaderText="Valor Cartão" />
                            <asp:BoundField DataField="VALOR_COMANDA" HeaderText="Valor da Comanda" />
                            <asp:BoundField DataField="VALOR_RETIRADA" HeaderText="Valor da Retirada" />
                        </Columns>
                        <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
