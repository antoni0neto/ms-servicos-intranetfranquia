<%@ Page Title="Total Produtos com Defeito Loja" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="admfis_total_itens_defeito.aspx.cs" Inherits="Relatorios.admfis_total_itens_defeito" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo ADM Nota Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Administrativo Nota Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Total Produtos com Defeito Loja</span>
                <div style="float: right; padding: 0;">
                    <a href="admfis_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <br />
                    <div class="rounded_corners">
                        <asp:GridView ID="gvTotalNotaDefeito" runat="server" Width="100%" AutoGenerateColumns="False"
                            ForeColor="#333333" Style="background: white"
                            OnSorting="gvTotalNotaDefeito_Sorting" OnDataBound="gvTotalNotaDefeito_DataBound" AllowSorting="true" ShowFooter="true">
                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                            <Columns>
                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="FILIAL" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                    HeaderText="Filial" HeaderStyle-Width="250px" SortExpression="FILIAL" />
                                <asp:TemplateField HeaderText="Total de Itens" HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Center"
                                    SortExpression="CONTADOR">
                                    <ItemTemplate>
                                        <asp:Literal ID="litContador" runat="server" Text='<%# Bind("CONTADOR") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                    HeaderText="" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
