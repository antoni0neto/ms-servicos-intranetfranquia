<%@ Page Title="Processos" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="jur_processo_cad.aspx.cs" Inherits="Relatorios.jur_processo_cad"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .jGrowl .manilla
        {
            color: black;
        }
        .jGrowl .redError
        {
            color: red;
        }
        .alinharDireita
        {
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo Jurídico&nbsp;&nbsp;>&nbsp;&nbsp;Processos&nbsp;&nbsp;>&nbsp;&nbsp;<asp:Label
                    ID="labTituloMenu" runat="server" Text="Novo Processo"></asp:Label></span>
                <div style="float: right; padding: 0;">
                    <a href="jur_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">
                        Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 962px; margin-left: 16%;">
                <fieldset>
                    <legend>
                        <asp:Label ID="labTitulo" runat="server" Text="Novo Processo"></asp:Label></legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="labNumero" runat="server" Text="Número"></asp:Label>
                                <asp:HiddenField ID="hidProcesso" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="labDataRecebimento" runat="server" Text="Data de Recebimento"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labRequerente" runat="server" Text="Requerente"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labCargo" runat="server" Text="Cargo"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labTipoProcesso" runat="server" Text="Tipo de Processo"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 220px;">
                                <asp:TextBox ID="txtNumeroProcesso" runat="server" MaxLength="50" Width="210px"></asp:TextBox>
                            </td>
                            <td style="width: 180px;">
                                <asp:TextBox ID="txtDataRecebimento" runat="server" MaxLength="10" Width="170px"
                                    onkeypress="return fnValidarData(event);"></asp:TextBox>
                            </td>
                            <td style="width: 180px;">
                                <asp:TextBox ID="txtRequerente" runat="server" Width="170px" MaxLength="50"></asp:TextBox>
                            </td>
                            <td style="width: 180px;">
                                <asp:TextBox ID="txtCargo" runat="server" Width="170px" MaxLength="50"></asp:TextBox>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTipoProcesso" Height="21px" runat="server" DataValueField="CODIGO"
                                    DataTextField="DESCRICAO" Width="175px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="labTipoInstancia" runat="server" Text="Fase"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labDataJulgamento" runat="server" Text="Data de Julgamento"></asp:Label>
                            </td>
                            <td>
                                Data de Sentença
                            </td>
                            <td>
                                Forma de Pagamento
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlTipoInstancia" Height="21px" runat="server" DataValueField="CODIGO"
                                    DataTextField="DESCRICAO" Width="214px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDataJulgamento" runat="server" MaxLength="10" Width="170px" onkeypress="return fnValidarData(event);"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDataCondenacao" runat="server" MaxLength="10" Width="170px" onkeypress="return fnValidarData(event);"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtFormaPgto" runat="server" MaxLength="50" Width="350px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                Observação
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:TextBox ID="txtObservacao" runat="server" Width="930px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                            <td colspan="2" style="text-align: right;">
                                <asp:Button ID="btIncluirProcesso" runat="server" Text="Incluir" Width="110px" OnClick="btIncluirProcesso_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvProcesso" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvProcesso_RowDataBound"
                                        DataKeyNames="CODIGO">
                                        <HeaderStyle BackColor="GradientActiveCaption" />
                                        <FooterStyle BackColor="GradientActiveCaption" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Fase" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="180px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litInstancia" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data de Julgamento" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="135px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDataJulgamento" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data de Sentença" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="140px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDataCondenacao" runat="server" MaxLength="10" Width="140px" OnTextChanged="txt_TextChanged"
                                                        onkeypress="return fnReadOnly(event);" Style="text-align: center;" AutoPostBack="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Forma de Pagamento" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="190px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtFormaPgto" runat="server" MaxLength="50" Width="190px" OnTextChanged="txt_TextChanged"
                                                        AutoPostBack="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Observação" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="143px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtObservacao" runat="server" Width="143px" OnTextChanged="txt_TextChanged"
                                                        AutoPostBack="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Button ID="btExcluir" runat="server" Width="108px" Text="Excluir" OnClick="btExcluir_Click"
                                                        OnClientClick="return ConfirmarExclusao();" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btExcluirProcesso" runat="server" Width="110px" Text="Excluir" Visible="false"
                                    OnClick="btExcluirProcesso_Click" OnClientClick="return ConfirmarExclusao();" />
                            </td>
                            <td colspan="2">
                                <asp:Label ID="labMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </td>
                            <td colspan="2" style="text-align: right;">
                                <asp:CheckBox ID="chkEmail" runat="server" Checked="false" Visible="false" Text="Enviar e-mail" />
                                &nbsp;
                                <asp:Button ID="btSalvar" runat="server" Width="110px" Text="Salvar" Visible="false"
                                    OnClick="btSalvar_Click" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div id="dialogPai" runat="server">
                <div id="dialog" title="Mensagem" class="divPop">
                    <table border="0" width="100%">
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; color: red;">
                                <strong>Processo:&nbsp;&nbsp;<asp:Label ID="labPopUp" runat="server" Text=""></asp:Label></strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; color: red;">
                                <asp:Label ID="labMensagem" runat="server" Text="CADASTRADO COM SUCESSO"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Por favor, selecione a tela desejada nos botões abaixo.
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
