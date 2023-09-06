<%@ Page Title="Seleção de Fotos Ecommerce" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ecom_cad_foto.aspx.cs" Inherits="Relatorios.ecom_cad_foto" %>

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro&nbsp;&nbsp;>&nbsp;&nbsp;Seleção de Fotos Ecommerce</span>
                <div style="float: right; padding: 0;">
                    <a href="ecom_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Seleção de Fotos Ecommerce</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>Produto
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 500px;">
                                <asp:DropDownList ID="ddlProduto" runat="server" Width="494px" Height="22px" DataTextField="DIRETORIO_DESC" DataValueField="DIRETORIO_ORIGEM"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btAbrirDiretorio" runat="server" Width="150px" Text="Abrir Produto" OnClick="btAbrirDiretorio_Click" />&nbsp;
                                <asp:Button ID="btLimpar" runat="server" Width="150px" Text="Limpar" OnClick="btLimpar_Click" />
                                <asp:Label ID="labErroDir" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Image ID="imgProdutoDesenv" runat="server" Width="" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div id="accordionP">
                                    <h3>Produtos</h3>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound"
                                                            OnDataBound="gvProduto_DataBound" ShowFooter="true">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="45px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="imgProduto" runat="server" Width="160px" Height="240px" ImageAlign="AbsMiddle" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Look" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="cbLook" runat="server" Checked="false" Text="Look" OnCheckedChanged="cbLook_CheckedChanged" AutoPostBack="true" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Frente Cabeça" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="cbFrenteCabeca" runat="server" Checked="false" Text="Frente Cabeça" OnCheckedChanged="cbLook_CheckedChanged" AutoPostBack="true" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Frente S/ Cabeça" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="cbFrenteSemCabeca" runat="server" Checked="false" Text="Frente S/ Cabeça" OnCheckedChanged="cbLook_CheckedChanged" AutoPostBack="true" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Costas" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="cbCostas" runat="server" Checked="false" Text="Costas" OnCheckedChanged="cbLook_CheckedChanged" AutoPostBack="true" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Detalhe" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="cbDetalhe" runat="server" Checked="false" Text="Detalhe" OnCheckedChanged="cbLook_CheckedChanged" AutoPostBack="true" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Lado" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="cbLado" runat="server" Checked="false" Text="Lado" OnCheckedChanged="cbLook_CheckedChanged" AutoPostBack="true" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Diretório" HeaderStyle-Width="45px" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litDiretorio" runat="server"></asp:Literal>
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
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btSalvar" runat="server" Text="Salvar Foto" Width="150px" OnClick="btSalvar_Click" />&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
