using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using DAL;
using System.IO;
using System.Drawing;
using System.Text;
using System.Globalization;

namespace Relatorios
{
    public partial class estoque_mercadoria_rel : System.Web.UI.Page
    {
        EstoqueController estoqueController = new EstoqueController();
        BaseController baseController = new BaseController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarFilial();
            }
        }

        #region "DADOS INICIAIS"
        protected void CalendarDataInicio_SelectionChanged(object sender, EventArgs e)
        {
            txtDataInicio.Text = CalendarDataInicio.SelectedDate.ToString("dd/MM/yyyy");
        }
        protected void CalendarDataFim_SelectionChanged(object sender, EventArgs e)
        {
            txtDataFim.Text = CalendarDataFim.SelectedDate.ToString("dd/MM/yyyy");
        }
        private void CarregarFilial()
        {
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                List<FILIAI> lstFilial = new List<FILIAI>();
                List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

                lstFilial = baseController.BuscaFiliais(usuario).Where(p => p.TIPO_FILIAL.Trim() == "LOJA").ToList();
                filialDePara = baseController.BuscaFilialDePara();

                lstFilial = lstFilial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();

                if (lstFilial.Count > 0)
                {
                    lstFilial.Insert(0, new FILIAI { COD_FILIAL = "", FILIAL = "" });
                    ddlFilial.DataSource = lstFilial;
                    ddlFilial.DataBind();

                    if (lstFilial.Count == 2)
                    {
                        ddlFilial.SelectedIndex = 1;
                        ddlFilial.Enabled = false;
                    }
                }
            }
        }
        #endregion

        #region "ACOES"
        private void CarregarStatusMercadoria()
        {
            DateTime? dataini = null;
            DateTime? datafim = null;

            if (txtDataInicio.Text.Trim() != "")
                dataini = Convert.ToDateTime(txtDataInicio.Text);

            if (txtDataFim.Text.Trim() != "")
                datafim = Convert.ToDateTime(txtDataFim.Text);

            var _relMercadoria = estoqueController.GerarRelatorioMercadoria(txtProduto.Text.Trim(), ddlFilial.SelectedItem.Text, txtNF.Text.Trim(), dataini, datafim);

            gvMercadoria.DataSource = _relMercadoria;
            gvMercadoria.DataBind();

        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            DateTime v_dataini;
            DateTime v_datafim;

            bool selected = false;

            labErro.Text = "";
            if (txtDataInicio.Text.Trim() != "" && !DateTime.TryParse(txtDataInicio.Text, out v_dataini))
            {
                labErro.Text = "INFORME UMA DATA INICIAL VÁLIDA.";
                return;
            }
            if (txtDataFim.Text.Trim() != "" && !DateTime.TryParse(txtDataFim.Text, out v_datafim))
            {
                labErro.Text = "INFORME UMA DATA FINAL VÁLIDA.";
                return;
            }

            if (txtDataInicio.Text != "" || txtDataFim.Text != "" || ddlFilial.SelectedItem.Text.Trim() != "" || txtNF.Text.Trim() != "" || txtProduto.Text.Trim() != "")
                selected = true;

            try
            {
                labErro.Text = "";
                if (selected)
                    CarregarStatusMercadoria();
                else
                    labErro.Text = "Informe pelo menos um Filtro.";
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }

        }
        protected void gvMercadoria_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_GERAR_RELATORIO_MERCADORIAResult _finalizado = e.Row.DataItem as SP_GERAR_RELATORIO_MERCADORIAResult;

                    coluna += 1;
                    if (_finalizado != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        CheckBox cbEst = e.Row.FindControl("cbEst") as CheckBox;
                        if (_finalizado.CONFERIDO == 'S')
                        {
                            cbEst.Checked = true;
                            e.Row.BackColor = Color.PaleGreen;
                        }
                        else
                        {
                            cbEst.Checked = false;
                            e.Row.BackColor = Color.White;
                        }


                    }
                }
            }
        }

        #endregion

        protected void cbEst_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                CheckBox cb = (CheckBox)sender;
                GridViewRow row = (GridViewRow)cb.NamingContainer;

                int codigoEstoqueLojaProduto = Convert.ToInt32(gvMercadoria.DataKeys[row.RowIndex].Value);

                var estLojaProd = estoqueController.ObterEstoqueLojaRecebProduto(codigoEstoqueLojaProduto);
                if (estLojaProd != null)
                {
                    estLojaProd.CONFERIDO = (cb.Checked) ? 'S' : 'N';

                    estoqueController.AtualizarEstoqueLojaRecebProduto(estLojaProd);

                    if (cb.Checked)
                        row.BackColor = Color.PaleGreen;
                    else
                        row.BackColor = Color.White;

                }

            }
            catch (Exception ex)
            {
                labErro.Text = ex.Message;
            }
        }
    }
}
