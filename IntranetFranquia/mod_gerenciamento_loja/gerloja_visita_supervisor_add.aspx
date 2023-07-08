<%@ Page Title="Agenda de Visitas" Language="C#" AutoEventWireup="true" CodeBehind="gerloja_visita_supervisor_add.aspx.cs"
    Inherits="Relatorios.gerloja_visita_supervisor_add" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Agenda de Visitas</title>
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

    <script src="../js/js.js" type="text/javascript"></script>

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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Supervisores&nbsp;&nbsp;>&nbsp;&nbsp;Agenda de Visitas&nbsp;&nbsp;>&nbsp;&nbsp;Filial</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset class="fs">
                            <legend>Agenda de Visitas - Filial</legend>
                            <table border="0" cellspacing="0" cellpadding="0" width="100%" class="tb">
                                <tr style="height: 10px; vertical-align: bottom;">
                                    <td>
                                        <asp:Label ID="labData" runat="server" Text="Data"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labCor" runat="server" Text="Cor da Data"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:HiddenField ID="hidCodigo" runat="server" />
                                    </td>
                                    <td>
                                        <asp:HiddenField ID="hidCodigoUsuario" runat="server" Value="" />
                                        <asp:HiddenField ID="hidAno" runat="server" Value="" />
                                        <asp:HiddenField ID="hidMes" runat="server" Value="" />
                                        <asp:HiddenField ID="hidDia" runat="server" Value="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 150px;">
                                        <asp:TextBox ID="txtData" runat="server" CssClass="" Width="140px" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td style="width: 100px;">
                                        <asp:DropDownList ID="ddlCor" Height="21px" runat="server" Width="94px">
                                            <asp:ListItem Text="Branco" Value="#FFFAFA"></asp:ListItem>
                                            <asp:ListItem Text="Azul" Value="#ADD8E6"></asp:ListItem>
                                            <asp:ListItem Text="Amarelo" Value="#EEE8AA"></asp:ListItem>
                                            <asp:ListItem Text="Verde" Value="#98FB98"></asp:ListItem>
                                            <asp:ListItem Text="Vermelho" Value="#FFA07A"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 260px;">
                                        <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Width="254px" Height="22px">
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
                                <tr>
                                    <td colspan="5">Descritivo</td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtDescritivo" runat="server" Width="100%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="height: 10px;">
                                    <td colspan="5"></td>
                                </tr>
                            </table>
                            <asp:Panel ID="pnlErro" runat="server" CssClass="pnlErro">
                                <asp:Label ID="labErro" runat="server" ForeColor="red" Text="Informe a Filial.">
                                </asp:Label>
                            </asp:Panel>
                            <div style="width: 100%; overflow: auto; height: 235px;">
                                <div>
                                    <table border="0" cellpadding="0" class="tb" width="100%">
                                        <tr>
                                            <td style="width: 100%;">
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvAgenda" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvAgenda_RowDataBound">
                                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Filial">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litFilial" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Data Visita">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litDataVisita" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Descritivo">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litObs" runat="server"></asp:Literal>
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
