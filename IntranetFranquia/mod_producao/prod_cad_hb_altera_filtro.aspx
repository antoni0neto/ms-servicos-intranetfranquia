<%@ Page Title="HB Alteração Filtro" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="prod_cad_hb_altera_filtro.aspx.cs" Inherits="Relatorios.prod_cad_hb_altera_filtro"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita
        {
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
                    de Corte&nbsp;&nbsp;>&nbsp;&nbsp;<asp:Label ID="labTituloMenu" runat="server"></asp:Label></span>
                <div style="float: right; padding: 0;">
                    <a href="prod_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1000px; margin-left: 15%;">
                <fieldset>
                    <legend>
                        <asp:Label ID="labTitulo" runat="server"></asp:Label></legend>
                    <asp:Panel ID="pnlHBBuscar" runat="server" BorderWidth="0" Visible="true">
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="width: 30px;">
                                    Coleção
                                </td>
                                <td>
                                    HB
                                </td>
                                <td>
                                    Mostruário
                                </td>
                                <td>
                                    <asp:HiddenField ID="hidTela" runat="server" />
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
                                <td style="width: 65px; text-align: center">
                                    <asp:CheckBox ID="chkMostruario" runat="server" Checked="false" />
                                </td>
                                <td>
                                    &nbsp;
                                    <asp:Button ID="btHBBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btHBBuscar_Click" />&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="labHBBuscar" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
