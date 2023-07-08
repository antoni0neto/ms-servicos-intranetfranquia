<%@ Page Title="RFV – Recência, Frequência e Valor" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="crm_rfv.aspx.cs" Inherits="Relatorios.crm_rfv"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
    </style>
    <script type="text/javascript" src="../js/js.js"></script>
    <script type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de CRM&nbsp;&nbsp;>&nbsp;&nbsp;Clientes&nbsp;&nbsp;>&nbsp;&nbsp;RFV – Recência, Frequência e Valor</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="RFV – Recência, Frequência e Valor"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Trimestre</td>
                        <td colspan="6">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 190px;">
                            <asp:DropDownList ID="ddlTri" runat="server" Width="184px" DataValueField="DATA" DataTextField="TRI">
                            </asp:DropDownList>
                        </td>
                        <td colspan="6">
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />&nbsp;&nbsp;<asp:Label ID="labErro" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">&nbsp;</td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td style="width: 233px;">
                            <asp:Label ID="labTri1" runat="server" Text="" ForeColor="Gray" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="width: 233px;">
                            <asp:Label ID="labTri2" runat="server" Text="" ForeColor="Gray" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="width: 233px;">
                            <asp:Label ID="labTri3" runat="server" Text="" ForeColor="Gray" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="width: 233px;">
                            <asp:Label ID="labTri4" runat="server" Text="" ForeColor="Gray" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="width: 172px;">
                            <asp:Label ID="labTri5" runat="server" Text="" ForeColor="Gray" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvRFV" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvRFV_RowDataBound" OnDataBound="gvRFV_DataBound">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Faixa" ItemStyle-Width="100px" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litFaixa" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Descrição" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litDescr" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="" ItemStyle-BackColor="LightGray" ItemStyle-Width="2px" />
                                        <%--1 TRI ATRAS--%>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Qtde Cliente" ItemStyle-Width="110px" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeCliente1" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Qtde Cliente (%)" ItemStyle-Width="110px" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeCliente1Porc" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="" ItemStyle-BackColor="LightGray" ItemStyle-Width="2px" />
                                        <%--2 TRI ATRAS--%>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Qtde Cliente" ItemStyle-Width="110px" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeCliente2" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Qtde Cliente (%)" ItemStyle-Width="110px" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeCliente2Porc" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="" ItemStyle-BackColor="LightGray" ItemStyle-Width="2px" />
                                        <%--3 TRI ATRAS--%>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Qtde Cliente" ItemStyle-Width="110px" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeCliente3" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Qtde Cliente (%)" ItemStyle-Width="110px" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeCliente3Porc" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="" ItemStyle-BackColor="LightGray" ItemStyle-Width="2px" />
                                        <%--4 TRI ATRAS--%>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Qtde Cliente" ItemStyle-Width="110px" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeCliente4" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Qtde Cliente (%)" ItemStyle-Width="110px" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeCliente4Porc" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="" ItemStyle-BackColor="LightGray" ItemStyle-Width="2px" />
                                        <%--5 TRI ATRAS--%>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Qtde Cliente" ItemStyle-Width="110px" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeCliente5" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Qtde Cliente (%)" ItemStyle-Width="110px" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeCliente5Porc" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="top" style="width: 33%;">
                                        <p><span style="font-weight: bold;">Recência / Data da Última Compra</span></p>
                                        <br />
                                        <p>1 - 13 A 24 meses</p>
                                        <p>2 - 7 A 12 meses</p>
                                        <p>3 - 1 A 6 meses</p>
                                    </td>
                                    <td valign="top" style="width: 33%;">
                                        <p><span style="font-weight: bold;">Frequência / Total de tickets no período</span></p>
                                        <br />
                                        <p>1 - 1 A 3 Tickets</p>
                                        <p>2 - 4 A 6 Tickets</p>
                                        <p>3 - > 6 Tickets</p>
                                    </td>
                                    <td valign="top" style="width: 34%;">
                                        <p><span style="font-weight: bold;">Valor / Total (R$) de compra no período</span></p>
                                        <br />
                                        <p>1 - R$ 0,01 a R$ 300,00</p>
                                        <p>2 - R$ 300,01 a R$ 600,00</p>
                                        <p>3 - > R$ 600,00</p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
