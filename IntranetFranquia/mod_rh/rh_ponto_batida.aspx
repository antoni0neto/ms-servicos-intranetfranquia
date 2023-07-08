<%@ Page Title="Batidas" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="rh_ponto_batida.aspx.cs" Inherits="Relatorios.rh_ponto_batida"
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

        .alinharCentro {
            text-align: center;
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
        <Triggers>
            <asp:PostBackTrigger ControlID="btSalvar" />
        </Triggers>
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do RH&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Ponto&nbsp;&nbsp;>&nbsp;&nbsp;<asp:Label
                    ID="labTituloMenu" runat="server" Text="Batidas"></asp:Label></span>
                <div style="float: right; padding: 0;">
                    <a href="rh_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 87%;">
                <fieldset>
                    <legend>
                        <asp:Label ID="labTitulo" runat="server" Text="Batidas"></asp:Label></legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labDataBatida" runat="server" Text="Data Referência"></asp:Label>
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 260px;">
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="22px" Width="254px" OnSelectedIndexChanged="ddlFilial_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 220px;">
                                <asp:TextBox ID="txtDataBatida" runat="server" MaxLength="10" Width="210px"
                                    onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btGerarBatida" runat="server" Text="Gerar Batida" Width="110px" OnClick="btGerarBatida_Click"
                                    OnClientClick="DesabilitarBotao(this);" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Button ID="btCancelarBatida" runat="server" Text="Sair Batida" Width="110px" OnClick="btCancelarBatida_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvBatida" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvBatida_RowDataBound"
                                        OnDataBound="gvBatida_DataBound" DataKeyNames="CODIGO">
                                        <HeaderStyle BackColor="GradientActiveCaption" />
                                        <FooterStyle BackColor="GradientActiveCaption" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data Referência" HeaderStyle-Width="155px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litData" runat="server" Text="&nbsp;"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Nome" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNome" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tipo Batida" HeaderStyle-Width="140px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlTipoBatida" DataValueField="CODIGO" DataTextField="DESCRICAO"
                                                        Height="22px" Width="160px" OnSelectedIndexChanged="ddlTipoBatida_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Entrada 1" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtEntrada1" runat="server" Width="100px" MaxLength="5" OnTextChanged="txtHora_TextChanged" AutoPostBack="true"
                                                        onkeypress="return fnValidarHora(event);" CssClass="alinharCentro"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Saída 1" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtSaida1" runat="server" Width="100px" MaxLength="5" OnTextChanged="txtHora_TextChanged" AutoPostBack="true"
                                                        onkeypress="return fnValidarHora(event);" CssClass="alinharCentro"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Entrada 2" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtEntrada2" runat="server" Width="100px" MaxLength="5" OnTextChanged="txtHora_TextChanged" AutoPostBack="true"
                                                        onkeypress="return fnValidarHora(event);" CssClass="alinharCentro"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Saída 2" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtSaida2" runat="server" Width="100px" MaxLength="5" OnTextChanged="txtHora_TextChanged" AutoPostBack="true"
                                                        onkeypress="return fnValidarHora(event);" CssClass="alinharCentro"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" Visible="false" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btExcluir" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/delete.png"
                                                        OnClick="btExcluir_Click" OnClientClick="return ConfirmarExclusao();" ToolTip="Excluir Batida" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvBatidaNova" runat="server" Width="100%" AutoGenerateColumns="False" ShowHeader="false"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvBatida_RowDataBound"
                                        OnDataBound="gvBatida_DataBound" ShowFooter="true" DataKeyNames="CODIGO">
                                        <HeaderStyle BackColor="GradientActiveCaption" />
                                        <FooterStyle BackColor="GradientActiveCaption" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" FooterStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Literal ID="litColunaF" Text="&nbsp;" runat="server"></asp:Literal>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data Referência" FooterStyle-Width="155px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litData" runat="server" Text="&nbsp;"></asp:Literal>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Literal ID="litDataF" Text="&nbsp;" runat="server"></asp:Literal>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Nome" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNome" runat="server" Text="&nbsp;"></asp:Literal>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtNomeF" runat="server" Width="291px" ToolTip="Informe o nome do Vendedor com o Código. Ex: 9999 - JOÃO DA SILVA"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tipo Batida" FooterStyle-Width="140px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlTipoBatida" DataValueField="CODIGO" DataTextField="DESCRICAO"
                                                        Height="22px" Width="160px" OnSelectedIndexChanged="ddlTipoBatida_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlTipoBatidaF" DataValueField="CODIGO" DataTextField="DESCRICAO"
                                                        Height="22px" Width="160px" OnSelectedIndexChanged="ddlTipoBatida_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Entrada 1" FooterStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtEntrada1" runat="server" Width="100px" MaxLength="5" OnTextChanged="txtHora_TextChanged" AutoPostBack="true"
                                                        onkeypress="return fnValidarHora(event);" CssClass="alinharCentro"></asp:TextBox>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtEntrada1F" runat="server" Width="100px" MaxLength="5" OnTextChanged="txtHora_TextChanged" AutoPostBack="true"
                                                        onkeypress="return fnValidarHora(event);" CssClass="alinharCentro"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Saída 1" FooterStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtSaida1" runat="server" Width="100px" MaxLength="5" OnTextChanged="txtHora_TextChanged" AutoPostBack="true"
                                                        onkeypress="return fnValidarHora(event);" CssClass="alinharCentro"></asp:TextBox>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtSaida1F" runat="server" Width="100px" MaxLength="5" OnTextChanged="txtHora_TextChanged" AutoPostBack="true"
                                                        onkeypress="return fnValidarHora(event);" CssClass="alinharCentro"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Entrada 2" FooterStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtEntrada2" runat="server" Width="100px" MaxLength="5" OnTextChanged="txtHora_TextChanged" AutoPostBack="true"
                                                        onkeypress="return fnValidarHora(event);" CssClass="alinharCentro"></asp:TextBox>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtEntrada2F" runat="server" Width="100px" MaxLength="5" OnTextChanged="txtHora_TextChanged" AutoPostBack="true"
                                                        onkeypress="return fnValidarHora(event);" CssClass="alinharCentro"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Saída 2" FooterStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtSaida2" runat="server" Width="100px" MaxLength="5" OnTextChanged="txtHora_TextChanged" AutoPostBack="true"
                                                        onkeypress="return fnValidarHora(event);" CssClass="alinharCentro"></asp:TextBox>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtSaida2F" runat="server" Width="100px" MaxLength="5" OnTextChanged="txtHora_TextChanged" AutoPostBack="true"
                                                        onkeypress="return fnValidarHora(event);" CssClass="alinharCentro"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" FooterStyle-Width="30px" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTotal" runat="server"></asp:Literal>&nbsp;&nbsp;
                                                    <asp:ImageButton ID="btExcluir" runat="server" Height="13px" Width="13px" ImageUrl="~/Image/delete.png"
                                                        OnClick="btExcluir_Click" OnClientClick="return ConfirmarExclusao();" ToolTip="Excluir Batida" />&nbsp;
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Button ID="btIncluirFuncionario" runat="server" Width="90px" Text="Incluir"
                                                        OnClick="btIncluirFuncionario_Click" ToolTip="Incluir" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="text-align: right;">
                                <asp:Literal ID="labAjuda" runat="server" Visible="false" Text="Para o ponto ficar <strong>Completo</strong>, não pode existir Tipo de Batida <font color='red'><strong>'A Preencher'</strong></font>."></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="labMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </td>
                            <td colspan="1" style="text-align: right;">
                                <asp:CheckBox ID="chkEmail" runat="server" Checked="false" Visible="false" Text="Enviar e-mail" />
                                &nbsp;
                                <asp:Button ID="btSalvar" runat="server" Width="110px" Text="Salvar" Visible="false"
                                    OnClick="btSalvar_Click" OnClientClick="DesabilitarBotao(this);" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div style="float: right; margin-top: 27px;">
                <table border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <td colspan="2">
                            <asp:Calendar ID="calBatida" runat="server" OnDayRender="calBatida_DayRender" OnVisibleMonthChanged="calBatida_VisibleMonthChanged"></asp:Calendar>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>Pendente
                        </td>
                        <td style="background-color: #E6E6FA;">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>Completo
                        </td>
                        <td style="background-color: #9AFF9A; width: 70px;">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>Incompleto
                        </td>
                        <td style="background-color: #CD5C5C;">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>Feriado NÃO Trab
                        </td>
                        <td style="background-color: #87CEFF;">&nbsp;
                        </td>
                    </tr>
                </table>

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
                                <strong>BATIDA:&nbsp;&nbsp;<asp:Literal ID="litPopUp" runat="server" Text=""></asp:Literal></strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; color: red;">
                                <asp:Label ID="labMensagem" runat="server" Text="FECHADA COM SUCESSO"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
