<%@ Page Title="Receber Material Tingido" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="desenv_pedido_aviamento_tingirmat_receb.aspx.cs" Inherits="Relatorios.desenv_pedido_aviamento_tingirmat_receb" %>

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Aviamentos&nbsp;&nbsp;>&nbsp;&nbsp;Receber Material Tingido</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Receber Material Tingido</legend>
                    <br />
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>Material</td>
                            <td>Grupo Material</td>
                            <td>SubGrupo Material</td>
                            <td>Cor</td>
                            <td>Cor Fornecedor</td>
                            <td>Aberto</td>
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
                            <td>
                                <asp:DropDownList ID="ddlAberto" runat="server" Width="150px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 130px;">
                                <asp:Button ID="btBuscar" runat="server" Width="122px" Text="Buscar" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />
                            </td>
                            <td style="text-align: right;">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <asp:Label ID="labErro" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td colspan="2">
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvMaterial" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvMaterial_RowDataBound"
                                                    OnDataBound="gvMaterial_DataBound"
                                                    ShowFooter="true" DataKeyNames="CODIGO_TING, MATERIAL, COR_MATERIAL">
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
                                                        <asp:BoundField DataField="QTDE_SOL" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderText="Qtde Enviada" HeaderStyle-Width="100px" />
                                                        <asp:TemplateField HeaderText="Data Envio" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litDataEnvio" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qtde Recebida" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtQtde" runat="server" Width="100px" OnTextChanged="txtQtde_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Data Recebimento" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litDataReceb" runat="server"></asp:Literal>
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
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="8">
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
