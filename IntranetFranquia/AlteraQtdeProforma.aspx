<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AlteraQtdeProforma.aspx.cs" Inherits="Relatorios.AlteraQtdeProforma" %>

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
        function SelectAllCheckboxes1(cbAlterado) 
        {
            $('#<%=GridViewProdutos.ClientID%>').find("input:checkbox").each(function () {
                if (this != cbAlterado) { this.checked = cbAlterado.checked; }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="accountInfo">
        <fieldset>
        </fieldset>
        <fieldset class="login">
            <legend>Busca de Produtos</legend>
            <div style="width: 600px;" class="alinhamento">
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
                <div style="width: 200px;"  class="alinhamento">
                    <label>Proforma:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlProforma" DataValueField="CODIGO_PROFORMA" DataTextField="DESCRICAO_PROFORMA" Height="26px" 
                        Width="200px" ondatabound="ddlProforma_DataBound"></asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btBuscarProdutos" Text="Buscar Produtos" OnClick="btBuscarProdutos_Click"/>
            <asp:ValidationSummary ID="ValidationSummaryProdutos" runat="server" ShowMessageBox="true" ShowSummary="false" />
        </div>
        <fieldset class="login">
        <table border="1" class="style1">
            <tr>
                <asp:GridView id="GridViewProdutos" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333" style="background:white" 
                    OnRowDataBound="GridViewProdutos_RowDataBound" DataKeyNames="CODIGO_PROFORMA_PRODUTO" >
	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="CODIGO_PROFORMA_PRODUTO" HeaderText="Código" Visible = "false"/>
                        <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Código" Visible = "false"/>
                        <asp:BoundField DataField="CODIGO_PRODUTO_COR" HeaderText="Código" Visible = "false"/>
                        <asp:BoundField DataField="CODIGO_FORNECEDOR" HeaderText="Código" Visible = "false"/>
                        <asp:BoundField DataField="CODIGO_PROFORMA" HeaderText="Código Proforma"  ItemStyle-HorizontalAlign="Center"/>
                        <asp:TemplateField HeaderText="Descrição" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralDescricao"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cor" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralCor"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fornecedor" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralFornecedor"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="QTDE_TOTAL" HeaderText="Qtde Total" ItemStyle-HorizontalAlign="Center"/>
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
                        <asp:TemplateField HeaderText="FOB">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtFob" AutoPostBack="false" Width="50"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Código Fornecedor">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtCodigoFornecedor" AutoPostBack="false" Width="70"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Armário">
                            <ItemTemplate>
                                <asp:DropDownList runat="server" ID="ddlArmario" DataValueField="CODIGO_ARMARIO" DataTextField="DESCRICAO"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pack Grade">
                            <ItemTemplate>
                                <asp:DropDownList runat="server" ID="ddlPackGrade" DataValueField="CODIGO_PACK_GRADE" DataTextField="DESCRICAO_TIPO"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pack Group">
                            <ItemTemplate>
                                <asp:DropDownList runat="server" ID="ddlPackGroup" DataValueField="CODIGO_PACK_GROUP" DataTextField="QTDE_TOTAL"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tipo Produto">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtTipoProduto" AutoPostBack="false" Width="100"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Alterado">
                            <HeaderTemplate>
                            <asp:CheckBox ID="cbSelectAll" runat="server" onclick="javascript:SelectAllCheckboxes1(this);"  />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="cbAlterado" AutoPostBack="false"  />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button runat="server" ID="ButtonDeletar" Text="Deletar" OnClick="ButtonDeletar_Click" OnClientClick="javascript: return confirm('Tem certeza que deseja excluir?');" />
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
