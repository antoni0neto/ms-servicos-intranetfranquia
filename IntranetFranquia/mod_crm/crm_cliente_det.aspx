<%@ Page Title="Resumo do Cliente" Language="C#" AutoEventWireup="true" CodeBehind="crm_cliente_det.aspx.cs"
    Inherits="Relatorios.crm_cliente_det" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Clientes - Detalhe</title>
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
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
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
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
                <div style="margin-top: 10px; padding-left: 12px; padding-right: 12px; padding-bottom: -350px; background-color: White;">
                    <br />
                    <div>
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de CRM&nbsp;&nbsp;>&nbsp;&nbsp;Clientes&nbsp;&nbsp;>&nbsp;&nbsp;Resumo do Cliente</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Resumo do Cliente</legend>
                            <fieldset>
                                <legend>Dados</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>Código Cliente
                                            <asp:HiddenField ID="hidCPF" runat="server" Value="" />
                                        </td>
                                        <td>CPF</td>
                                        <td>Cliente</td>
                                        <td>E-Mail</td>
                                        <td>Telefone</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 170px;">
                                            <asp:TextBox ID="txtCodigoCliente" runat="server" Width="160px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 150px;">
                                            <asp:TextBox ID="txtCPF" runat="server" Width="140px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 350px;">
                                            <asp:TextBox ID="txtCliente" runat="server" Width="340px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 320px;">
                                            <asp:TextBox ID="txtEmail" runat="server" Width="310px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtTelefone" runat="server" Width="200px" Enabled="false"></asp:TextBox>&nbsp;
                                            <asp:ImageButton ID="btWapp" runat="server" Width="15px" ImageUrl="~/Image/whatsapp.png" ToolTip="Número de Telefone" OnClick="btWapp_Click" />&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Tipo de Cliente</td>
                                        <td colspan="5">Observação
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:DropDownList ID="ddlTipoCliente" runat="server" Width="164px" Height="21px" DataTextField="TIPO" DataValueField="CODIGO">
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtObs" runat="server" Width="100%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" style="text-align: left;">
                                            <asp:Button ID="btSalvar" runat="server" Text="Atualizar" Width="120px" OnClick="btSalvar_Click" />&nbsp;&nbsp;<asp:Label ID="labErro" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">&nbsp;</td>
                                    </tr>
                                </table>
                            </fieldset>
                            <fieldset>
                                <legend>Handclub e Cupom</legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>Saldo Mês Atual</td>
                                        <td>
                                            <asp:HiddenField ID="hidCodigoPrograma" runat="server" Value="" />
                                        </td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 170px;">
                                            <asp:TextBox ID="txtSaldoAtual" runat="server" Width="160px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="btZerarHandClub" runat="server" Text="Zerar Handclub" Width="120px" OnClick="btZerarHandClub_Click" />
                                        </td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>Tipo de Cupom</td>
                                        <td>Frete Grátis</td>
                                        <td>Valor</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 170px;">
                                            <asp:DropDownList ID="ddlCupomTipo" runat="server" Width="164px" Height="21px">
                                                <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                                <asp:ListItem Value="H" Text="Handclub" Selected="True"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 150px;">
                                            <asp:DropDownList ID="ddlFreteGratis" runat="server" Width="144px" Height="21px">
                                                <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Sim"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Não"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 170px;">
                                            <asp:TextBox ID="txtCupomVal" runat="server" Enabled="true" Width="160px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="btGerarCupom" runat="server" Text="Gerar Cupom" Width="120px" OnClick="btGerarCupom_Click" />&nbsp;&nbsp;<asp:Label ID="labCupomErro" runat="server" ForeColor="Red" Text="&nbsp;" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvCupom" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvCupom_RowDataBound" OnDataBound="gvCupom_DataBound" ShowFooter="true"
                                                    DataKeyNames="CPF">
                                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Bold="true"></FooterStyle>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Data Criação" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="250px" SortExpression="DATA_CRIACAO"
                                                            HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cupom" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width=""
                                                            HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litCupom" runat="server" Text='<%# Bind("CUPOM") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Frete Grátis" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" SortExpression="VALOR"
                                                            HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litFreteGratis" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Valor Cupom" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="250px" SortExpression="VALOR"
                                                            HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litValCupom" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Exclusão" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="250px"
                                                            HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litDataExc" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="25px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btExcluirCupom" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/delete.png"
                                                                    OnClick="btExcluirCupom_Click" ToolTip="Excluir Cupom" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <fieldset>
                                            <legend>Ações Realizadas</legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <div class="rounded_corners">
                                                            <asp:GridView ID="gvAcao" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                                Style="background: white" OnRowDataBound="gvAcao_RowDataBound"
                                                                ShowFooter="true">
                                                                <HeaderStyle BackColor="GradientActiveCaption" />
                                                                <FooterStyle BackColor="GradientActiveCaption" Font-Bold="true" HorizontalAlign="Left" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Data" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Ação" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAcao" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="" HeaderText="" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <fieldset>
                                            <legend>Últimas Peças que
                                    <asp:Label ID="labNomeClienteTit2" runat="server" Font-Bold="true"></asp:Label>
                                                comprou</legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="text-align: center;">
                                                        <asp:ImageButton ID="imgProduto1" runat="server" Width="140px" ImageAlign="AbsMiddle" ToolTip=" " AlternateText=" " Visible="false" /><br />
                                                        <asp:Label ID="labProduto1" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:ImageButton ID="imgProduto2" runat="server" Width="140px" ImageAlign="AbsMiddle" ToolTip=" " AlternateText=" " Visible="false" /><br />
                                                        <asp:Label ID="labProduto2" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:ImageButton ID="imgProduto3" runat="server" Width="140px" ImageAlign="AbsMiddle" ToolTip=" " AlternateText=" " Visible="false" /><br />
                                                        <asp:Label ID="labProduto3" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:ImageButton ID="imgProduto4" runat="server" Width="140px" ImageAlign="AbsMiddle" ToolTip=" " AlternateText=" " Visible="false" /><br />
                                                        <asp:Label ID="labProduto4" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:ImageButton ID="imgProduto5" runat="server" Width="140px" ImageAlign="AbsMiddle" ToolTip=" " AlternateText=" " Visible="false" /><br />
                                                        <asp:Label ID="labProduto5" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:ImageButton ID="imgProduto6" runat="server" Width="140px" ImageAlign="AbsMiddle" ToolTip=" " AlternateText=" " Visible="false" /><br />
                                                        <asp:Label ID="labProduto6" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <fieldset>
                                            <legend>O que
                                    <asp:Label ID="labNomeClienteTit1" runat="server" Font-Bold="true"></asp:Label>
                                                mais compra</legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <div class="rounded_corners">
                                                            <asp:GridView ID="gvMaisCompra" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                                Style="background: white" OnRowDataBound="gvMaisCompra_RowDataBound"
                                                                ShowFooter="true">
                                                                <HeaderStyle BackColor="GradientActiveCaption" />
                                                                <FooterStyle BackColor="GradientActiveCaption" Font-Bold="true" HorizontalAlign="Left" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Griffe" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litGriffe" runat="server" Text='<%# Bind("GRIFFE") %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Grupo Produto" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litGrupoProduto" runat="server" Text='<%# Bind("GRUPO_PRODUTO") %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Qtde de Produto" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litQtdeProduto" runat="server" Text='<%# Bind("QTDE_GRUPO_PRODUTO") %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="" HeaderText="" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <fieldset>
                                            <legend>
                                                <asp:Label ID="labHistoricoMensal" runat="server" Text="Histórico"></asp:Label>
                                            </legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="text-align: right;">
                                                        <asp:Button ID="btCarregarHistorico" runat="server" Text="Carregar Todo Histórico" Width="200px" OnClick="btCarregarHistorico_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div class="rounded_corners">
                                                            <asp:GridView ID="gvHistCliente" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                                Style="background: white" OnRowDataBound="gvHistCliente_RowDataBound" OnDataBound="gvHistCliente_DataBound"
                                                                ShowFooter="true">
                                                                <HeaderStyle BackColor="GradientActiveCaption" />
                                                                <FooterStyle BackColor="GradientActiveCaption" Font-Bold="true" HorizontalAlign="Left" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center"
                                                                        ItemStyle-VerticalAlign="Middle">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../Image/plus.png"
                                                                                Width="15px" runat="server" />
                                                                            <asp:Panel ID="pnlProduto" runat="server" Style="display: none" Width="100%">
                                                                                <asp:GridView ID="gvHistClienteProduto" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvHistClienteProduto_RowDataBound" OnDataBound="gvHistClienteProduto_DataBound" ShowFooter="true"
                                                                                    Width="100%">
                                                                                    <HeaderStyle BackColor="LightSteelBlue" HorizontalAlign="Center"></HeaderStyle>
                                                                                    <RowStyle BorderColor="GradientActiveCaption" BorderWidth="1px" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                                            <ItemTemplate>
                                                                                                <asp:ImageButton ID="imgProduto" runat="server" Width="90%" ImageAlign="AbsMiddle" ToolTip=" " AlternateText=" " />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Tipo" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="TIPO_VENDA" HeaderStyle-HorizontalAlign="Left">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litTipo" runat="server" Text='<%# Bind("TIPO_VENDA") %>'></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>

                                                                                        <asp:BoundField DataField="SKU" HeaderText="SKU" HeaderStyle-Width="200px" SortExpression="SKU" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                                                        <asp:BoundField DataField="QTDE" HeaderText="Qtde" HeaderStyle-Width="200px" SortExpression="QTDE" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                                                        <asp:TemplateField HeaderText="Preço" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" SortExpression="PRECO" HeaderStyle-HorizontalAlign="Left">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litPreco" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:BoundField DataField="" HeaderText="" HeaderStyle-Width="200px" SortExpression="QTDE" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                                                        <asp:TemplateField HeaderText="Desconto" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" SortExpression="DESCONTO" HeaderStyle-HorizontalAlign="Left">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litDesconto" runat="server" Text=''></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Valor Total" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" SortExpression="VALOR_TOTAL" HeaderStyle-HorizontalAlign="Left">
                                                                                            <ItemTemplate>
                                                                                                <asp:Literal ID="litValTotal" runat="server" Text=''></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Venda" HeaderStyle-Width="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litTipo" runat="server" Text=''></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Ano" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAno" runat="server" Text='<%# Bind("ANO") %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Mês" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMes" runat="server" Text='<%# Bind("MES_EXT") %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Tickets" HeaderStyle-Width="200px" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litQtdePedido" runat="server" Text='<%# Bind("TOT_PEDIDO") %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Valor Bruto" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litValorBruto" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Valor Frete" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litValorFrete" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Desconto" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litDesconto" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Valor Pago" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litValorPago" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <fieldset>
                                            <legend>Histórico Tipo do Cliente</legend>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <div class="rounded_corners">
                                                            <asp:GridView ID="gvTipoCliente" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                                Style="background: white" OnRowDataBound="gvTipoCliente_RowDataBound"
                                                                ShowFooter="true">
                                                                <HeaderStyle BackColor="GradientActiveCaption" />
                                                                <FooterStyle BackColor="GradientActiveCaption" Font-Bold="true" HorizontalAlign="Left" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Data" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Tipo Cliente" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litTipoCliente" runat="server"></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Observação" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litObs" runat="server" Text='<%# Bind("OBS") %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
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
