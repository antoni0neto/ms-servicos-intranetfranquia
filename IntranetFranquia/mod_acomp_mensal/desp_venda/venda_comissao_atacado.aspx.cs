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
using iTextSharp.text;

namespace Relatorios
{
    public partial class venda_comissao_atacado : System.Web.UI.Page
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

        List<SP_DRE_OBTER_DADOSResult> vendaComissao;

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
            filial = baseController.BuscaTodasFiliais().Where(p => p.TIPO_FILIAL.Trim().ToUpper() == "ATACADO").ToList();
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
        private void CarregarComissaoRepresentante(DateTime dataIni, DateTime dataFim, string filial)
        {
            vendaComissao = dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 8, false); ;

            var vendaComissaoAgrupado = vendaComissao.ToList();
            vendaComissaoAgrupado = vendaComissaoAgrupado.GroupBy(b => new { FILIAL = b.Filial }).Select(
                                                                            k => new SP_DRE_OBTER_DADOSResult
                                                                            {
                                                                                Id = 1,
                                                                                Filial = k.Key.FILIAL.Trim().ToUpper(),
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

            vendaComissaoAgrupado.Insert(vendaComissaoAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Filial = "" });
            vendaComissaoAgrupado.Insert(vendaComissaoAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Filial = "Comissão Representantes" });
            vendaComissaoAgrupado.Insert(vendaComissaoAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Filial = "" });
            if (vendaComissaoAgrupado != null)
            {
                gvComissao.DataSource = vendaComissaoAgrupado;
                gvComissao.DataBind();
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
                CarregarComissaoRepresentante(v_dataini, v_datafim, filial);
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }

        }

        protected void gvComissao_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult comissao = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (comissao != null && comissao.Filial != "")
                    {
                        Literal _litFilial = e.Row.FindControl("litFilial") as Literal;
                        if (_litFilial != null)
                            _litFilial.Text = "<font size='2' face='Calibri'>&nbsp;" + comissao.Filial.Trim().ToUpper() + "</font> ";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(comissao.JANEIRO);
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(comissao.FEVEREIRO);
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(comissao.MARCO);
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(comissao.ABRIL);
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(comissao.MAIO);
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(comissao.JUNHO);
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(comissao.JULHO);
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(comissao.AGOSTO);
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(comissao.SETEMBRO);
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(comissao.OUTUBRO);
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(comissao.NOVEMBRO);
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(comissao.DEZEMBRO);

                        totalLinha = Convert.ToDecimal(comissao.JANEIRO + comissao.FEVEREIRO + comissao.MARCO + comissao.ABRIL + comissao.MAIO + comissao.JUNHO + comissao.JULHO + comissao.AGOSTO + comissao.SETEMBRO + comissao.OUTUBRO + comissao.NOVEMBRO + comissao.DEZEMBRO);
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                        //SOMATORIO
                        dJaneiro += Convert.ToDecimal(comissao.JANEIRO);
                        dFevereiro += Convert.ToDecimal(comissao.FEVEREIRO);
                        dMarco += Convert.ToDecimal(comissao.MARCO);
                        dAbril += Convert.ToDecimal(comissao.ABRIL);
                        dMaio += Convert.ToDecimal(comissao.MAIO);
                        dJunho += Convert.ToDecimal(comissao.JUNHO);
                        dJulho += Convert.ToDecimal(comissao.JULHO);
                        dAgosto += Convert.ToDecimal(comissao.AGOSTO);
                        dSetembro += Convert.ToDecimal(comissao.SETEMBRO);
                        dOutubro += Convert.ToDecimal(comissao.OUTUBRO);
                        dNovembro += Convert.ToDecimal(comissao.NOVEMBRO);
                        dDezembro += Convert.ToDecimal(comissao.DEZEMBRO);
                        dTotal += totalLinha;

                        e.Row.Cells[gvComissao.Columns.Count - 1].Attributes["style"] = "border-right: 1px solid #FFF";
                    }

                    if (comissao.Filial == "")
                        e.Row.Height = Unit.Pixel(5);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (comissao.Id == 1)
                        {
                            GridView gvComissaoItem = e.Row.FindControl("gvComissaoItem") as GridView;
                            if (gvComissaoItem != null)
                            {
                                gvComissaoItem.DataSource = vendaComissao.Where(p => p.Filial.Trim().ToUpper() == comissao.Filial.Trim().ToUpper());
                                gvComissaoItem.DataBind();
                            }
                            img.Visible = true;
                        }
                    }
                }
            }
        }
        protected void gvComissao_DataBound(object sender, EventArgs e)
        {
            //Tratamento de cores para linhas
            GridViewRow firstBlankRow = gvComissao.Rows[gvComissao.Rows.Count - 3];
            if (firstBlankRow != null)
            {
                firstBlankRow.BackColor = System.Drawing.Color.Gainsboro;
                firstBlankRow.Cells[0].Attributes["style"] = "border-bottom: 1px solid #F5F5F5";
            }

            GridViewRow lastBlankRow = gvComissao.Rows[gvComissao.Rows.Count - 1];
            if (lastBlankRow != null)
            {
                //Tratamento para cor
                lastBlankRow.BackColor = System.Drawing.Color.Gainsboro;
                int count = gvComissao.Columns.Count;
                for (int i = 0; i < count; i++)
                    lastBlankRow.Cells[i].BorderWidth = Unit.Pixel(1);
            }

            GridViewRow lastRow = gvComissao.Rows[gvComissao.Rows.Count - 2];
            if (lastRow != null)
            {
                //lastRow.Visible = false;

                lastRow.BackColor = System.Drawing.Color.GhostWhite;
                lastRow.Cells[1].Text = "&nbsp;COMISSÃO";
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

        protected void gvComissaoItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult comissaoLinha = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (comissaoLinha != null && comissaoLinha.Tipo != "")
                    {
                        Literal _litRepresentante = e.Row.FindControl("litRepresentante") as Literal;
                        if (_litRepresentante != null)
                            _litRepresentante.Text = "<font size='2' face='Calibri'>&nbsp;" + comissaoLinha.Tipo.Trim().ToUpper() + "</font> ";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(comissaoLinha.JANEIRO);
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(comissaoLinha.FEVEREIRO);
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(comissaoLinha.MARCO);
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(comissaoLinha.ABRIL);
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(comissaoLinha.MAIO);
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(comissaoLinha.JUNHO);
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(comissaoLinha.JULHO);
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(comissaoLinha.AGOSTO);
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(comissaoLinha.SETEMBRO);
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(comissaoLinha.OUTUBRO);
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(comissaoLinha.NOVEMBRO);
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(comissaoLinha.DEZEMBRO);

                        totalLinha = Convert.ToDecimal(comissaoLinha.JANEIRO + comissaoLinha.FEVEREIRO + comissaoLinha.MARCO + comissaoLinha.ABRIL + comissaoLinha.MAIO + comissaoLinha.JUNHO + comissaoLinha.JULHO + comissaoLinha.AGOSTO + comissaoLinha.SETEMBRO + comissaoLinha.OUTUBRO + comissaoLinha.NOVEMBRO + comissaoLinha.DEZEMBRO);
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
        protected void gvComissaoItem_DataBound(object sender, EventArgs e)
        {
            GridView gvComissaoItem = (GridView)sender;
            if (gvComissaoItem != null)
                if (gvComissaoItem.HeaderRow != null)
                    gvComissaoItem.HeaderRow.Visible = false;
        }

        /*protected void gvComissaoItemItemSub_RowDataBound(object sender, GridViewRowEventArgs e)
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
