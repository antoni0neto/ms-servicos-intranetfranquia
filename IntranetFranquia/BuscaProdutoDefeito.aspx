<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="BuscaProdutoDefeito.aspx.cs" Inherits="Relatorios.BuscaProdutoDefeito" %>

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
                            <label>Código:</label>
                            <asp:TextBox ID="txtCodigoProduto" runat="server" CssClass="textEntry" MaxLength="10" Height="22px" Width="100px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorCodigoProduto" runat="server" SetFocusOnError="true"
                                    Display="None" ErrorMessage="Preencha o campo Código de Produto" ControlToValidate="txtCodigoProduto" ValidationGroup="codigo"></asp:RequiredFieldValidator>
                            <asp:Button runat="server" ID="btBuscar" Text="Buscar" OnClick="btBuscar_Click" ValidationGroup="codigo"/>
                            <asp:ValidationSummary ID="ValidationSummaryCodigo" runat="server" ValidationGroup="codigo" ShowMessageBox="true" ShowSummary="false" />
                        </div>
                    </fieldset>
                    </td>
                    <td>
                    <fieldset class="login">
                        <div>
                            <label>Cor:</label>
                            <asp:DropDownList runat="server" ID="ddlProdutoCor" DataValueField="COR_PRODUTO" DataTextField="DESC_COR_PRODUTO" Height="22px" Width="200px" ondatabound="ddlProdutoCor_DataBound"></asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorProdutoCor" ControlToValidate="ddlProdutoCor" InitialValue="0" ValidationGroup="produto" Display="None" ErrorMessage="Selecione uma Cor">
                            </asp:RequiredFieldValidator>
                        </div>
                        <div>
                            <label>Tamanho:</label>
                            <asp:DropDownList runat="server" ID="ddlTamanho" DataValueField="CODIGO" DataTextField="DESCRICAO" Height="22px" Width="200px"></asp:DropDownList>
                        </div>
                    </fieldset>
                    </td>
                </tr>
            </table>
            <div>
                <asp:Button runat="server" ID="btPesquisar" Text="Pesquisar" OnClick="btPesquisar_Click" ValidationGroup="produto"/>
                <asp:ValidationSummary ID="ValidationSummaryProduto" runat="server" ValidationGroup="produto" ShowMessageBox="true" ShowSummary="false" />
                <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
            </div>
        </fieldset>
    </div>
    <div>
        <asp:GridView runat="server" ID="GridViewNotaRetiradaItem" AutoGenerateColumns="false" onrowdatabound="GridViewNotaRetiradaItem_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Filial">
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="LiteralFilial"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ITEM_PRODUTO" HeaderText="Item" />
                <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Código do Produto" />
                <asp:BoundField DataField="COR_PRODUTO" HeaderText="Cor do Produto" />
                <asp:BoundField DataField="DESCRICAO_PRODUTO" HeaderText="Descrição do Produto" />
                <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" />
                <asp:BoundField DataField="DATA_LANCAMENTO" HeaderText="Data do Lançamento" />
                <asp:BoundField DataField="CODIGO_DESTINO" HeaderText="Destino" />
                <asp:BoundField DataField="DATA_DESTINO" HeaderText="Data do Destino" />
                <asp:BoundField DataField="DATA_RETORNO" HeaderText="Data do Retorno" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
