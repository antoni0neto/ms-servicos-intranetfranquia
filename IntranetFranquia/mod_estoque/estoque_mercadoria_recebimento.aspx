<%@ Page Title="Recebimento de Mercadoria" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="estoque_mercadoria_recebimento.aspx.cs" Inherits="Relatorios.estoque_mercadoria_recebimento"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .jGrowl .manilla
        {
            background-color: #000;
            color: white;
        }
        .alinharCentro
        {
            text-align: center;
        }
    </style>
    <script type="text/javascript" src="http://code.jquery.com/jquery.js"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Controle de Estoque&nbsp;&nbsp;>&nbsp;&nbsp;Estoque
                    Loja&nbsp;&nbsp;>&nbsp;&nbsp;Recebimento de Mercadoria</span>
                <div style="float: right; padding: 0;">
                    <a href="estoque_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Recebimento de Mercadoria"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            Filial
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 256px;">
                            <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                Height="22px" Width="250px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvNotaMercadoria" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvNotaMercadoria_RowDataBound">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="EMISSAO" HeaderText="Data de Emissão" HeaderStyle-Width="250px"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="NUMERO_NF_TRANSFERENCIA" HeaderText="NF" HeaderStyle-Width="200px"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="SERIE_NF_ENTRADA" HeaderText="Série" HeaderStyle-Width="130px"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="FILIAL_ORIGEM" HeaderText="Empresa Emissora" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="QTDE_TOTAL" HeaderText="Quantidade Total" HeaderStyle-Width="150px"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Valor Nota" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValorNota" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Recebido Por" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtNome" runat="server" Width="150px" MaxLength="30"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button ID="btVerificarNota" runat="server" Text="Conferir" Width="100px" OnClick="btVerificarNota_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;
                            <asp:HiddenField ID="hidNF" runat="server" />
                            <asp:HiddenField ID="hidEmissao" runat="server" />
                            <asp:HiddenField ID="hidSerie" runat="server" />
                            <asp:HiddenField ID="hidEmpresaEmissora" runat="server" />
                            <asp:HiddenField ID="hidQtdeTotal" runat="server" />
                            <asp:HiddenField ID="hidNome" runat="server" />
                            <asp:HiddenField ID="hidRomaneio" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PRODUTO" HeaderText="Produto" HeaderStyle-Width="100px"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="DESC_PRODUTO" HeaderText="Descrição" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="350px" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="DESC_COR" HeaderText="Cor" HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="1px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hidCor" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="QTDE_ENTRADA" HeaderText="Quantidade" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="" HeaderStyle-Width="110px" />
                                        <asp:TemplateField HeaderText="EXP" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGradeEXP" runat="server" Width="50px" MaxLength="4" CssClass="alinharCentro"
                                                    onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="XP" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGradeXP" runat="server" Width="50px" MaxLength="4" CssClass="alinharCentro"
                                                    onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PP" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGradePP" runat="server" Width="50px" MaxLength="4" CssClass="alinharCentro"
                                                    onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGradeP" runat="server" Width="50px" MaxLength="4" CssClass="alinharCentro"
                                                    onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="M" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGradeM" runat="server" Width="50px" MaxLength="4" CssClass="alinharCentro"
                                                    onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="G" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGradeG" runat="server" Width="50px" MaxLength="4" CssClass="alinharCentro"
                                                    onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="GG" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGradeGG" runat="server" Width="50px" MaxLength="4" CssClass="alinharCentro"
                                                    onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OK" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkOK" runat="server" Checked="true" OnCheckedChanged="chkOK_CheckedChanged"
                                                    AutoPostBack="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Panel ID="pnlProdutoNovo" runat="server">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvProdutoNovo" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProdutoNovo_RowDataBound"
                                        ShowFooter="true" ShowHeader="false">
                                        <HeaderStyle BackColor="Gainsboro" />
                                        <FooterStyle BackColor="Gainsboro" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Produto" FooterStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labProduto" runat="server" Text='<%# Bind("PRODUTO") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtProduto" runat="server" Width="96px" CssClass="alinharCentro"
                                                        onkeypress="return fnValidarNumero(event);" OnTextChanged="txtProduto_TextChanged"
                                                        AutoPostBack="true"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descrição" FooterStyle-Width="350px" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDescricaoItem" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Literal ID="litDescricao" runat="server"></asp:Literal>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cor" FooterStyle-Width="180px" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litCor" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList ID="ddlCor" runat="server" Width="180px" DataValueField="COR_PRODUTO"
                                                        DataTextField="DESC_COR_PRODUTO">
                                                    </asp:DropDownList>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" FooterStyle-Width="1px" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hidCor" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" FooterStyle-Width="245px" ItemStyle-HorizontalAlign="Center">
                                                <FooterTemplate>
                                                    <asp:Literal ID="litEspaco" runat="server"></asp:Literal>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="EXP" FooterStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeEXP" runat="server" Text='<%# Bind("EXP_RECEBIDO") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtGradeEXP" runat="server" Width="50px" MaxLength="4" CssClass="alinharCentro"
                                                        onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="XP" FooterStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeXP" runat="server" Text='<%# Bind("XP_RECEBIDO") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtGradeXP" runat="server" Width="50px" MaxLength="4" CssClass="alinharCentro"
                                                        onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PP" FooterStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradePP" runat="server" Text='<%# Bind("PP_RECEBIDO") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtGradePP" runat="server" Width="50px" MaxLength="4" CssClass="alinharCentro"
                                                        onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P" FooterStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeP" runat="server" Text='<%# Bind("P_RECEBIDO") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtGradeP" runat="server" Width="50px" MaxLength="4" CssClass="alinharCentro"
                                                        onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="M" FooterStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeM" runat="server" Text='<%# Bind("M_RECEBIDO") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtGradeM" runat="server" Width="50px" MaxLength="4" CssClass="alinharCentro"
                                                        onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="G" FooterStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeG" runat="server" Text='<%# Bind("G_RECEBIDO") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtGradeG" runat="server" Width="50px" MaxLength="4" CssClass="alinharCentro"
                                                        onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="GG" FooterStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labGradeGG" runat="server" Text='<%# Bind("GG_RECEBIDO") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtGradeGG" runat="server" Width="50px" MaxLength="4" CssClass="alinharCentro"
                                                        onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" FooterStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Button ID="btExcluirProduto" runat="server" Text="Excluir" Width="100px" OnClick="btExcluirProduto_Click" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Button ID="btIncluirProduto" runat="server" Text="Incluir" Width="100px" OnClick="btIncluirProduto_Click" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align: right;">
                            <asp:Label ID="labBaixa" runat="server" Text="" ForeColor="Red"></asp:Label>
                            <asp:Button ID="btBaixarProduto" runat="server" Text="Baixar" Visible="false" Width="100px"
                                OnClientClick="return ConfirmarBaixa();" OnClick="btBaixarProduto_Click" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
