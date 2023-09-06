<%@ Page Title="Custo por Peso" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="fre_cad_valor_peso.aspx.cs" Inherits="Relatorios.mod_frete.fre_cad_valor_peso" %>

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
            $("#<%=gvCadValorPeso.ClientID %>")
              .thfloat();

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Frete&nbsp;&nbsp;>&nbsp;&nbsp;
                    Cadastros&nbsp;&nbsp;>&nbsp;&nbsp;Custo por Peso</span>
                <div style="float: right; padding: 0;">
                    <a href="fre_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1000px; margin-left: 15%;">
                <fieldset>
                    <legend>Cadastro de Custo por Peso</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr style="height: 10px; vertical-align: bottom;">
                            <td>
                                <asp:Label ID="labTipoEnvio" runat="server" Text="Tipo Envio"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labPeso" runat="server" Text="Peso"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labValor" runat="server" Text="Valor Unitário"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labDescontoPorcentagem" runat="server" Text="% Desconto"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:HiddenField ID="hidCodigo" runat="server" Value="0" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">
                                <asp:DropDownList ID="ddlTipoEnvio" DataValueField="CODIGO" DataTextField="DESCRICAO" Height="21px" runat="server" Width="144px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 120px;">
                                <asp:TextBox ID="txtPeso" runat="server" MaxLength="8" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                    Width="110px"></asp:TextBox>

                            </td>
                            <td style="width: 120px;">
                                <asp:TextBox ID="txtValor" runat="server" MaxLength="8" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                    Width="110px"></asp:TextBox>

                            </td>
                            <td style="width: 120px;">
                                <asp:TextBox ID="txtDescontoPorcentagem" MaxLength="3" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                    Width="110px"></asp:TextBox>
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
                            <td colspan="6">
                                <asp:Label ID="labErro" runat="server" ForeColor="red" Text="">
                                </asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td colspan="6"></td>
                        </tr>
                    </table>
                    <div style="width: 100%; overflow: auto; height: 450px;">
                        <div>

                            <div class="rounded_corners">
                                <asp:GridView ID="gvCadValorPeso" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CODIGO" Visible="false" />
                                        <asp:TemplateField HeaderText="Tipo Envio" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litTipoEnvio" Text='<%#ObterTipoEnvio(Int32.Parse(Eval("FRETE_TIPO_ENVIO").ToString())).DESCRICAO %>' runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PESO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Peso" HeaderStyle-Width="150px" />
                                        <asp:BoundField DataField="VALOR" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Valor Unitário" DataFormatString="{0:c}" />
                                        <asp:BoundField DataField="DESCONTO_PORC" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="% Desconto" />
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="55px">
                                            <ItemTemplate>
                                                <asp:Button ID="btAlterar" runat="server" Height="19px" CommandArgument='<%#Eval("CODIGO") %>' Text="Alterar" OnClick="btAlterar_Click" />
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
