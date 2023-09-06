<%@ Page Title="Consultar Produto Recebido" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="gerloja_consulta_entrega_loja.aspx.cs" Inherits="Relatorios.gerloja_consulta_entrega_loja"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Painel do Supervisor&nbsp;&nbsp;>&nbsp;&nbsp;Consultar Produto Recebido</span>
                <div style="float: right; padding: 0;">
                    <a href="gerloja_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Consultar Produto Recebido"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Filial</td>
                        <td>Griffe</td>
                        <td>Grupo</td>
                        <td>Produto</td>
                        <td>Cor</td>
                    </tr>
                    <tr>
                        <td style="width: 260px;">
                            <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Width="254px">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 230px;" valign="top">
                            <asp:DropDownList ID="ddlGriffe" runat="server" Width="224px" Height="21px" DataTextField="GRIFFE"
                                DataValueField="GRIFFE">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 170px;">
                            <asp:DropDownList ID="ddlGrupo" runat="server" Width="164px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                DataValueField="GRUPO_PRODUTO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 154px;" valign="top">
                            <asp:TextBox ID="txtProduto" runat="server" Width="140px" MaxLength="10" OnTextChanged="txtProduto_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </td>
                        <td valign="top">
                            <asp:DropDownList ID="ddlCor" runat="server" Width="214px" Height="21px" DataTextField="DESC_COR_PRODUTO"
                                DataValueField="COR_PRODUTO">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvProdutoRecebido" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                    Style="background: white" OnRowDataBound="gvProdutoRecebido_RowDataBound" OnDataBound="gvProdutoRecebido_DataBound"
                                    ShowFooter="true">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Filial" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="FILIAL" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="">
                                            <ItemTemplate>
                                                <asp:Label ID="labFilial" runat="server" Text='<%# Bind("FILIAL") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Produto" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            SortExpression="PRODUTO">
                                            <ItemTemplate>
                                                <asp:Label ID="labProduto" runat="server" Text='<%# Bind("PRODUTO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nome" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="255px"
                                            SortExpression="DESC_PRODUTO" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labNome" runat="server" Text='<%# Bind("DESC_PRODUTO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cor" HeaderStyle-Width="220px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="DESC_COR" ItemStyle-Font-Size="Smaller">
                                            <ItemTemplate>
                                                <asp:Label ID="labCor" runat="server" Text='<%# Bind("DESC_COR") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Enviado" HeaderStyle-Width="220px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="labDataEnviado" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Recebido" HeaderStyle-Width="220px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="labDataRecebido" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">&nbsp;
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
