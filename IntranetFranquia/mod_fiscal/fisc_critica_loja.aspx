<%@ Page Title="Vendas Divergência Cliente" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="fisc_critica_loja.aspx.cs" Inherits="Relatorios.fisc_critica_loja" %>

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

        .style2 {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div>
        <br />
        <div>
            <span style="font-family: Calibri; font-size: 14px;">Módulo do Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Controle Fiscal&nbsp;&nbsp;>&nbsp;&nbsp;Vendas Divergência Cliente</span>
            <div style="float: right; padding: 0;">
                <a href="fisc_menu.aspx" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
            </div>
        </div>
        <hr />
    </div>
    <div class="accountInfo">
        <fieldset class="login">
            <legend>Vendas Divergência Cliente</legend>
            <div style="width: 400px;" class="alinhamento">
                <div style="width: 200px;" class="alinhamento">
                    <label>Data Início&nbsp; </label>
                    <asp:TextBox ID="TextBoxDataInicio" runat="server" CssClass="textEntry" Height="22px" Width="198px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDataInicio" runat="server" OnSelectionChanged="CalendarDataInicio_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                </div>
                <div style="width: 200px; margin-left: 0px;" class="alinhamento">
                    <label>Data Fim&nbsp; </label>
                    <asp:TextBox ID="TextBoxDataFim" runat="server" CssClass="textEntry" Height="22px" Width="198px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDataFim" runat="server" OnSelectionChanged="CalendarDataFim_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btPesquisar" Text="Buscar Vendas" OnClick="btPesquisar_Click" />
            <asp:ValidationSummary ID="ValidationSummaryCritica" runat="server" ShowMessageBox="true" ShowSummary="false" />
        </div>
        <fieldset class="login">
            <table border="1" class="style1">
                <tr>
                    <asp:GridView ID="GridViewCritica" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333"
                        OnRowDataBound="GridViewCritica_RowDataBound" Style="background: white">
                        <FooterStyle HorizontalAlign="Center"></FooterStyle>
                        <RowStyle HorizontalAlign="Center"></RowStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="Número da Nota">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="LiteralNumeroNF" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Data de Emissão">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="LiteralDataNF" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Valor">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="LiteralValorNF" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nome do Cliente">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="LiteralNomeCliente" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CPF">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="LiteralCpf" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Código da Loja">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="LiteralCodigoLoja" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Descrição da Loja">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="LiteralDescricaoLoja" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
