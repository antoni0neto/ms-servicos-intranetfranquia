<%@ Page Title="Módulo de NF/Produção/Facção/Estoque/Contagem" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="estoque_cont_facc_prod_nf_menu.aspx.cs" Inherits="Relatorios.estoque_cont_facc_prod_nf_menu" %>

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
                        <legend>Intranet</legend>
                        <table border="0" class="style1">
                            <tr>
                                <td style="width: 465px;">
                                    <fieldset style="height: 200px;">
                                        <legend>Módulo de NF/Produção/Facção/Estoque/Contagem</legend>
                                        <asp:Menu ID="mnuNFProdFaccEstCont" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_adm_fiscal/admfis_menu.aspx" Text="1. Módulo ADM Nota Fiscal"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_producao/prod_menu.aspx" Text="2. Módulo de Produção"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_faccao/facc_menu.aspx" Text="3. Módulo de Facção"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_estoque/estoque_menu.aspx" Text="4. Módulo de Controle de Estoque"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_contagem/cont_menu.aspx" Text="5. Módulo de Contagem"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_menu.aspx" Text="6. Módulo de Gerenciamento de Loja"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_menu.aspx" Text="7. Módulo de Desenvolvimento"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_prodacab/pacab_menu.aspx" Text="8. Módulo de Produto Acabado"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                    </fieldset>
                                </td>
                                <td style="width: 465px;">&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
