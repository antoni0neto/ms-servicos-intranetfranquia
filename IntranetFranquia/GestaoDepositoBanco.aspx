<%@ Page Title="Depósito em Banco" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="GestaoDepositoBanco.aspx.cs" Inherits="Relatorios.GestaoDepositoBanco"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento
        {
            position: relative;
            float: left;
        }
        .style1
        {
            width: 100%;
            border: 1px solid #CCC;
        }
    </style>
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
        <span style="font-family: Calibri; font-size: 14px;">Módulo do Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Gestão
            de Depósito em Banco</span>
        <div style="float: right; padding: 0;">
            <a href="DefaultFinanceiro.aspx" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset class="login">
            <legend>Gestão de Depósito em Banco</legend>
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="border: 1px solid #CCC;">
                        <div style="width: 200px;" class="alinhamento">
                            <div style="width: 200px;" class="alinhamento">
                                <label>
                                    Filial:&nbsp;
                                </label>
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="26px" Width="200px" OnDataBound="ddlFilial_DataBound">
                                </asp:DropDownList>
                            </div>
                            <div style="width: 200px;" class="alinhamento">
                                <div style="width: 200px;" class="alinhamento">
                                    <label>
                                        Baixa:&nbsp;
                                    </label>
                                    <asp:DropDownList runat="server" ID="ddlBaixa" DataValueField="CODIGO_BAIXA" DataTextField="DESCRICAO"
                                        Height="26px" Width="200px" OnDataBound="ddlBaixa_DataBound">
                                    </asp:DropDownList>
                                    <br />
                                    <br />
                                </div>
                            </div>
                            <p style="height: 13px">
                                &nbsp;</p>
                            <div>
                                <asp:Button runat="server" ID="btBuscarDepositos" Text="Buscar Depósitos" OnClick="btBuscarDepositos_Click" />
                            </div>
                        </div>
                    </td>
                    <td style="border: 1px solid #CCC;">
                        <div>
                            <asp:GridView ID="GridViewDepositos" runat="server" Width="100%" CssClass="DataGrid_Padrao"
                                PageSize="10" PagerStyle-HorizontalAlign="Center" AllowPaging="True" AutoGenerateColumns="False"
                                ShowFooter="true" OnRowDataBound="GridViewDepositos_RowDataBound" OnPageIndexChanging="GridViewDepositos_PageIndexChanging">
                                <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
                                <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
                                <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
                                <RowStyle HorizontalAlign="Center"></RowStyle>
                                <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                                <Columns>
                                    <asp:BoundField DataField="data_inicio" HeaderText="Data Início" />
                                    <asp:BoundField DataField="data_fim" HeaderText="Data Fim" />
                                    <asp:BoundField DataField="valor_a_depositar" HeaderText="Valor a Depositar" />
                                    <asp:BoundField DataField="valor_depositado" HeaderText="Valor Depositado" />
                                    <asp:BoundField DataField="diferenca" HeaderText="Diferença" />
                                    <asp:BoundField DataField="assinatura" HeaderText="Assinatura" />
                                    <asp:TemplateField HeaderText="Baixa">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="LiteralBaixa" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button runat="server" ID="btAlterar" Text="Alterar" OnClick="btAlterar_Click"
                                                Enable="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="border: 1px solid #CCC;" valign="top">
                        <asp:GridView ID="GridViewMovimento" runat="server" Width="100%" CssClass="DataGrid_Padrao"
                            PageSize="1000" AllowPaging="True" Visible="false" AutoGenerateColumns="False"
                            ShowFooter="true" OnDataBound="GridViewMovimento_DataBound">
                            <FooterStyle HorizontalAlign="Center"></FooterStyle>
                            <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
                            <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
                            <RowStyle HorizontalAlign="Center"></RowStyle>
                            <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                            <Columns>
                                <asp:BoundField DataField="data" HeaderText="Data do Faturamento" />
                                <asp:BoundField DataField="valor_deposito" HeaderText="Saldo em Dinheiro" />
                            </Columns>
                            <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                        </asp:GridView>
                    </td>
                    <td>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 400px; border: 1px solid #CCC;" valign="top">
                                    <div id="divValores" runat="server" style="width: 500px;" visible="false">
                                        <div>
                                            <label>
                                                Valor a Depositar:</label>
                                            <asp:TextBox ID="txtValorADepositar" runat="server" CssClass="pcRight" MaxLength="20"
                                                Height="16px" Width="150px" Enabled="False"></asp:TextBox>
                                        </div>
                                        <div>
                                            <label>
                                                Valor Depositado:</label>
                                            <asp:TextBox ID="txtValorDepositado" runat="server" CssClass="pcRight" MaxLength="20"
                                                Height="16px" Width="150px" Enabled="False"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorValorDepositado" runat="server"
                                                SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Valor Depositado"
                                                ControlToValidate="txtValorDepositado" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorValorDepositado" runat="server"
                                                ErrorMessage="Valor Depositado Inválido !!!" SetFocusOnError="True" ControlToValidate="txtValorDepositado"
                                                ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="valores" Display="None"> </asp:RegularExpressionValidator>
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
                                            <asp:TextBox ID="txtAssinatura" runat="server" MaxLength="100" Height="16px" Width="150px"
                                                Enabled="False"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorAssinatura" runat="server"
                                                SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Assinatura"
                                                ControlToValidate="txtAssinatura" ValidationGroup="valores"></asp:RequiredFieldValidator>
                                        </div>
                                        <div>
                                            <label>
                                                Data do Depósito:&nbsp;
                                            </label>
                                            <asp:TextBox ID="txtDataDeposito" runat="server" CssClass="textEntry" Height="16px"
                                                Width="150px" Enabled="False"></asp:TextBox>
                                        </div>
                                        <div>
                                            <label>
                                                Obs:&nbsp;
                                            </label>
                                            <asp:TextBox ID="txtObs" runat="server" CssClass="textEntry" Height="80px" Width="450px"
                                                TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                </td>
                                <td style="border: 1px solid #CCC;" valign="top">
                                    <div>
                                        <asp:TextBox ID="txtDiretorio" runat="server" CssClass="pcRight" Height="16px" Width="100px"
                                            Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtArquivo" runat="server" CssClass="pcRight" Height="16px" Width="100px"
                                            Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtCodigoDeposito" runat="server" CssClass="pcRight" Height="16px"
                                            Width="100px" Visible="false"></asp:TextBox>
                                    </div>
                                    <div id="divMovimento" runat="server" visible="false">
                                        <div>
                                            <label>
                                                Conta:</label>
                                            <asp:DropDownList runat="server" ID="ddlContaContabil" DataValueField="CODIGO_BANCO"
                                                DataTextField="NOME" Height="26px" Width="300px">
                                            </asp:DropDownList>
                                        </div>
                                        <div>
                                            <label>
                                                Data de Lançamento:</label>
                                            <asp:TextBox ID="txtDataLancamento" runat="server" CssClass="pcRight" MaxLength="20"
                                                Height="16px" Width="234px"></asp:TextBox>
                                            <asp:Calendar ID="CalendarDataLancamento" runat="server" OnSelectionChanged="CalendarDataLancamento_SelectionChanged"
                                                CaptionAlign="Bottom"></asp:Calendar>
                                        </div>
                                        <div>
                                            <label>
                                                Valor:</label>
                                            <asp:TextBox ID="txtValor" runat="server" CssClass="pcRight" MaxLength="20" Height="16px"
                                                Width="109px"></asp:TextBox>
                                        </div>
                                        <div>
                                            <label>
                                                Imagem:</label>
                                            <asp:FileUpload ID="FileUpload1" runat="server" Width="300px" />
                                        </div>
                                        <p style="height: 13px">
                                            &nbsp;</p>
                                        <div>
                                            <asp:Button runat="server" ID="btSalvar" Text="Salvar Documento" OnClick="btSalvar_Click" />
                                            <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
                                        </div>
                                    </div>
                                    <div>
                                        <asp:GridView runat="server" ID="GridViewDocumento" AutoGenerateColumns="false" ShowFooter="true"
                                            OnDataBound="GridViewDocumento_DataBound" Width="500px" OnRowDataBound="GridViewDocumento_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="NOME_IMAGEM" HeaderText="Imagem" />
                                                <asp:BoundField DataField="CAMINHO_IMAGEM" HeaderText="Caminho" Visible="false" />
                                                <asp:BoundField DataField="VALOR" HeaderText="Valor" />
                                                <asp:TemplateField HeaderText="Banco">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="LiteralBanco" Text=""></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="DATA_LANCAMENTO" HeaderText="Data de Lançamento" />
                                                <asp:BoundField DataField="NUMERO_LANCAMENTO" HeaderText="No. Lançamento" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button runat="server" ID="btExcluir" Text="Excluir" OnClick="btExcluir_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <asp:HiddenField runat="server" ID="HiddenFieldCodigoDocumento" Value="0" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </fieldset>
        <span style="height: 13px">
            <asp:Label runat="server" ID="lblErroGravar" ForeColor="Red"></asp:Label></span>
        <div>
            <asp:Button runat="server" ID="btGravar" Text="Gravar Depósito" OnClick="btGravar_Click"
                ValidationGroup="valores" CausesValidation="true" Enabled="false" OnClientClick="javascript: return confirm('Tem certeza que deseja fazer depósito no Banco?');" />
        </div>
    </div>
</asp:Content>
