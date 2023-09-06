<%@ Page Title="Análise Hora de Atendimento" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="gerloja_analise_atendimento.aspx.cs" Inherits="Relatorios.gerloja_analise_atendimento" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento {
            position: relative;
            float: left;
        }

        .style1 {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="accountInfo">
        <br />
        <div>
            <span style="font-family: Calibri; font-size: 14px;">Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Vendedores&nbsp;&nbsp;>&nbsp;&nbsp;Análise Hora de Atendimento</span>
            <div style="float: right; padding: 0;">
                <a href="gerloja_menu.aspx" class="alink" title="Voltar">Voltar</a>
            </div>
        </div>
        <hr />
        <br />
        <fieldset class="login">
            <legend>Análise Hora de Atendimento</legend>
            <div style="width: 800px;" class="alinhamento">
                <div style="width: 200px;" class="alinhamento">
                    <label>Data Início:&nbsp; </label>
                    <asp:TextBox ID="TextBoxDataInicio" runat="server" CssClass="textEntry" Height="22px" Width="198px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDataInicio" runat="server" OnSelectionChanged="CalendarDataInicio_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                </div>
                <div style="width: 200px;" class="alinhamento">
                    <label>Data Fim:&nbsp; </label>
                    <asp:TextBox ID="TextBoxDataFim" runat="server" CssClass="textEntry" Height="22px" Width="198px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDataFim" runat="server" OnSelectionChanged="CalendarDataFim_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                </div>
                <div style="width: 250px;" class="alinhamento">
                    <label>Filial:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="26px" Width="250px">
                    </asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btPesquisarVendas" Text="Buscar Vendas" OnClick="btPesquisarVendas_Click" />
            <asp:ValidationSummary ID="ValidationSummaryVendas" runat="server" ShowMessageBox="true" ShowSummary="false" />
        </div>
        <fieldset>
            <br />
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 550px;" valign="top">
                        <div class="rounded_corners">
                            <asp:GridView ID="gvVendasHora" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" OnDataBound="gvVendasHora_DataBound">
                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true"></HeaderStyle>
                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                <Columns>
                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="HORA" HeaderText="Hora" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120px" />
                                    <asp:BoundField DataField="VALOR" HeaderText="Valor" HeaderStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="N_VENDEDOR" HeaderText="Qte Vendedor" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120px" />
                                    <asp:BoundField DataField="N_TICKET" HeaderText="Qte Ticket" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120px" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                    <td style="text-align: center;" valign="top">
                        <label style="font-family: Arial; color: #000000; text-decoration: underline"></label>
                        <asp:Chart ID="chartHora" runat="server" Width="600px" BackColor="#D3DFF0" BackSecondaryColor="White" BorderSkin-BorderDashStyle="Solid"
                            BackGradientStyle="TopBottom" BorderlineColor="#1A3B69" BorderSkin-BorderColor="#1A3B69">
                            <Titles>
                                <asp:Title Text="Valores (R$) por Hora"></asp:Title>
                            </Titles>
                            <Series>
                                <asp:Series Name="serieHora" ChartType="Column" BorderColor="180, 26, 59, 105" ToolTip="">
                                </asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="chartAreaHora" BackGradientStyle="TopBottom" BorderDashStyle="Solid" BackColor="64, 165, 191, 228" Area3DStyle-Inclination="15"
                                    Area3DStyle-Enable3D="false" ShadowColor="Transparent"
                                    BackSecondaryColor="White" BorderColor="64, 64, 64, 64">
                                    <AxisY LineColor="64, 64, 64, 64" Title="Valores (R$)">
                                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="R$ {0.00}" />
                                        <MajorGrid LineColor="64, 64, 64, 64" />
                                    </AxisY>
                                    <AxisX LineColor="64, 64, 64, 64" Interval="1">
                                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                        <MajorGrid LineColor="64, 64, 64, 64" />
                                    </AxisX>
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
