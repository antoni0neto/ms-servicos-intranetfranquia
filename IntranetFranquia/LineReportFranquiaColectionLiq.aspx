<%@ Page Title="Line Report Franquia Coleção" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="LineReportFranquiaColectionLiq.aspx.cs" Inherits="Relatorios.LineReportFranquiaColectionLiq" %>

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
    <div class="accountInfo">
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
            <div style="width: 1000px;" class="alinhamento">
                <div style="width: 200px;" class="alinhamento">
                    <label>
                        Categoria:&nbsp;
                    </label>
                    <asp:DropDownList runat="server" ID="ddlCategoria" DataValueField="COD_CATEGORIA"
                        DataTextField="CATEGORIA_PRODUTO" Height="22px" Width="198px" OnDataBound="ddlCategoria_DataBound">
                    </asp:DropDownList>
                </div>
                <div style="width: 200px;" class="alinhamento">
                    <label>
                        Coleção:&nbsp;
                    </label>
                    <asp:DropDownList runat="server" ID="ddlColecao" DataValueField="COLECAO" DataTextField="DESC_COLECAO"
                        Height="22px" Width="198px" OnDataBound="ddlColecao_DataBound">
                    </asp:DropDownList>
                </div>
                <div style="width: 400px;" class="alinhamento">
                    <label>
                        Ano/Semana 454:&nbsp;
                    </label>
                    <asp:DropDownList runat="server" ID="ddlSemana454" DataValueField="ANO_SEMANA_454"
                        DataTextField="TEXTO" Height="22px" Width="396px" OnDataBound="ddlSemana454_DataBound">
                    </asp:DropDownList>
                </div>
                <div style="width: 200px;" class="alinhamento">
                    <label>
                        Griffe:&nbsp;
                    </label>
                    <asp:DropDownList runat="server" ID="ddlGriffe" DataValueField="COD_GRIFFE" DataTextField="GRIFFE"
                        Height="22px" Width="198px" OnDataBound="ddlGriffe_DataBound">
                    </asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="ButtonPesquisarMovimento" Text="Buscar Histórico Produto"
                OnClick="ButtonPesquisarMovimento_Click" ValidationGroup="entrada" />
            <asp:ValidationSummary ID="ValidationSummaryEntrada" runat="server" ValidationGroup="entrada"
                ShowMessageBox="true" ShowSummary="false" />
            <asp:Label runat="server" ID="lblMensagemEntrada" ForeColor="Red"></asp:Label>
        </div>
        <br />
        <div style="overflow: scroll;">
            <div style="width:5000px;">
            <asp:GridView ID="GridViewMovimentoProduto" runat="server" Width="100%" CssClass="DataGrid_Padrao"
                PageSize="1000" AllowPaging="True" AutoGenerateColumns="False" ShowFooter="true"
                OnDataBound="GridViewMovimentoProduto_DataBound">
                <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
                <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
                <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
                <RowStyle HorizontalAlign="Center"></RowStyle>
                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center" CssClass="DataGrid_Header">
                </HeaderStyle>
                <Columns>
                    <asp:BoundField DataField="grupo" HeaderStyle-Width="80px" HeaderText="Grupo" />
                    <asp:BoundField DataField="colecao" HeaderStyle-Width="80px" HeaderText="Coleção" />
                    <asp:BoundField DataField="qt_estoque_sobra" HeaderStyle-Width="90px" HeaderText="Qtde Sobra" />
                    <asp:BoundField DataField="vl_estoque_sobra" HeaderStyle-Width="100px" HeaderText="Valor Sobra" />
                    <asp:BoundField DataField="qt_venda_semana" HeaderStyle-Width="120px" HeaderText="Qtde Venda Sem" />
                    <asp:BoundField DataField="vl_venda_semana" HeaderStyle-Width="120px" HeaderText="Valor Venda Sem" />
                    <asp:BoundField DataField="pc_medio_semana" HeaderStyle-Width="120px" HeaderText="Pç Médio Sem" />
                    <asp:BoundField DataField="qt_vd_sem_ano_pas" HeaderStyle-Width="180px" HeaderText="Qtde Venda Sem Ano Pas" />
                    <asp:BoundField DataField="vl_vd_sem_ano_pas" HeaderStyle-Width="180px" HeaderText="Valor Vd Sem Ano Pas" />
                    <asp:BoundField DataField="cota_semana_qtde" HeaderStyle-Width="120px" HeaderText="Cota Sem Qtde" />
                    <asp:BoundField DataField="cota_semana_valor" HeaderStyle-Width="120px" HeaderText="Cota Sem Valor" />
                    <asp:BoundField DataField="dif_semana_qtde" HeaderStyle-Width="120px" HeaderText="Dif Sem Qtde" />
                    <asp:BoundField DataField="dif_semana_valor" HeaderStyle-Width="120px" HeaderText="Dif Sem Valor" />
                    <asp:BoundField DataField="qt_venda_mes" HeaderStyle-Width="120px" HeaderText="Qtde Venda Mês" />
                    <asp:BoundField DataField="vl_venda_mes" HeaderStyle-Width="120px" HeaderText="Valor Venda Mês" />
                    <asp:BoundField DataField="pc_medio_mes" HeaderStyle-Width="100px" HeaderText="Pç Médio Mês" />
                    <asp:BoundField DataField="qt_vd_mes_ano_pas" HeaderStyle-Width="180px" HeaderText="Qtde Venda Mês Ano Pas" />
                    <asp:BoundField DataField="vl_vd_mes_ano_pas" HeaderStyle-Width="180px" HeaderText="Valor Venda Mês Ano Pas" />
                    <asp:BoundField DataField="cota_mes_qtde" HeaderStyle-Width="130px" HeaderText="Cota Mês Qtde" />
                    <asp:BoundField DataField="cota_mes_valor" HeaderStyle-Width="130px" HeaderText="Cota Mês Valor" />
                    <asp:BoundField DataField="dif_mes_qtde" HeaderStyle-Width="130px" HeaderText="Dif Mês Qtde" />
                    <asp:BoundField DataField="dif_mes_valor" HeaderStyle-Width="130px" HeaderText="Dif Mês Valor" />
                    <asp:BoundField DataField="dif_2011_2012" HeaderStyle-Width="180px" HeaderText="Dif AnoAnterior/AnoAtual" Visible="false" />
                    <asp:BoundField DataField="var_markup_original_antigo" HeaderStyle-Width="180px" HeaderText="Markup Original Tradicional" />
                    <asp:BoundField DataField="var_markup_liquido" HeaderStyle-Width="125px" HeaderText="Markup Líquido %" />
                    <asp:BoundField DataField="var_markup_liquido_antigo" HeaderStyle-Width="180px" HeaderText="Markup Líquido Tradicional" />
                    <asp:BoundField DataField="vl_remarcacao" HeaderStyle-Width="120px" HeaderText="% Remarcação" />
                    <asp:BoundField DataField="qt_estoque_ini_semana" HeaderStyle-Width="160px" HeaderText="Qtde Estoque Ini Sem" />
                    <asp:BoundField DataField="qt_estoque_semana" HeaderStyle-Width="160px" HeaderText="Qtde Estoque Fim Sem" />
                    <asp:BoundField DataField="vl_estoque_semana" HeaderStyle-Width="160px" HeaderText="Valor Estoque Sem" />
                    <asp:BoundField DataField="smu" HeaderStyle-Width="110px" HeaderText="SMU" />
                    <asp:BoundField DataField="pc_qt_giro" HeaderStyle-Width="110px" HeaderText="Giro" />
                    <asp:BoundField DataField="pc_qt_giro_antigo" HeaderStyle-Width="120px" HeaderText="Cobertura" />
                    <asp:BoundField DataField="qt_estoque_fabrica" HeaderStyle-Width="135px" HeaderText="Qtde Estoque Fáb" />
                    <asp:BoundField DataField="vl_estoque_fabrica" HeaderStyle-Width="135px" HeaderText="Valor Est Fáb" />
                </Columns>
                <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
            </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
