<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="CadastroProforma.aspx.cs" Inherits="Relatorios.CadastroProforma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
            <legend>Cadastro de Proforma</legend>
            <div>
                <label>Código:&nbsp;&nbsp;&nbsp; </label>
                <asp:TextBox ID="txtCodigo" runat="server" CssClass="textEntry" MaxLength="10" Height="22px" Width="100px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCodigo" runat="server" SetFocusOnError="true"
                        Display="None" ErrorMessage="Preencha o código da proforma" ControlToValidate="txtCodigo" ValidationGroup="proforma"></asp:RequiredFieldValidator>
            </div>
            <div>
                <label>Descrição:&nbsp; </label>
                <asp:TextBox ID="txtDescricao" runat="server" CssClass="textEntry" MaxLength="100" Height="22px" Width="200px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorDescricao" runat="server" SetFocusOnError="true"
                        Display="None" ErrorMessage="Preencha a descrição da proforma" ControlToValidate="txtDescricao" ValidationGroup="proforma"></asp:RequiredFieldValidator>
            </div>
            <div style="width: 200px;" class="alinhamento">
                <label>Data do Pagamento:&nbsp; </label>
                <asp:TextBox ID="txtDataPagamento" runat="server" CssClass="textEntry" Height="22px" Width="198px"></asp:TextBox>
                <asp:Calendar ID="CalendarDataPagamento" runat="server" OnSelectionChanged="CalendarDataPagamento_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorDataPagamento" runat="server" SetFocusOnError="true" Display="None" ErrorMessage="Escolha Data do Pagamento" ControlToValidate="txtDataPagamento"></asp:RequiredFieldValidator>
            </div>
            <div>
                <label>Valor do Pagamento:&nbsp; </label>
                <asp:TextBox ID="txtValorPagamento" runat="server" CssClass="textEntry" MaxLength="20" Height="22px" Width="50px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorValorPagamento" runat="server" SetFocusOnError="true"
                        Display="None" ErrorMessage="Preencha o valor do pagamento" ControlToValidate="txtValorPagamento"></asp:RequiredFieldValidator>
            </div>
            <div>
                <label>Coleção:&nbsp; </label>
                <asp:DropDownList runat="server" ID="ddlColecao" DataValueField="CODIGO_COLECAO" DataTextField="DESCRICAO" Height="22px" Width="230px" ondatabound="ddlColecao_DataBound"></asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorColecao" ControlToValidate="ddlColecao" InitialValue="0"></asp:RequiredFieldValidator>
            </div>
            <div>
                <label>Janela:&nbsp; </label>
                <asp:DropDownList runat="server" ID="ddlJanela" DataValueField="CODIGO_JANELA" DataTextField="DESCRICAO" Height="22px" Width="230px" ondatabound="ddlJanela_DataBound"></asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorJanela" ControlToValidate="ddlJanela" InitialValue="0"></asp:RequiredFieldValidator>
            </div>
            <p style="height: 13px">
                &nbsp;</p>
            <div>
                <asp:Button runat="server" ID="ButtonSalvar" Text="Salvar" OnClick="ButtonSalvar_Click" ValidationGroup="proforma"/>
                <asp:ValidationSummary ID="ValidationSummaryProforma" runat="server" ValidationGroup="proforma" ShowMessageBox="true" ShowSummary="false" />
                <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
            </div>
        </fieldset>
    </div>
    <div>
        <asp:GridView runat="server" ID="GridViewProforma" AutoGenerateColumns="false" onrowdatabound="GridViewProforma_RowDataBound">
            <Columns>
                <asp:BoundField DataField="CODIGO_PROFORMA" HeaderText="Código" />
                <asp:BoundField DataField="DESCRICAO_PROFORMA" HeaderText="Descrição" />
                <asp:BoundField DataField="DATA_PAGAMENTO" HeaderText="Data do Pagamento" />
                <asp:BoundField DataField="VALOR_PAGAMENTO" HeaderText="Valor do Pagamento" />
                <asp:BoundField DataField="CODIGO_COLECAO" HeaderText="Coleção" />
                <asp:BoundField DataField="CODIGO_JANELA" HeaderText="Janela" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonEditar" Text="Editar" OnClick="ButtonEditarProforma_Click" CausesValidation="false"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonExcluir" Text="Excluir" OnClick="ButtonExcluirProforma_Click" CausesValidation="false"/>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <asp:HiddenField runat="server" ID="HiddenFieldCodigoProforma" Value="0" />
</asp:Content>
