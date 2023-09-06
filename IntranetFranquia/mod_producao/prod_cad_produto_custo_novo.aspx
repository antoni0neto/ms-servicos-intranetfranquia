<%@ Page Title="Simulação de Custo/Preço" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="prod_cad_produto_custo_novo.aspx.cs" Inherits="Relatorios.prod_cad_produto_custo_novo"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Custo/Preço de Produto&nbsp;&nbsp;>&nbsp;&nbsp;Simulação de Custo/Preço</span>
                <div style="float: right; padding: 0;">
                    <a href="prod_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1200px; margin-left: 8%;">
                <fieldset>
                    <legend>Simulação de Custo/Preço</legend>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                        <tr style="height: 10px; vertical-align: bottom;">
                            <td>Coleção
                                <asp:HiddenField ID="hidCodigoHB" runat="server" />
                            </td>
                            <td>Produto
                            </td>
                            <td>Nome
                            </td>
                            <td>Mostruário
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
                            <td style="width: 65px; text-align: center">
                                <asp:CheckBox ID="chkMostruario" runat="server" Checked="false" />
                            </td>
                            <td style="width: 160px;">&nbsp;
                                <asp:Button runat="server" ID="btBuscar" Text="Buscar" OnClick="btBuscar_Click" Width="115px"
                                    Enabled="true" />&nbsp;&nbsp;
                            </td>
                            <td colspan="2">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="labErro" runat="server" ForeColor="red" Text="&nbsp;">
                                </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <hr />
                            </td>
                        </tr>
                    </table>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                        <asp:Panel ID="pnlCusto" runat="server" Visible="false">
                            <tr style="height: 10px; vertical-align: bottom;">
                                <td>HB
                                </td>
                                <td>Produto
                                </td>
                                <td>Grupo
                                </td>
                                <td>Nome
                                </td>
                                <td>Griffe
                                </td>
                                <td>Mostruário
                                </td>
                                <td rowspan="7" style="text-align: right;">
                                    <asp:Image ID="imgFotoPeca" Width='100px' Height='150px' runat="server" BorderColor="Black"
                                        BorderWidth="0" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 180px;">
                                    <asp:TextBox ID="txtHB" runat="server" ReadOnly="true" Width="170px" CssClass="alinharDireita"></asp:TextBox>
                                </td>
                                <td style="width: 160px;">
                                    <asp:TextBox ID="txtProduto" runat="server" ReadOnly="true" Width="150px" CssClass="alinharDireita"></asp:TextBox>
                                </td>
                                <td style="width: 160px;">
                                    <asp:TextBox ID="txtGrupo" runat="server" ReadOnly="true" Width="150px"></asp:TextBox>
                                </td>
                                <td style="width: 160px;">
                                    <asp:TextBox ID="txtNome" runat="server" ReadOnly="true" Width="150px"></asp:TextBox>
                                </td>
                                <td style="width: 160px;">
                                    <asp:TextBox ID="txtGriffe" runat="server" ReadOnly="true" Width="150px"></asp:TextBox>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtMostruario" runat="server" ReadOnly="true" Width="150px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">&nbsp;
                                </td>
                            </tr>

                            <tr style="height: 10px;">
                                <td colspan="7">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                    <fieldset>
                                        <legend>Tecidos</legend>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvTecido" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvTecido_RowDataBound">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Fornecedor" HeaderStyle-Width="174px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litFornecedor" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tecido" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTecido" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Detalhe" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litDetalhe" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qtde" HeaderStyle-Width="65px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litGrade" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ICMS (%)" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litICMS" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Preço" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litPrecoTecido" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cons. Custo" HeaderStyle-Width="95px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litConsumoCusto" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cons. p/ Preço" HeaderStyle-Width="95px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litConsumoPreco" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Custo" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTotalCusto" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total p/ Preço" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTotalPreco" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvTecidoNovo" runat="server" Width="100%" AutoGenerateColumns="False" ShowHeader="false"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvTecidoNovo_RowDataBound"
                                                OnDataBound="gvTecidoNovo_DataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" FooterStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btExcluirTecido" runat="server" Width="13px" ImageUrl="~/Image/delete.png"
                                                                ToolTip="Excluir Tecido" OnClick="btExcluirTecido_Click" OnClientClick="return ConfirmarExclusao();" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Fornecedor" FooterStyle-Width="174px" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litFornecedor" Text='<%# Bind("FORNECEDOR") %>' runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlFornecedor" runat="server" Width="174px" Height="21px" DataTextField="FORNECEDOR"
                                                                DataValueField="FORNECEDOR">
                                                            </asp:DropDownList>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tecido" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTecido" Text='<%# Bind("MATERIAL") %>' runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtTecido" runat="server" Width="176px" MaxLength="50"></asp:TextBox>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Detalhe" FooterStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litDetalhe" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Medida" Visible="false" FooterStyle-Width="130px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litMedida" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qtde" FooterStyle-Width="65px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litGrade" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ICMS (%)" FooterStyle-Width="75px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litICMS" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Preço" FooterStyle-Width="76px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litPrecoTecido" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtPreco" runat="server" CssClass="alinharCentro" onkeypress="return fnValidarNumeroDecimal(event);"
                                                                Width="76px"></asp:TextBox>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cons. Custo" FooterStyle-Width="91px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litConsumoCusto" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtConsumoCusto" runat="server" CssClass="alinharCentro" onkeypress="return fnValidarNumeroDecimal(event);"
                                                                Width="91px"></asp:TextBox>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cons. p/ Preço" FooterStyle-Width="91px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litConsumoPreco" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtConsumoPreco" runat="server" CssClass="alinharCentro" onkeypress="return fnValidarNumeroDecimal(event);"
                                                                Width="91px"></asp:TextBox>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Custo" ItemStyle-HorizontalAlign="Center" FooterStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTotalCusto" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total p/ Preço" ItemStyle-HorizontalAlign="Center" FooterStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTotalPreco" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Button ID="btIncluirTecido" runat="server" Width="90px" Text="Incluir" OnClick="btIncluirTecido_Click" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <div style="text-align: right;">
                                            <asp:Label ID="labErroTecido" runat="server" ForeColor="Red" Text=""></asp:Label>&nbsp;
                                        </div>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                    <fieldset>
                                        <legend>Aviamentos</legend>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvAviamento" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvAviamento_RowDataBound"
                                                ShowFooter="false" DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FORNECEDOR" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Fornecedor" HeaderStyle-Width="250px" />
                                                    <asp:BoundField DataField="AVIAMENTO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Aviamento" />
                                                    <asp:TemplateField HeaderText="ICMS (%)" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litICMS" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Preço" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtPreco" runat="server" Width="96px" Text="0" Height="13px" onkeypress="return fnValidarNumeroDecimal(event);"
                                                                CssClass="alinharCentro" OnTextChanged="txtPreco_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Consumo" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtConsumo" runat="server" Width="96px" Text="0" Height="13px" onkeypress="return fnValidarNumeroDecimal(event);"
                                                                CssClass="alinharCentro" OnTextChanged="txtConsumo_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="125px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvAviamentoNovo" runat="server" Width="100%" AutoGenerateColumns="False" ShowHeader="false"
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
                                                    <asp:TemplateField HeaderText="Fornecedor" FooterStyle-Width="250px" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litFornecedor" Text='<%# Bind("FORNECEDOR") %>' runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlFornecedor" runat="server" Width="250px" Height="21px" DataTextField="FORNECEDOR"
                                                                DataValueField="FORNECEDOR">
                                                            </asp:DropDownList>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Aviamento" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litAviamento" Text='<%# Bind("MATERIAL") %>' runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtAviamento" runat="server" Width="410px" MaxLength="50"></asp:TextBox>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ICMS (%)" FooterStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litICMS" runat="server"></asp:Literal>
                                                        </ItemTemplate>
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
                                <td colspan="7">
                                    <fieldset>
                                        <legend>Serviços</legend>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvServico" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvServico_RowDataBound"
                                                OnDataBound="gvServico_DataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Fornecedor" HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Left"
                                                        HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlFornecedor" runat="server" Width="250px" Height="21px" DataTextField="FORNECEDOR"
                                                                DataValueField="FORNECEDOR">
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Serviço" HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Left"
                                                        HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlServico" runat="server" Width="250px" Height="21px" DataTextField="DESCRICAO"
                                                                DataValueField="CODIGO">
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Valor Custo" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="162px">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtValorServico" runat="server" Width="158px" Text="0,00" onkeypress="return fnValidarNumeroDecimal(event);"
                                                                CssClass="alinharDireita" OnTextChanged="txtValorServico_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Valor p/ Preço" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="162px">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtValorPreco" runat="server" Width="158px" Text="0,00" onkeypress="return fnValidarNumeroDecimal(event);"
                                                                CssClass="alinharDireita" OnTextChanged="txtValorServico_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="" />
                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="75px">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btExcluir" runat="server" Text="Excluir" Width="75px" OnClick="btExcluir_Click" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <div>
                                            <fieldset id="fsFaccaoComRota" runat="server" style="margin-top: 3px; padding-top: 3px;">
                                                <legend>Rotas para o Controle de Facção</legend>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td style="text-align: center;">Estamparia
                                                        </td>
                                                        <td style="text-align: center;">Lavanderia
                                                        </td>
                                                        <td style="text-align: center;">Facção
                                                        </td>
                                                        <td style="text-align: center;">Acabamento
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: center;">
                                                            <asp:Label ID="labEstamparia" runat="server" ForeColor="blue" Text="NÃO"></asp:Label>
                                                        </td>
                                                        <td style="text-align: center;">
                                                            <asp:Label ID="labLavanderia" runat="server" ForeColor="blue" Text="NÃO"></asp:Label>
                                                        </td>
                                                        <td style="text-align: center;">
                                                            <asp:Label ID="labFaccao" runat="server" ForeColor="blue" Text="NÃO"></asp:Label>
                                                        </td>
                                                        <td style="text-align: center;">
                                                            <asp:Label ID="labAcabamento" runat="server" ForeColor="blue" Text="NÃO"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </div>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr style="height: 10px;">
                                <td>Etiquetas
                                </td>
                                <td>TAG
                                </td>
                                <td>Operacional (%)
                                </td>
                                <td colspan="4">&nbsp;
                                </td>
                            </tr>
                            <tr style="height: 10px;">
                                <td>
                                    <asp:TextBox ID="txtValorEtiqueta" runat="server" Width="170px" Text="" onkeypress="return fnValidarNumeroDecimal(event);"
                                        CssClass="alinharDireita"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTAG" runat="server" Width="150px" Text="" onkeypress="return fnValidarNumeroDecimal(event);"
                                        CssClass="alinharDireita"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOperacionalPorc" runat="server" Width="150px" Text="" Enabled="false" onkeypress="return fnValidarNumeroDecimal(event);"
                                        CssClass="alinharDireita"></asp:TextBox>
                                </td>
                                <td>&nbsp;
                                </td>
                                <td colspan="4" style="text-align: right;">
                                    <asp:Label ID="labCusto" runat="server" ForeColor="red" Text=""></asp:Label>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="chkFinalizado" runat="server" Text="Finalizado" TextAlign="Left" Checked="false" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btGerarCusto" runat="server" Text="Gerar Custo"
                                        Width="160px" OnClick="btGerarCusto_Click" OnClientClick="DesabilitarBotao(this);" />
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr style="height: 10px;">
                            <td colspan="7">&nbsp;
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td colspan="7">&nbsp;
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td colspan="7">&nbsp;
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
                            <td style="text-align: center; color: red;">SIMULAÇÃO DE CUSTO GERADA COM SUCESSO.
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
