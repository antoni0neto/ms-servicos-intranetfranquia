<%@ Page Title="Alterar Fase de Encaixe" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="facc_fase_alterar.aspx.cs" Inherits="Relatorios.facc_fase_alterar"
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Facção&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Facção&nbsp;&nbsp;>&nbsp;&nbsp;Alterar Fase de Encaixe</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Alterar Fase de Encaixe"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Coleção
                        </td>
                        <td>HB
                        </td>
                        <td style="text-align: center;">Mostruário
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 200px;">
                            <asp:DropDownList ID="ddlColecao" runat="server" Width="194px" Height="21px"
                                DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 200px;">
                            <asp:TextBox ID="txtHB" runat="server" Width="190px" MaxLength="5" CssClass="alinharDireita"
                                onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                        </td>
                        <td style="width: 90px; text-align: center;">
                            <asp:CheckBox ID="cbMostruario" runat="server" Checked="false" Text="" />
                        </td>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:HiddenField ID="hidCodigoHB" runat="server" Value="" />
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>Produto
                        </td>
                        <td>Nome
                        </td>
                        <td  colspan="2">Cor
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtProduto" runat="server" Width="190px" MaxLength="5" CssClass="alinharDireita" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNome" runat="server" Width="190px" Enabled="false"></asp:TextBox>
                        </td>
                        <td  colspan="2">
                            <asp:TextBox ID="txtCor" runat="server" Width="216px" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="4">&nbsp;
                        </td>
                    </tr>

                    <tr>
                        <td colspan="4">
                            <asp:Image ID="imgFoto" runat="server" Width="150px" BorderColor="Black" Visible="false"
                                BorderWidth="0" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                        </td>
                    </tr>

                    <tr>
                        <td>Fase Atual
                        </td>
                        <td>Para Fase
                        </td>
                        <td>&nbsp;</td>
                    </tr>

                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlFaseAtual" runat="server" Width="194px" Height="21px" DataTextField="DESCRICAO"
                                DataValueField="CODIGO">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlParaFase" runat="server" Width="194px" Height="21px" DataTextField="DESCRICAO"
                                DataValueField="CODIGO">
                            </asp:DropDownList>
                            <asp:HiddenField ID="hidCodigoSaida" runat="server" Value="" />
                        </td>
                        <td>
                            <asp:Button ID="btAlterarFase" runat="server" Width="120px" Text="Alterar Fase" OnClick="btAlterarFase_Click" />
                        </td>
                    </tr>

                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
