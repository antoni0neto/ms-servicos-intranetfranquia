<%@ Page Title="Pré-Pedido - Resumo" Language="C#" AutoEventWireup="true" CodeBehind="desenv_tripa_prepedido_resumo.aspx.cs"
    Inherits="Relatorios.desenv_tripa_prepedido_resumo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Pré-Pedido - Resumo</title>
    <style type="text/css">
        .fs {
            border: 1px solid #ccc;
            font-family: Calibri;
        }

        .alinharDireita {
            text-align: right;
        }
    </style>
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript">
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Planejamento
                                                                de Coleção Nacional&nbsp;&nbsp;>&nbsp;&nbsp;Resumo - Pré-Pedido de Tecido</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Resumo - Pré-Pedido de Tecido</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="labGrupoProduto" runat="server" Text="Grupo Produto"></asp:Label>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width: 350px;">
                                        <asp:DropDownList ID="ddlGrupoProduto" runat="server" Width="344px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                            DataValueField="GRUPO_PRODUTO">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btIncluirGrupo" runat="server" Width="100px" Text="Incluir" OnClick="btIncluirGrupo_Click" />
                                        <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvPrePedidoCota" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvPrePedidoCota_RowDataBound"
                                                OnDataBound="gvPrePedidoCota_DataBound" ShowFooter="true"
                                                DataKeyNames="COLECAO, GRIFFE, DESENV_PRODUTO_ORIGEM">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                                <FooterStyle BackColor="Gainsboro" Font-Bold="true" Font-Size="Smaller" />
                                                <PagerStyle HorizontalAlign="Center" CssClass="pageStyl" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="15px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField HeaderText="Griffe" DataField="GRIFFE" HeaderStyle-Width="75px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />
                                                    <asp:BoundField HeaderText="Origem" DataField="ORIGEM" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />

                                                    <asp:TemplateField HeaderText="Cota Atacado" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="LightBlue">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litCotaAtacado" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Valor Atacado" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="LightBlue">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litValorAtacado" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Dif Atacado" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="LightBlue">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litDiffAtacado" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Cota Varejo" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="LightGreen">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litCotaVarejo" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Valor Varejo" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="LightGreen">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litValorVarejo" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Dif Varejo" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="LightGreen">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litDiffVarejo" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Cota Total" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="LightCoral">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTotalCota" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Valor Total" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="LightCoral">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTotalValor" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Dif Total" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" FooterStyle-Font-Size="Smaller" ItemStyle-BackColor="LightCoral">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTotalDiff" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvPrePedido" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvPrePedido_RowDataBound"
                                                OnDataBound="gvPrePedido_DataBound" ShowFooter="true"
                                                OnSorting="gvPrePedido_Sorting" AllowSorting="true"
                                                DataKeyNames="GRUPO_PRODUTO">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                                <FooterStyle BackColor="Gainsboro" Font-Bold="true" />
                                                <PagerStyle HorizontalAlign="Center" CssClass="pageStyl" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="15px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField HeaderText="Grupo Produto" DataField="GRUPO_PRODUTO" SortExpression="GRUPO_PRODUTO" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />
                                                    <asp:BoundField HeaderText="Griffe" DataField="GRIFFE" SortExpression="GRIFFE" HeaderStyle-Width="75px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />

                                                    <asp:TemplateField HeaderText="SKU Plan Atacado" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" SortExpression="SKU_PLAN_ATACADO">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtSKUPlanejadoAtacado" runat="server" Width="90px" Text='<%#Bind("SKU_PLAN_ATACADO") %>' OnTextChanged="txtSKUPlanejado_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="SKU DEF Atacado" DataField="SKU_ATACADO" SortExpression="SKU_ATACADO" HeaderStyle-Width="95px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />
                                                    <asp:BoundField HeaderText="Dif SKU Atacado" DataField="SKU_ATACADO_DIFF" SortExpression="SKU_ATACADO_DIFF" HeaderStyle-Width="95px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />
                                                    <asp:BoundField HeaderText="Qtde Atacado" DataField="QTDE_ATACADO" SortExpression="QTDE_ATACADO" HeaderStyle-Width="95px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />
                                                    <asp:TemplateField HeaderText="Tot Atacado" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                                        SortExpression="VALOR_TOTAL_ATACADO">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litValorTotalAtacado" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SKU Plan Varejo" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" SortExpression="SKU_PLAN_VAREJO">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtSKUPlanejadoVarejo" runat="server" Width="90px" Text='<%#Bind("SKU_PLAN_VAREJO") %>' OnTextChanged="txtSKUPlanejado_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="SKU DEF Varejo" DataField="SKU_VAREJO" SortExpression="SKU_VAREJO" HeaderStyle-Width="95px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />
                                                    <asp:BoundField HeaderText="Dif SKU Varejo" DataField="SKU_VAREJO_DIFF" SortExpression="SKU_VAREJO_DIFF" HeaderStyle-Width="95px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />
                                                    <asp:BoundField HeaderText="Qtde Varejo" DataField="QTDE_VAREJO" SortExpression="QTDE_VAREJO" HeaderStyle-Width="85px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />
                                                    <asp:TemplateField HeaderText="Tot Varejo" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                                        SortExpression="VALOR_TOTAL_VAREJO">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litValorTotalVarejo" runat="server"></asp:Literal>
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
                    <div>
                        <asp:HiddenField ID="hidMarca" runat="server" Value="" />
                        <asp:HiddenField ID="hidColecao" runat="server" Value="" />
                        <asp:HiddenField ID="hidGriffe" runat="server" Value="" />
                        <asp:HiddenField ID="hidDesenvOrigem" runat="server" Value="" />
                    </div>
                    <br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
