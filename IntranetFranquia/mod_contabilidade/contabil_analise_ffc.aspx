<%@ Page Title="Análise Fundo Fixo de Loja" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="contabil_analise_ffc.aspx.cs" Inherits="Relatorios.contabil_analise_ffc" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento {
            position: relative;
            float: left;
        }

        .alinharcentro {
            text-align: center;
        }

        .style1 {
            width: 100%;
        }
    </style>
    <script src="../js/js.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function AbrirArquivo() {

            var Left = 400;
            var Top = 120;

            var nFec = this.document.getElementById("MainContent_hidNumeroFechamento").value;

            window.open("contabil_analise_ffc_imp.aspx?nfecx=" + nFec + "", "_blank", "status=no, resizable=no, help=no, menubar=no, scrollbars=no, titlebar=no, toolbar=no, width=800, height=600, top=" + Top + ", left=" + Left);

        }
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:updatepanel id="up1" runat="server" updatemode="Always" enableviewstate="true">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">
                    <asp:Label ID="labTitulo" runat="server" Text="Administração de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Fechamento&nbsp;&nbsp;>&nbsp;&nbsp;Análise Fundo Fixo"></asp:Label>
                </span>
                <div style="float: right; padding: 0;">
                    <a href="~/mod_adm_loja/admloj_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="accountInfo">
                <fieldset class="login">
                    <legend>Análise Fundo Fixo</legend>
                    <div style="width: 600px;" class="alinhamento">
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
                        <div style="width: 200px;" class="alinhamento">
                            <label>
                                Filial:&nbsp;
                            </label>
                            <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                Height="26px" Width="200px" OnDataBound="ddlFilial_DataBound">
                            </asp:DropDownList>
                            <br />
                            <br />
                        </div>
                        <div style="width: 200px;" class="alinhamento">
                            <label>
                                Baixa:&nbsp;
                            </label>
                            <asp:DropDownList runat="server" ID="ddlBaixa" DataValueField="CODIGO_BAIXA" DataTextField="DESCRICAO"
                                Height="26px" Width="200px" OnDataBound="ddlBaixa_DataBound">
                            </asp:DropDownList>
                        </div>
                    </div>
                </fieldset>
                <div>
                    <asp:Button runat="server" ID="btDocumentos" Text="Buscar Documentos" OnClick="btDocumentos_Click" />
                    <input id="hidNumeroFechamento" runat="server" value="" name="hidNumeroFechamento"
                        type="hidden" />
                </div>
                <fieldset class="login">
                    <legend>Histórico de Fechamento</legend>
                    <asp:GridView ID="GridViewSaldo" runat="server" Width="100%" CssClass="DataGrid_Padrao"
                        AutoGenerateColumns="False" OnRowDataBound="GridViewSaldo_RowDataBound">
                        <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
                        <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
                        <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
                        <RowStyle HorizontalAlign="Center"></RowStyle>
                        <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                        <Columns>
                            <asp:BoundField DataField="CODIGO" HeaderText="Número" />
                            <asp:BoundField DataField="DATA" HeaderText="Data" />
                            <asp:BoundField DataField="SALDO_ANTERIOR" HeaderText="Saldo Anterior" />
                            <asp:BoundField DataField="TOTAL_CREDITO" HeaderText="Total de Créditos" />
                            <asp:BoundField DataField="TOTAL_DEBITO" HeaderText="Total de Débitos" />
                            <asp:BoundField DataField="SALDO_ATUAL" HeaderText="Saldo Atual" />
                            <asp:TemplateField HeaderText="Baixa">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="LiteralStatus" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="btAnalisarFF" Text="Ver" OnClick="btAnalisarFF_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </fieldset>
                <fieldset id="fsFechamento" class="login" runat="server">
                    <legend>Nº
                        <asp:Label ID="labNumeroFechamento" runat="server" Text="123" ForeColor=""></asp:Label>
                    </legend>
                    <div style="float: right; margin-top: -15px; clear: both;">
                        <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>&nbsp;
                        <asp:Button runat="server" ID="btTextFile" Text="Imprimir" OnClientClick="AbrirArquivo();" />
                    </div>
                    <div style="">
                        <fieldset class="login">
                            <legend>Despesas</legend>
                            <asp:GridView ID="GridViewDebitos" runat="server" Width="100%" CssClass="DataGrid_Padrao"
                                ShowFooter="true" AutoGenerateColumns="False" OnRowDataBound="GridViewDebitos_RowDataBound"
                                OnDataBound="GridViewDebitos_DataBound"
                                DataKeyNames="CODIGO">
                                <FooterStyle CssClass="DataGrid_Footer" HorizontalAlign="Center"></FooterStyle>
                                <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
                                <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
                                <RowStyle HorizontalAlign="Center"></RowStyle>
                                <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                                <Columns>
                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="75px">
                                        <ItemTemplate>
                                            <asp:Button ID="btExcluir" runat="server" Text="Excluir" Width="75px" OnClick="btExcluir_Click" OnClientClick="javascript: return confirm('Tem certeza que deseja excluir esta despesa?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Despesa" HeaderStyle-Width="562px">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="LiteralDespesa" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DATA" HeaderText="Data" HeaderStyle-Width="400px" />

                                    <asp:TemplateField HeaderText="Valor" HeaderStyle-Width="200px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtValor" runat="server" Width="200px" Text="" CssClass="alinharcentro" onkeypress="return fnValidarNumeroDecimal(event);" OnTextChanged="txtValor_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="NUMERO_LANCAMENTO" HeaderText="Lançamento" />
                                </Columns>
                                <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                            </asp:GridView>
                        </fieldset>
                        <fieldset class="login">
                            <legend>Receitas</legend>
                            <asp:GridView ID="GridViewCreditos" runat="server" Width="100%" CssClass="DataGrid_Padrao"
                                ShowFooter="true" AutoGenerateColumns="False" OnRowDataBound="GridViewCreditos_RowDataBound"
                                OnDataBound="GridViewCreditos_DataBound">
                                <FooterStyle CssClass="DataGrid_Footer" HorizontalAlign="Center"></FooterStyle>
                                <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
                                <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
                                <RowStyle HorizontalAlign="Center"></RowStyle>
                                <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                                <Columns>
                                    <asp:TemplateField HeaderText="Receita" HeaderStyle-Width="562px">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="LiteralReceita" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DATA" HeaderText="Data" HeaderStyle-Width="458px" />
                                    <asp:BoundField DataField="VALOR" HeaderText="Valor" HeaderStyle-Width="159px" />
                                    <asp:BoundField DataField="NUMERO_LANCAMENTO" HeaderText="Lançamento" />
                                </Columns>
                                <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                            </asp:GridView>
                        </fieldset>
                    </div>
                    <asp:Button runat="server" ID="btConferir" Text="Conferir Fechamento" OnClick="btConferir_Click"
                        OnClientClick="javascript: return confirm('Tem certeza que deseja conferir este fechamento?');" />&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                            runat="server" ID="lblErroGravar" ForeColor="Red" Text=""></asp:Label>
                    <br />
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:updatepanel>
</asp:Content>
