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
    public partial class venda_salario : System.Web.UI.Page
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

        List<SP_DRE_OBTER_DADOSResult> lstSalario;

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
            List<FILIAI> filial = new List<FILIAI>();
            filial = baseController.BuscaFiliais();

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
        private void CarregarSalario(DateTime dataIni, DateTime dataFim, string filial)
        {
            lstSalario = dreController.ObterDRE(dataIni, dataFim, filial, intercompany, 7, false);

            var salarioAgrupado = lstSalario.ToList();
            salarioAgrupado = salarioAgrupado.GroupBy(b => new { LINHA = b.Linha }).Select(
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

            salarioAgrupado.Insert(salarioAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Linha = "" });
            salarioAgrupado.Insert(salarioAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Linha = "SAL" });
            salarioAgrupado.Insert(salarioAgrupado.Count, new SP_DRE_OBTER_DADOSResult { Linha = "" });
            if (salarioAgrupado != null)
            {
                gvSalario.DataSource = salarioAgrupado;
                gvSalario.DataBind();
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
                CarregarSalario(v_dataini, v_datafim, filial);
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message;
            }

        }

        protected void gvSalario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult salario = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (salario != null && salario.Linha != "")
                    {
                        Literal _litSalario = e.Row.FindControl("litSalario") as Literal;
                        if (_litSalario != null)
                            _litSalario.Text = "<font size='2' face='Calibri'>&nbsp;" + salario.Linha.Trim().ToUpper() + "</font> ";
                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(salario.JANEIRO);
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(salario.FEVEREIRO);
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(salario.MARCO);
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(salario.ABRIL);
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(salario.MAIO);
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(salario.JUNHO);
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(salario.JULHO);
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(salario.AGOSTO);
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(salario.SETEMBRO);
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(salario.OUTUBRO);
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(salario.NOVEMBRO);
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(salario.DEZEMBRO);

                        totalLinha = Convert.ToDecimal(salario.JANEIRO + salario.FEVEREIRO + salario.MARCO + salario.ABRIL + salario.MAIO + salario.JUNHO + salario.JULHO + salario.AGOSTO + salario.SETEMBRO + salario.OUTUBRO + salario.NOVEMBRO + salario.DEZEMBRO);
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                        //SOMATORIO
                        dJaneiro += Convert.ToDecimal(salario.JANEIRO);
                        dFevereiro += Convert.ToDecimal(salario.FEVEREIRO);
                        dMarco += Convert.ToDecimal(salario.MARCO);
                        dAbril += Convert.ToDecimal(salario.ABRIL);
                        dMaio += Convert.ToDecimal(salario.MAIO);
                        dJunho += Convert.ToDecimal(salario.JUNHO);
                        dJulho += Convert.ToDecimal(salario.JULHO);
                        dAgosto += Convert.ToDecimal(salario.AGOSTO);
                        dSetembro += Convert.ToDecimal(salario.SETEMBRO);
                        dOutubro += Convert.ToDecimal(salario.OUTUBRO);
                        dNovembro += Convert.ToDecimal(salario.NOVEMBRO);
                        dDezembro += Convert.ToDecimal(salario.DEZEMBRO);
                        dTotal += totalLinha;

                        e.Row.Cells[gvSalario.Columns.Count - 1].Attributes["style"] = "border-right: 1px solid #FFF";
                    }

                    if (salario.Linha == "")
                        e.Row.Height = Unit.Pixel(5);

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (salario.Linha.ToUpper().Trim() == "SALÁRIOS")
                        {
                            GridView gvSalarioItem = e.Row.FindControl("gvSalarioItem") as GridView;
                            if (gvSalarioItem != null)
                            {
                                gvSalarioItem.DataSource = lstSalario.Where(p => p.Linha.Trim().ToUpper() == salario.Linha.Trim().ToUpper()).GroupBy(b => new { LINHA = b.Linha, TIPO = b.Tipo }).Select(
                                                                            k => new SP_DRE_OBTER_DADOSResult
                                                                            {
                                                                                Id = 1,
                                                                                Linha = k.Key.LINHA.Trim().ToUpper(),
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
                                gvSalarioItem.DataBind();
                            }
                            img.Visible = true;
                        }

                        if (salario.Linha.ToUpper().Trim() == "ENCARGOS")
                        {
                            GridView gvSalarioItem = e.Row.FindControl("gvSalarioItem") as GridView;
                            if (gvSalarioItem != null)
                            {
                                gvSalarioItem.DataSource = lstSalario.Where(p => p.Linha.Trim().ToUpper() == salario.Linha.Trim().ToUpper()).GroupBy(b => new { TIPO = b.Tipo }).Select(
                                                                            k => new SP_DRE_OBTER_DADOSResult
                                                                            {
                                                                                Id = 2,
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
                                gvSalarioItem.DataBind();
                            }
                            img.Visible = true;
                        }
                    }
                }
            }
        }
        protected void gvSalario_DataBound(object sender, EventArgs e)
        {
            //Tratamento de cores para linhas
            GridViewRow firstBlankRow = gvSalario.Rows[gvSalario.Rows.Count - 3];
            if (firstBlankRow != null)
            {
                firstBlankRow.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
                firstBlankRow.Cells[0].Attributes["style"] = "border-bottom: 1px solid #F5F5F5";
            }

            GridViewRow lastBlankRow = gvSalario.Rows[gvSalario.Rows.Count - 1];
            if (lastBlankRow != null)
            {
                //Tratamento para cor
                lastBlankRow.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
                int count = gvSalario.Columns.Count;
                for (int i = 0; i < count; i++)
                    lastBlankRow.Cells[i].BorderWidth = Unit.Pixel(1);
            }

            GridViewRow lastRow = gvSalario.Rows[gvSalario.Rows.Count - 2];
            if (lastRow != null)
            {
                lastRow.BackColor = System.Drawing.Color.GhostWhite;
                lastRow.Cells[1].Text = "&nbsp;TOTAL";
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

        protected void gvSalarioItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult salarioItem = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (salarioItem != null && salarioItem.Tipo != "")
                    {
                        Literal _litSalarioItem = e.Row.FindControl("litSalarioItem") as Literal;
                        if (_litSalarioItem != null)
                            _litSalarioItem.Text = "<font size='2' face='Calibri'>&nbsp;(-) " + salarioItem.Tipo.Trim().ToUpper() + "</font> ";

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(salarioItem.JANEIRO);
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(salarioItem.FEVEREIRO);
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(salarioItem.MARCO);
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(salarioItem.ABRIL);
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(salarioItem.MAIO);
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(salarioItem.JUNHO);
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(salarioItem.JULHO);
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(salarioItem.AGOSTO);
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(salarioItem.SETEMBRO);
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(salarioItem.OUTUBRO);
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(salarioItem.NOVEMBRO);
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(salarioItem.DEZEMBRO);

                        totalLinha = Convert.ToDecimal(salarioItem.JANEIRO + salarioItem.FEVEREIRO + salarioItem.MARCO + salarioItem.ABRIL + salarioItem.MAIO + salarioItem.JUNHO + salarioItem.JULHO + salarioItem.AGOSTO + salarioItem.SETEMBRO + salarioItem.OUTUBRO + salarioItem.NOVEMBRO + salarioItem.DEZEMBRO);
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                    }


                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        img.Visible = false;
                        if (salarioItem.Id == 1)
                        {
                            GridView gvSubGrid = e.Row.FindControl("gvSalarioSubItem") as GridView;
                            if (gvSubGrid != null)
                            {
                                var salarioFil = lstSalario.Where(p => p.Linha.Trim().ToUpper() == "SALÁRIOS" && p.Tipo.Trim().ToUpper() == salarioItem.Tipo.Trim().ToUpper()).GroupBy(b => new { FILIAL = b.Filial }).Select(
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

                                gvSubGrid.DataSource = salarioFil;
                                gvSubGrid.DataBind();
                            }
                            img.Visible = true;
                        }
                    }
                }
            }
        }
        protected void gvSalarioItem_DataBound(object sender, EventArgs e)
        {
            GridView gvSalarioItem = (GridView)sender;
            if (gvSalarioItem != null)
                if (gvSalarioItem.HeaderRow != null)
                    gvSalarioItem.HeaderRow.Visible = false;
        }

        protected void gvSalarioSubItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_DRE_OBTER_DADOSResult subitem = e.Row.DataItem as SP_DRE_OBTER_DADOSResult;

                    if (subitem != null && subitem.Filial != "")
                    {
                        Literal _litSubItem = e.Row.FindControl("litSubItem") as Literal;
                        if (_litSubItem != null)
                            _litSubItem.Text = "<font size='1' face='Calibri'>&nbsp;" + subitem.Filial.Trim().ToUpper() + "</font> ";

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
        protected void gvSalarioSubItem_DataBound(object sender, EventArgs e)
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
