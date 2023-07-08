<%@ Page Title="Relatório de Aviamentos" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="prod_rel_aviamento.aspx.cs" Inherits="Relatorios.prod_rel_aviamento"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita
        {
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Aviamentos&nbsp;&nbsp;>&nbsp;&nbsp;Relatório
                    de Aviamentos</span>
                <div style="float: right; padding: 0;">
                    <a href="prod_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Relatório de Aviamentos"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            Coleção
                        </td>
                        <td>
                            HB
                        </td>
                        <td>
                            No. Orçamento
                        </td>
                        <td>
                            Aviamento
                        </td>
                        <td>
                            Fornecedor
                        </td>
                        <td>
                            Comprado
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 160px;">
                            <asp:DropDownList ID="ddlColecoesBuscar" runat="server" Width="154px" Height="21px"
                                DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 110px;">
                            <asp:TextBox ID="txtHBBuscar" runat="server" Width="100px" MaxLength="5" CssClass="alinharDireita"
                                onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                        </td>
                        <td style="width: 120px;">
                            <asp:TextBox ID="txtNumeroOrcamento" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                Width="110px"></asp:TextBox>
                        </td>
                        <td style="width: 200px;">
                            <asp:DropDownList ID="ddlAviamento" runat="server" Width="194px" Height="21px" DataValueField="CODIGO"
                                DataTextField="DESCRICAO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 180px;">
                            <asp:DropDownList ID="ddlFornecedor" runat="server" Width="174px" Height="21px" DataTextField="FORNECEDOR"
                                DataValueField="FORNECEDOR">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 180px;">
                            <asp:DropDownList ID="ddlComprado" runat="server" Width="174px" Height="21px">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvAviamento" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvAviamento_RowDataBound"
                                    DataKeyNames="CODIGO">
                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="HB" HeaderText="HB" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Nome" HeaderStyle-Width="205px" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-BorderColor="GradientActiveCaption"
                                            ItemStyle-BorderWidth="1px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litGrupoNome" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DESC_AVIAMENTO" HeaderText="Aviamento" HeaderStyle-Width="210px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="QTDE" HeaderText="Qtde" HeaderStyle-Width="130px" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Comprado" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litComprado" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DESCRICAO" HeaderText="Descrição" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField HeaderText="Compra Extra" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-BorderColor="GradientActiveCaption" ItemStyle-BorderWidth="1px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litCompraExtra" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" style="text-align: right;">
                            <asp:Label ID="labErroImpressao" runat="server" ForeColor="Red" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btImprimir" runat="server" Text="Imprimir" Width="100px" OnClick="btImprimir_Click" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
