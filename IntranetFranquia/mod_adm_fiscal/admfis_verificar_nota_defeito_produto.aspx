<%@ Page Title="Produtos - Nota de Defeito " Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="admfis_verificar_nota_defeito_produto.aspx.cs" EnableEventValidation="false" Inherits="Relatorios.admfis_verificar_nota_defeito_produto" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btExcel" />
        </Triggers>
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo ADM Nota Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Administrativo Nota Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Verificar Nota de Defeito&nbsp;&nbsp;>&nbsp;&nbsp;Produtos</span>
                <div style="float: right; padding: 0;">
                    <a href="admfis_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="accountInfo">
                <fieldset>
                    <legend>Produtos - Nota de Defeito</legend>
                    <table border="0" class="style1" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>CD - LUGZY
                            </td>
                            <td>CD LUGZY TRANSITO
                            </td>
                            <td>C-MAX (NOVA)
                            </td>
                            <td>C-MAX (NOVA) MOSTRUARIO
                            </td>
                            <td>C-MAX (NOVA) TRANSITO
                            </td>
                            <td>CD LUCIANA TRANSITO
                            </td>
                            <td>CD MOSTRUARIO
                            </td>
                            <td>CD TAGZY
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 140px;">
                                <asp:TextBox ID="txtCDLugzy" runat="server" Text="" Width="130px" Enabled="false" Style="text-align: right;"></asp:TextBox>
                            </td>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtCDLugzyTransito" runat="server" Text="" Width="150px" Enabled="false" Style="text-align: right;"></asp:TextBox>
                            </td>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtCmaxNova" runat="server" Text="" Width="150px" Enabled="false" Style="text-align: right;"></asp:TextBox>
                            </td>
                            <td style="width: 200px;">
                                <asp:TextBox ID="txtCmaxNovaMostruario" runat="server" Text="" Width="190px" Enabled="false" Style="text-align: right;"></asp:TextBox>
                            </td>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtCmaxNovaTransito" runat="server" Text="" Width="150px" Enabled="false" Style="text-align: right;"></asp:TextBox>
                            </td>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtCDLucianaTransito" runat="server" Text="" Width="150px" Enabled="false" Style="text-align: right;"></asp:TextBox>
                            </td>
                            <td style="width: 140px;">
                                <asp:TextBox ID="txtCDMostruario" runat="server" Text="" Width="130px" Enabled="false" Style="text-align: right;"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCDTagzy" runat="server" Text="" Width="120px" Enabled="false" Style="text-align: right;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btExcel" runat="server" Width="104px" Text="Excel" OnClick="btExcel_Click" />
                                <asp:HiddenField ID="hidCodigoNotaRetirada" runat="server" Value="" />
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                    <table border="0" class="style1" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvNotaRetiradaProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvNotaRetiradaProduto_RowDataBound"
                                        OnDataBound="gvNotaRetiradaProduto_DataBound" AllowSorting="true" ShowFooter="true">
                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PRODUTO" HeaderText="Produto" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="NOME" HeaderText="Nome" />
                                            <asp:BoundField DataField="COR" HeaderText="Cor" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="DESC_COR" HeaderText="Descrição Cor" HeaderStyle-Width="150px" />
                                            <asp:BoundField DataField="COLECAO" HeaderText="Coleção" HeaderStyle-Width="130px" />
                                            <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ORIGEM_DEFEITO" HeaderText="Origem" HeaderStyle-Width="" />
                                            <asp:BoundField DataField="DESCRICAO_DEFEITO" HeaderText="Defeito" HeaderStyle-Width="" />
                                            <asp:BoundField DataField="DATA_LANCAMENTO" HeaderText="Lançamento" HeaderStyle-Width="140px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="DESTINO" HeaderText="Destino" HeaderStyle-Width="" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
