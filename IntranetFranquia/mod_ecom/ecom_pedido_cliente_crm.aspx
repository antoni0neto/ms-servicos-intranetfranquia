<%@ Page Title="CRM de Clientes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="ecom_pedido_cliente_crm.aspx.cs" Inherits="Relatorios.ecom_pedido_cliente_crm"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }

        .corTD {
            background-color: #EFEFEF;
        }

        .pageStyl a, .pageStyl span {
            display: block;
            height: 15px;
            width: 15px;
            font-weight: bold;
            text-align: center;
            text-decoration: none;
        }

        .pageStyl a {
            background-color: #f5f5f5;
            color: #969696;
            border: 1px solid #969696;
        }

        .pageStyl span {
            background-color: #A1DCF2;
            color: #000;
            border: 1px solid #3AC0F2;
        }
    </style>
    <link href="../Styles/gridviewScroll.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/1.9.0/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.9.1/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../js/js.js"></script>
    <script type="text/javascript" src="../js/gridviewScroll.min.js"></script>

    <script type="text/javascript">

        function gridviewScroll() {

            gvAtacado = $('#MainContent_gvCliente').gridviewScroll({
                width: 1430,
                height: 550,
                railcolor: "#F0F0F0",
                barcolor: "#CDCDCD",
                barhovercolor: "#606060",
                bgcolor: "#F0F0F0",
                freezesize: 3,
                arrowsize: 30,
                varrowtopimg: "../Image/arrowvt.png",
                varrowbottomimg: "../Image/arrowvb.png",
                harrowleftimg: "../Image/arrowhl.png",
                harrowrightimg: "../Image/arrowhr.png",
                headerrowcount: 1,
                railsize: 16,
                barsize: 12
            });
        }

        function openwindow(l) {
            window.open(l, "PEDIDOMAG_CLIX", "menubar=1,resizable=0,width=1600,height=800");
        }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do E-Commerce&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Clientes&nbsp;&nbsp;>&nbsp;&nbsp;CRM de Clientes</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="CRM de Clientes"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Ano</td>
                        <td>Mês De</td>
                        <td>Até o Mês</td>
                        <td>Nome</td>
                        <td>CPF</td>
                        <td>Email</td>
                        <td>Qtde Meses</td>
                        <td>Qtde Pedidos</td>
                        <td>Teve Ação?</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 150px;">
                            <asp:DropDownList runat="server" ID="ddlAno" Height="22px" Width="144px" DataTextField="ANO"
                                DataValueField="ANO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 150px;">
                            <asp:DropDownList runat="server" ID="ddlMesDe" Height="22px" Width="144px" Enabled="true">
                                <asp:ListItem Value="1" Text="Janeiro" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Fevereiro"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Março"></asp:ListItem>
                                <asp:ListItem Value="4" Text="Abril"></asp:ListItem>
                                <asp:ListItem Value="5" Text="Maio"></asp:ListItem>
                                <asp:ListItem Value="6" Text="Junho"></asp:ListItem>
                                <asp:ListItem Value="7" Text="Julho"></asp:ListItem>
                                <asp:ListItem Value="8" Text="Agosto"></asp:ListItem>
                                <asp:ListItem Value="9" Text="Setembro"></asp:ListItem>
                                <asp:ListItem Value="10" Text="Outubro"></asp:ListItem>
                                <asp:ListItem Value="11" Text="Novembro"></asp:ListItem>
                                <asp:ListItem Value="12" Text="Dezembro"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 150px;">
                            <asp:DropDownList runat="server" ID="ddlMesAte" Height="22px" Width="144px" Enabled="true">
                                <asp:ListItem Value="1" Text="Janeiro"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Fevereiro"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Março"></asp:ListItem>
                                <asp:ListItem Value="4" Text="Abril"></asp:ListItem>
                                <asp:ListItem Value="5" Text="Maio"></asp:ListItem>
                                <asp:ListItem Value="6" Text="Junho"></asp:ListItem>
                                <asp:ListItem Value="7" Text="Julho"></asp:ListItem>
                                <asp:ListItem Value="8" Text="Agosto"></asp:ListItem>
                                <asp:ListItem Value="9" Text="Setembro"></asp:ListItem>
                                <asp:ListItem Value="10" Text="Outubro"></asp:ListItem>
                                <asp:ListItem Value="11" Text="Novembro"></asp:ListItem>
                                <asp:ListItem Value="12" Text="Dezembro" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 170px;">
                            <asp:TextBox ID="txtNome" runat="server" Width="160px" MaxLength="30" />
                        </td>
                        <td style="width: 110px;">
                            <asp:TextBox ID="txtCPF" runat="server" Width="100px" MaxLength="11" />
                        </td>
                        <td style="width: 150px;">
                            <asp:TextBox ID="txtEmail" runat="server" Width="140px" MaxLength="50" />
                        </td>
                        <td style="width: 150px;">
                            <asp:TextBox ID="txtQtdeMeses" runat="server" Width="140px" MaxLength="50" />
                        </td>
                        <td style="width: 150px;">
                            <asp:TextBox ID="txtQtdePedidos" runat="server" Width="140px" MaxLength="50" />
                        </td>
                        <td style="width: 150px;">
                            <asp:DropDownList runat="server" ID="ddlAcao" Height="22px" Width="144px" Enabled="true">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10">
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10">
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="114px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />
                        </td>
                    </tr>
                    <tr style="line-height: 20px;">
                        <td colspan="10">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10">
                            <div class="rounded_corners">
                                <asp:GridView ID="gvCliente" runat="server" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white; width: 1800px; border-collapse: collapse;" OnRowDataBound="gvCliente_RowDataBound"
                                    OnDataBound="gvCliente_DataBound" ShowFooter="true"
                                    OnSorting="gvCliente_Sorting" AllowSorting="true"
                                    DataKeyNames="CLIENTE_ATACADO">
                                    <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Size="Smaller"></HeaderStyle>
                                    <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Bold="true" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-Width="25px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="corTD">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-Width="25px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="corTD">
                                            <ItemTemplate>
                                                <asp:Literal ID="litAbrirCliente" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CLIENTE" HeaderText="Cliente" SortExpression="CLIENTE" HeaderStyle-Width="210px" ItemStyle-Width="210px" ItemStyle-CssClass="corTD" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="Data" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" ItemStyle-Width="60px" SortExpression="DATA_CRIACAO" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litDataUltimoAcao" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="QTDE_MESES" HeaderText="Tot Mes" SortExpression="QTDE_MESES" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />

                                        <asp:BoundField DataField="TOT_QTDE" HeaderText="Qtde Tot" SortExpression="TOT_QTDE" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="Val Tot" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="50px" SortExpression="TOT_VAL" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValTotal" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="JAN_QTDE" HeaderText="Qtde JAN" SortExpression="JAN_QTDE" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="Val JAN" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="50px" SortExpression="JAN_VAL" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValJAN" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="FEV_QTDE" HeaderText="Qtde FEV" SortExpression="FEV_QTDE" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="Val FEV" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="50px" SortExpression="FEV_VAL" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValFEV" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="MAR_QTDE" HeaderText="Qtde MAR" SortExpression="MAR_QTDE" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="Val MAR" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="50px" SortExpression="MAR_VAL" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValMAR" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="ABR_QTDE" HeaderText="Qtde ABR" SortExpression="ABR_QTDE" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="Val ABR" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="50px" SortExpression="ABR_VAL" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValABR" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="MAI_QTDE" HeaderText="Qtde MAI" SortExpression="MAI_QTDE" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="Val MAI" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="50px" SortExpression="MAI_VAL" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValMAI" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="JUN_QTDE" HeaderText="Qtde JUN" SortExpression="JUN_QTDE" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="Val JUN" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="50px" SortExpression="JUN_VAL" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValJUN" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="JUL_QTDE" HeaderText="Qtde JUL" SortExpression="JUL_QTDE" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="Val JUL" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="50px" SortExpression="JUL_VAL" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValJUL" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="AGO_QTDE" HeaderText="Qtde AGO" SortExpression="AGO_QTDE" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="Val AGO" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="50px" SortExpression="AGO_VAL" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValAGO" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="SET_QTDE" HeaderText="Qtde SET" SortExpression="SET_QTDE" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="Val SET" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="50px" SortExpression="SET_VAL" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValSET" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="OUT_QTDE" HeaderText="Qtde OUT" SortExpression="OUT_QTDE" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="Val OUT" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="50px" SortExpression="OUT_VAL" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValOUT" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="NOV_QTDE" HeaderText="Qtde NOV" SortExpression="NOV_QTDE" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="Val NOV" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="50px" SortExpression="NOV_VAL" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValNOV" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="DEZ_QTDE" HeaderText="Qtde DEZ" SortExpression="DEZ_QTDE" HeaderStyle-Width="50px" ItemStyle-Width="50px" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="Val DEZ" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="" SortExpression="DEZ_VAL" ItemStyle-Font-Size="Smaller" HeaderStyle-Font-Size="Smaller"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litValDEZ" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
