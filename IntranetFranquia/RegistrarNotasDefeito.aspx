<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="RegistrarNotasDefeito.aspx.cs" Inherits="Relatorios.RegistrarNotasDefeito" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="accountInfo">
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
            <legend>Registro de Notas de Retirada</legend>
            <table class="style1">
                <tr>
                    <td>
                    <fieldset class="login">
                        <div>
                            <label>Número da Nota Cmax:</label>
                            <asp:TextBox ID="txtNotaCmax" runat="server" CssClass="textEntry" MaxLength="10" Height="22px" Width="100px"></asp:TextBox>
                        </div>
                        <div>
                            <label>Número da Nota Hbf:</label>
                            <asp:TextBox ID="txtNotaHbf" runat="server" CssClass="textEntry" MaxLength="10" Height="22px" Width="100px"></asp:TextBox>
                        </div>
                        <div>
                            <label>Número Nota Hbf Calçados:</label>
                            <asp:TextBox ID="txtNotaHbfCalcados" runat="server" CssClass="textEntry" MaxLength="10" Height="22px" Width="100px"></asp:TextBox>
                        </div>
                        <div>
                            <label>Número Nota Hbf Outros:</label>
                            <asp:TextBox ID="txtNotaHbfOutros" runat="server" CssClass="textEntry" MaxLength="10" Height="22px" Width="100px"></asp:TextBox>
                        </div>
                        <div>
                            <label>Número Nota Lugzi:</label>
                            <asp:TextBox ID="txtNotaLugzi" runat="server" CssClass="textEntry" MaxLength="10" Height="22px" Width="100px"></asp:TextBox>
                        </div>
                    </fieldset>
                    </td>
                </tr>
            </table>
            <div>
                <asp:Button runat="server" ID="ButtonSalvar" Text="Salvar" OnClick="ButtonSalvar_Click" ValidationGroup="produto"/>
                <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
            </div>
        </fieldset>
    </div>
</asp:Content>
