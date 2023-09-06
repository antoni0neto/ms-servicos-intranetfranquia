<%@ Page Title="Vale Mercadoria" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="rh_vale_mercadoria_mensal.aspx.cs" Inherits="Relatorios.rh_vale_mercadoria_mensal"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo do RH&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Vendedores&nbsp;&nbsp;>&nbsp;&nbsp;Vale Mercadoria</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Vale Mercadoria"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="labAno" runat="server" Text="Ano"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="labMes" runat="server" Text="Mês"></asp:Label>
                        </td>
                        <td>Filial
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 210px;">
                            <asp:DropDownList runat="server" ID="ddlAno" Height="22px" Width="204px" DataTextField="ANO"
                                DataValueField="ANO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 210px;">
                            <asp:DropDownList runat="server" ID="ddlMes" Height="22px" Width="204px">
                                <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                <asp:ListItem Value="1" Text="JANEIRO"></asp:ListItem>
                                <asp:ListItem Value="2" Text="FEVEREIRO"></asp:ListItem>
                                <asp:ListItem Value="3" Text="MARÇO"></asp:ListItem>
                                <asp:ListItem Value="4" Text="ABRIL"></asp:ListItem>
                                <asp:ListItem Value="5" Text="MAIO"></asp:ListItem>
                                <asp:ListItem Value="6" Text="JUNHO"></asp:ListItem>
                                <asp:ListItem Value="7" Text="JULHO"></asp:ListItem>
                                <asp:ListItem Value="8" Text="AGOSTO"></asp:ListItem>
                                <asp:ListItem Value="9" Text="SETEMBRO"></asp:ListItem>
                                <asp:ListItem Value="10" Text="OUTUBRO"></asp:ListItem>
                                <asp:ListItem Value="11" Text="NOVEMBRO"></asp:ListItem>
                                <asp:ListItem Value="12" Text="DEZEMBRO"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 256px;">
                            <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                Height="22px" Width="250px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />
                        </td>

                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvVendedores" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                    Style="background: white" OnRowDataBound="gvVendedores_RowDataBound"
                                    OnDataBound="gvVendedores_DataBound"
                                    OnSorting="gvVendedores_Sorting" AllowSorting="true"
                                    ShowFooter="true">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Left" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Filial" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            SortExpression="FILIAL">
                                            <ItemTemplate>
                                                <asp:Literal ID="litFilial" runat="server" Text='<%# Bind("FILIAL") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CPF" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            SortExpression="CPF">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCPF" runat="server" Text='<%# Bind("CPF") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nome" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            SortExpression="NOME">
                                            <ItemTemplate>
                                                <asp:Literal ID="litNome" runat="server" Text='<%# Bind("NOME") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cargo" HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            SortExpression="DESC_CARGO">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCargo" runat="server" Text='<%# Bind("DESC_CARGO") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantidade" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                            SortExpression="QTDE">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdePeca" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Venda Bruta" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            SortExpression="VALOR_VENDA_BRUTA">
                                            <ItemTemplate>
                                                <asp:Literal ID="litVendaBruta" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Valor Pago" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            SortExpression="VALOR_PAGO">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValorPago" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Desconto" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            SortExpression="DESCONTO">
                                            <ItemTemplate>
                                                <asp:Literal ID="litDesconto" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
