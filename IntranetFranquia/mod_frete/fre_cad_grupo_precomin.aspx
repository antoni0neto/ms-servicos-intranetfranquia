<%@ Page Title="Grupos e Preço Mínimo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="fre_cad_grupo_precomin.aspx.cs" Inherits="Relatorios.mod_frete.fre_cad_grupo_precomin" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <script type="text/javascript" src="../js/1.9.0/jquery.min.js"></script>
    <script type="text/javascript" src="../js/js.js"></script>
    <script type="text/javascript" src="../js/fixheader/jquery.freezeheader.js"></script>

    <script type="text/javascript">

        function travaHeaderGrid() {
            $("#<%=gvCadGrupoPrecoMin.ClientID %>")
              .thfloat();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Frete&nbsp;&nbsp;>&nbsp;&nbsp;Cadastros&nbsp;&nbsp;>&nbsp;&nbsp;Grupos e Preço Mínimo</span>
                <div style="float: right; padding: 0;">
                    <a href="fre_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1000px; margin-left: 15%;">
                <fieldset>
                    <legend>Cadastro de Grupos e Preço Mínimo</legend>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                        <tr style="height: 10px; vertical-align: bottom;">
                            <td>
                                <asp:Label ID="labGrupoProduto" runat="server" Text="Grupo"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labValorUnitario" runat="server" Text="Valor Unitário"></asp:Label>
                            </td>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 200px;">
                                <asp:DropDownList ID="ddlGrupoProduto" DataValueField="GRUPO_PRODUTO"
                                    DataTextField="GRUPO_PRODUTO" Height="21px" runat="server" Width="194px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 150px;">
                                <asp:TextBox ID="txtValorUnitario" MaxLength="8" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                    Width="140px"></asp:TextBox>

                            </td>
                            <td>
                                <asp:Button runat="server" ID="btIncluir" CommandArgument="I" Text="Incluir" OnClick="btIncluir_Click"
                                    Width="100px" Enabled="true" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btCancelar" Width="100px" Text="Cancelar" OnClick="btCancelar_Click"
                                    Enabled="true" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="labErro" runat="server" ForeColor="red" Text="">
                                </asp:Label>
                                <asp:HiddenField ID="hidCodigo" runat="server" Value="0" />
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td colspan="4">&nbsp;
                            </td>
                        </tr>
                    </table>
                    <div style="width: 100%; overflow: auto; height: 450px;">
                        <div>
                            <div class="rounded_corners">
                                <asp:GridView ID="gvCadGrupoPrecoMin" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white; border-collapse: collapse;">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="GRUPO_PRODUTO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Grupo" />
                                        <asp:BoundField DataField="VALOR_UNITARIO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Valor Unitário" HeaderStyle-Width="250px" DataFormatString="{0:c}" />
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="55px">
                                            <ItemTemplate>
                                                <asp:Button ID="btAlterar" runat="server" Height="19px" CommandArgument='<%#Eval("GRUPO_PRODUTO") %>' Text="Alterar" OnClick="btAlterar_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-Width="55px">
                                            <ItemTemplate>
                                                <asp:Button ID="btExcluir" runat="server" Height="19px" Text="Excluir"
                                                    CommandArgument='<%#Eval("CODIGO") %>'
                                                    OnClientClick="return ConfirmarExclusao();"
                                                    OnClick="btExcluir_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
