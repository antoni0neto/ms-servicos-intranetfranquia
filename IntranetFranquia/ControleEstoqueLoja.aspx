<%@ Page Title="Controle de Estoque de Loja" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ControleEstoqueLoja.aspx.cs" Inherits="Relatorios.ControleEstoqueLoja" %>

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
            <div style="width: 900px;" class="alinhamento">
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
                <div style="width: 300px;"  class="alinhamento">
                    <label>Ano/Semana 454:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlSemana454" DataValueField="ANO_SEMANA_454" DataTextField="TEXTO" Height="22px" 
                        Width="298px" ondatabound="ddlSemana454_DataBound"></asp:DropDownList>
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
                            <asp:BoundField DataField="subGrupo" HeaderText="SubGrupo" />
                            <asp:BoundField DataField="tipoProduto" HeaderText="Tipo Produto" />
                            <asp:BoundField DataField="linhaProduto" HeaderText="Linha" />
                            <asp:BoundField DataField="venda" HeaderText="Vendas" />
                            <asp:BoundField DataField="estoque" HeaderText="Estoque" />
                            <asp:BoundField DataField="reposicao" HeaderText="Reposicao" />
                            <asp:BoundField DataField="novo" HeaderText="Novo" />
                            <asp:BoundField DataField="mar" HeaderText="Mar" />
                            <asp:BoundField DataField="ern" HeaderText="E+R+N" />
                        </Columns>
	                    <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        </fieldset>
    </div>
</asp:Content>
