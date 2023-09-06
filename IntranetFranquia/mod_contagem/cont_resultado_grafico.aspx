<%@ Page Title="Gráfico de Resultado" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="cont_resultado_grafico.aspx.cs" Inherits="Relatorios.cont_resultado_grafico" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento {
            float: left;
            position: static;
        }

        .style1 {
            width: 100%;
        }
    </style>
    <script src="../js/js.js"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo de Contagem&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Gráfico de Resultado&nbsp;&nbsp;</span>
        <div style="float: right; padding: 0; position: ;">
            <a href="cont_menu.aspx" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset>
            <legend>Gráfico de Resultado</legend>
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="3">
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>Supervisor
                                </td>
                                <td>Filial
                                </td>
                                <td>Período Inicial
                                </td>
                                <td>Período Final
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px;">
                                    <asp:DropDownList runat="server" ID="ddlSupervisor" DataValueField="CODIGO_USUARIO" DataTextField="NOME_USUARIO"
                                        Height="22px" Width="194px" OnSelectedIndexChanged="ddlSupervisor_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 310px;">
                                    <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                        Height="22px" Width="304px" OnSelectedIndexChanged="ddlFilial_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 300px;">
                                    <asp:DropDownList runat="server" ID="ddlDataContagemIni" DataValueField="Value" DataTextField="Text"
                                        Height="22px" Width="294px">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 300px;">
                                    <asp:DropDownList runat="server" ID="ddlDataContagemFim" DataValueField="Value" DataTextField="Text"
                                        Height="22px" Width="294px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="labErro" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 110px;">
                        <asp:Button runat="server" ID="btBuscar" Text="Buscar" OnClick="btBuscar_Click" Width="100px" OnClientClick="DesabilitarBotao(this);" />
                    </td>
                    <td>
                        <asp:CheckBox ID="cbRede" runat="server" Checked="false" />
                        Rede
                    </td>
                    <td style="text-align: right;">
                        <asp:Button runat="server" ID="btEnviarEmail" Text="E-Mail" OnClick="btEnviarEmail_Click" Width="100px" OnClientClick="DesabilitarBotao(this);" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div>
                            <fieldset>
                                <legend>Resultado de Contagem</legend>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvResultadoContagem" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvResultadoContagem_RowDataBound"
                                        OnDataBound="gvResultadoContagem_DataBound" ShowFooter="true">
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Última Contagem" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDiaUltimaContagem" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valor Aceitável" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValorAceitavel" runat="server" Text='<%# Bind("VALOR_ACEITAVEL") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Resultado de Peças" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litResultadoPeca" runat="server" Text='<%# Bind("RESULTADO_PECAS") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="% Perda" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPorcPerda" runat="server" Text='<%# Bind("PORC_PERDA") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Peças/Dia" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPecasDia" runat="server" Text='<%# Bind("PECAS_POR_DIA") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Gerente" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litGerente" runat="server" Text='<%# Bind("GERENTE") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <fieldset>
                            <legend>Gráfico</legend>
                            <asp:Panel ID="pnlResultado" runat="server" BackColor="White" BorderWidth="1" BorderColor="Black">
                            </asp:Panel>
                            <asp:HiddenField ID="chartValue" runat="server" />
                        </fieldset>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
