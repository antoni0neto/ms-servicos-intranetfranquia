<%@ Page Title="Verificar Fechamento de Caixa" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="VerificarFechamentoCaixa.aspx.cs" Inherits="Relatorios.VerificarFechamentoCaixa" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento
        {
            position: relative;
            float: left;
        }
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">
                    <asp:Label ID="labTitulo" runat="server" Text="Administração de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Fechamento&nbsp;&nbsp;>&nbsp;&nbsp;Verificar Fechamento Caixa"></asp:Label></span>
                <div style="float: right; padding: 0;">
                    <a href="~/mod_adm_loja/admloj_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">
                        Voltar</a>
                </div>
            </div>
            <hr />
            <div class="accountInfo">
                <fieldset class="login">
                    <legend>Verificar Fechamento Caixa</legend>
                    <div style="width: 600px;" class="alinhamento">
                        <div style="width: 200px;" class="alinhamento">
                            <label>
                                Data Início:&nbsp;
                            </label>
                            <asp:TextBox ID="TextBoxDataInicio" runat="server" CssClass="textEntry" Height="22px"
                                Width="198px"></asp:TextBox>
                            <asp:Calendar ID="CalendarDataInicio" runat="server" OnSelectionChanged="CalendarDataInicio_SelectionChanged"
                                CaptionAlign="Bottom"></asp:Calendar>
                        </div>
                        <div style="width: 200px;" class="alinhamento">
                            <label>
                                Data Fim:&nbsp;
                            </label>
                            <asp:TextBox ID="TextBoxDataFim" runat="server" CssClass="textEntry" Height="22px"
                                Width="198px"></asp:TextBox>
                            <asp:Calendar ID="CalendarDataFim" runat="server" OnSelectionChanged="CalendarDataFim_SelectionChanged"
                                CaptionAlign="Bottom"></asp:Calendar>
                        </div>
                        <div style="width: 200px;" class="alinhamento">
                            <label>
                                Filial:&nbsp;
                            </label>
                            <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL"
                                Height="26px" Width="200px" OnDataBound="ddlFilial_DataBound">
                            </asp:DropDownList>
                            &nbsp;&nbsp;&nbsp;<asp:Label ID="labErroFilial" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </div>
                    </div>
                </fieldset>
                <div>
                    <asp:Button runat="server" ID="ButtonPesquisarFechamento" Text="Buscar Fechamento de Caixa"
                        OnClick="ButtonPesquisarFechamento_Click" />
                </div>
                <table border="0" class="style1">
                    <tr>
                        <td>
                            <asp:GridView ID="GridViewFechamento" runat="server" Width="100%" CssClass="DataGrid_Padrao"
                                PageSize="1000" AllowPaging="True" AutoGenerateColumns="False" ShowFooter="true"
                                OnRowDataBound="GridViewFechamento_RowDataBound" OnDataBound="GridViewFechamento_DataBound">
                                <FooterStyle CssClass="DataGrid_Footer"></FooterStyle>
                                <SelectedRowStyle CssClass="DataGrid_SelectedItem"></SelectedRowStyle>
                                <AlternatingRowStyle CssClass="DataGrid_AlternatingItem"></AlternatingRowStyle>
                                <RowStyle HorizontalAlign="Center"></RowStyle>
                                <HeaderStyle HorizontalAlign="Center" CssClass="DataGrid_Header"></HeaderStyle>
                                <Columns>
                                    <asp:TemplateField HeaderText="Filial">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="LiteralFilial" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="data_fechamento" HeaderText="Data do Movimento" />
                                    <asp:BoundField DataField="valor_dinheiro" HeaderText="Valor em Dinheiro" />
                                    <asp:BoundField DataField="valor_retirada" HeaderText="Valor da Retirada" />
                                    <asp:BoundField DataField="valor_despesas" HeaderText="Valor das Despesas" />
                                    <asp:TemplateField HeaderText="Saldo Dinheiro">
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="LiteralSaldoDinheiro" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button runat="server" ID="ButtonPequisar" Text="Ver" OnClick="ButtonPesquisar_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="data" HeaderText="Data do Fechamento" />
                                    <%--                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="ButtonDeletar" Text="Deletar" OnClick="ButtonDeletar_Click" OnClientClick="javascript: return confirm('Tem certeza que deseja excluir?');" />
                                </ItemTemplate>
                            </asp:TemplateField>
                                    --%>
                                </Columns>
                                <PagerStyle CssClass="DataGrid_Pager"></PagerStyle>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
