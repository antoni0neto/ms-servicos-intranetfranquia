<%@ Page Title="SaidaProduto" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="VendaEstoque.aspx.cs" Inherits="Relatorios.VendaEstoque" %>

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
            <legend>Critérios de Produtos</legend>
            <div style="width: 900px;" class="alinhamento">
                <div style="width: 150px;"  class="alinhamento">
                    <label>Grupo:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlGrupo" DataValueField="CODIGO_GRUPO" DataTextField="GRUPO_PRODUTO" Height="22px" 
                        Width="150px" ondatabound="ddlGrupo_DataBound"></asp:DropDownList>
                </div>
                <div style="width: 150px;"  class="alinhamento">
                    <label>SubGrupo:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlSubGrupo" DataValueField="CODIGO_SUBGRUPO" DataTextField="SUBGRUPO_PRODUTO" Height="22px" 
                        Width="150px" ondatabound="ddlSubGrupo_DataBound"></asp:DropDownList>
                </div>
                <div style="width: 150px;"  class="alinhamento">
                    <label>Coleção:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlColecao" DataValueField="COLECAO" DataTextField="DESC_COLECAO" Height="22px" 
                        Width="150px" ondatabound="ddlColecao_DataBound"></asp:DropDownList>
                </div>
                <div style="width: 150px;"  class="alinhamento">
                    <label>Linha:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlLinha" DataValueField="COD_LINHA" DataTextField="LINHA" Height="22px" 
                        Width="150px" ondatabound="ddlLinha_DataBound"></asp:DropDownList>
                </div>
                <div style="width: 150px;"  class="alinhamento">
                    <label>Griffe:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlGriffe" DataValueField="COD_GRIFFE" DataTextField="GRIFFE" Height="22px" 
                        Width="150px" ondatabound="ddlGriffe_DataBound"></asp:DropDownList>
                </div>
                <div style="width: 150px;"  class="alinhamento">
                    <label>Tipo:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlTipo" DataValueField="COD_TIPO_PRODUTO" DataTextField="TIPO_PRODUTO" Height="22px" 
                        Width="150px" ondatabound="ddlTipo_DataBound"></asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="ButtonPesquisarProduto" Text="Buscar Produto" OnClick="ButtonPesquisarProduto_Click" ValidationGroup="produto"/>
            <asp:ValidationSummary ID="ValidationSummaryProduto" runat="server" ValidationGroup="produto" ShowMessageBox="true" ShowSummary="false" />
            <asp:Label runat="server" ID="lblMensagemProduto" ForeColor="Red"></asp:Label>
        </div>
        <fieldset class="login">
            <legend>Critérios de Vendas em Loja</legend>
            <div style="width: 900px;" class="alinhamento">
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
                    <label>Produto:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlProduto" DataValueField="PRODUTO1" 
                        DataTextField="DESC_PRODUTO" Height="26px" AutoPostBack="true"
                        Width="200px" ondatabound="ddlProduto_DataBound" 
                        onselectedindexchanged="ddlProduto_SelectedIndexChanged"></asp:DropDownList>
                </div>
                <div style="width: 200px;"  class="alinhamento">
                    <label>Cor:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlProdutoCores" DataValueField="COR_PRODUTO" DataTextField="DESC_COR_PRODUTO" Height="26px" 
                        Width="200px" ondatabound="ddlProdutoCores_DataBound"></asp:DropDownList>
                </div>
                <div style="width: 100px;"  class="alinhamento">
                    <label>Código Produto:&nbsp; </label>
                    <asp:TextBox ID="TextBoxCodigoProduto" runat="server" CssClass="textEntry" 
                        MaxLength="10" Height="22px" Width="100px" AutoPostBack="true" 
                        ontextchanged="TextBoxCodigoProduto_TextChanged"></asp:TextBox>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="ButtonPesquisarEntrada" Text="Buscar Vendas em Loja" OnClick="ButtonPesquisarEntrada_Click" ValidationGroup="entrada"/>
            <asp:ValidationSummary ID="ValidationSummaryEntrada" runat="server" ValidationGroup="entrada" ShowMessageBox="true" ShowSummary="false" />
            <asp:Label runat="server" ID="lblMensagemEntrada" ForeColor="Red"></asp:Label>
        </div>
        <table border="1" class="style1">
            <tr>
                <td>
                    <asp:GridView id="GridViewSaida" runat="server" Width="100%" CssClass="DataGrid_Padrao" PageSize="10" AllowPaging="True" AutoGenerateColumns="False" onpageindexchanging="GridViewSaida_PageIndexChanging">
	                    <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
	                    <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
	                    <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
	                    <RowStyle HorizontalAlign="Center"></RowStyle>
	                    <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                        <Columns>
                            <asp:BoundField DataField="FILIAL" HeaderText="Filial" />
                            <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" />
                            <asp:BoundField DataField="QTDE_VENDA" HeaderText="Qtd Venda" />
                            <asp:BoundField DataField="QTDE_ESTOQUE" HeaderText="Qtd Estoque" />
                        </Columns>
	                    <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
