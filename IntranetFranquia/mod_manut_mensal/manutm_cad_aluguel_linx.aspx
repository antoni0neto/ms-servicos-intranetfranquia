<%@ Page Title="Cadastro de Aluguéis" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="manutm_cad_aluguel_linx.aspx.cs" Inherits="Relatorios.manutm_cad_aluguel_linx"
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
    </style>
    <script src="http://code.jquery.com/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btPDFBoleto" />
        </Triggers>
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Manutenção Mensal&nbsp;&nbsp;>&nbsp;&nbsp;Cadastros&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Aluguéis</span>
                <div style="float: right; padding: 0;">
                    <a href="manutm_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 962px; margin-left: 16%;">
                <fieldset>
                    <legend>
                        <asp:Label ID="labTitulo" runat="server" Text="Cadastro de Aluguéis"></asp:Label></legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labVencimento" runat="server" Text="Vencimento"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labDia" runat="server" Text="Dia Vencimento"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labLancamento" runat="server" Text="Lançamento"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 254px;">
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="22px" Width="248px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 210px;">
                                <asp:DropDownList runat="server" ID="ddlVencimento" DataValueField="CODIGO_ACOMPANHAMENTO_MESANO"
                                    DataTextField="DESCRICAO" Height="22px" Width="204px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 150px;">
                                <asp:TextBox ID="txtDia" runat="server" MaxLength="15" Width="140px"
                                    onkeypress="return fnValidarNumero(event);" CssClass="alinharDireita"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLancamento" runat="server" MaxLength="15" Width="150px"
                                    onkeypress="return fnValidarNumero(event);" CssClass="alinharDireita"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="120px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;
                                <asp:Button ID="btLimpar" runat="server" Text="Limpar" Width="120px" OnClick="btLimpar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;<asp:Label ID="labErroBusca" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <fieldset>
                                    <legend>Despesas Linx</legend>

                                    <span style="color: #808080;">No DRE estas despesas são mostradas como competência, ou seja, mês do vencimento - 1.</span>
                                    <br />

                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvBoletoDespesa" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                            Style="background: white" OnRowDataBound="gvBoletoDespesa_RowDataBound" OnDataBound="gvBoletoDespesa_DataBound"
                                            ShowFooter="true"
                                            DataKeyNames="CODIGO_ACOMPANHAMENTO_ALUGUEL">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                            <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" Font-Bold="true" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Conta" HeaderStyle-Width="200px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litConta" runat="server" Text='<%# Bind("DESC_CONTA") %>'></asp:Literal>
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
                                                        <asp:Literal ID="litValor" runat="server" Text=''></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Conta Intranet" HeaderStyle-Width="170px">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlContaIntranet" runat="server" Width="170px" DataValueField="CODIGO_ACOMPANHAMENTO_ALUGUEL_DESPESA"
                                                            DataTextField="DESCRICAO">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </fieldset>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <fieldset style="margin-top: 4px; padding-top: 0;">
                                    <legend>
                                        <asp:Label ID="labBoleto" runat="server" Text="Boleto"></asp:Label></legend>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 150px; vertical-align: top;">
                                                <asp:FileUpload ID="uploadBoleto" runat="server" Width="300px" /><br />
                                                <br />
                                                <br />
                                                <asp:Button ID="btPDFBoleto" runat="server" OnClick="btPDFBoleto_Click"
                                                    Text=">>" Width="32px" />
                                            </td>
                                            <td>
                                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="text-align: right;">
                                                            <asp:Image ID="imgBoleto" runat="server" Width="50px" Height="50px" AlternateText="Nenhum Arquivo..." BorderWidth="0" /><br />
                                                            <asp:HyperLink ID="linkNomePDF" Text='' runat="server" Target="_blank"></asp:HyperLink>
                                                            <asp:HiddenField ID="hidArquivoPDF" runat="server" Value="" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="labObs" runat="server" Text="Observação"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:TextBox ID="txtObs" runat="server" TextMode="MultiLine" Height="40px" Width="919px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Button ID="btEnviar" runat="server" Text="Salvar" Width="110px" OnClick="btEnviar_Click"
                                    OnClientClick="DesabilitarBotao(this);" />&nbsp;
                                <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </td>
                            <td style="text-align: right;">
                                <asp:Button ID="btExcluir" runat="server" Text="Excluir" Width="110px" OnClick="btExcluir_Click"
                                    OnClientClick="DesabilitarBotao(this);" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:HiddenField ID="hidCodigoBoleto" runat="server" Value="" />
                                <asp:HiddenField ID="hidTotalBoleto" runat="server" Value="0" />
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
                                <strong>Boleto</strong>
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
