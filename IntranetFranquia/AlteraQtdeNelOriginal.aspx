<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AlteraQtdeNelOriginal.aspx.cs" Inherits="Relatorios.AlteraQtdeNelOriginal" %>

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
            <div style="width: 200px;" class="alinhamento">
                <div style="width: 200px;"  class="alinhamento">
                    <label>Coleção:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlColecao" DataValueField="CODIGO_COLECAO" DataTextField="DESCRICAO" Height="26px" Width="200px" ondatabound="ddlColecao_DataBound"></asp:DropDownList>
                </div>
                <div style="width: 200px;"  class="alinhamento">
                    <label>Janela:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlJanela" DataValueField="CODIGO_JANELA" DataTextField="DESCRICAO" Height="26px" Width="200px" ondatabound="ddlJanela_DataBound"></asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btBuscarNel" Text="Buscar Nel" OnClick="btBuscarNel_Click"/>
            <asp:ValidationSummary ID="ValidationSummaryNel" runat="server" ShowMessageBox="true" ShowSummary="false" />
        </div>
        <fieldset class="login">
            <legend>Escolher Nel</legend>
            <div style="width: 200px;" class="alinhamento">
                <div style="width: 200px;"  class="alinhamento">
                    <label>Nel:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlNel" DataValueField="CODIGO_NEL" DataTextField="DESCRICAO_NEL" Height="26px" Width="200px" ondatabound="ddlNel_DataBound"></asp:DropDownList>
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
                <asp:GridView id="GridViewProdutos" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333" style="background:white" 
                    OnRowDataBound="GridViewProdutos_RowDataBound" DataKeyNames="CODIGO_NEL_PRODUTO">
	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="CODIGO_NEL_PRODUTO" HeaderText="Produto" Visible = "false"/>
                        <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Produto" Visible = "false"/>
                        <asp:BoundField DataField="CODIGO_PRODUTO_COR" HeaderText="Cor" Visible = "false"/>
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
                        <asp:BoundField DataField="QTDE_TOTAL_ORIGINAL" HeaderText="Qtde Total"/>
                        <asp:TemplateField HeaderText="Qtde XP">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtQtdeXp" AutoPostBack="false" Width="50"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qtde PP">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtQtdePp" AutoPostBack="false" Width="50"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qtde PQ">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtQtdePq" AutoPostBack="false" Width="50"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qtde MD">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtQtdeMd" AutoPostBack="false" Width="50"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qtde GD">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtQtdeGd" AutoPostBack="false" Width="50"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qtde GG">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtQtdeGg" AutoPostBack="false" Width="50"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Alterado">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="cbAlterado" AutoPostBack="false"  />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </tr>
        </table>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btGravar" Text="Gravar Produtos" OnClick="btGravar_Click" Enabled="False"/>
            <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
        </div>
    </div>
</asp:Content>
