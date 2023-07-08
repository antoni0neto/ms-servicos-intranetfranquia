<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UploadImagem.aspx.cs" Inherits="Relatorios.UploadImagem" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="accountInfo">
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
            <legend>Cadastro de Imagem</legend>
            <div>
                <label>Nome da Imagem: </label>
                <asp:TextBox ID="TextBoxNomeImagem" runat="server" CssClass="textEntry" MaxLength="50" Height="22px" Width="400px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorNome" runat="server" SetFocusOnError="true"
                    Display="None" ErrorMessage="Preencha o campo Nome da Imagem" ControlToValidate="TextBoxNomeImagem" ValidationGroup="imagem"></asp:RequiredFieldValidator>
            </div>
            <div>
                <br />
                <asp:FileUpload ID="FileUpload1" runat="server" Width="399px" onprerender="UploadButton_Click" />
            </div>
            <div>
                <label>Ativo: </label>
                <asp:CheckBox ID="CheckBoxAtivo" runat="server" />
            </div>
            <div>
                <asp:Button runat="server" ID="ButtonSalvar" Text="Salvar" OnClick="ButtonSalvar_Click" ValidationGroup="imagem" />
                <asp:ValidationSummary ID="ValidationSummaryImagem" runat="server" ValidationGroup="imagem" ShowMessageBox="true" ShowSummary="false" />
                <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
            </div>
        </fieldset>
    </div>
    <div>
        <asp:GridView runat="server" ID="GridViewImagem" AutoGenerateColumns="false" 
            onrowdatabound="GridViewImagem_RowDataBound">
            <Columns>
                <asp:BoundField DataField="NOME_IMAGEM" HeaderText="Nome" />
                <asp:BoundField DataField="LOCAL_IMAGEM" HeaderText="Local" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonEditar" Text="Editar" OnClick="ButtonEditar_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonExcluir" Text="Excluir" OnClick="ButtonExcluir_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <asp:HiddenField runat="server" ID="HiddenFieldCodigoimagem" Value="0" />
</asp:Content>
