<%@ Page Title="Resultado de Loja" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="dre_loja_resultado.aspx.cs" Inherits="Relatorios.dre_loja_resultado"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento
        {
            position: relative;
            float: left;
        }
    </style>
    <script src="../../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo Acompanhamento Mensal&nbsp;&nbsp;>&nbsp;&nbsp;DRE&nbsp;&nbsp;>&nbsp;&nbsp;Resultado
            de Loja&nbsp;&nbsp;</span>
        <div style="float: right; padding: 0; position: ;">
            <a href="../acomp_menu.aspx" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset>
            <legend>Resultado de Loja</legend>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>
                        Ano
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 153px;">
                        <asp:DropDownList runat="server" ID="ddlAno" Height="24px" Width="150px" DataTextField="ANO"
                            DataValueField="ANO">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" />&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="labErro" runat="server" Text="" ForeColor="red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="padding: 0;" colspan="2">
                        <fieldset>
                            <div class="rounded_corners">
                                <% //RESULTADO LOJA %>
                                <asp:GridView ID="gvResultado" runat="server" Width="100%" AutoGenerateColumns="False"
                                    ForeColor="#333333" BorderColor="GradientActiveCaption" OnRowDataBound="gvResultado_RowDataBound"
                                    OnDataBound="gvResultado_DataBound" ShowFooter="true">
                                    <HeaderStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="GradientActiveCaption" HorizontalAlign="Center" Font-Bold="true" />
                                    <AlternatingRowStyle BackColor="GhostWhite" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-VerticalAlign="Middle" ItemStyle-BorderWidth="1px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Filial" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-BorderWidth="0" ItemStyle-Width="165px">
                                            <ItemTemplate>
                                                <asp:Literal ID="litFilial" runat="server"></asp:Literal>
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
                                <% // FIM RESULTADO%>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
