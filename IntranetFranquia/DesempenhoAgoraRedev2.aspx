<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DesempenhoAgoraRedev2.aspx.cs" Inherits="Relatorios.DesempenhoAgoraRedev2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div>
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView runat="server" ID="gvAtingido" AutoGenerateColumns="false" ShowFooter="true"
                        OnRowDataBound="gvAtingido_RowDataBound" OnDataBound="gvAtingido_DataBound" ForeColor="#333333">
                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                        <RowStyle HorizontalAlign="Center" />
                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FILIAL" HeaderText="Filial" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                            <asp:TemplateField HeaderText="Vendas" HeaderStyle-Width="75px">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="litVendas" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cotas" HeaderStyle-Width="75px">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="litCotas" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="% Atingido" HeaderStyle-Width="75px">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="litAtingido" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PA" HeaderStyle-Width="75px">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="litPA" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tickets" HeaderStyle-Width="75px">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="litTickets" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ticket Médio" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="litTicketMedio" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Última Venda" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="litUltimaVenda" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>&nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
