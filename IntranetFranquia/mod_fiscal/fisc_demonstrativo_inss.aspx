<%@ Page Title="Demonstrativo INSS" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="fisc_demonstrativo_inss.aspx.cs" Inherits="Relatorios.fisc_demonstrativo_inss" MaintainScrollPositionOnPostback="true" %>

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo do Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Demonstrativo INSS&nbsp;&nbsp;</span>
                <div style="float: right; padding: 0;">
                    <a id="hrefVoltar" runat="server" href="#" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="accountInfo">
                <fieldset>
                    <legend>Demonstrativo INSS</legend>
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
                                <asp:Label ID="labAliquota" runat="server" Text="Alíquota"></asp:Label>
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 350px;">
                                <asp:DropDownList runat="server" ID="ddlEmpresa" DataValueField="NOME" DataTextField="NOME"
                                    Height="22px" Width="344px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 150px;">
                                <asp:DropDownList runat="server" ID="ddlAno" Height="22px" Width="144px" DataTextField="ANO"
                                    DataValueField="ANO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 150px;">
                                <asp:DropDownList runat="server" ID="ddlMes" Height="22px" Width="144px">
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
                            <td style="width: 150px;">
                                <asp:TextBox ID="txtAliquota" runat="server" Text="2,50" Width="140px" CssClass="alinharDireita"
                                    onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" />&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="labErro" runat="server" Text="" ForeColor="red"></asp:Label>
                            </td>
                            <td style="text-align: right;">
                                <asp:Button ID="btImprimir" runat="server" Text="Imprimir" OnClick="btImprimir_Click" Width="100px" />&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 0;" colspan="6">
                                <fieldset>
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvINSS" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                            Style="background: white" OnRowDataBound="gvINSS_RowDataBound" OnDataBound="gvINSS_DataBound" ShowFooter="true">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                            <FooterStyle BackColor="GradientActiveCaption" Font-Bold="true" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Filial" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFilial" runat="server" Text='<%#Bind("FILIAL") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CNPJ" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="140px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litCNPJ" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="FAT Operacional" ItemStyle-HorizontalAlign="Justify" ItemStyle-Width="150px" ItemStyle-BackColor="WhiteSmoke">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFATOperacional" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="FAT NÃO Operacional" ItemStyle-HorizontalAlign="Justify" ItemStyle-Width="150px" ItemStyle-BackColor="WhiteSmoke">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFATNAOOperacional" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Devoluções de Vendas" ItemStyle-HorizontalAlign="Justify" ItemStyle-Width="150px" ItemStyle-BackColor="GhostWhite">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDEVVendas" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="RET MERC. NÃO Entregue" ItemStyle-HorizontalAlign="Justify" ItemStyle-Width="170px" ItemStyle-BackColor="GhostWhite">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litRETMercNAOEntregue" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Base de Cálculo" ItemStyle-HorizontalAlign="Justify" ItemStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litBaseCalculo" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Alíquota" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAliquota" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Valor do INSS" ItemStyle-HorizontalAlign="Justify" ItemStyle-Width="150px" ItemStyle-BackColor="WhiteSmoke">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litValorINSS" runat="server"></asp:Literal>
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
