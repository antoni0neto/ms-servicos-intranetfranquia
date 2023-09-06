<%@ Page Title="Responsável por Filial" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="fisc_cad_resp_filial.aspx.cs" Inherits="Relatorios.fisc_cad_resp_filial"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Cadastros&nbsp;&nbsp;>&nbsp;&nbsp;Responsável por Filial</span>
                <div style="float: right; padding: 0;">
                    <a href="fisc_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1200px; margin-left: 8%;">
                <fieldset>
                    <legend>Responsável por Filial</legend>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="tb">
                        <tr style="height: 10px; vertical-align: bottom;">
                            <td>
                                <asp:Label ID="labResponsavel" runat="server" Text="Responsável"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 300px;">
                                <asp:DropDownList ID="ddlResponsavel" runat="server" DataTextField="NOME_USUARIO" DataValueField="CODIGO_USUARIO" Width="294px"></asp:DropDownList>
                            </td>
                            <td style="width: 300px;">
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Width="294px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 600px;">
                                <asp:Button runat="server" ID="btIncluir" Text="Incluir" OnClick="btIncluir_Click"
                                    Width="100px" Enabled="true" /> &nbsp;
                                <asp:Label ID="labErro" runat="server" ForeColor="red" Text="">
                                </asp:Label>
                            </td>
                            <td>
                                <div style="float: right; margin-right: 0px;">
                                    &nbsp;
                                </div>
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td colspan="4"></td>
                        </tr>
                    </table>
                    <div style="width: 100%; overflow: auto; height: 380px;">
                        <div>
                            <table border="0" cellpadding="0" class="tb" width="100%">
                                <tr>
                                    <td style="width: 100%;">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvRespFilial" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvRespFilial_RowDataBound">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Responsável" HeaderStyle-Width="260px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litResponsavel" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Filial">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litFilial" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="65px">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btExcluir" runat="server" Height="19px" Text="Excluir" OnClientClick="return ConfirmarExclusao();"
                                                                OnClick="btExcluir_Click" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
