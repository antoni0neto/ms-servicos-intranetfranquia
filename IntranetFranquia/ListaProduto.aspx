<%@ Page Title="Lista Movimento de Produto" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ListaProduto.aspx.cs" Inherits="Relatorios.ListaProduto" %>

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
        <table border="1" class="style1">
            <tr>
                <td>
                    <label>Coleção:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlColecao" DataValueField="COLECAO" DataTextField="DESC_COLECAO" Height="22px" 
                        Width="198px" ondatabound="ddlColecao_DataBound"></asp:DropDownList>
                </td>
                <td>
                    <label>Produto:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlProduto" DataValueField="PRODUTO1" DataTextField="DESC_PRODUTO" Height="22px" 
                        Width="198px" ondatabound="ddlProduto_DataBound"></asp:DropDownList>
                </td>
            </tr>
        </table>
        </fieldset>
        <fieldset class="login">
        <table border="1" class="style1">
            <tr>
                <td>
                    <asp:Button runat="server" ID="btProduto" Text="Produto" OnClick="btProduto_Click"/>
                </td>
                <td>
                    <asp:Button runat="server" ID="btMovimento" Text="Movimento" OnClick="btMovimento_Click"/>
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                </td>
                <td>
                    <asp:TextBox ID="txtData_1" runat="server" BackColor="Moccasin"></asp:TextBox>
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                </td>
                <td>
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
                <asp:GridView id="GridViewMovimentoProduto" runat="server" Width="100%" 
                    CssClass="DataGrid_Padrao" PageSize="100000" AllowPaging="True" 
                    AutoGenerateColumns="False" ShowFooter="true" OnRowDataBound="GridViewMovimentoProduto_RowDataBound" ondatabound="GridViewMovimentoProduto_DataBound"> 
	                <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
	                <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
	                <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="Filial">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralNomeFilial"/>
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
