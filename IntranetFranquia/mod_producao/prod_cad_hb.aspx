<%@ Page Title="HB Novo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="prod_cad_hb.aspx.cs" Inherits="Relatorios.prod_cad_hb"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }

        .LinkButton {
            text-decoration: none;
        }

        .divPop {
            font-family: "Trebuchet MS", "Helvetica", "Arial", "Verdana", "sans-serif";
            font-size: 100 %;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function SomarGrade(control) {
            var total = 0;

            if (document.getElementById("MainContent_txtGradeEXP").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeEXP").value);

            if (document.getElementById("MainContent_txtGradeXP").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeXP").value);

            if (document.getElementById("MainContent_txtGradePP").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradePP").value);

            if (document.getElementById("MainContent_txtGradeP").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeP").value);

            if (document.getElementById("MainContent_txtGradeM").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeM").value);

            if (document.getElementById("MainContent_txtGradeG").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeG").value);

            if (document.getElementById("MainContent_txtGradeGG").value != "")
                total = total + parseInt(document.getElementById("MainContent_txtGradeGG").value);

            document.getElementById("MainContent_txtGradeTotal").value = total;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btFotoTecidoIncluir" />
            <asp:PostBackTrigger ControlID="btFotoPecaIncluir" />
            <asp:PostBackTrigger ControlID="btFotoTecidoDetalheIncluir" />
            <asp:PostBackTrigger ControlID="rdbAmpliacao" />
            <asp:PostBackTrigger ControlID="rdbRisco" />
            <asp:PostBackTrigger ControlID="cbMostruario" />
        </Triggers>
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Ordem
                    de Corte&nbsp;&nbsp;>&nbsp;&nbsp;HB Novo</span>
                <div style="float: right; padding: 0;">
                    <a id="hrefVoltar" runat="server" href="prod_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1000px; margin-left: 15%;">
                <fieldset>
                    <legend>HB Novo</legend>
                    <fieldset style="margin-top: -10px; padding-top: 0px;">
                        <legend>
                            <asp:Label ID="labTipo" runat="server" Text="Tipo"></asp:Label></legend>
                        <table border="0" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td colspan="2" style="text-align: right;">Simulaçao de Corte
                                </td>
                                <td style="text-align: right;">
                                    <asp:Label ID="labMostruario" runat="server" Text="Mostruário"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rdbAmpliacao" runat="server" TextAlign="Right" Text="Ampliação"
                                        GroupName="Tipo" OnCheckedChanged="rdbAmpliacao_CheckedChanged" AutoPostBack="true" />
                                    <asp:RadioButton ID="rdbRisco" runat="server" TextAlign="Right" Text="Risco" GroupName="Tipo"
                                        OnCheckedChanged="rdbRisco_CheckedChanged" AutoPostBack="true" />
                                </td>
                                <td style="text-align: right;">
                                    <asp:CheckBox ID="cbSimulacaoCorte" runat="server" Checked="false" />
                                </td>
                                <td style="text-align: right; width: 100px;">
                                    <asp:CheckBox ID="cbMostruario" runat="server" OnCheckedChanged="cbMostruario_CheckedChanged"
                                        AutoPostBack="true" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <asp:Panel ID="pnlHBCopia" runat="server" Visible="false">
                                        <tr>
                                            <td style="width: 160px;">Coleção
                                            </td>
                                            <td style="width: 80px;">HB
                                            </td>
                                            <td colspan="4">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 160px;">
                                                <asp:DropDownList ID="ddlColecaoCopia" runat="server" Width="154px" Height="21px"
                                                    DataTextField="DESC_COLECAO" DataValueField="COLECAO">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtHBCopia" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                                    Width="70px"></asp:TextBox>
                                            </td>
                                            <td colspan="4">
                                                <asp:Button ID="btCopiarHB" runat="server" Text="Copiar" Width="100px" OnClick="btCopiarHB_Click" />
                                                &nbsp;&nbsp;
                                                <asp:Label ID="labErroCopia" runat="server" Text="" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">&nbsp;<asp:HiddenField ID="hidTela" runat="server" />
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="pnlCabecalho" runat="server">
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 160px;">
                                                <asp:Label ID="labColecao" runat="server" Text="Coleção"></asp:Label>
                                                <asp:HiddenField ID="hidHB" runat="server" />
                                                <asp:HiddenField ID="hidData" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="labHB" runat="server" Text="HB"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labCodigoLinx" runat="server" Text="Cód. Produto Linx"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labGrupo" runat="server" Text="Grupo"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labNome" runat="server" Text="Nome"></asp:Label>
                                            </td>
                                            <td>Data
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 159px;">
                                                <asp:DropDownList ID="ddlColecoes" runat="server" Width="153px" Height="21px" DataTextField="DESC_COLECAO"
                                                    DataValueField="COLECAO" AutoPostBack="true" OnSelectedIndexChanged="ddlColecoes_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 80px;">
                                                <asp:TextBox ID="txtHB" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                                    Width="70px" OnTextChanged="txtHB_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            </td>
                                            <td style="width: 160px;">
                                                <asp:TextBox ID="txtCodigoLinx" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                                    Width="150px" OnTextChanged="txtCodigoLinx_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            </td>
                                            <td style="width: 180px;">
                                                <asp:DropDownList ID="ddlGrupo" runat="server" Width="174px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                                    DataValueField="GRUPO_PRODUTO">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 260px;">
                                                <asp:TextBox ID="txtNome" runat="server" Width="250px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtData" runat="server" CssClass="alinharDireita" ReadOnly="true"
                                                    Width="129px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <asp:Label ID="labOrigem" runat="server" Text="Origem"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <asp:DropDownList ID="ddlOrigem" runat="server" Width="153px" Height="21px" DataTextField="DESCRICAO"
                                                    DataValueField="CODIGO">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="labGrupoTecido" runat="server" Text="Tecido Grupo"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="labSubGrupoTecido" runat="server" Text="Tecido SubGrupo"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labCor" runat="server" Text="Cor"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labCorFornecedor" runat="server" Text="Cor Fornecedor"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:Label ID="labFornecedor" runat="server" Text="Fornecedor"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 160px;">
                                                <asp:DropDownList ID="ddlGrupoTecido" runat="server" Width="154px" Height="21px" DataTextField="GRUPO"
                                                    DataValueField="GRUPO" OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged"
                                                    AutoPostBack="true" Enabled="false">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 229px;" colspan="2">
                                                <asp:DropDownList ID="ddlSubGrupoTecido" runat="server" Width="233px" Height="21px" DataTextField="SUBGRUPO"
                                                    DataValueField="SUBGRUPO" OnSelectedIndexChanged="ddlSubGrupo_SelectedIndexChanged"
                                                    AutoPostBack="true" Enabled="false">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCor" runat="server" Width="173px" Height="21px" DataTextField="DESC_COR_PRODUTO"
                                                    DataValueField="COR_PRODUTO" Enabled="false">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 180px;">
                                                <asp:DropDownList ID="ddlCorFornecedor" runat="server" Width="174px" Height="21px"
                                                    DataTextField="REFER_FABRICANTE" DataValueField="REFER_FABRICANTE" Enabled="false">
                                                </asp:DropDownList>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="ddlFornecedor" runat="server" Width="213px" Height="21px" DataTextField="FORNECEDOR"
                                                    DataValueField="FORNECEDOR" Enabled="false">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="labMolde" runat="server" Text="Molde"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labPedidoNumero" runat="server" Text="No. Pedido"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labModelagem" runat="server" Text="Modelagem"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labLargura" runat="server" Text="Largura"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labCustoTecido" runat="server" Text="Custo Tecido"></asp:Label>
                                            </td>
                                            <td style="text-align: right;">Liquidar
                                            </td>
                                            <td style="text-align: right;">Liquidar Pqno
                                            </td>
                                            <td style="text-align: right;">Atacado
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: central;">
                                                <asp:TextBox ID="txtMolde" runat="server" CssClass="alinharDireita"
                                                    Width="125px" OnTextChanged="txtMolde_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                <asp:ImageButton ID="imgAbrirMolde" runat="server" ImageAlign="Top" Width="17px" ImageUrl="~/Image/add.png" OnClientClick="fnAbrirTelaCadastroMaior('prod_cad_molde.aspx');" />
                                            </td>
                                            <td style="width: 80px;">
                                                <asp:TextBox ID="txtPedidoNumero" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                                    Width="70px" OnTextChanged="txtNumeroPedido_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            </td>
                                            <td style="width: 160px;">
                                                <asp:TextBox ID="txtModelagem" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td style="width: 180px;">
                                                <asp:TextBox ID="txtLargura" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                                    Width="169px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCustoTecido" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimal(event);"
                                                    Width="170px"></asp:TextBox>
                                            </td>
                                            <td style="text-align: right;">
                                                <asp:CheckBox ID="cbLiquidar" runat="server" OnCheckedChanged="cbLiquidar_CheckedChanged" AutoPostBack="true" />
                                            </td>
                                            <td style="text-align: right;">
                                                <asp:CheckBox ID="cbLiquidarPqno" runat="server" OnCheckedChanged="cbLiquidarPqno_CheckedChanged" AutoPostBack="true" />
                                            </td>
                                            <td style="width: 70px; text-align: right;">
                                                <asp:CheckBox ID="cbAtacado" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="labGastoFolha" runat="server" Text="Gasto por Folha"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="labNomeGrade" runat="server" Text="Grade"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="labPrecoFaccMostruario" runat="server" Text="Preço Facção Mostruário"></asp:Label>
                                            </td>
                                            <td colspan="4">
                                                <asp:Label ID="labGabarito" runat="server" Text="Gabarito"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtGastoPorFolha" runat="server" CssClass="alinharDireita"
                                                    MaxLength="9" ToolTip="Simulação" Width="150px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                            </td>
                                            <td colspan="2">
                                                <asp:DropDownList ID="ddlGrade" runat="server" Width="234px" Height="21px" DataTextField="GRADE"
                                                    DataValueField="CODIGO" OnSelectedIndexChanged="ddlGrade_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPrecoFaccMostruario" runat="server" CssClass="alinharDireita" Text=""
                                                    MaxLength="9" ToolTip="Preço de Facção para Mostruário" Width="169px" onkeypress="return fnValidarNumeroDecimal(event);"></asp:TextBox>
                                            </td>
                                            <td colspan="4">
                                                <asp:DropDownList ID="ddlGabarito" runat="server" Width="174px" Height="21px">
                                                    <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="8">
                                                <asp:Label ID="labEstamparia" runat="server" Text="Estamparia"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="8">
                                                <asp:DropDownList ID="ddlEstamparia" runat="server" Width="154px" Height="21px">
                                                    <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <fieldset id="fsAmpliacao" runat="server" style="margin-top: 4px;" visible="false">
                        <legend>
                            <asp:Label ID="labAmpliacao" runat="server" Text="Ampliação - Medidas de Fabricação"></asp:Label></legend>
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <fieldset style="margin-top: -10px;">
                                        <legend>Elástico</legend>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvElastico" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvElastico_RowDataBound"
                                                DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Local" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtLocal" runat="server" Width="147px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="EXP" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGradeEXP" runat="server" Width="80px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="XP" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGradeXP" runat="server" Width="80px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PP" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGradePP" runat="server" Width="80px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="P" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGradeP" runat="server" Width="80px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="M" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGradeM" runat="server" Width="80px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="G" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGradeG" runat="server" Width="80px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="GG" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGradeGG" runat="server" Width="80px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Largura" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtLargura" runat="server" Width="110px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <fieldset style="margin-top: -10px;">
                                        <legend>Galão</legend>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvGalao" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvGalao_RowDataBound"
                                                DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quantidade" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="147px">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtQtde" runat="server" Width="147px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Local" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtLocal" runat="server" Width="265px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Comprimento" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtComprimento" runat="server" Width="220px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Largura" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtLargura" runat="server" Width="220px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        Descrição<br />
                                        <asp:TextBox ID="txtGalaoDescricao" runat="server" Width="100%"></asp:TextBox>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <fieldset style="margin-top: -10px;">
                                        <legend>Cortar Alça c/</legend>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvAlcaPronta" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvAlcaPronta_RowDataBound"
                                                DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="EXP" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGradeEXP" runat="server" Width="118px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="XP" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGradeXP" runat="server" Width="118px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PP" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGradePP" runat="server" Width="118px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="P" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGradeP" runat="server" Width="118px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="M" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGradeM" runat="server" Width="118px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="G" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGradeG" runat="server" Width="118px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="GG" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGradeGG" runat="server" Width="118px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td>Outros
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtOutro" runat="server" TextMode="MultiLine" Height="70px" Width="938px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <asp:Panel ID="pnlRisco" runat="server" Visible="false">
                        <fieldset style="margin-top: 4px; padding-top: 0;">
                            <legend>
                                <asp:Label ID="labFotoTecido" runat="server" Text="Foto Tecido"></asp:Label></legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 270px; vertical-align: top;">
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="float: left;">&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 150px; vertical-align: top;">
                                        <asp:FileUpload ID="uploadFotoTecido" runat="server" Width="300px" /><br />
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <asp:Button ID="btFotoTecidoIncluir" runat="server" OnClick="btFotoTecidoIncluir_Click"
                                            Text=">>" Width="32px" />
                                    </td>
                                    <td>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="float: right;">
                                                    <asp:Image ID="imgFotoTecido" Width='130px' Height='180px' runat="server" BorderColor="Black"
                                                        BorderWidth="0" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <div id="divErroFotoTecido" runat="server" style="text-align: center;">
                                            <asp:Label ID="labErroFotoTecido" runat="server" Visible="true" ForeColor="Red" Text=""></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset style="margin-top: -10px; padding-top: 0px;">
                            <legend>
                                <asp:Label ID="labComposicaoTecido" runat="server" Text="Composição Tecido"></asp:Label></legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 146px;">Quantidade
                                                    <asp:HiddenField ID="hidTotalComp" runat="server" />
                                                </td>
                                                <td style="width: 260px;">Descrição
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="width: 146px;">
                                                    <asp:TextBox ID="txtComposicaoQtde" runat="server" CssClass="alinharDireita" MaxLength="6"
                                                        onkeypress="return fnValidarNumeroDecimal(event);" Width="136px"></asp:TextBox>
                                                </td>
                                                <td style="width: 260px;">
                                                    <asp:TextBox ID="txtComposicaoDescricao" runat="server" Width="251px" MaxLength="70"></asp:TextBox>
                                                </td>
                                                <td style="width: 260px;">
                                                    <asp:Button ID="btComposicaoIncluir" runat="server" OnClick="btComposicaoIncluir_Click"
                                                        Text="Incluir" Width="116px" />&nbsp;&nbsp;&nbsp;
                                                </td>
                                                <td style="text-align: right;">
                                                    <asp:Button ID="btObterComposicao" runat="server" OnClick="btObterComposicao_Click"
                                                        Text="Obter Composição" Width="140px" />&nbsp;&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Label ID="labErroComposicao" runat="server" Visible="true" ForeColor="Red" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvComposicao" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvComposicao_RowDataBound"
                                                OnDataBound="gvComposicao_DataBound">
                                                <HeaderStyle BackColor="GradientActiveCaption" />
                                                <FooterStyle BackColor="GradientActiveCaption" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litComposicaoColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="QTDE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                        HeaderText="Quantidade" HeaderStyle-Width="140px" />
                                                    <asp:BoundField DataField="DESCRICAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Descrição" HeaderStyle-Width="715px" />
                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btComposicaoExcluir" runat="server" Height="19px" Text="Excluir"
                                                                OnClientClick="return ConfirmarExclusao();" OnClick="btComposicaoExcluir_Click" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset style="margin-top: -10px; padding-top: 0px;">
                            <legend>
                                <asp:Label ID="labGrade" runat="server" Text="Grade"></asp:Label></legend>
                            <table border="0" width="100%" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="width: 147px;">&nbsp;
                                    </td>
                                    <td style="width: 80px; text-align: center;">
                                        <asp:Label ID="labGradeEXP" runat="server" Text="EXP"></asp:Label>
                                    </td>
                                    <td style="width: 80px; text-align: center;">
                                        <asp:Label ID="labGradeXP" runat="server" Text="XP"></asp:Label>
                                    </td>
                                    <td style="width: 80px; text-align: center;">
                                        <asp:Label ID="labGradePP" runat="server" Text="PP"></asp:Label>
                                    </td>
                                    <td style="width: 80px; text-align: center;">
                                        <asp:Label ID="labGradeP" runat="server" Text="P"></asp:Label>
                                    </td>
                                    <td style="width: 80px; text-align: center;">
                                        <asp:Label ID="labGradeM" runat="server" Text="M"></asp:Label>
                                    </td>
                                    <td style="width: 80px; text-align: center;">
                                        <asp:Label ID="labGradeG" runat="server" Text="G"></asp:Label>
                                    </td>
                                    <td style="width: 80px; text-align: center;">
                                        <asp:Label ID="labGradeGG" runat="server" Text="GG"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">Total
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 147px;">Prevista
                                    </td>
                                    <td style="width: 80px;">
                                        <asp:TextBox ID="txtGradeEXP" runat="server" MaxLength="6" CssClass="alinharDireita"
                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" Width="70px"></asp:TextBox>
                                    </td>
                                    <td style="width: 80px;">
                                        <asp:TextBox ID="txtGradeXP" runat="server" MaxLength="6" CssClass="alinharDireita"
                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" Width="70px"></asp:TextBox>
                                    </td>
                                    <td style="width: 80px;">
                                        <asp:TextBox ID="txtGradePP" runat="server" MaxLength="6" CssClass="alinharDireita"
                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" Width="70px"></asp:TextBox>
                                    </td>
                                    <td style="width: 80px;">
                                        <asp:TextBox ID="txtGradeP" runat="server" MaxLength="6" CssClass="alinharDireita"
                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" Width="70px"></asp:TextBox>
                                    </td>
                                    <td style="width: 80px;">
                                        <asp:TextBox ID="txtGradeM" runat="server" MaxLength="6" CssClass="alinharDireita"
                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" Width="70px"></asp:TextBox>
                                    </td>
                                    <td style="width: 80px;">
                                        <asp:TextBox ID="txtGradeG" runat="server" MaxLength="6" CssClass="alinharDireita"
                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" Width="70px"></asp:TextBox>
                                    </td>
                                    <td style="width: 80px;">
                                        <asp:TextBox ID="txtGradeGG" runat="server" MaxLength="6" CssClass="alinharDireita"
                                            onkeypress="return fnValidarNumero(event);" onblur="SomarGrade(this);" Width="70px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:TextBox ID="txtGradeTotal" runat="server" MaxLength="6" CssClass="alinharDireita"
                                            onkeypress="return fnValidarNumero(event);" Width="70px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <label>
                            Observação</label>
                        <asp:TextBox ID="txtObservacao" TextMode="MultiLine" runat="server" Width="967px"
                            Height="50px"></asp:TextBox>
                        <fieldset style="margin-top: 0px; padding-top: 0px;">
                            <legend>Detalhes</legend>
                            <table border="0" width="100%" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>Detalhe
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlDetalhe" runat="server" Width="154px" DataTextField="DESCRICAO" Height="21px"
                                            DataValueField="CODIGO" AutoPostBack="true" OnSelectedIndexChanged="ddlDetalhe_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="pnlDetalhe" runat="server">
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>Tecido Grupo
                                                    </td>
                                                    <td>Tecido SubGrupo
                                                    </td>
                                                    <td>Cor
                                                    </td>
                                                    <td>Cor Fornecedor
                                                    </td>
                                                    <td>Fornecedor
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 160px;">
                                                        <asp:DropDownList ID="ddlDetalheGrupoTecido" runat="server" Width="154px" Height="21px"
                                                            DataTextField="GRUPO" DataValueField="GRUPO" OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged"
                                                            AutoPostBack="true" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 229px;">
                                                        <asp:DropDownList ID="ddlDetalheSubGrupoTecido" runat="server" Width="223px" Height="21px"
                                                            DataTextField="SUBGRUPO" DataValueField="SUBGRUPO" OnSelectedIndexChanged="ddlSubGrupo_SelectedIndexChanged"
                                                            AutoPostBack="true" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 180px;">
                                                        <asp:DropDownList ID="ddlDetalheCor" runat="server" Width="174px" Height="21px" DataTextField="DESC_COR_PRODUTO"
                                                            DataValueField="COR_PRODUTO" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 180px;">
                                                        <asp:DropDownList ID="ddlDetalheCorFornecedor" runat="server" Width="174px" Height="21px"
                                                            DataTextField="REFER_FABRICANTE" DataValueField="REFER_FABRICANTE" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlDetalheFornecedor" runat="server" Width="198px" Height="21px"
                                                            DataTextField="FORNECEDOR" DataValueField="FORNECEDOR" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>No. Pedido
                                                    </td>
                                                    <td>Molde
                                                    </td>
                                                    <td>Largura
                                                    </td>
                                                    <td>Custo Tecido
                                                    </td>
                                                    <td>Gasto por Folha
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtDetalheNumeroPedido" runat="server" CssClass="alinharDireita"
                                                            MaxLength="5" onkeypress="return fnValidarNumero(event);" Width="150px" OnTextChanged="txtNumeroPedido_TextChanged"
                                                            AutoPostBack="true"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDetalheMolde" runat="server" CssClass="alinharDireita"
                                                            Width="219px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDetalheLargura" runat="server" CssClass="alinharDireita" MaxLength="8"
                                                            onkeypress="return fnValidarNumeroDecimal(event);" Width="170px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDetalheCustoTecido" runat="server" CssClass="alinharDireita"
                                                            MaxLength="8" onkeypress="return fnValidarNumeroDecimal(event);" Width="170px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDetalheGastoPorFolha" runat="server" CssClass="alinharDireita"
                                                            MaxLength="8" onkeypress="return fnValidarNumeroDecimal(event);" Width="194px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5">Etiqueta de Composição
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5">
                                                        <asp:DropDownList ID="ddlEtiquetaComposicao" runat="server" Width="154px" Height="21px">
                                                            <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                                            <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                                            <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5">
                                                        <br />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <fieldset id="fsDetalheComposicao" runat="server" style="margin-top: 0; padding-top: 0px;">
                                                <legend>
                                                    <asp:Label ID="labComposicaoDetalhe" runat="server" Text="Composição Tecido Detalhe"></asp:Label></legend>
                                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td style="width: 146px;">Quantidade
                                                                        <asp:HiddenField ID="hidDetalheCompTotal" runat="server" />
                                                                    </td>
                                                                    <td style="width: 260px;">Descrição
                                                                    </td>
                                                                    <td>&nbsp;
                                                                    </td>
                                                                    <td>&nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 146px;">
                                                                        <asp:TextBox ID="txtDetalheQtdeComposicao" runat="server" CssClass="alinharDireita"
                                                                            MaxLength="6" onkeypress="return fnValidarNumeroDecimal(event);" Width="136px"></asp:TextBox>
                                                                    </td>
                                                                    <td style="width: 260px;">
                                                                        <asp:TextBox ID="txtDetalheDescricaoComposicao" runat="server" Width="251px" MaxLength="70"></asp:TextBox>
                                                                    </td>
                                                                    <td style="width: 260px;">
                                                                        <asp:Button ID="btDetalheComposicaoIncluir" runat="server" OnClick="btDetalheComposicaoIncluir_Click"
                                                                            Text="Incluir" Width="116px" />
                                                                    </td>
                                                                    <td style="text-align: right;">
                                                                        <asp:Button ID="btDetalheObterComposicao" runat="server" OnClick="btDetalheObterComposicao_Click"
                                                                            Text="Obter Composição" Width="140px" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4">
                                                                        <asp:Label ID="labErroDetalheComposicao" runat="server" Visible="true" ForeColor="Red"
                                                                            Text=""></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div class="rounded_corners">
                                                                <asp:GridView ID="gvComposicaoDetalhe" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                    ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvComposicaoDetalhe_RowDataBound"
                                                                    OnDataBound="gvComposicaoDetalhe_DataBound">
                                                                    <HeaderStyle BackColor="GradientActiveCaption" />
                                                                    <FooterStyle BackColor="GradientActiveCaption" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="litComposicaoColuna" runat="server"></asp:Literal>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="QTDE" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                            HeaderText="Quantidade" HeaderStyle-Width="140px" />
                                                                        <asp:BoundField DataField="DESCRICAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                            HeaderText="Descrição" HeaderStyle-Width="715px" />
                                                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Button ID="btDetalheComposicaoExcluir" runat="server" Height="19px" Text="Excluir"
                                                                                    OnClientClick="return ConfirmarExclusao();" OnClick="btDetalheComposicaoExcluir_Click" />
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
                                    <tr>
                                        <td>
                                            <fieldset style="margin-top: -1px; padding-top: 0;">
                                                <legend>Foto Tecido Detalhe</legend>
                                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 252px; vertical-align: top;">
                                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td style="float: left;">&nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="width: 300px; vertical-align: top;">
                                                            <asp:FileUpload ID="uploadFotoTecidoDetalhe" runat="server" Width="300px" /><br />
                                                            <br />
                                                            <br />
                                                            <br />
                                                            <br />
                                                            <asp:Button ID="btFotoTecidoDetalheIncluir" runat="server" OnClick="btFotoTecidoDetalheIncluir_Click"
                                                                Text=">>" Width="32px" />
                                                        </td>
                                                        <td>
                                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td style="float: right;">
                                                                        <asp:Image ID="imgFotoTecidoDetalhe" runat="server" Width="130px" Height="180px"
                                                                            BorderColor="Black" BorderWidth="0" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <div id="divErroFotoTecidoDetalhe" runat="server" style="text-align: center;">
                                                                <asp:Label ID="labErroFotoTecidoDetalhe" runat="server" Visible="true" ForeColor="Red"
                                                                    Text=""></asp:Label>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                                <label>
                                    Observação</label>
                                <asp:TextBox ID="txtDetalheObservacao" TextMode="MultiLine" runat="server" Width="939px"
                                    Height="50px"></asp:TextBox>
                                <br />
                                <br />
                                <div style="float: left; margin-right: 2px;">
                                    <asp:Button ID="btDetalheIncluir" runat="server" Text="Incluir Detalhe" Width="116px"
                                        OnClick="btDetalheIncluir_Click" ForeColor="Red" Font-Bold="true" />&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btDetalheLimpar" runat="server" Text="Limpar Detalhe" Width="116px"
                                        OnClick="btDetalheLimpar_Click" />&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="labErroDetalhe" runat="server" Visible="true" ForeColor="Red" Text=""></asp:Label>
                                    <asp:HiddenField ID="hidCodigoDetalhe" runat="server" />
                                </div>
                            </asp:Panel>
                            <br />
                            <br />
                            <div class="rounded_corners">
                                <asp:GridView ID="gvDetalhe" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" Style="background: white" OnRowDataBound="gvDetalhe_RowDataBound"
                                    DataKeyNames="CODIGO">
                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litDetalheColuna" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="HB" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="litHB" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Detalhe" HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="litDetalheDesc" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Número do Pedido" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDetalheNumeroPedido" runat="server" CssClass="alinharDireita"
                                                    onkeypress="return fnValidarNumero(event);" AutoPostBack="true" OnTextChanged="txtDetalheNumeroPedido_TextChanged"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Button ID="btDetalheAlterar" runat="server" Height="19px" Text="Alterar" OnClick="btDetalheAlterar_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Button ID="btDetalheExcluir" runat="server" Height="19px" Text="Excluir" OnClientClick="return ConfirmarExclusao();"
                                                    OnClick="btDetalheExcluir_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                        <fieldset style="margin-top: -10px; padding-top: 0px;">
                            <legend>
                                <asp:Label ID="labAviamentos" runat="server" Text="Aviamentos"></asp:Label></legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>Aviamento Grupo
                                                </td>
                                                <td>Aviamento SubGrupo
                                                </td>
                                                <td>Cor
                                                </td>
                                                <td colspan="2">Cor Fornecedor
                                                </td>

                                            </tr>
                                            <tr>
                                                <td style="width: 210px;">
                                                    <asp:DropDownList ID="ddlGrupoAviamento" runat="server" Width="204px" Height="21px"
                                                        DataTextField="GRUPO" DataValueField="CODIGO_GRUPO" OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 250px;">
                                                    <asp:DropDownList ID="ddlSubGrupoAviamento" runat="server" Width="244px" Height="21px"
                                                        DataTextField="SUBGRUPO" DataValueField="CODIGO_SUBGRUPO" OnSelectedIndexChanged="ddlSubGrupo_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 270px;">
                                                    <asp:DropDownList ID="ddlCorAviamento" runat="server" Width="264px" Height="21px" DataTextField="DESC_COR"
                                                        DataValueField="COR" OnSelectedIndexChanged="ddlCorAviamento_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                                <td colspan="2">
                                                    <asp:DropDownList ID="ddlCorFornecedorAviamento" runat="server" Width="215px" Height="21px"
                                                        DataTextField="REFER_FABRICANTE" DataValueField="REFER_FABRICANTE">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Quantidade Total
                                                </td>
                                                <td>Quantidade Por Peça
                                                </td>
                                                <td>Descrição
                                                </td>
                                                <td>Etiqueta Composição
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtAviamentoQtde" MaxLength="7" CssClass="alinharDireita" runat="server"
                                                        onkeypress="return fnValidarNumeroDecimal(event);" Width="200px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAviamentoQtdePorPeca" MaxLength="7" CssClass="alinharDireita" runat="server"
                                                        onkeypress="return fnValidarNumeroDecimal(event);" Width="240px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAviamentoDescricao" runat="server" Width="260px" Text="POR PEÇA" MaxLength="50"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlEtiquetaComposicaoAviamento" runat="server" Width="139px" Height="21px">
                                                        <asp:ListItem Value="" Text="Selecione"></asp:ListItem>
                                                        <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                                        <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="text-align: right;">
                                                    <asp:Button ID="btAviamentoIncluir" runat="server" OnClick="btAviamentoIncluir_Click"
                                                        Text="Incluir" Width="70px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Label ID="labErroAviamento" runat="server" Visible="true" ForeColor="Red" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvAviamento" runat="server" Width="100%" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white" OnRowDataBound="gvAviamento_RowDataBound"
                                                DataKeyNames="CODIGO">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                                <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litAviamentoColuna" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qtde Total" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtQtde" runat="server" CssClass="alinharDireita" Width="75px" onkeypress="return fnValidarNumeroDecimal(event);"
                                                                OnTextChanged="txtQtde_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Grupo" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litGrupo" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SubGrupo" HeaderStyle-Width="170px" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litSubGrupo" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cor" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litCor" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Desc Fornecedor" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litCorFornecedor" runat="server" Text='<%# Eval("COR_FORNECEDOR") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qtde Por Peça" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="labQtdePorPeca" runat="server" Width="90px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="DESCRICAO" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-HorizontalAlign="Left" HeaderText="Descrição" />
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btAviamentoExcluir" runat="server" Height="19px" Text="Excluir" OnClientClick="return ConfirmarExclusao();"
                                                                OnClick="btAviamentoExcluir_Click" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset style="margin-top: -1px; padding-top: 0;">
                            <legend>
                                <asp:Label ID="labFotoPeca" runat="server" Text="Foto Peça"></asp:Label></legend>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 270px; vertical-align: top;">
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="float: left;">&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 300px; vertical-align: top;">
                                        <asp:FileUpload ID="uploadFotoPeca" runat="server" Width="300px" /><br />
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <asp:Button ID="btFotoPecaIncluir" runat="server" OnClick="btFotoPecaIncluir_Click"
                                            Text=">>" Width="32px" />
                                    </td>
                                    <td>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="float: right;">
                                                    <asp:Image ID="imgFotoPeca" Width='130px' Height='180px' runat="server" BorderColor="Black"
                                                        BorderWidth="0" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <div id="divErroFotoPeca" runat="server" style="text-align: center;">
                                            <asp:Label ID="labErroFotoPeca" runat="server" Visible="true" ForeColor="Red" Text=""></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </asp:Panel>
                    <hr />
                    <div style="float: right;">
                        <asp:Label ID="labErroEnvio" runat="server" ForeColor="red" Text=""></asp:Label>
                        &nbsp;&nbsp;&nbsp;<asp:Button ID="btEnviar" runat="server" Text="Enviar HB" OnClick="btEnviar_Click"
                            Width="100px" OnClientClick="DesabilitarBotao(this);" />
                    </div>
                </fieldset>
            </div>
            <div id="dialogPai" runat="server">
                <div id="dialog" title="Mensagem" class="divPop">
                    <table border="0" width="100%">
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; color: red;">
                                <strong>HB
                                    <asp:Label ID="labHBPopUp" runat="server" Text=""></asp:Label></strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; color: red;">ENVIADO COM SUCESSO
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>Por favor, selecione a tela desejada nos botões abaixo.
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
