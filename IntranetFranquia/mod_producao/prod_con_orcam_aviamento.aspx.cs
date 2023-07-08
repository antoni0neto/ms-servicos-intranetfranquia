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

namespace Relatorios
{
    public partial class prod_con_orcam_aviamento : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        List<PROD_HB_PROD_AVIAMENTO> lstAviamento = new List<PROD_HB_PROD_AVIAMENTO>();

        int colunaAviamento = 0;
        int colunaAviamentoSel = 0;
        decimal qtdeSel = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Carregar data default
                CarregarAviamentos();

                //Controle de orcamento a realizar
                btFechar.Visible = false;
                divQtdeSelecionada.Visible = false;

                cbMarcarTodos.Visible = false;
                labMarcarTodos.Visible = false;

                Session["AVIAMENTO"] = null;
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarAviamentos()
        {
            List<PROD_AVIAMENTO> _aviamento = prodController.ObterAviamento();
            if (_aviamento != null)
            {
                _aviamento = _aviamento.Where(p => p.STATUS == 'A').ToList();
                _aviamento.Insert(0, new PROD_AVIAMENTO { CODIGO = 0, DESCRICAO = "" });

                ddlAviamento.DataSource = _aviamento;
                ddlAviamento.DataBind();
            }
        }
        #endregion

        #region "PESQUISA"
        protected void btBuscarAviamentos_Click(object sender, EventArgs e)
        {
            try
            {
                labErro.Text = "";

                /*if (ddlAviamento.SelectedValue == "0")
                {
                    labErro.Text = "Selecione o Aviamento.";
                    return;
                }*/

                Session["AVIAMENTO"] = null;
                CarregarAviamentosParaOrcamento();
                CarregarAviamentosSelecionados();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }

        }
        private void CarregarAviamentosParaOrcamento()
        {
            List<PROD_HB_AVIAMENTO_ORCAMENTO_AVI> aviamentoComprado = prodController.ObterAviamentoComprado();
            List<PROD_HB_PROD_AVIAMENTO> _aviamento = prodController.ObterAviamentoHB().Where(p => p.COMPRA_EXTRA == 'N').ToList();

            var resultado = _aviamento.Where(p => !aviamentoComprado.Any(j => j.PROD_HB_PROD_AVIAMENTO == p.CODIGO));

            if (ddlAviamento.SelectedValue != "0")
                resultado = resultado.Where(p => p.PROD_AVIAMENTO.ToString() == ddlAviamento.SelectedValue).ToList();

            if (txtHB.Text.Trim() != "")
                resultado = resultado.Where(p => p.PROD_HB1.HB.ToString() == txtHB.Text.Trim()).ToList();

            resultado = resultado.OrderBy(p => p.PROD_AVIAMENTO1.DESCRICAO).ToList();
            cbMarcarTodos.Visible = false;
            labMarcarTodos.Visible = false;
            labErroImpressao.Text = "";

            if (resultado != null)
            {
                gvAviamento.DataSource = resultado;
                gvAviamento.DataBind();

                if (resultado.Count() > 0)
                {
                    cbMarcarTodos.Visible = true;
                    labMarcarTodos.Visible = true;
                }
            }
        }
        protected void gvAviamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_PROD_AVIAMENTO _aviamento = e.Row.DataItem as PROD_HB_PROD_AVIAMENTO;

