<%@ Page Title="Mão de Obra Direta" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="dre_dre_custo_fixo.aspx.cs" Inherits="Relatorios.dre_dre_custo_fixo"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinharDireita {
            text-align: right;
        }
    </style>
    <script src="../../js/js.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo Acompanhamento Mensal&nbsp;&nbsp;>&nbsp;&nbsp;DRE&nbsp;&nbsp;>&nbsp;&nbsp;Mão de Obra Direta&nbsp;&nbsp;</span>
                <div style="float: right; padding: 0;">
                    <a href="javascript: window.close();" class="alink" title="Voltar">Fechar</a>
                </div>
            </div>
            <hr />
            <div class="accountInfo">
                <fieldset>
                    <legend>DRE - Mão de Obra Direta</legend>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>Competência</td>
                            <td>Tipo</td>
                            <td>Valor Total</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">
                                <asp:TextBox ID="txtCompetencia" runat="server" Width="140px" ReadOnly="true" Text=""></asp:TextBox>
                            </td>
                            <td style="width: 150px;">
                                <asp:TextBox ID="txtTipo" runat="server" Width="140px" ReadOnly="true" Text=""></asp:TextBox>
                            </td>
                            <td style="width: 150px;">
                                <asp:TextBox ID="txtValorTotal" runat="server" Width="140px" ReadOnly="true" Text=""></asp:TextBox>
                            </td>
                            <td>&nbsp;
                                <asp:HiddenField ID="hidMes" runat="server" />
                                <asp:HiddenField ID="hidAno" runat="server" />
                                <asp:HiddenField ID="hidTipo" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 0;" colspan="4">
                                <fieldset>
                                    <legend>Funcionários</legend>
                                    <div class="rounded_corners">
                                        <asp:GridView ID="gvProducao" runat="server" Width="100%" AutoGenerateColumns="False"
                                            ShowFooter="true" ForeColor="#333333" Style="background: white" OnRowDataBound="gvProducao_RowDataBound"
                                            OnDataBound="gvProducao_DataBound" OnSorting="gvProducao_Sorting" AllowSorting="true">
                                            <HeaderStyle BackColor="Gainsboro" Font-Size="Small" HorizontalAlign="Left"></HeaderStyle>
                                            <FooterStyle BackColor="Gainsboro" HorizontalAlign="Left" Font-Bold="true" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litColuna" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="FUNCIONARIO" HeaderText="Funcionário" HeaderStyle-HorizontalAlign="Left" SortExpression="FUNCIONARIO" />
                                                <asp:BoundField DataField="CARGO" HeaderText="Cargo" HeaderStyle-HorizontalAlign="Left" SortExpression="CARGO" />
                                                <asp:BoundField DataField="TIPOVALOR" HeaderText="Tipo" HeaderStyle-HorizontalAlign="Left" SortExpression="TIPOVALOR" />
                                                <asp:TemplateField HeaderText="Valor" HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="litValor" runat="server"></asp:Literal>
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
