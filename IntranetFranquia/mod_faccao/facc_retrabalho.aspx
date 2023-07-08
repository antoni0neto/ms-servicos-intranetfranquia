<%@ Page Title="Facção Retrabalho" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="facc_retrabalho.aspx.cs" Inherits="Relatorios.facc_retrabalho"
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
    <script type="text/javascript">
        function SomarGrade(control) {
            var total = 0;

            if (document.getElementById("MainContent_txtGradeEXP_E").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeEXP_E").value);

            if (document.getElementById("MainContent_txtGradeXP_E").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeXP_E").value);

            if (document.getElementById("MainContent_txtGradePP_E").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradePP_E").value);

            if (document.getElementById("MainContent_txtGradeP_E").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeP_E").value);

            if (document.getElementById("MainContent_txtGradeM_E").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeM_E").value);

            if (document.getElementById("MainContent_txtGradeG_E").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeG_E").value);

            if (document.getElementById("MainContent_txtGradeGG_E").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeGG_E").value);

            document.getElementById("MainContent_txtGradeTotal_E").value = total;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Facção&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Facção&nbsp;&nbsp;>&nbsp;&nbsp;Facção Retrabalho</span>
                <div style="float: right; padding: 0;">
                    <a href="facc_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Facção Retrabalho"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Coleção
                            <asp:HiddenField ID="hidCodigoHB" runat="server" />
                        </td>
                        <td>HB
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 180px;">
                            <asp:DropDownList ID="ddlColecao" runat="server" Width="174px" Height="21px"
                                DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 200px;">
                            <asp:TextBox ID="txtHB" runat="server" Width="190px" MaxLength="5" CssClass="alinharDireita"
                                onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />
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
                        <td>Produto
                        </td>
                        <td>Nome
                        </td>
                        <td>Cor
                        </td>
                    </tr>


                    <tr>
                        <td>
                            <asp:TextBox ID="txtProduto" runat="server" Width="170px" MaxLength="5" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNome" runat="server" Width="190px" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCor" runat="server" Width="250px" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="3">&nbsp;
                        </td>
                    </tr>

                    <tr>
                        <td colspan="3">
                            <asp:Image ID="imgFoto" runat="server" BorderColor="Black" Visible="false"
                                BorderWidth="0" />
                        </td>
                    </tr>

                    <tr>
                        <td colspan="3">
                            <fieldset>
                                <legend>
                                    <asp:Label ID="labGradeTitulo" runat="server" Text="Grade"></asp:Label></legend>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="text-align: left">&nbsp;
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:Label ID="labGradeEXP" runat="server" Text="-"></asp:Label>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:Label ID="labGradeXP" runat="server" Text="-"></asp:Label>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:Label ID="labGradePP" runat="server" Text="-"></asp:Label>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:Label ID="labGradeP" runat="server" Text="-"></asp:Label>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:Label ID="labGradeM" runat="server" Text="-"></asp:Label>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:Label ID="labGradeG" runat="server" Text="-"></asp:Label>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:Label ID="labGradeGG" runat="server" Text="-"></asp:Label>
                                        </td>
                                        <td style="text-align: center;">
                                            <asp:Label ID="labGradeTotal" runat="server" Text="Total"></asp:Label>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80px;" valign="middle">Original
                                        </td>
                                        <td style="width: 80px;">
                                            <asp:TextBox ID="txtGradeEXP_O" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 80px;">
                                            <asp:TextBox ID="txtGradeXP_O" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 80px;">
                                            <asp:TextBox ID="txtGradePP_O" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 80px;">
                                            <asp:TextBox ID="txtGradeP_O" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 80px;">
                                            <asp:TextBox ID="txtGradeM_O" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 80px;">
                                            <asp:TextBox ID="txtGradeG_O" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 80px;">
                                            <asp:TextBox ID="txtGradeGG_O" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                onkeypress="return fnValidarNumero(event);" Width="70px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 117px;">
                                            <asp:TextBox ID="txtGradeTotal_O" runat="server" MaxLength="6" CssClass="alinharCentro"
                                                onkeypress="return fnValidarNumero(event);" Width="107px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right;">&nbsp;
                                        </td>
                                    </tr>
                                    <tr style="line-height: 9px;">
                                        <td colspan="10">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80px;" valign="middle">Retrabalho
                                        </td>
                                        <td style="width: 80px;">
                                            <asp:TextBox ID="txtGradeEXP_E" runat="server" MaxLength="6" CssClass="alinharCentro" Text="0"
                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="70px"></asp:TextBox>
                                        </td>
                                        <td style="width: 80px;">
                                            <asp:TextBox ID="txtGradeXP_E" runat="server" MaxLength="6" CssClass="alinharCentro" Text="0"
                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="70px"></asp:TextBox>
                                        </td>
                                        <td style="width: 80px;">
                                            <asp:TextBox ID="txtGradePP_E" runat="server" MaxLength="6" CssClass="alinharCentro" Text="0"
                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="70px"></asp:TextBox>
                                        </td>
                                        <td style="width: 80px;">
                                            <asp:TextBox ID="txtGradeP_E" runat="server" MaxLength="6" CssClass="alinharCentro" Text="0"
                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="70px"></asp:TextBox>
                                        </td>
                                        <td style="width: 80px;">
                                            <asp:TextBox ID="txtGradeM_E" runat="server" MaxLength="6" CssClass="alinharCentro" Text="0"
                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="70px"></asp:TextBox>
                                        </td>
                                        <td style="width: 80px;">
                                            <asp:TextBox ID="txtGradeG_E" runat="server" MaxLength="6" CssClass="alinharCentro" Text="0"
                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="70px"></asp:TextBox>
                                        </td>
                                        <td style="width: 80px;">
                                            <asp:TextBox ID="txtGradeGG_E" runat="server" MaxLength="6" CssClass="alinharCentro" Text="0"
                                                onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" onkeyup="SomarGrade(this);" Width="70px"></asp:TextBox>
                                        </td>
                                        <td style="width: 117px;">
                                            <asp:TextBox ID="txtGradeTotal_E" runat="server" MaxLength="6" CssClass="alinharCentro" Text="0"
                                                onkeypress="return fnValidarNumero(event);" Width="107px"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right;">&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align: left;">
                            <asp:Button ID="btGerar" runat="server" Width="122px" Text="Gerar" OnClientClick="DesabilitarBotao(this);" OnClick="btGerar_Click" />&nbsp;&nbsp;
                            <asp:Label ID="labMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
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
