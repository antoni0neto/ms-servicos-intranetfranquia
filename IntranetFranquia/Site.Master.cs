using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lknAgoraRede.Visible = false;
            lknAgoraSuper.Visible = false;
            //lknGincana.Visible = false;

            lnkLogin.Visible = false;
            lknVoltar.Visible = false;

            //SE NAO FOR PRODUÇÃO
            if (Constante.Ambiente != "P")
                labAmbiente.Text = "Ambiente de HOMOLOGAÇÃO";

            if (!Request.Url.ToString().Contains("Login.aspx"))
            {
                // renova session
                if (Session["USUARIO"] == null)
                {
                    RefreshSession();
                    //Response.Redirect("~/Login.aspx");
                }

                var usuario = (USUARIO)Session["USUARIO"];

                lknAgoraRede.Visible = true;
                lknAgoraSuper.Visible = true;
                //lknGincana.Visible = true;
                lnkLogin.Visible = true;
                lknVoltar.Visible = true;

                if (usuario.CODIGO_PERFIL == 55 || usuario.CODIGO_PERFIL == 56 || usuario.CODIGO_PERFIL == 58)
                {
                    lknAgoraRede.Visible = false;
                    lknAgoraSuper.Visible = false;
                    //lknGincana.Visible = false;
                }


            }
        }

        protected void lknVoltar_Click(object sender, EventArgs e)
        {

            var usuario = (USUARIO)Session["USUARIO"];

            switch (usuario.CODIGO_PERFIL)
            {
                case 1:
                    Response.Redirect("~/menu/menu.aspx");
                    break;
                case 2:
                    Response.Redirect("~/DefaultFilial.aspx");
                    break;
                case 3:
                    Response.Redirect("~/DefaultFilial.aspx");
                    break;
                case 4:
                case 47:
                    Response.Redirect("~/mod_atacado/atac_menu.aspx");
                    break;
                case 5:
                    Response.Redirect("~/mod_financeiro/fin_menu.aspx");
                    break;
                case 6:
                    Response.Redirect("~/mod_estoque/estoque_menu.aspx");
                    break;
                case 8:
                    Response.Redirect("~/DefaultCaixaFilial.aspx");
                    break;
                case 10:
                    Response.Redirect("~/DefaultAdmNF.aspx");
                    break;
                case 11:
                    Response.Redirect("~/mod_rh/rh_menu.aspx");
                    break;
                case 12:
                    Response.Redirect("~/DefaultGestor.aspx");
                    break;
                case 13:
                case 35:
                    Response.Redirect("~/mod_fiscal/fisc_menu.aspx");
                    break;
                case 15:
                    Response.Redirect("~/DefaultMenuImportacao.aspx");
                    break;
                case 16:
                    Response.Redirect("~/DefaultMenuAdmSite.aspx");
                    break;
                case 17:
                    Response.Redirect("~/mod_contabilidade/contabil_fin_amensal_menu.aspx");
                    break;
                case 18:
                    Response.Redirect("~/mod_producao/prod_menu.aspx");
                    break;
                case 19:
                    Response.Redirect("~/mod_desenvolvimento/desenv_menu.aspx");
                    break;
                case 20:
                    Response.Redirect("~/mod_desenvolvimento/desenv_menu.aspx");
                    break;
                case 21:
                    Response.Redirect("~/mod_juridico/jur_menu.aspx");
                    break;
                case 24:
                case 34:
                    Response.Redirect("~/mod_contabilidade/contabil_menu.aspx");
                    break;
                case 25:
                    Response.Redirect("~/mod_contabilidade/contabil_financeiro_menu.aspx");
                    break;
                case 26:
                case 44:
                case 45:
                case 46:
                    Response.Redirect("~/mod_faccao/facc_menu.aspx");
                    break;
                case 27:
                    Response.Redirect("~/mod_financeiro/fin_prod_fin_menu.aspx");
                    break;
                case 28:
                    Response.Redirect("~/mod_faccao/facc_menu_relatorio.aspx");
                    break;
                case 29:
                    Response.Redirect("~/mod_seguro/seg_menu.aspx");
                    break;
                case 30:
                    Response.Redirect("~/mod_controle_produto/cprod_menu.aspx");
                    break;
                case 31:
                    Response.Redirect("~/mod_estoque/estoque_adm_facc_menu.aspx");
                    break;
                case 32:
                    Response.Redirect("~/mod_controle_produto/cprod_producao_menu.aspx");
                    break;
                case 33:
                    Response.Redirect("~/mod_faccao_adm/faccadm_menu.aspx");
                    break;
                case 36:
                    Response.Redirect("~/mod_contagem/cont_menu.aspx");
                    break;
                case 37:
                    Response.Redirect("~/mod_contabilidade/contabil_faccao_menu.aspx");
                    break;
                case 38:
                    Response.Redirect("~/mod_contabilidade/contabil_seguro_dre_menu.aspx");
                    break;
                case 39:
                    Response.Redirect("~/mod_faccao/facc_menu_producao.aspx");
                    break;
                case 40:
                    Response.Redirect("~/mod_estoque/estoque_cont_facc_prod_nf_menu.aspx");
                    break;
                case 41:
                    Response.Redirect("~/mod_acomp_mensal/acomp_menu.aspx");
                    break;
                case 42:
                    Response.Redirect("~/mod_producao/prod_menu_cad.aspx");
                    break;
                case 43:
                    Response.Redirect("~/mod_ecom/ecom_menu.aspx");
                    break;
                case 49:
                    Response.Redirect("~/mod_marketing/mkt_menu.aspx");
                    break;
                case 50:
                    Response.Redirect("~/mod_marketing/mkt_ger_loja_menu.aspx");
                    break;
                case 51:
                    Response.Redirect("~/mod_ecom/ecom_mkt_desenv_menu.aspx");
                    break;
                case 52:
                    Response.Redirect("~/mod_ecom/ecom_recepcao_menu.aspx");
                    break;
                case 53:
                    Response.Redirect("~/mod_acomp_mensal/dre/dre_dre_novo.aspx");
                    break;
                case 54:
                    Response.Redirect("~/mod_faccao/facc_menu_fiscal.aspx");
                    break;
                case 55:
                    Response.Redirect("~/mod_ecom/ecom_menu_externo.aspx");
                    break;
                case 56:
                    Response.Redirect("~/mod_atacado/atac_menu_externo.aspx");
                    break;
                case 57:
                    Response.Redirect("~/mod_gerenciamento_loja/gerloja_wapp_niver.aspx");
                    break;
                case 58:
                    Response.Redirect("~/photo_select/cad_phXHD123/photo_menu.aspx");
                    break;

            }

        }

        protected void lnkLogin_Click(object sender, EventArgs e)
        {
            Session["USUARIO"] = null;

            Response.Redirect("~/Login.aspx");
        }

        private void RefreshSession()
        {
            if (Session["USUARIO"] == null)
            {
                HttpCookie cookieUser = Request.Cookies["_resu"];
                HttpCookie cookiePass = Request.Cookies["_resuss"];
                var usuario = UsuarioCookie.RefreshSession(cookieUser, cookiePass);

                if (usuario == null)
                    Response.Redirect("~/Login.aspx");

                Session["USUARIO"] = usuario;
            }
        }


    }
}
