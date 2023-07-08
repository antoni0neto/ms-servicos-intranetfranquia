<%@ Page Title="Clientes Compraram Também" Language="C#" AutoEventWireup="true" CodeBehind="ecom_rel_config_rel_criar_random_compra.aspx.cs"
    Inherits="Relatorios.ecom_rel_config_rel_criar_random_compra" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Faturamento</title>
    <style type="text/css">
        .alinhamento {
            position: relative;
            float: left;
        }

        .style1 {
            width: 100%;
        }

        .imgX {
            height: 326px;
            width: 100%;
            overflow: hidden;
            vertical-align: middle;
        }

            .imgX img {
                height: auto;
                width: 225px;
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
                        <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Produtos Relacionados&nbsp;&nbsp;>&nbsp;&nbsp;Clientes Compraram Também</span>
                        <div style="float: right; padding: 0;">
                            <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <fieldset>
                            <legend>Clientes Compraram Também</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Repeater ID="repCompraramTB" runat="server" OnItemDataBound="repCompraramTB_ItemDataBound">
                                            <ItemTemplate>
                                                <table border="0" width="100%" cellpadding="2" cellspacing="2" style="background-color: #5F9EA0;">
                                                    <tr>
                                                        <td style="width: 33%">
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="background-color: white;">
                                                                <tr>
                                                                    <td colspan="2" style="text-align: center; border: solid 1px black; width: 33%; height: 330px;">
                                                                        <div class="imgX">
                                                                            <asp:Image runat="server" ID="imgProduto1" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="border-left: solid 1px black;">
                                                                        <label style="font-weight: bold;">SKU</label>
                                                                    </td>
                                                                    <td style="border-left: solid 1px black;">
                                                                        <label style="font-weight: bold;">Estoque</label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="border-left: solid 1px black; text-align: center;">
                                                                        <asp:Literal runat="server" ID="litSKU1" />
                                                                    </td>
                                                                    <td style="border-left: solid 1px black; text-align: center;">
                                                                        <asp:Literal runat="server" ID="litEstoque1" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="width: 33%">
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="background-color: white;">
                                                                <tr>
                                                                    <td colspan="2" style="text-align: center; border: solid 1px black; width: 33%; height: 330px;">
                                                                        <div class="imgX">
                                                                            <asp:Image runat="server" ID="imgProduto2" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="border-left: solid 1px black;">
                                                                        <label style="font-weight: bold;">SKU</label>
                                                                    </td>
                                                                    <td style="border-left: solid 1px black;">
                                                                        <label style="font-weight: bold;">Estoque</label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="border-left: solid 1px black; text-align: center;">
                                                                        <asp:Literal runat="server" ID="litSKU2" />
                                                                    </td>
                                                                    <td style="border-left: solid 1px black; text-align: center;">
                                                                        <asp:Literal runat="server" ID="litEstoque2" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="width: 33%">
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="background-color: white;">
                                                                <tr>
                                                                    <td colspan="2" style="text-align: center; border: solid 1px black; width: 33%; height: 330px;">
                                                                        <div class="imgX">
                                                                            <asp:Image runat="server" ID="imgProduto3" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="border-left: solid 1px black;">
                                                                        <label style="font-weight: bold;">SKU</label>
                                                                    </td>
                                                                    <td style="border-left: solid 1px black;">
                                                                        <label style="font-weight: bold;">Estoque</label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="border-left: solid 1px black; text-align: center;">
                                                                        <asp:Literal runat="server" ID="litSKU3" />
                                                                    </td>
                                                                    <td style="border-left: solid 1px black; text-align: center;">
                                                                        <asp:Literal runat="server" ID="litEstoque3" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:Repeater>
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
