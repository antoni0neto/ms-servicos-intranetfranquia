using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class FinalizadoComSucesso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["erro"] != null)
                LiteralErro.Text = Request.QueryString["erro"].ToString();
        }

        protected void btOk_Click(object sender, EventArgs e)
        {
            if (Session["USUARIO"] != null)
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                if (usuario.CODIGO_PERFIL == 1)                          // Administrador
                    Response.Redirect("~/menu/menu.aspx");
                else if (usuario.CODIGO_PERFIL == 2)                     // Gerente de Loja
                    Response.Redirect("~/DefaultFilial.aspx");
                else if (usuario.CODIGO_PERFIL == 3)                     // Supervisor de Loja
                    Response.Redirect("~/DefaultFilial.aspx");
                else if (usuario.CODIGO_PERFIL == 4)                     // Supervisor de Atacado
                    Response.Redirect("~/DefaultAtacado.aspx");
                else if (usuario.CODIGO_PERFIL == 5)                     // Financeiro
                    Response.Redirect("~/DefaultFinanceiro.aspx");
                else if (usuario.CODIGO_PERFIL == 8)                     // Caixa
                    Response.Redirect("~/DefaultCaixaFilial.aspx");
                else if (usuario.CODIGO_PERFIL == 10)                    // Adm_Nf
                    Response.Redirect("~/DefaultAdmNF.aspx");
                else if (usuario.CODIGO_PERFIL == 11)                    // Rh
                    Response.Redirect("~/DefaultRh.aspx");
                else if (usuario.CODIGO_PERFIL == 12)                    // Gestão
                    Response.Redirect("~/DefaultGestor.aspx");
                else if (usuario.CODIGO_PERFIL == 13)                    // Fiscal
                    Response.Redirect("~/DefaultFiscal.aspx");
                else if (usuario.CODIGO_PERFIL == 15)                    // Importação
                    Response.Redirect("~/DefaultMenuImportacao.aspx");
                else if (usuario.CODIGO_PERFIL == 16)                    // Administrador do Site
                    Response.Redirect("~/DefaultMenuAdmSite.aspx");
                else if (usuario.CODIGO_PERFIL == 26 || usuario.CODIGO_PERFIL == 44 || usuario.CODIGO_PERFIL == 45 || usuario.CODIGO_PERFIL == 46) // FACCAO
                    Response.Redirect("~/mod_faccao/facc_menu.aspx");
                else
                    Response.Redirect("~/Login.aspx");
            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }
        }
    }
}
