<%@ Page Title="Cadastro de Fase" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="jur_instancia_cad.aspx.cs" Inherits="Relatorios.jur_instancia_cad"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo Jurídico&nbsp;&nbsp;>&nbsp;&nbsp;Cadastros&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Fase</span>
                <div style="float: right; padding: 0;">
                    <a href="jur_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1200px; margin-left: 8%;">
                <fieldset>
                    <legend>Cadastro de Fase</legend>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="tb">
                        <tr style="height: 10px; vertical-align: bottom;">
                            <td style="width: 180px;">
                                <asp:Label ID="labDescricao" runat="server" Text="Descrição"></asp:Label>
                            </td>
                            <td style="width: 100px;">
                                <asp:Label ID="labOrdem" runat="server" Text="Ordem"></asp:Label>
                            </td>
                            <td style="width: 100px;">
                                <asp:Label ID="labStatus" runat="server" Text="Status"></asp:Label>
                            </td>
                            <td>
                                <asp:HiddenField ID="hidCodigo" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 190px;">
                                <asp:TextBox ID="txtDescricao" runat="server" Width="180px"></asp:TextBox>
                            </td>
                            <td style="width: 100px;">
                                <asp:TextBox ID="txtOrdem" runat="server" Width="90px" CssClass="alinharDireita"
                                    onkeypress="return fnValidarNumero(event);" MaxLength="1"></asp:TextBox>
                            </td>
                            <td style="width: 100px;">
                                <asp:DropDownList ID="ddlStatus" Height="21px" runat="server" Width="90px">
                                    <asp:ListItem Text="Selecione" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Ativo" Selected="True" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="Inativo" Value="I"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 600px;">
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
                    <div style="width: 100%; overflow: auto; height: 380px;">
                        <div>
                            <table border="0" cellpadding="0" class="tb" width="100%">
                                <tr>
                                    <td style="width: 100%;">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvTipoInstancia" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvTipoInstancia_RowDataBound">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="DESCRICAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Descrição" HeaderStyle-Width="320px" />
                                                    <asp:BoundField DataField="ORDEM" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Ordem" HeaderStyle-Width="102px" />
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Status">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litStatus" runat="server"></asp:Literal>
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
                        </div>
                    </div>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