                    colunaAviamento += 1;
                    if (_aviamento != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaAviamento.ToString();

                        Literal _HB = e.Row.FindControl("litHB") as Literal;
                        if (_HB != null)
                            _HB.Text = _aviamento.PROD_HB1.HB.ToString();

                        Literal _aviamentoDesc = e.Row.FindControl("litAviamento") as Literal;
                        if (_aviamentoDesc != null)
                            _aviamentoDesc.Text = _aviamento.PROD_AVIAMENTO1.DESCRICAO;

                        Literal _litMedida = e.Row.FindControl("litMedida") as Literal;
                        if (_litMedida != null)
                            _litMedida.Text = _aviamento.PROD_AVIAMENTO1.UNIDADE_MEDIDA1.DESCRICAO + "s";

                        Literal _litQtdeReal = e.Row.FindControl("litQtdeReal") as Literal;
                        if (_litQtdeReal != null)
                            _litQtdeReal.Text = (_aviamento.QTDE + _aviamento.QTDE_EXTRA).ToString().Replace(",000", "");

                        var o = prodController.ObterRelacaoAviamentoOrcamento().Where(p => p.PROD_HB_PROD_AVIAMENTO == _aviamento.CODIGO).Take(1).SingleOrDefault();
                        if (o != null && o.PROD_HB_PROD_AVIAMENTO > 0)
                            e.Row.ForeColor = Color.Red;
                    }
                }
            }
        }
        protected void cbMarcarTodos_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            if (cb.Checked)
                labMarcarTodos.Text = "Desmarcar Todos";
            else
                labMarcarTodos.Text = "Marcar Todos";

            //Des/Marcar todos
            colunaAviamentoSel = 0;
            foreach (GridViewRow item in gvAviamento.Rows)
            {
                CheckBox cbAlterado = item.FindControl("cbCheck") as CheckBox;
                if (cb.Checked != cbAlterado.Checked)
                {
                    cbAlterado.Checked = cb.Checked;
                    cbCheck_CheckedChanged(cbAlterado, null);
                }
            }
        }
        #endregion

        #region "AVIAMENTO SELECIONADO"
        private void CarregarAviamentosSelecionados()
        {
            List<PROD_HB_PROD_AVIAMENTO> _aviamento = Session["AVIAMENTO"] as List<PROD_HB_PROD_AVIAMENTO>;

            gvAviamentoSelecionado.DataSource = _aviamento;
            gvAviamentoSelecionado.DataBind();

            divQtdeSelecionada.Visible = false;
            if (_aviamento != null && _aviamento.Count > 0)
                divQtdeSelecionada.Visible = true;

        }
        protected void cbCheck_CheckedChanged(object sender, EventArgs e)
        {
            PROD_HB_PROD_AVIAMENTO avi = new PROD_HB_PROD_AVIAMENTO();

            CheckBox cb = (CheckBox)sender;
            if (cb != null)
            {
                btFechar.Visible = false;
                divQtdeSelecionada.Visible = false;
                GridViewRow row = (GridViewRow)cb.NamingContainer;
                if (row != null)
                {
                    if (Session["AVIAMENTO"] != null)
                        lstAviamento = Session["AVIAMENTO"] as List<PROD_HB_PROD_AVIAMENTO>;

                    avi = prodController.ObterAviamentoHBCodigo(Convert.ToInt32(gvAviamento.DataKeys[row.RowIndex].Value));
                    if (avi != null)
                    {
                        if (cb.Checked)
                            lstAviamento.Add(avi);
                        else
                            lstAviamento.RemoveAll(p => p.CODIGO == avi.CODIGO);
                    }

                    Session["AVIAMENTO"] = lstAviamento;
                }

                if (lstAviamento.Count > 0)
                {
                    btFechar.Visible = true;
                    divQtdeSelecionada.Visible = true;
                }
                CarregarAviamentosSelecionados();
            }
        }
        protected void gvAviamentoSelecionado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_PROD_AVIAMENTO _aviamento = e.Row.DataItem as PROD_HB_PROD_AVIAMENTO;

                    colunaAviamentoSel += 1;
                    if (_aviamento != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaAviamentoSel.ToString();

                        Literal _HB = e.Row.FindControl("litHB") as Literal;
                        if (_HB != null)
                            _HB.Text = _aviamento.PROD_HB1.HB.ToString();

                        Literal _aviamentoDesc = e.Row.FindControl("litAviamento") as Literal;
                        if (_aviamentoDesc != null)
                            _aviamentoDesc.Text = _aviamento.PROD_AVIAMENTO1.DESCRICAO;

                        qtdeSel += _aviamento.QTDE;
                    }
                }
            }
        }
        protected void gvAviamentoSelecionado_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvAviamentoSelecionado.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                footer.Cells[1].Text = "Total";
                footer.Cells[4].Text = qtdeSel.ToString("###,###,###,##0.000");
            }
        }
        #endregion

        #region "FECHAR ORÇAMENTO"
        protected void btFechar_Click(object sender, EventArgs e)
        {
            string _url = "";
            int cod_orcamento = 0;
            if (gvAviamentoSelecionado.Rows.Count > 0)
            {
                _url = "fnAbrirTelaCadastroMaior('prod_cad_orcam_aviamento.aspx?p=" + cod_orcamento.ToString() + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
            }
        }
        #endregion

        #region "COMPRAR EXTRA"
        protected void btComprarExtra_Click(object sender, EventArgs e)
        {
            string _url = "";
            _url = "fnAbrirTelaCadastroMaior('prod_con_compra_aviamento_extra.aspx');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _url, true);
        }
        #endregion

    }
}
