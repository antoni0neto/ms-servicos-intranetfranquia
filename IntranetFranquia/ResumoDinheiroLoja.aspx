<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="ResumoDinheiroLoja.aspx.cs" Inherits="Relatorios.ResumoDinheiroLoja" %>

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
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo do Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Movimento Dinheiro em Loja</span>
        <div style="float: right; padding: 0;">
            <a href="DefaultFinanceiro.aspx" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset class="login">
            <legend>Movimento Dinheiro em Loja</legend>
            <div style="width: 600px;" class="alinhamento">
                <div style="width: 200px;" class="alinhamento">
                    <label>
                        Data Início:&nbsp;
                    </label>
                    <asp:TextBox ID="TextBoxDataInicio" runat="server" CssClass="textEntry" Height="22px"
                        Width="198px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDataInicio" runat="server" OnSelectionChanged="CalendarDataInicio_SelectionChanged"
                        CaptionAlign="Bottom"></asp:Calendar>
                </div>
                <div style="width: 200px;" class="alinhamento">
                    <label>
                        Data Fim:&nbsp;
                    </label>
                    <asp:TextBox ID="TextBoxDataFim" runat="server" CssClass="textEntry" Height="22px"
                        Width="198px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDataFim" runat="server" OnSelectionChanged="CalendarDataFim_SelectionChanged"
                        CaptionAlign="Bottom"></asp:Calendar>
                </div>
                <div style="width: 200px;" class="alinhamento">
                    <label>
                        Filial:&nbsp;
                    </label>
                    <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                        Height="26px" Width="200px" OnDataBound="ddlFilial_DataBound">
                    </asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btPesquisar" Text="Buscar Movimento" Width="130px"
                OnClick="btPesquisar_Click" />
            &nbsp;&nbsp;
            <asp:Button runat="server" ID="btExcel" Text="Excel" Width="130px"
                OnClick="btExcel_Click" />
            <br /><br />
        </div>
        <table border="1" class="style1">
            <tr>
                <td>
                    <fieldset class="login">
                        <legend>Depósitos em Banco</legend>
                        <asp:GridView ID="GridViewBanco" runat="server" Width="100%" CssClass="DataGrid_Padrao"
                            PageSize="1000" AllowPaging="True" AutoGenerateColumns="False" ShowFooter="true"
                            OnDataBound="GridViewBanco_DataBound">
                            <FooterStyle HorizontalAlign="Center"></FooterStyle>
                            <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
                            <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
                            <RowStyle HorizontalAlign="Center"></RowStyle>
                            <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                            <Columns>
                                <asp:BoundField DataField="data_inicio" HeaderText="Data Início" />
                                <asp:BoundField DataField="data_fim" HeaderText="Data Fim" />
                                <asp:BoundField DataField="data_deposito" HeaderText="Data Depósito" />
                                <asp:BoundField DataField="valor_a_depositar" HeaderText="Valor a Depositar" />
                                <asp:BoundField DataField="valor_depositado" HeaderText="Valor Depositado" />
                            </Columns>
                            <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                        </asp:GridView>
                    </fieldset>
                </td>
                <td>
                    <fieldset class="login">
                        <legend>Cofre/Loja</legend>
                        <asp:GridView ID="GridViewCofre" runat="server" Width="100%" CssClass="DataGrid_Padrao"
                            PageSize="1000" AllowPaging="True" AutoGenerateColumns="False" ShowFooter="true"
                            OnDataBound="GridViewCofre_DataBound">
                            <FooterStyle HorizontalAlign="Center"></FooterStyle>
                            <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
                            <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
                            <RowStyle HorizontalAlign="Center"></RowStyle>
                            <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                            <Columns>
                                <asp:BoundField DataField="data" HeaderText="Data" />
                                <asp:BoundField DataField="valor_deposito" HeaderText="Valor" />
                            </Columns>
                            <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                        </asp:GridView>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
