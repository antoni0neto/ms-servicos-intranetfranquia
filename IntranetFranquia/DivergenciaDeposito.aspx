<%@ Page Title="Depósito em Banco" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="DivergenciaDeposito.aspx.cs" Inherits="Relatorios.DivergenciaDeposito"
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
    <div class="accountInfo">
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
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
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td>
                        <div>
                            <asp:Button runat="server" ID="btBuscarDepositos" Text="Buscar Depósitos" OnClick="btBuscarDepositos_Click" />&nbsp;&nbsp;&nbsp;<asp:Label ID="labErro" runat="server" Text="" ForeColor="red" Visible="false"></asp:Label></label>
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
                                    <asp:TemplateField HeaderText="Filial">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="LiteralFilial" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="data_inicio" HeaderText="Data Início" />
                                    <asp:BoundField DataField="data_fim" HeaderText="Data Fim" />
                                    <asp:BoundField DataField="valor_a_depositar" HeaderText="Valor a Depositar" />
                                    <asp:BoundField DataField="valor_depositado" HeaderText="Valor Depositado" />
                                    <asp:BoundField DataField="diferenca" HeaderText="Diferença" />
                                    <asp:BoundField DataField="assinatura" HeaderText="Assinatura" />
                                    <asp:TemplateField HeaderText="Baixa">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="LiteralBaixa" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
