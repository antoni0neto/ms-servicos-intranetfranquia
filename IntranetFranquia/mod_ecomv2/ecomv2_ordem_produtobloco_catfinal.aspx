<%@ Page Title="Atualizar Ordenação Magento" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master"
    CodeBehind="ecomv2_ordem_produtobloco_catfinal.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="Relatorios.ecomv2_ordem_produtobloco_catfinal" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" type="text/css" />
    <style type="text/css">
        /*.ui-draggable, .ui-droppable {
            background-position: top;
        }*/

        #sortable {
            list-style-type: none;
            margin: 0;
            padding: 0;
            width: 642px;
        }

            #sortable li {
                margin: 10px 10px 10px 0px;
                padding: 1px;
                float: left;
                width: 200px;
                height: 353px;
                text-align: center;
            }
    </style>

    <script src="https://code.jquery.com/jquery-1.12.4.js" type="text/javascript"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(function () {
            sortBlocoProduto();
        });

        function sortBlocoProduto() {
            $("#sortable").sortable({
                start: function (e, ui) {
                    ui.item.css('border-style', 'dashed');
                },
                stop: function (e, ui) {
                    ui.item.css('border-style', '');
                }
            }).disableSelection();
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Ordenação&nbsp;&nbsp;>&nbsp;&nbsp;Atualizar Ordenação Magento</span>
                <div style="float: right; padding: 0;">
                    <a href="~/mod_ecom/ecom_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <br />
            <div style="float: left; width: 600px; margin-left: 24%;">
                <fieldset>
                    <legend>Atualizar Ordenação Magento</legend>

                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="tb">
                        <tr>
                            <td>Categoria Magento</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 210px;">
                                <asp:DropDownList ID="ddlCategoriaMag" runat="server" Width="204px" Height="21px" DataTextField="GRUPO"
                                    DataValueField="CODIGO">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Button ID="btAtualizarMagento" runat="server" Text="Atualizar Magento" OnClick="btAtualizarMagento_Click" Width="150px" />&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;&nbsp;<asp:Label ID="labMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btRandomBloco" runat="server" Text="Reordenar Bloco" OnClick="btRandomBloco_Click" Width="150px" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Button ID="btRandomTudo" runat="server" Text="Reordenar Tudo" OnClick="btRandomTudo_Click" Width="150px" Enabled="false" />&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;
                            </td>
                        </tr>
                    </table>


                    <ul id="sortable">
                        <asp:Repeater ID="repProdutos" runat="server" OnItemDataBound="repProdutos_ItemDataBound">
                            <ItemTemplate>
                                <li class="ui-state-default">
                                    <asp:HiddenField ID="hidIdMag" runat="server" />
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="getHidden">
                                        <tr>
                                            <td style="text-align: left; border: none; vertical-align: top;">&nbsp;<asp:Label ID="labIdMag" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Smaller"></asp:Label>
                                            </td>
                                            <td style="text-align: right; border: none; vertical-align: top;">
                                                <asp:Label ID="labEstoque" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Smaller"></asp:Label>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="line-height: 4px;">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: center; border: none; vertical-align: top;">
                                                <asp:Label ID="labProdutoTitulo" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label><br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="line-height: 15px; text-align: left;">&nbsp;<asp:Label ID="labBloco" runat="server" Text="" ForeColor="DarkMagenta" Font-Bold="true" Font-Size="Smaller"></asp:Label>
                                            </td>
                                            <td style="line-height: 15px; text-align: right;">
                                                <asp:Label ID="labUltimaAtu" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Smaller"></asp:Label>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: center; border: none;">
                                                <asp:Image ID="imgFrenteCab" runat="server" Width="200px" Height="" />
                                            </td>
                                        </tr>
                                    </table>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </fieldset>
                <br />
                <br />
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
                                <strong>Aviso</strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; color: red;">ORDENAÇÃO REALIZADA COM SUCESSO
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
