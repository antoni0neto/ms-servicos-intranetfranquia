<%@ Page Title="Ordenação de Produtos no Bloco" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master"
    CodeBehind="ecom_ordem_produtobloco.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="Relatorios.ecom_ordem_produtobloco" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .connectedSortable {
        }
    </style>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.24/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.24/jquery-ui.min.js"></script>
    <script src="../js/js.js" type="text/javascript"></script>

    <script type="text/javascript">

        function isInt(value) {
            if (isNaN(value)) {
                return false;
            }
            var x = parseFloat(value);
            return (x | 0) === x;
        }

        $(function () {
            $("[id*=gvProduto1], [id*=gvProduto2], [id*=gvProduto3]").sortable({
                items: 'tr:not(tr:first-child)',
                cancel: "img",
                connectWith: ".connectedSortable",
                start: function (e, ui) {
                    ui.item.css('border-style', 'dashed');
                },
                stop: function (e, ui) {
                    ui.item.css('border-style', '');
                },
                update: function (e, ui) {

                    if (!ui.sender) {

                        var rows1 = $.map($('[id*=gvProduto1] tr td:first-child'), function (el) {
                            var v = $.trim($(el).text());
                            if (v != "") {
                                if (isInt(v))
                                    return v;
                            }
                        }).toString();

                        var rows2 = $.map($('[id*=gvProduto2] tr td:first-child'), function (el) {
                            var v = $.trim($(el).text());
                            if (v != "") {
                                if (isInt(v))
                                    return v;
                            }
                        }).toString();

                        var rows3 = $.map($('[id*=gvProduto3] tr td:first-child'), function (el) {
                            var v = $.trim($(el).text());
                            if (v != "") {
                                if (isInt(v))
                                    return v;
                            }
                        }).toString();

                        var data1 = new Array();
                        data1 = rows1.split(',');
                        var data2 = new Array();
                        data2 = rows2.split(',');
                        var data3 = new Array();
                        data3 = rows3.split(',');

                        var produtcs = new Array();

                        var arrayLen1 = data1.length;
                        var arrayLen2 = data2.length;
                        var arrayLen3 = data3.length;
                        var arrayLen1Aux = arrayLen1 + arrayLen2 + arrayLen3;
                        var arrayLen2Aux = arrayLen2 + arrayLen3;

                        var pos = 0;

                        for (var i = 0; i < arrayLen1Aux; i++) {

                            if (i < arrayLen1) {
                                produtcs[pos] = data1[i];
                                pos = pos + 1;
                            }

                            var break2 = 0;
                            for (var j = 0; j < arrayLen2Aux; j++) {

                                if (break2 == 1) {
                                    break2 = 0;
                                    break;
                                }

                                if (i < arrayLen2) {
                                    produtcs[pos] = data2[i];
                                    pos = pos + 1;
                                }

                                for (var k = 0; k < arrayLen3; k++) {
                                    break2 = 1;
                                    if (i < arrayLen3) {
                                        produtcs[pos] = data3[i];
                                        pos = pos + 1;
                                    }

                                    break;
                                }
                            }
                        }

                        $.ajax({
                            type: 'POST',
                            url: 'ecom_ordem_produtobloco.aspx/SalvarOrdem',
                            contentType: "application/json; charset=utf-8",
                            data: '{ produtos: [' + produtcs.join() + '] }',
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

                }
            }).disableSelection();
        });
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
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="width: 33%; vertical-align: top;">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvProduto1" runat="server" AutoGenerateColumns="false" CssClass="connectedSortable" Width="100%"
                                        OnRowDataBound="gvProduto_RowDataBound">
                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" ItemStyle-Width="30">
                                                <ItemTemplate>
                                                    <%# Eval("ID_PRODUTO_MAG") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" ItemStyle-Width="30">
                                                <ItemTemplate>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td valign="bottom" style="text-align: center; border: none; vertical-align: middle;">
                                                                <asp:Label ID="labProdutoTitulo1" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: center; border: none;">
                                                                <asp:Image ID="imgFrenteCab1" runat="server" Width="200px" Height="" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                            <td style="width: 33%; vertical-align: top;">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvProduto2" runat="server" AutoGenerateColumns="false" CssClass="connectedSortable" Width="100%"
                                        OnRowDataBound="gvProduto_RowDataBound">
                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" ItemStyle-Width="30">
                                                <ItemTemplate>
                                                    <%# Eval("ID_PRODUTO_MAG") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" ItemStyle-Width="30">
                                                <ItemTemplate>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td valign="bottom" style="text-align: center; border: none; vertical-align: middle;">
                                                                <asp:Label ID="labProdutoTitulo1" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: center; border: none;">
                                                                <asp:Image ID="imgFrenteCab1" runat="server" Width="200px" Height="" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                            <td style="width: 33%; vertical-align: top;">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvProduto3" runat="server" AutoGenerateColumns="false" CssClass="connectedSortable" Width="100%"
                                        OnRowDataBound="gvProduto_RowDataBound">
                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" ItemStyle-Width="30">
                                                <ItemTemplate>
                                                    <%# Eval("ID_PRODUTO_MAG") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" ItemStyle-Width="30">
                                                <ItemTemplate>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td valign="bottom" style="text-align: center; border: none; vertical-align: middle;">
                                                                <asp:Label ID="labProdutoTitulo1" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: center; border: none;">
                                                                <asp:Image ID="imgFrenteCab1" runat="server" Width="200px" Height="" />
                                                            </td>
                                                        </tr>
                                                    </table>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <br />
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
