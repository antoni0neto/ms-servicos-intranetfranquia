<%@ Page Title="Status Facção" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="facc_resumo_status.aspx.cs" Inherits="Relatorios.facc_resumo_status"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
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
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Facção&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Status Facção</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Status Facção"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Fornecedor</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 266px;">
                            <asp:DropDownList ID="ddlFornecedor" runat="server" Width="260px" Height="21px" DataTextField="FORNECEDOR"
                                DataValueField="FORNECEDOR" OnSelectedIndexChanged="ddlFornecedor_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />
                        </td>
                        <td style="text-align: right;">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="labFornecedorSub" runat="server" Text="Sub Fornecedor" Visible="false"></asp:Label>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>

                        <td>
                            <asp:DropDownList ID="ddlFornecedorSub" runat="server" Width="260px" Height="21px" DataTextField="FORNECEDOR_SUB"
                                DataValueField="FORNECEDOR_SUB" Visible="false">
                            </asp:DropDownList>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvFaccaoStatus" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                    Style="background: white" OnRowDataBound="gvFaccaoStatus_RowDataBound" OnDataBound="gvFaccaoStatus_DataBound"
                                    OnSorting="gvFaccaoStatus_Sorting"
                                    ShowFooter="true" AllowSorting="true">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fornecedor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="FORNECEDOR">
                                            <ItemTemplate>
                                                <asp:Label ID="labFornecedor" runat="server"></asp:Label>
                                                <asp:HiddenField ID="hidFornecedor" runat="server" Value='<%# Bind("FORNECEDOR") %>' />
                                                <asp:HiddenField ID="hidFornecedorSub" runat="server" Value='<%# Bind("FORNECEDOR_SUB") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Empresa" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" Visible="false"
                                            SortExpression="EMPRESA">
                                            <ItemTemplate>
                                                <asp:Label ID="labEmpresa" runat="server" Text='<%# Bind("EMPRESA") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="C. Grande" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="CORTE_GRANDE">
                                            <ItemTemplate>
                                                <asp:Label ID="labCorteGrande" runat="server" Text='<%# Bind("CORTE_GRANDE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="C. Pequeno" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="CORTE_PEQUENO">
                                            <ItemTemplate>
                                                <asp:Label ID="labCortePequeno" runat="server" Text='<%# Bind("CORTE_PEQUENO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="C. Grande ATR" HeaderStyle-Width="145px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="CORTE_GRANDE_ATR">
                                            <ItemTemplate>
                                                <asp:Label ID="labCorteGrandeAtrasado" runat="server" Text='<%# Bind("CORTE_GRANDE_ATR") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="C. Pequeno ATR" HeaderStyle-Width="145px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="CORTE_PEQUENO_ATR">
                                            <ItemTemplate>
                                                <asp:Label ID="labCortePequenoAtrasado" runat="server" Text='<%# Bind("CORTE_PEQUENO_ATR") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mostruário" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="MOSTRUARIO">
                                            <ItemTemplate>
                                                <asp:Label ID="labMostruario" runat="server" Text='<%# Bind("MOSTRUARIO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="T. C. Grande" HeaderStyle-Width="135px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="TOTAL_GRANDE">
                                            <ItemTemplate>
                                                <asp:Label ID="labCorteGrandeTotal" runat="server" Text='<%# Bind("TOTAL_GRANDE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="T. C. Pequeno" HeaderStyle-Width="135px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="TOTAL_PEQUENO">
                                            <ItemTemplate>
                                                <asp:Label ID="labCortePequenoTotal" runat="server" Text='<%# Bind("TOTAL_PEQUENO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="TOTAL">
                                            <ItemTemplate>
                                                <asp:Label ID="labCorteTotal" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-Width="25px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btAbrir" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/search.png"
                                                    OnClick="btAbrir_Click" ToolTip="Detalhes" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">&nbsp;
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
