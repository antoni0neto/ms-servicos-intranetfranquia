<%@ Page Title="Comprovantes de Depósito" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="fin_deposito_compimagem.aspx.cs" Inherits="Relatorios.fin_deposito_compimagem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo do Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Financeiro&nbsp;&nbsp;>&nbsp;&nbsp;Visualizar Imagens</span>
        <div style="float: right; padding: 0;">
            <a href="fin_menu.aspx" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <fieldset class="login">
        <legend>Comprovantes</legend>
        <table border="0" width="100%" cellpadding="0" cellspacing="0">
            <tr>
                <td>Filial</td>
                <td>Data Inicial</td>
                <td>Data Final</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 250px;">
                    <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="21px" Width="244px"></asp:DropDownList>
                </td>
                <td style="width: 130px;">
                    <asp:TextBox ID="txtDataInicial" runat="server" Width="120px" MaxLength="10" Style="text-align: right;"
                        onkeypress="return fnValidarData(event);"></asp:TextBox>
                </td>
                <td style="width: 130px;">
                    <asp:TextBox ID="txtDataFinal" runat="server" Width="120px" MaxLength="10" Style="text-align: right;"
                        onkeypress="return fnValidarData(event);"></asp:TextBox>
                </td>
                <td>
                    <asp:Button runat="server" ID="btBuscar" Text="Buscar" Width="120px" OnClick="btBuscar_Click" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="4">
                    <div class="rounded_corners">
                        <asp:GridView ID="gvComprovante" runat="server" Width="100%" AutoGenerateColumns="False"
                            ForeColor="#333333" Style="background: white" OnRowDataBound="gvComprovante_RowDataBound"
                            ShowFooter="true" DataKeyNames="">
                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center"></HeaderStyle>
                            <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                            <Columns>
                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Filial" ItemStyle-Width="250px" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="labFilial" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Data Digitada" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="labDataDigitada" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="NOME_IMAGEM" HeaderText="Comprovante" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderText="Arquivo" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlAbrirComp" Text='<%# Eval("LOCAL_IMAGEM") %>' runat="server" Target="_blank" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
