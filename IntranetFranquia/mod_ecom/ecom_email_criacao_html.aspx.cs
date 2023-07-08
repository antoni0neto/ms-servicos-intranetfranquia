using DAL;
using Relatorios.mod_ecom.emailMKT;
using Relatorios.mod_ecom.mag;
using Relatorios.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class ecom_email_criacao_html : System.Web.UI.Page
    {

        EcomController ecomController = new EcomController();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["em"] == null || Request.QueryString["em"] == "" ||
                    Session["USUARIO"] == null
                    )
                    Response.Redirect("ecom_menu.aspx");

                var codigoEmail = Convert.ToInt32(Request.QueryString["em"].ToString());

                var emailHTML = MontarEmailMKT(codigoEmail);

                txtHTML.Text = emailHTML;
            }

        }

        private string MontarEmailMKT(int codigoEmail)
        {
            var texto = new StringBuilder();

            var emailCorpo = ecomController.ObterEmailCorpoPorCodigoEmail(codigoEmail);

            foreach (var em in emailCorpo)
            {
                var conteudo = emailMKT.MontarConteudo(em.CODIGO, em.ECOM_EMAIL_BLOCO, em.ECOM_EMAIL_BLOCO1.HTML, true);
                texto.AppendLine(conteudo);
            }

            return texto.ToString();
        }


    }
}


