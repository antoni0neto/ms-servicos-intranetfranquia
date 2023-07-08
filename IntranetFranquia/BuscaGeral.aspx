<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BuscaGeral.aspx.cs" Inherits="Relatorios.BuscaGeral" %>

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
            <asp:Button runat="server" ID="btPesquisar" Text="Buscar Coleção" OnClick="btPesquisar_Click"/>
            <asp:ValidationSummary ID="ValidationSummaryColecao" runat="server" ShowMessageBox="true" ShowSummary="false" />
        </div>
        <fieldset class="login">
        <table border="1" class="style1">
        <tr>
            <td>
                <asp:GridView id="GridViewDraftFinal" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333"
                                OnRowDataBound="GridViewDraftFinal_RowDataBound" ondatabound="GridViewDraftFinal_DataBound" style="background:white">
	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="descricao" HeaderText="NeL-Draft Final" />
                        <asp:BoundField DataField="valor" HeaderText="Valor" />
                        <asp:BoundField DataField="qtde" HeaderText="Qtde" />
                    </Columns>
                </asp:GridView>
            </td>
            <td>
                <asp:GridView id="GridViewDraftOriginal" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333"
                                OnRowDataBound="GridViewDraftOriginal_RowDataBound" ondatabound="GridViewDraftOriginal_DataBound" style="background:white">
	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="descricao" HeaderText="NeL Original" />
                        <asp:BoundField DataField="valor" HeaderText="Valor" />
                        <asp:BoundField DataField="qtde" HeaderText="Qtde" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView id="GridViewProforma" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333"
                                OnRowDataBound="GridViewProforma_RowDataBound" ondatabound="GridViewProforma_DataBound" style="background:white">
	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="descricao" HeaderText="Proforma" />
                        <asp:BoundField DataField="data" HeaderText="Data Pagamento" />
                        <asp:BoundField DataField="valor" HeaderText="Valor" />
                    </Columns>
                </asp:GridView>
            </td>
            <td>
                <asp:GridView id="GridViewGriffe" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333"
                                OnRowDataBound="GridViewGriffe_RowDataBound" ondatabound="GridViewGriffe_DataBound" style="background:white">
	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="descricao" HeaderText="Griffe" />
                        <asp:BoundField DataField="valor" HeaderText="Valor" />
                        <asp:BoundField DataField="qtde" HeaderText="Qtde" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView id="GridViewGrupo" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333"
                                OnRowDataBound="GridViewGrupo_RowDataBound" ondatabound="GridViewGrupo_DataBound" style="background:white">
	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="descricao" HeaderText="Grupo" />
                        <asp:BoundField DataField="valor" HeaderText="Valor" />
                        <asp:BoundField DataField="qtde" HeaderText="Qtde" />
                    </Columns>
                </asp:GridView>
            </td>
            <td>
                <asp:GridView id="GridViewContainerOriginal" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333"
                                OnRowDataBound="GridViewContainerOriginal_RowDataBound" ondatabound="GridViewContainerOriginal_DataBound" style="background:white">
	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="descricao" HeaderText="Container" />
                        <asp:BoundField DataField="valor" HeaderText="Valor" />
                        <asp:BoundField DataField="qtde" HeaderText="Qtde" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button runat="server" ID="btBuscarNel" Text="Vêr NeL" OnClick="btBuscarNel_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        </table>
        </fieldset>
        <fieldset class="login">
        <table border="1" class="style1">
        <tr>
            <td>
                <asp:GridView id="GridViewNel" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333" Visible = "false"
                                OnRowDataBound="GridViewNel_RowDataBound" style="background:white">
	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="DESCRICAO_NEL" HeaderText="Descrição da Nel" />
                        <asp:BoundField DataField="DATA_PAGAMENTO" HeaderText="Data do Pagamento" />
                        <asp:BoundField DataField="VALOR_PAGAMENTO" HeaderText="Valor do Pagamento" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button runat="server" ID="btBuscarNelProdutos" Text="Vêr Produtos da NeL" OnClick="btBuscarNelProdutos_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        </table>
        </fieldset>
        <fieldset class="login">
        <table border="1" class="style1">
        <tr>
            <td>
                <asp:GridView id="GridViewNelProdutos" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333"
                                OnRowDataBound="GridViewNelProdutos_RowDataBound" style="background:white">
	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="CODIGO_NEL" HeaderText="NeL" Visible = "false"/>
                        <asp:BoundField DataField="CODIGO_JANELA" HeaderText="Janela" Visible = "false"/>
                        <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Produto" Visible = "false"/>
                        <asp:BoundField DataField="CODIGO_PRODUTO_COR" HeaderText="Cor" Visible = "false"/>
                        <asp:TemplateField HeaderText="Janela">
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
        </tr>
        </table>
        </fieldset>
    </div>
</asp:Content>
