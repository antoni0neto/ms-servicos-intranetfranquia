<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BuscaProdutoProforma.aspx.cs" Inherits="Relatorios.BuscaProdutoProforma" %>

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
            <legend>Critério de Busca</legend>
            <div style="width: 200px;" class="alinhamento">
                <div style="width: 200px;"  class="alinhamento">
                    <label>Coleção:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlColecao" DataValueField="CODIGO_COLECAO" DataTextField="DESCRICAO" Height="26px" 
                        Width="200px" ondatabound="ddlColecao_DataBound"></asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btBuscarProformaProdutos" Text="Buscar Produtos na Proforma" OnClick="btBuscarProformaProdutos_Click"/>
            <asp:ValidationSummary ID="ValidationSummaryProdutos" runat="server" ShowMessageBox="true" ShowSummary="false" />
        </div>
        <fieldset class="login">
        <table border="1" class="style1">
        <tr>
            <td>
                <asp:GridView id="GridViewProdutos" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333"
                                OnRowDataBound="GridViewProdutos_RowDataBound" style="background:white">
	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="CODIGO_PROFORMA" HeaderText="Proforma" Visible = "false"/>
                        <asp:BoundField DataField="CODIGO_JANELA" HeaderText="Janela" Visible = "false"/>
                        <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Produto" Visible = "false"/>
                        <asp:BoundField DataField="CODIGO_PRODUTO_COR" HeaderText="Cor" Visible = "false"/>
                        <asp:TemplateField HeaderText="Janela">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralProforma"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Janela">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralJanela"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Descrição">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralDescricao"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cor">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralCor"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="qtde_total" HeaderText="Qtde Total" />
                        <asp:BoundField DataField="qtde_xp" HeaderText="Qtde XP" />
                        <asp:BoundField DataField="qtde_pp" HeaderText="Qtde PP" />
                        <asp:BoundField DataField="qtde_pq" HeaderText="Qtde PQ" />
                        <asp:BoundField DataField="qtde_md" HeaderText="Qtde MD" />
                        <asp:BoundField DataField="qtde_gd" HeaderText="Qtde GD" />
                        <asp:BoundField DataField="qtde_gg" HeaderText="Qtde GG" />
                        <asp:BoundField DataField="fob" HeaderText="Fob" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        </table>
        </fieldset>
    </div>
</asp:Content>
