<%@ Page Title="Liberar Produtos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="desenv_pocket_con_gemea.aspx.cs" Inherits="Relatorios.desenv_pocket_con_gemea"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Planejamento
                    de Coleção Nacional&nbsp;&nbsp;>&nbsp;&nbsp;Liberar Produtos</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Liberar Produtos"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 30px;">Coleção
                        </td>
                        <td>Origem
                        </td>
                        <td>Grupo
                        </td>
                        <td>Produto
                        </td>
                        <td>Nome
                        </td>
                        <td>Tecido
                        </td>
                        <td>Cor Fornecedor
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td style="text-align: right;">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 160px;" valign="top">
                            <asp:DropDownList ID="ddlColecoes" runat="server" Width="154px" Height="21px" DataTextField="DESC_COLECAO"
                                DataValueField="COLECAO" OnSelectedIndexChanged="ddlColecoes_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 180px;" valign="top">
                            <asp:DropDownList ID="ddlOrigem" runat="server" Width="174px" Height="21px" DataTextField="DESCRICAO"
                                DataValueField="CODIGO" OnSelectedIndexChanged="ddlOrigem_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 170px;" valign="top">
                            <asp:DropDownList ID="ddlGrupo" runat="server" Width="164px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                DataValueField="GRUPO_PRODUTO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 130px;" valign="top">
                            <asp:TextBox ID="txtModelo" runat="server" Width="120px" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width: 170px;" valign="top">
                            <asp:TextBox ID="txtNome" runat="server" Width="160px" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width: 210px;" valign="top">
                            <asp:DropDownList ID="ddlTecido" runat="server" Width="204px" Height="21px" DataTextField="TECIDO_POCKET"
                                DataValueField="TECIDO_POCKET">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 210px;" valign="top">
                            <asp:DropDownList ID="ddlCorFornecedor" runat="server" Width="204px" Height="21px"
                                DataTextField="FORNECEDOR_COR" DataValueField="FORNECEDOR_COR">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 60px;" valign="top">
                            <asp:Button ID="btAdicionar" runat="server" Width="50px" Text=">>" OnClick="btAdicionar_Click" OnClientClick="DesabilitarBotao(this);" />
                        </td>
                        <td>&nbsp;
                        </td>
                        <td style="width: 140px; text-align: right;">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>Griffe
                        </td>
                        <td>Produto Acabado
                        </td>
                        <td>Signed
                        </td>
                        <td colspan="2">Signed Nome
                        </td>
                        <td colspan="5">Liberado
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlGriffe" runat="server" Width="154px" Height="21px" DataTextField="GRIFFE"
                                DataValueField="GRIFFE">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlProdutoAcabado" runat="server" Width="174px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSigned" runat="server" Width="164px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="ddlSignedNome" runat="server" Width="294px" Height="21px" DataTextField="SIGNED_NOME"
                                DataValueField="SIGNED_NOME">
                            </asp:DropDownList>
                        </td>
                        <td colspan="5">
                            <asp:DropDownList ID="ddlLiberado" runat="server" Width="204px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10">&nbsp;
                            <asp:Label ID="labBuscar" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10">Modelos Selecionados
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:ListBox ID="lstModeloSelecionado" runat="server" SelectionMode="Multiple" Width="503px"
                                Height="100px"></asp:ListBox>
                        </td>
                        <td valign="bottom">
                            <asp:Button ID="btExcluir" runat="server" Width="100px" Text="Excluir" OnClick="btExcluir_Click" /><br />
                            <br />
                            <asp:Button ID="btLimpar" runat="server" Width="100px" Text="Limpar" OnClick="btLimpar_Click" />
                        </td>
                        <td colspan="6">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">&nbsp;
                        </td>
                        <td style="text-align: left;">&nbsp;
                        </td>
                    </tr>
                    <tr style="line-height: 40px;">
                        <td colspan="9">
                            <asp:CheckBox ID="chkGrupoOrder" runat="server" Checked="false" Text="Ordenar por Grupo" />
                            &nbsp;&nbsp;
                            <asp:CheckBox ID="chkTecidoOrder" runat="server" Checked="false" Text="Ordenar por Tecido" />
                        </td>
                        <td style="text-align: left;">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td >
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;&nbsp;
                            <asp:HiddenField ID="hidTela" runat="server" />
                        </td>
                        <td colspan="9"style="text-align: right;">&nbsp;
                            <asp:Label ID="labImpressao" runat="server" ForeColor="Red" Text=""></asp:Label>
                            <asp:Button ID="btImprimir" runat="server" Text="Imprimir" Width="100px" OnClick="btImprimir_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10">&nbsp;
                        </td>
                    </tr>
                    <tr style="line-height: 20px;">
                        <td colspan="10">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10" style="text-align: left; line-height: 22px;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="cbLiberarTodos" runat="server" OnCheckedChanged="cbLiberarTodos_CheckedChanged" AutoPostBack="true" Checked="false" Text="Marcar Todos" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound"
                                    DataKeyNames="COLECAO, MODELO">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbLiberar" runat="server" Checked="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <table border='0' cellpadding='0' cellspacing='0' width='950px'>
                                                    <tr>
                                                        <td colspan='2' style='border-bottom: 1px solid #696969; line-height: 26px;'>&nbsp;&nbsp;
                                                            <asp:Label ID="labOrigem" runat="server" ForeColor="Red" Font-Bold="true" Text=""></asp:Label>
                                                            <asp:Label ID="labNomeProduto" runat="server" Font-Bold="true" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="text-align: left;">
                                                        <td style="width: 200px;">
                                                            <div id="divFoto" style='width: 300px; height: 150px; display: table-cell; vertical-align: middle; text-align: center;'>
                                                                <asp:Image ID="imgProduto" runat="server" Height="150px" />
                                                            </div>
                                                        </td>
                                                        <td valign="top">
                                                            <asp:GridView ID="gvProdutoCor" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvProdutoCor_RowDataBound">
                                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Cor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litCor" runat="server" Text=''></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Atacado" HeaderStyle-Width="140px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litAtacado" runat="server" Text=''></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Varejo" HeaderStyle-Width="140px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litVarejo" runat="server" Text=''></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Corte Mostruário" HeaderStyle-Width="140px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litMostruario" runat="server" Text='-'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
