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
    public partial class cmv_atacado : System.Web.UI.Page
    {
        DREController dreController = new DREController();
        int coluna = 0;

        char intercompany = 'N';

        decimal dJaneiro = 0;
        decimal dFevereiro = 0;
        decimal dMarco = 0;
        decimal dAbril = 0;
        decimal dMaio = 0;
        decimal dJunho = 0;
        decimal dJulho = 0;
        decimal dAgosto = 0;
        decimal dSetembro = 0;
        decimal dOutubro = 0;
        decimal dNovembro = 0;
        decimal dDezembro = 0;
        decimal dTotal = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarDataAno();

                //Carregar com a data atual
                btBuscar_Click(null, null);
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarDataAno()
        {
            var dataAno = (new BaseController().ObterDataAno());
            if (dataAno != null)
            {
                dataAno = dataAno.Where(p => p.STATUS == 'A').ToList();
                ddlAno.DataSource = dataAno;
                ddlAno.DataBind();

                if (ddlAno.Items.Count > 0)
                {
                    try
                    {
                        ddlAno.SelectedValue = DateTime.Now.Year.ToString();
                    }
                    catch (Exception) { }
                }
            }
        }
        #endregion

        #region "ACOES"
        private void CarregarAtacado(DateTime dataIni, DateTime dataFim, string filial)
        {
            var atacado = dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 5, false);

            if (atacado != null)
            {
                gvAtacado.DataSource = atacado;
                gvAtacado.DataBind();
            }
        }
        protected void btBuscar_Click(object sender, EventArgs e)
        {
            DateTime v_dataini;
            DateTime v_datafim;
            string filial = "";
            string ano = "";

            ano = ddlAno.SelectedValue;
            v_dataini = Convert.ToDateTime(ano + "-01-01");
            v_datafim = Convert.ToDateTime(ano + "-12-31");

            try
            {
                CarregarAtacado(v_dataini, v_datafim, filial);
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }

        }
        protected void gvAtacado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult atacado = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    coluna += 1;
                    if (atacado != null)
                    {
                        Literal _col = e.Row.FindControl("litColuna") as Literal;
                        if (_col != null)
                            _col.Text = "<font size='2' face='Calibri'>" + coluna.ToString() + "</font> ";
                        Literal _filial = e.Row.FindControl("litFilial") as Literal;
                        if (_filial != null)
                            _filial.Text = "<font size='2' face='Calibri'>" + atacado.Filial.Trim() + "</font> ";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(atacado.JANEIRO);
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(atacado.FEVEREIRO);
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(atacado.MARCO);
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(atacado.ABRIL);
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(atacado.MAIO);
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(atacado.JUNHO);
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(atacado.JULHO);
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(atacado.AGOSTO);
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(atacado.SETEMBRO);
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(atacado.OUTUBRO);
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(atacado.NOVEMBRO);
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(atacado.DEZEMBRO);
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(atacado.JANEIRO + atacado.FEVEREIRO + atacado.MARCO + atacado.ABRIL + atacado.MAIO + atacado.JUNHO + atacado.JULHO + atacado.AGOSTO + atacado.SETEMBRO + atacado.OUTUBRO + atacado.NOVEMBRO + atacado.DEZEMBRO);

                        //SOMATORIO
                        dJaneiro += Convert.ToDecimal(atacado.JANEIRO);
                        dFevereiro += Convert.ToDecimal(atacado.FEVEREIRO);
                        dMarco += Convert.ToDecimal(atacado.MARCO);
                        dAbril += Convert.ToDecimal(atacado.ABRIL);
                        dMaio += Convert.ToDecimal(atacado.MAIO);
                        dJunho += Convert.ToDecimal(atacado.JUNHO);
                        dJulho += Convert.ToDecimal(atacado.JULHO);
                        dAgosto += Convert.ToDecimal(atacado.AGOSTO);
                        dSetembro += Convert.ToDecimal(atacado.SETEMBRO);
                        dOutubro += Convert.ToDecimal(atacado.OUTUBRO);
                        dNovembro += Convert.ToDecimal(atacado.NOVEMBRO);
                        dDezembro += Convert.ToDecimal(atacado.DEZEMBRO);
                        dTotal += Convert.ToDecimal(atacado.JANEIRO + atacado.FEVEREIRO + atacado.MARCO + atacado.ABRIL + atacado.MAIO + atacado.JUNHO + atacado.JULHO + atacado.AGOSTO + atacado.SETEMBRO + atacado.OUTUBRO + atacado.NOVEMBRO + atacado.DEZEMBRO);
                    }
                }
            }
        }
        protected void gvAtacado_DataBound(object sender, EventArgs e)
        {
            //Tratamento para cor da coluna
            int count = gvAtacado.Columns.Count;
            for (int i = 0; i < count; i++)
                if (i % 2 == 0)
                    gvAtacado.Columns[i].ItemStyle.BackColor = System.Drawing.Color.GhostWhite;

            GridViewRow footer = gvAtacado.FooterRow;

            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                footer.Cells[2].Text = FormatarValor(dJaneiro);
                footer.Cells[3].Text = FormatarValor(dFevereiro);
                footer.Cells[4].Text = FormatarValor(dMarco);
                footer.Cells[5].Text = FormatarValor(dAbril);
                footer.Cells[6].Text = FormatarValor(dMaio);
                footer.Cells[7].Text = FormatarValor(dJunho);
                footer.Cells[8].Text = FormatarValor(dJulho);
                footer.Cells[9].Text = FormatarValor(dAgosto);
                footer.Cells[10].Text = FormatarValor(dSetembro);
                footer.Cells[11].Text = FormatarValor(dOutubro);
                footer.Cells[12].Text = FormatarValor(dNovembro);
                footer.Cells[13].Text = FormatarValor(dDezembro);
                footer.Cells[14].Text = FormatarValor(dTotal);
            }
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
