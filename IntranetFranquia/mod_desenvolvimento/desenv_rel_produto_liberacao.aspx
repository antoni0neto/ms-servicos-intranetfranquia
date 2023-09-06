<%@ Page Title="Produtos Liberados" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="desenv_rel_produto_liberacao.aspx.cs" Inherits="Relatorios.desenv_rel_produto_liberacao"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita
        {
            text-align: right;
        }
    </style>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../Image/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "../Image/plus.png");
            $(this).closest("tr").next().remove();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro
                    de Apoio&nbsp;&nbsp;>&nbsp;&nbsp;Produtos Liberados</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Produtos Liberados"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            Coleção
                        </td>
                        <td>
                            Origem
                        </td>
                        <td>
                            Grupo
                        </td>
                        <td>
                            Modelo
                        </td>
                        <td>
                            Nome
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 190px;">
                            <asp:DropDownList ID="ddlColecoes" runat="server" Width="184px" Height="21px" DataTextField="DESC_COLECAO"
                                DataValueField="COLECAO" OnSelectedIndexChanged="ddlColecoes_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 180px;">
                            <asp:DropDownList ID="ddlOrigem" runat="server" Width="174px" Height="21px" DataTextField="DESCRICAO"
                                DataValueField="CODIGO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 180px;">
                            <asp:DropDownList ID="ddlGrupo" runat="server" Width="174px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                DataValueField="GRUPO_PRODUTO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 160px;">
                            <asp:TextBox ID="txtModelo" runat="server" Width="150px"></asp:TextBox>
                        </td>
                        <td style="width: 180px;">
                            <asp:TextBox ID="txtNome" runat="server" Width="170px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound"
                                    DataKeyNames="CODIGO">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="35px" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <img alt="" style="cursor: pointer" src="../Image/plus.png" width="18px" />
                                                <asp:Panel ID="pnlFoto" runat="server" Style="display: none" Width="100%">
                                                    <asp:GridView ID="gvFoto" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvFoto_RowDataBound"
                                                        Width="100%" DataKeyNames="CODIGO">
                                                        <HeaderStyle BackColor="LightSteelBlue" HorizontalAlign="Center"></HeaderStyle>
                                                        <RowStyle BorderColor="Gainsboro" BorderWidth="1px" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Foto Produto" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50%" ItemStyle-BorderColor="Gainsboro"
                                                                ItemStyle-BorderWidth="1px">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgFotoProduto" Width="80" Height="130" runat="server" ImageAlign="Middle" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Modelo" HeaderStyle-Width="200px" />
                                        <asp:BoundField DataField="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Grupo" HeaderStyle-Width="200px" />
                                        <asp:BoundField DataField="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Nome" HeaderStyle-Width="150px" />
                                        <asp:BoundField DataField="" HeaderText="Cor" HeaderStyle-Width="98px" />
                                        <asp:BoundField DataField="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Origem" HeaderStyle-Width="200px" />
                                        <asp:BoundField DataField="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Qtde" HeaderStyle-Width="200px" />
                                        <asp:BoundField DataField="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="HB" HeaderStyle-Width="150px" />
                                        <asp:BoundField DataField="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Status" HeaderStyle-Width="150px" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
