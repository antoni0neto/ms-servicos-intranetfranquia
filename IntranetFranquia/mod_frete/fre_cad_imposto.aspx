<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="fre_cad_imposto.aspx.cs" Inherits="Relatorios.mod_frete.fre_cad_imposto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .jGrowl .manilla {
            background-color: #000;
            color: red;
            font-weight: bold;
        }

        .imageButtonEditOrDelete {
            width: 25px !important;
            height: 25px !important;
        }

        .ui-widget input {
            font-family: Verdana,Arial,sans-serif;
            font-size: 1em;
            height: 18px !important;
        }

        .legend {
            font-weight: 600;
            padding: 2px 4px 8px 4px;
            font-weight: 600;
            font-size: 1.1em;
            font-family: "Helvetica Neue", "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
            color: #696969;
        }
    </style>
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            $("#tabs").tabs();
        });

        function MarcarAba(vAba) {
            document.getElementById("MainContent_hidTabSelected").value = vAba;
        }

        function LocalizaAWB(e) {
            if (e && e.keyCode == 13) {
                __doPostBack('<%=((TextBox)frvCadImposto.FindControl("txtAWB")).ClientID %>', '');
            }
        };

        function CalculaDesembaraco(e) {
            if (e && e.keyCode == 13) {
                __doPostBack('<%=((TextBox)frvCadImposto.FindControl("txtTaxaDolar")).ClientID %>', '');
            }
        };       

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="updFormImposto" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo Frete&nbsp;&nbsp;>&nbsp;&nbsp;
                    Recebimentos/Impostos&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Imposto</span>
                <div style="float: right; padding: 0;">
                    <a href="fre_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <br />
            <div style="float: left; width: 1000px; margin-left: 15%;">
                <fieldset>
                    <legend>Cadastro de Imposto</legend>
                    <%--Botoes de Acao--%>
                    <div style="padding-left: 0px;">
                        <asp:ImageButton ID="ibtNovo" runat="server" Width="18px" ImageUrl="~/Image/add.png"
                            ToolTip="Novo" OnClick="ibtNovo_Click" Visible="true" />
                        <asp:ImageButton ID="ibtSalvar" CommandArgument="I" runat="server" Width="19px" ImageUrl="~/Image/approve.png"
                            ToolTip="Salvar" OnClick="ibtSalvar_Click" />
                        <asp:ImageButton ID="ibtCancelar" runat="server" Width="18px" ImageUrl="~/Image/cancel.png"
                            ToolTip="Cancelar" OnClick="ibtCancelar_Click" Visible="False" />
                        <%-- <asp:ImageButton ID="ibtEditar" runat="server" Width="20px" ImageUrl="~/Image/edit.jpg"
                            ToolTip="Editar" OnClick="ibtEditar_Click" />--%>
                        <asp:ImageButton ID="ibtExcluir" runat="server" Width="18px" ImageUrl="~/Image/delete.png"
                            ToolTip="Excluir" OnClick="ibtExcluirImpostoFormulario_Click" OnClientClick="return ConfirmarExclusao();" Visible="False" />
                        <asp:ImageButton ID="ibtLimpar" runat="server" Width="18px" ImageUrl="~/Image/clean.png"
                            ToolTip="Limpar" OnClick="ibtLimpar_Click" />
                        <asp:ImageButton ID="ibtPesquisar" runat="server" Width="19px" ImageUrl="~/Image/search.png"
                            ToolTip="Pesquisar" OnClick="ibtPesquisar_Click" />
                        <asp:HiddenField ID="hidTabSelected" runat="server" Value="" />
                        <asp:HiddenField ID="hidAcao" runat="server" Value="" />
                        <div style="text-align: right; float: right;">
                            <asp:Label ID="labErroGeral" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                    <hr />

                    <br />
                    <%--Abas exibidas na tela--%>
                    <div id="tabs">
                        <ul>
                            <li>
                                <a href="#tabs-imposto" id="tabImposto" runat="server" onclick="MarcarAba(0);">Inclusão/Alteração de Imposto</a>

                            </li>
                            <li>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </li>
                            <li>
                                <a href="#tabs-impostos-salvos" id="tabImpostoSalvos" runat="server" onclick="MarcarAba(1);">Todos</a>

                            </li>
                        </ul>
                        <div id="tabs-imposto">
                            <fieldset>
                                <legend class="legend">Imposto</legend>
                                <asp:ObjectDataSource ID="odsImposto" runat="server"
                                    DataObjectTypeName="DAL.FRETE_IMPOSTO"
                                    DeleteMethod="ExcluirFreteImposto"
                                    InsertMethod="InserirFreteImposto"
                                    SelectMethod="ObterFreteImposto"
                                    TypeName="DAL.FreteController"
                                    UpdateMethod="AtualizarFreteImposto">
                                    <DeleteParameters>
                                        <asp:Parameter Name="CODIGO" Type="Int32" DefaultValue="0"></asp:Parameter>
                                    </DeleteParameters>
                                    <SelectParameters>
                                        <asp:Parameter Name="CODIGO" Type="Int32" DefaultValue="0" />
                                        <asp:Parameter Name="AWB" Type="String" ConvertEmptyStringToNull="true"></asp:Parameter>
                                        <asp:Parameter Name="DATA_RECEBIMENTO" Type="DateTime"></asp:Parameter>
                                        <asp:Parameter Name="STATUS" Type="String" ConvertEmptyStringToNull="true"></asp:Parameter>
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <asp:FormView ID="frvCadImposto"
                                    Width="100%"
                                    runat="server"
                                    DataSourceID="odsImposto"
                                    DataKeyNames="CODIGO"
                                    OnItemInserting="frvCadImposto_ItemInserting"
                                    OnItemInserted="frvCadImposto_ItemInserted"
                                    OnItemUpdating="frvCadImposto_ItemUpdating"
                                    OnItemUpdated="frvCadImposto_ItemUpdated"
                                    OnDataBound="frvCadImposto_DataBound">
                                    <EmptyDataTemplate>
                                        <asp:Label runat="server" Text="Nenhum registro encontrado." />
                                    </EmptyDataTemplate>
                                    <ItemTemplate>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr style="height: 10px; vertical-align: bottom;">
                                                <td>
                                                    <asp:Label ID="labAWB" runat="server" Text="AWB"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labTaxaDolar" runat="server" Text="Taxa do Dolar" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labPeso" runat="server" Text="Peso" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labTotalFaturaEmDolar" runat="server" Text="Total Fatura ($)" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labValorTotalFaturaReal" runat="server" Text="Total Fatura (R$)" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labValorFrete" runat="server" Text="Valor Frete" />
                                                </td>

                                            </tr>
                                            <tr>
                                                <td style="width: 150px;">
                                                    <asp:TextBox ID="txtAWB" MaxLength="12" runat="server" Width="140px" onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                                </td>
                                                <td style="width: 140px;">
                                                    <asp:TextBox ID="txtTaxaDolar" MaxLength="8" CssClass="alinharDireita" runat="server" Width="130px" AutoPostBack="true" onkeypress="return fnValidarNumeroDecimal(event);CalculaDesembaraco(event);" OnTextChanged="txtTaxaDolar_TextChanged"></asp:TextBox>
                                                </td>
                                                <td style="width: 140px;">
                                                    <asp:TextBox ID="txtPeso" MaxLength="8" CssClass="alinharDireita" runat="server" Width="130px" AutoPostBack="true" onkeypress="return fnValidarNumeroDecimal(event);" OnTextChanged="txtPeso_TextChanged"></asp:TextBox>
                                                </td>
                                                <td style="width: 160px;">
                                                    <asp:TextBox ID="txtTotalFaturaEmDolar" Enabled="false" CssClass="alinharDireita" MaxLength="10" runat="server" Width="150px" ></asp:TextBox>
                                                </td>
                                                <td style="width: 160px;">
                                                    <asp:TextBox ID="txtValorTotalFaturaReal" Enabled="false" CssClass="alinharDireita" MaxLength="10" runat="server" Width="150px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValorFrete" Enabled="false" CssClass="alinharDireita" MaxLength="8" runat="server" Width="130px" ></asp:TextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="labValorII" runat="server" Text="Valor II" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labValorICMS" runat="server" Text="Valor ICMS" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labValorDesembaraco" runat="server" Text="Valor Desembaraço" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labValorReembInfra" runat="server" Text="Valor Reemb. Infra" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labFreteTaxaCombustivel" runat="server" Text="Taxa Combustível (%)" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labValorTotalImposto" runat="server" Text="Total Imposto" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtValorII" Enabled="false" CssClass="alinharDireita" MaxLength="8" runat="server" Width="140px" ></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValorICMS" Enabled="false" CssClass="alinharDireita" MaxLength="8" runat="server" Width="130px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValorDesembaraco" Enabled="false" CssClass="alinharDireita" MaxLength="8" runat="server" Width="130px" ></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValorReembInfra" Enabled="false" CssClass="alinharDireita" MaxLength="8" runat="server" Width="150px" ></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFreteTaxaCombustivel" Enabled="false" CssClass="alinharDireita" MaxLength="8" runat="server" Width="150px" ></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValorTotalImposto" Enabled="false" CssClass="alinharDireita" MaxLength="9999" runat="server" Width="130px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6"></td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">Boleto OK?
                                                    <asp:CheckBox ID="chkBoletoOK" runat="server" />
                                                </td>
                                            </tr>
                                        </table>

                                    </ItemTemplate>
                                    <InsertItemTemplate>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr style="height: 10px; vertical-align: bottom;">
                                                <td>
                                                    <asp:Label ID="labAWB" runat="server" Text="AWB"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labTaxaDolar" runat="server" Text="Taxa do Dolar" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labPeso" runat="server" Text="Peso" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labTotalFaturaEmDolar" runat="server" Text="Total Fatura ($)" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labValorTotalFaturaReal" runat="server" Text="Total Fatura (R$)" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labValorFrete" runat="server" Text="Valor Frete" />
                                                </td>

                                            </tr>
                                            <tr>
                                                <td style="width: 150px;">
                                                    <asp:TextBox ID="txtAWB" MaxLength="12" runat="server" Width="140px" onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                                </td>
                                                <td style="width: 140px;">
                                                    <asp:TextBox ID="txtTaxaDolar" MaxLength="8" CssClass="alinharDireita" runat="server" Width="130px" Text='<%#Bind("TAXA_DOLAR") %>' AutoPostBack="true" onkeypress="return fnValidarNumeroDecimal(event);CalculaDesembaraco(event);" OnTextChanged="txtTaxaDolar_TextChanged"></asp:TextBox>
                                                </td>
                                                <td style="width: 140px;">
                                                    <asp:TextBox ID="txtPeso" MaxLength="8" CssClass="alinharDireita" runat="server" Width="130px" Text='<%#Bind("PESO") %>' AutoPostBack="true" onkeypress="return fnValidarNumeroDecimal(event);" OnTextChanged="txtPeso_TextChanged"></asp:TextBox>
                                                </td>
                                                <td style="width: 160px;">
                                                    <asp:TextBox ID="txtTotalFaturaEmDolar" Enabled="false" CssClass="alinharDireita" MaxLength="8" runat="server" Width="150px" Text='<%#Bind("VALOR_FATURA_DOLAR") %>'></asp:TextBox>
                                                </td>
                                                <td style="width: 160px;">
                                                    <asp:TextBox ID="txtValorTotalFaturaReal" Enabled="false" CssClass="alinharDireita" MaxLength="9999" runat="server" Width="150px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValorFrete" Enabled="false" CssClass="alinharDireita" MaxLength="8" runat="server" Width="130px" Text='<%#Eval("VALOR_FRETE") %>' onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="labValorII" runat="server" Text="Valor II" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labValorICMS" runat="server" Text="Valor ICMS" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labValorDesembaraco" runat="server" Text="Valor Desembaraço" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labValorReembInfra" runat="server" Text="Valor Reemb. Infra" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labFreteTaxaCombustivel" runat="server" Text="Taxa Combustível (%)" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labValorTotalImposto" runat="server" Text="Total Imposto" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtValorII" Enabled="false" CssClass="alinharDireita" MaxLength="8" runat="server" Width="140px" Text='<%#Eval("VALOR_II") %>' onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValorICMS" Enabled="false" CssClass="alinharDireita" MaxLength="8" runat="server" Width="130px" Text='<%#Bind("VALOR_ICMS") %>' onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValorDesembaraco" Enabled="false" CssClass="alinharDireita" MaxLength="8" runat="server" Width="130px" Text='<%#Bind("VALOR_DESEMBARACO") %>' onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValorReembInfra" Enabled="false" CssClass="alinharDireita" MaxLength="8" runat="server" Width="150px" Text='<%#Bind("VALOR_REEMB_INFRA") %>' onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFreteTaxaCombustivel" Enabled="false" CssClass="alinharDireita" MaxLength="8" runat="server" Width="150px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValorTotalImposto" Enabled="false" CssClass="alinharDireita" MaxLength="9999" runat="server" Width="130px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6"></td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    Boleto OK?
                                                    <asp:CheckBox ID="chkBoletoOK" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr style="height: 10px; vertical-align: bottom;">
                                                <td>
                                                    <asp:Label ID="labAWB" runat="server" Text="AWB"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="labTaxaDolar" runat="server" Text="Taxa do Dolar" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labPeso" runat="server" Text="Peso" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labTotalFaturaEmDolar" runat="server" Text="Total Fatura ($)" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labValorTotalFaturaReal" runat="server" Text="Total Fatura (R$)" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labValorFrete" runat="server" Text="Valor Frete" />
                                                </td>

                                            </tr>
                                            <tr>
                                                <td style="width: 150px;">
                                                    <asp:TextBox ID="txtAWB" Text='<%#Eval("FRETE_RECEBIMENTO_NOTA1.AWB") %>' MaxLength="12" runat="server" Width="140px" onkeypress="return fnValidarNumero(event);" Enabled="false"></asp:TextBox>
                                                </td>
                                                <td style="width: 140px;">
                                                    <asp:TextBox ID="txtTaxaDolar" MaxLength="8" CssClass="alinharDireita" runat="server" Width="130px" Text='<%#Bind("TAXA_DOLAR") %>' AutoPostBack="true" onkeypress="return fnValidarNumeroDecimal(event);CalculaDesembaraco(event);" OnTextChanged="txtTaxaDolar_TextChanged"></asp:TextBox>
                                                </td>
                                                <td style="width: 140px;">
                                                    <asp:TextBox ID="txtPeso" MaxLength="8" CssClass="alinharDireita" runat="server" Width="130px" Text='<%#Bind("PESO") %>' AutoPostBack="true" onkeypress="return fnValidarNumeroDecimal(event);" OnTextChanged="txtPeso_TextChanged"></asp:TextBox>
                                                </td>
                                                <td style="width: 160px;">
                                                    <asp:TextBox ID="txtTotalFaturaEmDolar" Enabled="false" CssClass="alinharDireita" MaxLength="8" runat="server" Width="150px" Text='<%#Bind("VALOR_FATURA_DOLAR") %>'></asp:TextBox>
                                                </td>
                                                <td style="width: 160px;">
                                                    <asp:TextBox ID="txtValorTotalFaturaReal" Enabled="false" CssClass="alinharDireita" MaxLength="9999" runat="server" Width="150px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValorFrete" Enabled="false" CssClass="alinharDireita" MaxLength="8" runat="server" Width="130px" Text='<%#Eval("VALOR_FRETE") %>' onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="labValorII" runat="server" Text="Valor II" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labValorICMS" runat="server" Text="Valor ICMS" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labValorDesembaraco" runat="server" Text="Valor Desembaraço" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labValorReembInfra" runat="server" Text="Valor Reemb. Infra" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labFreteTaxaCombustivel" runat="server" Text="Taxa Combustível (%)" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labValorTotalImposto" runat="server" Text="Total Imposto" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtValorII" Enabled="false" CssClass="alinharDireita" MaxLength="8" runat="server" Width="140px" Text='<%#Eval("VALOR_II") %>' onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValorICMS" Enabled="false" CssClass="alinharDireita" MaxLength="8" runat="server" Width="130px" Text='<%#Bind("VALOR_ICMS") %>' onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValorDesembaraco" Enabled="false" CssClass="alinharDireita" MaxLength="8" runat="server" Width="130px" Text='<%#Bind("VALOR_DESEMBARACO") %>' onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValorReembInfra" Enabled="false" CssClass="alinharDireita" MaxLength="8" runat="server" Width="150px" Text='<%#Bind("VALOR_REEMB_INFRA") %>' onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFreteTaxaCombustivel" Enabled="false" CssClass="alinharDireita" MaxLength="8" runat="server" Width="150px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtValorTotalImposto" Enabled="false" CssClass="alinharDireita" MaxLength="9999" runat="server" Width="130px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6"></td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">Boleto OK?
                                                    <asp:CheckBox ID="chkBoletoOK" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                </asp:FormView>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>&nbsp;<asp:Label ID="labErroImposto" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>

                            </fieldset>

                        </div>

                        <div id="tabs-impostos-salvos">
                            <div class="rounded_corners">
                                <%--Grid com listagem de Impostos--%>
                                <asp:ObjectDataSource ID="odsGridImposto" runat="server"
                                    DataObjectTypeName="DAL.FRETE_IMPOSTO"
                                    SelectMethod="ObterFreteImposto"
                                    TypeName="DAL.FreteController"
                                    UpdateMethod="AtualizarFreteImposto">
                                    <SelectParameters>
                                        <asp:Parameter Name="CODIGO" Type="Int32" DefaultValue="0"></asp:Parameter>
                                        <asp:Parameter Name="AWB" Type="String" ConvertEmptyStringToNull="true"></asp:Parameter>
                                        <asp:Parameter Name="DATA_RECEBIMENTO" Type="DateTime"></asp:Parameter>
                                        <asp:Parameter Name="STATUS" Type="String" ConvertEmptyStringToNull="true"></asp:Parameter>
                                    </SelectParameters>
                                </asp:ObjectDataSource>

                                <asp:GridView ID="gvFreteImposto" runat="server"
                                    Width="100%"
                                    AutoGenerateColumns="false"
                                    AllowPaging="true"
                                    AllowSorting="true"
                                    EnablePaging="true"
                                    GridLines="None"
                                    Style="background: white"
                                    ForeColor="#333333"
                                    ShowFooter="true"
                                    DataKeyNames="CODIGO"
                                    DataSourceID="odsGridImposto"
                                    OnRowCommand="gvFreteImposto_RowCommand">
                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left"></HeaderStyle>
                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                    <Columns>

                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" Text='<%# (Container.DataItemIndex + 1) %>' runat="server"></asp:Literal>

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:ButtonField ButtonType="Image" ImageUrl="~/Image/edit.jpg" Text="Editar" ItemStyle-Height="15px" ItemStyle-Width="15px" CommandName="Select" />

                                        <asp:BoundField DataField="CODIGO" HeaderText="Código" ItemStyle-Width="30px" Visible="false" />

                                        <asp:TemplateField HeaderText="AWB" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("FRETE_RECEBIMENTO_NOTA1.AWB") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="PESO" HeaderText="Peso" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />

                                        <asp:BoundField DataField="TAXA_DOLAR" DataFormatString="{0:c}" HeaderText="Taxa Dólar" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />

                                        <asp:BoundField DataField="VALOR_FATURA_DOLAR" DataFormatString="{0:c}" HeaderText="Valor Fatura ($)" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />

                                        <asp:BoundField DataField="VALOR_FRETE" DataFormatString="{0:c}" HeaderText="Valor Frete" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />

                                        <asp:BoundField DataField="VALOR_II" DataFormatString="{0:c}" HeaderText="Valor II" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />

                                        <asp:BoundField DataField="VALOR_ICMS" DataFormatString="{0:c}" HeaderText="Valor ICMS" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />

                                        <asp:TemplateField HeaderText="Boleto OK?" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Convert.ToBoolean(Eval("BOLETO"))? "SIM": "NÃO" %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btExcluirImposto" runat="server" OnClick="btExcluirImposto_Click" ImageUrl="~/Image/delete.png" Text="Excluir" OnClientClick="return ConfirmarExclusao();" CommandArgument='<%#Eval("CODIGO") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </fieldset>
                <br />
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
