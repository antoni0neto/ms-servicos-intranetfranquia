<%@ Page Title="Painel de Solicitações" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="rh_sol_painel.aspx.cs" Inherits="Relatorios.rh_sol_painel"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do RH&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;<asp:Label
                    ID="labTituloMenu" runat="server" Text="Painel de Solicitações Finalizadas"></asp:Label></span>
                <div style="float: right; padding: 0;">
                    <a href="rh_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 100%;">
                <fieldset>
                    <legend>
                        <asp:Label ID="labTitulo" runat="server" Text="Painel de Solicitações Finalizadas"></asp:Label></legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>No. Solicitação
                            </td>
                            <td>Filial
                            </td>
                            <td>Tipo de Solicitação
                            </td>
                            <td>Status
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtNumeroSolicitacao" runat="server" Width="150px" CssClass="alinharDireita"
                                    onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                            </td>
                            <td style="width: 240px;">
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="22px" Width="234px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 230px;">
                                <asp:DropDownList runat="server" ID="ddlTipoSolicitacao" DataValueField="CODIGO" DataTextField="DESCRICAO"
                                    Height="22px" Width="224px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 200px; display: none;">
                                <asp:DropDownList runat="server" ID="ddlStatus" DataValueField="CODIGO" DataTextField="DESCRICAO"
                                    Height="22px" Width="194px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click"
                                    OnClientClick="DesabilitarBotao(this);" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvPainel" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                        Style="background: white" OnRowDataBound="gvPainel_RowDataBound" ShowFooter="true">
                                        <HeaderStyle BackColor="Gainsboro" />
                                        <FooterStyle BackColor="Gainsboro" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="No. Solicitação" HeaderStyle-Width="145px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNumeroSolicitacao" runat="server" Text='<%# Bind("NUMERO_SOLICITACAO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Filial" HeaderStyle-Width="235px" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litFilial" runat="server" Text='<%# Bind("FILIAL") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tipo de Solicitação" HeaderStyle-Width="275px" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTipoSolicitacao" runat="server" Text='<%# Bind("TIPO_SOL") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Funcionário" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litFuncionario" runat="server" Text='<%# Bind("NOME") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" HeaderStyle-Width="170px" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litStatus" runat="server" Text='<%# Bind("STATUS") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data" HeaderStyle-Width="145px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litData" runat="server" Text='<%# Bind("DESDE") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="25px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btAbrir" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/search.png"
                                                        OnClick="btAbrir_Click" ToolTip="Abrir Solicitação" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
