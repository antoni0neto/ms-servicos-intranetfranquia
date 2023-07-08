<%@ Page Title="Flash Varejo" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="gerloja_flash_varejo.aspx.cs" Inherits="Relatorios.gerloja_flash_varejo" MaintainScrollPositionOnPostback="true" %>

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Desempenho&nbsp;&nbsp;>&nbsp;&nbsp;Flash Varejo&nbsp;&nbsp;</span>
                <div style="float: right; padding: 0;">
                    <a id="hrefVoltar" runat="server" href="gerloja_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="accountInfo">
                <fieldset>
                    <legend>Flash Varejo</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="labAno" runat="server" Text="Ano"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labAnoComparativo" runat="server" Text="Ano Comparativo"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labMes" runat="server" Text="Mês"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labDiaIni" runat="server" Text="Dia Início"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labDiaFim" runat="server" Text="Dia Fim"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labSupervisor" runat="server" Text="Supervisor"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labTipoLoja" runat="server" Text="Tipo Loja"></asp:Label>
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
                            <td style="width: 130px;">
                                <asp:DropDownList runat="server" ID="ddlAnoComparativo" Height="22px" Width="124px" DataTextField="ANO"
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
                            <td style="width: 130px;">
                                <asp:DropDownList runat="server" ID="ddlDiaInicio" Height="22px" Width="124px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 130px;">
                                <asp:DropDownList runat="server" ID="ddlDiaFim" Height="22px" Width="124px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 180px;">
                                <asp:DropDownList runat="server" ID="ddlSupervisor" DataValueField="CODIGO_USUARIO" DataTextField="NOME_USUARIO"
                                    Height="22px" Width="174px" OnSelectedIndexChanged="ddlSupervisor_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 150px;">
                                <asp:DropDownList runat="server" ID="ddlTipoFilial" Height="22px" Width="144px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="OUTLET" Text="OUTLET"></asp:ListItem>
                                    <asp:ListItem Value="VAREJO" Text="VAREJO"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="22px" Width="274px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" />&nbsp;&nbsp;&nbsp;
                            </td>
                            <td colspan="7">
                                <asp:Label ID="labErro" runat="server" Text="" ForeColor="red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">&nbsp;
                            </td>
                        </tr>
                    </table>
                    <fieldset>
                        <legend>Comparativo</legend>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvFlashVarejoTotal" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvFlashVarejo_RowDataBound">
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
                                                        <asp:Literal ID="litFilial" runat="server" Text="REDE"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Qtde Peça" HeaderStyle-Width="90px" SortExpression="QTDE_PECA_IND">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litQtdePecaInd" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="P.A." HeaderStyle-Width="80px" SortExpression="PA_IND">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litPAInd" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="Preço Médio" HeaderStyle-Width="90px" SortExpression="PRECO_MEDIO_IND">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litPrecoMedioInd" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="Ticket Médio" HeaderStyle-Width="90px" SortExpression="TICKET_MEDIO_IND">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTicketMedioInd" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="Valor Venda" HeaderStyle-Width="90px" SortExpression="VALOR_VENDA_IND">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litValorVendaInd" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Atendimento" HeaderStyle-Width="90px" SortExpression="ATENDIMENTO_IND">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litQtdeAtendimentoInd" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="" HeaderStyle-Width="10px" ItemStyle-BackColor="GradientActiveCaption">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litEspaco" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Qtde Peça (%)" HeaderStyle-Width="95px" SortExpression="QTDE_PECA_IND_PORC">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litQtdePecaIndPorc" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="P.A. (%)" HeaderStyle-Width="80px" SortExpression="PA_IND_PORC">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litPAIndPorc" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="Preço Médio (%)" HeaderStyle-Width="115px" SortExpression="PRECO_MEDIO_IND_PORC">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litPrecoMedioIndPorc" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="Ticket Médio (%)" HeaderStyle-Width="115px" SortExpression="TICKET_MEDIO_IND_PORC">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTicketMedioIndPorc" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="Valor Venda (%)" HeaderStyle-Width="115px" SortExpression="VALOR_VENDA_IND_PORC">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litValorVendaIndPorc" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Atendimento (%)" HeaderStyle-Width="110px" SortExpression="ATENDIMENTO_IND_PORC">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litQtdeAtendimentoIndPorc" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvFlashVarejo" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvFlashVarejo_RowDataBound"
                                            OnDataBound="gvFlashVarejo_DataBound" ShowFooter="true"
                                            OnSorting="gvFlashVarejo_Sorting" AllowSorting="true">
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
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Qtde Peça" HeaderStyle-Width="90px" SortExpression="QTDE_PECA_IND">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litQtdePecaInd" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="P.A." HeaderStyle-Width="80px" SortExpression="PA_IND">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litPAInd" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="Preço Médio" HeaderStyle-Width="90px" SortExpression="PRECO_MEDIO_IND">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litPrecoMedioInd" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="Ticket Médio" HeaderStyle-Width="90px" SortExpression="TICKET_MEDIO_IND">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTicketMedioInd" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="Valor Venda" HeaderStyle-Width="90px" SortExpression="VALOR_VENDA_IND">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litValorVendaInd" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Atendimento" HeaderStyle-Width="90px" SortExpression="ATENDIMENTO_IND">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litQtdeAtendimentoInd" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="" HeaderStyle-Width="10px" ItemStyle-BackColor="GradientActiveCaption">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litEspaco" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Qtde Peça (%)" HeaderStyle-Width="95px" SortExpression="QTDE_PECA_IND_PORC">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litQtdePecaIndPorc" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="P.A. (%)" HeaderStyle-Width="80px" SortExpression="PA_IND_PORC">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litPAIndPorc" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="Preço Médio (%)" HeaderStyle-Width="115px" SortExpression="PRECO_MEDIO_IND_PORC">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litPrecoMedioIndPorc" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="Ticket Médio (%)" HeaderStyle-Width="115px" SortExpression="TICKET_MEDIO_IND_PORC">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTicketMedioIndPorc" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="Valor Venda (%)" HeaderStyle-Width="115px" SortExpression="VALOR_VENDA_IND_PORC">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litValorVendaIndPorc" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Atendimento (%)" HeaderStyle-Width="110px" SortExpression="ATENDIMENTO_IND_PORC">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litQtdeAtendimentoIndPorc" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset>
                        <legend>Lojas Abertas</legend>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvLojaAberta" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvLojaAberta_RowDataBound"
                                            OnDataBound="gvLojaAberta_DataBound" ShowFooter="true"
                                            OnSorting="gvLojaAberta_Sorting" AllowSorting="true">
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
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Qtde Peça" HeaderStyle-Width="90px" SortExpression="QTDE_PECA_IND">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litQtdePecaInd" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="P.A." HeaderStyle-Width="80px" SortExpression="PA_IND">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litPAInd" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="Preço Médio" HeaderStyle-Width="90px" SortExpression="PRECO_MEDIO_IND">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litPrecoMedioInd" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="Ticket Médio" HeaderStyle-Width="90px" SortExpression="TICKET_MEDIO_IND">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTicketMedioInd" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="Valor Venda" HeaderStyle-Width="90px" SortExpression="VALOR_VENDA_IND">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litValorVendaInd" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Atendimento" HeaderStyle-Width="90px" SortExpression="ATENDIMENTO_IND">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litQtdeAtendimentoInd" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="" HeaderStyle-Width="10px" ItemStyle-BackColor="GradientActiveCaption">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litEspaco" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Qtde Peça (%)" HeaderStyle-Width="95px" SortExpression="QTDE_PECA_IND_PORC">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litQtdePecaIndPorc" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="P.A. (%)" HeaderStyle-Width="80px" SortExpression="PA_IND_PORC">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litPAIndPorc" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="Preço Médio (%)" HeaderStyle-Width="115px" SortExpression="PRECO_MEDIO_IND_PORC">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litPrecoMedioIndPorc" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="Ticket Médio (%)" HeaderStyle-Width="115px" SortExpression="TICKET_MEDIO_IND_PORC">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTicketMedioIndPorc" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="Valor Venda (%)" HeaderStyle-Width="115px" SortExpression="VALOR_VENDA_IND_PORC">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litValorVendaIndPorc" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Atendimento (%)" HeaderStyle-Width="110px" SortExpression="ATENDIMENTO_IND_PORC">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litQtdeAtendimentoIndPorc" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset>
                        <legend>Lojas Fechadas</legend>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvLojaFechada" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvLojaFechada_RowDataBound"
                                            OnDataBound="gvLojaFechada_DataBound" ShowFooter="true"
                                            OnSorting="gvLojaFechada_Sorting" AllowSorting="true">
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
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Qtde Peça" HeaderStyle-Width="90px" SortExpression="QTDE_PECA_IND">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litQtdePecaInd" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="P.A." HeaderStyle-Width="80px" SortExpression="PA_IND">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litPAInd" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="Preço Médio" HeaderStyle-Width="90px" SortExpression="PRECO_MEDIO_IND">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litPrecoMedioInd" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="Ticket Médio" HeaderStyle-Width="90px" SortExpression="TICKET_MEDIO_IND">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTicketMedioInd" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="Valor Venda" HeaderStyle-Width="90px" SortExpression="VALOR_VENDA_IND">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litValorVendaInd" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Atendimento" HeaderStyle-Width="90px" SortExpression="ATENDIMENTO_IND">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litQtdeAtendimentoInd" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="" HeaderStyle-Width="10px" ItemStyle-BackColor="GradientActiveCaption">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litEspaco" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Qtde Peça (%)" HeaderStyle-Width="95px" SortExpression="QTDE_PECA_IND_PORC">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litQtdePecaIndPorc" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="P.A. (%)" HeaderStyle-Width="80px" SortExpression="PA_IND_PORC">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litPAIndPorc" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="Preço Médio (%)" HeaderStyle-Width="115px" SortExpression="PRECO_MEDIO_IND_PORC">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litPrecoMedioIndPorc" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="Ticket Médio (%)" HeaderStyle-Width="115px" SortExpression="TICKET_MEDIO_IND_PORC">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTicketMedioIndPorc" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
                                                    HeaderText="Valor Venda (%)" HeaderStyle-Width="115px" SortExpression="VALOR_VENDA_IND_PORC">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litValorVendaIndPorc" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Font-Size="Smaller" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Atendimento (%)" HeaderStyle-Width="110px" SortExpression="ATENDIMENTO_IND_PORC">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litQtdeAtendimentoIndPorc" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
