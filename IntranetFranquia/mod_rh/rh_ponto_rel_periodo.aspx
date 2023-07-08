<%@ Page Title="Relatório Batidas" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="rh_ponto_rel_periodo.aspx.cs" Inherits="Relatorios.rh_ponto_rel_periodo"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .jGrowl .manilla {
            color: black;
        }

        .jGrowl .redError {
            color: red;
        }

        .alinharDireita {
            text-align: right;
        }
    </style>
    <script src="http://code.jquery.com/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do RH&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;<asp:Label
                    ID="labTituloMenu" runat="server" Text="Período de Batida"></asp:Label></span>
                <div style="float: right; padding: 0;">
                    <a href="rh_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 87%;">
                <fieldset>
                    <legend>
                        <asp:Label ID="labTitulo" runat="server" Text="Batidas"></asp:Label></legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="labFilial" runat="server" Text="Filtro Filial"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labDataInicial" runat="server" Text="Data Inicial"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labDataFinal" runat="server" Text="Data Final"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labFuncionario" runat="server" Text="Funcionário"></asp:Label>
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 260px;">
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="22px" Width="254px" OnSelectedIndexChanged="ddlFilial_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 220px;">
                                <asp:TextBox ID="txtDataInicial" runat="server" MaxLength="10" Width="210px"
                                    onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                            </td>
                            <td style="width: 220px;">
                                <asp:TextBox ID="txtDataFinal" runat="server" MaxLength="10" Width="210px"
                                    onkeypress="return fnValidarData(event);" CssClass="alinharDireita"></asp:TextBox>
                            </td>
                            <td style="width: 260px;">
                                <asp:DropDownList runat="server" ID="ddlFuncionario" DataValueField="CODIGO" DataTextField="NOME"
                                    Height="22px" Width="254px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="110px" OnClick="btBuscar_Click"
                                    OnClientClick="DesabilitarBotao(this);" />
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvBatida" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvBatida_RowDataBound"
                                        OnDataBound="gvBatida_DataBound" ShowFooter="true">
                                        <HeaderStyle BackColor="GradientActiveCaption" />
                                        <FooterStyle BackColor="GradientActiveCaption" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-BorderWidth="1" ItemStyle-BorderColor="Gray" ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="GradientActiveCaption">
                                                <HeaderStyle BorderWidth="1" BorderColor="Gray" />
                                                <FooterStyle BorderWidth="1" BorderColor="Gray" />
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data Referência" HeaderStyle-Width="120px" ItemStyle-BorderWidth="1" ItemStyle-BorderColor="Gray"
                                                ItemStyle-BackColor="GradientActiveCaption" ItemStyle-HorizontalAlign="Left">
                                                <HeaderStyle BorderWidth="1" BorderColor="Gray" HorizontalAlign="Left" />
                                                <FooterStyle BorderWidth="1" BorderColor="Gray" Font-Bold="true" />
                                                <ItemTemplate>
                                                    <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Literal ID="litTotalF" runat="server" Text="Total"></asp:Literal>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Nome" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNome" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tipo Batida" HeaderStyle-Width="115px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTipoBatida" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Entrada 1" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litEntrada1" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Saída 1" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litSaida1" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Entrada 2" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litEntrada2" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Saída 2" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litSaida2" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Intervalo" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litIntervalo" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Falta" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                <FooterStyle Font-Bold="true" HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Literal ID="litFalta" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Extra" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                <FooterStyle Font-Bold="true" HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Literal ID="litHoraExtra" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                <FooterStyle Font-Bold="true" HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" style="text-align: right;">Total de Horas Trabalhadas: &nbsp;
                                <asp:Label ID="labTotalHoraTrabalhada" runat="server" Font-Size="Small" Font-Bold="true" ForeColor="Red" Text="00:00"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">&nbsp;
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div style="float: right; margin-top: 27px;">
                <table border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <td>Pendente
                        </td>
                        <td style="background-color: #E6E6FA;">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>Folga
                        </td>
                        <td style="background-color: #90EE90; width: 70px;">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>Falta
                        </td>
                        <td style="background-color: #CD5C5C;">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>Feriado Não Trab
                        </td>
                        <td style="background-color: #F5DEB3;">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>Feriado Trabalhado
                        </td>
                        <td style="background-color: #FFC0CB;">&nbsp;
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
