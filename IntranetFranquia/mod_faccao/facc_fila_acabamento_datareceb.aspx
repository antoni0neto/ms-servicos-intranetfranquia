<%@ Page Title="Acabamento Diário - Recebimento" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="facc_fila_acabamento_datareceb.aspx.cs" Inherits="Relatorios.facc_fila_acabamento_datareceb" %>

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
    <script type="text/javascript" language="javascript">
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Facção&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Acabamento Diário Data Recebimento</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Acabamento Diário - Recebimento</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>Data
                            </td>
                            <td>Produção
                            </td>
                            <td>Mostruário
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtDataPesquisa" runat="server" Width="150px" onkeypress="return fnValidarData(event);"
                                    MaxLength="10" Style="text-align: right;" />
                            </td>
                            <td style="width: 170px;">
                                <asp:DropDownList ID="ddlFornecedor" runat="server" Width="164px" Height="21px">
                                    <asp:ListItem Value="" Text="" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Somente Handbook"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Remover Handbook"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 65px; text-align: center;">
                                <asp:CheckBox ID="chkMostruario" runat="server" Checked="false" />
                            </td>
                            <td>&nbsp;
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" Width="100px" />
                                &nbsp;&nbsp;<asp:Label ID="labMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div id="accordionR">
                                    <h3>Entrada Acabamento</h3>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvRecebido" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvRecebido_RowDataBound"
                                                            OnDataBound="gvRecebido_DataBound" OnSorting="gvRecebido_Sorting" AllowSorting="true" ShowFooter="true"
                                                            DataKeyNames="CODIGO_ENTRADA_QTDE">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                            <RowStyle Font-Size="Smaller" />
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Fornecedor" HeaderStyle-Width="200px" SortExpression="FORNECEDOR" ItemStyle-Font-Size="">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litFornecedor" runat="server" Text='<%# Bind("FORNECEDOR") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Coleção" HeaderStyle-Width="130px" SortExpression="COLECAO" ItemStyle-Font-Size="">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColecao" runat="server" Text='<%# Bind("DESC_COLECAO") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="HB" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="HB" HeaderStyle-Width="100px" SortExpression="HB" ItemStyle-Font-Size="" />
                                                                <asp:BoundField DataField="PRODUTO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Produto" HeaderStyle-Width="100px" SortExpression="PRODUTO" />
                                                                <asp:BoundField DataField="NOME" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Nome" HeaderStyle-Width="150px" SortExpression="NOME" ItemStyle-Font-Size="" />
                                                                <asp:BoundField DataField="DESC_COR" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Cor" HeaderStyle-Width="150px" SortExpression="DESC_COR" ItemStyle-Font-Size="" />

                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Tipo" HeaderStyle-Width="140px" SortExpression="TIPO">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litTipo" runat="server" Text='<%# Bind("TIPO") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>


                                                                <%--<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="EXP" HeaderStyle-Width="140px" SortExpression="GRADE_EXP">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litQtdeEXP" runat="server" Text='<%# Bind("GRADE_EXP") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="XP" HeaderStyle-Width="140px" SortExpression="GRADE_XP">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litQtdeXP" runat="server" Text='<%# Bind("GRADE_XP") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="PP" HeaderStyle-Width="140px" SortExpression="GRADE_PP">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litQtdePP" runat="server" Text='<%# Bind("GRADE_PP") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="P" HeaderStyle-Width="140px" SortExpression="GRADE_P">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litQtdeP" runat="server" Text='<%# Bind("GRADE_P") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="M" HeaderStyle-Width="140px" SortExpression="GRADE_M">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litQtdeM" runat="server" Text='<%# Bind("GRADE_M") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="G" HeaderStyle-Width="140px" SortExpression="GRADE_G">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litQtdeG" runat="server" Text='<%# Bind("GRADE_G") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="GG" HeaderStyle-Width="140px" SortExpression="GRADE_GG">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litQtdeGG" runat="server" Text='<%# Bind("GRADE_GG") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Total" HeaderStyle-Width="140px" SortExpression="GRADE_TOTAL">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litQtdeTotal" runat="server" Text='<%# Bind("GRADE_TOTAL") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

<%--                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Linx" HeaderStyle-Width="80px">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="cbLinx" runat="server" Checked="false" OnCheckedChanged="cbLinx_CheckedChanged" AutoPostBack="true" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>

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
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btImprimir" runat="server" Text="Imprimir" OnClick="btImprimir_Click"
                                        Width="100px" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
