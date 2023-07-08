<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="AlteraNFDefeitoRetorno.aspx.cs" Inherits="Relatorios.AlteraNFDefeitoRetorno" %>

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
            <legend>Critérios de Busca de Produtos</legend>
            <div style="width: 200px;" class="alinhamento">
                <div style="width: 200px;"  class="alinhamento">
                    <label>Defeito:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlDefeito" DataValueField="CODIGO" DataTextField="DESCRICAO_DEFEITO" Height="26px" Width="200px" ondatabound="ddlDefeito_DataBound"></asp:DropDownList>
                </div>
                <div style="width: 200px;"  class="alinhamento">
                    <label>Destino:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlDestino" DataValueField="CODIGO" DataTextField="DESCRICAO_DESTINO" Height="26px" Width="200px" ondatabound="ddlDestino_DataBound"></asp:DropDownList>
                </div>
            </div>
            <div style="width: 400px;" class="alinhamento">
                <div style="width: 200px;" class="alinhamento">
                    <label>Data:&nbsp; </label>
                    <asp:TextBox ID="txtData" runat="server" CssClass="textEntry" Height="22px" Width="198px"></asp:TextBox>
                    <asp:Calendar ID="CalendarData" runat="server" OnSelectionChanged="CalendarData_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                </div>
                <div style="width: 200px;" class="alinhamento">
                    <label>Datas Destino Definidas:&nbsp; </label>
                    <asp:TextBox ID="txtDatasDestino" runat="server" CssClass="textEntry" Height="22px" Width="198px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDatasDestino" runat="server" CaptionAlign="Bottom"></asp:Calendar>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btBuscarProdutos" Text="Buscar Produtos" OnClick="btBuscarProdutos_Click"/>
        </div>
        <fieldset class="login">
            <legend>Alteração de Produtos - Retorno</legend>
            <div>
                <asp:GridView runat="server" ID="GridViewNotaRetiradaItem" AutoGenerateColumns="false" ShowFooter="true" onrowdatabound="GridViewNotaRetiradaItem_RowDataBound" DataKeyNames="CODIGO_NOTA_RETIRADA_ITEM">
                    <Columns>
                        <asp:BoundField DataField="CODIGO_NOTA_RETIRADA_ITEM" HeaderText="Código" Visible = "false"/>
                        <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Código do Produto" />
                        <asp:BoundField DataField="COR_PRODUTO" HeaderText="Cor do Produto" />
                        <asp:BoundField DataField="DESCRICAO_PRODUTO" HeaderText="Descrição do Produto" />
                        <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" />
                        <asp:BoundField DataField="ORIGEM_DEFEITO" HeaderText="Origem do Defeito" />
                        <asp:BoundField DataField="DEFEITO" HeaderText="Defeito" />
                        <asp:BoundField DataField="DATA_LANCAMENTO" HeaderText="Data do Lançamento" />
                        <asp:BoundField DataField="DESTINO" HeaderText="Destino" />
                        <asp:BoundField DataField="DATA_DESTINO" HeaderText="Data do Destino" />
                        <asp:BoundField DataField="TIPO_NOTA" HeaderText="Tipo de Nota" />
                        <asp:BoundField DataField="NUMERO_NOTA" HeaderText="Número da Nota" />
                        <asp:BoundField DataField="DATA_RETORNO" HeaderText="Data do Retorno" />
                        <asp:TemplateField HeaderText="Alterado">
                            <HeaderTemplate>
                            <asp:CheckBox ID="cbSelectAll" runat="server" onclick="javascript:SelectAllCheckboxes1(this);"  />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="cbAlterado" AutoPostBack="false"  />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Item">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralItem"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btGravar" Text="Gravar Retorno" OnClick="btGravar_Click" Enabled="false"/>
            <asp:Label runat="server" ID="lblMensagem" ForeColor="Red"></asp:Label>
        </div>
    </div>
</asp:Content>
