<%@ Page Title="Resultado de Peças" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master"
    CodeBehind="cont_resultado.aspx.cs" MaintainScrollPositionOnPostback="true"
    Inherits="Relatorios.cont_resultado" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .bodyResult {
            font-family: "Helvetica Neue", "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif !important;
            margin: 0px;
            padding: 0px;
            color: #696969;
        }

        .jGrowl .manilla {
            background-color: #000;
            color: red;
            font-weight: bold;
        }

        .alinharDireira {
            text-align: right;
        }
    </style>
    <link href="../Styles/jquery.jgrowl.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/jquery.jgrowl.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            $("#tabs").tabs();

        });
        function MarcarAba(vAba) {
            document.getElementById("MainContent_hidTabSelected").value = vAba;
        }

    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Contagem&nbsp;&nbsp;>&nbsp;&nbsp;Contagem&nbsp;&nbsp;>&nbsp;&nbsp;Resultado de Peças</span>
                <div style="float: right; padding: 0;">
                    <a href="cont_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <br />
            <div style="float: left; width: 1000px; margin-left: 15%;">
                <fieldset>
                    <legend>Resultado de Peças</legend>
                    <div style="padding-left: 0px;">
                        <asp:ImageButton ID="ibtPesquisar" runat="server" Width="19px" ImageUrl="~/Image/search.png"
                            ToolTip="Pesquisar" OnClick="ibtPesquisar_Click" OnClientClick="DesabilitarBotao(this);" />
                        <asp:ImageButton ID="ibtLimpar" runat="server" Width="18px" ImageUrl="~/Image/clean.png"
                            ToolTip="Limpar" OnClick="ibtLimpar_Click" />
                        <asp:HiddenField ID="hidTabSelected" runat="server" Value="" />
                        <asp:HiddenField ID="hidAcao" runat="server" Value="" />
                        <div style="text-align: right; float: right;">
                            <asp:Label ID="labErro" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                    <hr />
                    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyResult">
                        <tr>
                            <td>
                                <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="22px" Width="377px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div id="tabs">
                        <ul>
                            <li><a href="#tabs-resultado" id="tabResultado" runat="server" onclick="MarcarAba(0);">Resultado</a></li>
                        </ul>
                        <div id="tabs-resultado">
                            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyResult">
                                <tr>
                                    <td>Data
                                    </td>
                                    <td>Contagem de Peças
                                    </td>
                                    <td>% Valor Aceitável
                                    </td>
                                    <td>Gerente
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 260px;">
                                        <asp:DropDownList runat="server" ID="ddlDataContagem" DataValueField="Value" DataTextField="Text"
                                            Height="23px" Width="254px">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 210px;">
                                        <asp:TextBox ID="txtContagem" runat="server" CssClass="alinharDireira" Width="200px" onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                                    </td>
                                    <td style="width: 140px;">
                                        <asp:TextBox ID="txtPorcValorAceitavel" runat="server" CssClass="alinharDireira" Text="0,50" Width="130px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                    </td>
                                    <td style="width: 210px;">
                                        <asp:TextBox ID="txtGerente" runat="server" Width="200px" MaxLength="29"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Button ID="btIncluir" runat="server" Text="Incluir" OnClick="btIncluir_Click" Width="100px"
                                            OnClientClick="DesabilitarBotao(this);" />
                                    </td>
                                </tr>
                            </table>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0" class="bodyResult">
                                <tr>
                                    <td>
                                        <div>
                                            <fieldset>
                                                <legend>Resultado de Contagem</legend>
                                                <div class="rounded_corners">
                                                    <asp:GridView ID="gvResultadoContagem" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvResultadoContagem_RowDataBound"
                                                        ShowFooter="true">
                                                        <HeaderStyle BackColor="Gainsboro" HorizontalAlign="Center"></HeaderStyle>
                                                        <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Data" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litData" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Última Contagem" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litDiaUltimaContagem" runat="server"></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Valor Aceitável" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litValorAceitavel" runat="server" Text='<%# Bind("VALOR_ACEITAVEL") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Resultado de Peças" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litResultadoPeca" runat="server" Text='<%# Bind("RESULTADO_PECAS") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="% Perda" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litPorcPerda" runat="server" Text='<%# Bind("PORC_PERDA") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Peças/Dia" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litPecasDia" runat="server" Text='<%# Bind("PECAS_POR_DIA") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Gerente" HeaderStyle-Width="115px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="litGerente" runat="server" Text='<%# Bind("GERENTE") %>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btExcluir" runat="server" Height="15px" Width="15px"
                                                                        ImageUrl="~/Image/delete.png" OnClick="btExcluir_Click"
                                                                        ToolTip="Excluir" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </fieldset>
                <br />
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
