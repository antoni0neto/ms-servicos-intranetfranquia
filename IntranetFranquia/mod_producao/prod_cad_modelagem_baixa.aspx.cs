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
    public partial class prod_cad_modelagem_baixa : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        BaseController _base = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();
            }

            btBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btBuscar, null) + ";");
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {

            try
            {
                labErro.Text = "";
                if (ddlColecao.SelectedValue == "" || ddlColecao.SelectedValue == "0")
                {
                    labErro.Text = "Selecione a Coleção.";
                    return;
                }

                CarregarMolde();

                Session["COLECAO"] = ddlColecao.SelectedValue;
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        private List<SP_OBTER_HB_MOLDEResult> ObterMolde()
        {
            int? hb = null;
            char? mostruario = null;

            if (txtHB.Text.Trim() != "")
                hb = Convert.ToInt32(txtHB.Text);
            if (ddlMostruario.SelectedValue != "")
                mostruario = Convert.ToChar(ddlMostruario.SelectedValue);

            var m = prodController.ObterMoldeHB(ddlColecao.SelectedValue.Trim(), hb, txtProduto.Text.Trim().ToUpper(), txtNome.Text.Trim().ToUpper(), txtMolde.Text.Trim().ToUpper(), txtModelagem.Text.Trim().ToUpper(), mostruario);

            return m;
        }
        private void CarregarMolde()
        {
            var m = ObterMolde();
            gvMolde.DataSource = m;
            gvMolde.DataBind();

        }
        protected void gvMolde_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_HB_MOLDEResult molde = e.Row.DataItem as SP_OBTER_HB_MOLDEResult;
                    if (molde != null)
                    {
                        Literal _litMostruario = e.Row.FindControl("litMostruario") as Literal;
                        if (_litMostruario != null)
                        {
                            _litMostruario.Text = ((molde.MOSTRUARIO == 'S') ? "Sim" : "Não");
                        }
                    }
                }
            }
        }
        protected void gvMolde_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_OBTER_HB_MOLDEResult> molde = ObterMolde();

            string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            molde = molde.OrderBy(e.SortExpression + sortDirection);
            gvMolde.DataSource = molde;
            gvMolde.DataBind();
        }
        protected void txtUsuarioCAD_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txt = (TextBox)sender;
                if (txt != null)
                {
                    GridViewRow r = (GridViewRow)txt.NamingContainer;
                    if (r != null)
                    {
                        HiddenField h = r.FindControl("hidCodigoMolde") as HiddenField;
                        if (h != null)
                        {
                            string codigoMolde = h.Value;
                            if (codigoMolde != "")
                            {
                                var m = prodController.ObterMolde(Convert.ToInt32(codigoMolde));
                                if (m != null)
                                {
                                    m.USUARIO_CAD = txt.Text.Trim().ToUpper();
                                    m.USUARIO_CAD_BAIXA = DateTime.Now;

                                    prodController.AtualizarMolde(m);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #region "DADOS INICIAIS"
        private void CarregarColecoes()
        {
            var _colecoes = _base.BuscaColecoes();
            if (_colecoes != null)
            {
                _colecoes.Insert(0, new COLECOE { COLECAO = "0", DESC_COLECAO = "Selecione" });
                ddlColecao.DataSource = _colecoes;
                ddlColecao.DataBind();

                if (Session["COLECAO"] != null)
                    ddlColecao.SelectedValue = Session["COLECAO"].ToString();
            }
        }
        #endregion


    }
}
