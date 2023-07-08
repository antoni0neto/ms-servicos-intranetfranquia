<%@ Page Title="Geração de Pedidos de Acessórios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="desenv_acessorio_pedido.aspx.cs" Inherits="Relatorios.mod_desenvolvimento.desenv_acessorio_pedido" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>
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
    <script type="text/javascript" src="http://code.jquery.com/jquery.js"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Planejamento de Coleção de Acessórios&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Pedidos</span>
        <div style="float: right; padding: 0;">
            <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <fieldset style="background-color: White;">
        <legend>
            <asp:Label ID="labTitulo" runat="server" Text="Filtrar Acessórios Sem Pedidos"></asp:Label>
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
                <td width="210px" valign="bottom">
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
                <td>
                    <asp:DropDownList ID="ddlColecoesFiltro" runat="server" Width="200px" Height="21px" DataTextField="DESC_COLECAO"
                        DataValueField="COLECAO" AutoPostBack="true" OnSelectedIndexChanged="ddlColecoesFiltro_SelectedIndexChanged" >
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
                <td colspan="7">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Produto"></asp:Label>
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
                <td valign="top">
                    <asp:TextBox ID="txtProduto" runat="server" Width="200px" MaxLength="20"></asp:TextBox>
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
                <td colspan="7">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
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
                <td valign="top">
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
                        <asp:Label ID="labAcao" runat="server" Text="">Geração de Pedidos</asp:Label>
                    </legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0" style="margin-top: 0px;">
                        <tr>
                            <td>
                                <asp:Button ID="btnGerarPedido" runat="server" Text="Gerar Pedido" Width="110px" OnClick="btnGerarPedido_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="labSalvar" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <hr />
                            </td>
                        </tr>
                    </table>
                    <table border="0" cellpadding="0" cellspacing="0" width="1860px" style="margin-top: 0px;">
                        <tr>
                            <td colspan="19">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvAcessorio" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" ShowFooter="true"
                                        DataKeyNames="CODIGO" AllowSorting="true" OnRowDataBound="gvAcessorio_RowDataBound" OnSorting="gvAcessorio_Sorting" OnRowCommand="gvAcessorio_RowCommand">
                                        <HeaderStyle BackColor="GradientActiveCaption" />
                                        <FooterStyle BackColor="GradientActiveCaption" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkTodos" runat="server" AutoPostBack="true" OnCheckedChanged="chkTodos_CheckedChanged" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkGerarPedido" runat="server" OnCheckedChanged="chkGerarPedido_CheckedChanged" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="30px" />
                                                <ItemStyle BorderWidth="1px" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Produto" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px"
                                                SortExpression="PRODUTO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litProduto" runat="server" Text='<%# Bind("PRODUTO") %>'></asp:Literal>&nbsp;&nbsp;
                                                    <asp:HiddenField ID="hidCodigo" runat="server" Value='<%# Eval("CODIGO") %>'></asp:HiddenField>
                                                    <asp:Button ID="btnErros" runat="server" Text="Erro" Width="30px" CssClass="buttonhbf" Style="padding-left: 0.3em;" Visible="false" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Ver_Erros" />
                                                    <asp:HiddenField ID="hidErros" runat="server"></asp:HiddenField>
                                                    <asp:LinkButton ID="lnkSel" runat="server" ForeColor="White" Font-Size="2px">.</asp:LinkButton>
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
                                            <asp:TemplateField HeaderText="Quantidade" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="110px" SortExpression="QTDE">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litQuantidade" Text='<%# Bind("QTDE") %>' runat="server"></asp:Literal>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descrição Sugerida" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="250px" SortExpression="DESCRICAO_SUGERIDA">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDescricaoSugerida" runat="server" Text='<%# Bind("DESCRICAO_SUGERIDA") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="250px" />
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
