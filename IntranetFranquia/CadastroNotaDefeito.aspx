<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="CadastroNotaDefeito.aspx.cs" Inherits="Relatorios.CadastroNotaDefeito" %>

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
                                <label>Filial:</label>
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="22px" Width="200px" ondatabound="ddlFilial_DataBound"></asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorFilial" ControlToValidate="ddlFilial" InitialValue="0" ValidationGroup="produto" Display="None" ErrorMessage="Selecione a Filial">
                                </asp:RequiredFieldValidator>
                            </div>
                            <div>
                                <asp:Button runat="server" ID="btContinue" Text="Continue" OnClick="btContinue_Click"/>
                            </div>
                        </fieldset>
                    </td>
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
                            <div>
                                <label>Descrição:</label>
                                <asp:TextBox ID="txtDescricaoProduto" runat="server" CssClass="textEntry" MaxLength="50" Height="22px" Width="300px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorDescricaoProduto" runat="server" SetFocusOnError="true"
                                        Display="None" ErrorMessage="Preencha o campo Descrição do Produto" ControlToValidate="txtDescricaoProduto" ValidationGroup="produto"></asp:RequiredFieldValidator>
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
                        <div>
                            <label>Origem do Defeito:</label>
                            <asp:DropDownList runat="server" ID="ddlOrigemDefeito" DataValueField="CODIGO" DataTextField="DESCRICAO_ORIGEM_DEFEITO" Height="22px" Width="200px" ondatabound="ddlOrigemDefeito_DataBound"></asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorOrigemDefeito" ControlToValidate="ddlOrigemDefeito" InitialValue="0" ValidationGroup="produto" Display="None" ErrorMessage="Selecione a Origem do Defeito">
                            </asp:RequiredFieldValidator>
                        </div>
                        <div>
                            <label>Defeito:</label>
                            <asp:DropDownList runat="server" ID="ddlDefeito" DataValueField="CODIGO" DataTextField="DESCRICAO_DEFEITO" Height="22px" Width="200px" ondatabound="ddlDefeito_DataBound"></asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorDefeito" ControlToValidate="ddlDefeito" InitialValue="0" ValidationGroup="produto" Display="None" ErrorMessage="Selecione o Defeito">
                            </asp:RequiredFieldValidator>
                        </div>
                    </fieldset>
                    </td>
                </tr>
            </table>
            <div>
                <asp:Button runat="server" ID="ButtonSalvar" Text="Salvar" OnClick="ButtonSalvar_Click" ValidationGroup="produto" Enabled="False"/>
                <asp:ValidationSummary ID="ValidationSummaryProduto" runat="server" ValidationGroup="produto" ShowMessageBox="true" ShowSummary="false" />
                <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
            </div>
        </fieldset>
    </div>
    <div>
        <asp:GridView runat="server" ID="GridViewNotaRetiradaItem" AutoGenerateColumns="false" onrowdatabound="GridViewNotaRetiradaItem_RowDataBound">
            <Columns>
                <asp:BoundField DataField="ITEM_PRODUTO" HeaderText="Item" />
                <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Código do Produto" />
                <asp:BoundField DataField="COR_PRODUTO" HeaderText="Cor do Produto" />
                <asp:BoundField DataField="DESCRICAO_PRODUTO" HeaderText="Descrição do Produto" />
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
                <asp:BoundField DataField="CODIGO_DESTINO" HeaderText="Destino" />
                <asp:BoundField DataField="DATA_DESTINO" HeaderText="Data do Destino" />
                <asp:BoundField DataField="DATA_RETORNO" HeaderText="Data do Retorno" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonEditar" Text="Editar" OnClick="ButtonEditarNotaRetiradaItem_Click" CausesValidation="false" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonExcluir" Text="Excluir" OnClick="ButtonExcluirNotaRetiradaItem_Click" CausesValidation="false" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <asp:HiddenField runat="server" ID="HiddenFieldCodigoNotaRetiradaItem" Value="0" />
</asp:Content>
