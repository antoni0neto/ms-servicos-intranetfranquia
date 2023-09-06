<%@ Page Title="Facção por Atacado/Varejo" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="facc_resumo_atacadovarejo.aspx.cs" Inherits="Relatorios.facc_resumo_atacadovarejo"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Facção&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Facção por Atacado/Varejo</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Facção por Atacado/Varejo"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Serviço</td>
                        <td>Fornecedor
                        </td>
                        <td>Coleção
                        </td>
                        <td>HB
                        </td>
                        <td>Mostruário
                        </td>
                        <td>Status</td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 230px;">
                            <asp:DropDownList ID="ddlServico" runat="server" Width="224px" Height="21px" DataTextField="DESCRICAO"
                                DataValueField="CODIGO" OnSelectedIndexChanged="ddlServico_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 266px;">
                            <asp:DropDownList ID="ddlFornecedor" runat="server" Width="260px" Height="21px" DataTextField="FORNECEDOR"
                                DataValueField="FORNECEDOR" OnSelectedIndexChanged="ddlFornecedor_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 180px;">
                            <asp:DropDownList ID="ddlColecao" runat="server" Width="174px" Height="21px"
                                DataTextField="DESC_COLECAO" DataValueField="COLECAO">
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
                                <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 230px;">
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="224px" Height="21px" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" AutoPostBack="true">
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
                        <td>&nbsp;</td>
                        <td>
                            <asp:Label ID="labFornecedorSub" runat="server" Text="Sub Fornecedor" Visible="false"></asp:Label>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="labSubStatus" runat="server" Text="SubStatus" Visible="false"></asp:Label>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>

                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlFornecedorSub" runat="server" Width="260px" Height="21px" DataTextField="FORNECEDOR_SUB"
                                DataValueField="FORNECEDOR_SUB" Visible="false">
                            </asp:DropDownList>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSubStatus" runat="server" Width="224px" Height="21px" Visible="false">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="CORTE INTEIRO" Text="CORTE INTEIRO"></asp:ListItem>
                                <asp:ListItem Value="PONTA" Text="PONTA"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>

                    <tr>
                        <td colspan="8">
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">Total de SKUs Atacado:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="labTotalAtacado"
                            ForeColor="Green" Font-Bold="true" runat="server" Text=""></asp:Label>
                        </td>
                        <td style="text-align: right;">
                            <asp:Button ID="btImprimir" runat="server" Text="Imprimir" Width="100px" OnClick="btImprimir_Click" OnClientClick="DesabilitarBotao(this);" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">Total de SKUs Varejo:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                            ID="labTotalVarejo" runat="server" ForeColor="Green" Font-Bold="true" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" style="line-height: 8px;">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
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
                                        <asp:TemplateField HeaderText="Atacado" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="QTDE_ATACADO">
                                            <ItemTemplate>
                                                <asp:Label ID="labAtacado" runat="server" Text='<%# Bind("QTDE_ATACADO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Varejo" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="QTDE_VAREJO">
                                            <ItemTemplate>
                                                <asp:Label ID="labVarejo" runat="server" Text='<%# Bind("QTDE_VAREJO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total" HeaderStyle-Width="65px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="labTotal" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Emissão" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="EMISSAO">
                                            <ItemTemplate>
                                                <asp:Label ID="labEmissao" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Faltante" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="QTDE_FALTANTE">
                                            <ItemTemplate>
                                                <asp:Label ID="labFaltante" runat="server" Text='<%# Bind("QTDE_FALTANTE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="STATUS_ATACADOVAREJO">
                                            <ItemTemplate>
                                                <asp:Label ID="labStatus" runat="server" Text='<%# Bind("STATUS_ATACADOVAREJO") %>' Font-Size="Smaller"></asp:Label>
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
