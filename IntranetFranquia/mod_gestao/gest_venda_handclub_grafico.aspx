<%@ Page Title="Vendas Handclub" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="gest_venda_handclub_grafico.aspx.cs" Inherits="Relatorios.gest_venda_handclub_grafico" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento {
            float: left;
            position: static;
        }

        .style1 {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo de Gestão&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Vendas Handclub&nbsp;&nbsp;</span>
        <div style="float: right; padding: 0; position: ;">
            <a href="../acomp_menu.aspx" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset>
            <legend>Vendas Handclub</legend>
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>Ano
                                </td>
                                <td>Mês
                                </td>
                                <td>Filial
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 210px;">
                                    <asp:DropDownList runat="server" ID="ddlAno" Height="22px" Width="200px" DataTextField="ANO"
                                        DataValueField="ANO">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 210px;">
                                    <asp:DropDownList runat="server" ID="ddlMes" Height="22px" Width="200px">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Janeiro"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Fevereiro"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Março"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Abril"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="Maio"></asp:ListItem>
                                        <asp:ListItem Value="6" Text="Junho"></asp:ListItem>
                                        <asp:ListItem Value="7" Text="Julho"></asp:ListItem>
                                        <asp:ListItem Value="8" Text="Agosto"></asp:ListItem>
                                        <asp:ListItem Value="9" Text="Setembro"></asp:ListItem>
                                        <asp:ListItem Value="10" Text="Outubro"></asp:ListItem>
                                        <asp:ListItem Value="11" Text="Novembro"></asp:ListItem>
                                        <asp:ListItem Value="12" Text="Dezembro"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 210px;">
                                    <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                        Height="22px" Width="200px" OnDataBound="ddlFilial_DataBound">
                                    </asp:DropDownList>
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <asp:Button runat="server" ID="btCarregaGrafico" Text="Carregar Gráfico" OnClick="btCarregaGrafico_Click" />
                            <asp:Label ID="labErro" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlGrafico" runat="server" BackColor="Black" BorderWidth="1" BorderColor="Black">
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
