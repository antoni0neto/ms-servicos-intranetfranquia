<%@ Page Title="HB Alteração Ampliação" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" EnableEventValidation="false" CodeBehind="prod_cad_hb_ampliacao.aspx.cs"
    Inherits="Relatorios.prod_cad_hb_ampliacao" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita
        {
            text-align: right;
        }
        .LinkButton
        {
            text-decoration: none;
        }
        .divPop
        {
            font-family: "Trebuchet MS" , "Helvetica" , "Arial" , "Verdana" , "sans-serif";
            font-size: 100 %;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Ordem
                    de Corte&nbsp;&nbsp;>&nbsp;&nbsp;HB Alteração Ampliação</span>
                <div style="float: right; padding: 0;">
                    <a id="hrefVoltar" runat="server" href="prod_menu.aspx" class="alink" title="Voltar">
                        Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1000px; margin-left: 15%;">
                <fieldset>
                    <legend>HB Alteração Ampliação</legend>
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
                                        <td>
                                            Data
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 160px;">
                                            <asp:DropDownList ID="ddlColecoes" runat="server" Width="154px" Height="21px" DataTextField="DESC_COLECAO"
                                                DataValueField="COLECAO" Enabled="false">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 80px;">
                                            <asp:TextBox ID="txtHB" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                                Width="70px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 160px;">
                                            <asp:TextBox ID="txtCodigoLinx" runat="server" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"
                                                Width="150px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td style="width: 180px;">
                                            <asp:DropDownList ID="ddlGrupo" runat="server" Width="174px" Height="21px" DataTextField="GRUPO_PRODUTO"
                                                DataValueField="GRUPO_PRODUTO" Enabled="false">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 260px;">
                                            <asp:TextBox ID="txtNome" runat="server" Width="250px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtData" runat="server" CssClass="alinharDireita" Enabled="false"
                                                Width="129px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <fieldset>
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
                                <td>
                                    Outros
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtOutro" runat="server" TextMode="MultiLine" Height="70px" Width="938px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div style="float: right;">
                            <asp:Label ID="labErroEnvio" runat="server" ForeColor="red" Text=""></asp:Label>
                            &nbsp;&nbsp;&nbsp;<asp:Button ID="btEnviar" runat="server" Text="Enviar HB" OnClick="btEnviar_Click"
                                Width="100px" />
                        </div>
                    </fieldset>
                </fieldset>
            </div>
            <div id="dialogPai" runat="server">
                <div id="dialog" title="Mensagem" class="divPop">
                    <table border="0" width="100%">
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; color: red;">
                                <strong>HB
                                    <asp:Label ID="labHBPopUp" runat="server" Text=""></asp:Label></strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; color: red;">
                                AMPLIAÇÃO ALTERADA.
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Por favor, selecione a tela desejada nos botões abaixo.
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
