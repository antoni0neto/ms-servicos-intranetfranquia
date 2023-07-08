<%@ Page Title="Calendário Controle de Produção" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="desenv_controle_producao_calen.aspx.cs" Inherits="Relatorios.desenv_controle_producao_calen" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento {
            position: relative;
            float: left;
        }

        .style1 {
            width: 100%;
        }
    </style>

    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript">

        function openwindow(l) {
            window.open(l, "PROD_CALEN_LOCAL", "menubar=1,resizable=0,width=1200,height=600");
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="accountInfo">
        <br />
        <div>
            <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Calendário Controle de Produção</span>
            <div style="float: right; padding: 0;">
                <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
            </div>
        </div>
        <hr />
        <fieldset class="login">
            <legend>Calendário Controle de Produção</legend>
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" style="width: 50%;">
                        <fieldset>
                            <legend>Planejamento</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 210px">
                                        <asp:DropDownList ID="ddlColecaoPlan" runat="server" DataValueField="COLECAO" DataTextField="DESC_COLECAO" Width="204px"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="100px" OnClick="btBuscar_Click" />&nbsp;
                                    <asp:Label ID="labErro" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvPlanejamento" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                Style="background: white" OnRowDataBound="gvPlanejamento_RowDataBound" OnDataBound="gvPlanejamento_DataBound"
                                                OnSorting="gvPlanejamento_Sorting"
                                                ShowFooter="true" AllowSorting="true">
                                                <HeaderStyle BackColor="Gainsboro" />
                                                <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Right" Font-Size="Smaller" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Origem" HeaderStyle-Width="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        SortExpression="ORIGEM_DESC" ItemStyle-Font-Size="" FooterStyle-Font-Size="">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labOrigem" runat="server" Text='<%# Bind("ORIGEM_DESC") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SKU Varejo" HeaderStyle-Width="160px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                        SortExpression="SKU" ItemStyle-Font-Size="" FooterStyle-Font-Size="">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labSKU" runat="server" Text='<%# Bind("SKU") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="QTDE_VAREJO" HeaderText="Qtde Varejo" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                        ItemStyle-Font-Size="Smaller" DataFormatString="{0:###,###,##0}" SortExpression="QTDE_VAREJO" Visible="false" />

                                                    <asp:BoundField DataField="QTDE_ATACADO" HeaderText="Qtde Atacado" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                        ItemStyle-Font-Size="Smaller" DataFormatString="{0:###,###,##0}" SortExpression="QTDE_ATACADO" Visible="false" />

                                                    <asp:BoundField DataField="QTDE_TOTAL" HeaderText="Varejo" HeaderStyle-Width="160px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                        ItemStyle-Font-Size="" DataFormatString="{0:###,###,##0}" SortExpression="QTDE_TOTAL" />

                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                    <td valign="top" style="width: 50%;">
                        <fieldset>
                            <legend>Em Andamento</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 210px">&nbsp;
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Button ID="btAbrirPainel" runat="server" Text="Abrir Painel" Width="120px" OnClick="btAbrirPainel_Click" Enabled="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="line-height: 20px;">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvProducao" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                                Style="background: white" OnRowDataBound="gvProducao_RowDataBound" OnDataBound="gvProducao_DataBound"
                                                OnSorting="gvProducao_Sorting"
                                                ShowFooter="true" AllowSorting="true">
                                                <HeaderStyle BackColor="Gainsboro" />
                                                <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Right" Font-Size="Smaller" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litAbrirLocal" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Local" HeaderStyle-Width="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        SortExpression="LOCAL" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="" ItemStyle-Font-Bold="true">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labLocal" runat="server" Text='<%# Bind("LOCAL") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SKU Varejo" HeaderStyle-Width="85px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                        SortExpression="SKUVarejo" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labSKUVarejo" runat="server" Text='<%# Bind("SKUVarejo") %>'></asp:Label>&nbsp;
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="QTDE_VAREJO" HeaderText="Qtde Varejo" HeaderStyle-Width="85px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                        ItemStyle-Font-Size="Smaller" DataFormatString="{0:###,###,##0}" SortExpression="QTDE_VAREJO" Visible="false" />

                                                    <asp:TemplateField HeaderText="SKU Atacado" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                        SortExpression="SKUAtacado" ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="" ItemStyle-BackColor="FloralWhite" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labSKUAtacado" runat="server" Text='<%# Bind("SKUAtacado") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="QTDE_ATACADO" HeaderText="Qtde Atacado" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                        ItemStyle-Font-Size="Smaller" DataFormatString="{0:###,###,##0}" SortExpression="QTDE_ATACADO" ItemStyle-BackColor="FloralWhite" Visible="false" />

                                                    <asp:TemplateField HeaderText="SKU Varejo" HeaderStyle-Width="222px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                        SortExpression="SKU" ItemStyle-Font-Size="" FooterStyle-Font-Size="">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labSKU" runat="server" Text='<%# Bind("SKU") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="QTDE_TOTAL" HeaderText="Qtde Varejo" HeaderStyle-Width="220px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                        ItemStyle-Font-Size="" DataFormatString="{0:###,###,##0}" SortExpression="QTDE_TOTAL" />

                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvFaltaCorte" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333" ShowHeader="true"
                                                Style="background: white" OnRowDataBound="gvFaltaCorte_RowDataBound">
                                                <HeaderStyle BackColor="Gainsboro" />
                                                <RowStyle Font-Bold="true" />
                                                <FooterStyle BackColor="Gainsboro" Font-Bold="true" HorizontalAlign="Right" Font-Size="Smaller" />
                                                <Columns>

                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            &nbsp;
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litAbrirFalta" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="LOCAL" HeaderText="-" HeaderStyle-Width="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="" ItemStyle-Font-Bold="true" DataFormatString="{0:###,###,##0}" />


                                                    <asp:BoundField DataField="QTDE_TOTAL" HeaderText="Qtde Faltante" HeaderStyle-Width="110px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                        ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="" ItemStyle-ForeColor="Red" ItemStyle-Font-Bold="true" DataFormatString="{0:###,###,##0}" />

                                                    <asp:BoundField DataField="QTDE_ATACADO" HeaderText="Média por Dia" HeaderStyle-Width="110px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                        ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="" ItemStyle-ForeColor="Red" ItemStyle-Font-Bold="true" DataFormatString="{0:###,###,##0}" />

                                                    <asp:TemplateField HeaderText="Previsão de Término" HeaderStyle-Width="220px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                        ItemStyle-Font-Size="Smaller" FooterStyle-Font-Size="">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labPrevTermino" runat="server" ForeColor="Red" Font-Bold="true" Text="-"></asp:Label>&nbsp;
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
            </table>
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" style="width: 50%;">
                        <fieldset>
                            <legend>Produção</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="3">
                                        <asp:HiddenField ID="hidAno" runat="server" Value="" />
                                        <asp:HiddenField ID="hidMes" runat="server" Value="" />
                                        <asp:HiddenField ID="hidCodigoUsuario" runat="server" Value="0" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 33%;">
                                        <asp:DropDownList ID="ddlColecao" runat="server" DataValueField="COLECAO" DataTextField="DESC_COLECAO" Width="200px"></asp:DropDownList>
                                    </td>
                                    <td style="width: 33%; text-align: center;">
                                        <asp:Label ID="labPeriodo1" runat="server" Text="" Font-Bold="true" Font-Size="Larger"></asp:Label>
                                    </td>
                                    <td style="width: 33%; text-align: right;">
                                        <asp:Button ID="btAtualizar" runat="server" Text="Atualizar" Width="70px" OnClick="btAtualizar_Click" />&nbsp;
                            <asp:Button ID="btAnterior" runat="server" Text="<<" Width="70px" OnClick="btAnterior_Click" />&nbsp;
                        <asp:Button ID="btProximo" runat="server" Text=">>" Width="70px" OnClick="btProximo_Click" />
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
                                    <td colspan="10">&nbsp;</td>
                                </tr>
                                <tr style="background-color: black; text-align: center;">
                                    <td style="width: 17%">&nbsp;<asp:Label ID="labCol1Vazio1" runat="server" Text="" Font-Bold="true" ForeColor="White"></asp:Label></td>
                                    <td style="width: 9%">&nbsp;<asp:Label ID="labCol1Segunda" runat="server" Text="Segunda" Font-Bold="true" ForeColor="White"></asp:Label></td>
                                    <td style="width: 9%">&nbsp;<asp:Label ID="labCol1Terca" runat="server" Text="Terça" Font-Bold="true" ForeColor="White"></asp:Label></td>
                                    <td style="width: 9%">&nbsp;<asp:Label ID="labCol1Quarta" runat="server" Text="Quarta" Font-Bold="true" ForeColor="White"></asp:Label></td>
                                    <td style="width: 9%">&nbsp;<asp:Label ID="labCol1Quinta" runat="server" Text="Quinta" Font-Bold="true" ForeColor="White"></asp:Label></td>
                                    <td style="width: 9%">&nbsp;<asp:Label ID="labCol1Sexta" runat="server" Text="Sexta" Font-Bold="true" ForeColor="White"></asp:Label></td>
                                    <td style="width: 9%">&nbsp;<asp:Label ID="labCol1Sabado" runat="server" Text="Sábado" Font-Bold="true" ForeColor="White"></asp:Label></td>
                                    <td style="width: 9%">&nbsp;<asp:Label ID="labCol1Domingo" runat="server" Text="Domingo" Font-Bold="true" ForeColor="White"></asp:Label></td>
                                    <td style="width: 10%">&nbsp;<asp:Label ID="labCol1Vazio2" runat="server" Text="" Font-Bold="true" ForeColor="White"></asp:Label></td>
                                    <td style="width: 10%">&nbsp;<asp:Label ID="labCol1Vazio3" runat="server" Text="" Font-Bold="true" ForeColor="White"></asp:Label></td>
                                </tr>
                            </table>
                            <asp:Repeater ID="repProducao1" runat="server" OnItemDataBound="repProducao_ItemDataBound">
                                <ItemTemplate>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 17%">
                                                <asp:Table ID="tbCabeca" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%" Font-Names="Arial" Font-Size="Smaller">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Black" ForeColor="White">
                                                            <asp:Label ID="labTextoDia" runat="server" Font-Bold="true" Text="Dia"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black">
                                                            <asp:Label ID="labTextoRiscoSKU" runat="server" Font-Bold="true" Text="SKU RISCO"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labTextoRiscoQTDE" runat="server" Font-Bold="true" Text="QTDE RISCO"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Label ID="labTextoCorteSKU" runat="server" Font-Bold="true" Text="SKU CORTE"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Label ID="labTextoCorteQTDE" runat="server" Font-Bold="true" Text="QTDE CORTE"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labTextoFaccaoSKU" runat="server" Font-Bold="true" Text="SKU FACÇÃO"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labTextoFaccaoQTDE" runat="server" Font-Bold="true" Text="QTDE FACÇÃO"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labTextoAcabSKU" runat="server" Font-Bold="true" Text="SKU ACAB"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labTextoAcabQTDE" runat="server" Font-Bold="true" Text="QTDE ACAB"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>

                                            <td style="width: 9%">
                                                <asp:Table ID="tbSegunda" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%" Font-Names="Arial" Font-Size="Smaller" Font-Bold="true">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Black" ForeColor="White">
                                                            <asp:Label ID="litSegundaDia" runat="server" Font-Bold="true"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black">
                                                            <asp:Literal ID="litSegundaRiscoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSegundaRiscoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litSegundaCorteSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litSegundaCorteQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSegundaFaccaoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSegundaFaccaoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSegundaAcabSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSegundaAcabQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 9%">
                                                <asp:Table ID="tbTerca" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%" Font-Names="Arial" Font-Size="Smaller" Font-Bold="true">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Black" ForeColor="White">
                                                            <asp:Label ID="litTercaDia" runat="server" Font-Bold="true"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black">
                                                            <asp:Literal ID="litTercaRiscoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litTercaRiscoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litTercaCorteSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litTercaCorteQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litTercaFaccaoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litTercaFaccaoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litTercaAcabSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litTercaAcabQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 9%">
                                                <asp:Table ID="tbQuarta" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%" Font-Names="Arial" Font-Size="Smaller" Font-Bold="true">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Black" ForeColor="White">
                                                            <asp:Label ID="litQuartaDia" runat="server" Font-Bold="true"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black">
                                                            <asp:Literal ID="litQuartaRiscoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litQuartaRiscoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litQuartaCorteSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litQuartaCorteQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litQuartaFaccaoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litQuartaFaccaoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litQuartaAcabSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litQuartaAcabQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 9%">
                                                <asp:Table ID="tbQuinta" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%" Font-Names="Arial" Font-Size="Smaller" Font-Bold="true">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Black" ForeColor="White">
                                                            <asp:Label ID="litQuintaDia" runat="server" Font-Bold="true"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black">
                                                            <asp:Literal ID="litQuintaRiscoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litQuintaRiscoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litQuintaCorteSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litQuintaCorteQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litQuintaFaccaoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litQuintaFaccaoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litQuintaAcabSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litQuintaAcabQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 9%">
                                                <asp:Table ID="tbSexta" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%" Font-Names="Arial" Font-Size="Smaller" Font-Bold="true">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Black" ForeColor="White">
                                                            <asp:Label ID="litSextaDia" runat="server" Font-Bold="true"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black">
                                                            <asp:Literal ID="litSextaRiscoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSextaRiscoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litSextaCorteSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litSextaCorteQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSextaFaccaoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSextaFaccaoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSextaAcabSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSextaAcabQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 9%">
                                                <asp:Table ID="tbSabado" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%" Font-Names="Arial" Font-Size="Smaller" Font-Bold="true">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Black" ForeColor="White">
                                                            <asp:Label ID="litSabadoDia" runat="server" Font-Bold="true"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black">
                                                            <asp:Literal ID="litSabadoRiscoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSabadoRiscoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litSabadoCorteSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litSabadoCorteQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSabadoFaccaoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSabadoFaccaoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSabadoAcabSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSabadoAcabQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 9%">
                                                <asp:Table ID="tbDomingo" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%" Font-Names="Arial" Font-Size="Smaller" Font-Bold="true">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Black" ForeColor="White">
                                                            <asp:Label ID="litDomingoDia" runat="server" Font-Bold="true"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black">
                                                            <asp:Literal ID="litDomingoRiscoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litDomingoRiscoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litDomingoCorteSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litDomingoCorteQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litDomingoFaccaoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litDomingoFaccaoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litDomingoAcabSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litDomingoAcabQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 10%">
                                                <asp:Table ID="tbRodaSoma" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%" Font-Names="Arial" Font-Size="Smaller">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkGray" ForeColor="White">
                                                            <asp:Label ID="labDiaSomaVazio" runat="server" Font-Bold="true" Text="Soma"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black">
                                                            <asp:Label ID="labRiscoSKUSoma" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labRiscoQTDESoma" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Label ID="labCorteSKUSoma" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Label ID="labCorteQTDESoma" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labFaccaoSKUSoma" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labFaccaoQTDESoma" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labAcabSKUSoma" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labAcabQTDESoma" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>

                                            <td style="width: 10%">
                                                <asp:Table ID="tbRodaMedia" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%" Font-Names="Arial" Font-Size="Smaller">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkGray" ForeColor="White">
                                                            <asp:Label ID="labDiaMediaVazio" runat="server" Font-Bold="true" Text="Média"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black">
                                                            <asp:Label ID="labRiscoSKUMedia" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labRiscoQTDEMedia" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Label ID="labCorteSKUMedia" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Label ID="labCorteQTDEMedia" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labFaccaoSKUMedia" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labFaccaoQTDEMedia" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labAcabSKUMedia" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labAcabQTDEMedia" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:Repeater>
                        </fieldset>
                    </td>
                    <td valign="top" style="width: 50%;">
                        <fieldset>
                            <legend>-</legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 33%;">&nbsp;
                                    </td>
                                    <td style="width: 33%; text-align: center;">
                                        <asp:Label ID="labPeriodo2" runat="server" Text="" Font-Bold="true" Font-Size="Larger"></asp:Label>
                                    </td>
                                    <td style="width: 33%; text-align: right;">&nbsp;
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
                                    <td colspan="10">&nbsp;</td>
                                </tr>
                                <tr style="background-color: black; text-align: center;">
                                    <td style="width: 17%">&nbsp;<asp:Label ID="labCol2Vazio1" runat="server" Text="" Font-Bold="true" ForeColor="White"></asp:Label></td>
                                    <td style="width: 9%">&nbsp;<asp:Label ID="labCol2Segunda" runat="server" Text="Segunda" Font-Bold="true" ForeColor="White"></asp:Label></td>
                                    <td style="width: 9%">&nbsp;<asp:Label ID="labCol2Terca" runat="server" Text="Terça" Font-Bold="true" ForeColor="White"></asp:Label></td>
                                    <td style="width: 9%">&nbsp;<asp:Label ID="labCol2Quarta" runat="server" Text="Quarta" Font-Bold="true" ForeColor="White"></asp:Label></td>
                                    <td style="width: 9%">&nbsp;<asp:Label ID="labCol2Quinta" runat="server" Text="Quinta" Font-Bold="true" ForeColor="White"></asp:Label></td>
                                    <td style="width: 9%">&nbsp;<asp:Label ID="labCol2Sexta" runat="server" Text="Sexta" Font-Bold="true" ForeColor="White"></asp:Label></td>
                                    <td style="width: 9%">&nbsp;<asp:Label ID="labCol2Sabado" runat="server" Text="Sábado" Font-Bold="true" ForeColor="White"></asp:Label></td>
                                    <td style="width: 9%">&nbsp;<asp:Label ID="labCol2Domingo" runat="server" Text="Domingo" Font-Bold="true" ForeColor="White"></asp:Label></td>
                                    <td style="width: 10%">&nbsp;<asp:Label ID="labCol2Vazio2" runat="server" Text="" Font-Bold="true" ForeColor="White"></asp:Label></td>
                                    <td style="width: 10%">&nbsp;<asp:Label ID="labCol2Vazio3" runat="server" Text="" Font-Bold="true" ForeColor="White"></asp:Label></td>
                                </tr>
                            </table>
                            <asp:Repeater ID="repProducao2" runat="server" OnItemDataBound="repProducao_ItemDataBound">
                                <ItemTemplate>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 17%">
                                                <asp:Table ID="tbCabeca" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%" Font-Names="Arial" Font-Size="Smaller">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Black" ForeColor="White">
                                                            <asp:Label ID="labTextoDia" runat="server" Font-Bold="true" Text="Dia"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black">
                                                            <asp:Label ID="labTextoRiscoSKU" runat="server" Font-Bold="true" Text="SKU RISCO"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labTextoRiscoQTDE" runat="server" Font-Bold="true" Text="QTDE RISCO"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Label ID="labTextoCorteSKU" runat="server" Font-Bold="true" Text="SKU CORTE"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Label ID="labTextoCorteQTDE" runat="server" Font-Bold="true" Text="QTDE CORTE"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labTextoFaccaoSKU" runat="server" Font-Bold="true" Text="SKU FACÇÃO"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labTextoFaccaoQTDE" runat="server" Font-Bold="true" Text="QTDE FACÇÃO"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labTextoAcabSKU" runat="server" Font-Bold="true" Text="SKU ACAB"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labTextoAcabQTDE" runat="server" Font-Bold="true" Text="QTDE ACAB"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>

                                            <td style="width: 9%">
                                                <asp:Table ID="tbSegunda" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%" Font-Names="Arial" Font-Size="Smaller" Font-Bold="true">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Black" ForeColor="White">
                                                            <asp:Label ID="litSegundaDia" runat="server" Font-Bold="true"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black">
                                                            <asp:Literal ID="litSegundaRiscoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSegundaRiscoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litSegundaCorteSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litSegundaCorteQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSegundaFaccaoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSegundaFaccaoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSegundaAcabSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSegundaAcabQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 9%">
                                                <asp:Table ID="tbTerca" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%" Font-Names="Arial" Font-Size="Smaller" Font-Bold="true">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Black" ForeColor="White">
                                                            <asp:Label ID="litTercaDia" runat="server" Font-Bold="true"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black">
                                                            <asp:Literal ID="litTercaRiscoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litTercaRiscoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litTercaCorteSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litTercaCorteQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litTercaFaccaoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litTercaFaccaoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litTercaAcabSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litTercaAcabQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 9%">
                                                <asp:Table ID="tbQuarta" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%" Font-Names="Arial" Font-Size="Smaller" Font-Bold="true">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Black" ForeColor="White">
                                                            <asp:Label ID="litQuartaDia" runat="server" Font-Bold="true"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black">
                                                            <asp:Literal ID="litQuartaRiscoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litQuartaRiscoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litQuartaCorteSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litQuartaCorteQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litQuartaFaccaoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litQuartaFaccaoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litQuartaAcabSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litQuartaAcabQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 9%">
                                                <asp:Table ID="tbQuinta" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%" Font-Names="Arial" Font-Size="Smaller" Font-Bold="true">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Black" ForeColor="White">
                                                            <asp:Label ID="litQuintaDia" runat="server" Font-Bold="true"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black">
                                                            <asp:Literal ID="litQuintaRiscoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litQuintaRiscoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litQuintaCorteSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litQuintaCorteQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litQuintaFaccaoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litQuintaFaccaoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litQuintaAcabSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litQuintaAcabQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 9%">
                                                <asp:Table ID="tbSexta" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%" Font-Names="Arial" Font-Size="Smaller" Font-Bold="true">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Black" ForeColor="White">
                                                            <asp:Label ID="litSextaDia" runat="server" Font-Bold="true"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black">
                                                            <asp:Literal ID="litSextaRiscoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSextaRiscoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litSextaCorteSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litSextaCorteQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSextaFaccaoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSextaFaccaoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSextaAcabSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSextaAcabQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 9%">
                                                <asp:Table ID="tbSabado" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%" Font-Names="Arial" Font-Size="Smaller" Font-Bold="true">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Black" ForeColor="White">
                                                            <asp:Label ID="litSabadoDia" runat="server" Font-Bold="true"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black">
                                                            <asp:Literal ID="litSabadoRiscoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSabadoRiscoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litSabadoCorteSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litSabadoCorteQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSabadoFaccaoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSabadoFaccaoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSabadoAcabSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litSabadoAcabQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 9%">
                                                <asp:Table ID="tbDomingo" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%" Font-Names="Arial" Font-Size="Smaller" Font-Bold="true">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" Width="20%" Height="16px" BackColor="Black" ForeColor="White">
                                                            <asp:Label ID="litDomingoDia" runat="server" Font-Bold="true"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black">
                                                            <asp:Literal ID="litDomingoRiscoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litDomingoRiscoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litDomingoCorteSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Literal ID="litDomingoCorteQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litDomingoFaccaoSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litDomingoFaccaoQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litDomingoAcabSKU" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Literal ID="litDomingoAcabQTDE" runat="server"></asp:Literal>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 10%">
                                                <asp:Table ID="tbRodaSoma" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%" Font-Names="Arial" Font-Size="Smaller">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkGray" ForeColor="White">
                                                            <asp:Label ID="labDiaSomaVazio" runat="server" Font-Bold="true" Text="Soma"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black">
                                                            <asp:Label ID="labRiscoSKUSoma" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labRiscoQTDESoma" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Label ID="labCorteSKUSoma" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Label ID="labCorteQTDESoma" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labFaccaoSKUSoma" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labFaccaoQTDESoma" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labAcabSKUSoma" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labAcabQTDESoma" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>

                                            <td style="width: 10%">
                                                <asp:Table ID="tbRodaMedia" runat="server" BorderWidth="1" CellPadding="0" CellSpacing="0" Width="100%" Font-Names="Arial" Font-Size="Smaller">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkGray" ForeColor="White">
                                                            <asp:Label ID="labDiaMediaVazio" runat="server" Font-Bold="true" Text="Média"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black">
                                                            <asp:Label ID="labRiscoSKUMedia" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Turquoise" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labRiscoQTDEMedia" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Label ID="labCorteSKUMedia" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Navy" ForeColor="White" BorderWidth="1px" BorderColor="Black">
                                                            <asp:Label ID="labCorteQTDEMedia" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labFaccaoSKUMedia" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="DarkOrange" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labFaccaoQTDEMedia" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labAcabSKUMedia" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Right" Width="20%" Height="16px" BackColor="Peru" ForeColor="Black" BorderWidth="1px">
                                                            <asp:Label ID="labAcabQTDEMedia" runat="server" Font-Bold="true" Text="0"></asp:Label>&nbsp;
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:Repeater>
                        </fieldset>
                    </td>
                </tr>
            </table>



        </fieldset>
    </div>
</asp:Content>
