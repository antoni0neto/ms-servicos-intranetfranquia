<%@ Page Title="Conferência de Mercadoria por Produto" Language="C#" AutoEventWireup="true"
    CodeBehind="estoque_mercadoria_conferencia_detalhe.aspx.cs" Inherits="Relatorios.estoque_mercadoria_conferencia_detalhe"
    MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Conferência de Mercadoria por Filial</title>
    <style type="text/css">
        .fs
        {
            border: 1px solid #ccc;
            font-family: Calibri;
        }
        .tb
        {
            padding: 0;
            font-family: Calibri;
            font-size: 14px;
        }
        .alinharDireita
        {
            text-align: right;
        }
    </style>
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
    <form id="thisForm" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div style="margin-top: 10px; padding-left: 12px; padding-right: 12px; padding-bottom: -350px;
                background-color: White;">
                <br />
                <div>
                    <span style="font-family: Calibri; font-size: 14px;">Módulo de Controle de Estoque&nbsp;&nbsp;>&nbsp;&nbsp;Estoque
                        Loja&nbsp;&nbsp;>&nbsp;&nbsp;Conferência de Mercadoria&nbsp;&nbsp;>&nbsp;&nbsp;Conferência
                        de Mercadoria por Produto</span>
                    <div style="float: right; padding: 0;">
                        <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                    </div>
                </div>
                <hr />
                <div>
                    <fieldset class="fs">
                        <legend>Conferência de Mercadoria por Produto</legend>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <span style="font-size: 14px;">Produto:</span> <span style="font-weight: bold; font-size: 14px;">
                                        &nbsp;<asp:Label ID="labProduto" runat="server" Text=""></asp:Label></span>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Filial
                                </td>
                                <td>
                                    <asp:HiddenField ID="hidProduto" runat="server" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 256px;">
                                    <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                        Height="21px" Width="250px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btFiltrar" runat="server" Width="110px" Text="Buscar" OnClick="btFiltrar_Click" />
                                    &nbsp;&nbsp;<asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <fieldset>
                                        <legend>Produto Recebido</legend>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvProdutoRecebido" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvProdutoRecebido_RowDataBound"
                                                DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center"
                                                        ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <img alt="" style="cursor: pointer" src="../Image/plus.png" width="18px" />
                                                            <asp:Panel ID="pnlNota" runat="server" Style="display: none" Width="100%">
                                                                <asp:GridView ID="gvNotaMercadoriaRecebida" runat="server" AutoGenerateColumns="false"
                                                                    OnRowDataBound="gvNotaMercadoriaRecebida_RowDataBound" Width="100%" DataKeyNames="CODIGO">
                                                                    <HeaderStyle BackColor="LightSteelBlue" HorizontalAlign="Center"></HeaderStyle>
                                                                    <RowStyle BorderColor="GradientActiveCaption" BorderWidth="1px" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="23px" ItemStyle-HorizontalAlign="Center"
                                                                            ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="NUMERO_NF_TRANSFERENCIA" HeaderText="NF" ItemStyle-BorderColor="GradientActiveCaption"
                                                                            ItemStyle-BorderWidth="1px" HeaderStyle-Width="177px" HeaderStyle-HorizontalAlign="Left" />
                                                                        <asp:TemplateField HeaderText="Emissão" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                                            ItemStyle-BorderColor="GradientActiveCaption" HeaderStyle-Width="151px" ItemStyle-BorderWidth="1px">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="litEmissao" runat="server"></asp:Literal>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="SERIE_NF_ENTRADA" HeaderText="Série" ItemStyle-BorderColor="GradientActiveCaption"
                                                                            ItemStyle-BorderWidth="1px" HeaderStyle-Width="94px" HeaderStyle-HorizontalAlign="Left" />
                                                                        <asp:TemplateField HeaderText="Grade" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                                            ItemStyle-BorderColor="GradientActiveCaption" HeaderStyle-Width="95px" ItemStyle-BorderWidth="1px">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="litTipoNota" runat="server"></asp:Literal>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="EXP_RECEBIDO" HeaderText="EXP" ItemStyle-BorderColor="GradientActiveCaption"
                                                                            ItemStyle-BorderWidth="1px" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" />
                                                                        <asp:BoundField DataField="XP_RECEBIDO" HeaderText="XP" ItemStyle-BorderColor="GradientActiveCaption"
                                                                            ItemStyle-BorderWidth="1px" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" />
                                                                        <asp:BoundField DataField="PP_RECEBIDO" HeaderText="PP" ItemStyle-BorderColor="GradientActiveCaption"
                                                                            ItemStyle-BorderWidth="1px" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" />
                                                                        <asp:BoundField DataField="P_RECEBIDO" HeaderText="P" ItemStyle-BorderColor="GradientActiveCaption"
                                                                            ItemStyle-BorderWidth="1px" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" />
                                                                        <asp:BoundField DataField="M_RECEBIDO" HeaderText="M" ItemStyle-BorderColor="GradientActiveCaption"
                                                                            ItemStyle-BorderWidth="1px" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" />
                                                                        <asp:BoundField DataField="G_RECEBIDO" HeaderText="G" ItemStyle-BorderColor="GradientActiveCaption"
                                                                            ItemStyle-BorderWidth="1px" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" />
                                                                        <asp:BoundField DataField="GG_RECEBIDO" HeaderText="GG" ItemStyle-BorderColor="GradientActiveCaption"
                                                                            ItemStyle-BorderWidth="1px" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FILIAL" HeaderText="Filial" HeaderStyle-Width="180px"
                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="DESC_COR" HeaderText="Cor" HeaderStyle-Width="151px" ItemStyle-HorizontalAlign="Left"
                                                        HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="QTDE_ENTRADA_NOTA" HeaderText="Qtde Nota" HeaderStyle-Width="95px"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="QTDE_ENTRADA_REAL" HeaderText="Qtde Recebida" HeaderStyle-Width="95px"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="QTDE_SOMADA" HeaderText="Diferença" HeaderStyle-Width="90px"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="STATUS" HeaderText="Recebido" HeaderStyle-Width="115px"
                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="110px" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlSolucao" runat="server" Width="110px" Height="19px" OnSelectedIndexChanged="ddlSolucao_SelectedIndexChanged"
                                                                AutoPostBack="true">
                                                                <asp:ListItem Value="" Text="" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Value="L" Text="EM ANÁLISE"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Observação" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtObservacao" runat="server" Width="123px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btConferir" runat="server" Width="60px" Height="19px" Text="Conferir"
                                                                OnClientClick="return ConfirmarBaixa();" OnClick="btConferir_Click" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <fieldset>
                                        <legend>Produto Em Trânsito</legend>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvProdutoEmTransito" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvProdutoEmTransito_RowDataBound"
                                                DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FILIAL" HeaderText="Filial" HeaderStyle-Width="208px"
                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="DESC_COR" HeaderText="Cor" HeaderStyle-Width="155px" ItemStyle-HorizontalAlign="Left"
                                                        HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="QTDE_ENTRADA_NOTA" HeaderText="Qtde Nota" HeaderStyle-Width="95px"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="NUMERO_NF_TRANSFERENCIA" HeaderText="NF" ItemStyle-BorderColor="GradientActiveCaption"
                                                        ItemStyle-BorderWidth="1px" HeaderStyle-Width="189px" HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="SERIE_NF_ENTRADA" HeaderText="Série" ItemStyle-BorderColor="GradientActiveCaption"
                                                        ItemStyle-BorderWidth="1px" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="EMISSAO" HeaderText="Emissão" DataFormatString="{0:d}"
                                                        ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px" HeaderStyle-HorizontalAlign="Left" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Panel ID="pnlProdutoOK" runat="server" Visible="false">
                                        <fieldset>
                                            <legend>Produto OK</legend>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvProdutoRecebidoOK" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvProdutoRecebidoOK_RowDataBound"
                                                    DataKeyNames="CODIGO">
                                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FILIAL" HeaderText="Filial" HeaderStyle-Width="180px"
                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="DESC_COR" HeaderText="Cor" HeaderStyle-Width="155px" ItemStyle-HorizontalAlign="Left"
                                                            HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="QTDE_ENTRADA_NOTA" HeaderText="Qtde Nota" HeaderStyle-Width="100px"
                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="QTDE_ENTRADA_REAL" HeaderText="Qtde Recebida" HeaderStyle-Width="100px"
                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="QTDE_SOMADA" HeaderText="Diferença" HeaderStyle-Width="90px"
                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="STATUS" HeaderText="Status" HeaderStyle-Width="120px"
                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:TemplateField HeaderText="Solução" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litSolucao" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="OBSERVACAO" HeaderText="Observação" ItemStyle-HorizontalAlign="Left"
                                                            HeaderStyle-HorizontalAlign="Left" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </fieldset>
                                    </asp:Panel>
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
