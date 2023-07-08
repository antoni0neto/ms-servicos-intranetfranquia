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
    public partial class gerloja_permuta : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        BaseController baseController = new BaseController();
        LojaController lojaController = new LojaController();

        decimal valTot = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPeriodoInicial.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + txtPeriodoFinal.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy'});});", true);


            if (!Page.IsPostBack)
            {
                string tela = Request.QueryString["t"].ToString();
                if (tela != "1" && tela != "2" && tela != "3" && tela != "4")
                {
                    Session["USUARIO"] = null;
                    Response.Redirect("~/Login.aspx");
                }

                if (tela == "1")
                    hrefVoltar.HRef = "gerloja_menu.aspx";
                else if (tela == "2")
                    hrefVoltar.HRef = "../mod_estoque/estoque_menu.aspx";
                else if (tela == "3")
                    hrefVoltar.HRef = "../mod_desenvolvimento/desenv_menu.aspx";
                else if (tela == "4")
                    hrefVoltar.HRef = "../mod_adm_loja/admloj_menu.aspx";

                CarregarFilial();
            }

            //Evitar duplo clique no botão
            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        private List<SP_OBTER_PRODUTO_TRANSFRELResult> ObterTransLojaRel()
        {
            DateTime? dataIni = null;
            DateTime? dataFim = null;
            char? motivo = null;

            if (txtPeriodoInicial.Text != "")
                dataIni = Convert.ToDateTime(txtPeriodoInicial.Text);

            if (txtPeriodoFinal.Text != "")
                dataFim = Convert.ToDateTime(txtPeriodoFinal.Text);

            if (ddlMotivo.SelectedValue != "")
                motivo = Convert.ToChar(ddlMotivo.SelectedValue);

            var relTransLoja = lojaController.ObterTransLojaRel(ddlFilial.SelectedValue.Trim(), txtProduto.Text.Trim(), dataIni, dataFim, motivo);

            if (txtDesc.Text.Trim() != "")
                relTransLoja = relTransLoja.Where(p => p.DESCRICAO_RETIRADA.ToLower().Contains(txtDesc.Text.Trim().ToLower())).ToList();

            return relTransLoja;
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                var relTransLoja = ObterTransLojaRel();

                gvPermutaLoja.DataSource = relTransLoja;
                gvPermutaLoja.DataBind();

            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvPermutaLoja_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PRODUTO_TRANSFRELResult tran = e.Row.DataItem as SP_OBTER_PRODUTO_TRANSFRELResult;

                    Label labPrecoTO = e.Row.FindControl("labPrecoTO") as Label;
                    labPrecoTO.Text = "R$ " + Convert.ToDecimal(tran.PRECO_TO).ToString("###,###,###,##0.00");

                    Label labDataRetirada = e.Row.FindControl("labDataRetirada") as Label;
                    labDataRetirada.Text = Convert.ToDateTime(tran.DATA_RETIRADA).ToString("dd/MM/yyyy");

                    Label labMotivo = e.Row.FindControl("labMotivo") as Label;
                    labMotivo.Text = ((tran.MOTIVO == 'T') ? "TRANSFERÊNCIA" : ((tran.MOTIVO == 'R') ? "RETIRADA" : "PERMUTA"));

                    valTot += Convert.ToDecimal(tran.PRECO_TO);
                }
            }
        }
        protected void gvPermutaLoja_DataBound(object sender, EventArgs e)
        {
            GridViewRow _footer = gvPermutaLoja.FooterRow;
            if (_footer != null)
            {
                _footer.Cells[1].Text = "Total";
                _footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                _footer.Cells[7].Text = "R$ " + valTot.ToString("###,###,###,##0.00");

            }

        }
        protected void gvPermutaLoja_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_PRODUTO_TRANSFRELResult> transLoja = ObterTransLojaRel();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            transLoja = transLoja.OrderBy(e.SortExpression + sortDirection);
            gvPermutaLoja.DataSource = transLoja;
            gvPermutaLoja.DataBind();


        }

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                var filiais = baseController.BuscaFiliais(usuario);

                filiais.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });

                ddlFilial.DataSource = filiais;
                ddlFilial.DataBind();

                if (filiais.Count() == 2)
                {
                    ddlFilial.SelectedIndex = 1;
                    ddlFilial.Enabled = false;
                }
            }
        }

        #endregion


    }
}
