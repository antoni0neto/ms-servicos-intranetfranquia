<%@ Page Title="Ordenação de Produtos" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master"
    CodeBehind="ecom_dragdrop_ordenacao.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="Relatorios.ecom_dragdrop_ordenacao" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        #listView {
            padding: 10px 5px;
            margin-bottom: -1px;
            min-height: 400px;
        }

        .product {
            float: left;
            position: relative;
            width: 111px;
            height: 170px;
            margin: 0 5px;
            padding: 0;
            cursor: move;
        }

            .product img {
                width: 110px;
                height: 110px;
            }

            .product h3 {
                margin: 0;
                padding: 3px 5px 0 0;
                max-width: 96px;
                overflow: hidden;
                line-height: 1.1em;
                font-size: .9em;
                font-weight: normal;
                text-transform: uppercase;
            }

            .product p {
                visibility: hidden;
            }

        #listView .product:hover p {
            visibility: visible;
            position: absolute;
            width: 110px;
            height: 110px;
            top: 0;
            margin: 0;
            padding: 0;
            line-height: 110px;
            vertical-align: middle;
            text-align: center;
            color: #fff;
            background-color: rgba(0,0,0,0.75);
            transition: background .2s linear, color .2s linear;
            -moz-transition: background .2s linear, color .2s linear;
            -webkit-transition: background .2s linear, color .2s linear;
            -o-transition: background .2s linear, color .2s linear;
        }

        .k-listview:after {
            content: ".";
            display: block;
            height: 0;
            clear: both;
            visibility: hidden;
        }
    </style>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.24/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.24/jquery-ui.min.js"></script>
    <script src="../js/js.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $("[id*=gvLocations]").sortable({
                items: 'tr:not(tr:first-child)',
                cursor: 'pointer',
                axis: 'y',
                dropOnEmpty: false,
                start: function (e, ui) {
                    ui.item.addClass("selected");
                },
                stop: function (e, ui) {
                    ui.item.removeClass("selected");
                },
                receive: function (e, ui) {
                    $(this).find("tbody").append(ui.item);
                }
            });
        });
    </script>

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Produto&nbsp;&nbsp;>&nbsp;&nbsp;Ordenação de Produtos</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <br />
            <div style="float: left; width: 1000px; margin-left: 15%;">
                <fieldset>
                    <legend>Ordenação de Produtos</legend>

                    <asp:GridView ID="gvLocations" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField HeaderText="Id" ItemStyle-Width="30">
                                <ItemTemplate>
                                    <%# Eval("Id1") %>
                                    <input type="hidden" name="LocationId1" value='<%# Eval("Id1") %>' />
                                    <input name="Location1" value='<%# Eval("Location1") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Id" ItemStyle-Width="30">
                                <ItemTemplate>
                                    <%# Eval("Id2") %>
                                    <input type="hidden" name="LocationId2" value='<%# Eval("Id2") %>' />
                                    <input name="Location2" value='<%# Eval("Location2") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Id" ItemStyle-Width="30">
                                <ItemTemplate>
                                    <%# Eval("Id3") %>
                                    <input type="hidden" name="LocationId3" value='<%# Eval("Id3") %>' />
                                    <input name="Location3" value='<%# Eval("Location3") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Button ID="btUpdate" Text="Update Preference" runat="server" OnClick="btUpdate_Click" />

                </fieldset>
                <br />
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
