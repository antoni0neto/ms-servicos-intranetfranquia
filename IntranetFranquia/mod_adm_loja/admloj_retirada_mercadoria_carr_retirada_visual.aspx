<%@ Page Title="Pedidos de Transferência/Devolução" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="admloj_retirada_mercadoria_carr_retirada_visual.aspx.cs" Inherits="Relatorios.admloj_retirada_mercadoria_carr_retirada_visual" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://code.jquery.com/jquery.js"></script>
    <script src="js/jquery-ui.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="jquery-1.6.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="login">
        <legend>Pedidos de Transferência/Devolução</legend>
        <div>
            <fieldset>
                <legend>Produtos</legend>
                <div class="rounded_corners">
                    <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound"
                        ShowFooter="true" DataKeyNames="">
                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="labColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgProduto" runat="server" Width="90%" ImageAlign="AbsMiddle" ToolTip="" AlternateText="" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="PRODUTO" HeaderText="Produto" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="NOME" HeaderText="Nome" HeaderStyle-Width="220px" />
                            <asp:TemplateField HeaderText="Cor" HeaderStyle-Width="220px">
                                <ItemTemplate>
                                    <asp:Label ID="labCor" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TAMANHO" HeaderText="Grade" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="QTDE_SOL" HeaderText="Qtde" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="" HeaderText="" HeaderStyle-Width="" />

                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset>

            <br />
            <br />
        </div>
    </fieldset>
</asp:Content>
