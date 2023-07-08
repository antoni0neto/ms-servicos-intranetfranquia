<%@ Page Title="Totais Completo Super (Lojas)" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="gest_loja_compara_venda_super.aspx.cs" Inherits="Relatorios.gest_loja_compara_venda_super" %>

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
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo de Gestão&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios
            de Gestão&nbsp;&nbsp;>&nbsp;&nbsp;Totais Completo Super (Lojas)</span>
        <div style="float: right; padding: 0;">
            <a href="gest_menu.aspx" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset class="login">
            <legend>Totais Completo Super (Lojas)</legend>
            <div style="width: 600px;" class="alinhamento">
                <div style="width: 200px;" class="alinhamento">
                    <label>
                        Data Início:&nbsp;
                    </label>
                    <asp:TextBox ID="txtDataInicioAtual" runat="server" CssClass="textEntry" Height="22px"
                        Width="198px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDataInicioAtual" runat="server" OnSelectionChanged="CalendarDataInicioAtual_SelectionChanged"
                        CaptionAlign="Bottom"></asp:Calendar>
                </div>
                <div style="width: 200px;" class="alinhamento">
                    <label>
                        Data Fim:&nbsp;
                    </label>
                    <asp:TextBox ID="txtDataFimAtual" runat="server" CssClass="textEntry" Height="22px"
                        Width="198px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDataFimAtual" runat="server" OnSelectionChanged="CalendarDataFimAtual_SelectionChanged"
                        CaptionAlign="Bottom"></asp:Calendar>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btPesquisarVendas" Text="Buscar Vendas" Width="120px" OnClick="btPesquisarVendas_Click" />&nbsp;
            <asp:Button runat="server" ID="btGeraExcel" Text="Enviar E-mail" Width="120px" OnClick="btGeraExcel_Click" Enabled="false" />
            <asp:Label ID="labMsgEmail" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
        </div>
        <fieldset class="login">
            <asp:GridView ID="GridViewVendas" runat="server" Width="100%" AutoGenerateColumns="False"
                ShowFooter="true" ForeColor="#333333" OnRowDataBound="GridViewVendas_RowDataBound"
                OnDataBound="GridViewVendas_DataBound" Style="background: white">
                <FooterStyle HorizontalAlign="Center"></FooterStyle>
                <RowStyle HorizontalAlign="Center"></RowStyle>
                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                <Columns>
                    <asp:BoundField DataField="filial" HeaderText="Filial" />
                    <asp:TemplateField HeaderText="2021">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralVendasAtual" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="2020">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralVendasAnterior" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Diferença">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralDiferenca" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="% Crescimento">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralPercentual" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </fieldset>
        <table border="1" class="style1">
            <tr>
                <td valign="top" style="width: 50%;">
                    <fieldset class="login">
                        <legend>Dirceu de Lima</legend>
                        <asp:GridView ID="GridViewDirceu" runat="server" Width="100%" AutoGenerateColumns="False"
                            ShowFooter="true" ForeColor="#333333" OnRowDataBound="GridViewVendas_RowDataBound"
                            OnDataBound="GridViewDirceu_DataBound" Style="background: white">
                            <FooterStyle HorizontalAlign="Center"></FooterStyle>
                            <RowStyle HorizontalAlign="Center"></RowStyle>
                            <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                            <Columns>
                                <asp:BoundField DataField="filial" HeaderText="Filial" ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderText="2021">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralVendasAtual" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="2020">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralVendasAnterior" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Diferença">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralDiferenca" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="% Crescimento">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralPercentual" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                </td>
                <td valign="top" style="width: 50%;">
                    <fieldset class="login">
                        <legend>Maurílio</legend>
                        <asp:GridView ID="GridViewMaurilho" runat="server" Width="100%" AutoGenerateColumns="False"
                            ShowFooter="true" ForeColor="#333333" OnRowDataBound="GridViewVendas_RowDataBound"
                            OnDataBound="GridViewMaurilho_DataBound" Style="background: white">
                            <FooterStyle HorizontalAlign="Center"></FooterStyle>
                            <RowStyle HorizontalAlign="Center"></RowStyle>
                            <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                            <Columns>
                                <asp:BoundField DataField="filial" HeaderText="Filial" ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderText="2021">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralVendasAtual" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="2020">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralVendasAnterior" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Diferença">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralDiferenca" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="% Crescimento">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralPercentual" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
