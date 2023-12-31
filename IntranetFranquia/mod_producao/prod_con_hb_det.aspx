﻿<%@ Page Title="HB Consulta Detalhe" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="prod_con_hb_det.aspx.cs" Inherits="Relatorios.prod_con_hb_det"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .LinkButton {
            text-decoration: none;
        }

        .divPop {
            font-family: "Trebuchet MS", "Helvetica", "Arial", "Verdana", "sans-serif";
            font-size: 100%;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/js.js"></script>
    <script type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios
                    Ordem de Corte&nbsp;&nbsp;>&nbsp;&nbsp;HB Consulta Detalhe</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink"
                        title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login" style="float: left; width: 1000px; margin-left: 16%;">
                <fieldset style="padding-top: 0;">
                    <legend><strong>
                        <asp:Label ID="labTituloDetalhe" runat="server"></asp:Label></strong></legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="text-align: left; width: 160px;">
                                            <span style="font-weight: bold;">Data Inicial</span>
                                        </td>
                                        <td style="text-align: left;">
                                            <span style="font-weight: bold;">Data Final</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;">
                                            <asp:Label ID="labDataInicial" runat="server" Text="" ForeColor="Red"></asp:Label>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="labDataFinal" runat="server" Text="" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 160px;">
                                            <asp:Label ID="labColecao" runat="server" Text="Coleção"></asp:Label>
                                            <asp:HiddenField ID="hidDetalheHB" runat="server" />
                                        </td>
                                        <td style="width: 160px;">
                                            <asp:Label ID="labHB" runat="server" Text="HB"></asp:Label>
                                        </td>
                                        <td style="width: 260px;">
                                            <asp:Label ID="labGrupo" runat="server" Text="Grupo"></asp:Label>
                                        </td>
                                        <td style="width: 260px;">
                                            <asp:Label ID="labNome" runat="server" Text="Nome"></asp:Label>
                                        </td>
                                        <td>Data
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 160px;">
                                            <asp:DropDownList ID="ddlColecoes" runat="server" Width="154px" Height="21px" DataTextField="DESC_COLECAO"
                                                DataValueField="COLECAO">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 160px;">
                                            <asp:TextBox ID="txtHB" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                                Width="150px"></asp:TextBox>
                                        </td>
                                        <td style="width: 260px;">
                                            <asp:DropDownList ID="ddlGrupo" runat="server" Width="254px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                                DataValueField="GRUPO_PRODUTO">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 260px;">
                                            <asp:TextBox ID="txtNome" runat="server" Width="250px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtData" runat="server" CssClass="alinharDireita" ReadOnly="true"
                                                Width="126px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="labTecido" runat="server" Text="Tecido"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labCor" runat="server" Text="Cor"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labFornecedor" runat="server" Text="Fornecedor"></asp:Label>
                                        </td>
                                        <td>Cor Fornecedor
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 160px;">
                                            <asp:TextBox ID="txtTecido" runat="server" Width="150px"></asp:TextBox>
                                        </td>
                                        <td style="width: 160px;">
                                            <asp:DropDownList ID="ddlCor" runat="server" Width="154px" Height="21px" DataTextField="DESC_COR"
                                                DataValueField="COR">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 370px;">
                                            <asp:TextBox ID="txtFornecedor" runat="server" Width="360px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCorFornecedor" runat="server" Width="276px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>No. Pedido
                                        </td>
                                        <td>
                                            <asp:Label ID="labLargura" runat="server" Text="Largura"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labCustoTecido" runat="server" Text="Custo Tecido"></asp:Label>
                                        </td>
                                        <td>
                                            Molde
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtNumeroPedido" runat="server" CssClass="alinharDireita" Width="150px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLargura" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                                Width="150px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCustoTecido" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                                Width="360px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMolde" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                                Width="276px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table border="0" width="100%">
                        <tr>
                            <td valign="top" style="width: 660px;">
                                <fieldset style="margin-top: 0px; padding-top: 0px; width: 650px">
                                    <legend>
                                        <asp:Label ID="labComposicaoTecido" runat="server" Text="Composição Tecido"></asp:Label></legend>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvComposicao" runat="server" Width="649px" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvComposicao_RowDataBound">
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litComposicaoColuna" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="QTDE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Quantidade" HeaderStyle-Width="140px" />
                                                            <asp:BoundField DataField="DESCRICAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Descrição" HeaderStyle-Width="250px" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td>&nbsp;
                            </td>
                            <td valign="top">
                                <fieldset style="margin-top: -1px; padding-top: 0;">
                                    <legend>
                                        <asp:Label ID="labFotoTecido" runat="server" Text="Foto Tecido"></asp:Label></legend>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="center" style="text-align: center;">
                                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="" align="center">
                                                            <asp:Image ID="imgFotoTecido" runat="server" Width='150px' Height='200px' BorderColor="Black"
                                                                BorderWidth="0" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                    <fieldset style="margin-top: -10px; padding-top: 0px;">
                        <legend>
                            <asp:Label ID="labGrade" runat="server" Text="Grade"></asp:Label></legend>
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width: 147px;">&nbsp;
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="labGradeEXP" runat="server" Text="EXP"></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="labGradeXP" runat="server" Text="XP"></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="labGradePP" runat="server" Text="PP"></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="labGradeP" runat="server" Text="P"></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="labGradeM" runat="server" Text="M"></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="labGradeG" runat="server" Text="G"></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: center;">
                                    <asp:Label ID="labGradeGG" runat="server" Text="GG"></asp:Label>
                                </td>
                                <td style="text-align: center;">Total
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 147px;">Prevista
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeEXP" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeXP" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradePP" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeP" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeM" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeG" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeGG" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        Width="70px"></asp:TextBox>
                                </td>
                                <td style="text-align: center;">
                                    <asp:TextBox ID="txtGradeTotal" runat="server" ReadOnly="true" MaxLength="6" CssClass="alinharDireita"
                                        Width="70px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="line-height: 5px">
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 147px;">
                                    <asp:Label ID="labGradeReal" runat="server" Text="Real"></asp:Label>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeEXP_R" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeXP_R" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradePP_R" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeP_R" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeM_R" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeG_R" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        Width="70px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:TextBox ID="txtGradeGG_R" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        Width="70px"></asp:TextBox>
                                </td>
                                <td style="text-align: center;">
                                    <asp:TextBox ID="txtGradeTotal_R" runat="server" MaxLength="6" CssClass="alinharDireita"
                                        Width="70px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset>
                        <legend>
                            <asp:Label ID="labGradeCorte" runat="server" Text="Grade - Corte"></asp:Label></legend>
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td valign="top">
                                    <table border="0" width="100%" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvGradeCorteHB" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvGradeCorteHB_RowDataBound">
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="GRADE" HeaderStyle-Width="160px" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left" HeaderText="Grade" />
                                                            <asp:BoundField DataField="GASTO" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center" HeaderText="Gasto" />
                                                            <asp:BoundField DataField="FOLHA" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center" HeaderText="Folhas" />
                                                            <asp:BoundField DataField="ENFESTO" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left" HeaderText="Enfesto" />
                                                            <asp:BoundField DataField="CORTE" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left" HeaderText="Corte" />
                                                            <asp:BoundField DataField="AMARRADOR" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left" HeaderText="Amarrador" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset style="margin-top: 0px; padding-top: 0px;">
                        <legend>
                            <asp:Label ID="labGasto" runat="server" Text="Gasto"></asp:Label></legend>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="width: 206px;">
                                    <asp:Label ID="labGastoUnidade" runat="server" Text="Unidade de Medida"></asp:Label>
                                </td>
                                <td style="width: 110px;">
                                    <asp:Label ID="labGastoPorFolha" runat="server" Text="Por Folha"></asp:Label>
                                </td>
                                <td style="width: 110px;">
                                    <asp:Label ID="labGastoRetalhos" runat="server" Text="Retalhos"></asp:Label>
                                </td>
                                <td style="width: 110px;">
                                    <asp:Label ID="labGastoPorCorte" runat="server" Text="Por Corte"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labGastoPorPeca" runat="server" Text="Por Peça"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labGastoPorPecaCusto" runat="server" Text="Por Peça (Custo)"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlUnidade" runat="server" Height="21px" Width="200px" DataTextField="DESCRICAO"
                                        DataValueField="CODIGO" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGastoPorFolha" runat="server" CssClass="alinharDireita" onblur="CalcularCorte(this);"
                                        MaxLength="9" Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGastoRetalhos" runat="server" CssClass="alinharDireita" onblur="CalcularCorte(this);"
                                        MaxLength="9" Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGastoPorCorte" runat="server" CssClass="alinharDireita" MaxLength="9"
                                        Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGastoPorPeca" runat="server" CssClass="alinharDireita" MaxLength="9"
                                        Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGastoPorPecaCusto" runat="server" CssClass="alinharDireita" MaxLength="9"
                                        Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <label>
                        Observação</label>
                    <asp:TextBox ID="txtObservacao" TextMode="MultiLine" ReadOnly="true" runat="server"
                        Width="967px" Height="50px"></asp:TextBox>
                    <hr />
                    <div style="float: right;">
                        <asp:Label ID="labErroEnvio" runat="server" ForeColor="red" Text=""></asp:Label>
                        <asp:Button ID="btImprimir" runat="server" Text="Imprimir" OnClick="btImprimir_Click"
                            Width="100px" />
                    </div>
                </fieldset>
            </div>
            <div id="dialogPai" runat="server">
                <div id="dialog" title="Mensagem" class="divPop">
                    <table border="0" width="100%">
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; color: red;">
                                <strong>HB
                                    <asp:Label ID="labHBPopUp" runat="server" Text=""></asp:Label></strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; color: red;">
                                <asp:Label ID="labConteudoDiv" runat="server" Text="BAIXADO COM SUCESSO"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
