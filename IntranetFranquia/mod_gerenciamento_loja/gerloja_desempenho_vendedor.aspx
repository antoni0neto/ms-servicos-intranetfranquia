<%@ Page Title="Desempenho de Vendas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="gerloja_desempenho_vendedor.aspx.cs" Inherits="Relatorios.gerloja_desempenho_vendedor" %>

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
    <div class="accountInfo">
        <br />
        <div>
            <span style="font-family: Calibri; font-size: 14px;">Módulo de Gerenciamento de Loja&nbsp;&nbsp;>&nbsp;&nbsp;Vendedores&nbsp;&nbsp;>&nbsp;&nbsp;Desempenho de Vendas</span>
            <div style="float: right; padding: 0;">
                <a href="gerloja_menu.aspx" class="alink" title="Voltar">Voltar</a>
            </div>
        </div>
        <hr />
        <br />
        <fieldset class="login">
            <legend>Desempenho de Vendas</legend>
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
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="ButtonPesquisarVendas" Text="Buscar Vendas" OnClick="ButtonPesquisarVendas_Click" />
            <asp:ValidationSummary ID="ValidationSummaryVendas" runat="server" ShowMessageBox="true"
                ShowSummary="false" />
        </div>
        <asp:Panel ID="pnlFilial" runat="server">
            <fieldset class="login">
                <table border="1" class="style1">
                    <tr>
                        <asp:GridView ID="GridViewDesempenho" runat="server" Width="100%" AutoGenerateColumns="False"
                            ShowFooter="true" ForeColor="#333333" OnPageIndexChanging="GridViewDesempenho_PageIndexChanging"
                            OnRowDataBound="GridViewDesempenho_RowDataBound" OnDataBound="GridViewDesempenho_DataBound"
                            Style="background: white">
                            <FooterStyle HorizontalAlign="Center"></FooterStyle>
                            <RowStyle HorizontalAlign="Center"></RowStyle>
                            <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                            <Columns>
                                <asp:BoundField DataField="Nome_Vendedor" HeaderText="Nome do Funcionário" />
                                <asp:BoundField DataField="Vendas_0" HeaderText="Zero Peça" />
                                <asp:TemplateField HeaderText="Por cento">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralPerc0" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Vendas_1" HeaderText="Uma Peça" />
                                <asp:TemplateField HeaderText="Por cento">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralPerc1" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Vendas_2" HeaderText="Duas Peças" />
                                <asp:TemplateField HeaderText="Por cento">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralPerc2" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Vendas_3" HeaderText="Três Peças" />
                                <asp:TemplateField HeaderText="Por cento">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralPerc3" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Vendas_4" HeaderText="Quatro Peças" />
                                <asp:TemplateField HeaderText="Por cento">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralPerc4" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Vendas_5" HeaderText="Cinco Peças" />
                                <asp:TemplateField HeaderText="Por cento">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralPerc5" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PA">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralPA" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Media Atendido">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralMediaAte" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ticket Medio">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralMediaVl" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="%$" Visible="false">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralPercVl" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nota">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralNota" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Com Link">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralComLink" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Vendas_Consumidor" HeaderText="Sem Link" />
                                <asp:TemplateField HeaderText="Total Peças">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralTotalPcs" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Atendido">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralTotalAte" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nota Final">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralNotaFinal" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Numero_Dias" HeaderText="Número Dias" />
                                <asp:TemplateField HeaderText="Ranking">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralRanking" />
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
                        <asp:GridView ID="GridViewRede" runat="server" Width="100%" AutoGenerateColumns="False"
                            ShowFooter="true" ForeColor="#333333" OnRowDataBound="GridViewRede_RowDataBound"
                            Style="background: white">
                            <FooterStyle HorizontalAlign="Center"></FooterStyle>
                            <RowStyle HorizontalAlign="Center"></RowStyle>
                            <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                            <Columns>
                                <asp:TemplateField HeaderText="PA">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralPA" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Media Qtde Peças">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralMediaAte" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Valor Ticket Medio">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralMediaVl" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qtde">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralPerc11" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qtde 1/2">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralPerc1" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qtde 4/5">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="LiteralPerc5" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </tr>
                </table>
            </fieldset>
        </asp:Panel>
        <asp:Panel ID="pnlGeral" runat="server" Width="100%">
            <fieldset>
                <% //<div style="overflow: scroll; width:1435px;"> %>
                <% //<div style="width: 1770px;"> %>
                <asp:GridView ID="gvDesempenhoGeral" runat="server" Width="100%" AutoGenerateColumns="False"
                    ShowFooter="true" ForeColor="#333333" OnRowDataBound="gvDesempenhoGeral_RowDataBound"
                    OnDataBound="gvDesempenhoGeral_DataBound" Style="background: white">
                    <FooterStyle HorizontalAlign="Center"></FooterStyle>
                    <RowStyle HorizontalAlign="Center"></RowStyle>
                    <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="Filial">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralFilial" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Vendas_0" HeaderText="Zero Peça" HeaderStyle-Width="62px" />
                        <asp:TemplateField HeaderText="Por cento" HeaderStyle-Width="62px">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralPerc0" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Vendas_1" HeaderText="Uma Peça" HeaderStyle-Width="62px" />
                        <asp:TemplateField HeaderText="Por cento" HeaderStyle-Width="62px">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralPerc1" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Vendas_2" HeaderText="Duas Peças" HeaderStyle-Width="70px" />
                        <asp:TemplateField HeaderText="Por cento" HeaderStyle-Width="62px">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralPerc2" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Vendas_3" HeaderText="Três Peças" HeaderStyle-Width="68px" />
                        <asp:TemplateField HeaderText="Por cento" HeaderStyle-Width="62px">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralPerc3" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Vendas_4" HeaderText="Quatro Peças" HeaderStyle-Width="80px" />
                        <asp:TemplateField HeaderText="Por cento" HeaderStyle-Width="62px">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralPerc4" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Vendas_5" HeaderText="Cinco Peças" HeaderStyle-Width="72px" />
                        <asp:TemplateField HeaderText="Por cento" HeaderStyle-Width="62px">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralPerc5" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PA" HeaderStyle-Width="35px">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralPA" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Media Atendido" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralMediaAte" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ticket Medio" HeaderStyle-Width="85px">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralMediaVl" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="%$" Visible="false">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralPercVl" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nota" HeaderStyle-Width="32px">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralNota" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Com Link" HeaderStyle-Width="57px">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralComLink" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Vendas_Consumidor" HeaderText="Sem Link" HeaderStyle-Width="55px" />
                        <asp:TemplateField HeaderText="Total Peças" HeaderStyle-Width="70px">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralTotalPcs" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Atendido" HeaderStyle-Width="95px">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralTotalAte" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nota Final" HeaderStyle-Width="82px">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralNotaFinal" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Numero_Dias" HeaderText="Número Dias" HeaderStyle-Width="80px" />
                        <asp:TemplateField HeaderText="Ranking" HeaderStyle-Width="57px">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="LiteralRanking" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <% //</div> %>
                <% //</div> %>
            </fieldset>
        </asp:Panel>
    </div>
</asp:Content>
