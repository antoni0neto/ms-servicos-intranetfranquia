<%@ Page Title="Farol" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="gest_farol.aspx.cs" Inherits="Relatorios.gest_farol" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento
        {
            position: relative;
            float: left;
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
        <span style="font-family: Calibri; font-size: 14px;">Módulo de Gestão&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios
            de Gestão&nbsp;&nbsp;>&nbsp;&nbsp;Farol</span>
        <div style="float: right; padding: 0;">
            <a href="gest_menu.aspx" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset class="login">
            <legend>Farol</legend>
            <div style="width: 400px;" class="alinhamento">
                <div style="width: 200px;" class="alinhamento">
                    <label>
                        Data:&nbsp;
                    </label>
                    <asp:TextBox ID="txtData" runat="server" CssClass="textEntry" Height="22px" Width="198px"></asp:TextBox>
                    <asp:Calendar ID="CalendarData" runat="server" OnSelectionChanged="CalendarData_SelectionChanged"
                        CaptionAlign="Bottom"></asp:Calendar>
                </div>
                <div style="width: 200px;" class="alinhamento">
                    <label>
                        Supervisor:&nbsp;
                    </label>
                    <asp:DropDownList runat="server" ID="ddlSupervisor" DataValueField="CODIGO_USUARIO"
                        DataTextField="NOME_USUARIO" Height="26px" Width="200px" OnDataBound="ddlSupervisor_DataBound">
                    </asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btBuscarMovimento" Text="Buscar Movimento" OnClick="btBuscarMovimento_Click" />
        </div>
        <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
            <HeaderTemplate>
                <table border="1" style="font: 6.5pt verdana">
                    <tr bgcolor="#33cc99">
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td bgcolor="#33cc99">
                            <label>
                                Dia</label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                                Semana</label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                                Mês</label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                                Trimestre</label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                    </tr>
                    <tr bgcolor="#33cc99">
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                                Atual</label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                                Ano</label>
                        </td>
                        <td>
                            <label>
                                Anterior</label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                                Atual</label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                                Ano</label>
                        </td>
                        <td>
                            <label>
                                Anterior</label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                                Atual</label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                                Ano</label>
                        </td>
                        <td>
                            <label>
                                Anterior</label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                    </tr>
                    <tr bgcolor="#33cc99">
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                                Cota</label>
                        </td>
                        <td>
                            <label>
                                Real</label>
                        </td>
                        <td>
                            <label>
                                ▲%</label>
                        </td>
                        <td>
                            <label>
                                Cota</label>
                        </td>
                        <td>
                            <label>
                                Real</label>
                        </td>
                        <td>
                            <label>
                                ▲%</label>
                        </td>
                        <td>
                            <label>
                                ▲C</label>
                        </td>
                        <td>
                            <label>
                                ▲R</label>
                        </td>
                        <td>
                            <label>
                                Cota</label>
                        </td>
                        <td>
                            <label>
                                Real</label>
                        </td>
                        <td>
                            <label>
                                ▲%</label>
                        </td>
                        <td>
                            <label>
                                Cota</label>
                        </td>
                        <td>
                            <label>
                                Real</label>
                        </td>
                        <td>
                            <label>
                                ▲%</label>
                        </td>
                        <td>
                            <label>
                                ▲C</label>
                        </td>
                        <td>
                            <label>
                                ▲R</label>
                        </td>
                        <td>
                            <label>
                                Cota</label>
                        </td>
                        <td>
                            <label>
                                Real</label>
                        </td>
                        <td>
                            <label>
                                ▲%</label>
                        </td>
                        <td>
                            <label>
                                Cota</label>
                        </td>
                        <td>
                            <label>
                                Real</label>
                        </td>
                        <td>
                            <label>
                                ▲%</label>
                        </td>
                        <td>
                            <label>
                                ▲C</label>
                        </td>
                        <td>
                            <label>
                                ▲R</label>
                        </td>
                        <td>
                            <label>
                            </label>
                        </td>
                        <td>
                            <label>
                                Atual</label>
                        </td>
                        <td>
                            <label>
                                Anterior</label>
                        </td>
                        <td>
                            <label>
                                ▲</label>
                        </td>
                    </tr>
            </HeaderTemplate>
            <AlternatingItemTemplate>
                <tr style="background-color: Aqua">
                    <td>
                        <asp:Literal runat="server" ID="LiteralFilial"></asp:Literal>
                    </td>
                    <td>
                        <label>
                            $</label>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlCotaDia"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlVendaDia"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtendidoDia"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlCotaDiaAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlVendaDiaAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtendidoDiaAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtendidoDiaCota"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtendidoDiaVenda"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlCotaSem"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlVendaSem"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtendidoSem"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlCotaSemAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlVendaSemAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtendidoSemAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtendidoSemCota"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtendidoSemVenda"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlCotaMes"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlVendaMes"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtendidoMes"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlCotaMesAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlVendaMesAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtendidoMesAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtendidoMesCota"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtendidoMesVenda"></asp:Literal>
                    </td>
                    <td>
                        <label>
                            Ct</label>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlVendaTri"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlVendaTriAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercVendaAtingidoTri"></asp:Literal>
                    </td>
                </tr>
                <tr style="background-color: Aqua">
                    <td>
                        <asp:Literal runat="server" ID="Literal1"></asp:Literal>
                    </td>
                    <td>
                        <label>
                            Pç</label>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralQtCotaDia"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralQtVendaDia"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal4"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralQtCotaDiaAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralQtVendaDiaAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal7"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal8"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercQtAtendidoDia"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralQtCotaSem"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralQtVendaSem"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal12"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralQtCotaSemAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralQtVendaSemAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal15"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal16"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercQtAtendidoSem"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralQtCotaMes"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralQtVendaMes"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal20"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralQtCotaMesAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralQtVendaMesAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal23"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal24"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercQtAtendidoMes"></asp:Literal>
                    </td>
                    <td>
                        <label>
                            Ac</label>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlCotaTri"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlCotaTriAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercCotaAtingidoTri"></asp:Literal>
                    </td>
                </tr>
                <tr style="background-color: Aqua">
                    <td>
                        <asp:Literal runat="server" ID="Literal2"></asp:Literal>
                    </td>
                    <td>
                        <label>
                            PM</label>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal3"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlMedioDia"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal6"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal9"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlMedioDiaAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal11"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal13"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlMedioDiaAtingido"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal17"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlMedioSem"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal19"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal21"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlMedioSemAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal25"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal26"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlMedioSemAtingido"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal28"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlMedioMes"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal30"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal31"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlMedioMesAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal33"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal34"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlMedioMesAtingito"></asp:Literal>
                    </td>
                    <td>
                        <label>
                            ▲%</label>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtingidoTri"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtingidoTriAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal38"></asp:Literal>
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <ItemTemplate>
                <tr style="background-color: Silver">
                    <td>
                        <asp:Literal runat="server" ID="LiteralFilial"></asp:Literal>
                    </td>
                    <td>
                        <label>
                            $</label>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlCotaDia"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlVendaDia"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtendidoDia"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlCotaDiaAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlVendaDiaAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtendidoDiaAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtendidoDiaCota"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtendidoDiaVenda"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlCotaSem"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlVendaSem"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtendidoSem"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlCotaSemAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlVendaSemAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtendidoSemAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtendidoSemCota"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtendidoSemVenda"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlCotaMes"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlVendaMes"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtendidoMes"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlCotaMesAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlVendaMesAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtendidoMesAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtendidoMesCota"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtendidoMesVenda"></asp:Literal>
                    </td>
                    <td>
                        <label>
                            Ct</label>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlVendaTri"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlVendaTriAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercVendaAtingidoTri"></asp:Literal>
                    </td>
                </tr>
                <tr style="background-color: Silver">
                    <td>
                        <asp:Literal runat="server" ID="Literal1"></asp:Literal>
                    </td>
                    <td>
                        <label>
                            Pç</label>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralQtCotaDia"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralQtVendaDia"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal4"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralQtCotaDiaAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralQtVendaDiaAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal7"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal8"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercQtAtendidoDia"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralQtCotaSem"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralQtVendaSem"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal12"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralQtCotaSemAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralQtVendaSemAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal15"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal16"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercQtAtendidoSem"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralQtCotaMes"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralQtVendaMes"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal20"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralQtCotaMesAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralQtVendaMesAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal23"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal24"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercQtAtendidoMes"></asp:Literal>
                    </td>
                    <td>
                        <label>
                            Ac</label>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlCotaTri"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlCotaTriAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercCotaAtingidoTri"></asp:Literal>
                    </td>
                </tr>
                <tr style="background-color: Silver">
                    <td>
                        <asp:Literal runat="server" ID="Literal2"></asp:Literal>
                    </td>
                    <td>
                        <label>
                            PM</label>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal3"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlMedioDia"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal6"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal9"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlMedioDiaAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal11"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal13"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlMedioDiaAtingido"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal17"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlMedioSem"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal19"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal21"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlMedioSemAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal25"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal26"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlMedioSemAtingido"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal28"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlMedioMes"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal30"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal31"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlMedioMesAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal33"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal34"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralVlMedioMesAtingito"></asp:Literal>
                    </td>
                    <td>
                        <label>
                            ▲%</label>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtingidoTri"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="LiteralPercAtingidoTriAnoAnt"></asp:Literal>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="Literal38"></asp:Literal>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
