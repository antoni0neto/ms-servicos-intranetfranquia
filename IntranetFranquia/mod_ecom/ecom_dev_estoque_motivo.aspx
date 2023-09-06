<%@ Page Title="Motivo de Devolução" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ecom_dev_estoque_motivo.aspx.cs" Inherits="Relatorios.ecom_dev_estoque_motivo" %>

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Estoque&nbsp;&nbsp;>&nbsp;&nbsp;Motivo de Devolução</span>
                <div style="float: right; padding: 0;">
                    <a href="ecom_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Devolução Estoque</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>Produto
                            </td>
                            <td>NF Entrada
                            </td>
                            <td>Motivo
                            </td>
                            <td>Período Inicial
                            </td>
                            <td>Período Final
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 160px">
                                <asp:TextBox ID="txtProduto" runat="server" onkeypress="return fnValidarNumero(event);"
                                    Width="150px"></asp:TextBox>
                            </td>
                            <td style="width: 160px">
                                <asp:TextBox ID="txtNFEntrada" runat="server" onkeypress="return fnValidarNumero(event);"
                                    Width="150px"></asp:TextBox>
                            </td>
                            <td style="width: 160px">
                                <asp:DropDownList ID="ddlMotivo" runat="server" Width="154px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="CUPOM" Text="CUPOM"></asp:ListItem>
                                    <asp:ListItem Value="ESTORNO" Text="ESTORNO"></asp:ListItem>
                                    <asp:ListItem Value="TROCA" Text="TROCA"></asp:ListItem>
                                    <asp:ListItem Value="VAZIO" Text="VAZIO"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtDataIni" runat="server" Width="150px" onkeypress="return fnValidarData(event);"
                                    MaxLength="10" Style="text-align: right;" />
                            </td>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtDataFim" runat="server" Width="150px" onkeypress="return fnValidarData(event);"
                                    MaxLength="10" Style="text-align: right;" />
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Width="100px" Text="Buscar" OnClick="btBuscar_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <div id="accordionP">
                                    <h3>Devolução Produto</h3>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound" OnDataBound="gvProduto_DataBound" ShowFooter="true"
                                                            DataKeyNames="CODIGO">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="NOME_CLIFOR" HeaderText="Cliente" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                                <asp:BoundField DataField="PRODUTO" HeaderText="Produto" HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                                <asp:BoundField DataField="DESC_PRODUTO" HeaderText="Nome" HeaderStyle-Width="300px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                                <asp:BoundField DataField="DESC_COR" HeaderText="Cor" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                                <asp:BoundField DataField="NF_ENTRADA" HeaderText="NF Entrada" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                                <asp:TemplateField HeaderText="Recebimento" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litRecebimento" runat="server" Text=""></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlMotivo" runat="server" Width="160px" OnSelectedIndexChanged="ddlMotivo_SelectedIndexChanged" AutoPostBack="true">
                                                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                                                            <asp:ListItem Value="CUPOM" Text="CUPOM"></asp:ListItem>
                                                                            <asp:ListItem Value="ESTORNO" Text="ESTORNO"></asp:ListItem>
                                                                            <asp:ListItem Value="TROCA" Text="TROCA"></asp:ListItem>
                                                                        </asp:DropDownList>
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
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
