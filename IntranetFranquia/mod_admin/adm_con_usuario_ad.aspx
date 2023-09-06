<%@ Page Title="Consulta Usuário AD" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="adm_con_usuario_ad.aspx.cs" Inherits="Relatorios.adm_con_usuario_ad"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita
        {
            text-align: right;
        }
    </style>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Administração do Site&nbsp;&nbsp;>&nbsp;&nbsp;Usuários&nbsp;&nbsp;>&nbsp;&nbsp;Consulta
                    Usuário AD</span>
                <div style="float: right; padding: 0;">
                    <a href="adm_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Usuário AD"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            Usuário
                        </td>
                        <td>
                            Nome
                        </td>
                        <td>
                            Setor
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 160px;">
                            <asp:TextBox ID="txtUsuario" runat="server" Width="150px"></asp:TextBox>
                        </td>
                        <td style="width: 160px;">
                            <asp:TextBox ID="txtNome" runat="server" Width="150px"></asp:TextBox>
                        </td>
                        <td style="width: 160px;">
                            <asp:TextBox ID="txtSetor" runat="server" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                            <asp:HiddenField ID="hidTela" runat="server" />
                        </td>
                        <td style="text-align: right;">
                            <asp:Button ID="btExcelAD" runat="server" Text="Excel" Width="100px" OnClick="btExcelAD_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvUsuario" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvUsuario_RowDataBound">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="USUARIO" HeaderText="Usuário" HeaderStyle-Width="210px" />
                                        <asp:BoundField DataField="NOME" HeaderText="Nome" HeaderStyle-Width="250px" />
                                        <asp:BoundField DataField="SETOR" HeaderText="Setor" HeaderStyle-Width="210px" />
                                        <asp:BoundField DataField="EMAIL" HeaderText="E-Mail" HeaderStyle-Width="230px" />
                                        <asp:BoundField DataField="MEMBRO_DE" HeaderText="Membro De" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btExcelAD" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
