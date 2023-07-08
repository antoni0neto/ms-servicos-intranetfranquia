<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BuscaPorCodigoCliente.aspx.cs" Inherits="Relatorios.BuscaPorCodigoCliente" %>

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
            <div style="width: 200px;" class="alinhamento">
                <div style="width: 200px;"  class="alinhamento">
                    <label>Coleção:&nbsp; </label>
                    <asp:DropDownList runat="server" ID="ddlColecao" DataValueField="COLECAO" DataTextField="DESC_COLECAO" Height="22px" 
                        Width="198px" ondatabound="ddlColecao_DataBound"></asp:DropDownList>
                </div>
                <div>
                    <label>Código do Cliente:</label>
                    <asp:TextBox ID="txtCodigoCliente" runat="server" CssClass="pcRight" MaxLength="20" Height="16px" Width="100px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorCodigoCliente" runat="server" SetFocusOnError="true" Display="None" ErrorMessage="Preencha o campo Código do Cliente" ControlToValidate="txtCodigoCliente" ValidationGroup="valores"></asp:RequiredFieldValidator>
                </div>
            </div>
        </fieldset>
        <div>
            <asp:Button runat="server" ID="btBuscar" Text="Buscar" OnClick="btBuscar_Click"/>
        </div>
        <fieldset class="login">
            <asp:GridView id="GridViewAtacado" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true" ForeColor="#333333" style="background:white"
                          onrowdatabound="GridViewAtacado_RowDataBound" ondatabound="GridViewAtacado_DataBound">
	            <FooterStyle HorizontalAlign="Center"></FooterStyle>
	            <RowStyle HorizontalAlign="Center"></RowStyle>
	            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <Columns>
                    <asp:BoundField DataField="codigo_cliente" HeaderText="Código do Cliente" />
                    <asp:BoundField DataField="representante" HeaderText="Representante" />
                    <asp:BoundField DataField="nome_cliente" HeaderText="Nome do Cliente" />
                    <asp:BoundField DataField="status_pedido" HeaderText="Status do Pedido" />
                    <asp:BoundField DataField="credito" HeaderText="Crédito" />
                    <asp:BoundField DataField="data_cadastro" HeaderText="Data do Cadastro" />
                    <asp:BoundField DataField="obs_faturamento" HeaderText="Obs do Faturamento" />
                    <asp:BoundField DataField="obs_geral" HeaderText="Obs" />
                    <asp:BoundField DataField="numero_pedido" HeaderText="Número do Pedido" />
                    <asp:TemplateField HeaderText="Qtde Original">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralQtdeOriginal"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Valor Original">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralValorOriginal"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Qtde Cancelada">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralQtdeCancelada"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Valor Cancelado">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralValorCancelado"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Qtde Entregue">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralQtdeEntregue"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Valor Entregue">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralValorEntregue"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Qtde Embalada">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralQtdeEmbalada"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Qtde a Entregar">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralQtdeAEntregar"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Valor a Entregar">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralValorAEntregar"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Qtde Devolvida">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralQtdeDevolvida"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="LiteralStatus"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </fieldset>
    </div>
</asp:Content>
