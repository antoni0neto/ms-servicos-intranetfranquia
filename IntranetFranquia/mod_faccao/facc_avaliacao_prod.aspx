<%@ Page Title="Avaliação de Produção" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="facc_avaliacao_prod.aspx.cs" Inherits="Relatorios.facc_avaliacao_prod" %>

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Facção&nbsp;&nbsp;>&nbsp;&nbsp;<asp:Label ID="labMeioTitulo" runat="server" Text="Controle de Facção"></asp:Label>&nbsp;&nbsp;>&nbsp;&nbsp;<asp:Label ID="labTitulo" runat="server" Text="Avaliação de Produção"></asp:Label></span>
                <div style="float: right; padding: 0;">
                    <a href="facc_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>
                        <asp:Label ID="labSubTitulo" runat="server" Text="Avaliação de Produção"></asp:Label></legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>Fornecedor
                            </td>
                            <td>Coleção
                            </td>
                            <td>HB</td>
                            <td>Produto</td>
                            <td>Tem Entrada</td>
                            <td>Sem Avaliação</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 266px;">
                                <asp:DropDownList ID="ddlFornecedor" runat="server" Width="260px" Height="21px" DataTextField="FORNECEDOR"
                                    DataValueField="FORNECEDOR">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 250px;">
                                <asp:DropDownList ID="ddlColecoes" runat="server" Width="244px" Height="21px" DataTextField="DESC_COLECAO"
                                    DataValueField="COLECAO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 140px;">
                                <asp:TextBox ID="txtHB" runat="server" Width="130px" MaxLength="5" CssClass="alinharDireita"
                                    onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                            </td>
                            <td style="width: 140px;">
                                <asp:TextBox ID="txtProduto" runat="server" Width="130px" MaxLength="5" CssClass="alinharDireita"
                                    onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                            </td>
                            <td style="width: 130px;">
                                <asp:DropDownList ID="ddlTemEntrada" runat="server" Width="124px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: center; width: 90px;">
                                <asp:CheckBox ID="cbSemAvaliacao" runat="server" Checked="true" />
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" OnClientClick="DesabilitarBotao(this);" />
                                &nbsp;&nbsp;<asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <table border="0" cellpadding="0" class="tb" width="100%">
                                    <tr>
                                        <td style="width: 100%;">
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvAvaliacao" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvAvaliacao_RowDataBound" OnDataBound="gvAvaliacao_DataBound"
                                                    OnSorting="gvAvaliacao_Sorting" AllowSorting="true"
                                                    ShowFooter="true" DataKeyNames="CODIGO">
                                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" Font-Bold="true" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderText="Fornecedor" SortExpression="FORNECEDOR" ItemStyle-Font-Size="X-Small">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litFornecedor" runat="server" Text='<%# Bind("FORNECEDOR") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderText="Coleção" SortExpression="DESC_COLECAO">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColecao" runat="server" Text='<%# Bind("DESC_COLECAO") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="HB" HeaderStyle-Width="40px" SortExpression="HB">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litHB" runat="server" Text='<%# Bind("HB") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="Produto" HeaderStyle-Width="70px" SortExpression="PRODUTO">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litProduto" runat="server" Text='<%# Bind("PRODUTO") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderText="Nome" SortExpression="NOME" ItemStyle-Font-Size="X-Small">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litNome" runat="server" Text='<%# Bind("NOME") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderText="Cor" HeaderStyle-Width="120px" ItemStyle-Font-Size="X-Small">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litCor" runat="server" Text='<%# Bind("DESC_COR") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="Grade Total" HeaderStyle-Width="80px" SortExpression="GRADE_TOTAL">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litGrade" runat="server" Text='<%# Bind("GRADE_TOTAL") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="Envio" HeaderStyle-Width="80px">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litEnvio" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="Entrada" HeaderStyle-Width="80px">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litEntrada" runat="server" Text='<%# Bind("QTDE_FALTANTE") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="Nota 1" HeaderStyle-Width="60px" SortExpression="NOTA1">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litNota1" runat="server" Text='<%# Bind("NOTA1") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderText="Observação 1" HeaderStyle-Width="160px" ItemStyle-Font-Size="X-Small">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litOBS1" runat="server" Text='<%# Bind("OBS1") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="Nota 2" HeaderStyle-Width="60px" SortExpression="NOTA2">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litNota2" runat="server" Text='<%# Bind("NOTA2") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderText="Observação 2" HeaderStyle-Width="160px" ItemStyle-Font-Size="X-Small">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litOBS2" runat="server" Text='<%# Bind("OBS2") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            HeaderText="Avaliação" HeaderStyle-Width="85px">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litDataAvaliacao" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="55px">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btBaixar" runat="server" Text="Baixar" Height="21px" Width="55px" OnClick="btAvaliar_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
