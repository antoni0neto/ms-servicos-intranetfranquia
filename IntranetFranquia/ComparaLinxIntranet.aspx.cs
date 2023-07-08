using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;

namespace Relatorios
{
    public partial class ComparaLinxIntranet : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        public decimal qtdeProforma = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CarregaDropDownListColecao();
        }

        private void CarregaDropDownListColecao()
        {
            ddlColecao.DataSource = baseController.BuscaColecao();
            ddlColecao.DataBind();
        }

        private void CarregaDropDownListGrupo()
        {
            ddlGrupo.DataSource = baseController.BuscaGruposProduto("01", ddlColecao.SelectedValue);
            ddlGrupo.DataBind();
        }

        private void CarregaDropDownListGriffe()
        {
            ddlGriffe.DataSource = baseController.BuscaGriffe();
            ddlGriffe.DataBind();
        }

        private void CarregaGridView()
        {
            List<IMPORTACAO_PROFORMA_PRODUTO> listaProformaProduto = baseController.BuscaProformaProduto(Convert.ToInt32(ddlColecao.SelectedValue), 
                                                                                                         Convert.ToInt32(ddlGriffe.SelectedValue), 
                                                                                                         ddlGrupo.SelectedItem.ToString());

            List<Sp_Busca_Produtos_Pecas_ColecaoResult> listaProdutosLinx = baseController.BuscaProdutoPecaColecao(ddlColecao.SelectedValue, 
                                                                                                                   ddlGriffe.SelectedItem.ToString(), 
                                                                                                                   ddlGrupo.SelectedItem.ToString());

            GridViewFob.DataSource = baseController.BuscaDivergenciaFob(listaProdutosLinx, 
                                                                        listaProformaProduto);
            GridViewFob.DataBind();

            GridViewLinx.DataSource = baseController.BuscaDivergenciaLinx(listaProdutosLinx,
                                                                          listaProformaProduto);
            GridViewLinx.DataBind();

            GridViewIntranet.DataSource = baseController.BuscaDivergenciaIntranet(listaProdutosLinx,
                                                                                  listaProformaProduto);

            GridViewIntranet.DataBind();
        }

        protected void ddlColecao_DataBound(object sender, EventArgs e)
        {
            ddlColecao.Items.Add(new ListItem("Selecione", "0"));
            ddlColecao.SelectedValue = "0";
        }

        protected void ddlGrupo_DataBound(object sender, EventArgs e)
        {
            ddlGrupo.Items.Add(new ListItem("Selecione", "0"));
            ddlGrupo.SelectedValue = "0";
        }

        protected void ddlGriffe_DataBound(object sender, EventArgs e)
        {
            ddlGriffe.Items.Add(new ListItem("Selecione", "0"));
            ddlGriffe.SelectedValue = "0";
        }

        protected void btGrupos_Click(object sender, EventArgs e)
        {
            if (ddlColecao.SelectedValue.ToString().Equals("0") ||
                ddlColecao.SelectedValue.ToString().Equals(""))
                return;

            CarregaDropDownListGrupo();
            CarregaDropDownListGriffe();

            btPesquisar.Enabled = true;
        }

        protected void btPesquisar_Click(object sender, EventArgs e)
        {
            if (ddlGrupo.SelectedValue.ToString().Equals("0") ||
                ddlGrupo.SelectedValue.ToString().Equals("") ||
                ddlGriffe.SelectedValue.ToString().Equals("0") ||
                ddlGriffe.SelectedValue.ToString().Equals(""))
                return;

            CarregaGridView();
        }
    }
}
