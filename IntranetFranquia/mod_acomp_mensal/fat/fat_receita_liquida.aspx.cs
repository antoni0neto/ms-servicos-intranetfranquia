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
    public partial class fat_receita_liquida : System.Web.UI.Page
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

        List<SP_DRE_OBTER_DADOSResult> atacado;
        List<SP_DRE_OBTER_DADOSResult> varejo;
        List<SP_DRE_OBTER_DADOSResult> taxaCartao;
        List<SP_DRE_OBTER_DADOSResult> imposto;

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
        private void CarregarReceitaLiquida(DateTime dataIni, DateTime dataFim, string filial)
        {
            atacado = dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 1, false);
            varejo = dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 2, false);
            taxaCartao = dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 3, false);
            imposto = dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 4, false);

            var receitaBruta = varejo.Union(atacado);
            receitaBruta = receitaBruta.GroupBy(b => new { GRUPO = b.Grupo }).Select(
                                                                            k => new SP_DRE_OBTER_DADOSResult
                                                                            {
                                                                                Id = 1,
                                                                                Grupo = "RECEITAS BRUTAS",
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

            var deducoesReceitaBruta = taxaCartao.Union(imposto);
            deducoesReceitaBruta = deducoesReceitaBruta.GroupBy(b => new { GRUPO = b.Grupo }).Select(
                                                                k => new SP_DRE_OBTER_DADOSResult
                                                                {
                                                                    Id = 2,
                                                                    Grupo = "DEDUÇÕES RECEITAS BRUTAS",
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

            var receitaLiquida = receitaBruta.Union(deducoesReceitaBruta).ToList();
            receitaLiquida.Insert(receitaLiquida.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "" });
            receitaLiquida.Insert(receitaLiquida.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "RECEITAS LÍQUIDAS" });
            receitaLiquida.Insert(receitaLiquida.Count, new SP_DRE_OBTER_DADOSResult { Grupo = "" });
            if (receitaLiquida != null)
            {
                gvReceitaLiquida.DataSource = receitaLiquida;
                gvReceitaLiquida.DataBind();
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
                CarregarReceitaLiquida(v_dataini, v_datafim, filial);
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }

        }

        protected void gvReceitaLiquida_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult receitaLiquida = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (receitaLiquida != null && receitaLiquida.Grupo != "")
                    {
                        Literal _receitaBruta = e.Row.FindControl("litReceitaBruta") as Literal;
                        if (_receitaBruta != null)
                            _receitaBruta.Text = "<font size='2' face='Calibri'>&nbsp;" + receitaLiquida.Grupo.Trim() + "</font> ";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(receitaLiquida.JANEIRO);
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(receitaLiquida.FEVEREIRO);
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(receitaLiquida.MARCO);
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(receitaLiquida.ABRIL);
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(receitaLiquida.MAIO);
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(receitaLiquida.JUNHO);
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(receitaLiquida.JULHO);
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(receitaLiquida.AGOSTO);
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(receitaLiquida.SETEMBRO);
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(receitaLiquida.OUTUBRO);
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(receitaLiquida.NOVEMBRO);
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(receitaLiquida.DEZEMBRO);

                        totalLinha = Convert.ToDecimal(receitaLiquida.JANEIRO + receitaLiquida.FEVEREIRO + receitaLiquida.MARCO + receitaLiquida.ABRIL + receitaLiquida.MAIO + receitaLiquida.JUNHO + receitaLiquida.JULHO + receitaLiquida.AGOSTO + receitaLiquida.SETEMBRO + receitaLiquida.OUTUBRO + receitaLiquida.NOVEMBRO + receitaLiquida.DEZEMBRO);
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                        //SOMATORIO
                        dJaneiro += Convert.ToDecimal(receitaLiquida.JANEIRO);
                        dFevereiro += Convert.ToDecimal(receitaLiquida.FEVEREIRO);
                        dMarco += Convert.ToDecimal(receitaLiquida.MARCO);
                        dAbril += Convert.ToDecimal(receitaLiquida.ABRIL);
                        dMaio += Convert.ToDecimal(receitaLiquida.MAIO);
                        dJunho += Convert.ToDecimal(receitaLiquida.JUNHO);
                        dJulho += Convert.ToDecimal(receitaLiquida.JULHO);
                        dAgosto += Convert.ToDecimal(receitaLiquida.AGOSTO);
                        dSetembro += Convert.ToDecimal(receitaLiquida.SETEMBRO);
                        dOutubro += Convert.ToDecimal(receitaLiquida.OUTUBRO);
                        dNovembro += Convert.ToDecimal(receitaLiquida.NOVEMBRO);
                        dDezembro += Convert.ToDecimal(receitaLiquida.DEZEMBRO);
                        dTotal += totalLinha;

                        e.Row.Cells[gvReceitaLiquida.Columns.Count - 1].Attributes["style"] = "border-right: 1px solid #FFF";
                    }

                    if (receitaLiquida.Grupo == "")
                        e.Row.Height = Unit.Pixel(5);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (receitaLiquida.Id == 1)
                        {
                            GridView gvFaturamento = e.Row.FindControl("gvFaturamento") as GridView;
                            if (gvFaturamento != null)
                            {
                                var faturamentoBruto = atacado.Union(varejo).ToList();
                                faturamentoBruto = faturamentoBruto.GroupBy(b => new { LINHA = b.Linha }).Select(
                                                                        k => new SP_DRE_OBTER_DADOSResult
                                                                        {
                                                                            Id = 1,
                                                                            Linha = k.Key.LINHA.Replace("Faturamento", "").Trim().ToUpper(),
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
                                gvFaturamento.DataSource = faturamentoBruto;
                                gvFaturamento.DataBind();
                            }
                            img.Visible = true;
                        }
                        if (receitaLiquida.Id == 2)
                        {
                            GridView gvFaturamento = e.Row.FindControl("gvFaturamento") as GridView;
                            if (gvFaturamento != null)
                            {
                                var deducoesBruto = imposto.Union(taxaCartao).ToList();
                                deducoesBruto = deducoesBruto.GroupBy(b => new { LINHA = b.Linha }).Select(
                                                                        k => new SP_DRE_OBTER_DADOSResult
                                                                        {
                                                                            Id = 2,
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
                                gvFaturamento.DataSource = deducoesBruto;
                                gvFaturamento.DataBind();
                            }
                            img.Visible = true;
                        }
                    }
                }
            }
        }
        protected void gvReceitaLiquida_DataBound(object sender, EventArgs e)
        {
            //Tratamento de cores para linhas
            GridViewRow firstBlankRow = gvReceitaLiquida.Rows[gvReceitaLiquida.Rows.Count - 3];
            if (firstBlankRow != null)
            {
                firstBlankRow.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
                firstBlankRow.Cells[0].Attributes["style"] = "border-bottom: 1px solid #F5F5F5";
            }

            GridViewRow lastBlankRow = gvReceitaLiquida.Rows[gvReceitaLiquida.Rows.Count - 1];
            if (lastBlankRow != null)
            {
                //Tratamento para cor
                lastBlankRow.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
                int count = gvReceitaLiquida.Columns.Count;
                for (int i = 0; i < count; i++)
                    lastBlankRow.Cells[i].BorderWidth = Unit.Pixel(1);
            }

            GridViewRow lastRow = gvReceitaLiquida.Rows[gvReceitaLiquida.Rows.Count - 2];
            if (lastRow != null)
            {
                lastRow.BackColor = System.Drawing.Color.GhostWhite;
                lastRow.Cells[1].Text = "&nbsp;RECEITAS LÍQUIDAS";
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

        protected void gvFaturamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;
            string url_link = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult faturamento = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (faturamento != null && faturamento.Grupo != "")
                    {
                        Literal _litFaturamento = e.Row.FindControl("litFaturamento") as Literal;
                        if (_litFaturamento != null)
                        {
                            if (faturamento.Linha.Contains("ATACADO") || faturamento.Linha.Contains("VAREJO"))
                            {
                                if (faturamento.Linha.ToUpper().Contains("VAREJO"))
                                    url_link = "fat_varejo.aspx";
                                else
                                    url_link = "fat_atacado.aspx";
                                _litFaturamento.Text = "<font size='2' face='Calibri'><a href='" + url_link + "' class='adre' title='" + faturamento.Linha.Trim() + "'>&nbsp;" + faturamento.Linha.Trim().ToUpper() + "</a></font> ";
                            }
                            else
                            {
                                _litFaturamento.Text = "<font size='2' face='Calibri'>&nbsp;(-) " + faturamento.Linha.Trim().ToUpper() + "</font> ";
                            }
                        }
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(faturamento.JANEIRO);
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(faturamento.FEVEREIRO);
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(faturamento.MARCO);
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(faturamento.ABRIL);
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(faturamento.MAIO);
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(faturamento.JUNHO);
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(faturamento.JULHO);
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(faturamento.AGOSTO);
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(faturamento.SETEMBRO);
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(faturamento.OUTUBRO);
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(faturamento.NOVEMBRO);
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(faturamento.DEZEMBRO);

                        totalLinha = Convert.ToDecimal(faturamento.JANEIRO + faturamento.FEVEREIRO + faturamento.MARCO + faturamento.ABRIL + faturamento.MAIO + faturamento.JUNHO + faturamento.JULHO + faturamento.AGOSTO + faturamento.SETEMBRO + faturamento.OUTUBRO + faturamento.NOVEMBRO + faturamento.DEZEMBRO);
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (faturamento.Linha.Trim().ToUpper() == "IMPOSTOS S/VENDA")
                        {
                            GridView gvSubGrid = e.Row.FindControl("gvSubGrid") as GridView;
                            if (gvSubGrid != null)
                            {
                                imposto = imposto.GroupBy(b => new { TIPO = b.Tipo }).Select(
                                                                        k => new SP_DRE_OBTER_DADOSResult
                                                                        {
                                                                            Linha = k.Key.TIPO.Trim().ToUpper(),
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

                                gvSubGrid.DataSource = imposto;
                                gvSubGrid.DataBind();
                            }
                            img.Visible = true;
                        }

                        if (faturamento.Linha.Trim().ToUpper() == "VAREJO")
                        {
                            GridView gvSubGrid = e.Row.FindControl("gvSubGrid") as GridView;
                            if (gvSubGrid != null)
                            {
                                gvSubGrid.DataSource = varejo;
                                gvSubGrid.DataBind();
                            }
                            img.Visible = true;
                        }
                    }
                }
            }
        }
        protected void gvFaturamento_DataBound(object sender, EventArgs e)
        {
            GridView gvFaturamento = (GridView)sender;
            if (gvFaturamento != null)
                if (gvFaturamento.HeaderRow != null)
                    gvFaturamento.HeaderRow.Visible = false;
        }

        protected void gvSubGrid_RowDataBound(object sender, GridViewRowEventArgs e)
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
                            if (subitem.Linha.Contains("Varejo"))
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
        protected void gvSubGrid_DataBound(object sender, EventArgs e)
        {
            GridView gvSubItem = (GridView)sender;
            if (gvSubItem != null)
                if (gvSubItem.HeaderRow != null)
                    gvSubItem.HeaderRow.Visible = false;

            if (gvSubItem.Rows.Count > 0)
            {
                int count = gvSubItem.Columns.Count;
                foreach (GridViewRow row in gvSubItem.Rows)
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
