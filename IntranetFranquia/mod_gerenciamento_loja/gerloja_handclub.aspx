<%@ Page Title="Vendas Handclub" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="gerloja_handclub.aspx.cs" Inherits="Relatorios.gerloja_handclub" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Always" EnableViewState="true">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Desempenho&nbsp;&nbsp;>&nbsp;&nbsp;Vendas Handclub&nbsp;&nbsp;</span>
                <div style="float: right; padding: 0;">
                    <a id="hrefVoltar" runat="server" href="gerloja_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="accountInfo">
                <fieldset>
                    <legend>Vendas Handclub</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="labAno" runat="server" Text="Ano"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labMes" runat="server" Text="Mês"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px;">
                                <asp:DropDownList runat="server" ID="ddlAno" Height="22px" Width="124px" DataTextField="ANO"
                                    DataValueField="ANO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 150px;">
                                <asp:DropDownList runat="server" ID="ddlMes" Height="22px" Width="144px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="01" Text="Janeiro"></asp:ListItem>
                                    <asp:ListItem Value="02" Text="Fevereiro"></asp:ListItem>
                                    <asp:ListItem Value="03" Text="Março"></asp:ListItem>
                                    <asp:ListItem Value="04" Text="Abril"></asp:ListItem>
                                    <asp:ListItem Value="05" Text="Maio"></asp:ListItem>
                                    <asp:ListItem Value="06" Text="Junho"></asp:ListItem>
                                    <asp:ListItem Value="07" Text="Julho"></asp:ListItem>
                                    <asp:ListItem Value="08" Text="Agosto"></asp:ListItem>
                                    <asp:ListItem Value="09" Text="Setembro"></asp:ListItem>
                                    <asp:ListItem Value="10" Text="Outubro"></asp:ListItem>
                                    <asp:ListItem Value="11" Text="Novembro"></asp:ListItem>
                                    <asp:ListItem Value="12" Text="Dezembro"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="22px" Width="274px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" />&nbsp;&nbsp;&nbsp;
                            </td>
                            <td colspan="3">
                                <asp:Label ID="labErro" runat="server" Text="" ForeColor="red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr />
                            </td>
                        </tr>
                    </table>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>

                            <td style="width: 552px;">&nbsp;<asp:Label ID="labTituloAnterior" runat="server" Text="" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>&nbsp;<asp:Label ID="labTituloAtual" runat="server" Text="" Font-Bold="true" Font-Size="Medium"></asp:Label></legend>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvFlashHandclub" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvFlashHandclub_RowDataBound"
                                        OnSorting="gvFlashHandclub_Sorting" AllowSorting="true">
                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Filial" SortExpression="FILIAL">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litFilial" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--MES ANTERIOR--%>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Handclub" HeaderStyle-Width="100px" SortExpression="HANDCLUB_PORC_1">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPHandclub_1" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Fat. Crescimento" HeaderStyle-Width="110px" SortExpression="FATOR_CRESCIMENTO_1">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPFatorCrescimento_1" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Resgate Hand" HeaderStyle-Width="100px" SortExpression="RESGATE_PORC_1">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPResgateHandclub_1" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="" ItemStyle-BackColor="LightGray" HeaderStyle-Width="2px" />
                                            <%--MES ATUAL--%>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Venda Líquida" HeaderStyle-Width="100px" SortExpression="VENDA_LIQUIDA_2" ItemStyle-BackColor="WhiteSmoke">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litVendaLiquida_2" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Venda Hand" HeaderStyle-Width="100px" SortExpression="VENDA_HANDCLUB_2" ItemStyle-BackColor="WhiteSmoke">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litVendaHandclub_2" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Handclub" HeaderStyle-Width="110px" SortExpression="HANDCLUB_PORC_2" ItemStyle-BackColor="WhiteSmoke">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPHandclub_2" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Hand Disponível" HeaderStyle-Width="110px" SortExpression="HANDCLUB_DISPONIVEL_2" ItemStyle-BackColor="WhiteSmoke">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litHandclubDisp_2" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Val. Resgatado" HeaderStyle-Width="110px" SortExpression="VALOR_RESGATADO_2" ItemStyle-BackColor="WhiteSmoke">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValResgate_2" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Val. Venda Resgate" HeaderStyle-Width="110px" SortExpression="VALOR_VENDA_RESGATADO_2" ItemStyle-BackColor="WhiteSmoke">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValVendaResgate_2" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Fator Crescimento" HeaderStyle-Width="110px" SortExpression="FATOR_CRESCIMENTO_2" ItemStyle-BackColor="WhiteSmoke">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPFatorCrescimento_2" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Resgate Hand" HeaderStyle-Width="100px" SortExpression="RESGATE_PORC_2" ItemStyle-BackColor="WhiteSmoke">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPResgateHandclub_2" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvFlashHandclubTotal" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvFlashHandclub_RowDataBound"
                                        ShowHeader="false">
                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text="&nbsp;"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Filial" SortExpression="FILIAL">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litFilial" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--MES ANTERIOR--%>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Handclub" ItemStyle-Width="100px" SortExpression="HANDCLUB_PORC_1">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPHandclub_1" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Fat. Crescimento" ItemStyle-Width="110px" SortExpression="FATOR_CRESCIMENTO_1">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPFatorCrescimento_1" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Resgate Hand" ItemStyle-Width="100px" SortExpression="RESGATE_PORC_1">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPResgateHandclub_1" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="" ItemStyle-BackColor="LightGray" ItemStyle-Width="4px" />
                                            <%--MES ATUAL--%>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Venda Líquida" ItemStyle-Width="100px" SortExpression="VENDA_LIQUIDA_2" ItemStyle-BackColor="WhiteSmoke">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litVendaLiquida_2" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Venda Hand" ItemStyle-Width="100px" SortExpression="VENDA_HANDCLUB_2" ItemStyle-BackColor="WhiteSmoke">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litVendaHandclub_2" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Handclub" ItemStyle-Width="110px" SortExpression="HANDCLUB_PORC_2" ItemStyle-BackColor="WhiteSmoke">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPHandclub_2" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Hand Disponível" ItemStyle-Width="110px" SortExpression="HANDCLUB_DISPONIVEL_2" ItemStyle-BackColor="WhiteSmoke">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litHandclubDisp_2" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Val. Resgatado" ItemStyle-Width="110px" SortExpression="VALOR_RESGATADO_2" ItemStyle-BackColor="WhiteSmoke">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValResgate_2" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Val. Venda Resgate" ItemStyle-Width="110px" SortExpression="VALOR_VENDA_RESGATADO_2" ItemStyle-BackColor="WhiteSmoke">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValVendaResgate_2" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Fator Crescimento" ItemStyle-Width="110px" SortExpression="FATOR_CRESCIMENTO_2" ItemStyle-BackColor="WhiteSmoke">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPFatorCrescimento_2" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Resgate Hand" ItemStyle-Width="100px" SortExpression="RESGATE_PORC_2" ItemStyle-BackColor="WhiteSmoke">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litPResgateHandclub_2" runat="server"></asp:Literal>
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
