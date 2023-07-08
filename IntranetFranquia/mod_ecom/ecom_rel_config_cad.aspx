<%@ Page Title="Relacionados - Cadastro de Configurações" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="ecom_rel_config_cad.aspx.cs" Inherits="Relatorios.ecom_rel_config_cad"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">

    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Produtos Relacionados&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Configurações</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Cadastro de Configurações"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Griffe</td>
                        <td>Grupo Produto</td>
                        <td>Nome
                        </td>
                        <td>Tipo
                        </td>
                        <td>
                            <asp:HiddenField ID="hidCodigoConfig" runat="server" Value="" />
                            <asp:HiddenField ID="hidGriffe" runat="server" Value="" />
                            <asp:HiddenField ID="hidGrupoProduto" runat="server" Value="" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 200px">
                            <asp:DropDownList ID="ddlGriffe" runat="server" Width="194px" Height="21px" DataTextField="GRIFFE"
                                DataValueField="GRIFFE">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 200px">
                            <asp:DropDownList ID="ddlGrupo" runat="server" Width="194px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                DataValueField="GRUPO_PRODUTO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 350px;">
                            <asp:TextBox ID="txtNome" runat="server" Width="340px" MaxLength="100" />
                        </td>
                        <td style="width: 200px;">
                            <asp:DropDownList ID="ddlTipo" runat="server" Width="194px">
                                <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Regra Entrada"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Regra Saída"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btSalvar" runat="server" Width="100px" Text="Incluir" OnClick="btSalvar_Click" />&nbsp;
                            <asp:Button ID="btLimpar" runat="server" Width="100px" Text="Limpar" OnClick="btLimpar_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Panel ID="pnlFiltro" runat="server" Visible="false">
                                <fieldset>
                                    <legend>Configurações</legend>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>Campo
                                            </td>
                                            <td>Operação
                                            </td>
                                            <td>Valor</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style="width: 210px;">
                                                <asp:DropDownList ID="ddlFiltro" runat="server" Width="204px" DataTextField="CAMPO_WHERE" DataValueField="CODIGO" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 170px;">
                                                <asp:DropDownList ID="ddlOperacao" runat="server" Width="164px" DataTextField="OPERACAO" DataValueField="CODIGO">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 210px;">
                                                <asp:DropDownList ID="ddlValores" runat="server" Width="204px" DataTextField="TEXT" DataValueField="VALUE">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Button ID="btAddFiltro" runat="server" Width="100px" Text="Adicionar" OnClick="btAddFiltro_Click" />
                                                &nbsp;
                                                <asp:Label ID="labErroFiltro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvCampos" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvCampos_RowDataBound"
                                                        OnDataBound="gvCampos_DataBound" ShowFooter="true"
                                                        DataKeyNames="CODIGO">
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Campo" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="172px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litCampo" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Operação" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="172px">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litOperacao" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Valores" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:ListBox ID="lboxValores" runat="server" SelectionMode="Multiple" Width="600px"
                                                                        Height="80px"></asp:ListBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="65px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btExcluir" runat="server" Text="Excluir" Width="65px" OnClick="btExcluir_Click" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
