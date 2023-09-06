<%@ Page Title="CMV Varejo" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="cmv_varejo.aspx.cs" Inherits="Relatorios.cmv_varejo" MaintainScrollPositionOnPostback="true" %>

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
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo Acompanhamento Mensal&nbsp;&nbsp;>&nbsp;&nbsp;Custo
                    do Produto&nbsp;&nbsp;>&nbsp;&nbsp;Varejo&nbsp;&nbsp;</span>
                <div style="float: right; padding: 0; position: ;">
                    <a href="../acomp_menu.aspx" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="accountInfo">
                <fieldset>
                    <legend>CMV Varejo</legend>
                    <table border="0" width="100%">
                        <tr>
                            <td>
                                Ano
                            </td>
                            <td>
                                Filial
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
                            <td style="width: 207px;">
                                <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                    Height="24px" Width="200px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" Width="100px" />&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="labErro" runat="server" Text="" ForeColor="red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 0;" colspan="3">
                                <fieldset>
                                    <legend>Atacado</legend>
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvVarejo" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvVarejo_RowDataBound"
                                            OnDataBound="gvVarejo_DataBound">
                                            <HeaderStyle BackColor="Gainsboro" Font-Size="Small" HorizontalAlign="Center">
                                            </HeaderStyle>
                                            <RowStyle Font-Bold="true" />
                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Right" Font-Bold="true" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Filial" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFilial" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Janeiro" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJaneiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Fevereiro" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litFevereiro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Março" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMarco" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Abril" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAbril" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Maio" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litMaio" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Junho" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJunho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Julho" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litJulho" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Agosto" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litAgosto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Setembro" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litSetembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Outubro" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litOutubro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Novembro" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litNovembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dezembro" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litDezembro" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
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
