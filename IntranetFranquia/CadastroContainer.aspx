<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="CadastroContainer.aspx.cs" Inherits="Relatorios.CadastroContainer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="accountInfo">
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
            <legend>Cadastro de Container</legend>
            <div>
                <label>Código:&nbsp;&nbsp;&nbsp; </label>
                <asp:TextBox ID="txtCodigo" runat="server" CssClass="textEntry" MaxLength="10" Height="22px" Width="100px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCodigo" runat="server" SetFocusOnError="true"
                        Display="None" ErrorMessage="Preencha o código do Container" ControlToValidate="txtCodigo" ValidationGroup="container"></asp:RequiredFieldValidator>
            </div>
            <div>
                <label>Descrição:&nbsp; </label>
                <asp:TextBox ID="txtDescricao" runat="server" CssClass="textEntry" MaxLength="100" Height="22px" Width="200px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorDescricao" runat="server" SetFocusOnError="true"
                        Display="None" ErrorMessage="Preencha a descrição do container" ControlToValidate="txtDescricao" ValidationGroup="container"></asp:RequiredFieldValidator>
            </div>
            <div>
                <label>Coleção:&nbsp; </label>
                <asp:DropDownList runat="server" ID="ddlColecao" DataValueField="CODIGO_COLECAO" DataTextField="DESCRICAO" Height="22px" Width="230px" ondatabound="ddlColecao_DataBound"></asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorColecao" ControlToValidate="ddlColecao" ValidationGroup="container" InitialValue="0"></asp:RequiredFieldValidator>
            </div>
            <div>
                <label>Janela:&nbsp; </label>
                <asp:DropDownList runat="server" ID="ddlJanela" DataValueField="CODIGO_JANELA" DataTextField="DESCRICAO" Height="22px" Width="230px" ondatabound="ddlJanela_DataBound"></asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorJanela" ControlToValidate="ddlJanela" ValidationGroup="container" InitialValue="0"></asp:RequiredFieldValidator>
            </div>
            <p style="height: 13px">
                &nbsp;</p>
            <div>
                <asp:Button runat="server" ID="ButtonSalvar" Text="Salvar" OnClick="ButtonSalvar_Click" ValidationGroup="container"/>
                <asp:ValidationSummary ID="ValidationSummaryContainer" runat="server" ValidationGroup="container" ShowMessageBox="true" ShowSummary="false" />
                <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
            </div>
        </fieldset>
    </div>
    <div>
        <asp:GridView runat="server" ID="GridViewContainer" AutoGenerateColumns="false" onrowdatabound="GridViewContainer_RowDataBound">
            <Columns>
                <asp:BoundField DataField="CODIGO_CONTAINER" HeaderText="Código" />
                <asp:BoundField DataField="DESCRICAO" HeaderText="Descrição" />
                <asp:BoundField DataField="CODIGO_COLECAO" HeaderText="Coleção" />
                <asp:BoundField DataField="CODIGO_JANELA" HeaderText="Janela" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonEditar" Text="Editar" OnClick="ButtonEditarContainer_Click" causesvalidation = "false"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonExcluir" Text="Excluir" OnClick="ButtonExcluirContainer_Click" causesvalidation = "false"/>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <asp:HiddenField runat="server" ID="HiddenFieldCodigoContainer" Value="0" />
</asp:Content>
