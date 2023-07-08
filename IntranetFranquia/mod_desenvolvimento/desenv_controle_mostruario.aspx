<%@ Page Title="Controle de Mostruário" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="desenv_controle_mostruario.aspx.cs" Inherits="Relatorios.desenv_controle_mostruario" EnableEventValidation="false"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btExcel" />
        </Triggers>
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Desenvolvimento&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Mostruário</span>
                <div style="float: right; padding: 0;">
                    <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="labTitulo" runat="server" Text="Controle de Mostruário"></asp:Label></legend>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Coleção
                        </td>
                        <td>Origem
                        </td>
                        <td>Grupo
                        </td>
                        <td>Produto
                        </td>
                        <td>Nome
                        </td>
                        <td>Fabricante
                        </td>
                        <td>Griffe
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 200px;" valign="top">
                            <asp:DropDownList ID="ddlColecoes" runat="server" Width="194px" Height="21px" DataTextField="DESC_COLECAO"
                                DataValueField="COLECAO" OnSelectedIndexChanged="ddlColecoes_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 180px;" valign="top">
                            <asp:DropDownList ID="ddlOrigem" runat="server" Width="174px" Height="21px" DataTextField="DESCRICAO"
                                DataValueField="CODIGO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 170px;" valign="top">
                            <asp:DropDownList ID="ddlGrupo" runat="server" Width="164px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                DataValueField="GRUPO_PRODUTO">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 150px;" valign="top">
                            <asp:TextBox ID="txtProduto" runat="server" Width="140px" MaxLength="10"></asp:TextBox>
                        </td>
                        <td style="width: 210px;" valign="top">
                            <asp:TextBox ID="txtNome" runat="server" Width="200px" MaxLength="20"></asp:TextBox>
                        </td>
                        <td style="width: 160px;" valign="top">
                            <asp:DropDownList ID="ddlFabricante" runat="server" Width="154px" Height="21px" DataTextField="FABRICANTE"
                                DataValueField="FABRICANTE">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlGriffe" runat="server" Width="170px" Height="21px" DataTextField="GRIFFE"
                                DataValueField="GRIFFE">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <asp:Button ID="btBuscar" runat="server" Text="Buscar" Width="92px" OnClick="btBuscar_Click" OnClientClick="DesabilitarBotao(this);" />&nbsp;&nbsp;
                            <asp:Button ID="btExcel" runat="server" Width="92px" Text="Excel" OnClick="btExcel_Click" />&nbsp;
                            <asp:CheckBox ID="cbExcel" runat="server" OnCheckedChanged="cbExcel_CheckedChanged" AutoPostBack="true" />&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="labErro" runat="server" ForeColor="Red" Font-Bold="true" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>Total de Modelos:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="labModeloFiltroValor"
                            ForeColor="Green" Font-Bold="true" runat="server" Text="0"></asp:Label>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td colspan="5">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>Total de SKUs:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                            ID="labEstiloFiltroValor" runat="server" ForeColor="Green" Font-Bold="true" Text="0"></asp:Label>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td colspan="5">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 28px;">&nbsp;
                                    </td>
                                    <td style="width: 100px;">&nbsp;
                                    </td>
                                    <td style="width: 200px;">&nbsp;
                                    </td>
                                    <td style="width: 268px;">&nbsp;
                                    </td>
                                    <td style="width: 99px;">
                                        <asp:DropDownList ID="ddlFiltroFabricante" runat="server" DataTextField="FABRICANTE" DataValueField="FABRICANTE" Width="99px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 103px;">
                                        <asp:DropDownList ID="ddlFiltroHB" runat="server" DataTextField="" DataValueField="" Width="103px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                            <asp:ListItem Value="0" Text="NOK"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="OK"></asp:ListItem>
                                            <asp:ListItem Value="9999" Text="CARTELINHA"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 103px;">
                                        <asp:DropDownList ID="ddlFiltroFaccao" runat="server" DataTextField="" DataValueField="" Width="103px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                            <asp:ListItem Value="0" Text="NOK"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="OK"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 102px;">
                                        <asp:DropDownList ID="ddlFiltroCusto" runat="server" DataTextField="" DataValueField="" Width="102px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                            <asp:ListItem Value="0" Text="NOK"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="OK"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 104px;">
                                        <asp:DropDownList ID="ddlFiltroFaturado" runat="server" DataTextField="" DataValueField="" Width="104px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                            <asp:ListItem Value="0" Text="NÃO"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="SIM"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 104px;">
                                        <asp:DropDownList ID="ddlFiltroPedido" runat="server" DataTextField="" DataValueField="" Width="104px" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                            <asp:ListItem Value="0" Text="NOK"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="OK"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="11">
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvControleMostruario" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvControleMostruario_RowDataBound" ShowFooter="true"
                                                AllowSorting="true" OnSorting="gvControleMostruario_Sorting" DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                                <FooterStyle BackColor="GradientActiveCaption" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Produto" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                        SortExpression="PRODUTO">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litProduto" runat="server" Text='<%# Bind("PRODUTO")%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Nome" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left"
                                                        SortExpression="NOME">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litNome" runat="server" Text='<%# Bind("NOME")%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Cor" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left"
                                                        SortExpression="COR">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litCor" runat="server" Text='<%# Bind("COR")%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Griffe" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left"
                                                        SortExpression="GRIFFE">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litGriffe" runat="server" Text='<%# Bind("GRIFFE")%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Fabricante" ItemStyle-HorizontalAlign="Left"
                                                        SortExpression="FABRICANTE">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litFabricante" runat="server" Text='<%# Bind("FABRICANTE")%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="HB" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                        SortExpression="HB">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litHB" runat="server" Text='<%# Bind("HB")%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Facção" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                        SortExpression="FACCAO">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litFaccao" runat="server" Text='<%# Bind("FACCAO")%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Custo" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                        SortExpression="CUSTO">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litCusto" runat="server" Text='<%# Bind("CUSTO")%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Faturado" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                        SortExpression="FATURADO">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litFaturado" runat="server" Text='<%# Bind("FATURADO")%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pedido" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                        SortExpression="PEDIDO_COMPRA">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litPedido" runat="server" Text='<%# Bind("PEDIDO_COMPRA")%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Observação" HeaderStyle-Width="180px" ItemStyle-Width="180px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        ItemStyle-Font-Size="Smaller" ItemStyle-CssClass="corTD" FooterStyle-HorizontalAlign="Center" SortExpression="OBS_MOSTRUARIO">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtObs" runat="server" Width="180px" Height="14px" OnTextChanged="txtObs_TextChanged" AutoPostBack="true"
                                                                Visible='<%# !cbExcel.Checked %>'></asp:TextBox>
                                                            <asp:Literal ID="litObs" runat="server" Text='<%# Bind("OBS_MOSTRUARIO") %>'
                                                                Visible='<%# cbExcel.Checked %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Ok" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                        SortExpression="MOSTRUARIO_OK">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtMarcar" runat="server" Width="19px" ImageUrl="~/Image/approve.png"
                                                                ToolTip="Mostruário Ok" OnClick="ibtMarcar_Click" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
