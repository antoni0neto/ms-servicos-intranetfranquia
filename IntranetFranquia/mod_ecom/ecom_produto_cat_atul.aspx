<%@ Page Title="Atualizar Categoria dos Produtos" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master"
    CodeBehind="ecom_produto_cat_atul.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="Relatorios.ecom_produto_cat_atul" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" type="text/css" />
    <style type="text/css">
        /*.ui-draggable, .ui-droppable {
            background-position: top;
        }*/

        #sortable {
            list-style-type: none;
            margin: 0;
            padding: 0;
            width: 642px;
        }

            #sortable li {
                margin: 10px 10px 10px 0px;
                padding: 1px;
                float: left;
                width: 200px;
                height: 353px;
                text-align: center;
            }
    </style>

    <script src="https://code.jquery.com/jquery-1.12.4.js" type="text/javascript"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>

    <script type="text/javascript">

    </script>

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Cadastros&nbsp;&nbsp;>&nbsp;&nbsp;Atualizar Categoria dos Produtos</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <br />
            <div style="float: left; width: 600px; margin-left: 24%;">
                <fieldset>
                    <legend>Atualizar Categoria dos Produtos</legend>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="tb">
                        <tr>
                            <td>Categoria Magento</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 350px;">
                                <asp:DropDownList ID="ddlCategoriaMag" runat="server" Width="344px" Height="21px" DataTextField="GRUPO"
                                    DataValueField="CODIGO">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btAtualizarMagento" runat="server" Text="Atualizar Magento" OnClick="btAtualizarMagento_Click" Width="150px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="labMsg" runat="server" Text="Utilize com moderação..." ForeColor="Red" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
