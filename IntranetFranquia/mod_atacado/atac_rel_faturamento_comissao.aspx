<%@ Page Title="Comissão de Representante" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" EnableEventValidation="false" CodeBehind="atac_rel_faturamento_comissao.aspx.cs" Inherits="Relatorios.atac_rel_faturamento_comissao" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../../Image/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "../../Image/plus.png");
            $(this).closest("tr").next().remove();
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btExcelComissao" />
        </Triggers>
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do Atacado&nbsp;&nbsp;>&nbsp;&nbsp;
                    Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;
                    Comissão de Representante</span>
                <div style="float: right; padding: 0;">
                    <a href="atac_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Comissão de Representante"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Ano
                        </td>
                        <td>Mês
                        </td>
                        <td>Representante
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 170px;">
                            <asp:DropDownList runat="server" ID="ddlAno" Height="22px" Width="164px" DataTextField="ANO"
                                DataValueField="ANO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 170px;">
                            <asp:DropDownList runat="server" ID="ddlMes" Height="22px" Width="164px">
                                <asp:ListItem Value="1" Text="Janeiro"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Fevereiro"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Março"></asp:ListItem>
                                <asp:ListItem Value="4" Text="Abril"></asp:ListItem>
                                <asp:ListItem Value="5" Text="Maio"></asp:ListItem>
                                <asp:ListItem Value="6" Text="Junho"></asp:ListItem>
                                <asp:ListItem Value="7" Text="Julho"></asp:ListItem>
                                <asp:ListItem Value="8" Text="Agosto"></asp:ListItem>
                                <asp:ListItem Value="9" Text="Setembro"></asp:ListItem>
                                <asp:ListItem Value="10" Text="Outubro"></asp:ListItem>
                                <asp:ListItem Value="11" Text="Novembro"></asp:ListItem>
                                <asp:ListItem Value="12" Text="Dezembro"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 204px;">
                            <asp:DropDownList runat="server" ID="ddlRepresentante"
                                DataValueField="REPRESENTANTE" DataTextField="REPRESENTANTE" Height="22px"
                                Width="198px">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 300px;">
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td style="text-align: right;">&nbsp;
                            <asp:Button runat="server" ID="btExcelComissao" Text="Excel" Width="100px" OnClick="btExcelComissao_Click" ToolTip="Extrair as informações para Excel." />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvComissao" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvComissao_RowDataBound" OnDataBound="gvComissao_DataBound" ShowFooter="true">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" Font-Bold="true" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-BorderWidth="1px">
                                            <ItemTemplate>
                                                <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                    Width="18px" runat="server" />
                                                <asp:Panel ID="pnlComissaoDetalhe" runat="server" Style="display: none" Width="100%">
                                                    <asp:Panel ID="pnlFaturamento" runat="server" BackColor="LightSteelBlue">
                                                        <br />
                                                        <asp:Literal ID="litFaturamento" runat="server" Text="<span style='font-weight:bold;'>&nbsp;&nbsp;FATURADO</span>"></asp:Literal>
                                                        <br />
                                                        <br />
                                                    </asp:Panel>
                                                    <asp:GridView ID="gvFaturado" runat="server" CellPadding="0" CellSpacing="0"
                                                        AutoGenerateColumns="false" OnRowDataBound="gvDetalhe_RowDataBound" OnDataBound="gvDetalhe_DataBound" ShowFooter="true"
                                                        Width="100%">
                                                        <HeaderStyle BackColor="LightBlue"></HeaderStyle>
                                                        <FooterStyle BackColor="LightBlue" Font-Bold="true"></FooterStyle>
                                                        <RowStyle BorderColor="Gainsboro" Height="22px" Font-Bold="false" BorderWidth="1" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColunaDet" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Cliente" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litCliente" runat="server" Text='<%# Bind("CLIENTE") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Coleção" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-Width="150px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColecao" runat="server" Text='<%# Bind("DESC_COLECAO") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Pedido" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="100px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litPedido" runat="server" Text='<%# Bind("PEDIDO") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="NF Saída" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="130px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litNotaFiscal" runat="server" Text='<%# Bind("NOTA") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-Width="140px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Quantidade" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="100px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litQtde" runat="server" Text='<%# Bind("QTDE_TOTAL") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Valor" ItemStyle-HorizontalAlign="Right"
                                                                ItemStyle-Width="125px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litValor" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="% Com." ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="80px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litComissaoPorc" runat="server" Text='<%# Bind("COMISSAO_PORC") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Comissão" ItemStyle-HorizontalAlign="Right"
                                                                ItemStyle-Width="125px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litComissao" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Descrição" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-BorderWidth="1" ItemStyle-Width="230px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litMotivo" runat="server" Text='<%# Bind("MOTIVO") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <%//DEVOLVIDOS %>

                                                    <asp:Panel ID="pnlDevolvido" runat="server" BackColor="LightSteelBlue">
                                                        <br />
                                                        <asp:Literal ID="litDevolvido" runat="server" Text="<span style='font-weight:bold;'>&nbsp;&nbsp;DEVOLVIDO</span>"></asp:Literal>
                                                        <br />
                                                        <br />
                                                    </asp:Panel>
                                                    <asp:GridView ID="gvDevolvido" runat="server" CellPadding="0" CellSpacing="0"
                                                        AutoGenerateColumns="false" OnRowDataBound="gvDetalhe_RowDataBound" OnDataBound="gvDetalhe_DataBound" ShowFooter="true"
                                                        Width="100%">
                                                        <HeaderStyle BackColor="LightBlue"></HeaderStyle>
                                                        <FooterStyle BackColor="LightBlue" Font-Bold="true"></FooterStyle>
                                                        <RowStyle BorderColor="Gainsboro" Height="22px" Font-Bold="false" BorderWidth="1" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColunaDet" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Cliente" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litCliente" runat="server" Text='<%# Bind("CLIENTE") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Coleção" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-Width="150px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColecao" runat="server" Text='<%# Bind("DESC_COLECAO") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Pedido" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="100px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litPedido" runat="server" Text='<%# Bind("PEDIDO") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="NF Entrada" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="130px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litNotaFiscal" runat="server" Text='<%# Bind("NOTA") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-Width="140px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Quantidade" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="100px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litQtde" runat="server" Text='<%# Bind("QTDE_TOTAL") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Valor" ItemStyle-HorizontalAlign="Right"
                                                                ItemStyle-Width="125px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litValor" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="% Com." ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="80px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litComissaoPorc" runat="server" Text='<%# Bind("COMISSAO_PORC") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Comissão" ItemStyle-HorizontalAlign="Right"
                                                                ItemStyle-Width="125px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litComissao" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Descrição" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-BorderWidth="1" ItemStyle-Width="230px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litMotivo" runat="server" Text='<%# Bind("MOTIVO") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>

                                                    <%//CANCELADOS %>
                                                    <asp:Panel ID="pnlCancelado" runat="server" BackColor="LightSteelBlue">
                                                        <br />
                                                        <asp:Literal ID="litCancelado" runat="server" Text="<span style='font-weight:bold;'>&nbsp;&nbsp;CANCELADO</span>"></asp:Literal>
                                                        <br />
                                                        <br />
                                                    </asp:Panel>
                                                    <asp:GridView ID="gvCancelado" runat="server" CellPadding="0" CellSpacing="0"
                                                        AutoGenerateColumns="false" OnRowDataBound="gvDetalhe_RowDataBound" OnDataBound="gvDetalhe_DataBound" ShowFooter="true"
                                                        Width="100%">
                                                        <HeaderStyle BackColor="LightBlue"></HeaderStyle>
                                                        <FooterStyle BackColor="LightBlue" Font-Bold="true"></FooterStyle>
                                                        <RowStyle BorderColor="Gainsboro" Height="22px" Font-Bold="false" BorderWidth="1" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColunaDet" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Cliente" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litCliente" runat="server" Text='<%# Bind("CLIENTE") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Coleção" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-Width="150px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColecao" runat="server" Text='<%# Bind("DESC_COLECAO") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Pedido" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="100px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litPedido" runat="server" Text='<%# Bind("PEDIDO") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="-" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="130px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litNotaFiscal" runat="server" Text='<%# Bind("NOTA") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-Width="140px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Quantidade" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="100px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litQtde" runat="server" Text='<%# Bind("QTDE_TOTAL") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Valor" ItemStyle-HorizontalAlign="Right"
                                                                ItemStyle-Width="125px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litValor" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="% Com." ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="80px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litComissaoPorc" runat="server" Text='<%# Bind("COMISSAO_PORC") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Comissão" ItemStyle-HorizontalAlign="Right"
                                                                ItemStyle-Width="125px" ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litComissao" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Descrição" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-BorderWidth="1" ItemStyle-Width="230px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litMotivo" runat="server" Text='<%# Bind("MOTIVO") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <br />
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Representante" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litRepresentante" runat="server" Text='<%# Bind("REPRESENTANTE") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Filial" HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litFilial" runat="server" Text='<%# Bind("FILIAL") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qtde Total" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeTotal" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Valor Total" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValorTotal" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Comissão" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Literal ID="litComissao" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Último E-Mail Enviado" HeaderStyle-Width="230px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litDataUltimoEmail" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="25px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btEmail" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/email.png"
                                                    OnClick="btEmail_Click" ToolTip="E-Mail" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <asp:Panel ID="pnlExcel" runat="server">
                <asp:GridView ID="gvExcel" runat="server" CellPadding="0" CellSpacing="0" Width="100%" AutoGenerateColumns="False">
                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                    <FooterStyle BackColor="Gainsboro" Font-Bold="true" />
                    <Columns>
                        <asp:BoundField DataField="RAZAO_SOCIAL" HeaderText="Razão Social" />
                        <asp:BoundField DataField="REPRESENTANTE" HeaderText="Representante" />
                        <asp:BoundField DataField="CPFCNPJ" HeaderText="CNPJ" />
                        <asp:BoundField DataField="CLIENTE" HeaderText="Cliente" />
                        <asp:BoundField DataField="FILIAL" HeaderText="Filial" />
                        <asp:BoundField DataField="PEDIDO" HeaderText="No. Pedido" />
                        <asp:BoundField DataField="NOTA" HeaderText="Nota Fiscal" />
                        <asp:BoundField DataField="DATA" HeaderText="Data" />
                        <asp:BoundField DataField="DESC_COLECAO" HeaderText="Coleção" />
                        <asp:BoundField DataField="QTDE_TOTAL" HeaderText="Quantidade Total" />
                        <asp:BoundField DataField="VALOR_TOTAL" HeaderText="Valor Total" />
                        <asp:BoundField DataField="COMISSAO" HeaderText="Comissão" />
                        <asp:BoundField DataField="COMISSAO_PORC" HeaderText="% Comissão" />
                        <asp:BoundField DataField="STATUS" HeaderText="Status" />
                        <asp:BoundField DataField="MOTIVO" HeaderText="Descrição" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
