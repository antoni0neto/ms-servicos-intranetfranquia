<%@ Page Title="Cadastro de Feriado" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="rh_cad_feriado.aspx.cs" Inherits="Relatorios.rh_cad_feriado"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo do RH&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Apoio&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Feriado</span>
                <div style="float: right; padding: 0;">
                    <a href="rh_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1200px; margin-left: 8%;">
                <fieldset>
                    <legend>Cadastro de Feriado</legend>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="tb">
                        <tr style="height: 10px; vertical-align: bottom;">
                            <td>
                                <asp:Label ID="labDescricao" runat="server" Text="Feriado"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labData" runat="server" Text="Data"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labTipo" runat="server" Text="Tipo"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labTrabalhado" runat="server" Text="Trabalhado"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labStatus" runat="server" Text="Status"></asp:Label>
                            </td>
                            <td>
                                <asp:HiddenField ID="hidCodigo" runat="server" />
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 190px;">
                                <asp:TextBox ID="txtDescricao" runat="server" Width="180px"></asp:TextBox>
                            </td>
                            <td style="width: 130px;">
                                <asp:TextBox ID="txtData" runat="server" Width="120px" MaxLength="10" Style="text-align: right;"
                                    onkeypress="return fnValidarData(event);"></asp:TextBox>
                            </td>
                            <td style="width: 256px;">
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="22px" Width="250px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 130px;">
                                <asp:DropDownList ID="ddlTipo" runat="server" Width="124px" Height="22px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="L" Text="Local"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Nacional"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 130px;">
                                <asp:DropDownList ID="ddlTrabalhado" runat="server" Width="124px" Height="22px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 100px;">
                                <asp:DropDownList ID="ddlStatus" Height="22px" runat="server" Width="90px">
                                    <asp:ListItem Text="Selecione" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Ativo" Selected="True" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="Inativo" Value="I"></asp:ListItem>
                                </asp:DropDownList>
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
                            <td colspan="8">
                                <asp:Label ID="labErro" runat="server" ForeColor="red" Text="">
                                </asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td colspan="8"></td>
                        </tr>
                    </table>
                    <div style="width: 100%; overflow: auto; height: 380px;">
                        <div>
                            <table border="0" cellpadding="0" class="tb" width="100%">
                                <tr>
                                    <td style="width: 100%;">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvFeriado" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvFeriado_RowDataBound">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="DESCRICAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Descrição" HeaderStyle-Width="250px" />
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                        HeaderText="Data" HeaderStyle-Width="150px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Filial">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litFilial" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Tipo">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTipo" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Trabalhado">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litTrabalhado" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
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
                                                    <asp:TemplateField HeaderText="" Visible="false" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="65px">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btExcluir" runat="server" Height="19px" Text="Excluir" OnClientClick="return ConfirmarExclusao();"
                                                                OnClick="btExcluir_Click" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:ImageButton ID="btIncluir" runat="server" Text="Incluir" Width="100px" OnClick="btIncluir_Click" />
                                                        </FooterTemplate>
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
