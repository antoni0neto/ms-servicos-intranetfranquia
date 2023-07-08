<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DesempenhoAgoraRede.aspx.cs" Inherits="Relatorios.DesempenhoAgoraRede" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinhamento {
            position: relative;
            float: left;
        }

        .style1 {
            width: 100%;
            background-color: White;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="accountInfo" style="border: 1px">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <div>
                        <asp:GridView runat="server" ID="GridViewAtingido" AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="GridViewAtingido_RowDataBound" OnDataBound="GridViewAtingido_DataBound" ForeColor="#333333">
                            <FooterStyle HorizontalAlign="Center" Font-Bold="true"></FooterStyle>
                            <RowStyle HorizontalAlign="Center"></RowStyle>
                            <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                            <Columns>
                                <asp:BoundField DataField="item" HeaderText="" HeaderStyle-Width="25px" />
                                <asp:BoundField DataField="filial" HeaderText="Filial" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderText="Vendas" HeaderStyle-Width="90px">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralVendas" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cotas" HeaderStyle-Width="90px">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralCota" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="atingido" HeaderText="Atingido" HeaderStyle-Width="90px" />
                                <asp:BoundField DataField="pa" HeaderText="PA" HeaderStyle-Width="90px" />
                                <asp:BoundField DataField="tickets" HeaderText="Tickets" HeaderStyle-Width="90px" />
                                <asp:TemplateField HeaderText="Ticket Médio" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralTicketMedio" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="hora" HeaderText="Última Venda" HeaderStyle-Width="100px" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
