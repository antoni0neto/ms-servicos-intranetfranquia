<%@ Page Title="Corte" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="prod_fila_corte.aspx.cs" Inherits="Relatorios.prod_fila_corte" %>

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Gestão
                    de Ordem de Corte&nbsp;&nbsp;>&nbsp;&nbsp;Corte</span>
                <div style="float: right; padding: 0;">
                    <a href="prod_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <table border="0" class="table">
                    <tr>
                        <td>
                            <fieldset style="padding-top: 0;">
                                <legend>Corte</legend>
                                <br />
                                <div id="accordionP">
                                    <h3>Principal</h3>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvPrincipal" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvPrincipal_RowDataBound"
                                                            OnDataBound="gvPrincipal_DataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btVoltarRisco" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/voltar.png"
                                                                            OnClick="btVoltarRisco_Click" OnClientClick="return Confirmar('Deseja relamente voltar este HB para o Risco?');" ToolTip="Voltar HB para o Risco" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Coleção" HeaderStyle-Width="130px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColecao" runat="server"></asp:Literal>
                                                                        <asp:HiddenField ID="hidPROD_DETALHE" runat="server" Value='<%# Eval("PROD_DETALHE") %>'></asp:HiddenField>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="HB" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="HB" HeaderStyle-Width="100px" />
                                                                <asp:BoundField DataField="GRUPO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Grupo" HeaderStyle-Width="200px" />
                                                                <asp:BoundField DataField="NOME" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Nome" HeaderStyle-Width="200px" />
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Quantidade" HeaderStyle-Width="120px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litQtde" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Data Inicial" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tempo" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litTempo" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Data Baixa" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="65px">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtDataBaixa" runat="server" Height="12px" onkeypress="return fnReadOnly(event);"
                                                                            MaxLength="10" Style="text-align: center;" Text='<%# DateTime.Now.ToShortDateString() %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
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
                                <div id="accordionD">
                                    <h3>Detalhes</h3>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvDetalhe" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvDetalhe_RowDataBound"
                                                            OnDataBound="gvDetalhe_DataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btVoltarRisco" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/voltar.png"
                                                                            OnClick="btVoltarRisco_Click" OnClientClick="return Confirmar('Deseja relamente voltar este HB para o Risco?');" ToolTip="Voltar HB para o Risco" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Coleção" HeaderStyle-Width="130px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColecao" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="HB" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="HB" HeaderStyle-Width="100px" />
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Detalhe" HeaderStyle-Width="80px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litDetalhe" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="GRUPO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Grupo" HeaderStyle-Width="117px" />
                                                                <asp:BoundField DataField="NOME" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Nome" HeaderStyle-Width="200px" />
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Quantidade" HeaderStyle-Width="120px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litQtde" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Data Inicial" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tempo" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litTempo" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Data Baixa" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="65px">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtDataBaixa" runat="server" Height="12px" onkeypress="return fnReadOnly(event);"
                                                                            MaxLength="10" Style="text-align: center;" Text='<%# DateTime.Now.ToShortDateString() %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="65px">
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
                                <%--<div id="accordionT">
                                    <h3>Produção</h3>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvCorte" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvCorte_RowDataBound"
                                                            OnDataBound="gvCorte_DataBound" ShowFooter="true"
                                                            OnSorting="gvCorte_Sorting" AllowSorting="true" DataKeyNames="CODIGO">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btVoltarRisco" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/voltar.png"
                                                                            OnClick="btVoltarRisco_Click" OnClientClick="return Confirmar('Deseja relamente voltar este HB para o Risco?');" ToolTip="Voltar HB para o Risco" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="DESC_COLECAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Coleção" HeaderStyle-Width="130px" SortExpression="DESC_COLECAO" />
                                                                <asp:BoundField DataField="HB" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="HB" HeaderStyle-Width="100px" SortExpression="HB" />
                                                                <asp:BoundField DataField="DESC_DETALHE" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Detalhe" HeaderStyle-Width="130px" SortExpression="DESC_DETALHE" />
                                                                <asp:BoundField DataField="GRUPO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Grupo" HeaderStyle-Width="180px" SortExpression="GRUPO" />
                                                                <asp:BoundField DataField="NOME" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Nome" HeaderStyle-Width="200px" SortExpression="NOME" />
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Quantidade" HeaderStyle-Width="105px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litQtde" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Data Inicial" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tempo" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litTempo" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Data Baixa" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="65px">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtDataBaixa" runat="server" Height="12px" onkeypress="return fnReadOnly(event);"
                                                                            MaxLength="10" Style="text-align: center;" Text='<%# DateTime.Now.ToShortDateString() %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
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
                                </div>--%>
                                <br />
                                <div style="float: right;">
                                    <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btImprimir" runat="server" Text="Imprimir" OnClick="btImprimir_Click"
                                        Width="100px" />
                                    <asp:HiddenField ID="hidOrdem" runat="server" Value="HB" />
                                    <asp:HiddenField ID="hidOrdenacao" runat="server" Value="ASC" />
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
