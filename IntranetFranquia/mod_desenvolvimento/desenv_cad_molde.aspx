<%@ Page Title="Molde" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="desenv_cad_molde.aspx.cs" Inherits="Relatorios.desenv_cad_molde"
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
                    de Coleção Nacional&nbsp;&nbsp;>&nbsp;&nbsp;Molde</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Consultar Coleção"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 30px;">Coleção
                        </td>
                        <td>Origem
                        </td>
                        <td>Griffe
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
                    </tr>
                    <tr>
                        <td style="width: 160px;" valign="top">
                            <asp:DropDownList ID="ddlColecoes" runat="server" Width="154px" Height="21px" DataTextField="DESC_COLECAO"
                                DataValueField="COLECAO" OnSelectedIndexChanged="ddlColecoes_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 180px;" valign="top">
                            <asp:ListBox ID="lstOrigem" runat="server" DataValueField="CODIGO" Width="174px" DataTextField="DESCRICAO" SelectionMode="Multiple" Height="110px"></asp:ListBox>
                        </td>
                        <td style="width: 160px;" valign="top">
                            <asp:ListBox ID="lstGriffe" runat="server" DataValueField="GRIFFE" Width="154px" DataTextField="GRIFFE" SelectionMode="Multiple" Height="110px"></asp:ListBox>
                        </td>
                        <td style="width: 170px;" valign="top">
                            <asp:DropDownList ID="ddlGrupo" runat="server" Width="164px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                DataValueField="GRUPO_PRODUTO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 130px;" valign="top">
                            <asp:TextBox ID="txtModelo" runat="server" Width="116px" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width: 170px;" valign="top">
                            <asp:TextBox ID="txtNome" runat="server" Width="156px" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width: 210px;" valign="top">
                            <asp:DropDownList ID="ddlTecido" runat="server" Width="204px" Height="21px" DataTextField="TECIDO_POCKET"
                                DataValueField="TECIDO_POCKET" OnSelectedIndexChanged="ddlTecido_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td valign="top">
                            <asp:DropDownList ID="ddlCorFornecedor" runat="server" Width="204px" Height="21px"
                                DataTextField="FORNECEDOR_COR" DataValueField="FORNECEDOR_COR">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvMolde" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvMolde_RowDataBound">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:Image ID="imgProduto" runat="server" Width="150px" ImageAlign="AbsMiddle" />
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:BoundField DataField="MOLDE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Molde" HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="MODELAGEM" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Modelagem" HeaderStyle-Width="200px" />
                                        <asp:BoundField DataField="PRODUTO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Produto" HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="DESC_PRODUTO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Nome" />
                                        <asp:BoundField DataField="GRIFFE" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Griffe" />
                                        <asp:BoundField DataField="TECIDO_POCKET" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Tecido" />
                                        <asp:BoundField DataField="COR_FORNECEDOR" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Cor" />
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
