<%@ Page Title="Lista Aprovação de Preço" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="prod_cad_produto_custo_aprov_lista.aspx.cs" Inherits="Relatorios.prod_cad_produto_custo_aprov_lista"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Custo/Preço de Produto&nbsp;&nbsp;>&nbsp;&nbsp;Lista Aprovação de Preço</span>
                <div style="float: right; padding: 0;">
                    <a href="prod_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1200px; margin-left: 8%;">
                <fieldset>
                    <legend>Lista Aprovação de Preço</legend>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                        <tr>
                            <td>
                                <asp:Button runat="server" ID="btAtualizar" Text="Atualizar" Width="115px" OnClick="btAtualizar_Click" OnClientClick="DesabilitarBotao(this);"
                                    Enabled="true" />
                                <asp:HiddenField ID="hidTipoCusto" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvProdutoSimulado" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                        Style="background: white" OnRowDataBound="gvProdutoSimulado_RowDataBound"
                                        OnSorting="gvProdutoSimulado_Sorting" AllowSorting="true">
                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                        <RowStyle HorizontalAlign="Left" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Coleção" HeaderStyle-Width="200px" SortExpression="COLECAO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColecao" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PRODUTO" HeaderText="Produto" HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-HorizontalAlign="Center" SortExpression="PRODUTO" />
                                            <asp:BoundField DataField="GRUPO" HeaderText="Grupo" HeaderStyle-Width="180px" SortExpression="GRUPO" />
                                            <asp:BoundField DataField="GRIFFE" HeaderText="Griffe" HeaderStyle-Width="180px" SortExpression="GRIFFE" />
                                            <asp:BoundField DataField="NOME" HeaderText="Nome" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" SortExpression="NOME" />
                                            <asp:TemplateField HeaderText="Mostruário" HeaderStyle-Width="160px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" SortExpression="MOSTRUARIO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litMostruario" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtPesquisar" runat="server" Width="19px" ImageUrl="~/Image/search.png"
                                                        ToolTip="Pesquisar" OnClick="ibtPesquisar_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td>&nbsp;
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
