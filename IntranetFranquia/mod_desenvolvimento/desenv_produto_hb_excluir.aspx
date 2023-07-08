<%@ Page Title="Separar HB" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="desenv_produto_hb_excluir.aspx.cs" Inherits="Relatorios.desenv_produto_hb_excluir"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita
        {
            text-align: right;
        }
    </style>
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Controle
                    de Produto&nbsp;&nbsp;>&nbsp;&nbsp;Separar HB</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Separar HB"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            Coleção
                        </td>
                        <td>
                            HB
                        </td>
                        <td>
                            Modelo
                        </td>
                        <td>
                            Nome
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 150px;">
                            <asp:DropDownList ID="ddlColecoes" runat="server" Width="143px" Height="21px" DataTextField="DESC_COLECAO"
                                DataValueField="COLECAO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 160px;">
                            <asp:TextBox ID="txtHB" runat="server" Width="150px" MaxLength="8" CssClass="alinharDireita"
                                onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                        </td>
                        <td style="width: 160px;">
                            <asp:TextBox ID="txtModelo" runat="server" Width="150px"></asp:TextBox>
                        </td>
                        <td style="width: 160px;">
                            <asp:TextBox ID="txtNome" runat="server" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvProduto" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvProduto_RowDataBound"
                                    DataKeyNames="CODIGO">
                                    <HeaderStyle BackColor="GradientActiveCaption" />
                                    <FooterStyle BackColor="GradientActiveCaption" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="35px" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <img alt="" style="cursor: pointer" src="../Image/plus.png" width="18px" />
                                                <asp:Panel ID="pnlFoto" runat="server" Style="display: none" Width="100%">
                                                    <asp:GridView ID="gvFoto" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvFoto_RowDataBound"
                                                        Width="100%" DataKeyNames="CODIGO">
                                                        <HeaderStyle BackColor="LightSteelBlue" HorizontalAlign="Center"></HeaderStyle>
                                                        <RowStyle BorderColor="GradientActiveCaption" BorderWidth="1px" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Foto Produto" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50%" ItemStyle-BorderColor="GradientActiveCaption"
                                                                ItemStyle-BorderWidth="1px">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgFotoProduto" Width="80" Height="130" runat="server" ImageAlign="Middle" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Foto Peça" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgFotoPeca" Width="80" Height="130" runat="server" ImageAlign="Middle" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="P_GRUPO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Produto Grupo" HeaderStyle-Width="200px" ItemStyle-BackColor="WhiteSmoke" />
                                        <asp:BoundField DataField="P_MODELO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Produto Modelo" HeaderStyle-Width="200px" ItemStyle-BackColor="WhiteSmoke" />
                                        <asp:BoundField DataField="P_COR" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Produto Cor" HeaderStyle-Width="150px" ItemStyle-BackColor="WhiteSmoke" />
                                        <asp:BoundField DataField="" HeaderText="" HeaderStyle-Width="98px" />
                                        <asp:BoundField DataField="HB_GRUPO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="HB Grupo" HeaderStyle-Width="200px" ItemStyle-BackColor="WhiteSmoke" />
                                        <asp:BoundField DataField="HB_NOME" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="HB Nome" HeaderStyle-Width="200px" ItemStyle-BackColor="WhiteSmoke" />
                                        <asp:BoundField DataField="HB_COR" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="HB Cor" HeaderStyle-Width="150px" ItemStyle-BackColor="WhiteSmoke" />
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Button ID="btExcluir" runat="server" Height="19px" OnClientClick="return ConfirmarExclusao();"
                                                    Text="Excluir" OnClick="btExcluir_Click" />
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
