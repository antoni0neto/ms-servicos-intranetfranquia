<%@ Page Title="Lista Movimento de Produto" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ListaMovimentoProduto.aspx.cs" Inherits="Relatorios.ListaMovimentoProduto" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

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
        .style2
        {
            width: 125px;
        }
        </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="accountInfo">
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
        <table border="1" class="style1">
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                </td>
                <td>
                    <asp:TextBox ID="txtData_1" runat="server" BackColor="Moccasin"></asp:TextBox>
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                </td>
                <td>
                    <asp:TextBox ID="txtData_2" runat="server" BackColor="NavajoWhite"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtData_3" runat="server" BackColor="PeachPuff"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtData_4" runat="server" BackColor="Bisque"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtData_5" runat="server" BackColor="BlanchedAlmond"></asp:TextBox>
                </td>
            </tr>
        </table>
        </fieldset>
        <fieldset class="login">
        <table border="1" class="style1">
            <tr>
                <td>
                <asp:GridView id="GridViewProduto" runat="server" Width="100%" 
                    CssClass="DataGrid_Padrao" PageSize="100000" AllowPaging="True" 
                    AutoGenerateColumns="False" ShowFooter="true" OnRowDataBound="GridViewProduto_RowDataBound" ondatabound="GridViewProduto_DataBound"> 
	                <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
	                <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
	                <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="Filial">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralFilial"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="venda_5" HeaderText="Venda" />
                        <asp:BoundField DataField="estoque_5" HeaderText="Estoque"/>
                        <asp:BoundField DataField="recebido_5" HeaderText="Recebido" />
                        <asp:BoundField DataField="venda_total" HeaderText="Vd Tot"/>
                        <asp:BoundField DataField="recebido_total" HeaderText="Rec Tot"/>
                        <asp:BoundField DataField="venda_4" HeaderText="Venda"/>
                        <asp:BoundField DataField="estoque_4" HeaderText="Estoque"/>
                        <asp:BoundField DataField="recebido_4" HeaderText="Recebido"/>
                        <asp:BoundField DataField="venda_3" HeaderText="Venda"/>
                        <asp:BoundField DataField="estoque_3" HeaderText="Estoque"/>
                        <asp:BoundField DataField="recebido_3" HeaderText="Recebido"/>
                        <asp:BoundField DataField="venda_2" HeaderText="Venda"/>
                        <asp:BoundField DataField="estoque_2" HeaderText="Estoque"/>
                        <asp:BoundField DataField="recebido_2" HeaderText="Recebido"/>
                        <asp:BoundField DataField="venda_1" HeaderText="Venda"/>
                        <asp:BoundField DataField="estoque_1" HeaderText="Estoque"/>
                        <asp:BoundField DataField="recebido_1" HeaderText="Recebido"/>
                    </Columns>
	                <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                </asp:GridView>
                </td>
            </tr>
        </table>
        </fieldset>
    </div>
</asp:Content>
