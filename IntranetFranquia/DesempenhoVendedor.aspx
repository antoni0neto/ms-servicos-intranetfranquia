<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DesempenhoVendedor.aspx.cs" Inherits="Relatorios.DesempenhoVendedor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .alinhamento
        {
            position: relative;
            float: left;
        }
        .style1
        {
            width: 100%;
            background-color:White;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="accountInfo">
        <fieldset>
        </fieldset>
        <fieldset class="login">
            <legend>Critérios de Vendas em Loja</legend>
            <div style="width: 600px;" class="alinhamento">
                <div style="width: 200px;" class="alinhamento">
                    <label>Data Início:&nbsp; </label>
                    <asp:TextBox ID="TextBoxDataInicio" runat="server" CssClass="textEntry" Height="22px" Width="198px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDataInicio" runat="server" OnSelectionChanged="CalendarDataInicio_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                </div>
                <div style="width: 200px;" class="alinhamento">
                    <label>Data Fim:&nbsp; </label>
                    <asp:TextBox ID="TextBoxDataFim" runat="server" CssClass="textEntry" Height="22px" Width="198px"></asp:TextBox>
                    <asp:Calendar ID="CalendarDataFim" runat="server" OnSelectionChanged="CalendarDataFim_SelectionChanged" CaptionAlign="Bottom"></asp:Calendar>
                </div>
                <div style="width: 200px;"  class="alinhamento">
                    <label>Filial:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlFilial" DataValueField="COD_FILIAL" DataTextField="FILIAL" Height="26px" Width="200px" ondatabound="ddlFilial_DataBound"></asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="ButtonPesquisarVendas" Text="Buscar Vendas" OnClick="ButtonPesquisarVendas_Click"/>
            <asp:ValidationSummary ID="ValidationSummaryVendas" runat="server" ShowMessageBox="true" ShowSummary="false" />
        </div>
        <fieldset class="login">
        <legend>Desempenho de Vendas na Loja</legend>
        <table border="1" class="style1">
            <tr>
                <asp:GridView id="GridViewDesempenho" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333"
                                OnRowDataBound="GridViewDesempenho_RowDataBound" ondatabound="GridViewDesempenho_DataBound" style="background:white">
	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="Nome_Vendedor" HeaderText="Nome do Funcionário" />
                        <asp:BoundField DataField="Data_Ativacao" HeaderText="Data Ativação" />
                        <asp:BoundField DataField="Data_Desativacao" HeaderText="Data Desativação" />
                        <asp:TemplateField HeaderText="PA">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralPA"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Media Qtde Peças">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralMediaAte"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Valor Ticket Medio">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralMediaVl"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qtde 1">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralPerc11"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qtde 1/2">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralPerc1"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qtde 4/5">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralPerc5"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nota">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralNota"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ranking">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralRanking"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </tr>
        </table>
        </fieldset>
        <fieldset class="login">
        <legend>Desempenho de Vendas do Grupo</legend>
        <table border="1" class="style1">
            <tr>
                <asp:GridView id="GridViewGrupo" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333"
                                OnRowDataBound="GridViewGrupo_RowDataBound" style="background:white">
	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="PA">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralPA"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Media Qtde Peças">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralMediaAte"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Valor Ticket Medio">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralMediaVl"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qtde 1">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralPerc11"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qtde 1/2">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralPerc1"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qtde 4/5">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralPerc5"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </tr>
        </table>
        </fieldset>
        <fieldset class="login">
        <legend>Desempenho de Vendas da Rede</legend>
        <table border="1" class="style1">
            <tr>
                <asp:GridView id="GridViewRede" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333"
                                OnRowDataBound="GridViewRede_RowDataBound" style="background:white">
	                <FooterStyle HorizontalAlign="Center"></FooterStyle>
	                <RowStyle HorizontalAlign="Center"></RowStyle>
	                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="PA">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralPA"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Media Qtde Peças">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralMediaAte"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Valor Ticket Medio">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralMediaVl"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qtde">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralPerc11"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qtde 1/2">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralPerc1"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qtde 4/5">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralPerc5"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </tr>
        </table>
        </fieldset>
    </div>
</asp:Content>
