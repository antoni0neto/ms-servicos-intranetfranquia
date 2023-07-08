<%@ Page Title="Lista Movimento de Grupo de Produto" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ListaMovimentoGrupo.aspx.cs" Inherits="Relatorios.ListaMovimentoGrupo" %>

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
        <legend>Grupo Feminino</legend>
        <table border="1" class="style1">
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                </td>
                <td>
                    <asp:TextBox ID="txtProdutoFem_1" runat="server" BackColor="Moccasin"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtProdutoFem_2" runat="server" BackColor="NavajoWhite"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtProdutoFem_3" runat="server" BackColor="PeachPuff"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtProdutoFem_4" runat="server" BackColor="Bisque"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtProdutoFem_5" runat="server" BackColor="PeachPuff"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtProdutoFem_6" runat="server" BackColor="Bisque"></asp:TextBox>
                </td>
            </tr>
        </table>
        </fieldset>
        <fieldset class="login">
        <table border="1" class="style1">
            <tr>
                <td>
                <asp:GridView id="GridViewGrupoFeminino" runat="server" Width="100%" 
                    CssClass="DataGrid_Padrao" PageSize="100000" AllowPaging="True" 
                    AutoGenerateColumns="False" ShowFooter="true" ondatabound="GridViewGrupoFeminino_DataBound"> 
	                <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
	                <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
	                <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="loja" HeaderText="Loja" />
                        <asp:BoundField DataField="cota" HeaderText="% Cota" />
                        <asp:BoundField DataField="qtde_prod_1_sem_1" HeaderText="Qtde 1a Sem"/>
                        <asp:BoundField DataField="qtde_prod_1_sem_2" HeaderText="Qtde 2a Sem" />
                        <asp:BoundField DataField="qtde_prod_1_sem_3" HeaderText="Qtde 3a Sem" />
                        <asp:BoundField DataField="qtde_prod_1_sem_4" HeaderText="Qtde 4a Sem" />
                        <asp:BoundField DataField="qtde_prod_1_total" HeaderText="Qtde Total" />
                        <asp:BoundField DataField="qtde_prod_2_sem_1" HeaderText="Qtde 1a Sem"/>
                        <asp:BoundField DataField="qtde_prod_2_sem_2" HeaderText="Qtde 2a Sem" />
                        <asp:BoundField DataField="qtde_prod_2_sem_3" HeaderText="Qtde 3a Sem" />
                        <asp:BoundField DataField="qtde_prod_2_sem_4" HeaderText="Qtde 4a Sem" />
                        <asp:BoundField DataField="qtde_prod_2_total" HeaderText="Qtde Total" />
                        <asp:BoundField DataField="qtde_prod_3_sem_1" HeaderText="Qtde 1a Sem"/>
                        <asp:BoundField DataField="qtde_prod_3_sem_2" HeaderText="Qtde 2a Sem" />
                        <asp:BoundField DataField="qtde_prod_3_sem_3" HeaderText="Qtde 3a Sem" />
                        <asp:BoundField DataField="qtde_prod_3_sem_4" HeaderText="Qtde 4a Sem" />
                        <asp:BoundField DataField="qtde_prod_3_total" HeaderText="Qtde Total" />
                        <asp:BoundField DataField="qtde_prod_4_sem_1" HeaderText="Qtde 1a Sem"/>
                        <asp:BoundField DataField="qtde_prod_4_sem_2" HeaderText="Qtde 2a Sem" />
                        <asp:BoundField DataField="qtde_prod_4_sem_3" HeaderText="Qtde 3a Sem" />
                        <asp:BoundField DataField="qtde_prod_4_sem_4" HeaderText="Qtde 4a Sem" />
                        <asp:BoundField DataField="qtde_prod_4_total" HeaderText="Qtde Total" />
                        <asp:BoundField DataField="qtde_prod_5_sem_1" HeaderText="Qtde 1a Sem"/>
                        <asp:BoundField DataField="qtde_prod_5_sem_2" HeaderText="Qtde 2a Sem" />
                        <asp:BoundField DataField="qtde_prod_5_sem_3" HeaderText="Qtde 3a Sem" />
                        <asp:BoundField DataField="qtde_prod_5_sem_4" HeaderText="Qtde 4a Sem" />
                        <asp:BoundField DataField="qtde_prod_5_total" HeaderText="Qtde Total" />
                        <asp:BoundField DataField="qtde_prod_6_sem_1" HeaderText="Qtde 1a Sem"/>
                        <asp:BoundField DataField="qtde_prod_6_sem_2" HeaderText="Qtde 2a Sem" />
                        <asp:BoundField DataField="qtde_prod_6_sem_3" HeaderText="Qtde 3a Sem" />
                        <asp:BoundField DataField="qtde_prod_6_sem_4" HeaderText="Qtde 4a Sem" />
                        <asp:BoundField DataField="qtde_prod_6_total" HeaderText="Qtde Total" />
                    </Columns>
	                <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                </asp:GridView>
                </td>
            </tr>
        </table>
        </fieldset>
        <fieldset class="login">
        <legend>Grupo Masculino</legend>
        <table border="1" class="style1">
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                </td>
                <td>
                    <asp:TextBox ID="txtProdutoMas_1" runat="server" BackColor="Moccasin"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtProdutoMas_2" runat="server" BackColor="NavajoWhite"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtProdutoMas_3" runat="server" BackColor="PeachPuff"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtProdutoMas_4" runat="server" BackColor="Bisque"></asp:TextBox>
                </td>
            </tr>
        </table>
        </fieldset>
        <fieldset class="login">
        <table border="1" class="style1">
            <tr>
                <td>
                <asp:GridView id="GridViewGrupoMasculino" runat="server" Width="100%" 
                    CssClass="DataGrid_Padrao" PageSize="100000" AllowPaging="True" 
                    AutoGenerateColumns="False" ShowFooter="true" ondatabound="GridViewGrupoMasculino_DataBound"> 
	                <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
	                <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
	                <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="loja" HeaderText="Loja" />
                        <asp:BoundField DataField="cota" HeaderText="% Cota" />
                        <asp:BoundField DataField="qtde_prod_1_sem_1" HeaderText="Qtde 1a Sem"/>
                        <asp:BoundField DataField="qtde_prod_1_sem_2" HeaderText="Qtde 2a Sem" />
                        <asp:BoundField DataField="qtde_prod_1_sem_3" HeaderText="Qtde 3a Sem" />
                        <asp:BoundField DataField="qtde_prod_1_sem_4" HeaderText="Qtde 4a Sem" />
                        <asp:BoundField DataField="qtde_prod_1_total" HeaderText="Qtde Total" />
                        <asp:BoundField DataField="qtde_prod_2_sem_1" HeaderText="Qtde 1a Sem"/>
                        <asp:BoundField DataField="qtde_prod_2_sem_2" HeaderText="Qtde 2a Sem" />
                        <asp:BoundField DataField="qtde_prod_2_sem_3" HeaderText="Qtde 3a Sem" />
                        <asp:BoundField DataField="qtde_prod_2_sem_4" HeaderText="Qtde 4a Sem" />
                        <asp:BoundField DataField="qtde_prod_2_total" HeaderText="Qtde Total" />
                        <asp:BoundField DataField="qtde_prod_3_sem_1" HeaderText="Qtde 1a Sem"/>
                        <asp:BoundField DataField="qtde_prod_3_sem_2" HeaderText="Qtde 2a Sem" />
                        <asp:BoundField DataField="qtde_prod_3_sem_3" HeaderText="Qtde 3a Sem" />
                        <asp:BoundField DataField="qtde_prod_3_sem_4" HeaderText="Qtde 4a Sem" />
                        <asp:BoundField DataField="qtde_prod_3_total" HeaderText="Qtde Total" />
                        <asp:BoundField DataField="qtde_prod_4_sem_1" HeaderText="Qtde 1a Sem"/>
                        <asp:BoundField DataField="qtde_prod_4_sem_2" HeaderText="Qtde 2a Sem" />
                        <asp:BoundField DataField="qtde_prod_4_sem_3" HeaderText="Qtde 3a Sem" />
                        <asp:BoundField DataField="qtde_prod_4_sem_4" HeaderText="Qtde 4a Sem" />
                        <asp:BoundField DataField="qtde_prod_4_total" HeaderText="Qtde Total" />
                    </Columns>
	                <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                </asp:GridView>
                </td>
            </tr>
        </table>
        </fieldset>
    </div>
</asp:Content>
