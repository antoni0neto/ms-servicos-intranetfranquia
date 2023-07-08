<%@ Page Title="RFV – Score" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="crm_rfv_score.aspx.cs" Inherits="Relatorios.crm_rfv_score"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de CRM&nbsp;&nbsp;>&nbsp;&nbsp;Clientes&nbsp;&nbsp;>&nbsp;&nbsp;RFV – Score</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="RFV – Score"></asp:Label></legend>
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
                        <td colspan="7">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvRFV" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvRFV_RowDataBound" OnDataBound="gvRFV_DataBound" ShowFooter="true">
                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Score" ItemStyle-Font-Size="">
                                            <ItemTemplate>
                                                <asp:Literal ID="litScore" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="" ItemStyle-BackColor="LightGray" ItemStyle-Width="2px" />
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Ativação" ItemStyle-Width="130px" ItemStyle-Font-Size="">
                                            <ItemTemplate>
                                                <asp:Literal ID="litAtivacao" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="" ItemStyle-Width="80px" ItemStyle-Font-Size="">
                                            <ItemTemplate>
                                                <asp:Literal ID="litAtivacaoPorc" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="" ItemStyle-BackColor="LightGray" ItemStyle-Width="2px" />
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Potencialização" ItemStyle-Width="130px" ItemStyle-Font-Size="">
                                            <ItemTemplate>
                                                <asp:Literal ID="litPotencializacao" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="" ItemStyle-Width="80px" ItemStyle-Font-Size="">
                                            <ItemTemplate>
                                                <asp:Literal ID="litPotencializacaoPorc" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="" ItemStyle-BackColor="LightGray" ItemStyle-Width="2px" />
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Fidelização" ItemStyle-Width="130px" ItemStyle-Font-Size="">
                                            <ItemTemplate>
                                                <asp:Literal ID="litFidelizacao" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="" ItemStyle-Width="80px" ItemStyle-Font-Size="">
                                            <ItemTemplate>
                                                <asp:Literal ID="litFidelizacaoPorc" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="" ItemStyle-BackColor="LightGray" ItemStyle-Width="2px" />
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Recuperação" ItemStyle-Width="130px" ItemStyle-Font-Size="">
                                            <ItemTemplate>
                                                <asp:Literal ID="litRecuperacao" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="" ItemStyle-Width="80px" ItemStyle-Font-Size="">
                                            <ItemTemplate>
                                                <asp:Literal ID="litRecuperacaoPorc" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="" ItemStyle-BackColor="LightGray" ItemStyle-Width="2px" />
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Clientes Ativos" ItemStyle-Width="130px" ItemStyle-Font-Size="">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCliAtivo" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="" ItemStyle-Width="80px" ItemStyle-Font-Size="">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCliAtivoPorc" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="" ItemStyle-BackColor="LightGray" ItemStyle-Width="2px" />
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Reativação" ItemStyle-Width="130px" ItemStyle-Font-Size="">
                                            <ItemTemplate>
                                                <asp:Literal ID="litReativacao" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="" ItemStyle-Width="80px" ItemStyle-Font-Size="">
                                            <ItemTemplate>
                                                <asp:Literal ID="litReativacaoPorc" runat="server"></asp:Literal>
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
