<%@ Page Title="Módulo do E-Commerce" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="ecomv2_menu.aspx.cs" Inherits="Relatorios.ecomv2_menu" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1 {
            width: 100%;
            font-family: Calibri;
            font-size: 14px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="login">
        <table border="0" class="style1">
            <tr>
                <td>
                    <fieldset class="login">
                        <legend>Módulo do E-Commerce</legend>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Cadastros</legend>
                                        <asp:Menu ID="mnuCadastro" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_ecomv2/ecomv2_cad_produto.aspx" Text="1. Produto" Enabled="true"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">
                                    <fieldset style="height: 88px;">
                                        <legend>Controle de Clientes</legend>
                                        <asp:Menu ID="mnuCRM" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_ecomv2/ecomv2_pedido_mag.aspx?t=1" Text="1. Pedidos" Enabled="true"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                    <fieldset style="height: 68px;">
                                        <legend>Controle de Emails</legend>
                                        <asp:Menu ID="mnuEmailMkt" runat="server">
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="height: 200px;">
                                        <legend>Controle de Pedido</legend>
                                        <asp:Menu ID="mnuControle" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_ecomv2/ecomv2_cad_produto_faturamento.aspx" Text="4. Faturamento" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecomv2/ecomv2_rastreio_entrega.aspx" Text="6. Rastreio de Pedido" Enabled="true"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Ordenação de Produtos</legend>
                                        <asp:Menu ID="mnuOrdem" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_ecomv2/ecomv2_ordem_produtobloco_catfinal.aspx" Text="5. Atualizar Ordenação Magento" Enabled="true"></asp:MenuItem>

                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Controle de Produtos Relacionados</legend>
                                        <asp:Menu ID="mnuControleProdutoRel" runat="server">
                                            <Items>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="height: 200px;">
                                        <legend>Relatórios</legend>
                                        <asp:Menu ID="mnuRelatorio" runat="server">
                                            <Items>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 80px;">
                                        <legend>Ajuda</legend>
                                        <asp:Menu ID="mnuAjuda" runat="server">
                                            <Items>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                    <fieldset style="height: 76px;">
                                        <legend>Parametrização</legend>
                                        <asp:Menu ID="mnuIntegracao" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_parametros_api.aspx" Text="1. API Pedido"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Estoque</legend>
                                        <asp:Menu ID="mnuEstoque" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_ecomv2/ecomv2_cad_produto_magento.aspx" Text="1. Liberação Estoque"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecomv2/ecomv2_dev_estoque.aspx" Text="2. Devolução Estoque"></asp:MenuItem>

                                                <asp:MenuItem NavigateUrl="~/mod_ecomv2/ecomv2_produto_magento_estoque.aspx" Text="4. Estoque Magento" Enabled="true"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecomv2/ecomv2_produto_magento_estoque_tamanho.aspx" Text="5. Estoque Magento Tamanho" Enabled="true"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="height: 200px;">
                                        <legend>Controle de Preços</legend>
                                        <asp:Menu ID="mnuPreco" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_ecomv2/ecomv2_produto_preco.aspx" Text="1. Remarcação de Preços" Enabled="true"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
