<%@ Page Title="Resumo Facção" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="facc_resumo.aspx.cs" Inherits="Relatorios.facc_resumo"
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
    </script>


    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Facção&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Resumo Facção</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Resumo Facção"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Serviço</td>
                        <td>Coleção
                        </td>
                        <td>Griffe
                        </td>
                        <td>Grupo Produto
                        </td>
                        <td>HB
                        </td>
                        <td>Mostruário
                        </td>
                        <td>Fornecedor
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 160px;">
                            <asp:DropDownList ID="ddlServico" runat="server" Width="154px" Height="21px" DataTextField="DESCRICAO"
                                DataValueField="CODIGO" OnSelectedIndexChanged="ddlServico_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 180px;">
                            <asp:DropDownList ID="ddlColecao" runat="server" Width="174px" Height="21px"
                                DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 180px">
                            <asp:DropDownList ID="ddlGriffe" runat="server" Width="174px" Height="21px" DataTextField="GRIFFE"
                                DataValueField="GRIFFE">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 190px;" valign="top">
                            <asp:DropDownList ID="ddlGrupo" runat="server" Width="184px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                DataValueField="GRUPO_PRODUTO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 110px;">
                            <asp:TextBox ID="txtHB" runat="server" Width="100px" MaxLength="5" CssClass="alinharDireita"
                                onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                        </td>
                        <td style="width: 120px;">
                            <asp:DropDownList ID="ddlMostruario" runat="server" Width="114px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                <asp:ListItem Value="N" Text="Não" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 250px;">
                            <asp:DropDownList ID="ddlFornecedor" runat="server" Width="244px" Height="21px" DataTextField="FORNECEDOR"
                                DataValueField="FORNECEDOR" OnSelectedIndexChanged="ddlFornecedor_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />
                        </td>
                        <td style="text-align: right;">
                            <asp:Button ID="btLimpar" runat="server" Text="Limpar" Width="100px" OnClick="btLimpar_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>Status</td>
                        <td>
                            <asp:Label ID="labDataPrevEntregaIni" runat="server" Text="Previsão Entrega Início"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="labDataPrevEntregaFim" runat="server" Text="Previsão Entrega Fim"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="labSubStatus" runat="server" Text="SubStatus" Visible="false"></asp:Label>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="labFornecedorSub" runat="server" Text="Sub Fornecedor" Visible="false"></asp:Label>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="154px" Height="21px" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDataPrevIni" runat="server" autocomplete="off" Width="170px" MaxLength="10" Style="text-align: right;"
                                onkeypress="return fnValidarData(event);"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDataPrevFim" runat="server" autocomplete="off" Width="170px" MaxLength="10" Style="text-align: right;"
                                onkeypress="return fnValidarData(event);"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSubStatus" runat="server" Width="184px" Height="21px" Visible="false">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="CORTE INTEIRO" Text="CORTE INTEIRO"></asp:ListItem>
                                <asp:ListItem Value="PONTA" Text="PONTA"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlFornecedorSub" runat="server" Width="244px" Height="21px" DataTextField="FORNECEDOR_SUB"
                                DataValueField="FORNECEDOR_SUB" Visible="false">
                            </asp:DropDownList>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>

                    <tr>
                        <td colspan="9">
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">&nbsp;
                        </td>
                        <td style="text-align: right;">
                            <asp:Button ID="btImprimir" runat="server" Text="Imprimir" Width="100px" OnClick="btImprimir_Click" OnClientClick="DesabilitarBotao(this);" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">Total de SKUs Varejo:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                            ID="labTotalVarejo" runat="server" ForeColor="Green" Font-Bold="true" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9" style="line-height: 8px;">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvFaccaoHB" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                    Style="background: white" OnRowDataBound="gvFaccaoHB_RowDataBound" OnDataBound="gvFaccaoHB_DataBound"
                                    OnSorting="gvFaccaoHB_Sorting"
                                    ShowFooter="true" AllowSorting="true">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgProduto" runat="server" Width="90%" ImageAlign="AbsMiddle" ToolTip="" AlternateText="" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Serviço" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="SERVICO">
                                            <ItemTemplate>
                                                <asp:Label ID="labServico" runat="server" Text='<%# Bind("SERVICO") %>' Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fornecedor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="FORNECEDOR">
                                            <ItemTemplate>
                                                <asp:Label ID="labFornecedor" runat="server" Text='<%# Bind("FORNECEDOR") %>' Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Coleção" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="DESC_COLECAO">
                                            <ItemTemplate>
                                                <asp:Label ID="labColecao" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="HB" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="HB">
                                            <ItemTemplate>
                                                <asp:Label ID="labHB" runat="server" Text='<%# Bind("HB") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nome" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="NOME">
                                            <ItemTemplate>
                                                <asp:Label ID="labNome" runat="server" Text='<%# Bind("NOME") %>' Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Griffe" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="GRIFFE">
                                            <ItemTemplate>
                                                <asp:Label ID="labGriffe" runat="server" Text='<%# Bind("GRIFFE") %>' Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Varejo" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="QTDE_VAREJO">
                                            <ItemTemplate>
                                                <asp:Label ID="labVarejo" runat="server" Text='<%# Bind("QTDE_VAREJO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="NF Saída" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="labNFSaida" runat="server" Text='<%# Bind("NF_SAIDA") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Emissão" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="EMISSAO">
                                            <ItemTemplate>
                                                <asp:Label ID="labEmissao" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Recebida" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="QTDE_RECEBIDA">
                                            <ItemTemplate>
                                                <asp:Label ID="labRecebida" runat="server" Text='<%# Bind("QTDE_RECEBIDA") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="NF Entrada" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="labNFEntrada" runat="server" Text='<%# Bind("NF_ENTRADA") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Recebimento" HeaderStyle-Width="110px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="RECEBIMENTO">
                                            <ItemTemplate>
                                                <asp:Label ID="labRecebimento" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Faltante" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="QTDE_FALTANTE">
                                            <ItemTemplate>
                                                <asp:Label ID="labFaltante" runat="server" Text='<%# Bind("QTDE_FALTANTE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="STATUS">
                                            <ItemTemplate>
                                                <asp:Label ID="labStatus" runat="server" Text='<%# Bind("STATUS") %>' Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Prev Entrega" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="DATA_PREV_ENTREGA">
                                            <ItemTemplate>
                                                <asp:Label ID="labDataPrevEntrega" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">&nbsp;
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
