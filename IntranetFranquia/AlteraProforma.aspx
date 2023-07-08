<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="AlteraProforma.aspx.cs" Inherits="Relatorios.AlteraProforma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="accountInfo">
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
            <legend>Cadastro de Usuários</legend>
            <div>
                <label>Nome:&nbsp;&nbsp;&nbsp; </label>
                <asp:TextBox ID="TextBoxNomeFuncionario" runat="server" CssClass="textEntry" MaxLength="50" Height="22px" Width="400px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorNomeFuncionario" runat="server" SetFocusOnError="true"
                        Display="None" ErrorMessage="Preencha o campo Nome do Funcionario" ControlToValidate="TextBoxNomeFuncionario" ValidationGroup="funcionario"></asp:RequiredFieldValidator>
            </div>
            <div>
                <label>Perfil:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </label>
                <asp:DropDownList runat="server" ID="DropDownListPerfil" DataValueField="CODIGO_PERFIL" DataTextField="DESCRICAO_PERFIL" Height="22px" 
                    Width="230px" ondatabound="DropDownListPerfil_DataBound"></asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorPerfil" ControlToValidate="DropDownListPerfil" InitialValue="0" ValidationGroup="funcionario"
                    ErrorMessage="Selecione um Perfil">
                </asp:RequiredFieldValidator>
            </div>
            <div>
                <label>Usuario:&nbsp; </label>
                <asp:TextBox ID="TextBoxUsuario" runat="server" CssClass="textEntry" 
                    MaxLength="25" Height="22px" Width="150px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorUsuario" runat="server" SetFocusOnError="true"
                        Display="None" ErrorMessage="Preencha o campo Usuario" ControlToValidate="TextBoxUsuario" ValidationGroup="funcionario"></asp:RequiredFieldValidator>
            </div>
            <div>
                <label>Senha:&nbsp;&nbsp;&nbsp; </label>
                <asp:TextBox ID="TextBoxSenha" runat="server" CssClass="passwordEntry" 
                    TextMode="Password" MaxLength="25" Height="22px" Width="150px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorSenha" runat="server" SetFocusOnError="true"
                        Display="None" ErrorMessage="Preencha o campo Senha" ControlToValidate="TextBoxSenha" ValidationGroup="funcionario"></asp:RequiredFieldValidator>
            </div>
            <div>
                <label>Email:&nbsp; </label>
                <asp:TextBox ID="TextBoxEmail" runat="server" CssClass="textEntry"  MaxLength="50" Height="22px" Width="400px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorEmail" runat="server" SetFocusOnError="true" Display="None"
                    ErrorMessage="Preencha o campo Email" ControlToValidate="TextBoxEmail" ValidationGroup="funcionario"> </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmail" runat="server" ErrorMessage="Email inválido"
                    SetFocusOnError="True" ControlToValidate="TextBoxEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                    ValidationGroup="funcionario" Display="None"> </asp:RegularExpressionValidator>
            </div>
            <p style="height: 13px">
                &nbsp;</p>
            <div>
                <asp:Button runat="server" ID="ButtonSalvar" Text="Salvar" OnClick="ButtonSalvar_Click" ValidationGroup="funcionario"/>
                <asp:ValidationSummary ID="ValidationSummaryFuncionario" runat="server" ValidationGroup="funcionario" ShowMessageBox="true" ShowSummary="false" />
                <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
            </div>
        </fieldset>
    </div>
    <div>
        <asp:GridView runat="server" ID="GridViewUsuario" AutoGenerateColumns="false" 
            onrowdatabound="GridViewUsuario_RowDataBound">
            <Columns>
                <asp:BoundField DataField="NOME_USUARIO" HeaderText="Nome" />
                <asp:BoundField DataField="USUARIO1" HeaderText="Usuario" />
                <asp:BoundField DataField="SENHA" HeaderText="Senha" />
                <asp:TemplateField HeaderText="Perfil">
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="LiteralPerfil"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="EMAIL" HeaderText="Email" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonEditar" Text="Editar" OnClick="ButtonEditarUsuario_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonExcluir" Text="Excluir" OnClick="ButtonExcluirUsuario_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <asp:HiddenField runat="server" ID="HiddenFieldCodigofuncionario" Value="0" />
</asp:Content>
