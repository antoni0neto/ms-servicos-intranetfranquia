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
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Globalization;


namespace Relatorios
{
    public partial class facc_resumo_custo : System.Web.UI.Page
    {
        FaccaoController faccController = new FaccaoController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Valida queryString
                if (Request.QueryString["t"] == null || Request.QueryString["t"] == "")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2" && tela != "3")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "facc_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_producao/prod_menu.aspx";

                if (tela == "3")
                    hrefVoltar.HRef = "facc_menu_relatorio.aspx";

                CarregarColecoes();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_FACCAO_CUSTOResult> ObterResumoCusto()
        {
            List<SP_OBTER_FACCAO_CUSTOResult> listaRetorno = new List<SP_OBTER_FACCAO_CUSTOResult>();
            int? numeroHB = null;
            char mostruario = 'N';

            if (txtHB.Text.Trim() != "")
                numeroHB = Convert.ToInt32(txtHB.Text.Trim());

            mostruario = (cbMostruario.Checked) ? 'S' : 'N';

            listaRetorno = faccController.ObterFaccaoCusto(ddlColecao.SelectedValue.Trim(), numeroHB, txtProduto.Text.Trim(), txtNome.Text.Trim().ToUpper(), mostruario);

            return listaRetorno;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                gvFaccaoCusto.DataSource = ObterResumoCusto();
                gvFaccaoCusto.DataBind();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvFaccaoCusto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FACCAO_CUSTOResult faccaoCusto = e.Row.DataItem as SP_OBTER_FACCAO_CUSTOResult;

                    if (faccaoCusto != null)
                    {
                        Label _labCustoEstimado = e.Row.FindControl("labCustoEstimado") as Label;
                        if (_labCustoEstimado != null)
                            _labCustoEstimado.Text = "R$ " + faccaoCusto.CUSTO_ESTIMADO.ToString("###,###,###,###,##0.00");

                        Label _labCustoAprovado = e.Row.FindControl("labCustoAprovado") as Label;
                        if (_labCustoAprovado != null)
                            _labCustoAprovado.Text = "R$ " + Convert.ToDecimal(faccaoCusto.CUSTO_APROVADO).ToString("###,###,###,###,##0.00");

                        //Popular GRID VIEW FILHO
                        if (faccaoCusto.FOTO_PECA != null && faccaoCusto.FOTO_PECA.Trim() != "")
                        {
                            GridView gvFoto = e.Row.FindControl("gvFoto") as GridView;
                            if (gvFoto != null)
                            {
                                List<SP_OBTER_FACCAO_CUSTOResult> _fotoProduto = new List<SP_OBTER_FACCAO_CUSTOResult>();
                                _fotoProduto.Add(new SP_OBTER_FACCAO_CUSTOResult { FOTO_PECA = faccaoCusto.FOTO_PECA });
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
        protected void gvFoto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_FACCAO_CUSTOResult _faccaoCusto = e.Row.DataItem as SP_OBTER_FACCAO_CUSTOResult;
                    if (_faccaoCusto != null)
                    {
                        System.Web.UI.WebControls.Image _imgFotoPeca = e.Row.FindControl("imgFotoPeca") as System.Web.UI.WebControls.Image;
                        if (_imgFotoPeca != null)
                            _imgFotoPeca.ImageUrl = _faccaoCusto.FOTO_PECA;
                    }
                }
            }
        }
        protected void gvFaccaoCusto_DataBound(object sender, EventArgs e)
        {
        }
        protected void gvFaccaoCusto_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_FACCAO_CUSTOResult> _faccResumo = ObterResumoCusto();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            _faccResumo = _faccResumo.OrderBy(e.SortExpression + sortDirection);
            gvFaccaoCusto.DataSource = _faccResumo;
            gvFaccaoCusto.DataBind();
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });
                ddlColecao.DataSource = _colecoes;
                ddlColecao.DataBind();
            }
        }
        #endregion

    }
}
