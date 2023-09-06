<%@ Page Title="Cliente Handclub" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="gest_confere_hand_cliente.aspx.cs" Inherits="Relatorios.gest_confere_hand_cliente"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Gestão&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Cliente Handclub</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Cliente Handclub"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>CPF
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 220px;">
                            <asp:TextBox ID="txtCPFFiltro" runat="server" Width="210px" MaxLength="11" onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                        </td>
                    </tr>

                    <tr>
                        <td colspan="2">
                            <fieldset>
                                <legend>Cliente</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>Codigo do Cliente
                                        </td>
                                        <td>CPF
                                        </td>
                                        <td>Cliente
                                        </td>
                                        <td>DDD
                                        </td>
                                        <td>Telefone
                                        </td>
                                        <td>E-Mail
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:TextBox ID="txtCodigoCliente" runat="server" Width="140px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 150px;">
                                            <asp:TextBox ID="txtCPF" runat="server" Width="140px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 300px;">
                                            <asp:TextBox ID="txtCliente" runat="server" Width="290px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 80px;">
                                            <asp:TextBox ID="txtDDD" runat="server" Width="70px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 130px;">
                                            <asp:TextBox ID="txtTelefone" runat="server" Width="120px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEmail" runat="server" Width="400px" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <fieldset>
                                <legend>Pontos</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>Saldo de Pontos
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:TextBox ID="txtSaldoPonto" runat="server" Width="140px" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvCampanha" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                    Style="background: white" OnRowDataBound="gvCampanha_RowDataBound" OnDataBound="gvCampanha_DataBound"
                                                    ShowFooter="true">
                                                    <HeaderStyle BackColor="Gainsboro" />
                                                    <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Left" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Código Programa" HeaderStyle-Width="170px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litCodigoPrograma" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Código Campanha" HeaderStyle-Width="170px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litCodigoCampanha" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Campanha" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litCampanha" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Pontos" HeaderStyle-Width="170px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litPonto" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <fieldset>
                                <legend>Pontos Acumulados</legend>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvPontoAcumulado" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                        Style="background: white" OnRowDataBound="gvPonto_RowDataBound" OnDataBound="gvPontoAcumulado_DataBound"
                                        ShowFooter="true">
                                        <HeaderStyle BackColor="Gainsboro" />
                                        <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Left" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Filial" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litFilial" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ticket" HeaderStyle-Width="170px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data Venda" HeaderStyle-Width="170px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDataVenda" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="" HeaderStyle-Width="160px" />
                                            <asp:TemplateField HeaderText="Valor Pago" HeaderStyle-Width="170px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValorPago" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pontos Acumulados" HeaderStyle-Width="170px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPonto" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <fieldset>
                                <legend>Pontos Resgatados</legend>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvPontoResgatado" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                        Style="background: white" OnRowDataBound="gvPonto_RowDataBound" OnDataBound="gvPontoResgatado_DataBound"
                                        ShowFooter="true">
                                        <HeaderStyle BackColor="Gainsboro" />
                                        <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Left" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Filial" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litFilial" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ticket" HeaderStyle-Width="170px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data Venda" HeaderStyle-Width="170px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDataVenda" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valor Venda Bruta" HeaderStyle-Width="170px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValorBruto" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valor Pago" HeaderStyle-Width="170px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValorPago" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Desconto" HeaderStyle-Width="170px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDesconto" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
