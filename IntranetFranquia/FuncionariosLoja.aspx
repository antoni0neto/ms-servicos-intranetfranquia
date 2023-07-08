<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FuncionariosLoja.aspx.cs" Inherits="Relatorios.FuncionariosLoja" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="accountInfo">
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
            <legend>Critério de Filial</legend>
            <div style="width: 200px;" class="alinhamento">
                <div style="width: 200px;"  class="alinhamento">
                    <label>Filial:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="22px" 
                        Width="200px" ondatabound="ddlFilial_DataBound"></asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="ButtonPesquisarFuncionarios" Text="Buscar Funcionários" OnClick="ButtonPesquisarFuncionarios_Click"/>
        </div>
        <table border="1" class="style1">
            <tr>
                <td>
                    <asp:GridView id="GridViewFuncionario" runat="server" Width="100%" CssClass="DataGrid_Padrao" PageSize="10" AllowPaging="True" AutoGenerateColumns="False" onpageindexchanging="GridViewFuncionario_PageIndexChanging">
	                    <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
	                    <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
	                    <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
	                    <RowStyle HorizontalAlign="Center"></RowStyle>
	                    <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                        <Columns>
                            <asp:BoundField DataField="NOME_VENDEDOR" HeaderText="Vendedor" />
                            <asp:BoundField DataField="DATA_ATIVACAO" HeaderText="Data de Ativação" />
                            <asp:BoundField DataField="DATA_DESATIVACAO" HeaderText="Data de Desativação" />
                            <asp:BoundField DataField="MESES" HeaderText="Tempo em Meses" />
                        </Columns>
	                    <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </td>
                <td>
                    <label style="font-family: Arial; color: #000000; text-decoration: underline">Os Cinco Mais</label>
                    <asp:Chart ID="Chart1" runat="server">
                        <series>
                            <asp:Series Name="Series1">
                            </asp:Series>
                        </series>
                        <chartareas>
                            <asp:ChartArea Name="ChartArea1">
                            </asp:ChartArea>
                        </chartareas>
                    </asp:Chart>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
