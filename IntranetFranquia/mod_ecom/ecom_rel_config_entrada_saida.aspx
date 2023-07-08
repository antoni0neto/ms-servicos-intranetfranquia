<%@ Page Title="Relacionados - Configurações Entrada e Saída" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="ecom_rel_config_entrada_saida.aspx.cs" Inherits="Relatorios.ecom_rel_config_entrada_saida"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">

    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Produtos Relacionados&nbsp;&nbsp;>&nbsp;&nbsp;Configurações Entrada e Saída</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Configurações Entrada e Saída"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Regra Entrada</td>
                        <td>Regra Saída</td>
                        <td>Qtde de Produto</td>
                        <td>
                            <asp:HiddenField ID="hidOrdem" runat="server" Value=" ASC " />
                            <asp:HiddenField ID="hidCampo" runat="server" Value=" ECOM_REL_CONFIG_DE " />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 400px">
                            <asp:DropDownList ID="ddlRegraEntrada" runat="server" Width="394px" Height="21px" DataTextField="NOME"
                                DataValueField="CODIGO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 400px">
                            <asp:DropDownList ID="ddlRegraSaida" runat="server" Width="394px" Height="21px" DataTextField="NOME"
                                DataValueField="CODIGO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 250px;">
                            <asp:TextBox ID="txtTopQtde" runat="server" Width="240px" MaxLength="2" Text="" />
                        </td>
                        <td>
                            <asp:Button ID="btSalvar" runat="server" Width="100px" Text="Incluir" OnClick="btSalvar_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvConfig" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvConfig_RowDataBound"
                                    OnDataBound="gvConfig_DataBound" OnSorting="gvConfig_Sorting" AllowSorting="true" ShowFooter="true">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Bold="true" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Regra de Entrada" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" SortExpression="ECOM_REL_CONFIG.NOME">
                                            <ItemTemplate>
                                                <asp:Literal ID="litEntrada" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Regra de Saída" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" SortExpression="ECOM_REL_CONFIG1.NOME">
                                            <ItemTemplate>
                                                <asp:Literal ID="litSaida" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qtde de Produto" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" SortExpression="QTDE_PRODUTO">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtde" runat="server" Text='<%#Bind("QTDE_PRODUTO")%>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="65px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button ID="btExcluir" runat="server" Text="Excluir" Width="65px" OnClick="btExcluir_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
