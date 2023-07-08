<%@ Page Title="Devolução Interna de Produto" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="admloj_retirada_mercadoria_porarquivo.aspx.cs" Inherits="Relatorios.admloj_retirada_mercadoria_porarquivo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="../js/jquery-1.11.1.min.js"></script>
    <script type="text/javascript" src="http://code.jquery.com/jquery.js"></script>
    <script type="text/javascript" src="../js/jquery-ui.min.js"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="login">
        <legend>Devolução Interna de Produto</legend>
        <div class="accountInfo">
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                        <asp:HiddenField ID="hidCodigoTransferencia" runat="server" Value="" />
                    </td>
                    <td>
                        <asp:Label ID="labArquivo" runat="server" Text="Arquivo TXT"></asp:Label>
                    </td>
                </tr>
                <tr>
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
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btImportar" runat="server" Text="Importar" Width="100px" OnClick="btImportar_Click" />&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="labErroImport" runat="server" ForeColor="Red" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
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
                            <asp:BoundField DataField="FILIAL" HeaderText="Filial" />
                            <asp:BoundField DataField="FILIAL_ORIGEM" HeaderText="Filial Origem" />
                            <asp:BoundField DataField="CODIGO_BARRA" HeaderText="Código de Barra" HeaderStyle-Width="220px" />
                            <asp:BoundField DataField="PRODUTO" HeaderText="Produto" HeaderStyle-Width="220px" />
                            <asp:BoundField DataField="NOME" HeaderText="Nome" />
                            <asp:TemplateField HeaderText="Cor" HeaderStyle-Width="250px">
                                <ItemTemplate>
                                    <asp:Label ID="labCor" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TAMANHO" HeaderText="Grade" />

                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset>
            <div>
                <br />
                <br />
                <asp:Button runat="server" ID="btSalvar" Text="Salvar" Width="120px" OnClick="btSalvar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;
                <asp:Label runat="server" ID="labErro" ForeColor="Red"></asp:Label>
            </div>
            <div>
                <br />
                <br />
                <br />
            </div>
        </div>
        <div id="dialogPai" runat="server">
            <div id="dialog" title="Mensagem" class="divPop">
                <table border="0" width="100%">
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; color: red;">
                            <strong>Aviso</strong>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; color: red;">ENVIADO COM SUCESSO
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </fieldset>
</asp:Content>
