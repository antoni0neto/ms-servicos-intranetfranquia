<%@ Page Title="HB Consulta Filtro" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="prod_con_hb_filtro.aspx.cs" Inherits="Relatorios.prod_con_hb_filtro"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios
                    Ordem de Corte&nbsp;&nbsp;>&nbsp;&nbsp;<asp:Label ID="labTituloMenu" runat="server"></asp:Label></span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1000px; margin-left: 15%;">
                <fieldset>
                    <legend>
                        <asp:Label ID="labTitulo" runat="server"></asp:Label></legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 30px;">Coleção
                            </td>
                            <td>HB
                            </td>
                            <td>Nome
                            </td>
                            <td>Mostruário
                            </td>
                            <td>&nbsp;
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
                            <td style="width: 150px;">
                                <asp:TextBox ID="txtNomeBuscar" runat="server" Width="140px"></asp:TextBox>
                            </td>
                            <td style="width: 65px; text-align: center">
                                <asp:CheckBox ID="chkMostruario" runat="server" Checked="false" />
                            </td>
                            <td>&nbsp;
                                <asp:Button ID="btHBBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btHBBuscar_Click" />&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="labHBBuscar" runat="server" ForeColor="Red" Text=""></asp:Label>
                                <asp:HiddenField ID="hidTela" runat="server" />
                                <asp:HiddenField ID="hidDetalhe" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvHB" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                        Style="background: white" OnRowDataBound="gvHB_RowDataBound">
                                        <HeaderStyle BackColor="Gainsboro" />
                                        <FooterStyle BackColor="Gainsboro" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="HB" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="HB" HeaderStyle-Width="120px" />
                                            <asp:TemplateField HeaderText="Detalhe" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="170px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDetalhe" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Nome" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="200px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNome" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cor" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="200px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litCor" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mostruário" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litMostruario" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:Button ID="btHB" runat="server" Height="19px" Width="70px" Text="Abrir" OnClick="btHB_Click" />
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
