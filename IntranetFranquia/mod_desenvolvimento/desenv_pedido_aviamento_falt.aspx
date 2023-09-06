<%@ Page Title="Aviamento Faltante" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="desenv_pedido_aviamento_falt.aspx.cs" Inherits="Relatorios.desenv_pedido_aviamento_falt" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
        }

        .alinharcentro {
            text-align: center;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
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
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Aviamentos&nbsp;&nbsp;>&nbsp;&nbsp;Aviamento Faltante</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Aviamento Faltante</legend>
                    <br />
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">

                        <tr>
                            <td>Coleção</td>
                            <td>HB</td>
                            <td>Produto</td>
                            <td>Nome</td>
                            <td>Pedido Realizado</td>
                        </tr>
                        <tr>
                            <td style="width: 230px;" valign="top">
                                <asp:DropDownList ID="ddlColecoes" runat="server" Width="224px" Height="21px" DataTextField="DESC_COLECAO"
                                    DataValueField="COLECAO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 150px;">
                                <asp:TextBox ID="txtHB" runat="server" Width="140px" MaxLength="10"></asp:TextBox>
                            </td>
                            <td style="width: 150px;">
                                <asp:TextBox ID="txtProduto" runat="server" Width="140px" MaxLength="10"></asp:TextBox>
                            </td>
                            <td style="width: 260px;">
                                <asp:TextBox ID="txtNome" runat="server" Width="250px" MaxLength="40"></asp:TextBox>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPedidoRealizado" runat="server" Width="120px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" OnClientClick="DesabilitarBotao(this);" />
                                &nbsp;&nbsp;<asp:Label ID="labMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvMaterialFalt" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvMaterialFalt_RowDataBound" ShowFooter="true"
                                        DataKeyNames="">
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Bold="true" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Coleção" HeaderStyle-Width="100px" SortExpression="DESC_COLECAO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColecao" runat="server" Text='<%# Bind("DESC_COLECAO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="HB" HeaderStyle-Width="100px" SortExpression="HB">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litHB" runat="server" Text='<%# Bind("HB") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Produto" HeaderStyle-Width="100px" SortExpression="PRODUTO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litProduto" runat="server" Text='<%# Bind("PRODUTO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Nome" HeaderStyle-Width="" SortExpression="DESC_MODELO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNome" runat="server" Text='<%# Bind("DESC_PRODUTO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Cor Produto" HeaderStyle-Width="" SortExpression="DESC_COR_PRODUTO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litCor" runat="server" Text='<%# Bind("DESC_COR_PRODUTO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="SubGrupo" HeaderStyle-Width="" SortExpression="SUBGRUPO_MATERIAL">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litSubGrupo" runat="server" Text='<%# Bind("SUBGRUPO_MATERIAL") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Cor Material" HeaderStyle-Width="" SortExpression="COR_FORNECEDOR_MATERIAL">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litCorMaterial" runat="server" Text='<%# Bind("COR_FORNECEDOR_MATERIAL") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Qtde" HeaderStyle-Width="130px" SortExpression="QTDE_FALTANTE">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtde" runat="server" Text='<%# Bind("QTDE_FALTANTE") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Data Pedido" HeaderStyle-Width="130px" SortExpression="DATA_BAIXA">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDataBaixa" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="" HeaderStyle-Width="80px" SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnBaixar" runat="server" Width="80px" Text="Baixar" OnClick="btnBaixar_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
