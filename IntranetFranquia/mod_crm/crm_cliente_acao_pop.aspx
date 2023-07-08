<%@ Page Title="Clientes - Ação" Language="C#" AutoEventWireup="true" CodeBehind="crm_cliente_acao_pop.aspx.cs"
    Inherits="Relatorios.crm_cliente_acao_pop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Expedição</title>
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }
    </style>

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../js/js.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
                <div style="margin-top: 10px; padding-left: 12px; padding-right: 12px; padding-bottom: -350px; background-color: White;">
                    <br />
                    <div>
                        <span style="font-family: Calibri; font-size: 14px;">>Módulo de CRM&nbsp;&nbsp;>&nbsp;&nbsp;Clientes&nbsp;&nbsp;>&nbsp;&nbsp;Clientes - Ação</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Clientes - Ação</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>CPF
                                        <asp:HiddenField ID="hidCodigoAcao" runat="server" Value="" />
                                    </td>
                                    <td>Cliente
                                    </td>
                                    <td>Filial
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 280px;">
                                        <asp:TextBox ID="txtCPF" runat="server" Enabled="false" Width="270px"></asp:TextBox>
                                    </td>
                                    <td style="width: 510px;">
                                        <asp:TextBox ID="txtNome" runat="server" Width="500px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Telefone
                                    </td>
                                    <td>Celular
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtTelefone" runat="server" Width="270px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCelular" runat="server" Width="270px"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">Endereço
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtEndereco" runat="server" Width="100%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">Observação
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtObs" runat="server" Width="100%" TextMode="MultiLine" Height="100px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">Status</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="140px">
                                            <asp:ListItem Value="S" Text="Aberto" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="N" Text="Concluído"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2" style="text-align: right;">
                                        <asp:Label ID="labErro" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                                        &nbsp;<asp:Button ID="btSalvar" runat="server" Text="Salvar" Width="120px" OnClick="btSalvar_Click" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <br />
                </div>
                <div id="dialogPai" runat="server">
                    <div id="dialog" title="Mensagem" class="divPop">
                        <table border="0" width="100%">
                            <tr>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; color: red;">
                                    <strong>Aviso</strong>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; color: red;">BAIXADO COM SUCESSO
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
