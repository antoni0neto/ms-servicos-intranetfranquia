<%@ Page Title="Compra de Aviamentos" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="prod_con_compra_aviamento.aspx.cs" Inherits="Relatorios.prod_con_compra_aviamento"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinharDireita
        {
            text-align: right;
        }
    </style>
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
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Aviamentos&nbsp;&nbsp;>&nbsp;&nbsp;Compra
                    de Aviamentos</span>
                <div style="float: right; padding: 0;">
                    <a href="prod_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>Compra de Aviamentos</legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            Coleção
                        </td>
                        <td>
                            HB
                        </td>
                        <td>
                            Aviamento
                        </td>
                        <td>
                            Fornecedor
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 160px;">
                            <asp:DropDownList ID="ddlColecoesBuscar" runat="server" Width="154px" Height="21px"
                                DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 120px;">
                            <asp:TextBox ID="txtHB" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                Width="110px"></asp:TextBox>
                        </td>
                        <td style="width: 200px;">
                            <asp:DropDownList ID="ddlAviamento" runat="server" Width="194px" Height="21px" DataValueField="CODIGO"
                                DataTextField="DESCRICAO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 180px;">
                            <asp:DropDownList ID="ddlFornecedor" runat="server" Width="174px" Height="21px" DataTextField="FORNECEDOR"
                                DataValueField="FORNECEDOR">
                            </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Button ID="btBuscarAviamentos" runat="server" Text="Buscar" OnClick="btBuscarAviamentos_Click"
                                Width="113px" />&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="labErro" runat="server" Text="" ForeColor="red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" style="padding: 0;">
                            <br />
                            <div class="rounded_corners">
                                <asp:GridView ID="gvAviamento" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvAviamento_RowDataBound"
                                    DataKeyNames="CODIGO">
                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
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
                                                <asp:Panel ID="pnlAviamentoHB" runat="server" Style="display: none" Width="100%">
                                                    <asp:GridView ID="gvAviamentoHB" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvAviamentoHB_RowDataBound"
                                                        Width="100%" DataKeyNames="CODIGO">
                                                        <HeaderStyle BackColor="LightSteelBlue" HorizontalAlign="Center"></HeaderStyle>
                                                        <RowStyle BorderColor="GradientActiveCaption" BorderWidth="1px" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="23px" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="HB" HeaderText="HB" HeaderStyle-Width="105px" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="DESC_AVIAMENTO" HeaderText="Aviamento" HeaderStyle-Width="343px"
                                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="QTDE" HeaderText="Qtde" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="DESCRICAO" HeaderText="Descrição" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left" />
                                                            <asp:TemplateField HeaderText="Compra Extra" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litCompraExtra" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FORNECEDOR" HeaderText="Fornecedor" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="QTDE_PACOTE" HeaderText="Qtde de Pacote" HeaderStyle-Width="110px"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="QTDE_POR_PACOTE" HeaderText="Qtde por Pacote" HeaderStyle-Width="110px"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Qtde Total" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeTotal" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Preço por Pacote" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="110px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litPrecoOrcamento" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DESCONTO_ORCAMENTO" HeaderText="Desconto (%)" HeaderStyle-Width="100px"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Valor Total" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValorTotal" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Valor Pacote" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtValorReal" runat="server" Width="100px" CssClass="alinharDireita"
                                                    onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Desconto (%)" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDescontoReal" runat="server" Width="100px" CssClass="alinharDireita"
                                                    onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="65px">
                                            <ItemTemplate>
                                                <asp:Button ID="btCancelar" runat="server" Width="65px" Text="Cancelar" OnClick="btCancelar_Click"
                                                    OnClientClick="return ConfirmarCancelamento();" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="65px">
                                            <ItemTemplate>
                                                <asp:Button ID="btComprar" runat="server" Width="65px" Text="Baixar" OnClick="btComprar_Click"
                                                    OnClientClick="return ConfirmarBaixa();" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" style="text-align: right;">
                            <asp:Label ID="labErroImpressao" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <br />
                <div id="divFinalizarCompra" runat="server">
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                Preço por Pacote Pago
                                <asp:HiddenField ID="hidCodigoOrcamento" runat="server" />
                            </td>
                            <td>
                                Desconto (%)
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 180px;">
                                <asp:TextBox ID="txtPrecoReal" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                    Width="170px"></asp:TextBox>
                            </td>
                            <td style="width: 140px;">
                                <asp:TextBox ID="txtDescontoReal" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                    Width="130px"></asp:TextBox>
                            </td>
                            <td style="width: 150px;">
                                <asp:Button ID="btFinalizar" runat="server" Width="150px" Text="Finalizar Compra"
                                    OnClick="btFinalizar_Click" />
                            </td>
                            <td>
                                &nbsp;&nbsp;<asp:Label ID="labErroFinalizarCompra" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Label ID="labMsgSucesso" runat="server" Text="" ForeColor="Red"></asp:Label>
                <br />
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
