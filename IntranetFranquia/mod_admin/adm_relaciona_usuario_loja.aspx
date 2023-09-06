<%@ Page Title="Relacionamento de Loja x Usuário" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="adm_relaciona_usuario_loja.aspx.cs" Inherits="Relatorios.adm_relaciona_usuario_loja" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo de Administração do Site&nbsp;&nbsp;>&nbsp;&nbsp;Usuários&nbsp;&nbsp;>&nbsp;&nbsp;Relacionamento de Loja x Usuário</span>
        <div style="float: right; padding: 0;">
            <a href="adm_menu.aspx" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset class="login">
            <legend>Relacionamento de Loja x Usuário</legend>
            <div>
                <label>Loja&nbsp;&nbsp; </label>
                <asp:DropDownList runat="server" ID="DropDownListLoja" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                    Height="22px" Width="230px" OnDataBound="DropDownListLoja_DataBound">
                </asp:DropDownList>
                <label>Usuário </label>
                <asp:DropDownList runat="server" ID="DropDownListUsuario" DataValueField="CODIGO_USUARIO" DataTextField="NOME_USUARIO"
                    Height="22px" Width="230px" OnDataBound="DropDownListUsuario_DataBound">
                </asp:DropDownList>
            </div>
            <p style="height: 13px">
                &nbsp;
            </p>
            <div>
                <asp:Button runat="server" ID="ButtonSalvar" Text="Salvar" OnClick="ButtonSalvar_Click" ValidationGroup="loja" Width="120px" />
                <asp:ValidationSummary ID="ValidationSummaryLoja" runat="server" ValidationGroup="loja" ShowMessageBox="true" ShowSummary="false" />
                <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
            </div>
        </fieldset>
    </div>
    <div>
        <div class="rounded_corners">
            <asp:GridView runat="server" ID="GridViewLoja" AutoGenerateColumns="false" Width="100%"
                ForeColor="#333333" OnRowDataBound="GridViewLoja_RowDataBound">
                <HeaderStyle HorizontalAlign="Left" BackColor="Gainsboro" />
                <FooterStyle BackColor="Gainsboro" />
                <Columns>
                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Usuário">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralUsuario" Text=""></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Filial">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralFilial"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Button runat="server" ID="ButtonEditar" Text="Editar" Width="100px" OnClick="ButtonEditar_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Button runat="server" ID="ButtonExcluir" Text="Excluir" Width="100px" OnClick="ButtonExcluir_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="HiddenFieldCodigoUsuarioLoja" Value="0" />
</asp:Content>
