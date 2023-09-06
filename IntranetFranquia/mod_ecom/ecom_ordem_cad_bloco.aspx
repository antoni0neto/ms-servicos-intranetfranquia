<%@ Page Title="Gerenciamento de Blocos" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="ecom_ordem_cad_bloco.aspx.cs" Inherits="Relatorios.ecom_ordem_cad_bloco"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.24/themes/smoothness/jquery-ui.css" />
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }
    </style>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.24/jquery-ui.min.js"></script>
    <script type="text/javascript">
        $(function () {
            sortTableBloco();
        });


        function sortTableBloco() {
            $("[id*=gvBloco]").sortable({
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
                },
                update: function (e, ui) {

                    var rows = $.map($('[id*=gvBloco] tr td:first-child'), function (el) {
                        return $.trim($(el).next().next().next().next().text());
                    }).toString();


                    var blocos = new Array();
                    blocos = rows.split(',');

                    $.ajax({
                        type: 'POST',
                        url: 'ecom_ordem_cad_bloco.aspx/SalvarOrdem',
                        contentType: "application/json; charset=utf-8",
                        data: '{ blocos: [' + blocos.join() + '] }',
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
            });
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Ordenação de Produtos&nbsp;&nbsp;>&nbsp;&nbsp;Gerenciamento de Blocos</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1200px; margin-left: 8%;">
                <fieldset>
                    <legend>Gerenciamento de Blocos</legend>
                    <fieldset>
                        <legend>Inclusão/Alteração</legend>
                        <table border="0" cellspacing="0" cellpadding="0" width="100%" class="tb">
                            <tr style="height: 10px; vertical-align: bottom;">
                                <td>
                                    <asp:Label ID="labCategoria" runat="server" Text="Categoria Magento"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labBloco" runat="server" Text="Nome do Bloco"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labOrdem" runat="server" Text="Ordem"></asp:Label>
                                </td>
                                <td>
                                    <asp:HiddenField ID="hidCodigo" runat="server" />
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 180px;">
                                    <asp:DropDownList ID="ddlCategoriaMag" runat="server" Width="174px" Height="21px" DataTextField="GRUPO"
                                        DataValueField="CODIGO">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 300px;">
                                    <asp:TextBox ID="txtBloco" runat="server" Width="290px"></asp:TextBox>
                                </td>
                                <td style="width: 160px;">
                                    <asp:TextBox ID="txtOrdem" runat="server" Width="150px" onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                </td>
                                <td style="width: 200px;">
                                    <asp:Button runat="server" ID="btIncluir" Text="Incluir" OnClick="btIncluir_Click"
                                        Width="100px" Enabled="true" />
                                </td>
                                <td>
                                    <div style="float: right; margin-right: 0px;">
                                        <asp:Button runat="server" ID="btCancelar" Text="Cancelar" OnClick="btCancelar_Click"
                                            Enabled="true" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="labErro" runat="server" ForeColor="red" Text="">
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">&nbsp;</td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset>
                        <legend>Manutenção</legend>
                        <table border="0" cellpadding="0" cellspacing="0" class="tb" width="100%">
                            <tr>
                                <td>Filtro Categoria Magento
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 300px;">
                                    <asp:DropDownList ID="ddlCategoriaMagFiltro" runat="server" Width="294px" Height="21px" DataTextField="GRUPO"
                                        DataValueField="CODIGO">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btFiltrar" runat="server" OnClick="btFiltrar_Click" Text="Buscar" Width="100px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvBloco" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvBloco_RowDataBound">
                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Ordem" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litOrdem" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Categoria Magento" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="300px" ItemStyle-Width="300px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litCatMagento" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="BLOCO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderText="Nome Bloco" HeaderStyle-Width="300px" ItemStyle-Width="300px" />

                                                <asp:TemplateField HeaderText="Qtde Produto" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTot4" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Id" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Eval("CODIGO") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="65px"
                                                    ItemStyle-Width="65px">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btAlterar" runat="server" Height="19px" Width="65px" Text="Alterar" OnClick="btAlterar_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" Visible="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="65px"
                                                    ItemStyle-Width="65px">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btExcluir" runat="server" Height="19px" Text="Excluir" Width="65px" OnClientClick="return ConfirmarExclusao();"
                                                            OnClick="btExcluir_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
