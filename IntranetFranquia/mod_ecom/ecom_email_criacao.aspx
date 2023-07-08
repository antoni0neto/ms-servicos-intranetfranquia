<%@ Page Title="Criação de Emails MKT" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="ecom_email_criacao.aspx.cs" Inherits="Relatorios.ecom_email_criacao"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">

    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Emails&nbsp;&nbsp;>&nbsp;&nbsp;Criação de Emails MKT</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Criação de Emails MKT"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Título</td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:HiddenField ID="hidCodigoEmail" runat="server" Value="" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 350px;">
                            <asp:TextBox ID="txtTitulo" runat="server" Width="340px" MaxLength="50" />
                        </td>
                        <td>
                            <asp:Button ID="btSalvar" runat="server" Width="100px" Text="Incluir" OnClick="btSalvar_Click" />&nbsp;
                            <asp:Button ID="btCancelar" runat="server" Width="100px" Text="Cancelar" OnClick="btCancelar_Click" />&nbsp;
                        </td>
                        <td style="text-align: right;">
                            <asp:Button ID="btVisualizar" runat="server" Width="100px" Text="Visualizar" OnClick="btVisualizar_Click" />&nbsp;
                            <asp:Button ID="btGerarHTML" runat="server" Width="100px" Text="Gerar HTML" OnClick="btGerarHTML_Click" />&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Panel ID="pnlCorpo" runat="server" Visible="false">
                                <fieldset>
                                    <legend>Corpo do Email - Blocos</legend>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>Bloco</td>
                                            <td>Posição</td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style="width: 250px">
                                                <asp:DropDownList ID="ddlBloco" runat="server" Width="244px" Height="21px" DataTextField="NOME"
                                                    DataValueField="CODIGO">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 100px;">
                                                <asp:TextBox ID="txtPosicao" runat="server" Width="90px" MaxLength="10" Text="1" Enabled="false" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btAddBloco" runat="server" Text="Adicionar Bloco" Width="120px" OnClick="btAddBloco_Click" />
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">&nbsp;</td>
                                        </tr>

                                        <tr>
                                            <td colspan="4">
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvCorpoBloco" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvCorpoBloco_RowDataBound"
                                                        OnDataBound="gvCorpoBloco_DataBound" ShowFooter="true"
                                                        DataKeyNames="CODIGO">
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Ordem" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btUp" runat="server" ImageAlign="AbsMiddle" Width="13px" ImageUrl="~/Image/up.png" OnClick="btUp_Click" AlternateText=" " />&nbsp;
                                                                    <asp:ImageButton ID="btDown" runat="server" ImageAlign="AbsMiddle" Width="13px" ImageUrl="~/Image/down.png" OnClick="btDown_Click" AlternateText=" " />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Bloco" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litBloco" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Corpo" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0px">
                                                                <ItemTemplate>
                                                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td style="border-color: white;">
                                                                                <asp:Panel ID="pnlConteudo" runat="server" BackColor="White" Height="100%" BorderWidth="0" BorderColor="White"></asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="65px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btAtualizar" runat="server" Text="Atualizar" Width="65px" OnClick="btAtualizar_Click" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="65px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btAlterar" runat="server" Text="Alterar" Width="65px" OnClick="btAlterar_Click" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="65px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btExcluir" runat="server" Text="Excluir" Width="65px" OnClick="btExcluir_Click" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
