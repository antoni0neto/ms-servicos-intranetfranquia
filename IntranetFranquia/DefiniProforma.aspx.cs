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
    public partial class DefiniProforma : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        UsuarioController usuarioController = new UsuarioController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CarregaDropDownListColecao();
        }

        private void CarregaDropDownListDeProforma()
        {
            ddlDeProforma.DataSource = baseController.BuscaProformasProvisoria();
            ddlDeProforma.DataBind();
        }

        private void CarregaDropDownListParaProforma()
        {
            ddlParaProforma.DataSource = baseController.BuscaProformasColecao(Convert.ToInt32(ddlColecao.SelectedValue));
            ddlParaProforma.DataBind();
        }

        private void CarregaDropDownListColecao()
        {
            ddlColecao.DataSource = baseController.BuscaColecao();
            ddlColecao.DataBind();
        }

        private void CarregaGridViewProformaProduto()
        {
            GridViewProformaProduto.DataSource = baseController.BuscaProformaProdutoPorProforma(Convert.ToInt32(ddlParaProforma.SelectedValue));
            GridViewProformaProduto.DataBind();
        }

        protected void ddlDeProforma_DataBound(object sender, EventArgs e)
        {
            ddlDeProforma.Items.Add(new ListItem("Selecione", "0"));
            ddlDeProforma.SelectedValue = "0";
        }

        protected void ddlColecao_DataBound(object sender, EventArgs e)
        {
            ddlColecao.Items.Add(new ListItem("Selecione", "0"));
            ddlColecao.SelectedValue = "0";
        }

        protected void ddlParaProforma_DataBound(object sender, EventArgs e)
        {
            ddlParaProforma.Items.Add(new ListItem("Selecione", "0"));
            ddlParaProforma.SelectedValue = "0";
        }

        protected void btProforma_Click(object sender, EventArgs e)
        {
            if (ddlColecao.SelectedValue.ToString().Equals("0") ||
                ddlColecao.SelectedValue.ToString().Equals(""))
                return;

            CarregaDropDownListDeProforma();
            CarregaDropDownListParaProforma();

            this.btDefinir.Enabled = true;
        }

        protected void btDefinir_Click(object sender, EventArgs e)
        {
            if (ddlDeProforma.SelectedValue.ToString().Equals("0") ||
                ddlDeProforma.SelectedValue.ToString().Equals("") ||
                ddlParaProforma.SelectedValue.ToString().Equals("0") ||
                ddlParaProforma.SelectedValue.ToString().Equals(""))
                return;
            
            int i = 0;

            List<IMPORTACAO_PROFORMA_PRODUTO> proformaProduto = baseController.BuscaProformaProdutoPorProforma(Convert.ToInt32(ddlDeProforma.SelectedValue));

            if (proformaProduto != null)
            {
                foreach (IMPORTACAO_PROFORMA_PRODUTO item in proformaProduto)
                {
                    i++;

                    baseController.AtualizaProformaProduto(item, Convert.ToInt32(ddlParaProforma.SelectedValue));
                }
            }

            LabelFeedBack.Text = i + " Produtos Integrados a Proforma: " + ddlDeProforma.SelectedValue + "!!!";

            CarregaGridViewProformaProduto();
        }

        protected void GridViewProformaProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            IMPORTACAO_PROFORMA_PRODUTO proformaProduto = e.Row.DataItem as IMPORTACAO_PROFORMA_PRODUTO;

            if (proformaProduto != null)
            {
                Literal literalDescricao = e.Row.FindControl("LiteralDescricao") as Literal;

                if (literalDescricao != null)
                    literalDescricao.Text = baseController.BuscaProduto(proformaProduto.CODIGO_PRODUTO.ToString()).DESC_PRODUTO;

                Literal literalCor = e.Row.FindControl("LiteralCor") as Literal;

                if (literalCor != null)
                    literalCor.Text = baseController.BuscaProdutoCor(proformaProduto.CODIGO_PRODUTO.ToString(), proformaProduto.CODIGO_PRODUTO_COR.ToString()).DESC_COR_PRODUTO;
            }
        }
    }
}
