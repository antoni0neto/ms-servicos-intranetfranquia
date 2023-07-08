<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="AlteraItemNotaDefeito.aspx.cs" Inherits="Relatorios.AlteraItemNotaDefeito" %>

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
        function SelectAllCheckboxes1(cbAlterado) {
            $('#<%=GridViewNotaRetiradaItem.ClientID%>').find("input:checkbox").each(function () {
                if (this != cbAlterado) { this.checked = cbAlterado.checked; }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="accountInfo">
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
            <legend>Alteração de Produtos</legend>
            <div>
                <asp:GridView runat="server" ID="GridViewNotaRetiradaItem" AutoGenerateColumns="false" ShowFooter="true" onrowdatabound="GridViewNotaRetiradaItem_RowDataBound" DataKeyNames="CODIGO_NOTA_RETIRADA_ITEM">
                    <Columns>
                        <asp:BoundField DataField="CODIGO_NOTA_RETIRADA_ITEM" HeaderText="Código" Visible = "false"/>
                        <asp:BoundField DataField="ITEM_PRODUTO" HeaderText="Item" />
                        <asp:TemplateField HeaderText="Código do Produto">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtCodigoProduto" AutoPostBack="false" Width="50"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cor do Produto">
                            <ItemTemplate>
                                <asp:DropDownList runat="server" ID="ddlCorProduto" DataValueField="COR_PRODUTO" DataTextField="DESC_COR_PRODUTO"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DESCRICAO_PRODUTO" HeaderText="Descrição do Produto" />
                        <asp:TemplateField HeaderText="Coleção">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralDescricaoColecao"></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" />
                        <asp:TemplateField HeaderText="Origem do Defeito">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralOrigemDefeito"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Defeito">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralDefeito"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DATA_LANCAMENTO" HeaderText="Data do Lançamento" />
                        <asp:TemplateField HeaderText="Destino">
                            <ItemTemplate>
                                <asp:DropDownList runat="server" ID="ddlDestino" DataValueField="CODIGO" DataTextField="DESCRICAO_DESTINO"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TIPO_NOTA" HeaderText="Tipo de Nota" />
                        <asp:BoundField DataField="DATA_DESTINO" HeaderText="Data do Destino" />
                        <asp:BoundField DataField="DATA_RETORNO" HeaderText="Data do Retorno" />
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
                                <asp:Button runat="server" ID="btExcluir" Text="Excluir" OnClick="btExcluir_Click" CausesValidation="false" OnClientClick="javascript: return confirm('Tem certeza que deseja excluir?');"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btGravar" Text="Gravar Produtos" OnClick="btGravar_Click"/>
            <asp:Label runat="server" ID="lblMensagem" ForeColor="Red"></asp:Label>
        </div>
    </div>
</asp:Content>
