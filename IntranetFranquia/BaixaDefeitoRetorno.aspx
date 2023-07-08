<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BaixaDefeitoRetorno.aspx.cs" Inherits="Relatorios.BaixaDefeitoRetorno" %>

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
            <legend>Critérios de Busca</legend>
            <div style="width: 200px;" class="alinhamento">
                <div style="width: 200px;"  class="alinhamento">
                    <label>Griffe:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlGriffe" DataValueField="COD_GRIFFE" DataTextField="GRIFFE" Height="26px" Width="200px" ondatabound="ddlGriffe_DataBound"></asp:DropDownList>
                </div>
                <div style="width: 200px;"  class="alinhamento">
                    <label>Grupo:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlGrupo" DataValueField="CODIGO_GRUPO" DataTextField="GRUPO_PRODUTO" Height="26px" Width="200px" ondatabound="ddlGrupo_DataBound"></asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btDefeitoRetorno" Text="Buscar Defeito Retorno" OnClick="btDefeitoRetorno_Click"/>
        </div>
        <fieldset class="login">
        <table border="1" class="style1">
            <tr>
                <asp:GridView id="GridViewProdutos" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333" style="background:white" 
                    OnRowDataBound="GridViewProdutos_RowDataBound">
	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="DESTINO"/>
                        <asp:TemplateField HeaderText="Destino">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralDestino"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="GRIFFE" HeaderText="Griffe"/>
                        <asp:BoundField DataField="GRUPO" HeaderText="Grupo"/>
                        <asp:BoundField DataField="PRODUTO"/>
                        <asp:TemplateField HeaderText="Produto">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralProduto"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="COR"/>
                        <asp:TemplateField HeaderText="Cor">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralCor"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TAMANHO"/>
                        <asp:TemplateField HeaderText="Tamanho">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralTamanho"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TOTAL" HeaderText="Qtde Total"/>
                        <asp:TemplateField HeaderText="Qtde Baixa">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtQtdeBaixa" AutoPostBack="false" Width="50"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Baixado">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralBaixado"/>
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
                    </Columns>
                </asp:GridView>
            </tr>
        </table>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btBaixar" Text="Baixar Produtos" OnClick="btBaixar_Click"/>
            <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
        </div>
    </div>
</asp:Content>
