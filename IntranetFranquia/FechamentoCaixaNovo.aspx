<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="FechamentoCaixaNovo.aspx.cs" Inherits="Relatorios.FechamentoCaixaNovo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .textEntry
        {
        }
    </style>
    <script type="text/javascript" src="jquery-1.6.min.js"></script>
    <script type="text/javascript" src="jquery.maskedinput-1.3.min.js"></script>
    <script type="text/javascript">
        jQuery(function () {

            $(".txtDinheiroDeposito").mask("99999,99");
            $(".cpf").mask("999.999.999-99");
            $(".cep").mask("99999-999");
            $(".data").mask("99/99/9999");

        });
        $(document).ready(function () {
            $('input').keypress(function (e) {
                var code = null;
                code = (e.keyCode ? e.keyCode : e.which);
                return (code == 13) ? false : true;
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">
            <asp:Label ID="labTitulo" runat="server" Text="Administração de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Fechamento&nbsp;&nbsp;>&nbsp;&nbsp;Fechamento Caixa"></asp:Label></span>
        <div style="float: right; padding: 0;">
            <a href="~/mod_adm_loja/admloj_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">
                Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset class="login">
            <legend>Fechamento de Caixa</legend>
            <table class="style1">
                <tr>
                    <td valign="top">
                        <fieldset class="login">
                            <legend>Filial</legend>
                            <div>
                                <asp:TextBox ID="txtFilial" runat="server" MaxLength="20" Height="22px" Width="196px"
                                    Enabled="False"></asp:TextBox>
                            </div>
                            <div style="width: 200px;" class="alinhamento">
                                <label>
                                    Data do Fechamento:&nbsp;
                                </label>
                                <asp:TextBox ID="txtDataFechamento" runat="server" CssClass="textEntry" Height="22px"
                                    Width="196px" Enabled="False"></asp:TextBox>
                                <asp:Calendar ID="CalendarDataFechamento" runat="server" OnDayRender="CalendarDataFechamento_DayRender"
                                    OnSelectionChanged="CalendarDataFechamento_SelectionChanged" CaptionAlign="Bottom">
                                </asp:Calendar>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorDataFechamento" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Escolha Data do Fechamento"
                                    ControlToValidate="txtDataFechamento" ValidationGroup="valores"></asp:RequiredFieldValidator>
                            </div>
                        </fieldset>
                    </td>
                    <td valign="top">
                        <fieldset class="login">
                            <legend>Venda</legend>
                            <div>
                                <label>
                                    Dinheiro:</label>
                                <asp:TextBox ID="txtDinheiroVenda" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px" Enabled="False" textAlign="rigth"></asp:TextBox>
                            </div>
                            <div>
                                <label>
                                    Cheque:</label>
                                <asp:TextBox ID="txtChequeVenda" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px" Enabled="False"></asp:TextBox>
                            </div>
                            <div>
                                <label>
                                    Cartão:</label>
                                <asp:TextBox ID="txtCartaoVenda" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px" Enabled="False"></asp:TextBox>
                            </div>
                            <div>
                                <label>
                                    Sobra:</label>
                                <asp:TextBox ID="txtSobra" runat="server" CssClass="pcRight" MaxLength="20" Height="16px"
                                    Width="100px" Enabled="False"></asp:TextBox>
                            </div>
                            <div>
                                <label>
                                    Falta:</label>
                                <asp:TextBox ID="txtFalta" runat="server" CssClass="pcRight" MaxLength="20" Height="16px"
                                    Width="100px" Enabled="False"></asp:TextBox>
                            </div>
                            <div>
                                <label>
                                    Total:</label>
                                <asp:TextBox ID="txtTotal" runat="server" CssClass="pcRight" MaxLength="20" Height="16px"
                                    Width="100px" Enabled="False"></asp:TextBox>
                            </div>
                        </fieldset>
                    </td>
                    <td valign="top">
                        <fieldset class="login">
                            <legend>Depósito</legend>
                            <div>
                                <label>
                                    Dinheiro</label>
                                <asp:TextBox ID="txtDinheiroDeposito" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorDinheiro" runat="server" SetFocusOnError="true"
                                    Display="None" ErrorMessage="Preencha o campo Dinheiro" ControlToValidate="txtDinheiroDeposito"
                                    ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorDinheiro" runat="server"
                                    ErrorMessage="Valor de Dinheiro Inválido !!!" SetFocusOnError="True" ControlToValidate="txtDinheiroDeposito"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Cheque</label>
                                <asp:TextBox ID="txtChequeDeposito" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCheque" runat="server" SetFocusOnError="true"
                                    Display="None" ErrorMessage="Preencha o campo Cheque" ControlToValidate="txtChequeDeposito"
                                    ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorCheque" runat="server"
                                    ErrorMessage="Valor de Cheque Inválido !!!" SetFocusOnError="True" ControlToValidate="txtChequeDeposito"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Cheque_Pré</label>
                                <asp:TextBox ID="txtChequePreRetorno" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorChequePre" runat="server" SetFocusOnError="true"
                                    Display="None" ErrorMessage="Preencha o campo Cheque Pré" ControlToValidate="txtChequePreRetorno"
                                    ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorChequePre" runat="server"
                                    ErrorMessage="Valor de Cheque Pré Inválido !!!" SetFocusOnError="True" ControlToValidate="txtChequePreRetorno"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Cartão</label>
                                <asp:TextBox ID="txtCartaoRetorno" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCartao" runat="server" SetFocusOnError="true"
                                    Display="None" ErrorMessage="Preencha o campo Cartão" ControlToValidate="txtCartaoRetorno"
                                    ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorCartao" runat="server"
                                    ErrorMessage="Valor de Cartão Inválido !!!" SetFocusOnError="True" ControlToValidate="txtCartaoRetorno"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Comanda</label>
                                <asp:TextBox ID="txtComanda" runat="server" CssClass="pcRight" MaxLength="20" Height="16px"
                                    Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorComanda" runat="server" SetFocusOnError="true"
                                    Display="None" ErrorMessage="Preencha o campo Comanda" ControlToValidate="txtComanda"
                                    ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorComanda" runat="server"
                                    ErrorMessage="Valor de Comanda Inválido !!!" SetFocusOnError="True" ControlToValidate="txtComanda"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Fundo Fixo</label>
                                <asp:TextBox ID="txtRetirada" runat="server" CssClass="pcRight" MaxLength="20" Height="16px"
                                    Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorRetirada" runat="server" SetFocusOnError="true"
                                    Display="None" ErrorMessage="Preencha o campo Fundo Fixo" ControlToValidate="txtRetirada"
                                    ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorRetirada" runat="server"
                                    ErrorMessage="Valor de Retirada Inválido !!!" SetFocusOnError="True" ControlToValidate="txtRetirada"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Devolução</label>
                                <asp:TextBox ID="txtDevolucao" runat="server" CssClass="pcRight" MaxLength="20" Height="16px"
                                    Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorDevolucao" runat="server" SetFocusOnError="true"
                                    Display="None" ErrorMessage="Preencha o campo Devolução" ControlToValidate="txtDevolucao"
                                    ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorDevolucao" runat="server"
                                    ErrorMessage="Valor de Devolução Inválido !!!" SetFocusOnError="True" ControlToValidate="txtDevolucao"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Outros Cartões</label>
                                <asp:TextBox ID="txtOutrosCartoes" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorOutrosCartoes" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Outros Cartões"
                                    ControlToValidate="txtOutrosCartoes" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorOutrosCartoes" runat="server"
                                    ErrorMessage="Valor de Outros Cartões Inválido !!!" SetFocusOnError="True" ControlToValidate="txtOutrosCartoes"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Responsavel</label>
                                <asp:TextBox ID="txtResponsavel" runat="server" CssClass="pcRight" MaxLength="100"
                                    Height="16px" Width="150px"></asp:TextBox>
                            </div>
                        </fieldset>
                    </td>
                    <td valign="top">
                        <fieldset class="login">
                            <legend>Visa</legend>
                            <div>
                                <label>
                                    Visa</label>
                                <asp:TextBox ID="txtVisa" runat="server" CssClass="pcRight" MaxLength="20" Height="16px"
                                    Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorVisa" runat="server" SetFocusOnError="true"
                                    Display="None" ErrorMessage="Preencha o campo Visa" ControlToValidate="txtVisa"
                                    ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorVisa" runat="server"
                                    ErrorMessage="Valor Visa Inválido !!!" SetFocusOnError="True" ControlToValidate="txtVisa"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Cred.Vista</label>
                                <asp:TextBox ID="txtVisaCredVista" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorVisaCredVista" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Visa Cred.Vista"
                                    ControlToValidate="txtVisaCredVista" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorVisaCredVista" runat="server"
                                    ErrorMessage="Valor Visa Cred.Vista Inválido !!!" SetFocusOnError="True" ControlToValidate="txtVisaCredVista"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Deb.Vista</label>
                                <asp:TextBox ID="txtVisaDebVista" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorVisaDebVista" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Visa Deb.Vista"
                                    ControlToValidate="txtVisaDebVista" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorVisaDebVista" runat="server"
                                    ErrorMessage="Valor Visa Deb.Vista Inválido !!!" SetFocusOnError="True" ControlToValidate="txtVisaDebVista"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Admin.</label>
                                <asp:TextBox ID="txtVisaParcAdmin" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorVisaParcAdmin" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Visa Parc.Admin."
                                    ControlToValidate="txtVisaParcAdmin" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorVisaParcAdmin" runat="server"
                                    ErrorMessage="Valor Visa ParcAdmin Inválido !!!" SetFocusOnError="True" ControlToValidate="txtVisaParcAdmin"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.2x</label>
                                <asp:TextBox ID="txtVisaParcEstab2x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorVisaParcEstab2x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Visa Parc.Estab.2x"
                                    ControlToValidate="txtVisaParcEstab2x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorVisaParcEstab2x" runat="server"
                                    ErrorMessage="Valor Visa Parc.Estab.2x Inválido !!!" SetFocusOnError="True" ControlToValidate="txtVisaParcEstab2x"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.3x</label>
                                <asp:TextBox ID="txtVisaParcEstab3x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorVisaParcEstab3x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Visa Parc.Estab.3x"
                                    ControlToValidate="txtVisaParcEstab3x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorVisaParcEstab3x" runat="server"
                                    ErrorMessage="Valor Visa Parc.Estab.3x Inválido !!!" SetFocusOnError="True" ControlToValidate="txtVisaParcEstab3x"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.4x</label>
                                <asp:TextBox ID="txtVisaParcEstab4x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorVisaParcEstab4x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Visa Parc.Estab.4x"
                                    ControlToValidate="txtVisaParcEstab4x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorVisaParcEstab4x" runat="server"
                                    ErrorMessage="Valor Visa Parc.Estab.4x Inválido !!!" SetFocusOnError="True" ControlToValidate="txtVisaParcEstab4x"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.5x</label>
                                <asp:TextBox ID="txtVisaParcEstab5x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorVisaParcEstab5x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Visa Parc.Estab.5x"
                                    ControlToValidate="txtVisaParcEstab5x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorVisaParcEstab5x" runat="server"
                                    ErrorMessage="Valor Visa Parc.Estab.5x Inválido !!!" SetFocusOnError="True" ControlToValidate="txtVisaParcEstab5x"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.6x</label>
                                <asp:TextBox ID="txtVisaParcEstab6x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorVisaParcEstab6x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Visa Parc.Estab.6x"
                                    ControlToValidate="txtVisaParcEstab6x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorVisaParcEstab6x" runat="server"
                                    ErrorMessage="Valor Visa Parc.Estab.6x Inválido !!!" SetFocusOnError="True" ControlToValidate="txtVisaParcEstab6x"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.7x</label>
                                <asp:TextBox ID="txtVisaParcEstab7x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorVisaParcEstab7x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Visa Parc.Estab.7x"
                                    ControlToValidate="txtVisaParcEstab7x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorVisaParcEstab7x" runat="server"
                                    ErrorMessage="Valor Visa Parc.Estab.7x Inválido !!!" SetFocusOnError="True" ControlToValidate="txtVisaParcEstab7x"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.8x</label>
                                <asp:TextBox ID="txtVisaParcEstab8x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorVisaParcEstab8x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Visa Parc.Estab.8x"
                                    ControlToValidate="txtVisaParcEstab8x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorVisaParcEstab8x" runat="server"
                                    ErrorMessage="Valor Visa Parc.Estab.8x Inválido !!!" SetFocusOnError="True" ControlToValidate="txtVisaParcEstab8x"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.9x</label>
                                <asp:TextBox ID="txtVisaParcEstab9x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorVisaParcEstab9x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Visa Parc.Estab.9x"
                                    ControlToValidate="txtVisaParcEstab9x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorVisaParcEstab9x" runat="server"
                                    ErrorMessage="Valor Visa Parc.Estab.9x Inválido !!!" SetFocusOnError="True" ControlToValidate="txtVisaParcEstab9x"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.10x</label>
                                <asp:TextBox ID="txtVisaParcEstab10x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorVisaParcEstab10x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Visa Parc.Estab.10x"
                                    ControlToValidate="txtVisaParcEstab10x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorVisaParcEstab10x" runat="server"
                                    ErrorMessage="Valor Visa Parc.Estab.10x Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtVisaParcEstab10x" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                        </fieldset>
                    </td>
                    <td valign="top">
                        <fieldset class="login">
                            <legend>Mastercard</legend>
                            <div>
                                <label>
                                    Mastercard</label>
                                <asp:TextBox ID="txtMaster" runat="server" CssClass="pcRight" MaxLength="20" Height="16px"
                                    Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorMaster" runat="server" SetFocusOnError="true"
                                    Display="None" ErrorMessage="Preencha o campo Mastercard" ControlToValidate="txtMaster"
                                    ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorMaster" runat="server"
                                    ErrorMessage="Valor Mastercard Inválido !!!" SetFocusOnError="True" ControlToValidate="txtMaster"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Cred.Vista</label>
                                <asp:TextBox ID="txtMasterCredVista" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorMasterCredVista" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Mastercard Cred Vista"
                                    ControlToValidate="txtMasterCredVista" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorMasterCredVista" runat="server"
                                    ErrorMessage="Valor Mastercard Cred.Vista Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtMasterCredVista" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Deb.Vista</label>
                                <asp:TextBox ID="txtMasterDebVista" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorMasterDebVista" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Mastercad Deb.Vista"
                                    ControlToValidate="txtMasterDebVista" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorMasterDebVista" runat="server"
                                    ErrorMessage="Valor Mastercard Deb.Vista Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtMasterDebVista" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Admin.</label>
                                <asp:TextBox ID="txtMasterParcAdmin" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorMasterParcAdmin" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Mastercard Parc.Admin."
                                    ControlToValidate="txtMasterParcAdmin" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorMasterParcAdmin" runat="server"
                                    ErrorMessage="Valor Mastercard Parc.Admin Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtMasterParcAdmin" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.2x</label>
                                <asp:TextBox ID="txtMasterParcEstab2x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorMasterParcEstab2x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Mastercard Parc.Estab.2x"
                                    ControlToValidate="txtMasterParcEstab2x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorMasterParcEstab2x"
                                    runat="server" ErrorMessage="Valor Mastercard Parc.Estab.2x Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtMasterParcEstab2x" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.3x</label>
                                <asp:TextBox ID="txtMasterParcEstab3x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorMasterParcEstab3x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Mastercard Parc.Estab.3x"
                                    ControlToValidate="txtMasterParcEstab3x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorMasterParcEstab3x"
                                    runat="server" ErrorMessage="Valor Mastercard Parc.Estab.3x Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtMasterParcEstab3x" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.4x</label>
                                <asp:TextBox ID="txtMasterParcEstab4x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorMasterParcEstab4x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Mastercard Parc.Estab.4x"
                                    ControlToValidate="txtMasterParcEstab4x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorMasterParcEstab4x"
                                    runat="server" ErrorMessage="Valor Mastercard Parc.Estab.4x Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtMasterParcEstab4x" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.5x</label>
                                <asp:TextBox ID="txtMasterParcEstab5x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorMasterParcEstab5x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Mastercard Parc.Estab.5x"
                                    ControlToValidate="txtMasterParcEstab5x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorMasterParcEstab5x"
                                    runat="server" ErrorMessage="Valor Mastercard Parc.Estab.5x Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtMasterParcEstab5x" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.6x</label>
                                <asp:TextBox ID="txtMasterParcEstab6x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorMasterParcEstab6x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Mastercard Parc.Estab.6x"
                                    ControlToValidate="txtMasterParcEstab6x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorMasterParcEstab6x"
                                    runat="server" ErrorMessage="Valor Mastercard Parc.Estab.6x Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtMasterParcEstab6x" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.7x</label>
                                <asp:TextBox ID="txtMasterParcEstab7x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorMasterParcEstab7x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Mastercard Parc.Estab.7x"
                                    ControlToValidate="txtMasterParcEstab7x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorMasterParcEstab7x"
                                    runat="server" ErrorMessage="Valor Mastercard Parc.Estab.7x Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtMasterParcEstab7x" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.8x</label>
                                <asp:TextBox ID="txtMasterParcEstab8x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorMasterParcEstab8x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Mastercard Parc.Estab.8x"
                                    ControlToValidate="txtMasterParcEstab8x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorMasterParcEstab8x"
                                    runat="server" ErrorMessage="Valor Mastercard Parc.Estab.8x Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtMasterParcEstab8x" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.9x</label>
                                <asp:TextBox ID="txtMasterParcEstab9x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorMasterParcEstab9x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Mastercard Parc.Estab.9x"
                                    ControlToValidate="txtMasterParcEstab9x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorMasterParcEstab9x"
                                    runat="server" ErrorMessage="Valor Mastercard Parc.Estab.9x Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtMasterParcEstab9x" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.10x</label>
                                <asp:TextBox ID="txtMasterParcEstab10x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorMasterParcEstab10x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Mastercard Parc.Estab.10x"
                                    ControlToValidate="txtMasterParcEstab10x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorMasterParcEstab10x"
                                    runat="server" ErrorMessage="Valor Mastercard Parc.Estab.10x Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtMasterParcEstab10x" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                        </fieldset>
                    </td>
                    <td valign="top">
                        <fieldset class="login">
                            <legend>Amex</legend>
                            <div>
                                <label>
                                    Amex</label>
                                <asp:TextBox ID="txtAmex" runat="server" CssClass="pcRight" MaxLength="20" Height="16px"
                                    Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorAmex" runat="server" SetFocusOnError="true"
                                    Display="None" ErrorMessage="Preencha o campo Amex" ControlToValidate="txtAmex"
                                    ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorAmex" runat="server"
                                    ErrorMessage="Valor Amex Inválido !!!" SetFocusOnError="True" ControlToValidate="txtAmex"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Cred.Vista</label>
                                <asp:TextBox ID="txtAmexCredVista" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorAmexCredVista" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Amex Cred.Vista"
                                    ControlToValidate="txtAmexCredVista" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorAmexCredVista" runat="server"
                                    ErrorMessage="Valor Amex Cred.Vista Inválido !!!" SetFocusOnError="True" ControlToValidate="txtAmexCredVista"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Deb.Vista</label>
                                <asp:TextBox ID="txtAmexDebVista" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorAmexDebVista" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Amex Deb.Vista"
                                    ControlToValidate="txtAmexDebVista" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorAmexDebVista" runat="server"
                                    ErrorMessage="Valor Amex Deb.Vista Inválido !!!" SetFocusOnError="True" ControlToValidate="txtAmexDebVista"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Admin.</label>
                                <asp:TextBox ID="txtAmexParcAdmin" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorAmexParcAdmin" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Amex Parc.Admin."
                                    ControlToValidate="txtAmexParcAdmin" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorAmexParcAdmin" runat="server"
                                    ErrorMessage="Valor Amex Parc.Admin Inválido !!!" SetFocusOnError="True" ControlToValidate="txtAmexParcAdmin"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.2x</label>
                                <asp:TextBox ID="txtAmexParcEstab2x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorAmexParcEstab2x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Amex Parc.Estab.2x"
                                    ControlToValidate="txtAmexParcEstab2x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorAmexParcEstab2x" runat="server"
                                    ErrorMessage="Valor Amex Parc.Estab.2x Inválido !!!" SetFocusOnError="True" ControlToValidate="txtAmexParcEstab2x"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.3x</label>
                                <asp:TextBox ID="txtAmexParcEstab3x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorAmexParcEstab3x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Amex Parc.Estab.3x"
                                    ControlToValidate="txtAmexParcEstab3x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorAmexParcEstab3x" runat="server"
                                    ErrorMessage="Valor Amex Parc.Estab.3x Inválido !!!" SetFocusOnError="True" ControlToValidate="txtAmexParcEstab3x"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.4x</label>
                                <asp:TextBox ID="txtAmexParcEstab4x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorAmexParcEstab4x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Amex Parc.Estab.4x"
                                    ControlToValidate="txtAmexParcEstab4x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorAmexParcEstab4x" runat="server"
                                    ErrorMessage="Valor Amex Parc.Estab.4x Inválido !!!" SetFocusOnError="True" ControlToValidate="txtAmexParcEstab4x"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.5x</label>
                                <asp:TextBox ID="txtAmexParcEstab5x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorAmexParcEstab5x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Amex Parc.Estab.5x"
                                    ControlToValidate="txtAmexParcEstab5x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorAmexParcEstab5x" runat="server"
                                    ErrorMessage="Valor Amex Parc.Estab.5x Inválido !!!" SetFocusOnError="True" ControlToValidate="txtAmexParcEstab5x"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.6x</label>
                                <asp:TextBox ID="txtAmexParcEstab6x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorAmexParcEstab6x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Amex Parc.Estab.6x"
                                    ControlToValidate="txtAmexParcEstab6x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorAmexParcEstab6x" runat="server"
                                    ErrorMessage="Valor Amex Parc.Estab.6x Inválido !!!" SetFocusOnError="True" ControlToValidate="txtAmexParcEstab6x"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.7x</label>
                                <asp:TextBox ID="txtAmexParcEstab7x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorAmexParcEstab7x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Amex Parc.Estab.7x"
                                    ControlToValidate="txtAmexParcEstab7x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorAmexParcEstab7x" runat="server"
                                    ErrorMessage="Valor Amex Parc.Estab.7x Inválido !!!" SetFocusOnError="True" ControlToValidate="txtAmexParcEstab7x"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.8x</label>
                                <asp:TextBox ID="txtAmexParcEstab8x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorAmexParcEstab8x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Amex Parc.Estab.8x"
                                    ControlToValidate="txtAmexParcEstab8x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorAmexParcEstab8x" runat="server"
                                    ErrorMessage="Valor Amex Parc.Estab.8x Inválido !!!" SetFocusOnError="True" ControlToValidate="txtAmexParcEstab8x"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.9x</label>
                                <asp:TextBox ID="txtAmexParcEstab9x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorAmexParcEstab9x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Amex Parc.Estab.9x"
                                    ControlToValidate="txtAmexParcEstab9x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorAmexParcEstab9x" runat="server"
                                    ErrorMessage="Valor Amex Parc.Estab.9x Inválido !!!" SetFocusOnError="True" ControlToValidate="txtAmexParcEstab9x"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.10x</label>
                                <asp:TextBox ID="txtAmexParcEstab10x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorAmexParcEstab10x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Amex Parc.Estab.10x"
                                    ControlToValidate="txtAmexParcEstab10x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorAmexParcEstab10x" runat="server"
                                    ErrorMessage="Valor Amex Parc.Estab.10x Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtAmexParcEstab10x" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                        </fieldset>
                    </td>
                    <td valign="top">
                        <fieldset class="login">
                            <legend>Elo</legend>
                            <div>
                                <label>
                                    Elo</label>
                                <asp:TextBox ID="txtHiper" runat="server" CssClass="pcRight" MaxLength="20" Height="16px"
                                    Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorHiper" runat="server" SetFocusOnError="true"
                                    Display="None" ErrorMessage="Preencha o campo Elo" ControlToValidate="txtHiper"
                                    ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorHiper" runat="server"
                                    ErrorMessage="Valor Elo Inválido !!!" SetFocusOnError="True" ControlToValidate="txtHiper"
                                    ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Cred.Vista</label>
                                <asp:TextBox ID="txtHiperCredVista" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorHiperCredVista" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Elo Cred Vista"
                                    ControlToValidate="txtHiperCredVista" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorHiperCredVista" runat="server"
                                    ErrorMessage="Valor Elo Cred.Vista Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtHiperCredVista" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Deb.Vista</label>
                                <asp:TextBox ID="txtHiperDebVista" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorHiperDebVista" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Hipercad Deb.Vista"
                                    ControlToValidate="txtHiperDebVista" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorHiperDebVista" runat="server"
                                    ErrorMessage="Valor Elo Deb.Vista Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtHiperDebVista" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Admin.</label>
                                <asp:TextBox ID="txtHiperParcAdmin" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorHiperParcAdmin" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Elo Parc.Admin."
                                    ControlToValidate="txtHiperParcAdmin" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorHiperParcAdmin" runat="server"
                                    ErrorMessage="Valor Elo Parc.Admin Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtHiperParcAdmin" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.2x</label>
                                <asp:TextBox ID="txtHiperParcEstab2x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorHiperParcEstab2x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Elo Parc.Estab.2x"
                                    ControlToValidate="txtHiperParcEstab2x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorHiperParcEstab2x" runat="server"
                                    ErrorMessage="Valor Elo Parc.Estab.2x Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtHiperParcEstab2x" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.3x</label>
                                <asp:TextBox ID="txtHiperParcEstab3x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorHiperParcEstab3x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Elo Parc.Estab.3x"
                                    ControlToValidate="txtHiperParcEstab3x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorHiperParcEstab3x" runat="server"
                                    ErrorMessage="Valor Elo Parc.Estab.3x Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtHiperParcEstab3x" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.4x</label>
                                <asp:TextBox ID="txtHiperParcEstab4x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorHiperParcEstab4x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Elo Parc.Estab.4x"
                                    ControlToValidate="txtHiperParcEstab4x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorHiperParcEstab4x" runat="server"
                                    ErrorMessage="Valor Elo Parc.Estab.4x Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtHiperParcEstab4x" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.5x</label>
                                <asp:TextBox ID="txtHiperParcEstab5x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorHiperParcEstab5x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Elo Parc.Estab.5x"
                                    ControlToValidate="txtHiperParcEstab5x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorHiperParcEstab5x" runat="server"
                                    ErrorMessage="Valor Elo Parc.Estab.5x Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtHiperParcEstab5x" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.6x</label>
                                <asp:TextBox ID="txtHiperParcEstab6x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorHiperParcEstab6x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Elo Parc.Estab.6x"
                                    ControlToValidate="txtHiperParcEstab6x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorHiperParcEstab6x" runat="server"
                                    ErrorMessage="Valor Elo Parc.Estab.6x Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtHiperParcEstab6x" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.7x</label>
                                <asp:TextBox ID="txtHiperParcEstab7x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorHiperParcEstab7x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Elo Parc.Estab.7x"
                                    ControlToValidate="txtHiperParcEstab7x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorHiperParcEstab7x" runat="server"
                                    ErrorMessage="Valor Elo Parc.Estab.7x Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtHiperParcEstab7x" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.8x</label>
                                <asp:TextBox ID="txtHiperParcEstab8x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorHiperParcEstab8x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Elo Parc.Estab.8x"
                                    ControlToValidate="txtHiperParcEstab8x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorHiperParcEstab8x" runat="server"
                                    ErrorMessage="Valor Elo Parc.Estab.8x Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtHiperParcEstab8x" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.9x</label>
                                <asp:TextBox ID="txtHiperParcEstab9x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorHiperParcEstab9x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Elo Parc.Estab.9x"
                                    ControlToValidate="txtHiperParcEstab9x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorHiperParcEstab9x" runat="server"
                                    ErrorMessage="Valor Elo Parc.Estab.9x Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtHiperParcEstab9x" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>
                                    Parc.Estab.10x</label>
                                <asp:TextBox ID="txtHiperParcEstab10x" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorHiperParcEstab10x" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Elo Parc.Estab.10x"
                                    ControlToValidate="txtHiperParcEstab10x" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorHiperParcEstab10x"
                                    runat="server" ErrorMessage="Valor Elo Parc.Estab.10x Inválido !!!" SetFocusOnError="True"
                                    ControlToValidate="txtHiperParcEstab10x" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                    ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                        </fieldset>
                    </td>
                    <td valign="top">
                        <fieldset class="login">
                            <legend>Observação</legend>
                            <div>
                                <label>
                                    Observação Geral:</label>
                                <asp:TextBox ID="txtObsGeral" runat="server" CssClass="textEntry" MaxLength="1000"
                                    Height="100px" Width="300px" TextMode="MultiLine"></asp:TextBox>
                            </div>
                            <div>
                                <label>
                                    Retirar p/ Fundo Fixo - Valor:</label>
                                <asp:TextBox ID="txtValorFundoFixo" runat="server" CssClass="pcRight" MaxLength="20"
                                    Height="16px" Width="100px" Enabled="False"></asp:TextBox>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
            <div>
                <asp:Button runat="server" ID="ButtonAtualizar" Text="Atualizar" OnClick="ButtonAtualizar_Click"
                    ValidationGroup="valores" CausesValidation="true" />
                <asp:ValidationSummary ID="ValidationSummaryValores" runat="server" ValidationGroup="valores"
                    ShowMessageBox="true" ShowSummary="false" />
                <asp:Button runat="server" ID="ButtonEnviar" Text="Enviar" OnClick="ButtonEnviar_Click"
                    Enabled="False" />
                <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
            </div>
        </fieldset>
    </div>
</asp:Content>
