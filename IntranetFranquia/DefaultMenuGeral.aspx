<%@ Page Title="Intranet" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="DefaultMenuGeral.aspx.cs" Inherits="Relatorios.DefaultMenuGeral" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1 {
            width: 100%;
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
                                    <fieldset>
                                        <legend>Menu</legend>
                                        <asp:Menu ID="mnuAdministracao" runat="server">
                                            <Items>
                                                <asp:MenuItem NavigateUrl="~/mod_admin/adm_menu.aspx" Text="1. Módulo de Administração do Site"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_gestao/gest_menu.aspx" Text="2. Módulo de Gestão"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_atacado/atac_menu.aspx" Text="3. Módulo do Atacado"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_financeiro/fin_menu.aspx" Text="4. Módulo do Financeiro"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_contabilidade/contabil_menu.aspx" Text="5. Módulo de Contabilidade"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_fiscal/fisc_menu.aspx" Text="6. Módulo do Fiscal"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_adm_fiscal/admfis_menu.aspx" Text="7. Módulo ADM Nota Fiscal"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_adm_loja/admloj_menu.aspx" Text="8. Módulo Administração de Loja"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_gerenciamento_loja/gerloja_menu.aspx" Text="9. Módulo Gerenciamento de Loja"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_manut_mensal/manutm_menu.aspx" Text="10. Módulo Manutenção Mensal"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_acomp_mensal/acomp_menu.aspx" Text="11. Módulo Acompanhamento Mensal"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_desenvolvimento/desenv_menu.aspx" Text="12. Módulo de Desenvolvimento"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_controle_produto/cprod_menu.aspx" Text="13. Módulo de Controle de Produto"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_producao/prod_menu.aspx" Text="14. Módulo de Produção"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_prodacab/pacab_menu.aspx" Text="15. Módulo de Produto Acabado"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_faccao/facc_menu.aspx" Text="16. Módulo de Facção"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_estoque/estoque_menu.aspx" Text="17. Módulo de Controle de Estoque"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_contagem/cont_menu.aspx" Text="18. Módulo de Contagem"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_seguro/seg_menu.aspx" Text="19. Módulo de Seguros"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_rh/rh_menu.aspx" Text="20. Módulo do RH"></asp:MenuItem>
                                                <asp:MenuItem NavigateUrl="~/mod_ecom/ecom_menu.aspx" Text="21. Módulo do E-Commerce"></asp:MenuItem>
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
