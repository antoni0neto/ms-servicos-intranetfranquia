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
    public partial class prod_rel_produto_custo : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            char mostruario = ' ';

            try
            {
                labErro.Text = "";

                if (ddlMostruario.SelectedValue.Trim() != "")
                    mostruario = Convert.ToChar(ddlMostruario.SelectedValue.Trim());

                List<SP_OBTER_PRODUTO_SEM_CUSTOResult> produtoSemCusto = new List<SP_OBTER_PRODUTO_SEM_CUSTOResult>();

                produtoSemCusto = prodController.ObterProdutoSemCusto(ddlColecoesBuscar.SelectedValue.Trim(), txtProdutoBuscar.Text.Trim().ToUpper(), txtNomeBuscar.Text.Trim().ToUpper(), mostruario).Where(p => p.CORTADO == true).ToList();

                gvProduto.DataSource = produtoSemCusto;
                gvProduto.DataBind();

                Session["COLECAO"] = ddlColecoesBuscar.SelectedValue;
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTO_SEM_CUSTOResult _pSemCusto = e.Row.DataItem as SP_OBTER_PRODUTO_SEM_CUSTOResult;

                    coluna += 1;
                    if (_pSemCusto != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Literal _litColecao = e.Row.FindControl("litColecao") as Literal;
                        if (_litColecao != null)
                            _litColecao.Text = baseController.BuscaColecaoAtual(_pSemCusto.COLECAO).DESC_COLECAO.Trim();

                        Literal _litMostruario = e.Row.FindControl("litMostruario") as Literal;
                        if (_litMostruario != null)
                        {
                            string m = "-";
                            if (_pSemCusto.MOSTRUARIO == 'S')
                                m = "Sim";
                            if (_pSemCusto.MOSTRUARIO == 'N')
                                m = "Não";

                            _litMostruario.Text = m;
                        }
                    }
                }
            }
        }
        protected void ibtPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                ImageButton b = (ImageButton)sender;

                if (b != null)
                {
                    GridViewRow row = (GridViewRow)b.NamingContainer;
                    if (row != null)
                    {
                        string colecao = baseController.BuscaColecoes().Where(p => p.DESC_COLECAO.Trim() == ((Literal)row.FindControl("litColecao")).Text.Trim()).SingleOrDefault().COLECAO;
                        string produto = row.Cells[2].Text.Trim();
                        string mostruario = (((Literal)row.FindControl("litMostruario")).Text.Trim().ToUpper() == "SIM") ? "S" : "N";

                        Response.Redirect("prod_cad_produto_custo.aspx?c=" + colecao + "&p=" + produto + "&m=" + mostruario);
                    }
                }
            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            BaseController _base = new BaseController();
            List<COLECOE> _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "", DESC_COLECAO = "" });
                ddlColecoesBuscar.DataSource = _colecoes;
                ddlColecoesBuscar.DataBind();

                if (Session["COLECAO"] != null)
                    ddlColecoesBuscar.SelectedValue = Session["COLECAO"].ToString();
            }
        }
        #endregion
    }
}
