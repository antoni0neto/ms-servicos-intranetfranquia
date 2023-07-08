<%@ Page Title="Orçamento de Aviamentos" Language="C#" AutoEventWireup="true" CodeBehind="prod_cad_orcam_aviamento.aspx.cs"
    Inherits="Relatorios.prod_cad_orcam_aviamento" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Orçamento de Aviamentos</title>
    <style type="text/css">
        .fs
        {
            border: 1px solid #ccc;
            font-family: Calibri;
        }
        .tb
        {
            padding: 0;
            font-family: Calibri;
            font-size: 14px;
        }
        .alinharDireita
        {
            text-align: right;
        }
    </style>
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../js/js.js" type="text/javascript"></script>
</head>
<body>
    <form id="thisForm" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div style="margin-top: 10px; padding-left: 12px; padding-right: 12px; padding-bottom: -350px;
                background-color: White;">
                <br />
                <div>
                    <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Aviamentos&nbsp;&nbsp;>&nbsp;&nbsp;Orçamento
                        de Aviamentos</span>
                    <div style="float: right; padding: 0;">
                        <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                    </div>
                </div>
                <hr />
                <div>
                    <fieldset class="fs">
                        <legend>Orçamento de Aviamentos</legend>
                        <div id="divQtdeSelecionada" runat="server">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvAviamentoSelecionado" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvAviamentoSelecionado_RowDataBound"
                                                OnDataBound="gvAviamentoSelecionado_DataBound" DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                        HeaderText="HB" HeaderStyle-Width="100px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litHB" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Aviamento" HeaderStyle-Width="200px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litAviamento" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="DESCRICAO" HeaderText="Descrição" HeaderStyle-Width="600px"
                                                        HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="QTDE" HeaderText="Quantidade Prevista" ItemStyle-HorizontalAlign="Center" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <div id="divOrcamento" runat="server">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        Fornecedor
                                    </td>
                                    <td>
                                        Qtde por Pacote
                                    </td>
                                    <td>
                                        Qtde de Pacote
                                    </td>
                                    <td>
                                        Preço do Pacote
                                    </td>
                                    <td>
                                        Desconto (%)
                                    </td>
                                    <td>
                                        Observação
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 210px;">
                                        <asp:DropDownList ID="ddlFornecedor" runat="server" Width="204px" Height="21px" DataTextField="FORNECEDOR"
                                            DataValueField="FORNECEDOR">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 120px;">
                                        <asp:TextBox ID="txtQtdePorPacote" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                            Width="110px" OnTextChanged="txtQtdePorPacote_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    </td>
                                    <td style="width: 120px;">
                                        <asp:TextBox ID="txtQtdePacote" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                            Width="110px"></asp:TextBox>
                                    </td>
                                    <td style="width: 120px;">
                                        <asp:TextBox ID="txtPreco" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                            Width="110px"></asp:TextBox>
                                    </td>
                                    <td style="width: 90px;">
                                        <asp:TextBox ID="txtDesconto" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                            Width="80px"></asp:TextBox>
                                    </td>
                                    <td style="width: 363px;">
                                        <asp:TextBox ID="txtObs" runat="server" Width="353px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Button ID="btSalvarOrcamento" runat="server" Text="Salvar Orçamento" OnClick="btSalvarOrcamento_Click"
                                            Width="125px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <asp:Label ID="labErro" runat="server" ForeColor="Red" Text="" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                </div>
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
