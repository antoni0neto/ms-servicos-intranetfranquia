<%@ Page Title="Liberação Financeiro" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="atac_cad_cliente_bloqueio.aspx.cs" Inherits="Relatorios.atac_cad_cliente_bloqueio" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        table {
            width: 100%;
            font-size: 13px;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo do Atacado&nbsp;&nbsp;>&nbsp;&nbsp;<asp:Label ID="labMeioTitulo" runat="server"></asp:Label>&nbsp;&nbsp;>&nbsp;&nbsp;<asp:Label ID="labTitulo" runat="server"></asp:Label></span>
                <div style="float: right; padding: 0;">
                    <a href="atac_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>
                        <asp:Label ID="labSubTitulo" runat="server"></asp:Label></legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td colspan="9">Coleção
                            </td>
                        </tr>
                        <tr>
                            <td colspan="9">
                                <asp:DropDownList runat="server" ID="ddlColecao" DataValueField="COLECAO" DataTextField="DESC_COLECAO" Height="22px"
                                    Width="154px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Clifor
                            </td>
                            <td>Nome Cliente</td>
                            <td>CNPJ</td>
                            <td>UF</td>
                            <td>Faturamento Bloqueado</td>
                            <td>Sem Crédito</td>
                            <td>Tipo Bloqueio</td>
                            <td>Cliente Verificado</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtCodClifor" runat="server" Width="150px" MaxLength="8" CssClass="alinharDireita"
                                    onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                            </td>
                            <td style="width: 220px;">
                                <asp:TextBox ID="txtNomeCliente" runat="server" Width="210px"></asp:TextBox>
                            </td>
                            <td style="width: 180px;">
                                <asp:TextBox ID="txtCNPJ" runat="server" Width="170px" onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                            </td>
                            <td style="width: 80px;">
                                <asp:DropDownList ID="ddlUF" runat="server" Width="74px" DataValueField="UF" DataTextField="UF" Height="22px"></asp:DropDownList>
                            </td>
                            <td style="width: 180px;">
                                <asp:DropDownList ID="ddlFatBloqueado" runat="server" Width="174px" Height="22px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 150px;">
                                <asp:DropDownList ID="ddlSemCredito" runat="server" Width="144px" Height="22px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 190px;">
                                <asp:DropDownList ID="ddlTipoBloqueio" runat="server" Width="184px" Height="22px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="CONFERIR" Text="CONFERIR"></asp:ListItem>
                                    <asp:ListItem Value="JURIDICO" Text="JURIDICO"></asp:ListItem>
                                    <asp:ListItem Value="LIBERADO" Text="LIBERADO"></asp:ListItem>
                                    <asp:ListItem Value="LIMITE DE CREDITO" Text="LIMITE DE CREDITO"></asp:ListItem>
                                    <asp:ListItem Value="OBSERVACAO" Text="OBSERVACAO"></asp:ListItem>
                                    <asp:ListItem Value="SEM CREDITO" Text="SEM CREDITO"></asp:ListItem>

                                </asp:DropDownList>
                            </td>
                            <td style="width: 150px;">
                                <asp:DropDownList ID="ddlClienteVerificado" runat="server" Width="144px" Height="22px">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" OnClientClick="DesabilitarBotao(this);" />
                                &nbsp;&nbsp;<asp:Label ID="labMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="9">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="9">
                                <div id="accordionP">
                                    <h3>Clientes</h3>
                                    <div>
                                        <table border="0" cellpadding="0" class="tb" width="100%">
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvCliente" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvCliente_RowDataBound"
                                                            OnDataBound="gvCliente_DataBound" OnSorting="gvCliente_Sorting" AllowSorting="true"
                                                            ShowFooter="true" DataKeyNames="CLIFOR">
                                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                            <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="115px"
                                                                    HeaderText="CLIFOR" SortExpression="CLIFOR">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litClifor" runat="server" Text='<%# Bind("CLIFOR") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Cliente" SortExpression="CLIENTE_ATACADO">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litClienteAtacado" runat="server" Text='<%# Bind("CLIENTE_ATACADO") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="170px"
                                                                    HeaderText="CNPJ" SortExpression="CGC_CPF">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litCNPJ" runat="server" Text=''></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="270px"
                                                                    HeaderText="Representante" SortExpression="REPRESENTANTE">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litDescCondicaoPgto" runat="server" Text='<%# Bind("REPRESENTANTE") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="140px"
                                                                    HeaderText="Sem Crédito" SortExpression="SEM_CREDITO">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litSemCredito" runat="server" Text=''></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"
                                                                    HeaderText="Data Pedido" SortExpression="ULTIMO_PEDIDO_DATA">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litDataPedido" runat="server" Text=''></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px"
                                                                    HeaderText="Último Movimento" SortExpression="ULTIMO_MOV">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litUltimoMov" runat="server" Text=''></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px">
                                                                    <ItemTemplate>
                                                                        <asp:Button ID="btAbrirCliente" runat="server" Text="Abrir" Height="21px" Width="80px" OnClick="btAbrirCliente_Click" />
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
                                <br />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
