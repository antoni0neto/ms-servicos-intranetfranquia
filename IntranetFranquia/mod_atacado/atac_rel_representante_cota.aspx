<%@ Page Title="Histórico de Clientes/Cota" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="atac_rel_representante_cota.aspx.cs" Inherits="Relatorios.atac_rel_representante_cota" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .alinharCentro {
            text-align: center;
        }

        .corTD {
            background-color: #EFEFEF;
        }

        .pageStyl a, .pageStyl span {
            display: block;
            height: 15px;
            width: 15px;
            font-weight: bold;
            text-align: center;
            text-decoration: none;
        }

        .pageStyl a {
            background-color: #f5f5f5;
            color: #969696;
            border: 1px solid #969696;
        }

        .pageStyl span {
            background-color: #A1DCF2;
            color: #000;
            border: 1px solid #3AC0F2;
        }
    </style>
    <link href="../Styles/gridviewScroll.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/1.9.0/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.9.1/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../js/js.js"></script>
    <script type="text/javascript" src="../js/gridviewScroll.min.js"></script>

    <script type="text/javascript">

        function gridviewScroll() {

            gvAtacado = $('#MainContent_gvClienteColecao').gridviewScroll({
                width: 1430,
                height: 550,
                railcolor: "#F0F0F0",
                barcolor: "#CDCDCD",
                barhovercolor: "#606060",
                bgcolor: "#F0F0F0",
                freezesize: 8,
                arrowsize: 30,
                varrowtopimg: "../Image/arrowvt.png",
                varrowbottomimg: "../Image/arrowvb.png",
                harrowleftimg: "../Image/arrowhl.png",
                harrowrightimg: "../Image/arrowhr.png",
                headerrowcount: 1,
                railsize: 16,
                barsize: 12
            });
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo do Atacado&nbsp;&nbsp;>&nbsp;&nbsp;
                    Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;
                    Histórico de Clientes/Cota</span>
        <div style="float: right; padding: 0;">
            <a href="atac_menu.aspx" class="alink" title="Voltar">Voltar</a>
        </div>

    </div>
    <hr />

    <div class="login">
        <fieldset>
            <legend>Histórico de Clientes/Cota</legend>
            <asp:UpdatePanel ID="updClientes" UpdateMode="Always" runat="server">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btExcel" />
                </Triggers>
                <ContentTemplate>
                    <asp:Panel ID="pnlHistorico" runat="server" DefaultButton="btBuscar">
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="labColecao" runat="server" Text="Coleção"></asp:Label>
                                </td>
                                <td colspan="2">Representante
                                </td>
                                <td>Nome do Cliente
                                </td>
                                <td>UF
                                </td>
                                <td>Nota
                                </td>
                                <td>Crédito
                                </td>
                                <td>Cota
                                </td>
                                <td>Comprou Verão
                                </td>
                                <td>Comprou Primavera
                                </td>
                                <td>Comprou Inverno
                                </td>
                                <td>Comprou Outono
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="width: 200px;">
                                    <asp:DropDownList runat="server" ID="ddlColecao" DataValueField="COLECAO" DataTextField="DESC_COLECAO" Height="22px"
                                        Width="194px">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2" style="width: 200px;">
                                    <asp:DropDownList runat="server" ID="ddlRepresentante" DataValueField="REPRESENTANTE" DataTextField="REPRESENTANTE" Height="22px"
                                        Width="194px">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 200px;">
                                    <asp:TextBox ID="txtNomeCliente" runat="server" CssClass="alinharEsquerda" MaxLength="100" Height="16px" Width="190px"></asp:TextBox>
                                </td>
                                <td style="width: 80px;">
                                    <asp:DropDownList ID="ddlUF" runat="server" Width="74px" DataValueField="UF" DataTextField="UF" Height="22px"></asp:DropDownList>
                                </td>
                                <td style="width: 80px;">
                                    <asp:DropDownList ID="ddlNota" runat="server" Width="74px" Height="22px">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                        <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                        <asp:ListItem Value="7" Text="7"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 90px;">
                                    <asp:DropDownList ID="ddlCredito" runat="server" Width="84px" Height="22px">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="SIM" Text="SIM"></asp:ListItem>
                                        <asp:ListItem Value="NÃO" Text="NÃO"></asp:ListItem>
                                        <asp:ListItem Value="VAZIO" Text="VAZIO"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 100px;">
                                    <asp:DropDownList ID="ddlCota" runat="server" Width="94px" Height="22px">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="S" Text="Tem Cota"></asp:ListItem>
                                        <asp:ListItem Value="N" Text="Não Tem Cota"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 120px;">
                                    <asp:DropDownList ID="ddlComprouVerao" runat="server" Width="114px" Height="22px">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                        <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 120px;">
                                    <asp:DropDownList ID="ddlComprouPrimavera" runat="server" Width="114px" Height="22px">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                        <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 120px;">
                                    <asp:DropDownList ID="ddlComprouInverno" runat="server" Width="114px" Height="22px">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                        <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlComprouOutono" runat="server" Width="114px" Height="22px">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                        <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Editar Cotas
                                </td>
                                <td>
                                    <asp:Label ID="labPrecoMedio" runat="server" Text="Preço Médio" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labPorcAumentoCota" runat="server" Text="% de Aumento Cota" Visible="false"></asp:Label></td>
                                <td>&nbsp;
                                </td>
                                <td colspan="9"></td>
                            </tr>
                            <tr>
                                <td style="width: 100px;">
                                    <asp:CheckBox ID="cbEditarCota" runat="server" Checked="false" OnCheckedChanged="cbEditarCota_CheckedChanged" AutoPostBack="true" />
                                </td>
                                <td style="width: 100px;">
                                    <asp:TextBox ID="txtPrecoMedio" runat="server" CssClass="alinharDireita" Width="90px"
                                        onkeypress="return fnValidarNumeroDecimal(event);" Visible="false"></asp:TextBox>
                                </td>
                                <td style="width: 150px;">
                                    <asp:TextBox ID="txtPorcAumentoCota" runat="server" CssClass="alinharDireita" Width="140px"
                                        onkeypress="return fnValidarNumeroDecimalNegativo(event);" Visible="false"></asp:TextBox>
                                </td>
                                <td style="width: 50px;">
                                    <asp:ImageButton ID="imgAtualizarCota" runat="server" ImageUrl="~/Image/update.png" Width="18px" ToolTip="Atualizar Cota" OnClick="imgAtualizarCota_Click" Visible="false"
                                        OnClientClick="return Confirmar('Você irá atualizar todas as cotas do filtro selecionado. Deseja continuar?');" />
                                </td>
                                <td>
                                    <asp:Button ID="btCopiarSugestaoCota" runat="server" Text="Copiar Sugestão de Cota" Width="200px" OnClick="btCopiarSugestaoCota_Click"
                                        OnClientClick="return Confirmar('Você irá copiar todas as Sugestões de Cota do filtro selecionado. Deseja continuar?');" Visible="false" />
                                </td>
                                <td colspan="8" style="text-align: right;">
                                    <asp:Label ID="labErro" runat="server" ForeColor="Red" Text=""></asp:Label>&nbsp;&nbsp;
                                    <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="90px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;
                                <asp:Button runat="server" ID="btExcel" Text="Excel" Width="90px" OnClick="btExcel_Click" ToolTip="Extrair as informações para Excel." />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="13">
                                    <asp:HiddenField ID="hidSortExpression" runat="server" Value="" />
                                    <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <br />
                    <div>
                        <div class="rounded_corners">
                            <div style="text-align: right; color: red;">
                                <i>Página <%=gvClienteColecao.PageIndex + 1%> de <%=gvClienteColecao.PageCount%></i>&nbsp;&nbsp;&nbsp;
                            </div>
                            <asp:GridView ID="gvClienteColecao" runat="server" AutoGenerateColumns="False" ShowFooter="true"
                                ForeColor="#333333" Style="background: white; width: 3900px; border-collapse: collapse;"
                                OnPageIndexChanging="gvClienteColecao_PageIndexChanging" AllowPaging="true" PageSize="20"
                                OnRowDataBound="gvClienteColecao_RowDataBound" OnSorting="gvClienteColecao_Sorting" AllowSorting="true"
                                OnDataBound="gvClienteColecao_DataBound">
                                <HeaderStyle BackColor="GradientActiveCaption"></HeaderStyle>
                                <FooterStyle BackColor="GradientActiveCaption" Font-Bold="true" />
                                <PagerStyle HorizontalAlign="Center" CssClass="pageStyl" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="corTD">
                                        <ItemTemplate>
                                            <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="REPRESENTANTE_UF" HeaderText="Representante" SortExpression="REPRESENTANTE_UF" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-CssClass="corTD" ItemStyle-Font-Size="Smaller" Visible="false" />
                                    <asp:TemplateField HeaderText="Cliente" HeaderStyle-Width="180px" ItemStyle-Width="180px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                        ItemStyle-CssClass="corTD" ItemStyle-Font-Size="Smaller" SortExpression="CLIENTE">
                                        <ItemTemplate>
                                            <asp:Label ID="labCliente" Text='<%# Bind("CLIENTE") %>' runat="server" ToolTip='<%# Bind("REPRESENTANTE_UF") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="MUNICIPIO" HeaderText="Município" SortExpression="MUNICIPIO" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="145px" ItemStyle-Width="145px" ItemStyle-CssClass="corTD" ItemStyle-Font-Size="Smaller" />
                                    <asp:BoundField DataField="UF" HeaderText="UF" SortExpression="UF" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="55px" ItemStyle-Width="55px" ItemStyle-CssClass="corTD" ItemStyle-Font-Size="Smaller" />
                                    <asp:BoundField DataField="NOTA_CIDADE" HeaderText="Nota" SortExpression="NOTA_CIDADE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-CssClass="corTD" ItemStyle-Font-Size="Smaller" />
                                    <asp:BoundField DataField="CREDITO" HeaderText="Crédito" SortExpression="CREDITO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-Font-Size="Smaller" ItemStyle-CssClass="corTD" />

                                    <asp:TemplateField HeaderText="Cota Qtde" HeaderStyle-Width="80px" ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-Font-Size="Smaller" ItemStyle-CssClass="corTD" FooterStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCotaQtde" runat="server" Width="80px" Height="10px" Text='<%# Bind("COTA_QTDE_COL_ATUAL") %>' Style='text-align: center;'
                                                onkeypress="return fnValidarNumero(event);" OnTextChanged="txtCotaQtde_TextChanged" AutoPostBack="true"
                                                Visible='<%# Eval("CLIENTE") != "" && cbEditarCota.Checked %>'></asp:TextBox>
                                            <asp:Literal ID="litCotaQtde" runat="server" Text='<%# Bind("COTA_QTDE_COL_ATUAL") %>'
                                                Visible='<%# !cbEditarCota.Checked %>'></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cota Valor" HeaderStyle-Width="120px" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                        ItemStyle-Font-Size="Smaller" ItemStyle-CssClass="corTD">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCotaValor" runat="server" Width="119px" Height="10px" Text='<%# Bind("COTA_VALOR_COL_ATUAL") %>' Style='text-align: left;'
                                                onkeypress="return fnValidarNumeroDecimal(event);" OnTextChanged="txtCotaValor_TextChanged" AutoPostBack="true"
                                                Visible='<%# Eval("CLIENTE") != "" && cbEditarCota.Checked %>'></asp:TextBox>
                                            <asp:Literal ID="litCotaValor" runat="server" Text='<%# "R$ " + Eval("COTA_VALOR_COL_ATUAL") %>'
                                                Visible='<%# !cbEditarCota.Checked %>'></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="COTA_VAL_SUGESTAO" HeaderText="Sugestão de Cota" SortExpression="COTA_VAL_SUGESTAO" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="140px" ItemStyle-Width="140px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" />
                                    
                                    <asp:BoundField DataField="TOT_QTDE_ORIGINAL_COLATUAL" HeaderText="Qtde Col Atual" SortExpression="TOT_QTDE_ORIGINAL_COLATUAL" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="150px" ItemStyle-Width="150px" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#98FB98"  />
                                    <asp:BoundField DataField="TOT_VALOR_ORIGINAL_COLATUAL" HeaderText="Valor Col Atual" SortExpression="TOT_VALOR_ORIGINAL_COLATUAL" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="160px" ItemStyle-Width="160px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#98FB98" />

                                    <asp:BoundField DataField="MEDIA_VAL_A" HeaderText="Val. Médio A. Verão" SortExpression="MEDIA_VAL_A" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="160px" ItemStyle-Width="160px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#99ccff" />
                                    <asp:BoundField DataField="MEDIA_VAL_P" HeaderText="Val. Médio Primavera" SortExpression="MEDIA_VAL_P" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="160px" ItemStyle-Width="160px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#99ccff" />
                                    <asp:BoundField DataField="MEDIA_VAL_AP" HeaderText="Val. Médio Verão" SortExpression="MEDIA_VAL_AP" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="160px" ItemStyle-Width="160px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#99ccff" />

                                    <asp:BoundField DataField="MEDIA_VAL_W" HeaderText="Val. Médio A. Inverno" SortExpression="MEDIA_VAL_W" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="160px" ItemStyle-Width="160px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#ccffff" />
                                    <asp:BoundField DataField="MEDIA_VAL_U" HeaderText="Val. Médio Outono" SortExpression="MEDIA_VAL_U" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="160px" ItemStyle-Width="160px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#ccffff" />
                                    <asp:BoundField DataField="MEDIA_VAL_WU" HeaderText="Val. Médio Inverno" SortExpression="MEDIA_VAL_WU" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="160px" ItemStyle-Width="160px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#ccffff" />

                                    <asp:BoundField DataField="MEDIA_VAL_TOT" HeaderText="Val. Médio Total" SortExpression="MEDIA_VAL_TOT" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="160px" ItemStyle-Width="160px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#6699ff" />

                                    <asp:BoundField DataField="QTDE_COLECAO_1" HeaderText="No. Coleções Verão" SortExpression="QTDE_COLECAO_1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="150px" ItemStyle-Width="150px" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#ffffcc" />
                                    <asp:BoundField DataField="TOT_QTDE_ORIGINAL_MEDIA_1" HeaderText="Qtde Média Verão" SortExpression="TOT_QTDE_ORIGINAL_MEDIA_1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="185px" ItemStyle-Width="185px" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#ffffcc" />
                                    <asp:BoundField DataField="TOT_VALOR_ORIGINAL_MEDIA_1" HeaderText="Val. Médio Verão" SortExpression="TOT_VALOR_ORIGINAL_MEDIA_1" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="190px" ItemStyle-Width="190px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#ffffcc" />

                                    <asp:BoundField DataField="QTDE_COLECAO_2" HeaderText="No. Coleções Primavera" SortExpression="QTDE_COLECAO_2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="165px" ItemStyle-Width="165px" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#ffccff" />
                                    <asp:BoundField DataField="TOT_QTDE_ORIGINAL_MEDIA_2" HeaderText="Qtde Média Primavera" SortExpression="TOT_QTDE_ORIGINAL_MEDIA_2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="185px" ItemStyle-Width="185px" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#ffccff" />
                                    <asp:BoundField DataField="TOT_VALOR_ORIGINAL_MEDIA_2" HeaderText="Val. Médio Primavera" SortExpression="TOT_VALOR_ORIGINAL_MEDIA_2" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="190px" ItemStyle-Width="190px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#ffccff" />

                                    <asp:BoundField DataField="QTDE_COLECAO_3" HeaderText="No. Coleções Inverno" SortExpression="QTDE_COLECAO_3" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="150px" ItemStyle-Width="150px" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#ccffcc" />
                                    <asp:BoundField DataField="TOT_QTDE_ORIGINAL_MEDIA_3" HeaderText="Qtde Média Inverno" SortExpression="TOT_QTDE_ORIGINAL_MEDIA_3" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="185px" ItemStyle-Width="185px" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#ccffcc" />
                                    <asp:BoundField DataField="TOT_VALOR_ORIGINAL_MEDIA_3" HeaderText="Val. Médio Inverno" SortExpression="TOT_VALOR_ORIGINAL_MEDIA_3" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="190px" ItemStyle-Width="190px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#ccffcc" />

                                    <asp:BoundField DataField="QTDE_COLECAO_4" HeaderText="No. Coleções Outono" SortExpression="QTDE_COLECAO_4" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="150px" ItemStyle-Width="150px" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#ffcccc" />
                                    <asp:BoundField DataField="TOT_QTDE_ORIGINAL_MEDIA_4" HeaderText="Qtde Média Outono" SortExpression="TOT_QTDE_ORIGINAL_MEDIA_4" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="185px" ItemStyle-Width="185px" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#ffcccc" />
                                    <asp:BoundField DataField="TOT_VALOR_ORIGINAL_MEDIA_4" HeaderText="Val. Médio Outono" SortExpression="TOT_VALOR_ORIGINAL_MEDIA_4" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="190px" ItemStyle-Width="190px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#ffcccc" />

                                    <asp:BoundField DataField="TOT_QTDE_ORIGINAL_COL4" HeaderText="QtdeOrigCol4" SortExpression="TOT_QTDE_ORIGINAL_COL4" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="235px" ItemStyle-Width="235px" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#efefef" />
                                    <asp:BoundField DataField="TOT_VALOR_ORIGINAL_COL4" HeaderText="ValOrigCol4" SortExpression="TOT_VALOR_ORIGINAL_COL4" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="235px" ItemStyle-Width="235px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#efefef" />
                                    <asp:BoundField DataField="TOT_QTDE_ENTREGUE_COL4" HeaderText="QtdeEntCol4" SortExpression="TOT_QTDE_ENTREGUE_COL4" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="235px" ItemStyle-Width="235px" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#efefef" />
                                    <asp:BoundField DataField="TOT_VALOR_ENTREGUE_COL4" HeaderText="ValEntCol4" SortExpression="TOT_VALOR_ENTREGUE_COL4" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="235px" ItemStyle-Width="235px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#efefef" />
                                    <asp:BoundField DataField="TOT_QTDE_CANCELADA_COL4" HeaderText="QtdeCancCol4" SortExpression="TOT_QTDE_CANCELADA_COL4" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="235px" ItemStyle-Width="235px" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#efefef" />
                                    <asp:BoundField DataField="TOT_VALOR_CANCELADO_COL4" HeaderText="ValCancCol4" SortExpression="TOT_VALOR_CANCELADO_COL4" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="235px" ItemStyle-Width="235px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#efefef" />

                                    <asp:BoundField DataField="TOT_QTDE_ORIGINAL_COL3" HeaderText="QtdeOrigCol3" SortExpression="TOT_QTDE_ORIGINAL_COL3" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="235px" ItemStyle-Width="235px" ItemStyle-Font-Size="Smaller" />
                                    <asp:BoundField DataField="TOT_VALOR_ORIGINAL_COL3" HeaderText="ValOrigCol3" SortExpression="TOT_VALOR_ORIGINAL_COL3" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="235px" ItemStyle-Width="235px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" />
                                    <asp:BoundField DataField="TOT_QTDE_ENTREGUE_COL3" HeaderText="QtdeEntCol3" SortExpression="TOT_QTDE_ENTREGUE_COL3" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="235px" ItemStyle-Width="235px" ItemStyle-Font-Size="Smaller" />
                                    <asp:BoundField DataField="TOT_VALOR_ENTREGUE_COL3" HeaderText="ValEntCol3" SortExpression="TOT_VALOR_ENTREGUE_COL3" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="235px" ItemStyle-Width="235px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" />
                                    <asp:BoundField DataField="TOT_QTDE_CANCELADA_COL3" HeaderText="QtdeCancCol3" SortExpression="TOT_QTDE_CANCELADA_COL3" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="235px" ItemStyle-Width="235px" ItemStyle-Font-Size="Smaller" />
                                    <asp:BoundField DataField="TOT_VALOR_CANCELADO_COL3" HeaderText="ValCancCol3" SortExpression="TOT_VALOR_CANCELADO_COL3" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="235px" ItemStyle-Width="235px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" />


                                    <asp:BoundField DataField="TOT_QTDE_ORIGINAL_COL2" HeaderText="QtdeOrigCol2" SortExpression="TOT_QTDE_ORIGINAL_COL2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="235px" ItemStyle-Width="235px" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#efefef" />
                                    <asp:BoundField DataField="TOT_VALOR_ORIGINAL_COL2" HeaderText="ValOrigCol2" SortExpression="TOT_VALOR_ORIGINAL_COL2" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="235px" ItemStyle-Width="235px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#efefef" />
                                    <asp:BoundField DataField="TOT_QTDE_ENTREGUE_COL2" HeaderText="QtdeEntCol2" SortExpression="TOT_QTDE_ENTREGUE_COL2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="235px" ItemStyle-Width="235px" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#efefef" />
                                    <asp:BoundField DataField="TOT_VALOR_ENTREGUE_COL2" HeaderText="ValEntCol2" SortExpression="TOT_VALOR_ENTREGUE_COL2" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="235px" ItemStyle-Width="235px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#efefef" />
                                    <asp:BoundField DataField="TOT_QTDE_CANCELADA_COL2" HeaderText="QtdeCancCol2" SortExpression="TOT_QTDE_CANCELADA_COL2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="235px" ItemStyle-Width="235px" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#efefef" />
                                    <asp:BoundField DataField="TOT_VALOR_CANCELADO_COL2" HeaderText="ValCancCol2" SortExpression="TOT_VALOR_CANCELADO_COL2" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="235px" ItemStyle-Width="235px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" ItemStyle-BackColor="#efefef" />

                                    <asp:BoundField DataField="TOT_QTDE_ORIGINAL_COL1" HeaderText="QtdeOrigCol1" SortExpression="TOT_QTDE_ORIGINAL_COL1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="235px" ItemStyle-Width="235px" ItemStyle-Font-Size="Smaller" />
                                    <asp:BoundField DataField="TOT_VALOR_ORIGINAL_COL1" HeaderText="ValOrigCol1" SortExpression="TOT_VALOR_ORIGINAL_COL1" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="235px" ItemStyle-Width="235px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" />
                                    <asp:BoundField DataField="TOT_QTDE_ENTREGUE_COL1" HeaderText="QtdeEntCol1" SortExpression="TOT_QTDE_ENTREGUE_COL1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="235px" ItemStyle-Width="235px" ItemStyle-Font-Size="Smaller" />
                                    <asp:BoundField DataField="TOT_VALOR_ENTREGUE_COL1" HeaderText="ValEntCol1" SortExpression="TOT_VALOR_ENTREGUE_COL1" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="235px" ItemStyle-Width="235px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" />
                                    <asp:BoundField DataField="TOT_QTDE_CANCELADA_COL1" HeaderText="QtdeCancCol1" SortExpression="TOT_QTDE_CANCELADA_COL1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="235px" ItemStyle-Width="235px" ItemStyle-Font-Size="Smaller" />
                                    <asp:BoundField DataField="TOT_VALOR_CANCELADO_COL1" HeaderText="ValCancCol1" SortExpression="TOT_VALOR_CANCELADO_COL1" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="235px" ItemStyle-Width="235px" DataFormatString="{0:c}" ItemStyle-Font-Size="Smaller" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
    </div>
</asp:Content>
