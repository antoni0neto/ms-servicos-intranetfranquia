<%@ Page Title="Planejamento de Produto" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="desenv_produto_cad.aspx.cs" Inherits="Relatorios.desenv_produto_cad"
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
        <Triggers>
            <asp:PostBackTrigger ControlID="btImportar" />
        </Triggers>
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Planejamento
                    de Coleção Nacional&nbsp;&nbsp;>&nbsp;&nbsp;Planejamento de Produto</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1200px; margin-left: 8%;">
                <fieldset>
                    <legend>Planejamento de Produto</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 30px;">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 300px;">
                                <asp:FileUpload ID="upTecidoExcel" runat="server" />
                            </td>
                            <td style="width: 110px;">
                                &nbsp;
                            </td>
                            <td>
                                <asp:Button ID="btImportar" runat="server" Text="Importar" Width="100px" OnClick="btImportar_Click" />&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                <asp:Label ID="labErroOleDB" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvErro" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvErro_RowDataBound">
                                        <HeaderStyle BackColor="Gainsboro" />
                                        <FooterStyle BackColor="Gainsboro" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="LINHA" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Linha" HeaderStyle-Width="30px" />
                                            <asp:BoundField DataField="CODREF" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Código" HeaderStyle-Width="50px" />
                                            <asp:BoundField DataField="ORIGEM" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Origem" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="COLECAO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Coleção" HeaderStyle-Width="120px" />
                                            <asp:BoundField DataField="GRUPO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Grupo" HeaderStyle-Width="120px" />
                                            <asp:BoundField DataField="MODELO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Modelo" HeaderStyle-Width="60px" />
                                            <asp:BoundField DataField="OBS" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Observação" HeaderStyle-Width="200px" />
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
