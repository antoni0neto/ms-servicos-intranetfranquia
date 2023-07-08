<%@ Page Title="Cadastro de Aluguéis" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="manutm_cad_aluguel_antigo.aspx.cs" Inherits="Relatorios.manutm_cad_aluguel_antigo" %>

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
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Manutenção Mensal&nbsp;&nbsp;>&nbsp;&nbsp;Cadastros&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Aluguéis</span>
                <div style="float: right; padding: 0;">
                    <a href="manutm_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="accountInfo" style="float: left;">
                <fieldset class="login">
                    <legend>Cadastro de Aluguéis</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="width: 240px;">
                                <fieldset style="height: 500px;">
                                    <legend>Filial</legend>
                                    <div>
                                        <label>
                                            Filial
                                        </label>
                                        <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="22px" Width="200px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorFilial" ControlToValidate="ddlFilial"
                                            InitialValue="" ValidationGroup="contas" ErrorMessage="Selecione uma Filial"></asp:RequiredFieldValidator>
                                    </div>
                                    <div>
                                        <label>
                                            Vencimento
                                        </label>
                                        <asp:DropDownList runat="server" ID="ddlReferencia" DataValueField="CODIGO_ACOMPANHAMENTO_MESANO"
                                            DataTextField="DESCRICAO" Height="22px" Width="200px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorReferencia"
                                            ControlToValidate="ddlReferencia" InitialValue="0" ValidationGroup="contas" ErrorMessage="Selecione um Vencimento."></asp:RequiredFieldValidator>
                                    </div>
                                    <div>
                                        <asp:Button runat="server" ID="btBuscarAluguel" Text="Buscar Aluguel" Width="120px" OnClick="btBuscarAluguel_Click"
                                            ValidationGroup="contas" />
                                        <asp:ValidationSummary ID="ValidationSummaryBuscar" runat="server" ValidationGroup="contas"
                                            ShowMessageBox="false" ShowSummary="false" />
                                        <asp:Label runat="server" ID="lblBuscarAluguel" ForeColor="Red"></asp:Label>
                                    </div>
                                </fieldset>
                            </td>
                            <td style="width: 20px;">&nbsp;
                            </td>
                            <td style="width: 240px;">
                                <fieldset style="height: 500px">
                                    <legend>Item do Aluguel</legend>
                                    <label>
                                        Data Vencimento
                                    </label>
                                    <asp:TextBox ID="txtDataVencimento" Enabled="false" runat="server" CssClass="textEntry"
                                        Height="21px" Width="196px"></asp:TextBox>

                                    <asp:Calendar ID="calVencimento" runat="server" Width="202px" OnSelectionChanged="calVencimento_SelectionChanged"
                                        CaptionAlign="Bottom" OnDayRender="calVencimento_DayRender"></asp:Calendar>

                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorData" ControlToValidate="txtDataVencimento"
                                        InitialValue="0" ValidationGroup="pesquisa" ErrorMessage="Selecione uma Data de Vencimento"></asp:RequiredFieldValidator>
                                    <label>
                                        Descrição
                                    </label>
                                    <asp:DropDownList runat="server" ID="ddlContas" DataValueField="CODIGO_ACOMPANHAMENTO_ALUGUEL_DESPESA"
                                        DataTextField="DESCRICAO" Height="22px" Width="200px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorContas" ControlToValidate="ddlContas"
                                        InitialValue="0" ValidationGroup="pesquisa" ErrorMessage="Selecione uma Conta de Despesa"></asp:RequiredFieldValidator>
                                    <label>
                                        Valor
                                    </label>
                                    <asp:TextBox ID="txtValor" runat="server" CssClass="pcRight" MaxLength="15" Height="21px"
                                        Width="196px" textAlign="rigth"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorValor" runat="server" SetFocusOnError="true"
                                        Display="None" ErrorMessage="Preencha o campo Valor" ControlToValidate="txtValor"
                                        ValidationGroup="pesquisa"></asp:RequiredFieldValidator>
                                    <label>Desconto:</label>
                                    <asp:TextBox ID="txtDesconto" runat="server" CssClass="pcRight" MaxLength="15" Height="21px"
                                        Width="196px" textAlign="rigth"></asp:TextBox>
                                    <p style="height: 10px">
                                        &nbsp;
                                    </p>
                                    <label>
                                        Tem Boleto?
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
                                    <asp:Button runat="server" ID="btSalvar" Text="Incluir" Width="120px" OnClick="btSalvar_Click"
                                        ValidationGroup="pesquisa" Enabled="false" />
                                    <asp:ValidationSummary ID="ValidationSummaryConta" runat="server" ValidationGroup="pesquisa"
                                        ShowMessageBox="true" ShowSummary="false" />
                                    <asp:Label runat="server" ID="labErro" ForeColor="Red"></asp:Label>
                                </fieldset>
                            </td>
                            <td style="width: 20px;">&nbsp;
                            </td>
                            <td style="width: 100%;">
                                <fieldset style="height: 500px;">
                                    <legend>Aluguéis</legend>
                                    <asp:GridView runat="server" ID="gvAluguel" AutoGenerateColumns="false"
                                        Width="100%" OnRowDataBound="gvAluguel_RowDataBound" OnDataBound="gvAluguel_DataBound"
                                        ShowFooter="true">
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Bold="true" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item" HeaderStyle-Width="250px">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="litAluguel" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tem Boleto?" HeaderStyle-Width="110px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="litBoleto" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="VALOR" HeaderText="Valor (R$)" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="140px" />
                                            <asp:BoundField DataField="DESCONTO" HeaderText="Desconto (R$)" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="140px" />

                                            <asp:TemplateField HeaderText="Total (R$)" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="litTotal" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btExcluir" Height="20px" Width="100px" Text="Excluir"
                                                        OnClick="btExcluir_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <asp:HiddenField runat="server" ID="hidCodigoAluguelItem" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
