<%@ Page Title="Consulta de Pedidos de Acessórios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="desenv_acessorio_pedido_con.aspx.cs" Inherits="Relatorios.mod_desenvolvimento.desenv_acessorio_pedido_con" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .jGrowl .manilla {
            background-color: #000;
            color: white;
        }

        .alinharDireita {
            text-align: right;
        }
    </style>
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://code.jquery.com/jquery.js"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Planejamento de Coleção de Acessórios&nbsp;&nbsp;>&nbsp;&nbsp;Consulta de Pedidos</span>
        <div style="float: right; padding: 0;">
            <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <fieldset style="background-color: White;">
        <legend>
            <asp:Label ID="labTitulo" runat="server" Text="Filtrar Acessórios Com Pedidos"></asp:Label>
        </legend>
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td rowspan="8" width="200px">
                    <div style="width: 200px;" class="alinhamento">
                        <label>
                            Previsão de Entrega - Data Início:&nbsp;
                        </label>
                        <asp:TextBox ID="txtDtPrazoInicio" runat="server" CssClass="textEntry" Height="22px"
                            Width="198px"></asp:TextBox>
                        <asp:Calendar ID="CalendarDataInicio" runat="server"
                            CaptionAlign="Bottom" OnSelectionChanged="CalendarDataInicio_SelectionChanged"></asp:Calendar>
                    </div>
                </td>
                <td rowspan="8" width="200px">
                    <div style="width: 200px;" class="alinhamento">
                        <label>
                            Data Fim:&nbsp;
                        </label>
                        <asp:TextBox ID="txtDtPrazoFim" runat="server" CssClass="textEntry" Height="22px"
                            Width="198px"></asp:TextBox>
                        <asp:Calendar ID="CalendarDataFim" runat="server"
                            CaptionAlign="Bottom" OnSelectionChanged="CalendarDataFim_SelectionChanged"></asp:Calendar>
                    </div>
                </td>
                <td width="10px">&nbsp;</td>
                <td colspan="2" valign="bottom">
                    <asp:Label ID="labColecao" runat="server" Text="Coleção"></asp:Label>
                </td>
                <td width="210px" valign="bottom">
                    <asp:Label ID="labOrigem" runat="server" Text="Origem"></asp:Label>
                </td>
                <td width="210px">&nbsp;</td>
                <td width="100px">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td colspan="2">
                    <asp:DropDownList ID="ddlColecoesFiltro" runat="server" Width="200px" Height="21px" DataTextField="DESC_COLECAO"
                        DataValueField="COLECAO" AutoPostBack="true" OnSelectedIndexChanged="ddlColecoesFiltro_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddlOrigemFiltro" runat="server" Width="200px" Height="21px" DataTextField="DESCRICAO"
                        DataValueField="CODIGO" AutoPostBack="true" >
                    </asp:DropDownList>
                </td>
                <td align="right">
                    <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="95px" OnClientClick="DesabilitarBotao(this);" OnClick="btBuscar_Click" />&nbsp;&nbsp;
                    <asp:Button ID="btLimpar" runat="server" Text="Limpar" Width="95px" OnClick="btLimpar_Click" />&nbsp;&nbsp;
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="8">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Pedido"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label7" runat="server" Text="Produto"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Grupo"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Griffe"></asp:Label>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td width="105px" valign="top">
                    <asp:TextBox ID="txtPedido" runat="server" Width="90px" MaxLength="20"></asp:TextBox>
                </td>
                <td width="105px" valign="top">
                    <asp:TextBox ID="txtProduto" runat="server" Width="90px" MaxLength="20"></asp:TextBox>
                </td>
                <td valign="top">
                    <asp:DropDownList ID="ddlGrupoFiltro" runat="server" Width="200px" Height="21px" DataTextField="GRUPO_PRODUTO"
                        DataValueField="GRUPO_PRODUTO" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td valign="top">
                    <asp:DropDownList ID="ddlGriffeFiltro" runat="server" Width="200px" Height="21px" DataTextField="GRIFFE"
                        DataValueField="GRIFFE" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="8">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td colspan="2">
                    <asp:Label ID="Label4" runat="server" Text="Cor"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label5" runat="server" Text="Cor Fornecedor"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label6" runat="server" Text="Fornecedor"></asp:Label>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td colspan="2" valign="top">
                    <asp:DropDownList ID="ddlCorFiltro" runat="server" Width="200px" Height="21px" DataTextField="DESC_COR"
                        DataValueField="COR" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td valign="top">
                    <asp:DropDownList ID="ddlCorFornecedorFiltro" runat="server" Width="200px" Height="21px" DataTextField="COR_FORNECEDOR"
                        DataValueField="COR_FORNECEDOR" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td colspan="2" valign="top">
                    <asp:DropDownList ID="ddlFornecedorFiltro" runat="server" Width="300px" Height="21px" DataTextField="FORNECEDOR"
                        DataValueField="FORNECEDOR" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </fieldset>
    <asp:Panel ID="pnlTripa" runat="server" Visible="false">
        <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
                <fieldset style="margin-top: 0px; background-color: White;">
                    <legend>
                        <asp:Label ID="labAcao" runat="server" Text="">Listagem de Pedidos</asp:Label>
                    </legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="2350px" style="margin-top: 0px;">
                        <tr>
                            <td colspan="19">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvAcessorio" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" ShowFooter="true"
                                        DataKeyNames="CODIGO_DA" AllowSorting="true" OnRowDataBound="gvAcessorio_RowDataBound" OnSorting="gvAcessorio_Sorting" OnRowCommand="gvAcessorio_RowCommand">
                                        <HeaderStyle BackColor="Gainsboro" />
                                        <FooterStyle BackColor="Gainsboro" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Pedido" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px"
                                                SortExpression="PEDIDO">
                                                <ItemTemplate>
                                                    <%--<asp:LinkButton ID="lnkSel" runat="server" ForeColor="White">.</asp:LinkButton>--%>
                                                    <asp:Literal ID="litPedido" runat="server" Text='<%# Bind("PEDIDO") %>'></asp:Literal>&nbsp;&nbsp;
                                                    <asp:HiddenField ID="hidCodigoDAP" runat="server" Value='<%# Eval("CODIGO_DAP") %>'></asp:HiddenField>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Gerado" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px"
                                                SortExpression="DATA_PEDIDO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDataPedido" runat="server" Text='<%# Bind("DATA_PEDIDO") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Produto" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px"
                                                SortExpression="PRODUTO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litProduto" runat="server" Text='<%# Bind("PRODUTO") %>'></asp:Literal>&nbsp;&nbsp;
                                                    <asp:HiddenField ID="hidCodigoDA" runat="server" Value='<%# Eval("CODIGO_DA") %>'></asp:HiddenField>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Grupo" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px"
                                                SortExpression="GRUPO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litGrupo" runat="server" Text='<%# Bind("GRUPO") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Nome" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px"
                                                SortExpression="NOME">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNome" runat="server" Text='<%# Bind("NOME") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Origem" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px"
                                                SortExpression="DESCRICAO_ORIGEM">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litOrigem" runat="server" Text='<%# Bind("DESCRICAO_ORIGEM") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Griffe" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px"
                                                SortExpression="GRIFFE">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litGriffe" runat="server" Text='<%# Bind("GRIFFE") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cor" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px"
                                                SortExpression="DESC_COR">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litCor" runat="server" Text='<%# Bind("DESC_COR") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cor Fornecedor" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="150px" SortExpression="COR_FORNECEDOR">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litCorFornecedor" runat="server" Text='<%# Bind("COR_FORNECEDOR") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fornecedor" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="250px"
                                                SortExpression="FORNECEDOR">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litFornecedor" runat="server" Text='<%# Bind("FORNECEDOR") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="250px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descrição Sugerida" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="250px" SortExpression="DESCRICAO_SUGERIDA">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDescricaoSugerida" runat="server" Text='<%# Bind("DESCRICAO_SUGERIDA") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="250px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Previsão Entrega" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="120px"
                                                SortExpression="DATA_PREVISAO_ENTREGA">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPrevisaoEntrega" runat="server" Text='<%# Bind("DATA_PREVISAO_ENTREGA") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="120px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Preço" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="110px" SortExpression="PRECO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPreco" Text='<%# Bind("PRECO") %>' runat="server"></asp:Literal>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Custo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="110px" SortExpression="CUSTO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litCusto" Text='<%# Bind("CUSTO") %>' runat="server"></asp:Literal>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qtde.Original" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="120px" SortExpression="QTDE">
                                                <ItemTemplate>
                                                    <%--<asp:Literal ID="litQuantidade" Text='<%# Bind("QTDE") %>' runat="server"></asp:Literal>--%>
                                                    <asp:HiddenField ID="hidQuantidade" runat="server" Value='<%# Eval("QTDE") %>'></asp:HiddenField>
                                                    <asp:TextBox ID="txtQuantidade" runat="server" Text='<%# Bind("QTDE") %>' MaxLength="20" Width="75px" BorderStyle="None" style="text-align: right"></asp:TextBox>
                                                    <asp:Button ID="btnGerar" runat="server" Text="+" Width="25px" CssClass="buttonhbf" Style="padding-left: 0.3em; padding-top: -0.3em; font-size: 14px;" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Gerar_Pedido" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qtde.Entregar" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="110px" SortExpression="QTDE">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtdeEntregar" Text='<%# Bind("QTDE_ENTREGAR") %>' runat="server"></asp:Literal>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qtde.Entregue" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="110px" SortExpression="QTDE">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQtdeEntregue" Text='<%# Bind("QTDE_ENTREGUE") %>' runat="server"></asp:Literal>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="19">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="19">&nbsp;
                            </td>
                        </tr>
                    </table>
                </fieldset>

                <span id="popMensagem" style="position:fixed; top: 100px; left: 500px;">
                <div id="divMensagem" class="rounded_modals">
                    <asp:Panel ID="panMensagem" runat="server" width="505px" Height="200px" HorizontalAlign="left" Visible="false" BackColor="#EFF3FB" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="4px">
                        <table width="500px" cellpadding="0" cellspacing="0">
                            <tr style="height: 10px;">
                                <td colspan="2"></td>
                            </tr>
                            <tr>
                                <td width="18%" align="center" valign="top">
                                    <img src="/Image/warning.png" alt="Alerta" />
                                </td>
                                <td width="82%">
                                    <asp:TextBox ID="txtMensagemErro" runat="server" Text="" Width="400px" Height="150px" TextMode="MultiLine" ForeColor="Red" BorderStyle="None" ReadOnly="true" BackColor="#EFF3FB"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="height: 5px;">
                                <td colspan="2"></td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btnFecharMensagem" runat="server" width="80px" 
                                        Text="OK" cssclass="buttonhbf" OnClick="btnFecharMensagem_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                </span>

            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
