<%@ Page Title="Clientes" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="crm_cliente.aspx.cs" Inherits="Relatorios.crm_cliente"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .pageStyl a, .pageStyl span {
            display: block;
            height: 15px;
            width: 15px;
            font-weight: bold;
            text-align: center;
            text-decoration: none;
        }

        .pageStyl a {
            background-color: #f5f5f5;
            color: #969696;
            border: 1px solid #969696;
        }

        .pageStyl span {
            background-color: #A1DCF2;
            color: #000;
            border: 1px solid #3AC0F2;
        }
    </style>
    <script type="text/javascript" src="../js/js.js"></script>

    <script type="text/javascript">
        function openwindow(l) {
            window.open(l, "CRM_CLIX", "menubar=1,resizable=0,width=1600,height=800");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de CRM&nbsp;&nbsp;>&nbsp;&nbsp;Clientes&nbsp;&nbsp;>&nbsp;&nbsp;Clientes</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Clientes"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Trimestre</td>
                        <td>CPF</td>
                        <td>Nome</td>
                        <td>Status</td>
                        <td>Recência</td>
                        <td>Frequência</td>
                        <td>Valor</td>
                        <td>CLI4F</td>
                    </tr>
                    <tr>
                        <td style="width: 190px;">
                            <asp:DropDownList ID="ddlTri" runat="server" Width="184px" DataValueField="DATA" DataTextField="TRI" Height="21px">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 200px;">
                            <asp:TextBox ID="txtCPF" runat="server" Width="190px"></asp:TextBox>
                        </td>
                        <td style="width: 200px;">
                            <asp:TextBox ID="txtNome" runat="server" Width="190px"></asp:TextBox>
                        </td>
                        <td style="width: 170px;">
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="164px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="A" Text="Ativo"></asp:ListItem>
                                <asp:ListItem Value="I" Text="Inativo"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 170px;">
                            <asp:DropDownList ID="ddlRecencia" runat="server" Width="164px" Height="21px">
                                <asp:ListItem Value="0" Text=""></asp:ListItem>
                                <asp:ListItem Value="1" Text="1 - 13 A 24 meses"></asp:ListItem>
                                <asp:ListItem Value="2" Text="2 - 7 A 12 meses"></asp:ListItem>
                                <asp:ListItem Value="3" Text="3 - 1 A 6 meses"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 170px;">
                            <asp:DropDownList ID="ddlFrequencia" runat="server" Width="164px" Height="21px">
                                <asp:ListItem Value="0" Text=""></asp:ListItem>
                                <asp:ListItem Value="1" Text="1 - 1 A 3 Tickets"></asp:ListItem>
                                <asp:ListItem Value="2" Text="2 - 4 A 6 Tickets"></asp:ListItem>
                                <asp:ListItem Value="3" Text="3 - > 6 Tickets"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 170px;">
                            <asp:DropDownList ID="ddlValor" runat="server" Width="164px" Height="21px">
                                <asp:ListItem Value="0" Text=""></asp:ListItem>
                                <asp:ListItem Value="1" Text="1 - R$ 0,01 a R$ 300,00"></asp:ListItem>
                                <asp:ListItem Value="2" Text="2 - R$ 300,01 a R$ 600,00"></asp:ListItem>
                                <asp:ListItem Value="3" Text="3 - > R$ 600,00"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCLI4F" runat="server" Width="155px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Ciclo Atual</td>
                        <td>Ciclo Anterior</td>
                        <td>Compra Online</td>
                        <td>Tipo Cliente</td>
                        <td>Comprou Última Ação?</td>
                        <td>Participou de Ação?</td>
                        <td>Ação</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlCicloAtual" runat="server" Width="184px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="1-Ativação" Text="Ativação"></asp:ListItem>
                                <asp:ListItem Value="2-Potencialização" Text="Potencialização"></asp:ListItem>
                                <asp:ListItem Value="3-Fidelização" Text="Fidelização"></asp:ListItem>
                                <asp:ListItem Value="4-Recuperação" Text="Recuperação"></asp:ListItem>
                                <asp:ListItem Value="5-Reativação" Text="Reativação"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCicloAnterior" runat="server" Width="194px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="1-Ativação" Text="Ativação"></asp:ListItem>
                                <asp:ListItem Value="2-Potencialização" Text="Potencialização"></asp:ListItem>
                                <asp:ListItem Value="3-Fidelização" Text="Fidelização"></asp:ListItem>
                                <asp:ListItem Value="4-Recuperação" Text="Recuperação"></asp:ListItem>
                                <asp:ListItem Value="5-Reativação" Text="Reativação"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCompraOnline" runat="server" Width="194px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTipoCliente" runat="server" Width="164px" Height="21px" DataTextField="TIPO" DataValueField="CODIGO">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlComprouUltimaAcao" runat="server" Width="164px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTeveAcao" runat="server" Width="164px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="ddlAcao" runat="server" Width="325px" Height="21px" DataTextField="ACAO" DataValueField="CODIGO">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="120px" OnClick="btBuscar_Click" />&nbsp;&nbsp;<asp:Label ID="labErro" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:DropDownList ID="ddlPaginaNumero" runat="server" Width="63px" OnSelectedIndexChanged="ddlPaginaNumero_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="10" Text="10"></asp:ListItem>
                                <asp:ListItem Value="50" Text="50" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="100" Text="100"></asp:ListItem>
                                <asp:ListItem Value="200" Text="200"></asp:ListItem>
                                <asp:ListItem Value="400" Text="400"></asp:ListItem>
                                <asp:ListItem Value="600" Text="600"></asp:ListItem>
                                <asp:ListItem Value="800" Text="800"></asp:ListItem>
                                <asp:ListItem Value="1000" Text="1000"></asp:ListItem>
                                <asp:ListItem Value="2000" Text="2000"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td colspan="2" style="text-align: right;">
                            <span style="color: red;">
                                <asp:Label ID="labQtdeCliente" runat="server" Text="0"></asp:Label>
                                clientes encontrados</span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvCliente" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvCliente_RowDataBound" OnDataBound="gvCliente_DataBound" ShowFooter="true"
                                    OnPageIndexChanging="gvCliente_PageIndexChanging" AllowPaging="true" PageSize="50"
                                    OnSorting="gvCliente_Sorting" AllowSorting="true"
                                    DataKeyNames="">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Size="Smaller"></HeaderStyle>
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                    <PagerStyle HorizontalAlign="Left" CssClass="pageStyl" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-Width="25px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="corTD">
                                            <ItemTemplate>
                                                <asp:Literal ID="litAbrirCliente" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CÓD. Cliente" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="CODIGO_CLIENTE">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCODCliente" runat="server" Text='<%# Bind("CODIGO_CLIENTE") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cliente" HeaderStyle-Width="180px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="CLIENTE">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCliente" runat="server" Text='<%# Bind("CLIENTE") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CPF" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="CPF">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCPF" runat="server" Text='<%# Bind("CPF") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Recência" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="NRECENCIA">
                                            <ItemTemplate>
                                                <asp:Literal ID="litRecencia" runat="server" Text='<%# Bind("NRECENCIA") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Frequência" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="NFREQUENCIA">
                                            <ItemTemplate>
                                                <asp:Literal ID="litFrequencia" runat="server" Text='<%# Bind("NFREQUENCIA") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Valor" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="NVALOR">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValor" runat="server" Text='<%# Bind("NVALOR") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Últ. Ação" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="ULTIMA_ACAO">
                                            <ItemTemplate>
                                                <asp:Literal ID="litUltimaAcao" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ação" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="">
                                            <ItemTemplate>
                                                <asp:Literal ID="litAcao" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tickets" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="TICKETS">
                                            <ItemTemplate>
                                                <asp:Literal ID="litTickets" runat="server" Text='<%# Bind("TICKETS") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Valor Total" HeaderStyle-Width="85px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="VALOR_TOTAL">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValorTotal" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Últ Compra" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="ULTIMA_COMPRA">
                                            <ItemTemplate>
                                                <asp:Literal ID="litUltimaCompra" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tkt Online" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="TICKETS_ONLINE">
                                            <ItemTemplate>
                                                <asp:Literal ID="litTicketsOnline" runat="server" Text='<%# Bind("TICKETS_ONLINE") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Val Online" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="VALOR_TOTAL_ONLINE">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValorTotalOnline" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Últ Compra On" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="ULTIMA_COMPRA_ONLINE">
                                            <ItemTemplate>
                                                <asp:Literal ID="litUltimaCompraOnline" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="4F" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="CLI4F">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCLI4F" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Ciclo Atual" HeaderStyle-Width="95px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="CICLO_ATUAL">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCicloAtual" runat="server" Text='<%# Bind("CICLO_ATUAL") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ciclo Ant" HeaderStyle-Width="95px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="Smaller" ItemStyle-Font-Size="Smaller"
                                            SortExpression="CICLO_ANTERIOR">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCicloAnt" runat="server" Text='<%# Bind("CICLO_ANTERIOR") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>



                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
