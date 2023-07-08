<%@ Page Title="Faturamento Atacado" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="atac_rel_faturamento.aspx.cs" Inherits="Relatorios.atac_rel_faturamento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharEsquerda {
            text-align: left;
        }

        .corTD {
            background-color: #EFEFEF;
        }
    </style>
    <link href="../Styles/gridviewScroll.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/1.9.0/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.9.1/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../js/js.js"></script>
    <script type="text/javascript" src="../js/gridviewScroll.min.js"></script>

    <script type="text/javascript">

        function gridviewScroll() {

            gvAtacado = $('#MainContent_gvAtacado').gridviewScroll({
                width: 1450,
                height: 650,
                railcolor: "#F0F0F0",
                barcolor: "#CDCDCD",
                barhovercolor: "#606060",
                bgcolor: "#F0F0F0",
                freezesize: 8,
                arrowsize: 30,
                varrowtopimg: "../Image/arrowvt.png",
                varrowbottomimg: "../Image/arrowvb.png",
                harrowleftimg: "../Image/arrowhl.png",
                harrowrightimg: "../Image/arrowhr.png",
                headerrowcount: 1,
                railsize: 16,
                barsize: 12
            });
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo do Atacado&nbsp;&nbsp;>&nbsp;&nbsp;
                    Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;
                    Faturamento Atacado</span>
        <div style="float: right; padding: 0;">
            <a href="atac_menu.aspx" class="alink" title="Voltar">Voltar</a>
        </div>

    </div>
    <hr />

    <div class="login">
        <fieldset>
            <legend>Faturamento Atacado</legend>
            <asp:UpdatePanel ID="updRelFaturamento" runat="server">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btExcelFaturamento" />
                </Triggers>
                <ContentTemplate>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="labColecao" runat="server" Text="Coleção"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labSubColecao" runat="server" Text="SubColeção"></asp:Label>
                            </td>
                            <td>Status
                            </td>
                            <td>Representante
                            </td>
                            <td>Nome do Cliente
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 204px;">
                                <asp:DropDownList runat="server" ID="ddlColecao" DataValueField="COLECAO" DataTextField="DESC_COLECAO" Height="22px" OnSelectedIndexChanged="ddlColecao_SelectedIndexChanged" AutoPostBack="true"
                                    Width="198px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 204px;">
                                <asp:DropDownList runat="server" ID="ddlSubColecao" DataValueField="COLECAO" DataTextField="DESC_COLECAO" Height="22px"
                                    Width="198px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 204px;">
                                <asp:DropDownList runat="server" ID="ddlStatus" Height="22px" Width="198px">
                                    <asp:ListItem Text="" Value="0" />
                                    <asp:ListItem Text="NOVO" Value="NOVO" />
                                    <asp:ListItem Text="CANCELADO" Value="CANCELADO" />
                                    <asp:ListItem Text="FATURAMENTO" Value="FATURAMENTO" />
                                    <asp:ListItem Text="FAT. TOTAL" Value="FAT. TOTAL" />
                                    <asp:ListItem Text="SEM CRÉDITO" Value="SEM CRÉDITO" />
                                    <asp:ListItem Text="CONFERIR" Value="CONFERIR" />
                                </asp:DropDownList>
                            </td>
                            <td style="width: 204px;">
                                <asp:DropDownList runat="server" ID="ddlRepresentante" DataValueField="REPRESENTANTE" DataTextField="REPRESENTANTE" Height="22px"
                                    Width="198px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNomeCliente" runat="server" CssClass="alinharEsquerda" MaxLength="100" Height="16px" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;
                                <asp:Button runat="server" ID="btExcelFaturamento" Text="Excel" Width="100px" OnClick="btExcelFaturamento_Click" ToolTip="Extrair as informações para Excel." />&nbsp;&nbsp;
                                    <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;
                            </td>
                        </tr>
                    </table>
                    <div>
                        <div class="rounded_corners">
                            <asp:GridView ID="gvAtacado" runat="server" AutoGenerateColumns="False" ShowFooter="true"
                                ForeColor="#333333" Style="background: white; width: 3900px; border-collapse: collapse;"
                                OnRowDataBound="gvAtacado_RowDataBound" OnSorting="gvAtacado_Sorting" AllowSorting="true" OnDataBound="gvAtacado_DataBound">
                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                <FooterStyle BackColor="GradientActiveCaption" Font-Bold="true" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="corTD">
                                        <ItemTemplate>
                                            <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="status" HeaderText="Status" SortExpression="status" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-CssClass="corTD" ItemStyle-Font-Size="Smaller" />

                                    <asp:BoundField DataField="representante" HeaderText="Representante" SortExpression="representante" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-CssClass="corTD" ItemStyle-Font-Size="Smaller" />
                                    <asp:BoundField DataField="CODIGO_CLIENTE" HeaderText="Clifor" SortExpression="CODIGO_CLIENTE" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="65px" ItemStyle-Width="65px" Visible="true" ItemStyle-CssClass="corTD" ItemStyle-Font-Size="Smaller" />
                                    <asp:BoundField DataField="nome_cliente" HeaderText="Nome do Cliente" SortExpression="nome_cliente" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-CssClass="corTD" ItemStyle-Font-Size="Smaller" />
                                    <%--<asp:BoundField DataField="status_pedido" HeaderText="Status do Pedido" SortExpression="status_pedido" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-CssClass="corTD" ItemStyle-Font-Size="Smaller" />--%>
                                    <asp:TemplateField HeaderText="% Entrega" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-CssClass="corTD" ItemStyle-Font-Size="Smaller"
                                        SortExpression="QTDE_ENTREGUE_PORC">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="litQtdeEntregaPorc" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="credito" HeaderText="Crédito" SortExpression="credito" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="90px" ItemStyle-Width="90px" ItemStyle-CssClass="corTD" ItemStyle-Font-Size="Smaller" />
                                    <asp:BoundField DataField="DESC_COND_PGTO" HeaderText="Cond. Pgto" SortExpression="DESC_COND_PGTO" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="130px" ItemStyle-Width="130px" ItemStyle-CssClass="corTD" ItemStyle-Font-Size="Smaller" />
                                    <asp:TemplateField HeaderText="Qtde Original" HeaderStyle-Width="200px">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="LiteralQtdeOriginal" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Valor Original" HeaderStyle-Width="220px">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="LiteralValorOriginal" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qtde Cancelada" HeaderStyle-Width="220px">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="LiteralQtdeCancelada" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Valor Cancelado" HeaderStyle-Width="220px">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="LiteralValorCancelado" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qtde Entregue" HeaderStyle-Width="220px">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="LiteralQtdeEntregue" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Valor Entregue" HeaderStyle-Width="220px">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="LiteralValorEntregue" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qtde Embalada" HeaderStyle-Width="220px">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="LiteralQtdeEmbalada" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Valor Embalado" HeaderStyle-Width="220px">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="LiteralValorEmbalado" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qtde a Entregar" HeaderStyle-Width="220px">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="LiteralQtdeAEntregar" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Valor a Entregar" HeaderStyle-Width="220px">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="LiteralValorAEntregar" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qtde Devolvida" HeaderStyle-Width="220px">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="LiteralQtdeDevolvida" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="data_cadastro" HeaderText="Data do Cadastro" SortExpression="data_cadastro" HeaderStyle-Width="270px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="data_fundacao" HeaderText="Data de Fundação" SortExpression="data_fundacao" HeaderStyle-Width="270px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="data_serasa" HeaderText="Data Serasa" SortExpression="data_serasa" HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="tem_protesto" HeaderText="Tem Protesto" SortExpression="tem_protesto" HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="qtde_protesto" HeaderText="Qtde Protesto" SortExpression="qtde_protesto" HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="resumo" HeaderText="Resumo" SortExpression="resumo" HeaderStyle-Width="350px" />
                                    <asp:BoundField DataField="obs_geral" HeaderText="Observação" SortExpression="obs_geral" HeaderStyle-Width="350px" />
                                    <asp:BoundField DataField="numero_pedido" HeaderText="Número do Pedido" SortExpression="numero_pedido" HeaderStyle-Width="250px" />

                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
    </div>
</asp:Content>
