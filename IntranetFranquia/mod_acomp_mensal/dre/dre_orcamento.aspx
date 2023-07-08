<%@ Page Title="Orçamento" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="dre_orcamento.aspx.cs" Inherits="Relatorios.dre_orcamento" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <script src="../../js/js.js"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Always" EnableViewState="true">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo Acompanhamento Mensal&nbsp;&nbsp;>&nbsp;&nbsp;DRE&nbsp;&nbsp;>&nbsp;&nbsp;Orçamento&nbsp;&nbsp;</span>
                <div style="float: right; padding: 0;">
                    <a href="../acomp_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="accountInfo">
                <fieldset>
                    <legend>Orçamento</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="labAno" runat="server" Text="Ano"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labGrupo" runat="server" Text="Grupo"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labSubGrupo" runat="server" Text="SubGrupo"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labItem" runat="server" Text="Item"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">
                                <asp:DropDownList runat="server" ID="ddlAno" Height="22px" Width="144px" DataTextField="ANO"
                                    DataValueField="ANO" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 260px;">
                                <asp:DropDownList runat="server" ID="ddlGrupo" Height="22px" Width="254px" DataTextField="GRUPO"
                                    DataValueField="GRUPO" OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 260px;">
                                <asp:DropDownList runat="server" ID="ddlSubGrupo" Height="22px" Width="254px" DataTextField="LINHA"
                                    DataValueField="LINHA" OnSelectedIndexChanged="ddlSubGrupo_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 380px;">
                                <asp:DropDownList runat="server" ID="ddlItem" Height="22px" Width="374px" DataTextField="TIPO"
                                    DataValueField="TIPO" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="FILIAL" DataTextField="FILIAL"
                                    Height="22px" Width="260px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" />
                                &nbsp;&nbsp;
                                <asp:Label ID="labErro" runat="server" Text="" ForeColor="red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 0;" colspan="6">
                                <fieldset>
                                    <legend>
                                        <asp:Label ID="labTitulo" runat="server" Text=""></asp:Label></legend>
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvOrcamento" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                            Style="background: white" OnRowDataBound="gvOrcamento_RowDataBound" OnDataBound="gvOrcamento_DataBound"
                                            ShowFooter="true" DataKeyNames="CODIGO">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                            <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Right" Font-Bold="true" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Filial" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFilial" runat="server" Text='<%#Bind("FILIAL") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Janeiro" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtJaneiro" runat="server" Width="90px" Text='<%# Bind("JANEIRO") %>' OnTextChanged="txtValor_TextChanged" AutoPostBack="true"
                                                            CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Fevereiro" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtFevereiro" runat="server" Width="90px" Text='<%# Bind("FEVEREIRO") %>' OnTextChanged="txtValor_TextChanged" AutoPostBack="true"
                                                            CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Março" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtMarco" runat="server" Width="90px" Text='<%# Bind("MARCO") %>' OnTextChanged="txtValor_TextChanged" AutoPostBack="true"
                                                            CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Abril" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAbril" runat="server" Width="90px" Text='<%# Bind("ABRIL") %>' OnTextChanged="txtValor_TextChanged" AutoPostBack="true"
                                                            CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Maio" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtMaio" runat="server" Width="90px" Text='<%# Bind("MAIO") %>' OnTextChanged="txtValor_TextChanged" AutoPostBack="true"
                                                            CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Junho" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtJunho" runat="server" Width="90px" Text='<%# Bind("JUNHO") %>' OnTextChanged="txtValor_TextChanged" AutoPostBack="true"
                                                            CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Julho" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtJulho" runat="server" Width="90px" Text='<%# Bind("JULHO") %>' OnTextChanged="txtValor_TextChanged" AutoPostBack="true"
                                                            CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Agosto" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAgosto" runat="server" Width="90px" Text='<%# Bind("AGOSTO") %>' OnTextChanged="txtValor_TextChanged" AutoPostBack="true"
                                                            CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Setembro" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtSetembro" runat="server" Width="90px" Text='<%# Bind("SETEMBRO") %>' OnTextChanged="txtValor_TextChanged" AutoPostBack="true"
                                                            CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Outubro" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtOutubro" runat="server" Width="90px" Text='<%# Bind("OUTUBRO") %>' OnTextChanged="txtValor_TextChanged" AutoPostBack="true"
                                                            CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Novembro" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtNovembro" runat="server" Width="90px" Text='<%# Bind("NOVEMBRO") %>' OnTextChanged="txtValor_TextChanged" AutoPostBack="true"
                                                            CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dezembro" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDezembro" runat="server" Width="90px" Text='<%# Bind("DEZEMBRO") %>' OnTextChanged="txtValor_TextChanged" AutoPostBack="true"
                                                            CssClass="alinharDireita" onkeypress="return fnValidarNumeroDecimalNegativo(event);"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
