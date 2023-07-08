<%@ Page Title="Painel de Produção" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="desenv_painel_producaov2.aspx.cs" Inherits="Relatorios.desenv_painel_producaov2" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
        }

        .aligncenter {
            text-align: center;
        }
    </style>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../js/js.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Produção&nbsp;&nbsp;>&nbsp;&nbsp;Painel de Produção</span>
        <div style="float: right; padding: 0;">
            <a href="desenv_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="login" style="background-color: white;">
        <fieldset style="padding-top: 0; background-color: white;">
            <legend>Painel de Produção</legend>
            <div style="width: 100%;">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>Coleção</td>
                                    <td>Griffe</td>
                                    <td>Grupo Produto</td>
                                    <td>Previsão Inicial</td>
                                    <td>Previsão Final</td>
                                    <td>Recebido -90%?</td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 190px">
                                        <asp:DropDownList ID="ddlColecao" runat="server" Width="184px" Height="21px" DataTextField="DESC_COLECAO"
                                            DataValueField="COLECAO">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 150px">
                                        <asp:DropDownList ID="ddlGriffe" runat="server" Width="144px" Height="21px" DataTextField="GRIFFE"
                                            DataValueField="GRIFFE">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 178px;">
                                        <asp:DropDownList ID="ddlGrupo" runat="server" Width="172px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                            DataValueField="GRUPO_PRODUTO">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 120px;">
                                        <asp:TextBox ID="txtDataPrevIni" runat="server" autocomplete="off" Width="110px" MaxLength="10" Style="text-align: right;"
                                            onkeypress="return fnValidarData(event);"></asp:TextBox>
                                    </td>
                                    <td style="width: 120px;">
                                        <asp:TextBox ID="txtDataPrevFim" runat="server" autocomplete="off" Width="110px" MaxLength="10" Style="text-align: right;"
                                            onkeypress="return fnValidarData(event);"></asp:TextBox>
                                    </td>
                                    <td style="width: 130px;">
                                        <asp:DropDownList ID="ddl90Porc" runat="server" Width="124px" Height="21px">
                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                            <asp:ListItem Value="S" Text="Sim" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btBuscar" runat="server" Width="100px" Text="Buscar" OnClick="btBuscar_Click" />&nbsp;<asp:Label ID="labErro" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <fieldset>
                                            <legend>Visibilidade de Painéis</legend>
                                            <asp:CheckBox ID="cbTodos" runat="server" Checked="true" OnCheckedChanged="cbTodos_CheckedChanged" AutoPostBack="true" />Todos<br />
                                            <asp:CheckBox ID="cbPrePedido" runat="server" Checked="true" />Pré-Pedido&nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="cbPreModelo" runat="server" Checked="true" />Pré-modelo&nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="cbModEDes" runat="server" Checked="true" />Modelagem e Design&nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="cb1Peca" runat="server" Checked="true" />1ª Peça&nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="cbModelagem" runat="server" Checked="true" />Modelagem&nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="cbPreRisco" runat="server" Checked="true" />Pré-Risco&nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="cbRisco" runat="server" Checked="true" />Risco&nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="cbCorte" runat="server" Checked="true" />Corte&nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="cbEncFaccao" runat="server" Checked="true" />Encaixe Facção&nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="cbFaccao" runat="server" Checked="true" />Facção&nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="cbEncAcabamento" runat="server" Checked="true" />Encaixe Acabamento&nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="cbAcabamento" runat="server" Checked="true" />Acabamento&nbsp;&nbsp;&nbsp;
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <div style="width: 9500px;">
                    <div id="divPrePedido" runat="server" visible="false" style="width: 850px; float: left; margin-right: 5px;">
                        <fieldset id="fsPrePedido" runat="server" style="background-color: #DDA0DD;">
                            &nbsp;<span style="font-weight: bold; color: black;">PRÉ-PEDIDO -
                                    <asp:Label ID="labTotPrePedido" runat="server" Text="0" />
                            </span>
                            <br />
                            <br />
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlPrePedido" runat="server" BackColor="White" BorderWidth="1" BorderColor="White" Height="100%"></asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div id="divPreModelo" runat="server" visible="false" style="width: 850px; float: left; margin-right: 5px;">
                        <fieldset id="fsPreModelo" runat="server" style="background-color: #F5F5F5;">
                            &nbsp;<span style="font-weight: bold; color: black;">PRÉ-MODELO -
                                    <asp:Label ID="labTotPreModelo" runat="server" Text="0" /></span>
                            <br />
                            <br />
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlPreModelo" runat="server" BackColor="White" BorderWidth="1" BorderColor="White" Height="100%">
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div id="divModEDes" runat="server" visible="false" style="width: 850px; float: left; margin-right: 5px;">
                        <fieldset id="fsModEDes" runat="server" style="background-color: #FFC0CB;">
                            &nbsp;<span style="font-weight: bold; color: black;">MODELAGEM DE CRIAÇÃO E DESIGN DE ESTAMPAS -
                                    <asp:Label ID="labTotMODeDES" runat="server" Text="0" /></span>
                            <br />
                            <br />
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlModEDes" runat="server" BackColor="White" BorderWidth="1" BorderColor="White" Height="100%">
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div id="div1Peca" runat="server" visible="false" style="width: 850px; float: left; margin-right: 5px;">
                        <fieldset id="fs1peca" runat="server" style="background-color: #F5DEB3;">
                            &nbsp;<span style="font-weight: bold; color: black;">1ª PEÇA -
                                    <asp:Label ID="labTot1Peca" runat="server" Text="0" /></span>
                            <br />
                            <br />
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnl1Peca" runat="server" BackColor="White" BorderWidth="1" BorderColor="White" Height="100%">
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div id="divModelagem" runat="server" visible="false" style="width: 850px; float: left; margin-right: 5px;">
                        <fieldset id="fsModelagem" runat="server"
                            style="background-color: azure;">
                            &nbsp;<span style="font-weight: bold; color: black;">MODELAGEM -
                                    <asp:Label ID="labTotModelagem" runat="server" Text="0" /></span>
                            <br />
                            <br />
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlModelagem" runat="server" BackColor="White" BorderWidth="1" BorderColor="White" Height="100%">
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div id="divPreRisco" runat="server" visible="false" style="width: 850px; float: left; margin-right: 5px;">
                        <fieldset id="fsPreRisco" runat="server" style="background-color: cadetblue;">
                            &nbsp;<span style="font-weight: bold; color: white;">PRÉ-RISCO -
                                    <asp:Label ID="labTotPreRisco" runat="server" Text="0" /></span><br />
                            <br />
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlPreRisco" runat="server" BackColor="White" BorderWidth="1" BorderColor="White" Height="100%">
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div id="divRisco" runat="server" visible="false" style="width: 850px; float: left; margin-right: 5px;">
                        <fieldset id="fsRisco" runat="server" style="background-color: aquamarine;">
                            &nbsp;<span style="font-weight: bold; color: black;">RISCO -
                                    <asp:Label ID="labTotRisco" runat="server" Text="0" /></span><br />
                            <br />
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlRisco" runat="server" BackColor="White" BorderWidth="1" BorderColor="White" Height="100%">
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div id="divCorte" runat="server" visible="false" style="width: 850px; float: left; margin-right: 5px;">
                        <fieldset id="fsCorte" runat="server" style="background-color: darkblue;">
                            &nbsp;<span style="font-weight: bold; color: white;">CORTE -
                                    <asp:Label ID="labTotCorte" runat="server" Text="0" /></span><br />
                            <br />
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlCorte" runat="server" BackColor="White" BorderWidth="1" BorderColor="White" Height="100%">
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div id="divEncFaccao" runat="server" visible="false" style="width: 850px; float: left; margin-right: 5px;">
                        <fieldset id="fsEncFaccao" runat="server" style="background-color: orange;">
                            &nbsp;<span style="font-weight: bold; color: black;">ENCAIXE FACÇÃO -
                                    <asp:Label ID="labTotEncFaccao" runat="server" Text="0" /></span><br />
                            <br />
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlEncaixeFaccao" runat="server" BackColor="White" BorderWidth="1" BorderColor="White" Height="100%">
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div id="divFaccao" runat="server" visible="false" style="width: 850px; float: left; margin-right: 5px;">
                        <fieldset id="fsFaccao" runat="server" style="background-color: darkorange;">
                            &nbsp;<span style="font-weight: bold; color: black;">FACÇÃO -
                                    <asp:Label ID="labTotFaccao" runat="server" Text="0" /></span><br />
                            <br />
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlFaccao" runat="server" BackColor="White" BorderWidth="1" BorderColor="White" Height="100%">
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div id="divEncAcabamento" runat="server" visible="false" style="width: 850px; float: left; margin-right: 5px;">
                        <fieldset id="fsEncAcabamento" runat="server" style="background-color: saddlebrown;">
                            &nbsp;<span style="font-weight: bold; color: white;">ENCAIXE ACABAMENTO -
                                    <asp:Label ID="labTotEncAcabamento" runat="server" Text="0" /></span><br />
                            <br />
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlEncaixeAcabamento" runat="server" BackColor="White" BorderWidth="1" BorderColor="White" Height="100%">
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div id="divAcabamento" runat="server" visible="false" style="width: 850px; float: left; margin-right: 5px;">
                        <fieldset id="fsAcabamento" runat="server" style="background-color: rosybrown;">
                            &nbsp;<span style="font-weight: bold; color: white;">ACABAMENTO -
                                    <asp:Label ID="labTotAcabamento" runat="server" Text="0" /></span><br />
                            <br />
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlAcabamento" runat="server" BackColor="White" BorderWidth="1" BorderColor="White" Height="100%">
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                </div>

            </div>

        </fieldset>
    </div>

</asp:Content>
