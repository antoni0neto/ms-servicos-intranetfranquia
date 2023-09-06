<%@ Page Title="Encaixe Lote" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="facc_encaixe_n.aspx.cs" Inherits="Relatorios.facc_encaixe_n" %>

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Facção&nbsp;&nbsp;>&nbsp;&nbsp;<asp:Label ID="labMeioTitulo" runat="server"></asp:Label>&nbsp;&nbsp;>&nbsp;&nbsp;<asp:Label ID="labTitulo" runat="server"></asp:Label></span>
                <div style="float: right; padding: 0;">
                    <a href="facc_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>
                        <asp:Label ID="labSubTitulo" runat="server"></asp:Label></legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="labFornecedor" runat="server" Text="Fornecedor"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labTipo" runat="server" Text="Setor"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labServico" runat="server" Text="Fase"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labPrecoCusto" runat="server" Text="Preço Custo"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labPrecoProducao" runat="server" Text="Preço Produção"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labVolume" runat="server" Text="Volume"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 306px;">
                                            <asp:DropDownList ID="ddlFornecedor" runat="server" Width="300px" Height="21px" DataTextField="FORNECEDOR"
                                                DataValueField="FORNECEDOR" OnSelectedIndexChanged="ddlFornecedor_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 120px;">
                                            <asp:DropDownList ID="ddlTipo" runat="server" Width="114px" Height="21px">
                                                <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                                <asp:ListItem Value="I" Text="INTERNO"></asp:ListItem>
                                                <asp:ListItem Value="E" Text="EXTERNO"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 267px;">
                                            <asp:DropDownList ID="ddlServico" runat="server" Width="261px" Height="21px" DataTextField="DESCRICAO"
                                                DataValueField="CODIGO">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 200px;">
                                            <asp:TextBox ID="txtPrecoCusto" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                                Width="190px"></asp:TextBox>
                                        </td>
                                        <td style="width: 130px;">
                                            <asp:TextBox ID="txtPrecoProducao" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                                Width="120px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtVolume" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                                Width="118px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <fieldset>
                        <legend>
                            <asp:Label ID="labTituloLote" runat="server" Text="Lote"></asp:Label>
                        </legend>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td>
                                    Coleção
                                    <asp:HiddenField ID="hidTela" runat="server" />
                                    <asp:HiddenField ID="hidCodigoUsuario" runat="server" />
                                </td>
                                <td>HB</td>
                                <td>Mostruário</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 250px;">
                                    <asp:DropDownList ID="ddlColecoes" runat="server" Width="244px" Height="21px" DataTextField="DESC_COLECAO"
                                        DataValueField="COLECAO">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 140px;">
                                    <asp:TextBox ID="txtHB" runat="server" Width="130px" MaxLength="5" CssClass="alinharDireita"
                                        onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                </td>
                                <td style="width: 65px; text-align: center">
                                    <asp:CheckBox ID="chkMostruario" runat="server" Checked="false" />
                                </td>
                                <td>&nbsp;<asp:Button ID="btInserir" runat="server" Text="Inserir" OnClick="btInserir_Click" Width="100px" OnClientClick="DesabilitarBotao(this);" />
                                    &nbsp;&nbsp;&nbsp;<asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvPrincipal" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvPrincipal_RowDataBound"
                                                            ShowFooter="true" DataKeyNames="CODIGO">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Coleção" HeaderStyle-Width="180px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColecao" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="HB" HeaderStyle-Width="100px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litHB" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Produto" HeaderStyle-Width="120px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litProduto" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Nome">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litNome" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Cor" HeaderStyle-Width="200px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litCor" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Quantidade" HeaderStyle-Width="120px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litQtde" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Mostruário" HeaderStyle-Width="125px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litMostruario" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="65px">
                                                                    <ItemTemplate>
                                                                        <asp:Button ID="btExcluir" runat="server" Text="Excluir" Height="21px" Width="65px" OnClick="btExcluir_Click" OnClientClick="return ConfirmarExclusao();" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <asp:Button ID="btEncaixar" runat="server" Text="Baixar" OnClick="btEncaixar_Click" OnClientClick="DesabilitarBotao(this);" />
                                &nbsp;&nbsp;<asp:Label ID="labErroEnxaixe" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
