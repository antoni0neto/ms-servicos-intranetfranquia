<%@ Page Title="Line Report Loja Coleção" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="LineReportLojaColection.aspx.cs" Inherits="Relatorios.LineReportLojaColection" %>

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
            <div style="width: 800px;" class="alinhamento">
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
                <div style="width: 200px;"  class="alinhamento">
                    <label>Filial:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="22px" 
                        Width="198px" ondatabound="ddlFilial_DataBound"></asp:DropDownList>
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
                    <asp:GridView id="GridViewMovimentoProduto" runat="server" Width="100%" CssClass="DataGrid_Padrao" PageSize="1000" AllowPaging="True" 
                        AutoGenerateColumns="False" ShowFooter="true" ondatabound="GridViewMovimentoProduto_DataBound"> 
	                    <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
	                    <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
	                    <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
	                    <RowStyle HorizontalAlign="Center"></RowStyle>
	                    <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                        <Columns>
                            <asp:BoundField DataField="grupo" HeaderText="Grupo"/>
                            <asp:BoundField DataField="colecao" HeaderText="Cole ção" />
                            <asp:BoundField DataField="descricao" HeaderText="Descrição" />
                            <asp:BoundField DataField="qt_venda_semana" HeaderText="Qt Vd Sem" />
                            <asp:BoundField DataField="vl_venda_semana" HeaderText="Vl Vd Sem" />
                            <asp:BoundField DataField="pc_medio_semana" HeaderText="Pç Medio Sem" />
                            <asp:BoundField DataField="qt_vd_sem_ano_pas" HeaderText="Qt Vd Sem Ano Pas" />
                            <asp:BoundField DataField="vl_vd_sem_ano_pas" HeaderText="Vl Vd Sem Ano Pas" />
                            <asp:BoundField DataField="qt_venda_mes" HeaderText="Qt Vd Mês" />
                            <asp:BoundField DataField="vl_venda_mes" HeaderText="Vl Vd Mês" />
                            <asp:BoundField DataField="pc_medio_mes" HeaderText="Pç Medio Mês" />
                            <asp:BoundField DataField="qt_vd_mes_ano_pas" HeaderText="Qt Vd Mês Ano Pas" />
                            <asp:BoundField DataField="vl_vd_mes_ano_pas" HeaderText="Vl Vd Mês Ano Pas" />
                            <asp:BoundField DataField="dif_mes_qtde" HeaderText="Dif Mês Qtde" />
                            <asp:BoundField DataField="dif_mes_valor" HeaderText="Dif Mês Valor" />
                            <asp:BoundField DataField="dif_2011_2012" HeaderText="Dif 2011 2012" />
                            <asp:BoundField DataField="qt_estoque_semana" HeaderText="Qt Est Sem" />
                            <asp:BoundField DataField="vl_estoque_semana" HeaderText="Vl Est Sem" />
                            <asp:BoundField DataField="pc_qt_giro_antigo" HeaderText="Cober tura" />
                        </Columns>
	                    <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        </fieldset>
    </div>
</asp:Content>
