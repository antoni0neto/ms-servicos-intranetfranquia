<%@ Page Title="Controle de Produção" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="desenv_controle_producao.aspx.cs" Inherits="Relatorios.desenv_controle_producao" EnableEventValidation="false"
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
        <Triggers>
            <asp:PostBackTrigger ControlID="btExcel" />
        </Triggers>
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Produção</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Controle de Produção"></asp:Label></legend>
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
                        <td>Cor Fornecedor
                        </td>
                        <td>Tecido
                        </td>
                        <td>Griffe
                        </td>
                        <td>Fabricante
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 180px;" valign="top">
                            <asp:DropDownList ID="ddlColecoes" runat="server" Width="174px" Height="21px" DataTextField="DESC_COLECAO"
                                DataValueField="COLECAO" OnSelectedIndexChanged="ddlColecoes_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 160px;" valign="top">
                            <asp:DropDownList ID="ddlOrigem" runat="server" Width="154px" Height="21px" DataTextField="DESCRICAO"
                                DataValueField="CODIGO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 170px;" valign="top">
                            <asp:DropDownList ID="ddlGrupo" runat="server" Width="164px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                DataValueField="GRUPO_PRODUTO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 100px;" valign="top">
                            <asp:TextBox ID="txtProduto" runat="server" Width="90px" MaxLength="10"></asp:TextBox>
                        </td>
                        <td style="width: 160px;" valign="top">
                            <asp:TextBox ID="txtNome" runat="server" Width="150px" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width: 170px;" valign="top">
                            <asp:DropDownList ID="ddlCorFornecedor" runat="server" Width="164px" Height="21px" DataTextField="FORNECEDOR_COR"
                                DataValueField="FORNECEDOR_COR">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 170px;" valign="top">
                            <asp:DropDownList ID="ddlTecido" runat="server" Width="164px" Height="21px" DataTextField="TECIDO_POCKET"
                                DataValueField="TECIDO_POCKET">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 140px;">
                            <asp:DropDownList ID="ddlGriffe" runat="server" Width="134px" Height="21px" DataTextField="GRIFFE"
                                DataValueField="GRIFFE">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlFabricante" runat="server" Width="154px" Height="21px" DataTextField="FABRICANTE"
                                DataValueField="FABRICANTE">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Sem Corte</td>
                        <td colspan="8">
                        </td>
                    </tr>
                    <tr>
                        <td style="width:50px; text-align: left;">
                            <asp:CheckBox ID="cbSemCorte" runat="server" Text="" />
                        </td>
                        <td colspan="8">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="92px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;
                            <asp:Button ID="btExcel" runat="server" Width="92px" Text="Excel" OnClick="btExcel_Click" />&nbsp;
                            <asp:CheckBox ID="cbExcel" runat="server" OnCheckedChanged="cbExcel_CheckedChanged" AutoPostBack="true" />&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Font-Bold="true" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>Total de Modelos:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="labModeloFiltroValor"
                            ForeColor="Green" Font-Bold="true" runat="server" Text="0"></asp:Label>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td colspan="9">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>Total de SKUs:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                            ID="labEstiloFiltroValor" runat="server" ForeColor="Green" Font-Bold="true" Text="0"></asp:Label>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td colspan="9">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvControleProducao" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvControleProducao_RowDataBound" ShowFooter="true"
                                                AllowSorting="true" OnSorting="gvControleProducao_Sorting" DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                                <FooterStyle BackColor="GradientActiveCaption" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Produto" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                        SortExpression="PRODUTO">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litProduto" runat="server" Text='<%# Bind("PRODUTO")%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Nome" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left"
                                                        SortExpression="NOME">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litNome" runat="server" Text='<%# Bind("NOME")%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cor" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left"
                                                        SortExpression="COR">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litCor" runat="server" Text='<%# Bind("COR")%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cor Fornecedor" HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Left"
                                                        SortExpression="COR_FORNECEDOR">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litCorFornecedor" runat="server" Text='<%# Bind("COR_FORNECEDOR")%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tecido" HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Left"
                                                        SortExpression="TECIDO_POCKET">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTecido" runat="server" Text='<%# Bind("TECIDO_POCKET")%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Griffe" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left"
                                                        SortExpression="GRIFFE">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litGriffe" runat="server" Text='<%# Bind("GRIFFE")%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Fabricante" ItemStyle-HorizontalAlign="Left"
                                                        SortExpression="FABRICANTE">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litFabricante" runat="server" Text='<%# Bind("FABRICANTE")%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qtde Varejo" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litQtdeVarejo" runat="server" Text='<%# Bind("QTDE_VAREJO")%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qtde Atacado" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litQtdeAtacado" runat="server" Text='<%# Bind("QTDE_ATACADO")%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="HB" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                        SortExpression="HB">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litHB" runat="server" Text='<%# Bind("HB")%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
