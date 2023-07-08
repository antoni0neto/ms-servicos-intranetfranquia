<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="AlteraNel.aspx.cs" Inherits="Relatorios.AlteraNel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
    <div class="accountInfo">
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
            <legend>Critérios de Busca</legend>
            <div style="width: 1200px;" class="alinhamento">
                <div style="width: 200px;"  class="alinhamento">
                    <label>Coleção:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlColecao" DataValueField="CODIGO_COLECAO" DataTextField="DESCRICAO" Height="26px" 
                        Width="200px" ondatabound="ddlColecao_DataBound"></asp:DropDownList>
                </div>
                <div style="width: 200px;"  class="alinhamento">
                    <label>Janela:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlJanela" DataValueField="CODIGO_JANELA" DataTextField="DESCRICAO" Height="26px" 
                        Width="200px" ondatabound="ddlJanela_DataBound"></asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btBuscarNel" Text="Buscar Nel" OnClick="btBuscarNel_Click" CausesValidation = "false"/>
        </div>
        <fieldset class="login">
            <legend>Cadastro de NeL</legend>
            <table class="style1">
                <tr>
                    <td>
                        <fieldset class="login">
                            <div>
                                <label>Código:&nbsp;&nbsp;&nbsp; </label>
                                <asp:TextBox ID="txtCodigo" runat="server" CssClass="textEntry" MaxLength="10" Height="22px" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCodigo" runat="server" SetFocusOnError="true"
                                        Display="None" ErrorMessage="Preencha o código da NeL" ControlToValidate="txtCodigo" ValidationGroup="nel"></asp:RequiredFieldValidator>
                            </div>
                            <div>
                                <label>Descrição:&nbsp; </label>
                                <asp:TextBox ID="txtDescricao" runat="server" CssClass="textEntry" MaxLength="100" Height="22px" Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorDescricao" runat="server" SetFocusOnError="true"
                                        Display="None" ErrorMessage="Preencha a descrição da NeL" ControlToValidate="txtDescricao" ValidationGroup="nel"></asp:RequiredFieldValidator>
                            </div>
                            <div style="width: 200px;" class="alinhamento">
                                <label>Data Pagamento(dd/mm/aaaa):&nbsp; </label>
                                <asp:TextBox ID="txtDataPagamento" runat="server" CssClass="data" Height="22px" Width="198px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorDataPagamento" runat="server" SetFocusOnError="true" Display="None" ErrorMessage="Escolha Data do Pagamento" ControlToValidate="txtDataPagamento"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionDataPagamento" runat="server" ErrorMessage="Data do Pagamento Inválida !!!"
                                    SetFocusOnError="True" ControlToValidate="txtDataPagamento" ValidationExpression="(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d"
                                    ValidationGroup="nel" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                        </fieldset>
                    </td>
                    <td>
                        <fieldset class="login">
                            <div>
                                <label>Valor do Pagamento:&nbsp; </label>
                                <asp:TextBox ID="txtValorPagamento" runat="server" CssClass="textEntry" MaxLength="20" Height="20px" Width="50px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorValorPagamento" runat="server" SetFocusOnError="true"
                                        Display="None" ErrorMessage="Preencha o valor do pagamento" ControlToValidate="txtValorPagamento"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorValorPagamento" runat="server" ErrorMessage="Valor Pagamento Inválido !!!"
                                    SetFocusOnError="True" ControlToValidate="txtValorPagamento" ValidationExpression="^[0-9]+(\,[0-9]{1,2})?$" ValidationGroup="nel" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>Coleção:&nbsp; </label>
                                <asp:DropDownList runat="server" ID="ddlColecaoNel" DataValueField="CODIGO_COLECAO" DataTextField="DESCRICAO" Height="22px" Width="230px" ondatabound="ddlColecaoNel_DataBound"></asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorColecaoNel" ControlToValidate="ddlColecaoNel" InitialValue="0"></asp:RequiredFieldValidator>
                            </div>
                            <div>
                                <label>Janela:&nbsp; </label>
                                <asp:DropDownList runat="server" ID="ddlJanelaNel" DataValueField="CODIGO_JANELA" DataTextField="DESCRICAO" Height="22px" Width="230px" ondatabound="ddlJanelaNel_DataBound"></asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorJanelaNel" ControlToValidate="ddlJanelaNel" InitialValue="0"></asp:RequiredFieldValidator>
                            </div>
                            <div>
                                <label>Container:&nbsp; </label>
                                <asp:DropDownList runat="server" ID="ddlContainer" DataValueField="CODIGO_CONTAINER" DataTextField="DESCRICAO" Height="22px" Width="230px" ondatabound="ddlContainer_DataBound"></asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorContainer" ControlToValidate="ddlContainer" InitialValue="0"></asp:RequiredFieldValidator>
                            </div>
                        </fieldset>
                    </td>
                    <td>
                        <fieldset class="login">
                            <div>
                                <label>Data Draft(dd/mm/aaaa):&nbsp; </label>
                                <asp:TextBox ID="txtDataDraft" runat="server" CssClass="data" Height="16px" Width="198px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorDataDraft" runat="server" SetFocusOnError="true" Display="None" ErrorMessage="Escolha Data do Draft" ControlToValidate="txtDataDraft"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorDataDraft" runat="server" ErrorMessage="Data do Draft Inválida !!!"
                                    SetFocusOnError="True" ControlToValidate="txtDataDraft" ValidationExpression="(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d"
                                    ValidationGroup="nel" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <label>Data Recebimento(dd/mm/aaaa):&nbsp; </label>
                                <asp:TextBox ID="txtDataRecebimento" runat="server" CssClass="data" Height="16px" Width="198px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorDataRecebimento" runat="server" SetFocusOnError="true" Display="None" ErrorMessage="Escolha Data do Recebimento" ControlToValidate="txtDataRecebimento"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorDataRecebimento" runat="server" ErrorMessage="Data do Recebimento Inválida !!!"
                                    SetFocusOnError="True" ControlToValidate="txtDataRecebimento" ValidationExpression="(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d"
                                    ValidationGroup="nel" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div style="width: 200px;" class="alinhamento">
                                <label>Data Despachante(dd/mm/aaaa):&nbsp; </label>
                                <asp:TextBox ID="txtDataDespachante" runat="server" CssClass="data" Height="16px" Width="198px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorDataDespachante" runat="server" SetFocusOnError="true" Display="None" ErrorMessage="Escolha Data do Despachante" ControlToValidate="txtDataDespachante"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorDataDespachante" runat="server" ErrorMessage="Data do Despachante Inválida !!!"
                                    SetFocusOnError="True" ControlToValidate="txtDataDespachante" ValidationExpression="(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d"
                                    ValidationGroup="nel" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div style="width: 200px;" class="alinhamento">
                                <label>Data LI(dd/mm/aaaa):&nbsp; </label>
                                <asp:TextBox ID="txtDataLI" runat="server" CssClass="data" Height="16px" Width="198px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorDataLI" runat="server" SetFocusOnError="true" Display="None" ErrorMessage="Escolha Data da LI" ControlToValidate="txtDataLI"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorDataLI" runat="server" ErrorMessage="Data da LI Inválida !!!"
                                    SetFocusOnError="True" ControlToValidate="txtDataLI" ValidationExpression="(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d"
                                    ValidationGroup="nel" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                        </fieldset>
                    </td>
                    <td>
                        <fieldset class="login">
                            <div style="width: 200px;" class="alinhamento">
                                <label>Data Booking(dd/mm/aaaa):&nbsp; </label>
                                <asp:TextBox ID="txtDataBooking" runat="server" CssClass="data" Height="16px" Width="198px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorDataBooking" runat="server" SetFocusOnError="true" Display="None" ErrorMessage="Escolha Data do Booking" ControlToValidate="txtDataBooking"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorDataBooking" runat="server" ErrorMessage="Data do Booking Inválida !!!"
                                    SetFocusOnError="True" ControlToValidate="txtDataBooking" ValidationExpression="(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d"
                                    ValidationGroup="nel" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div style="width: 200px;" class="alinhamento">
                                <label>Data Embarque(dd/mm/aaaa):&nbsp; </label>
                                <asp:TextBox ID="txtDataEmbarque" runat="server" CssClass="data" Height="16px" Width="198px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorDataEmbarque" runat="server" SetFocusOnError="true" Display="None" ErrorMessage="Escolha Data do Embarque" ControlToValidate="txtDataEmbarque"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorDataEmbarque" runat="server" ErrorMessage="Data do Embarque Inválida !!!"
                                    SetFocusOnError="True" ControlToValidate="txtDataEmbarque" ValidationExpression="(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d"
                                    ValidationGroup="nel" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div style="width: 200px;" class="alinhamento">
                                <label>Data Chegada(dd/mm/aaaa):&nbsp; </label>
                                <asp:TextBox ID="txtDataChegada" runat="server" CssClass="data" Height="16px" Width="198px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorDataChegada" runat="server" SetFocusOnError="true" Display="None" ErrorMessage="Escolha Data da Chegada" ControlToValidate="txtDataChegada"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorDataChegada" runat="server" ErrorMessage="Data da Chegada Inválida !!!"
                                    SetFocusOnError="True" ControlToValidate="txtDataChegada" ValidationExpression="(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d"
                                    ValidationGroup="nel" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                            <div style="width: 200px;" class="alinhamento">
                                <label>Data Desembaraço(dd/mm/aaaa):&nbsp; </label>
                                <asp:TextBox ID="txtDataDesembaraco" runat="server" CssClass="data" Height="16px" Width="198px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorDataDesembaraco" runat="server" SetFocusOnError="true" Display="None" ErrorMessage="Escolha Data do Desembaraço" ControlToValidate="txtDataDesembaraco"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorDataDesembaraco" runat="server" ErrorMessage="Data do Desembaraço Inválida !!!"
                                    SetFocusOnError="True" ControlToValidate="txtDataDesembaraco" ValidationExpression="(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d"
                                    ValidationGroup="nel" Display="None"> </asp:RegularExpressionValidator>
                            </div>
                        </fieldset>
                    </td>
                    <td>
                        <fieldset class="login">
                            <div>
                                <label>Original: </label>
                                <asp:CheckBox ID="cbOriginal" runat="server" />
                            </div>
                            <div>
                                <label>DI:&nbsp; </label>
                                <asp:TextBox ID="txtDI" runat="server" CssClass="textEntry" MaxLength="100" Height="22px" Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorDI" runat="server" SetFocusOnError="true"
                                        Display="None" ErrorMessage="Preencha a descrição da DI" ControlToValidate="txtDI"></asp:RequiredFieldValidator>
                            </div>
                            <div>
                                <label>BL:&nbsp; </label>
                                <asp:TextBox ID="txtBL" runat="server" CssClass="textEntry" MaxLength="100" Height="22px" Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorBL" runat="server" SetFocusOnError="true"
                                        Display="None" ErrorMessage="Preencha a descrição da BL" ControlToValidate="txtBL"></asp:RequiredFieldValidator>
                            </div>
                        </fieldset>
                    </td>
                    <td>
                        <fieldset class="login">
                            <div>
                                <label>Pack: </label>
                                <asp:CheckBox ID="cbPack" runat="server" />
                            </div>
                            <div>
                                <label>Comercial Invoice: </label>
                                <asp:CheckBox ID="cbComercialInvoce" runat="server" />
                            </div>
                            <div>
                                <label>Status de Liberação:&nbsp; </label>
                                <asp:TextBox ID="txtStatus" runat="server" CssClass="textEntry" MaxLength="100" Height="22px" Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorStatus" runat="server" SetFocusOnError="true"
                                        Display="None" ErrorMessage="Preencha a descrição do Status" ControlToValidate="txtStatus"></asp:RequiredFieldValidator>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="ButtonSalvar" Text="Salvar" OnClick="ButtonSalvar_Click" ValidationGroup="nel"/>
            <asp:ValidationSummary ID="ValidationSummaryNel" runat="server" ValidationGroup="nel" ShowMessageBox="true" ShowSummary="false" />
            <asp:Label runat="server" ID="LabelFeedBack" ForeColor="Red"></asp:Label>
        </div>
    </div>
    <div>
        <asp:GridView runat="server" ID="GridViewNel" AutoGenerateColumns="false" onrowdatabound="GridViewNel_RowDataBound">
            <Columns>
                <asp:BoundField DataField="CODIGO_NEL" HeaderText="Código" />
                <asp:BoundField DataField="DESCRICAO_NEL" HeaderText="Descrição" />
                <asp:BoundField DataField="DATA_PAGAMENTO" HeaderText="Data do Pagamento" />
                <asp:BoundField DataField="VALOR_PAGAMENTO" HeaderText="Valor do Pagamento" />
                <asp:BoundField DataField="DATA_DRAFT" HeaderText="Data do Draft" />
                <asp:BoundField DataField="ORIGINAL" HeaderText="Original" />
                <asp:BoundField DataField="DATA_RECEBIMENTO" HeaderText="Data do Recebimento" />
                <asp:BoundField DataField="DATA_DESPACHANTE" HeaderText="Data do Despachante" />
                <asp:BoundField DataField="DATA_LI" HeaderText="Data da LI" />
                <asp:BoundField DataField="DATA_BOOKING" HeaderText="Data do Booking" />
                <asp:BoundField DataField="DATA_EMBARQUE" HeaderText="Data do Embarque" />
                <asp:BoundField DataField="DESCRICAO_DI" HeaderText="DI" />
                <asp:BoundField DataField="DESCRICAO_BL" HeaderText="BL" />
                <asp:BoundField DataField="PACK" HeaderText="Pack" />
                <asp:BoundField DataField="COMERCIAL_INVOCE" HeaderText="Comercial Invoice" />
                <asp:BoundField DataField="DATA_PREVISAO_CHEGADA" HeaderText="Data da Chegada" />
                <asp:BoundField DataField="DATA_DESEMBARACO" HeaderText="Data do Desembaraço" />
                <asp:BoundField DataField="STATUS_LIBERACAO" HeaderText="Status de Liberação" />
                <asp:BoundField DataField="CODIGO_CONTAINER" HeaderText="Container" />
                <asp:BoundField DataField="CODIGO_COLECAO" HeaderText="Coleção" />
                <asp:BoundField DataField="CODIGO_JANELA" HeaderText="Janela" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonEditar" Text="Editar" OnClick="ButtonEditarNel_Click" CausesValidation="false"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="ButtonExcluir" Text="Excluir" OnClick="ButtonExcluirNel_Click" CausesValidation="false"/>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <asp:HiddenField runat="server" ID="HiddenFieldCodigoNel" Value="0" />
</asp:Content>
