<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="EscanearProdutoNotaTransfer.aspx.cs" Inherits="Relatorios.EscanearProdutoNotaTransfer" %>

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
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
            <legend>Cadastro de Produto</legend>
            <table class="style1">
                <tr>
                    <td>
                    <fieldset class="login">
                        <div>
                            <label>Descrição da Nota de Transferência:</label>
                            <asp:TextBox ID="txtDescricaoNota" runat="server" CssClass="textEntry" MaxLength="100" Height="22px" Width="150px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorDescricaoNota" runat="server" SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Descrição Nota" ControlToValidate="txtDescricaoNota" ValidationGroup="produto"></asp:RequiredFieldValidator>
                        </div>
                        <div>
                            <label>Código de Barra do Produto:</label>
                            <asp:TextBox ID="txtCodigoProduto" runat="server" CssClass="textEntry" MaxLength="11" Height="22px" Width="100px"></asp:TextBox>
                            <asp:Button runat="server" ID="btGravar" Text="" OnClick="btGravar_Click" ValidationGroup="produto"/>
                        </div>
                    </fieldset>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div>
        <asp:GridView runat="server" ID="GridViewProdutos" AutoGenerateColumns="false" OnRowDataBound="GridViewProdutos_RowDataBound">
            <Columns>
                <asp:BoundField DataField="INDICEPRODUTO" HeaderText="Item" />
                <asp:BoundField DataField="CODIGOBARRA" HeaderText="Código do Produto" />
                <asp:BoundField DataField="DESCRICAOPRODUTO" HeaderText="Descrição do Produto" />
                <asp:BoundField DataField="CORPRODUTO" HeaderText="Cor do Produto" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="btExcluirProduto" Text="Excluir" OnClick="btExcluirProduto_Click" OnClientClick="javascript: return confirm('Tem certeza que deseja excluir produto ?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:Button runat="server" ID="btSalvar" Text="Salvar" OnClick="btSalvar_Click"/>
            <asp:ValidationSummary ID="ValidationSummarySalvar" runat="server" ValidationGroup="produto" ShowMessageBox="true" ShowSummary="false" />
        <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
    </div>
</asp:Content>
