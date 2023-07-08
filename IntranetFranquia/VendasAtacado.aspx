<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VendasAtacado.aspx.cs" Inherits="Relatorios.VendasAtacado" %>

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
        <fieldset class="login">
        </fieldset>
        <fieldset class="login">
            <div style="width: 400px;" class="alinhamento">
                <div style="width: 200px;"  class="alinhamento">
                    <label>Coleção:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlColecao" DataValueField="COLECAO" DataTextField="DESC_COLECAO" Height="22px" 
                        Width="198px" ondatabound="ddlColecao_DataBound"></asp:DropDownList>
                </div>
                <div style="width: 200px;"  class="alinhamento">
                    <label>Data:(AAAAMMDD)&nbsp; </label>
                    <asp:TextBox ID="txtDataCorte" runat="server" Enabled="False" Width="70px"></asp:TextBox>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btBuscar" Text="Buscar" OnClick="btBuscar_Click"/>
        </div>
        <fieldset class="login">
            <asp:GridView id="GridViewJanela1" runat="server" Width="100%" 
                AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333" 
                style="background:white" onrowdatabound="GridViewJanela1_RowDataBound">
	            <FooterStyle HorizontalAlign="Center"></FooterStyle>
	            <RowStyle HorizontalAlign="Center"></RowStyle>
	            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <Columns>
                    <asp:TemplateField HeaderText="Coleção">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralColecao"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ORIGEM" HeaderText="Origem" />
                    <asp:TemplateField HeaderText="Qtde Original">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralTotalQtdeOriginal"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Valor Original">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralTotalValorOriginal"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Qtde Cancelada">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralTotalQtdeCancelada"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Valor Cancelado">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralTotalValorCancelado"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="% Cancelado">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralPercCancelado"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Qtde Real">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralQtReal"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Valor Real">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralVlReal"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Qtde Entregue">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralTotalQtdeEntregue"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Valor Entregue">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralTotalValorEntregue"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="% Entregue">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralPercEntregue"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Qtde Entregar">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralTotalQtdeEntregar"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Valor Entregar">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralTotalValorEntregar"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="% Entregar">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralPercEntregar"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Estoque Inicial">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralEstoque"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="% Retorno">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralPercRetorno"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </fieldset>
        <fieldset class="login">
            <asp:GridView id="GridViewJanela2" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333" 
                                             onrowdatabound="GridViewJanela2_RowDataBound" style="background:white">
	            <FooterStyle HorizontalAlign="Center"></FooterStyle>
	            <RowStyle HorizontalAlign="Center"></RowStyle>
	            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <Columns>
                    <asp:TemplateField HeaderText="Coleção">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralColecao"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ORIGEM" HeaderText="Origem" />
                    <asp:TemplateField HeaderText="Qtde Original">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralTotalQtdeOriginal"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Valor Original">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralTotalValorOriginal"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Qtde Cancelada">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralTotalQtdeCancelada"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Valor Cancelado">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralTotalValorCancelado"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="% Cancelado">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralPercCancelado"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Qtde Real">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralQtReal"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Valor Real">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralVlReal"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Qtde Entregue">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralTotalQtdeEntregue"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Valor Entregue">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralTotalValorEntregue"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="% Entregue">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralPercEntregue"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Qtde Entregar">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralTotalQtdeEntregar"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Valor Entregar">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralTotalValorEntregar"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="% Entregar">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralPercEntregar"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Estoque Inicial">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralEstoque"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="% Retorno">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralPercRetorno"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </fieldset>
        <fieldset class="login">
            <asp:Label ID="Label1" runat="server" ForeColor="Red" 
                Text="Atenção !!! Quando necessário informar data de corte para o administrador do sistema !!!"></asp:Label>
        </fieldset>
    </div>
</asp:Content>
