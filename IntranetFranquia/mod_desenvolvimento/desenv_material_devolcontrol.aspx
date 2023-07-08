<%@ Page Title="Controle de Devolução" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="desenv_material_devolcontrol.aspx.cs" Inherits="Relatorios.desenv_material_devolcontrol" %>

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Tecidos&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Devoluções</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Controle de Devolução</legend>
                    <br />
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">

                        <tr>
                            <td>Fornecedor</td>
                            <td>Coleção</td>
                            <td>Pedido</td>
                            <td>Grupo</td>
                            <td>SubGrupo</td>
                            <td>Status</td>
                        </tr>
                        <tr>
                            <td style="width: 300px;">
                                <asp:DropDownList ID="ddlFornecedor" runat="server" Width="294px" Height="21px"
                                    DataTextField="FORNECEDOR" DataValueField="FORNECEDOR">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 250px;">
                                <asp:DropDownList ID="ddlColecao" runat="server" Width="244px" Height="21px"
                                    DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 140px;">
                                <asp:TextBox ID="txtNumeroPedido" runat="server" Width="130px" CssClass="alinharDireita"></asp:TextBox>
                            </td>
                            <td style="width: 200px;">
                                <asp:DropDownList ID="ddlMaterialGrupo" runat="server" Width="194px" Height="21px"
                                    DataTextField="GRUPO" DataValueField="CODIGO_GRUPO" OnSelectedIndexChanged="ddlMaterialGrupo_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 230px;">
                                <asp:DropDownList ID="ddlMaterialSubGrupo" runat="server" Width="224px" Height="21px"
                                    DataTextField="SUBGRUPO" DataValueField="CODIGO_SUBGRUPO">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" Width="200px" Height="21px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="A" Text="Aguardando Aprovação"></asp:ListItem>
                                    <asp:ListItem Value="B" Text="Aguardando Emissão NF"></asp:ListItem>
                                    <asp:ListItem Value="C" Text="Aguardando Retirada"></asp:ListItem>
                                    <asp:ListItem Value="D" Text="Retirado"></asp:ListItem>
                                </asp:DropDownList>
                            </td>

                        </tr>
                        <tr>
                            <td colspan="6">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" OnClientClick="DesabilitarBotao(this);" />
                                &nbsp;&nbsp;<asp:Label ID="labMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvDevolucao" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvDevolucao_RowDataBound"
                                        OnDataBound="gvDevolucao_DataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fornecedor" ItemStyle-Width="" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labFornecedor" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Coleção" ItemStyle-Width="" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labColecao" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tecido" ItemStyle-Width="" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labTecido" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cor Fornecedor" ItemStyle-Width="" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labCorFornecedor" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qtde Devolvida" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labQtdeDev" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data Autorização" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labDataEntrada" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data Sol. Nota" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labDataNota" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Emissão" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labEmissao" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data Retirada" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labDataRetirada" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Obs" ItemStyle-Width="" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labObs" runat="server" Font-Size="Smaller" Text='<% #Bind("OBS")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labStatus" runat="server" Font-Size="Smaller"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btEntrarQtde" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/update.png"
                                                        OnClick="btEntrarQtde_Click" ToolTip="Receber Pedido" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
