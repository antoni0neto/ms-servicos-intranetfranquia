<%@ Page Title="Intranet" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="Relatorios.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .login {
            height: 130px;
            width: 325px;
        }
    </style>
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
    <h2>Login
    </h2>
    <p>
        Por Favor, entre com seu usuário e senha.
    </p>
    <div>
        <span class="failureNotification">
            <asp:Literal ID="FailureText" runat="server"></asp:Literal>
        </span>
        <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification"
            ValidationGroup="LoginUserValidationGroup" />
        <div class="accountInfo">
            <fieldset class="login">
                <legend>Informações da conta</legend>
                <div>
                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Usuário</asp:Label>
                    <asp:TextBox ID="UserName" runat="server" CssClass="textEntry" Height="22px" Width="226px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                        CssClass="failureNotification" ErrorMessage="Informe o seu Usuário." ToolTip="Informe o seu Usuário."
                        ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Senha</asp:Label>
                    <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"
                        Height="22px" Width="226px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                        CssClass="failureNotification" ErrorMessage="Informe sua Senha." ToolTip="Informe sua Senha."
                        ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                </div>
            </fieldset>
            <div>
                <asp:Button ID="LoginButton" runat="server" Text="Entrar" Width="100px" ValidationGroup="LoginUserValidationGroup"
                    OnClick="LoginButton_Click" />
                &nbsp;
                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text="" />
            </div>
            <p class="submitButton">
                &nbsp;
            </p>
        </div>
    </div>
</asp:Content>
