<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DesempenhoAgoraSuper.aspx.cs" Inherits="Relatorios.DesempenhoAgoraSuper" %>

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
    <div class="accountInfo">
        <%--<fieldset>
        </fieldset>--%>
        <fieldset class="login">
            <legend>Agora Super</legend>
            <table border="0" class="style1" cellpadding="1" cellspacing="1">
                <tr>
                    <td valign="top">
                        <fieldset class="login" style="height: 400px;">
                            <legend>Dennis</legend>
                            <div class="rounded_corners">
                                <asp:GridView runat="server" ID="GridViewAtingidoDennis" AutoGenerateColumns="false" ShowFooter="true"
                                    OnRowDataBound="GridViewAtingidoDennis_RowDataBound" OnDataBound="GridViewAtingidoDennis_DataBound" ForeColor="#333333">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Size="Smaller"></HeaderStyle>
                                    <RowStyle Font-Size="Smaller" />
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" Font-Size="Smaller" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="filial" HeaderText="Filial" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="Vendas" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="LiteralVendas" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cotas" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="LiteralCota" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="atingido" HeaderText="Atingido" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="pa" HeaderText="PA" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="tickets" HeaderText="Tickets" ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Ticket Médio" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="LiteralTicketMedio" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="hora" HeaderText="Ultima Venda" ItemStyle-HorizontalAlign="Left" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                    </td>
                    <td valign="top">
                        <fieldset class="login" style="height: 400px;">
                            <legend>Claudia</legend>
                            <div class="rounded_corners">
                                <asp:GridView runat="server" ID="GridViewAtingidoClaudia" AutoGenerateColumns="false" ShowFooter="true"
                                    OnRowDataBound="GridViewAtingidoClaudia_RowDataBound" OnDataBound="GridViewAtingidoClaudia_DataBound" ForeColor="#333333">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Size="Smaller"></HeaderStyle>
                                    <RowStyle Font-Size="Smaller" />
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" Font-Size="Smaller" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="filial" HeaderText="Filial" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="Vendas" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="LiteralVendas" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cotas" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="LiteralCota" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="atingido" HeaderText="Atingido" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="pa" HeaderText="PA" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="tickets" HeaderText="Tickets" ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Ticket Médio" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="LiteralTicketMedio" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="hora" HeaderText="Ultima Venda" ItemStyle-HorizontalAlign="Left" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                    </td>
                    <td valign="top">
                        <fieldset class="login" style="height: 400px;">
                            <legend>Marcielly</legend>
                            <div class="rounded_corners">
                                <asp:GridView runat="server" ID="GridViewAtingidoMarcielly" AutoGenerateColumns="false" ShowFooter="true"
                                    OnRowDataBound="GridViewAtingidoMarcielly_RowDataBound" OnDataBound="GridViewAtingidoMarcielly_DataBound" ForeColor="#333333">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Size="Smaller"></HeaderStyle>
                                    <RowStyle Font-Size="Smaller" />
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" Font-Size="Smaller" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="filial" HeaderText="Filial" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="Vendas" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="LiteralVendas" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cotas" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="LiteralCota" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="atingido" HeaderText="Atingido" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="pa" HeaderText="PA" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="tickets" HeaderText="Tickets" ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Ticket Médio" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="LiteralTicketMedio" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="hora" HeaderText="Ultima Venda" ItemStyle-HorizontalAlign="Left" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" valign="top">
                        <fieldset class="login">
                            <legend>Rede</legend>
                            <div class="rounded_corners">
                                <asp:GridView runat="server" ID="GridViewAtingidoRede" AutoGenerateColumns="false" ShowFooter="true"
                                    OnRowDataBound="GridViewAtingidoRede_RowDataBound" ForeColor="#333333" Width="100%">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Size="Smaller"></HeaderStyle>
                                    <RowStyle Font-Size="Smaller" />
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:BoundField DataField="filial" HeaderText="" ItemStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField HeaderText="Vendas" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="LiteralVendas" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cotas" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="LiteralCota" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="atingido" HeaderText="Atingido" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="pa" HeaderText="PA" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="tickets" HeaderText="Tickets" ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Ticket Médio" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="LiteralTicketMedio" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="hora" HeaderText="Ultima Venda" ItemStyle-HorizontalAlign="Left" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
