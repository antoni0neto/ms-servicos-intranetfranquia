<%@ Page Title="Detalhamento Despesa" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="contabil_det_despesa_ffc.aspx.cs" Inherits="Relatorios.contabil_det_despesa_ffc"
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
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo de Contabilidade&nbsp;&nbsp;>&nbsp;&nbsp;Conciliação&nbsp;&nbsp;>&nbsp;&nbsp;Histórico de Despesas Fundo Fixo</span>
        <div style="float: right; padding: 0;">
            <a href="contabil_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset class="login">
            <legend>Histórico de Despesas Fundo Fixo</legend>
            <table border="0" class="style1">
                <tr>
                    <td>
                        <div style="width: 600px;" class="alinhamento">
                            <div style="width: 200px;" class="alinhamento">
                                <label>
                                    <span style="color: Red;">*</span> Data Início:&nbsp;
                                </label>
                                <asp:TextBox ID="TextBoxDataInicio" runat="server" CssClass="textEntry" Height="22px"
                                    Width="198px"></asp:TextBox>
                                <asp:Calendar ID="CalendarDataInicio" runat="server" OnSelectionChanged="CalendarDataInicio_SelectionChanged"
                                    CaptionAlign="Bottom"></asp:Calendar>
                            </div>
                            <div style="width: 200px;" class="alinhamento">
                                <label>
                                    <span style="color: Red;">*</span> Data Fim:&nbsp;
                                </label>
                                <asp:TextBox ID="TextBoxDataFim" runat="server" CssClass="textEntry" Height="22px"
                                    Width="198px"></asp:TextBox>
                                <asp:Calendar ID="CalendarDataFim" runat="server" OnSelectionChanged="CalendarDataFim_SelectionChanged"
                                    CaptionAlign="Bottom"></asp:Calendar>
                            </div>
                            <div style="width: 200px;" class="alinhamento">
                                <label>
                                    <span style="color: Red;">*</span> Filial:&nbsp;
                                </label>
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="26px" Width="200px" OnDataBound="ddlFilial_DataBound">
                                </asp:DropDownList>
                            </div>
                            <div style="width: 200px;" class="alinhamento">
                                <label>
                                    Despesa:&nbsp;
                                </label>
                                <asp:DropDownList runat="server" ID="ddlDespesa" DataValueField="CODIGO" DataTextField="DESCRICAO"
                                    Height="22px" Width="230px" OnDataBound="ddlDespesa_DataBound">
                                </asp:DropDownList>
                            </div>
                            <p style="height: 13px">
                                &nbsp;</p>
                            <div>
                                <asp:Button runat="server" ID="btBuscarDespesas" Text="Buscar Despesas" OnClick="btBuscarDespesas_Click" />
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                            <br />
                            <asp:GridView ID="GridViewDespesas" runat="server" Width="100%" CssClass="DataGrid_Padrao"
                                AutoGenerateColumns="False" ShowFooter="true" OnDataBound="GridViewDespesas_DataBound">
                                <FooterStyle HorizontalAlign="Center"></FooterStyle>
                                <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
                                <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
                                <RowStyle HorizontalAlign="Center"></RowStyle>
                                <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                                <Columns>
                                    <asp:BoundField DataField="CODIGO" HeaderText="Conta Despesa" />
                                    <asp:BoundField DataField="DESCRICAO" HeaderText="Descrição" />
                                    <asp:BoundField DataField="DATA" HeaderText="Data" />
                                    <asp:BoundField DataField="VALOR" HeaderText="Valor" />
                                </Columns>
                                <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                            </asp:GridView>
                        </div>
                        <div style="text-align: center;">
                            <asp:Label ID="labErro" runat="server" Text="" ForeColor="red" Visible="false"></asp:Label></label>
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
