<%@ Page Title="Análise Hora de Atendimento Dia" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="gerloja_analise_atendimento_dia.aspx.cs" Inherits="Relatorios.gerloja_analise_atendimento_dia" %>

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
            <span style="font-family: Calibri; font-size: 14px;">Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Vendedores&nbsp;&nbsp;>&nbsp;&nbsp;Análise Hora de Atendimento Dia</span>
            <div style="float: right; padding: 0;">
                <a href="gerloja_menu.aspx" class="alink" title="Voltar">Voltar</a>
            </div>
        </div>
        <hr />
        <br />
        <fieldset class="login">
            <legend>Análise Hora de Atendimento Dia</legend>
            <div style="width: 800px;" class="alinhamento">
                <div style="width: 200px;" class="alinhamento">
                    <label>Ano:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlAno" Height="22px" Width="198px">
                        <asp:ListItem>Selecione</asp:ListItem>
                        <asp:ListItem>2014</asp:ListItem>
                        <asp:ListItem>2015</asp:ListItem>
                        <asp:ListItem>2016</asp:ListItem>
                        <asp:ListItem>2017</asp:ListItem>
                        <asp:ListItem>2018</asp:ListItem>
                        <asp:ListItem>2019</asp:ListItem>
                        <asp:ListItem>2020</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div style="width: 200px;" class="alinhamento">
                    <label>Mês:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlMes" Height="22px" Width="198px">
                        <asp:ListItem>Selecione</asp:ListItem>
                        <asp:ListItem>Janeiro</asp:ListItem>
                        <asp:ListItem>Fevereiro</asp:ListItem>
                        <asp:ListItem>Marco</asp:ListItem>
                        <asp:ListItem>Abril</asp:ListItem>
                        <asp:ListItem>Maio</asp:ListItem>
                        <asp:ListItem>Junho</asp:ListItem>
                        <asp:ListItem>Julho</asp:ListItem>
                        <asp:ListItem>Agosto</asp:ListItem>
                        <asp:ListItem>Setembro</asp:ListItem>
                        <asp:ListItem>Outubro</asp:ListItem>
                        <asp:ListItem>Novembro</asp:ListItem>
                        <asp:ListItem>Dezembro</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div style="width: 200px;" class="alinhamento">
                    <label>Dia:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlDia" Height="22px" Width="198px">
                        <asp:ListItem>Selecione</asp:ListItem>
                        <asp:ListItem>Segunda</asp:ListItem>
                        <asp:ListItem>Terca</asp:ListItem>
                        <asp:ListItem>Quarta</asp:ListItem>
                        <asp:ListItem>Quinta</asp:ListItem>
                        <asp:ListItem>Sexta</asp:ListItem>
                        <asp:ListItem>Sabado</asp:ListItem>
                        <asp:ListItem>Domingo</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div style="width: 200px;" class="alinhamento">
                    <label>Filial:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="26px"
                        Width="200px" OnDataBound="ddlFilial_DataBound">
                    </asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="ButtonPesquisarVendas" Text="Buscar Vendas" OnClick="ButtonPesquisarVendas_Click" />
            <asp:Button runat="server" ID="ButtonVerDetalhe" Text="Buscar Detalhe" OnClick="ButtonVerDetalhe_Click" />
        </div>
        <table border="1" class="style1">
            <tr>
                <td>
                    <asp:GridView ID="GridViewVendas" runat="server" Width="100%" CssClass="DataGrid_Padrao" PageSize="15" AllowPaging="True" AutoGenerateColumns="False" ShowFooter="true"
                        OnDataBound="GridViewVendas_DataBound">
                        <FooterStyle HorizontalAlign="Center"></FooterStyle>
                        <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
                        <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
                        <RowStyle HorizontalAlign="Center"></RowStyle>
                        <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                        <Columns>
                            <asp:BoundField DataField="diaSemana" HeaderText="Dia" />
                            <asp:BoundField DataField="hora" HeaderText="Hora" />
                            <asp:BoundField DataField="valor" HeaderText="Valor" />
                            <asp:BoundField DataField="qtVendedor" HeaderText="Qt Vendedor" />
                            <asp:BoundField DataField="qtTicket" HeaderText="Qt Ticket's" />
                        </Columns>
                        <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="GridViewVendas_1" runat="server" Width="100%" CssClass="DataGrid_Padrao" PageSize="15" AllowPaging="True" AutoGenerateColumns="False"
                        OnDataBound="GridViewVendas_1_DataBound">
                        <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
                        <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
                        <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
                        <RowStyle HorizontalAlign="Center"></RowStyle>
                        <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="Data">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="LiteralData_1" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="HORA" HeaderText="Hora" />
                            <asp:BoundField DataField="VALOR" HeaderText="Valor" />
                            <asp:BoundField DataField="N_VENDEDOR" HeaderText="Qt Vendedor" />
                            <asp:BoundField DataField="N_TICKET" HeaderText="Qt Ticket's" />
                        </Columns>
                        <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="GridViewVendas_2" runat="server" Width="100%" CssClass="DataGrid_Padrao" PageSize="15" AllowPaging="True" AutoGenerateColumns="False" ShowFooter="true"
                        OnDataBound="GridViewVendas_2_DataBound">
                        <FooterStyle HorizontalAlign="Center"></FooterStyle>
                        <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
                        <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
                        <RowStyle HorizontalAlign="Center"></RowStyle>
                        <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="Data">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="LiteralData_2" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="HORA" HeaderText="Hora" />
                            <asp:BoundField DataField="VALOR" HeaderText="Valor" />
                            <asp:BoundField DataField="N_VENDEDOR" HeaderText="Qt Vendedor" />
                            <asp:BoundField DataField="N_TICKET" HeaderText="Qt Ticket's" />
                        </Columns>
                        <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="GridViewVendas_3" runat="server" Width="100%" CssClass="DataGrid_Padrao" PageSize="15" AllowPaging="True" AutoGenerateColumns="False"
                        OnDataBound="GridViewVendas_3_DataBound">
                        <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
                        <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
                        <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
                        <RowStyle HorizontalAlign="Center"></RowStyle>
                        <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="Data">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="LiteralData_3" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="HORA" HeaderText="Hora" />
                            <asp:BoundField DataField="VALOR" HeaderText="Valor" />
                            <asp:BoundField DataField="N_VENDEDOR" HeaderText="Qt Vendedor" />
                            <asp:BoundField DataField="N_TICKET" HeaderText="Qt Ticket's" />
                        </Columns>
                        <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="GridViewVendas_4" runat="server" Width="100%" CssClass="DataGrid_Padrao" PageSize="15" AllowPaging="True" AutoGenerateColumns="False"
                        OnDataBound="GridViewVendas_4_DataBound">
                        <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
                        <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
                        <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
                        <RowStyle HorizontalAlign="Center"></RowStyle>
                        <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="Data">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="LiteralData_4" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="HORA" HeaderText="Hora" />
                            <asp:BoundField DataField="VALOR" HeaderText="Valor" />
                            <asp:BoundField DataField="N_VENDEDOR" HeaderText="Qt Vendedor" />
                            <asp:BoundField DataField="N_TICKET" HeaderText="Qt Ticket's" />
                        </Columns>
                        <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="GridViewVendas_5" runat="server" Width="100%" CssClass="DataGrid_Padrao" PageSize="15" AllowPaging="True" AutoGenerateColumns="False"
                        OnDataBound="GridViewVendas_5_DataBound">
                        <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
                        <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
                        <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
                        <RowStyle HorizontalAlign="Center"></RowStyle>
                        <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="Data">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="LiteralData_5" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="HORA" HeaderText="Hora" />
                            <asp:BoundField DataField="VALOR" HeaderText="Valor" />
                            <asp:BoundField DataField="N_VENDEDOR" HeaderText="Qt Vendedor" />
                            <asp:BoundField DataField="N_TICKET" HeaderText="Qt Ticket's" />
                        </Columns>
                        <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
