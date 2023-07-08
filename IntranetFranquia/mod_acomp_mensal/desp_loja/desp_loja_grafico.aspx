<%@ Page Title="Aluguel" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="desp_loja_grafico.aspx.cs" Inherits="Relatorios.desp_loja_grafico" %>

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
            Lojas&nbsp;&nbsp;>&nbsp;&nbsp;Aluguel&nbsp;&nbsp;</span>
        <div style="float: right; padding: 0; position: ;">
            <a href="../acomp_menu.aspx" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset>
            <legend>Gráfico de Aluguel</legend>
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    Filial
                                </td>
                                <td>
                                    Ano
                                </td>
                                <td>
                                    Despesa
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 210px;">
                                    <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                        Height="22px" Width="200px" OnDataBound="ddlFilial_DataBound">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 210px;">
                                    <asp:DropDownList runat="server" ID="ddlAno" Height="22px" Width="200px" DataTextField="ANO"
                                        DataValueField="ANO">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlContas" DataValueField="CODIGO_ACOMPANHAMENTO_ALUGUEL_DESPESA"
                                        DataTextField="DESCRICAO" Height="22px" Width="200px" OnDataBound="ddlContas_DataBound">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    &nbsp;
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
                        <div>
                            <asp:Button runat="server" ID="btCarregaGrafico" Text="Carregar Gráfico" OnClick="btCarregaGrafico_Click" /><asp:Label
                                ID="labErro" runat="server" ForeColor="Red"></asp:Label>
                        </div>
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
