<%@ Page Title="Depósito em Banco" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="fin_deposito_banco.aspx.cs" Inherits="Relatorios.fin_deposito_banco" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
    </style>

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>

    <script type="text/javascript">


    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">
            <asp:Label ID="labTitulo" runat="server" Text="Administração de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Fechamento&nbsp;&nbsp;>&nbsp;&nbsp;Depósito em Banco"></asp:Label></span>
        <div style="float: right; padding: 0;">
            <a href="~/mod_adm_loja/admloj_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="">
        <fieldset class="login">
            <legend>Depósitos Pendentes</legend>
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top">
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td valign="top">Filial
                                </td>
                                <td valign="top">Data Início
                                </td>
                                <td valign="top">Data Fim
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 250px;" valign="top">
                                    <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Width="244px" Height="21px" OnSelectedIndexChanged="ddlFilial_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                </td>
                                <td style="width: 130px;" valign="top">
                                    <asp:TextBox ID="txtDataini" runat="server" autocomplete="off" Width="120px" MaxLength="10" Style="text-align: right;" onkeypress="return fnValidarData(event);"></asp:TextBox>
                                </td>
                                <td style="width: 130px;" valign="top">
                                    <asp:TextBox ID="txtDataFim" runat="server" autocomplete="off" Width="120px" MaxLength="10" Style="text-align: right;" onkeypress="return fnValidarData(event);"></asp:TextBox>
                                </td>
                                <td valign="top">
                                    <asp:Button runat="server" ID="btBuscarDeposito" Text="Buscar" Width="100px" OnClick="btBuscarDeposito_Click" />&nbsp;
                        <asp:Label ID="labErro" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvMovimento" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true"
                                            OnDataBound="gvMovimento_DataBound">
                                            <HeaderStyle BackColor="Gainsboro" />
                                            <FooterStyle BackColor="Gainsboro" Font-Bold="true" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="data" HeaderText="Data do Faturamento" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="250px" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="valor_deposito" HeaderText="Saldo em Dinheiro" HeaderStyle-HorizontalAlign="Left" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 25px;">&nbsp;
                    </td>
                    <td valign="top" style="width: 230px; text-align: center;">
                        <fieldset style="width: 230px;">
                            <legend>Depósitos Realizados</legend>
                            <asp:Calendar ID="calDepositoRealizado" runat="server" CaptionAlign="Bottom"></asp:Calendar>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset>
            <legend>Lançamentos</legend>
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <div class="rounded_corners">
                            <asp:GridView ID="gridALancar" runat="server" Width="100%" AutoGenerateColumns="False">
                                <HeaderStyle BackColor="Gainsboro" />
                                <FooterStyle BackColor="Gainsboro" Font-Bold="true" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Lançar" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkLancar" runat="server" OnCheckedChanged="chkLancar_CheckedChanged" AutoPostBack="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="data" HeaderText="Data do Faturamento" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="160px" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="valor_deposito" HeaderText="Saldo em Dinheiro" HeaderStyle-Width="220px" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="" HeaderText="" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" style="width: 310px;">
                        <fieldset style="width: 310px; margin-top: 0px; height: 255px;">
                            <legend>Dados do Depósito</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 150px;">Valor A Depositar</td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtValorADepositar" runat="server" CssClass="pcRight" MaxLength="20" Height="16px" Width="150px" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="line-height: 5px;">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Valor Depositado</td>
                                    <td style="width: 80px;">
                                        <asp:TextBox ID="txtValorDepositadoCalculado" runat="server" CssClass="pcRight" MaxLength="20" Height="16px" Width="70px" Enabled="False" AutoPostBack="True" OnTextChanged="txtValorDepositadoCalculado_TextChanged"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValorDepositado" runat="server" CssClass="pcRight" MaxLength="20" Height="16px" Width="70px" AutoPostBack="True" OnTextChanged="txtValorDepositado_TextChanged"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="line-height: 5px;">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Diferença</td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtDiferenca" runat="server" CssClass="pcRight" MaxLength="20" Height="16px" Width="150px" Enabled="False"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td style="line-height: 5px;">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Assinatura</td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAssinatura" runat="server" CssClass="pcRight" MaxLength="100" Height="16px" Width="150px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="line-height: 5px;">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Diferença Anterior</td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtDiferencaAnterior" runat="server" CssClass="pcRight" MaxLength="20" Text="-" Height="16px" Width="150px" Enabled="False"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td style="line-height: 5px;">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Banco Depositado</td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtBancoDepositado" runat="server" CssClass="pcRight" MaxLength="20" Height="16px" Width="150px" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="line-height: 5px;">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Ag./Conta</td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAgenciaConta" runat="server" CssClass="pcRight" MaxLength="20" Height="16px" Width="150px" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                    <td style="width: 15px;">&nbsp;</td>
                    <td valign="top" style="width: 230px;">
                        <fieldset style="width: 230px; margin-top: 0px; height: 255px;">
                            <legend>Informações do Comprovante</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>Data do Depósito</td>
                                    <td>Hora</td>
                                </tr>
                                <tr>
                                    <td style="width: 120px;">
                                        <asp:TextBox ID="txtDataDeposito" runat="server" AutoPostBack="true" Width="110px"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txtHoraDeposito" runat="server" MaxLength="5" AutoPostBack="true" Width="106px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="line-height: 5px;">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Calendar ID="calDataDeposito" runat="server" OnSelectionChanged="calDataDeposito_SelectionChanged" CaptionAlign="Bottom" Width="100%"></asp:Calendar>
                                        <asp:Label runat="server" ID="labInfoHora" ForeColor="Red" Font-Size="Smaller">*Hora do depósito no comprovante</asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                    <td style="width: 15px;">&nbsp;</td>
                    <td valign="top">
                        <fieldset style="margin-top: 0px; height: 255px;">
                            <legend>Imagens dos Comprovantes</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <div>
                                            <label>
                                                Comprovante 1:</label>
                                            <asp:FileUpload ID="FileUpload1" runat="server" Width="300px" OnPreRender="UploadButton1_Click" />
                                            <asp:Label runat="server" ID="LabelFeedBack1" ForeColor="Red"></asp:Label>
                                            <asp:Button runat="server" ID="btAtualizar1" Text="Atualizar Imagem 1" OnClick="btAtualizar1_Click"
                                                ValidationGroup="valores" Width="300px" CausesValidation="true" Enabled="false" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="line-height: 10px;">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <label>
                                                Comprovante 2:</label>
                                            <asp:FileUpload ID="FileUpload2" runat="server" Width="300px" OnPreRender="UploadButton2_Click" />
                                            <asp:Label runat="server" ID="LabelFeedBack2" ForeColor="Red"></asp:Label>
                                            <asp:Button runat="server" ID="btAtualizar2" Text="Atualizar Imagem 2" OnClick="btAtualizar2_Click"
                                                ValidationGroup="valores" Width="300px" CausesValidation="true" Enabled="false" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="line-height: 10px;">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <label>
                                                Comprovante 3:</label>
                                            <asp:FileUpload ID="FileUpload3" runat="server" Width="300px" OnPreRender="UploadButton3_Click" />
                                            <asp:Label runat="server" ID="LabelFeedBack3" ForeColor="Red"></asp:Label>
                                            <asp:Button runat="server" ID="btAtualizar3" Text="Atualizar Imagem 3" OnClick="btAtualizar3_Click"
                                                ValidationGroup="valores" Width="300px" CausesValidation="true" Enabled="false" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="line-height: 10px;">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <label>
                                                Comprovante 4:</label>
                                            <asp:FileUpload ID="FileUpload4" runat="server" Width="300px" OnPreRender="UploadButton4_Click" />
                                            <asp:Label runat="server" ID="LabelFeedBack4" ForeColor="Red"></asp:Label>
                                            <asp:Button runat="server" ID="btAtualizar4" Text="Atualizar Imagem 4" OnClick="btAtualizar4_Click"
                                                ValidationGroup="valores" Width="300px" CausesValidation="true" Enabled="false" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
            </table>
            <div>
                <asp:Button runat="server" ID="btGravar" Text="Gravar Depósito" Width="140px" OnClick="btGravar_Click" Height="22px" />
                <asp:Label runat="server" ID="lblMensagem" ForeColor="Red"></asp:Label>
            </div>
        </fieldset>
    </div>
</asp:Content>
