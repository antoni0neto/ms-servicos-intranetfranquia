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
    public partial class BuscaGeral : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();

        public decimal totalDraftFinal = 0;
        public decimal totalDraftOriginal = 0;
        public decimal totalContainerOriginal = 0;
        public decimal totalGriffe = 0;
        public decimal totalGrupo = 0;
        public decimal totalProforma = 0;

        public decimal qtdeDraftFinal = 0;
        public decimal qtdeDraftOriginal = 0;
        public decimal qtdeContainerOriginal = 0;
        public decimal qtdeGriffe = 0;
        public decimal qtdeGrupo = 0;
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

        private void CarregaGridViewGeral()
        {
            GridViewDraftFinal.DataSource = baseController.BuscaDraftFinal(Convert.ToInt32(ddlColecao.SelectedValue));
            GridViewDraftFinal.DataBind();

            GridViewDraftOriginal.DataSource = baseController.BuscaDraftOriginal(Convert.ToInt32(ddlColecao.SelectedValue));
            GridViewDraftOriginal.DataBind();

            GridViewContainerOriginal.DataSource = baseController.BuscaContainerOriginal(Convert.ToInt32(ddlColecao.SelectedValue));
            GridViewContainerOriginal.DataBind();

            GridViewGriffe.DataSource = baseController.BuscaPorGriffe(Convert.ToInt32(ddlColecao.SelectedValue));
            GridViewGriffe.DataBind();

            GridViewGrupo.DataSource = baseController.BuscaPorGrupo(Convert.ToInt32(ddlColecao.SelectedValue));
            GridViewGrupo.DataBind();

            GridViewProforma.DataSource = baseController.BuscaPorProforma(Convert.ToInt32(ddlColecao.SelectedValue));
            GridViewProforma.DataBind();
        }

        protected void ddlColecao_DataBound(object sender, EventArgs e)
        {
            ddlColecao.Items.Add(new ListItem("Selecione", "0"));
            ddlColecao.SelectedValue = "0";
        }

        protected void btPesquisar_Click(object sender, EventArgs e)
        {
            if (ddlColecao.SelectedValue.ToString().Equals("0") ||
                ddlColecao.SelectedValue.ToString().Equals(""))
                return;

            CarregaGridViewGeral();
        }

        protected void GridViewDraftFinal_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewDraftFinal.FooterRow;

            if (footer != null)
            {
                footer.Cells[0].Text = "Total";
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[1].Text = totalDraftFinal.ToString("N2");
                footer.Cells[1].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[2].Text = qtdeDraftFinal.ToString();
                footer.Cells[2].BackColor = System.Drawing.Color.PeachPuff;
            }
        }

        protected void GridViewDraftFinal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Sp_Busca_Draft_FinalResult draftFinal = e.Row.DataItem as Sp_Busca_Draft_FinalResult;

            if (draftFinal != null)
            {
                totalDraftFinal += Convert.ToDecimal(draftFinal.valor);
                qtdeDraftFinal += Convert.ToInt32(draftFinal.qtde);
            }
        }

        protected void GridViewDraftOriginal_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewDraftOriginal.FooterRow;

            if (footer != null)
            {
                footer.Cells[0].Text = "Total";
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[1].Text = totalDraftOriginal.ToString("N2");
                footer.Cells[1].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[2].Text = qtdeDraftOriginal.ToString();
                footer.Cells[2].BackColor = System.Drawing.Color.PeachPuff;
            }
        }

        protected void GridViewDraftOriginal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Sp_Busca_Draft_OriginalResult draftOriginal = e.Row.DataItem as Sp_Busca_Draft_OriginalResult;

            if (draftOriginal != null)
            {
                totalDraftOriginal += Convert.ToDecimal(draftOriginal.valor);
                qtdeDraftOriginal += Convert.ToInt32(draftOriginal.qtde);
            }
        }

        protected void GridViewContainerOriginal_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewContainerOriginal.FooterRow;

            if (footer != null)
            {
                footer.Cells[0].Text = "Total";
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[1].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[1].Text = totalContainerOriginal.ToString("N2");
                footer.Cells[2].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[2].Text = qtdeContainerOriginal.ToString();
            }
        }

        protected void GridViewContainerOriginal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Sp_Busca_Container_OriginalResult containerOriginal = e.Row.DataItem as Sp_Busca_Container_OriginalResult;

            if (containerOriginal != null)
            {
                totalContainerOriginal += Convert.ToDecimal(containerOriginal.valor);
                qtdeContainerOriginal += Convert.ToInt32(containerOriginal.qtde);
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btBuscarNel = e.Row.FindControl("btBuscarNel") as Button;
            
                if (btBuscarNel != null)
                {
                    if (e.Row.DataItem != null)
                    {
                        Sp_Busca_Container_OriginalResult listaContainerOriginal = e.Row.DataItem as Sp_Busca_Container_OriginalResult;
                        
                        btBuscarNel.CommandArgument = listaContainerOriginal.descricao;
                    }
                }
            }
        }

        protected void btBuscarNel_Click(object sender, EventArgs e)
        {
            Button btBuscarNel = sender as Button;

            if (btBuscarNel != null)
                CarregaGridViewNel(btBuscarNel.CommandArgument);
        }

        private void CarregaGridViewNel(string descricao)
        {
            IMPORTACAO_CONTAINER container = baseController.BuscaContainerPelaDescricao(descricao);

            if (container != null)
            {
                GridViewNel.Visible = true;
                GridViewNel.DataSource = baseController.BuscaNelPorContainer(container.CODIGO_CONTAINER);
                GridViewNel.DataBind();
            }
        }

        protected void GridViewGriffe_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewGriffe.FooterRow;

            if (footer != null)
            {
                footer.Cells[0].Text = "Total";
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[1].Text = totalGriffe.ToString("N2");
                footer.Cells[1].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[2].Text = qtdeGriffe.ToString();
                footer.Cells[2].BackColor = System.Drawing.Color.PeachPuff;
            }
        }

        protected void GridViewGriffe_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Sp_Busca_Por_GriffeResult griffe = e.Row.DataItem as Sp_Busca_Por_GriffeResult;

            if (griffe != null)
            {
                totalGriffe += Convert.ToDecimal(griffe.valor);
                qtdeGriffe += Convert.ToInt32(griffe.qtde);
            }
        }

        protected void GridViewGrupo_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewGrupo.FooterRow;

            if (footer != null)
            {
                footer.Cells[0].Text = "Total";
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[1].Text = totalGrupo.ToString("N2");
                footer.Cells[1].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[2].Text = qtdeGrupo.ToString();
                footer.Cells[2].BackColor = System.Drawing.Color.PeachPuff;
            }
        }

        protected void GridViewGrupo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Sp_Busca_Por_GrupoResult grupo = e.Row.DataItem as Sp_Busca_Por_GrupoResult;

            if (grupo != null)
            {
                totalGrupo += Convert.ToDecimal(grupo.valor);
                qtdeGrupo += Convert.ToInt32(grupo.qtde);
            }
        }

        protected void GridViewProforma_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewProforma.FooterRow;

            if (footer != null)
            {
                footer.Cells[0].Text = "Total";
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[1].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[2].Text = totalProforma.ToString("N2");
                footer.Cells[2].BackColor = System.Drawing.Color.PeachPuff;
            }
        }

        protected void GridViewProforma_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Sp_Busca_Por_ProformaResult proforma = e.Row.DataItem as Sp_Busca_Por_ProformaResult;

            if (proforma != null)
                totalProforma += Convert.ToDecimal(proforma.valor);
        }

        protected void btBuscarNelProdutos_Click(object sender, EventArgs e)
        {
            Button btBuscarNelProdutos = sender as Button;

            if (btBuscarNelProdutos != null)
                CarregaGridViewNelProdutos(btBuscarNelProdutos.CommandArgument);
        }

        private void CarregaGridViewNelProdutos(string codigoNel)
        {
            GridViewNelProdutos.DataSource = baseController.BuscaNelProdutos(Convert.ToInt32(codigoNel));
            GridViewNelProdutos.DataBind();
        }

        protected void GridViewNelProdutos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            IMPORTACAO_NEL_PRODUTO nelProduto = e.Row.DataItem as IMPORTACAO_NEL_PRODUTO;

            if (nelProduto != null)
            {
                Literal literalNel = e.Row.FindControl("LiteralNel") as Literal;

                if (literalNel != null)
                    literalNel.Text = baseController.BuscaNelPeloCodigo(nelProduto.CODIGO_NEL).DESCRICAO_NEL;

                Literal literalJanela = e.Row.FindControl("LiteralJanela") as Literal;

                if (literalJanela != null)
                    literalJanela.Text = baseController.BuscaJanela(nelProduto.CODIGO_JANELA).DESCRICAO;

                Literal literalDescricao = e.Row.FindControl("LiteralDescricao") as Literal;

                if (literalDescricao != null)
                    literalDescricao.Text = baseController.BuscaProduto(nelProduto.CODIGO_PRODUTO.ToString()).DESC_PRODUTO;

                Literal literalCor = e.Row.FindControl("LiteralCor") as Literal;

                if (literalCor != null)
                    literalCor.Text = baseController.BuscaProdutoCor(nelProduto.CODIGO_PRODUTO.ToString(), nelProduto.CODIGO_PRODUTO_COR.ToString()).DESC_COR_PRODUTO;
            }
        }

        protected void GridViewNel_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btBuscarNelProdutos = e.Row.FindControl("btBuscarNelProdutos") as Button;

                if (btBuscarNelProdutos != null)
                {
                    if (e.Row.DataItem != null)
                    {
                        IMPORTACAO_NEL listaNel = e.Row.DataItem as IMPORTACAO_NEL;

                        btBuscarNelProdutos.CommandArgument = listaNel.CODIGO_NEL.ToString();
                    }
                }
            }
        }
    }
}
