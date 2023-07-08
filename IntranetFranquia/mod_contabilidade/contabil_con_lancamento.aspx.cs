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
    public partial class contabil_con_lancamento : System.Web.UI.Page
    {
        ContabilidadeController contabilController = new ContabilidadeController();
        BaseController baseController = new BaseController();

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
                if (tela != "1" && tela != "2")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "contabil_menu.aspx";

                if (tela == "2")
                    hrefVoltar.HRef = "../mod_fiscal/fisc_menu.aspx";

            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        #region "DADOS INICIAIS"
        private bool ValidarCampos()
        {
            System.Drawing.Color _OK = System.Drawing.Color.Gray;
            System.Drawing.Color _notOK = System.Drawing.Color.Red;
            bool retorno = true;

            labLancamento.ForeColor = _OK;
            if (txtLancamento.Text.Trim() == "")
            {
                labLancamento.ForeColor = _notOK;
                retorno = false;
            }

            return retorno;
        }
        #endregion

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                if (!ValidarCampos())
                {
                    labErro.Text = "Informe o Número do Lançamento.";
                    return;
                }

                int lancamento = 0;

                lancamento = Convert.ToInt32(txtLancamento.Text.Trim());

                var lstLancamento = contabilController.ObterLancamento(lancamento);

                if (lstLancamento == null || lstLancamento.Count() <= 0)
                {
                    labErro.Text = "Lançamento não encontrado. Refaça sua pesquisa.";
                    gvLancamento.DataSource = new List<CTB_LANCAMENTO_ITEM>();
                    gvLancamento.DataBind();
                    return;
                }

                gvLancamento.DataSource = lstLancamento;
                gvLancamento.DataBind();

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }

        }

        protected void gvLancamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    CTB_LANCAMENTO_ITEM item = e.Row.DataItem as CTB_LANCAMENTO_ITEM;

                    if (item != null)
                    {

                        Literal _litFilial = e.Row.FindControl("litFilial") as Literal;
                        if (_litFilial != null)
                            _litFilial.Text = baseController.BuscaFilialCodigo(Convert.ToInt32(item.RATEIO_FILIAL)).FILIAL.Trim();

                        Literal _litCentroCusto = e.Row.FindControl("litCentroCusto") as Literal;
                        if (_litCentroCusto != null)
                        {
                            if (item.RATEIO_CENTRO_CUSTO != null)
                            {
                                var centroCusto = new DREController().ObterCentroCusto().Where(p => p.CENTRO_CUSTO == item.RATEIO_CENTRO_CUSTO).SingleOrDefault();
                                if (centroCusto != null)
                                    _litCentroCusto.Text = centroCusto.DESC_CENTRO_CUSTO.Trim();
                            }
                        }

                        Literal _litCLIFOR = e.Row.FindControl("litCLIFOR") as Literal;
                        if (_litCLIFOR != null)
                        {
                            if (item.COD_CLIFOR != null)
                            {
                                var nomeCLIFOR = baseController.ObterCadastroCLIFORCodigo(item.COD_CLIFOR);
                                if (nomeCLIFOR != null)
                                    _litCLIFOR.Text = nomeCLIFOR.NOME_CLIFOR.Trim();
                            }
                        }

                    }
                }
            }
        }


    }
}
