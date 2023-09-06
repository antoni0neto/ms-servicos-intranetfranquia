<%@ Page Title="Mostruário" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="prod_fila_mostruario.aspx.cs" Inherits="Relatorios.prod_fila_mostruario" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table
        {
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Gestão
                    de Ordem de Corte&nbsp;&nbsp;>&nbsp;&nbsp;Mostruário</span>
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
                                <legend>Mostruário</legend>
                                <br />
                                <div id="accordionA">
                                    <h3>
                                        Ampliação</h3>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvAmpliacao" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvAmpliacao_RowDataBound"
                                                            OnDataBound="gvAmpliacao_DataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Coleção" HeaderStyle-Width="140px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColecao" runat="server"></asp:Literal>
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
                                                                        <asp:Literal ID="litQtde" runat="server" Text="0"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Data Inicial" HeaderStyle-Width="180px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litDataInicial" runat="server"></asp:Literal>
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
                                                                            MaxLength="10" Width="150px" Style="text-align: center;" Text='<%# DateTime.Now.ToShortDateString() %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                                    <ItemTemplate>
                                                                        <asp:Button ID="btRisco" runat="server" Text="Risco" Width="60px" Height="21px" OnClick="btRisco_Click" />
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
                                <div id="accordionR">
                                    <h3>
                                        Risco</h3>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvRisco" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvRisco_RowDataBound"
                                                            OnDataBound="gvRisco_DataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Coleção" HeaderStyle-Width="140px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColecao" runat="server"></asp:Literal>
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
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Data Inicial" HeaderStyle-Width="180px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litDataInicial" runat="server"></asp:Literal>
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
                                                                            MaxLength="10" Width="150px" Style="text-align: center;" Text='<%# DateTime.Now.ToShortDateString() %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                                    <ItemTemplate>
                                                                        <asp:Button ID="btBaixarRisco" runat="server" Text="Baixar" Width="60px" Height="21px"
                                                                            OnClick="btBaixarRisco_Click" />
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
                                <div id="accordionC">
                                    <h3>
                                        Corte</h3>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvCorte" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvCorte_RowDataBound"
                                                            OnDataBound="gvCorte_DataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server"></asp:Literal>
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
                                <br />
                                <div style="float: right;">
                                    <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btImprimir" runat="server" Text="Imprimir" OnClick="btImprimir_Click"
                                        Width="100px" />
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
