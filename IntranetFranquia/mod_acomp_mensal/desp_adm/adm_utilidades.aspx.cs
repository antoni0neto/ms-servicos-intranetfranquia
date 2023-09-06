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
    public partial class adm_utilidades : System.Web.UI.Page
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

        List<SP_DRE_OBTER_DADOSResult> admUtilidade;

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
            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");

            List<FILIAI> filial = new List<FILIAI>();
            filial = baseController.BuscaTodasFiliais();
            if (filial != null)
            {
                filial.Insert(0, new FILIAI { COD_FILIAL = "0", FILIAL = "" });
                ddlFilial.DataSource = filial;
                ddlFilial.DataBind();
            }
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
        private void CarregarUtilidade(DateTime dataIni, DateTime dataFim, string filial)
        {
            admUtilidade = dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 10, false);

            var admUtilidadeAgrupado = admUtilidade.ToList();
            admUtilidadeAgrupado = admUtilidadeAgrupado.GroupBy(b => new { TIPO = b.Tipo }).Select(
                                                                            k => new SP_DRE_OBTER_DADOSResult
                                                                            {
                                                                                Id = 1,
                                                                                Tipo = k.Key.TIPO.Trim().ToUpper(),
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

            admUtilidadeAgrupado.Insert(admUtilidadeAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Tipo = "" });
            admUtilidadeAgrupado.Insert(admUtilidadeAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Tipo = "Utilidades" });
            admUtilidadeAgrupado.Insert(admUtilidadeAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Tipo = "" });
            if (admUtilidadeAgrupado != null)
            {
                gvUtilidade.DataSource = admUtilidadeAgrupado;
                gvUtilidade.DataBind();
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

            if (ddlFilial.SelectedValue.Trim() != "" && ddlFilial.SelectedValue.Trim() != "0")
                filial = ddlFilial.SelectedValue;

            try
            {
                CarregarUtilidade(v_dataini, v_datafim, filial);
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }

        }

        protected void gvUtilidade_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult utilidade = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (utilidade != null && utilidade.Tipo != "")
                    {
                        Literal _litUtilidade = e.Row.FindControl("litUtilidade") as Literal;
                        if (_litUtilidade != null)
                            _litUtilidade.Text = "<font size='2' face='Calibri'>&nbsp;" + utilidade.Tipo.Trim().ToUpper() + "</font> ";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(utilidade.JANEIRO);
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(utilidade.FEVEREIRO);
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(utilidade.MARCO);
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(utilidade.ABRIL);
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(utilidade.MAIO);
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(utilidade.JUNHO);
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(utilidade.JULHO);
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(utilidade.AGOSTO);
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(utilidade.SETEMBRO);
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(utilidade.OUTUBRO);
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(utilidade.NOVEMBRO);
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(utilidade.DEZEMBRO);

                        totalLinha = Convert.ToDecimal(utilidade.JANEIRO + utilidade.FEVEREIRO + utilidade.MARCO + utilidade.ABRIL + utilidade.MAIO + utilidade.JUNHO + utilidade.JULHO + utilidade.AGOSTO + utilidade.SETEMBRO + utilidade.OUTUBRO + utilidade.NOVEMBRO + utilidade.DEZEMBRO);
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                        //SOMATORIO
                        dJaneiro += Convert.ToDecimal(utilidade.JANEIRO);
                        dFevereiro += Convert.ToDecimal(utilidade.FEVEREIRO);
                        dMarco += Convert.ToDecimal(utilidade.MARCO);
                        dAbril += Convert.ToDecimal(utilidade.ABRIL);
                        dMaio += Convert.ToDecimal(utilidade.MAIO);
                        dJunho += Convert.ToDecimal(utilidade.JUNHO);
                        dJulho += Convert.ToDecimal(utilidade.JULHO);
                        dAgosto += Convert.ToDecimal(utilidade.AGOSTO);
                        dSetembro += Convert.ToDecimal(utilidade.SETEMBRO);
                        dOutubro += Convert.ToDecimal(utilidade.OUTUBRO);
                        dNovembro += Convert.ToDecimal(utilidade.NOVEMBRO);
                        dDezembro += Convert.ToDecimal(utilidade.DEZEMBRO);
                        dTotal += totalLinha;

                        e.Row.Cells[gvUtilidade.Columns.Count - 1].Attributes["style"] = "border-right: 1px solid #FFF";
                    }

                    if (utilidade.Tipo == "")
                        e.Row.Height = Unit.Pixel(5);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (utilidade.Id == 1)
                        {
                            GridView gvUtilidadeItem = e.Row.FindControl("gvUtilidadeItem") as GridView;
                            if (gvUtilidadeItem != null)
                            {
                                gvUtilidadeItem.DataSource = admUtilidade.Where(p => p.Tipo.Trim().ToUpper() == utilidade.Tipo.Trim().ToUpper());
                                gvUtilidadeItem.DataBind();
                            }
                            img.Visible = true;
                        }
                    }
                }
            }
        }
        protected void gvUtilidade_DataBound(object sender, EventArgs e)
        {
            //Tratamento de cores para linhas
            GridViewRow firstBlankRow = gvUtilidade.Rows[gvUtilidade.Rows.Count - 3];
            if (firstBlankRow != null)
            {
                firstBlankRow.BackColor = Color.Gainsboro;
                firstBlankRow.Cells[0].Attributes["style"] = "border-bottom: 1px solid #F5F5F5";
            }

            GridViewRow lastBlankRow = gvUtilidade.Rows[gvUtilidade.Rows.Count - 1];
            if (lastBlankRow != null)
            {
                //Tratamento para cor
                lastBlankRow.BackColor = Color.Gainsboro;
                int count = gvUtilidade.Columns.Count;
                for (int i = 0; i < count; i++)
                    lastBlankRow.Cells[i].BorderWidth = Unit.Pixel(1);
            }

            GridViewRow lastRow = gvUtilidade.Rows[gvUtilidade.Rows.Count - 2];
            if (lastRow != null)
            {
                //lastRow.Visible = false;

                lastRow.BackColor = System.Drawing.Color.GhostWhite;
                lastRow.Cells[1].Text = "&nbsp;UTILIDADES";
                lastRow.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                lastRow.Cells[1].Font.Name = "Calibri";
                lastRow.Cells[2].Text = FormatarValor(dJaneiro);
                lastRow.Cells[2].Font.Name = "Calibri";
                lastRow.Cells[3].Text = FormatarValor(dFevereiro);
                lastRow.Cells[3].Font.Name = "Calibri";
                lastRow.Cells[4].Text = FormatarValor(dMarco);
                lastRow.Cells[4].Font.Name = "Calibri";
                lastRow.Cells[5].Text = FormatarValor(dAbril);
                lastRow.Cells[5].Font.Name = "Calibri";
                lastRow.Cells[6].Text = FormatarValor(dMaio);
                lastRow.Cells[6].Font.Name = "Calibri";
                lastRow.Cells[7].Text = FormatarValor(dJunho);
                lastRow.Cells[7].Font.Name = "Calibri";
                lastRow.Cells[8].Text = FormatarValor(dJulho);
                lastRow.Cells[8].Font.Name = "Calibri";
                lastRow.Cells[9].Text = FormatarValor(dAgosto);
                lastRow.Cells[9].Font.Name = "Calibri";
                lastRow.Cells[10].Text = FormatarValor(dSetembro);
                lastRow.Cells[10].Font.Name = "Calibri";
                lastRow.Cells[11].Text = FormatarValor(dOutubro);
                lastRow.Cells[11].Font.Name = "Calibri";
                lastRow.Cells[12].Text = FormatarValor(dNovembro);
                lastRow.Cells[12].Font.Name = "Calibri";
                lastRow.Cells[13].Text = FormatarValor(dDezembro);
                lastRow.Cells[13].Font.Name = "Calibri";
                lastRow.Cells[14].Text = FormatarValor(dTotal);
                lastRow.Cells[14].Font.Name = "Calibri";
            }
        }

        protected void gvUtilidadeItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult utilidadeLinha = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (utilidadeLinha != null && utilidadeLinha.Tipo != "")
                    {
                        Literal _litFilial = e.Row.FindControl("litFilial") as Literal;
                        if (_litFilial != null)
                            _litFilial.Text = "<font size='2' face='Calibri'>&nbsp;" + utilidadeLinha.Filial.Trim().ToUpper() + "</font> ";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(utilidadeLinha.JANEIRO);
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(utilidadeLinha.FEVEREIRO);
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(utilidadeLinha.MARCO);
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(utilidadeLinha.ABRIL);
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(utilidadeLinha.MAIO);
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(utilidadeLinha.JUNHO);
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(utilidadeLinha.JULHO);
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(utilidadeLinha.AGOSTO);
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(utilidadeLinha.SETEMBRO);
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(utilidadeLinha.OUTUBRO);
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(utilidadeLinha.NOVEMBRO);
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(utilidadeLinha.DEZEMBRO);

                        totalLinha = Convert.ToDecimal(utilidadeLinha.JANEIRO + utilidadeLinha.FEVEREIRO + utilidadeLinha.MARCO + utilidadeLinha.ABRIL + utilidadeLinha.MAIO + utilidadeLinha.JUNHO + utilidadeLinha.JULHO + utilidadeLinha.AGOSTO + utilidadeLinha.SETEMBRO + utilidadeLinha.OUTUBRO + utilidadeLinha.NOVEMBRO + utilidadeLinha.DEZEMBRO);
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        /*
                        if (utilidadeLinha.Linha.Trim().ToUpper() == "VAREJO")
                        {
                            GridView gvCMVItemSub = e.Row.FindControl("gvCMVItemSub") as GridView;
                            if (gvCMVItemSub != null)
                            {
                                gvCMVItemSub.DataSource = cmvvarejo;
                                gvCMVItemSub.DataBind();
                            }
                            img.Visible = true;
                        }*/
                    }
                }
            }
        }
        protected void gvUtilidadeItem_DataBound(object sender, EventArgs e)
        {
            GridView gvUtilidadeItem = (GridView)sender;
            if (gvUtilidadeItem != null)
                if (gvUtilidadeItem.HeaderRow != null)
                    gvUtilidadeItem.HeaderRow.Visible = false;
        }

        /*protected void gvUtilidadeItemSub_RowDataBound(object sender, GridViewRowEventArgs e)
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
        }*/
        /*protected void gvUtilidadeItemSub_DataBound(object sender, EventArgs e)
        {
            GridView gvUtilidadeItemSub = (GridView)sender;
            if (gvUtilidadeItemSub != null)
                if (gvUtilidadeItemSub.HeaderRow != null)
                    gvUtilidadeItemSub.HeaderRow.Visible = false;

            if (gvUtilidadeItemSub.Rows.Count > 0)
            {
                int count = gvUtilidadeItemSub.Columns.Count;
                foreach (GridViewRow row in gvUtilidadeItemSub.Rows)
                    row.Attributes["style"] = "border: 1px solid #FFF";
            }
        }*/
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
