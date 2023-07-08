<%@ Page Title="Cadastro de Cores" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="desenv_pocket_cor.aspx.cs" Inherits="Relatorios.desenv_pocket_cor"
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
            <asp:PostBackTrigger ControlID="btIncluir" />
        </Triggers>
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Planejamento
                    de Coleção Nacional&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Cores</span>
                <div style="float: right; padding: 0;">
                    <a href="desenv_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1200px; margin-left: 8%;">
                <fieldset>
                    <legend>Cadastro de Cores</legend>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="tb">
                        <tr style="height: 10px; vertical-align: bottom;">
                            <td style="width: 250px;">
                                <asp:Label ID="labCor" runat="server" Text="Cor"></asp:Label>
                            </td>
                            <td style="width: 400px;">
                                <asp:Label ID="labFotoTecido" runat="server" Text="Foto"></asp:Label>
                            </td>
                            <td>
                                <asp:HiddenField ID="hidCodigo" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlCorFornecedor" runat="server" Width="244px" Height="21px"
                                    DataTextField="FORNECEDOR_COR" DataValueField="FORNECEDOR_COR">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:FileUpload ID="upFoto" runat="server" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btIncluir" Text="Incluir" OnClick="btIncluir_Click"
                                    Width="100px" Enabled="true" />
                                <asp:Label ID="labErro" runat="server" ForeColor="red" Text="">
                                </asp:Label>
                            </td>
                            <td>
                                <div style="float: right; margin-right: 0px;">
                                    <asp:Button runat="server" ID="btCancelar" Text="Cancelar" OnClick="btCancelar_Click"
                                        Enabled="true" />
                                </div>
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td colspan="5">
                            </td>
                        </tr>
                    </table>
                    <table border="0" cellpadding="0" class="tb" width="100%">
                        <tr>
                            <td style="width: 100%;">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvCores" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvCores_RowDataBound">
                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="COR" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Cor" HeaderStyle-Width="350px" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-VerticalAlign="Middle" HeaderText="Foto">
                                                <ItemTemplate>
                                                    <asp:Image ID="imgFoto" runat="server" Width="170" Height="70" BorderColor="White"
                                                        AlternateText="SEM FOTO" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="65px">
                                                <ItemTemplate>
                                                    <asp:Button ID="btAlterar" runat="server" Height="19px" Text="Alterar" OnClick="btAlterar_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="65px">
                                                <ItemTemplate>
                                                    <asp:Button ID="btExcluir" runat="server" Height="19px" Text="Excluir" OnClientClick="return ConfirmarExclusao();"
                                                        OnClick="btExcluir_Click" />
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
