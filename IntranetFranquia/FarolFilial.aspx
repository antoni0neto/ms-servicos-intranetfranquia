<%@ Page Title="Farol Loja" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="FarolFilial.aspx.cs" Inherits="Relatorios.FarolFilial" %>

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
    <div class="accountInfo">
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
            <div style="width: 400px;" class="alinhamento">
                <div style="width: 200px;" class="alinhamento">
                    <label>Data:&nbsp; </label>
                    <asp:TextBox ID="txtData" runat="server" CssClass="textEntry" Height="22px" Width="196px"></asp:TextBox>
                    <asp:Calendar ID="CalendarData" runat="server" OnSelectionChanged="CalendarData_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btBuscarMovimento" Text="Buscar Movimento" OnClick="btBuscarMovimento_Click"/>
        </div>
        <asp:Repeater id="Repeater1" runat="server" onitemdatabound="Repeater1_ItemDataBound">
          <HeaderTemplate>
             <table border="1" style="font: 6.5pt verdana">
             <tr bgcolor="#33cc99">
                <td> 
                    <label></label>
                </td>
                <td> 
                    <label></label>
                </td>
                <td bgcolor="#33cc99"> 
                    <label>Dia</label>
                </td>
                <td> 
                    <label></label>
                </td>
                <td> 
                    <label></label>
                </td>
                <td> 
                    <label></label>
                </td>
                <td> 
                    <label></label>
                </td>
                <td> 
                    <label></label>
                </td>
                <td> 
                    <label></label>
                </td>
                <td> 
                    <label></label>
                </td>
             </tr>
             <tr bgcolor="#33cc99">
                <td> 
                    <label></label>
                </td>
                <td> 
                    <label></label>
                </td>
                <td> 
                    <label>Atual</label>
                </td>
                <td> 
                    <label></label>
                </td>
                <td> 
                    <label></label>
                </td>
                <td> 
                    <label>Ano</label>
                </td>
                <td> 
                    <label>Anterior</label>
                </td>
                <td> 
                    <label></label>
                </td>
                <td> 
                    <label></label>
                </td>
                <td> 
                    <label></label>
                </td>
             </tr>
             <tr bgcolor="#33cc99">
                <td> 
                    <label></label>
                </td>
                <td> 
                    <label></label>
                </td>
                <td> 
                    <label>Cota</label>
                </td>
                <td> 
                    <label>Real</label>
                </td>
                <td> 
                    <label>▲%</label>
                </td>
                <td> 
                    <label>Cota</label>
                </td>
                <td> 
                    <label>Real</label>
                </td>
                <td> 
                    <label>▲%</label>
                </td>
                <td> 
                    <label>▲C</label>
                </td>
                <td> 
                    <label>▲R</label>
                </td>
             </tr>
          </HeaderTemplate>
          <AlternatingItemTemplate>
             <tr style="background-color:Aqua">
                <td> 
                    <asp:Literal runat="server" ID="LiteralFilial"></asp:Literal> 
                </td>
                <td> 
                    <label>$</label>
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
             </tr>
             <tr style="background-color:Aqua">
                <td> 
                    <asp:Literal runat="server" ID="Literal1"></asp:Literal> 
                </td>
                <td> 
                    <label>Pç</label>
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
             </tr>
             <tr style="background-color:Aqua">
                <td> 
                    <asp:Literal runat="server" ID="Literal2"></asp:Literal> 
                </td>
                <td> 
                    <label>PM</label>
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
             </tr>
          </AlternatingItemTemplate>
          <ItemTemplate>
             <tr style="background-color:Silver">
                <td> 
                    <asp:Literal runat="server" ID="LiteralFilial"></asp:Literal> 
                </td>
                <td> 
                    <label>$</label>
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
             </tr>
             <tr style="background-color:Silver">
                <td> 
                    <asp:Literal runat="server" ID="Literal1"></asp:Literal> 
                </td>
                <td> 
                    <label>Pç</label>
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
             </tr>
             <tr style="background-color:Silver">
                <td> 
                    <asp:Literal runat="server" ID="Literal2"></asp:Literal> 
                </td>
                <td> 
                    <label>PM</label>
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
             </tr>
          </ItemTemplate>
          <FooterTemplate>
             </table>
          </FooterTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
