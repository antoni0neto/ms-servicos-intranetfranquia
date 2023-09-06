<%@ Page Title="Baixa de Modelagem" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="prod_cad_modelagem_baixa.aspx.cs" Inherits="Relatorios.prod_cad_modelagem_baixa"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Ordem de Corte&nbsp;&nbsp;>&nbsp;&nbsp;Baixa de Modelagem</span>
                <div style="float: right; padding: 0;">
                    <a href="prod_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Baixa de Modelagem"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Coleção
                        </td>
                        <td>HB
                        </td>
                        <td>Produto
                        </td>
                        <td>Nome
                        </td>
                        <td>Molde
                        </td>
                        <td>Modelagem
                        </td>
                        <td>Mostruário
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 160px;">
                            <asp:DropDownList ID="ddlColecao" runat="server" Width="154px" Height="21px"
                                DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 110px;">
                            <asp:TextBox ID="txtHB" runat="server" Width="100px" MaxLength="5" CssClass="alinharDireita"
                                onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                        </td>
                        <td style="width: 110px;">
                            <asp:TextBox ID="txtProduto" runat="server" Width="100px" MaxLength="6"></asp:TextBox>
                        </td>
                        <td style="width: 150px;">
                            <asp:TextBox ID="txtNome" runat="server" Width="140px"></asp:TextBox>
                        </td>
                        <td style="width: 150px;">
                            <asp:TextBox ID="txtMolde" runat="server" Width="140px" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                        </td>
                        <td style="width: 150px;">
                            <asp:TextBox ID="txtModelagem" runat="server" Width="140px"></asp:TextBox>
                        </td>
                        <td style="width: 110px;">
                            <asp:DropDownList ID="ddlMostruario" runat="server" Width="104px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
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
                                <asp:GridView ID="gvMolde" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                    Style="background: white" OnRowDataBound="gvMolde_RowDataBound"
                                    OnSorting="gvMolde_Sorting" AllowSorting="true">
                                    <HeaderStyle BackColor="Gainsboro" />
                                    <FooterStyle BackColor="Gainsboro" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DESC_COLECAO" HeaderText="Coleção" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left" SortExpression="COLECAO" />
                                        <asp:BoundField DataField="HB" HeaderText="HB" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center" SortExpression="HB" />
                                        <asp:BoundField DataField="PRODUTO" HeaderText="Produto" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center" SortExpression="PRODUTO" />
                                        <asp:BoundField DataField="NOME" HeaderText="Nome" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" SortExpression="NOME" />
                                        <asp:TemplateField HeaderText="Mostruário" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" SortExpression="MOSTRUARIO">
                                            <ItemTemplate>
                                                <asp:Literal ID="litMostruario" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="MOLDE_HB" HeaderText="Molde" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" SortExpression="MOLDE_HB" />
                                        <asp:BoundField DataField="MODELAGEM_HB" HeaderText="Modelagem" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="" HeaderStyle-HorizontalAlign="Left" SortExpression="MODELAGEM_HB" />
                                        <asp:TemplateField HeaderText="Usuário CAD" HeaderStyle-Width="300px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="USUARIO_CAD">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtUsuarioCAD" runat="server" Width="300px" Text='<%# Bind("USUARIO_CAD") %>' OnTextChanged="txtUsuarioCAD_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="1px">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hidCodigoMolde" runat="server" Value='<%# Bind("CODIGO_MOLDE") %>' />
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
