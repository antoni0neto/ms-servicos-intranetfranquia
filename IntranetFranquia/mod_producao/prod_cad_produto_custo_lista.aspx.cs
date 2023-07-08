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

namespace Relatorios
{
    public partial class prod_cad_produto_custo_lista : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string tipoCusto = "";
                if (Request.QueryString["c"] != null && Request.QueryString["c"].ToString() != "")
                    tipoCusto = Request.QueryString["c"].ToString();

                hidTipoCusto.Value = tipoCusto;

                CarregarProdutoSimulado(tipoCusto);
            }

            btAtualizar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btAtualizar, null) + ";");
        }

        private void CarregarProdutoSimulado(string tipoCusto)
        {
            var produtoSimulado = prodController.ObterProdutoCustoSimulado("", "", "", ' ', 'N', tipoCusto);

            gvProdutoSimulado.DataSource = produtoSimulado;
            gvProdutoSimulado.DataBind();
        }
        protected void gvProdutoSimulado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTO_SIMULADOResult _pSimulado = e.Row.DataItem as SP_OBTER_PRODUTO_SIMULADOResult;

                    if (_pSimulado != null)
                    {
                        Literal _litColecao = e.Row.FindControl("litColecao") as Literal;
                        if (_litColecao != null)
                            _litColecao.Text = baseController.BuscaColecaoAtual(_pSimulado.COLECAO).DESC_COLECAO.Trim();

                        Literal _litMostruario = e.Row.FindControl("litMostruario") as Literal;
                        if (_litMostruario != null)
                        {
                            string m = "-";
                            if (_pSimulado.MOSTRUARIO == 'S')
                                m = "Sim";
                            if (_pSimulado.MOSTRUARIO == 'N')
                                m = "Não";
                            _litMostruario.Text = m;
                        }


                        if (prodController.ObterCustoServico(_pSimulado.PRODUTO, Convert.ToChar(_pSimulado.MOSTRUARIO)).Count() > 0)
                        {
                            e.Row.BackColor = Color.LightGray;
                            Literal _litEstamparia = e.Row.FindControl("litEstamparia") as Literal;
                            if (_litEstamparia != null)
                            {
                                if (_pSimulado.ESTAMPARIA > 0)
                                {
                                    if (_pSimulado.CUSTO_ESTAMP > 0)
                                    {
                                        _litEstamparia.Text = "Preço OK";
                                    }
                                    else
                                    {
                                        _litEstamparia.Text = "Falta Preço";
                                        e.Row.Cells[7].BackColor = Color.Moccasin;
                                    }
                                }
                                else
                                {
                                    _litEstamparia.Text = "-";
                                }
                            }

                            Literal _litFaccao = e.Row.FindControl("litFaccao") as Literal;
                            if (_litFaccao != null)
                            {
                                if (_pSimulado.FACCAO > 0)
                                {
                                    if (_pSimulado.CUSTO_FACCAO > 0)
                                    {
                                        _litFaccao.Text = "Preço OK";
                                    }
                                    else
                                    {
                                        _litFaccao.Text = "Falta Preço";
                                        e.Row.Cells[8].BackColor = Color.Moccasin;
                                    }
                                }
                                else
                                {
                                    _litFaccao.Text = "-";
                                }
                            }
                        }

                        //Se produto possui aviamento zerado
                        if (_pSimulado.CUSTO_MIN_AVI != null && _pSimulado.CUSTO_MIN_AVI == 0)
                            e.Row.BackColor = Color.LightBlue;

                        //Se produto ainda nao cortado.
                        if (!_pSimulado.CORTADO == true)
                            e.Row.BackColor = Color.Thistle;


                    }

                }
            }
        }
        protected void gvProdutoSimulado_Sorting(object sender, GridViewSortEventArgs e)
        {

            IEnumerable<SP_OBTER_PRODUTO_SIMULADOResult> produtoCustoSimulado = prodController.ObterProdutoCustoSimulado("", "", "", ' ', 'N', hidTipoCusto.Value);

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            produtoCustoSimulado = produtoCustoSimulado.OrderBy(e.SortExpression + sortDirection);
            gvProdutoSimulado.DataSource = produtoCustoSimulado;
            gvProdutoSimulado.DataBind();

        }

        protected void ibtPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton b = (ImageButton)sender;

                if (b != null)
                {
                    GridViewRow row = (GridViewRow)b.NamingContainer;
                    if (row != null)
                    {
                        string colecaoQ = baseController.BuscaColecoes().Where(p => p.DESC_COLECAO.Trim() == ((Literal)row.FindControl("litColecao")).Text.Trim()).SingleOrDefault().COLECAO;
                        string produto = row.Cells[2].Text.Trim();
                        string mostruario = (((Literal)row.FindControl("litMostruario")).Text.Trim().ToUpper() == "SIM") ? "S" : "N";

                        var colecao = colecaoQ.Trim();
                        if (colecao == "21" || colecao == "22" || colecao == "23" || colecao == "24" || colecao == "25" || colecao == "26")
                            Response.Redirect(("prod_cad_produto_custo.aspx?c=" + colecaoQ + "&p=" + produto + "&m=" + mostruario), "_blank", "");
                        else
                            Response.Redirect(("prod_cad_produto_custo_novo.aspx?c=" + colecaoQ + "&p=" + produto + "&m=" + mostruario), "_blank", "");

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btAtualizar_Click(object sender, EventArgs e)
        {
            CarregarProdutoSimulado(hidTipoCusto.Value);
        }


        private PROD_HB ObterProduto(string colecao, string produto, bool mostruario)
        {
            var pHB = prodController.ObterHB().Where(p => p.COLECAO.Trim() == colecao.Trim() && p.CODIGO_PRODUTO_LINX.Trim() == produto.Trim() && p.MOSTRUARIO == ((mostruario) ? 'S' : 'N')).Take(1).SingleOrDefault();
            return pHB;
        }

    }
}
