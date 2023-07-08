<%@ Page Title="Simulação sem Corte" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="prod_cad_produto_custo_simulacao_novo.aspx.cs" Inherits="Relatorios.prod_cad_produto_custo_simulacao_novo"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ct" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produção&nbsp;&nbsp;>&nbsp;&nbsp;Custo/Preço de Produto&nbsp;&nbsp;>&nbsp;&nbsp;Simulação sem Corte</span>
                <div style="float: right; padding: 0;">
                    <a href="prod_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div style="float: left; width: 1200px; margin-left: 8%;">
                <fieldset>
                    <legend>Simulação sem Corte</legend>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                        <tr style="height: 10px; vertical-align: bottom;">
                            <td>Coleção
                                <asp:HiddenField ID="hidCodigoHB" runat="server" />
                            </td>
                            <td>Produto
                            </td>
                            <td>Nome
                            </td>
                            <td>Mostruário
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 180px;">
                                <asp:DropDownList ID="ddlColecoes" runat="server" Width="174px" Height="21px" DataTextField="DESC_COLECAO"
                                    DataValueField="COLECAO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtProdutoLinxFiltro" runat="server" Text="" CssClass="alinharDireita"
                                    onkeypress="return fnValidarNumero(event);" Width="150px"></asp:TextBox>
                            </td>
                            <td style="width: 160px;">
                                <asp:TextBox ID="txtNomeFiltro" runat="server" Text=""
                                    Width="150px"></asp:TextBox>
                            </td>
                            <td style="width: 65px; text-align: center">
                                <asp:CheckBox ID="chkMostruario" runat="server" Checked="false" />
                            </td>
                            <td style="width: 160px;">&nbsp;
                                <asp:Button runat="server" ID="btBuscar" Text="Copiar" OnClick="btBuscar_Click" Width="115px"
                                    Enabled="true" />&nbsp;&nbsp;
                            </td>
                            <td colspan="2">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="labErro" runat="server" ForeColor="red" Text="&nbsp;">
                                </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">&nbsp;
                            </td>
                        </tr>
                    </table>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                        <asp:Panel ID="pnlCusto" runat="server">
                            <tr>
                                <td>
                                    <fieldset style="width: 300px;">
                                        <legend>Tecidos</legend>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvTecido" runat="server" Width="300px" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white"
                                                ShowFooter="true">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                                <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" Font-Bold="true" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Tecido" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt" runat="server" Width="100px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Consumo" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="qtde" runat="server" Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"
                                                                CssClass="alinharDireita"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Preço" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="valor" runat="server" Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"
                                                                CssClass="alinharDireita"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="width: 300px;">
                                        <legend>Aviamentos</legend>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvAviamento" runat="server" Width="300px" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white"
                                                ShowFooter="true">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Aviamento" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt" runat="server" Width="100px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Consumo" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="qtde" runat="server" Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"
                                                                CssClass="alinharDireita"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Preço" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="valor" runat="server" Width="100px" onkeypress="return fnValidarNumeroDecimal(event);"
                                                                CssClass="alinharDireita"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="width: 300px;">
                                        <legend>Serviços</legend>
                                        <div class="rounded_corners">
                                            <asp:GridView ID="gvServico" runat="server" Width="300px" AutoGenerateColumns="False"
                                                ForeColor="#333333" Style="background: white"
                                                ShowFooter="true">
                                                <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Serviço" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt" runat="server" Width="150px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Valor" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="valor" runat="server" Width="150px" onkeypress="return fnValidarNumeroDecimal(event);"
                                                                CssClass="alinharDireita"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr style="height: 10px;">
                                <td>Etiquetas
                                </td>
                                <td>TAG
                                </td>
                                <td>Operacional (%)
                                </td>
                            </tr>
                            <tr style="height: 10px;">
                                <td>
                                    <asp:TextBox ID="txtValorEtiqueta" runat="server" Width="170px" Text="" onkeypress="return fnValidarNumeroDecimal(event);"
                                        CssClass="alinharDireita"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTAG" runat="server" Width="150px" Text="" onkeypress="return fnValidarNumeroDecimal(event);"
                                        CssClass="alinharDireita"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOperacionalPorc" runat="server" Width="150px" Text="" Enabled="true" onkeypress="return fnValidarNumeroDecimal(event);"
                                        CssClass="alinharDireita"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <fieldset>
                                        <legend>Simulação de Preço</legend>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="width: 65%;">
                                                    <asp:Button ID="btnCalcular" runat="server" Text="Calcular" OnClick="btnCalcular_Click" />
                                                    <asp:Button ID="btnLimpar" runat="server" Text="Limpar" OnClick="btnLimpar_Click" />
                                                </td>
                                                <td style="text-align: right;">
                                                    <div class="rounded_corners">
                                                        <asp:GridView ID="gvTotalItem" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvTotalItem_RowDataBound" OnDataBound="gvTotalItem_DataBound">
                                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                            <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Descrição" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litDesc" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Valor" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="198px">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litValor" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
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
                                                        <asp:GridView ID="gvCustoSimulacao" runat="server" Width="100%" AutoGenerateColumns="False" ShowHeader="false"
                                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvCustoSimulacao_RowDataBound">
                                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                                            <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" Visible="false" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="COLUNA_NOME" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="" HeaderStyle-Width="100px" ItemStyle-Font-Bold="true" />
                                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColunaA" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColunaB" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColunaC" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColunaD" runat="server"></asp:Literal>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="litColunaE" runat="server"></asp:Literal>
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
                                <td>Preço Aprovado
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>
                                    <asp:HiddenField ID="hidTecido" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidAviamento" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidServico" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidOperacional" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidEtiqueta" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidTAG" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidMargem" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidImposto" runat="server" Value="0" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:TextBox ID="txtPrecoAprovado" runat="server" Width="170px" Text="" onkeypress="return fnValidarNumeroDecimal(event);"
                                        CssClass="alinharDireita"></asp:TextBox>
                                    <asp:Button runat="server" ID="btValidar" Text="Validar" OnClick="btValidar_Click" OnClientClick="DesabilitarBotao(this);"
                                        Width="115px" />
                                </td>
                                <td colspan="2">
                                    <div class="rounded_corners" id="divGridReal" runat="server">
                                        <asp:GridView ID="gvCustoReal" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvCustoReal_RowDataBound">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                            <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" Visible="false" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColunaNome" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColunaA" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </asp:Panel>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
