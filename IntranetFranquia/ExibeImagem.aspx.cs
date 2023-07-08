using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Relatorios
{
    public partial class ExibeImagem : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        LojaController lojaController = new LojaController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CarregaDropDownListFilial();
        }

        private void CarregaDropDownListFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                ddlFilial.DataSource = baseController.BuscaFiliais_Intermediario(usuario);
                ddlFilial.DataBind();
            }
        }

        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Add(new ListItem("Geral", "0"));
            ddlFilial.SelectedValue = "0";
        }

        protected void btBuscarImagens_Click(object sender, EventArgs e)
        {
            CarregaGridViewArquivo(ddlFilial.SelectedValue.ToString().Trim());
        }

        private void CarregaGridViewArquivo(string filial)
        {
            //GridViewArquivo.DataSource = baseController.BuscaImagensLoja(Convert.ToInt32(filial));
            //GridViewArquivo.DataBind();
        }

        protected void GridViewArquivo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            IMAGEM imagem = e.Row.DataItem as IMAGEM;

            if (imagem != null)
            {
                HyperLink hyperLinkOpen = e.Row.FindControl("Open") as HyperLink;

                if (hyperLinkOpen != null)
                    hyperLinkOpen.NavigateUrl = string.Format("~/Upload/{0}", imagem.LOCAL_IMAGEM);
            }
        }
    }
}