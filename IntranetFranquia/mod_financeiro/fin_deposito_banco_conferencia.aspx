<%@ Page Title="Conferência de Depósitos em Banco" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="fin_deposito_banco_conferencia.aspx.cs" Inherits="Relatorios.mod_financeiro.fin_Deposito_Banco_Conferencia" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinhamento {
            position: relative;
            float: left;
        }

        .Label_Grid_Vazio {
            font-weight: bold;
            font-size: 8pt;
            color: red;
            font-family: arial;
            text-align: center;
            background-color: #CCCCCC;
            border-style: none;
        }
    </style>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../../Image/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "../../Image/plus.png");
            $(this).closest("tr").next().remove();
        });
        function AbrirAguarde() {
            document.getElementById('<%= panProcessando.ClientID %>').Visible = true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btSalvarImagemAvulsa" />
        </Triggers>
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Financeiro
            &nbsp;&nbsp;>&nbsp;&nbsp;Conferência de Depósitos em Banco&nbsp;&nbsp;</span>
                <div style="float: right; padding: 0;">
                    <a href="../DefaultFinanceiro.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <asp:Panel ID="panPrincipal" runat="server">
                <div class="accountInfo">
                    <fieldset>
                        <legend>Conferência de Depósitos em Banco - Filtro:</legend>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>Data do Depósito
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 1006px;">
                                    <div style="width: 1000px;" class="alinhamento">
                                        <div style="width: 250px;" class="alinhamento">
                                            <asp:Button ID="btnDiaAnterior" runat="server" Text="<" Width="25px" Height="26px" CssClass="buttonhbf" OnClick="btnDiaAnterior_Click" />
                                            <asp:TextBox ID="txtDeposito" runat="server" onkeypress="return fnValidarData(event);"
                                                Height="19px" MaxLength="10" Width="175px" AutoPostBack="True"></asp:TextBox>
                                            <asp:Button ID="btnDiaSeguinte" runat="server" Text=">" Width="25px" Height="26px" CssClass="buttonhbf" OnClick="btnDiaSeguinte_Click" />
                                            <asp:Calendar ID="CalendarDeposito" runat="server" CaptionAlign="Bottom" OnSelectionChanged="CalendarDeposito_SelectionChanged"></asp:Calendar>
                                        </div>
                                        <div style="width: 750px;" class="alinhamento">
                                            <asp:RadioButton ID="optDataDeposito" runat="server" Text="Buscar pela data do Depósito" GroupName="optTipoData" Checked="true" />
                                            <asp:RadioButton ID="optDataBanco" runat="server" Text="Buscar pela data do Lançamento Bancário" GroupName="optTipoData" />
                                            <asp:RadioButton ID="optDataFechamento" runat="server" Text="Buscar pela data do Fechamento" GroupName="optTipoData" />
                                        </div>
                                        <div style="width: 750px; top: 20px" class="alinhamento">
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="7%">
                                                        <asp:Label ID="Label4" runat="server" Text="Filial:"></asp:Label>
                                                    </td>
                                                    <td width="93%">
                                                        <asp:DropDownList ID="ddlFilial" Width="250px" runat="server" DataValueField="COD_FILIAL" DataTextField="FILIAL" OnDataBound="ddlFilial_DataBound"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div style="width: 750px; top: 40px" class="alinhamento">
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="7%">
                                                        <asp:Label ID="Label5" runat="server" Text="Banco:"></asp:Label>
                                                    </td>
                                                    <td width="35%">
                                                        <asp:DropDownList ID="ddlBanco" Width="250px" runat="server" AutoPostBack="true" DataValueField="NOME" DataTextField="NOME" OnDataBound="ddlBanco_DataBound" OnSelectedIndexChanged="ddlBanco_SelectedIndexChanged"></asp:DropDownList>
                                                    </td>
                                                    <td width="18%" align="center">
                                                        <asp:Label ID="Label6" runat="server" Text="Conta Corrente:"></asp:Label>
                                                    </td>
                                                    <td width="40%">
                                                        <asp:DropDownList ID="ddlContaCorrente" Width="280px" runat="server" DataValueField="CODIGO_BANCO" DataTextField="CONTA_CORRENTE" OnDataBound="ddlContaCorrente_DataBound"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div style="width: 750px; top: 60px" class="alinhamento">
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="7%">
                                                        <asp:Label ID="Label7" runat="server" Text="Status:"></asp:Label>
                                                    </td>
                                                    <td width="93%">
                                                        <asp:DropDownList ID="ddlStatus" Width="150px" runat="server" AutoPostBack="true" DataValueField="CODIGO_BAIXA" DataTextField="DESCRICAO" OnDataBound="ddlStatus_DataBound" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"></asp:DropDownList>
                                                        &nbsp;
                                                    <asp:CheckBox ID="chkIgnorarData" runat="server" Text="Ignorar Data" Visible="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div style="width: 750px; top: 100px" class="alinhamento">
                                            <asp:Button ID="btnLimpar" runat="server" Text="Limpar" Width="100px" OnClick="btnLimpar_Click" />&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="labErro" runat="server" Text="" ForeColor="red" Visible="false"></asp:Label>
                                            <asp:Label ID="labSucesso" runat="server" Text="" ForeColor="blue" Visible="false"></asp:Label>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div class="accountInfo">
                    <fieldset>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <div class="rounded_corners" id="scrollDiv">
                                        <asp:GridView ID="gvDepositos" runat="server" Width="100.5%" AutoGenerateColumns="False"
                                            ForeColor="#333333" Style="background: white"
                                            ShowFooter="true" DataKeyNames="CODIGO_FILIAL,FILIAL,DATA_DEPOSITO,VALOR_DEPOSITADO" OnDataBound="gvDepositos_DataBound" OnRowCommand="gvDepositos_RowCommand" OnRowDataBound="gvDepositos_RowDataBound">
                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Filial" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="1" ItemStyle-Width="150px" SortExpression="FILIAL">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFilial" Text='<%# Bind("FILIAL") %>' runat="server"></asp:Literal>
                                                        <asp:HiddenField ID="hdnFilial" runat="server" Value='<%# Bind("CODIGO_FILIAL") %>'></asp:HiddenField>
                                                        <asp:HiddenField ID="hdnCodigoDepositogrd" runat="server" Value='<%# Bind("CODIGO_DEPOSITO") %>'></asp:HiddenField>
                                                    </ItemTemplate>

                                                    <ItemStyle HorizontalAlign="Left" BorderWidth="1px" Width="150px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Depósito" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="110px" SortExpression="DATA_DEPOSITO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDataDeposito" Text='<%# Bind("DATA_DEPOSITO") %>' runat="server"></asp:Literal>
                                                        <asp:Literal ID="litHoraDeposito" Text='<%# Bind("HORA_DEPOSITO") %>' runat="server"></asp:Literal>
                                                    </ItemTemplate>

                                                    <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="110px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Banco" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="75px" SortExpression="DATA_BANCO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDataBanco" Text='<%# Bind("DATA_BANCO") %>' runat="server"></asp:Literal>
                                                    </ItemTemplate>

                                                    <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="75px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Vl.Depositar" ItemStyle-HorizontalAlign="Right" ItemStyle-BorderWidth="1" ItemStyle-Width="100px" SortExpression="VALOR_A_DEPOSITAR">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litValorADepositar" Text='<%# Bind("VALOR_A_DEPOSITAR") %>' runat="server"></asp:Literal>
                                                    </ItemTemplate>

                                                    <ItemStyle HorizontalAlign="Right" BorderWidth="1px" Width="100px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Vl.Depositado" ItemStyle-HorizontalAlign="Right" ItemStyle-BorderWidth="1" ItemStyle-Width="100px" SortExpression="VALOR_DEPOSITADO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litValorDepositado" Text='<%# Bind("VALOR_DEPOSITADO") %>' runat="server"></asp:Literal>
                                                    </ItemTemplate>

                                                    <ItemStyle HorizontalAlign="Right" BorderWidth="1px" Width="100px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Diferença" ItemStyle-HorizontalAlign="Right" ItemStyle-BorderWidth="1" ItemStyle-Width="80px" SortExpression="DIFERENCA">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDiferenca" Text='<%# Bind("DIFERENCA") %>' runat="server"></asp:Literal>
                                                    </ItemTemplate>

                                                    <ItemStyle HorizontalAlign="Right" BorderWidth="1px" Width="80px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Saldo Anterior" ItemStyle-HorizontalAlign="Right" ItemStyle-BorderWidth="1" ItemStyle-Width="110px" SortExpression="VALOR_SALDO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litValorSaldo" Text='<%# Bind("VALOR_SALDO") %>' runat="server"></asp:Literal>
                                                    </ItemTemplate>

                                                    <ItemStyle HorizontalAlign="Right" BorderWidth="1px" Width="110px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dados Bancários" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="1" ItemStyle-Width="220px" SortExpression="BANCO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litBanco" Text='<%# Bind("BANCO") %>' runat="server"></asp:Literal>
                                                        <asp:Literal ID="litSep1" Text="-Ag:" runat="server"></asp:Literal>
                                                        <asp:Literal ID="litAgencia" Text='<%# Bind("AGENCIA") %>' runat="server"></asp:Literal>
                                                        <asp:Literal ID="litSep2" Text="-CC:" runat="server"></asp:Literal>
                                                        <asp:Literal ID="litContaCorrente" Text='<%# Bind("CONTA_CORRENTE") %>' runat="server"></asp:Literal>
                                                    </ItemTemplate>

                                                    <ItemStyle HorizontalAlign="Left" BorderWidth="1px" Width="220px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dias de Referência" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="1" ItemStyle-Width="150px" SortExpression="DTS_REFERENCIA">
                                                    <ItemTemplate>
                                                        <table border="0" width="150px" cellpadding="0" cellspacing="0" style="border-collapse: collapse">
                                                            <tr>
                                                                <td width="120px" align="center" style="border: 0px solid #000;">
                                                                    <asp:HiddenField ID="hdnCodigoSaldo" runat="server" Value='<%# Bind("CODIGO_SALDO") %>'></asp:HiddenField>
                                                                    <asp:Literal ID="litDTsReferencia" Text='<%# Bind("DTS_REFERENCIA") %>' runat="server"></asp:Literal>
                                                                </td>
                                                                <td width="30px" align="center" style="border: 0px solid #000;">
                                                                    <asp:Button ID="btnReferencia" runat="server" Text="Ver" Width="30px" CssClass="buttonhbf" Style="padding-left: 0.3em;" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Buscar_Referencia" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>

                                                    <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="150px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Imagens" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="50px" SortExpression="IMAGENS">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litImagens" Text='<%# Bind("IMAGENS") %>' runat="server"></asp:Literal>
                                                        <asp:Button ID="btnImagens" runat="server" Text="Ver" Width="50px" CssClass="buttonhbf" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Buscar_Imagens" />
                                                    </ItemTemplate>

                                                    <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="50px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Lançamentos" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="1" ItemStyle-Width="230px" SortExpression="DATA_DEPOSITO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litLancamentos" Text='<%# Bind("LANCAMENTOS") %>' runat="server"></asp:Literal>
                                                        <asp:Literal ID="litSep3" Text="-" runat="server"></asp:Literal>
                                                        <asp:Literal ID="litDataLancamento" Text='<%# Bind("DATA_LANCAMENTO") %>' runat="server"></asp:Literal>
                                                        <asp:Literal ID="litSep4" Text="-" runat="server"></asp:Literal>
                                                        <asp:Literal ID="litConferido" Text='<%# Bind("VERIFICADO") %>' runat="server"></asp:Literal>
                                                        <asp:Button ID="btnObs" runat="server" Text="Obs" Width="30px" CssClass="buttonhbf" Style="padding-left: 0.3em;" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Ver_Obs" />
                                                        <asp:Literal ID="litLblObs" Text="-Obs:" runat="server"></asp:Literal>
                                                        <asp:Literal ID="litObs" Text='<%# Bind("OBS") %>' runat="server"></asp:Literal>
                                                        <asp:Button ID="btnFecharObs" runat="server" Text="<<" Width="19px" CssClass="buttonminhbf" Style="padding-left: 0.2em;" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Fechar_Obs" />

                                                        <asp:Literal ID="litEmAberto" Text="Em Aberto" runat="server"></asp:Literal>
                                                    </ItemTemplate>

                                                    <ItemStyle HorizontalAlign="Left" BorderWidth="1px" Width="230px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Pendente" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="75px" SortExpression="DATA_DEPOSITO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDataPendente" Text='<%# Bind("DATA_PENDENTE") %>' runat="server"></asp:Literal>
                                                        <asp:Button ID="btnPendenteRemover" runat="server" Text="-" Width="15px" CssClass="buttonminhbf" Style="padding-left: 0.3em;" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Deposito_Pendente_Remover" />
                                                        <asp:Button ID="btnPendenteRetornar" runat="server" Text="<<" Width="19px" CssClass="buttonminhbf" Style="padding-left: 0.2em;" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Deposito_Pendente_Retornar" />
                                                        <asp:Button ID="btnPendenteAvancar" runat="server" Text=">>" Width="19px" CssClass="buttonminhbf" Style="padding-left: 0.2em;" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Deposito_Pendente_Avancar" />
                                                    </ItemTemplate>

                                                    <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="75px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Excluir" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="60px" SortExpression="DATA_DEPOSITO">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnExcluir" runat="server" Text="Excluir" Width="55px" CssClass="buttonhbf" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Excluir_Deposito" />
                                                        <asp:Panel ID="panExluir" runat="server" Width="200px" Height="100px" HorizontalAlign="Center" Visible="false" BackColor="#EFF3FB" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px">
                                                            <table border="0" width="100%" cellpadding="0" cellspacing="0" style="border-collapse: collapse">
                                                                <tr style="height: 2px;">
                                                                    <td colspan="2" align="left" style="border: 0px solid #000;"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" align="left" style="border: 0px solid #000;">
                                                                        <asp:Label ID="lblMotivoExcluir" runat="server" Font-Size="Smaller" Text="Motivo:"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" align="center" style="border: 0px solid #000;">
                                                                        <asp:TextBox ID="txtMotivoExclusao" runat="server" Text="" Width="190px" CssClass="textEntry" MaxLength="1000" Height="50px" TextMode="MultiLine"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 2px;">
                                                                    <td colspan="2" align="left" style="border: 0px solid #000;"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="50%" align="left" style="border: 0px solid #000;">
                                                                        <asp:Button ID="btnExcluirDeposito" runat="server" Text="Confirmar" Width="80px" CssClass="buttonhbf" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Excluir_Deposito_Confirmar" Style="margin-left: 2px" />
                                                                    </td>
                                                                    <td width="50%" align="right" style="border: 0px solid #000;">
                                                                        <asp:Button ID="btnCancelarDeposito" runat="server" Text="Cancelar" Width="80px" CssClass="buttonhbf" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Excluir_Deposito_Cancelar" Style="margin-right: 2px" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </ItemTemplate>

                                                    <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="60px"></ItemStyle>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
            </asp:Panel>


            <span id="popReferencia" style="position: fixed; top: 250px; left: 350px;">
                <div id="divReferencia" class="rounded_modals">
                    <asp:Panel ID="panReferencia" runat="server" Width="795px" Height="270px" HorizontalAlign="Center" Visible="false" BackColor="#EFF3FB" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="4px">
                        <table width="780px" cellpadding="0" cellspacing="0">
                            <tr style="height: 4px;">
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text="Filial: "></asp:Label>
                                                <asp:Label ID="lblRefFilial" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" Text="Data do Depósito: "></asp:Label>
                                                <asp:Label ID="lblRefDataDeposito" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label2" runat="server" Text="Data do Banco: "></asp:Label>
                                                <asp:Label ID="lblRefDataBanco" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label18" runat="server" Text="Valor do Documento: "></asp:Label>
                                                <asp:Label ID="lblRefValor" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="height: 5px;">
                                <td>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Panel ID="Panel2" runat="server" Width="790px" Height="203px" HorizontalAlign="Center" ScrollBars="Vertical">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvReferencia"
                                                runat="server" Width="101%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white"
                                                ShowFooter="true" DataKeyNames="CODIGO_FILIAL,DATA_FECHAMENTO,VALOR_DINHEIRO" OnDataBound="gvReferencia_DataBound" OnRowCommand="gvReferencia_RowCommand" OnRowDataBound="gvReferencia_RowDataBound">
                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Data do Fechamento" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="160px" SortExpression="DATA_FECHAMENTO">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litDataFechamento" Text='<%# Bind("DATA_FECHAMENTO") %>' runat="server"></asp:Literal>
                                                            <asp:HiddenField ID="hdnCodigoFilial" runat="server" Value='<%# Bind("CODIGO_FILIAL") %>'></asp:HiddenField>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="160px" HorizontalAlign="Center" BorderWidth="1px" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Valor Dinheiro" ItemStyle-HorizontalAlign="Right" ItemStyle-BorderWidth="1" ItemStyle-Width="150px" SortExpression="VALOR_DINHEIRO">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litValorDinheiro" Text='<%# Bind("VALOR_DINHEIRO") %>' runat="server"></asp:Literal>
                                                        </ItemTemplate>

                                                        <ItemStyle HorizontalAlign="Right" BorderWidth="1px" Width="150px"></ItemStyle>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Valor Retirada" ItemStyle-HorizontalAlign="Right" ItemStyle-BorderWidth="1" ItemStyle-Width="150px" SortExpression="VALOR_RETIRADA">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litValorRetirada" Text='<%# Bind("VALOR_RETIRADA") %>' runat="server"></asp:Literal>
                                                        </ItemTemplate>

                                                        <ItemStyle HorizontalAlign="Right" BorderWidth="1px" Width="150px"></ItemStyle>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Valor Devolução" ItemStyle-HorizontalAlign="Right" ItemStyle-BorderWidth="1" ItemStyle-Width="150px" SortExpression="VALOR_DEVOLUCAO">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litValorDevolucao" Text='<%# Bind("VALOR_DEVOLUCAO") %>' runat="server"></asp:Literal>
                                                        </ItemTemplate>

                                                        <ItemStyle HorizontalAlign="Right" BorderWidth="1px" Width="150px"></ItemStyle>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Valor para Depósito" ItemStyle-HorizontalAlign="Right" ItemStyle-BorderWidth="1" ItemStyle-Width="150px" SortExpression="VALOR_DEPOSITO">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litValorDeposito" Text='<%# Bind("VALOR_DEPOSITO") %>' runat="server"></asp:Literal>
                                                        </ItemTemplate>

                                                        <ItemStyle HorizontalAlign="Right" BorderWidth="1px" Width="150px"></ItemStyle>
                                                    </asp:TemplateField>

                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lbl_Vazio" runat="server" CssClass="Label_Grid_Vazio" Text="Nenhum Fechamento foi Localizado !" Width="790px"></asp:Label>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr style="height: 7px;">
                                <td></td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btnFecharReferencia" runat="server" Text="Fechar" CssClass="buttonhbf" OnClick="btnFecharReferencia_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </span>


            <span id="popImagens" style="position: fixed; top: 5px; left: 380px;">
                <div id="divImagens" class="rounded_modals">
                    <asp:Panel ID="panImagens" runat="server" Width="820px" Height="860px" HorizontalAlign="Center" Visible="false" BackColor="#EFF3FB" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="4px">
                        <table width="800px" cellpadding="0" cellspacing="0">
                            <tr style="height: 4px;">
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label8" runat="server" Text="Filial: "></asp:Label>
                                                <asp:Label ID="lblImgFilial" runat="server" Text=""></asp:Label>
                                                <asp:HiddenField ID="hdnImgCodFilial" runat="server"></asp:HiddenField>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label10" runat="server" Text="Data do Depósito: "></asp:Label>
                                                <asp:Label ID="lblImgDataDeposito" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label12" runat="server" Text="Data do Banco: "></asp:Label>
                                                <asp:Label ID="lblImgDataBanco" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label14" runat="server" Text="Valor do Documento: "></asp:Label>
                                                <asp:Label ID="lblImgValor" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:Button ID="btnFecharImagens" runat="server" Text="X" CssClass="buttonhbf" OnClick="btnFecharImagens_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="height: 1px;">
                                <td>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <fieldset class="login" style="margin: 2px; padding: 2px;">
                                        <legend style="text-align: left;"><font size="2" face="Calibri">Imagens vinculadas ao lançamento Linx</font></legend>
                                        <asp:Panel ID="panImagensVinculadasNaoLancadas" runat="server" Width="810px" Height="120px" HorizontalAlign="Center" ScrollBars="Vertical">
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvImagensVinculadas"
                                                    runat="server" Width="100%" AutoGenerateColumns="False" Font-Size="9px"
                                                    ForeColor="#333333" Style="background: white"
                                                    ShowFooter="true" DataKeyNames="CODIGO_DOCUMENTO,NOME_IMAGEM,DATA_LANCAMENTO,VALOR" OnDataBound="gvImagensVinculadas_DataBound" OnRowCommand="gvImagensVinculadas_RowCommand" OnRowDataBound="gvImagensVinculadas_RowDataBound">
                                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Arquivo" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="1" ItemStyle-Width="400px" SortExpression="NOME_IMAGEM">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="linkNomeImagemVinculada" Text='<%# Eval("NOME_IMAGEM") %>' runat="server" Target="_blank"></asp:HyperLink>
                                                                <asp:Literal ID="litNomeImagemVinculada" Text='<%# Bind("NOME_IMAGEM") %>' runat="server"></asp:Literal>
                                                                <asp:HiddenField ID="hdnCodigoDocumento" runat="server" Value='<%# Bind("CODIGO_DOCUMENTO") %>'></asp:HiddenField>
                                                                <asp:HiddenField ID="hdnCodigoBanco" runat="server" Value='<%# Bind("CODIGO_BANCO") %>'></asp:HiddenField>
                                                                <asp:HiddenField ID="hdnCaminhoImagem" runat="server" Value='<%# Bind("CAMINHO_IMAGEM") %>'></asp:HiddenField>
                                                                <asp:HiddenField ID="hdnNomeImagemVinculada" runat="server" Value='<%# Bind("NOME_IMAGEM") %>'></asp:HiddenField>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="400px" HorizontalAlign="Left" BorderWidth="1px" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Banco" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="1" ItemStyle-Width="250px">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litBancoDepositado" Text='' runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Valor Depositado" ItemStyle-HorizontalAlign="Right" ItemStyle-BorderWidth="1" ItemStyle-Width="180px">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litVlrDepositadoImgVinculada" Text='<%# Bind("VALOR") %>' runat="server"></asp:Literal>
                                                            </ItemTemplate>

                                                            <ItemStyle HorizontalAlign="Right" BorderWidth="1px" Width="180px"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Data do Lançamento" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="150px">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litDataLancamento" Text='<%# Bind("DATA_LANCAMENTO") %>' runat="server"></asp:Literal>
                                                            </ItemTemplate>

                                                            <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="150px"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Excluir" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="60px">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnExcluirImgDocto" runat="server" Text="Excluir" Width="50px" CssClass="buttonhbf" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Excluir_Imagem" />
                                                            </ItemTemplate>

                                                            <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="60px"></ItemStyle>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lbl_Vazio" runat="server" CssClass="Label_Grid_Vazio" Text="Nenhuma Imagem Vinculada !" Width="800px"></asp:Label>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel ID="panImagensVinculadasLancadas" runat="server" Width="810px" Height="120px" HorizontalAlign="Center" ScrollBars="Vertical">
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvImagensLancadas"
                                                    runat="server" Width="100%" AutoGenerateColumns="False" Font-Size="9px"
                                                    ForeColor="#333333" Style="background: white"
                                                    ShowFooter="true" DataKeyNames="CODIGO_DOCUMENTO,NOME_IMAGEM,DATA_LANCAMENTO,VALOR" OnDataBound="gvImagensLancadas_DataBound" OnRowCommand="gvImagensLancadas_RowCommand" OnRowDataBound="gvImagensLancadas_RowDataBound">
                                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Arquivo" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="1" ItemStyle-Width="300px" SortExpression="NOME_IMAGEM">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="linkNomeImagemVinculada" Text='<%# Eval("NOME_IMAGEM") %>' runat="server" Target="_blank"></asp:HyperLink>
                                                                <asp:Literal ID="litNomeImagemVinculada" Text='<%# Bind("NOME_IMAGEM") %>' runat="server"></asp:Literal>
                                                                <asp:HiddenField ID="hdnCodigoDocumento" runat="server" Value='<%# Bind("CODIGO_DOCUMENTO") %>'></asp:HiddenField>
                                                                <asp:HiddenField ID="hdnCaminhoImagem" runat="server" Value='<%# Bind("CAMINHO_IMAGEM") %>'></asp:HiddenField>
                                                                <asp:HiddenField ID="hdnNomeImagemVinculada" runat="server" Value='<%# Bind("NOME_IMAGEM") %>'></asp:HiddenField>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="300px" HorizontalAlign="Left" BorderWidth="1px" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="NOME_IMAGEM" HeaderText="Imagem" Visible="false" />
                                                        <asp:BoundField DataField="CAMINHO_IMAGEM" HeaderText="Caminho" Visible="false" />
                                                        <asp:BoundField DataField="VALOR" HeaderText="Valor" />
                                                        <asp:TemplateField HeaderText="Banco">
                                                            <ItemTemplate>
                                                                <asp:Literal runat="server" ID="LiteralBanco" Text=""></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="DATA_LANCAMENTO" HeaderText="Dt.Lançamento" DataFormatString="{0:dd/MM/yyyy}" />
                                                        <asp:BoundField DataField="NUMERO_LANCAMENTO" HeaderText="No.Lançamento" />
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Button runat="server" ID="btExcluir" Text="Excluir" Width="50px" CssClass="buttonhbf" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Excluir_Imagem" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lbl_Vazio" runat="server" CssClass="Label_Grid_Vazio" Text="Nenhuma Imagem Vinculada !" Width="800px"></asp:Label>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </div>
                                        </asp:Panel>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <fieldset class="login" style="margin: 2px; padding: 2px;">
                                        <legend style="text-align: left;"><font size="2" face="Calibri">Fechamentos do dia do depósito selecionado</font></legend>
                                        <asp:Panel ID="popImagensReferencias" runat="server" Width="810px" Height="120px" HorizontalAlign="Center" ScrollBars="Vertical">
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvImagensReferencias"
                                                    runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ForeColor="#333333" Style="background: white"
                                                    ShowFooter="true" DataKeyNames="CODIGO_FILIAL,DATA_FECHAMENTO,VALOR_DINHEIRO" OnDataBound="gvImagensReferencias_DataBound" OnRowDataBound="gvImagensReferencias_RowDataBound">
                                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Data do Fechamento" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="190px" SortExpression="DATA_FECHAMENTO">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litDataFechamento" Text='<%# Bind("DATA_FECHAMENTO") %>' runat="server"></asp:Literal>
                                                                <asp:HiddenField ID="hdnCodigoFilial" runat="server" Value='<%# Bind("CODIGO_FILIAL") %>'></asp:HiddenField>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="190px" HorizontalAlign="Center" BorderWidth="1px" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Valor Dinheiro" ItemStyle-HorizontalAlign="Right" ItemStyle-BorderWidth="1" ItemStyle-Width="150px" SortExpression="VALOR_DINHEIRO">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litValorDinheiro" Text='<%# Bind("VALOR_DINHEIRO") %>' runat="server"></asp:Literal>
                                                            </ItemTemplate>

                                                            <ItemStyle HorizontalAlign="Right" BorderWidth="1px" Width="150px"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Valor Retirada" ItemStyle-HorizontalAlign="Right" ItemStyle-BorderWidth="1" ItemStyle-Width="150px" SortExpression="VALOR_RETIRADA">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litValorRetirada" Text='<%# Bind("VALOR_RETIRADA") %>' runat="server"></asp:Literal>
                                                            </ItemTemplate>

                                                            <ItemStyle HorizontalAlign="Right" BorderWidth="1px" Width="150px"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Valor Devolução" ItemStyle-HorizontalAlign="Right" ItemStyle-BorderWidth="1" ItemStyle-Width="150px" SortExpression="VALOR_DEVOLUCAO">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litValorDevolucao" Text='<%# Bind("VALOR_DEVOLUCAO") %>' runat="server"></asp:Literal>
                                                            </ItemTemplate>

                                                            <ItemStyle HorizontalAlign="Right" BorderWidth="1px" Width="150px"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Valor para Depósito" ItemStyle-HorizontalAlign="Right" ItemStyle-BorderWidth="1" ItemStyle-Width="150px" SortExpression="VALOR_DEPOSITO">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litValorDeposito" Text='<%# Bind("VALOR_DEPOSITO") %>' runat="server"></asp:Literal>
                                                            </ItemTemplate>

                                                            <ItemStyle HorizontalAlign="Right" BorderWidth="1px" Width="150px"></ItemStyle>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lbl_Vazio" runat="server" CssClass="Label_Grid_Vazio" Text="Nenhum Fechamento foi Localizado !" Width="800px"></asp:Label>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </div>
                                        </asp:Panel>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <fieldset class="login" style="margin: 2px; padding: 2px;">
                                        <legend style="text-align: left;"><font size="2" face="Calibri">Imagens gravadas no dia do depósito selecionado</font></legend>
                                        <asp:Panel ID="Panel1" runat="server" Width="810px" Height="220px" HorizontalAlign="Center" ScrollBars="Vertical">
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvImagensDeposito"
                                                    runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ForeColor="#333333" Style="background: white" Font-Size="9px"
                                                    ShowFooter="true" DataKeyNames="CODIGO_IMAGEM,NOME_IMAGEM,DATA_DIGITADA" OnRowCommand="gvImagensDeposito_RowCommand" OnRowDataBound="gvImagensDeposito_RowDataBound" OnDataBound="gvImagensDeposito_DataBound">
                                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Arquivo" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="1" ItemStyle-Width="400px" SortExpression="NOME_IMAGEM">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="linkNomeImagemDeposito" Text='<%# Eval("NOME_IMAGEM") + " (" + Eval("LOCAL_IMAGEM") + ")" %>' runat="server" Target="_blank"></asp:HyperLink>
                                                                <asp:HiddenField ID="hdnCodigoImagem" runat="server" Value='<%# Bind("CODIGO_IMAGEM") %>'></asp:HiddenField>
                                                                <asp:HiddenField ID="hdnLocalImagem" runat="server" Value='<%# Bind("LOCAL_IMAGEM") %>'></asp:HiddenField>
                                                                <asp:HiddenField ID="hdnNomeImagem" Value='<%# Bind("NOME_IMAGEM") %>' runat="server"></asp:HiddenField>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="400px" HorizontalAlign="Left" BorderWidth="1px" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Data Digitada" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="150px" SortExpression="DATA_DIGITADA">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litDataDigitada" Text='<%# Bind("DATA_DIGITADA") %>' runat="server"></asp:Literal>
                                                            </ItemTemplate>

                                                            <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="150px"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Vínculo Lançamento Linx" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="1" ItemStyle-Width="240px">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnVincular" runat="server" Text="Vincular" Width="100px" CssClass="buttonhbf" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Vincular_Imagem" />
                                                                <asp:Panel ID="panVincular" runat="server" Width="240px" Height="140px" HorizontalAlign="Center" Visible="false" BackColor="#EFF3FB" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px">
                                                                    <table border="0" width="100%" cellpadding="0" cellspacing="0" style="border-collapse: collapse">
                                                                        <tr style="height: 2px;">
                                                                            <td colspan="2" align="left" style="border: 0px solid #000;"></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2" align="left" style="border: 0px solid #000;">
                                                                                <asp:Label ID="lblCCVinculo" runat="server" Font-Size="Smaller" Text="Conta:"></asp:Label>
                                                                                <asp:DropDownList runat="server" ID="ddlCCVinculo" Height="20px" Width="235px" DataValueField="CODIGO_BANCO"
                                                                                    DataTextField="NOME">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="height: 2px;">
                                                                            <td colspan="2" align="left" style="border: 0px solid #000;"></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2" align="left" style="border: 0px solid #000;">
                                                                                <asp:Label ID="lblValLctoVinculo" runat="server" Font-Size="Smaller" Text="Valor:"></asp:Label><br />
                                                                                <asp:TextBox ID="txtValLctoVinculo" runat="server" CssClass="pcRight" MaxLength="20" Height="15px" Width="100px"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="height: 2px;">
                                                                            <td colspan="2" align="left" style="border: 0px solid #000;"></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2" align="left" style="border: 0px solid #000;">
                                                                                <asp:Label ID="lblDtLctoVinculo" runat="server" Font-Size="Smaller" Text="Data:"></asp:Label><br />
                                                                                <asp:TextBox ID="txtDtLctoVinculo" runat="server" MaxLength="10" Height="15px" Width="230px"></asp:TextBox>
                                                                                <%--<asp:Button ID="btnDtLctoVinculo" runat="server" Text="..." Width="20px" CssClass="buttonhbf" Height="20px" Style="padding-left: 0.3em;" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Abrir_Calendario_Vinculo" />--%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="height: 5px;">
                                                                            <td colspan="2" align="left" style="border: 0px solid #000;"></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="50%" align="left" style="border: 0px solid #000;">
                                                                                <asp:Button ID="btnConfirmarVinculo" runat="server" Text="Confirmar" Width="80px" CssClass="buttonhbf" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Confirmar_Vinculo" Style="margin-left: 2px" />
                                                                            </td>
                                                                            <td width="50%" align="right" style="border: 0px solid #000;">
                                                                                <asp:Button ID="btnCancelarVinculo" runat="server" Text="Cancelar" Width="80px" CssClass="buttonhbf" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Cancelar_Vinculo" Style="margin-right: 2px" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                                <asp:Panel ID="panImgCalendarioVinculo" runat="server" Width="240px" Height="195px" HorizontalAlign="Center" Visible="false" BackColor="#EFF3FB" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px">
                                                                    <table border="0" width="100%" cellpadding="0" cellspacing="0" style="border-collapse: collapse">
                                                                        <tr style="height: 2px;">
                                                                            <td align="left" style="border: 0px solid #000;"></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="border: 0px solid #000;">
                                                                                <asp:Calendar ID="CalendarDtLctoVinculo" runat="server"
                                                                                    CaptionAlign="Bottom" OnSelectionChanged="CalendarDtLctoVinculo_SelectionChanged"></asp:Calendar>
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="height: 5px;">
                                                                            <td align="left" style="border: 0px solid #000;"></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="right" style="border: 0px solid #000;">
                                                                                <asp:Button ID="btnFecharCalendarDtLctoVinculo" runat="server" Text="Voltar" Width="80px" CssClass="buttonhbf" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Fechar_Calendario_Vinculo" Style="margin-right: 2px" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </ItemTemplate>

                                                            <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="240px"></ItemStyle>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lbl_Vazio" runat="server" CssClass="Label_Grid_Vazio" Text="Nenhuma Imagem Gravada com o Depósito !" Width="800px"></asp:Label>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </div>
                                        </asp:Panel>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="60%">
                                                <fieldset class="login" style="margin: 2px; padding: 2px;">
                                                    <legend><font size="2" face="Calibri">Vincular imagens avulsas</font></legend>
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="27%" rowspan="4">
                                                                <div style="width: 200px; height" class="alinhamento">
                                                                    <label><font size="2" face="Calibri">Data de Lançamento:</font></label>
                                                                    <asp:TextBox ID="txtDtLctoImgAvulsa" runat="server" MaxLength="10"
                                                                        Height="15px" Width="196px" AutoPostBack="True"></asp:TextBox>
                                                                    <asp:Calendar ID="CalendarDtLctoImgAvulsa" runat="server"
                                                                        CaptionAlign="Bottom" OnSelectionChanged="CalendarDtLctoImgAvulsa_SelectionChanged"></asp:Calendar>
                                                                </div>
                                                            </td>
                                                            <td width="73%" valign="top">
                                                                <label><font size="2" face="Calibri">Conta:</font></label>
                                                                <asp:DropDownList runat="server" ID="ddlCCImgAvulsa" Height="20px" Width="300px" DataValueField="CODIGO_BANCO"
                                                                    DataTextField="NOME">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" valign="top">
                                                                <label><font size="2" face="Calibri">Valor:</font></label>
                                                                <asp:TextBox ID="txtValorImgAvulsa" runat="server" CssClass="pcRight" MaxLength="20" Width="100px" AutoPostBack="True"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" valign="top">
                                                                <label><font size="2" face="Calibri">Imagem do Comprovante:</font></label>
                                                                <asp:FileUpload ID="FileUpload1" runat="server" Width="300px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" valign="bottom" align="right">
                                                                <asp:Label ID="lblErroImg" runat="server" Text="" ForeColor="Blue"></asp:Label>
                                                                <asp:Button runat="server" ID="btSalvarImagemAvulsa" Text="Salvar Documento" CssClass="buttonhbf" OnClick="btSalvarImagemAvulsa_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                            <td width="40%" valign="top">
                                                <fieldset class="login" style="margin: 2px; padding: 2px;">
                                                    <legend><font size="2" face="Calibri">Lançamento Linx</font></legend>
                                                    <table border="0" width="100%" cellpadding="0" cellspacing="0" style="border-collapse: collapse">
                                                        <tr>
                                                            <td width="45%" align="left" style="border: 0px solid #000;">
                                                                <label><font size="2" face="Calibri">Valor Real:</font></label>
                                                                <asp:TextBox ID="txtValorReal" runat="server" CssClass="pcRight" MaxLength="20" Width="100px" AutoPostBack="True"></asp:TextBox>
                                                            </td>
                                                            <td width="55%" align="left" style="border: 0px solid #000;">
                                                                <label><font size="2" face="Calibri">Diferença:</font></label>
                                                                <asp:TextBox ID="txtDiferenca" runat="server" CssClass="pcRight" MaxLength="20" Width="100px" AutoPostBack="True"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 10px;">
                                                            <td colspan="2" align="left" style="border: 0px solid #000;"></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="left" style="border: 0px solid #000;">
                                                                <asp:Label ID="lblObs" runat="server" Font-Size="Smaller" Text="Observação:"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="left" style="border: 0px solid #000;">
                                                                <asp:TextBox ID="txtObs" runat="server" Width="275px" CssClass="textEntry" MaxLength="1000" Height="107px" TextMode="MultiLine"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 10px;">
                                                            <td colspan="2" align="left" style="border: 0px solid #000;"></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" style="border: 0px solid #000;">
                                                                <asp:Button ID="btnGravarLancamento" runat="server" Text="Gravar Lançamento Linx" Width="150px" CssClass="buttonhbf" OnClick="btnGravarLancamento_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </span>

            <span id="popMensagem" style="position: fixed; top: 100px; left: 500px;">
                <div id="divMensagem" class="rounded_modals">
                    <asp:Panel ID="panMensagem" runat="server" Width="505px" Height="200px" HorizontalAlign="left" Visible="false" BackColor="#EFF3FB" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="4px">
                        <table width="500px" cellpadding="0" cellspacing="0">
                            <tr style="height: 10px;">
                                <td colspan="2"></td>
                            </tr>
                            <tr>
                                <td width="18%" align="center" valign="top">
                                    <img src="/Image/warning.png" alt="Alerta" />
                                </td>
                                <td width="82%">
                                    <asp:TextBox ID="txtMensagemErro" runat="server" Text="" Width="400px" Height="150px" TextMode="MultiLine" ForeColor="Red" BorderStyle="None" ReadOnly="true" BackColor="#EFF3FB"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="height: 5px;">
                                <td colspan="2"></td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btnFecharMensagem" runat="server" Width="80px"
                                        Text="OK" CssClass="buttonhbf" OnClick="btnFecharMensagem_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </span>

            <span id="popProcessando" style="position: fixed; top: 100px; left: 500px;">
                <div class="rounded_modals">
                    <asp:Panel ID="panProcessando" runat="server" Width="505px" Height="200px" HorizontalAlign="left" Visible="false" BackColor="#EFF3FB" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="4px">
                        Carregando. Por favor aguarde...<br />
                        <br />
                        <img src="/Image/update.png" alt="" />
                    </asp:Panel>
                </div>
            </span>

            <asp:HiddenField ID="hdnCodigoDeposito_Consultas" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hdnCodigoFilial_Consultas" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hdnDataDeposito_Consultas" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hdnLinha" runat="server"></asp:HiddenField>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
