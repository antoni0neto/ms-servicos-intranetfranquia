<%@ Page Title="Total Vendas Mês por Cliente" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="fin_confere_venda_mensal.aspx.cs" Inherits="Relatorios.fin_confere_venda_mensal"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Clientes&nbsp;&nbsp;>&nbsp;&nbsp;Total Vendas Mês por Cliente</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Total Vendas Mês por Cliente"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="labAno" runat="server" Text="Ano"></asp:Label>
                            <asp:HiddenField ID="hidTela" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="labMes" runat="server" Text="Mês"></asp:Label>
                        </td>
                        <td>Filial
                        </td>
                        <td>CPF
                        </td>
                        <td>
                            <asp:Label ID="labQtdeTickets" runat="server" Text="Qtde Tickets Maior ou Igual"></asp:Label>
                        </td>
                        <td>Funcionário</td>
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
                        <td style="width: 220px;">
                            <asp:TextBox ID="txtCPF" runat="server" Width="210px" MaxLength="11" onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                        </td>
                        <td style="width: 220px;">
                            <asp:TextBox ID="txtQtdeTickets" runat="server" Width="210px" MaxLength="3" Text="4" onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                        </td>
                        <td style="width: 150px;">
                            <asp:DropDownList runat="server" ID="ddlFuncionario"
                                Height="21px" Width="144px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="SIM" Text="Sim"></asp:ListItem>
                                <asp:ListItem Value="NÃO" Text="Não" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />
                        </td>

                    </tr>
                    <tr>
                        <td colspan="7">
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvClientes" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                    Style="background: white" OnRowDataBound="gvClientes_RowDataBound"
                                    OnSorting="gvClientes_Sorting" AllowSorting="true"
                                    ShowFooter="true"
                                    DataKeyNames="CPF">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbAcompanhar" runat="server" Checked="false" OnCheckedChanged="cbAcompanhar_CheckedChanged" AutoPostBack="true" />
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
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Button ID="btAbrir" runat="server" Text="Abrir Vendas" Width="100px" OnClick="btAbrir_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">&nbsp;
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
