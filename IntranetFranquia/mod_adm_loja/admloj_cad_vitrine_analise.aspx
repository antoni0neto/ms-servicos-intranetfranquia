<%@ Page Title="Vitrine da Semana - Análise" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="admloj_cad_vitrine_analise.aspx.cs" Inherits="Relatorios.admloj_cad_vitrine_analise"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Administração de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Vitrine&nbsp;&nbsp;>&nbsp;&nbsp;<asp:Label ID="labTituloMenu" runat="server" Text="Vitrine da Semana - Análise"></asp:Label></span>
                <div style="float: right; padding: 0;">
                    <a href="admloj_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1100px; margin-left: 12%;">
                <fieldset>
                    <legend>
                        <asp:Label ID="labTitulo" runat="server" Text="Vitrine da Semana - Análise"></asp:Label></legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="labSemana" runat="server" Text="Semana"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labFavorito" runat="server" Text="Tem Foto Favorita?"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labFechado" runat="server" Text="Fotos Cadastradas?"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labAnalise" runat="server" Text="Fotos Analisadas?"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labSugestao" runat="server" Text="Sugestão"></asp:Label>
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 200px;">
                                <asp:DropDownList runat="server" ID="ddlSemana" DataValueField="CODIGO" DataTextField="SEMANA"
                                    Height="21px" Width="194px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 240px;">
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="21px" Width="234px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 130px;">
                                <asp:DropDownList ID="ddlFavorito" runat="server" Width="124px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 130px;">
                                <asp:DropDownList ID="ddlFechado" runat="server" Width="124px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 130px;">
                                <asp:DropDownList ID="ddlAnalisado" runat="server" Width="124px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 110px;">
                                <asp:DropDownList ID="ddlSugestao" runat="server" Width="104px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Width="100px" Text="Buscar" OnClick="btBuscar_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <div class="rounded_corners">
                                    <asp:Repeater ID="repVitrine" runat="server" OnItemDataBound="repVitrine_ItemDataBound">
                                        <ItemTemplate>

                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td style="width: 30px; text-align: center;">
                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.ItemIndex + 1 %>'></asp:Literal>
                                                    </td>
                                                    <td style="width: 120px; font-size: smaller; text-align: center;">
                                                        <asp:Literal ID="litFilial" runat="server"></asp:Literal><br />
                                                        <asp:ImageButton ID="btFilialAnalise" runat="server" Height="15px" OnClick="btFilialAnalise_Click" ImageUrl="~/Image/disapprove.png" AlternateText=" " ToolTip=" " />
                                                        <asp:ImageButton ID="btFilialAtualizar" runat="server" Height="15px" OnClick="btFilialAtualizar_Click" ImageUrl="~/Image/update.png" AlternateText=" " ToolTip=" " />
                                                    </td>
                                                    <td style="border-width: 1px;">
                                                        <div style="width: 100%; text-align: center; vertical-align: middle; background-color: #DCDCDC; color: black; padding-top: 5px; padding-bottom: 5px;">
                                                            <asp:Label ID="labFemininoFilial" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                        </div>
                                                        <br />
                                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" style="text-align: center;">
                                                            <tr>
                                                                <td colspan="3" style="border-width: 0px; border-top: 1px;">&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" style="border-width: 0px;">
                                                                    <asp:ImageButton ID="btFemininoPrincipalFav" runat="server" Height="30px" ImageUrl="~/Image/favorito_n.png" OnClick="btFavorito_Click" /><br />
                                                                    <asp:ImageButton ID="btFemininoPrincipal" runat="server" Height="300px" OnClick="btZoomFoto_Click" AlternateText=" " ToolTip=" " />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" style="border-width: 0px;">&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" style="border-width: 0px;">
                                                                    <asp:ImageButton ID="btFemininoPrincipalHarmonia" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Harmonia<br />
                                                                    <asp:ImageButton ID="btFemininoPrincipalPosicao" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Posição<br />
                                                                    <asp:ImageButton ID="btFemininoPrincipalObs" runat="server" Height="30px" ImageUrl="~/Image/warning.png" Visible="false" />&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" style="border-width: 0px;">&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" style="border: solid 0px #DCDCDC; border-top: solid 1px #DCDCDC;">&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="border-width: 0px;">
                                                                    <asp:ImageButton ID="btFemininoMundo1Fav" runat="server" Height="30px" ImageUrl="~/Image/favorito_n.png" OnClick="btFavorito_Click" /><br />
                                                                    <asp:ImageButton ID="btFemininoMundo1" runat="server" Width="250px" OnClick="btZoomFoto_Click" ImageAlign="Middle" AlternateText=" " ToolTip=" " />
                                                                </td>
                                                                <td style="border-width: 0px;">
                                                                    <asp:ImageButton ID="btFemininoMundo2Fav" runat="server" Height="30px" ImageUrl="~/Image/favorito_n.png" OnClick="btFavorito_Click" /><br />
                                                                    <asp:ImageButton ID="btFemininoMundo2" runat="server" Width="250px" OnClick="btZoomFoto_Click" ImageAlign="Middle" AlternateText=" " ToolTip=" " />
                                                                </td>
                                                                <td style="border-width: 0px;">
                                                                    <asp:ImageButton ID="btFemininoMundo3Fav" runat="server" Height="30px" ImageUrl="~/Image/favorito_n.png" OnClick="btFavorito_Click" /><br />
                                                                    <asp:ImageButton ID="btFemininoMundo3" runat="server" Width="250px" OnClick="btZoomFoto_Click" ImageAlign="Middle" AlternateText=" " ToolTip=" " />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" style="border-width: 0px;">&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="border-width: 0px;">
                                                                    <asp:ImageButton ID="btFemininoMundo1Harmonia" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Harmonia<br />
                                                                    <asp:ImageButton ID="btFemininoMundo1Posicao" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Posição<br />
                                                                    <asp:ImageButton ID="btFemininoMundo1Montagem" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Montagem<br />
                                                                    <asp:ImageButton ID="btFemininoMundo1Cubos" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Cubos<br />
                                                                    <asp:ImageButton ID="btFemininoMundo1Acessorios" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Acessórios<br />
                                                                    <asp:ImageButton ID="btFemininoMundo1Obs" runat="server" Height="30px" ImageUrl="~/Image/warning.png" Visible="false" />&nbsp;
                                                                </td>
                                                                <td style="border-width: 0px;">
                                                                    <asp:ImageButton ID="btFemininoMundo2Harmonia" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Harmonia<br />
                                                                    <asp:ImageButton ID="btFemininoMundo2Posicao" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Posição<br />
                                                                    <asp:ImageButton ID="btFemininoMundo2Montagem" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Montagem<br />
                                                                    <asp:ImageButton ID="btFemininoMundo2Cubos" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Cubos<br />
                                                                    <asp:ImageButton ID="btFemininoMundo2Acessorios" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Acessórios<br />
                                                                    <asp:ImageButton ID="btFemininoMundo2Obs" runat="server" Height="30px" ImageUrl="~/Image/warning.png" Visible="false" />&nbsp;
                                                                </td>
                                                                <td style="border-width: 0px;">
                                                                    <asp:ImageButton ID="btFemininoMundo3Harmonia" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Harmonia<br />
                                                                    <asp:ImageButton ID="btFemininoMundo3Posicao" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Posição<br />
                                                                    <asp:ImageButton ID="btFemininoMundo3Montagem" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Montagem<br />
                                                                    <asp:ImageButton ID="btFemininoMundo3Cubos" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Cubos<br />
                                                                    <asp:ImageButton ID="btFemininoMundo3Acessorios" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Acessórios<br />
                                                                    <asp:ImageButton ID="btFemininoMundo3Obs" runat="server" Height="30px" ImageUrl="~/Image/warning.png" Visible="false" />&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" style="border-width: 0px;">&nbsp;</td>
                                                            </tr>
                                                        </table>

                                                        <div style="width: 100%; text-align: center; vertical-align: middle; background-color: #DCDCDC; color: black; padding-top: 5px; padding-bottom: 5px;">
                                                            <asp:Label ID="labMasculinoFilial" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                        </div>
                                                        <br />
                                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" style="text-align: center;">
                                                            <tr>
                                                                <td colspan="2" style="border-width: 0px; border-top: 1px;">&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="border-width: 0px;">
                                                                    <asp:ImageButton ID="btMasculinoPrincipalFav" runat="server" Height="30px" ImageUrl="~/Image/favorito_n.png" OnClick="btFavorito_Click" /><br />
                                                                    <asp:ImageButton ID="btMasculinoPrincipal" runat="server" Height="300px" OnClick="btZoomFoto_Click" AlternateText=" " ToolTip=" " />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="border-width: 0px;">&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="border-width: 0px;">
                                                                    <asp:ImageButton ID="btMasculinoPrincipalHarmonia" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Harmonia<br />
                                                                    <asp:ImageButton ID="btMasculinoPrincipalPosicao" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Posição<br />
                                                                    <asp:ImageButton ID="btMasculinoPrincipalObs" runat="server" Height="30px" ImageUrl="~/Image/warning.png" Visible="false" />&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="border-width: 0px;">&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="border: solid 0px #DCDCDC; border-top: solid 1px #DCDCDC;">&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="border-width: 0px;">
                                                                    <asp:ImageButton ID="btMasculinoMundo1Fav" runat="server" Height="30px" ImageUrl="~/Image/favorito_n.png" OnClick="btFavorito_Click" /><br />
                                                                    <asp:ImageButton ID="btMasculinoMundo1" runat="server" Width="250px" OnClick="btZoomFoto_Click" ImageAlign="Middle" AlternateText=" " ToolTip=" " />
                                                                </td>
                                                                <td style="border-width: 0px;">
                                                                    <asp:ImageButton ID="btMasculinoMundo2Fav" runat="server" Height="30px" ImageUrl="~/Image/favorito_n.png" OnClick="btFavorito_Click" /><br />
                                                                    <asp:ImageButton ID="btMasculinoMundo2" runat="server" Width="250px" OnClick="btZoomFoto_Click" ImageAlign="Middle" AlternateText=" " ToolTip=" " />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="border-width: 0px;">&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="border-width: 0px;">
                                                                    <asp:ImageButton ID="btMasculinoMundo1Harmonia" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Harmonia<br />
                                                                    <asp:ImageButton ID="btMasculinoMundo1Posicao" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Posição<br />
                                                                    <asp:ImageButton ID="btMasculinoMundo1Montagem" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Montagem<br />
                                                                    <asp:ImageButton ID="btMasculinoMundo1Cubos" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Cubos<br />
                                                                    <asp:ImageButton ID="btMasculinoMundo1Acessorios" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Acessórios<br />
                                                                    <asp:ImageButton ID="btMasculinoMundo1Obs" runat="server" Height="30px" ImageUrl="~/Image/warning.png" Visible="false" />&nbsp;
                                                                </td>
                                                                <td style="border-width: 0px;">
                                                                    <asp:ImageButton ID="btMasculinoMundo2Harmonia" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Harmonia<br />
                                                                    <asp:ImageButton ID="btMasculinoMundo2Posicao" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Posição<br />
                                                                    <asp:ImageButton ID="btMasculinoMundo2Montagem" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Montagem<br />
                                                                    <asp:ImageButton ID="btMasculinoMundo2Cubos" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Cubos<br />
                                                                    <asp:ImageButton ID="btMasculinoMundo2Acessorios" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Acessórios<br />
                                                                    <asp:ImageButton ID="btMasculinoMundo2Obs" runat="server" Height="30px" ImageUrl="~/Image/warning.png" Visible="false" />&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="border-width: 0px;">&nbsp;</td>
                                                            </tr>
                                                        </table>


                                                        <div style="width: 100%; text-align: center; vertical-align: middle; background-color: #DCDCDC; color: black; padding-top: 5px; padding-bottom: 5px;">
                                                            <asp:Label ID="labPortaFilial" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                        </div>
                                                        <br />
                                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" style="text-align: center;">
                                                            <tr>
                                                                <td colspan="2" style="border-width: 0px; border-top: 1px;">&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="border-width: 0px;">
                                                                    <asp:ImageButton ID="btPortaPrincipalFav" runat="server" Height="30px" ImageUrl="~/Image/favorito_n.png" OnClick="btFavorito_Click" /><br />
                                                                    <asp:ImageButton ID="btPortaPrincipal" runat="server" Height="300px" OnClick="btZoomFoto_Click" AlternateText=" " ToolTip=" " />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="border-width: 0px;">&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="border-width: 0px;">
                                                                    <asp:ImageButton ID="btPortaPrincipalHarmonia" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Harmonia<br />
                                                                    <asp:ImageButton ID="btPortaPrincipalPosicao" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Posição<br />
                                                                    <asp:ImageButton ID="btPortaPrincipalObs" runat="server" Height="30px" ImageUrl="~/Image/warning.png" Visible="false" />&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="border-width: 0px;">&nbsp;</td>
                                                            </tr>
                                                        </table>


                                                        <div style="width: 100%; text-align: center; vertical-align: middle; background-color: #DCDCDC; color: black; padding-top: 5px; padding-bottom: 5px;">
                                                            <asp:Label ID="labExtraFilial" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                        </div>
                                                        <br />
                                                        <table border="0" width="100%" cellpadding="0" cellspacing="0" style="text-align: center;">
                                                            <tr>
                                                                <td colspan="2" style="border-width: 0px; border-top: 1px;">&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="border-width: 0px;">
                                                                    <asp:ImageButton ID="btExtraPrincipalFav" runat="server" Height="30px" ImageUrl="~/Image/favorito_n.png" OnClick="btFavorito_Click" /><br />
                                                                    <asp:ImageButton ID="btExtraPrincipal" runat="server" Height="300px" OnClick="btZoomFoto_Click" AlternateText=" " ToolTip=" " />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="border-width: 0px;">&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="border-width: 0px;">
                                                                    <asp:ImageButton ID="btExtraPrincipalHarmonia" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Harmonia<br />
                                                                    <asp:ImageButton ID="btExtraPrincipalPosicao" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Posição<br />
                                                                    <asp:ImageButton ID="btExtraPrincipalObs" runat="server" Height="30px" ImageUrl="~/Image/warning.png" Visible="false" />&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="border-width: 0px;">&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="border: solid 0px #DCDCDC; border-top: solid 1px #DCDCDC;">&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="border-width: 0px;">
                                                                    <asp:ImageButton ID="btExtraMundo1Fav" runat="server" Height="30px" ImageUrl="~/Image/favorito_n.png" OnClick="btFavorito_Click" /><br />
                                                                    <asp:ImageButton ID="btExtraMundo1" runat="server" Width="250px" OnClick="btZoomFoto_Click" ImageAlign="Middle" AlternateText=" " ToolTip=" " />
                                                                </td>
                                                                <td style="border-width: 0px;">
                                                                    <asp:ImageButton ID="btExtraMundo2Fav" runat="server" Height="30px" ImageUrl="~/Image/favorito_n.png" OnClick="btFavorito_Click" /><br />
                                                                    <asp:ImageButton ID="btExtraMundo2" runat="server" Width="250px" OnClick="btZoomFoto_Click" ImageAlign="Middle" AlternateText=" " ToolTip=" " />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="border-width: 0px;">&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="border-width: 0px;">
                                                                    <asp:ImageButton ID="btExtraMundo1Harmonia" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Harmonia<br />
                                                                    <asp:ImageButton ID="btExtraMundo1Posicao" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Posição<br />
                                                                    <asp:ImageButton ID="btExtraMundo1Montagem" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Montagem<br />
                                                                    <asp:ImageButton ID="btExtraMundo1Cubos" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Cubos<br />
                                                                    <asp:ImageButton ID="btExtraMundo1Acessorios" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Acessórios<br />
                                                                    <asp:ImageButton ID="btExtraMundo1Obs" runat="server" Height="30px" ImageUrl="~/Image/warning.png" Visible="false" />&nbsp;
                                                                </td>
                                                                <td style="border-width: 0px;">
                                                                    <asp:ImageButton ID="btExtraMundo2Harmonia" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Harmonia<br />
                                                                    <asp:ImageButton ID="btExtraMundo2Posicao" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Posição<br />
                                                                    <asp:ImageButton ID="btExtraMundo2Montagem" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Montagem<br />
                                                                    <asp:ImageButton ID="btExtraMundo2Cubos" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Cubos<br />
                                                                    <asp:ImageButton ID="btExtraMundo2Acessorios" runat="server" Height="12px" ImageUrl="~/Image/disapprove.png" />&nbsp;Acessórios<br />
                                                                    <asp:ImageButton ID="btExtraMundo2Obs" runat="server" Height="30px" ImageUrl="~/Image/warning.png" Visible="false" />&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="border-width: 0px;">&nbsp;</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
