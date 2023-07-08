<%@ Page Title="Solicitar Tingimento" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="desenv_pedido_aviamento_tingirmat.aspx.cs" Inherits="Relatorios.desenv_pedido_aviamento_tingirmat" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
        }

        .alinharcentro {
            text-align: center;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../Image/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "../Image/plus.png");
            $(this).closest("tr").next().remove();
        });
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Aviamentos&nbsp;&nbsp;>&nbsp;&nbsp;Solicitar Tingimento</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Solicitar Tingimento</legend>
                    <br />
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>Material</td>
                            <td>Grupo Material</td>
                            <td>SubGrupo Material</td>
                            <td>Cor</td>
                            <td>Cor Fornecedor</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtMaterial" runat="server" Width="150px" OnTextChanged="txtMaterial_TextChanged" AutoPostBack="true"></asp:TextBox>
                            </td>
                            <td style="width: 230px;">
                                <asp:DropDownList ID="ddlMaterialGrupo" runat="server" Width="224px" Height="21px"
                                    DataTextField="GRUPO" DataValueField="GRUPO" OnSelectedIndexChanged="ddlMaterialGrupo_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 230px;">
                                <asp:DropDownList ID="ddlMaterialSubGrupo" runat="server" Width="224px" Height="21px"
                                    DataTextField="SUBGRUPO" DataValueField="SUBGRUPO" OnSelectedIndexChanged="ddlMaterialSubGrupo_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 230px;">
                                <asp:DropDownList ID="ddlCor" runat="server" Width="224px" Height="21px" DataTextField="DESC_COR" DataValueField="COR" OnSelectedIndexChanged="ddlCor_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 230px;">
                                <asp:DropDownList ID="ddlCorFornecedor" runat="server" Width="224px" Height="21px" DataTextField="REFER_FABRICANTE" DataValueField="REFER_FABRICANTE">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 130px;">
                                <asp:Button ID="btBuscar" runat="server" Width="122px" Text="Buscar" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Button ID="btCancelar" runat="server" Width="120px" Text="Cancelar" OnClick="btCancelar_Click" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">Estoque</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtEstoque" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="labErro" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <fieldset>
                                    <legend>HBs</legend>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td>Cor
                                            </td>
                                            <td>Status
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 241px;">
                                                <asp:DropDownList ID="ddlCorFiltro" runat="server" Width="241px" Height="21px" DataTextField="DESC_COR" DataValueField="COR" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlStatus" runat="server" Width="123px" Height="21px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                                    <asp:ListItem Value="S" Text="Aberto"></asp:ListItem>
                                                    <asp:ListItem Value="N" Text="Fechado"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvMaterial" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvMaterial_RowDataBound"
                                                        OnDataBound="gvMaterial_DataBound"
                                                        ShowFooter="true" DataKeyNames="CODIGO, MATERIAL, COR_MATERIAL">
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Coleção" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColecao" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="HB" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="HB" HeaderStyle-Width="85px" />
                                                            <asp:BoundField DataField="PRODUTO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Produto" HeaderStyle-Width="120px" />
                                                            <asp:BoundField DataField="NOME" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Nome" HeaderStyle-Width="200px" />
                                                            <asp:BoundField DataField="SUBGRUPO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="SubGrupo" HeaderStyle-Width="" />
                                                            <asp:BoundField DataField="DESC_COR" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Cor" HeaderStyle-Width="" />
                                                            <asp:BoundField DataField="COR_FORNECEDOR" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Cor Fornecedor" HeaderStyle-Width="" />
                                                            <asp:BoundField DataField="QTDE" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Qtde" HeaderStyle-Width="100px" />
                                                            <asp:TemplateField HeaderText="Qtde Total" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtQtde" runat="server" Width="100px" OnTextChanged="txtQtde_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Data Tingimento" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litDataTing" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="cbTingido" runat="server" Checked="false" OnCheckedChanged="cbTingido_CheckedChanged" AutoPostBack="true" />
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
                            <td colspan="7">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td>Qtde Selecionada</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="labErroMsg" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
                                        </td>
                                        <td style="width: 160px;">
                                            <asp:TextBox ID="txtQtdeSol" runat="server" Width="160px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 130px; text-align: right;">
                                            <asp:Button ID="btSalvar" runat="server" Text="Salvar" Width="124px" OnClick="btSalvar_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
