<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BuscaPorDescricaoProduto.aspx.cs" Inherits="Relatorios.BuscaPorDescricaoProduto" %>

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
            <div style="width: 200px;"  class="alinhamento">
                <label>Coleção:&nbsp; </label>
                <asp:DropDownList runat="server" ID="ddlColecao" DataValueField="CODIGO_COLECAO" DataTextField="DESCRICAO" Height="26px" 
                    Width="200px" ondatabound="ddlColecao_DataBound"></asp:DropDownList>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btPesquisar" Text="Buscar Coleção" OnClick="btPesquisar_Click"/>
            <asp:ValidationSummary ID="ValidationSummaryColecao" runat="server" ShowMessageBox="true" ShowSummary="false" />
        </div>
        <fieldset class="login">
            <legend>Critério de Busca</legend>
            <div style="width: 200px;" class="alinhamento">
                <div style="width: 200px;"  class="alinhamento">
                    <label>Produto:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlProdutoNelProforma" DataValueField="PRODUTO1" DataTextField="DESC_PRODUTO" Height="26px" 
                        Width="200px" ondatabound="ddlProdutoNelProforma_DataBound"></asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btProdutoNelProforma" Text="Buscar Produtos Nel/Proforma" OnClick="btProdutoNelProforma_Click" Enabled="false"/>
            <asp:ValidationSummary ID="ValidationSummaryNelProforma" runat="server" ShowMessageBox="true" ShowSummary="false" />
        </div>
        <fieldset class="login">
        <table border="1" class="style1">
        <tr>
            <td>
                <asp:GridView id="GridViewProdutoNel" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333"
                                OnRowDataBound="GridViewProdutoNel_RowDataBound" style="background:white">
	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="CODIGO_NEL" HeaderText="NeL" Visible = "false"/>
                        <asp:BoundField DataField="CODIGO_JANELA" HeaderText="Janela" Visible = "false"/>
                        <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Produto" Visible = "false"/>
                        <asp:BoundField DataField="CODIGO_PRODUTO_COR" HeaderText="Cor" Visible = "false"/>
                        <asp:TemplateField HeaderText="Nel">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralNel"/>
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
                        <asp:BoundField DataField="qtde_pack_a" HeaderText="Qtde Pack-A" />
                        <asp:BoundField DataField="qtde_pack_b" HeaderText="Qtde Pack-B" />
                        <asp:BoundField DataField="qtde_pack_c" HeaderText="Qtde Pack-C" />
                        <asp:BoundField DataField="fob" HeaderText="Fob" />
                    </Columns>
                </asp:GridView>
            </td>
            <td>
                <asp:GridView id="GridViewProdutoProforma" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333"
                                OnRowDataBound="GridViewProdutoProforma_RowDataBound" style="background:white">
	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="CODIGO_PROFORMA" HeaderText="Proforma" Visible = "false"/>
                        <asp:BoundField DataField="CODIGO_JANELA" HeaderText="Janela" Visible = "false"/>
                        <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Produto" Visible = "false"/>
                        <asp:BoundField DataField="CODIGO_PRODUTO_COR" HeaderText="Cor" Visible = "false"/>
                        <asp:TemplateField HeaderText="Proforma">
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
