<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="admfis_reg_nota.aspx.cs" Inherits="Relatorios.admfis_reg_nota" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo ADM Nota Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Administrativo Nota Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Registro de Notas de Retirada</span>
        <div style="float: right; padding: 0;">
            <a href="admfis_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset class="login">
            <legend>Registro de Notas de Retirada</legend>
            <table border="0" class="style1" cellpadding="0" cellspacing="0">
                <tr>
                    <td>Número Nota da Fiscal
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtNotaLugzi" runat="server" CssClass="textEntry" MaxLength="10" Width="160px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Button runat="server" ID="btSalvar" Text="Salvar" Width="120px" OnClick="btSalvar_Click" />
                        <asp:Label runat="server" ID="labErro" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>

        </fieldset>
    </div>
</asp:Content>
