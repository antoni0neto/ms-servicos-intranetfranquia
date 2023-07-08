<%@ Page Title="Conta Partes de Produto" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ContaPartesProdutoMaster.aspx.cs" Inherits="Relatorios.ContaPartesProdutoMaster" %>

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
                        <asp:GridView id="GridViewKitNatal" runat="server" Width="100%" CssClass="DataGrid_Padrao" AutoGenerateColumns="False">
	                        <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
	                        <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
	                        <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
	                        <RowStyle HorizontalAlign="Center"></RowStyle>
	                        <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                            <Columns>
                                <asp:BoundField DataField="SUPERVISOR" HeaderText="Supervisor" />
                                <asp:BoundField DataField="FILIAL" HeaderText="Filial" />
                                <asp:BoundField DataField="PERC_23_11" HeaderText="23/11" />
                                <asp:BoundField DataField="PERC_24_11" HeaderText="24/11" />
                                <asp:BoundField DataField="PERC_25_11" HeaderText="25/11" />
                                <asp:BoundField DataField="PERC_26_11" HeaderText="26/11" />
                                <asp:BoundField DataField="PERC_27_11" HeaderText="27/11" />
                                <asp:BoundField DataField="PERC_28_11" HeaderText="28/11" />
                                <asp:BoundField DataField="PERC_29_11" HeaderText="29/11" />
                                <asp:BoundField DataField="PERC_30_11" HeaderText="30/11" />
                                <asp:BoundField DataField="PERC_01_12" HeaderText="01/12" />
                                <asp:BoundField DataField="PERC_02_12" HeaderText="02/12" />
                                <asp:BoundField DataField="PERC_03_12" HeaderText="03/12" />
                                <asp:BoundField DataField="PERC_04_12" HeaderText="04/12" />
                                <asp:BoundField DataField="PERC_05_12" HeaderText="05/12" />
                                <asp:BoundField DataField="PERC_06_12" HeaderText="06/12" />
                                <asp:BoundField DataField="PERC_07_12" HeaderText="07/12" />
                                <asp:BoundField DataField="PERC_08_12" HeaderText="08/12" />
                                <asp:BoundField DataField="PERC_09_12" HeaderText="09/12" />
                                <asp:BoundField DataField="PERC_10_12" HeaderText="10/12" />
                                <asp:BoundField DataField="PERC_11_12" HeaderText="11/12" />
                                <asp:BoundField DataField="PERC_12_12" HeaderText="12/12" />
                                <asp:BoundField DataField="PERC_13_12" HeaderText="13/12" />
                                <asp:BoundField DataField="PERC_14_12" HeaderText="14/12" />
                                <asp:BoundField DataField="PERC_15_12" HeaderText="15/12" />
                                <asp:BoundField DataField="PERC_16_12" HeaderText="16/12" />
                                <asp:BoundField DataField="PERC_17_12" HeaderText="17/12" />
                                <asp:BoundField DataField="PERC_18_12" HeaderText="18/12" />
                                <asp:BoundField DataField="PERC_19_12" HeaderText="19/12" />
                                <asp:BoundField DataField="PERC_20_12" HeaderText="20/12" />
                                <asp:BoundField DataField="PERC_21_12" HeaderText="21/12" />
                                <asp:BoundField DataField="PERC_TOTAL" HeaderText="Total" />
                            </Columns>
	                        <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
