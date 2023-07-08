<%@ Page Title="WhatsApp Clientes" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="gerloja_wapp_niver.aspx.cs" Inherits="Relatorios.gerloja_wapp_niver" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
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
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript">
        function openwindow(l) {
            window.open(l, "NIVER", "menubar=1,resizable=0,width=1600,height=800");
        }
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="accountInfo">
                <br />
                <div>
                    <span style="font-family: Calibri; font-size: 14px;">Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Vendedores&nbsp;&nbsp;>&nbsp;&nbsp;WhatsApp Clientes</span>
                    <div style="float: right; padding: 0;">
                        &nbsp;
                    </div>
                </div>
                <hr />
                <fieldset>
                    <legend>
                        <asp:Label ID="labTitulo" runat="server" Text="WhatsApp Aniversariantes"></asp:Label></legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>Vendedor</td>
                            <td>CPF</td>
                            <td>Última Compra Ini</td>
                            <td>Última Compra Fim</td>
                            <td>Valor Ini</td>
                            <td>Valor Fim</td>
                            <td>Mês Niver</td>
                            <td>Dia Niver</td>
                            <td>Tem Handclub?</td>
                            <td>Já Falei?</td>
                            <td>Ja fiz Contato?</td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtUsuario" runat="server" Width="146px" Enabled="false" />
                            </td>
                            <td style="width: 110px;">
                                <asp:TextBox ID="txtCPF" runat="server" Width="96px" />
                            </td>
                            <td style="width: 140px;">
                                <asp:TextBox ID="txtPeriodoInicial" runat="server" autocomplete="off" Width="126px" MaxLength="10" Style="text-align: right;"
                                    onkeypress="return fnValidarData(event);"></asp:TextBox>
                            </td>
                            <td style="width: 140px;">
                                <asp:TextBox ID="txtPeriodoFinal" runat="server" autocomplete="off" Width="126px" MaxLength="10" Style="text-align: right;"
                                    onkeypress="return fnValidarData(event);"></asp:TextBox>
                            </td>
                            <td style="width: 110px;">
                                <asp:TextBox ID="txtValorIni" runat="server" Width="96px" />
                            </td>
                            <td style="width: 110px;">
                                <asp:TextBox ID="txtValorFim" runat="server" Width="96px" />
                            </td>
                            <td style="width: 150px;">
                                <asp:DropDownList ID="ddlMes" runat="server" Width="144px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
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
                                    <asp:ListItem Value="12" Text="Dezembro"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 70px;">
                                <asp:DropDownList ID="ddlDia" runat="server" Width="64px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                    <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                    <asp:ListItem Value="7" Text="7"></asp:ListItem>
                                    <asp:ListItem Value="8" Text="8"></asp:ListItem>
                                    <asp:ListItem Value="9" Text="9"></asp:ListItem>
                                    <asp:ListItem Value="10" Text="10"></asp:ListItem>
                                    <asp:ListItem Value="11" Text="11"></asp:ListItem>
                                    <asp:ListItem Value="12" Text="12"></asp:ListItem>
                                    <asp:ListItem Value="13" Text="13"></asp:ListItem>
                                    <asp:ListItem Value="14" Text="14"></asp:ListItem>
                                    <asp:ListItem Value="15" Text="15"></asp:ListItem>
                                    <asp:ListItem Value="16" Text="16"></asp:ListItem>
                                    <asp:ListItem Value="17" Text="17"></asp:ListItem>
                                    <asp:ListItem Value="18" Text="18"></asp:ListItem>
                                    <asp:ListItem Value="19" Text="19"></asp:ListItem>
                                    <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                    <asp:ListItem Value="21" Text="21"></asp:ListItem>
                                    <asp:ListItem Value="22" Text="22"></asp:ListItem>
                                    <asp:ListItem Value="23" Text="23"></asp:ListItem>
                                    <asp:ListItem Value="24" Text="24"></asp:ListItem>
                                    <asp:ListItem Value="25" Text="25"></asp:ListItem>
                                    <asp:ListItem Value="26" Text="26"></asp:ListItem>
                                    <asp:ListItem Value="27" Text="27"></asp:ListItem>
                                    <asp:ListItem Value="28" Text="28"></asp:ListItem>
                                    <asp:ListItem Value="29" Text="29"></asp:ListItem>
                                    <asp:ListItem Value="30" Text="30"></asp:ListItem>
                                    <asp:ListItem Value="31" Text="31"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 110px;">
                                <asp:DropDownList ID="ddlTemHandclub" runat="server" Width="104px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 110px;">
                                <asp:DropDownList ID="ddlJaFalei" runat="server" Width="104px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 110px;">
                                <asp:DropDownList ID="ddJaFizContato" runat="server" Width="104px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btBuscar" Text="Buscar Clientes" Width="120px" OnClick="btBuscar_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="12">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="12">
                                <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvVendas" runat="server" ForeColor="#333333" Width="100%" AutoGenerateColumns="False"
                                        OnRowDataBound="gvVendas_RowDataBound" ShowFooter="true"
                                        OnSorting="gvVendas_Sorting" AllowSorting="true"
                                        DataKeyNames="CODIGO">
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left"></HeaderStyle>
                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left"></FooterStyle>
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-Width="25px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="corTD">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litAbrirCliente" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litAbrirWapp" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CLIENTE_VAREJO" HeaderText="Cliente" HeaderStyle-Width="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" SortExpression="CLIENTE_VAREJO" />
                                            <asp:BoundField DataField="FILIAL" HeaderText="Compra + na Filial" HeaderStyle-Width="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" SortExpression="FILIAL" />
                                            <asp:BoundField DataField="NOME_VENDEDOR" HeaderText="+Atendido p/ Vendedor" HeaderStyle-Width="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" SortExpression="NOME_VENDEDOR" />
                                            <asp:TemplateField HeaderText="Última Compra" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" SortExpression="ULTIMA_COMPRA">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litUltimaCompra" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="QTDE_TICKET" HeaderText="Tickets" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" SortExpression="QTDE_TICKET" />
                                            <asp:TemplateField HeaderText="Valor Pago" HeaderStyle-Width="130px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" SortExpression="VALOR_PAGO">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValPago" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Handclub" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" SortExpression="HANDCLUB">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litHandclub" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Obs" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" SortExpression="OBS">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtObs" runat="server" Width="150px" TextMode="MultiLine" Height="40px" OnTextChanged="txtObs_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Falei c/ Cliente" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" SortExpression="DATA_BAIXA">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litFaleiCliente" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Button ID="btJaFalei" runat="server" Text="Já Falei" Width="70px" OnClick="btJaFalei_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
