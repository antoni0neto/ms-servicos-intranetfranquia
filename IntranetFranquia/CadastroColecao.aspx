<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="CadastroColecao.aspx.cs" Inherits="Relatorios.CadastroColecao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="accountInfo">
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
            <legend>Cadastro de Coleção</legend>
            <div>
                <label>Código:&nbsp;&nbsp;&nbsp; </label>
                <asp:TextBox ID="txtCodigo" runat="server" CssClass="textEntry" MaxLength="10" Height="22px" Width="100px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCodigo" runat="server" SetFocusOnError="true"
                        Display="None" ErrorMessage="Preencha o código da coleção" ControlToValidate="txtCodigo" ValidationGroup="colecao"></asp:RequiredFieldValidator>
            </div>
            <div>
                <label>Descrição:&nbsp; </label>
                <asp:TextBox ID="txtDescricao" runat="server" CssClass="textEntry" MaxLength="100" Height="22px" Width="200px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorDescricao" runat="server" SetFocusOnError="true"
                        Display="None" ErrorMessage="Preencha a descrição da coleção" ControlToValidate="txtDescricao" ValidationGroup="colecao"></asp:RequiredFieldValidator>
            </div>
            <div>
                <label>Taxa:&nbsp; </label>
                <asp:TextBox ID="txtTaxa" runat="server" CssClass="textEntry" MaxLength="2" Height="22px" Width="10px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorTaxa" runat="server" SetFocusOnError="true"
                        Display="None" ErrorMessage="Preencha a taxa da coleção" ControlToValidate="txtTaxa" ValidationGroup="colecao"></asp:RequiredFieldValidator>
            </div>
            <p style="height: 13px">
                &nbsp;</p>
            <div>
                <asp:Button runat="server" ID="ButtonSalvar" Text="Salvar" OnClick="ButtonSalvar_Click" ValidationGroup="colecao"/>
                <asp:ValidationSummary ID="ValidationSummaryColecao" runat="server" ValidationGroup="colecao" ShowMessageBox="true" ShowSummary="false" />
                <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
            </div>
        </fieldset>
    </div>
    <div>
        <asp:GridView runat="server" ID="GridViewColecao" AutoGenerateColumns="false" onrowdatabound="GridViewColecao_RowDataBound">
            <Columns>
                <asp:BoundField DataField="CODIGO_COLECAO" HeaderText="Código" />
                <asp:BoundField DataField="DESCRICAO" HeaderText="Descrição" />
                <asp:BoundField DataField="TAXA" HeaderText="Taxa" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonEditar" Text="Editar" OnClick="ButtonEditarColecao_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonExcluir" Text="Excluir" OnClick="ButtonExcluirColecao_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <asp:HiddenField runat="server" ID="HiddenFieldCodigoColecao" Value="0" />
</asp:Content>
