<%@ Page Title="Conferência de Forma de Pagamento" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="fin_formapgto_conferencia.aspx.cs" Inherits="Relatorios.fin_formapgto_conferencia"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita
        {
            text-align: right;
        }
        .alinhamento
        {
            position: relative;
            float: left;
        }
    </style>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Conferência
                    de Forma de Pagamento</span>
                <div style="float: right; padding: 0;">
                    <a href="~/DefaultFinanceiro.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">
                        Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Conferência
                    de Forma de Pagamento"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <div style="width: 600px;" class="alinhamento">
                                <div style="width: 200px;" class="alinhamento">
                                    <label>
                                        <span style="color: Red;"></span>Data Início Venda:&nbsp;
                                    </label>
                                    <asp:TextBox ID="txtDataInicio" runat="server" onkeypress="return fnValidarData(event);"
                                        Height="22px" MaxLength="10" Width="196px"></asp:TextBox>
                                    <asp:Calendar ID="CalendarDataInicio" runat="server" OnSelectionChanged="CalendarDataInicio_SelectionChanged"
                                        CaptionAlign="Bottom"></asp:Calendar>
                                </div>
                                <div style="width: 200px;" class="alinhamento">
                                    <label>
                                        <span style="color: Red;"></span>Data Fim Venda:&nbsp;
                                    </label>
                                    <asp:TextBox ID="txtDataFim" runat="server" onkeypress="return fnValidarData(event);"
                                        Height="22px" MaxLength="10" Width="196px"></asp:TextBox>
                                    <asp:Calendar ID="CalendarDataFim" runat="server" OnSelectionChanged="CalendarDataFim_SelectionChanged"
                                        CaptionAlign="Bottom"></asp:Calendar>
                                </div>
                                <div style="width: 200px;" class="alinhamento">
                                    <label>
                                        <span style="color: Red;"></span>Filial:&nbsp;
                                    </label>
                                    <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                        Height="26px" Width="200px">
                                    </asp:DropDownList>
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
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="rounded_corners">
                                <asp:GridView ID="gvFormaPagamento" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvFormaPagamento_RowDataBound"
                                    ShowFooter="true" DataKeyNames="CODIGO">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="NUMERO_PEDIDO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Número Pedido" HeaderStyle-Width="120px" />
                                        <asp:BoundField DataField="FORNECEDOR" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderText="Fornecedor" HeaderStyle-Width="200px" />
                                        <asp:BoundField DataField="COR" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="170px" HeaderText="Cor" />
                                        <asp:BoundField DataField="DATA_PEDIDO" DataFormatString="{0:d}" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" HeaderText="Data do Pedido" HeaderStyle-Width="150px" />
                                        <asp:BoundField DataField="DATA_ENTREGA_PREV" DataFormatString="{0:d}" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" HeaderText="Previsão de Entrega" HeaderStyle-Width="150px" />
                                        <asp:TemplateField HeaderText="Qtde de Produto" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litQtdeProduto" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DATA_ENTREGA_REAL" DataFormatString="{0:d}" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" HeaderText="Data Entrega" HeaderStyle-Width="150px" />
                                        <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
