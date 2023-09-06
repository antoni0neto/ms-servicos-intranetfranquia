<%@ Page Title="Clientes Inadimplentes" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" EnableEventValidation="false" CodeBehind="atac_rel_cliente_inadimplente.aspx.cs" Inherits="Relatorios.atac_rel_cliente_inadimplente" %>

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
            <asp:PostBackTrigger ControlID="btExcel" />
        </Triggers>
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do Atacado&nbsp;&nbsp;>&nbsp;&nbsp;
                    Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;
                    Clientes Inadimplentes</span>
                <div style="float: right; padding: 0;">
                    <a href="atac_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Clientes Inadimplentes"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Representante
                        </td>
                        <td>Coleção
                        </td>
                        <td>Clientes Inadimplentes
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 300px;">
                            <asp:DropDownList runat="server" ID="ddlRepresentante"
                                DataValueField="REPRESENTANTE" DataTextField="REPRESENTANTE" Height="22px"
                                Width="294px">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 204px;">
                            <asp:DropDownList runat="server" ID="ddlColecao" DataValueField="COLECAO" DataTextField="DESC_COLECAO" Height="22px"
                                Width="198px">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 200px;">
                            <asp:DropDownList runat="server" ID="ddlClienteAtrasado" Height="22px" Width="194px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 300px;">
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td style="text-align: right;">&nbsp;
                            <asp:Button runat="server" ID="btExcel" Text="Excel" Width="100px" OnClick="btExcel_Click" ToolTip="Extrair as informações para Excel." />
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
                                <asp:GridView ID="gvRepresentante" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvRepresentante_RowDataBound" OnDataBound="gvRepresentante_DataBound" ShowFooter="true">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" Font-Bold="true" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1px">
                                            <ItemTemplate>
                                                <asp:Image ID="imgExpand" alt="" Style="cursor: pointer" src="../../Image/plus.png"
                                                    Width="18px" runat="server" />
                                                <asp:Panel ID="pnlRepreDetalhe" runat="server" Style="display: none" Width="100%">
                                                    <asp:GridView ID="gvCliente" runat="server" CellPadding="0" CellSpacing="0"
                                                        AutoGenerateColumns="false" OnRowDataBound="gvCliente_RowDataBound" OnDataBound="gvCliente_DataBound" ShowFooter="true"
                                                        Width="100%">
                                                        <HeaderStyle BackColor="LightBlue"></HeaderStyle>
                                                        <FooterStyle BackColor="LightBlue" Font-Bold="true"></FooterStyle>
                                                        <RowStyle BorderColor="Gainsboro" Height="22px" Font-Bold="false" BorderWidth="1" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColunaDet" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Clifor" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-BorderWidth="1" HeaderStyle-Width="100px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litCodigoCliente" runat="server" Text='<%# Bind("COD_CLIENTE") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Cliente" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-BorderWidth="1">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litCliente" runat="server" Text='<%# Bind("CLIENTE_ATACADO") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Coleção" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-BorderWidth="1" HeaderStyle-Width="252px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litDescColecao" runat="server" Text='<%# Bind("DESC_COLECAO") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Valor Total" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="251px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litValorTotal" runat="server" Text=''></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Valor em Atraso" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="252px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litValorAtraso" runat="server" Text=''></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="% Inadimplente" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="250px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litInadimplentePerc" runat="server" Text=''></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Representante" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litRepresentante" runat="server" Text='<%# Bind("REPRESENTANTE") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Clientes" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="250px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litTotCliente" runat="server" Text='<%# Bind("CLIENTE_ATACADO") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Valor Total" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="250px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValorTotal" runat="server" Text=''></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Valor em Atraso" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="250px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValorAtraso" runat="server" Text=''></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="% Inadimplente" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="250px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litInadimplentePerc" runat="server" Text=''></asp:Literal>
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
                        <asp:BoundField DataField="REPRESENTANTE" HeaderText="Representante" />
                        <asp:BoundField DataField="COD_CLIENTE" HeaderText="Código Cliente" />
                        <asp:BoundField DataField="CLIENTE_ATACADO" HeaderText="Cliente" />
                        <asp:BoundField DataField="COLECAO" HeaderText="Coleção" />
                        <asp:BoundField DataField="DESC_COLECAO" HeaderText="Descrição Coleção" />
                        <asp:BoundField DataField="VALOR_ORIGINAL" HeaderText="Valor Total" />
                        <asp:BoundField DataField="VALOR_EM_ATRASO" HeaderText="Valor em Atraso" />
                        <asp:TemplateField HeaderText="% Inadimplente">
                            <ItemTemplate>
                                <asp:Literal ID="litInadimplentePerc" runat="server" Text='<%# (100.00*Double.Parse(Eval("VALOR_EM_ATRASO").ToString()) / (Double.Parse(Eval("VALOR_ORIGINAL").ToString()))).ToString() %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
