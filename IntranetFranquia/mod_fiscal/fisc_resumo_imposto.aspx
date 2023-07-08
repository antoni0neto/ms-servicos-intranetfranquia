<%@ Page Title="Resumo dos Impostos" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="fisc_resumo_imposto.aspx.cs" Inherits="Relatorios.fisc_resumo_imposto" MaintainScrollPositionOnPostback="true" %>

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo do Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Resumo dos Impostos&nbsp;&nbsp;</span>
                <div style="float: right; padding: 0;">
                    <a id="hrefVoltar" runat="server" href="#" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="accountInfo">
                <fieldset>
                    <legend>Resumo dos Impostos</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="labAnoDe" runat="server" Text="Ano De"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labMesDe" runat="server" Text="Mês De"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labAnoAte" runat="server" Text="Até Ano"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labMesAte" runat="server" Text="Até Mês"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labEmpresa" runat="server" Text="Empresa"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>

                            <td style="width: 130px;">
                                <asp:DropDownList runat="server" ID="ddlAnoDe" Height="22px" Width="124px" DataTextField="ANO"
                                    DataValueField="ANO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 140px;">
                                <asp:DropDownList runat="server" ID="ddlMesDe" Height="22px" Width="134px">
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
                            <td style="width: 130px;">
                                <asp:DropDownList runat="server" ID="ddlAnoAte" Height="22px" Width="124px" DataTextField="ANO"
                                    DataValueField="ANO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 140px;">
                                <asp:DropDownList runat="server" ID="ddlMesAte" Height="22px" Width="134px">
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
                            <td style="width: 350px;">
                                <asp:DropDownList runat="server" ID="ddlEmpresa" DataValueField="NOME" DataTextField="NOME"
                                    Height="22px" Width="344px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 250px;">
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="22px" Width="244px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Button ID="btImprimir" runat="server" Text="Imprimir" OnClick="btImprimir_Click" Width="100px" />&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <asp:Label ID="labErro" runat="server" Text="" ForeColor="red"></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td style="padding: 0;" colspan="8">
                                <fieldset>
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvResumoImposto" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                            Style="background: white" OnRowDataBound="gvResumoImposto_RowDataBound" OnDataBound="gvResumoImposto_DataBound" ShowFooter="true">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                            <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" Font-Bold="true" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Competência" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="WhiteSmoke">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litCompetencia" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PIS" ItemStyle-HorizontalAlign="Justify" ItemStyle-Width="150px" ItemStyle-BackColor="WhiteSmoke">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litPIS" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="COFINS" ItemStyle-HorizontalAlign="Justify" ItemStyle-Width="150px" ItemStyle-BackColor="WhiteSmoke">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litCOFINS" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ICMS" ItemStyle-HorizontalAlign="Justify" ItemStyle-Width="150px" ItemStyle-BackColor="WhiteSmoke">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litICMS" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="INSS" ItemStyle-HorizontalAlign="Justify" ItemStyle-Width="150px" ItemStyle-BackColor="WhiteSmoke">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litINSS" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IRPJ" ItemStyle-HorizontalAlign="Justify" ItemStyle-Width="150px" ItemStyle-BackColor="WhiteSmoke">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litIRPJ" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CSLL" ItemStyle-HorizontalAlign="Justify" ItemStyle-Width="150px" ItemStyle-BackColor="WhiteSmoke">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litCSLL" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Simples Nacional" ItemStyle-HorizontalAlign="Justify" ItemStyle-Width="150px" ItemStyle-BackColor="WhiteSmoke">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litSN" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Justify" ItemStyle-Width="150px" ItemStyle-BackColor="WhiteSmoke">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal>
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
