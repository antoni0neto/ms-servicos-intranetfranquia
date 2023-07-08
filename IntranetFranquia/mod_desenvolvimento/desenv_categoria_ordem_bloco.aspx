<%@ Page Title="Ordenação Produtos na Categoria" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master"
    CodeBehind="desenv_categoria_ordem_bloco.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="Relatorios.desenv_categoria_ordem_bloco" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" type="text/css" />
    <style type="text/css">
        /*.ui-draggable, .ui-droppable {
            background-position: top;
        }*/

        @media print {
            .background-force {
                -webkit-print-color-adjust: exact;
            }
        }

        #sortable {
            list-style-type: none;
            padding: 0;
            width: 100%;
        }

            #sortable li {
                margin: 10px 5px 10px 5px;
                padding: 1.2px;
                float: left;
                width: 200px;
                height: 380px;
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

                    var codigoCategoria = parseInt($("#MainContent_ddlCategoria").val());

                    //console.log(codigoCategoria);

                    var produtos = new Array();
                    var pos = 0;
                    $('#sortable .getHidden').each(function () {

                        var codigoProduto = $(this).prev().val();

                        //console.log(codigoProduto);

                        produtos[pos] = codigoProduto;
                        pos = pos + 1;
                    });

                    //console.log(produtos.join());

                    $.ajax({
                        type: 'POST',
                        url: 'desenv_categoria_ordem_bloco.aspx/SalvarOrdem',
                        contentType: "application/json; charset=utf-8",
                        data: '{ produtos: [' + produtos.join() + '], codigoCategoria: ' + codigoCategoria + ' }',
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Análise de Coleção&nbsp;&nbsp;>&nbsp;&nbsp;Ordenação Produtos na Categoria</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <br />
            <div style="width: 90%; margin-left: 5%;">
                <fieldset>
                    <legend>Ordenação Produtos na Categoria</legend>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="tb">
                        <tr>
                            <td>
                                <asp:Label ID="labColecao" runat="server" Text="Coleção"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labGriffe" runat="server" Text="Griffe"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labCategoria" runat="server" Text="Categoria"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 220px;">
                                <asp:DropDownList ID="ddlColecao" runat="server" Width="214px" Height="21px" DataTextField="DESC_COLECAO"
                                    DataValueField="COLECAO" OnSelectedIndexChanged="ddlColecao_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 200px;">
                                <asp:DropDownList ID="ddlGriffe" runat="server" Width="194px" Height="21px" DataTextField="GRIFFE"
                                    DataValueField="GRIFFE" OnSelectedIndexChanged="ddlGriffe_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 180px;">
                                <asp:DropDownList ID="ddlCategoria" runat="server" Width="174px" Height="21px" DataTextField="NOME"
                                    DataValueField="CODIGO">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" />
                            </td>
                            <td style="text-align: right;">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;&nbsp;<asp:Label ID="labMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;
                            </td>
                        </tr>
                    </table>
                    <ul id="sortable" style="width: 100%;">
                        <asp:Repeater ID="repProdutos" runat="server" OnItemDataBound="repProdutos_ItemDataBound">
                            <ItemTemplate>
                                <li class="ui-state-default">
                                    <asp:HiddenField ID="hidCodigoProduto" runat="server" />
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="getHidden">
                                        <tr>
                                            <td colspan="2" style="line-height: 15px;">
                                                <asp:Label ID="labOrigem" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Smaller"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: center; border: none; vertical-align: top; line-height: 15px;">
                                                <asp:Label ID="labProdutoTitulo" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Smaller"></asp:Label><br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="line-height: 20px;">
                                                <asp:Label ID="labTecido" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Smaller"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: center; border: none; height: 265px;">
                                                <asp:Image ID="imgProduto" runat="server" Style="height: 260px; max-width: 195px;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="line-height: 30px; width: 50%; text-align: left;  border-bottom: 1px solid #b1afaf; border-top: 1px solid #b1afaf;">
                                                &nbsp;&nbsp;<asp:Label ID="labCor" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="XX-Small"></asp:Label>
                                            </td>
                                            <td style="line-height: 30px; text-align:right; border-bottom: 1px solid #b1afaf; border-top: 1px solid #b1afaf;">
                                                <asp:Label ID="labCorFornecedor" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="XX-Small"></asp:Label>&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="tdBack" runat="server" class='background-force' style="line-height: 30px; width: 50%; text-align: center;">&nbsp;
                                            </td>
                                            <td style="line-height: 30px; width: 50%; text-align: right;">
                                                <asp:Label ID="labVarejo" runat="server" Text="VAREJO:" ForeColor="Gray" Font-Bold="true" Font-Size="XX-Small"></asp:Label>&nbsp;&nbsp;<asp:Label ID="labQtdeVarejo" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Smaller"></asp:Label>&nbsp;&nbsp;
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
