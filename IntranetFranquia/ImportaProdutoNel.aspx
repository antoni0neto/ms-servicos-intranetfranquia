<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportaProdutoNel.aspx.cs" Inherits="Relatorios.ImportaProdutoNel" %>

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
    <script type="text/javascript" src="jquery-1.6.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('input').keypress(function (e) {
                var code = null;
                code = (e.keyCode ? e.keyCode : e.which);
                return (code == 13) ? false : true;
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="accountInfo">
        <fieldset>
        </fieldset>
        <fieldset class="login">
            <legend>Busca Froformas</legend>
            <div style="width: 200px;"  class="alinhamento">
                <label>Coleção:&nbsp; </label>
                <asp:DropDownList runat="server" ID="ddlColecao" DataValueField="CODIGO_COLECAO" DataTextField="DESCRICAO" Height="26px" 
                    Width="200px" ondatabound="ddlColecao_DataBound"></asp:DropDownList>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btProforma" Text="Buscar Proformas" OnClick="btProforma_Click"/>
        </div>
        <fieldset class="login">
            <legend>Critérios de Busca de Produtos</legend>
            <div style="width: 600px;" class="alinhamento">
                <div style="width: 200px;"  class="alinhamento">
                    <label>Janela:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlJanela" DataValueField="CODIGO_JANELA" DataTextField="DESCRICAO" Height="26px" 
                        Width="200px" ondatabound="ddlJanela_DataBound"></asp:DropDownList>
                </div>
                <div style="width: 200px;"  class="alinhamento">
                    <label>Proforma:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlProforma" DataValueField="CODIGO_PROFORMA" DataTextField="DESCRICAO_PROFORMA" Height="26px" 
                        Width="200px" ondatabound="ddlProforma_DataBound"></asp:DropDownList>
                </div>
                <div style="width: 200px;"  class="alinhamento">
                    <label>Fornecedor:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlFornecedor" DataValueField="CODIGO_FORNECEDOR" DataTextField="DESCRICAO" Height="26px" 
                        Width="200px" ondatabound="ddlFornecedor_DataBound"></asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btBuscarNel" Text="Buscar Nel" OnClick="btBuscarNel_Click" Enabled="false"/>
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
                <asp:GridView id="GridViewProdutos" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333" style="background:white" onrowdatabound="GridViewProdutos_RowDataBound"> 
	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="CODIGO_PROFORMA" HeaderText="Código Proforma"/>
                        <asp:BoundField DataField="CODIGO_GRIFFE" HeaderText="Código da Griffe"/>
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
                        <asp:BoundField DataField="GRUPO_PRODUTO" HeaderText="Grupo" />
                        <asp:BoundField DataField="FOB" HeaderText="Fob" />
                        <asp:TemplateField HeaderText="Importa ?">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="cbImporta" AutoPostBack="false"  />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </tr>
        </table>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btImportar" Text="Vincular Produtos a NeL" OnClick="btImportar_Click" Enabled="False"/>
            <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
        </div>
    </div>
</asp:Content>
