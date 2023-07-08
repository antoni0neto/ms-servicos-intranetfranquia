<%@ Page Title="Os Mais Vendidos" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="MaisVendidos.aspx.cs" Inherits="Relatorios.MaisVendidos" %>

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
                    <label>Filial:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="22px" 
                        Width="198px" ondatabound="ddlFilial_DataBound"></asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="ButtonPesquisarMovimento" Text="Buscar os 30 mais" OnClick="ButtonPesquisarMovimento_Click" ValidationGroup="entrada"/>
            <asp:ValidationSummary ID="ValidationSummaryEntrada" runat="server" ValidationGroup="entrada" ShowMessageBox="true" ShowSummary="false" />
            <asp:Label runat="server" ID="lblMensagemEntrada" ForeColor="Red"></asp:Label>
        </div>
        <fieldset class="login">
        <asp:Repeater id="Repeater1" runat="server" onitemdatabound="Repeater1_ItemDataBound">
          <ItemTemplate>
            <table border="1" style="font: 6.5pt verdana">
             <tr bgcolor="#33cc99">
                <td> 
                    <table>                    
                        <tr>
                            <asp:Image runat="server" ID="ImageProduto_1" Width="280" Height="200"/>
                        </tr>
                        <tr>
                            <td>
                                <label>Código:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralCodigoProduto_1"/>
                            </td>
                            <td>
                                <label>Descrição:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralDescricaoProduto_1"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Cor:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralDescricaoCor_1"/>
                            </td>
                            <td>
                                <label>Venda Rede:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralQtdeVendidaRede_1"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Venda Loja:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralQtdeVendida_1"/>
                            </td>
                            <td>
                                <label>Estoque Loja:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralQtdeEstoque_1"/>
                            </td>
                        </tr>
                    </table>                    
                </td>
                <td>
                    <table>                    
                        <tr>
                            <asp:Image runat="server" ID="ImageProduto_2" Width="280" Height="200"/>
                        </tr>
                        <tr>
                            <td>
                                <label>Código:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralCodigoProduto_2"/>
                            </td>
                            <td>
                                <label>Descrição:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralDescricaoProduto_2"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Cor:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralDescricaoCor_2"/>
                            </td>
                            <td>
                                <label>Venda Rede:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralQtdeVendidaRede_2"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Venda Loja:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralQtdeVendida_2"/>
                            </td>
                            <td>
                                <label>Estoque Loja:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralQtdeEstoque_2"/>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table>                    
                        <tr>
                            <asp:Image runat="server" ID="ImageProduto_3" Width="280" Height="200"/>
                        </tr>
                        <tr>
                            <td>
                                <label>Código:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralCodigoProduto_3"/>
                            </td>
                            <td>
                                <label>Descrição:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralDescricaoProduto_3"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Cor:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralDescricaoCor_3"/>
                            </td>
                            <td>
                                <label>Venda Rede:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralQtdeVendidaRede_3"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Venda Loja:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralQtdeVendida_3"/>
                            </td>
                            <td>
                                <label>Estoque Loja:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralQtdeEstoque_3"/>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table>                    
                        <tr>
                            <asp:Image runat="server" ID="ImageProduto_4" Width="280" Height="200"/>
                        </tr>
                        <tr>
                            <td>
                                <label>Código:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralCodigoProduto_4"/>
                            </td>
                            <td>
                                <label>Descrição:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralDescricaoProduto_4"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Cor:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralDescricaoCor_4"/>
                            </td>
                            <td>
                                <label>Venda Rede:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralQtdeVendidaRede_4"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Venda Loja:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralQtdeVendida_4"/>
                            </td>
                            <td>
                                <label>Estoque Loja:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralQtdeEstoque_4"/>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table>                    
                        <tr>
                            <asp:Image runat="server" ID="ImageProduto_5" Width="280" Height="200"/>
                        </tr>
                        <tr>
                            <td>
                                <label>Código:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralCodigoProduto_5"/>
                            </td>
                            <td>
                                <label>Descrição:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralDescricaoProduto_5"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Cor:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralDescricaoCor_5"/>
                            </td>
                            <td>
                                <label>Venda Rede:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralQtdeVendidaRede_5"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Venda Loja:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralQtdeVendida_5"/>
                            </td>
                            <td>
                                <label>Estoque Loja:</label>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="LiteralQtdeEstoque_5"/>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            </table>
          </ItemTemplate>
        </asp:Repeater>
        </fieldset>
        <fieldset class="login">
            <legend><asp:Label runat="server" text="Os mais vendidos"></asp:Label></legend>
            <table border="1" class="style1">
                <tr>
                    <td>
                        <asp:GridView id="GridViewMaisVendidos" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333" style="background:white" ondatabound="GridViewMaisVendidos_DataBound">
	                        <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                        <RowStyle HorizontalAlign="Center"></RowStyle>
	                        <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                            <Columns>
                                <asp:BoundField DataField="PRODUTO" HeaderText="Produto" />
                                <asp:BoundField DataField="DESCRICAO" HeaderText="Descrição" />
                                <asp:BoundField DataField="COR" HeaderText="Cor"/>
                                <asp:TemplateField HeaderText="Descrição">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralCor"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="QTDE" HeaderText="Qtde" />
                            </Columns>
	                        <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
