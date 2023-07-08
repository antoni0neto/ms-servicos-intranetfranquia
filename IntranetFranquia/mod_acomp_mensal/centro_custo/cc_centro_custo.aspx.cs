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
    public partial class cc_centro_custo : System.Web.UI.Page
    {
        DREController dreController = new DREController();
        BaseController baseController = new BaseController();

        Color corTitulo = System.Drawing.SystemColors.GradientActiveCaption;
        Color corFundo = Color.WhiteSmoke;
        string tagCorNegativo = "#CD2626";

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

        List<SP_OBTER_CENTRO_CUSTO_LANCAMENTOResult> gCentroCusto = new List<SP_OBTER_CENTRO_CUSTO_LANCAMENTOResult>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                CarregarDataAno();
                CarregarCentroCustoGrupo();
                CarregarCentroCusto(0);
                CarregarFilial();

                //Carregar com a data atual
                //btBuscar_Click(null, null);
            }
        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            DateTime v_dataini;
            DateTime v_datafim;
            string ano = "";
            int? centroCustoGrupo = null;
            string centroCusto = "";
            string filial = "";

            ano = ddlAno.SelectedValue;
            v_dataini = Convert.ToDateTime(ano + "-01-01");
            v_datafim = Convert.ToDateTime(ano + "-12-31");

            //Obter Grupo Centro de Custo
            if (ddlCCustoGrupo.SelectedValue.Trim() != "" && ddlCCustoGrupo.SelectedValue.Trim() != "0")
                centroCustoGrupo = Convert.ToInt32(ddlCCustoGrupo.SelectedValue);

            //Obter Centro de Custo
            if (ddlCCusto.SelectedValue.Trim() != "" && ddlCCusto.SelectedValue.Trim() != "0")
                centroCusto = ddlCCusto.SelectedValue;

            //Obter Filial
            if (ddlFilial.SelectedValue.Trim() != "" && ddlFilial.SelectedValue.Trim() != "0")
                filial = ddlFilial.SelectedValue;

            try
            {

                labErro.Text = "";
                CarregarCentroCusto(v_dataini, v_datafim, centroCustoGrupo, centroCusto, filial);
                ZerarValores();
            }
            catch (Exception ex)
            {
                labErro.Text = "ERRO: " + ex.Message + "\n" + ex.StackTrace;
            }

        }

        #region "DADOS INICIAIS"
        private void CarregarFilial()
        {
            List<FILIAI> filial = new List<FILIAI>();
            List<FILIAIS_DE_PARA> filialDePara = new List<FILIAIS_DE_PARA>();

            filial = baseController.BuscaFiliais();
            filialDePara = baseController.BuscaFilialDePara();

            filial = filial.Where(i => !filialDePara.Any(g => g.DE == i.COD_FILIAL)).ToList();
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
        private void CarregarCentroCustoGrupo()
        {
            var centroCustoGrupo = dreController.ObterCentroCustoGrupo();
            if (centroCustoGrupo != null)
            {
                centroCustoGrupo.Insert(0, new CTB_CENTRO_CUSTO_GRUPO { ID_GRUPO_CENTRO_CUSTO = 0, DESC_GRUPO_CENTRO_CUSTO = "" });

                ddlCCustoGrupo.DataSource = centroCustoGrupo;
                ddlCCustoGrupo.DataBind();
            }
        }
        protected void ddlCCustoGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCCustoGrupo.SelectedValue != "" && ddlCCustoGrupo.SelectedValue != "0")
                CarregarCentroCusto(Convert.ToInt32(ddlCCustoGrupo.SelectedValue));
            else
                CarregarCentroCusto(0);
        }
        private void CarregarCentroCusto(int idGrupo)
        {
            var centroCusto = dreController.ObterCentroCusto().Where(p => !p.INATIVA).ToList();
            if (centroCusto != null)
            {
                if (idGrupo > 0)
                    centroCusto = centroCusto.Where(p => p.ID_GRUPO_CENTRO_CUSTO == idGrupo).OrderBy(x => x.DESC_CENTRO_CUSTO).ToList();

                centroCusto.Insert(0, new CTB_CENTRO_CUSTO { CENTRO_CUSTO = "0", DESC_CENTRO_CUSTO = "" });

                ddlCCusto.DataSource = centroCusto;
                ddlCCusto.DataBind();
            }
        }
        private string FormatarValor(decimal? valor)
        {
            string tagCor = "#000";
            string retorno = "";
            if (valor < 0)
                tagCor = tagCorNegativo;

            retorno = "<font size='2' face='Calibri' color='" + tagCor + "'>" + Convert.ToDecimal(valor).ToString("###,###,###,##0.00;(###,###,###,##0.00)") + "</font> ";
            return retorno;
        }
        private void ZerarValores()
        {
            dJaneiro = 0;
            dFevereiro = 0;
            dMarco = 0;
            dAbril = 0;
            dMaio = 0;
            dJunho = 0;
            dJulho = 0;
            dAgosto = 0;
            dSetembro = 0;
            dOutubro = 0;
            dNovembro = 0;
            dDezembro = 0;
            dTotal = 0;
        }
        #endregion

        #region "ACOES"

        private void CarregarCentroCusto(DateTime dataIni, DateTime dataFim, int? centroCustoGrupo, string centroCusto, string filial)
        {
            gCentroCusto = dreController.ObterCentroCustoLancamento(dataIni, dataFim, centroCustoGrupo, centroCusto, filial);

            var grupo = gCentroCusto.GroupBy(b => new { ID_CC_GRUPO = b.ID_CC_GRUPO, CC_GRUPO = b.CC_GRUPO }).Select(
                                                                            k => new SP_OBTER_CENTRO_CUSTO_LANCAMENTOResult
                                                                            {
                                                                                ID_CC_GRUPO = k.Key.ID_CC_GRUPO,
                                                                                CC_GRUPO = k.Key.CC_GRUPO,
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
            gvCentroCustoGrupo.DataSource = grupo;
            gvCentroCustoGrupo.DataBind();
        }

        #endregion

        #region "CENTRO DE CUSTO"

        protected void gvCentroCustoGrupo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CENTRO_CUSTO_LANCAMENTOResult centroCustoGrupo = e.Row.DataItem as SP_OBTER_CENTRO_CUSTO_LANCAMENTOResult;

                    if (centroCustoGrupo != null)
                    {
                        Literal _litCentroCustoGrupo = e.Row.FindControl("litCentroCustoGrupo") as Literal;
                        if (_litCentroCustoGrupo != null)
                            _litCentroCustoGrupo.Text = "<font size='2' face='Calibri'>&nbsp;" + centroCustoGrupo.CC_GRUPO.Trim() + "</font> ";

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(centroCustoGrupo.JANEIRO);
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(centroCustoGrupo.FEVEREIRO);
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(centroCustoGrupo.MARCO);
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(centroCustoGrupo.ABRIL);
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(centroCustoGrupo.MAIO);
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(centroCustoGrupo.JUNHO);
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(centroCustoGrupo.JULHO);
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(centroCustoGrupo.AGOSTO);
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(centroCustoGrupo.SETEMBRO);
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(centroCustoGrupo.OUTUBRO);
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(centroCustoGrupo.NOVEMBRO);
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(centroCustoGrupo.DEZEMBRO);

                        totalLinha = Convert.ToDecimal(centroCustoGrupo.JANEIRO + centroCustoGrupo.FEVEREIRO + centroCustoGrupo.MARCO + centroCustoGrupo.ABRIL + centroCustoGrupo.MAIO + centroCustoGrupo.JUNHO + centroCustoGrupo.JULHO + centroCustoGrupo.AGOSTO + centroCustoGrupo.SETEMBRO + centroCustoGrupo.OUTUBRO + centroCustoGrupo.NOVEMBRO + centroCustoGrupo.DEZEMBRO);
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                        //SOMATORIO
                        dJaneiro += Convert.ToDecimal(centroCustoGrupo.JANEIRO);
                        dFevereiro += Convert.ToDecimal(centroCustoGrupo.FEVEREIRO);
                        dMarco += Convert.ToDecimal(centroCustoGrupo.MARCO);
                        dAbril += Convert.ToDecimal(centroCustoGrupo.ABRIL);
                        dMaio += Convert.ToDecimal(centroCustoGrupo.MAIO);
                        dJunho += Convert.ToDecimal(centroCustoGrupo.JUNHO);
                        dJulho += Convert.ToDecimal(centroCustoGrupo.JULHO);
                        dAgosto += Convert.ToDecimal(centroCustoGrupo.AGOSTO);
                        dSetembro += Convert.ToDecimal(centroCustoGrupo.SETEMBRO);
                        dOutubro += Convert.ToDecimal(centroCustoGrupo.OUTUBRO);
                        dNovembro += Convert.ToDecimal(centroCustoGrupo.NOVEMBRO);
                        dDezembro += Convert.ToDecimal(centroCustoGrupo.DEZEMBRO);
                        dTotal += totalLinha;

                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        GridView gvCentroCusto = e.Row.FindControl("gvCentroCusto") as GridView;
                        if (gvCentroCusto != null)
                        {
                            var centroCusto = gCentroCusto.Where(p => p.ID_CC_GRUPO == centroCustoGrupo.ID_CC_GRUPO).GroupBy(b => new { ID_CCUSTO = b.ID_CCUSTO, CCUSTO = b.CCUSTO }).Select(
                                                                               k => new SP_OBTER_CENTRO_CUSTO_LANCAMENTOResult
                                                                               {
                                                                                   ID_CCUSTO = k.Key.ID_CCUSTO,
                                                                                   CCUSTO = k.Key.CCUSTO,
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

                            gvCentroCusto.DataSource = centroCusto;
                            gvCentroCusto.DataBind();
                        }
                    }
                }
            }
        }
        protected void gvCentroCustoGrupo_DataBound(object sender, EventArgs e)
        {

            GridViewRow footer = gvCentroCustoGrupo.FooterRow;
            if (footer != null)
            {
                footer.Cells[1].Text = "&nbsp;TOTAL";
                footer.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                footer.Cells[1].Font.Name = "Calibri";
                footer.Cells[2].Text = FormatarValor(dJaneiro);
                footer.Cells[2].Font.Name = "Calibri";
                footer.Cells[3].Text = FormatarValor(dFevereiro);
                footer.Cells[3].Font.Name = "Calibri";
                footer.Cells[4].Text = FormatarValor(dMarco);
                footer.Cells[4].Font.Name = "Calibri";
                footer.Cells[5].Text = FormatarValor(dAbril);
                footer.Cells[5].Font.Name = "Calibri";
                footer.Cells[6].Text = FormatarValor(dMaio);
                footer.Cells[6].Font.Name = "Calibri";
                footer.Cells[7].Text = FormatarValor(dJunho);
                footer.Cells[7].Font.Name = "Calibri";
                footer.Cells[8].Text = FormatarValor(dJulho);
                footer.Cells[8].Font.Name = "Calibri";
                footer.Cells[9].Text = FormatarValor(dAgosto);
                footer.Cells[9].Font.Name = "Calibri";
                footer.Cells[10].Text = FormatarValor(dSetembro);
                footer.Cells[10].Font.Name = "Calibri";
                footer.Cells[11].Text = FormatarValor(dOutubro);
                footer.Cells[11].Font.Name = "Calibri";
                footer.Cells[12].Text = FormatarValor(dNovembro);
                footer.Cells[12].Font.Name = "Calibri";
                footer.Cells[13].Text = FormatarValor(dDezembro);
                footer.Cells[13].Font.Name = "Calibri";
                footer.Cells[14].Text = FormatarValor(dTotal);
                footer.Cells[14].Font.Name = "Calibri";
            }
        }

        protected void gvCentroCusto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CENTRO_CUSTO_LANCAMENTOResult centroCusto = e.Row.DataItem as SP_OBTER_CENTRO_CUSTO_LANCAMENTOResult;

                    if (centroCusto != null)
                    {
                        Literal _litCentroCusto = e.Row.FindControl("litCentroCusto") as Literal;
                        if (_litCentroCusto != null)
                            _litCentroCusto.Text = "<font size='1' face='Calibri'>" + centroCusto.CCUSTO.Replace("DESENVOLVIMENTO", "DESENV.").Replace("FINANCEIRA", "FINANC.").Replace("DIRETORIA", "DIRET.").Replace("ADMINISTRATIVA", "ADMINIST.").Trim() + "</font>";

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(centroCusto.JANEIRO);
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(centroCusto.FEVEREIRO);
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(centroCusto.MARCO);
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(centroCusto.ABRIL);
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(centroCusto.MAIO);
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(centroCusto.JUNHO);
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(centroCusto.JULHO);
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(centroCusto.AGOSTO);
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(centroCusto.SETEMBRO);
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(centroCusto.OUTUBRO);
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(centroCusto.NOVEMBRO);
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(centroCusto.DEZEMBRO);

                        totalLinha = Convert.ToDecimal(centroCusto.JANEIRO + centroCusto.FEVEREIRO + centroCusto.MARCO + centroCusto.ABRIL + centroCusto.MAIO + centroCusto.JUNHO + centroCusto.JULHO + centroCusto.AGOSTO + centroCusto.SETEMBRO + centroCusto.OUTUBRO + centroCusto.NOVEMBRO + centroCusto.DEZEMBRO);
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        GridView gvContaContabil = e.Row.FindControl("gvContaContabil") as GridView;
                        if (gvContaContabil != null)
                        {
                            var contaContabil = gCentroCusto.Where(p => p.ID_CCUSTO == centroCusto.ID_CCUSTO).GroupBy(b => new { ID_CCUSTO = b.ID_CCUSTO, CONTA_CONTABIL = b.CONTA_CONTABIL, DESC_CONTA_CONTABIL = b.DESC_CONTA_CONTABIL }).Select(
                                                                               k => new SP_OBTER_CENTRO_CUSTO_LANCAMENTOResult
                                                                               {
                                                                                   ID_CCUSTO = k.Key.ID_CCUSTO,
                                                                                   CONTA_CONTABIL = k.Key.CONTA_CONTABIL,
                                                                                   DESC_CONTA_CONTABIL = k.Key.DESC_CONTA_CONTABIL,
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

                            gvContaContabil.DataSource = contaContabil;
                            gvContaContabil.DataBind();
                        }
                    }
                }
            }
        }
        protected void gvCentroCusto_DataBound(object sender, EventArgs e)
        {
            GridView gvCentroCusto = (GridView)sender;
            if (gvCentroCusto != null)
                if (gvCentroCusto.HeaderRow != null)
                    gvCentroCusto.HeaderRow.Visible = false;
        }

        protected void gvContaContabil_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CENTRO_CUSTO_LANCAMENTOResult contaContabil = e.Row.DataItem as SP_OBTER_CENTRO_CUSTO_LANCAMENTOResult;

                    if (contaContabil != null)
                    {
                        Literal _litContaContabil = e.Row.FindControl("litContaContabil") as Literal;
                        if (_litContaContabil != null)
                            _litContaContabil.Text = "<font size='1' face='Calibri'>" + contaContabil.DESC_CONTA_CONTABIL.Trim() + "</font>";

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(contaContabil.JANEIRO);
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(contaContabil.FEVEREIRO);
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(contaContabil.MARCO);
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(contaContabil.ABRIL);
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(contaContabil.MAIO);
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(contaContabil.JUNHO);
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(contaContabil.JULHO);
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(contaContabil.AGOSTO);
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(contaContabil.SETEMBRO);
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(contaContabil.OUTUBRO);
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(contaContabil.NOVEMBRO);
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(contaContabil.DEZEMBRO);

                        totalLinha = Convert.ToDecimal(contaContabil.JANEIRO + contaContabil.FEVEREIRO + contaContabil.MARCO + contaContabil.ABRIL + contaContabil.MAIO + contaContabil.JUNHO + contaContabil.JULHO + contaContabil.AGOSTO + contaContabil.SETEMBRO + contaContabil.OUTUBRO + contaContabil.NOVEMBRO + contaContabil.DEZEMBRO);
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                    }

                    System.Web.UI.WebControls.Image img = e.Row.FindControl("imgExpand") as System.Web.UI.WebControls.Image;
                    if (img != null)
                    {
                        GridView gvFilial = e.Row.FindControl("gvFilial") as GridView;
                        if (gvFilial != null)
                        {
                            var filial = gCentroCusto.Where(p => p.ID_CCUSTO == contaContabil.ID_CCUSTO && p.CONTA_CONTABIL == contaContabil.CONTA_CONTABIL).GroupBy(b => new { FILIAL = b.FILIAL }).Select(
                                                                               k => new SP_OBTER_CENTRO_CUSTO_LANCAMENTOResult
                                                                               {
                                                                                   FILIAL = k.Key.FILIAL,
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

                            gvFilial.DataSource = filial;
                            gvFilial.DataBind();
                        }
                    }

                }
            }
        }
        protected void gvContaContabil_DataBound(object sender, EventArgs e)
        {
            GridView gvContaContabil = (GridView)sender;
            if (gvContaContabil != null)
                if (gvContaContabil.HeaderRow != null)
                    gvContaContabil.HeaderRow.Visible = false;
        }

        protected void gvFilial_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal totalLinha = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    SP_OBTER_CENTRO_CUSTO_LANCAMENTOResult filial = e.Row.DataItem as SP_OBTER_CENTRO_CUSTO_LANCAMENTOResult;

                    if (filial != null)
                    {
                        Literal _litFilial = e.Row.FindControl("litFilial") as Literal;
                        if (_litFilial != null)
                            _litFilial.Text = "<font size='1' face='Calibri'>" + filial.FILIAL.Trim() + "</font>";

                        Literal _janeiro = e.Row.FindControl("litJaneiro") as Literal;
                        if (_janeiro != null)
                            _janeiro.Text = FormatarValor(filial.JANEIRO);
                        Literal _fevereiro = e.Row.FindControl("litFevereiro") as Literal;
                        if (_fevereiro != null)
                            _fevereiro.Text = FormatarValor(filial.FEVEREIRO);
                        Literal _marco = e.Row.FindControl("litMarco") as Literal;
                        if (_marco != null)
                            _marco.Text = FormatarValor(filial.MARCO);
                        Literal _abril = e.Row.FindControl("litAbril") as Literal;
                        if (_abril != null)
                            _abril.Text = FormatarValor(filial.ABRIL);
                        Literal _maio = e.Row.FindControl("litMaio") as Literal;
                        if (_maio != null)
                            _maio.Text = FormatarValor(filial.MAIO);
                        Literal _junho = e.Row.FindControl("litJunho") as Literal;
                        if (_junho != null)
                            _junho.Text = FormatarValor(filial.JUNHO);
                        Literal _julho = e.Row.FindControl("litJulho") as Literal;
                        if (_julho != null)
                            _julho.Text = FormatarValor(filial.JULHO);
                        Literal _agosto = e.Row.FindControl("litAgosto") as Literal;
                        if (_agosto != null)
                            _agosto.Text = FormatarValor(filial.AGOSTO);
                        Literal _setembro = e.Row.FindControl("litSetembro") as Literal;
                        if (_setembro != null)
                            _setembro.Text = FormatarValor(filial.SETEMBRO);
                        Literal _outubro = e.Row.FindControl("litOutubro") as Literal;
                        if (_outubro != null)
                            _outubro.Text = FormatarValor(filial.OUTUBRO);
                        Literal _novembro = e.Row.FindControl("litNovembro") as Literal;
                        if (_novembro != null)
                            _novembro.Text = FormatarValor(filial.NOVEMBRO);
                        Literal _dezembro = e.Row.FindControl("litDezembro") as Literal;
                        if (_dezembro != null)
                            _dezembro.Text = FormatarValor(filial.DEZEMBRO);

                        totalLinha = Convert.ToDecimal(filial.JANEIRO + filial.FEVEREIRO + filial.MARCO + filial.ABRIL + filial.MAIO + filial.JUNHO + filial.JULHO + filial.AGOSTO + filial.SETEMBRO + filial.OUTUBRO + filial.NOVEMBRO + filial.DEZEMBRO);
                        Literal _total = e.Row.FindControl("litTotal") as Literal;
                        if (_total != null)
                            _total.Text = FormatarValor(totalLinha);

                    }
                }
            }
        }
        protected void gvFilial_DataBound(object sender, EventArgs e)
        {
            GridView gvFilial = (GridView)sender;
            if (gvFilial != null)
                if (gvFilial.HeaderRow != null)
                    gvFilial.HeaderRow.Visible = false;
        }

        #endregion

    }
}
