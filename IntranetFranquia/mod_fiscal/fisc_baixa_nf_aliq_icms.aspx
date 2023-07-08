<%@ Page Title="Diferencial Alíquota ICMS" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="fisc_baixa_nf_aliq_icms.aspx.cs" Inherits="Relatorios.fisc_baixa_nf_aliq_icms" %>

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
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Controle de Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Diferencial Alíquota ICMS</span>
                <div style="float: right; padding: 0;">
                    <a href="fisc_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="login">
                <fieldset style="padding-top: 0;">
                    <legend>Diferencial Alíquota ICMS</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvNFAliquotaICMS" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvNFAliquotaICMS_RowDataBound"
                                        ShowFooter="true">
                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Filial" HeaderStyle-Width="240px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litFilial" runat="server" Text='<%# Bind("FILIAL") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="NF Saída" HeaderStyle-Width="180px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNFSaida" runat="server" Text='<%# Bind("NF_SAIDA") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Série" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litSerieNF" runat="server" Text='<%# Bind("SERIE_NF") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Emissão" HeaderStyle-Width="150px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litEmissao" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Nome CLIFOR">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNomeCLIFOR" runat="server" Text='<%# Bind("NOME_CLIFOR") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="UF" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litUF" runat="server" Text='<%# Bind("UF") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Autorização NFE" HeaderStyle-Width="150px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litAutorizacaoNFE" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Button ID="btBaixar1" runat="server" Text="Baixar" Height="21px" Width="100px" OnClick="btBaixar_Click" OnClientClick="javascript: return confirm('Tem certeza que deseja baixar?');" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>

                <fieldset style="padding-top: 0;">
                    <legend>ICMS Regime Especial de Fiscalização</legend>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="rounded_corners">
                                    <asp:GridView ID="gvNFREgimeEspecial" runat="server" Width="100%" AutoGenerateColumns="False"
                                        ForeColor="#333333" Style="background: white" OnRowDataBound="gvNFAliquotaICMS_RowDataBound"
                                        ShowFooter="true">
                                        <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                                        <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Filial" HeaderStyle-Width="240px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litFilial" runat="server" Text='<%# Bind("FILIAL") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="NF Saída" HeaderStyle-Width="180px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNFSaida" runat="server" Text='<%# Bind("NF_SAIDA") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Série" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litSerieNF" runat="server" Text='<%# Bind("SERIE_NF") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Emissão" HeaderStyle-Width="150px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litEmissao" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Nome CLIFOR">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litNomeCLIFOR" runat="server" Text='<%# Bind("NOME_CLIFOR") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="UF" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litUF" runat="server" Text='<%# Bind("UF") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Autorização NFE" HeaderStyle-Width="150px">
                                                <ItemTemplate>
                                                    <asp:Literal ID="litAutorizacaoNFE" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Button ID="btBaixar2" runat="server" Text="Baixar" Height="21px" Width="100px" OnClick="btBaixar_Click" OnClientClick="javascript: return confirm('Tem certeza que deseja baixar?');" />
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
