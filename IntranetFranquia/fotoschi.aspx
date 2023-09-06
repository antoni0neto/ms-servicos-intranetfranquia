<%@ Page Title="Fotos" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="fotoschi.aspx.cs" Inherits="Relatorios.fotoschi" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Fotos</span>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Fotos</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td colspan="2">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound" ShowFooter="true">
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>
                                                    <asp:Image ID="imgProduto" runat="server" Width="160px" Height="240px" ImageAlign="AbsMiddle" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Referência" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRef" runat="server" Width="500px" OnTextChanged="txtRef_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
