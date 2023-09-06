<%@ Page Title="Aprovação de Preço Terceiros" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="prod_cad_produto_custo_aprovterceiro.aspx.cs" Inherits="Relatorios.prod_cad_produto_custo_aprovterceiro"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Custo/Preço de Produto&nbsp;&nbsp;>&nbsp;&nbsp;Aprovação de Preço Terceiros</span>
                <div style="float: right; padding: 0;">
                    <a href="prod_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1200px; margin-left: 8%;">
                <fieldset>
                    <legend>Aprovação de Preço</legend>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="tb">
                        <tr style="height: 10px; vertical-align: bottom;">
                            <td>Coleção
                            </td>
                            <td>Produto
                            </td>
                            <td>Nome
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 180px;">
                                <asp:DropDownList ID="ddlColecoes" runat="server" Width="174px" Height="21px" DataTextField="DESC_COLECAO"
                                    DataValueField="COLECAO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtProdutoLinxFiltro" runat="server" Text="" CssClass="alinharDireita"
                                    onkeypress="return fnValidarNumero(event);" Width="150px"></asp:TextBox>
                            </td>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtNomeFiltro" runat="server" Text=""
                                    Width="150px"></asp:TextBox>
                            </td>
                            <td style="width: 160px;">&nbsp;
                                <asp:Button runat="server" ID="btBuscar" Text="Buscar" OnClick="btBuscar_Click" Width="115px"
                                    Enabled="true" />
                            </td>
                            <td>&nbsp;
                            </td>
                            <td style="text-align: right;">
                                <asp:Button runat="server" ID="btDesaprovarCusto" Text="Desaprovar" OnClick="btDesaprovarCusto_Click" Width="115px"
                                    Enabled="true" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Label ID="labErro" runat="server" ForeColor="red" Text="&nbsp;">
                                </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">&nbsp;
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td colspan="6">
                                <hr />
                            </td>
                        </tr>
                    </table>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                        <asp:Panel ID="pnlCusto" runat="server" Visible="false">
                            <tr>
                                <td>Produto
                                </td>
                                <td>Grupo
                                </td>
                                <td>Nome
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                                <td rowspan="8" style="text-align: right;">
                                    <asp:Image ID="imgFotoPeca" Width='100px' Height='150px' runat="server" BorderColor="Black"
                                        BorderWidth="0" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 180px;">
                                    <asp:TextBox ID="txtProduto" runat="server" ReadOnly="true" Width="170px" CssClass="alinharDireita"></asp:TextBox>
                                </td>
                                <td style="width: 160px;">
                                    <asp:TextBox ID="txtGrupo" runat="server" ReadOnly="true" Width="150px"></asp:TextBox>
                                </td>
                                <td style="width: 160px;">
                                    <asp:TextBox ID="txtNome" runat="server" ReadOnly="true" Width="150px"></asp:TextBox>
                                </td>
                                <td style="width: 160px;"></td>
                                <td colspan="2" style="width: 180px;"></td>
                            </tr>
                            <tr>
                                <td colspan="6">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <fieldset>
                                        <legend>Aviamentos</legend>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvAviamentoNovo" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvAviamentoNovo_RowDataBound"
                                                OnDataBound="gvAviamentoNovo_DataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" FooterStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btExcluirAviamento" runat="server" Width="13px" ImageUrl="~/Image/delete.png"
                                                                ToolTip="Excluir Aviamento" OnClick="btExcluirAviamento_Click" OnClientClick="return ConfirmarExclusao();" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Aviamento" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litAviamento" Text='<%# Bind("MATERIAL") %>' runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtAviamento" runat="server" Width="410px" MaxLength="50"></asp:TextBox>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Preço" FooterStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litPreco" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtPreco" runat="server" Width="96px" Text="" Height="13px" onkeypress="return fnValidarNumeroDecimal(event);"
                                                                CssClass="alinharCentro"></asp:TextBox>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Consumo" FooterStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litConsumo" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtConsumo" runat="server" Width="96px" Text="" Height="13px" onkeypress="return fnValidarNumeroDecimal(event);"
                                                                CssClass="alinharCentro"></asp:TextBox>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FooterStyle-Width="125px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Button ID="btIncluirAviamento" runat="server" Width="125px" Text="Incluir"
                                                                OnClick="btIncluirAviamento_Click" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <div style="text-align: right;">
                                            <asp:Label ID="labErroAviamento" runat="server" ForeColor="Red" Text=""></asp:Label>&nbsp;
                                        </div>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <fieldset>
                                        <legend>Custo</legend>
                                        <table border="0" cellspacing="0" cellpadding="0" width="100%">
                                            <tr>
                                                <td>Valor
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 167px;">
                                                    <asp:TextBox ID="txtValorCusto" runat="server" Width="157px" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnCalcular" runat="server" Text="Calcular" Width="100px" OnClick="btnCalcular_Click" />&nbsp;
                                                    <asp:Label ID="labErroCalculo" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvCustoSimulacao" runat="server" Width="100%" AutoGenerateColumns="False" ShowHeader="true"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvCustoSimulacao_RowDataBound">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" Visible="false" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="COLUNA_NOME" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Formação de Preço TT" HeaderStyle-Width="200px" ItemStyle-Font-Bold="true" />
                                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColunaA" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColunaB" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColunaC" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColunaD" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColunaE" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <fieldset>
                                        <legend>Preço LINX</legend>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td colspan="3">Preço Sugerido
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtPrecoSugerido" runat="server" ReadOnly="true" Width="157px" CssClass="alinharDireita"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Preço TO
                                                </td>
                                                <td>Preço TA
                                                </td>
                                                <td>Preço TL
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 167px;">
                                                    <asp:TextBox ID="txtPrecoTO" runat="server" ReadOnly="true" Width="157px" CssClass="alinharDireita"></asp:TextBox>
                                                </td>
                                                <td style="width: 167px;">
                                                    <asp:TextBox ID="txtPrecoTA" runat="server" ReadOnly="true" Width="157px" CssClass="alinharDireita"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPrecoTL" runat="server" ReadOnly="true" Width="157px" CssClass="alinharDireita"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td>Preço Aprovado
                                </td>
                                <td>&nbsp;
                                </td>
                                <td colspan="4">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:TextBox ID="txtPrecoAprovado" runat="server" Width="170px" Text="" onkeypress="return fnValidarNumeroDecimal(event);"
                                        CssClass="alinharDireita"></asp:TextBox>
                                </td>
                                <td valign="top">
                                    <asp:Button runat="server" ID="btValidar" Text="Validar" OnClick="btValidar_Click" OnClientClick="DesabilitarBotao(this);"
                                        Width="115px" />&nbsp;&nbsp;
                                </td>
                                <td colspan="4">
                                    <div class="rounded_corners" id="divGridReal" runat="server">
                                        <asp:GridView ID="gvCustoReal" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvCustoReal_RowDataBound">
                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" Visible="false" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColunaNome" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColunaA" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr style="height: 10px;">
                                <td colspan="6">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="labErroValidacao" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                                <td valign="top" style="text-align: right;">
                                    <asp:Button runat="server" ID="btAprovar" Text="Aprovar" OnClick="btAprovar_Click"
                                        Width="115px" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr style="height: 10px;">
                            <td colspan="6">&nbsp;
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td colspan="6">&nbsp;
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td colspan="6">&nbsp;
                            </td>
                        </tr>
                    </table>
                </fieldset>
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
                                <strong>PRODUTO:
                                    <asp:Label ID="labProduto" runat="server" Text=""></asp:Label></strong>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; color: red;">
                                <asp:Label ID="labMensagem" runat="server" Text="PREÇO APROVADO COM SUCESSO."></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>Por favor, selecione a tela desejada nos botões abaixo.
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
