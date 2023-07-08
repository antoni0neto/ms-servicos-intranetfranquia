<%@ Page Title="DRE - Atualização Lançamento" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="dre_lancamento_atual.aspx.cs" Inherits="Relatorios.dre_lancamento_atual" MaintainScrollPositionOnPostback="true" %>

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
                <span style="font-family: Calibri; font-size: 14px;">Módulo Acompanhamento Mensal&nbsp;&nbsp;>&nbsp;&nbsp;DRE&nbsp;&nbsp;>&nbsp;&nbsp;DRE - Atualização Lançamento&nbsp;&nbsp;</span>
                <div style="float: right; padding: 0;">
                    <a href="../acomp_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="accountInfo">
                <fieldset>
                    <legend>DRE - Atualização Lançamento</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="labAno" runat="server" Text="Ano"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labMes" runat="server" Text="Mês"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labTipo" runat="server" Text="Tipo"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labFilial" runat="server" Text="Filial"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labConta" runat="server" Text="Conta Contábil"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labCentroCusto" runat="server" Text="Centro de Custo"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 110px;">
                                <asp:DropDownList runat="server" ID="ddlAno" Height="22px" Width="104px" DataTextField="ANO"
                                    DataValueField="ANO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 120px;">
                                <asp:DropDownList runat="server" ID="ddlMes" Height="22px" Width="114px" Enabled="true">
                                    <asp:ListItem Value="" Text="Selecione" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="01" Text="Janeiro"></asp:ListItem>
                                    <asp:ListItem Value="02" Text="Fevereiro"></asp:ListItem>
                                    <asp:ListItem Value="03" Text="Março"></asp:ListItem>
                                    <asp:ListItem Value="04" Text="Abril"></asp:ListItem>
                                    <asp:ListItem Value="05" Text="Maio"></asp:ListItem>
                                    <asp:ListItem Value="06" Text="Junho"></asp:ListItem>
                                    <asp:ListItem Value="07" Text="Julho"></asp:ListItem>
                                    <asp:ListItem Value="08" Text="Agosto"></asp:ListItem>
                                    <asp:ListItem Value="09" Text="Setembro"></asp:ListItem>
                                    <asp:ListItem Value="10" Text="Outubro"></asp:ListItem>
                                    <asp:ListItem Value="11" Text="Novembro"></asp:ListItem>
                                    <asp:ListItem Value="12" Text="Dezembro"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 180px;">
                                <asp:DropDownList runat="server" ID="ddlTipo" Height="22px" Width="174px" Enabled="true">
                                    <asp:ListItem Value="" Text="" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="CUSTOS FIXOS" Text="CUSTOS FIXOS"></asp:ListItem>
                                    <asp:ListItem Value="DESPESAS ESPECIFICAS" Text="DESPESAS ESPECIFICAS"></asp:ListItem>
                                    <asp:ListItem Value="DESPESAS VENDA" Text="DESPESAS VENDA"></asp:ListItem>
                                    <asp:ListItem Value="DESPESAS ADMINISTRATIVAS" Text="DESPESAS ADMINISTRATIVAS"></asp:ListItem>
                                    <asp:ListItem Value="DESPESAS EVENTUAIS" Text="DESPESAS EVENTUAIS"></asp:ListItem>
                                    <asp:ListItem Value="DESPESAS FINANCEIRAS" Text="DESPESAS FINANCEIRAS"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 300px;">
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="22px" Width="294px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 400px;">
                                <asp:DropDownList runat="server" ID="ddlContaContabil" DataValueField="CONTA_CONTABIL" DataTextField="DESC_CONTA"
                                    Height="22px" Width="394px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlCCusto" DataValueField="CENTRO_CUSTO" DataTextField="DESC_CENTRO_CUSTO"
                                    Height="22px" Width="244px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Lançamento
                            </td>
                            <td colspan="5">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtLancamento" runat="server" Text="" Width="100px" CssClass="alinharDireita" onkeypress="return fnValidarNumero(event);"></asp:TextBox>
                            </td>
                            <td colspan="5">
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" />
                                &nbsp;&nbsp;
                                <asp:Label ID="labErro" runat="server" Text="" ForeColor="red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 0;" colspan="6">
                                <fieldset>
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvLancamento" runat="server" Width="100%" AutoGenerateColumns="False" ForeColor="#333333"
                                            Style="background: white" OnRowDataBound="gvLancamento_RowDataBound" OnDataBound="gvLancamento_DataBound"
                                            OnSorting="gvLancamento_Sorting"
                                            ShowFooter="true" AllowSorting="true">
                                            <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" />
                                            <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Left" Font-Bold="true" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btEditar" runat="server" Height="15px" Width="15px" ImageUrl="~/Image/edit.jpg"
                                                            OnClick="btEditar_Click" ToolTip="Editar" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tipo" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller"
                                                    SortExpression="TIPO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTipo" runat="server" Text='<%#Bind("TIPO") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Filial" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller"
                                                    SortExpression="FILIAL">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFilial" runat="server" Text='<%#Bind("FILIAL") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Lançamento" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="60px"
                                                    SortExpression="LANCAMENTO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litLancamento" runat="server" Text='<%#Bind("LANCAMENTO") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="35px"
                                                    SortExpression="ITEM">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litItem" runat="server" Text='<%#Bind("ITEM") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Conta Contábil" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller"
                                                    SortExpression="DESC_CONTA">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litContaContabil" runat="server" Text='<%#Bind("DESC_CONTA") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Centro Custo" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="120px"
                                                    SortExpression="DESC_CENTRO_CUSTO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litCentroCusto" runat="server" Text='<%#Bind("DESC_CENTRO_CUSTO") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Data" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller"
                                                    SortExpression="DATA_LANCAMENTO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litData" runat="server" Text='<%#Bind("DATA_LANCAMENTO") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Débito" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller" HeaderStyle-Width="95px"
                                                    SortExpression="DEBITO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDebito" runat="server" Text='<%#Bind("DEBITO") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Histórico" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Size="Smaller"
                                                    SortExpression="HISTORICO">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litHistorico" runat="server" Text='<%#Bind("HISTORICO") %>'></asp:Literal>
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
