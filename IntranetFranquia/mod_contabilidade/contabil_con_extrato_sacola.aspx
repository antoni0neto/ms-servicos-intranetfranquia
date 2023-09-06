<%@ Page Title="Extrato Estoque de Sacolas" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="contabil_con_extrato_sacola.aspx.cs" Inherits="Relatorios.contabil_con_extrato_sacola"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .jGrowl .manilla {
            color: black;
        }

        .jGrowl .redError {
            color: red;
        }

        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }
    </style>
    <script src="http://code.jquery.com/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Contabilidade&nbsp;&nbsp;>&nbsp;&nbsp;Apoio&nbsp;&nbsp;>&nbsp;&nbsp;<asp:Label
                    ID="labTituloMenu" runat="server" Text="Extrato Estoque de Sacolas"></asp:Label></span>
                <div style="float: right; padding: 0;">
                    <a href="contabil_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 100%;">
                <fieldset>
                    <legend>
                        <asp:Label ID="labTitulo" runat="server" Text="Extrato Estoque de Sacolas"></asp:Label></legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>Sacola
                            </td>
                            <td>Natureza
                            </td>
                            <td>Data Inicial
                            </td>
                            <td>Data Final
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 360px;">
                                <asp:DropDownList ID="ddlSacola" runat="server" Width="354px">
                                    <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                    <asp:ListItem Value="MC00020" Text="SACOLA P 18X10X26"></asp:ListItem>
                                    <asp:ListItem Value="MC000011" Text="SACOLA G 32X12X42"></asp:ListItem>
                                    <asp:ListItem Value="MC00024" Text="SACOLA G 40X45X12+1,5"></asp:ListItem>
                                    <asp:ListItem Value="MC000445" Text="SACOLA HANDBOOK P 24X30X8+1,5"></asp:ListItem>
                                    <asp:ListItem Value="MC000444" Text="SACOLA HANDBOOK M 36X40X10+1,5"></asp:ListItem>
                                    <asp:ListItem Value="MC000443" Text="SACOLA HANDBOOK G 44X50X15+1,5"></asp:ListItem>
                                    <asp:ListItem Value="MC00021" Text="SACOLA OFF SET 32X13X31,5"></asp:ListItem>
                                    <asp:ListItem Value="MC00022" Text="SACOLA KRAFT G 18X25X10"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 400px;">
                                <asp:DropDownList ID="ddlNatureza" runat="server" Width="394px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="200.03" Text="200.03 - COMPRA DE MATERIAL DE USO OU CONSUMO"></asp:ListItem>
                                    <asp:ListItem Value="299.01" Text="299.01 - OUTRAS ENTRADAS NÃO ESPECIFICADAS"></asp:ListItem>
                                    <asp:ListItem Value="183.01" Text="183.01 - VENDA DE EMBALAGENS SACOLA/CAIXA"></asp:ListItem>
                                    <asp:ListItem Value="120.05" Text="120.05 - TRANSF DE SACOLA"></asp:ListItem>
                                    <asp:ListItem Value="199.01" Text="199.01 - OUTRAS SAÍDAS NÃO ESPECIFICADAS"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtDataIni" runat="server" Width="150px" onkeypress="return fnValidarData(event);"
                                    MaxLength="10" Style="text-align: right;"></asp:TextBox>
                            </td>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtDataFim" runat="server" Width="150px" onkeypress="return fnValidarData(event);"
                                    MaxLength="10" Style="text-align: right;"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="110px" OnClick="btBuscar_Click"
                                    OnClientClick="DesabilitarBotao(this);" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text="&nbsp;"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <div id="accordionE">
                                    <h3>Extrato Estoque de Sacolas</h3>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvExtratoSacola" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvExtratoSacola_RowDataBound"
                                                            OnDataBound="gvExtratoSacola_DataBound"
                                                            ShowFooter="true">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Data" HeaderStyle-Width="110px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litData" runat="server" Text=''></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="LANCAMENTO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Lançamento" HeaderStyle-Width="110px" />
                                                                <asp:BoundField DataField="ITEM" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Item" HeaderStyle-Width="250px" />
                                                                <asp:BoundField DataField="NATUREZA" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Natureza" />
                                                                <asp:BoundField DataField="NOTA_FISCAL" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Nota Fiscal" HeaderStyle-Width="125px" />
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Qtde Entrada" HeaderStyle-Width="100px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litQtdeEntrada" runat="server" Text=''></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Qtde Saída" HeaderStyle-Width="100px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litQtdeSaida" runat="server" Text=''></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Qtde Saldo" HeaderStyle-Width="100px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litQtdeSaldo" runat="server" Text=''></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Valor Entrada" HeaderStyle-Width="100px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litValorEntrada" runat="server" Text=''></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Valor Saída" HeaderStyle-Width="100px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litValorSaida" runat="server" Text=''></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Valor Saldo" HeaderStyle-Width="100px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litValorSaldo" runat="server" Text=''></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
