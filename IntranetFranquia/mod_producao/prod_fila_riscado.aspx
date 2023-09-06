<%@ Page Title="Riscado" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="prod_fila_riscado.aspx.cs" Inherits="Relatorios.prod_fila_riscado" %>

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Gestão
                    de Ordem de Corte&nbsp;&nbsp;>&nbsp;&nbsp;Riscado</span>
                <div style="float: right; padding: 0;">
                    <a href="prod_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Riscado</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>Data
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
                            <td style="width: 65px; text-align: center;">
                                <asp:CheckBox ID="chkMostruario" runat="server" Checked="false" />
                            </td>
                            <td>&nbsp;
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" />
                                &nbsp;&nbsp;<asp:Label ID="labMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
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
                                                                        <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Coleção" HeaderStyle-Width="130px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColecao" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Tipo" HeaderStyle-Width="130px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litTipo" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="HB" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="HB" HeaderStyle-Width="100px" />
                                                                <asp:BoundField DataField="GRUPO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Grupo" HeaderStyle-Width="280px" />
                                                                <asp:BoundField DataField="NOME" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Nome" HeaderStyle-Width="150px" />
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Quantidade" HeaderStyle-Width="140px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litQtde" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Data Inicial" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litDataInicial" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Data Final" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="160px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litDataFinal" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tempo de Produção" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litTempo" runat="server"></asp:Literal>
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
                                                                        <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Coleção" HeaderStyle-Width="130px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColecao" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Tipo" HeaderStyle-Width="130px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litTipo" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="HB" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="HB" HeaderStyle-Width="100px" />
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Detalhe" HeaderStyle-Width="130px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litDetalhe" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="GRUPO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Grupo" HeaderStyle-Width="150px" />
                                                                <asp:BoundField DataField="NOME" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Nome" HeaderStyle-Width="150px" />
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                    HeaderText="Quantidade" HeaderStyle-Width="140px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litQtde" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Data Inicial" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litDataInicial" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Data Final" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="160px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litDataFinal" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tempo de Produção" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litTempo" runat="server"></asp:Literal>
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
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
