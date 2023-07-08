<%@ Page Title="Pedidos de Transferência/Devolução" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="admloj_retirada_mercadoria_carr_retirada.aspx.cs" Inherits="Relatorios.admloj_retirada_mercadoria_carr_retirada" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://code.jquery.com/jquery.js"></script>
    <script src="js/jquery-ui.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="jquery-1.6.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="login">
        <legend>Pedidos de Transferência/Devolução</legend>
        <div class="accountInfo">
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Label ID="labDescricao" runat="server" Text="Descrição"></asp:Label></td>
                    <td>
                        <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                        <asp:HiddenField ID="hidCodigoTransferencia" runat="server" Value="" />
                    </td>
                    <td>
                        <asp:Label ID="labArquivo" runat="server" Text="Arquivo TXT"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 300px;">
                        <asp:TextBox ID="txtDescricao" runat="server" MaxLength="1000" Width="290px"></asp:TextBox>
                    </td>
                    <td style="width: 256px;">
                        <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                            Height="22px" Width="250px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:FileUpload ID="uploadArquivoProduto" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Button ID="btImportar" runat="server" Text="Importar" Width="100px" OnClick="btImportar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="labErroImport" runat="server" ForeColor="Red" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
        <div>
            <fieldset>
                <legend>Produtos</legend>
                <div style="position: relative; float: right; width: 100%;">
                    <div style="position: relative; float: right; width: 100%; text-align: right;">
                        <asp:Button ID="btExcluir" runat="server" OnClick="btExcluir_Click" Text="Excluir Produtos" Width="150px" />
                        <br />
                        <br />
                    </div>
                </div>
                <div class="rounded_corners">
                    <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound"
                        ShowFooter="true" DataKeyNames="CODIGO">
                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="labColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgProduto" runat="server" Width="90%" ImageAlign="AbsMiddle" ToolTip="" AlternateText="" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="CODIGO_BARRA" HeaderText="Código de Barras" HeaderStyle-Width="220px" />
                            <asp:BoundField DataField="PRODUTO" HeaderText="Produto" HeaderStyle-Width="220px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="NOME" HeaderText="Nome" />
                            <asp:TemplateField HeaderText="Cor" HeaderStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label ID="labCor" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TAMANHO" HeaderText="Grade" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />

                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset>

            <br />
            <br />
        </div>
    </fieldset>
</asp:Content>
