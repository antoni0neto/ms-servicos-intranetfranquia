<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DefaultRh.aspx.cs" Inherits="Relatorios.DefaultRh" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .login
        {
            height: 180px;
            width: 280px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="login">
        <fieldset class="login">
            <legend>Recursos Humanos</legend>
            <asp:Menu ID="Menu6" runat="server" StaticDisplayLevels="3">
                <Items>
                    <asp:MenuItem NavigateUrl="~/Candidato.aspx" Text="1. Cadastro de Candidatos" Value="B_1"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/AgendaAvaliacao.aspx" Text="2. Agendamento de Avaliação" Value="B_2"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/Experiencia.aspx" Text="3. FeedBack de Experiência de Candidato" Value="B_3"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/Documentos.aspx" Text="4. Registro de Documentos do Candidato" Value="B_4"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/Demissao.aspx" Text="5. Registro de Dados p/ Desligamento" Value="B_5"></asp:MenuItem>
                    <asp:MenuItem NavigateUrl="~/FuncionariosLoja.aspx" Text="6. Turnover de Funcionários" Value="B_6"></asp:MenuItem>
                </Items>
            </asp:Menu>
        </fieldset>
    </div>
</asp:Content>

