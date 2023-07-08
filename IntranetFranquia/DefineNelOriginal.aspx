<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DefineNelOriginal.aspx.cs" Inherits="Relatorios.DefineNelOriginal" %>

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
            <legend>Critérios de Busca de Nel</legend>
            <div style="width: 1200px;" class="alinhamento">
                <div style="width: 200px;"  class="alinhamento">
                    <label>Coleção:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlColecao" DataValueField="CODIGO_COLECAO" DataTextField="DESCRICAO" Height="26px" 
                        Width="200px" ondatabound="ddlColecao_DataBound"></asp:DropDownList>
                </div>
                <div style="width: 200px;"  class="alinhamento">
                    <label>Janela:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlJanela" DataValueField="CODIGO_JANELA" DataTextField="DESCRICAO" Height="26px" 
                        Width="200px" ondatabound="ddlJanela_DataBound"></asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btBuscarNel" Text="Buscar Nel" OnClick="btBuscarNel_Click"/>
            <asp:ValidationSummary ID="ValidationSummaryNel" runat="server" ShowMessageBox="true" ShowSummary="false" />
        </div>
        <fieldset class="login">
            <legend>Escolher NeL</legend>
            <div style="width: 200px;" class="alinhamento">
                <div style="width: 200px;"  class="alinhamento">
                    <label>NeL:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlNel" DataValueField="CODIGO_NEL" DataTextField="DESCRICAO_NEL" Height="26px" 
                        Width="200px" ondatabound="ddlNel_DataBound"></asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btBuscarProdutos" Text="Buscar Produtos" OnClick="btBuscarProdutos_Click" Enabled = "false"/>
            <asp:ValidationSummary ID="ValidationSummaryProdutos" runat="server" ShowMessageBox="true" ShowSummary="false" />
        </div>
        <fieldset class="login">
        <table border="1" class="style1">
            <tr>
                <asp:GridView id="GridViewNelProdutos" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333" style="background:white" onrowdatabound="GridViewNelProdutos_RowDataBound"> 
	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="CODIGO_NEL_PRODUTO" HeaderText="Código NeL Produto" />
                        <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Código do Produto" />
                        <asp:TemplateField HeaderText="Descrição">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralDescricaoProduto"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CODIGO_PRODUTO_COR" HeaderText="Código da Cor" />
                        <asp:TemplateField HeaderText="Cor">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralDescricaoProdutoCor"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="QTDE_TOTAL" HeaderText="Qtde Total" />
                        <asp:BoundField DataField="QTDE_XP" HeaderText="Qtde XP" />
                        <asp:BoundField DataField="QTDE_PP" HeaderText="Qtde PP" />
                        <asp:BoundField DataField="QTDE_PQ" HeaderText="Qtde PQ" />
                        <asp:BoundField DataField="QTDE_MD" HeaderText="Qtde MD" />
                        <asp:BoundField DataField="QTDE_GD" HeaderText="Qtde GD" />
                        <asp:BoundField DataField="QTDE_GG" HeaderText="Qtde GG" />
                    </Columns>
                </asp:GridView>
            </tr>
        </table>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btReplica" Text="Replicar NeL p/ Nel Original" OnClick="btReplica_Click" Enabled="False"/>
            <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
        </div>
    </div>
</asp:Content>
