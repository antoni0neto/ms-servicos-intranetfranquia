<%@ Page Title="Vitrine da Semana" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="zold_admloj_cad_vitrine_foto.aspx.cs" Inherits="Relatorios.zold_admloj_cad_vitrine_foto"
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
        <Triggers>
            <asp:PostBackTrigger ControlID="btIncluirFotoVitrine" />
        </Triggers>
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Administração de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Marketing&nbsp;&nbsp;>&nbsp;&nbsp;<asp:Label ID="labTituloMenu" runat="server" Text="Vitrine da Semana"></asp:Label></span>
                <div style="float: right; padding: 0;">
                    <a href="admloj_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1100px; margin-left: 12%;">
                <fieldset>
                    <legend>
                        <asp:Label ID="labTitulo" runat="server" Text="Vitrine da Semana"></asp:Label></legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labAnoSemana" runat="server" Text="Semana"></asp:Label>
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 250px;">
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="21px" Width="244px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 250px;">
                                <asp:DropDownList runat="server" ID="ddlAnoSemana" DataValueField="ANOSEMANA" DataTextField="DESC_ANOSEMANA"
                                    Height="21px" Width="244px" OnTextChanged="ddlAnoSemana_TextChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <fieldset style="margin-top: 4px; padding-top: 0;">
                                    <legend>
                                        <asp:Label ID="labFotoVitrineTitulo" runat="server" Text="Foto"></asp:Label></legend>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 150px; vertical-align: top;">
                                                <asp:FileUpload ID="uploadFotoVitrine" runat="server" Width="300px" /><br />
                                                <br />
                                                <br />
                                                <br />
                                                <br />
                                                <asp:Button ID="btIncluirFotoVitrine" runat="server" OnClick="btIncluirFotoVitrine_Click"
                                                    Text=">>" Width="32px" />
                                            </td>
                                            <td>
                                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="text-align: right;">
                                                            <asp:Image ID="imgFotoVitrine" runat="server" Width="80px" Height="100px" AlternateText="Nenhum Arquivo..." BorderWidth="1" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="labSetor" runat="server" Text="Setor"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="labVitrine" runat="server" Text="Vitrine"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlSetor" runat="server" Width="244px" DataValueField="CODIGO" DataTextField="SETOR" Enabled="false"
                                    OnSelectedIndexChanged="ddlSetor_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSetorTipo" runat="server" Width="244px" DataValueField="CODIGO" DataTextField="TIPO">
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: right;">
                                <asp:Button ID="btSalvar" runat="server" Text="Salvar" Width="100px" OnClick="btSalvar_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvVitrineFoto" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                        Style="background: white" OnRowDataBound="gvVitrineFoto_RowDataBound" ShowFooter="true">
                                        <HeaderStyle BackColor="GradientActiveCaption" />
                                        <FooterStyle BackColor="GradientActiveCaption" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Vitrine" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDescricao" runat="server" Text='<%# Bind("DESCRICAO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Foto" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Image ID="imgFotoVitrine" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:Button ID="btExcluirFoto" runat="server" Width="70px" Text="Excluir" OnClick="btExcluirFoto_Click" />
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
