<%@ Page Title="AnaliseVenda" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="AnaliseVenda.aspx.cs" Inherits="Relatorios.AnaliseVenda" %>

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
                <div style="width: 400px;"  class="alinhamento">
                    <label>Ano/Semana 454:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlSemana454" DataValueField="ANO_SEMANA_454" DataTextField="TEXTO" Height="22px" 
                        Width="396px" ondatabound="ddlSemana454_DataBound"></asp:DropDownList>
                </div>    
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="ButtonPesquisarMovimento" Text="Buscar Vendas Loja" OnClick="ButtonPesquisarMovimento_Click" ValidationGroup="entrada"/>
            <asp:ValidationSummary ID="ValidationSummaryEntrada" runat="server" ValidationGroup="entrada" ShowMessageBox="true" ShowSummary="false" />
            <asp:Label runat="server" ID="lblMensagemEntrada" ForeColor="Red"></asp:Label>
        </div>
        <fieldset class="login">
        <table border="1" class="style1">
            <asp:Label ID="Label1" runat="server" Text="SEMANA"></asp:Label>
        </table>
        <table border="1" class="style1">
            <tr>
                <td>
                    <asp:GridView id="GridViewVendaPeriodo" runat="server" Width="100%" ForeColor="#333333"
                        CssClass="DataGrid_Padrao" PageSize="1000" AllowPaging="True" 
                        AutoGenerateColumns="False" ShowFooter="true" 
                        onrowdatabound="GridViewVendaPeriodo_RowDataBound" ondatabound="GridViewVendaPeriodo_DataBound"> 
	                    <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
	                    <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
	                    <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
	                    <RowStyle HorizontalAlign="Center"></RowStyle>
	                    <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                        <Columns>
                            <asp:BoundField DataField="item" HeaderText="Loja" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="ButtonPequisar" Text="Vêr" OnClick="ButtonPesquisar_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ticket" HeaderText="Ticket Anterior" />
                            <asp:BoundField DataField="valor" HeaderText="Valor Anterior" />
                            <asp:BoundField DataField="peca" HeaderText="Peças Anterior" />
                            <asp:BoundField DataField="ticket_2" HeaderText="Ticket Atual" />
                            <asp:BoundField DataField="valor_2" HeaderText="Valor Atual" />
                            <asp:BoundField DataField="peca_2" HeaderText="Peças Atual" />
                            <asp:BoundField DataField="perc_ticket" HeaderText="% Ticket" />
                            <asp:BoundField DataField="perc_valor" HeaderText="% Valor" />
                            <asp:TemplateField HeaderText="Diferença Ticket">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="LiteralDifer"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="media_ticket" HeaderText="Média Ticket" />
                            <asp:BoundField DataField="futuro" HeaderText="F.u.t.u.r.o" />
                            <asp:TemplateField HeaderText="Pa Anterior">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="LiteralPa"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Pa Atual">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="LiteralPa_2"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
	                    <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <table border="1" class="style1">
            <asp:Label ID="Label2" runat="server" Text="GRUPO"></asp:Label>
        </table>
        <table border="1" class="style1">
            <tr>
                <td>
                    <asp:GridView id="GridViewVendaPeriodoGrupo" runat="server" Width="100%" 
                        CssClass="DataGrid_Padrao" PageSize="1000" AllowPaging="True" 
                        AutoGenerateColumns="False" ShowFooter="true"> 
	                    <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
	                    <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
	                    <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
	                    <RowStyle HorizontalAlign="Center"></RowStyle>
	                    <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                        <Columns>
                            <asp:BoundField DataField="item" HeaderText="Mês" />
                            <asp:BoundField DataField="ticket" HeaderText="Ticket Anterior" />
                            <asp:BoundField DataField="valor" HeaderText="Valor Anterior" />
                            <asp:BoundField DataField="peca" HeaderText="Peças Anterior" />
                            <asp:BoundField DataField="ticket_2" HeaderText="Ticket Atual" />
                            <asp:BoundField DataField="valor_2" HeaderText="Valor Atual" />
                            <asp:BoundField DataField="peca_2" HeaderText="Peças Atual" />
                            <asp:BoundField DataField="perc_ticket" HeaderText="% Ticket" />
                            <asp:BoundField DataField="perc_valor" HeaderText="% Valor" />
                            <asp:BoundField DataField="difer_ticket" HeaderText="Diferença Ticket" />
                            <asp:BoundField DataField="media_ticket" HeaderText="Média Ticket" />
                            <asp:BoundField DataField="futuro" HeaderText="Futuro" />
                            <asp:BoundField DataField="pa" HeaderText="Pa Anterior" />
                            <asp:BoundField DataField="pa_2" HeaderText="Pa Atual" />
                        </Columns>
	                    <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <table border="1" class="style1">
            <asp:Label ID="Label3" runat="server" Text="LOJA"></asp:Label>
        </table>
        <table border="1" class="style1">
            <tr>
                <td>
                    <asp:GridView id="GridViewVendaPeriodoLoja" runat="server" Width="100%" 
                        CssClass="DataGrid_Padrao" PageSize="1000" AllowPaging="True" 
                        AutoGenerateColumns="False" ShowFooter="true"> 
	                    <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
	                    <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
	                    <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
	                    <RowStyle HorizontalAlign="Center"></RowStyle>
	                    <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                        <Columns>
                            <asp:BoundField DataField="item" HeaderText="Mês" />
                            <asp:BoundField DataField="ticket" HeaderText="Ticket Anterior" />
                            <asp:BoundField DataField="valor" HeaderText="Valor Anterior" />
                            <asp:BoundField DataField="peca" HeaderText="Peças Anterior" />
                            <asp:BoundField DataField="ticket_2" HeaderText="Ticket Atual" />
                            <asp:BoundField DataField="valor_2" HeaderText="Valor Atual" />
                            <asp:BoundField DataField="peca_2" HeaderText="Peças Atual" />
                            <asp:BoundField DataField="perc_ticket" HeaderText="% Ticket" />
                            <asp:BoundField DataField="perc_valor" HeaderText="% Valor" />
                            <asp:BoundField DataField="difer_ticket" HeaderText="Diferença Ticket" />
                            <asp:BoundField DataField="media_ticket" HeaderText="Média Ticket" />
                            <asp:BoundField DataField="futuro" HeaderText="Futuro" />
                            <asp:BoundField DataField="pa" HeaderText="Pa Anterior" />
                            <asp:BoundField DataField="pa_2" HeaderText="Pa Atual" />
                        </Columns>
	                    <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        </fieldset>
    </div>
</asp:Content>
