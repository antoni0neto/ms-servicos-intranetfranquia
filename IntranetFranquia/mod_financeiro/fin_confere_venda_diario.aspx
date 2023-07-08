<%@ Page Title="Total Vendas Dia por Cliente" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="fin_confere_venda_diario.aspx.cs" Inherits="Relatorios.fin_confere_venda_diario"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Clientes&nbsp;&nbsp;>&nbsp;&nbsp;Total Vendas Dia por Cliente</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Total Vendas Dia por Cliente"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="labInicial" runat="server" Text="Período Inicial"></asp:Label>
                            <asp:HiddenField ID="hidTela" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="labFinal" runat="server" Text="Período Final"></asp:Label>
                        </td>
                        <td>Filial
                        </td>
                        <td>CPF
                        </td>
                        <td>
                            <asp:Label ID="labQtdeTickets" runat="server" Text="Qtde Tickets Maior ou Igual"></asp:Label>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 130px;">
                            <asp:TextBox ID="txtPeriodoInicial" runat="server" Width="120px" MaxLength="10" Style="text-align: right;"
                                onkeypress="return fnValidarData(event);"></asp:TextBox>
                        </td>
                        <td style="width: 130px;">
                            <asp:TextBox ID="txtPeriodoFinal" runat="server" Width="120px" MaxLength="10" Style="text-align: right;"
                                onkeypress="return fnValidarData(event);"></asp:TextBox>
                        </td>
                        <td style="width: 256px;">
                            <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                Height="22px" Width="250px">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 220px;">
                            <asp:TextBox ID="txtCPF" runat="server" Width="210px" MaxLength="11" onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                        </td>
                        <td style="width: 220px;">
                            <asp:TextBox ID="txtQtdeTickets" runat="server" Width="210px" MaxLength="3" Text="2" onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                        </td>

                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />
                        </td>

                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvClientes" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                    Style="background: white" OnRowDataBound="gvClientes_RowDataBound"
                                    OnSorting="gvClientes_Sorting" AllowSorting="true"
                                    ShowFooter="true">
                                    <HeaderStyle BackColor="GradientActiveCaption" />
                                    <FooterStyle BackColor="GradientActiveCaption" Font-Bold="true" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Data" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="DATA">
                                            <ItemTemplate>
                                                <asp:Literal ID="litData" runat="server" Text='<%# Bind("DATA") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CPF" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            SortExpression="CPF">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCPF" runat="server" Text='<%# Bind("CPF") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cliente" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            SortExpression="CLIENTE_VAREJO">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCliente" runat="server" Text='<%# Bind("CLIENTE_VAREJO") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Funcionário" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                            SortExpression="FUNCIONARIO">
                                            <ItemTemplate>
                                                <asp:Literal ID="litFuncionario" runat="server" Text='<%# Bind("FUNCIONARIO") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qtde de Tickets" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="QTDE_TICKETS">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeTickets" runat="server" Text='<%# Bind("QTDE_TICKETS") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="E-Mail" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            SortExpression="EMAIL">
                                            <ItemTemplate>
                                                <asp:Literal ID="litEmail" runat="server" Text='<%# Bind("EMAIL") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DDD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="DDD">
                                            <ItemTemplate>
                                                <asp:Literal ID="litDDD" runat="server" Text='<%# Bind("DDD") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Telefone" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            SortExpression="TELEFONE">
                                            <ItemTemplate>
                                                <asp:Literal ID="litTelefone" runat="server" Text='<%# Bind("TELEFONE") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Button ID="btHistHandclub" runat="server" Text="Hist Handclub" Width="100px" OnClick="btHistHandclub_Click" />
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
