<%@ Page Title="Aprovação de Preço" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="prod_cad_produto_custo_aprov_novo.aspx.cs" Inherits="Relatorios.prod_cad_produto_custo_aprov_novo"
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
    <script src="../js/js.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Custo/Preço de Produto&nbsp;&nbsp;>&nbsp;&nbsp;Aprovação de Preço</span>
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
                        <tr style="height: 10px;">
                            <td colspan="7">
                                <hr />
                            </td>
                        </tr>
                    </table>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                        <asp:Panel ID="pnlCusto" runat="server" Visible="false">
                            <tr>
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
                                <td rowspan="8" style="text-align: right;">
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
                            <tr>
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
                                                    <asp:BoundField DataField="FORNECEDOR" HeaderStyle-Width="235px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Fornecedor" />
                                                    <asp:TemplateField HeaderText="Tecido" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTecido" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Detalhe" HeaderStyle-Width="115px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litDetalhe" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qtde" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litGrade" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ICMS (%)" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litICMS" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Preço" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litPrecoTecido" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cons. Custo" Visible="false" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litConsumoCusto" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cons. p/ Preço" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litConsumoPreco" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Custo" Visible="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTotalCusto" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total p/ Preço" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="125px">
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
                                                            <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Fornecedor" FooterStyle-Width="235px" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litFornecedor" Text='<%# Bind("FORNECEDOR") %>' runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tecido" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTecido" Text='<%# Bind("MATERIAL") %>' runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Detalhe" FooterStyle-Width="115px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litDetalhe" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Medida" Visible="false" FooterStyle-Width="130px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litMedida" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qtde" FooterStyle-Width="75px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litGrade" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ICMS (%)" FooterStyle-Width="75px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litICMS" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Preço" FooterStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litPrecoTecido" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cons. p/ Preço" FooterStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litConsumoPreco" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total p/ Preço" ItemStyle-HorizontalAlign="Center" FooterStyle-Width="125px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTotalPreco" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
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
                                                DataKeyNames="CODIGO">
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
                                                            <asp:Literal ID="litPreco" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Consumo" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litConsumo" runat="server"></asp:Literal>
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
                                                            <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Fornecedor" FooterStyle-Width="250px" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litFornecedor" Text='<%# Bind("FORNECEDOR") %>' runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Aviamento" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litAviamento" Text='<%# Bind("MATERIAL") %>' runat="server"></asp:Literal>
                                                        </ItemTemplate>
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
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Consumo" FooterStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litConsumo" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FooterStyle-Width="125px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
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
                                                            <asp:Literal ID="litFornecedor" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Serviço" ItemStyle-HorizontalAlign="Left"
                                                        HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litServico" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Valor Custo" Visible="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="125px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litValorServico" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Valor Preço" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="125px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litValorPreco" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                    <fieldset>
                                        <legend>Outros</legend>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvOutros" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvOutros_RowDataBound"
                                                OnDataBound="gvOutros_DataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Item" ItemStyle-HorizontalAlign="Left"
                                                        HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litItem" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Valor" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="125px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litValor" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="7">
                                    <fieldset>
                                        <legend>Simulação de Preço</legend>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="width: 65%;">&nbsp;</td>
                                                <td style="text-align: right;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvTotalItem" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvTotalItem_RowDataBound" OnDataBound="gvTotalItem_DataBound">
                                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Descrição" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litDesc" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Valor" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="198px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litValor" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
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
                                                                    HeaderText="Formação de Preço TS" HeaderStyle-Width="200px" ItemStyle-Font-Bold="true" />
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
                                <td colspan="7">
                                    <fieldset>
                                        <legend>Preço LINX</legend>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
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
                                <td colspan="5">&nbsp;
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
                                <td colspan="5">
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
                                <td colspan="7">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:Label ID="labErroValidacao" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                                <td valign="top" style="text-align: right;">
                                    <asp:Button runat="server" ID="btAprovar" Text="Aprovar" OnClick="btAprovar_Click"
                                        Width="115px" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;&nbsp;&nbsp;
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
