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
    public partial class prod_con_hb_status : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        int coluna, qtde_total = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                string tela = Request.QueryString["t"].ToString();
                if (tela == "1")
                    hrefVoltar.HRef = "prod_menu.aspx";
                else
                    hrefVoltar.HRef = "../mod_faccao/facc_menu.aspx";

                CarregarColecoes();
            }
        }

        protected void btHBBuscar_Click(object sender, EventArgs e)
        {

            try
            {
                labHBBuscar.Text = "";
                if (ddlColecoesBuscar.SelectedValue == "" || ddlColecoesBuscar.SelectedValue == "0")
                {
                    labHBBuscar.Text = "Selecione a Coleção.";
                    return;
                }
                if ((txtHBBuscar.Text == "" || Convert.ToInt32(txtHBBuscar.Text) <= 0) && txtNomeBuscar.Text.Trim() == "")
                {
                    labHBBuscar.Text = "Informe o número do HB e/ou NOME.";
                    return;
                }

                List<PROD_HB> lstProdHB = new List<PROD_HB>();
                if (txtHBBuscar.Text.Trim() != "")
                    lstProdHB = prodController.ObterNumeroHB(ddlColecoesBuscar.SelectedValue, Convert.ToInt32(txtHBBuscar.Text));
                else
                    lstProdHB = prodController.ObterHB().Where(p => p.COLECAO.Trim() == ddlColecoesBuscar.SelectedValue.Trim()
                        && p.NOME.Contains(txtNomeBuscar.Text.Trim().ToUpper())).ToList();

                PROD_HB prod_hb = null;
                prod_hb = lstProdHB.Where(p => p.MOSTRUARIO == ((chkMostruario.Checked) ? 'S' : 'N')).OrderBy(x => x.STATUS).Take(1).SingleOrDefault();

                if (prod_hb == null)
                    labHBBuscar.Text = "Nenhum HB encontrado. Refaça a sua pesquisa.";

                if (prod_hb != null && prod_hb.STATUS.ToString() == "X")
                    labHBBuscar.Text = "Nenhum HB encontrado. Refaça a sua pesquisa.";

                if (labHBBuscar.Text != "")
                {
                    gvHB.DataSource = null;
                }
                else
                {
                    //Obter processo do HB
                    List<SP_OBTER_HB_PROCESSOResult> _processado = new List<SP_OBTER_HB_PROCESSOResult>();
                    int? hb = null;
                    if (txtHBBuscar.Text.Trim() != "")
                        hb = Convert.ToInt32(txtHBBuscar.Text.Trim());

                    _processado = prodController.ObterHBProcesso(hb, ddlColecoesBuscar.SelectedValue, txtNomeBuscar.Text.Trim()).Where(p => p.MOSTRUARIO == ((chkMostruario.Checked) ? 'S' : 'N')).ToList();

                    gvHB.DataSource = _processado.OrderBy(p => p.HB).ThenBy(x => x.COR).ThenBy(g => g.DETALHE).ToList();
                }
                gvHB.DataBind();

                Session["COLECAO"] = ddlColecoesBuscar.SelectedValue;
            }
            catch (Exception ex)
            {
                labHBBuscar.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvHB_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_HB_PROCESSOResult _finalizado = e.Row.DataItem as SP_OBTER_HB_PROCESSOResult;

                    coluna += 1;
                    if (_finalizado != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Literal _colecao = e.Row.FindControl("litColecao") as Literal;
                        if (_colecao != null)
                            _colecao.Text = (new BaseController().BuscaColecaoAtual(_finalizado.COLECAO)).DESC_COLECAO;

                        qtde_total += Convert.ToInt32(_finalizado.QTDE);
                    }
                }
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
