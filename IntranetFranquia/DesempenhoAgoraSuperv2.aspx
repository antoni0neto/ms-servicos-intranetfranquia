<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DesempenhoAgoraSuperv2.aspx.cs" Inherits="Relatorios.DesempenhoAgoraSuperv2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td>
                <fieldset>
                    <legend>Agora Super</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <fieldset>
                                    <legend>Dirceu de Lima</legend>
                                    <div class="rounded_corners">
                                        <asp:GridView runat="server" ID="gvDirceu" AutoGenerateColumns="false" ShowFooter="true" Width="100%"
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
                                                <asp:TemplateField HeaderText="Vendas" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litVendas" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cotas" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litCotas" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="% Atingido" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litAtingido" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PA" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litPA" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tickets" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litTickets" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ticket Médio" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litTicketMedio" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Última Venda" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litUltimaVenda" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <fieldset>
                                    <legend>Diego</legend>
                                    <div class="rounded_corners">
                                        <asp:GridView runat="server" ID="gvDiego" AutoGenerateColumns="false" ShowFooter="true" Width="100%"
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
                                                <asp:TemplateField HeaderText="Vendas" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litVendas" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cotas" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litCotas" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="% Atingido" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litAtingido" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PA" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litPA" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tickets" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litTickets" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ticket Médio" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litTicketMedio" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Última Venda" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litUltimaVenda" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <fieldset>
                                    <legend>Edna Costa</legend>
                                    <div class="rounded_corners">
                                        <asp:GridView runat="server" ID="gvEdna" AutoGenerateColumns="false" ShowFooter="true" Width="100%"
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
                                                <asp:TemplateField HeaderText="Vendas" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litVendas" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cotas" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litCotas" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="% Atingido" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litAtingido" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PA" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litPA" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tickets" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litTickets" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ticket Médio" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litTicketMedio" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Última Venda" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litUltimaVenda" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <fieldset>
                                    <legend>Maurilio</legend>
                                    <div class="rounded_corners">
                                        <asp:GridView runat="server" ID="gvMaurilio" AutoGenerateColumns="false" ShowFooter="true" Width="100%"
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
                                                <asp:TemplateField HeaderText="Vendas" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litVendas" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cotas" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litCotas" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="% Atingido" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litAtingido" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PA" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litPA" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tickets" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litTickets" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ticket Médio" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litTicketMedio" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Última Venda" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litUltimaVenda" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <fieldset>
                                    <legend>Grabriel Zeitunlian</legend>
                                    <div class="rounded_corners">
                                        <asp:GridView runat="server" ID="gvGabriel" AutoGenerateColumns="false" ShowFooter="true" Width="100%"
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
                                                <asp:TemplateField HeaderText="Vendas" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litVendas" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cotas" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litCotas" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="% Atingido" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litAtingido" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PA" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litPA" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tickets" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litTickets" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ticket Médio" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litTicketMedio" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Última Venda" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litUltimaVenda" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <fieldset>
                                    <legend>Rede</legend>
                                    <div class="rounded_corners">
                                        <asp:GridView runat="server" ID="gvRede" AutoGenerateColumns="false" ShowFooter="true" Width="100%"
                                            OnRowDataBound="gvAtingido_RowDataBound" ForeColor="#333333">
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
                                                <asp:TemplateField HeaderText="Vendas" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litVendas" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cotas" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litCotas" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="% Atingido" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litAtingido" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PA" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litPA" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tickets" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litTickets" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ticket Médio" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litTicketMedio" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Última Venda" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="litUltimaVenda" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
    </table>
</asp:Content>
