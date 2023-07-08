<%@ Page Title="Análise de Cotas" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="gerloja_visita_supervisor_cotas.aspx.cs" Inherits="Relatorios.gerloja_visita_supervisor_cotas" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>


</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="accountInfo">
        <br />
        <div>
            <span style="font-family: Calibri; font-size: 14px;">Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Supervisores&nbsp;&nbsp;>&nbsp;&nbsp;Análise de Cotas</span>
            <div style="float: right; padding: 0;">
                <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
            </div>
        </div>
        <hr />
        <fieldset class="login">
            <legend>Análise de Cotas</legend>
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>Data Início
                    </td>
                    <td>Data Fim
                    </td>
                    <td>Filial
                    </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 200px;">
                        <asp:TextBox ID="txtPeriodoInicial" runat="server" Width="190px" MaxLength="10" Style="text-align: right;" Enabled="false" onkeypress="return fnValidarData(event);"></asp:TextBox>
                    </td>
                    <td style="width: 200px;">
                        <asp:TextBox ID="txtPeriodoFinal" runat="server" Width="190px" MaxLength="10" Style="text-align: right;" Enabled="false" onkeypress="return fnValidarData(event);"></asp:TextBox>
                    </td>
                    <td style="width: 320px;">
                        <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="21px" Width="316px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="labErro" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="text-align: right;">
                        <asp:Button ID="btGriffePorc" runat="server" Text="Abrir Griffe Porcentagem" OnClick="btGriffePorc_Click" Width="160px" />&nbsp;
                        <asp:Button ID="btAbrir60Mais" runat="server" Text="Abrir 60+ Vendidos" OnClick="btAbrir60Mais_Click" Width="160px" />
                    </td>
                </tr>
            </table>


            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" style="width: 50%;">
                        <fieldset>

                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="3">
                                        <asp:HiddenField ID="hidAno" runat="server" Value="" />
                                        <asp:HiddenField ID="hidMes" runat="server" Value="" />
                                        <asp:HiddenField ID="hidCodigoUsuario" runat="server" Value="0" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 33%;">
                                        <asp:Label ID="labPeriodo" runat="server" Text="ABRIL DE 2019" Font-Bold="true" Font-Size="Larger"></asp:Label>
                                    </td>
                                    <td style="width: 33%; text-align: center;">
                                        <asp:Label ID="labSupervisor" runat="server" Text="" Font-Bold="true" Font-Size="Larger"></asp:Label>
                                    </td>
                                    <td style="width: 33%; text-align: right;">
                                        <asp:Button ID="btAtualizar" runat="server" Text="Atualizar" Width="70px" OnClick="btAtualizar_Click" />&nbsp;
                            <asp:Button ID="btAnterior" runat="server" Text="<<" Width="70px" OnClick="btAnterior_Click" />&nbsp;
                        <asp:Button ID="btProximo" runat="server" Text=">>" Width="70px" Enabled="false" OnClick="btProximo_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <hr />
                                    </td>
                                </tr>
                            </table>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="7">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width: 14%">&nbsp;<asp:Label ID="labSegunda" runat="server" Text="Segunda" Font-Bold="true"></asp:Label></td>
                                    <td style="width: 15%">&nbsp;<asp:Label ID="labTerca" runat="server" Text="Terça" Font-Bold="true"></asp:Label></td>
                                    <td style="width: 14%">&nbsp;<asp:Label ID="labQuarta" runat="server" Text="Quarta" Font-Bold="true"></asp:Label></td>
                                    <td style="width: 15%">&nbsp;<asp:Label ID="labQuinta" runat="server" Text="Quinta" Font-Bold="true"></asp:Label></td>
                                    <td style="width: 14%">&nbsp;<asp:Label ID="labSexta" runat="server" Text="Sexta" Font-Bold="true"></asp:Label></td>
                                    <td style="width: 15%">&nbsp;<asp:Label ID="labSabado" runat="server" Text="Sábado" Font-Bold="true"></asp:Label></td>
                                    <td style="width: 14%">&nbsp;<asp:Label ID="labDomingo" runat="server" Text="Domingo" Font-Bold="true"></asp:Label></td>
                                </tr>
                            </table>
                            <asp:Repeater ID="repAnaliseCotas" runat="server" OnItemDataBound="repAnaliseCotas_ItemDataBound">
                                <ItemTemplate>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 14%">
                                                <asp:Table ID="tbSegunda" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right" Width="20%" Height="8px">
                                                            <asp:Label ID="litSegundaDia" runat="server" Font-Bold="true"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="20%" Height="60px" ForeColor="Black" Font-Bold="true" Font-Names="Arial" Font-Size="Smaller">
                                                            <br />
                                                            &nbsp;<asp:Literal ID="litSegundaPorc" runat="server"></asp:Literal><br />
                                                            &nbsp;<asp:Literal ID="litSegundaValor" runat="server"></asp:Literal>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:Table ID="tbTerca" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right" Width="20%" Height="8px">
                                                            <asp:Label ID="litTercaDia" runat="server" Font-Bold="true"></asp:Label>
                                                            &nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="20%" Height="60px" ForeColor="Black" Font-Bold="true" Font-Names="Arial" Font-Size="Smaller">
                                                            <br />
                                                            &nbsp;<asp:Literal ID="litTercaPorc" runat="server"></asp:Literal><br />
                                                            &nbsp;<asp:Literal ID="litTercaValor" runat="server"></asp:Literal>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 14%">
                                                <asp:Table ID="tbQuarta" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right" Width="20%" Height="8px">
                                                            <asp:Label ID="litQuartaDia" runat="server" Font-Bold="true"></asp:Label>
                                                            &nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="20%" Height="60px" ForeColor="Black" Font-Bold="true" Font-Names="Arial" Font-Size="Smaller">
                                                            <br />
                                                            &nbsp;<asp:Literal ID="litQuartaPorc" runat="server"></asp:Literal><br />
                                                            &nbsp;<asp:Literal ID="litQuartaValor" runat="server"></asp:Literal>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:Table ID="tbQuinta" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right" Width="20%" Height="8px">
                                                            <asp:Label ID="litQuintaDia" runat="server" Font-Bold="true"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="20%" Height="60px" ForeColor="Black" Font-Bold="true" Font-Names="Arial" Font-Size="Smaller">
                                                            <br />
                                                            &nbsp;<asp:Literal ID="litQuintaPorc" runat="server"></asp:Literal><br />
                                                            &nbsp;<asp:Literal ID="litQuintaValor" runat="server"></asp:Literal>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 14%">
                                                <asp:Table ID="tbSexta" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right" Width="20%" Height="8px">
                                                            <asp:Label ID="litSextaDia" runat="server" Font-Bold="true"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="20%" Height="60px" ForeColor="Black" Font-Bold="true" Font-Names="Arial" Font-Size="Smaller">
                                                            <br />
                                                            &nbsp;<asp:Literal ID="litSextaPorc" runat="server"></asp:Literal><br />
                                                            &nbsp;<asp:Literal ID="litSextaValor" runat="server"></asp:Literal>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 15%">
                                                <asp:Table ID="tbSabado" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right" Width="20%" Height="8px">
                                                            <asp:Label ID="litSabadoDia" runat="server" Font-Bold="true"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="20%" Height="60px" ForeColor="Black" Font-Bold="true" Font-Names="Arial" Font-Size="Smaller">
                                                            <br />
                                                            &nbsp;<asp:Literal ID="litSabadoPorc" runat="server"></asp:Literal><br />
                                                            &nbsp;<asp:Literal ID="litSabadoValor" runat="server"></asp:Literal>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 14%">
                                                <asp:Table ID="tbDomingo" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right" Width="20%" Height="8px">
                                                            <asp:Label ID="litDomingoDia" runat="server" Font-Bold="true"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="20%" Height="60px" ForeColor="Black" Font-Bold="true" Font-Names="Arial" Font-Size="Smaller">
                                                            <br />
                                                            &nbsp;<asp:Literal ID="litDomingoPorc" runat="server"></asp:Literal><br />
                                                            &nbsp;<asp:Literal ID="litDomingoValor" runat="server"></asp:Literal>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:Repeater>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="2">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width: 50%">
                                        <asp:Label ID="labCotaT" runat="server" Text="COTA: " Font-Names="Arial" Font-Bold="true" Font-Size="Smaller"></asp:Label><asp:Label ID="labCotaValor" runat="server" Font-Names="Arial" Font-Size="Smaller"></asp:Label></td>
                                    <td style="width: 50%; text-align: right;">
                                        <asp:Label ID="labVendaT" runat="server" Text="VENDA: " Font-Names="Arial" Font-Bold="true" Font-Size="Smaller"></asp:Label><asp:Label ID="labVendaValor" runat="server" Font-Names="Arial" Font-Size="Smaller"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="">
                                        <asp:Label ID="labAtingidoT" runat="server" Text="ATINGIDO: " Font-Names="Arial" Font-Bold="true" Font-Size="Smaller"></asp:Label><asp:Label ID="labAtingidoPorc" runat="server" Font-Names="Arial" Font-Size="Smaller"></asp:Label></td>
                                    <td style="text-align: right;">
                                        <asp:Label ID="labDiffT" runat="server" Text="DIFERENÇA: " Font-Names="Arial" Font-Bold="true" Font-Size="Smaller"></asp:Label><asp:Label ID="labDiffValor" runat="server" Font-Names="Arial" Font-Size="Smaller"></asp:Label></td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                    <td valign="top" style="width: 50%;">
                        <fieldset>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 67px;">&nbsp;
                                    </td>
                                    <td style="width: 318px;">
                                        <asp:Label ID="labAnoAnt" runat="server" Text="" Font-Bold="true" Font-Size="Larger"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labAnoAtu" runat="server" Text="" Font-Bold="true" Font-Size="Larger"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <hr />
                                    </td>
                                </tr>
                            </table>
                            <div class="rounded_corners">
                                <asp:GridView ID="gvCotaComp" runat="server" Width="100%"
                                    AutoGenerateColumns="False" ShowFooter="true" OnRowDataBound="gvCotaComp_RowDataBound"
                                    OnDataBound="gvCotaComp_DataBound">
                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" Font-Names="Arial" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Mês" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="60px" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Names="Arial" FooterStyle-Font-Size="Smaller" FooterStyle-ForeColor="Black">
                                            <ItemTemplate>
                                                <asp:Label ID="labMesExtenso" runat="server" Text='<%# Bind("MES_EXTENSO")%>' Font-Names="Arial" Font-Bold="true" Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cota" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" HeaderStyle-Width="97px" FooterStyle-Font-Names="Arial" FooterStyle-Font-Size="Smaller" FooterStyle-ForeColor="Black">
                                            <ItemTemplate>
                                                <asp:Label ID="labCotaValAnt" runat="server" Font-Names="Arial" Font-Bold="true" Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Venda" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" HeaderStyle-Width="97px" FooterStyle-Font-Names="Arial" FooterStyle-Font-Size="Smaller" FooterStyle-ForeColor="Black">
                                            <ItemTemplate>
                                                <asp:Label ID="labVendaValAnt" runat="server" Font-Names="Arial" Font-Bold="true" Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="(%)" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" HeaderStyle-Width="50px" FooterStyle-Font-Names="Arial" FooterStyle-Font-Size="Smaller" FooterStyle-ForeColor="Black">
                                            <ItemTemplate>
                                                <asp:Label ID="labAtingidoPorcAnt" runat="server" Font-Names="Arial" Font-Bold="true" Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Prêmio" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="55px" FooterStyle-Font-Names="Arial" FooterStyle-Font-Size="Smaller" FooterStyle-ForeColor="Black">
                                            <ItemTemplate>
                                                <asp:Label ID="labQtdePremioAnt" runat="server" Text='<%# Bind("QTDE_PREMIO_ANT")%>' Font-Names="Arial" Font-Bold="true" Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="" HeaderText="" HeaderStyle-Width="2px" ItemStyle-BackColor="GradientActiveCaption" />

                                        <asp:TemplateField HeaderText="Cota" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" HeaderStyle-Width="" FooterStyle-Font-Names="Arial" FooterStyle-Font-Size="Smaller" FooterStyle-ForeColor="Black">
                                            <ItemTemplate>
                                                <asp:Label ID="labCotaValAtual" runat="server" Font-Names="Arial" Font-Bold="true" Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Venda" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" HeaderStyle-Width="" FooterStyle-Font-Names="Arial" FooterStyle-Font-Size="Smaller" FooterStyle-ForeColor="Black">
                                            <ItemTemplate>
                                                <asp:Label ID="labVendaValAtual" runat="server" Font-Names="Arial" Font-Bold="true" Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="(%)" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" HeaderStyle-Width="" FooterStyle-Font-Names="Arial" FooterStyle-Font-Size="Smaller" FooterStyle-ForeColor="Black">
                                            <ItemTemplate>
                                                <asp:Label ID="labAtingidoPorcAtual" runat="server" Font-Names="Arial" Font-Bold="true" Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Prêmio" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-Font-Names="Arial" FooterStyle-Font-Size="Smaller" FooterStyle-ForeColor="Black">
                                            <ItemTemplate>
                                                <asp:Label ID="labQtdePremioAtual" runat="server" Text='<%# Bind("QTDE_PREMIO_ATU")%>' Font-Names="Arial" Font-Bold="true" Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>



                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>

            <asp:Panel ID="pnlFilial" runat="server" Visible="false">
                <fieldset>
                    <legend>Vendedores</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="top" style="width: 50%;">
                                <fieldset>
                                    <legend>Plano de Ação</legend>


                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvDesempenhoVendedor" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ShowFooter="true" ForeColor="#333333" OnRowDataBound="gvDesempenhoVendedor_RowDataBound" OnDataBound="gvDesempenhoVendedor_DataBound"
                                                        Style="background: white">
                                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" Font-Size="Small"></FooterStyle>
                                                        <RowStyle HorizontalAlign="Center" Height="25px" />
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="NOME_VENDEDOR" ItemStyle-BackColor="GhostWhite" HeaderText="Nome" ItemStyle-Font-Bold="true" ItemStyle-Font-Size="X-Small" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" />
                                                            <asp:TemplateField HeaderText="Valor" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" HeaderStyle-Width="105px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litValorPago" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="TICKET_TOTAL" HeaderText="Tot Atendido" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="75px" />
                                                            <asp:TemplateField HeaderText="PA" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="45px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litPA" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Tkt. Médio" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litTicketMedio" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="% 1 Peça" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="55px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litTicket1PORC" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="RANKING" HeaderText="Ranking" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="55px" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                                <br />
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvDesempenhoVendedorAnoAnt" runat="server" Width="100%" AutoGenerateColumns="False" ShowHeader="false"
                                                        ShowFooter="false" ForeColor="#333333" OnRowDataBound="gvDesempenhoVendedorAnoAnt_RowDataBound" Style="background: white">
                                                        <RowStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" Font-Size="Small"></RowStyle>
                                                        <Columns>
                                                            <asp:BoundField DataField="" HeaderText="" ItemStyle-Width="20px" />
                                                            <asp:BoundField DataField="NOME_VENDEDOR" ItemStyle-BackColor="GhostWhite" HeaderText="Nome" ItemStyle-Font-Bold="true" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Left" />
                                                            <asp:TemplateField HeaderText="Valor" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="105px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litValorPago" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="TICKET_TOTAL" HeaderText="Tot Atendido" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="75px" />
                                                            <asp:TemplateField HeaderText="PA" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="45px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litPA" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Tkt. Médio" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litTicketMedio" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="% 1 Peça" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="55px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litTicket1PORC" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="" HeaderText="" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="55px" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td valign="top" style="width: 50%;">
                                <fieldset>
                                    <legend>
                                        <asp:Label ID="labMesAnteriorVendedor" runat="server" Text="_"></asp:Label>
                                        (Mês Anterior)
                                    </legend>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvDesempenhoVendedorMesAnterior" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ShowFooter="true" ForeColor="#333333" OnRowDataBound="gvDesempenhoVendedorMesAnterior_RowDataBound" OnDataBound="gvDesempenhoVendedorMesAnterior_DataBound"
                                                        Style="background: white">
                                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" Font-Size="Small"></FooterStyle>
                                                        <RowStyle HorizontalAlign="Center" Height="25px" />
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="NOME_VENDEDOR" ItemStyle-BackColor="GhostWhite" HeaderText="Nome" ItemStyle-Font-Bold="true" ItemStyle-Font-Size="X-Small" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="TEMPO_CASA" ItemStyle-BackColor="GhostWhite" HeaderText="Período" ItemStyle-Font-Bold="true" ItemStyle-Font-Size="X-Small" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" />
                                                            <asp:TemplateField HeaderText="Valor" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" HeaderStyle-Width="95px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litValorPago" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="TICKET_TOTAL" HeaderText="Tot Atendido" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="75px" />
                                                            <asp:TemplateField HeaderText="PA" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="45px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litPA" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Tkt. Médio" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="65px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litTicketMedio" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="% 1 Peça" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="55px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litTicket1PORC" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="RANKING" HeaderText="Ranking" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="55px" />
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
            </asp:Panel>
            <fieldset style="height: 80px;">
                <legend>Atalho</legend>
                <asp:Menu ID="mnuAtalhoSuper" runat="server">
                    <Items>
                        <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_mais_vendidos.aspx?t=1" Text="1. 60+ Vendidos"></asp:MenuItem>
                        <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_griffe_porc.aspx?t=1" Text="2. Griffe Porcentagem"></asp:MenuItem>
                        <asp:MenuItem NavigateUrl="~/mod_gestao/gest_loja_compara_cota.aspx?t=2" Text="3. Farol (Vendas x Lojas)"></asp:MenuItem>
                    </Items>
                </asp:Menu>
            </fieldset>
        </fieldset>
    </div>
</asp:Content>
