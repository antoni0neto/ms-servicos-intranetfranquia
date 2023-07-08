<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="FechamentoFundoFixo.aspx.cs" Inherits="Relatorios.FechamentoFundoFixo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="jquery-1.6.min.js"></script>
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">
            <asp:Label ID="labTitulo" runat="server" Text="Administração de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Fechamento&nbsp;&nbsp;>&nbsp;&nbsp;Fechamento Fundo Fixo"></asp:Label></span>
        <div style="float: right; padding: 0;">
            <a href="~/mod_adm_loja/admloj_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div>
        <fieldset class="login">
            <legend>Fechamento de Fundo Fixo</legend>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td valign="top" style="width: 750px;">
                        <fieldset>
                            <legend>Cadastro de Despesas</legend>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>Filial
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtFilial" runat="server" Width="200px"
                                            Enabled="False" Height="18px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Despesa
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlContas" DataValueField="CODIGO" DataTextField="DESCRICAO"
                                            Height="22px" Width="204px" OnDataBound="ddlContas_DataBound">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorContas" ControlToValidate="ddlContas"
                                            InitialValue="" ValidationGroup="contas" ErrorMessage="Selecione uma Conta de Despesa."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Descrição Despesa
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtDescricaoDespesa" runat="server" MaxLength="70"
                                            Width="200px" Height="17px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorDescricao" runat="server" SetFocusOnError="true"
                                            Display="None" ErrorMessage="Preencher o campo Descrição Despesa." ControlToValidate="txtDescricaoDespesa"
                                            ValidationGroup="contas"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Valor
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtValor" runat="server" CssClass="pcRight" MaxLength="12"
                                            Width="202px" Height="18px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorValor" runat="server" SetFocusOnError="true"
                                            Display="None" ErrorMessage="Preencher o campo Valor." ControlToValidate="txtValor"
                                            ValidationGroup="contas"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button runat="server" ID="ButtonSalvar" Text="Salvar" OnClick="ButtonSalvar_Click"
                                            ValidationGroup="contas" Width="130px" />
                                        <asp:ValidationSummary ID="ValidationSummaryConta" runat="server" ValidationGroup="contas"
                                            ShowMessageBox="true" ShowSummary="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView runat="server" ID="GridViewDespesa" Width="100%"
                                            AutoGenerateColumns="false" OnRowDataBound="GridViewDespesa_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Despesa" HeaderStyle-Width="245px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="LiteralDespesa" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Descrição" HeaderStyle-Width="350px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="LiteralDespesaDescricao" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Data" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="LiteralData" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="VALOR" HeaderText="Valor" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Right" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button runat="server" ID="ButtonExcluirDespesa" Text="Excluir" OnClick="ButtonExcluirDespesa_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                    <td style="width: 5px;">&nbsp;
                    </td>
                    <td valign="top" style="width: 230px;">
                        <fieldset class="login" style="height: 219px;">
                            <legend>Acompanhamento de Saldo</legend>
                            <div>
                                <label>
                                    Saldo Inicial
                                </label>
                                <asp:TextBox ID="txtSaldoInicial" runat="server" CssClass="pcRight" MaxLength="20"
                                    Width="200px" Enabled="False" Height="18px"></asp:TextBox>
                            </div>
                            <div>
                                <label>
                                    Total das Despesas
                                </label>
                                <asp:TextBox ID="txtTotalDespesas" runat="server" CssClass="pcRight" MaxLength="20"
                                    Width="200px" Enabled="False" Height="18px"></asp:TextBox>
                            </div>
                            <div>
                                <label>
                                    Saldo Final
                                </label>
                                <asp:TextBox ID="txtSaldoFinal" runat="server" CssClass="pcRight" MaxLength="20"
                                    Width="200px" Enabled="False" Height="18px"></asp:TextBox>
                            </div>
                            <p style="height: 20px;">&nbsp;</p>
                            <div style="text-align: right;">
                                <asp:Button runat="server" ID="btFechar" Text="Fechar Fundo Fixo" OnClick="btFechar_Click" Width="130px"
                                    OnClientClick="javascript: return confirm('Tem certeza que deseja fechar despesas?');" />
                            </div>
                        </fieldset>
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <asp:HiddenField runat="server" ID="HiddenFieldCodigoDespesa" Value="0" />
</asp:Content>
