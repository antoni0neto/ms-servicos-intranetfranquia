<%@ Page Title="Extrato Caixa Loja" Language="C#" MasterPageFile="~/Site.master"
    EnableEventValidation="false" AutoEventWireup="true" CodeBehind="contabil_extrato_caixa.aspx.cs"
    Inherits="Relatorios.contabil_extrato_caixa" %>

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Contabilidade&nbsp;&nbsp;>&nbsp;&nbsp;Conciliação&nbsp;&nbsp;>&nbsp;&nbsp;Extrato
                    Caixa Loja</span>
                <div style="float: right; padding: 0;">
                    <a href="contabil_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="accountInfo">
                <fieldset class="login">
                    <legend>Extrato Caixa Loja</legend>
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
                    <asp:Button runat="server" ID="btExtratoLoja" Text="Buscar Extrato" OnClick="btExtratoLoja_Click" />&nbsp;&nbsp;<asp:Button
                        runat="server" ID="btExcelExtrato" Text="Excel" OnClick="btExcelExtrato_Click"
                        ToolTip="Extrair as informações para Excel." />
                </div>
                <fieldset class="login">
                    <legend>Extrato</legend>
                    <div class="rounded_corners">
                        <asp:GridView ID="gvExtrato" runat="server" Width="100%" CssClass="DataGrid_Padrao"
                            AutoGenerateColumns="False" OnRowDataBound="gvExtrato_RowDataBound">
                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                            <RowStyle HorizontalAlign="Center" />
                            <Columns>
                                <asp:BoundField DataField="COD_FILIAL" HeaderText="Código Filial" Visible="false" />
                                <asp:BoundField DataField="FILIAL" HeaderText="Filial" Visible="false" />
                                <asp:BoundField DataField="DESCRICAO" HeaderText="Descrição" />
                                <asp:BoundField DataField="DATA" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="SAIDAS" HeaderText="Saídas" />
                                <asp:BoundField DataField="ENTRADAS" HeaderText="Entradas" />
                                <asp:BoundField DataField="SALDO" HeaderText="Saldo" />
                            </Columns>
                            <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                        </asp:GridView>
                    </div>
                </fieldset>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btExcelExtrato" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
