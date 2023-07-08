<%@ Page Title="Verificar Nota de Defeito" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="VerificarNotaDefeito.aspx.cs" Inherits="Relatorios.VerificarNotaDefeito" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .alinhamento {
            position: relative;
            float: left;
        }

        .style1 {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div>
                <span style="font-family: Calibri; font-size: 14px;">Módulo de Produto Acabado&nbsp;&nbsp;>&nbsp;&nbsp;Controle&nbsp;&nbsp;>&nbsp;&nbsp;Entrada de Produto Acabado</span>
                <div style="float: right; padding: 0;">
                    <a href="pacab_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
                </div>
            </div>
            <hr />
            <div class="accountInfo">
                <fieldset class="login">
                    <legend>Selecione</legend>
                    <div style="width: 200px;" class="alinhamento">
                        <div style="width: 200px;" class="alinhamento">
                            <label>Filial:&nbsp; </label>
                            <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="26px"
                                Width="200px" OnDataBound="ddlFilial_DataBound">
                            </asp:DropDownList>
                        </div>
                    </div>
                </fieldset>
                <div>
                    <asp:Button runat="server" ID="btNotaRetirada" Text="Buscar Notas de Defeito" OnClick="btNotaRetirada_Click" />
                </div>
                <table border="1" class="style1">
                    <tr>
                        <td>
                            <asp:GridView ID="GridViewNotaRetirada" runat="server" Width="100%"
                                CssClass="DataGrid_Padrao" PageSize="1000" AllowPaging="True"
                                AutoGenerateColumns="False" ShowFooter="true" OnRowDataBound="GridViewNotaRetirada_RowDataBound"
                                OnDataBound="GridViewNotaRetirada_DataBound">
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
                                    <asp:BoundField DataField="codigo_nota_retirada" HeaderText="Número do Processo" />
                                    <asp:BoundField DataField="numero_nota_cmax" HeaderText="Número da Nota Cmax" />
                                    <asp:BoundField DataField="data_nota_cmax" HeaderText="Data da Nota Cmax" />
                                    <asp:BoundField DataField="numero_nota_hbf" HeaderText="Número da Nota Hbf" />
                                    <asp:BoundField DataField="data_nota_hbf" HeaderText="Data da Nota Hbf" />
                                    <asp:BoundField DataField="numero_nota_calcados" HeaderText="Número Nota Hbf Calçados" />
                                    <asp:BoundField DataField="data_nota_calcados" HeaderText="Data da Nota Hbf Calçados" />
                                    <asp:BoundField DataField="numero_nota_outros" HeaderText="Número da Nota Hbf Outros" />
                                    <asp:BoundField DataField="data_nota_outros" HeaderText="Data da Nota Hbf Outros" />
                                    <asp:BoundField DataField="numero_nota_lugzi" HeaderText="Número da Nota Lugzi" />
                                    <asp:BoundField DataField="data_nota_lugzi" HeaderText="Data da Nota Lugzi" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button runat="server" ID="btAlteraItem" Text="Alterar Item" OnClick="btAlteraItem_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button runat="server" ID="btAlteraNota" Text="Alterar Nota" OnClick="btAlteraNota_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button runat="server" ID="btAtualizaItens" Text="Atualiza Itens" OnClick="btAtualizaItens_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
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
