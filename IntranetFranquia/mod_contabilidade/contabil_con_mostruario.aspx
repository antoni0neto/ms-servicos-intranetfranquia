<%@ Page Title="Controle de Mostruário" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="contabil_con_mostruario.aspx.cs" Inherits="Relatorios.contabil_con_mostruario"
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

        .alinharCentro {
            text-align: center;
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Contabilidade&nbsp;&nbsp;>&nbsp;&nbsp;Apoio&nbsp;&nbsp;>&nbsp;&nbsp;<asp:Label
                    ID="labTituloMenu" runat="server" Text="Controle de Mostruário"></asp:Label></span>
                <div style="float: right; padding: 0;">
                    <a href="contabil_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 100%;">
                <fieldset>
                    <legend>
                        <asp:Label ID="labTitulo" runat="server" Text="Controle de Mostruário"></asp:Label></legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>Representante
                            </td>
                            <td>Natureza</td>
                            <td>Data Inicial
                            </td>
                            <td>Data Final
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 250px;">
                                <asp:DropDownList ID="ddlRepresentante" runat="server" DataValueField="REPRESENTANTE" DataTextField="REPRESENTANTE" Width="244px"></asp:DropDownList>
                            </td>
                            <td style="width: 400px;">
                                <asp:DropDownList ID="ddlNatureza" runat="server" Width="394px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="100.07" Text="100.07 - VENDA DE MERCADORIA FABRICA"></asp:ListItem>
                                    <asp:ListItem Value="128.02" Text="128.02 - REM. BONIFICACAO OU DOACAO"></asp:ListItem>
                                    <asp:ListItem Value="133.01" Text="133.01 - REM. MERC P/DEMONSTRACAO"></asp:ListItem>
                                    <asp:ListItem Value="244.01" Text="244.01 - RET. MERCADORIA REMET. EM DEMONSTRACAO"></asp:ListItem>
                                    <asp:ListItem Value="244.02" Text="244.02 - RET. SIMBOLICO MERC REMET DEMONSTRACAO"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtDataIni" runat="server" Width="150px" onkeypress="return fnValidarData(event);"
                                    MaxLength="10" Style="text-align: right;"></asp:TextBox>
                            </td>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtDataFim" runat="server" Width="150px" onkeypress="return fnValidarData(event);"
                                    MaxLength="10" Style="text-align: right;"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="110px" OnClick="btBuscar_Click"
                                    OnClientClick="DesabilitarBotao(this);" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="labErro" runat="server" ForeColor="Red" Text="&nbsp;"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <fieldset>
                                    <legend>Fiscal</legend>
                                    <div id="accordionE">
                                        <h3>Controle de Remessa</h3>
                                        <div>
                                            <table border="0" cellpadding="0" class="tb" width="100%">
                                                <tr>
                                                    <td style="width: 100%;">
                                                        <div class="rounded_corners">
                                                            <asp:GridView ID="gvControleRemessa" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvControleRemessa_RowDataBound"
                                                                OnDataBound="gvControleRemessa_DataBound"
                                                                ShowFooter="true">
                                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                        HeaderText="Data" HeaderStyle-Width="90px">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litData" runat="server" Text=''></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                        HeaderText="Data Retorno" HeaderStyle-Width="90px">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litDataPrevRet" runat="server" Text='<%# Bind("DATA_RETORNO") %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="LANCAMENTO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                        HeaderText="Lançamento" HeaderStyle-Width="90px" />
                                                                    <asp:BoundField DataField="REPRESENTANTE" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Representante" HeaderStyle-Width="230px" />
                                                                    <asp:BoundField DataField="NATUREZA" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Natureza" />
                                                                    <asp:BoundField DataField="NOTA_FISCAL" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Nota Fiscal" HeaderStyle-Width="110px" />
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Entradas" HeaderStyle-Width="90px">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litEntrada" runat="server" Text=''></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Saídas" HeaderStyle-Width="90px">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litSaida" runat="server" Text=''></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Saldo" HeaderStyle-Width="100px">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litSaldo" runat="server" Text=''></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <div id="accordionB">
                                        <h3>Bonificação</h3>
                                        <div>
                                            <table border="0" cellpadding="0" class="tb" width="100%">
                                                <tr>
                                                    <td style="width: 100%;">
                                                        <div class="rounded_corners">
                                                            <asp:GridView ID="gvBonificacao" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvBonificacao_RowDataBound"
                                                                OnDataBound="gvBonificacao_DataBound"
                                                                ShowFooter="true">
                                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                        HeaderText="Data" HeaderStyle-Width="184px">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litData" runat="server" Text=''></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="LANCAMENTO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                        HeaderText="Lançamento" HeaderStyle-Width="90px" />
                                                                    <asp:BoundField DataField="REPRESENTANTE" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Representante" HeaderStyle-Width="230px" />
                                                                    <asp:BoundField DataField="NATUREZA" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Natureza" />
                                                                    <asp:BoundField DataField="NOTA_FISCAL" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Nota Fiscal" HeaderStyle-Width="110px" />
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Entradas" HeaderStyle-Width="90px">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litEntrada" runat="server" Text=''></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Saídas" HeaderStyle-Width="90px">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litSaida" runat="server" Text=''></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Saldo" HeaderStyle-Width="100px">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litSaldo" runat="server" Text=''></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </fieldset>
                                <fieldset>
                                    <legend>Financeiro</legend>
                                    <div id="accordionR">
                                        <h3>Recebimento</h3>
                                        <div>
                                            <table border="0" cellpadding="0" class="tb" width="100%">
                                                <tr>
                                                    <td style="width: 100%;">
                                                        <div class="rounded_corners">
                                                            <asp:GridView ID="gvRecebimento" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvRecebimento_RowDataBound"
                                                                OnDataBound="gvRecebimento_DataBound"
                                                                ShowFooter="true">
                                                                <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                                <FooterStyle BackColor="Gainsboro" HorizontalAlign="Center" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                        HeaderText="Data" HeaderStyle-Width="110px">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litData" runat="server" Text=''></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="LANCAMENTO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                        HeaderText="Lançamento" HeaderStyle-Width="110px" />
                                                                    <asp:BoundField DataField="REPRESENTANTE" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Representante" HeaderStyle-Width="230px" />
                                                                    <asp:BoundField DataField="NATUREZA" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Natureza" />
                                                                    <asp:BoundField DataField="NOTA_FISCAL" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Nota Fiscal" HeaderStyle-Width="115px" />
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Entradas" HeaderStyle-Width="90px">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litEntrada" runat="server" Text=''></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Saídas" HeaderStyle-Width="90px">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litSaida" runat="server" Text=''></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Saldo" HeaderStyle-Width="100px">
                                                                        <ItemTemplate>
                                                                            <asp:Literal ID="litSaldo" runat="server" Text=''></asp:Literal>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
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
