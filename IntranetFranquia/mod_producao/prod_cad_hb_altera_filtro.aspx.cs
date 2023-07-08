using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Drawing.Drawing2D;
using DAL;
using System.Text;

namespace Relatorios
{
    public partial class prod_cad_hb_altera_filtro : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Valida queryString
                if (Request.QueryString["d"] == null || Request.QueryString["d"] == "")
                    Response.Redirect("prod_menu.aspx");

                string tela = Request.QueryString["d"].ToString();
                if (tela != "1" && tela != "2" && tela != "3")
                    Response.Redirect("prod_menu.aspx");

                if (tela == "1")
                {
                    labTitulo.Text = "HB Alteração";
                    labTituloMenu.Text = "HB Alteração";
                }
                else if (tela == "2")
                {
                    labTitulo.Text = "HB Alteração Corte";
                    labTituloMenu.Text = "HB Alteração Corte";
                }
                else
                {
                    labTitulo.Text = "HB Alteração Ampliação";
                    labTituloMenu.Text = "HB Alteração Ampliação";

                }

                hidTela.Value = tela;
                CarregarColecoes();
            }
        }

        protected void btHBBuscar_Click(object sender, EventArgs e)
        {
            labHBBuscar.Text = "";
            if (ddlColecoesBuscar.SelectedValue == "" || ddlColecoesBuscar.SelectedValue == "0")
            {
                labHBBuscar.Text = "Selecione a Coleção.";
                return;
            }
            if (txtHBBuscar.Text == "" || Convert.ToInt32(txtHBBuscar.Text) <= 0)
            {
                labHBBuscar.Text = "Informe o número do HB.";
                return;
            }

            try
            {

                List<PROD_HB> lstProdHB = new List<PROD_HB>();
                lstProdHB = prodController.ObterNumeroHB(ddlColecoesBuscar.SelectedValue, Convert.ToInt32(txtHBBuscar.Text));

                PROD_HB prod_hb = null;
                prod_hb = lstProdHB.Where(p => p.MOSTRUARIO == ((chkMostruario.Checked) ? 'S' : 'N')).SingleOrDefault();

                if (prod_hb == null)
                {
                    labHBBuscar.Text = "Nenhum HB encontrato. Refaça a sua pesquisa.";
                    return;
                }

                if (prod_hb.STATUS.ToString() == "E")
                {
                    labHBBuscar.Text = "Este HB foi EXCLUÍDO. Refaça a sua pesquisa.";
                    return;
                }

                //if (hidTela.Value == "1" && chkMostruario.Checked && prod_hb.STATUS.ToString() == "B")
                //{
                //    labHBBuscar.Text = "Este HB já foi FINALIZADO. Refaça a sua pesquisa.";
                //    return;
                //}

                Session["COLECAO"] = ddlColecoesBuscar.SelectedValue;

                if (hidTela.Value == "1")
                    Response.Redirect("prod_cad_hb_altera.aspx?c=" + prod_hb.CODIGO + "&t=" + hidTela.Value);
                else if (hidTela.Value == "2")
                    Response.Redirect("prod_cad_hb_altera_cortado.aspx?c=" + prod_hb.CODIGO);
                else
                    Response.Redirect("prod_cad_hb_ampliacao.aspx?c=" + prod_hb.CODIGO);

                labHBBuscar.Text = "OK";
            }
            catch (Exception ex)
            {
                labHBBuscar.Text = "ERRO: " + ex.Message;
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "0", DESC_COLECAO = "Selecione" });
                ddlColecoesBuscar.DataSource = _colecoes;
                ddlColecoesBuscar.DataBind();

                if (Session["COLECAO"] != null)
                    ddlColecoesBuscar.SelectedValue = Session["COLECAO"].ToString();
            }
        }
        #endregion
    }
}
