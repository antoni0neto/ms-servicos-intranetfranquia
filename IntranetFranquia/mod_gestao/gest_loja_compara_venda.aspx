<%@ Page Title="Comparáveis (Lojas)" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="gest_loja_compara_venda.aspx.cs" Inherits="Relatorios.gest_loja_compara_venda" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../Image/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "../Image/plus.png");
            $(this).closest("tr").next().remove();
        });
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo de Gestão&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios
            de Gestão&nbsp;&nbsp;>&nbsp;&nbsp;Comparáveis (Lojas)</span>
        <div style="float: right; padding: 0;">
            <a href="gest_menu.aspx" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset class="login">
            <legend>Comparáveis (Lojas)</legend>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>Data Inicial</td>
                    <td>Data Final</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 199px;">
                        <asp:TextBox ID="txtDataInicioAtual" runat="server" Height="20px" Width="195px" Enabled="false"></asp:TextBox>
                    </td>
                    <td style="width: 199px;">
                        <asp:TextBox ID="txtDataFimAtual" runat="server" Height="20px" Width="195px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Calendar ID="CalendarDataInicioAtual" runat="server" OnSelectionChanged="CalendarDataInicioAtual_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                    </td>
                    <td>
                        <asp:Calendar ID="CalendarDataFimAtual" runat="server" OnSelectionChanged="CalendarDataFimAtual_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btPesquisarVendas" Text="Buscar Vendas" Width="120px" OnClick="btPesquisarVendas_Click" />&nbsp;
            <asp:Button ID="btGerarExcel" runat="server" Text="Enviar E-mail" Width="120px" OnClick="btGerarExcel_Click" Enabled="false" />
            <asp:Label ID="labMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
        </div>
        <fieldset>
            <div class="rounded_corners">
                <asp:GridView ID="gvVendas" runat="server" Width="100%" AutoGenerateColumns="False"
                    ShowFooter="true" ForeColor="#333333" OnRowDataBound="gvVendas_RowDataBound"
                    OnDataBound="gvVendas_DataBound" Style="background: white">
                    <HeaderStyle BackColor="GradientActiveCaption" />
                    <FooterStyle BackColor="GradientActiveCaption" Font-Bold="true" HorizontalAlign="Center" />
                    <Columns>
                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center"
                            ItemStyle-VerticalAlign="Middle">
                            <ItemTemplate>
                                <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../Image/plus.png"
                                    Width="15px" runat="server" />
                                <asp:Panel ID="pnlGrafico" runat="server" Style="display: none" Width="100%">
                                    <asp:GridView ID="gvGrafico" runat="server" AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvGrafico_RowDataBound">
                                        <HeaderStyle BackColor="LightSteelBlue" HorizontalAlign="Center"></HeaderStyle>
                                        <RowStyle BorderColor="GradientActiveCaption" BorderWidth="1px" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px">
                                                <ItemTemplate>
                                                    <fieldset>
                                                        <asp:Panel ID="pnlGraficoResultado" runat="server" BackColor="White" BorderWidth="1" BorderColor="Black">
                                                        </asp:Panel>
                                                    </fieldset>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="filial" HeaderText="Filial" />
                        <asp:TemplateField HeaderText="2021" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralVendasAtual" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="2020" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralVendasAnterior" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Diferença" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralDiferenca" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="% Crescimento" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralPercentual" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </fieldset>
        <fieldset>
            <div class="rounded_corners">
                <asp:GridView ID="gvFilialSemVenda" runat="server" Width="100%" AutoGenerateColumns="False"
                    ShowFooter="true" ForeColor="#333333" Style="background: white">
                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                    <FooterStyle BackColor="GradientActiveCaption" Font-Bold="true" HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="filial" HeaderText="Filial" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                </asp:GridView>
            </div>
        </fieldset>
    </div>
</asp:Content>
