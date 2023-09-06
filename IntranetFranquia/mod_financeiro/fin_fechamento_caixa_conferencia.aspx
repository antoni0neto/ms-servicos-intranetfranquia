<%@ Page Title="Conferência de Fechamento de Caixa" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="fin_fechamento_caixa_conferencia.aspx.cs" Inherits="Relatorios.mod_financeiro.fin_fechamento_caixa_conferencia" MaintainScrollPositionOnPostback="true" %>

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

        .style1 {
            width: 100%;
        }
    </style>
    <script src="../../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
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
        function AbrirJanela(codigoCaixa) {
            var left = ((screen.width / 2) - (800 / 2)) / 2;
            var top = ((screen.height / 2) - (800 / 2)) / 2;
            window.open("../AlterarFechamentoCaixa.aspx?CodigoCaixa=" + codigoCaixa, "Fechamento de Caixa", "toolbar=no, location=no, directories=no, status=no, menubar=no, resizable=no, copyhistory=no, height = 765 , width = 1520, scrollbars = no, top=" + top + ", left=" + left);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Financeiro
            &nbsp;&nbsp;>&nbsp;&nbsp;Conferência de Fechamento de Caixa&nbsp;&nbsp;</span>
                <div style="float: right; padding: 0;">
                    <a href="../DefaultFinanceiro.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <asp:Panel ID="panPrincipal" runat="server">
                <div class="accountInfo">
                    <fieldset>
                        <legend>Conferência de Fechamento de Caixa - Filtro:</legend>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="width: 1006px;">
                                    <div style="width: 800px;" class="alinhamento">
                                        <div style="width: 200px;" class="alinhamento">
                                            <label>
                                                Data Início:&nbsp;
                                            </label>
                                            <asp:TextBox ID="TextBoxDataInicio" runat="server" CssClass="textEntry" Height="22px"
                                                Width="198px"></asp:TextBox>
                                            <asp:Calendar ID="CalendarDataInicio" runat="server"
                                                CaptionAlign="Bottom" OnSelectionChanged="CalendarDataInicio_SelectionChanged"></asp:Calendar>
                                        </div>
                                        <div style="width: 200px;" class="alinhamento">
                                            <label>
                                                Data Fim:&nbsp;
                                            </label>
                                            <asp:TextBox ID="TextBoxDataFim" runat="server" CssClass="textEntry" Height="22px"
                                                Width="198px"></asp:TextBox>
                                            <asp:Calendar ID="CalendarDataFim" runat="server"
                                                CaptionAlign="Bottom" OnSelectionChanged="CalendarDataFim_SelectionChanged"></asp:Calendar>
                                        </div>
                                        <div style="width: 230px;" class="alinhamento">
                                            <label>
                                                Filial:&nbsp;
                                            </label>
                                            <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                                Height="26px" Width="215px" OnDataBound="ddlFilial_DataBound">
                                            </asp:DropDownList>
                                            &nbsp;&nbsp;&nbsp;<asp:Label ID="labErroFilial" runat="server" ForeColor="Red" Text=""></asp:Label>
                                        </div>
                                        <div style="width: 250px; top: 151px" class="alinhamento">
                                            <asp:Button ID="btnLimpar" runat="server" Text="Limpar" Width="100px" OnClick="btnLimpar_Click" />&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />
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
                                        <asp:GridView ID="gvFechamentos" runat="server" Width="100.5%" AutoGenerateColumns="False"
                                            ForeColor="#333333" Style="background: white"
                                            ShowFooter="true" DataKeyNames="DATA_FECHAMENTO" OnDataBound="gvFechamentos_DataBound" OnRowCommand="gvFechamentos_RowCommand" OnRowDataBound="gvFechamentos_RowDataBound">
                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                            <Columns>

                                                <asp:TemplateField HeaderText="Filial">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="LiteralFilial" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="data_fechamento" HeaderText="Data do Movimento" />
                                                <asp:BoundField DataField="data" HeaderText="Data do Fechamento" />
                                                <asp:BoundField DataField="valor_dinheiro" HeaderText="Valor em Dinheiro" />
                                                <asp:BoundField DataField="valor_retirada" HeaderText="Valor da Retirada" />
                                                <asp:BoundField DataField="valor_despesas" HeaderText="Valor das Despesas" />
                                                <asp:BoundField DataField="valor_devolucao" HeaderText="Valor da Devolução" />
                                                <asp:TemplateField HeaderText="Saldo Dinheiro">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="LiteralSaldoDinheiro" />
                                                        <asp:HiddenField ID="hdnCodigoFechamentogrd" runat="server"></asp:HiddenField>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button runat="server" ID="btnVer" Text="Ver" Width="60px" CssClass="buttonhbf" CommandArgument='<%# Container.DataItemIndex %>' CommandName="ver_Fechamento" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="70px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button runat="server" ID="btnReabrir" Text="Reabrir" Width="60px" CssClass="buttonhbf" CommandArgument='<%# Container.DataItemIndex %>' CommandName="reabrir_Fechamento" />
                                                        <asp:Literal runat="server" ID="litDepositado" Visible="false" Text="Depositado" />
                                                        <asp:Literal runat="server" ID="litLancado" Visible="false" Text="Lançado" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" BorderWidth="1px" Width="70px"></ItemStyle>
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

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
