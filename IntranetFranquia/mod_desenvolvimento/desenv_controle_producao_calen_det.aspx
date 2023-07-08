<%@ Page Title="Calendário Controle de Produção" Language="C#" AutoEventWireup="true" CodeBehind="desenv_controle_producao_calen_det.aspx.cs"
    Inherits="Relatorios.desenv_controle_producao_calen_det" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Venda de Produtos Semana</title>
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
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../js/js.js" type="text/javascript"></script>

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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Calendário Controle de Produção</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Calendário Controle de Produção</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="8">
                                        <asp:HiddenField ID="hidColecao" runat="server" Value="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Coleção</td>
                                    <td>Local</td>
                                    <td>Griffe</td>
                                    <td>Grupo Produto</td>
                                    <td>Previsão Inicial</td>
                                    <td>Previsão Final</td>
                                    <td>Recebido -90%?</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width: 135px;">
                                        <asp:TextBox ID="txtColecao" runat="server" Width="125px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 170px;">
                                        <asp:DropDownList ID="ddlLocal" runat="server" Width="164px" Height="21px">
                                            <asp:ListItem Value="pre-modelo" Text="">PRÉ-MODELO</asp:ListItem>
                                            <asp:ListItem Value="modelagem_de_criacao" Text="">MODELAGEM DE CRIAÇÃO</asp:ListItem>
                                            <asp:ListItem Value="design_de_estampas" Text="">DESIGN DE ESTAMPAS</asp:ListItem>
                                            <asp:ListItem Value="modelagem_e_design" Text="">MODELAGEM E DESIGN</asp:ListItem>
                                            <asp:ListItem Value="1ª_peca" Text="">1ª PEÇA</asp:ListItem>
                                            <asp:ListItem Value="modelagem" Text="">MODELAGEM</asp:ListItem>
                                            <asp:ListItem Value="pre-risco" Text="PRÉ-RISCO"></asp:ListItem>
                                            <asp:ListItem Value="risco" Text="RISCO"></asp:ListItem>
                                            <asp:ListItem Value="corte" Text="CORTE"></asp:ListItem>
                                            <asp:ListItem Value="encaixe_faccao" Text="ENCAIXE FACÇÃO"></asp:ListItem>
                                            <asp:ListItem Value="faccao" Text="FACÇÃO"></asp:ListItem>
                                            <asp:ListItem Value="encaixe_acabamento" Text="ENCAIXE ACABAMENTO"></asp:ListItem>
                                            <asp:ListItem Value="acabamento" Text="ACABAMENTO"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 150px">
                                        <asp:DropDownList ID="ddlGriffe" runat="server" Width="144px" Height="21px" DataTextField="GRIFFE"
                                            DataValueField="GRIFFE">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 180px;">
                                        <asp:DropDownList ID="ddlGrupo" runat="server" Width="174px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                            DataValueField="GRUPO_PRODUTO">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 120px;">
                                        <asp:TextBox ID="txtDataPrevIni" runat="server" autocomplete="off" Width="110px" MaxLength="10" Style="text-align: right;"
                                            onkeypress="return fnValidarData(event);"></asp:TextBox>
                                    </td>
                                    <td style="width: 120px;">
                                        <asp:TextBox ID="txtDataPrevFim" runat="server" autocomplete="off" Width="110px" MaxLength="10" Style="text-align: right;"
                                            onkeypress="return fnValidarData(event);"></asp:TextBox>
                                    </td>
                                    <td style="width: 130px;">
                                        <asp:DropDownList ID="ddl90Porc" runat="server" Width="124px" Height="21px">
                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                            <asp:ListItem Value="S" Text="Sim" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btBuscar" runat="server" Width="100px" Text="Buscar" OnClick="btBuscar_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                Style="background: white" OnRowDataBound="gvProduto_RowDataBound" OnDataBound="gvProduto_DataBound" ShowFooter="true"
                                                OnSorting="gvProduto_Sorting" AllowSorting="true">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                                <FooterStyle BackColor="Gainsboro" Font-Bold="true" />
                                                <RowStyle HorizontalAlign="Left" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgProduto" runat="server" Width="90%" ImageAlign="AbsMiddle" OnClick="imgProduto_Click" ToolTip="" AlternateText="" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Origem" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width=""
                                                        SortExpression="ORIGEM" ItemStyle-Font-Size="Smaller">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litOrigem" runat="server" Text='<%# Bind("ORIGEM") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Griffe" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width=""
                                                        SortExpression="GRIFFE" ItemStyle-Font-Size="Smaller">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litGriffe" runat="server" Text='<%# Bind("GRIFFE") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Produto" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px"
                                                        SortExpression="PRODUTO">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litProduto" runat="server" Text='<%# Bind("PRODUTO") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="HB" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px"
                                                        SortExpression="HB">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litHB" runat="server" Text='<%# Bind("HB") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Nome" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width=""
                                                        SortExpression="DESC_PRODUTO" ItemStyle-Font-Size="Smaller">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litDescProduto" runat="server" Text='<%# Bind("DESC_PRODUTO") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width=""
                                                        SortExpression="DESC_COR" ItemStyle-Font-Size="Smaller">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litDescCor" runat="server" Text='<%# Bind("DESC_COR") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tecido" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width=""
                                                        SortExpression="TECIDO" ItemStyle-Font-Size="Smaller">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTecido" runat="server" Text='<%# Bind("TECIDO") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Qtde Varejo" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="100px"
                                                        SortExpression="QTDE_TOTAL">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litQtdeTotal" runat="server" Text='<%# Bind("QTDE_TOTAL") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Facção" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="100px"
                                                        SortExpression="FORNECEDOR" ItemStyle-Font-Size="Smaller">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litFornecedor" runat="server" Text='<%# Bind("FORNECEDOR") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Dias Entrada" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="110px"
                                                        SortExpression="TEMPO_MODULO_DIAS">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litDiasEntrada" runat="server" Text='<%# Bind("TEMPO_MODULO_DIAS") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Prev. Dias" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="120px"
                                                        SortExpression="TEMPO_PREVISAO_DIAS">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litDiasPrevisao" runat="server" Text='<%# Bind("TEMPO_PREVISAO_DIAS") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Previsão" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="80px"
                                                        SortExpression="DATA_PREVISAO">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litDataPrevisao" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="V. Facção" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderStyle-Width="80px"
                                                        SortExpression="VALOR_TOT_FACCAO">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litValorFaccaoTotal" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
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
