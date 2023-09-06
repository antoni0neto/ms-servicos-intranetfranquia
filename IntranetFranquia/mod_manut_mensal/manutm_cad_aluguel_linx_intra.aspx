<%@ Page Title="Cadastro de Aluguéis" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="manutm_cad_aluguel_linx_intra.aspx.cs" Inherits="Relatorios.manutm_cad_aluguel_linx_intra"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .alinharDireita {
            text-align: right;
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Manutenção Mensal&nbsp;&nbsp;>&nbsp;&nbsp;Cadastros&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Aluguéis</span>
                <div style="float: right; padding: 0;">
                    <a href="manutm_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 100%;">
                <fieldset>
                    <legend>
                        <asp:Label ID="labTitulo" runat="server" Text="Cadastro de Aluguéis"></asp:Label></legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="labLancamento" runat="server" Text="Lançamento"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 164px;">
                                <asp:TextBox ID="txtLancamento" runat="server" MaxLength="15" Width="150px" Text=""
                                    onkeypress="return fnValidarNumero(event);" CssClass="alinharDireita"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="120px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="labErroBusca" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset>
                                    <legend>Despesas Linx</legend>
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvBoletoDespesa" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                            Style="background: white" OnRowDataBound="gvBoletoDespesa_RowDataBound" OnDataBound="gvBoletoDespesa_DataBound"
                                            ShowFooter="true"
                                            DataKeyNames="RATEIO_FILIAL, CODIGO_ACOMPANHAMENTO_ALUGUEL">
                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Bold="true" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="cbMarcar" runat="server" Checked="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Conta" HeaderStyle-Width="200px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litConta" runat="server" Text='<%# Bind("DESC_CONTA") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Filial" HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFilial" runat="server" Text='<%# Bind("FILIAL") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litItem" runat="server" Text='<%# Bind("ITEM") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Histórico">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litHistorico" runat="server" Text='<%# Bind("HISTORICO") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Valor" HeaderStyle-Width="120px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litValor" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Desconto/Acréscimo" HeaderStyle-Width="130px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDesconto" runat="server" Width="130px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Conta Intranet" HeaderStyle-Width="170px">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlContaIntranet" runat="server" Width="170px" DataValueField="CODIGO_ACOMPANHAMENTO_ALUGUEL_DESPESA"
                                                            DataTextField="DESCRICAO">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Vencimento" HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtVencimento" runat="server" Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Competência"  HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litCompetencia" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </fieldset>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="labCompetencia" runat="server" Text="Competência"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlCompetencia" DataValueField="CODIGO_ACOMPANHAMENTO_MESANO"
                                    DataTextField="DESCRICAO" Height="21px" Width="158px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btEnviar" runat="server" Text="Salvar" Width="120px" OnClick="btEnviar_Click"
                                    OnClientClick="DesabilitarBotao(this);" />&nbsp;
                                <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div id="dialogPai" runat="server">
                <div id="dialog" title="Mensagem" class="divPop">
                    <table border="0" width="100%">
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; color: red;">
                                <strong>Vencimentos</strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; color: red;">
                                <asp:Label ID="labMensagem" runat="server" Text="ATUALIZADO COM SUCESSO"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
