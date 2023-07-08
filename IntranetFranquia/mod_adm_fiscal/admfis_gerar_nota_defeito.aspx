<%@ Page Title="Gerar Arquivo de Produtos c/ Defeito" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="admfis_gerar_nota_defeito.aspx.cs" Inherits="Relatorios.admfis_gerar_nota_defeito" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

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
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo ADM Nota Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Administrativo Nota Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Gerar Arquivo de Produtos c/ Defeito</span>
        <div style="float: right; padding: 0;">
            <a href="admfis_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset class="login">
            <legend>Gerar Arquivo de Produtos c/ Defeito</legend>
            <table border="0" class="style1" cellpadding="0" cellspacing="0">
                <tr>
                    <td>Filial
                    </td>
                    <td>
                        <asp:HiddenField runat="server" ID="hidCodigoNotaRetirada" Value="" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 256px">
                        <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="21px" Width="250px"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btBuscar" Text="Buscar" Width="120px" OnClick="btBuscar_Click" />&nbsp;
                        <asp:Label runat="server" ID="labErro" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="rounded_corners">
                            <asp:GridView ID="gvNotaRetirada" runat="server" Width="100%" AutoGenerateColumns="False"
                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvNotaRetirada_RowDataBound" ShowFooter="true">
                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Filial" HeaderStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="LiteralFilial" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="100px" HeaderText="CD - LUGZY" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNFCDLugzy" runat="server" Width="100px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="140px" HeaderText="CD LUGZY TRANSITO" HeaderStyle-Font-Size="Smaller">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNFCDLugzyTransito" runat="server" Width="140px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="100px" HeaderText="C-MAX (NOVA)" HeaderStyle-Font-Size="Smaller">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNFCmaxNova" runat="server" Width="100px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="160px" HeaderText="C-MAX (NOVA) MOSTRUARIO" HeaderStyle-Font-Size="Smaller">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNFCmaxNovaMostruario" runat="server" Width="160px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="140px" HeaderText="C-MAX (NOVA) TRANSITO" HeaderStyle-Font-Size="Smaller">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNFCmaxNovaTransito" runat="server" Width="140px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="140px" HeaderText="CD LUCIANA TRANSITO" HeaderStyle-Font-Size="Smaller">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNFLucianaTransito" runat="server" Width="140px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="100px" HeaderText="CD MOSTRUARIO" HeaderStyle-Font-Size="Smaller">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNFCDMostruario" runat="server" Width="100px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="100px" HeaderText="CD TAGZY" HeaderStyle-Font-Size="Smaller">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNFCDTagzy" runat="server" Width="100px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Button runat="server" ID="btGerarArquivo" Text="Gerar Arquivo" Width="100px" OnClick="btGerarArquivo_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Button runat="server" ID="btRegistrarNota" Text="Registrar Nota" Width="100px" OnClick="btRegistrarNota_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:Button runat="server" ID="btAbrirNota" Text=">>" Width="40px" OnClick="btAbrirNota_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;
                    </td>
                </tr>
            </table>

            <fieldset id="fsCDLugzy" runat="server" visible="false">
                <legend>CD - LUGZY</legend>
                <table border="0" class="style1" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div class="rounded_corners">
                                <asp:GridView ID="gvCDLugzy" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound" ShowFooter="true">
                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Produto" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="140px" />
                                        <asp:BoundField DataField="DESCRICAO_PRODUTO" HeaderText="Nome" />
                                        <asp:BoundField DataField="COR_PRODUTO" HeaderText="Cor" ItemStyle-Width="220px" />
                                        <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" ItemStyle-Width="170px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Origem Defeito" ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="litOrigemDefeito" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Defeito" ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="litDefeito" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TIPO_NOTA" HeaderText="Tipo da Nota" ItemStyle-Width="150px" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset id="fsCDLugzyTransito" runat="server" visible="false">
                <legend>CD LUGZY TRANSITO</legend>
                <table border="0" class="style1" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div class="rounded_corners">
                                <asp:GridView ID="gvCDLugzyTransito" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound" ShowFooter="true">
                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Produto" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="140px" />
                                        <asp:BoundField DataField="DESCRICAO_PRODUTO" HeaderText="Nome" />
                                        <asp:BoundField DataField="COR_PRODUTO" HeaderText="Cor" ItemStyle-Width="220px" />
                                        <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" ItemStyle-Width="170px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Origem Defeito" ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="litOrigemDefeito" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Defeito" ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="litDefeito" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TIPO_NOTA" HeaderText="Tipo da Nota" ItemStyle-Width="150px" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset id="fsCmaxNova" runat="server" visible="false">
                <legend>C-MAX (NOVA)</legend>
                <table border="0" class="style1" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div class="rounded_corners">
                                <asp:GridView ID="gvCmaxNova" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound" ShowFooter="true">
                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Produto" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="140px" />
                                        <asp:BoundField DataField="DESCRICAO_PRODUTO" HeaderText="Nome" />
                                        <asp:BoundField DataField="COR_PRODUTO" HeaderText="Cor" ItemStyle-Width="220px" />
                                        <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" ItemStyle-Width="170px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Origem Defeito" ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="litOrigemDefeito" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Defeito" ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="litDefeito" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TIPO_NOTA" HeaderText="Tipo da Nota" ItemStyle-Width="150px" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset id="fsCmaxNovaMostruario" runat="server" visible="false">
                <legend>C-MAX (NOVA) MOSTRUARIO</legend>
                <table border="0" class="style1" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div class="rounded_corners">
                                <asp:GridView ID="gvCmaxNovaMostruario" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound" ShowFooter="true">
                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Produto" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="140px" />
                                        <asp:BoundField DataField="DESCRICAO_PRODUTO" HeaderText="Nome" />
                                        <asp:BoundField DataField="COR_PRODUTO" HeaderText="Cor" ItemStyle-Width="220px" />
                                        <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" ItemStyle-Width="170px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Origem Defeito" ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="litOrigemDefeito" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Defeito" ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="litDefeito" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TIPO_NOTA" HeaderText="Tipo da Nota" ItemStyle-Width="150px" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset id="fsCmaxNovaTransito" runat="server" visible="false">
                <legend>C-MAX (NOVA) TRANSITO</legend>
                <table border="0" class="style1" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div class="rounded_corners">
                                <asp:GridView ID="gvCmaxNovaTransito" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound" ShowFooter="true">
                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Produto" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="140px" />
                                        <asp:BoundField DataField="DESCRICAO_PRODUTO" HeaderText="Nome" />
                                        <asp:BoundField DataField="COR_PRODUTO" HeaderText="Cor" ItemStyle-Width="220px" />
                                        <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" ItemStyle-Width="170px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Origem Defeito" ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="litOrigemDefeito" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Defeito" ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="litDefeito" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TIPO_NOTA" HeaderText="Tipo da Nota" ItemStyle-Width="150px" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset id="fsCDLucianaTransito" runat="server" visible="false">
                <legend>CD LUCIANA TRANSITO</legend>
                <table border="0" class="style1" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div class="rounded_corners">
                                <asp:GridView ID="gvCDLucianaTransito" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound" ShowFooter="true">
                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Produto" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="140px" />
                                        <asp:BoundField DataField="DESCRICAO_PRODUTO" HeaderText="Nome" />
                                        <asp:BoundField DataField="COR_PRODUTO" HeaderText="Cor" ItemStyle-Width="220px" />
                                        <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" ItemStyle-Width="170px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Origem Defeito" ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="litOrigemDefeito" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Defeito" ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="litDefeito" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TIPO_NOTA" HeaderText="Tipo da Nota" ItemStyle-Width="150px" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset id="fsCDMostruario" runat="server" visible="false">
                <legend>CD MOSTRUARIO</legend>
                <table border="0" class="style1" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div class="rounded_corners">
                                <asp:GridView ID="gvCDMostruario" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound" ShowFooter="true">
                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Produto" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="140px" />
                                        <asp:BoundField DataField="DESCRICAO_PRODUTO" HeaderText="Nome" />
                                        <asp:BoundField DataField="COR_PRODUTO" HeaderText="Cor" ItemStyle-Width="220px" />
                                        <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" ItemStyle-Width="170px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Origem Defeito" ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="litOrigemDefeito" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Defeito" ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="litDefeito" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TIPO_NOTA" HeaderText="Tipo da Nota" ItemStyle-Width="150px" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset id="fsCDTagzy" runat="server" visible="false">
                <legend>CD TAGZY</legend>
                <table border="0" class="style1" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div class="rounded_corners">
                                <asp:GridView ID="gvCDTagzy" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound" ShowFooter="true">
                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Produto" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="140px" />
                                        <asp:BoundField DataField="DESCRICAO_PRODUTO" HeaderText="Nome" />
                                        <asp:BoundField DataField="COR_PRODUTO" HeaderText="Cor" ItemStyle-Width="220px" />
                                        <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" ItemStyle-Width="170px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Origem Defeito" ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="litOrigemDefeito" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Defeito" ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="litDefeito" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TIPO_NOTA" HeaderText="Tipo da Nota" ItemStyle-Width="150px" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset id="fsSemSaldo" runat="server" visible="false">
                <legend>Outros/Sem Saldo</legend>
                <table border="0" class="style1" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div class="rounded_corners">
                                <asp:GridView ID="gvOutros" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound" ShowFooter="true">
                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CODIGO_PRODUTO" HeaderText="Produto" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="140px" />
                                        <asp:BoundField DataField="DESCRICAO_PRODUTO" HeaderText="Nome" />
                                        <asp:BoundField DataField="COR_PRODUTO" HeaderText="Cor" ItemStyle-Width="220px" />
                                        <asp:BoundField DataField="TAMANHO" HeaderText="Tamanho" ItemStyle-Width="170px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Origem Defeito">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="litOrigemDefeito" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Defeito">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="litDefeito" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TIPO_NOTA" HeaderText="Tipo da Nota" ItemStyle-Width="150px" />
                                    </Columns>
                                </asp:GridView>
                            </div>

                        </td>
                    </tr>
                </table>
            </fieldset>
        </fieldset>
    </div>
</asp:Content>
