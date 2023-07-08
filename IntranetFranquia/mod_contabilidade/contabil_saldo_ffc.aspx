<%@ Page Title="Saldo Fundo Fixo de Loja" Language="C#" MasterPageFile="~/Site.master"
    EnableEventValidation="false" AutoEventWireup="true" CodeBehind="contabil_saldo_ffc.aspx.cs"
    Inherits="Relatorios.contabil_saldo_ffc" %>

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
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Always" EnableViewState="true">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Contabilidade&nbsp;&nbsp;>&nbsp;&nbsp;Conciliação&nbsp;&nbsp;>&nbsp;&nbsp;Saldo Fundo Fixo</span>
                <div style="float: right; padding: 0;">
                    <a href="contabil_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="accountInfo">
                <fieldset class="login">
                    <legend>Saldo Fundo Fixo</legend>
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
                    </div>
                </fieldset>
                <div>
                    <asp:Button runat="server" ID="btSaldoFundoFixo" Text="Buscar Saldo" OnClick="btSaldoFundoFixo_Click" />&nbsp;&nbsp;
                    <asp:Button runat="server" ID="btExcelSaldo" Text="Excel" OnClick="btExcelSaldo_Click" ToolTip="Extrair as informações para Excel." />
                </div>
                <fieldset class="login">
                    <legend>Histórico de Saldo de Fundo Fixo</legend>
                    <asp:GridView ID="GridViewSaldoFundoFixo" runat="server" Width="100%" CssClass="DataGrid_Padrao"
                        PageSize="30" AllowPaging="True" AutoGenerateColumns="False" OnRowDataBound="GridViewSaldoFundoFixo_RowDataBound"
                        OnPageIndexChanging="GridViewSaldoFundoFixo_PageIndexChanging">
                        <FooterStyle HorizontalAlign="Center"></FooterStyle>
                        <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
                        <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
                        <RowStyle HorizontalAlign="Center"></RowStyle>
                        <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                        <Columns>
                            <asp:BoundField DataField="COD_FILIAL" HeaderText="Código Filial" Visible="false" />
                            <asp:BoundField DataField="FILIAL" HeaderText="Filial" Visible="false" />
                            <asp:BoundField DataField="CONTA" HeaderText="Conta" />
                            <asp:BoundField DataField="DESCRICAO" HeaderText="Descrição" />
                            <asp:BoundField DataField="DATA" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="SAIDAS" HeaderText="Saidas" />
                            <asp:BoundField DataField="ENTRADAS" HeaderText="Entradas" />
                            <asp:BoundField DataField="SALDO" HeaderText="Saldo" />
                        </Columns>
                        <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                    </asp:GridView>
                </fieldset>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btExcelSaldo" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
