<%@ Page Title="Performance de Rede" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="desp_loja_performance_rede.aspx.cs" Inherits="Relatorios.desp_loja_performance_rede" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento
        {
            float: left;
            position: static;
        }
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo Acompanhamento Mensal&nbsp;&nbsp;>&nbsp;&nbsp;Despesas
            Lojas&nbsp;&nbsp;>&nbsp;&nbsp;Performance de Rede</span>
        <div style="float: right; padding: 0;">
            <a href="../acomp_menu.aspx" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset>
            <legend>Performance de Rede</legend>
            <table border="0" class="style1">
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <label>
                                        Ano&nbsp;
                                    </label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 210px;">
                                    <asp:DropDownList runat="server" ID="ddlAno" Height="22px" Width="200px" DataTextField="ANO"
                                        DataValueField="ANO">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btCarregaGrafico" Text="Carregar Performance" Width="150px"
                                        OnClick="btCarregaGrafico_Click" /><asp:Label ID="labErro" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnl1" runat="server" BackColor="Black" BorderWidth="1" BorderColor="Black">
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
