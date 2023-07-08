<%@ Page Title="Desempenho de Vendas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="gerloja_desempenho_vendedorv2.aspx.cs" Inherits="Relatorios.gerloja_desempenho_vendedorv2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
    <div>
        <br />
        <div>
            <span style="font-family: Calibri; font-size: 14px;">Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Vendedores&nbsp;&nbsp;>&nbsp;&nbsp;Desempenho de Vendas</span>
            <div style="float: right; padding: 0;">
                <a href="gerloja_menu.aspx" class="alink" title="Voltar">Voltar</a>
            </div>
        </div>
        <hr />
        <fieldset>
            <legend>Desempenho de Vendas</legend>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>Data Início
                    </td>
                    <td>Data Fim
                    </td>
                    <td>Filial
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 200px;">
                        <asp:TextBox ID="txtPeriodoInicial" runat="server" Width="190px" MaxLength="10" Style="text-align: right;" onkeypress="return fnValidarData(event);"></asp:TextBox>
                    </td>
                    <td style="width: 200px;">
                        <asp:TextBox ID="txtPeriodoFinal" runat="server" Width="190px" MaxLength="10" Style="text-align: right;" onkeypress="return fnValidarData(event);"></asp:TextBox>
                    </td>
                    <td style="width: 310px;">
                        <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="21px" Width="304px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btBuscar" Text="Buscar Vendas" OnClick="btBuscar_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlFilial" runat="server" Visible="false">
                <fieldset>
                    <legend>Vendedores</legend>
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
                                            <asp:BoundField DataField="NOME_VENDEDOR" ItemStyle-BackColor="GhostWhite" HeaderText="Nome" ItemStyle-Font-Bold="true" ItemStyle-Font-Size="X-Small" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" />


                                            <asp:TemplateField HeaderText="0 Peça" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket0" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="%" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket0PORC" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="1 Peça" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket1" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="%" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket1PORC" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="2 Peças" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket2" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="%" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket2PORC" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="3 Peças" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket3" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="%" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket3PORC" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="4 Peças" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket4" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="%" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket4PORC" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="5+ Peças" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket5" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="%" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket5PORC" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="PA" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPA" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Méd. Atendido" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="liMediaAtendimento" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Tkt. Médio" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicketMedio" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Nota" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNotaLetra" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="CLIENTE_LINK" HeaderText="C/Link" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="CLIENTE_SEMLINK" HeaderText="S/Link" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" />

                                            <asp:BoundField DataField="QTDE_PRODUTO_TOTAL" HeaderText="Total Peças" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="TICKET_TOTAL" HeaderText="Total Atendido" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" />

                                            <asp:TemplateField HeaderText="Nota Final" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNotaFinal" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="NUMERO_DIAS" HeaderText="No. Dias" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="RANKING" HeaderText="Ranking" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <%--REDE--%>
                        <tr>
                            <td>
                                <fieldset>
                                    <legend>Rede</legend>

                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvRede" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ShowFooter="true" ForeColor="#333333" OnRowDataBound="gvRede_RowDataBound" Style="background: white">
                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" Font-Size="Small"></FooterStyle>
                                            <RowStyle HorizontalAlign="Left" Height="25px" />
                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                            <Columns>

                                                <asp:TemplateField HeaderText="PA" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litPA" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Média Qtde Peças" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="17%">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMediaQtdePeca" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Tkt. Médio" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="17%">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTicketMedio" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Quantidade 1" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="17%">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litQtde1" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Quantidade 1/2" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="17%">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litQtde12" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Quantidade 4/5" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="17%">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litQtde45" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                    </table>

                </fieldset>
            </asp:Panel>
            <asp:Panel ID="pnlGeral" runat="server" Visible="false">
                <fieldset>
                    <legend>Filiais</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvDesempenhoFilial" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ShowFooter="true" ForeColor="#333333" OnRowDataBound="gvDesempenhoFilial_RowDataBound" OnDataBound="gvDesempenhoFilial_DataBound"
                                        Style="background: white">
                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" Font-Size="Small"></FooterStyle>
                                        <RowStyle HorizontalAlign="Center" Height="25px" />
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundField DataField="FILIAL" ItemStyle-BackColor="GhostWhite" HeaderText="Filial" ItemStyle-Font-Bold="true" ItemStyle-Font-Size="X-Small" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left" />


                                            <asp:TemplateField HeaderText="0 Peça" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket0" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="%" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket0PORC" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="1 Peça" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket1" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="%" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket1PORC" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="2 Peças" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket2" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="%" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket2PORC" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="3 Peças" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket3" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="%" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket3PORC" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="4 Peças" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket4" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="%" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket4PORC" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="5+ Peças" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket5" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="%" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicket5PORC" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="PA" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPA" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Méd. Atendido" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="liMediaAtendimento" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Tkt. Médio" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTicketMedio" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Nota" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNotaLetra" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="CLIENTE_LINK" HeaderText="C/Link" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="CLIENTE_SEMLINK" HeaderText="S/Link" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" />

                                            <asp:BoundField DataField="QTDE_PRODUTO_TOTAL" HeaderText="Total Peças" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="TICKET_TOTAL" HeaderText="Total Atendido" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" />

                                            <asp:TemplateField HeaderText="Nota Final" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNotaFinal" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="NUMERO_DIAS" HeaderText="No. Dias" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="RANKING" HeaderText="Ranking" ItemStyle-Font-Size="Small" HeaderStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </asp:Panel>
        </fieldset>
    </div>
</asp:Content>
