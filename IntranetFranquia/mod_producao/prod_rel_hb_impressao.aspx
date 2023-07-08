<%@ Page Title="Impressão de Ficha" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="prod_rel_hb_impressao.aspx.cs" Inherits="Relatorios.prod_rel_hb_impressao"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Ordem
                    de Corte&nbsp;&nbsp;>&nbsp;&nbsp;Impressão de Ficha</span>
                <div style="float: right; padding: 0;">
                    <a href="prod_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Impressão de Ficha"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 30px;">Coleção
                        </td>
                        <td>HB
                        </td>
                        <td>Ficha
                        </td>
                        <td>Mostruário
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 160px;">
                            <asp:DropDownList ID="ddlColecoesBuscar" runat="server" Width="154px" Height="21px"
                                DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 110px;">
                            <asp:TextBox ID="txtHBBuscar" runat="server" Width="100px" MaxLength="5" CssClass="alinharDireita"
                                onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                        </td>
                        <td style="width: 160px">
                            <asp:DropDownList ID="ddlFicha" runat="server" Width="154px" Height="21px">
                                <asp:ListItem Value="" Text="Selecione" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="7" Text="FICHA AMPLIAÇÃO"></asp:ListItem>
                                <asp:ListItem Value="1" Text="FICHA RISCO"></asp:ListItem>
                                <asp:ListItem Value="2" Text="FICHA CORTE"></asp:ListItem>
                                <asp:ListItem Value="3" Text="FACÇÃO"></asp:ListItem>
                                <asp:ListItem Value="4" Text="AVIAMENTO"></asp:ListItem>
                                <asp:ListItem Value="5" Text="LOGÍSTICA"></asp:ListItem>
                                <asp:ListItem Value="6" Text="DETALHES"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 65px; text-align: center">
                            <asp:CheckBox ID="chkMostruario" runat="server" Checked="false" />
                        </td>
                        <td>&nbsp;
                            <asp:Button ID="btImprimir" runat="server" Text="Imprimir" Width="100px" OnClick="btImprimir_Click"
                                OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="labImpressao" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
