<%@ Page Title="Seguros" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="seg_rel_seguro.aspx.cs" Inherits="Relatorios.seg_rel_seguro"
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
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Seguros&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios&nbsp;&nbsp;>&nbsp;&nbsp;Seguros</span>
                <div style="float: right; padding: 0;">
                    <a href="seg_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 100%;">
                <fieldset>
                    <legend>
                        <asp:Label ID="labTitulo" runat="server" Text="Seguros"></asp:Label></legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>Item
                            </td>
                            <td>Empresa
                            </td>
                            <td>Tipo de Seguro
                            </td>
                            <td>Apolice/Proposta
                            </td>
                            <td>Seguradora
                            </td>
                            <td>Corretor
                            </td>
                            <td>Vencimento Inicial
                            </td>
                            <td>Vencimento Final
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 180px;">
                                <asp:DropDownList runat="server" ID="ddlItem" DataValueField="CODIGO" DataTextField="ITEM"
                                    Height="22px" Width="174px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 178px;">
                                <asp:DropDownList runat="server" ID="ddlEmpresa" DataValueField="CODIGO_EMPRESA" DataTextField="NOME"
                                    Height="22px" Width="172px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 210px;">
                                <asp:DropDownList runat="server" ID="ddlTipoSeguro" DataValueField="CODIGO" DataTextField="DESCRICAO"
                                    Height="22px" Width="204px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 170px;">
                                <asp:TextBox ID="txtApoliceProposta" runat="server" Width="160px"></asp:TextBox>
                            </td>
                            <td style="width: 240px;">
                                <asp:DropDownList runat="server" ID="ddlSeguradora" DataValueField="CODIGO" DataTextField="DESCRICAO"
                                    Height="22px" Width="234px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 200px;">
                                <asp:DropDownList runat="server" ID="ddlCorretor" DataValueField="CODIGO" DataTextField="DESCRICAO"
                                    Height="22px" Width="194px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 130px;">
                                <asp:TextBox ID="txtVencimentoIni" runat="server" Width="120px" MaxLength="10" Style="text-align: right;"
                                    onkeypress="return fnValidarData(event);"></asp:TextBox>
                            </td>
                            <td style="width: 130px;">
                                <asp:TextBox ID="txtVencimentoFim" runat="server" Width="120px" MaxLength="10" Style="text-align: right;"
                                    onkeypress="return fnValidarData(event);"></asp:TextBox>
                            </td>

                        </tr>
                        <tr>
                            <td colspan="8">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" OnClick="btBuscar_Click" Text="Buscar" Width="100px" />
                            </td>
                            <td colspan="7">
                                <asp:Label ID="labErro" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td colspan="8">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvSeguro" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                        Style="background: white" OnRowDataBound="gvSeguro_RowDataBound" ShowFooter="true">
                                        <HeaderStyle BackColor="Gainsboro" />
                                        <FooterStyle BackColor="Gainsboro" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ITEM" HeaderText="Item" ItemStyle-Font-Size="Smaller" />
                                            <asp:BoundField DataField="EMPRESA" HeaderText="Empresa" ItemStyle-Font-Size="Smaller" />
                                            <asp:BoundField DataField="TIPO_SEGURO" HeaderText="Tipo de Seguro" ItemStyle-Font-Size="Smaller" />

                                            <asp:BoundField DataField="APOLPROPOSTA" HeaderText="Apólice/Proposta" HeaderStyle-Width="" ItemStyle-Font-Size="Smaller" />
                                            <asp:BoundField DataField="SEGURADORA" HeaderText="Seguradora" HeaderStyle-Width="" ItemStyle-Font-Size="Smaller" />
                                            <asp:BoundField DataField="CORRETOR" HeaderText="Corretor" HeaderStyle-Width="" ItemStyle-Font-Size="Smaller" />

                                            <asp:TemplateField HeaderText="Vencimento" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litVencimento" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Cobertura" HeaderStyle-Width="130px" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litCobertura" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="OBSERVACAO" HeaderText="Observação" HeaderStyle-Width="130px" ItemStyle-Font-Size="Smaller" />

                                            <asp:TemplateField HeaderText="Valor" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litValor" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-Width="15px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btAbrir" runat="server" Height="10px" Width="10px" ImageUrl="~/Image/search.png"
                                                        OnClick="btAbrir_Click" ToolTip="Abrir Seguro" />
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
