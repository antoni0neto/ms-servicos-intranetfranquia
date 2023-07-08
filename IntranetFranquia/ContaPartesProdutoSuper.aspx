<%@ Page Title="Conta Partes de Produto" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ContaPartesProdutoSuper.aspx.cs" Inherits="Relatorios.ContaPartesProdutoSuper" %>

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
            <legend>Filtro de Pesquisa</legend>
            <div style="width: 900px;" class="alinhamento">
                <div style="width: 300px;" class="alinhamento">
                    <label>Data Início:&nbsp; </label>
                    <asp:TextBox ID="TextBoxDataInicio" runat="server" CssClass="textEntry" Height="22px" Width="298px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDataInicio" runat="server" ondayrender="CalendarDataInicio_DayRender" OnSelectionChanged="CalendarDataInicio_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                </div>
                <div style="width: 300px;" class="alinhamento">
                    <label>Data Fim:&nbsp; </label>
                    <asp:TextBox ID="TextBoxDataFim" runat="server" CssClass="textEntry" Height="22px" Width="298px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDataFim" runat="server" OnSelectionChanged="CalendarDataFim_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                </div>
                <div style="width: 300px;"  class="alinhamento">
                    <label>Supervisor:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlSupervisor" DataValueField="CODIGO_USUARIO" DataTextField="NOME_USUARIO" Height="26px" 
                        Width="298px" ondatabound="ddlSupervisor_DataBound"></asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btBuscar" Text="Buscar Produtos" OnClick="btBuscar_Click"/>
            <asp:ValidationSummary ID="ValidationSummaryVendas" runat="server" ShowMessageBox="true" ShowSummary="false" />
        </div>
        <table border="1" class="style1">
            <tr>
                <td>
                    <asp:GridView id="GridViewProdutos" runat="server" Width="100%" CssClass="DataGrid_Padrao" PageSize="10" AllowPaging="True" AutoGenerateColumns="False" 
                                  onpageindexchanging="GridViewProdutos_PageIndexChanging">
	                    <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
	                    <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
	                    <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
	                    <RowStyle HorizontalAlign="Center"></RowStyle>
	                    <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                        <Columns>
                            <asp:BoundField DataField="FILIAL" HeaderText="Filial" />
                            <asp:BoundField DataField="TICKET" HeaderText="Ticket" />
                            <asp:BoundField DataField="KIT_NATAL" HeaderText="Kit Natal" />
                            <asp:BoundField DataField="PERC_ATINGIDO" HeaderText="Percentual Atingido" />
                            <asp:BoundField DataField="CIMA_1" HeaderText="1 Qt - Cima" />
                            <asp:BoundField DataField="CIMA_2" HeaderText="2 Qt - Cima" />
                            <asp:BoundField DataField="CIMA_3" HeaderText="Acima de 2 Qt - Cima" />
                            <asp:BoundField DataField="BAIXO_1" HeaderText="1 Qt - Baixo" />
                            <asp:BoundField DataField="BAIXO_2" HeaderText="2 Qt - Baixo" />
                            <asp:BoundField DataField="BAIXO_3" HeaderText="Acima de 3 Qt - Baixo" />
                        </Columns>
	                    <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
