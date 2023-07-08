<%@ Page Title="Alteração de Venda" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="rh_sol_alteracao_venda.aspx.cs" Inherits="Relatorios.rh_sol_alteracao_venda"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .jGrowl .manilla {
            color: black;
        }

        .jGrowl .redError {
            color: red;
        }

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo do RH&nbsp;&nbsp;>&nbsp;&nbsp;Solicitações&nbsp;&nbsp;>&nbsp;&nbsp;<asp:Label
                    ID="labTituloMenu" runat="server" Text="Alteração de Venda"></asp:Label></span>
                <div style="float: right; padding: 0;">
                    <a href="rh_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 962px; margin-left: 16%;">
                <fieldset>
                    <legend>
                        <asp:Label ID="labTitulo" runat="server" Text="Alteração de Venda"></asp:Label></legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="2">
                                <asp:Literal ID="litNumeroSolicitacao" runat="server" Text="" Visible="false"></asp:Literal>
                            </td>
                            <td colspan="2" style="text-align: right;">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="labFilial" runat="server" Text="Filial Atual"></asp:Label>
                                <asp:HiddenField ID="hidCodigoWF" runat="server" />
                            </td>
                            <td colspan="2">
                                <asp:Label ID="labFuncionarioDe" runat="server" Text="Funcionário De"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labFuncionarioPara" runat="server" Text="Para Funcionário"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="22px" Width="234px" OnSelectedIndexChanged="ddlFilial_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList runat="server" ID="ddlFuncionarioDe" DataValueField="CODIGO" DataTextField="NOME"
                                    Height="22px" Width="354px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlFuncionarioPara" DataValueField="CODIGO" DataTextField="NOME"
                                    Height="22px" Width="335px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="labTicket" runat="server" Text="Ticket"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labDataVenda" runat="server" Text="Data Venda"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labTerminal" runat="server" Text="Terminal"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labValor" runat="server" Text="Valor Venda"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 240px;">
                                <asp:TextBox ID="txtTicket" runat="server" MaxLength="10" Width="230px"></asp:TextBox>
                            </td>
                            <td style="width: 210px;">
                                <asp:TextBox ID="txtDataVenda" runat="server" MaxLength="10" Width="200px"
                                    onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                            </td>
                            <td style="width: 150px;">
                                <asp:TextBox ID="txtTerminal" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                    MaxLength="3" Width="140px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtValorVenda" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                    MaxLength="10" Width="170px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4"></td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;</td>
                        </tr>
                        <asp:Panel ID="pnlStatus" runat="server">
                            <tr>
                                <td>
                                    <asp:Label ID="labStatus" runat="server" Text="Status"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="labMotivo" runat="server" Text="Motivo"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlStatus" Height="22px" Width="234px" DataTextField="DESCRICAO" DataValueField="CODIGO">
                                        <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtMotivo" runat="server" Width="690px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                                <td style="text-align: right; line-height: 40px;">
                                    <asp:Button ID="btAlterarVenda" runat="server" Text="Alterar Venda" Width="110px" OnClick="btAlterarVenda_Click" OnClientClick="DesabilitarBotao(this);" Visible="false" />&nbsp;
                                    <asp:Button ID="btEnviar" runat="server" Text="Enviar" Width="110px" OnClick="btEnviar_Click"
                                        OnClientClick="DesabilitarBotao(this);" />
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr>
                            <td colspan="4">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="labHistoricoTitulo" runat="server" Visible="false" Text="Histórico da Solicitação" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvStatusHistorico" runat="server" Visible="false" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                        Style="background: white" OnRowDataBound="gvStatusHistorico_RowDataBound">
                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                        <FooterStyle BackColor="GradientActiveCaption" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data" HeaderStyle-Width="145px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Motivo" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litMotivo" runat="server"></asp:Literal>
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
            <div id="dialogPai" runat="server">
                <div id="dialog" title="Mensagem" class="divPop">
                    <table border="0" width="100%">
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; color: red;">
                                <strong>Solicitação:&nbsp;&nbsp;<asp:Label ID="labPopUp" runat="server" Text=""></asp:Label></strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; color: red;">
                                <asp:Label ID="labMensagem" runat="server" Text="ENVIADA COM SUCESSO"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>Por favor, selecione a tela desejada nos botões abaixo.
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
