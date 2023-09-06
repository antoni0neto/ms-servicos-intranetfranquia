<%@ Page Title="Tipos de Pagamento" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="fre_cad_tipo_pagamento.aspx.cs" Inherits="Relatorios.mod_frete.fre_cad_tipo_pagamento" %>

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
            $("#<%=gvCadTipoPagamento.ClientID %>")
              .thfloat();

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Frete&nbsp;&nbsp;>&nbsp;&nbsp;Cadastros&nbsp;&nbsp;>&nbsp;&nbsp;Tipos de Pagamento</span>
                <div style="float: right; padding: 0;">
                    <a href="fre_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1000px; margin-left: 15%;">
                <fieldset>
                    <legend>Cadastro de Tipos de Pagamento</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr style="height: 10px; vertical-align: bottom;">
                            <td>
                                <asp:Label ID="labDescricao" runat="server" Text="Descrição"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labStatus" runat="server" Text="Status"></asp:Label>
                            </td>                            
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 200px;">
                                <asp:TextBox ID="txtDescricao" MaxLength="50" runat="server" Width="190px"></asp:TextBox>
                            </td>
                            <td style="width: 120px;">
                                <asp:DropDownList ID="ddlStatus" Height="21px" runat="server" Width="114px">
                                    <asp:ListItem Text="Selecione" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Ativo" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Inativo" Value="0"></asp:ListItem>
                                </asp:DropDownList>
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
                            <td colspan="4">
                            </td>
                        </tr>
                    </table>
                    <div style="width: 100%; overflow: auto; height: 450px;">
                        <div>

                            <div class="rounded_corners">
                                <asp:GridView ID="gvCadTipoPagamento" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CODIGO" Visible="false" />
                                        <asp:BoundField DataField="DESCRICAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Descrição" />
                                        <asp:TemplateField HeaderText="Status" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litStatus" Text='<%#Eval("STATUS").ToString().Equals("1") ? "Ativo": "Inativo" %>' runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
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
