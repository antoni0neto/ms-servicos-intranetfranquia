<%@ Page Title="Declaração de Faturamento" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="fisc_declaracao_faturamento.aspx.cs" Inherits="Relatorios.fisc_declaracao_faturamento" MaintainScrollPositionOnPostback="true" %>

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo do Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Declaração de Faturamento&nbsp;&nbsp;</span>
                <div style="float: right; padding: 0;">
                    <a id="hrefVoltar" runat="server" href="#" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="accountInfo">
                <fieldset>
                    <legend>Declaração de Faturamento</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="labEmpresa" runat="server" Text="Empresa"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labAno" runat="server" Text="Ano"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labMes" runat="server" Text="Mês"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labDescTribut" runat="server" Text="Descrição Tributação"></asp:Label>
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 350px;">
                                <asp:DropDownList runat="server" ID="ddlEmpresa" DataValueField="NOME" DataTextField="NOME"
                                    Height="22px" Width="344px" OnSelectedIndexChanged="ddlEmpresa_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 130px;">
                                <asp:DropDownList runat="server" ID="ddlAno" Height="22px" Width="124px" DataTextField="ANO"
                                    DataValueField="ANO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 130px;">
                                <asp:DropDownList runat="server" ID="ddlMes" Height="22px" Width="124px">
                                    <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
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
                            <td style="width: 280px;">
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="22px" Width="274px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 270px;">
                                <asp:TextBox ID="txtDescTribut" runat="server" Width="260px" MaxLength="35"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" />&nbsp;&nbsp;&nbsp;
                            </td>
                            <td style="text-align: right;">
                                <asp:Button ID="btImprimir" runat="server" Text="Imprimir" OnClick="btImprimir_Click" Width="100px" />&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="labErro" runat="server" Text="" ForeColor="red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 0;" colspan="7">
                                <fieldset>
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvDeclaracaoFAT" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                            Style="background: white" OnRowDataBound="gvDeclaracaoFAT_RowDataBound" OnDataBound="gvDeclaracaoFAT_DataBound" ShowFooter="true">
                                            <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                            <FooterStyle BackColor="Gainsboro" Font-Bold="true" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mês" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMes" runat="server" Text='<%#Bind("COMPETENCIA") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Valor" ItemStyle-HorizontalAlign="Justify" ItemStyle-BackColor="WhiteSmoke" ItemStyle-Width="250px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litValor" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Valor Intercompany" ItemStyle-HorizontalAlign="Justify" ItemStyle-BackColor="WhiteSmoke">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litValorIntercompany" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
