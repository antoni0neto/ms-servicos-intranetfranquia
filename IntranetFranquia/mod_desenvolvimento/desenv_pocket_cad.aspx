<%@ Page Title="Desenvolvimento de Coleção" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="desenv_pocket_cad.aspx.cs" Inherits="Relatorios.desenv_pocket_cad"
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
        <Triggers>
            <asp:PostBackTrigger ControlID="btImportar" />
        </Triggers>
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Planejamento
                    de Coleção Nacional&nbsp;&nbsp;>&nbsp;&nbsp;Desenvolvimento de Coleção</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1200px; margin-left: 8%;">
                <fieldset>
                    <legend>Desenvolvimento de Coleção</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlImportacao" runat="server">
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr style="line-height: 24px;">
                                            <td style="width: 30px;">Modelo Excel
                                            </td>
                                            <td style="width: 30px;">Foto do Modelo &nbsp;<span style="color: Red; font-size: 10px; font-weight: bold;">(300px
                                                    x 320px)</span>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 300px;">
                                                <asp:FileUpload ID="upPocketExcel" runat="server" />
                                            </td>
                                            <td style="width: 300px;">
                                                <asp:FileUpload ID="upPocketFoto" runat="server" />
                                            </td>
                                            <td style="width: 110px;">&nbsp;
                                            </td>
                                            <td style='text-align: right;'>
                                                <asp:Button ID="btImportar" runat="server" Text="Importar" Width="100px" OnClick="btImportar_Click" />
                                            </td>
                                        </tr>
                                        <tr style="line-height: 20px;">
                                            <td colspan="4">
                                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                                <asp:Label ID="labErroOleDB" runat="server" ForeColor="Red" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <div id="divErro" class="rounded_corners" runat="server">
                                                    <asp:GridView ID="gvErro" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvErro_RowDataBound">
                                                        <HeaderStyle BackColor="GradientActiveCaption" />
                                                        <FooterStyle BackColor="GradientActiveCaption" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="LINHA" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Linha" HeaderStyle-Width="30px" />
                                                            <asp:BoundField DataField="COLECAO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Coleção" HeaderStyle-Width="120px" />
                                                            <asp:BoundField DataField="ORIGEM" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Origem" HeaderStyle-Width="100px" />
                                                            <asp:BoundField DataField="GRUPO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Grupo" HeaderStyle-Width="120px" />
                                                            <asp:BoundField DataField="MODELO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Modelo" HeaderStyle-Width="60px" />
                                                            <asp:BoundField DataField="COR" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Cor" HeaderStyle-Width="60px" />
                                                            <asp:BoundField DataField="OBS" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Observação" HeaderStyle-Width="200px" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="width: 100%;">
                                <asp:Panel ID="pnlModelo" runat="server" BorderColor="" BorderWidth="">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Button ID="btNovo" runat="server" Text="Novo" Width="100px" OnClick="btNovo_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" style="width: 548px;">
                                                <div id="divModelo" class="rounded_corners" runat="server">
                                                    <asp:GridView ID="gvModelo" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvModelo_RowDataBound">
                                                        <HeaderStyle BackColor="GradientActiveCaption" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="MODELO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Modelo" HeaderStyle-Width="150px" />
                                                            <asp:BoundField DataField="CODIGO_REF" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Código Referência" HeaderStyle-Width="150px" />
                                                            <asp:BoundField DataField="COR" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Cor" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center;">
                                                <div id="divPocket" runat="server" style='text-align: center;'>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
