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
    public partial class jur_processo_rel : System.Web.UI.Page
    {
        JuridicoController jurController = new JuridicoController();
        int coluna = 0, colunaInstancia = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarTipoProcesso();
                CarregarInstancia();
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
        private void CarregarTipoProcesso()
        {
            List<JUR_TIPO_PROCESSO> _processo = jurController.ObterTipoProcesso();

            if (_processo != null)
            {
                _processo.Insert(0, new JUR_TIPO_PROCESSO { CODIGO = 0, DESCRICAO = "" });
                ddlTipoProcesso.DataSource = _processo;
                ddlTipoProcesso.DataBind();
            }
        }
        private void CarregarInstancia()
        {
            List<JUR_TIPO_INSTANCIA> _instancia = jurController.ObterTipoInstancia();

            if (_instancia != null)
            {
                _instancia.Insert(0, new JUR_TIPO_INSTANCIA { CODIGO = 0, DESCRICAO = "" });
                ddlInstancia.DataSource = _instancia;
                ddlInstancia.DataBind();
            }
        }
        #endregion

        #region "ACOES"

        private List<SP_OBTER_PROCESSO_JURIDICOResult> ObterProcessos()
        {
            DateTime? dataIni = null;
            DateTime? dataFim = null;
            string numero = null;
            int? tipoProcesso = null;
            int? instancia = null;
            char? status = null;
            List<SP_OBTER_PROCESSO_JURIDICOResult> _processos = new List<SP_OBTER_PROCESSO_JURIDICOResult>();

            if (txtDataInicio.Text.Trim() != "")
                dataIni = Convert.ToDateTime(txtDataInicio.Text);
            if (txtDataFim.Text.Trim() != "")
                dataFim = Convert.ToDateTime(txtDataFim.Text);
            if (txtNumero.Text.Trim() != "")
                numero = txtNumero.Text.Trim();
            if (ddlTipoProcesso.SelectedValue != "0")
                tipoProcesso = Convert.ToInt32(ddlTipoProcesso.SelectedValue);
            if (ddlInstancia.SelectedValue != "0")
                instancia = Convert.ToInt32(ddlInstancia.SelectedValue);
            if (ddlStatus.SelectedValue != "")
                status = Convert.ToChar(ddlStatus.SelectedValue);

            _processos = jurController.ObterProcessoJuridico(dataIni, dataFim, numero, tipoProcesso, instancia, status);

            return _processos;
        }
        private void CarregarProcessos()
        {
            List<SP_OBTER_PROCESSO_JURIDICOResult> _processos = new List<SP_OBTER_PROCESSO_JURIDICOResult>();
            _processos = ObterProcessos();

            if (_processos != null)
            {
                gvProcesso.DataSource = _processos;
                gvProcesso.DataBind();
            }
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            DateTime v_dataini;
            DateTime v_datafim;

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

            try
            {
                CarregarProcessos();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }

        }
        protected void gvProcesso_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_PROCESSO_JURIDICOResult _processos = e.Row.DataItem as SP_OBTER_PROCESSO_JURIDICOResult;

                    coluna += 1;
                    if (_processos != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = coluna.ToString();

                        Literal _litStatus = e.Row.FindControl("litStatus") as Literal;
                        if (_litStatus != null)
                        {
                            if (_processos.STATUS == "1")
                                _litStatus.Text = "AGUAR. JULGAMENTO";
                            else if (_processos.STATUS == "2")
                                _litStatus.Text = "AGUAR. SENTENÇA";
                            else
                                _litStatus.Text = "FINALIZADO";
                        }

                        GridView gvInstancia = e.Row.FindControl("gvInstancia") as GridView;
                        if (gvInstancia != null)
                        {
                            var instancia = jurController.ObterProcessoInstancia().Where(p => p.JUR_PROCESSO == _processos.CODIGO);
                            if (instancia != null)
                            {
                                gvInstancia.DataSource = instancia;
                                gvInstancia.DataBind();

                                colunaInstancia = 0;
                            }
                        }
                    }
                }
            }
        }
        protected void gvInstancia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    JUR_PROCESSO_INSTANCIA _instancia = e.Row.DataItem as JUR_PROCESSO_INSTANCIA;

                    colunaInstancia += 1;
                    if (_instancia != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = colunaInstancia.ToString();

                        Literal _litInstancia = e.Row.FindControl("litInstancia") as Literal;
                        if (_litInstancia != null)
                            _litInstancia.Text = (_instancia.JUR_TIPO_INSTANCIA1 == null) ? "" : _instancia.JUR_TIPO_INSTANCIA1.DESCRICAO;
                    }
                }
            }
        }
        #endregion

    }
}
