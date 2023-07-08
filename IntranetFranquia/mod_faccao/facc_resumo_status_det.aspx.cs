using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Web.UI.HtmlControls;
using Relatorios.mod_desenvolvimento.modelo_pocket;

namespace Relatorios
{
    public partial class facc_resumo_status_det : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();

        int corte = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["f"] == null || Request.QueryString["f"] == "" || Session["USUARIO"] == null)
                {
                    Response.Write("SESSÃO ENCERRADA.");
                    Response.End();
                }

                string fornecedor = Request.QueryString["f"].ToString();
                string fornecedorSub = Request.QueryString["s"].ToString();

                CarregarCortes(fornecedor, fornecedorSub);
            }

        }

        private void CarregarCortes(string fornecedor, string fornecedorSub)
        {
            txtFornecedor.Text = fornecedor + " " + fornecedorSub;

            var f = faccController.ObterFaccaoStatusDetalhe(fornecedor, fornecedorSub);

            corte = 0;
            gvCorteGrande.DataSource = f.Where(p => p.STS.Trim() == "CORTE_GRANDE");
            gvCorteGrande.DataBind();
            if (gvCorteGrande.Rows.Count > 0)
                pnlCorteGrande.Visible = true;

            corte = 0;
            gvCortePequeno.DataSource = f.Where(p => p.STS.Trim() == "CORTE_PEQUENO");
            gvCortePequeno.DataBind();
            if (gvCortePequeno.Rows.Count > 0)
                pnlCortePequeno.Visible = true;

            corte = 0;
            gvCorteGrandeAtrasado.DataSource = f.Where(p => p.STS.Trim() == "CORTE_GRANDE_ATR");
            gvCorteGrandeAtrasado.DataBind();
            if (gvCorteGrandeAtrasado.Rows.Count > 0)
                pnlCorteGrandeAtr.Visible = true;

            corte = 0;
            gvCortePequenoAtrasado.DataSource = f.Where(p => p.STS.Trim() == "CORTE_PEQUENO_ATR");
            gvCortePequenoAtrasado.DataBind();
            if (gvCortePequenoAtrasado.Rows.Count > 0)
                pnlCortePequenoAtr.Visible = true;

            corte = 0;
            gvMostruario.DataSource = f.Where(p => p.STS.Trim() == "MOSTRUARIO");
            gvMostruario.DataBind();
            if (gvMostruario.Rows.Count > 0)
                pnlMostruario.Visible = true;

            txtTotal.Text = f.Sum(p => p.TOTAL).ToString();
        }

        protected void gvCorte_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FACCAO_STATUS_DETALHEResult _det = e.Row.DataItem as SP_OBTER_FACCAO_STATUS_DETALHEResult;

                    if (_det != null)
                    {
                        corte += _det.TOTAL;

                        //Popular GRID VIEW FILHO
                        if (_det.FOTO_PECA != null && _det.FOTO_PECA.Trim() != "")
                        {
                            GridView gvFoto = e.Row.FindControl("gvFoto") as GridView;
                            if (gvFoto != null)
                            {
                                List<PROD_HB> _fotoProduto = new List<PROD_HB>();
                                _fotoProduto.Add(new PROD_HB { FOTO_PECA = _det.FOTO_PECA });
                                gvFoto.DataSource = _fotoProduto;
                                gvFoto.DataBind();
                            }
                        }
                        else
                        {
                            System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                            if (img != null)
                                img.Visible = false;
                        }
                    }
                }
            }
        }
        protected void gvCorte_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = ((GridView)sender).FooterRow;
            if (footer != null)
            {
                footer.Cells[2].Text = "Total";
                footer.Cells[2].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[5].Text = corte.ToString();
            }
        }

        protected void gvFoto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB _hb = e.Row.DataItem as PROD_HB;
                    if (_hb != null)
                    {
                        System.Web.UI.WebControls.Image _imgFotoPeca = e.Row.FindControl("imgFotoPeca") as System.Web.UI.WebControls.Image;
                        if (_imgFotoPeca != null)
                            _imgFotoPeca.ImageUrl = _hb.FOTO_PECA;
                    }
                }
            }
        }
    }
}
