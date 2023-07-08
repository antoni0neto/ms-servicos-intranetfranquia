<%@ Page Title="Alterar Forma de Pagamento Vale Funcionário" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="fin_altera_forma_pgto_funcionario.aspx.cs" Inherits="Relatorios.fin_altera_forma_pgto_funcionario" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
        }
    </style>
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://code.jquery.com/jquery.js"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>

    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Alterar Forma de Pagamento Vale Funcionário</span>
                <div style="float: right; padding: 0;">
                    <a href="fin_prod_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Alterar Forma de Pagamento Vale Funcionário</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>Filial
                            </td>
                            <td>Período Inicial
                            </td>
                            <td>Período Final
                            </td>
                            <td>Forma de Pagamento
                            </td>
                            <td>Ticket
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 300px">
                                <asp:DropDownList ID="ddlFilial" runat="server" Width="294px" Height="21px"
                                    DataTextField="FILIAL" DataValueField="COD_FILIAL">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 180px">
                                <asp:TextBox ID="txtDataInicial" runat="server" onkeypress="return fnValidarData(event);"
                                    Width="170px"></asp:TextBox>
                            </td>
                            <td style="width: 180px">
                                <asp:TextBox ID="txtDataFinal" runat="server" onkeypress="return fnValidarData(event);"
                                    Width="170px"></asp:TextBox>
                            </td>
                            <td style="width: 300px">
                                <asp:DropDownList ID="ddlFormaPgtoCliente" runat="server" Width="294px" Height="21px"
                                    DataTextField="FORMA_PGTO" DataValueField="COD_FORMA_PGTO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 190px">
                                <asp:TextBox ID="txtTicket" runat="server" Width="180px"></asp:TextBox>
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <div id="accordionP">
                                    <h3>Tickets</h3>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvTicket" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvTicket_RowDataBound"
                                                            ShowFooter="true"
                                                            DataKeyNames="CODIGO_FILIAL, TICKET, DATA_VENDA">
                                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                                                            <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="FILIAL" HeaderText="Filial" HeaderStyle-Width="250px" />
                                                                <asp:BoundField DataField="TICKET" HeaderText="Ticket" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                                <asp:TemplateField HeaderText="Data Venda" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litDataVenda" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="PGTO" HeaderText="Forma de Pagamento" HeaderStyle-Width="220px" />
                                                                <asp:TemplateField HeaderText="Valor Venda Bruta" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litValVendaBruta" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="NOME_VENDEDOR" HeaderText="Nome" />
                                                                <asp:BoundField DataField="DESC_CARGO" HeaderText="Cargo" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlFormaPgtoValeFuncionario" runat="server" Width="294px" Height="21px"
                                    DataTextField="FORMA_PGTO" DataValueField="COD_FORMA_PGTO">
                                </asp:DropDownList>
                            </td>
                            <td colspan="5">
                                <asp:Label ID="labErroBaixa" runat="server" Text="" ForeColor="Red"></asp:Label>
                                <asp:Button ID="btAtualizar" runat="server" Text="Atualizar" Width="130px" OnClick="btAtualizar_Click" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
