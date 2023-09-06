<%@ Page Title="Controle Risco/Corte" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="prod_fila_riscocorte.aspx.cs" Inherits="Relatorios.prod_fila_riscocorte" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
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
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Gestão
                    de Ordem de Corte&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Risco/Corte</span>
                <div style="float: right; padding: 0;">
                    <a href="prod_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Controle de Risco/Corte</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>Produção
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 200px;">
                                <asp:DropDownList ID="ddlProducao" runat="server" Width="194px" Height="21px">
                                    <asp:ListItem Value="0" Text="Selecione"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Risco"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Corte"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" OnClientClick="DesabilitarBotao(this);" />
                                &nbsp;&nbsp;<asp:Label ID="labMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div id="accordionT">
                                    <h3>Produção</h3>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">

                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvRiscoCorte" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvRiscoCorte_RowDataBound"
                                                            OnDataBound="gvRiscoCorte_DataBound" ShowFooter="true"
                                                            OnSorting="gvRiscoCorte_Sorting" AllowSorting="true" DataKeyNames="CODIGO">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="HB" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="HB" HeaderStyle-Width="100px" SortExpression="HB" />
                                                                <asp:BoundField DataField="DESC_DETALHE" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Detalhe" HeaderStyle-Width="130px" SortExpression="DESC_DETALHE" />
                                                                <asp:BoundField DataField="GRUPO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Grupo" HeaderStyle-Width="180px" SortExpression="GRUPO" />
                                                                <asp:BoundField DataField="NOME" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Nome" HeaderStyle-Width="200px" SortExpression="NOME" />
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Quantidade" HeaderStyle-Width="105px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litQtde" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Data Inicial" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tempo" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litTempo" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Liquidar" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center"
                                                                    SortExpression="LIQUIDAR">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litLiquidar" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <br />
                                <div style="float: right;">
                                    <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btImprimir" runat="server" Text="Imprimir" OnClick="btImprimir_Click"
                                        Width="100px" />
                                    <asp:HiddenField ID="hidOrdem" runat="server" Value="HB" />
                                    <asp:HiddenField ID="hidOrdenacao" runat="server" Value="ASC" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
