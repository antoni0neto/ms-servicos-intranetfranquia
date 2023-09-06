<%@ Page Title="Produtos Relacionados" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ecom_cad_produto_relacionamento_edit.aspx.cs" Inherits="Relatorios.ecom_cad_produto_relacionamento_edit" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Produtos&nbsp;&nbsp;>&nbsp;&nbsp;Produtos Relacionados</span>
                <div style="float: right; padding: 0;">
                    <a href="ecom_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Produtos Relacionados</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="text-align: center">
                                <asp:Label ID="labProduto" runat="server" Font-Size="Large" Font-Bold="true" Text=""></asp:Label>
                            </td>
                            <td style="text-align: center">
                                <asp:HiddenField ID="hidTipoLook" runat="server" Value="" />
                                <asp:HiddenField ID="hidGriffe" runat="server" Value="" />
                                <asp:HiddenField ID="hidColecao" runat="server" Value="" />
                                <asp:DropDownList ID="ddlLook" runat="server" Width="100px" OnSelectedIndexChanged="ddlLook_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="up_sell" Text="2 Produtos"></asp:ListItem>
                                    <asp:ListItem Value="related" Text="Relacionados"></asp:ListItem>
                                </asp:DropDownList>&nbsp;&nbsp;
                                <asp:Label ID="labProdutoLook" runat="server" Font-Size="Large" Font-Bold="true" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center">
                                <asp:Image ID="imgProduto" runat="server" Height="390px" />
                            </td>
                            <td style="text-align: center">
                                <asp:Image ID="imgProdutoLook" runat="server" Height="390px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:HiddenField ID="hidCodEcomProduto" runat="server" Value="" />
                            </td>
                            <td>
                                <asp:HiddenField ID="hidCodEcomProdutoLook" runat="server" Value="" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvProdutoRelacionado" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProdutoRelacionado_RowDataBound"
                                        OnDataBound="gvProdutoRelacionado_DataBound" ShowFooter="true" BorderWidth="0px">
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="labColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Image ID="imgProduto" runat="server" Width="80px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PRODUTO" HeaderText="Produto" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="NOME" HeaderText="Nome" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="DESC_COR" HeaderText="Cor" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderText="Tipo" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="labTipo" runat="server" Text=''></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr />
                            </td>
                        </tr>
                    </table>


                    <div>
                        <fieldset>
                            <legend>Tecido/Cor</legend>
                            <div id="accordionTC">
                                <h3>Abrir</h3>
                                <div>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvTecidoCor" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound"
                                                        OnDataBound="gvProduto_DataBound" ShowFooter="true" BorderWidth="0px" ShowHeader="false">
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" ItemStyle-Width="33%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="0px">
                                                                <ItemTemplate>
                                                                    <asp:Panel ID="pnl1" runat="server" BorderWidth="1">
                                                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                                                            <tr>
                                                                                <td valign="bottom" colspan="3" style="text-align: center; border: none; vertical-align: middle;">
                                                                                    <asp:HiddenField ID="hidCodEcom1" runat="server" Value="" />
                                                                                    <asp:HiddenField ID="hidTipo1" runat="server" Value="" />
                                                                                    <asp:DropDownList ID="ddl1" runat="server" Width="100px" OnSelectedIndexChanged="ddl1_SelectedIndexChanged" AutoPostBack="true">
                                                                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                                                                        <asp:ListItem Value="up_sell" Text="2 Produtos"></asp:ListItem>
                                                                                        <asp:ListItem Value="related" Text="Relacionados"></asp:ListItem>
                                                                                    </asp:DropDownList>&nbsp;&nbsp;
                                                                                <asp:Label ID="labProdutoTitulo1" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Large"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: center; border: none">
                                                                                    <asp:Label ID="labGrupo1" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                </td>
                                                                                <td style="text-align: center; border: none">
                                                                                    <asp:Label ID="labLinha1" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                </td>
                                                                                <td style="text-align: center; border: none">
                                                                                    <asp:Label ID="labGriffe1" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="3" style="text-align: center; border: none;">
                                                                                    <asp:Image ID="imgFrenteCab1" runat="server" Width="380px" Height="" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-Width="33%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="0px">
                                                                <ItemTemplate>
                                                                    <asp:Panel ID="pnl2" runat="server" BorderWidth="1">
                                                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                                                            <tr>
                                                                                <td colspan="3" style="text-align: center; border: none;">
                                                                                    <asp:HiddenField ID="hidCodEcom2" runat="server" Value="" />
                                                                                    <asp:HiddenField ID="hidTipo2" runat="server" Value="" />
                                                                                    <asp:DropDownList ID="ddl2" runat="server" Width="100px" OnSelectedIndexChanged="ddl2_SelectedIndexChanged" AutoPostBack="true">
                                                                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                                                                        <asp:ListItem Value="up_sell" Text="2 Produtos"></asp:ListItem>
                                                                                        <asp:ListItem Value="related" Text="Relacionados"></asp:ListItem>
                                                                                    </asp:DropDownList>&nbsp;&nbsp;
                                                                                <asp:Label ID="labProdutoTitulo2" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Large"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: center; border: none">
                                                                                    <asp:Label ID="labGrupo2" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                </td>
                                                                                <td style="text-align: center; border: none">
                                                                                    <asp:Label ID="labLinha2" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                </td>
                                                                                <td style="text-align: center; border: none">
                                                                                    <asp:Label ID="labGriffe2" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="3" style="text-align: center; border: none;">
                                                                                    <asp:Image ID="imgFrenteCab2" runat="server" Width="380px" Height="" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-Width="33%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="0px">
                                                                <ItemTemplate>
                                                                    <asp:Panel ID="pnl3" runat="server" BorderWidth="1">
                                                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                                                            <tr>
                                                                                <td colspan="3" style="text-align: center; border: none;">
                                                                                    <asp:HiddenField ID="hidCodEcom3" runat="server" Value="" />
                                                                                    <asp:HiddenField ID="hidTipo3" runat="server" Value="" />
                                                                                    <asp:DropDownList ID="ddl3" runat="server" Width="100px" OnSelectedIndexChanged="ddl3_SelectedIndexChanged" AutoPostBack="true">
                                                                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                                                                        <asp:ListItem Value="up_sell" Text="2 Produtos"></asp:ListItem>
                                                                                        <asp:ListItem Value="related" Text="Relacionados"></asp:ListItem>
                                                                                    </asp:DropDownList>&nbsp;&nbsp;
                                                                                <asp:Label ID="labProdutoTitulo3" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Large"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: center; border: none">
                                                                                    <asp:Label ID="labGrupo3" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                </td>
                                                                                <td style="text-align: center; border: none">
                                                                                    <asp:Label ID="labLinha3" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                </td>
                                                                                <td style="text-align: center; border: none">
                                                                                    <asp:Label ID="labGriffe3" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="3" style="text-align: center; border: none;">
                                                                                    <asp:Image ID="imgFrenteCab3" runat="server" Width="380px" Height="" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </fieldset>
                    </div>

                    <div>
                        <fieldset>
                            <legend>Características</legend>
                            <div>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td colspan="7">Grupo Produto
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7">
                                            <asp:DropDownList ID="ddlGrupoProdutoFiltroCaract" runat="server" Width="194px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                                DataValueField="GRUPO_PRODUTO" OnSelectedIndexChanged="ddlGrupoProdutoFiltroCaract_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Tipo Modelagem
                                        </td>
                                        <td>Tipo Tecido
                                        </td>
                                        <td>Tipo Manga
                                        </td>
                                        <td>Tipo Gola
                                        </td>
                                        <td>Tipo Comprimento
                                        </td>
                                        <td>Tipo Estilo
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px;">
                                            <asp:DropDownList ID="ddlTipoModelagem" runat="server" Width="194px" Height="21px" DataValueField="CODIGO" DataTextField="TIPO_MODELAGEM"></asp:DropDownList>
                                        </td>
                                        <td style="width: 200px;">
                                            <asp:DropDownList ID="ddlTipoTecido" runat="server" Width="194px" Height="21px" DataValueField="CODIGO" DataTextField="TIPO_TECIDO"></asp:DropDownList>
                                        </td>
                                        <td style="width: 200px;">
                                            <asp:DropDownList ID="ddlTipoManga" runat="server" Width="194px" Height="21px" DataValueField="CODIGO" DataTextField="TIPO_MANGA"></asp:DropDownList>
                                        </td>
                                        <td style="width: 200px;">
                                            <asp:DropDownList ID="ddlTipoGola" runat="server" Width="194px" Height="21px" DataValueField="CODIGO" DataTextField="TIPO_GOLA"></asp:DropDownList>
                                        </td>
                                        <td style="width: 200px;">
                                            <asp:DropDownList ID="ddlTipoComprimento" runat="server" Width="194px" Height="21px" DataValueField="CODIGO" DataTextField="TIPO_COMPRIMENTO"></asp:DropDownList>
                                        </td>
                                        <td style="width: 200px;">
                                            <asp:DropDownList ID="ddlTipoEstilo" runat="server" Width="194px" Height="21px" DataValueField="CODIGO" DataTextField="TIPO_ESTILO"></asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="btBuscarCaract" runat="server" Text="Buscar" Width="100px" OnClick="btBuscarCaract_Click" />&nbsp;&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7">&nbsp;</td>
                                    </tr>
                                </table>
                            </div>
                            <div id="accordionCA">
                                <h3>Abrir</h3>
                                <div>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvCaracteristicas" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound"
                                                        OnDataBound="gvProduto_DataBound" ShowFooter="true" BorderWidth="0px" ShowHeader="false">
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" ItemStyle-Width="33%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="0px">
                                                                <ItemTemplate>
                                                                    <asp:Panel ID="pnl1" runat="server" BorderWidth="1">
                                                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                                                            <tr>
                                                                                <td valign="bottom" colspan="3" style="text-align: center; border: none; vertical-align: middle;">
                                                                                    <asp:HiddenField ID="hidCodEcom1" runat="server" Value="" />
                                                                                    <asp:HiddenField ID="hidTipo1" runat="server" Value="" />
                                                                                    <asp:DropDownList ID="ddl1" runat="server" Width="100px" OnSelectedIndexChanged="ddl1_SelectedIndexChanged" AutoPostBack="true">
                                                                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                                                                        <asp:ListItem Value="up_sell" Text="2 Produtos"></asp:ListItem>
                                                                                        <asp:ListItem Value="related" Text="Relacionados"></asp:ListItem>
                                                                                    </asp:DropDownList>&nbsp;&nbsp;
                                                                                <asp:Label ID="labProdutoTitulo1" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Large"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: center; border: none">
                                                                                    <asp:Label ID="labGrupo1" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                </td>
                                                                                <td style="text-align: center; border: none">
                                                                                    <asp:Label ID="labLinha1" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                </td>
                                                                                <td style="text-align: center; border: none">
                                                                                    <asp:Label ID="labGriffe1" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="3" style="text-align: center; border: none;">
                                                                                    <asp:Image ID="imgFrenteCab1" runat="server" Width="380px" Height="" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-Width="33%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="0px">
                                                                <ItemTemplate>
                                                                    <asp:Panel ID="pnl2" runat="server" BorderWidth="1">
                                                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                                                            <tr>
                                                                                <td colspan="3" style="text-align: center; border: none;">
                                                                                    <asp:HiddenField ID="hidCodEcom2" runat="server" Value="" />
                                                                                    <asp:HiddenField ID="hidTipo2" runat="server" Value="" />
                                                                                    <asp:DropDownList ID="ddl2" runat="server" Width="100px" OnSelectedIndexChanged="ddl2_SelectedIndexChanged" AutoPostBack="true">
                                                                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                                                                        <asp:ListItem Value="up_sell" Text="2 Produtos"></asp:ListItem>
                                                                                        <asp:ListItem Value="related" Text="Relacionados"></asp:ListItem>
                                                                                    </asp:DropDownList>&nbsp;&nbsp;
                                                                                <asp:Label ID="labProdutoTitulo2" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Large"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: center; border: none">
                                                                                    <asp:Label ID="labGrupo2" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                </td>
                                                                                <td style="text-align: center; border: none">
                                                                                    <asp:Label ID="labLinha2" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                </td>
                                                                                <td style="text-align: center; border: none">
                                                                                    <asp:Label ID="labGriffe2" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="3" style="text-align: center; border: none;">
                                                                                    <asp:Image ID="imgFrenteCab2" runat="server" Width="380px" Height="" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-Width="33%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="0px">
                                                                <ItemTemplate>
                                                                    <asp:Panel ID="pnl3" runat="server" BorderWidth="1">
                                                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                                                            <tr>
                                                                                <td colspan="3" style="text-align: center; border: none;">
                                                                                    <asp:HiddenField ID="hidCodEcom3" runat="server" Value="" />
                                                                                    <asp:HiddenField ID="hidTipo3" runat="server" Value="" />
                                                                                    <asp:DropDownList ID="ddl3" runat="server" Width="100px" OnSelectedIndexChanged="ddl3_SelectedIndexChanged" AutoPostBack="true">
                                                                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                                                                        <asp:ListItem Value="up_sell" Text="2 Produtos"></asp:ListItem>
                                                                                        <asp:ListItem Value="related" Text="Relacionados"></asp:ListItem>
                                                                                    </asp:DropDownList>&nbsp;&nbsp;
                                                                                <asp:Label ID="labProdutoTitulo3" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Large"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: center; border: none">
                                                                                    <asp:Label ID="labGrupo3" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                </td>
                                                                                <td style="text-align: center; border: none">
                                                                                    <asp:Label ID="labLinha3" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                </td>
                                                                                <td style="text-align: center; border: none">
                                                                                    <asp:Label ID="labGriffe3" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="3" style="text-align: center; border: none;">
                                                                                    <asp:Image ID="imgFrenteCab3" runat="server" Width="380px" Height="" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <fieldset>
                        <legend>Filtro Geral</legend>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td>Coleção
                                </td>
                                <td>Grupo Produto
                                </td>
                                <td>Grupo Magento
                                </td>
                                <td>Produto
                                </td>
                                <td>Cor Linx
                                </td>
                                <td>Cor Fornecedor
                                </td>
                                <td>Griffe
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px">
                                    <asp:DropDownList ID="ddlColecao" runat="server" Width="194px" Height="21px"
                                        DataTextField="DESC_COLECAO" DataValueField="COLECAO" OnSelectedIndexChanged="ddlColecao_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 200px">
                                    <asp:DropDownList ID="ddlGrupoLinx" runat="server" Width="194px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                        DataValueField="GRUPO_PRODUTO">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 160px">
                                    <asp:DropDownList ID="ddlGrupoMagento" runat="server" Width="154px" Height="21px" DataTextField="GRUPO"
                                        DataValueField="CODIGO">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 140px">
                                    <asp:TextBox ID="txtProduto" runat="server" onkeypress="return fnValidarNumero(event);"
                                        Width="130px"></asp:TextBox>
                                </td>
                                <td style="width: 200px">
                                    <asp:DropDownList ID="ddlCorLinx" runat="server" Width="194px" Height="21px" DataTextField="DESC_COR"
                                        DataValueField="COR">
                                    </asp:DropDownList>
                                </td>

                                <td style="width: 310px">
                                    <asp:DropDownList ID="ddlCorFornecedor" runat="server" Width="304px" Height="21px"
                                        DataTextField="FORNECEDOR_COR" DataValueField="FORNECEDOR_COR">
                                    </asp:DropDownList>
                                </td>

                                <td>
                                    <asp:DropDownList ID="ddlGriffe" runat="server" Width="200px" Height="21px" DataTextField="GRIFFE"
                                        DataValueField="GRIFFE">
                                    </asp:DropDownList>
                                </td>


                            </tr>
                            <tr>
                                <td>Categoria
                                </td>
                                <td>Tecido
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlCodCategoria" runat="server" Width="194px" Height="21px">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="01" Text="PEÇAS"></asp:ListItem>
                                        <asp:ListItem Value="02" Text="ACESSÓRIOS"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTecido" runat="server" Width="194px" Height="21px" DataTextField="TECIDO_POCKET"
                                        DataValueField="TECIDO_POCKET">
                                    </asp:DropDownList>
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>

                            <tr>
                                <td colspan="7">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                    <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                    <div id="accordionT">
                                        <h3>Produtos</h3>
                                        <div>
                                            <table border="0" cellpadding="0" class="tb" width="100%">
                                                <tr>
                                                    <td style="width: 100%;">
                                                        <div class="rounded_corners">
                                                            <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound"
                                                                OnDataBound="gvProduto_DataBound" ShowFooter="true" BorderWidth="0px" ShowHeader="false">
                                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Width="33%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="0px">
                                                                        <ItemTemplate>
                                                                            <asp:Panel ID="pnl1" runat="server" BorderWidth="1">
                                                                                <table border="0" cellpadding="0" class="tb" width="100%">
                                                                                    <tr>
                                                                                        <td valign="bottom" colspan="3" style="text-align: center; border: none; vertical-align: middle;">
                                                                                            <asp:HiddenField ID="hidCodEcom1" runat="server" Value="" />
                                                                                            <asp:HiddenField ID="hidTipo1" runat="server" Value="" />
                                                                                            <asp:DropDownList ID="ddl1" runat="server" Width="100px" OnSelectedIndexChanged="ddl1_SelectedIndexChanged" AutoPostBack="true">
                                                                                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                                                                                <asp:ListItem Value="up_sell" Text="2 Produtos"></asp:ListItem>
                                                                                                <asp:ListItem Value="related" Text="Relacionados"></asp:ListItem>
                                                                                            </asp:DropDownList>&nbsp;&nbsp;
                                                                                <asp:Label ID="labProdutoTitulo1" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Large"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="text-align: center; border: none">
                                                                                            <asp:Label ID="labGrupo1" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                        </td>
                                                                                        <td style="text-align: center; border: none">
                                                                                            <asp:Label ID="labLinha1" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                        </td>
                                                                                        <td style="text-align: center; border: none">
                                                                                            <asp:Label ID="labGriffe1" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="3" style="text-align: center; border: none;">
                                                                                            <asp:Image ID="imgFrenteCab1" runat="server" Width="380px" Height="" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </asp:Panel>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Width="33%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="0px">
                                                                        <ItemTemplate>
                                                                            <asp:Panel ID="pnl2" runat="server" BorderWidth="1">
                                                                                <table border="0" cellpadding="0" class="tb" width="100%">
                                                                                    <tr>
                                                                                        <td colspan="3" style="text-align: center; border: none;">
                                                                                            <asp:HiddenField ID="hidCodEcom2" runat="server" Value="" />
                                                                                            <asp:HiddenField ID="hidTipo2" runat="server" Value="" />
                                                                                            <asp:DropDownList ID="ddl2" runat="server" Width="100px" OnSelectedIndexChanged="ddl2_SelectedIndexChanged" AutoPostBack="true">
                                                                                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                                                                                <asp:ListItem Value="up_sell" Text="2 Produtos"></asp:ListItem>
                                                                                                <asp:ListItem Value="related" Text="Relacionados"></asp:ListItem>
                                                                                            </asp:DropDownList>&nbsp;&nbsp;
                                                                                <asp:Label ID="labProdutoTitulo2" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Large"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="text-align: center; border: none">
                                                                                            <asp:Label ID="labGrupo2" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                        </td>
                                                                                        <td style="text-align: center; border: none">
                                                                                            <asp:Label ID="labLinha2" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                        </td>
                                                                                        <td style="text-align: center; border: none">
                                                                                            <asp:Label ID="labGriffe2" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="3" style="text-align: center; border: none;">
                                                                                            <asp:Image ID="imgFrenteCab2" runat="server" Width="380px" Height="" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </asp:Panel>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="" ItemStyle-Width="33%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="0px">
                                                                        <ItemTemplate>
                                                                            <asp:Panel ID="pnl3" runat="server" BorderWidth="1">
                                                                                <table border="0" cellpadding="0" class="tb" width="100%">
                                                                                    <tr>
                                                                                        <td colspan="3" style="text-align: center; border: none;">
                                                                                            <asp:HiddenField ID="hidCodEcom3" runat="server" Value="" />
                                                                                            <asp:HiddenField ID="hidTipo3" runat="server" Value="" />
                                                                                            <asp:DropDownList ID="ddl3" runat="server" Width="100px" OnSelectedIndexChanged="ddl3_SelectedIndexChanged" AutoPostBack="true">
                                                                                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                                                                                <asp:ListItem Value="up_sell" Text="2 Produtos"></asp:ListItem>
                                                                                                <asp:ListItem Value="related" Text="Relacionados"></asp:ListItem>
                                                                                            </asp:DropDownList>&nbsp;&nbsp;
                                                                                <asp:Label ID="labProdutoTitulo3" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Large"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="text-align: center; border: none">
                                                                                            <asp:Label ID="labGrupo3" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                        </td>
                                                                                        <td style="text-align: center; border: none">
                                                                                            <asp:Label ID="labLinha3" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                        </td>
                                                                                        <td style="text-align: center; border: none">
                                                                                            <asp:Label ID="labGriffe3" runat="server" Text="" ForeColor="Gray" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="3" style="text-align: center; border: none;">
                                                                                            <asp:Image ID="imgFrenteCab3" runat="server" Width="380px" Height="" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </asp:Panel>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
