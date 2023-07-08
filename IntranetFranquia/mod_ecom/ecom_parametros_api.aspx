<%@ Page Title="Parâmetros API" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master"
    CodeBehind="ecom_parametros_api.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="Relatorios.ecom_parametros_api" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .bodyMaterial {
            font-family: "Helvetica Neue", "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif !important;
            margin: 0px;
            padding: 0px;
            color: #696969;
        }

        .jGrowl .manilla {
            background-color: #000;
            color: red;
            font-weight: bold;
        }
    </style>
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            $("#tabs").tabs();

        });
        function MarcarAba(vAba) {
            document.getElementById("MainContent_hidTabSelected").value = vAba;
        }

    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Cadastros&nbsp;&nbsp;>&nbsp;&nbsp;Parâmetros API</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <br />
            <div style="float: left; width: 1000px; margin-left: 15%;">
                <fieldset>
                    <legend>Parâmetros API</legend>
                    <div style="padding-left: 0px;">
                        <asp:ImageButton ID="ibtSalvar" runat="server" Width="19px" ImageUrl="~/Image/approve.png"
                            ToolTip="Salvar" OnClick="ibtSalvar_Click" />
                        <asp:ImageButton ID="ibtCancelar" runat="server" Width="18px" ImageUrl="~/Image/cancel.png"
                            ToolTip="Cancelar" OnClick="ibtCancelar_Click" />
                        <asp:ImageButton ID="ibtEditar" runat="server" Width="20px" ImageUrl="~/Image/edit.jpg"
                            ToolTip="Editar" OnClick="ibtEditar_Click" />
                        <asp:HiddenField ID="hidTabSelected" runat="server" Value="" />
                        <asp:HiddenField ID="hidAcao" runat="server" Value="" />
                        <div style="text-align: right; float: right;">
                            <asp:Label ID="labErro" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                    <hr />
                    <div id="tabs">
                        <ul>
                            <li><a href="#tabs-venda" id="tabVenda" runat="server" onclick="MarcarAba(0);">Linx</a></li>
                            <li><a href="#tabs-email" id="tabEmail" runat="server" onclick="MarcarAba(1);">E-Mail</a></li>
                            <li><a href="#tabs-pedido" id="tabPedido" runat="server" onclick="MarcarAba(2);">Aprovação Pedido</a></li>
                        </ul>
                        <div id="tabs-venda">
                            <fieldset>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                    <tr>
                                        <td>
                                            <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labTransportadora" runat="server" Text="Transportadora"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labContaContabilCliente" runat="server" Text="Conta Contábil Cliente"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 295px;">
                                            <asp:DropDownList ID="ddlFilial" runat="server" Width="289px" DataTextField="FILIAL" DataValueField="COD_FILIAL"></asp:DropDownList>
                                        </td>
                                        <td style="width: 295px;">
                                            <asp:DropDownList ID="ddlTransportadora" runat="server" Width="289px" DataTextField="TRANSPORTADORA1" DataValueField="TRANSPORTADORA1"></asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtContaContabilCliente" runat="server" Width="295px" Height="15px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="labColecao" runat="server" Text="Coleção"></asp:Label>

                                        </td>
                                        <td>
                                            <asp:Label ID="labTabelaPreco" runat="server" Text="Tabela de Preço"></asp:Label>

                                        </td>
                                        <td>
                                            <asp:Label ID="labRepresentante" runat="server" Text="Representante"></asp:Label></td>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlColecao" runat="server" Width="289px" DataTextField="DESC_COLECAO" DataValueField="COLECAO"></asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTabelaPreco" runat="server" Width="289px" DataTextField="CODIGO_TAB_PRECO" DataValueField="CODIGO_TAB_PRECO"></asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlRepresentante" runat="server" Width="299px">
                                                <asp:ListItem Value="HANDBOOK ONLINE" Text="HANDBOOK ONLINE"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="labTipo" runat="server" Text="Tipo Venda"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labTipoDocumento" runat="server" Text="Tipo Documento"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labAprovacao" runat="server" Text="Aprovação"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoVenda" runat="server" Width="289px" DataTextField="TIPO" DataValueField="TIPO"></asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoDocumento" runat="server" Width="289px" DataTextField="TIPO_DOCUMENTO" DataValueField="LX_TIPO_DOCUMENTO"></asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlAprovacao" runat="server" Width="299px">
                                                <asp:ListItem Value="E" Text="EM ESTUDO"></asp:ListItem>
                                                <asp:ListItem Value="A" Text="APROVADO" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="R" Text="REPROVADO"></asp:ListItem>
                                                <asp:ListItem Value="N" Text="NO AGUARDO"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="labContaContabilICR" runat="server" Text="Conta Contábil ICR"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labContaContabilLTA" runat="server" Text="Conta Contábil LTA"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labContaContabilIAC" runat="server" Text="Conta Contábil IAC"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtContaContabilICR" runat="server" Width="285px" Height="15px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtContaContabilLTA" runat="server" Width="285px" Height="15px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtContaContabilIAC" runat="server" Width="295px" Height="15px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="labCentroCustoICR" runat="server" Text="Centro de Custo ICR"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labCentroCustoLTA" runat="server" Text="Centro de Custo LTA"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labCentroCustoIAC" runat="server" Text="Centro de Custo IAC"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtCentroCustoICR" runat="server" Width="285px" Height="15px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCentroCustoLTA" runat="server" Width="285px" Height="15px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCentroCustoIAC" runat="server" Width="295px" Height="15px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                        <div id="tabs-email">
                            <fieldset>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cbConfiguraEmail" runat="server" Text="Configurar E-mail?" TextAlign="Right" OnCheckedChanged="cbConfiguraEmail_CheckedChanged" AutoPostBack="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="labEmailRemetente" runat="server" Text="E-Mail Remetente"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="width: 901px;">
                                            <asp:TextBox ID="txtRemetente" runat="server" Width="891px" Height="15px"></asp:TextBox></td>
                                        <td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="labUsuario" runat="server" Text="Usuário"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="labSenha" runat="server" Text="Senha"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtUsuario" runat="server" Width="340px" Height="15px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSenha" runat="server" Width="200px" Height="15px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="labSMTP" runat="server" Text="SMTP"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="labPorta" runat="server" Text="Porta"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 350px;">
                                            <asp:TextBox ID="txtSMTP" runat="server" Width="340px" Height="15px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPorta" runat="server" Width="200px" Height="15px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                        <div id="tabs-pedido">
                            <fieldset>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyMaterial">
                                    <tr>
                                        <td>
                                            <asp:Label ID="labValorPedidoAprovacao" runat="server" Text="Valor Mínimo Pedido"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtValorPedidoAprovacao" runat="server" Width="285px" Height="15px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                    </div>
                </fieldset>
                <br />
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
