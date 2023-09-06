<%@ Page Title="Controle de Vendedores" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="rh_alt_venda_vendedor.aspx.cs" Inherits="Relatorios.rh_alt_venda_vendedor"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .jGrowl .manilla {
            color: black;
        }

        .jGrowl .redError {
            color: red;
        }

        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }
    </style>
    <script src="http://code.jquery.com/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do RH&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Vendedores&nbsp;&nbsp;>&nbsp;&nbsp;<asp:Label
                    ID="labTituloMenu" runat="server" Text="Alteração de Venda de Vendedor"></asp:Label></span>
                <div style="float: right; padding: 0;">
                    <a href="rh_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 962px; margin-left: 16%;">
                <fieldset>
                    <legend>
                        <asp:Label ID="labTitulo" runat="server" Text="Alteração de Venda de Vendedor"></asp:Label></legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labDataVenda" runat="server" Text="Data Venda"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labTicket" runat="server" Text="Ticket"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 260px;">
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="22px" Width="254px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 220px;">
                                <asp:TextBox ID="txtDataVenda" runat="server" MaxLength="10" Width="210px"
                                    onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                            </td>
                            <td style="width: 220px;">
                                <asp:TextBox ID="txtTicket" runat="server" MaxLength="15" Width="210px"
                                    onkeypress="return fnValidarNumero(event);" CssClass="alinharDireita"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click"
                                    OnClientClick="DesabilitarBotao(this);" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text="&nbsp;"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvTicket" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvTicket_RowDataBound" DataKeyNames="VENDEDOR">
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Vendedor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litVendedor" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valor" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValor" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Para Vendedor" HeaderStyle-Width="340px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlVendedor" DataValueField="VENDEDOR" DataTextField="NOME"
                                                        Height="22px" Width="340px">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="65px" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Button ID="btAlterar" runat="server" Height="19px" Width="70px" OnClientClick="return Confirmar('Deseja realmente Alterar este Ticket?');" OnClick="btAlterar_Click" Text="Alterar" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="labMsg" runat="server" ForeColor="Red" Font-Bold="true" Text="&nbsp;"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
