<%@ Page Title="Liberação de TAG" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="desenv_fila_tag.aspx.cs" Inherits="Relatorios.desenv_fila_tag" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Liberação Produto Acabado&nbsp;&nbsp;>&nbsp;&nbsp;Liberação de TAG</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Liberação de TAG</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">

                        <tr>
                            <td>HB</td>
                            <td>Produto</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 140px;">
                                <asp:TextBox ID="txtHB" runat="server" Width="130px" MaxLength="5" CssClass="alinharDireita"
                                    onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                            </td>
                            <td style="width: 140px;">
                                <asp:TextBox ID="txtProduto" runat="server" Width="130px" MaxLength="5" CssClass="alinharDireita"
                                    onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" OnClientClick="DesabilitarBotao(this);" />
                                &nbsp;&nbsp;<asp:Label ID="labMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div id="accordionP">
                                    <h3>Produção</h3>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvPrincipal" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvPrincipal_RowDataBound"
                                                            OnDataBound="gvPrincipal_DataBound" OnSorting="gvPrincipal_Sorting" AllowSorting="true"
                                                            ShowFooter="true" DataKeyNames="TIPO, PEDIDO, PRODUTO, COR_PRODUTO, ENTREGA">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="DESC_COLECAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Coleção" HeaderStyle-Width="200px" SortExpression="DESC_COLECAO" />
                                                                <asp:BoundField DataField="HB" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="HB" HeaderStyle-Width="120px" SortExpression="HB" />
                                                                <asp:BoundField DataField="PRODUTO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Produto" HeaderStyle-Width="150px" SortExpression="PRODUTO" />
                                                                <asp:BoundField DataField="NOME" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Nome" HeaderStyle-Width="200px" SortExpression="NOME" />
                                                                <asp:BoundField DataField="DESC_COR" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Cor" HeaderStyle-Width="" SortExpression="DESC_COR" />
                                                                <asp:BoundField DataField="TIPO_DESC" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Tipo" HeaderStyle-Width="" SortExpression="TIPO_DESC" />
                                                                <asp:BoundField DataField="GRADE_TOTAL" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Grade Total" HeaderStyle-Width="" SortExpression="GRADE_TOTAL" />
                                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                                    <ItemTemplate>
                                                                        <asp:Button ID="btBaixar" runat="server" Text="Baixar" Height="21px" OnClick="btBaixar_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <br />
                                <div style="float: right;">
                                    <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
