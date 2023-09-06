<%@ Page Title="Cadastro de Comissão de Representante" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="atac_cad_rep_comissao.aspx.cs" Inherits="Relatorios.atac_cad_rep_comissao"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <script src="http://code.jquery.com/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do Atacado&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Apoio&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Comissão de Representante</span>
                <div style="float: right; padding: 0;">
                    <a href="atac_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1200px; margin-left: 8%;">
                <fieldset>
                    <legend>Cadastro de Comissão de Representante</legend>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="tb">
                        <tr style="height: 10px; vertical-align: bottom;">
                            <td>
                                <asp:Label ID="labRepresentante" runat="server" Text="Representante"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labComissão" runat="server" Text="Comissão"></asp:Label>
                            </td>
                            <td>
                                <asp:HiddenField ID="hidCodigo" runat="server" />
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 256px;">
                                <asp:DropDownList runat="server" ID="ddlRepresentante" DataValueField="REPRESENTANTE" DataTextField="REPRESENTANTE"
                                    Height="22px" Width="250px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 256px;">
                                <asp:DropDownList runat="server" ID="ddlFilial" Height="22px" Width="250px">
                                    <asp:ListItem Value="Selecione" Text="Selecione"></asp:ListItem>
                                    <asp:ListItem Value="ATACADO HANDBOOK" Text="ATACADO HANDBOOK"></asp:ListItem>
                                    <asp:ListItem Value="ATACADO HBF ITAJAÍ" Text="ATACADO HBF ITAJAÍ"></asp:ListItem>
                                    <asp:ListItem Value="ATACADO POP UP STORE" Text="ATACADO POP UP STORE"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 170px;">
                                <asp:TextBox ID="txtComissao" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                    Width="160px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btIncluir" Text="Incluir" OnClick="btIncluir_Click"
                                    Width="100px" Enabled="true" />
                            </td>
                            <td>
                                <div style="float: right; margin-right: 0px;">
                                    <asp:Button runat="server" ID="btCancelar" Text="Cancelar" OnClick="btCancelar_Click"
                                        Enabled="true" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="labErro" runat="server" ForeColor="red" Text="">
                                </asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td colspan="5"></td>
                        </tr>
                    </table>
                    <div style="width: 100%; overflow: auto; height: 380px;">
                        <div>
                            <table border="0" cellpadding="0" class="tb" width="100%">
                                <tr>
                                    <td style="width: 100%;">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvComissao" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvComissao_RowDataBound">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="REPRESENTANTE" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Representante" HeaderStyle-Width="250px" />
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Filial">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litFilial" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                        HeaderText="Comissão">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litComissao" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="65px">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btAlterar" runat="server" Height="19px" Text="Alterar" OnClick="btAlterar_Click" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" Visible="false" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="65px">
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
