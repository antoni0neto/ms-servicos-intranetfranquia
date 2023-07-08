<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ListaDepositoPendencias.aspx.cs" Inherits="Relatorios.ListaDepositoPendencias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="accountInfo">
        <fieldset class="login">
            <legend>Depósitos Pendentes</legend>
        </fieldset>
        <table border="1" class="style1">
            <tr>
                <td>
                    <fieldset class="login">
                        <div style="width: 200px;" class="alinhamento">
                            <label>Data Início:&nbsp; </label>
                            <asp:TextBox ID="txtDataInicio" runat="server" CssClass="textEntry" Height="22px" Width="198px"></asp:TextBox>
                            <asp:Calendar ID="CalendarDataInicio" runat="server" OnSelectionChanged="CalendarDataInicio_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                        </div>
                    </fieldset>
                </td>
                <td>
                    <fieldset class="login">
                        <div style="width: 200px;" class="alinhamento">
                            <label>Data Fim:&nbsp; </label>
                            <asp:TextBox ID="txtDataFim" runat="server" CssClass="textEntry" Height="22px" Width="198px"></asp:TextBox>
                            <asp:Calendar ID="CalendarDataFim" runat="server" OnSelectionChanged="CalendarDataFim_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                        </div>
                    </fieldset>
                </td>
                <td>
                    <fieldset class="login">
                        <div style="width: 200px;"  class="alinhamento">
                            <label>Filial:&nbsp; </label>
                            <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="26px" Width="200px" ondatabound="ddlFilial_DataBound"></asp:DropDownList>
                        </div>
                    </fieldset>
                </td>
            </tr>
        </table>
        <div>
            <asp:Button runat="server" ID="ButtonPesquisar" Text="Pesquisar" OnClick="ButtonPesquisar_Click"/>
            <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
        </div>
    </div>
    <div>
        <table class="style1">
            <tr>
                <td>
                    <fieldset class="login">
                        <legend>Depósitos Realizados no Período</legend>
                        <asp:GridView runat="server" ID="GridViewDepositosRealizados" AutoGenerateColumns="false" ShowFooter="true" ondatabound="GridViewDepositosRealizados_DataBound" onrowdatabound="GridViewDepositosRealizados_RowDataBound">
	                    <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
	                    <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
	                    <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
	                    <RowStyle HorizontalAlign="Center"></RowStyle>
	                    <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                            <Columns>
                                <asp:TemplateField HeaderText="Filial">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralFilial"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DATA" HeaderText="Data" />
                                <asp:BoundField DataField="VALOR_DEPOSITO" HeaderText="Valor" />
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                </td>
                <td>
                    <fieldset class="login">
                        <legend>Depósitos Pendentes no Período</legend>
                        <asp:GridView runat="server" ID="GridViewDepositosPendentes" AutoGenerateColumns="false" ShowFooter="true" ondatabound="GridViewDepositosPendentes_DataBound" onrowdatabound="GridViewDepositosPendentes_RowDataBound">
	                    <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
	                    <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
	                    <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
	                    <RowStyle HorizontalAlign="Center"></RowStyle>
	                    <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                            <Columns>
                                <asp:TemplateField HeaderText="Filial">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralFilial"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DATA" HeaderText="Data" />
                                <asp:BoundField DataField="VALOR_DEPOSITO" HeaderText="Valor" />
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                </td>
                <td>
                    <fieldset class="login">
                        <legend>Comprovantes de Depósitos Realizados no Período</legend>
                        <asp:GridView runat="server" ID="GridViewComprovantesRealizados" AutoGenerateColumns="false" ShowFooter="true" ondatabound="GridViewComprovantesRealizados_DataBound" onrowdatabound="GridViewComprovantesRealizados_RowDataBound">
	                    <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
	                    <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
	                    <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
	                    <RowStyle HorizontalAlign="Center"></RowStyle>
	                    <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                            <Columns>
                                <asp:TemplateField HeaderText="Filial">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralFilial"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DATA_INICIO" HeaderText="Data Início" />
                                <asp:BoundField DataField="DATA_FIM" HeaderText="Data Fim" />
                                <asp:BoundField DataField="VALOR" HeaderText="Valor" />
                                <asp:BoundField DataField="OBS" HeaderText="Observação" />
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                </td>
                <td>
                    <fieldset class="login">
                        <legend>Comprovantes de Depósitos Pendentes no Período</legend>
                        <asp:GridView runat="server" ID="GridViewComprovantesPendentes" AutoGenerateColumns="false" ShowFooter="true" ondatabound="GridViewComprovantesPendentes_DataBound" onrowdatabound="GridViewComprovantesPendentes_RowDataBound">
	                    <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
	                    <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
	                    <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
	                    <RowStyle HorizontalAlign="Center"></RowStyle>
	                    <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                            <Columns>
                                <asp:TemplateField HeaderText="Filial">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralFilial"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DATA_INICIO" HeaderText="Data Início" />
                                <asp:BoundField DataField="DATA_FIM" HeaderText="Data Fim" />
                                <asp:BoundField DataField="VALOR" HeaderText="Valor" />
                                <asp:BoundField DataField="OBS" HeaderText="Observação" />
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
