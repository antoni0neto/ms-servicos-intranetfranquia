<%@ Page Title="Ordenação de Produtos no Bloco" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master"
    CodeBehind="ecom_ordem_produtoblocov2.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="Relatorios.ecom_ordem_produtoblocov2" %>

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
                },
                update: function (e, ui) {

                    var codigoBloco = parseInt($("#MainContent_ddlBloco").val());

                    var produtos = new Array();
                    var pos = 0;
                    $('#sortable .getHidden').each(function () {

                        var idMag = $(this).prev().val();
                        produtos[pos] = idMag;
                        pos = pos + 1;
                    });

                    $.ajax({
                        type: 'POST',
                        url: 'ecom_ordem_produtoblocov2.aspx/SalvarOrdem',
                        contentType: "application/json; charset=utf-8",
                        data: '{ produtos: [' + produtos.join() + '], codigoBloco: ' + codigoBloco + ' }',
                        dataType: 'json',
                        success: function (results) {
                        },
                        error: function (a, b, c) {
                            console.log(a);
                            console.log(b);
                            console.log(c);
                        }
                    });

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Ordenação&nbsp;&nbsp;>&nbsp;&nbsp;Ordenação de Produtos no Bloco</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <br />
            <div style="float: left; width: 600px; margin-left: 24%;">
                <fieldset>
                    <legend>Ordenação de Produtos no Bloco</legend>

                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="tb">
                        <tr>
                            <td>Categoria Magento</td>
                            <td>Bloco</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 210px;">
                                <asp:DropDownList ID="ddlCategoriaMag" runat="server" Width="204px" Height="21px" DataTextField="GRUPO"
                                    DataValueField="CODIGO" OnSelectedIndexChanged="ddlCategoriaMag_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 180px;">
                                <asp:DropDownList ID="ddlBloco" runat="server" Width="174px" Height="21px" DataTextField="BLOCO"
                                    DataValueField="CODIGO">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Button ID="btRandom" runat="server" Text="Reordenar" OnClick="btRandom_Click" Width="100px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;&nbsp;<asp:Label ID="labMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;
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
                                            <td colspan="2" style="line-height: 15px;">
                                                <asp:Label ID="labUltimaAtu" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Smaller"></asp:Label>
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
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
