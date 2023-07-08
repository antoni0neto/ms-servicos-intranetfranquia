<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="facc_verificar_hb_liberado.aspx.cs" Inherits="Relatorios.mod_faccao.facc_verificar_hb_liberado" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Facção&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Verificar Liberações para Facção</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>
                        Verificar Liberações para Facção</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>Coleção
                                <asp:HiddenField ID="hidTela" runat="server" />
                            </td>
                            <td>HB
                            </td>
                            <td>Fornecedor
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 250px;">
                                <asp:DropDownList ID="ddlColecoes" runat="server" Width="244px" Height="21px" DataTextField="DESC_COLECAO"
                                    DataValueField="COLECAO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 110px;">
                                <asp:TextBox ID="txtHB" runat="server" Width="100px" MaxLength="5" CssClass="alinharDireita"
                                    onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                            </td>
                            <td style="width: 250px;">
                                <asp:DropDownList ID="ddlFornecedor" runat="server" Width="244px" Height="21px" DataTextField="FORNECEDOR"
                                            DataValueField="FORNECEDOR">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClientClick="DesabilitarBotao(this);" OnClick="btBuscar_Click" />
                                &nbsp;&nbsp;<asp:Label ID="labMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div>
                                    <table border="0" cellpadding="0" class="tb" width="100%">
                                        <tr>
                                            <td style="width: 100%;">
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvLiberados" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" AllowSorting="true"
                                                        ShowFooter="true" DataKeyNames="CODIGO" OnSorting="gvLiberados_Sorting" OnRowCommand="gvLiberados_RowCommand" OnRowDataBound="gvLiberados_RowDataBound">
                                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Coleção" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                SortExpression="DESC_COLECAO">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labCOLECAO" runat="server" Text='<%# Bind("DESC_COLECAO") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hidCOLECAO" runat="server" Value='<%# Eval("COLECAO") %>'></asp:HiddenField>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="HB" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                SortExpression="HB">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labHB" runat="server" Text='<%# Bind("HB") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hidCodigo" runat="server" Value='<%# Eval("CODIGO") %>'></asp:HiddenField>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Mostruário" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                SortExpression="MOSTRUARIO">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labMOSTRUARIO" runat="server" Text='<%# Bind("MOSTRUARIO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Detalhe" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                SortExpression="DETALHE">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labDETALHE" runat="server" Text='<%# Bind("DESCRICAO_DETALHE") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hidPROD_DETALHE" runat="server" Value='<%# Eval("PROD_DETALHE") %>'></asp:HiddenField>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Fornecedor Facção" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                SortExpression="FORNECEDOR">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labFORNECEDOR" runat="server" Text='<%# Bind("FORNECEDOR") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Produto" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                SortExpression="NOME">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labNOME" runat="server" Text='<%# Bind("NOME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Grupo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                SortExpression="GRUPO">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labGRUPO" runat="server" Text='<%# Bind("GRUPO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Cor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                SortExpression="COR">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labCOR" runat="server" Text='<%# Bind("COR") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Cor Fornecedor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                SortExpression="COR_FORNECEDOR">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labCOR_FORNECEDOR" runat="server" Text='<%# Bind("COR_FORNECEDOR") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Fornecedor Tecido" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                SortExpression="FORNECEDOR_TECIDO">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labFORNECEDOR_TECIDO" runat="server" Text='<%# Bind("FORNECEDOR_TECIDO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Tecido" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                SortExpression="TECIDO">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="labTECIDO" runat="server" Text='<%# Bind("TECIDO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
