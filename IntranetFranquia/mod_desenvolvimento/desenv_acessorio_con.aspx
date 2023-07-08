<%@ Page Title="Consultar Coleção Acessórios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="desenv_acessorio_con.aspx.cs" Inherits="Relatorios.desenv_acessorio_con"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Planejamento de Coleção de Acessórios&nbsp;&nbsp;>&nbsp;&nbsp;Consultar Coleção Acessórios</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Consultar Coleção Acessórios"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Coleção
                        </td>
                        <td>Origem
                        </td>
                        <td>Grupo
                        </td>
                        <td>Produto
                        </td>
                        <td>Nome
                        </td>
                        <td>Fornecedor
                        </td>
                        <td>Griffe
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
                            <asp:TextBox ID="txtProduto" runat="server" Width="120px" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width: 170px;" valign="top">
                            <asp:TextBox ID="txtNome" runat="server" Width="160px" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width: 220px;" valign="top">
                            <asp:DropDownList ID="ddlFornecedor" runat="server" Width="214px" Height="21px"
                                DataTextField="FORNECEDOR" DataValueField="FORNECEDOR">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 200px;" valign="top">
                            <asp:DropDownList ID="ddlGriffe" runat="server" Width="194px" Height="21px" DataTextField="GRIFFE"
                                DataValueField="GRIFFE">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 60px;" valign="top">
                            <asp:Button ID="btAdicionar" runat="server" Width="50px" Text=">>" OnClick="btAdicionar_Click" OnClientClick="DesabilitarBotao(this);" />
                        </td>
                        <td>&nbsp;
                        </td>
                        <td style="width: 140px; text-align: right;">
                            <asp:CheckBox ID="chkMultiplo" runat="server" Text="Múltiplos Modelos" Checked="false" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10"></td>
                    </tr>
                    <tr>
                        <td colspan="10">&nbsp;
                            <asp:Label ID="labBuscar" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10">Produtos Selecionados
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:ListBox ID="lstProdutoSelecionado" runat="server" SelectionMode="Multiple" Width="503px"
                                Height="100px"></asp:ListBox>
                        </td>
                        <td valign="bottom">
                            <asp:Button ID="btExcluir" runat="server" Width="100px" Height="22px" Text="Excluir" OnClick="btExcluir_Click" /><br />
                            <br />
                            <asp:Button ID="btLimpar" runat="server" Width="100px"  Height="22px" Text="Limpar" OnClick="btLimpar_Click" />
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
                        </td>
                        <td style="text-align: left;">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;&nbsp;
                            <asp:HiddenField ID="hidTela" runat="server" />
                        </td>
                        <td style="text-align: right;">
                            <asp:Button ID="btImprimir" runat="server" Text="Imprimir" Width="100px" OnClick="btImprimir_Click" OnClientClick="DesabilitarBotao(this);" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10">
                            <hr />
                        </td>
                    </tr>
                    <tr style="line-height: 20px;">
                        <td colspan="10">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10">
                            <asp:Panel ID="pnlPocket" runat="server" BackColor="White" BorderWidth="1" BorderColor="White"
                                Height="100%">
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
