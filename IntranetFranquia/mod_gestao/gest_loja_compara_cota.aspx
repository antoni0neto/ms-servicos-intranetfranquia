<%@ Page Title="Farol (Vendas x Lojas)" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="gest_loja_compara_cota.aspx.cs" Inherits="Relatorios.gest_loja_compara_cota" %>

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
            de Gestão&nbsp;&nbsp;>&nbsp;&nbsp;Farol (Vendas x Lojas)</span>
        <div style="float: right; padding: 0;">
            <a href="#" id="hrefVoltar" runat="server" class="alink" title="Voltar">Voltar</a>
        </div>
    </div>
    <hr />
    <div class="accountInfo">
        <fieldset class="login">
            <legend>Farol (Vendas x Lojas)</legend>
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
            <asp:Label ID="labMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
        </div>
        <fieldset class="login">
            <asp:GridView ID="GridViewVendas" runat="server" Width="100%" AutoGenerateColumns="False"
                ShowFooter="true" ForeColor="#333333" OnRowDataBound="GridViewVendas_RowDataBound"
                OnDataBound="GridViewVendas_DataBound" Style="background: white">
                <FooterStyle HorizontalAlign="Center"></FooterStyle>
                <RowStyle HorizontalAlign="Center"></RowStyle>
                <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                <Columns>
                    <asp:BoundField DataField="filial" HeaderText="Filial" />
                    <asp:TemplateField HeaderText="Atingido">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralPercentual" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="gerente" HeaderText="Gerente" />
                    <asp:BoundField DataField="supervisor" HeaderText="Supervisor" />
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralSobe" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </fieldset>
        <table border="1" width="100%" class="style1">
            <tr>
                <td style="width: 900px">
                    <fieldset class="login">
                        <asp:GridView ID="GridViewFarol" runat="server" Width="100%" AutoGenerateColumns="False"
                            ForeColor="#333333" Style="background: white" OnDataBound="GridViewFarol_DataBound">
                            <FooterStyle HorizontalAlign="Center"></FooterStyle>
                            <RowStyle HorizontalAlign="Center"></RowStyle>
                            <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                            <Columns>
                                <asp:BoundField DataField="cor" HeaderText="Cor" />
                                <asp:BoundField DataField="descricao" HeaderText="Descricao" />
                                <asp:BoundField DataField="descricao_completa" HeaderText="Descricao Completa" />
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                </td>
                <td>
                    <fieldset class="login">
                        <asp:GridView ID="GridViewResumo" runat="server" Width="500px" AutoGenerateColumns="False"
                            ForeColor="#333333" Style="background: white" OnRowDataBound="GridViewResumo_RowDataBound"
                            OnDataBound="GridViewResumo_DataBound">
                            <FooterStyle HorizontalAlign="Center"></FooterStyle>
                            <RowStyle HorizontalAlign="Center"></RowStyle>
                            <HeaderStyle BackColor="PeachPuff" HorizontalAlign="Center"></HeaderStyle>
                            <Columns>
                                <asp:BoundField DataField="supervisor" HeaderStyle-Width="140px" HeaderText="Supervisor"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderText="Atingido">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="litAtingido" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="qtde_verde" HeaderStyle-Width="50px" HeaderText="" />
                                <asp:BoundField DataField="qtde_amarelo" HeaderStyle-Width="50px" HeaderText="" />
                                <asp:BoundField DataField="qtde_vermelho" HeaderStyle-Width="50px" HeaderText="" />
                                <asp:BoundField DataField="qtde_roxo" HeaderStyle-Width="50px" HeaderText="" />
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
