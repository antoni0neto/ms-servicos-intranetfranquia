<%@ Page Title="Desempenho de Facção" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="facc_analise_faccao_mensal_media.aspx.cs" Inherits="Relatorios.facc_analise_faccao_mensal_media"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }

        .jGrowl .manilla {
            background-color: #000;
            color: white;
        }
    </style>
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://code.jquery.com/jquery.js"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Facção&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Desempenho de Facção</span>
                <div style="float: right; padding: 0;">
                    <a href="facc_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Desempenho de Facção"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Fornecedor</td>
                        <td>Coleção</td>
                        <td>Mostruário</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 344px;">
                            <asp:DropDownList ID="ddlFornecedor" runat="server" Width="338px" Height="21px" DataTextField="FORNECEDOR"
                                DataValueField="FORNECEDOR">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 180px;">
                            <asp:DropDownList ID="ddlColecao" runat="server" Width="174px" Height="21px"
                                DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 120px;">
                            <asp:DropDownList ID="ddlMostruario" runat="server" Width="114px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                <asp:ListItem Value="N" Text="Não"></asp:ListItem>
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
                            <fieldset style="margin-top: 0px;">
                                <legend>Dados Fornecedor</legend>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td>Quantidade de Funcionário</td>
                                        <td>Peças por Funcionário</td>
                                        <td>Peças por Dia</td>
                                        <td>Nota</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 210px;">
                                            <asp:TextBox ID="txtQtdeFun" runat="server" Width="200px" CssClass="alinharDireita" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td style="width: 210px;">
                                            <asp:TextBox ID="txtPecaFun" runat="server" Width="200px" CssClass="alinharDireita" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td style="width: 210px;">
                                            <asp:TextBox ID="txtPecaDia" runat="server" Width="200px" CssClass="alinharDireita" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNota" runat="server" Width="200px" BackColor="WhiteSmoke" BorderColor="Gainsboro" 
                                                Font-Bold="true" CssClass="alinharCentro" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <fieldset style="margin-top: -3px;">
                                <legend>Mensal</legend>
                                <div style="margin-top: -17px;">
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                            <td style="width: 120px;">
                                                <asp:Label ID="labSaldoMercadoria" runat="server" Text="Saldo de Mercadoria"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                            <td style="width: 120px;">
                                                <asp:TextBox ID="txtSaldoMercadoria" runat="server" Width="120px" CssClass="alinharCentro"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="line-height: 8px;">&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvFaccaoAnaliseMensal" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                        Style="background: white" OnDataBound="gvFaccaoAnaliseMensal_DataBound" ShowFooter="true">
                                        <Columns>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <fieldset style="margin-top: -3px;">
                                <legend>Volume de Corte</legend>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvFaccaoAnaliseVolume" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                        Style="background: white" OnRowDataBound="gvFaccaoAnaliseVolume_RowDataBound" ShowFooter="true">
                                        <Columns>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <fieldset style="margin-top: -3px;">
                                <legend>Entrega</legend>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvFaccaoAnaliseEntrega" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                        Style="background: white" OnRowDataBound="gvFaccaoAnaliseEntrega_RowDataBound"
                                        OnDataBound="gvFaccaoAnaliseEntrega_DataBound" ShowFooter="true">
                                        <HeaderStyle BackColor="GradientActiveCaption" />
                                        <RowStyle HorizontalAlign="Center" />
                                        <FooterStyle BackColor="GradientActiveCaption" Font-Bold="true" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:BoundField DataField="DIAS" HeaderText="Dias" ItemStyle-Width="150px" />
                                            <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" />
                                            <asp:BoundField DataField="QTDE_HB" HeaderText="Qtde HB" />
                                            <asp:BoundField DataField="QTDE_PRAZO" HeaderText="No Prazo" ItemStyle-Width="144px" />
                                            <asp:BoundField DataField="QTDE_1_SEMANA" HeaderText="Atraso 1 Semana" ItemStyle-Width="144px" />
                                            <asp:BoundField DataField="QTDE_2_SEMANA" HeaderText="Atraso 2 Semana" ItemStyle-Width="145px" />
                                            <asp:BoundField DataField="QTDE_3_SEMANA" HeaderText="Atraso 3 Semana" ItemStyle-Width="145px" />
                                            <asp:BoundField DataField="QTDE_4_SEMANA" HeaderText="Atraso 4 Semana" ItemStyle-Width="145px" />
                                            <asp:BoundField DataField="QTDE_5_SEMANA" HeaderText="Atraso 5 Semana" ItemStyle-Width="145px" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <fieldset style="margin-top: -3px;">
                                <legend>% Entrega</legend>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvFaccaoAnaliseEntregaPorc" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                        Style="background: white" OnRowDataBound="gvFaccaoAnaliseEntregaPorc_RowDataBound" OnDataBound="gvFaccaoAnaliseEntregaPorc_DataBound"
                                        ShowFooter="true">
                                        <HeaderStyle BackColor="GradientActiveCaption" />
                                        <RowStyle HorizontalAlign="Center" />
                                        <FooterStyle BackColor="GradientActiveCaption" />
                                        <Columns>
                                            <asp:BoundField DataField="DIAS" HeaderText="Dias" ItemStyle-Width="150px" />
                                            <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" />
                                            <asp:BoundField DataField="QTDE_HB" HeaderText="Qtde HB" />
                                            <asp:TemplateField HeaderText="No Prazo" HeaderStyle-Width="144px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtdePrazo" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Atraso 1 Semana" HeaderStyle-Width="144px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtdeAtraso1" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Atraso 2 Semana" HeaderStyle-Width="144px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtdeAtraso2" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Atraso 3 Semana" HeaderStyle-Width="144px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtdeAtraso3" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Atraso 4 Semana" HeaderStyle-Width="144px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtdeAtraso4" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Atraso 5 Semana" HeaderStyle-Width="144px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtdeAtraso5" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
