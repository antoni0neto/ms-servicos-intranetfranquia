<%@ Page Title="LineReportColection" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="LineReportColection.aspx.cs" Inherits="Relatorios.LineReportColection" %>

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
            <div style="width: 1000px;" class="alinhamento">
                <div style="width: 200px;"  class="alinhamento">
                    <label>Categoria:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlCategoria" DataValueField="COD_CATEGORIA" DataTextField="CATEGORIA_PRODUTO" Height="22px" 
                        Width="198px" ondatabound="ddlCategoria_DataBound"></asp:DropDownList>
                </div>
                <div style="width: 200px;"  class="alinhamento">
                    <label>Coleção:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlColecao" DataValueField="COLECAO" DataTextField="DESC_COLECAO" Height="22px" 
                        Width="198px" ondatabound="ddlColecao_DataBound"></asp:DropDownList>
                </div>
                <div style="width: 400px;"  class="alinhamento">
                    <label>Ano/Semana 454:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlSemana454" DataValueField="ANO_SEMANA_454" DataTextField="TEXTO" Height="22px" 
                        Width="396px" ondatabound="ddlSemana454_DataBound"></asp:DropDownList>
                </div>    
                <div style="width: 200px;"  class="alinhamento">
                    <label>Griffe:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlGriffe" DataValueField="COD_GRIFFE" 
                        DataTextField="GRIFFE" Height="22px" 
                        Width="198px" ondatabound="ddlGriffe_DataBound"></asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="ButtonPesquisarMovimento" Text="Buscar Histórico Produto" OnClick="ButtonPesquisarMovimento_Click" ValidationGroup="entrada"/>
            <asp:ValidationSummary ID="ValidationSummaryEntrada" runat="server" ValidationGroup="entrada" ShowMessageBox="true" ShowSummary="false" />
            <asp:Label runat="server" ID="lblMensagemEntrada" ForeColor="Red"></asp:Label>
        </div>
        <fieldset class="login">
        <table border="1" class="style1">
            <tr>
                <td>
                    <asp:GridView id="GridViewMovimentoProduto" runat="server" Width="100%" 
                        CssClass="DataGrid_Padrao" PageSize="1000" AllowPaging="True" 
                        AutoGenerateColumns="False" ShowFooter="true"> 
	                    <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
	                    <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
	                    <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
	                    <RowStyle HorizontalAlign="Center"></RowStyle>
	                    <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                        <Columns>
                            <asp:BoundField DataField="grupo" HeaderText="Grupo" />
                            <asp:BoundField DataField="colecao" HeaderText="Cole ção" />
                            <asp:BoundField DataField="descricao" HeaderText="Descrição" />
                            <asp:BoundField DataField="qt_venda_semana" HeaderText="Qt Vd Sem" />
                            <asp:BoundField DataField="vl_venda_semana" HeaderText="Vl Vd Sem" />
                            <asp:BoundField DataField="pc_medio_semana" HeaderText="Pç Medio Sem" />
                            <asp:BoundField DataField="cota_semana_1" HeaderText="Cota Sem 1" />
                            <asp:BoundField DataField="cota_semana_2" HeaderText="Cota Sem 2" />
                            <asp:BoundField DataField="cota_semana_3" HeaderText="Cota Sem 3" />
                            <asp:BoundField DataField="qt_venda_mes" HeaderText="Qt Vd Mês" />
                            <asp:BoundField DataField="vl_venda_mes" HeaderText="Vl Vd Mês" />
                            <asp:BoundField DataField="pc_medio_mes" HeaderText="Pç Medio Mês" />
                            <asp:BoundField DataField="cota_mes_1" HeaderText="Cota Mês 1" />
                            <asp:BoundField DataField="cota_mes_2" HeaderText="Cota Mês 2" />
                            <asp:BoundField DataField="cota_mes_3" HeaderText="Cota Mês 3" />
                            <asp:BoundField DataField="var_markup_original" HeaderText="Markup Original" />
                            <asp:BoundField DataField="var_markup_original_antigo" HeaderText="Markup Original Antigo" />
                            <asp:BoundField DataField="var_markup_liquido" HeaderText="Markup Líquido" />
                            <asp:BoundField DataField="var_markup_liquido_antigo" HeaderText="Markup Líquido Antigo" />
                            <asp:BoundField DataField="vl_remarcacao" HeaderText="% Remar cação" />
                            <asp:BoundField DataField="qt_estoque_semana" HeaderText="Qt Est Sem" />
                            <asp:BoundField DataField="vl_estoque_semana" HeaderText="Vl Est Sem" />
                            <asp:BoundField DataField="smu" HeaderText="SMU" />
                            <asp:BoundField DataField="pc_qt_giro" HeaderText="Giro" />
                            <asp:BoundField DataField="pc_qt_giro_antigo" HeaderText="Cober tura" />
                            <asp:BoundField DataField="qt_estoque_fabrica" HeaderText="Qt Est Fáb" />
                            <asp:BoundField DataField="vl_estoque_fabrica" HeaderText="Vl Est Fáb" />
                        </Columns>
	                    <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        </fieldset>
    </div>
</asp:Content>
