<%@ Page Title="Depósito em Banco" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="DepositoBanco.aspx.cs" Inherits="Relatorios.DepositoBanco" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento {
            position: relative;
            float: left;
        }

        .style1 {
            width: 100%;
        }
    </style>
    <script src="../../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" src="jquery-1.6.min.js"></script>
    <script type="text/javascript" src="jquery.maskedinput-1.3.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('input').keypress(function (e) {
                var code = null;
                code = (e.keyCode ? e.keyCode : e.which);
                return (code == 13) ? false : true;
            });
        });
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
    <div class="accountInfo">
        <fieldset class="login">
            <legend>Consulta de Depósito em Banco</legend>
            <table border="1" class="style1">
                <tr>
                    <td>
                        <div style="width: 1000px;" class="alinhamento">
                            <div style="width: 200px;" class="alinhamento">
                                <label>
                                    Data Início:&nbsp;
                                </label>
                                <asp:TextBox ID="TextBoxDataInicio" runat="server" CssClass="textEntry" Height="22px"
                                    Width="198px"></asp:TextBox>
                                <asp:Calendar ID="CalendarDataInicio" runat="server" OnSelectionChanged="CalendarDataInicio_SelectionChanged"
                                    CaptionAlign="Bottom"></asp:Calendar>
                            </div>
                            <div style="width: 200px;" class="alinhamento">
                                <label>
                                    Data Fim:&nbsp;
                                </label>
                                <asp:TextBox ID="TextBoxDataFim" runat="server" CssClass="textEntry" Height="22px"
                                    Width="198px"></asp:TextBox>
                                <asp:Calendar ID="CalendarDataFim" runat="server" OnSelectionChanged="CalendarDataFim_SelectionChanged"
                                    CaptionAlign="Bottom"></asp:Calendar>
                            </div>
                            <div style="width: 600px;" class="alinhamento">
                                <label>
                                    Filial:&nbsp;
                                </label>
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="26px" Width="250px" OnDataBound="ddlFilial_DataBound">
                                </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button runat="server" ID="btMovimento" Text="Buscar Movimento Dinheiro" OnClick="btMovimento_Click" />
                                <br />
                                <br />
                                <asp:Label runat="server" ID="lblValidaBusca" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                    </td>
                    <td>
                        <fieldset class="login">
                            <legend>Depósitos Realizados</legend>
                            <asp:Calendar ID="CalendarDepositosRealizados" runat="server" CaptionAlign="Bottom"></asp:Calendar>
                        </fieldset>
                    </td>
                </tr>
            </table>
            <br />
            <table border="1" class="style1">
                <tr>
                    <td>
                        <asp:GridView ID="GridViewMovimento" runat="server" Width="100%" CssClass="DataGrid_Padrao"
                            PageSize="1000" AllowPaging="True" AutoGenerateColumns="False" ShowFooter="true"
                            OnDataBound="GridViewMovimento_DataBound">
                            <FooterStyle HorizontalAlign="Center"></FooterStyle>
                            <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
                            <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
                            <RowStyle HorizontalAlign="Center"></RowStyle>
                            <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                            <Columns>
                                <asp:BoundField DataField="data" HeaderText="Data do Faturamento" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="valor_deposito" HeaderText="Saldo em Dinheiro" />
                            </Columns>
                            <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset class="login">
            <legend>Lançamentos de Depósito em Banco</legend>
            <table border="1" class="style1" style="height: 280px;">
                <tr>
                    <td valign="top">
                        <div style="width: 200px;" class="alinhamento">
                            <asp:GridView ID="gridALancar" runat="server" Width="100%" CssClass="DataGrid_Padrao"
                                PageSize="1000" AllowPaging="True" AutoGenerateColumns="False">
                                <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
                                <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
                                <RowStyle HorizontalAlign="Center"></RowStyle>
                                <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                                <Columns>
                                    <asp:TemplateField HeaderText="Lançar">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkLancar" runat="server" OnCheckedChanged="chkLancar_CheckedChanged" AutoPostBack="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="data" HeaderText="Data do Faturamento" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="valor_deposito" HeaderText="Saldo em Dinheiro" />
                                </Columns>
                                <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                            </asp:GridView>
                        </div>
                    </td>
                    <td>
                        <div>
                            <label>
                                Valor Total a Depositar:</label>
                            <asp:TextBox ID="txtValorADepositar" runat="server" CssClass="pcRight" MaxLength="20"
                                Height="16px" Width="150px" Enabled="False"></asp:TextBox>
                        </div>
                        <div>
                            <label>
                                Valor Depositado:</label>
                            <asp:TextBox ID="txtValorDepositadoCalculado" runat="server" CssClass="pcRight" MaxLength="20"
                                Height="16px" Width="70px" Enabled="False" AutoPostBack="True" OnTextChanged="txtValorDepositadoCalculado_TextChanged"></asp:TextBox>
                            <asp:TextBox ID="txtValorDepositado" runat="server" CssClass="pcRight" MaxLength="20"
                                Height="16px" Width="75px" AutoPostBack="True" OnTextChanged="txtValorDepositado_TextChanged"></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidatorValorDepositado" runat="server"
                                SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Valor Depositado"
                                ControlToValidate="txtValorDepositado" ValidationGroup="valores"></asp:RequiredFieldValidator>--%>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorValorDepositado" runat="server"
                                Display="None" ErrorMessage="Valor Depositado Inválido !!!" SetFocusOnError="True"
                                ControlToValidate="txtValorDepositado" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$"
                                ValidationGroup="valores"> </asp:RegularExpressionValidator>
                        </div>
                        <div>
                            <label>
                                Diferença:</label>
                            <asp:TextBox ID="txtDiferenca" runat="server" CssClass="pcRight" MaxLength="20" Height="16px"
                                Width="150px" Enabled="False"></asp:TextBox>
                        </div>
                        <div>
                            <label>
                                Assinatura:</label>
                            <asp:TextBox ID="txtAssinatura" runat="server" CssClass="pcRight" MaxLength="100"
                                Height="16px" Width="150px"></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidatorAssinatura" runat="server"
                                SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Assinatura"
                                ControlToValidate="txtAssinatura" ValidationGroup="valores"></asp:RequiredFieldValidator>--%>
                        </div>
                        <div>
                            <label>
                                Diferença Anterior:</label>
                            <asp:TextBox ID="txtDiferencaAnterior" runat="server" CssClass="pcRight" MaxLength="20" Text="-"
                                Height="16px" Width="150px" Enabled="False"></asp:TextBox>
                        </div>
                        <div>
                            <label>
                                Banco Depositado:</label>
                            <asp:TextBox ID="txtBancoDepositado" runat="server" CssClass="pcRight" MaxLength="20"
                                Height="16px" Width="150px" Enabled="False"></asp:TextBox>
                        </div>
                        <div>
                            <label>
                                Ag./Conta:</label>
                            <asp:TextBox ID="txtAgenciaConta" runat="server" CssClass="pcRight" MaxLength="20"
                                Height="16px" Width="150px" Enabled="False"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div style="width: 200px;" class="alinhamento">
                            <div style="width: 200px;" class="alinhamento">
                                <label>
                                    Data do Depósito:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Hora: *
                                </label>
                                <div style="width: 139px;" class="alinhamento">
                                    <asp:TextBox ID="txtDataDeposito" runat="server" CssClass="textEntry" Height="22px" AutoPostBack="true"
                                        Width="133px"></asp:TextBox>&nbsp;
                                </div>
                                <div style="width: 59px;" class="alinhamento">
                                    <asp:TextBox ID="txtHoraDeposito" runat="server" CssClass="textEntry" Height="22px" MaxLength="5" AutoPostBack="true"
                                        Width="59px"></asp:TextBox>
                                </div>
                                <asp:Calendar ID="CalendarDataDeposito" runat="server" OnSelectionChanged="CalendarDataDeposito_SelectionChanged"
                                    CaptionAlign="Bottom"></asp:Calendar>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidatorDataDeposito" runat="server"
                                    SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Data do Deposito"
                                    ControlToValidate="txtDataDeposito" ValidationGroup="valores"></asp:RequiredFieldValidator>--%>
                                <br />
                                <asp:Label runat="server" ID="Label1" ForeColor="Red" Font-Size="Smaller">*Hora do depósito no comprovante</asp:Label>
                            </div>
                        </div>
                    </td>
                    <td>
                        <fieldset class="login">
                            <table border="1" class="style1">
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
            <p style="height: 13px">
                &nbsp;
            </p>
            <div>
                <asp:Button runat="server" ID="btGravar" Text="Gravar Depósito" OnClick="btGravar_Click"
                    ValidationGroup="valores" CausesValidation="true" />
                <asp:ValidationSummary ID="ValidationSummaryProcesso" runat="server" ValidationGroup="valores"
                    ShowMessageBox="true" ShowSummary="false" />
                <asp:Label runat="server" ID="lblMensagem" ForeColor="Red"></asp:Label>
            </div>
        </fieldset>
    </div>
</asp:Content>
