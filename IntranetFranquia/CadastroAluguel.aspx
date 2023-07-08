<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="CadastroAluguel.aspx.cs" Inherits="Relatorios.CadastroAluguel" %>

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
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="accountInfo" style="float: left;">
                <fieldset class="login">
                    <legend>Cadastro de Aluguéis</legend>
                    <table class="style1" border="0" width="100%">
                        <tr>
                            <td style="width: 250px;">
                                <fieldset class="" style="height: 500px">
                                    <legend>Filial</legend>
                                    <div>
                                        <label>
                                            Filial:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </label>
                                        <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                            Height="22px" Width="230px" OnDataBound="ddlFilial_DataBound">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorFilial" ControlToValidate="ddlFilial"
                                            InitialValue="0" ValidationGroup="contas" ErrorMessage="Selecione uma Filial."></asp:RequiredFieldValidator>
                                    </div>
                                    <div>
                                        <label>
                                            Vencimento:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </label>
                                        <asp:DropDownList runat="server" ID="ddlReferencia" DataValueField="CODIGO_ACOMPANHAMENTO_MESANO"
                                            DataTextField="DESCRICAO" Height="22px" Width="230px" OnDataBound="ddlReferencia_DataBound">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorReferencia"
                                            ControlToValidate="ddlReferencia" InitialValue="0" ValidationGroup="contas" ErrorMessage="Selecione um Vencimento."></asp:RequiredFieldValidator>
                                    </div>
                                    <div>
                                        <asp:Button runat="server" ID="btBuscarAluguel" Text="Buscar Aluguel" OnClick="btBuscarAluguel_Click"
                                            ValidationGroup="contas" />
                                        <asp:ValidationSummary ID="ValidationSummaryBuscar" runat="server" ValidationGroup="contas"
                                            ShowMessageBox="false" ShowSummary="false" />
                                        <asp:Label runat="server" ID="lblBuscarAluguel" ForeColor="Red"></asp:Label>
                                    </div>
                                </fieldset>
                            </td>
                            <td style="width: 10px;">&nbsp;
                            </td>
                            <td style="width: 180px;">
                                <div id="divItem" runat="server" style="width: 280px;">
                                    <fieldset class="" style="height: 500px">
                                        <legend>Item do Aluguel</legend>
                                        <label>
                                            Data Vencimento:&nbsp;
                                        </label>
                                        <asp:TextBox ID="txtDataVencimento" Enabled="false" runat="server" CssClass="textEntry"
                                            Height="22px" Width="200px"></asp:TextBox>
                                        <asp:Calendar ID="CalendarDataVencimento" runat="server" Width="202px" OnSelectionChanged="CalendarDataVencimento_SelectionChanged"
                                            CaptionAlign="Bottom" OnDayRender="CalendarDataVencimento_DayRender"></asp:Calendar>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorData" ControlToValidate="txtDataVencimento"
                                            InitialValue="0" ValidationGroup="pesquisa" ErrorMessage="Selecione uma Data de Vencimento"></asp:RequiredFieldValidator>
                                        <label>
                                            Descrição:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </label>
                                        <asp:DropDownList runat="server" ID="ddlContas" DataValueField="CODIGO_ACOMPANHAMENTO_ALUGUEL_DESPESA"
                                            DataTextField="DESCRICAO" Height="22px" Width="200px" OnDataBound="ddlContas_DataBound">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorContas" ControlToValidate="ddlContas"
                                            InitialValue="0" ValidationGroup="pesquisa" ErrorMessage="Selecione uma Conta de Despesa"></asp:RequiredFieldValidator>
                                        <label>
                                            Valor:&nbsp;
                                        </label>
                                        <asp:TextBox ID="txtValor" runat="server" CssClass="pcRight" MaxLength="25" Height="22px"
                                            Width="200px" textAlign="rigth"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorValor" runat="server" SetFocusOnError="true"
                                            Display="None" ErrorMessage="Preencha o campo Valor" ControlToValidate="txtValor"
                                            ValidationGroup="pesquisa"></asp:RequiredFieldValidator>
                                        <p style="height: 10px">
                                            &nbsp;
                                        </p>
                                        <label>
                                            Valor Boleto:&nbsp;
                                        </label>
                                        <asp:DropDownList runat="server" ID="ddlValorBoleto" Height="22px" Width="200px">
                                            <asp:ListItem Value="S" Text="SIM"></asp:ListItem>
                                            <asp:ListItem Value="N" Text="NÃO"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorValorBoleto" runat="server" SetFocusOnError="true"
                                            Display="None" ErrorMessage="Selecione o Valor Boleto" ControlToValidate="ddlValorBoleto"
                                            ValidationGroup="pesquisa"></asp:RequiredFieldValidator>
                                        <p style="height: 13px">
                                            &nbsp;
                                        </p>
                                        <asp:Button runat="server" ID="ButtonSalvar" Text="Incluir" OnClick="ButtonSalvar_Click"
                                            ValidationGroup="pesquisa" Enabled="false" />
                                        <asp:ValidationSummary ID="ValidationSummaryConta" runat="server" ValidationGroup="pesquisa"
                                            ShowMessageBox="true" ShowSummary="false" />
                                        <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
                                    </fieldset>
                                </div>
                            </td>
                            <td style="width: 10px;">&nbsp;
                            </td>
                            <td style="width: 500px;">
                                <div id="divGrid" runat="server">
                                    <fieldset style="height: 500px; width: 690px;">
                                        <legend>Aluguéis</legend>
                                        <fieldset style="width: 750px; visibility: hidden;">
                                            <legend>Conferido</legend>
                                            <asp:GridView runat="server" ID="GridViewAluguel" AutoGenerateColumns="false" Width="650px"
                                                OnRowDataBound="GridViewAluguel_RowDataBound" OnDataBound="GridViewAluguel_DataBound"
                                                ShowFooter="True">
                                                <FooterStyle HorizontalAlign="Left" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Filial">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="LiteralFilial" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Vencimento" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="LiteralReferencia" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Item" HeaderStyle-Width="150px">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="LiteralAluguel" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="VALOR" HeaderText="Valor" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="110px" />
                                                    <asp:TemplateField HeaderText="Valor Boleto" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="LiteralValorBoleto" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--
                                                        <asp:TemplateField Visible="false"> <ItemTemplate> <asp:Button runat="server" ID="btExcluirAluguel" Text="Excluir" OnClick="btExcluirAluguel_Click" /> </ItemTemplate> </asp:TemplateField>
                                                    --%>
                                                </Columns>
                                            </asp:GridView>
                                        </fieldset>
                                        <asp:GridView runat="server" ID="GridViewAluguelPendente" AutoGenerateColumns="false"
                                            Width="750px" OnRowDataBound="GridViewAluguelPendente_RowDataBound" OnDataBound="GridViewAluguelPendente_DataBound"
                                            ShowFooter="true">
                                            <FooterStyle HorizontalAlign="Left" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Filial">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="LiteralFilial" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Vencimento" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="LiteralReferencia" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item" HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="LiteralAluguel" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="VALOR" HeaderText="Valor" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="110px" />
                                                <asp:TemplateField HeaderText="Valor Boleto" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="LiteralValorBoleto" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button runat="server" ID="btExcluirAluguelPend" Height="20px" Text="Excluir"
                                                            OnClick="btExcluirAluguelPend_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <div style="float: right;">
                                            <asp:Button runat="server" ID="btConferir" Text="Conferir Aluguel" OnClick="btConferir_Click" />
                                        </div>
                                    </fieldset>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <asp:HiddenField runat="server" ID="HiddenFieldCodigoAluguelItem" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
