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
    public partial class prod_cad_hb_altera_data : System.Web.UI.Page
    {
        ProducaoController prodController = new ProducaoController();
        int coluna = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CarregarColecoes();
            }
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {

            char mostruario = ' ';

            try
            {
                labErro.Text = "";
                if (ddlColecao.SelectedValue == "" || ddlColecao.SelectedValue == "0")
                {
                    labErro.Text = "Selecione a Coleção.";
                    return;
                }
                if ((txtHB.Text == "" || Convert.ToInt32(txtHB.Text) <= 0))
                {
                    labErro.Text = "Informe o número do HB.";
                    return;
                }

                mostruario = (chkMostruario.Checked) ? 'S' : 'N';
                var processoHB = prodController.ObterProcesso(ddlColecao.SelectedValue, Convert.ToInt32(txtHB.Text), mostruario);

                gvProcessoHB.DataSource = processoHB.OrderBy(p => ((p.PROD_HB1.PROD_DETALHE == null) ? "" : p.PROD_HB1.PROD_DETALHE1.DESCRICAO)).ThenBy(o => o.PROD_PROCESSO);
                gvProcessoHB.DataBind();

                Session["COLECAO"] = ddlColecao.SelectedValue;
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }
        }

        protected void gvProcessoHB_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    PROD_HB_PROD_PROCESSO processoHB = e.Row.DataItem as PROD_HB_PROD_PROCESSO;

                    coluna += 1;
                    if (processoHB != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Literal _litNome = e.Row.FindControl("litNome") as Literal;
                        if (_litNome != null)
                            _litNome.Text = processoHB.PROD_HB1.NOME;

                        Literal _litCor = e.Row.FindControl("litCor") as Literal;
                        if (_litCor != null)
                            _litCor.Text = prodController.ObterCoresBasicas(processoHB.PROD_HB1.COR).DESC_COR;

                        Literal _litDetalhe = e.Row.FindControl("litDetalhe") as Literal;
                        if (_litDetalhe != null)
                            _litDetalhe.Text = (processoHB.PROD_HB1.PROD_DETALHE1 == null) ? "PRINCIPAL" : processoHB.PROD_HB1.PROD_DETALHE1.DESCRICAO;

                        Literal _litProcesso = e.Row.FindControl("litProcesso") as Literal;
                        if (_litProcesso != null)
                            _litProcesso.Text = processoHB.PROD_PROCESSO1.DESCRICAO.ToUpper();

                        TextBox _txtDataInicial = e.Row.FindControl("txtDataInicial") as TextBox;
                        if (_txtDataInicial != null)
                        {
                            _txtDataInicial.Text = "";
                            _txtDataInicial.Enabled = true;
                            if (processoHB.DATA_INICIO != null)
                                _txtDataInicial.Text = Convert.ToDateTime(processoHB.DATA_INICIO).ToString("dd/MM/yyyy");
                            else
                                _txtDataInicial.Enabled = false;

                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + _txtDataInicial.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);
                        }

                        TextBox _txtDataFinal = e.Row.FindControl("txtDataFinal") as TextBox;
                        if (_txtDataFinal != null)
                        {
                            _txtDataFinal.Text = "";
                            _txtDataFinal.Enabled = true;
                            if (processoHB.DATA_FIM != null)
                                _txtDataFinal.Text = Convert.ToDateTime(processoHB.DATA_FIM).ToString("dd/MM/yyyy");
                            else
                                _txtDataFinal.Enabled = false;

                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "$(function() {$( '#" + _txtDataFinal.ClientID + "' ).datepicker({dateFormat: 'dd/mm/yy', maxDate: new Date() });});", true);
                        }

                    }
                }
            }
        }
        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                int codigoProcesso = 0;
                TextBox _txtDataIni = null;
                TextBox _txtDataFim = null;
                PROD_HB_PROD_PROCESSO processoHB = null;

                foreach (GridViewRow row in gvProcessoHB.Rows)
                {
                    codigoProcesso = Convert.ToInt32(gvProcessoHB.DataKeys[row.RowIndex].Value);

                    _txtDataIni = row.FindControl("txtDataInicial") as TextBox;
                    _txtDataFim = row.FindControl("txtDataFinal") as TextBox;

                    processoHB = new PROD_HB_PROD_PROCESSO();
                    processoHB.CODIGO = codigoProcesso;
                    if (_txtDataIni != null && _txtDataIni.Text.Trim() != "")
                        processoHB.DATA_INICIO = Convert.ToDateTime(_txtDataIni.Text);

                    if (_txtDataFim != null && _txtDataFim.Text.Trim() != "")
                        processoHB.DATA_FIM = Convert.ToDateTime(_txtDataFim.Text);

                    prodController.AtualizarProcessoHB(processoHB);
                }

                labErro.Text = "Processo do HB Atualizado com Sucesso.";
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
