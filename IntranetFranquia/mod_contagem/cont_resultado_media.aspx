<%@ Page Title="Resultado de Rede" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="cont_resultado_media.aspx.cs" Inherits="Relatorios.cont_resultado_media" %>

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
        <span style="font-family: Calibri; font-size: 14px;">Módulo de Contagem&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Resultado de Rede&nbsp;&nbsp;</span>
        <div style="float: right; padding: 0; position: ;">
            <a href="cont_menu.aspx" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset>
            <legend>Resultado de Rede</legend>
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="2">
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>Supervisor
                                </td>
                                <td>Filial
                                </td>
                                <td colspan="2">Período Inicial
                                </td>
                                <td colspan="2">Período Final
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
                                        Height="22px" Width="304px">
                                    </asp:DropDownList>
                                </td>

                                <td style="width: 150px;">
                                    <asp:DropDownList runat="server" ID="ddlAnoDe" Height="22px" Width="144px" DataTextField="ANO"
                                        DataValueField="ANO">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 150px;">
                                    <asp:DropDownList runat="server" ID="ddlMesDe" Height="22px" Width="144px">
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
                                    <asp:DropDownList runat="server" ID="ddlAnoAte" Height="22px" Width="144px" DataTextField="ANO"
                                        DataValueField="ANO">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlMesAte" Height="22px" Width="144px">
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
                                <td>&nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 110px;">
                        <asp:Button runat="server" ID="btBuscar" Text="Buscar" OnClick="btBuscar_Click" Width="100px" OnClientClick="DesabilitarBotao(this);" />
                    </td>
                    <td>
                        <asp:Label ID="labErro" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div>
                            <fieldset>
                                <legend>Resultado de Rede</legend>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvResultadoContagem" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvResultadoContagem_RowDataBound"
                                        OnDataBound="gvResultadoContagem_DataBound" ShowFooter="true"
                                        AllowSorting="true" OnSorting="gvResultadoContagem_Sorting">
                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" Font-Bold="true" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Supervisor" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                SortExpression="NOME_SUPERVISOR">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litSupervisor" runat="server" Text='<%# Bind("NOME_SUPERVISOR") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Filial" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                SortExpression="FILIAL">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litFilial" runat="server" Text='<%# Bind("FILIAL") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Última Contagem" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center"
                                                SortExpression="DATA_CONTAGEM">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Dias" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center"
                                                SortExpression="DIAS_ULTIMA_CONTAGEM">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litDias" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valor Aceitável" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center"
                                                SortExpression="VALOR_ACEITAVEL">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValorAceitavel" runat="server" Text='<%# Bind("VALOR_ACEITAVEL") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Resultado de Peças" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center"
                                                SortExpression="RESULTADO_PECAS">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litResultadoPeca" runat="server" Text='<%# Bind("RESULTADO_PECAS") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="% Perda" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPorcPerda" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Peças/Dia" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPecasDia" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
