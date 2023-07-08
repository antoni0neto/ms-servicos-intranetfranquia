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
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Text;

namespace Relatorios
{
    public partial class dre_dre_custo_fixo : System.Web.UI.Page
    {
        DREController dreController = new DREController();

        decimal total = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Request.QueryString["a"] == null || Request.QueryString["a"] == "" ||
                    Request.QueryString["m"] == null || Request.QueryString["m"] == "")
                {
                    Response.Write("NENHUM REGISTRO ENCONTRADO.");
                    Response.End();
                }

                string mes = Request.QueryString["m"].ToString();
                string ano = Request.QueryString["a"].ToString();
                string tipo = Request.QueryString["t"].ToString();

                hidMes.Value = mes;
                hidAno.Value = ano;
                hidTipo.Value = tipo;

                CarregarFuncionarios(mes, ano, tipo);
            }
        }

        #region "ACOES"
        private List<SP_DRE_MAODEOBRADIRETAResult> ObterMaoDeObraDireta(int mes, int ano, string tipo)
        {
            var maoObraDireta = dreController.ObterMaoDeObraDireta(mes, ano);


            string tipoAux = "";

            if (tipo == "SALÁRIO LÍQUIDO")
                tipoAux = "SalarioLiquido";
            else if (tipo == "FERIAS")
                tipoAux = "Ferias";
            else if (tipo == "RESCISOES")
                tipoAux = "Rescisoes";
            else if (tipo == "BENEFICIOS")
                tipoAux = "Beneficios";
            else if (tipo == "ENCARGOS")
                tipoAux = "FGTS";
            else
                tipoAux = "";

            maoObraDireta = maoObraDireta.Where(p => p.TIPOVALOR.Trim().ToUpper().Contains(tipoAux.Trim().ToUpper())).ToList();

            return maoObraDireta;
        }
        private void CarregarFuncionarios(string mes, string ano, string tipo)
        {

            txtCompetencia.Text = ((mes.Length == 1) ? "0" + mes : mes) + "/" + ano;
            txtTipo.Text = tipo;

            gvProducao.DataSource = ObterMaoDeObraDireta(Convert.ToInt32(mes), Convert.ToInt32(ano), tipo);
            gvProducao.DataBind();

        }
        protected void gvProducao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_MAODEOBRADIRETAResult maoObraDireta = e.Row.DataItem as SP_DRE_MAODEOBRADIRETAResult;

                    if (maoObraDireta != null)
                    {
                        Literal _litValor = e.Row.FindControl("litValor") as Literal;
                        if (_litValor != null)
                            _litValor.Text = "R$ " + maoObraDireta.VALOR.ToString("###,###,##0.00");

                        total += maoObraDireta.VALOR;

                    }
                }
            }
        }
        protected void gvProducao_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvProducao.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;

                footer.Cells[3].Text = "R$ " + total.ToString("###,###,###,###,##0.00");

                txtValorTotal.Text = total.ToString("###,###,###,###,##0.00");


            }
        }
        protected void gvProducao_Sorting(object sender, GridViewSortEventArgs e)
        {
            IEnumerable<SP_DRE_MAODEOBRADIRETAResult> maoObraDireta = ObterMaoDeObraDireta(Convert.ToInt32(hidMes.Value), Convert.ToInt32(hidAno.Value), hidTipo.Value);

            //string sortExpression = e.SortExpression;
            SortDirection sort;

            Utils.WebControls.GridViewSortDirection(((GridView)sender), e, out sort);

            if (sort == SortDirection.Ascending)
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - >>");
            else
                Utils.WebControls.GetBoundFieldIndexByName(((GridView)sender), e.SortExpression, " - <<");

            string sortDirection = (sort == SortDirection.Ascending) ? " ASC " : " DESC ";

            maoObraDireta = maoObraDireta.OrderBy(e.SortExpression + sortDirection);
            gvProducao.DataSource = maoObraDireta;
            gvProducao.DataBind();
        }
        #endregion

        private string FormatarValor(decimal? valor)
        {
            string tagCor = "#000";
            if (valor < 0)
                tagCor = "#CD2626";

            return "<font size='2' face='Calibri' color='" + tagCor + "'>" + Convert.ToDecimal(valor).ToString("###,###,###,##0.00;(###,###,###,##0.00)") + "</font> ";
        }

    }
}

