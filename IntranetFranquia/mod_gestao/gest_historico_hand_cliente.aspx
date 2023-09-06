<%@ Page Title="Histórico Cliente Handclub" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="gest_historico_hand_cliente.aspx.cs" Inherits="Relatorios.gest_historico_hand_cliente"
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
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../Image/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "../Image/plus.png");
            $(this).closest("tr").next().remove();
        });

        function openwindow(l) {
            window.open(l, "HANDCLUB", "menubar=1,resizable=0,width=1000,height=500");
        }

        function CopiarColarWhatsApp() {

            document.getElementById('MainContent_txtTextoWhatsApp').select();

            var copied;

            try {
                // Copy the text
                copied = document.execCommand('copy');
            }
            catch (ex) {
                copied = false;
            }

            if (copied) {
                alert("Msg WhatsApp copiada com sucesso.");
            }


        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Gestão&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Histórico Cliente Handclub</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Histórico Cliente Handclub"></asp:Label></legend>
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
                                        <td>Saldo do Mês</td>
                                        <td>
                                            <asp:HiddenField ID="hidCodigoProgFidelidade" runat="server" Value="" />
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
                                        <td style="width: 300px;">
                                            <asp:TextBox ID="txtEmail" runat="server" Width="290px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 120px;">
                                            <asp:TextBox ID="txtSaldoMesAtual" runat="server" Width="110px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right;">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                            <fieldset>
                                                <legend>SMS</legend>
                                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>Texto SMS
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtTextoSMS" runat="server" Width="100%" Enabled="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="1">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="1">
                                                            <asp:Button ID="btEnviarSMS" runat="server" Text="Enviar" Width="100px" OnClick="btEnviarSMS_Click" Enabled="false" />
                                                            &nbsp;<asp:Label ID="labSMS" runat="server" ForeColor="Red" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>

                                            <fieldset>
                                                <legend>Whats App</legend>
                                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td colspan="2">Texto WhatsApp&nbsp;&nbsp;<asp:ImageButton ID="btCopiarMsgWhats" runat="server" Width="15px" ImageUrl="~/Image/copy.png" ToolTip="" OnClientClick="CopiarColarWhatsApp();" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="txtTextoWhatsApp" runat="server" Width="100%" Enabled="true" BackColor="Gainsboro" Height="80px" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btEnviarWhatsApp" runat="server" Text="Enviar" Width="100px" OnClick="btEnviarWhatsApp_Click" Enabled="false" />
                                                            &nbsp;<asp:Label ID="labWhatsApp" runat="server" ForeColor="Red" Text=""></asp:Label>
                                                        </td>
                                                        <td style="text-align: right;">
                                                            <asp:Button ID="btBloquearTelefone" runat="server" Text="Bloquear Telefone" Width="130px" OnClick="btBloquearTelefone_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="2">
                            <fieldset>
                                <legend>Histórico Handclub</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvHistHand" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                    Style="background: white" OnRowDataBound="gvHistHand_RowDataBound" OnDataBound="gvHistHand_DataBound"
                                                    ShowFooter="true">
                                                    <HeaderStyle BackColor="Gainsboro" />
                                                    <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Left" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-VerticalAlign="Middle">
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../Image/plus.png"
                                                                    Width="15px" runat="server" />
                                                                <asp:Panel ID="pnlVendas" runat="server" Style="display: none" BorderWidth="2" Width="100%">
                                                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td colspan="2" style="border: 0px;">
                                                                                <fieldset>
                                                                                    <legend>Pontos</legend>
                                                                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td style="border: 0px;">Saldo de Pontos
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="width: 150px; border: 0px;">
                                                                                                <asp:TextBox ID="txtSaldoPonto" runat="server" Width="140px" Enabled="false"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="border: 0px;">&nbsp;</td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="border: 0px;">
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
                                                                            <td colspan="2" style="border: 0px;">
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
                                                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center"
                                                                                                    ItemStyle-VerticalAlign="Middle">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../Image/plus.png"
                                                                                                            Width="15px" runat="server" />
                                                                                                        <asp:Panel ID="pnlVendasA" runat="server" Style="display: none" BorderWidth="2" Width="100%">
                                                                                                            <asp:GridView ID="gvHistCancelamento" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                                                                                Style="background: white" OnRowDataBound="gvHistCancelamento_RowDataBound" OnDataBound="gvHistCancelamento_DataBound"
                                                                                                                ShowFooter="true">
                                                                                                                <HeaderStyle BackColor="Gainsboro" />
                                                                                                                <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Left" />
                                                                                                                <Columns>
                                                                                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Data Evento" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Literal ID="litDataEvento" runat="server"></asp:Literal>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Evento" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Literal ID="litEvento" runat="server" Text='<%# Bind("DESCRICAO") %>'></asp:Literal>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Histórico" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Literal ID="litHistorico" runat="server" Text='<%# Bind("HISTORICO") %>'></asp:Literal>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Quem Cancelou" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Literal ID="litQuemCancelou" runat="server" Text='<%# Bind("QUEM_CANCELOU") %>'></asp:Literal>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Cargo" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Literal ID="litCargo" runat="server" Text='<%# Bind("DESC_CARGO") %>'></asp:Literal>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                </Columns>
                                                                                                            </asp:GridView>
                                                                                                        </asp:Panel>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Filial" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Literal ID="litFilial" runat="server"></asp:Literal>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Vendedor" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Literal ID="litVendedor" runat="server"></asp:Literal>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Ticket" HeaderStyle-Width="105px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Literal ID="litTicket" runat="server"></asp:Literal>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Data Venda" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Literal ID="litDataVenda" runat="server"></asp:Literal>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Valor Venda Bruta" HeaderStyle-Width="125px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Literal ID="litValorBruto" runat="server"></asp:Literal>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Valor Troca" HeaderStyle-Width="125px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Literal ID="litValorTroca" runat="server"></asp:Literal>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Valor Pago" HeaderStyle-Width="125px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Literal ID="litValorPago" runat="server"></asp:Literal>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Pontos Acumulados" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Literal ID="litPonto" runat="server"></asp:Literal>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Literal ID="litAbrirProduto" runat="server" />
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                    </div>
                                                                                </fieldset>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2" style="border: 0px;">
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
                                                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center"
                                                                                                    ItemStyle-VerticalAlign="Middle">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../Image/plus.png"
                                                                                                            Width="15px" runat="server" />
                                                                                                        <asp:Panel ID="pnlVendasA" runat="server" Style="display: none" BorderWidth="2" Width="100%">
                                                                                                            <asp:GridView ID="gvHistCancelamento" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                                                                                Style="background: white" OnRowDataBound="gvHistCancelamento_RowDataBound" OnDataBound="gvHistCancelamento_DataBound"
                                                                                                                ShowFooter="true">
                                                                                                                <HeaderStyle BackColor="Gainsboro" />
                                                                                                                <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Left" />
                                                                                                                <Columns>
                                                                                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Data Evento" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Literal ID="litDataEvento" runat="server"></asp:Literal>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Evento" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Literal ID="litEvento" runat="server" Text='<%# Bind("DESCRICAO") %>'></asp:Literal>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Histórico" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Literal ID="litHistorico" runat="server" Text='<%# Bind("HISTORICO") %>'></asp:Literal>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Quem Cancelou" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Literal ID="litQuemCancelou" runat="server" Text='<%# Bind("QUEM_CANCELOU") %>'></asp:Literal>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Cargo" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Literal ID="litCargo" runat="server" Text='<%# Bind("DESC_CARGO") %>'></asp:Literal>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                </Columns>
                                                                                                            </asp:GridView>
                                                                                                        </asp:Panel>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Filial" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Literal ID="litFilial" runat="server"></asp:Literal>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Vendedor" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Literal ID="litVendedor" runat="server"></asp:Literal>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Ticket" HeaderStyle-Width="105px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Literal ID="litTicket" runat="server"></asp:Literal>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Data Venda" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Literal ID="litDataVenda" runat="server"></asp:Literal>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Valor Venda Bruta" HeaderStyle-Width="125px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Literal ID="litValorBruto" runat="server"></asp:Literal>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Valor Troca" HeaderStyle-Width="125px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Literal ID="litValorTroca" runat="server"></asp:Literal>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Valor Pago" HeaderStyle-Width="125px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Literal ID="litValorPago" runat="server"></asp:Literal>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Desconto" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Literal ID="litDesconto" runat="server"></asp:Literal>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Literal ID="litAbrirProduto" runat="server" />
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                    </div>
                                                                                </fieldset>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2" style="border: 0px;">&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Código Programa" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litCodigoPrograma" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Programa" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litPrograma" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Total de Vendas" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litTotVenda" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Pontos Acumulados" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litPontosAcumulados" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Saldo de Pontos" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litSaldo" runat="server"></asp:Literal>
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
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
