<%@ Page Title="Depósito em Banco" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ListaDepositoStatus.aspx.cs" Inherits="Relatorios.ListaDepositoStatus"
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
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Lista
                    Depósitos Status</span>
                <div style="float: right; padding: 0;">
                    <a href="DefaultFinanceiro.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="accountInfo">
                <fieldset class="login">
                    <legend>Lista Depósitos Status</legend>
                    <table border="0" class="style1">
                        <tr>
                            <td>
                                <div style="width: 600px;" class="alinhamento">
                                    <div style="width: 200px;" class="alinhamento">
                                        <label>
                                            <span style="color: Red;"></span>Data Início:&nbsp;
                                        </label>
                                        <asp:TextBox ID="TextBoxDataInicio" runat="server" CssClass="textEntry" Height="22px"
                                            Width="198px"></asp:TextBox>
                                        <asp:Calendar ID="CalendarDataInicio" runat="server" OnSelectionChanged="CalendarDataInicio_SelectionChanged"
                                            CaptionAlign="Bottom"></asp:Calendar>
                                    </div>
                                    <div style="width: 200px;" class="alinhamento">
                                        <label>
                                            <span style="color: Red;"></span>Data Fim:&nbsp;
                                        </label>
                                        <asp:TextBox ID="TextBoxDataFim" runat="server" CssClass="textEntry" Height="22px"
                                            Width="198px"></asp:TextBox>
                                        <asp:Calendar ID="CalendarDataFim" runat="server" OnSelectionChanged="CalendarDataFim_SelectionChanged"
                                            CaptionAlign="Bottom"></asp:Calendar>
                                    </div>
                                    <div style="width: 200px;" class="alinhamento">
                                        <label>
                                            <span style="color: Red;"></span>Filial:&nbsp;
                                        </label>
                                        <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                            Height="26px" Width="200px" OnDataBound="ddlFilial_DataBound">
                                        </asp:DropDownList>
                                        <br />
                                        <br />
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
                                            <br />
                                        </div>
                                    </div>
                                    <p style="height: 13px">
                                        &nbsp;</p>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <asp:Button runat="server" ID="btBuscarDepositos" Text="Buscar Depósitos" OnClick="btBuscarDepositos_Click" />&nbsp;&nbsp;&nbsp;<asp:Label
                                        ID="labErro" runat="server" Text="" ForeColor="red" Visible="false"></asp:Label></label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <asp:GridView ID="GridViewDepositos" runat="server" Width="100%" CssClass="DataGrid_Padrao"
                                        AllowPaging="True" PageSize="25" AutoGenerateColumns="False" ShowFooter="true"
                                        OnRowDataBound="GridViewDepositos_RowDataBound" OnPageIndexChanging="GridViewDepositos_PageIndexChanging">
                                        <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
                                        <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
                                        <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
                                        <RowStyle HorizontalAlign="Center"></RowStyle>
                                        <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Data Início">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDataInicio" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data Fim">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDataFim" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valor a Depositar">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValorDepositar" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valor Depositado">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValorDepositado" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="VALOR" HeaderText="Valor Lançamento" />
                                            <asp:BoundField DataField="NUMERO_LANCAMENTO" HeaderText="No. Lançamento" />
                                        </Columns>
                                        <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
