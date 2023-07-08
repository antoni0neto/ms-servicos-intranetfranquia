<%@ Page Title="Cadastro de Molde" Language="C#" AutoEventWireup="true" CodeBehind="prod_cad_molde.aspx.cs"
    Inherits="Relatorios.prod_cad_molde" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cadastro de Molde</title>
    <style type="text/css">
        .fs {
            border: 1px solid #ccc;
            font-family: Calibri;
        }

        .tb {
            padding: 0;
            font-family: Calibri;
            font-size: 14px;
        }

        .pnlErro {
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
</head>
<body>
    <form id="thisForm" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
                <div style="margin-top: 10px; padding-left: 12px; padding-right: 12px; padding-bottom: -350px; background-color: White;">
                    <br />
                    <div>
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Ordem
                    de Corte&nbsp;&nbsp;>&nbsp;&nbsp;HB Novo&nbsp;&nbsp;>&nbsp;&nbsp;Cadastro de Molde</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset class="fs">
                            <legend>Cadastro de Molde</legend>
                            <table border="0" cellspacing="0" cellpadding="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="labMolde" runat="server" Text="Molde"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labModelagem" runat="server" Text="Modelagem"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labProduto" runat="server" Text="Produto"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labNome" runat="server" Text="Nome"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:HiddenField ID="hidCodigo" runat="server" />
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100px;">
                                        <asp:TextBox ID="txtMolde" runat="server" CssClass="textEntry" Width="90px"></asp:TextBox>
                                    </td>
                                    <td style="width: 200px;">
                                        <asp:TextBox ID="txtModelagem" runat="server" CssClass="textEntry" Width="190px"></asp:TextBox>
                                    </td>
                                    <td style="width: 150px;">
                                        <asp:TextBox ID="txtProduto" runat="server" CssClass="textEntry" Width="140px" OnTextChanged="txtProduto_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    </td>
                                    <td style="width: 200px;">
                                        <asp:TextBox ID="txtNome" runat="server" CssClass="textEntry" Width="190px" Enabled="false"></asp:TextBox>
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
                                    <td colspan="6"></td>
                                </tr>
                            </table>
                            <asp:Panel ID="pnlErro" runat="server" CssClass="pnlErro">
                                <asp:Label ID="labErro" runat="server" ForeColor="red" Text="">
                                </asp:Label>
                            </asp:Panel>
                            <br />
                            <div style="width: 100%; overflow: auto; height: 450px;">
                                <div>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td style="width: 33px;">&nbsp;
                                            </td>
                                            <td style="width: 100px;">
                                                <asp:DropDownList ID="ddlMoldeFiltro" runat="server" Width="125px" DataTextField="MOLDE" DataValueField="MOLDE" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </td>
                                            <td style="width: 203px;">
                                                <asp:DropDownList ID="ddlModelagemFiltro" runat="server" Width="203px" DataTextField="MODELAGEM" DataValueField="MODELAGEM" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </td>
                                            <td style="width: 203px;">
                                                <asp:DropDownList ID="ddlProdutoFiltro" runat="server" Width="203px" DataTextField="PRODUTO" DataValueField="PRODUTO" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </td>
                                            <td style="width: 200px;">
                                                <asp:DropDownList ID="ddlNomeProdutoFiltro" runat="server" Width="200px" DataTextField="DESC_PRODUTO" DataValueField="DESC_PRODUTO" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </td>
                                            <td style="width: 20px;">&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td colspan="7" style="width: 100%;">
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvMolde" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvMolde_RowDataBound">
                                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="MOLDE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Molde" HeaderStyle-Width="120px" />
                                                            <asp:BoundField DataField="MODELAGEM" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Modelagem" HeaderStyle-Width="200px" />
                                                            <asp:BoundField DataField="PRODUTO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Produto" HeaderStyle-Width="200px" />
                                                            <asp:BoundField DataField="DESC_PRODUTO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Nome" />
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="55px">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btAlterar" runat="server" Height="19px" Text="Alterar" OnClick="btAlterar_Click" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="55px">
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
