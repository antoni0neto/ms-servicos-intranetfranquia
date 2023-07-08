<%@ Page Title="Comparar Vendas x Cotas" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="gest_loja_compara_cota_ano.aspx.cs" Inherits="Relatorios.gest_loja_compara_cota_ano" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinhamento {
            position: relative;
            float: left;
        }

        .style1 {
            width: 100%;
            background-color: White;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <br />
    <div>
        <span style="font-family: Calibri; font-size: 14px;">Módulo de Gestão&nbsp;&nbsp;>&nbsp;&nbsp;Relatórios
            de Gestão&nbsp;&nbsp;>&nbsp;&nbsp;Comparar Vendas x Cotas</span>
        <div style="float: right; padding: 0;">
            <a href="gest_menu.aspx" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset class="login">
            <legend>Comparar Vendas x Cotas</legend>
            <div style="width: 600px;" class="alinhamento">
                <div style="width: 200px;" class="alinhamento">
                    <label>
                        Data Início:&nbsp;
                    </label>
                    <asp:TextBox ID="txtDataInicioAtual" runat="server" CssClass="textEntry" Height="22px"
                        Width="198px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDataInicioAtual" runat="server" OnSelectionChanged="CalendarDataInicioAtual_SelectionChanged"
                        CaptionAlign="Bottom"></asp:Calendar>
                </div>
                <div style="width: 200px;" class="alinhamento">
                    <label>
                        Data Fim:&nbsp;
                    </label>
                    <asp:TextBox ID="txtDataFimAtual" runat="server" CssClass="textEntry" Height="22px"
                        Width="198px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDataFimAtual" runat="server" OnSelectionChanged="CalendarDataFimAtual_SelectionChanged"
                        CaptionAlign="Bottom"></asp:Calendar>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btPesquisarVendas" Text="Buscar Vendas" Width="120px" OnClick="btPesquisarVendas_Click" />&nbsp;
            <asp:Button runat="server" ID="btGeraExcel" Text="Enviar E-mail" Width="120px" OnClick="btGeraExcel_Click" Enabled="false" />
            <asp:Label ID="labMsgEmail" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
        </div>
        <fieldset class="login">
            <asp:GridView ID="GridViewVendas" runat="server" Width="100%" AutoGenerateColumns="False"
                ShowFooter="true" ForeColor="#333333" OnRowDataBound="GridViewVendas_RowDataBound"
                OnDataBound="GridViewVendas_DataBound" Style="background: white">
                <FooterStyle HorizontalAlign="Center"></FooterStyle>
                <RowStyle HorizontalAlign="Center"></RowStyle>
                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                <Columns>
                    <asp:BoundField DataField="supervisor" HeaderText="Supervisor" />
                    <asp:BoundField DataField="filial" HeaderText="Filial" />
                    <asp:TemplateField HeaderText="Cotas">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralCotas" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Venda Atual">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralVendas" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Atingido">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralAtingido" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Diferença">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralDiferenca" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Venda Ano Anterior">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralVendasAnoAnterior" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="%">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralDiferencaAtingido" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </fieldset>
    </div>
</asp:Content>
