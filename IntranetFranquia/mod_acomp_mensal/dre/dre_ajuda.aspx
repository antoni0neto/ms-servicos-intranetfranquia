<%@ Page Title="Ajuda DRE" Language="C#" AutoEventWireup="true" CodeBehind="dre_ajuda.aspx.cs"
    Inherits="Relatorios.dre_ajuda" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ajuda DRE</title>
    <style type="text/css">
    </style>
    <link href="../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../../js/js.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo Acompanhamento Mensal&nbsp;&nbsp;>&nbsp;&nbsp;DRE&nbsp;&nbsp;>&nbsp;&nbsp;Ajuda DRE&nbsp;&nbsp;</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Ajuda DRE</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>Grupo
                                        <asp:HiddenField ID="hidID" runat="server" Value="" />
                                        <asp:HiddenField ID="hidTipo" runat="server" Value="" />
                                    </td>
                                    <td>Linha
                                    </td>
                                    <td>Linha Extra
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 410px;">
                                        <asp:TextBox ID="txtGrupo" runat="server" Width="400px"></asp:TextBox>
                                    </td>
                                    <td style="width: 360px;">
                                        <asp:TextBox ID="txtLinha" runat="server" Width="350px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinhaExtra" runat="server" Width="250px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">Descrição
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtDescricao" runat="server" TextMode="MultiLine" Width="100%" Height="150px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">Query
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtQuery" runat="server" TextMode="MultiLine" Width="100%" Height="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Button ID="btSalvar" runat="server" Width="100px" Text="Salvar" OnClick="btSalvar_Click" OnClientClick="return Confirmar('Tem certeza?');" />
                                        <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
