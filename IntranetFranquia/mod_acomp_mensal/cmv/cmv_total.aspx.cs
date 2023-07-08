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
    public partial class cmv_total : System.Web.UI.Page
    {
        DREController dreController = new DREController();
        BaseController baseController = new BaseController();

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

        List<SP_DRE_OBTER_DADOSResult> cmvatacado;
        List<SP_DRE_OBTER_DADOSResult> cmvvarejo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarDataAno();
                //Carregar Filiais
                CarregarFilial();
                //Carregar com a data atual
                btBuscar_Click(null, null);
            }
        }

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            /*
            List<FILIAI> filial = new List<FILIAI>();
            filial = baseController.BuscaFiliais();

            if (filial != null)
            {
                filial.Insert(0, new FILIAI { COD_FILIAL = "0", FILIAL = "" });
                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();
            }*/
        }
        private void CarregarDataAno()
        {
            var dataAno = baseController.ObterDataAno();
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
        private void CarregarCMV(DateTime dataIni, DateTime dataFim, string filial)
        {
            cmvatacado = dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 5, false);
            cmvvarejo = dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 6, false);

            var cmv = cmvatacado.Union(cmvvarejo).ToList();
            cmv = cmv.GroupBy(b => new { GRUPO = b.Grupo }).Select(
                                                                            k => new SP_DRE_OBTER_DADOSResult
                                                                            {
                                                                                Id = 1,
                                                                                Grupo = "CUSTO DOS PRODUTOS",
                                                                                JANEIRO = k.Sum(j => j.JANEIRO),
                                                                                FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                                MARCO = k.Sum(j => j.MARCO),
                                                                                ABRIL = k.Sum(j => j.ABRIL),
                                                                                MAIO = k.Sum(j => j.MAIO),
                                                                                JUNHO = k.Sum(j => j.JUNHO),
                                                                                JULHO = k.Sum(j => j.JULHO),
                                                                                AGOSTO = k.Sum(j => j.AGOSTO),
                                                                                SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                                OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                                NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                                DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                            }).ToList();

            cmv.Insert(cmv.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "" });
            cmv.Insert(cmv.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "CMV" });
            cmv.Insert(cmv.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "" });
            if (cmv != null)
            {
                gvCMV.DataSource = cmv;
                gvCMV.DataBind();
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

            //if (ddlFilial.SelectedValue.Trim() != "" && ddlFilial.SelectedValue.Trim() != "0")
            //filial = ddlFilial.SelectedValue;

            try
            {
                CarregarCMV(v_dataini, v_datafim, filial);
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }

        }

        protected void gvCMV_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult cmv = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (cmv != null && cmv.Grupo != "")
                    {
                        Literal _receitaBruta = e.Row.FindControl("litReceitaBruta") as Literal;
                        if (_receitaBruta != null)
                            _receitaBruta.Text = "<font size='2' face='Calibri'>&nbsp;" + cmv.Grupo.Trim() + "</font> ";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(cmv.JANEIRO);
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(cmv.FEVEREIRO);
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(cmv.MARCO);
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(cmv.ABRIL);
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(cmv.MAIO);
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(cmv.JUNHO);
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(cmv.JULHO);
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(cmv.AGOSTO);
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(cmv.SETEMBRO);
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(cmv.OUTUBRO);
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(cmv.NOVEMBRO);
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(cmv.DEZEMBRO);

                        totalLinha = Convert.ToDecimal(cmv.JANEIRO + cmv.FEVEREIRO + cmv.MARCO + cmv.ABRIL + cmv.MAIO + cmv.JUNHO + cmv.JULHO + cmv.AGOSTO + cmv.SETEMBRO + cmv.OUTUBRO + cmv.NOVEMBRO + cmv.DEZEMBRO);
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                        //SOMATORIO
                        dJaneiro += Convert.ToDecimal(cmv.JANEIRO);
                        dFevereiro += Convert.ToDecimal(cmv.FEVEREIRO);
                        dMarco += Convert.ToDecimal(cmv.MARCO);
                        dAbril += Convert.ToDecimal(cmv.ABRIL);
                        dMaio += Convert.ToDecimal(cmv.MAIO);
                        dJunho += Convert.ToDecimal(cmv.JUNHO);
                        dJulho += Convert.ToDecimal(cmv.JULHO);
                        dAgosto += Convert.ToDecimal(cmv.AGOSTO);
                        dSetembro += Convert.ToDecimal(cmv.SETEMBRO);
                        dOutubro += Convert.ToDecimal(cmv.OUTUBRO);
                        dNovembro += Convert.ToDecimal(cmv.NOVEMBRO);
                        dDezembro += Convert.ToDecimal(cmv.DEZEMBRO);
                        dTotal += totalLinha;

                        e.Row.Cells[gvCMV.Columns.Count - 1].Attributes["style"] = "border-right: 1px solid #FFF";
                    }

                    if (cmv.Grupo == "")
                        e.Row.Height = Unit.Pixel(5);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (cmv.Id == 1)
                        {
                            GridView gvCMVItem = e.Row.FindControl("gvCMVItem") as GridView;
                            if (gvCMVItem != null)
                            {
                                var cmvAgrupado = cmvatacado.Union(cmvvarejo).ToList();
                                cmvAgrupado = cmvAgrupado.GroupBy(b => new { LINHA = b.Linha }).Select(
                                                                        k => new SP_DRE_OBTER_DADOSResult
                                                                        {
                                                                            Id = 1,
                                                                            Linha = k.Key.LINHA.Trim().ToUpper(),
                                                                            JANEIRO = k.Sum(j => j.JANEIRO),
                                                                            FEVEREIRO = k.Sum(j => j.FEVEREIRO),
                                                                            MARCO = k.Sum(j => j.MARCO),
                                                                            ABRIL = k.Sum(j => j.ABRIL),
                                                                            MAIO = k.Sum(j => j.MAIO),
                                                                            JUNHO = k.Sum(j => j.JUNHO),
                                                                            JULHO = k.Sum(j => j.JULHO),
                                                                            AGOSTO = k.Sum(j => j.AGOSTO),
                                                                            SETEMBRO = k.Sum(j => j.SETEMBRO),
                                                                            OUTUBRO = k.Sum(j => j.OUTUBRO),
                                                                            NOVEMBRO = k.Sum(j => j.NOVEMBRO),
                                                                            DEZEMBRO = k.Sum(j => j.DEZEMBRO)
                                                                        }).ToList();
                                gvCMVItem.DataSource = cmvAgrupado;
                                gvCMVItem.DataBind();
                            }
                            img.Visible = true;
                        }

                    }
                }
            }
        }
        protected void gvCMV_DataBound(object sender, EventArgs e)
        {
            //Tratamento de cores para linhas
            GridViewRow firstBlankRow = gvCMV.Rows[gvCMV.Rows.Count - 3];
            if (firstBlankRow != null)
            {
                firstBlankRow.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
                firstBlankRow.Cells[0].Attributes["style"] = "border-bottom: 1px solid #F5F5F5";
            }

            GridViewRow lastBlankRow = gvCMV.Rows[gvCMV.Rows.Count - 1];
            if (lastBlankRow != null)
            {
                //Tratamento para cor
                lastBlankRow.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
                int count = gvCMV.Columns.Count;
                for (int i = 0; i < count; i++)
                    lastBlankRow.Cells[i].BorderWidth = Unit.Pixel(1);
            }

            GridViewRow lastRow = gvCMV.Rows[gvCMV.Rows.Count - 2];
            if (lastRow != null)
            {
                lastRow.Visible = false;
                /*
                lastRow.BackColor = System.Drawing.Color.GhostWhite;
                lastRow.Cells[1].Text = "&nbsp;CMV";
                lastRow.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                lastRow.Cells[1].Font.Name = "Calibri";
                lastRow.Cells[2].Text = dJaneiro.ToString("###,###,###,###,##0.00");
                lastRow.Cells[2].Font.Name = "Calibri";
                lastRow.Cells[3].Text = dFevereiro.ToString("###,###,###,###,##0.00");
                lastRow.Cells[3].Font.Name = "Calibri";
                lastRow.Cells[4].Text = dMarco.ToString("###,###,###,###,##0.00");
                lastRow.Cells[4].Font.Name = "Calibri";
                lastRow.Cells[5].Text = dAbril.ToString("###,###,###,###,##0.00");
                lastRow.Cells[5].Font.Name = "Calibri";
                lastRow.Cells[6].Text = dMaio.ToString("###,###,###,###,##0.00");
                lastRow.Cells[6].Font.Name = "Calibri";
                lastRow.Cells[7].Text = dJunho.ToString("###,###,###,###,##0.00");
                lastRow.Cells[7].Font.Name = "Calibri";
                lastRow.Cells[8].Text = dJulho.ToString("###,###,###,###,##0.00");
                lastRow.Cells[8].Font.Name = "Calibri";
                lastRow.Cells[9].Text = dAgosto.ToString("###,###,###,###,##0.00");
                lastRow.Cells[9].Font.Name = "Calibri";
                lastRow.Cells[10].Text = dSetembro.ToString("###,###,###,###,##0.00");
                lastRow.Cells[10].Font.Name = "Calibri";
                lastRow.Cells[11].Text = dOutubro.ToString("###,###,###,###,##0.00");
                lastRow.Cells[11].Font.Name = "Calibri";
                lastRow.Cells[12].Text = dNovembro.ToString("###,###,###,###,##0.00");
                lastRow.Cells[12].Font.Name = "Calibri";
                lastRow.Cells[13].Text = dDezembro.ToString("###,###,###,###,##0.00");
                lastRow.Cells[13].Font.Name = "Calibri";
                lastRow.Cells[14].Text = dTotal.ToString("###,###,###,###,##0.00");
                lastRow.Cells[14].Font.Name = "Calibri";
                 * */
            }
        }

        protected void gvCMVItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;
            string url_link = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult cmvlinha = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (cmvlinha != null && cmvlinha.Grupo != "")
                    {
                        Literal _litFaturamento = e.Row.FindControl("litFaturamento") as Literal;
                        if (_litFaturamento != null)
                        {
                            if (cmvlinha.Linha.ToUpper().Trim() == "VAREJO")
                                url_link = "cmv_varejo.aspx";
                            else
                                url_link = "cmv_atacado.aspx";
                            _litFaturamento.Text = "<font size='2' face='Calibri'><a href='" + url_link + "' class='adre' title='" + cmvlinha.Linha.Trim() + "'>&nbsp;" + cmvlinha.Linha.Trim().ToUpper() + "</a></font> ";
                        }

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(cmvlinha.JANEIRO);
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(cmvlinha.FEVEREIRO);
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(cmvlinha.MARCO);
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(cmvlinha.ABRIL);
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(cmvlinha.MAIO);
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(cmvlinha.JUNHO);
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(cmvlinha.JULHO);
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(cmvlinha.AGOSTO);
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(cmvlinha.SETEMBRO);
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(cmvlinha.OUTUBRO);
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(cmvlinha.NOVEMBRO);
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(cmvlinha.DEZEMBRO);

                        totalLinha = Convert.ToDecimal(cmvlinha.JANEIRO + cmvlinha.FEVEREIRO + cmvlinha.MARCO + cmvlinha.ABRIL + cmvlinha.MAIO + cmvlinha.JUNHO + cmvlinha.JULHO + cmvlinha.AGOSTO + cmvlinha.SETEMBRO + cmvlinha.OUTUBRO + cmvlinha.NOVEMBRO + cmvlinha.DEZEMBRO);
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (cmvlinha.Linha.Trim().ToUpper() == "VAREJO")
                        {
                            GridView gvCMVItemSub = e.Row.FindControl("gvCMVItemSub") as GridView;
                            if (gvCMVItemSub != null)
                            {
                                gvCMVItemSub.DataSource = cmvvarejo;
                                gvCMVItemSub.DataBind();
                            }
                            img.Visible = true;
                        }
                    }
                }
            }
        }
        protected void gvCMVItem_DataBound(object sender, EventArgs e)
        {
            GridView gvCMVItem = (GridView)sender;
            if (gvCMVItem != null)
                if (gvCMVItem.HeaderRow != null)
                    gvCMVItem.HeaderRow.Visible = false;
        }

        protected void gvCMVItemSub_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult subitem = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (subitem != null && subitem.Grupo != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                        {
                            if (subitem.Linha.ToUpper().Contains("VAREJO"))
                                _litSubItem.Text = "<font size='1' face='Calibri'>&nbsp;" + subitem.Filial.Trim().ToUpper() + "</font> ";
                            else
                                _litSubItem.Text = "<font size='2' face='Calibri'>&nbsp;&nbsp;&nbsp;&nbsp;(-) " + subitem.Linha.Trim().ToUpper() + "</font> ";
                        }

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(subitem.JANEIRO);
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(subitem.FEVEREIRO);
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(subitem.MARCO);
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(subitem.ABRIL);
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(subitem.MAIO);
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(subitem.JUNHO);
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(subitem.JULHO);
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(subitem.AGOSTO);
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(subitem.SETEMBRO);
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(subitem.OUTUBRO);
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(subitem.NOVEMBRO);
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(subitem.DEZEMBRO);

                        totalLinha = Convert.ToDecimal(subitem.JANEIRO + subitem.FEVEREIRO + subitem.MARCO + subitem.ABRIL + subitem.MAIO + subitem.JUNHO + subitem.JULHO + subitem.AGOSTO + subitem.SETEMBRO + subitem.OUTUBRO + subitem.NOVEMBRO + subitem.DEZEMBRO);
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                    }

                }
            }
        }
        protected void gvCMVItemSub_DataBound(object sender, EventArgs e)
        {
            GridView gvCMVItemSub = (GridView)sender;
            if (gvCMVItemSub != null)
                if (gvCMVItemSub.HeaderRow != null)
                    gvCMVItemSub.HeaderRow.Visible = false;

            if (gvCMVItemSub.Rows.Count > 0)
            {
                int count = gvCMVItemSub.Columns.Count;
                foreach (GridViewRow row in gvCMVItemSub.Rows)
                    row.Attributes["style"] = "border: 1px solid #FFF";
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
