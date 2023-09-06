<%@ Page Title="Histórico Representante" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" EnableEventValidation="false" CodeBehind="atac_rel_hist_representante.aspx.cs" Inherits="Relatorios.atac_rel_hist_representante" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .corTD {
            background-color: #EFEFEF;
        }
    </style>
    <link href="../Styles/gridviewScroll.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/1.9.0/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.9.1/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../js/js.js"></script>
    <script type="text/javascript" src="../js/gridviewScroll.min.js"></script>

    <script type="text/javascript">

        function gridviewScroll() {

            gvAtacado = $('#MainContent_gvHistRep').gridviewScroll({
                width: 1450,
                height: 470,
                railcolor: "#F0F0F0",
                barcolor: "#CDCDCD",
                barhovercolor: "#606060",
                bgcolor: "#F0F0F0",
                freezesize: 2,
                arrowsize: 30,
                varrowtopimg: "../Image/arrowvt.png",
                varrowbottomimg: "../Image/arrowvb.png",
                harrowleftimg: "../Image/arrowhl.png",
                harrowrightimg: "../Image/arrowhr.png",
                headerrowcount: 1,
                railsize: 16,
                barsize: 12
            });
        }
	</script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do Atacado&nbsp;&nbsp;>&nbsp;&nbsp;
                    Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;
                    Histórico Representante</span>
                <div style="float: right; padding: 0;">
                    <a href="atac_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Histórico Representante"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Coleção De
                        </td>
                        <td>Até
                        </td>
                        <td>Representante
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 170px;">
                            <asp:DropDownList runat="server" ID="ddlColecaoDe" Height="22px" Width="164px" DataTextField="DESC_COLECAO"
                                DataValueField="COLECAO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 170px;">
                            <asp:DropDownList runat="server" ID="ddlColecaoAte" Height="22px" Width="164px" DataTextField="DESC_COLECAO"
                                DataValueField="COLECAO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 204px;">
                            <asp:DropDownList runat="server" ID="ddlRepresentante"
                                DataValueField="REPRESENTANTE" DataTextField="REPRESENTANTE" Height="22px"
                                Width="198px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;&nbsp;
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
                </table>
                <div class="rounded_corners">
                    <asp:GridView ID="gvHistRep" runat="server" Width="100%" AutoGenerateColumns="False" HeaderStyle-CssClass="GVFixedHeader"
                        ForeColor="#333333" OnRowDataBound="gvHistRep_RowDataBound" OnDataBound="gvHistRep_DataBound" ShowFooter="true">
                        <HeaderStyle BackColor="Gainsboro" />
                        <FooterStyle BackColor="Gainsboro" Font-Bold="true" />
                    </asp:GridView>
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
