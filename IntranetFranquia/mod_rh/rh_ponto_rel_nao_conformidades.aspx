<%@ Page Title="Não Conformidades" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" 
    AutoEventWireup="true" CodeBehind="rh_ponto_rel_nao_conformidades.aspx.cs" Inherits="Relatorios.mod_rh.rh_ponto_rel_nao_conformidades" 
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

        .alternate {
            background-color: lightgray;
        }

        .textSize {
            font-size: x-small;
        }
    </style>
    <script src="http://code.jquery.com/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server" RenderMode="Inline">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do RH&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;<asp:Label
                    ID="labTituloMenu" runat="server" Text="Não Conformidades"></asp:Label></span>
                <div style="float: right; padding: 0;">
                    <a href="rh_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 87%;">
                <fieldset>
                    <legend><asp:Label ID="labTitulo" runat="server" Text="Não Conformidades"></asp:Label></legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labMesReferencia" runat="server" Text="Mês Referência"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 260px;">
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="22px" Width="254px" AutoPostBack="false">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 220px;">
                                <asp:DropDownList runat="server" ID="ddlMesReferencia" DataValueField="MES_COMPETENCIA" DataTextField="MES_COMPETENCIA"
                                    Height="22px" Width="254px" AutoPostBack="false">
                                </asp:DropDownList>
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="110px" OnClick="btBuscar_Click" />
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
                            <td colspan="4">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvNConformidades" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" DataKeyNames="CODIGO">
                                        <HeaderStyle BackColor="GradientActiveCaption" />
                                        <FooterStyle BackColor="GradientActiveCaption" />
                                        <AlternatingRowStyle CssClass="alternate" />
                                        <Columns>
                                            <asp:BoundField DataField="FILIAL" HeaderText="Loja" SortExpression="FILIAL" />
                                            <asp:BoundField DataField="DATA" HeaderText="Data Referência" DataFormatString="{0:dd/MM/yyyy}" SortExpression="DATA" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="NOME" HeaderText="Nome" SortExpression="NOME" />
                                            <asp:BoundField DataField="DESC_CARGO" HeaderText="Cargo" SortExpression="DESC_CARGO" />
                                            <asp:BoundField DataField="DESCRICAO" HeaderText="Período Contratado" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ENTRADA1" HeaderText="Entrada 1" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="SAIDA1" HeaderText="Saída 1" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ENTRADA2" HeaderText="Entrada 2" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="SAIDA2" HeaderText="Saída 2" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="TOTAL_HORAS" HeaderText="Total de Horas" ItemStyle-HorizontalAlign="Center" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;</td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="position: relative; width: 100%; display: inline-block;">
        <asp:Button ID="btPDF" runat="server" Text="Exportar PDF" Width="110px" OnClick="btPDF_Click" />
    </div>
    <span>&nbsp;</span>
</asp:Content>