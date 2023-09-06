<%@ Page Title="Cadastro de Usuários" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="adm_cad_usuario.aspx.cs" Inherits="Relatorios.adm_cad_usuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../js/js.js" type="text/javascript"></script>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo de Administração do Site&nbsp;&nbsp;>&nbsp;&nbsp;Usuários&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro
            de Usuários</span>
        <div style="float: right; padding: 0;">
            <a href="adm_menu.aspx" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset class="login">
            <legend>Cadastro de Usuários</legend>
            <div>
                <label>
                    Nome:&nbsp;&nbsp;&nbsp;
                </label>
                <asp:TextBox ID="txtNome" runat="server" MaxLength="50" Width="400px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorNomeFuncionario" runat="server"
                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Nome do Funcionario"
                    ControlToValidate="txtNome" ValidationGroup="funcionario"></asp:RequiredFieldValidator>
            </div>
            <div>
                <label>
                    Perfil:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </label>
                <asp:DropDownList runat="server" ID="ddlPerfil" DataValueField="CODIGO_PERFIL"
                    DataTextField="DESCRICAO_PERFIL" Height="22px" Width="230px">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorPerfil" runat="server" SetFocusOnError="true"
                    InitialValue="0" Display="None" ErrorMessage="Selecione um Perfil" ControlToValidate="ddlPerfil"
                    ValidationGroup="funcionario"></asp:RequiredFieldValidator>
            </div>
            <div>
                <label>
                    Usuario:&nbsp;
                </label>
                <asp:TextBox ID="txtUsuario" runat="server" MaxLength="25" Width="228px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorUsuario" runat="server" SetFocusOnError="true"
                    Display="None" ErrorMessage="Preencha o campo Usuario" ControlToValidate="txtUsuario"
                    ValidationGroup="funcionario"></asp:RequiredFieldValidator>
            </div>
            <div>
                <label>
                    Senha:&nbsp;&nbsp;&nbsp;
                </label>
                <asp:TextBox ID="txtSenha" runat="server" TextMode="Password" MaxLength="25"
                    Width="228px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorSenha" runat="server" SetFocusOnError="true"
                    Display="None" ErrorMessage="Preencha o campo Senha" ControlToValidate="txtSenha"
                    ValidationGroup="funcionario"></asp:RequiredFieldValidator>
            </div>
            <div>
                <label>
                    Email:&nbsp;
                </label>
                <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" Width="400px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorEmail" runat="server" SetFocusOnError="true"
                    Display="None" ErrorMessage="Preencha o campo Email" ControlToValidate="txtEmail"
                    ValidationGroup="funcionario"> </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmail" runat="server"
                    ErrorMessage="Email inválido" SetFocusOnError="True" ControlToValidate="txtEmail"
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="funcionario"
                    Display="None"> </asp:RegularExpressionValidator>
            </div>
            <p style="height: 10px">
                &nbsp;
            </p>
            <div>
                <asp:Button runat="server" ID="btSalvar" Text="Salvar" OnClick="btSalvar_Click" ValidationGroup="funcionario" Width="120px" />
                <asp:ValidationSummary ID="ValidationSummaryFuncionario" runat="server" ValidationGroup="funcionario" ShowMessageBox="true" ShowSummary="false" />
                <asp:Label runat="server" ID="labErro" ForeColor="Red"></asp:Label>
            </div>
            <br />
            <div>
                <div class="rounded_corners">
                    <asp:GridView runat="server" ID="gvUsuario" AutoGenerateColumns="false" Width="100%"
                        ForeColor="#333333" OnRowDataBound="gvUsuario_RowDataBound">
                        <HeaderStyle HorizontalAlign="Left" BackColor="Gainsboro" />
                        <FooterStyle BackColor="Gainsboro" />
                        <Columns>

                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="NOME_USUARIO" HeaderText="Nome" HeaderStyle-Width="320px" />
                            <asp:BoundField DataField="USUARIO1" HeaderText="Usuario" HeaderStyle-Width="200px" />
                            <asp:BoundField DataField="SENHA" HeaderText="Senha" Visible="false" />
                            <asp:TemplateField HeaderText="Perfil" HeaderStyle-Width="">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="litPerfil" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="EMAIL" HeaderText="Email" />
                            <asp:TemplateField HeaderStyle-Width="75px">
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="btEditar" Width="75px" Text="Editar" OnClick="btEditar_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </fieldset>
    </div>
    <asp:HiddenField runat="server" ID="hidCodigoUsuario" Value="0" />
</asp:Content>
