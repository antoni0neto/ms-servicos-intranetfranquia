<%@ Page Title="Transferência de Fotos para Lookbook" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ecom_cad_foto_lookbook_transfer_old.aspx.cs" Inherits="Relatorios.ecom_cad_foto_lookbook_transfer_old" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }
    </style>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro&nbsp;&nbsp;>&nbsp;&nbsp;Transferência de Fotos para Lookbook</span>
        <div style="float: right; padding: 0;">
            <a href="ecom_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div>
        <fieldset class="login">
            <legend>Transferência de Fotos para Lookbook</legend>
            <div class="login">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button runat="server" ID="btGerarFoto" Text="Transferir Fotos" Width="150px" OnClick="btGerarFoto_Click" />
                            &nbsp;<asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="1">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="1">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvFotoLookbook" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white;" OnRowDataBound="gvFotoLookbook_RowDataBound"
                                    ShowFooter="true" DataKeyNames="CODIGO">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="labColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Foto" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Image ID="imgFoto" runat="server" Width="100px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nome Foto" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litNomeFoto" runat="server" Text='<%# Bind("NOME_FOTO")%>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </fieldset>
    </div>
</asp:Content>
