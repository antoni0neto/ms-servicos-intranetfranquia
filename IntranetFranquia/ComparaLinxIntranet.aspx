<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ComparaLinxIntranet.aspx.cs" Inherits="Relatorios.ComparaLinxIntranet" %>

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
            background-color:White;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="accountInfo">
        <fieldset>
        </fieldset>
        <fieldset class="login">
            <legend>Busca Grupos de Produto</legend>
            <div style="width: 200px;"  class="alinhamento">
                <label>Coleção:&nbsp; </label>
                <asp:DropDownList runat="server" ID="ddlColecao" DataValueField="CODIGO_COLECAO" DataTextField="DESCRICAO" Height="26px" 
                    Width="200px" ondatabound="ddlColecao_DataBound"></asp:DropDownList>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btGrupos" Text="Buscar Grupos de Produto" OnClick="btGrupos_Click"/>
        </div>
        <fieldset class="login">
            <legend>Busca de Produtos Divergentes</legend>
            <div style="width: 400px;" class="alinhamento">
                <div style="width: 200px;"  class="alinhamento">
                    <label>Griffe:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlGriffe" DataValueField="CODIGO_GRIFFE" DataTextField="DESCRICAO" Height="26px" 
                        Width="200px" ondatabound="ddlGriffe_DataBound"></asp:DropDownList>
                </div>
                <div style="width: 200px;"  class="alinhamento">
                    <label>Grupo:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlGrupo" DataValueField="CODIGO_GRUPO" DataTextField="GRUPO" Height="26px" 
                        Width="200px" ondatabound="ddlGrupo_DataBound"></asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btPesquisar" Text="Buscar Divergências" OnClick="btPesquisar_Click" Enabled = "false"/>
            <asp:ValidationSummary ID="ValidationSummaryColecao" runat="server" ShowMessageBox="true" ShowSummary="false" />
        </div>
        <table border="1" class="style1">
        <tr>
            <td>
                <fieldset class="login">
                <legend>Preço Fob diferente</legend>
                <asp:GridView id="GridViewFob" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333" style="background:white">
	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="codigoProduto" HeaderText="Código" />
                        <asp:BoundField DataField="descricaoProduto" HeaderText="Descrição" />
                        <asp:BoundField DataField="descricaoProdutoCor" HeaderText="Cor" />
                        <asp:BoundField DataField="fobLinx" HeaderText="Fob Linx" />
                        <asp:BoundField DataField="fobIntranet" HeaderText="Fob Intranet" />
                    </Columns>
                </asp:GridView>
                </fieldset>
            </td>
            <td>
                <fieldset class="login">
                <legend>Existe Linx, não existe Intranet</legend>
                <asp:GridView id="GridViewLinx" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333" style="background:white">
	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="codigoProduto" HeaderText="Código" />
                        <asp:BoundField DataField="descricaoProduto" HeaderText="Descrição" />
                        <asp:BoundField DataField="descricaoProdutoCor" HeaderText="Cor" />
                    </Columns>
                </asp:GridView>
                </fieldset>
            </td>
            <td>
                <fieldset class="login">
                <legend>Existe Intranet, não existe Linx</legend>
                <asp:GridView id="GridViewIntranet" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333" style="background:white">
	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="codigoProduto" HeaderText="Código" />
                        <asp:BoundField DataField="descricaoProduto" HeaderText="Descrição" />
                        <asp:BoundField DataField="descricaoProdutoCor" HeaderText="Cor" />
                    </Columns>
                </asp:GridView>
                </fieldset>
            </td>
        </tr>
        </table>
    </div>
</asp:Content>
