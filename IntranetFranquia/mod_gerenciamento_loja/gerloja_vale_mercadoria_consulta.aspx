<%@ Page Title="Consulta Preço Funcionário" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" 
    CodeBehind="gerloja_vale_mercadoria_consulta.aspx.cs" Inherits="Relatorios.gerloja_vale_mercadoria_consulta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function doClick(buttonName, e) {
            //the purpose of this function is to allow the enter key to 
            //point to the correct button to click.
            var key;

            if (window.event)
                key = window.event.keyCode;     //IE
            else
                key = e.which;     //firefox

            if (key == 13) {
                //Get the button the user wants to have clicked
                var btn = document.getElementById(buttonName);
                if (btn != null) { //If we find the button click it
                    btn.click();
                    event.keyCode = 0
                }
            }
        }
    </script>
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="accountInfo">
        <br />
        <div>
            <span style="font-family: Calibri; font-size: 14px;">Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Vendedores&nbsp;&nbsp;>&nbsp;&nbsp;Consulta Preço Funcionário</span>
            <div style="float: right; padding: 0;">
                <a href="gerloja_menu.aspx" class="alink" title="Voltar">Voltar</a>
            </div>
        </div>
        <hr />
        <br />
        <fieldset class="login">
            <legend>Consulta Preço Funcionário</legend>
            <table class="style1">
                <tr>
                    <td>
                    <fieldset class="login">
                        <div>
                            <label>Código:</label>
                            <asp:TextBox ID="txtCodigoProduto" runat="server" CssClass="textEntry" MaxLength="5" Height="22px" Width="100px"></asp:TextBox>
                            <asp:Button runat="server" ID="btGravar" Text="" OnClick="btGravar_Click" ValidationGroup="produto"/>
                        </div>
                        <div>
                            <label>Total Desconto:</label>
                            <asp:TextBox ID="txtTotalDesconto" runat="server" CssClass="pcRight" MaxLength="20" Height="16px" Width="100px" Enabled="False" textAlign = "rigth"></asp:TextBox>
                        </div>
                    </fieldset>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div>
        <asp:GridView runat="server" ID="GridViewProduto" AutoGenerateColumns="false" OnRowDataBound="GridViewProduto_RowDataBound">
            <Columns>
                <asp:BoundField DataField="INDICEPRODUTO" HeaderText="Item" />
                <asp:BoundField DataField="CODIGOBARRA" HeaderText="Código do Produto" />
                <asp:BoundField DataField="DESCRICAOPRODUTO" HeaderText="Descrição do Produto" />
                <asp:BoundField DataField="GRADEPRODUTO" HeaderText="Grade" />
                <asp:BoundField DataField="PRECOORIGINAL" HeaderText="Preço Original" />
                <asp:BoundField DataField="DESCONTO" HeaderText="Desconto 40%" />
                <asp:BoundField DataField="PRECOLIQUIDO" HeaderText="Preço Líquido" />
                <asp:BoundField DataField="PRECOLOJA" HeaderText="Preço Loja" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonDeletar" Text="Deletar" OnClick="ButtonDeletar_Click" OnClientClick="javascript: return confirm('Tem certeza que deseja excluir?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:Button runat="server" ID="btSomar" Text="Somar" OnClick="btSomar_Click"/>
        <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
    </div>
</asp:Content>
