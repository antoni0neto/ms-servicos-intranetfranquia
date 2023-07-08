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
    public partial class LineReportLojaColection : System.Web.UI.Page
    {
        EstoqueController estoqueController = new EstoqueController();
        EntradaProdutoController entradaProdutoController = new EntradaProdutoController();
        HistoricoProdutoController historicoProdutoController = new HistoricoProdutoController();
        BaseController baseController = new BaseController();

        List<GrupoProduto> grupo = new List<GrupoProduto>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListCategoria();
                CarregaDropDownListColecao();
                CarregaDropDownListFilial();
                CarregaDropDownListGriffe();
            }
        }
        private void CarregaDropDownListFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                ddlFilial.DataSource = baseController.BuscaFiliais(usuario);
                ddlFilial.DataBind();
            }
        }

        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Add(new ListItem("Selecione", "0"));
            ddlFilial.SelectedValue = "0";
        }

        private void CarregaDropDownListCategoria()
        {
            ddlCategoria.DataSource = baseController.BuscaCategorias();
            ddlCategoria.DataBind();
        }

        private void CarregaDropDownListColecao()
        {
            ddlColecao.DataSource = baseController.BuscaColecoes();
            ddlColecao.DataBind();
        }

        private void CarregaDropDownListGriffe()
        {
            ddlGriffe.DataSource = baseController.BuscaGriffes();
            ddlGriffe.DataBind();
        }

        private void CarregaGridViewColecaoProduto()
        {
            GridViewMovimentoProduto.DataSource = historicoProdutoController.BuscaMovimentoLojaColecao(ddlCategoria.SelectedValue,
                                                                                                       ddlColecao.SelectedValue,
                                                                                                       ddlFilial.SelectedValue,
                                                                                                       ddlGriffe.SelectedItem.ToString());
            GridViewMovimentoProduto.DataBind();
        }

        protected void ddlGriffe_DataBound(object sender, EventArgs e)
        {
            ddlGriffe.Items.Add(new ListItem("Selecione", "0"));
            ddlGriffe.SelectedValue = "0";
        }

        protected void ddlCategoria_DataBound(object sender, EventArgs e)
        {
            ddlCategoria.Items.Add(new ListItem("Selecione", "0"));
            ddlCategoria.SelectedValue = "0";
        }

        protected void ddlColecao_DataBound(object sender, EventArgs e)
        {
            ddlColecao.Items.Add(new ListItem("Selecione", "0"));
            ddlColecao.SelectedValue = "0";
        }

        protected void ButtonPesquisarMovimento_Click(object sender, EventArgs e)
        {
            if (ddlGriffe.SelectedValue.ToString().Equals("0")    || 
                ddlGriffe.SelectedValue.ToString().Equals("")     ||
                ddlCategoria.SelectedValue.ToString().Equals("0") ||
                ddlCategoria.SelectedValue.ToString().Equals("")  ||
                ddlFilial.SelectedValue.ToString().Equals("0")    ||
                ddlFilial.SelectedValue.ToString().Equals("")     ||
                ddlColecao.SelectedValue.ToString().Equals("0")   || 
                ddlColecao.SelectedValue.ToString().Equals(""))
                 return;

            CarregaGridViewColecaoProduto();
        }

        protected void GridViewMovimentoProduto_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewMovimentoProduto.FooterRow;

            foreach (GridViewRow item in GridViewMovimentoProduto.Rows)
            {
                item.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                item.Cells[1].BackColor = System.Drawing.Color.PeachPuff;
                item.Cells[2].BackColor = System.Drawing.Color.PeachPuff;
                item.Cells[6].BackColor = System.Drawing.Color.PeachPuff;
                item.Cells[7].BackColor = System.Drawing.Color.PeachPuff;
                item.Cells[11].BackColor = System.Drawing.Color.PeachPuff;
                item.Cells[12].BackColor = System.Drawing.Color.PeachPuff;

                if (item.Cells[13].Text.Substring(0, 1).Equals("-"))
                    item.Cells[13].BackColor = System.Drawing.Color.Red;
                else
                    item.Cells[13].BackColor = System.Drawing.Color.GreenYellow;

                if (item.Cells[14].Text.Substring(0, 1).Equals("-"))
                {
                    item.Cells[14].BackColor = System.Drawing.Color.Red;
                    item.Cells[15].BackColor = System.Drawing.Color.GreenYellow;
                }
                else
                {
                    item.Cells[14].BackColor = System.Drawing.Color.GreenYellow;
                    item.Cells[15].BackColor = System.Drawing.Color.Red;
                }
            }

            if (footer != null)
            {
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[1].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[2].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[6].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[7].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[11].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[12].BackColor = System.Drawing.Color.PeachPuff;
            }
        }
    }
}
