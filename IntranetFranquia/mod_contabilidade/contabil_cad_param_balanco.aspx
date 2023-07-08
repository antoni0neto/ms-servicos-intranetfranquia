<%@ Page Title="Manutenção de Contas do Balanço" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="contabil_cad_param_balanco.aspx.cs" Inherits="Relatorios.mod_financeiro.contabil_cad_param_balanco" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharEsquerda {
            text-align: left;
        }
    </style>

    <script type="text/javascript" src="../js/1.9.0/jquery.min.js"></script>
    <script type="text/javascript" src="../js/js.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo de Contabilidade&nbsp;&nbsp;>&nbsp;&nbsp;Apoio&nbsp;&nbsp;>&nbsp;&nbsp;Manutenção de Contas do Balanço</span>
        <div style="float: right; padding: 0;">
            <a href="contabil_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div>
        <fieldset>
            <legend>Manutenção de Contas do Balanço</legend>
            <asp:UpdatePanel ID="up1" runat="server">
                <ContentTemplate>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="labGrupo" runat="server" Text="Grupo"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labSubGrupo" runat="server" Text="SubGrupo"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labClassificacao" runat="server" Text="Classificação"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labConta" runat="server" Text="Conta"></asp:Label>
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 200px;">
                                <asp:DropDownList ID="ddlGrupo" Height="21px" runat="server" Width="194px" OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                    <asp:ListItem Value="ATIVO" Text="ATIVO"></asp:ListItem>
                                    <asp:ListItem Value="PASSIVO" Text="PASSIVO"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 200px;">
                                <asp:DropDownList ID="ddlSubGrupo" runat="server" Width="194px" Height="21px" OnSelectedIndexChanged="ddlSubGrupo_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 210px;">
                                <asp:DropDownList ID="ddlClassificacao" runat="server" Width="204px" Height="21px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 450px;">
                                <asp:DropDownList ID="ddlConta" runat="server" Width="444px" Height="21px" DataValueField="CLASSIFICACAO" DataTextField="DESCR_CONTA">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 110px;">
                                <asp:Button runat="server" ID="btIncluir" Width="100px" Text="Incluir" OnClick="btIncluir_Click"
                                    Enabled="true" />

                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Label ID="labErro" runat="server" ForeColor="red" Text="">
                                </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">&nbsp;
                            </td>
                        </tr>
                    </table>
                    <div>
                        <div class="rounded_corners">
                            <asp:GridView ID="gvParamBalanco" runat="server" AutoGenerateColumns="False" ShowFooter="true"
                                OnRowDataBound="gvParamBalanco_RowDataBound"
                                AllowSorting="true" OnSorting="gvParamBalanco_Sorting"
                                ForeColor="#333333" Style="background: white; width: 1450px; border-collapse: collapse;">
                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Grupo" HeaderStyle-HorizontalAlign="Left"
                                        SortExpression="GRUPO" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Literal ID="litGrupo" runat="server" Text='<%# Bind("GRUPO") %>'></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SubGrupo" HeaderStyle-HorizontalAlign="Left"
                                        SortExpression="SUBGRUPO" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Literal ID="litSubGrupo" runat="server" Text='<%# Bind("SUBGRUPO") %>'></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Classificação" HeaderStyle-HorizontalAlign="Left"
                                        SortExpression="CLASSIFICACAO" HeaderStyle-Width="300px" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Literal ID="litClassificacao" runat="server" Text='<%# Bind("CLASSIFICACAO") %>'></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Conta" HeaderStyle-HorizontalAlign="Left"
                                        SortExpression="CONTA" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Literal ID="litConta" runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="24px">
                                        <ItemTemplate>
                                            <asp:Button ID="btExcluir" runat="server" Height="19px" Text="Excluir"
                                                OnClientClick="return ConfirmarExclusao();"
                                                CommandArgument='<%# Bind("CODIGO") %>'
                                                OnClick="btExcluir_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
    </div>
</asp:Content>
