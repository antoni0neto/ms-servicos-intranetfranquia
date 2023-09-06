<%@ Page Title="HB Consulta Status" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="prod_con_hb_status.aspx.cs" Inherits="Relatorios.prod_con_hb_status"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios
                    Ordem de Corte&nbsp;&nbsp;>&nbsp;&nbsp;HB Consulta Status</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="HB Consulta Status"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 30px;">
                            Coleção
                        </td>
                        <td>
                            HB
                        </td>
                        <td>
                            Nome
                        </td>
                        <td>
                            Mostruário
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
                        <td style="width: 150px;">
                            <asp:TextBox ID="txtNomeBuscar" runat="server" Width="140px"></asp:TextBox>
                        </td>
                        <td style="width: 65px; text-align: center">
                            <asp:CheckBox ID="chkMostruario" runat="server" Checked="false" />
                        </td>
                        <td>
                            &nbsp;
                            <asp:Button ID="btHBBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btHBBuscar_Click" />&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="labHBBuscar" runat="server" ForeColor="Red" Text=""></asp:Label>
                            <asp:HiddenField ID="hidTela" runat="server" />
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
                                        <asp:BoundField DataField="HB" HeaderText="HB" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="DETALHE" HeaderText="Detalhe" HeaderStyle-Width="130px"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="GRUPO" HeaderText="Grupo" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="NOME" HeaderText="Nome" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="DT_INI_AMPLIACAO" HeaderText="Início Ampliação" HeaderStyle-Width="150px"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="DT_INI_RISCO" HeaderText="Início Risco" HeaderStyle-Width="150px"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="DT_INI_CORTE" HeaderText="Início Corte" HeaderStyle-Width="150px"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="DT_FINALIZADO" HeaderText="Finalizado" HeaderStyle-Width="150px"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="QTDE" HeaderText="Qtde" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="PROCESSO" HeaderText="Processo Atual" HeaderStyle-Width="150px"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
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
