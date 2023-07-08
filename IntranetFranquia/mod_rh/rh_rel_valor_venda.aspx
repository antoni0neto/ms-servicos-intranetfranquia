<%@ Page Title="Calcular Venda" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="rh_rel_valor_venda.aspx.cs" Inherits="Relatorios.rh_rel_valor_venda"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>

    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do RH&nbsp;&nbsp;>&nbsp;&nbsp;Folha&nbsp;&nbsp;>&nbsp;&nbsp;Calcular Venda</span>
                <div style="float: right; padding: 0;">
                    <a href="rh_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Calcular Venda"></asp:Label></legend>
                <fieldset style="margin-top: -10px;">
                    <legend>Filtros</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>Data Inicial
                            </td>
                            <td>Data Final
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 180px;">
                                <asp:TextBox ID="txtDataIni" runat="server" Width="170px" onkeypress="return fnValidarData(event);"
                                    MaxLength="10" Style="text-align: right;" />
                            </td>
                            <td style="width: 180px;">
                                <asp:TextBox ID="txtDataFim" runat="server" Width="170px" onkeypress="return fnValidarData(event);"
                                    MaxLength="10" Style="text-align: right;" />
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <fieldset style="margin-top: 2px;">
                                    <legend>Por Filial (Para cálculo de Gerentes e Subgerentes)</legend>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cbFilial" runat="server" Checked="false" OnCheckedChanged="cbFilial_CheckedChanged" AutoPostBack="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Filial
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                                    Height="22px" Width="250px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <fieldset style="margin-top: -10px;">
                                    <legend>Por Vendedor (Para cálculo de Vendedores)</legend>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cbVendedor" runat="server" Checked="false" OnCheckedChanged="cbVendedor_CheckedChanged" AutoPostBack="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>CPF
                                            </td>
                                            <td>CÓD.Vendedor</td>
                                        </tr>
                                        <tr>
                                            <td style="width: 160px;">
                                                <asp:TextBox ID="txtCPF" runat="server" Width="150px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtVendedor" runat="server" Width="220px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;
                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <fieldset>
                    <legend>Valores</legend>

                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="3">&nbsp;                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvVendedor" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvVendedor_RowDataBound"
                                        OnDataBound="gvVendedor_DataBound" ShowFooter="true">
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Bold="true" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Filial" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="" SortExpression="FILIAL"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litFilial" runat="server" Text='<%# Bind("FILIAL") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Vendedor" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="" SortExpression="VENDEDOR"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-BackColor="" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litVendedor" runat="server" Text='<%# Bind("VENDEDOR") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Nome" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="" SortExpression="NOME_VENDEDOR"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNomeVendedor" runat="server" Text='<%# Bind("NOME_VENDEDOR") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cargo" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="" SortExpression="DESC_CARGO"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litCargo" runat="server" Text='<%# Bind("DESC_CARGO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CPF" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="" SortExpression="CPF"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litCPF" runat="server" Text='<%# Bind("CPF") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="(%) Comissão" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="" SortExpression="COMISSAO"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litComissao" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Val. Vendido" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="" SortExpression="VALOR_VENDA"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValorVendido" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Val. Comissão" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="" SortExpression="VALOR_COMISSAO"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValorComissao" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="(%) Com Ecom" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="" SortExpression="COMISSAO_ECOM"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litComissaoEcom" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Val. Vendido Ecom" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="" SortExpression="VALOR_VENDA_ECOM"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValorVendidoEcom" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Val. Comissão Ecom" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="" SortExpression="VALOR_COMISSAO_ECOM"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValorComissaoEcom" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Val. Venda Tot" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="" SortExpression=""
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValorVendaComissaoTot" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Val. Comissão Tot" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="" SortExpression=""
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValorComissaoTot" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
