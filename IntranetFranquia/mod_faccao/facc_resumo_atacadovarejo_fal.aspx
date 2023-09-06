<%@ Page Title="Facção por Atacado/Varejo Faltante" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="facc_resumo_atacadovarejo_fal.aspx.cs" Inherits="Relatorios.facc_resumo_atacadovarejo_fal"
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
        <Triggers>
            <asp:PostBackTrigger ControlID="btExcel" />
        </Triggers>
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Facção&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Facção por Atacado/Varejo Faltante</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Facção por Atacado/Varejo Faltante"></asp:Label></legend>
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
                        <td>Fornecedor</td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 160px;">
                            <asp:DropDownList ID="ddlServico" runat="server" Width="154px" Height="21px" DataTextField="DESCRICAO"
                                DataValueField="CODIGO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 160px;">
                            <asp:DropDownList ID="ddlColecao" runat="server" Width="154px" Height="21px"
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
                            <asp:TextBox ID="txtHB" runat="server" Width="96px" MaxLength="5" CssClass="alinharDireita"
                                onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                        </td>
                        <td style="width: 120px;">
                            <asp:DropDownList ID="ddlMostruario" runat="server" Width="114px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                <asp:ListItem Value="N" Text="Não" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 288px;">
                            <asp:DropDownList ID="ddlFornecedor" runat="server" Width="282px" Height="21px"
                                DataTextField="FORNECEDOR" DataValueField="CODIGO" OnSelectedIndexChanged="ddlFornecedor_SelectedIndexChanged" AutoPostBack="true">
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
                        <td>Problema Facção
                        </td>
                        <td>
                            <asp:Label ID="labDataPrevEntregaIni" runat="server" Text="Previsão Entrega Início"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="labDataPrevEntregaFim" runat="server" Text="Previsão Entrega Fim"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="labRemoverHandbook" runat="server" Text="Retirar Handbook"></asp:Label>
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
                            <asp:DropDownList ID="ddlFaccaoProblema" runat="server" Width="154px" Height="21px" DataTextField="PROBLEMA" DataValueField="CODIGO">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDataPrevIni" runat="server" autocomplete="off" Width="146px" MaxLength="10" Style="text-align: right;"
                                onkeypress="return fnValidarData(event);"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDataPrevFim" runat="server" autocomplete="off" Width="166px" MaxLength="10" Style="text-align: right;"
                                onkeypress="return fnValidarData(event);"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlRemoverHandbook" runat="server" Width="184px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="S" Text="Sim" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlFornecedorSub" runat="server" Width="224px" Height="21px" DataTextField="FORNECEDOR_SUB"
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
                        <td colspan="6">&nbsp;</td>
                        <td colspan="3" style="text-align: right;">
                            <asp:Button ID="btExcel" runat="server" Text="Excel" Width="100px" OnClick="btExcel_Click" OnClientClick="DesabilitarBotao(this);" />
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
                                    OnSorting="gvFaccaoHB_Sorting" ShowFooter="true" AllowSorting="true"
                                    DataKeyNames="PROD_HB">
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
                                        <asp:TemplateField HeaderText="HB" HeaderStyle-Width="55px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="HB">
                                            <ItemTemplate>
                                                <asp:Label ID="labHB" runat="server" Text='<%# Bind("HB") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Produto" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="PRODUTO">
                                            <ItemTemplate>
                                                <asp:Label ID="labProduto" runat="server" Text='<%# Bind("PRODUTO") %>'></asp:Label>
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
                                        <asp:TemplateField HeaderText="Varejo" HeaderStyle-Width="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="QTDE_VAREJO">
                                            <ItemTemplate>
                                                <asp:Label ID="labVarejo" runat="server" Text='<%# Bind("QTDE_VAREJO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Varejo Falt" HeaderStyle-Width="110px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="QTDE_VAREJO_FALTANTE">
                                            <ItemTemplate>
                                                <asp:Label ID="labFaltanteVarejo" runat="server" Text='<%# Bind("QTDE_VAREJO_FALTANTE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Envio" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" DataField="ENVIO" SortExpression="ENVIO" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="false" />
                                        <asp:TemplateField HeaderText="Tempo na Facção" HeaderStyle-Width="130px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="DIFERENCA">
                                            <ItemTemplate>
                                                <asp:Label ID="labDiferenca" runat="server" Text='<%# Bind("DIFERENCA") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fornecedor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="FORNECEDOR">
                                            <ItemTemplate>
                                                <asp:Label ID="labFornecedor" runat="server" Text='<%# Bind("FORNECEDOR") %>' Font-Size="Smaller"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Obs" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="OBS_REL_FALTANTE">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtObsFaltante" runat="server" Text='<%# Bind("OBS_REL_FALTANTE") %>' Width="120px" Font-Size="Smaller" OnTextChanged="txtObsFaltante_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Problema" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="PROBLEMA">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlFaccaoProblema" runat="server" Width="100px" DataTextField="PROBLEMA" DataValueField="CODIGO" OnSelectedIndexChanged="ddlFaccaoProblema_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Prev Entrega" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="DATA_PREV_ENTREGA">
                                            <ItemTemplate>
                                                <asp:Label ID="labDataPrevEntrega" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Prazo de Entrega" HeaderStyle-Width="110px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="labPrazoEntrega" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">&nbsp;
                        </td>
                    </tr>
                </table>
            </fieldset>
            <div>
                <asp:Panel ID="pnlExcel" runat="server">
                    <asp:GridView ID="gvExcel" runat="server" Width="100%" AutoGenerateColumns="False" OnRowDataBound="gvExcel_RowDataBound"
                        ForeColor="#333333" Style="background: white" ShowFooter="true">
                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="SERVICO" HeaderText="Serviço" />
                            <asp:BoundField DataField="HB" HeaderText="HB" />
                            <asp:BoundField DataField="NOME" HeaderText="Nome" />
                            <asp:BoundField DataField="QTDE_VAREJO" HeaderText="Qtde Varejo" />
                            <asp:BoundField DataField="QTDE_VAREJO_FALTANTE" HeaderText="Qtde Varejo Fal" />
                            <asp:BoundField DataField="ENVIO" HeaderText="Envio" />
                            <asp:BoundField DataField="DIFERENCA" HeaderText="Diferença" />
                            <asp:BoundField DataField="FORNECEDOR" HeaderText="Fornecedor" />
                            <asp:BoundField DataField="OBS_REL_FALTANTE" HeaderText="Observação" />
                            <asp:BoundField DataField="PROBLEMA" HeaderText="Problema" />
                            <asp:BoundField DataField="DATA_PREV_ENTREGA" HeaderText="Previsão" />
                            <asp:TemplateField HeaderText="Prazo de Entrega">
                                <ItemTemplate>
                                    <asp:Literal ID="litPrazoEntrega" runat="server" Text=''></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
