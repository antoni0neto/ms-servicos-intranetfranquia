<%@ Page Title="Cadastro de Aviamento" Language="C#" AutoEventWireup="true" CodeBehind="prod_cad_aviamento.aspx.cs"
    Inherits="Relatorios.prod_cad_aviamento" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cadastro de Aviamento</title>
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
        .pnlErro
        {
            /*border-bottom-style:dashed dotted*/
            border: 1px solid #FA8072;
            background-color: #FFF8DC;
            text-align: center;
            padding-top: 9px;
            width: 100%;
            height: 30px;
            overflow: auto;
        }
    </style>
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">
        function CarregarAviamentoParent() {
            window.opener.CarregarAviamentos();
        }
    </script>
</head>
<body onunload="CarregarAviamentoParent();">
    <form id="thisForm" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div style="margin-top: 10px; padding-left: 12px; padding-right: 12px; padding-bottom: -350px;
                background-color: White;">
                <br />
                <div>
                    <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Ordem
                        de Corte&nbsp;&nbsp;>&nbsp;&nbsp;HB Novo&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Aviamento</span>
                    <div style="float: right; padding: 0;">
                        <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                    </div>
                </div>
                <hr />
                <div>
                    <fieldset class="fs">
                        <legend>Cadastro de Aviamento</legend>
                        <table border="0" cellspacing="0" cellpadding="0" width="100%" class="tb">
                            <tr style="height: 10px; vertical-align: bottom;">
                                <td style="width: 201px;">
                                    <asp:Label ID="labDescricao" runat="server" Text="Descrição"></asp:Label>
                                </td>
                                <td style="width: 180px;">
                                    <asp:Label ID="labUnidadeMedida" runat="server" Text="Unidade de Medida"></asp:Label>
                                </td>
                                <td style="width: 100px;">
                                    <asp:Label ID="labStatus" runat="server" Text="Status"></asp:Label>
                                </td>
                                <td>
                                    <asp:HiddenField ID="hidCodigo" runat="server" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 201px;">
                                    <asp:TextBox ID="txtDescricao" runat="server" CssClass="textEntry" Width="190px"></asp:TextBox>
                                </td>
                                <td style="width: 180px;">
                                    <asp:DropDownList ID="ddlUnidade" Height="21px" runat="server" DataValueField="CODIGO"
                                        DataTextField="DESCRICAO" Width="173px">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 100px;">
                                    <asp:DropDownList ID="ddlStatus" Height="21px" runat="server" Width="90px">
                                        <asp:ListItem Text="Selecione" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Ativo" Selected="True" Value="A"></asp:ListItem>
                                        <asp:ListItem Text="Inativo" Value="I"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 90px;">
                                    <asp:Button runat="server" ID="btIncluir" Text="Incluir" OnClick="btIncluir_Click"
                                        Width="100px" Enabled="true" />
                                </td>
                                <td>
                                    <div style="float: right; margin-right: 0px;">
                                        <asp:Button runat="server" ID="btCancelar" Text="Cancelar" OnClick="btCancelar_Click"
                                            Enabled="true" />
                                    </div>
                                </td>
                            </tr>
                            <tr style="height: 10px;">
                                <td colspan="5">
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="pnlErro" runat="server" CssClass="pnlErro">
                            <asp:Label ID="labErro" runat="server" ForeColor="red" Text="Informe a Descricao do produto.">
                            </asp:Label>
                        </asp:Panel>
                        <div style="width: 100%; overflow: auto; height: 250px;">
                            <div>
                                <table border="0" cellpadding="0" class="tb" width="100%">
                                    <tr>
                                        <td style="width: 100%;">
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvAviamento" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvAviamento_RowDataBound">
                                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="DESCRICAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderText="Descrição" HeaderStyle-Width="200px" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderText="Unidade de Medida" HeaderStyle-Width="200px">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litUnidadeMedida" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            HeaderText="Status">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="65px">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btAlterar" runat="server" Height="19px" Text="Alterar" OnClick="btAlterar_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="65px">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btExcluir" runat="server" Height="19px" Text="Excluir" OnClientClick="return ConfirmarExclusao();"
                                                                    OnClick="btExcluir_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
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
