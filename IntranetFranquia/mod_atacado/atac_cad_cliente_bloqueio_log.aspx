<%@ Page Title="Liberação Financeiro" Language="C#" AutoEventWireup="true" CodeBehind="atac_cad_cliente_bloqueio_log.aspx.cs"
    Inherits="Relatorios.atac_cad_cliente_bloqueio_log" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bloqueio de Clientes</title>
    <style type="text/css">
        .fs {
            border: 1px solid #ccc;
            font-family: Calibri;
        }

        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }
    </style>
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../js/js.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="up1" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btArquivoSerasaIncluir" />
            </Triggers>
            <ContentTemplate>
                <div style="margin-top: 10px; padding-left: 12px; padding-right: 12px; padding-bottom: -350px; background-color: White;">
                    <br />
                    <div>
                        <span style="font-family: Calibri; font-size: 14px;">Módulo do Atacado&nbsp;&nbsp;>&nbsp;&nbsp;Controle&nbsp;&nbsp;>&nbsp;&nbsp;Liberação Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Cliente</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Liberação Financeiro Cliente</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>Cliente
                                        <asp:HiddenField ID="hidClifor" runat="server" Value="" />
                                        <asp:HiddenField ID="hidColecao" runat="server" Value="" />
                                    </td>
                                    <td colspan="3">Razão Social
                                    </td>
                                    <td colspan="2">CLIFOR</td>
                                    <td>Data de Fundação</td>
                                </tr>
                                <tr>
                                    <td style="width: 310px;">
                                        <asp:TextBox ID="txtCliente" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtRazaoSocial" runat="server" Width="450px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtClifor" runat="server" Width="202px" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDataFundacao" runat="server" Width="196px" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>CNPJ
                                    </td>
                                    <td colspan="3">E-Mail
                                    </td>
                                    <td>DDD</td>
                                    <td>Telefone</td>
                                    <td>Qtde de Protesto
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtCNPJ" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtEmail" runat="server" Width="450px"></asp:TextBox>
                                    </td>
                                    <td style="width: 70px;">
                                        <asp:TextBox ID="txtDDD" runat="server" Width="60px" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                    <td style="width: 142px;">
                                        <asp:TextBox ID="txtTelefone" runat="server" Width="132px" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtQtdeProtesto" runat="server" Width="196px" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Representante
                                    </td>
                                    <td>Data Serasa
                                    </td>
                                    <td>Limite de Crédito
                                    </td>
                                    <td>Tipo Bloqueio
                                    </td>
                                    <td colspan="2">Sem Crédito</td>
                                    <td>Faturamento Bloqueado</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtRepresentante" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 160px;">
                                        <asp:TextBox ID="txtDataSerasa" runat="server" Width="150px" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                    <td style="width: 130px;">
                                        <asp:TextBox ID="txtLimiteCredito" runat="server" Width="120px" CssClass="alinharDireita"></asp:TextBox>
                                    </td>
                                    <td style="width: 170px;">
                                        <asp:DropDownList ID="ddlTipoBloqueio" runat="server" Width="164px" Height="22px" OnSelectedIndexChanged="ddlTipoBloqueio_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                            <asp:ListItem Value="JURIDICO" Text="JURIDICO"></asp:ListItem>
                                            <asp:ListItem Value="LIBERADO" Text="LIBERADO"></asp:ListItem>
                                            <asp:ListItem Value="LIMITE DE CREDITO" Text="LIMITE DE CREDITO"></asp:ListItem>
                                            <asp:ListItem Value="OBSERVACAO" Text="OBSERVACAO"></asp:ListItem>
                                            <asp:ListItem Value="SEM CREDITO" Text="SEM CREDITO"></asp:ListItem>

                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlSemCredito" runat="server" Width="206px" Height="22px">
                                            <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                            <asp:ListItem Value="N" Text="Não" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlBloquearFaturamento" runat="server" Width="200px" Height="22px">
                                            <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                            <asp:ListItem Value="N" Text="Não" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">Inscrição Estadual</td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <asp:TextBox ID="txtInsEstadual" runat="server" Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <fieldset style="margin-top: 4px; padding-top: 0;">
                                            <legend>
                                                <asp:Label ID="labSerasaPDF" runat="server" Text="Arquivo Serasa"></asp:Label></legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 150px; vertical-align: top;">
                                                        <asp:FileUpload ID="uploadArquivoSerasa" runat="server" Width="300px" /><br />
                                                        <br />
                                                        <br />
                                                        <asp:Button ID="btArquivoSerasaIncluir" runat="server" OnClick="btArquivoSerasaIncluir_Click"
                                                            Text=">>" Width="32px" />
                                                    </td>
                                                    <td>
                                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="text-align: right;">
                                                                    <asp:Button ID="btExcluirArquivo" runat="server" Text="Excluir" Width="75px" OnClick="btExcluirArquivo_Click" Visible="false" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: right;">
                                                                    <br />
                                                                    <asp:Image ID="imgArquivoSerasa" runat="server" Width="50px" Height="50px" AlternateText="Nenhum Arquivo..." BorderWidth="0" /><br />
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
                                    <td colspan="7">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td>Observações Gerais
                                                </td>
                                                <td>Observações de Faturamento</td>
                                            </tr>
                                            <tr>
                                                <td style="width: 610px;">
                                                    <asp:TextBox ID="txtObsGeral" runat="server" Width="600px" TextMode="MultiLine" Height="200px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtObsFaturamento" runat="server" Width="565px" TextMode="MultiLine" Height="200px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="7" style="text-align: right;">
                                        <asp:Button ID="btSemCredito" runat="server" Width="150px" Text="Sem Crédito" OnClientClick="DesabilitarBotao(this);" OnClick="btSemCredito_Click" />&nbsp;&nbsp;
                                        <asp:Button ID="btBloquearFaturamento" runat="server" Width="150px" Text="Bloquear Faturamento" OnClientClick="DesabilitarBotao(this);" OnClick="btBloquearFaturamento_Click" />&nbsp;&nbsp;
                                        <asp:Button ID="btSalvar" runat="server" Width="150px" Text="Salvar" OnClientClick="DesabilitarBotao(this);" OnClick="btSalvar_Click" />&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">&nbsp;
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="7">
                                        <fieldset>
                                            <legend>Pedidos da Coleção</legend>
                                            <div>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>Condição de Pagamento</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 210px;">
                                                            <asp:DropDownList ID="ddlCondPgto" runat="server" Width="204px" DataTextField="DESC_COND_PGTO" DataValueField="CONDICAO_PGTO">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btAlterarCondPgto" runat="server" Text="Alterar" Width="120px" OnClick="btAlterarCondPgto_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <br />
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvColecaoPedido" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                    Style="background: white" OnRowDataBound="gvColecaoPedido_RowDataBound" OnDataBound="gvColecaoPedido_DataBound"
                                                    ShowFooter="true">
                                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Bold="true" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Pedido">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litPedido" runat="server" Text='<%# Bind("PEDIDO") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Condição de Pagamento">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litCondPagamento" runat="server" Text='<%# Bind("DESC_COND_PGTO") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Valor Original">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litValorOriginal" runat="server" Text=''></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <fieldset>
                                            <legend>Últimas 4 Compras</legend>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvHistVenda" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                    Style="background: white" OnRowDataBound="gvHistVenda_RowDataBound" OnDataBound="gvHistVenda_DataBound"
                                                    ShowFooter="true">
                                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Bold="true" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Coleção">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColecao" runat="server" Text='<%# Bind("DESC_COLECAO") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SubColeção">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litSubColecao" runat="server" Text='<%# Bind("DESC_SUB_COLECAO") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Valor Original">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litValorOriginal" runat="server" Text=''></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Valor Faturado">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litValorFaturado" runat="server" Text=''></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <fieldset>
                                            <legend>Faturas em Atraso</legend>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvFaturaAtraso" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                    Style="background: white" OnRowDataBound="gvFaturaAtraso_RowDataBound" OnDataBound="gvFaturaAtraso_DataBound"
                                                    ShowFooter="true">
                                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Bold="true" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Coleção">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColecao" runat="server" Text='<%# Bind("DESC_COLECAO") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Nota Fiscal">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litNotaFiscal" runat="server" Text='<%# Bind("FATURA") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Parcela">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litFornecedor" runat="server" Text='<%# Bind("ID_PARCELA") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Data de Vencimento">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litDataVencimento" runat="server" Text=''></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Valor em Atraso">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litValorAtraso" runat="server" Text=''></asp:Literal>
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
                    </div>
                    <br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
