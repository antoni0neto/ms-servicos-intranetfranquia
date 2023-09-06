<%@ Page Title="Ciclo de Clientes" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="crm_ciclo_cliente.aspx.cs" Inherits="Relatorios.crm_ciclo_cliente"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de CRM&nbsp;&nbsp;>&nbsp;&nbsp;Clientes&nbsp;&nbsp;>&nbsp;&nbsp;Ciclo de Clientes</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Ciclo de Clientes"></asp:Label></legend>
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
                        <td style="width: 243px;">
                            <asp:Label ID="labTri1" runat="server" Text="" ForeColor="Gray" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="width: 243px;">
                            <asp:Label ID="labTri2" runat="server" Text="" ForeColor="Gray" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="width: 243px;">
                            <asp:Label ID="labTri3" runat="server" Text="" ForeColor="Gray" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="width: 243px;">
                            <asp:Label ID="labTri4" runat="server" Text="" ForeColor="Gray" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="width: 180px;">
                            <asp:Label ID="labTri5" runat="server" Text="" ForeColor="Gray" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvCiclo" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvCiclo_RowDataBound" OnDataBound="gvCiclo_DataBound">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="">
                                            <ItemTemplate>
                                                <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="" ItemStyle-BackColor="LightGray" ItemStyle-Width="2px" />
                                        <%--1 TRI ATRAS--%>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Qtde Cliente" ItemStyle-Width="115px" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeCliente1" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Qtde Cliente (%)" ItemStyle-Width="115px" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeCliente1Porc" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="" ItemStyle-BackColor="LightGray" ItemStyle-Width="2px" />
                                        <%--2 TRI ATRAS--%>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Qtde Cliente" ItemStyle-Width="115px" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeCliente2" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Qtde Cliente (%)" ItemStyle-Width="115px" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeCliente2Porc" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="" ItemStyle-BackColor="LightGray" ItemStyle-Width="2px" />
                                        <%--3 TRI ATRAS--%>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Qtde Cliente" ItemStyle-Width="115px" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeCliente3" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Qtde Cliente (%)" ItemStyle-Width="115px" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeCliente3Porc" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="" ItemStyle-BackColor="LightGray" ItemStyle-Width="2px" />
                                        <%--4 TRI ATRAS--%>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Qtde Cliente" ItemStyle-Width="115px" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeCliente4" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Qtde Cliente (%)" ItemStyle-Width="115px" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeCliente4Porc" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="" ItemStyle-BackColor="LightGray" ItemStyle-Width="2px" />
                                        <%--5 TRI ATRAS--%>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Qtde Cliente" ItemStyle-Width="115px" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeCliente5" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Qtde Cliente (%)" ItemStyle-Width="115px" ItemStyle-Font-Size="Smaller">
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
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td style="border-left: 1px solid red; border-bottom: 1px solid red;">
                            <asp:Label ID="labLinha5" runat="server" Text="&nbsp;" ForeColor="Gray" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="border-bottom: 1px solid red;">
                            <asp:Label ID="labLinha4" runat="server" Text="&nbsp;" ForeColor="Gray" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="border-bottom: 1px solid red;">
                            <asp:Label ID="labLinha3" runat="server" Text="&nbsp;" ForeColor="Gray" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="border-bottom: 1px solid red;">
                            <asp:Label ID="labLinha2" runat="server" Text="&nbsp;" ForeColor="Gray" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="border-left: 1px solid red;">
                            <asp:Label ID="labLinha1" runat="server" Text="&nbsp;" ForeColor="Gray" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td colspan="3" style="padding-right: 150px; text-align: right;">
                            <asp:Label ID="labDiffP" runat="server" Text="" ForeColor="Red" Font-Size="X-Large" Font-Bold="true"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>

                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="7">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <asp:Panel ID="pnlGraficoCiclo" runat="server" BackColor="White" BorderWidth="1" BorderColor="Black">
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="top" style="width: 20%;">
                                        <span style="font-weight: bold;">Ativação</span>
                                        <hr />
                                        1 compra e data da ultima compra nos últimos 6 meses
                                    </td>
                                    <td valign="top" style="width: 20%;">
                                        <span style="font-weight: bold;">Potencialização</span>
                                        <hr />
                                        Mais de 1 compra e data da última compra nos últimos 6 meses e grupo de produto distintos nos últimos 6 M <= 2
                                    </td>
                                    <td valign="top" style="width: 20%;">
                                        <span style="font-weight: bold;">Fidelização</span>
                                        <hr />
                                        Mais de 1 compra e data da última compra nos últimos 6 meses e grupo de produto distintos nos últimos 6 M >= 3
                                    </td>
                                    <td valign="top" style="width: 20%;">
                                        <span style="font-weight: bold;">Recuperação</span>
                                        <hr />
                                        1 ou mais compras e data da última compra nos últimos 7 a 12 meses 
                                    </td>
                                    <td valign="top" style="width: 20%;">
                                        <span style="font-weight: bold;">Reativação</span>
                                        <hr />
                                        1 ou mais compras e data da última compra nos últimos 13 a 24 meses 
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
