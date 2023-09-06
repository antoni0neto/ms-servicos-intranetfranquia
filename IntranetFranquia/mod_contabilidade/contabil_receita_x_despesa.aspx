<%@ Page Title="Receitas X Despesas" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="contabil_receita_x_despesa.aspx.cs" Inherits="Relatorios.contabil_receita_x_despesa" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento {
            position: relative;
            float: left;
        }
    </style>
    <script src="../../js/js.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../../Image/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "../../Image/plus.png");
            $(this).closest("tr").next().remove();
        });

    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Always" EnableViewState="true">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Contabilidade&nbsp;&nbsp;>&nbsp;&nbsp;Conciliação&nbsp;&nbsp;>&nbsp;&nbsp;Receitas X Despesas&nbsp;&nbsp;</span>
                <div style="float: right; padding: 0;">
                    <a href="contabil_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="accountInfo">
                <fieldset>
                    <legend>Receitas X Despesas</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>Ano
                            </td>
                            <td>Matriz Contábil
                            </td>
                            <td>Tipo Conta
                            </td>
                            <td>Conta Contábil
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">
                                <asp:DropDownList runat="server" ID="ddlAno" Height="24px" Width="144px" DataTextField="ANO"
                                    DataValueField="ANO">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 250px;">
                                <asp:DropDownList runat="server" ID="ddlMatrizContabil" Height="24px" Width="244px" DataTextField="NOME_CLIFOR" DataValueField="COD_CLIFOR"></asp:DropDownList>
                            </td>
                            <td style="width: 250px;">
                                <asp:DropDownList runat="server" ID="ddlTipoConta" Height="24px" Width="244px" DataTextField="DESC_CONTA_TIPO" DataValueField="TIPO_CONTA" OnSelectedIndexChanged="ddlTipoConta_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 300px;">
                                <asp:DropDownList runat="server" ID="ddlContaContabil" Height="24px" Width="294px" DataTextField="DESC_CONTA" DataValueField="CONTA_CONTABIL">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" />&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="labErro" runat="server" Text="" ForeColor="red"></asp:Label>
                            </td>
                            <td style="text-align: right;">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 0;" colspan="6">
                                <fieldset>
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvClassificacao" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CellPadding="0" CellSpacing="5" ForeColor="#333333" Style="background: white"
                                            BackColor="Gainsboro" BorderColor="Gainsboro" OnRowDataBound="gvClassificacao_RowDataBound"
                                            OnDataBound="gvClassificacao_DataBound" ShowFooter="true">
                                            <HeaderStyle BackColor="Gainsboro" Height="22px" HorizontalAlign="Center"></HeaderStyle>
                                            <FooterStyle BackColor="Gainsboro" Height="22px" HorizontalAlign="Center" Font-Bold="true"></FooterStyle>
                                            <RowStyle Font-Bold="true" Height="24px" BackColor="GhostWhite" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Left" ItemStyle-BorderWidth="0">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litClassificacao" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                    ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal>
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
