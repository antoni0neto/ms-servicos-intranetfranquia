<%@ Page Title="Seguro" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="seg_seguro_cad.aspx.cs" Inherits="Relatorios.seg_seguro_cad"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Seguros&nbsp;&nbsp;>&nbsp;&nbsp;Seguros&nbsp;&nbsp;>&nbsp;&nbsp;Seguro</span>
                <div style="float: right; padding: 0;">
                    <a href="seg_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 962px; margin-left: 16%;">
                <fieldset>
                    <legend>
                        <asp:Label ID="labTitulo" runat="server" Text="Seguro"></asp:Label></legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="labProposta" runat="server" Text="Proposta"></asp:Label>
                                <asp:HiddenField ID="hidCodigoSeguro" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="labApolice" runat="server" Text="Apólice"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labEmpresa" runat="server" Text="Empresa"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labTipoSeguro" runat="server" Text="Tipo de Seguro"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 220px;">
                                <asp:TextBox ID="txtProposta" runat="server" MaxLength="50" Width="210px"></asp:TextBox>
                            </td>
                            <td style="width: 220px;">
                                <asp:TextBox ID="txtApolice" runat="server" Width="210px" MaxLength="50"></asp:TextBox>
                            </td>
                            <td style="width: 250px;">
                                <asp:DropDownList runat="server" ID="ddlEmpresa" DataValueField="CODIGO_EMPRESA" DataTextField="NOME"
                                    Height="22px" Width="244px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlTipoSeguro" DataValueField="CODIGO" DataTextField="DESCRICAO"
                                    Height="22px" Width="244px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="labSeguradora" runat="server" Text="Seguradora"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labCorretor" runat="server" Text="Corretor"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labDataVencimento" runat="server" Text="Data de Vencimento"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labValorPremio" runat="server" Text="Valor Prêmio"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlSeguradora" DataValueField="CODIGO" DataTextField="DESCRICAO"
                                    Height="22px" Width="214px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlCorretor" DataValueField="CODIGO" DataTextField="DESCRICAO"
                                    Height="22px" Width="214px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDataVencimento" runat="server" MaxLength="10" Width="240px" onkeypress="return fnValidarData(event);"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtValorPremio" runat="server" MaxLength="50" Width="240px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">Observação
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:TextBox ID="txtObservacao" runat="server" Width="928px" TextMode="MultiLine" Height="30px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <fieldset>
                                    <legend>
                                        <asp:Label ID="labItens" runat="server" Text="Itens"></asp:Label></legend>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td style="width: 340px;">
                                                <table border="0" cellpadding="0" cellspacing="0" width="320">
                                                    <tr>
                                                        <td colspan="3">
                                                        Item
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:TextBox ID="txtItem" runat="server" Width="320px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Tipo de Cobertura
                                                        </td>
                                                        <td>Valor</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 210px;">
                                                            <asp:DropDownList runat="server" ID="ddlTipoCobertura" DataValueField="CODIGO" DataTextField="DESCRICAO"
                                                                Height="22px" Width="204px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:TextBox ID="txtValorCobertura" runat="server" MaxLength="50" Width="70px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btIncluirTipoCobertura" runat="server" Text=">>" Width="34px" OnClick="btIncluirTipoCobertura_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <div class="rounded_corners">
                                                                <asp:GridView ID="gvTipoCobertura" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                                    Style="background: white" OnRowDataBound="gvTipoCobertura_RowDataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                                                    <HeaderStyle BackColor="Gainsboro" />
                                                                    <FooterStyle BackColor="Gainsboro" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Tipo de Cobertura">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="litTipoCobertura" runat="server"></asp:Literal>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Valor">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="litValorCobertura" runat="server"></asp:Literal>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                            ItemStyle-Width="15px">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="btExcluirTipoCobertura" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/delete.png"
                                                                                    OnClick="btExcluirTipoCobertura_Click" ToolTip="Excluir Tipo Cobertura" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" style="text-align: right;">
                                                            <asp:Button ID="btIncluirItemCobertura" runat="server" Text="Incluir" Width="100px" OnClick="btIncluirItemCobertura_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td valign="top" style="padding-top: 10px;">
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvItemCobertura" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                        Style="background: white" OnRowDataBound="gvItemCobertura_RowDataBound" ShowFooter="true" DataKeyNames="SEG_SEGURO_ITEM">
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                                        <FooterStyle BackColor="Gainsboro" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Item" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litItem" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Tipo de Cobertura">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litTipoCobertura" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="15px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btExcluirItem" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/delete.png"
                                                                        OnClick="btExcluirItem_Click" ToolTip="Excluir Item" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btExcluir" runat="server" Width="110px" Text="Excluir" Visible="false"
                                    OnClick="btExcluir_Click" OnClientClick="return ConfirmarExclusao();" />
                            </td>
                            <td colspan="2" style="text-align: right;">
                                <asp:Button ID="btSalvar" runat="server" Width="110px" Text="Salvar"
                                    OnClick="btSalvar_Click" />
                            </td>
                        </tr>
                    </table>
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
                                <strong>Seguro&nbsp;&nbsp;<asp:Label ID="labPopUp" runat="server" Text=""></asp:Label></strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; color: red;">
                                <asp:Label ID="labMensagem" runat="server" Text="CADASTRADO COM SUCESSO"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>Por favor, selecione a tela desejada nos botões abaixo.
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
