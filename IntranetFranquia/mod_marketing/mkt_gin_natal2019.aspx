<%@ Page Title="Gincana de Natal 2019" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="mkt_gin_natal2019.aspx.cs" Inherits="Relatorios.mkt_gin_natal2019" %>

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
                <span style="font-family: Calibri; font-size: 14px;">Gincana de Natal 2019</span>
                <div style="float: right; padding: 0;">
                    &nbsp;
                </div>
            </div>
            <hr />
            <div class="login">
                <table border="0" class="table">
                    <tr>
                        <td>
                            <fieldset style="padding-top: 0;">
                                <legend>Pontuação</legend>
                                <div style="text-align:right; width:100%;">
                                    <asp:Label ID="labRegraPA" runat="server" Text="*PA - Não entra venda de doces" ForeColor="Red" Font-Size="Smaller"></asp:Label>
                                </div>
                                <br />
                                <div id="accordionP">
                                    <h3>
                                        <asp:Label ID="labTitulo" runat="server" Text="Início da Pontuação - 01/10/2019"></asp:Label>
                                    </h3>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvPrincipal" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvPrincipal_RowDataBound"
                                                            OnDataBound="gvPrincipal_DataBound" ShowFooter="true">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="FILIAL" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Filial" HeaderStyle-Width="" />
                                                                <asp:BoundField DataField="COTA_PONTO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Cota" HeaderStyle-Width="220px" />
                                                                <asp:BoundField DataField="PA_PONTO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="PA" HeaderStyle-Width="220px" />
                                                                <asp:BoundField DataField="MASC_PONTO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Masculino" HeaderStyle-Width="220px" />
                                                                <asp:BoundField DataField="PONTOS" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Pontos da Semana" HeaderStyle-Width="160px" />
                                                                <asp:BoundField DataField="PONTOS_ACUM" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Pontos Acumulados" HeaderStyle-Width="160px" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <br />
                            </fieldset>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
